using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using SGAC.Accesorios;
using SGAC.BE.MRE;
using SGAC.BE.MRE.Custom;
using SGAC.Configuracion.Maestro.BL;
using SGAC.Configuracion.Sistema.BL;
using SGAC.Controlador;
using SGAC.Registro.Actuacion.BL;
using SGAC.Registro.Persona.BL;
using SGAC.WebApp.Accesorios;
using AjaxControlToolkit;
using Microsoft.Security.Application;
using SGAC.Almacen.BL;


namespace SGAC.WebApp.Registro
{
    public partial class FrmActuacionNotarialExtraProtocolar : MyBasePage
    {
        #region Constantes

        const int iTabTipoActoIndice = 0;
        const int iTabParticipanteIndice = 1;
        const int iTabCuerpoIndice = 2;
        const int iTabAprobacionPagoIndice = 3;
        const int iTabVinculacionIndice = 4;
        const int iTabDigitalizacionIndice = 5;

        #endregion
        DataTable _dtPersona = new DataTable();
        private DataTable dtTarifarioFiltrado;
        private BE.MRE.SI_TARIFARIO objTarifarioBE;

        private string strVariableAccion = "Actuacion_Accion";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //ucCtrlActuacionPago.ddlTipoPagoSelectorChanged += new Accesorios.SharedControls.ctrlActuacionPago.ddlTipoPagoCommandEventHandler(ucCtrlActuacionPago_ddlTipoPagoSelectorChanged);

            isSessionTimeOut();
            ctrlToolTipoActo.VisibleIButtonGrabar = true;

            ctrlToolTipoActo.btnGrabar.Click += new EventHandler(btnGrabarTipoActo_Click);
            ctrlToolTipoActo.btnGrabar.OnClientClick = "return ValidarTabRegistro();";

            pnlTipoActo.DefaultButton = ctrlToolTipoActo.btnGrabar.UniqueID;

            ctrlToolBarParticipante.VisibleIButtonCancelar = true;
            ctrlToolBarParticipante.btnCancelar.Click += new EventHandler(btnCancelar3_Click);

            ctrlToolBarParticipante.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBarParticipante.btnCancelar.Text = "    Limpiar";

            ctrlToolBarParticipante.VisibleIButtonGrabar = true;
            ctrlToolBarParticipante.btnGrabar.Click += new EventHandler(btnGrabar3_Click);
            ctrlToolBarParticipante.btnGrabar.OnClientClick = "abrirPopupEsperaCarga();";

            ctrlToolBar5.VisibleIButtonGrabar = true;
            

            //ctrlToolBar5.btnGrabar.OnClientClick = "return ValidarControlPago();";///esto siempre estaba comentado
            ctrlToolBar5.btnGrabar.OnClientClick = "return ValidarRegistroActuacion();";
            ctrlToolBar5.btnGrabar.Click += new EventHandler(btnGrabar5_Click);
            ctrFecPago.EndDate = DateTime.Today;

            //this.hdn_NOTARIA_IDIOMA.Value = ConfigurationManager.AppSettings["NotarialIdioma"];

            this.hdn_NOTARIA_IDIOMA.Value = Session["NotarialIdioma"].ToString();

            this.hdn_PARTICIPANTE_OTORGANTE.Value = Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.OTORGANTE);
            this.hdn_PARTICIPANTE_APODERADO.Value = Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.APODERADO);
            this.hdn_PARTICIPANTE_MENOR.Value = Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.MENOR);
            this.hdn_PARTICIPANTE_PADRE.Value = Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.PADRE);
            this.hdn_PARTICIPANTE_MADRE.Value = Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.MADRE);
            this.hdn_PARTICIPANTE_INTERPRETE.Value = Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.INTERPRETE);
            this.hdn_PARTICIPANTE_TITULAR.Value = Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.TITULAR);
            this.hdn_PARTICIPANTE_ACOMPANANTE.Value = Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.ACOMPANANTE);
            this.hdn_PARTICIPANTE_TESTIGO_A_RUEGO.Value = Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO);
            this.hdn_PARTICIPANTE_RECURRENTE.Value = Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.RECURRENTE);

            this.hdn_GENERO_MASCULINO.Value = Convert.ToString((int)Enumerador.enmGenero.MASCULINO);
            this.hdn_GENERO_FEMENINO.Value = Convert.ToString((int)Enumerador.enmGenero.FEMENINO);

            //------------------------------------------------------
            // Jonatan -- 20/07/2017 -- Botones controles de usuario, reimpresión y anulación de autoadhesivos
            ctrlReimprimirbtn1.btnReimprimirHandler += new Accesorios.SharedControls.ctrlReimprimirbtn.OnButtonReimprimirClick(ctrlReimprimirbtn_btnReimprimirHandler);
            ctrlBajaAutoadhesivo1.btnAnularHandler += new Accesorios.SharedControls.ctrlBajaAutoadhesivo.OnButtonAnularClick(ctrlBajaAutoadhesivo_btnAnularAutoahesivo);
            ctrlBajaAutoadhesivo1.btnAceptarAnularHandler += new Accesorios.SharedControls.ctrlBajaAutoadhesivo.OnButtonAceptarAnulacionClick(ctrlBajaAutoadhesivo_btnAceptarAnularAutoahesivo);
            //------------------------------------------------------

            if (Session["iArchivoAdjuntado"] != null)
            {
                this.hidNomAdjFile2.Value = Session["iArchivoAdjuntado"].ToString();
            }

            txtFecNac.StartDate = new DateTime(1900, 1, 1);
            txtFecNac.EndDate = DateTime.Now;

            //Gdv_Tarifa.Columns[10].Visible = false;

            if (ddlRegistroGenero.SelectedValue == Convert.ToString((int)Enumerador.enmGenero.FEMENINO))
            {
                lblIncapacidadFirmar.Text = "Incapacitada de firmar por ser:";
            }
            else
            {
                lblIncapacidadFirmar.Text = "Incapacitado de firmar por ser:";
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

            //this.hdn_acno_iActoNotarialId.Value = (Session[Constantes.CONST_SESION_ACTONOTARIAL_ID]).ToString();
            //this.hdn_acno_iActuacionId.Value = (Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value]).ToString();
            if (ViewState[Constantes.CONST_SESION_ACTONOTARIAL_ID] != null)
            {
                this.hdn_acno_iActoNotarialId.Value = (ViewState[Constantes.CONST_SESION_ACTONOTARIAL_ID]).ToString();
            }
            else {
                this.hdn_acno_iActoNotarialId.Value = Session[Constantes.CONST_SESION_ACTONOTARIAL_ID].ToString();
                ViewState[Constantes.CONST_SESION_ACTONOTARIAL_ID] = Session[Constantes.CONST_SESION_ACTONOTARIAL_ID];
                Session[Constantes.CONST_SESION_ACTONOTARIAL_ID] = null;
            }

            if (ViewState[Constantes.CONST_SESION_ACTUACION_ID] != null)
            {
                this.hdn_acno_iActuacionId.Value = (ViewState[Constantes.CONST_SESION_ACTUACION_ID]).ToString();
            }
            else
            {
                this.hdn_acno_iActuacionId.Value = (Session[Constantes.CONST_SESION_ACTUACION_ID]).ToString();
                ViewState[Constantes.CONST_SESION_ACTUACION_ID] = Session[Constantes.CONST_SESION_ACTUACION_ID];
                //Session[Constantes.CONST_SESION_ACTUACION_ID] = null;
            }
            ctrlReimprimirbtn1.GUID = HFGUID.Value;

            //EjecutarScript("seleccionado();", "pageLoad()");
            if (!Page.IsPostBack)
            {
                string codPersona = "0";
                if (Request.QueryString["CodPer"] != null)
                {
                    codPersona = Util.DesEncriptar(Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString()));
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
                else
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Formulario", "alert('Por favor evitar abrir 2 pestañas, o copiar el formulario');", true);
                    Response.Redirect("../Default.aspx", false);
                    return;
                }

                if (Convert.ToInt64(codPersona) > 0)
                {
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

                txtFecNac.Text = "";
                hdn_bCheckAceptacion.Value = "0";
                hdn_bCuerpoGrabado.Value = "0";

                HF_PAGADO_EN_LIMA.Value = Convert.ToString((int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA);
                HF_DEPOSITO_CUENTA.Value = Convert.ToString((int)Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA);
                HF_TRANSFERENCIA.Value = Convert.ToString((int)Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA);

                hdn_PODER_FUERA_REGISTRO.Value = Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.PODER_FUERA_REGISTRO).ToString();
                hdn_AUTORIZACION_VIAJE_MENOR.Value = Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR).ToString();

                CargarDatosRecurrente();
                CargarFuncionarios(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), 0);

                grd_Participantes.DataSource = new DataTable();
                grd_Participantes.DataBind();


                //Gdv_Tarifa.DataSource = new DataTable();
                //Gdv_Tarifa.DataBind();

                Session["TamanoLetra"] = 7;
                Session["dtActuacionesRowCount"] = 0;
                //Session["iParticipanteEditando"] = null;
                //Session["iExisteTestigoRuego"] = null;
                //Session["PersonaAlmacenada"] = null;
                Session["iOficinaConsularDiferente"] = null;
                //Session["vCamposPersonaAlmacenada"] = null;
                Session["vDocumentoActualPFR"] = null;
                Session["vDocumentoActualPFRPrevio"] = null;
                //ViewState["ActoNotarialParticipanteId"] = "0";
                ViewState["ExtraIndiceEdicion"] = -1;

                Session["DocumentoDigitalizadoContainer"] = null;
                //Session["ParticipanteContainer"] = new List<CBE_PARTICIPANTE>();
                //Session["ParticipanteContainerCount"] = null;
                Session[strVarActuacionDetalleDT] = null;
                Session["anep_iSelectedTabId"] = null;
                Session[strVarActuacionDetalleIndice] = -1;
                LblDescMtoML.Text = Session[Constantes.CONST_SESION_TIPO_MONEDA].ToString();
                LblDescTotML.Text = Session[Constantes.CONST_SESION_TIPO_MONEDA].ToString();
                //LblDescTotML2.Text = Session[Constantes.CONST_SESION_TIPO_MONEDA].ToString() + ":";
                Session["PasoActualTab"] = "0";

                this.hdn_actu_iPersonaRecurrenteId.Value = (ViewState["iPersonaId"]).ToString();
                this.hdn_acno_sOficinaConsularId.Value = (Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString();

                lblTamanoMaxAdjnuto.Text = "Tamaño Máximo " + Constantes.CONST_TAMANIO_MAX_ADJUNTO_KB + "KB";

                //// ID(s) que deben cambiar al EDITAR ....
                this.hdn_ancu_iActoNotarialCuerpoId.Value = "0";
                this.hdm_empr_sTipoEmpresaId.Value = "0";
                //// Variables de LOG(s)
                this.hdn_acno_sUsuarioCreacion.Value = (Session[Constantes.CONST_SESION_USUARIO_ID]).ToString();
                this.hdn_acno_vIPCreacion.Value = (Session[Constantes.CONST_SESION_DIRECCION_IP]).ToString();
                this.hdn_acno_sUsuarioModificacion.Value = (Session[Constantes.CONST_SESION_USUARIO_ID]).ToString();
                this.hdn_acno_vIPModificacion.Value = (Session[Constantes.CONST_SESION_DIRECCION_IP]).ToString();
                this.hdn_acno_iActoNotarialReferenciaId.Value = "0";  // Esto debera ser con una busqueda ....

                //HabilitarTabs();
                CargarListadosDesplegables();
                //--------------------------------------------------
                //Fecha: 06/07/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Consultar la tabla PS_SISTEMA.SI_Pais
                //--------------------------------------------------
                DataTable dtPaises = new DataTable();
                dtPaises = Comun.ConsultarPaises();
                //--------------------------------------------------
                //Fecha: 03/04/2020
                //Autor: Miguel Márquez Beltrán
                //Motivo: Asignar Paises a un ViewState.
                //--------------------------------------------------
                ViewState["Paises"] = dtPaises;

                //----------------------------------------------------
                //Fecha: 03/03/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Mostrar la Nacionalidad según requerimiento:
                //          OBSERVACIONES_SGAC_24022021 item: 5.
                //----------------------------------------------------
                Util.CargarDropDownList(ddlPaisOrigen, dtPaises, "PAIS_VNACIONALIDAD", "PAIS_SPAISID", true);
                //----------------------------------------------------
                ddlPaisOrigen.SelectedIndex = 0;
                RefrescarNacionalidad();
                //--------------------------------------------------

                hdn_Tipo_Participante_Editando.Value = "-1";

                //--------------------------------------------------
                //Fecha: 26/07/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Inicializar Sub Tipo de Acto Notarial
                //--------------------------------------------------
                lbl_subTipoActoNotarialExtra.Visible = false;
                ddlSubTipoNotarialExtra.Visible = false;
                ddlCondiciones.Visible = false;
                lblCondiciones.Visible = false;
                //--------------------------------------------------

                lblSustentoTipoPago.Visible = false;
                txtSustentoTipoPago.Visible = false;
                lblValSustentoTipoPago.Visible = false;
                RBNormativa.Visible = false;
                RBSustentoTipoPago.Visible = false;
                lblExoneracion.Visible = false;
                ddlExoneracion.Visible = false;
                lblValExoneracion.Visible = false;
                
                CargarActoNotarial();


                if (Session["PasoActualTab"].ToString() == iTabDigitalizacionIndice.ToString())
                {
                    Session["EditorTextoEstado"] = "1";
                }
                else
                {
                    Session["EditorTextoEstado"] = "0";
                }

                pintarLista();

                MostrarCamposPorTipoParticipante();

                string vScript = "SetTabs('" + Session["PasoActualTab"].ToString() + "','" + ddlTipoActoNotarialExtra.SelectedValue.ToString() + "');";
                if (Session["anep_iSelectedTabId"] != null)
                {
                    if (Session["anep_iSelectedTabId"].ToString() == iTabParticipanteIndice.ToString())
                    {
                        //vScript += "HabilitarPorParticipante(false);";
                    }
                    else if (Session["anep_iSelectedTabId"].ToString() == iTabCuerpoIndice.ToString())
                    {
                        vScript += "DesactivarAceptacion();";
                    }
                }

                EjecutarScript(vScript, "pageLoad()");

                //--------------------------------------------------
                //Fecha: 17/07/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Inicializar Otro documento
                //--------------------------------------------------
                LblOtroDocumento.Visible = false;
                txtDescOtroDocumento.Visible = false;
                lblDescOtroDocObl.Visible = false;

                btnGrabarCuerpoTemporal.Attributes.Add("disabled", "disabled");

                string strScript = string.Empty;
                strScript = @"$(function(){{
                                            DeshabilitarConforme();
                                        }});";
                strScript = string.Format(strScript);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "DeshabilitarCheckConforme", strScript, true);


                // Jonatan -- 20/07/2017
                if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
                {
                    ctrlReimprimirbtn1.Activar = false;
                    
                    ctrlBajaAutoadhesivo1.Activar = false;
                    btnGrabarVinculacion.Enabled = false;
                    ctrlToolBar5.btnGrabar.Enabled = false;
                    ctrlToolTipoActo.btnGrabar.Enabled = false;
                }
                else
                {
                    string valor = Convert.ToString(Request.QueryString["cod"]);
                    if (valor == "1")
                    {
                        ctrlReimprimirbtn1.Activar = false;
                    }
                    else
                    {
                        ctrlReimprimirbtn1.Activar = chkImpresion.Checked;
                    }
                    CargarUltimoInsumo();
                }

                //if (Session["Actuacion_Accion"].ToString() == Enumerador.enmTipoOperacion.CONSULTA.ToString())
                //{
                //    if (Convert.ToBoolean(Session["ModoEdicionProtocolar"]) == false)
                //    {
                //        btnGrabarVinculacion.Enabled = false;
                //    }
                //    else {
                //        btnGrabarVinculacion.Enabled = true;
                //    }
                //}

                Incapacitado();

                if (VerificarSiTieneAutoadhesivos() == 0)
                {
                    ctrlToolBar5.btnGrabar.Enabled = false;
                }
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "RecargarUbigeo", "RecargarUbigeoViaje();", true);
        }

        private void CargarUbigeo()
        {
            // Jonatan Silva, Se llena todo los combos de continente con una sola consulta
            beUbigeoListas obeUbigeoListas = new beUbigeoListas();
            UbigeoConsultasBL objUbigeoBL = new UbigeoConsultasBL();

            obeUbigeoListas = objUbigeoBL.obtenerUbiGeo();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonStringProvincia = serializer.Serialize(obeUbigeoListas.Ubigeo02);
            string jsonStringDistrito = serializer.Serialize(obeUbigeoListas.Ubigeo03);

            string javaScript = "Guardarlocalstorage(" + jsonStringProvincia + "," + jsonStringDistrito + ");";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", javaScript, true);

            ViewState["Ubigeo"] = obeUbigeoListas;
            if (obeUbigeoListas != null)
            {
                if (obeUbigeoListas.Ubigeo01.Count > 0)
                {
                    obeUbigeoListas.Ubigeo01.Insert(0, new beUbicaciongeografica { Ubi01 = "00", Departamento = "-- SELECCIONE --" });
                    ddl_UbigeoPaisViajeDestino.DataSource = obeUbigeoListas.Ubigeo01;
                    ddl_UbigeoPaisViajeDestino.DataValueField = "Ubi01";
                    ddl_UbigeoPaisViajeDestino.DataTextField = "Departamento";
                    ddl_UbigeoPaisViajeDestino.DataBind();
                }
            }
        }
        private int VerificarSiTieneAutoadhesivos()
        {
            ActuacionConsultaBL ActuacionConsultaBL = new ActuacionConsultaBL();
            int intStock = ActuacionConsultaBL.ObtenerSaldoAutoadhesivos(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                                                                         Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]),
                                                                         Convert.ToInt16(Enumerador.enmInsumoTipo.AUTOADHESIVO));
            H_CAN_AUTOADHESIVO.Value = intStock.ToString();
            return intStock;
        }

        //--------------------------------------------------
        //Fecha: 07/08/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: Pinta la lista
        //--------------------------------------------------
        public void pintarLista()
        {
            for (int i = 0; i < ddlTipoActoNotarialExtra.Items.Count; i++)
            {

                if (ddlTipoActoNotarialExtra.Items[i].Value != Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.OTRAS_CERTIFICACIONES_NOTARIALES).ToString() &&
                    ddlTipoActoNotarialExtra.Items[i].Value != "0")
                {
                    ddlTipoActoNotarialExtra.Items[i].Attributes.Add("style", "background-color:#C8C8C8; font-weight:bold;");
                }
            }
        }
        public Boolean isSessionTimeOut()
        {

            Boolean bResultado = true;
            HttpContext ctx = HttpContext.Current;
            if (ctx == null) bResultado = false;
            if (ctx.Session == null) bResultado = false;
            //if (!ctx.Session.IsNewSession) bResultado = false;
            if (ctx.Session[Constantes.CONST_SESION_USUARIO] == null) bResultado = false;

            if (!bResultado)
            {
                Response.Redirect("../Cuenta/FrmLogin.aspx");
            }

            return bResultado;

        }

        // IMPORTANTE-FORMATO
        private void SetearIntroduccionyConlusionCuerpo()
        {

            if (Convert.ToInt32(Session["PasoActualTab"].ToString()) > iTabParticipanteIndice)
            {
                string documento = string.Empty;
                string[] introConclu = new string[] { };

                if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.PODER_FUERA_REGISTRO).ToString())
                {
                    documento = ObtenerDocumentoPFR().Replace("<br />", "");

                    introConclu = documento.Split(new string[] { "<tab></tab>" }, StringSplitOptions.None);

                    lblcuerpoIntroduccion.Text = introConclu[1];
                    lblcuerpoConclusion.Text = introConclu[4];
                }
                else
                {
                    txtCuerpo.Visible = false;
                    txtInfoAdicional.Visible = false;


                    ActoNotarialConsultaBL BL = new ActoNotarialConsultaBL();
                    DataTable dt = new DataTable();
                    StringBuilder sScript = new StringBuilder();
                    sScript.Append("<br />");
                    sScript.Append("<p align=\"center\">");
                    sScript.Append("<h1><font face=\"impact\">#cambio#</font></h1>");
                    sScript.Append("</p>");
                    sScript.Append("<br />");
                    sScript.Append("<tab></tab>");

                    if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.CERTIFICADO_SUPERVIVENCIA).ToString())
                    {
                        dt = (DataTable)BL.ReporteSupervivencia(Convert.ToInt32(ViewState[Constantes.CONST_SESION_ACTONOTARIAL_ID]),
                        Convert.ToInt32(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                        sScript.Replace("#cambio#", "CERTIFICADO DE SUPERVIVENCIA");

                        documento = sScript.Append(ObtenerDocumentoCertificado(dt)).ToString();
                    }
                    else if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR).ToString())
                    {
                        dt = (DataTable)BL.ReporteAutorizacionViaje(Convert.ToInt32(ViewState[Constantes.CONST_SESION_ACTONOTARIAL_ID]),
                        Convert.ToInt32(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                        //sScript.Replace("#cambio#", "AUTORIZACIÓN DE VIAJE DE MENOR AL EXTERIOR");
                        sScript.Replace("#cambio#", "AUTORIZACIÓN DE VIAJE DE MENOR");

                        documento = sScript.Append(ObtenerDocumentoAutorizacionMenor(dt)).ToString();
                    }

                    introConclu = documento.Split(new string[] { "<tab></tab>" }, StringSplitOptions.None);
                    lblcuerpoIntroduccion.Text = introConclu[1];

                    Session["vDocumentoActualPFRPrevio"] = documento;
                }


                UpdatePanel4.Update();

            }
        }

        #region Formatos

        private string ObtenerDocumentoPFR()
        {
            //  return sScript.ToString();
            //String sIdiomaCastellano = ConfigurationManager.AppSettings["NotarialIdioma"].ToString();
            String sIdiomaCastellano = Session["NotarialIdioma"].ToString();

            DataTable dt = new DataTable();
            //DataTable dtAux = new DataTable();

            ActoNotarialConsultaBL BL = new ActoNotarialConsultaBL();

            dt = (DataTable)BL.ReportePoderFueraRegistro(Convert.ToInt32(ViewState[Constantes.CONST_SESION_ACTONOTARIAL_ID]),
                Convert.ToInt32(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));

            DataTable dtDatosEscritura = new DataTable();

            dtDatosEscritura = BL.ActonotarialObtenerDatosPrincipales(Convert.ToInt64(ViewState[Constantes.CONST_SESION_ACTONOTARIAL_ID]), Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));

            String strOficinaConsularNombre = dtDatosEscritura.Rows[0]["NombreOficinaConsular"].ToString().Trim();
            String strFuncionarioAutorizador = dtDatosEscritura.Rows[0]["NombreFuncionario"].ToString().Trim();
            String strCargoFuncionarioAutorizador = dtDatosEscritura.Rows[0]["CargoFuncionario"].ToString().Trim();
            String strUbigeoOficinaConsular = dtDatosEscritura.Rows[0]["CiudadOficinaConsular"].ToString().Trim();
            String strUbigeoOficinaConsularCiudad = dtDatosEscritura.Rows[0]["CiudadOficinaConsular"].ToString().Trim() + ", " + dtDatosEscritura.Rows[0]["Provincia"].ToString().Trim();
            String strFecha = Util.ObtenerFechaParaDocumentoLegalProtocolar(Comun.FormatearFecha(dtDatosEscritura.Rows[0]["Fecha"].ToString())).ToUpper();


            StringBuilder sScript = new StringBuilder();
            StringBuilder sScriptOtorgante = new StringBuilder();
            StringBuilder sScriptInterprete = new StringBuilder();
            StringBuilder sScriptPrimerInterprete = new StringBuilder();
            StringBuilder sScriptApoderado = new StringBuilder();
            StringBuilder sScriptTestigo_a_Ruegos = new StringBuilder();

            //if (lParticipantes.Where(x => x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.MENOR) && x.anpa_cEstado != "E").Count() == 0)
            Int16 sContarOtogartes = 0, sContarMujeresOtorgantes = 0, sContarHombresOtorgantes = 0, sContarAux = 1;
            Int16 sContarInterprete = 0, sContarMujeresInterprete = 0, sContarHombresInterprete = 0, sContarAuxInterprete = 1;
            Int16 sContarApoderado = 0, sContarMujeresApoderado = 0, sContarHombresApoderado = 0;
            Int16 sContarTestigo = 0;
            Int16 sContarOtorganteMujerIdiomaEx = 0;
            Int16 sContarOtorganteHombreIdiomaEx = 0;
            Int16 sContarOtorganteIdiomaEx = 0;

            String strIdiomas = String.Empty;
            string strIdiomaOtorganteEx = string.Empty;
            String strLA_EL_LOS_LAS = String.Empty;
            string strLA_EL_LOS_LAS_Otorgante = string.Empty;
            String strPluralSingularLaElInterprete = String.Empty;
            String strPluralSingularOtorgante = String.Empty;
            string strPluralSingularOtorganteEx = string.Empty;
            String strPluralSingularInterprete = String.Empty;
            String strPluralSingularInteligente = String.Empty;
            String strPluralSingularLaElApoderado = String.Empty;
            string strPluralSingularOtorga = string.Empty;

            String strPluralSingularApoderado = String.Empty;
            string strPluralSinguralEsOtorgante = string.Empty;

            String strGenerpIdentificadoInterprete = String.Empty;
            String strGenerpIdentificadoApoderado = String.Empty;

            String strPluralSinguralProcede = String.Empty;
            String strPluralSinguralQuien = String.Empty;
            string strPluralSingularExpreso = string.Empty;

            String strPluralSinguralQueda = String.Empty;
            String strPluralSinguralFacultado = String.Empty;

            String strPluralSinguralQuienInterprete = String.Empty;
            String strPluralSinguralDeclaraInterprete = String.Empty;

            Boolean ExisteInterorete = false;


            String HtmlInterpreteParticipante = String.Empty;
            String HtmlOtorganteParticipante = String.Empty;
            String HtmlTestigoRuegoParticipante = String.Empty;
            String HtmlTestigoRuegoParticipanteAux = String.Empty;
            String vLetraPlural = String.Empty;
            string strParticipanteOtorganteEx = string.Empty;

            String IdentificadoGeneroFuncionario = String.Empty;
            //-------------------------------------------------------------
            //Fecha: 13/12/2017
            //Objetivo: Si el valos es 1 es mujer / si es 2 es hombre
            //-------------------------------------------------------------

            if (dtDatosEscritura.Rows[0]["sGeneroFuncionario"].ToString().Trim().Equals("2"))
            {
                IdentificadoGeneroFuncionario = "IDENTIFICADA";
            }
            else
            {
                IdentificadoGeneroFuncionario = "IDENTIFICADO";
            }

            if (IdentificadoGeneroFuncionario == String.Empty) IdentificadoGeneroFuncionario = "IDENTIFICADO";


            sScript.Append("<p align=\"left\">");
            sScript.Append("CONSULADO GENERAL");
            sScript.Append("</p>");

            sScript.Append("<br />");
            sScript.Append("<br />");
            sScript.Append("<p align=\"center\">");
            sScript.Append("<h1><font face=\"impact\">PODER FUERA DE REGISTRO</font></h1>");
            sScript.Append("<br>");
            sScript.Append("</p><br>");

            sScript.Append("<tab></tab>");
            sScript.Append("<br>");
            sScript.Append("<br>");

            sScript.Append("<p align=\"justify\"; style=\"background-color:transparent;\">");
            if (strOficinaConsularNombre.Contains("SECCIÓN"))
            {
                sScript.Append("EN LA ");
            }
            else {
                sScript.Append("EN EL ");
            }
            sScript.Append(strOficinaConsularNombre);
            //sScript.Append(", EL ");
            sScript.Append(", ");
            sScript.Append(strFecha);
            sScript.Append(", ANTE MÍ, ");
            sScript.Append(strFuncionarioAutorizador);
            
            sScript.Append(", " + IdentificadoGeneroFuncionario);
            sScript.Append(" CON DOCUMENTO NACIONAL DE IDENTIDAD ");
            sScript.Append(dtDatosEscritura.Rows[0]["NumeroDocumentoFuncionario"].ToString().ToUpper());
            
            sScript.Append(", ");
            sScript.Append(strCargoFuncionarioAutorizador);
            sScript.Append(" EN ");
            sScript.Append(strUbigeoOficinaConsularCiudad);

            if (Convert.ToInt16(dt.Rows[0]["CantidadOtorgantes"].ToString()) > 1)
            {
                sScript.Append("; COMPARECEN: ");
            }
            else {
                sScript.Append("; COMPARECIÓ: ");
            }

            int sContarApoderadoCant = 0;

            foreach (DataRow Row in dt.Rows)
            {
                if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.APODERADO))
                {
                    sContarApoderadoCant = sContarApoderadoCant + 1;
                }
            }
            foreach (DataRow Row in dt.Rows)
            {
                if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.OTORGANTE))
                {

                    #region Otorgante

                    sScriptOtorgante.Append(Row["vParticipante"].ToString().ToUpper().Trim());
                    sScriptOtorgante.Append(", DE NACIONALIDAD " + Row["vNacionalidad"].ToString().ToUpper());
                    sScriptOtorgante.Append(", DE PROFESIÓN U OCUPACIÓN  " + Row["vOcupacion"].ToString().ToUpper());
                    sScriptOtorgante.Append(", CON  " + Row["vTipoDocumento"] + " " + Row["vDocumentoNumero"].ToString().ToUpper());
                    sScriptOtorgante.Append(", ESTADO CIVIL  " + Row["vEstadoCivil"].ToString().ToUpper());
                    sScriptOtorgante.Append(", CON DOMICILIO EN " + Row["vDireccion"].ToString().ToUpper());
                    sScriptOtorgante.Append("; ");
                    sContarOtogartes++;

                    if (Convert.ToInt16(Row["sGeneroId"].ToString()) == (int)Enumerador.enmGenero.FEMENINO)
                    {
                        sContarMujeresOtorgantes++;
                    }
                    else
                    {
                        sContarHombresOtorgantes++;
                    }

                    if (sIdiomaCastellano != Row["sIdiomaId"].ToString())
                    {

                        if (sContarAux == 1)
                        {
                            strIdiomas = strIdiomas + Row["vIdioma"].ToString().ToUpper();
                            strIdiomaOtorganteEx = Row["vIdioma"].ToString().ToUpper();
                            strParticipanteOtorganteEx = Row["vParticipante"].ToString().ToUpper();
                        }
                        else
                        {
                            strIdiomas = strIdiomas + ", " + Row["vIdioma"].ToString().ToUpper();
                        }
                        sContarAux++;
                        sContarOtorganteIdiomaEx++;
                        if (Convert.ToInt16(Row["sGeneroId"].ToString()) == (int)Enumerador.enmGenero.FEMENINO)
                        {
                            sContarOtorganteMujerIdiomaEx++;

                        }
                        else
                        {
                            sContarOtorganteHombreIdiomaEx++;
                        }
                    }

                    if (Convert.ToBoolean(Row["pers_bIncapacidadFlag"].ToString()) == false)
                    {
                        if (sContarOtogartes == 1)
                            //HtmlOtorganteParticipante = HtmlOtorganteParticipante + ". FIRMA E IMPRIME SU HUELLA DACTILAR " + Row["vParticipante"].ToString().ToUpper() + " EL " + strFecha;
                            HtmlOtorganteParticipante = HtmlOtorganteParticipante + ". FIRMA E IMPRIME SU HUELLA DACTILAR " + Row["vParticipante"].ToString().ToUpper() + " " + strFecha;
                        else
                            //HtmlOtorganteParticipante = HtmlOtorganteParticipante + "" + ". FIRMA E IMPRIME SU HUELLA DACTILAR " + Row["vParticipante"].ToString().ToUpper() + " EL " + strFecha;
                            HtmlOtorganteParticipante = HtmlOtorganteParticipante + "" + ". FIRMA E IMPRIME SU HUELLA DACTILAR " + Row["vParticipante"].ToString().ToUpper() + " " + strFecha;
                    }

                    DataTable dtp1 = (DataTable)BL.ReportePoderFueraRegistro(Convert.ToInt32(ViewState[Constantes.CONST_SESION_ACTONOTARIAL_ID]),
                Convert.ToInt32(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), Convert.ToInt64(Row["anpa_iActoNotarialParticipanteId"].ToString().ToUpper()));

                    //var dtp = null;


                    if (dtp1 != null)
                    {
                        if (dtp1.Rows.Count > 0)
                        {
                            //HtmlTestigoRuegoParticipante
                            foreach (DataRow RowTA in dtp1.Rows)
                            {
                                if (RowTA["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO))
                                {
                                    HtmlTestigoRuegoParticipante += sScriptOtorgante.ToString();
                                    HtmlTestigoRuegoParticipante += " QUIEN PROCEDE POR PROPIO DERECHO";

                                    if (Convert.ToInt16(Row["sGeneroId"].ToString()) == (int)Enumerador.enmGenero.FEMENINO)
                                    {
                                        HtmlTestigoRuegoParticipante += " Y SE ENCUENTRA INCAPACITADA DE FIRMAR POR " + Row["pers_vDescripcionIncapacidad"].ToString().ToUpper();
                                    }
                                    else
                                    {
                                        HtmlTestigoRuegoParticipante += " Y SE ENCUENTRA INCAPACITADO DE FIRMAR POR " + Row["pers_vDescripcionIncapacidad"].ToString().ToUpper();
                                    }

                                    HtmlTestigoRuegoParticipante += ", POR LO QUE IMPRIME SU HUELLA DACTILAR E INTERVIENE ";
                                    HtmlTestigoRuegoParticipante += dtp1.Rows[0]["vParticipante"].ToString().ToUpper().Trim();
                                    HtmlTestigoRuegoParticipante += ", DE NACIONALIDAD " + RowTA["vNacionalidad"].ToString().ToUpper();
                                    HtmlTestigoRuegoParticipante += ", DE PROFESIÓN U OCUPACIÓN " + RowTA["vOcupacion"].ToString().ToUpper();
                                    HtmlTestigoRuegoParticipante += ", CON  " + RowTA["vTipoDocumento"].ToString().ToUpper() + " " + RowTA["vDocumentoNumero"].ToString().ToUpper();
                                    HtmlTestigoRuegoParticipante += ", ESTADO CIVIL  " + RowTA["vEstadoCivil"].ToString().ToUpper();
                                    HtmlTestigoRuegoParticipante += ", CON DOMICILIO EN " + RowTA["vDireccion"].ToString().ToUpper();
                                    if (Convert.ToInt16(Row["sGeneroId"].ToString()) == (int)Enumerador.enmGenero.FEMENINO)
                                    {
                                        HtmlTestigoRuegoParticipante += ", QUIEN ACTÚA EN CALIDAD DE TESTIGO A RUEGO POR DESIGNACION DE LA OTORGANTE.";
                                    }
                                    else {
                                        HtmlTestigoRuegoParticipante += ", QUIEN ACTÚA EN CALIDAD DE TESTIGO A RUEGO POR DESIGNACION DEL OTORGANTE.";
                                    }
                                    
                                    HtmlTestigoRuegoParticipante += "</p>";
                                    break;
                                }
                            }
                        }
                    }

                    #endregion
                }
                else if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.INTERPRETE))
                {

                    #region Interprete


                    ExisteInterorete = true;
                    vLetraPlural = "N ";
                    sContarInterprete++;
                    if (Convert.ToInt16(Row["sGeneroId"].ToString()) == (int)Enumerador.enmGenero.FEMENINO)
                    {
                        sContarMujeresInterprete++;
                        strGenerpIdentificadoInterprete = "IDENTIFICADA";
                    }
                    else
                    {
                        sContarHombresInterprete++;
                        strGenerpIdentificadoInterprete = "IDENTIFICADO";

                    }


                    sScriptInterprete.Append(Row["vParticipante"].ToString().ToUpper().Trim());
                    //sScriptInterprete.Append(", DE NACIONALIDAD " + Row["vNacionalidad"].ToString().ToUpper());
                    //sScriptInterprete.Append(", DE PROFESIÓN U OCUPACIÓN  " + Row["vOcupacion"].ToString().ToUpper());
                    sScriptInterprete.Append(", " + strGenerpIdentificadoInterprete + " CON  " + Row["vTipoDocumento"].ToString().ToUpper() + " " + Row["vDocumentoNumero"].ToString().ToUpper());
                    //sScriptInterprete.Append(", ESTADO CIVIL  " + Row["vEstadoCivil"].ToString().ToUpper());
                    //sScriptInterprete.Append(", CON DOMICILIO EN " + Row["vDireccion"].ToString().ToUpper());
                    sScriptInterprete.Append("; ");


                    if (sContarAuxInterprete == 1)
                    {
                        //HtmlInterpreteParticipante = HtmlInterpreteParticipante + ". FIRMA E IMPRIME SU HUELLA DACTILAR " + Row["vParticipante"].ToString() + " EL " + strFecha + "";
                        HtmlInterpreteParticipante = HtmlInterpreteParticipante + ". FIRMA E IMPRIME SU HUELLA DACTILAR " + Row["vParticipante"].ToString() + " " + strFecha + "";

                        sScriptPrimerInterprete.Append(Row["vParticipante"].ToString().ToUpper().Trim());
                        sScriptPrimerInterprete.Append(", DE NACIONALIDAD " + Row["vNacionalidad"].ToString().ToUpper());
                        sScriptPrimerInterprete.Append(", DE PROFESIÓN U OCUPACIÓN  " + Row["vOcupacion"].ToString().ToUpper());
                        sScriptPrimerInterprete.Append(", " + strGenerpIdentificadoInterprete + " CON  " + Row["vTipoDocumento"].ToString().ToUpper() + " " + Row["vDocumentoNumero"].ToString().ToUpper());
                        sScriptPrimerInterprete.Append(", ESTADO CIVIL  " + Row["vEstadoCivil"].ToString().ToUpper());
                        sScriptPrimerInterprete.Append(", CON DOMICILIO EN " + Row["vDireccion"].ToString().ToUpper());
                        sScriptPrimerInterprete.Append("; ");
                    }
                    else
                    {
                        //HtmlInterpreteParticipante = HtmlInterpreteParticipante + "" + ". FIRMA E IMPRIME SU HUELLA DACTILAR " + Row["vParticipante"].ToString() + " EL " + strFecha + "";
                        HtmlInterpreteParticipante = HtmlInterpreteParticipante + "" + ". FIRMA E IMPRIME SU HUELLA DACTILAR " + Row["vParticipante"].ToString() + " " + strFecha + "";
                    }
                    sContarAuxInterprete++;

                    #endregion

                }
                
                else if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.APODERADO))
                {

                    #region Apoderado


                    sContarApoderado++;
                    strGenerpIdentificadoApoderado = "IDENTIFICADO";
                    if (Convert.ToInt16(Row["sGeneroId"].ToString()) == (int)Enumerador.enmGenero.FEMENINO)
                    {
                        sContarMujeresApoderado++;
                        strGenerpIdentificadoApoderado = "IDENTIFICADA";
                    }
                    else
                    {
                        sContarHombresApoderado++;
                        //strGenerpIdentificadoApoderado = "IDENTIFICADO";

                    }
                    sScriptApoderado.Append(Row["vParticipante"].ToString().ToUpper().Trim());
                    //sScriptApoderado.Append(", DE NACIONALIDAD " + Row["vNacionalidad"].ToString());
                    //sScriptApoderado.Append(", DE PROFESIÓN U OCUPACIÓN  " + Row["vOcupacion"].ToString());
                    sScriptApoderado.Append(", " + strGenerpIdentificadoApoderado + " CON  " + Row["vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString());
                    //sScriptApoderado.Append(", ESTADO CIVIL  " + Row["vEstadoCivil"].ToString());
                    //sScriptApoderado.Append(", CON DOMICILIO EN " + Row["vDireccion"].ToString());

                    if (sContarApoderadoCant == sContarApoderado)
                    {
                        sScriptApoderado.Append("; ");
                    }
                    else {
                        sScriptApoderado.Append(" Y/O ");
                    }
                    

                    #endregion
                }

                else if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO))
                {

                    #region TestigoRuego

                    sContarTestigo++;


                    if (sContarTestigo == 1)
                    {
                        String str_Par = "";
                        foreach (DataRow Row1 in dt.Rows)
                        {
                            if (Row["anpa_iReferenciaParticipanteId"].ToString() == Row1["anpa_iActoNotarialParticipanteId"].ToString() && Row1["vtipoParticipante"].ToString() == Enumerador.enmNotarialTipoParticipante.OTORGANTE.ToString())
                            {
                                str_Par = Row1["vParticipante"].ToString().ToUpper();
                                break;
                            }
                        }

                        bool bImprimeHuella = true;
                        foreach (DataRow Row1 in dt.Rows)
                        {
                            if ((Convert.ToBoolean(Row1["anpa_bFlagHuella"]) == false) && Row1["vtipoParticipante"].ToString() == Enumerador.enmNotarialTipoParticipante.OTORGANTE.ToString())
                            {
                                bImprimeHuella = false;
                                break;
                            }
                        }
                        if (bImprimeHuella)
                        {
                            //HtmlTestigoRuegoParticipanteAux = HtmlTestigoRuegoParticipanteAux + ". FIRMA E IMPRIME SU HUELLA DACTILAR " + Row["vParticipante"].ToString().ToUpper() + " EL " + strFecha + ", IMPRIME HUELLA DACTILAR " + str_Par + " EL " + strFecha + "";
                            HtmlTestigoRuegoParticipanteAux = HtmlTestigoRuegoParticipanteAux + ". FIRMA E IMPRIME SU HUELLA DACTILAR " + Row["vParticipante"].ToString().ToUpper() + " " + strFecha + ", IMPRIME HUELLA DACTILAR " + str_Par + " " + strFecha + "";
                        }
                        else {
                            //HtmlTestigoRuegoParticipanteAux = HtmlTestigoRuegoParticipanteAux + ". FIRMA E IMPRIME SU HUELLA DACTILAR " + Row["vParticipante"].ToString().ToUpper() + " EL " + strFecha + "";
                            HtmlTestigoRuegoParticipanteAux = HtmlTestigoRuegoParticipanteAux + ". FIRMA E IMPRIME SU HUELLA DACTILAR " + Row["vParticipante"].ToString().ToUpper() + " " + strFecha + "";
                        }
                        //HtmlTestigoRuegoParticipanteAux = HtmlTestigoRuegoParticipanteAux + ". FIRMA E IMPRIME SU HUELLA DIGITAL " + Row["vParticipante"].ToString() + ", " + strFecha;
                    }
                    else
                    {
                        HtmlInterpreteParticipante = HtmlTestigoRuegoParticipanteAux + "" + ". FIRMA E IMPRIME SU HUELLA DACTILAR " + Row["vParticipante"].ToString().ToUpper() + ", " + strFecha;
                    }

                    #endregion
                }
            }

            /* OTORGANTES */
            if (sContarMujeresOtorgantes == 1 && sContarHombresOtorgantes == 0)
            {
                strLA_EL_LOS_LAS = "LA";
                strPluralSingularOtorgante = "OTORGANTE";
            }
            else if (sContarMujeresOtorgantes >= 1 && sContarHombresOtorgantes == 0)
            {
                strLA_EL_LOS_LAS = "LAS";
                strPluralSingularOtorgante = "OTORGANTES";
            }

            else if (sContarMujeresOtorgantes == 0 && sContarHombresOtorgantes == 1)
            {
                strLA_EL_LOS_LAS = "EL";
                strPluralSingularOtorgante = "OTORGANTE";
            }
            else if (sContarMujeresOtorgantes == 0 && sContarHombresOtorgantes >= 1)
            {
                strLA_EL_LOS_LAS = "LOS";
                strPluralSingularOtorgante = "OTORGANTES";
            }
            else if (sContarMujeresOtorgantes >= 1 && sContarHombresOtorgantes >= 1)
            {
                strLA_EL_LOS_LAS = "LOS";
                strPluralSingularOtorgante = "OTORGANTES";
            }
            else
            {
                strLA_EL_LOS_LAS = "EL";
                strPluralSingularOtorgante = "OTORGANTE";
            }



            #region Interprete PluralSingularLaElInterprete
            /* INTERPRETE */
            if (sContarMujeresInterprete == 1 && sContarHombresInterprete == 0)
            {
                strPluralSingularLaElInterprete = "LA";
            }
            else if (sContarMujeresInterprete >= 1 && sContarHombresInterprete == 0)
            {
                strPluralSingularLaElInterprete = "LAS";
            }

            else if (sContarMujeresInterprete == 0 && sContarHombresInterprete == 1)
            {
                strPluralSingularLaElInterprete = "EL";
            }
            else if (sContarMujeresInterprete == 0 && sContarHombresInterprete >= 1)
            {
                strPluralSingularLaElInterprete = "LOS";
            }

            else if (sContarMujeresInterprete >= 1 && sContarHombresInterprete >= 1)
            {
                strPluralSingularLaElInterprete = "LOS";
            }
            else
            {
                strPluralSingularLaElInterprete = "EL";
            }

            #endregion

            #region Apoderado PluralSingularLaElApoderado

            /* APODERADOS */
            if (sContarMujeresApoderado == 1 && sContarHombresApoderado == 0)
            {
                strPluralSingularLaElApoderado = "LA";
                strPluralSingularApoderado = "APODERADA";
            }
            else if (sContarMujeresApoderado >= 1 && sContarHombresApoderado == 0)
            {
                strPluralSingularLaElApoderado = "LAS";
                strPluralSingularApoderado = "APODERADAS";

            }

            else if (sContarMujeresApoderado == 0 && sContarHombresApoderado == 1)
            {
                strPluralSingularLaElApoderado = "EL";
                strPluralSingularApoderado = "APODERADO";

            }
            else if (sContarMujeresApoderado == 0 && sContarHombresApoderado >= 1)
            {
                strPluralSingularLaElApoderado = "LOS";
                strPluralSingularApoderado = "APODERADOS";

            }

            else if (sContarMujeresApoderado >= 1 && sContarHombresApoderado >= 1)
            {
                strPluralSingularLaElApoderado = "LOS";
                strPluralSingularApoderado = "APODERADOS";

            }
            else
            {
                strPluralSingularLaElApoderado = "EL";
                strPluralSingularApoderado = "APODERADO";
            }

            #endregion

            String HtmlAux = String.Empty;

            if (sContarOtogartes <= 1)
            {
                strPluralSingularInteligente = "INTELIGENTE";
                strPluralSinguralProcede = "PROCEDE";
                strPluralSinguralQuien = "QUIEN";
                strPluralSingularExpreso = "EXPRESÓ";
                HtmlAux = " QUIEN PROCEDE POR PROPIO DERECHO.";
                strPluralSinguralEsOtorgante = "ES";
                strPluralSingularOtorga = "OTORGA";
            }
            else
            {
                strPluralSingularInteligente = "INTELIGENTES";
                strPluralSinguralProcede = "PROCEDEN";
                strPluralSinguralQuien = "QUIENES";
                strPluralSingularExpreso = "EXPRESARON";
                HtmlAux = " QUIENES PROCEDEN POR PROPIO DERECHO.";
                strPluralSinguralEsOtorgante = "SON";
                strPluralSingularOtorga = "OTORGAN";
            }

            if (sContarApoderado <= 1)
            {
                strPluralSinguralQueda = "QUEDA";
                strPluralSinguralFacultado = "FACULTADO";
            }
            else
            {
                strPluralSinguralQueda = "QUEDAN";
                strPluralSinguralFacultado = "FACULTADOS";
            }

            if (sContarInterprete <= 1)
            {
                strPluralSingularInterprete = "INTÉRPRETE";
                strPluralSinguralQuienInterprete = "QUIEN";
                strPluralSinguralDeclaraInterprete = "DECLARA";
            }
            else
            {
                strPluralSingularInterprete = "INTÉRPRETES";
                strPluralSinguralQuienInterprete = "QUIENES";
                strPluralSinguralDeclaraInterprete = "DECLARAN";
            }

            if (HtmlTestigoRuegoParticipante == String.Empty)
            {
                sScript.Append(sScriptOtorgante);
                sScript.Append(HtmlAux + "</p>");

            }
            else
                sScript.Append(HtmlTestigoRuegoParticipante);



            if (ExisteInterorete)
            {
                #region SiExisteInterprete


                #region Lista de Interpretes

                List<ClaseInterprete> listaInterpretes = new List<ClaseInterprete>();

                StringBuilder sScriptInterprete2 = new StringBuilder();
                ClaseInterprete objInterprete;

                foreach (DataRow Row1 in dt.Rows)
                {
                    if (Row1["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.INTERPRETE))
                    {
                        sScriptInterprete2.Append(Row1["vParticipante"].ToString().ToUpper().Trim());
                        // sScriptInterprete2.Append(", DE NACIONALIDAD " + Row1["vNacionalidad"].ToString().ToUpper());
                        // sScriptInterprete2.Append(", DE PROFESIÓN U OCUPACIÓN  " + Row1["vOcupacion"].ToString().ToUpper());
                        sScriptInterprete2.Append(", " + strGenerpIdentificadoInterprete + " CON  " + Row1["vTipoDocumento"].ToString().ToUpper() + " " + Row1["vDocumentoNumero"].ToString().ToUpper());
                        // sScriptInterprete2.Append(", ESTADO CIVIL  " + Row1["vEstadoCivil"].ToString().ToUpper());
                        // sScriptInterprete2.Append(", CON DOMICILIO EN " + Row1["vDireccion"].ToString().ToUpper());
                        sScriptInterprete2.Append("; ");

                        objInterprete = new ClaseInterprete();
                        objInterprete.idioma = Row1["vIdioma"].ToString().ToUpper();
                        objInterprete.SetDatosPersonales(sScriptInterprete2);

                        listaInterpretes.Add(objInterprete);

                        sScriptInterprete2.Clear();
                    }
                }

                #endregion

                #region Enlace Otorgante con el Interprete por el Idioma Extranjero

                string strIdiomaExtranjero = "";

                foreach (DataRow Row2 in dt.Rows)
                {
                    if (Row2["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.OTORGANTE))
                    {
                        if (sIdiomaCastellano != Row2["sIdiomaId"].ToString())
                        {
                            if (Convert.ToInt16(Row2["sGeneroId"].ToString()) == (int)Enumerador.enmGenero.FEMENINO)
                            {
                                strLA_EL_LOS_LAS_Otorgante = "LA";
                            }
                            else
                            {
                                strLA_EL_LOS_LAS_Otorgante = "EL";
                            }
                            strIdiomaExtranjero = Row2["vIdioma"].ToString().ToUpper();


                            foreach (ClaseInterprete objInterpretes2 in listaInterpretes)
                            {
                                if (objInterpretes2.idioma.Equals(strIdiomaExtranjero))
                                {
                                    sScript.Append("<p align=\"justify\"; style=\"background-color:transparent;\">");

                                    sScript.Append(strLA_EL_LOS_LAS_Otorgante + " OTORGANTE " + Row2["vParticipante"].ToString().ToUpper().Trim());
                                    sScript.Append(" ES HÁBIL EN EL IDIOMA " + objInterpretes2.idioma + " Y NO CONOCE EL IDIOMA CASTELLANO, ");
                                    sScript.Append(" POR LO QUE DESIGNA A " + objInterpretes2.GetDatosPersonales().ToString());
                                    sScript.Append(" QUIEN PROCEDE EN CALIDAD DE INTÉRPRETE, DE CONFORMIDAD CON LO ESTABLECIDO EN EL ARTÍCULO 30° DEL DECRETO LEGISLATIVO N° 1049, Y ");
                                    sScript.Append("MANIFIESTA SER HÁBIL EN EL IDIOMA " + objInterpretes2.idioma + " Y EL IDIOMA CASTELLANO, ");
                                    sScript.Append("Y TENER EL CONOCIMIENTO Y EXPERIENCIA SUFICIENTE PARA EFECTUAR LA TRADUCCIÓN QUE LE HA SIDO SOLICITADA.");

                                    sScript.Append("</p>");
                                    break;
                                }
                            }
                        }
                    }
                }

                #endregion

                sScript.Append("<p align=\"justify\"; style=\"background-color:transparent;\">");

                sScript.Append(strLA_EL_LOS_LAS + " " + strPluralSingularOtorgante + " Y " + strPluralSingularLaElInterprete + " " + strPluralSingularInterprete);
                sScript.Append(" SON MAYORES DE EDAD, PROCEDEN CON CAPACIDAD, LIBERTAD PARA CELEBRAR CONTRATOS Y PLENO CONOCIMIENTO DEL ACTO JURIDICO QUE " + strPluralSingularOtorga + ", DE LO QUE DOY FE; ");
                sScript.Append("MANIFESTÁNDOME " + strLA_EL_LOS_LAS + " " + strPluralSingularOtorgante + " SU VOLUNTAD DE OTORGAR UN PODER FUERA DE REGISTRO A FAVOR DE ");
                sScript.Append(sScriptApoderado);
                if (sContarApoderado > 1)
                { sScript.Append(" PARA QUE EN SU NOMBRE Y REPRESENTACION REALICEN INDISTINTAMENTE LOS SIGUIENTES ACTOS:"); }
                else { sScript.Append(" PARA QUE EN SU NOMBRE Y REPRESENTACION REALICE LOS SIGUIENTES ACTOS:"); }
                sScript.Append("</p>");

                #endregion
            }
            else
            {
                #region Si no existe Interprete

                sScript.Append("<p align=\"justify\"; style=\"background-color:transparent;\">");

                sScript.Append(strLA_EL_LOS_LAS + " " + strPluralSingularOtorgante + " " + strPluralSinguralEsOtorgante + " " + strPluralSingularInteligente + " EN EL IDIOMA CASTELLANO, ");
                sScript.Append(strPluralSinguralProcede + " CON CAPACIDAD PARA CONTRATAR, PLENA LIBERTAD PARA CELEBRAR CONTRATOS Y PLENO CONOCIMIENTO DEL ACTO JURIDICO QUE " + strPluralSingularOtorga + ", DE LO QUE DOY FE, ");
                sScript.Append(strPluralSinguralQuien + " ME " + strPluralSingularExpreso + " SU VOLUNTAD DE OTORGAR UN PODER FUERA DE REGISTRO A FAVOR DE ");
                sScript.Append(sScriptApoderado);
                if (sContarApoderado > 1)
                { sScript.Append(" PARA QUE EN SU NOMBRE Y REPRESENTACION REALICEN INDISTINTAMENTE LOS SIGUIENTES ACTOS:"); }
                else { sScript.Append(" PARA QUE EN SU NOMBRE Y REPRESENTACION REALICE LOS SIGUIENTES ACTOS:"); }
                sScript.Append("</p>");

                #endregion

            }


            #region Contenido del Cuerpo

            sScript.Append("<tab></tab>");

            sScript.Append("<br />");
            sScript.Append(txtCuerpo.Text);
            sScript.Append("<br />");
            sScript.Append("<tab></tab>");

            sScript.Append("<p align=\"justify\"; style=\"background-color:transparent;\">");
            sScript.Append("EN EJERCICIO DEL PRESENTE MANDATO, ");
            sScript.Append(strPluralSingularLaElApoderado);
            sScript.Append(" " + strPluralSingularApoderado + " " + strPluralSinguralQueda + " " + strPluralSinguralFacultado);
            sScript.Append(" PARA EFECTUAR LOS TRÁMITES NECESARIOS Y FIRMAR ");
            sScript.Append("TODOS LOS DOCUMENTOS QUE SEAN REQUERIDOS PARA LOS ");
            sScript.Append("FINES INDICADOS EN EL PRESENTE INSTRUMENTO, DEJANDO ");
            sScript.Append("CONSTANCIA QUE EL PRESENTE PODER TENDRÁ VIGENCIA DE UN AÑO. ");
            sScript.Append("</p>");

            sScript.Append("<tab></tab>");

            #endregion

            #region Contenido de la Conclusión

            sScript.Append("<p align=\"justify\"; style=\"background-color:transparent;\">");

            sScript.Append("<b>");

            sScript.Append("CONCLUSIÓN: ");
            sScript.Append("</b>");

            sScript.Append("FORMALIZADO EL INSTRUMENTO, YO, ");
            sScript.Append(strFuncionarioAutorizador);
            sScript.Append(", ");
            sScript.Append(strCargoFuncionarioAutorizador);
            sScript.Append(" EN ");
            sScript.Append(strUbigeoOficinaConsularCiudad);
            sScript.Append(", DOY FE DE HABER INSTRUÍDO");


            sScript.Append(" A " + strLA_EL_LOS_LAS);
            sScript.Append(" " + strPluralSingularOtorgante);
            sScript.Append(", SOBRE EL OBJETO Y FINES DEL PRESENTE PODER, QUE FUE LEÍDO ");
            sScript.Append("POR MÍ EN SU INTEGRIDAD, ");

            if (ExisteInterorete)
            {
                sScript.Append("Y QUE HA VERIFICADO LA EXACTITUD DE SU TEXTO, SIENDO QUE LA LECTURA, ADVERTENCIA Y DECLARACIONES HAN SIDO TRADUCIDAS ");
                sScript.Append("SIMULTANEAMENTE AL IDIOMA ");
                sScript.Append(strIdiomas);
                sScript.Append(", POR " + strPluralSingularLaElInterprete + " " + strPluralSingularInterprete + ","); // + HtmlInterpreteParticipante);
                sScript.Append(" " + strPluralSinguralQuienInterprete + " " + strPluralSinguralDeclaraInterprete + " BAJO SU RESPONSABILIDAD QUE LA TRADUCCIÓN QUE HA REALIZADO ES CONFORME Y EXACTA, ");
                sScript.Append("DESPUÉS DE LO CUAL SE AFIRMARON Y RATIFICARON ");
            }
            else
            {
                sScript.Append("QUIEN DESPUÉS DE LO CUAL SE AFIRMÓ Y RATIFICÓ ");
            }


            sScript.Append("EN TODO SU CONTENIDO, DECLARANDO ADEMÁS, QUE HA");
            sScript.Append(vLetraPlural);
            sScript.Append(" VERIFICADO QUE LOS DATOS CONSIGNADOS COMO: NOMBRE, ");
            sScript.Append("APELLIDOS, ESTADO CIVIL, OCUPACIÓN, DIRECCIÓN E IDENTIFICACIÓN ");
            sScript.Append("SON CORRECTOS. CONCLUYE EL PROCESO DE FIRMAS, ANTE MÍ, ");
            sScript.Append(strFuncionarioAutorizador);
            sScript.Append(", ");
            sScript.Append(strCargoFuncionarioAutorizador);
            sScript.Append(" EN ");
            sScript.Append(strUbigeoOficinaConsularCiudad);
            //sScript.Append(", EL ");
            sScript.Append(", ");
            sScript.Append(strFecha);
            //sScript.Append(". ");

            //if (HtmlTestigoRuegoParticipanteAux == String.Empty)
                sScript.Append(HtmlOtorganteParticipante);
            //else
                sScript.Append(HtmlTestigoRuegoParticipanteAux);

            sScript.Append(HtmlInterpreteParticipante);
            sScript.Append("; DE LO QUE DOY FE.");
            sScript.Append("</p>");

            sScript.Append("<br />");
            sScript.Append("<tab></tab>");

            #endregion

            sScript.Append(txtInfoAdicional.Text);



            return sScript.ToString();

        }

        private string ObtenerDocumentoCertificado(DataTable dt)
        {
            string strLetraGenero = "O";
            string vObservacion = string.Empty;

            if (dt.Rows[0]["vObservaciones"] != null)
            {
                if (dt.Rows[0]["vObservaciones"].ToString() != string.Empty)
                {
                    vObservacion = dt.Rows[0]["vObservaciones"].ToString().ToUpper();
                }
            }

            if (dt.Rows[0]["GeneroPersona"].ToString().Equals(Enumerador.enmGenero.FEMENINO.ToString()))
                strLetraGenero = "A";


            String IdentificadoGeneroFuncionario = String.Empty;
            //-------------------------------------------------------------
            //Fecha: 13/12/2017
            //Objetivo: Si el valos es 1 es mujer / si es 2 es hombre
            //-------------------------------------------------------------

            if (dt.Rows[0]["GeneroFuncionario"].ToString().Trim().Equals("2"))
            {
                IdentificadoGeneroFuncionario = "IDENTIFICADA";
            }
            else
            {
                IdentificadoGeneroFuncionario = "IDENTIFICADO";
            }

            if (IdentificadoGeneroFuncionario == String.Empty) IdentificadoGeneroFuncionario = "IDENTIFICADO";

            StringBuilder sScript = new StringBuilder();
            //sScript.Append("<br />");
            //sScript.Append("<p align=\"center\">");
            //sScript.Append("<h1><font face=\"impact\">CERTIFICADO DE SUPERVIVENCIA</font></h1>");
            //sScript.Append("</p>");
            //sScript.Append("<br />");
            //sScript.Append("<tab></tab>");
            sScript.Append("<p align=\"justify\"; style=\"background-color:transparent;\">");
            sScript.Append("EN LA CIUDAD DE ");
            sScript.Append(dt.Rows[0]["Ciudad"].ToString().ToUpper());
            //sScript.Append(", EL ");
            sScript.Append(", ");
            sScript.Append(Util.ObtenerFechaParaDocumentoLegalProtocolar(Comun.FormatearFecha(dt.Rows[0]["Fecha"].ToString())).ToUpper().Trim());
            //sScript.Append(", EN EL CONSULADO GENERAL DEL PERÚ EN ");
            if (dt.Rows[0]["NombreOficinaConsular"].ToString().Contains("SECCIÓN"))
            {
                sScript.Append(", EN LA ");
            }
            else {
                sScript.Append(", EN EL ");
            }
            sScript.Append(dt.Rows[0]["NombreOficinaConsular"].ToString().ToUpper());
            //sScript.Append(dt.Rows[0]["OficinaConsular"].ToString().ToUpper());
            sScript.Append(", YO, ");
            sScript.Append(dt.Rows[0]["NombreFuncionario"].ToString().ToUpper());
            sScript.Append(", " + IdentificadoGeneroFuncionario);
            sScript.Append(" CON DOCUMENTO NACIONAL DE IDENTIDAD ");
            sScript.Append(dt.Rows[0]["NumeroDocumentoFuncionario"].ToString().ToUpper());
            sScript.Append(", ");
            sScript.Append(dt.Rows[0]["CargoFuncionario"].ToString().ToUpper());
            sScript.Append(" EN ");
            sScript.Append(dt.Rows[0]["CiudadOficinaFuncionario"].ToString().ToUpper());
            sScript.Append("; <b>CERTIFICO</b>: QUE HE VERIFICADO LA SUPERVIVENCIA DE ");
            sScript.Append(dt.Rows[0]["NombrePersona"].ToString().ToUpper());
            if (dt.Rows[0]["NacionalidadPersona"].ToString() != string.Empty)
            {
                sScript.Append(", DE NACIONALIDAD ");
                sScript.Append(dt.Rows[0]["NacionalidadPersona"].ToString());
            }
            sScript.Append(", IDENTIFICAD" + strLetraGenero);
            sScript.Append(" CON ");
            sScript.Append(dt.Rows[0]["TipoDocumentoPersona"].ToString().ToUpper());
            sScript.Append(" ");
            sScript.Append(dt.Rows[0]["NumeroDocumentoPersona"].ToString().ToUpper());
            if (dt.Rows[0]["EstadoCivilPersona"].ToString() != string.Empty)
            {
                sScript.Append(", DE ESTADO CIVIL ");
                sScript.Append(dt.Rows[0]["EstadoCivilPersona"].ToString().ToUpper());
            }
            if (dt.Rows[0]["ResidenciaPersona"].ToString() != string.Empty)
            {
                sScript.Append(", DOMICILIAD");
                sScript.Append(strLetraGenero);
                sScript.Append(" EN ");
                sScript.Append(dt.Rows[0]["ResidenciaPersona"].ToString().ToUpper());
            }
            sScript.Append(".");
            sScript.Append("</p>");

            if (vObservacion != string.Empty)
            {
                sScript.Append("<p align=\"justify\"; style=\"background-color:transparent;\">");
                sScript.Append(vObservacion);
                sScript.Append("</p>");
            }
            sScript.Append("<br />");
            sScript.Append("<tab></tab>");

            return sScript.ToString();
        }

        private string ObtenerDocumentoAutorizacionMenor(DataTable dt)
        {

            //String sIdiomaCastellano = ConfigurationManager.AppSettings["NotarialIdioma"].ToString();

            String sIdiomaCastellano = Session["NotarialIdioma"].ToString();

            ActoNotarialConsultaBL BL = new ActoNotarialConsultaBL();
            DataTable dtDatosEscritura = new DataTable();


            String strObservacion = String.Empty;

            strObservacion = dt.Rows[0]["acno_vObservaciones"].ToString();
            string strSubTipoActoExtraProtocolar = dt.Rows[0]["vSubTipoNotarialExtraProtocolar"].ToString();

            dtDatosEscritura = BL.ActonotarialObtenerDatosPrincipales(Convert.ToInt64(ViewState[Constantes.CONST_SESION_ACTONOTARIAL_ID]), Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));

            String strOficinaConsularNombre = dtDatosEscritura.Rows[0]["NombreOficinaConsular"].ToString().ToUpper().Trim();
            String strFuncionarioAutorizador = dtDatosEscritura.Rows[0]["NombreFuncionario"].ToString().ToUpper().Trim();
            String strCargoFuncionarioAutorizador = dtDatosEscritura.Rows[0]["CargoFuncionario"].ToString().ToUpper().Trim();
            String strUbigeoOficinaConsular = dtDatosEscritura.Rows[0]["CiudadOficinaConsular"].ToString().ToUpper().Trim();
            String strUbigeoOficinaConsularCiudad = dtDatosEscritura.Rows[0]["CiudadOficinaConsular"].ToString().ToUpper().Trim() + ", " + dtDatosEscritura.Rows[0]["Provincia"].ToString().ToUpper().Trim();
            String strFecha = Util.ObtenerFechaParaDocumentoLegalProtocolar(Comun.FormatearFecha(dtDatosEscritura.Rows[0]["Fecha"].ToString())).ToUpper();
            String StrNumeroDocumentoFuncionario = dtDatosEscritura.Rows[0]["NumeroDocumentoFuncionario"].ToString().ToUpper().Trim();
            String GeneroFuncionario = dtDatosEscritura.Rows[0]["sGeneroFuncionario"].ToString().Trim();
            string strSubTipoActoViajeMejor = dtDatosEscritura.Rows[0]["vSubTipoActoNotarialExtraProtocolar"].ToString().Trim();
            string strNroEscrituraPublica = dtDatosEscritura.Rows[0]["NumeroEscrituraPublica"].ToString().Trim();
            string strNroPartidaRegistral = dtDatosEscritura.Rows[0]["vPartidaRegistral"].ToString().Trim();

            StringBuilder sScript = new StringBuilder();
            StringBuilder sScriptPadre = new StringBuilder();
            StringBuilder sScriptMadre = new StringBuilder();
            StringBuilder sScriptMenor = new StringBuilder();
            StringBuilder sScriptAcompanate = new StringBuilder();
            StringBuilder sScriptInterprete = new StringBuilder();
            StringBuilder sScriptApoderado = new StringBuilder();


            String strPluralSingularComparece = String.Empty;

            String strGenerpIdentificadoMadre = String.Empty;

            String strGeneroHijo = String.Empty;
            String strGeneroIdentificado = String.Empty;
            string strHijoSoloSola = string.Empty;

            String strGeneroHijoAcompa = String.Empty;
            String strGeneroIdentificadoAcompa = String.Empty;

            String strArticuloEL_LA = String.Empty;
            String strSingularPluralCOMPARECIENTE = String.Empty;
            String StrSingularPluralQuien = String.Empty;
            String StrSingularPluralDeclara = String.Empty;
            String StrSingularPluralAsume = String.Empty;
            String strSingularPluralRatifican = String.Empty;
            String strSingularPluralFirma = String.Empty;
            String strSingularPluralsON = String.Empty;
            String strSingularPluralHabilInter = String.Empty;
            String strGeneroIdentificadoInterprete = String.Empty;
            string strSingularPluralAutorizan = string.Empty;
            string strGeneroIdentificadoApoderado = String.Empty;
            string strGeneroRepresentadoApoderado = String.Empty;

            String strGenerpLA_ELInterprete = String.Empty;
            String strPluralSingularInterprete = String.Empty;

            String strPluralSinguralInterprete = String.Empty;
            String strPluralSinguralManifiesta = String.Empty;
            String strPluralSinguralQueda = String.Empty;
            String strPluralSinguralFacultado = String.Empty;

            String strPluralSinguralQuienInterprete = String.Empty;
            String strPluralSinguralDeclaraInterprete = String.Empty;


            String strIdiomaMadre = String.Empty;
            String strIdiomaPadre = String.Empty;
            String strIdioma = String.Empty;
            bool bIdiomaPadre = false;
            bool bIdiomaMadre = false;

            Int16 sContarPadres = 0;
            Int16 sContarInterprete = 0, sContarMujeresInterprete = 0, sContarHombresInterprete = 0;
            Int16 sContarMujeresApoderado = 0, sContarHombresApoderado = 0;

            Int16 intContarHijos = 0;
            Int16 intNumeroHijos = 0;
            bool ExisteHijoVarones = false;

            String HtmlTestigoRuegoParticipante = String.Empty;
            String strGeneroIdentificadoFuncionario = String.Empty;
            Boolean ExisteInterprete = false, bExisteTestigoRuego = false, bExisteApoderado = false, bExisteCompania = false;

            strGeneroIdentificadoFuncionario = "IDENTIFICADA";
            if (GeneroFuncionario == "1")
            {
                strGeneroIdentificadoFuncionario = "IDENTIFICADO";
            }

            sScript.Append("<p align=\"justify\"; style=\"background-color:transparent;\">");

            sScript.Append("EN LA CIUDAD DE ");
            sScript.Append(strUbigeoOficinaConsular);
            //sScript.Append(", EL ");
            sScript.Append(", ");
            sScript.Append(strFecha);
            sScript.Append(", ANTE MÍ, ");
            sScript.Append(strFuncionarioAutorizador);
            sScript.Append(", " + strGeneroIdentificadoFuncionario + " CON DOCUMENTO NACIONAL DE IDENTIDAD");
            sScript.Append(" ");
            sScript.Append(StrNumeroDocumentoFuncionario);
            sScript.Append(", ");
            sScript.Append(strCargoFuncionarioAutorizador);
            sScript.Append(" EN ");
            sScript.Append(strUbigeoOficinaConsularCiudad);

            //-------------------------------------------------------
            bool bExistePadre = false;
            bool bExisteMadre = false;

            foreach (DataRow Row in dt.Rows)
            {
                if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.MENOR))
                {
                    intNumeroHijos++;

                    if ((int)Enumerador.enmGenero.MASCULINO == Convert.ToUInt16(Row["sGeneroId"].ToString()))
                    {
                        ExisteHijoVarones = true;
                    }
                }
                if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.PADRE))
                {
                    strGeneroRepresentadoApoderado = "REPRESENTADO";
                    bExistePadre = true;
                }
                if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.MADRE))
                {
                    strGeneroRepresentadoApoderado = "REPRESENTADA";
                    bExisteMadre = true;
                }
                if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.ACOMPANANTE))
                {
                    bExisteCompania = true;
                }
            }

            if (bExistePadre && bExisteMadre)
            {
                strGeneroRepresentadoApoderado = "REPRESENTADOS";
            }

            Boolean bHuellaMadre = false;
            Boolean bHuellaPadre = false;

            foreach (DataRow Row in dt.Rows)
            {

                if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.MADRE))
                {
                    #region Madre

                    if (strSubTipoActoViajeMejor == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - PADRE REPRESENTA A MADRE" ||
                        strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A MADRE" ||
                        strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE Y MADRE")
                    {
                        sScriptMadre.Append(Row["vParticipante"].ToString().ToUpper());
                        sScriptMadre.Append(", DE NACIONALIDAD " + Row["vNacionalidad"].ToString());
                        sScriptMadre.Append(", IDENTIFICADA CON  " + Row["vTipoDocumento"].ToString().ToUpper() + " " + Row["vDocumentoNumero"].ToString().ToUpper());
                    }
                    else {
                        //-------------------------------------------------
                        //Fecha: 13/09/2017
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: Asignar si hizo check en No Huella  
                        //-------------------------------------------------

                        if (Row["anpa_bFlagHuella"].ToString() == "True")
                        {
                            bHuellaMadre = true;
                        }
                        else
                        {
                            bHuellaMadre = false;
                        }
                        //-------------------------------------------------
                        sScriptMadre.Append(Row["vParticipante"].ToString().ToUpper());
                        sScriptMadre.Append(", DE NACIONALIDAD " + Row["vNacionalidad"].ToString());
                        sScriptMadre.Append(", IDENTIFICADA CON  " + Row["vTipoDocumento"].ToString().ToUpper() + " " + Row["vDocumentoNumero"].ToString().ToUpper());
                        sScriptMadre.Append(", ESTADO CIVIL  " + Row["vEstadoCivil"].ToString().ToUpper());
                        sScriptMadre.Append(", DE OCUPACIÓN  " + Row["vOcupacion"].ToString().ToUpper());
                        sScriptMadre.Append(", CON DOMICILIO EN " + Row["vDireccion"].ToString().ToUpper());

                        if (sIdiomaCastellano != Row["sIdiomaId"].ToString().ToUpper())
                        {
                            if (Row["vIdioma"].ToString() != strIdioma)
                            {
                                strIdioma = strIdioma + ", " + Row["vIdioma"].ToString().ToUpper();
                                strIdiomaMadre = Row["vIdioma"].ToString().ToUpper();
                            }
                        }
                        sContarPadres++;

                        DataTable dtp1 = (DataTable)BL.ReporteAutorizacionViaje(Convert.ToInt32(ViewState[Constantes.CONST_SESION_ACTONOTARIAL_ID]),
                        Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), Convert.ToInt64(Row["anpa_iActoNotarialParticipanteId"].ToString()));


                        if (dtp1 != null)
                        {
                            if (dtp1.Rows.Count > 0)
                            {
                                //HtmlTestigoRuegoParticipante
                                foreach (DataRow RowTA in dtp1.Rows)
                                {
                                    if (RowTA["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO))
                                    {
                                        bExisteTestigoRuego = true;

                                        sScriptMadre.Append("; QUIEN SE ENCUENTRA INCAPACITADA DE FIRMAR POR " + Row["pers_vDescripcionIncapacidad"].ToString().ToUpper());


                                        if (bHuellaMadre)
                                        {
                                            sScriptMadre.Append(", POR LO QUE IMPRIME SU HUELLA DACTILAR, Y DESIGNA A ");
                                        }
                                        else
                                        {
                                            sScriptMadre.Append(", Y DESIGNA A ");
                                        }

                                        sScriptMadre.Append(RowTA["vParticipante"].ToString().ToUpper());
                                        //sScriptMadre.Append(", DE NACIONALIDAD " + RowTA["vNacionalidad"].ToString().ToUpper());
                                        //sScriptMadre.Append(", DE OCUPACIÓN " + RowTA["vOcupacion"].ToString().ToUpper());
                                        sScriptMadre.Append(", CON  " + RowTA["vTipoDocumento"].ToString().ToUpper() + " " + RowTA["vDocumentoNumero"].ToString().ToUpper());
                                        //sScriptMadre.Append(", ESTADO CIVIL  " + RowTA["vEstadoCivil"].ToString().ToUpper());
                                        //sScriptMadre.Append(", CON DOMICILIO EN " + RowTA["vDireccion"].ToString().ToUpper());
                                        sScriptMadre.Append(", QUIEN ACTÚA EN CALIDAD DE TESTIGO A RUEGO");
                                        sScriptMadre.Append(", DE CONFORMIDAD CON LO ESTABLECIDO EN EL ART.107° DEL DECRETO LEGISLATIVO N° 1049;");
                                        break;
                                    }
                                }

                            }
                        }
                    }
                    
                    #endregion
                }


                if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.PADRE))
                {
                    #region Padre
                    if (strSubTipoActoViajeMejor == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - MADRE REPRESENTA A PADRE" ||
                        strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE" ||
                        strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE Y MADRE")
                    {
                        sScriptPadre.Append(Row["vParticipante"].ToString().ToUpper());
                        sScriptPadre.Append(", DE NACIONALIDAD " + Row["vNacionalidad"].ToString().ToUpper());
                        sScriptPadre.Append(", IDENTIFICADO CON  " + Row["vTipoDocumento"].ToString().ToUpper() + " " + Row["vDocumentoNumero"].ToString().ToUpper());
                    }
                    else {
                        //-------------------------------------------------
                        //Fecha: 13/09/2017
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: Asignar si hizo check en No Huella  
                        //-------------------------------------------------
                        if (Row["anpa_bFlagHuella"].ToString() == "True")
                        {
                            bHuellaPadre = true;
                        }
                        else
                        {
                            bHuellaPadre = false;
                        }
                        //-------------------------------------------------

                        sScriptPadre.Append(Row["vParticipante"].ToString().ToUpper());
                        sScriptPadre.Append(", DE NACIONALIDAD " + Row["vNacionalidad"].ToString().ToUpper());
                        sScriptPadre.Append(", IDENTIFICADO CON  " + Row["vTipoDocumento"].ToString().ToUpper() + " " + Row["vDocumentoNumero"].ToString().ToUpper());
                        sScriptPadre.Append(", ESTADO CIVIL  " + Row["vEstadoCivil"].ToString().ToUpper());
                        sScriptPadre.Append(", DE OCUPACIÓN  " + Row["vOcupacion"].ToString().ToUpper());
                        sScriptPadre.Append(", CON DOMICILIO EN " + Row["vDireccion"].ToString().ToUpper());
                        sContarPadres++;

                        if (sIdiomaCastellano != Row["sIdiomaId"].ToString())
                        {
                            if (Row["vIdioma"].ToString() != strIdioma)
                            {
                                strIdioma = strIdioma + ", " + Row["vIdioma"].ToString().ToUpper();
                                strIdiomaPadre = Row["vIdioma"].ToString().ToUpper();
                            }
                        }


                        DataTable dtp1 = (DataTable)BL.ReporteAutorizacionViaje(Convert.ToInt32(ViewState[Constantes.CONST_SESION_ACTONOTARIAL_ID]),
                         Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), Convert.ToInt64(Row["anpa_iActoNotarialParticipanteId"].ToString()));


                        if (dtp1 != null)
                        {
                            if (dtp1.Rows.Count > 0)
                            {
                                foreach (DataRow RowTA in dtp1.Rows)
                                {
                                    if (RowTA["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO))
                                    {
                                        bExisteTestigoRuego = true;

                                        sScriptPadre.Append("; QUIEN SE ENCUENTRA INCAPACITADO DE FIRMAR POR " + Row["pers_vDescripcionIncapacidad"].ToString().ToUpper());

                                        if (bHuellaPadre)
                                        {
                                            sScriptPadre.Append(", POR LO QUE IMPRIME SU HUELLA DACTILAR, Y DESIGNA A ");
                                        }
                                        else
                                        {
                                            sScriptPadre.Append(", Y DESIGNA A ");
                                        }

                                        sScriptPadre.Append(dtp1.Rows[0]["vParticipante"].ToString().ToUpper());
                                        //sScriptPadre.Append(", DE NACIONALIDAD " + dtp1.Rows[0]["vNacionalidad"].ToString().ToUpper());
                                        //sScriptPadre.Append(", DE OCUPACIÓN " + dtp1.Rows[0]["vOcupacion"].ToString().ToUpper());
                                        sScriptPadre.Append(", CON  " + dtp1.Rows[0]["vTipoDocumento"].ToString().ToUpper() + " " + dtp1.Rows[0]["vDocumentoNumero"].ToString().ToUpper());
                                        //sScriptPadre.Append(", ESTADO CIVIL  " + dtp1.Rows[0]["vEstadoCivil"].ToString().ToUpper());
                                        //sScriptPadre.Append(", CON DOMICILIO EN " + dtp1.Rows[0]["vDireccion"].ToString().ToUpper());
                                        sScriptPadre.Append(", QUIEN ACTÚA EN CALIDAD DE TESTIGO A RUEGO");
                                        sScriptPadre.Append(", DE CONFORMIDAD CON LO ESTABLECIDO EN EL ART.107° DEL DECRETO LEGISLATIVO N° 1049;");
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }


                if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.MENOR))
                {
                    #region Menor

                    intContarHijos++;
                    if ((int)Enumerador.enmGenero.FEMENINO == Convert.ToUInt16(Row["sGeneroId"].ToString()))
                    {
                        if (intContarHijos == 1)
                        {
                            if (intNumeroHijos > 1)
                            {
                                if (ExisteHijoVarones)
                                {
                                    strGeneroHijo = "HIJOS";
                                }
                                else
                                {
                                    strGeneroHijo = "HIJAS";
                                }
                            }
                            else
                            {
                                strGeneroHijo = "HIJA";
                            }
                        }
                        //else
                        //{
                        //    if (intNumeroHijos == intContarHijos)
                        //    {
                        //        strGeneroHijo = "Y SU HIJA";
                        //    }
                        //    else
                        //    {
                        //        strGeneroHijo = "HIJA";
                        //    }
                        //}
                        strGeneroIdentificado = "IDENTIFICADA";
                    }
                    else
                    {
                        if (intContarHijos == 1)
                        {
                            if (intNumeroHijos > 1)
                            {
                                if (ExisteHijoVarones)
                                {
                                    strGeneroHijo = "HIJOS";
                                }
                            }
                            else
                            {
                                strGeneroHijo = "HIJO";
                            }
                        }
                        //else
                        //{
                        //    if (intNumeroHijos == intContarHijos)
                        //    {
                        //        strGeneroHijo = "Y SU HIJO";
                        //    }
                        //    else
                        //    {
                        //        strGeneroHijo = "HIJO";
                        //    }
                        //}
                        strGeneroIdentificado = "IDENTIFICADO";
                    }
                    if (intContarHijos == 1)
                    {
                        sScriptMenor.Append(strGeneroHijo + " ");
                    }
                    if (intNumeroHijos > 1)
                    {
                        if (intNumeroHijos == intContarHijos)
                        {
                            sScriptMenor.Append(" Y ");
                        }
                        else 
                        {
                            sScriptMenor.Append(", ");
                        }
                    }
                    
                    sScriptMenor.Append(Row["vParticipante"].ToString().ToUpper());
                    sScriptMenor.Append(", DE " + Row["Edad"].ToString().ToUpper());
                    sScriptMenor.Append(" DE EDAD, " + strGeneroIdentificado + " CON ");
                    sScriptMenor.Append(Row["vTipoDocumento"].ToString().ToUpper());
                    sScriptMenor.Append(" ");
                    sScriptMenor.Append(Row["vDocumentoNumero"].ToString().ToUpper());
                    if (intNumeroHijos == intContarHijos)
                    {
                        sScriptMenor.Append(", A ");
                        sScriptMenor.Append(Row["PaisDestino"].ToString().ToUpper());
                        sScriptMenor.Append(", ");
                    }
                    

                    #endregion
                }


                if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.ACOMPANANTE))
                {
                    #region Acompañante

                    sScriptAcompanate.Append(" EN COMPAÑÍA DE ");
                    sScriptAcompanate.Append(Row["vParticipante"].ToString().ToUpper());
                    sScriptAcompanate.Append(",");

                    if (Convert.ToInt16(Row["sGeneroId"].ToString()) == (int)Enumerador.enmGenero.FEMENINO)
                    {
                        sScriptAcompanate.Append(" IDENTIFICADA CON ");
                    }
                    else
                    {
                        sScriptAcompanate.Append(" IDENTIFICADO CON ");
                    }


                    sScriptAcompanate.Append(Row["vTipoDocumento"].ToString().ToUpper() + " " + Row["vDocumentoNumero"].ToString().ToUpper());
                    sScriptAcompanate.Append(",");

                    #endregion
                }


                if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.INTERPRETE))
                {
                    #region Interprete

                    ExisteInterprete = true;

                    if (Convert.ToInt16(Row["sGeneroId"].ToString()) == (int)Enumerador.enmGenero.FEMENINO)
                    {
                        sContarMujeresInterprete++;
                        strGeneroIdentificadoInterprete = "IDENTIFICADA";
                    }
                    else
                    {
                        sContarHombresInterprete++;
                        strGeneroIdentificadoInterprete = "IDENTIFICADO";
                    }

                    sScriptInterprete.Append(Row["vParticipante"].ToString().ToUpper());
                    //sScriptInterprete.Append(", DE NACIONALIDAD " + Row["vNacionalidad"].ToString().ToUpper());
                    //sScriptInterprete.Append(", OCUPACIÓN  " + Row["vOcupacion"].ToString().ToUpper());
                    sScriptInterprete.Append(", " + strGeneroIdentificadoInterprete + " CON  " + Row["vTipoDocumento"].ToString().ToUpper() + " " + Row["vDocumentoNumero"].ToString().ToUpper());
                    //sScriptInterprete.Append(", ESTADO CIVIL  " + Row["vEstadoCivil"].ToString().ToUpper());
                    //sScriptInterprete.Append(", CON DOMICILIO EN " + Row["vDireccion"].ToString().ToUpper());
                    sScriptInterprete.Append("; ");

                    #endregion
                }

                //-----------------------------------------------------------------------
                //Fecha: 14/09/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Incluir el Apoderado en la Autorización de Viaje de Menores
                //-----------------------------------------------------------------------

                if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.APODERADO))
                {
                    #region Apoderado

                    bExisteApoderado = true;
                    if (Convert.ToInt16(Row["sGeneroId"].ToString()) == (int)Enumerador.enmGenero.FEMENINO)
                    {
                        sContarMujeresApoderado++;
                        strGeneroIdentificadoApoderado = "IDENTIFICADA";
                    }
                    else
                    {
                        sContarHombresApoderado++;
                        strGeneroIdentificadoApoderado = "IDENTIFICADO";
                    }


                    if (strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A MADRE" ||
                        strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE" ||
                        strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE Y MADRE")
                    {
                        sScriptApoderado.Append(Row["vParticipante"].ToString().ToUpper());
                        sScriptApoderado.Append(", DE NACIONALIDAD " + Row["vNacionalidad"].ToString().ToUpper());
                        sScriptApoderado.Append(", " + strGeneroIdentificadoApoderado + " CON  " + Row["vTipoDocumento"].ToString().ToUpper() + " " + Row["vDocumentoNumero"].ToString().ToUpper());
                        sScriptApoderado.Append(", ESTADO CIVIL  " + Row["vEstadoCivil"].ToString().ToUpper());
                        sScriptApoderado.Append(", DE OCUPACIÓN  " + Row["vOcupacion"].ToString().ToUpper());
                        sScriptApoderado.Append(", CON DOMICILIO EN " + Row["vDireccion"].ToString().ToUpper());
                    }
                    else
                    {
                        sScriptApoderado.Append(", DEBIDAMENTE " + strGeneroRepresentadoApoderado + " POR ");
                        sScriptApoderado.Append(Row["vParticipante"].ToString().ToUpper());
                        sScriptApoderado.Append(", " + strGeneroIdentificadoApoderado + " CON  " + Row["vTipoDocumento"].ToString().ToUpper() + " " + Row["vDocumentoNumero"].ToString().ToUpper());
                        sScriptApoderado.Append(" MEDIANTE PODER POR ESCRITURA PÚBLICA N° " + Row["anpa_vNumeroEscrituraPublica"].ToString().ToUpper());
                        sScriptApoderado.Append(" INSCRITO EN LA PARTIDA N° " + Row["anpa_vNumeroPartida"].ToString().ToUpper());
                        sScriptApoderado.Append(" DE LOS REGISTROS PÚBLICOS; ");
                    }
                    
                    #endregion
                }
                //-----------------------------------------------------------------------
            }
            //-------------------------------------------------------

            if (strIdiomaMadre != String.Empty && strIdiomaPadre == String.Empty)
            {
                strIdioma = strIdiomaMadre;
                bIdiomaMadre = true;
            }
            else if (strIdiomaMadre == String.Empty && strIdiomaPadre != String.Empty)
            {
                strIdioma = strIdiomaPadre;
                bIdiomaPadre = true;
            }
            else if (strIdiomaMadre != String.Empty && strIdiomaPadre != String.Empty)
            {
                if (strIdiomaMadre == strIdiomaPadre)
                { strIdioma = strIdiomaMadre; }
                else { strIdioma = strIdiomaMadre + ", " + strIdiomaPadre;}
                //strIdioma = strIdiomaMadre + ", " + strIdiomaPadre;
                bIdiomaPadre = true;
                bIdiomaMadre = true;
            }


            if (sScriptMadre.Length > 0 && sScriptPadre.Length == 0)
            {
                sScript.Append("; COMPARECE:");
                sScript.Append(" " + sScriptMadre);
                sScript.Append(CrearTextoExtraInterprete(ExisteInterprete, bExisteTestigoRuego, strIdioma, sScriptInterprete.ToString(),true));
                //sScript.Append(".");
                strArticuloEL_LA = "LA";
                strSingularPluralCOMPARECIENTE = "COMPARECIENTE";
                StrSingularPluralQuien = "QUIEN";
                StrSingularPluralDeclara = "DECLARA";
                StrSingularPluralAsume = "ASUME";
                strSingularPluralRatifican = "RATIFICA";
                strSingularPluralFirma = "FIRMA";
                strSingularPluralsON = "ES";
                strSingularPluralHabilInter = "HÁBIL";
                strSingularPluralAutorizan = "AUTORIZA";
            }
            else if (sScriptMadre.Length == 0 && sScriptPadre.Length > 0)
            {
                sScript.Append("; COMPARECE:");
                sScript.Append(" " + sScriptPadre);
                sScript.Append(CrearTextoExtraInterprete(ExisteInterprete, bExisteTestigoRuego, strIdioma, sScriptInterprete.ToString(),true));
                //sScript.Append(".");
                strArticuloEL_LA = "EL";
                strSingularPluralCOMPARECIENTE = "COMPARECIENTE";
                StrSingularPluralQuien = "QUIEN";
                StrSingularPluralDeclara = "DECLARA";
                StrSingularPluralAsume = "ASUME";
                strSingularPluralRatifican = "RATIFICA";
                strSingularPluralFirma = "FIRMA";
                strSingularPluralsON = "ES";
                strSingularPluralHabilInter = "HÁBIL";
                strSingularPluralAutorizan = "AUTORIZA";
            }
            else if (sScriptMadre.Length > 0 && sScriptPadre.Length > 0)
            {
                string sStringApoderadoGenero = string.Empty;

                if (strGeneroIdentificadoApoderado == "IDENTIFICADA")
                {
                    sStringApoderadoGenero = "FACULTADA";
                }
                else {
                    sStringApoderadoGenero = "FACULTADO";
                }

               
                if (strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE Y MADRE")
                {
                    sScript.Append("; COMPARECE:");
                    sScript.Append(" " + sScriptApoderado);
                    sScript.Append(" QUIEN PROCEDE EN REPRESENTACIÓN DE ");
                    sScript.Append(" " + sScriptPadre);
                    sScript.Append(" Y ");
                    sScript.Append(sScriptMadre);

                    sScript.Append(", DEBIDAMENTE " + sStringApoderadoGenero  + " SEGÚN EL PODER INSCRITO EN LA PARTIDA ");
                    sScript.Append(strNroEscrituraPublica);
                    sScript.Append(" DEL REGISTRO DE MANDATOS Y PODERES DE LOS REGISTROS PÚBLICOS DE N° ");
                    sScript.Append(strNroPartidaRegistral);
                }
                else if (strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A MADRE")
                {
                    sScript.Append("; COMPARECEN:");
                    sScript.Append(" " + sScriptPadre);
                    sScript.Append(" Y ");
                    sScript.Append(sScriptApoderado);
                    //sScript.Append(CrearTextoExtraInterprete(ExisteInterprete, bExisteTestigoRuego, strIdiomaPadre, sScriptInterprete.ToString(), true));
                    sScript.Append(" QUIEN PROCEDE EN REPRESENTACIÓN DE ");
                    sScript.Append(sScriptMadre);

                    sScript.Append(", DEBIDAMENTE " + sStringApoderadoGenero + " SEGÚN EL PODER INSCRITO EN LA PARTIDA ");
                    sScript.Append(strNroEscrituraPublica);
                    sScript.Append(" DEL REGISTRO DE MANDATOS Y PODERES DE LOS REGISTROS PÚBLICOS DE N° ");
                    sScript.Append(strNroPartidaRegistral);
                }
                else if (strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE")
                {
                    sScript.Append("; COMPARECEN:");
                    sScript.Append(" " + sScriptMadre);
                    sScript.Append(" Y ");
                    sScript.Append(sScriptApoderado);
                    //sScript.Append(CrearTextoExtraInterprete(ExisteInterprete, bExisteTestigoRuego, strIdiomaPadre, sScriptInterprete.ToString(), true));
                    sScript.Append(" QUIEN PROCEDE EN REPRESENTACIÓN DE ");
                    sScript.Append(sScriptPadre);

                    sScript.Append(", DEBIDAMENTE " + sStringApoderadoGenero + " SEGÚN EL PODER INSCRITO EN LA PARTIDA ");
                    sScript.Append(strNroEscrituraPublica);
                    sScript.Append(" DEL REGISTRO DE MANDATOS Y PODERES DE LOS REGISTROS PÚBLICOS DE N° ");
                    sScript.Append(strNroPartidaRegistral);
                }
                else if (strSubTipoActoViajeMejor == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - PADRE REPRESENTA A MADRE")
                {
                    if (bIdiomaPadre)
                    {
                        sScript.Append("; COMPARECE:");
                        sScript.Append(" " + sScriptPadre);
                        
                        sScript.Append(CrearTextoExtraInterprete(ExisteInterprete, bExisteTestigoRuego, strIdiomaPadre, sScriptInterprete.ToString(), true));
                        sScript.Append(" QUIEN PROCEDE POR DERECHO PROPIO Y EN REPRESENTACIÓN DE ");
                        sScript.Append(sScriptMadre);
                        //sScript.Append(".");
                    }
                    else
                    {
                        sScript.Append("; COMPARECE:");
                        sScript.Append(" " + sScriptPadre);
                    
                        sScript.Append(" QUIEN PROCEDE POR DERECHO PROPIO Y EN REPRESENTACIÓN DE ");
                        sScript.Append(sScriptMadre);
                    }

                    sScript.Append(", DEBIDAMENTE " + "FACULTADO" + " SEGÚN EL PODER INSCRITO EN LA PARTIDA ");
                    sScript.Append(strNroEscrituraPublica);
                    sScript.Append(" DEL REGISTRO DE MANDATOS Y PODERES DE LOS REGISTROS PÚBLICOS DE N° ");
                    sScript.Append(strNroPartidaRegistral);
                }
                else if (strSubTipoActoViajeMejor == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - MADRE REPRESENTA A PADRE")
                {
                    if (bIdiomaMadre)
                    {
                        sScript.Append("; COMPARECE:");
                        sScript.Append(" " + sScriptMadre);
                        
                        sScript.Append(CrearTextoExtraInterprete(ExisteInterprete, bExisteTestigoRuego, strIdiomaMadre, sScriptInterprete.ToString(), true));


                        sScript.Append(" QUIEN PROCEDE POR DERECHO PROPIO Y EN REPRESENTACIÓN DE ");
                        sScript.Append(sScriptPadre);
                    }
                    else
                    {
                        sScript.Append("; COMPARECE:");
                        sScript.Append(" " + sScriptMadre);

                        sScript.Append(" QUIEN PROCEDE POR DERECHO PROPIO Y EN REPRESENTACIÓN DE ");
                        sScript.Append(sScriptPadre);
                    }

                    sScript.Append(", DEBIDAMENTE " + "FACULTADA" + " SEGÚN EL PODER INSCRITO EN LA PARTIDA ");
                    sScript.Append(strNroEscrituraPublica);
                    sScript.Append(" DEL REGISTRO DE MANDATOS Y PODERES DE LOS REGISTROS PÚBLICOS DE N° ");
                    sScript.Append(strNroPartidaRegistral);
                }
                else {
                    if (bIdiomaPadre)
                    {
                        sScript.Append("; COMPARECEN:");
                        sScript.Append(" " + sScriptMadre);
                        if (bIdiomaMadre)
                        {
                            sScript.Append(CrearTextoExtraInterprete(ExisteInterprete, bExisteTestigoRuego, strIdiomaMadre, sScriptInterprete.ToString(), false));
                        }

                        sScript.Append(" Y ");
                        sScript.Append(sScriptPadre);
                        if (bIdiomaPadre)
                        {
                            if (bIdiomaMadre)
                            { sScript.Append(CrearTextoExtraInterprete(ExisteInterprete, bExisteTestigoRuego, strIdioma, sScriptInterprete.ToString(), true, true)); }
                            else { sScript.Append(CrearTextoExtraInterprete(ExisteInterprete, bExisteTestigoRuego, strIdioma, sScriptInterprete.ToString(), true)); }
                        }
                        //sScript.Append(".");
                    }
                    else if (bIdiomaMadre)
                    {
                        sScript.Append("; COMPARECEN:");
                        sScript.Append(" " + sScriptPadre);
                        if (bIdiomaPadre)
                        {
                            sScript.Append(CrearTextoExtraInterprete(ExisteInterprete, bExisteTestigoRuego, strIdiomaPadre, sScriptInterprete.ToString(), false));
                        }

                        sScript.Append(" Y ");
                        sScript.Append(sScriptMadre);
                        if (bIdiomaMadre)
                        {
                            if (bIdiomaPadre)
                            { sScript.Append(CrearTextoExtraInterprete(ExisteInterprete, bExisteTestigoRuego, strIdioma, sScriptInterprete.ToString(), true, true)); }
                            else { sScript.Append(CrearTextoExtraInterprete(ExisteInterprete, bExisteTestigoRuego, strIdioma, sScriptInterprete.ToString(), true)); }
                        }
                        //sScript.Append(".");
                    }
                    else
                    {
                        sScript.Append("; COMPARECEN:");
                        sScript.Append(" " + sScriptMadre);
                        if (bIdiomaMadre)
                        {
                            sScript.Append(CrearTextoExtraInterprete(ExisteInterprete, bExisteTestigoRuego, strIdiomaMadre, sScriptInterprete.ToString(), false));
                        }

                        sScript.Append(" Y ");
                        sScript.Append(sScriptPadre);
                        if (bIdiomaPadre)
                        {
                            if (bIdiomaMadre)
                            { sScript.Append(CrearTextoExtraInterprete(ExisteInterprete, bExisteTestigoRuego, strIdioma, sScriptInterprete.ToString(), true, true)); }
                            else { sScript.Append(CrearTextoExtraInterprete(ExisteInterprete, bExisteTestigoRuego, strIdioma, sScriptInterprete.ToString(), true)); }
                        }
                        //sScript.Append(".");
                    }
                }

                //cambio 28/02/2019
                if (strSubTipoActoViajeMejor == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - PADRE REPRESENTA A MADRE")
                {
                    strArticuloEL_LA = "EL";
                    strSingularPluralCOMPARECIENTE = "COMPARECIENTE";
                    StrSingularPluralQuien = "QUIEN";
                    StrSingularPluralDeclara = "DECLARA";
                    StrSingularPluralAsume = "ASUME";
                    strSingularPluralRatifican = "RATIFICA";
                    strSingularPluralFirma = "FIRMA";
                    strSingularPluralsON = "ES";
                    strSingularPluralHabilInter = "HÁBIL";
                    strSingularPluralAutorizan = "AUTORIZA";
                }
                else if (strSubTipoActoViajeMejor == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - MADRE REPRESENTA A PADRE")
                {
                    strArticuloEL_LA = "LA";
                    strSingularPluralCOMPARECIENTE = "COMPARECIENTE";
                    StrSingularPluralQuien = "QUIEN";
                    StrSingularPluralDeclara = "DECLARA";
                    StrSingularPluralAsume = "ASUME";
                    strSingularPluralRatifican = "RATIFICA";
                    strSingularPluralFirma = "FIRMA";
                    strSingularPluralsON = "ES";
                    strSingularPluralHabilInter = "HÁBIL";
                    strSingularPluralAutorizan = "AUTORIZA";
                }
                else if (strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE Y MADRE")
                {
                    if (strGeneroIdentificadoApoderado == "IDENTIFICADA")
                    {
                        strArticuloEL_LA = "LA";
                        strSingularPluralCOMPARECIENTE = "COMPARECIENTE";
                        StrSingularPluralQuien = "QUIEN";
                        StrSingularPluralDeclara = "DECLARA";
                        StrSingularPluralAsume = "ASUME";
                        strSingularPluralRatifican = "RATIFICA";
                        strSingularPluralFirma = "FIRMA";
                        strSingularPluralsON = "ES";
                        strSingularPluralHabilInter = "HÁBIL";
                        strSingularPluralAutorizan = "AUTORIZA";
                    }
                    else
                    {
                        strArticuloEL_LA = "EL";
                        strSingularPluralCOMPARECIENTE = "COMPARECIENTE";
                        StrSingularPluralQuien = "QUIEN";
                        StrSingularPluralDeclara = "DECLARA";
                        StrSingularPluralAsume = "ASUME";
                        strSingularPluralRatifican = "RATIFICA";
                        strSingularPluralFirma = "FIRMA";
                        strSingularPluralsON = "ES";
                        strSingularPluralHabilInter = "HÁBIL";
                        strSingularPluralAutorizan = "AUTORIZA";
                    }
                }
                else
                {
                    strSingularPluralCOMPARECIENTE = "COMPARECIENTES";
                    StrSingularPluralQuien = "QUIENES";
                    StrSingularPluralDeclara = "DECLARAN";
                    StrSingularPluralAsume = "ASUMEN";
                    strSingularPluralRatifican = "RATIFICAN";
                    strSingularPluralFirma = "FIRMAN";
                    strSingularPluralsON = "SON";
                    strSingularPluralHabilInter = "HÁBILES";
                    strSingularPluralAutorizan = "AUTORIZAN";
                }
                
            }
            // 


            /* INTERPRETE */
            #region ArticuloInterprete

            if (sContarMujeresInterprete == 1 && sContarHombresInterprete == 0)
            {
                strGenerpLA_ELInterprete = "LA";
            }
            else if (sContarMujeresInterprete >= 1 && sContarHombresInterprete == 0)
            {
                strGenerpLA_ELInterprete = "LAS";
            }

            else if (sContarMujeresInterprete == 0 && sContarHombresInterprete == 1)
            {
                strGenerpLA_ELInterprete = "EL";
            }
            else if (sContarMujeresInterprete == 0 && sContarHombresInterprete >= 1)
            {
                strGenerpLA_ELInterprete = "LOS";
            }

            else if (sContarMujeresInterprete >= 1 && sContarHombresInterprete >= 1)
            {
                strGenerpLA_ELInterprete = "LOS";
            }
            else
            {
                strGenerpLA_ELInterprete = "EL";
            }
            #endregion

            String strPluralSingularInterpreteHA = String.Empty;
            if (sContarInterprete <= 1)
            {
                strPluralSingularInterprete = "INTÉRPRETE";
                strPluralSinguralQuienInterprete = "QUIEN";
                strPluralSinguralDeclaraInterprete = "DECLARA";
                strPluralSingularInterpreteHA = "HA";
            }
            else
            {
                strPluralSingularInterprete = "INTÉRPRETES";
                strPluralSinguralQuienInterprete = "QUIENES";
                strPluralSinguralDeclaraInterprete = "DECLARAN";
                strPluralSingularInterpreteHA = "HAN";
            }

            //JONATAN SILVA CACHAY
            //if (ExisteInterprete)
            //{
            //    if (bExisteTestigoRuego)
            //    {
            //        sScript.Append(" QUIEN ES HÁBIL");
            //    }
            //    else
            //    {
            //        sScript.Append(", QUIEN ES HÁBIL");
            //    }
            //    sScript.Append(" EN EL IDIOMA " + strIdioma);
            //    sScript.Append(" Y NO CONOCE EL IDIOMA CASTELLANO, POR LO QUE DESIGNA A ");
            //    sScript.Append(sScriptInterprete);

            //    sScript.Append("QUIEN PROCEDE EN CALIDAD DE INTÉRPRETE DE CONFORMIDAD CON LO ESTABLECIDO EN EL ÁRTICULO 30° DEL DECRETO LEGISLATIVO N° 1049,");
            //    sScript.Append(" MANIFIESTA SER HÁBIL EN EL IDIOMA " + strIdioma + " Y EL IDIOMA CASTELLANO Y TENER EL CONOCIMIENTO Y EXPERIENCIA SUFICIENTE PARA EFECTUAR LA TRADUCCIÓN QUE LE HA SIDO SOLICITADA.");
            //}

            if (ExisteInterprete == true || bExisteTestigoRuego == true)
            {
                if (intContarHijos == 1)
                {
                    sScript.Append(" " + strArticuloEL_LA + " " + strSingularPluralCOMPARECIENTE + " " + strSingularPluralAutorizan + " EL VIAJE A SU MENOR");
                }
                else
                {
                    sScript.Append(" " + strArticuloEL_LA + " " + strSingularPluralCOMPARECIENTE + " " + strSingularPluralAutorizan + " EL VIAJE A SUS MENORES");
                }
            }
            else
            {
                if (bExisteApoderado)
                {
                    if (strSubTipoActoViajeMejor != "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A MADRE" &&
                        strSubTipoActoViajeMejor != "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE" &&
                        strSubTipoActoViajeMejor != "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE Y MADRE")
                    {
                        sScript.Append(sScriptApoderado);
                    }
                }

                if (intContarHijos == 1)
                {
                    sScript.Append(" A FIN DE AUTORIZAR EL VIAJE DE SU MENOR");
                }
                else
                {
                    sScript.Append(" A FIN DE AUTORIZAR EL VIAJE DE SUS MENORES");
                }
            }
            //----------------------------------------------------------
            if (!bExisteCompania)
            {
                if (intContarHijos == 1)
                {
                    if (ExisteHijoVarones)
                    {
                        strHijoSoloSola = "QUIEN VIAJA SOLO";
                    }
                    else
                    {
                        strHijoSoloSola = "QUIEN VIAJA SOLA";
                    }
                }
                else
                {
                    if (ExisteHijoVarones)
                    {
                        strHijoSoloSola = "QUIENES VIAJAN SOLOS";
                    }
                    else
                    {
                        strHijoSoloSola = "QUIENES VIAJAN SOLAS";
                    }
                }
            }
            //----------------------------------------------------------
            sScript.Append(" " + sScriptMenor);
            if (sScriptAcompanate.Length > 0)
            { sScript.Append(" " + sScriptAcompanate); }

            if (!bExisteCompania)
            {
                sScript.Append(" " + strHijoSoloSola + ",");
            }

            sScript.Append(" DE ACUERDO AL SIGUIENTE ITINERARIO ");
            sScript.Append(dt.Rows[0]["RutaTitular"].ToString().ToUpper());
            sScript.Append(";");
            sScript.Append(" DE CONFORMIDAD CON LO SEÑALADO EN EL ARTÍCULO 111 DEL CÓDIGO");
            sScript.Append(" DE LOS NIÑOS Y ADOLESCENTES - LEY 27337. LA PRESENTE ACTA DE AUTORIZACIÓN DE VIAJE FUE LEÍDA POR ");
            
            sScript.Append(strArticuloEL_LA + " " + strSingularPluralCOMPARECIENTE);
            //-----------------------------------------------
            //Autor: Miguel Márquez Beltrán
            //Fecha: 31/08/2017
            //Motivo: Cambio del formato de autorización de viaje de menor al exterior
            //-----------------------------------------------
            if (strSubTipoActoExtraProtocolar.Equals("PROGENITOR SUPERVIVIENTE") || strSubTipoActoExtraProtocolar.Equals("ÚNICO PROGENITOR QUE REALIZO EL RECONOCIMIENTO"))
            {
                if (strSubTipoActoExtraProtocolar.Equals("PROGENITOR SUPERVIVIENTE"))
                {
                    sScript.Append(", EN SU CALIDAD DE " + strSubTipoActoExtraProtocolar.Trim());
                }
                if (strSubTipoActoExtraProtocolar.Equals("ÚNICO PROGENITOR QUE REALIZO EL RECONOCIMIENTO"))
                {
                    sScript.Append(", EN CALIDAD DE " + strSubTipoActoExtraProtocolar.Trim());
                }
            }
            //-----------------------------------------------
            sScript.Append(", " + StrSingularPluralQuien);
            sScript.Append(" " + StrSingularPluralDeclara);
            sScript.Append(" QUE " + StrSingularPluralAsume);

            if (bExisteTestigoRuego)
            {
                if ((bExistePadre && bHuellaPadre) || (bExisteMadre && bHuellaMadre))
                {
                    sScript.Append(" TODAS LAS RESPONSABILIDADES QUE DE ELLA EMANEN Y SE " + strSingularPluralRatifican + " EN SU CONTENIDO");
                    sScript.Append(" E IMPRIMEN SU HUELLA DACTILAR EN LA FECHA.");
                }
                else
                {
                    sScript.Append(" TODAS LAS RESPONSABILIDADES QUE DE ELLA EMANEN Y SE " + strSingularPluralRatifican + " EN SU CONTENIDO.");
                }
            }
            else
            {
                sScript.Append(" TODAS LAS RESPONSABILIDADES QUE DE ELLA EMANEN Y SE " + strSingularPluralRatifican + " EN SU CONTENIDO Y LO " + strSingularPluralFirma);
                if (strArticuloEL_LA == "LA" || strArticuloEL_LA == "EL")
                {
                    sScript.Append(" E IMPRIME SU HUELLA DACTILAR EN LA FECHA."); 
                }
                else {
                    sScript.Append(" E IMPRIMEN SU HUELLA DACTILAR EN LA FECHA."); 
                }
                
            }


            if (ExisteInterprete)
            {
                sScript.Append(" POR SU PARTE " + strGenerpLA_ELInterprete + " " + strPluralSingularInterprete + " " + strPluralSinguralDeclaraInterprete + " BAJO SU RESPONSABILIDAD QUE LA TRADUCCIÓN QUE " + strPluralSingularInterpreteHA + " REALIZADO ES CONFORME Y EXACTA.");
            }
            sScript.Append("</p>");
            sScript.Append("<tab></tab>");


            if (strObservacion.Trim() != String.Empty)
            {
                sScript.Append("<p align=\"justify\"; style=\"background-color:transparent;\">");
                sScript.Append("OBSERVACIONES: ");
                sScript.Append(strObservacion.ToUpper());
                sScript.Append("</p>");
            }

            return sScript.ToString();
        }

        private string ObtenerDocumentoConformidad(string vNombre, string vDocumento)
        {

            bool bEsMujer = false;

            

            if (ViewState["PER_GENERO"].ToString() == Convert.ToInt32(Enumerador.enmGenero.FEMENINO).ToString())
            {
                bEsMujer = true;
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
            sScript.Append("declaro que he leído y revisado en su detalle el documento de ");

            if (ddlTipoActoNotarialExtra.SelectedValue == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR).ToString())
            {
                //sScript.Append("Autorización de Viaje de Menor al Exterior");
                sScript.Append("Autorización de Viaje de Menor");
                
            }
            else if (ddlTipoActoNotarialExtra.SelectedValue == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.CERTIFICADO_SUPERVIVENCIA).ToString())
            {
                sScript.Append("Certificado de Supervivencia");

            }
            else if (ddlTipoActoNotarialExtra.SelectedValue == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.PODER_FUERA_REGISTRO).ToString())
            {
                sScript.Append("Poder Fuera de Registro");
            }

            sScript.Append(", que he tenido a la vista y me ha sido entregado en la fecha,");
            sScript.Append(" manifestando mi conformidad con su contenido.");
            sScript.Append("</p>");
            sScript.Append("<br />");


            sScript.Append("<p align=\"right\"; style=\"background-color:transparent; font-family:arial;\">");
            DateTime dt_Fecha = Comun.FormatearFecha(Accesorios.Comun.ObtenerFechaActualTexto(HttpContext.Current.Session));
            string str_Fecha = dt_Fecha.ToString("dd") + " de " + AplicarInicialMayuscula(dt_Fecha.ToString("MMMM")) + " de " + dt_Fecha.ToString("yyyy");


            if (Session["CiudadOficinaConsular"] != null)
            {

                str_Fecha = AplicarInicialMayuscula(Session["CiudadOficinaConsular"].ToString()).ToUpper() + ", " + str_Fecha;
            }

            sScript.Append(str_Fecha);
            sScript.Append("</p>");
            sScript.Append("<br />");
            sScript.Append("<tab></tab>");


            return sScript.ToString();
        }

        #endregion

        private void ImprimirCuerpo(string cuerpo)
        {

            #region Firmas

            DataTable dt = new DataTable();
            ActoNotarialConsultaBL BL = new ActoNotarialConsultaBL();

            dt = (DataTable)BL.ReportePoderFueraRegistro(Convert.ToInt32(ViewState[Constantes.CONST_SESION_ACTONOTARIAL_ID]),
                Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));

            bool bExisteTestigoRuego = false;
            bool bExisteInterprete = false;


            foreach (DataRow Row in dt.Rows)
            {

                if ((int)Enumerador.enmNotarialTipoParticipante.OTORGANTE == Convert.ToUInt32(Row["sTipoParticipanteId"]))
                {
                    if (Row["pers_vDescripcionIncapacidad"].ToString().Trim() != String.Empty)
                    {
                        bExisteTestigoRuego = true;

                    }
                }
                if ((int)Enumerador.enmNotarialTipoParticipante.INTERPRETE == Convert.ToUInt32(Row["sTipoParticipanteId"]))
                {
                    bExisteInterprete = true;
                }
            }




            List<DocumentoFirma> listObjects = new List<DocumentoFirma>();
            DocumentoFirma objetos = new DocumentoFirma();

            List<CBE_PARTICIPANTE> loPARTICIPANTES = (List<CBE_PARTICIPANTE>)Session["ParticipanteContainer"];

            foreach (CBE_PARTICIPANTE participante in loPARTICIPANTES)
            {
                objetos = new DocumentoFirma();
                if ((participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.OTORGANTE)) ||
                    (participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.INTERPRETE) && bExisteInterprete) ||
                    (participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO) && bExisteTestigoRuego))
                {
                    objetos.vNombreCompleto = participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno;

                    if (Convert.ToInt32(participante.Identificacion.peid_sDocumentoTipoId) == (int)Enumerador.enmTipoDocumento.OTROS)
                    {
                        objetos.vNroDocumentoCompleto = participante.Identificacion.peid_vTipodocumento + " " + participante.Identificacion.peid_vDocumentoNumero;
                    }
                    else
                    {
                        objetos.vNroDocumentoCompleto = ObtenerTextoTipoDocumento(participante.Identificacion.peid_sDocumentoTipoId.ToString()) + " " + participante.Identificacion.peid_vDocumentoNumero;
                    }

                    objetos.bIncapacitado = participante.Persona.pers_bIncapacidadFlag;
                    objetos.bImprimirFirma = participante.anpa_bFlagFirma;

                    objetos.bAplicaHuellaDigital = participante.anpa_bFlagHuella;

                    listObjects.Add(objetos);
                }
            }
            objetos = new DocumentoFirma();



            #endregion

            List<string> listPalabrasClaves = new List<string>();
            listPalabrasClaves.Add("PODER FUERA DE REGISTRO");
            listPalabrasClaves.Add("CONCLUSIÓN:");

            Comun.CrearDocumentoiTextSharpTamanoLetra(null, cuerpo, Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE].ToString(), Server.MapPath("~/Images/Escudo.JPG"), listObjects, true, listPalabrasClaves, "Poder_Fuera_Registro", true, Convert.ToDouble(HttpContext.Current.Session["TamanoLetra"]));

        }

        DataTable CrearTmpTabla()
        {
            DataTable dtTablaTemporal = new DataTable();

            dtTablaTemporal.Columns.Add("strCadenaBuscar", typeof(string));
            dtTablaTemporal.Columns.Add("strCadenaReemplazar", typeof(string));

            return dtTablaTemporal;
        }

        void btnCancelarCuerpo_Click(object sender, EventArgs e)
        {

        }

        void GrabarCuerpo(int iActoNotarialId)
        {
            if (iActoNotarialId == 0)
            {
                EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "CUERPO", "No se ha ingresado información al Cuerpo."));
                return;
            }
            else
            {
                CBE_CUERPO lCuerpo = new CBE_CUERPO();

                #region Creando objeto ....
                lCuerpo.ancu_iActoNotarialCuerpoId = Convert.ToInt32(hdn_ancu_iActoNotarialCuerpoId.Value);
                lCuerpo.ancu_iActoNotarialId = iActoNotarialId;
                Session["vDocumentoActualPFR"] = Session["vDocumentoActualPFRPrevio"].ToString();
                lCuerpo.ancu_vCuerpo = Session["vDocumentoActualPFR"].ToString();
                lCuerpo.ancu_vFirmaIlegible = "";
                lCuerpo.ancu_sUsuarioCreacion = Convert.ToInt16(hdn_acno_sUsuarioCreacion.Value);
                lCuerpo.ancu_vIPCreacion = hdn_acno_vIPCreacion.Value;
                lCuerpo.ancu_sUsuarioModificacion = Convert.ToInt16(hdn_acno_sUsuarioModificacion.Value);
                lCuerpo.ancu_vIPModificacion = hdn_acno_vIPModificacion.Value;
                #endregion

                ActoNotarialMantenimiento mnt = new ActoNotarialMantenimiento();
                mnt.Insertar_ActoNotarialCuerpoExtraProtocolar(lCuerpo, null);

                if (lCuerpo.ancu_iActoNotarialCuerpoId != 0 && lCuerpo.Message == null)
                {
                    hdn_ancu_iActoNotarialCuerpoId.Value = lCuerpo.ancu_iActoNotarialCuerpoId.ToString();
                    EjecutarScript("EnableTabIndex(iTabAprobacionPagoIndice);" + Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CUERPO", "Registro almacenado correctamente."));
                    Session["anep_iSelectedTabId"] = iTabAprobacionPagoIndice;

                    hdn_bCheckAceptacion.Value = "1";
                    hdn_bCuerpoGrabado.Value = "1";
                    ddlFuncionario.Enabled = false;
                    ctrlToolTipoActo.btnGrabar.Enabled = false;
                    ddlTipoActoNotarialExtra_SelectedIndexChanged(null, null);
                    updTipoActo.Update();
                }
                else
                {
                    if (lCuerpo.ancu_iActoNotarialCuerpoId == 0 && lCuerpo.Message != null)
                    {
                        EjecutarScript("EnableTabIndex(iTabAprobacionPagoIndice);" + Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "CUERPO", lCuerpo.Message));
                        Session["anep_iSelectedTabId"] = iTabAprobacionPagoIndice;

                        hdn_bCheckAceptacion.Value = "1";
                        hdn_bCuerpoGrabado.Value = "1";
                    }
                }

                HabilitarBotonesImpresion(true);
            }
        }

        void btnGrabarTipoActo_Click(object sender, EventArgs e)
        {
            bool bModificaTipoActo = false;

            if (hTipActo.Value != "0")
            {
                if (ddlTipoActoNotarialExtra.SelectedValue.ToString() != hTipActo.Value)
                {
                    bModificaTipoActo = true;
                }
            }

            hTipActo.Value = ddlTipoActoNotarialExtra.SelectedValue.ToString();
            GrabarTipoActo(bModificaTipoActo);
            txtCantidad.Text = "1";
            txtCantidad.Enabled = false;
            ctrlToolTipoActo.VisibleIButtonGrabar = false;
        }

        void btnGrabar5_Click(object sender, EventArgs e)
        {
            Btn_RegistrarPago_Click(null, null);
        }

        void btnGrabar3_Click(object sender, EventArgs e)
        {
            if (ValidarParticipantesGrabar())
            {
                //--------------------------------------------------
                //Fecha: 02/10/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Limpiar Participante
                //--------------------------------------------------
                LimpiarTabParticipante1();
                //-------------------------------------                
                MostrarCamposPorTipoParticipante();
                btnSave_tab4_Click(null, null);

                //---------------------------------------------------------------
                //Fecha: 16/10/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Deshabilitar los controles de tipo,  
                //  subtipo de acto notarial y funcionario después de grabar.
                //---------------------------------------------------------------
                ddlTipoActoNotarialExtra.Enabled = false;
                ddlSubTipoNotarialExtra.Enabled = false;
                ddlCondiciones.Visible = false;
                lblCondiciones.Visible = false;
                ctrlToolTipoActo.btnGrabar.Enabled = false;

                //---------------------------------------------------------------
                //Fecha: 17/12/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Deshabilitar Grabar, aprobar y el check de leído y 
                //        conforme cuando se graban los participantes.
                //---------------------------------------------------------------
                btnGrabarCuerpoTemporal.Attributes.Add("disabled", "disabled");
                btnGrabarCuerpo.Attributes.Add("disabled", "disabled");  
                string strScript = string.Empty;
                strScript = @"$(function(){{
                                            DeshabilitarConforme();
                                        }});";
                strScript = string.Format(strScript);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "DeshabilitarCheckConforme", strScript, true);
                //---------------------------------------------------------------

                if (Convert.ToInt32(ddlTipoActoNotarialExtra.SelectedValue) == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR))
                {
                    ddlCondiciones.Visible = true;
                    lblCondiciones.Visible = true;
                }

                updTipoActo.Update();
                //---------------------------------------------------------------
            }
            else
            {
                //EjecutarScript("HabilitarPorParticipante(false);");
            }
        }

        void btnCancelar3_Click(object sender, EventArgs e)
        {
            LimpiarTabParticipante1();

            hdn_Tipo_Participante_Editando.Value = "-1";
            Btn_AgregarParticipante.Text = "Agregar";
            //UpdParticipante.Update();
        }

        private void CargarActoNotarial()
        {
            if (Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value) != 0)
            {

                RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();
                ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();
                lACTONOTARIAL.acno_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
                lACTONOTARIAL.acno_iActuacionId = Convert.ToInt64(this.hdn_acno_iActuacionId.Value);
                lACTONOTARIAL.acno_sTipoActoNotarialId = Convert.ToInt16(Enumerador.enmNotarialTipoActo.EXTRAPROTOCOLAR);
                lACTONOTARIAL = lActoNotarialConsultaBL.obtener(lACTONOTARIAL);
                string vScript = string.Empty;
                bool bConsulta = false;

                if (Session["iFlujoExtraProtocolar"] != null)
                {
                    if (Session["iFlujoExtraProtocolar"].ToString() == Convert.ToInt32(Enumerador.enmFlujoProtocolar.CONSULTA).ToString())
                    {
                        bConsulta = true;

                    }
                    Session.Remove("iFlujoExtraProtocolar");
                }

                if (Int16.Parse(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString()) != lACTONOTARIAL.acno_sOficinaConsularId || bConsulta == true)
                {
                    HabilitarTabParticipanteTotal(false);
                    HabilitarTabCuerpo(false);
                    Session["iOficinaConsularDiferente"] = "1";
                }

                #region REGISTRO

                ddlTipoActoNotarialExtra.SelectedValue = Convert.ToString(lACTONOTARIAL.acno_sSubTipoActoNotarialId);
                ddlFuncionario.SelectedValue = Convert.ToString(lACTONOTARIAL.acno_IFuncionarioAutorizadorId);

                if (ddlTipoActoNotarialExtra.SelectedItem.Text == "AUTORIZACIÓN DE VIAJE DE MENOR")
                {
                    ddlSubTipoNotarialExtra.SelectedValue = Convert.ToString(lACTONOTARIAL.acno_sSubTipoActoNotarialExtraProtocolarId);
                    lbl_subTipoActoNotarialExtra.Visible = true;
                    ddlCondiciones.SelectedValue = Convert.ToString(lACTONOTARIAL.acno_sCondicionTipoActoNotarialId);
                    ddlSubTipoNotarialExtra.Visible = true;
                    ddlCondiciones.Visible = true;
                    lblCondiciones.Visible = true;
                    if (ddlCondiciones.SelectedItem.Text == "MENOR VIAJA SIN ACOMPAÑANTE")
                    {
                        observación.Visible = true;
                    }
                    else {
                        observación.Visible = false;
                    }


                    if (ddlSubTipoNotarialExtra.SelectedItem.Text == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - PADRE REPRESENTA A MADRE" ||
                        ddlSubTipoNotarialExtra.SelectedItem.Text == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - MADRE REPRESENTA A PADRE" )
                    {
                        OtrosDatos.Visible = true;
                    }
                    else if (ddlSubTipoNotarialExtra.SelectedItem.Text == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE" ||
                        ddlSubTipoNotarialExtra.SelectedItem.Text == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A MADRE" ||
                        ddlSubTipoNotarialExtra.SelectedItem.Text == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE Y MADRE")
                    {
                        //txtEscrituraPublica.Enabled = false;
                        //txtPartidaSunarp.Enabled = false;
                        OtrosDatos.Visible = false;
                    }
                    else 
                    {
                        OtrosDatos.Visible = false;
                    }

                }
                else
                {
                    ddlSubTipoNotarialExtra.SelectedIndex = 0;
                    lbl_subTipoActoNotarialExtra.Visible = false;
                    ddlSubTipoNotarialExtra.Visible = false;
                    ddlCondiciones.Visible = false;
                    lblCondiciones.Visible = false;
                }

                CargarParticipantesTipoActo((Enumerador.enmExtraprotocolarTipo)Convert.ToInt32(ddlTipoActoNotarialExtra.SelectedValue.ToString()));
                SetearTarifario();

                if (Convert.ToInt32(Session["PasoActualTab"]) < iTabParticipanteIndice)
                {
                    Session["PasoActualTab"] = iTabParticipanteIndice.ToString();
                }

                Session["anep_iSelectedTabId"] = iTabParticipanteIndice;
                vScript = "EnableTabIndex(" + iTabParticipanteIndice.ToString() + "); SetTabs('" + iTabParticipanteIndice.ToString() + "','" + ddlTipoActoNotarialExtra.SelectedValue.ToString() + "');";



                #endregion

                #region PARTICIPANTES


                ParticipanteConsultaBL _obj3 = new ParticipanteConsultaBL();
                DataTable _dt = new DataTable();
                _dt = _obj3.ObtenerParticipantesExtraprotocolar(Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value));

                #region Llena Campos Para Validar
                if (_dt.Rows.Count > 0)
                {
                    hExisteInterprete.Value = _dt.Rows[0]["ExisteInterprete"].ToString();
                    hExisteIdiomaExtranjero.Value = _dt.Rows[0]["ExisteIdiomaExtranjero"].ToString();

                    hExistePadre.Value = "0";
                    hExisteMadre.Value = "0";
                    hExisteAcompañante.Value = "0";


                    foreach (DataRow row in _dt.Rows)
                    {
                        if (Convert.ToInt32(row["anpa_sTipoParticipanteId"]) == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.PADRE))
                        {
                            hExistePadre.Value = "1";
                        }
                        if (Convert.ToInt32(row["anpa_sTipoParticipanteId"]) == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.MADRE))
                        {
                            hExisteMadre.Value = "1";
                        }
                        if (Convert.ToInt32(row["anpa_sTipoParticipanteId"]) == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.ACOMPANANTE))
                        {
                            hExisteAcompañante.Value = "1";
                        }
                    }
                }
                else
                {
                    hExisteInterprete.Value = "0";
                    hExisteIdiomaExtranjero.Value = "0";

                    hExistePadre.Value = "0";
                    hExisteMadre.Value = "0";
                    hExisteAcompañante.Value = "0";
                }
                #endregion
                grd_Participantes.DataSource = _dt;
                grd_Participantes.DataBind();

                if (_dt.Rows.Count == 0)
                {
                    //-----------------------------------------------------------------------------------------
                    // Autor: Miguel Márquez Beltrán
                    // Objetivo: Ejecutar el metodo de asignar los datos del participante  
                    // automaticamente para el tipo de acto notarial: Certificado de Supervivencia
                    // solo cuando es una nueva acto extraprotocolar del tipo Certificado de Supervivencia
                    //-----------------------------------------------------------------------------------------
                    if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.CERTIFICADO_SUPERVIVENCIA).ToString() ||
                        ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.PODER_FUERA_REGISTRO).ToString())
                    {
                        AsignarParticipanteRecurrente();
                    }
                    //-----------------------------------------------------------------------------------------
                    //EjecutarScript(vScript + " HabilitarPorParticipante(false);");
                    EjecutarScript(vScript);
                    return;
                }
                else
                {
                    ddlTipoActoNotarialExtra.Enabled = false;
                    ddlSubTipoNotarialExtra.Enabled = false;
                    ddlCondiciones.Enabled = false;
                    lblCondiciones.Enabled = false;

                    if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.CERTIFICADO_SUPERVIVENCIA).ToString())
                    {
                        chkImprimirFirmaTitular1.Attributes.Add("style", "display:block;");
                        chkImprimirFirmaTitular1.Checked = Convert.ToBoolean(_dt.Rows[0]["anpa_bFlagFirma"].ToString());
                    }
                    else
                    {
                        chkImprimirFirmaTitular1.Attributes.Add("style", "display:none;");
                    }

                    // MDIAZ
                    //ddlViajeMenorCertificador.SelectedValue = lACTONOTARIAL.acno_IFuncionarioCertificadorId.ToString();
                    //txtObservacionesCertificador.Text = lACTONOTARIAL.acno_vObservaciones;
                    txtObservacion.Text = lACTONOTARIAL.acno_vObservaciones;
                    txtEscrituraPublica.Text = lACTONOTARIAL.acno_vNumeroEscrituraPublica;
                    txtPartidaSunarp.Text = lACTONOTARIAL.acno_vPartidaRegistral;
                    if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR).ToString())
                    {
                        // Solo cuando le dan clic en guardar el ubigeo esta lleno
                        if (lACTONOTARIAL.acno_cUbigeoDestino != "")
                        {
                            ViewState["UbigeoViajeDestino"] = lACTONOTARIAL.acno_cUbigeoDestino;
                            ddl_UbigeoPaisViajeDestino.SelectedValue = lACTONOTARIAL.acno_cUbigeoDestino.Substring(0, 2);
                            ddl_UbigeoPaisViajeDestino_SelectedIndexChanged(null, null);

                            txtItinerario.Text = lACTONOTARIAL.acno_vItinerario;

                            if (Convert.ToInt32(Session["PasoActualTab"]) < iTabCuerpoIndice)
                            {
                                Session["PasoActualTab"] = iTabCuerpoIndice.ToString();
                            }

                            Session["anep_iSelectedTabId"] = iTabCuerpoIndice;
                            vScript = "EnableTabIndex(" + iTabCuerpoIndice.ToString() + "); SetTabs('" + iTabCuerpoIndice.ToString() + "','" + ddlTipoActoNotarialExtra.SelectedValue.ToString() + "');";

                            CargarComboIncapacitados();
                        }
                    }
                    else {

                        DataTable _dtVerifica = new DataTable();
                        ParticipanteConsultaBL _obj = new ParticipanteConsultaBL();
                        _dtVerifica = _obj.VerificarRegistroParticipantesExtraprotocolar(Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value), Convert.ToInt16(ddlTipoActoNotarialExtra.SelectedValue), Convert.ToInt64(this.hdn_actu_iPersonaRecurrenteId.Value), Convert.ToInt16(ddlSubTipoNotarialExtra.SelectedValue));

                        if (_dtVerifica.Rows[0]["Resultado"].ToString().Length <= 0)
                        {
                            if (Convert.ToInt32(Session["PasoActualTab"]) < iTabCuerpoIndice)
                            {
                                Session["PasoActualTab"] = iTabCuerpoIndice.ToString();
                            }

                            Session["anep_iSelectedTabId"] = iTabCuerpoIndice;
                            vScript = "EnableTabIndex(" + iTabCuerpoIndice.ToString() + "); SetTabs('" + iTabCuerpoIndice.ToString() + "','" + ddlTipoActoNotarialExtra.SelectedValue.ToString() + "');";

                            CargarComboIncapacitados();
                        }
                    }
                    
                }

                #endregion


                #region CUERPO

                SetearIntroduccionyConlusionCuerpo();


                RE_ACTONOTARIALCUERPO lACTONOTARIALCUERPO = new RE_ACTONOTARIALCUERPO();
                ActoNotarialCuerpoConsultaBL lActoNotarialCuerpoConsultaBL = new ActoNotarialCuerpoConsultaBL();
                lACTONOTARIALCUERPO.ancu_iActoNotarialId = lACTONOTARIAL.acno_iActoNotarialId;
                lACTONOTARIALCUERPO = lActoNotarialCuerpoConsultaBL.obtener(lACTONOTARIALCUERPO);

                if (lACTONOTARIALCUERPO.ancu_iActoNotarialCuerpoId != 0)
                {
                    ddlTamanoLetra.SelectedValue = lACTONOTARIAL.acno_sTamanoLetra.ToString();
                    Session["TamanoLetra"] = lACTONOTARIAL.acno_sTamanoLetra.ToString();
                    string[] introConclu = new string[] { };
                    introConclu = lACTONOTARIALCUERPO.ancu_vCuerpo.Split(new string[] { "<tab></tab>" }, StringSplitOptions.None);
                    if (introConclu.Length > 1)
                    {
                        this.txtCuerpo.Text = introConclu[1].ToString();
                        this.hCuerpo.Value = introConclu[1].ToString();
                    }
                    else
                    {
                        this.txtCuerpo.Text = introConclu[0].ToString();
                        this.hCuerpo.Value = introConclu[0].ToString();
                    }
                    if (introConclu.Count() == 4)
                    {
                        this.txtInfoAdicional.Text = introConclu[3].ToString();
                        this.hInfoAdicional.Value = introConclu[3].ToString();
                    }

                    Session["vDocumentoActualPFR"] = lACTONOTARIALCUERPO.ancu_vCuerpo.ToString();
                    Session["vDocumentoActualPFRPrevio"] = lACTONOTARIALCUERPO.ancu_vCuerpo.ToString();

                    if (lACTONOTARIAL.acno_sEstadoId == (int)Enumerador.enmNotarialProtocolarEstado.TRANSCRITA)
                    {
                        hdn_bCheckAceptacion.Value = "0";
                        hdn_bCuerpoGrabado.Value = "0";

                        EjecutarScript(vScript);
                        return;
                    }
                    else
                    {
                        hdn_bCheckAceptacion.Value = "1";
                        hdn_bCuerpoGrabado.Value = "1";
                        if (Convert.ToInt32(Session["PasoActualTab"]) < iTabAprobacionPagoIndice)
                        {
                            Session["PasoActualTab"] = iTabAprobacionPagoIndice.ToString();
                        }

                        Session["anep_iSelectedTabId"] = iTabAprobacionPagoIndice;
                        vScript = "EnableTabIndex(" + iTabAprobacionPagoIndice.ToString() + "); SetTabs('" + iTabAprobacionPagoIndice.ToString() + "','" + ddlTipoActoNotarialExtra.SelectedValue.ToString() + "');";

                        ddlFuncionario.Enabled = false;
                        ctrlToolTipoActo.btnGrabar.Enabled = false;
                    }
                }
                else
                {

                    hdn_bCheckAceptacion.Value = "0";
                    hdn_bCuerpoGrabado.Value = "0";

                    EjecutarScript(vScript);
                    return;
                }


                #endregion

                #region APROBACIÓN Y PAGO

                string vCodigoUnicoFabrica = string.Empty;
                string vTipoInsumoId = string.Empty;



                DataTable dtActuacionDetalle = new DataTable();
                DataTable dtActuacionInsumoDetalle = new DataTable();
                Int64 lngActuacionId = lACTONOTARIAL.acno_iActuacionId;
                dtActuacionDetalle = lActoNotarialConsultaBL.ListarActuacionDetalle(lngActuacionId);

                SGAC.Registro.Actuacion.BL.ActuacionMantenimientoBL objActuacionMantenimientoBL = new SGAC.Registro.Actuacion.BL.ActuacionMantenimientoBL();


                int IntTotalCount = 0;
                int IntTotalPages = 0;
                int intPaginaCantidad = Constantes.CONST_PAGE_SIZE_ADJUNTOS;
                int PaginaActual = 1;

                bool bDataTableContieneInformacion = false;

                if (dtActuacionDetalle != null)
                {
                    if (dtActuacionDetalle.Rows.Count > 0)
                    {
                        ViewState[Constantes.CONST_SESION_ACTUACIONDET_ID] = dtActuacionDetalle.Rows[0]["acde_iActuacionDetalleId"].ToString();
                        //Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value] = dtActuacionDetalle.Rows[0]["acde_iActuacionDetalleId"].ToString();

                        Int64 iActuacionDetalleId = Convert.ToInt64(dtActuacionDetalle.Rows[0]["acde_iActuacionDetalleId"].ToString());
                        bDataTableContieneInformacion = true;
                        dtActuacionInsumoDetalle = objActuacionMantenimientoBL.Obtener_ActuacionInsumoDetalle(iActuacionDetalleId, PaginaActual, intPaginaCantidad, ref IntTotalCount, ref  IntTotalPages);

                    }
                }


                if (bDataTableContieneInformacion)
                {
                    vCodigoUnicoFabrica = dtActuacionDetalle.Rows[0]["insu_vCodigoUnicoFabrica"].ToString().Trim();
                    vTipoInsumoId = dtActuacionDetalle.Rows[0]["insu_sInsumoTipoId"].ToString().Trim();

                    //Gdv_Tarifa.DataSource = dtActuacionDetalle;
                    //Gdv_Tarifa.DataBind();

                    LblDescMtoML.Text = dtActuacionDetalle.Rows[0]["vMoneda"].ToString().Trim();
                    LblDescTotML.Text = dtActuacionDetalle.Rows[0]["vMoneda"].ToString().Trim();

                    string strFormato = ConfigurationManager.AppSettings["FormatoMonto"].ToString();

                    Txt_MtoSC.Text = Convert.ToDouble(dtActuacionDetalle.Rows[0]["FSolesConsular"].ToString()).ToString(strFormato);
                    Txt_MontML.Text = Convert.ToDouble(dtActuacionDetalle.Rows[0]["FMonedaExtranjera"].ToString()).ToString(strFormato);
                    Txt_TotSC.Text = Convert.ToDouble(dtActuacionDetalle.Rows[0]["pago_FMontoSolesConsulares"].ToString()).ToString(strFormato);
                    Txt_TotML.Text = Convert.ToDouble(dtActuacionDetalle.Rows[0]["pago_FMontoMonedaLocal"].ToString()).ToString(strFormato);
                    
                    LblFecha.Text = dtActuacionDetalle.Rows[0]["acde_dFechaRegistro"].ToString();

                   // LblDescTotML2.Text = dtActuacionDetalle.Rows[0]["vMoneda"].ToString().Trim() + ":";

                    Session["dtActuacionesRowCount"] = dtActuacionDetalle.Rows.Count;

                    //Session[strVarActuacionDetalleDT] = dtActuacionDetalle;

                    // Datos Pago
                    //double dblTotalSolesConsulares = 0;
                    //double dblTotalMonedaLocal = 0;
                    //foreach (DataRow dr in dtActuacionDetalle.Rows)
                    //{
                    //    dblTotalMonedaLocal += Convert.ToDouble(dr["pago_FMontoMonedaLocal"]);
                    //    dblTotalSolesConsulares += Convert.ToDouble(dr["pago_FMontoSolesConsulares"]);

                    //}
                    //Lbl_TotalGeneral.Text = dblTotalSolesConsulares.ToString();
                    //Lbl_TotalExtranjera.Text = dblTotalMonedaLocal.ToString();

                    //ddlTipoPago.SelectedValue = dtActuacionDetalle.Rows[0]["pago_sPagoTipoId"].ToString();
                    HF_TIPO_PAGO.Value = dtActuacionDetalle.Rows[0]["pago_sPagoTipoId"].ToString();                    

                    //if (!string.IsNullOrEmpty(dtActuacionDetalle.Rows[0]["pago_vNumeroVoucher"].ToString()))
                    //     ucCtrlActuacionPago.vNroVoucher = dtActuacionDetalle.Rows[0]["pago_vNumeroVoucher"].ToString();

                    if (!string.IsNullOrEmpty(dtActuacionDetalle.Rows[0]["pago_sBancoId"].ToString()))
                        ddlNomBanco.SelectedValue = dtActuacionDetalle.Rows[0]["pago_sBancoId"].ToString();

                    txtNroOperacion.Text = dtActuacionDetalle.Rows[0]["pago_vBancoNumeroOperacion"].ToString();

                    //ucCtrlActuacionPago.vNroOperacion = dtActuacionDetalle.Rows[0]["pago_vBancoNumeroOperacion"].ToString();

                    ctrFecPago.Text = Comun.FormatearFecha(dtActuacionDetalle.Rows[0]["pago_dFechaOperacion"].ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);

                    
                    txtMtoCancelado.Text = dtActuacionDetalle.Rows[0]["FMonedaExtranjera"].ToString();


                    //bool bNoCobrado = ExisteInafecto_Exoneracion(ucCtrlActuacionPago.iTipoPago.ToString());

                    bool bNoCobrado = ExisteInafecto_Exoneracion(HF_TIPO_PAGO.Value);

                    if (bNoCobrado ||
                        HF_TIPO_PAGO.Value == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.GRATIS).ToString() ||
                        HF_TIPO_PAGO.Value == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.NO_COBRADO).ToString())
                    {
                        Txt_TotSC.Text = "0.00";
                        Txt_TotML.Text = "0.00";

                    }

                    if (Convert.ToInt32(HF_TIPO_PAGO.Value) == (int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA ||
                       Convert.ToInt32(HF_TIPO_PAGO.Value) == (int)Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA ||
                       Convert.ToInt32(HF_TIPO_PAGO.Value) == (int)Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA)
                    {
                        if (dtActuacionDetalle.Rows[0]["pago_FMontoSolesConsulares"].ToString() != string.Empty)
                        {
                            if (Convert.ToInt32(dtActuacionDetalle.Rows[0]["pago_sBancoId"]) != 0)
                            {
                                txtNroOperacion.Text = dtActuacionDetalle.Rows[0]["pago_vBancoNumeroOperacion"].ToString();
                                ddlNomBanco.SelectedValue = dtActuacionDetalle.Rows[0]["pago_sBancoId"].ToString();
                                ctrFecPago.set_Value = Comun.FormatearFecha(Convert.ToString(dtActuacionDetalle.Rows[0]["pago_dFechaOperacion"]));
                                txtMtoCancelado.Text = dtActuacionDetalle.Rows[0]["FMonedaExtranjera"].ToString();
                            }
                        }
                    }

                    
                    Txt_TarifaId.Text = string.Format("{0:0.00}", dtActuacionDetalle.Rows[0]["tari_vNumero"].ToString());
                    Txt_TarifaId.Enabled = false;
                    Txt_TarifaDescripcion.Text = dtActuacionDetalle.Rows[0]["tari_vDescripcionCorta"].ToString();

                    txtCantidad.Text = "1";
                    txtCantidad.Enabled = false;
                    Lst_Tarifario.Enabled = false;

                    CargarTipoPagoNormaTarifario();
                    ddlTipoPago.SelectedValue = dtActuacionDetalle.Rows[0]["pago_sPagoTipoId"].ToString();
                    txtSustentoTipoPago.Text = dtActuacionDetalle.Rows[0]["pago_vSustentoTipoPago"].ToString().ToUpper();


                    CargarDatosTarifaPago();

                    string strDescTipoPagoOrigen = Comun.ObtenerDescripcionTipoPago(Session, ddlTipoPago.SelectedValue);


                    Comun.ActualizarControlPago(Session, strDescTipoPagoOrigen, Txt_TarifaId.Text, txtCantidad.Text,
                        ref ctrlToolBar5.btnGrabar, ref ddlTipoPago, ref txtNroOperacion, ref txtAutoadhesivo,
                        ref ddlNomBanco, ref ctrFecPago, ref ddlExoneracion, ref lblExoneracion, ref lblValExoneracion,
                        ref txtSustentoTipoPago, ref lblSustentoTipoPago, ref lblValSustentoTipoPago,
                        ref RBNormativa, ref RBSustentoTipoPago, ref Txt_MontML, ref Txt_MtoSC,
                        ref Txt_TotML, ref Txt_TotSC, ref LblDescMtoML, ref LblDescTotML,
                        ref pnlPagLima, ref txtMtoCancelado);
                   



                    if (ddlTipoPago.SelectedItem.Text == "GRATUITO POR LEY" ||
                        ddlTipoPago.SelectedItem.Text == "INAFECTO POR INDIGENCIA")
                    {
                        ddlExoneracion.SelectedValue = dtActuacionDetalle.Rows[0]["pago_iNormaTarifarioId"].ToString();
                    }
                   

                    //---------------------------------------------------
                    
                    HabilitarBotonesImpresion(true);
                    

                    Lst_Tarifario.Enabled = false;


                    if (Convert.ToInt32(Session["PasoActualTab"]) < iTabVinculacionIndice)
                    {
                        Session["PasoActualTab"] = iTabVinculacionIndice.ToString();
                    }

                    Session["anep_iSelectedTabId"] = iTabVinculacionIndice;
                    vScript = "EnableTabIndex(" + iTabVinculacionIndice.ToString() + "); SetTabs('" + iTabVinculacionIndice.ToString() + "','" + ddlTipoActoNotarialExtra.SelectedValue.ToString() + "');";


                }
                else
                {
                    EjecutarScript(vScript);
                    return;
                }

                #endregion

                #region VINCULACION

                txtAutoadhesivo.Focus();
                if (vCodigoUnicoFabrica != string.Empty)
                {
                    Btn_ImprimirAutoadhesivo.Enabled = false;
                    
                    HabilitarTabParticipanteTotal(false);
                    HabilitarBotonesImpresion(false);
                    HabilitarTabCuerpo(false);
                    txtAutoadhesivo.Text = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.ALMACEN_TIPO_INSUMO, Convert.ToInt32(vTipoInsumoId), "valor") + vCodigoUnicoFabrica;
                    txtAutoadhesivo.Enabled = false;

                    if (Convert.ToInt32(Session["PasoActualTab"]) < iTabDigitalizacionIndice)
                    {
                        Session["PasoActualTab"] = iTabDigitalizacionIndice.ToString();
                    }

                    string strFlag = string.Empty;
                    if (dtActuacionInsumoDetalle.Rows.Count > 0)
                    {
                        if (dtActuacionInsumoDetalle.Rows.Count == 1)
                        {

                            strFlag = dtActuacionInsumoDetalle.Rows[0]["aide_bFlagImpresion"].ToString();
                            if (strFlag.Equals("SI"))
                            {
                                chkImpresion.Checked = true;
                                
                                Btn_ImprimirAutoadhesivo.Enabled = false;
                                hnd_ImpresionCorrecta.Value = "1";
                                txtAutoadhesivo.Enabled = false;
                                //Session["anep_iSelectedTabId"] = iTabDigitalizacionIndice;
                                Session["anep_iSelectedTabId"] = iTabVinculacionIndice;
                            }
                            else
                            {
                                Btn_ImprimirAutoadhesivo.Enabled = true;
                                Session["anep_iSelectedTabId"] = iTabVinculacionIndice;
                            }

                            btnGrabarVinculacion.Enabled = false;


                            Session[Constantes.CONST_ACTUACION_INSUMO_DETALLE_ID + HFGUID.Value] = dtActuacionInsumoDetalle.Rows[0]["aide_iActuacionInsumoDetalleId"].ToString();
                            hCodAutoadhesivo.Value = dtActuacionInsumoDetalle.Rows[0]["insu_iInsumoId"].ToString();
                            //Jonatan -- 20/07/2017 
                            if (txtAutoadhesivo.Text.Length > 0)
                            {
                                ctrlBajaAutoadhesivo1.Activar = true;
                            }
                            else
                            {
                                ctrlBajaAutoadhesivo1.Activar = false;
                            }
                        }



                        dtActuacionInsumoDetalle = null;
                    }


                    if (strFlag.Equals("SI"))
                    {
                        vScript = "EnableTabIndex(" + iTabDigitalizacionIndice.ToString() + "); SetTabs('" + iTabDigitalizacionIndice.ToString() + "','" + ddlTipoActoNotarialExtra.SelectedValue.ToString() + "'); ";
                        //vScript = "EnableTabIndex(" + iTabVinculacionIndice.ToString() + "); SetTabs('" + iTabVinculacionIndice.ToString() + "','" + ddlTipoActoNotarialExtra.SelectedValue.ToString() + "'); ";
                    }
                    else
                    {
                        vScript = "EnableTabIndex(" + iTabVinculacionIndice.ToString() + "); SetTabs('" + iTabVinculacionIndice.ToString() + "','" + ddlTipoActoNotarialExtra.SelectedValue.ToString() + "'); ";
                    }



                }
                else
                {
                    btnGrabarVinculacion.Enabled = true;
                    
                    txtAutoadhesivo.Enabled = true;
                    EjecutarScript(vScript);
                    return;
                }


                #endregion

                #region ARCHIVO DIGITAL


                RE_ACTONOTARIALDOCUMENTO documento = new RE_ACTONOTARIALDOCUMENTO();
                documento.ando_iActoNotarialId = lACTONOTARIAL.acno_iActoNotarialId;

                var s_lista_Documentos = new SGAC.Registro.Actuacion.BL.ActoNotarialMantenimiento().ListaActoNotarialDocumento(documento);

                var s_Lista_Digitalizados = s_lista_Documentos.FindAll(
                                         p => p.ando_sTipoDocumentoId == (Int16)Enumerador.enmTipoAdjunto.PDF ||
                                         p.ando_sTipoDocumentoId == (Int16)Enumerador.enmTipoAdjunto.DOCUMENTO_DIGITALIZA);


                Session["DocumentoDigitalizadoContainer"] = s_Lista_Digitalizados.ToList();
                Gdv_Adjunto.DataSource = s_Lista_Digitalizados.ToList();
                Gdv_Adjunto.DataBind();
                //EjecutarScript("tab_06(); EnableTabIndex(" + iTabDigitalizacionIndice.ToString() + "); SetTabs('" + iTabDigitalizacionIndice.ToString() + "','" + ddlTipoActoNotarialExtra.SelectedValue.ToString() + "');");



                #endregion

            }
        }

        protected void btnSave_tab4_Click(object sender, EventArgs e)
        {
            Int32 sIdiomaCastellanoId = Convert.ToInt32(Session["NotarialIdioma"].ToString());

            if (grd_Participantes.Rows.Count <= 0)
            {
                EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", Constantes.CONST_MENSAJE_SIN_PARTICIPANTES), "MensajeFaltaActoNotarial");
                return;
            }
            else
            {
                if ((ViewState[Constantes.CONST_SESION_ACTONOTARIAL_ID]).ToString() != "0")
                {

                    DataTable _dt = new DataTable();
                    ParticipanteConsultaBL _obj = new ParticipanteConsultaBL();
                    _dt = _obj.VerificarRegistroParticipantesExtraprotocolar(Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value), Convert.ToInt16(ddlTipoActoNotarialExtra.SelectedValue), Convert.ToInt64(this.hdn_actu_iPersonaRecurrenteId.Value), Convert.ToInt16(ddlSubTipoNotarialExtra.SelectedValue));

                    if (_dt.Rows[0]["Resultado"].ToString().Length > 0)
                    {
                        EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", _dt.Rows[0]["Resultado"].ToString()), "MensajeFaltaActoNotarial");
                        return;
                    }

                
                    #region CARGANDO ACTONOTARIAL
                    RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();
                    ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();

                    lACTONOTARIAL.acno_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
                    lACTONOTARIAL = lActoNotarialConsultaBL.obtener(lACTONOTARIAL);
                    //lACTONOTARIAL.acno_IFuncionarioCertificadorId = Convert.ToInt32(ddlViajeMenorCertificador.SelectedValue); // MDIAZ

                    //Actualizando información
                    // No encontre el CERFIFICADOR
                    //lACTONOTARIAL.acno_vObservaciones = txtObservacionesCertificador.Text;
                    lACTONOTARIAL.acno_vObservaciones = txtObservacion.Text;
                    lACTONOTARIAL.acno_vItinerario = txtItinerario.Text;
                    //lACTONOTARIAL.acno_cUbigeoDestino = ddl_UbigeoPaisViajeDestino.SelectedValue.ToString() +
                    //    ddl_UbigeoRegionViajeDestino.SelectedValue.ToString() +
                    //    ddl_UbigeoCiudadViajeDestino.SelectedValue.ToString();
                    lACTONOTARIAL.acno_cUbigeoDestino = hbigeoViajeDestino.Value;

                    lACTONOTARIAL.acno_dFechaModificacion = DateTime.Now;
                    lACTONOTARIAL.acno_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    lACTONOTARIAL.acno_vIPModificacion = lACTONOTARIAL.acno_vIPCreacion;
                    lACTONOTARIAL.acno_vPartidaRegistral = txtPartidaSunarp.Text;
                    lACTONOTARIAL.acno_vNumeroEscrituraPublica = txtEscrituraPublica.Text;
                    #endregion

                    ActoNotarialExtraProtocolarMantenimiento actoNotarialExtraProtocolarMantenimiento = new ActoNotarialExtraProtocolarMantenimiento();
                    int intResultado = actoNotarialExtraProtocolarMantenimiento.ActoNotarial_actualizar(lACTONOTARIAL);

                    if (intResultado == (int)Enumerador.enmResultadoQuery.OK)
                    {
                        //Comun.EjecutarScript(Page, "cerrarPopupEsperaCarga();"); 
                        EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Actuación", "Se han guardado los participantes satisfactoriamente."), "MensajeExitoGuardarParticipantes");
                    }
                    else
                    {
                        EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuación", Constantes.CONST_MENSAJE_OPERACION_FALLIDA));
                        return;
                    }

                    
                    Session["anep_iSelectedTabId"] = iTabCuerpoIndice;

                    hdn_bCheckAceptacion.Value = "0";
                    hdn_bCuerpoGrabado.Value = "0";

                    if (Convert.ToInt32(Session["PasoActualTab"]) <= iTabCuerpoIndice)
                    {
                        Session["PasoActualTab"] = iTabCuerpoIndice.ToString();

                        EjecutarScript("SetTabs('" + iTabCuerpoIndice.ToString() + "','" + ddlTipoActoNotarialExtra.SelectedValue.ToString() + "'); EnableTabIndex(" + iTabCuerpoIndice.ToString() + "); DesactivarAceptacion();", "MoverACuerpo");
                    }
                    else
                    {
                        EjecutarScript("EnableTabIndex(" + iTabCuerpoIndice.ToString() + "); DesactivarAceptacion();", "MoverACuerpo");
                    }

                    SetearIntroduccionyConlusionCuerpo();

                    CargarComboIncapacitados();
                    

                }
                else
                {
                    EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", "Se debe agregar el tipo de acto notarial."), "MensajeFaltaActoNotarial");
                }
                // txtCuerpo.Text = "";
                txtInfoAdicional.Text = "";
            }
        }

        
       

        protected void ddl_UbigeoPaisViajeDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_UbigeoPaisViajeDestino.SelectedIndex > 0)
            {
                comun_Part3.CargarUbigeo(Session, ddl_UbigeoRegionViajeDestino, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_UbigeoPaisViajeDestino.SelectedValue.ToString(), "", true);
                comun_Part3.CargarUbigeo(Session, ddl_UbigeoCiudadViajeDestino, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, string.Empty, string.Empty, true);

                if (ViewState["UbigeoViajeDestino"] != null)
                {
                    ddl_UbigeoRegionViajeDestino.SelectedValue = ViewState["UbigeoViajeDestino"].ToString().Substring(2, 2);
                    ddl_UbigeoRegionViajeDestino_SelectedIndexChanged(null, null);
                }
                else
                    ddl_UbigeoPaisViajeDestino.Focus();
            }
            else
            {
                ddl_UbigeoRegionViajeDestino.DataSource = new DataTable();
            }
            //EjecutarScript("HabilitarPorParticipante(false);HabilitarApellidoCasada();");
        }

        protected void ddl_UbigeoRegionViajeDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_UbigeoRegionViajeDestino.SelectedIndex > 0)
            {
                comun_Part3.CargarUbigeo(Session, ddl_UbigeoCiudadViajeDestino, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddl_UbigeoPaisViajeDestino.SelectedValue.ToString(), ddl_UbigeoRegionViajeDestino.SelectedValue.ToString(), true);

                if (ViewState["UbigeoViajeDestino"] != null)
                {
                    ddl_UbigeoCiudadViajeDestino.SelectedValue = ViewState["UbigeoViajeDestino"].ToString().Substring(4, 2);
                    ddl_UbigeoCiudadViajeDestino_SelectedIndexChanged(null, null);
                }
                ddl_UbigeoRegionViajeDestino.Focus();
            }
            else
            {
                ddl_UbigeoCiudadViajeDestino.SelectedIndex = 0;
            }
            //EjecutarScript("HabilitarPorParticipante(false);HabilitarApellidoCasada();");
        }

        protected void ddl_UbigeoRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_UbigeoRegion.SelectedIndex > 0)
            {
                comun_Part3.CargarUbigeo(Session, ddl_UbigeoCiudad, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddl_UbigeoPais.SelectedValue.ToString(), ddl_UbigeoRegion.SelectedValue.ToString(), true);

                if (ViewState["UbigeoParticipante"] != null)
                {
                    ddl_UbigeoCiudad.SelectedValue = ViewState["UbigeoParticipante"].ToString().Substring(4, 2);
                    ViewState.Remove("UbigeoParticipante");
                }
                else
                    ddl_UbigeoRegion.Focus();
            }
            else
            {
                ddl_UbigeoCiudad.SelectedIndex = 0;
            }
            //EjecutarScript("HabilitarPorParticipante(false);HabilitarApellidoCasada();");
        }

        protected void ddl_UbigeoPais_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (ddl_UbigeoPais.SelectedIndex > 0)
            {
                comun_Part3.CargarUbigeo(Session, ddl_UbigeoRegion, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_UbigeoPais.SelectedValue.ToString(), "", true);
                comun_Part3.CargarUbigeo(Session, ddl_UbigeoCiudad, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, string.Empty, string.Empty, true);

                if (ViewState["UbigeoParticipante"] != null)
                {
                    ddl_UbigeoRegion.SelectedValue = ViewState["UbigeoParticipante"].ToString().Substring(2, 2);
                    ddl_UbigeoRegion_SelectedIndexChanged(null, null);
                }
                else
                {
                    ddl_UbigeoPais.Focus();
                }
            }
            else
            {
                ddl_UbigeoRegion.DataSource = new DataTable();
            }
            //EjecutarScript("HabilitarPorParticipante(false);HabilitarApellidoCasada();");
        }

        private void AgregarParticipante()
        {
            try
            {
                if (!ValidarCamposPorTipoParticipanteTramite())
                {
                    return;
                }

                Int32 sIdiomaCastellanoId = Convert.ToInt32(Session["NotarialIdioma"].ToString());
                bool bError = false;
                string vNumeroDocumento = txtDocumentoNro.Text;
                Int16 iTipoParticipante_Editar = Convert.ToInt16(hTipParticipante_Editar.Value);
                Int16 iTipoParticipante = Convert.ToInt16(ddlRegistroTipoParticipante.SelectedValue);
                Int16 iTipoDocumento = Convert.ToInt16(ddlRegistroTipoDoc.SelectedValue);
                Int16 iGenero = Convert.ToInt16(ddlRegistroGenero.SelectedValue);

                #region Validar que se ingrese un solo tipo y numero de documento
                bool DocumentoIgual = false;
                Int16 iTipParticipanteConDocumentoIgual = 0;
                if (grd_Participantes.Rows.Count == 1 && Btn_AgregarParticipante.Text != "Actualizar")
                {
                    foreach (GridViewRow row in grd_Participantes.Rows)
                    {
                        Int16 iTipDocumentoGrilla = Convert.ToInt16(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "peid_sDocumentoTipoId")].Text);
                        string vNumDocumentoGrilla = row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "peid_vDocumentoNumero")].Text;
                        Int16 iTipParticipante = Convert.ToInt16(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_sTipoParticipanteId")].Text);
                        if (iTipDocumentoGrilla == iTipoDocumento && vNumDocumentoGrilla == vNumeroDocumento)
                        {
                            DocumentoIgual = true;
                            iTipParticipanteConDocumentoIgual = iTipParticipante;
                            break;
                        }
                    }
                    if (DocumentoIgual)
                    {
                        if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR).ToString())
                        {
                            #region Autorizacion_Viaje_Menor

                            if (iTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.ACOMPANANTE))
                            {
                                if (iTipParticipanteConDocumentoIgual != Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.PADRE) &&
                                    iTipParticipanteConDocumentoIgual != Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.MADRE))
                                {
                                    EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "Solo debe existir un participante con el mismo Número y Tipo de Documento."), "SoloUnParticipanteIgualDocumento");
                                    bError = true;
                                }
                            }
                            else if (iTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.PADRE))
                            {
                                if (iTipParticipanteConDocumentoIgual != Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.ACOMPANANTE))
                                {
                                    EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "Solo debe existir un participante con el mismo Número y Tipo de Documento."), "SoloUnParticipanteIgualDocumento");
                                    bError = true;
                                }
                            }
                            else if (iTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.MADRE))
                            {
                                if (iTipParticipanteConDocumentoIgual != Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.ACOMPANANTE))
                                {
                                    EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "Solo debe existir un participante con el mismo Número y Tipo de Documento."), "SoloUnParticipanteIgualDocumento");
                                    bError = true;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "Solo debe existir un participante con el mismo Número y Tipo de Documento."), "SoloUnParticipanteIgualDocumento");
                            bError = true;
                        }
                    }
                }
                else
                {
                    foreach (GridViewRow row in grd_Participantes.Rows)
                    {
                        Int16 iTipDocumentoGrilla = Convert.ToInt16(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "peid_sDocumentoTipoId")].Text);
                        string vNumDocumentoGrilla = row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "peid_vDocumentoNumero")].Text;
                        Int16 iTipParticipante = Convert.ToInt16(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_sTipoParticipanteId")].Text);
                        if (iTipDocumentoGrilla == iTipoDocumento && vNumDocumentoGrilla == vNumeroDocumento)
                        {
                            if (hTipDoc_Editar.Value != iTipoDocumento.ToString() && hNumDoc_Editar.Value != vNumDocumentoGrilla.ToString())
                            {
                                DocumentoIgual = true;
                                iTipParticipanteConDocumentoIgual = iTipParticipante;
                                break;
                            }
                        }
                    }
                    if (DocumentoIgual)
                    {
                        if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR).ToString())
                        {
                            #region Autorizacion_Viaje_Menor
                            if (iTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.ACOMPANANTE))
                            {
                                if (iTipParticipanteConDocumentoIgual != Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.PADRE) &&
                                    iTipParticipanteConDocumentoIgual != Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.MADRE))
                                {
                                    EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "Solo debe existir un participante con el mismo Número y Tipo de Documento."), "SoloUnParticipanteIgualDocumento");
                                    bError = true;
                                }
                            }
                            else if (iTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.PADRE))
                            {
                                if (iTipParticipanteConDocumentoIgual != Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.ACOMPANANTE))
                                {
                                    EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "Solo debe existir un participante con el mismo Número y Tipo de Documento."), "SoloUnParticipanteIgualDocumento");
                                    bError = true;
                                }
                            }
                            else if (iTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.MADRE))
                            {
                                if (iTipParticipanteConDocumentoIgual != Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.ACOMPANANTE))
                                {
                                    EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "Solo debe existir un participante con el mismo Número y Tipo de Documento."), "SoloUnParticipanteIgualDocumento");
                                    bError = true;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "Solo debe existir un participante con el mismo Número y Tipo de Documento."), "SoloUnParticipanteIgualDocumento");
                            bError = true;
                        }
                    }
                }
                #endregion

                #region Validación un solo interprete para poder fuera de registro

                if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.PODER_FUERA_REGISTRO).ToString())
                {
                    if (Btn_AgregarParticipante.Text == "Actualizar")
                    {
                        // Esta actualizando y se seleccionó para editar a un participante distinto a interprete
                        if (iTipoParticipante_Editar != Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.INTERPRETE))
                        {
                            // verifico que al dar clic en agregar que participante esta que ingresa
                            if (iTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.INTERPRETE))
                            {
                                // si es interprete, verifico que dentro de la grilla no exista algún interprete
                                if (hExisteInterprete.Value == "1")
                                {
                                    EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "Solo debe existir un participante Interprete."), "SoloUnParticipanteInterprete");
                                    bError = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        // verifico que al dar clic en agregar que participante esta que ingresa
                        if (iTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.INTERPRETE))
                        {
                            // si es interprete, verifico que dentro de la grilla no exista algún interprete
                            if (hExisteInterprete.Value == "1")
                            {
                                EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "Solo debe existir un participante Interprete."), "SoloUnParticipanteInterprete");
                                bError = true;
                            }
                        }
                    }
                }
                #endregion

                #region Validación de Género para autorización de viaje

                if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR).ToString())
                {
                    if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.PADRE) &&
                                    iGenero != Convert.ToInt32(Enumerador.enmGenero.MASCULINO))
                    {
                        EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "El tipo de participante PADRE debe poseer de género masculino."), "ExisteParticipante3");
                        bError = true;
                    }
                    else if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.MADRE) &&
                        iGenero != Convert.ToInt32(Enumerador.enmGenero.FEMENINO))
                    {
                        EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "El tipo de participante MADRE debe poseer de género femenino."), "ExisteParticipante4");
                        bError = true;
                    }
                    else if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.MENOR))
                    {
                        int edad = Comun.CalcularEdad(txtFecNac.Value());

                        if (edad >= 18)
                        {
                            EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "El tipo de participante MENOR debe poseer menos de 18 años."), "ExisteParticipante4");
                            bError = true;
                        }
                    }
                }

                #endregion

                #region Validación debe existir un solo participante para Certificado de Supervivencia
                if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.CERTIFICADO_SUPERVIVENCIA).ToString())
                {
                    if (grd_Participantes.Rows.Count == 1 && Btn_AgregarParticipante.Text != "Actualizar")
                    {
                        EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "Solo debe existir un participante para el Trámite de Certificado de Supervivencia."), "SoloUnParticipante");
                        bError = true;
                    }
                }
                #endregion

                #region Validación debe existir un solo tipo de participante (Padre, Madre o Acompañante) para Autorización viaje a menor
                if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR).ToString())
                {
                    if (Btn_AgregarParticipante.Text == "Actualizar")
                    {
                        // Esta actualizando y se seleccionó para editar a un participante distinto a Padre
                        if (iTipoParticipante_Editar != Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.PADRE))
                        {
                            // verifico que al dar clic en agregar que participante esta que ingresa
                            if (iTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.PADRE))
                            {
                                // si es Padre, verifico que dentro de la grilla no exista algún Padre
                                if (hExistePadre.Value == "1")
                                {
                                    EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "Solo debe existir un participante Padre."), "SoloUnParticipantePadre");
                                    bError = true;
                                }
                            }
                        }
                        // Esta actualizando y se seleccionó para editar a un participante distinto a Madre
                        if (iTipoParticipante_Editar != Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.MADRE))
                        {
                            // verifico que al dar clic en agregar que participante esta que ingresa
                            if (iTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.MADRE))
                            {
                                // si es Madre, verifico que dentro de la grilla no exista algún Madre
                                if (hExisteMadre.Value == "1")
                                {
                                    EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "Solo debe existir un participante Madre."), "SoloUnParticipanteMadre");
                                    bError = true;
                                }
                            }
                        }
                        // Esta actualizando y se seleccionó para editar a un participante distinto a ACOMPANANTE
                        if (iTipoParticipante_Editar != Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.ACOMPANANTE))
                        {
                            // verifico que al dar clic en agregar que participante esta que ingresa
                            if (iTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.ACOMPANANTE))
                            {
                                // si es ACOMPANANTE, verifico que dentro de la grilla no exista algún ACOMPANANTE
                                if (hExisteAcompañante.Value == "1")
                                {
                                    EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "Solo debe existir un participante Acompañante."), "SoloUnParticipanteAcompañante");
                                    bError = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        // verifico que al dar clic en agregar que participante esta que ingresa
                        if (iTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.PADRE))
                        {
                            // si es Padre, verifico que dentro de la grilla no exista algún Padre
                            if (hExistePadre.Value == "1")
                            {
                                EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "Solo debe existir un participante Padre."), "SoloUnParticipantePadre");
                                bError = true;
                            }
                        }
                        // verifico que al dar clic en agregar que participante esta que ingresa
                        if (iTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.MADRE))
                        {
                            // si es Madre, verifico que dentro de la grilla no exista algún Madre
                            if (hExisteMadre.Value == "1")
                            {
                                EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "Solo debe existir un participante Madre."), "SoloUnParticipanteMadre");
                                bError = true;
                            }
                        }
                        // verifico que al dar clic en agregar que participante esta que ingresa
                        if (iTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.ACOMPANANTE))
                        {
                            // si es ACOMPANANTE, verifico que dentro de la grilla no exista algún ACOMPANANTE
                            if (hExisteAcompañante.Value == "1")
                            {
                                EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "Solo debe existir un participante Acompañante."), "SoloUnParticipanteAcompañante");
                                bError = true;
                            }
                        }
                    }

                    #region Validar que se ingrese un solo Apoderado en Autorización de Viaje a Menor
                    bool bExisteApoderado = false;
                    if (Btn_AgregarParticipante.Text != "Actualizar")
                    {
                        foreach (GridViewRow row in grd_Participantes.Rows)
                        {
                            Int16 iTipParticipante_grilla = Convert.ToInt16(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_sTipoParticipanteId")].Text);
                            if (iTipoParticipante == iTipParticipante_grilla)
                            {
                                if (iTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.APODERADO))
                                {
                                    bExisteApoderado = true;
                                    break;
                                }
                            }
                        }
                        if (bExisteApoderado)
                        {
                            EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "Solo debe existir un participante Apoderado."), "SoloUnParticipanteIgualDocumento");
                            bError = true;
                        }
                    }
                    else
                    {
                        foreach (GridViewRow row in grd_Participantes.Rows)
                        {
                            Int16 iTipParticipante_grilla = Convert.ToInt16(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_sTipoParticipanteId")].Text);

                            if (iTipoParticipante == iTipParticipante_grilla)
                            {
                                if (iTipoParticipante_Editar != iTipParticipante_grilla)
                                {
                                    if (iTipoParticipante_Editar == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.APODERADO))
                                    {
                                        bExisteApoderado = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (bExisteApoderado)
                        {
                            EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "Solo debe existir un participante Apoderado."), "SoloUnParticipanteIgualDocumento");
                            bError = true;
                        }
                    }
                    #endregion
                }
                #endregion

                #region Validación solo se ingresa interprete cuando el otorgante tiene idioma distinto a español
                if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.PODER_FUERA_REGISTRO).ToString())
                {
                    // verifico que al dar clic en agregar que participante esta que ingresa
                    bool bOtorganteExtranjero = false;
                    if (iTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.INTERPRETE))
                    {
                        foreach (GridViewRow row in grd_Participantes.Rows)
                        {
                            string strIdiomaGrilla = row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "pers_sIdiomaNatalId")].Text;
                            Int16 iIdiomaGrilla = 0;
                            if (strIdiomaGrilla == "&nbsp;")
                            {
                                iIdiomaGrilla = 0;
                            }
                            else
                            {
                                iIdiomaGrilla = Convert.ToInt16(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "pers_sIdiomaNatalId")].Text);
                            }
                            //Int16 iIdiomaGrilla = Convert.ToInt16(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "pers_sIdiomaNatalId")].Text);
                            Int16 iTipParticipante = Convert.ToInt16(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_sTipoParticipanteId")].Text);
                            if (iIdiomaGrilla != sIdiomaCastellanoId && iTipParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.OTORGANTE))
                            {
                                bOtorganteExtranjero = true;
                            }
                        }
                        if (bOtorganteExtranjero == false)
                        {
                            EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "No es necesario que ingrese un interprete."), "SoloUnParticipanteIgualDocumento");
                            bError = true;
                        }
                    }
                }

                if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR).ToString())
                {
                    // verifico que al dar clic en agregar que participante esta que ingresa
                    bool bPadreMadreExtranjero = false;
                    if (iTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.INTERPRETE))
                    {
                        foreach (GridViewRow row in grd_Participantes.Rows)
                        {
                            string strIdiomaGrilla = row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "pers_sIdiomaNatalId")].Text;
                            Int16 iIdiomaGrilla = 0;
                            if (strIdiomaGrilla == "&nbsp;")
                            {
                                iIdiomaGrilla = 0;
                            }
                            else
                            {
                                iIdiomaGrilla = Convert.ToInt16(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "pers_sIdiomaNatalId")].Text);
                            }
                            Int16 iTipParticipante = Convert.ToInt16(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_sTipoParticipanteId")].Text);
                            if (iIdiomaGrilla != sIdiomaCastellanoId && iTipParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.MADRE))
                            {
                                bPadreMadreExtranjero = true;
                            }
                            if (iIdiomaGrilla != sIdiomaCastellanoId && iTipParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.PADRE))
                            {
                                bPadreMadreExtranjero = true;
                            }
                        }
                        if (bPadreMadreExtranjero == false)
                        {
                            EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "No es necesario que ingrese un interprete."), "SoloUnParticipanteIgualDocumento");
                            bError = true;
                        }
                    }
                }
                #endregion

                #region Validar que no se ingrese acompañante cuando tiene la condición de que menor viaja sin acompañante
                if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR).ToString())
                {
                    if (ddlCondiciones.SelectedItem.Text == "MENOR VIAJA SIN ACOMPAÑANTE")
                    {
                        if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.ACOMPANANTE))
                        {
                            EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "No es necesario agregar un ACOMPAÑANTE, porque la condición es que el MENOR VIAJA SIN ACOMPAÑANTE"), "ExisteParticipante4");
                            bError = true;
                        }
                    }
                }
                #endregion

                #region Validar que no se ingrese Participante apoderado cuando el subtipo sea Derecho Propio y en representacion de
                if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR).ToString())
                {
                    if (ddlSubTipoNotarialExtra.SelectedItem.Text == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - PADRE REPRESENTA A MADRE")
                    {
                        if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.APODERADO))
                        {
                            EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "No es necesario agregar un APODERADO, porque según el sub tipo de acto notarial el PADRE se consignará como el apoderado"), "ExisteParticipante4");
                            bError = true;
                        }
                    }
                    if (ddlSubTipoNotarialExtra.SelectedItem.Text == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - MADRE REPRESENTA A PADRE")
                    {
                        if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.APODERADO))
                        {
                            EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "No es necesario agregar un APODERADO, porque según el sub tipo de acto notarial la MADRE se consignará como el apoderado"), "ExisteParticipante4");
                            bError = true;
                        }
                    }
                }
                #endregion


                #region Para Tipos de Tramites en donde el apoderado representa a uno a ambos padres
                if (ddlSubTipoNotarialExtra.SelectedItem.Text == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE" ||
                            ddlSubTipoNotarialExtra.SelectedItem.Text == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A MADRE" ||
                            ddlSubTipoNotarialExtra.SelectedItem.Text == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE Y MADRE")
                {
                    if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.APODERADO))
                    {
                        txtEscrituraPublica.Text = txtNumeroEscritura.Text;
                        txtPartidaSunarp.Text = txtNumeroPartida.Text;
                    }
                }
                #endregion
                if (!bError)
                {
                    #region Grabar y Actualizar
                    RE_ACTONOTARIALPARTICIPANTE Entparticipante = LlenarEntidadParticipante();
                    RE_PERSONA EntPersona = LlenarEntidadPersona();

                    ParticipanteMantenimientoBL _obj = new ParticipanteMantenimientoBL();
                    ActoNotarialExtraProtocolarMantenimiento _obj2 = new ActoNotarialExtraProtocolarMantenimiento();
                    if (hCodPersona.Value != "0")
                    {
                        _obj.actualizar_minirune(EntPersona);
                    }
                    else
                    {
                        EntPersona = _obj.insertar_minirune(EntPersona);
                        Entparticipante.anpa_iPersonaId = EntPersona.pers_iPersonaId;
                    }

                    if (Btn_AgregarParticipante.Text == "Actualizar")
                    {
                        Entparticipante.anpa_iActoNotarialParticipanteId = Convert.ToInt64(hParticipanteID_Editar.Value);
                        _obj2.actualizar(Entparticipante);
                    }
                    else
                    {
                        _obj2.insertar(Entparticipante);
                    }

                    ParticipanteConsultaBL _obj3 = new ParticipanteConsultaBL();
                    DataTable _dt = new DataTable();
                    _dt = _obj3.ObtenerParticipantesExtraprotocolar(Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value));
                    grd_Participantes.DataSource = _dt;
                    grd_Participantes.DataBind();

                    LimpiarTabParticipante1();
                    CargarComboIncapacitados();
                    Btn_AgregarParticipante.Text = "Agregar";
                    MostrarCamposPorTipoParticipante();

                    #endregion

                    #region Llena Campos Para Validar
                    hExisteInterprete.Value = _dt.Rows[0]["ExisteInterprete"].ToString();
                    hExisteIdiomaExtranjero.Value = _dt.Rows[0]["ExisteIdiomaExtranjero"].ToString();

                    hExistePadre.Value = "0";
                    hExisteMadre.Value = "0";
                    hExisteAcompañante.Value = "0";

                    foreach (DataRow row in _dt.Rows)
                    {
                        if (Convert.ToInt32(row["anpa_sTipoParticipanteId"]) == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.PADRE))
                        {
                            hExistePadre.Value = "1";
                        }
                        if (Convert.ToInt32(row["anpa_sTipoParticipanteId"]) == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.MADRE))
                        {
                            hExisteMadre.Value = "1";
                        }
                        if (Convert.ToInt32(row["anpa_sTipoParticipanteId"]) == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.ACOMPANANTE))
                        {
                            hExisteAcompañante.Value = "1";
                        }
                    }
                    #endregion
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        protected void Btn_AgregarParticipante_Click(object sender, EventArgs e)
        {
            try
            {
                AgregarParticipante();
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

        protected void Clst_BaseLegal_SelectedIndexChanged(object sender, EventArgs e)
        {
            string TextoCuerpo = "";

            if (txtCuerpo.Text != "")
            {
                TextoCuerpo = txtCuerpo.Text;
            }

            foreach (ListItem item in Clst_BaseLegal.Items)
            {
                if (item.Selected)
                {
                    txtCuerpo.Text = TextoCuerpo + " - ARTICULOS - " + Clst_BaseLegal.SelectedValue.ToString();
                }
            }
        }
        

        # region Métodos

        private void HabilitarTabs()
        {
            try
            {
                string strScript = string.Empty;
                strScript += Util.HabilitarTab(0);
                strScript += Util.DeshabilitarTab(1);
                strScript += Util.DeshabilitarTab(2);
                strScript += Util.DeshabilitarTab(3);
                strScript += Util.DeshabilitarTab(4);
                strScript += Util.DeshabilitarTab(5);
                strScript += Util.DeshabilitarTab(6);
                EjecutarScript(strScript);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void HabilitarVistaPrevia(int pintExtraProtocolarTipoId)
        {
            var enmExtraProtocolarTipoId = (SGAC.Accesorios.Enumerador.enmExtraprotocolarTipo)pintExtraProtocolarTipoId;
            switch (enmExtraProtocolarTipoId)
            {
                case Enumerador.enmExtraprotocolarTipo.PODER_FUERA_REGISTRO:
                    {
                        trPoderFueraRegistro.Visible = true;
                        break;
                    }
                case Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR:
                    {
                        trAutorizacionViajeMenor.Visible = true;
                        break;
                    }
                case Enumerador.enmExtraprotocolarTipo.CERTIFICADO_SUPERVIVENCIA:
                    {
                        trSupervivencia.Visible = true;
                        break;
                    }
            }
        }

        [System.Web.Services.WebMethod]
        public static void SetSession(string variable, string valor)
        {

            if (valor.ToLower() == "true")
            {
                HttpContext.Current.Session[variable] = true;
            }
            else if (valor.ToLower() == "false")
            {
                HttpContext.Current.Session[variable] = false;
            }
            else
            {
                HttpContext.Current.Session[variable] = valor;
            }
        }

        [System.Web.Services.WebMethod]
        public static string GetSession(string variable)
        {
            string strReturn = string.Empty;
            if (HttpContext.Current.Session[variable] != null)
                strReturn = HttpContext.Current.Session[variable].ToString();
            return strReturn.Trim();
        }

        [System.Web.Services.WebMethod]
        public static string ExisteDocumentosAdjuntos()
        {
            string strReturn = string.Empty;
            if (HttpContext.Current.Session["DocumentoDigitalizadoContainer"] != null)
            {
                if (((List<RE_ACTONOTARIALDOCUMENTO>)HttpContext.Current.Session["DocumentoDigitalizadoContainer"]).Count > 0)
                {
                    return "1";
                }
            }

            return "0";
        }

        [System.Web.Services.WebMethod]
        public static string ExisteActuacion()
        {
            string strReturn = string.Empty;
            if (HttpContext.Current.Session["dtActuacionDetalle"] != null)
            {
                if (((DataTable)HttpContext.Current.Session["dtActuacionDetalle"]).Rows.Count > 0)
                {
                    return "1";
                }
            }

            return "0";
        }

        [System.Web.Services.WebMethod]
        public static string insert_archivodigital(string archivodigital)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var jsonObject = serializer.Deserialize<dynamic>(archivodigital);

            RE_ACTONOTARIALDOCUMENTO lACTONOTARIALDOCUMENTO = new RE_ACTONOTARIALDOCUMENTO();
            #region TO OBJECT ...
            lACTONOTARIALDOCUMENTO.ando_iActoNotarialDocumentoId = Convert.ToInt64(jsonObject["ando_iActoNotarialDocumentoId"]);
            lACTONOTARIALDOCUMENTO.ando_iActoNotarialId = Convert.ToInt64(jsonObject["ando_iActoNotarialId"]);
            lACTONOTARIALDOCUMENTO.ando_sTipoDocumentoId = Convert.ToInt16(jsonObject["ando_sTipoDocumentoId"]);
            lACTONOTARIALDOCUMENTO.ando_vDescripcion = Convert.ToString(jsonObject["ando_vDescripcion"]);
            lACTONOTARIALDOCUMENTO.ando_vRutaArchivo = Convert.ToString(jsonObject["ando_vRutaArchivo"]);
            lACTONOTARIALDOCUMENTO.ando_sUsuarioCreacion = Convert.ToInt16(jsonObject["ando_sUsuarioCreacion"]);
            lACTONOTARIALDOCUMENTO.ando_vIPCreacion = Convert.ToString(jsonObject["ando_vIPCreacion"]);
            lACTONOTARIALDOCUMENTO.ando_sUsuarioModificacion = Convert.ToInt16(jsonObject["ando_sUsuarioModificacion"]);
            lACTONOTARIALDOCUMENTO.ando_vIPModificacion = Convert.ToString(jsonObject["ando_vIPModificacion"]);

            #endregion

            ActoNotarialMantenimiento mnt = new ActoNotarialMantenimiento();
            return serializer.Serialize(mnt.InsertarActoNotarialDocumento(lACTONOTARIALDOCUMENTO)).ToString();
        }

        [System.Web.Services.WebMethod]
        public static string insert_registro(string actonotarial)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var jsonObject = serializer.Deserialize<dynamic>(actonotarial);

            RE_ACTONOTARIAL lactonotarial = new RE_ACTONOTARIAL();
            #region Creando objeto
            lactonotarial.acno_iActoNotarialId = Convert.ToInt64(jsonObject["acno_iActoNotarialId"]);
            lactonotarial.acno_iActuacionId = Convert.ToInt64(jsonObject["acno_iActuacionId"]);
            lactonotarial.acno_sOficinaConsularId = Convert.ToInt16(jsonObject["acno_sOficinaConsularId"]);

            //Cuando es EDICIÓN 
            if ((lactonotarial.acno_iActoNotarialId != 0) || (lactonotarial.acno_iActuacionId != 0))
            {
                ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();
                lactonotarial = lActoNotarialConsultaBL.obtener(lactonotarial);
            }
            //////

            lactonotarial.acno_iActoNotarialReferenciaId = Convert.ToInt16(jsonObject["acno_iActoNotarialReferenciaId"]);
            lactonotarial.acno_sTipoActoNotarialId = Convert.ToInt16(Enumerador.enmNotarialTipoActo.EXTRAPROTOCOLAR);
            lactonotarial.acno_sSubTipoActoNotarialId = Convert.ToInt16(jsonObject["acno_sSubTipoActoNotarialId"]);
            lactonotarial.acno_bFlagMinuta = ((string)jsonObject["acno_bFlagMinuta"] == "0") ? false : true;
            lactonotarial.acno_vNumeroEscrituraPublica = Convert.ToString(jsonObject["acno_vNumeroEscrituraPublica"]);
            lactonotarial.acno_vDenominacion = Convert.ToString(jsonObject["acno_vDenominacion"]);

            //Log : Insersión
            lactonotarial.acno_dFechaCreacion = DateTime.Now;
            lactonotarial.acno_sUsuarioCreacion = Convert.ToInt16(jsonObject["acno_sUsuarioCreacion"]);
            lactonotarial.acno_vIPCreacion = Convert.ToString(jsonObject["acno_vIPCreacion"]);

            //Log : Modificación
            lactonotarial.acno_dFechaModificacion = DateTime.Now;
            lactonotarial.acno_sUsuarioModificacion = Convert.ToInt16(jsonObject["acno_sUsuarioModificacion"]);
            lactonotarial.acno_vIPModificacion = Convert.ToString(jsonObject["acno_vIPModificacion"]);
            #endregion

            RE_ACTUACION lACTUACION = new RE_ACTUACION();
            #region ACTUACION
            lACTUACION.actu_iPersonaRecurrenteId = Convert.ToInt64(jsonObject["actu_iPersonaRecurrenteId"]);
            lACTUACION.actu_sOficinaConsularId = lactonotarial.acno_sOficinaConsularId;
            lACTUACION.actu_dFechaRegistro = lactonotarial.acno_dFechaCreacion;
            lACTUACION.actu_sUsuarioCreacion = lactonotarial.acno_sUsuarioCreacion;
            lACTUACION.actu_sEstado = (int)Enumerador.enmActuacionEstado.REGISTRADO;
            lACTUACION.actu_vIPCreacion = lactonotarial.acno_vIPCreacion;
            lACTUACION.actu_dFechaCreacion = lactonotarial.acno_dFechaCreacion;
            lACTUACION.actu_FCantidad = 1;
            #endregion

            lactonotarial.ACTUACION = lACTUACION;

            ActoNotarialMantenimiento mnt = new ActoNotarialMantenimiento();
            return serializer.Serialize(mnt.Insertar_ActoNotarial(lactonotarial)).ToString();
        }

        public static string insert_registroCodeBehind(string actonotarial)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var jsonObject = serializer.Deserialize<dynamic>(actonotarial);

            RE_ACTONOTARIAL lactonotarial = new RE_ACTONOTARIAL();
            #region Creando objeto
            lactonotarial.acno_iActoNotarialId = Convert.ToInt64(jsonObject["acno_iActoNotarialId"]);
            lactonotarial.acno_iActuacionId = Convert.ToInt64(jsonObject["acno_iActuacionId"]);
            lactonotarial.acno_sOficinaConsularId = Convert.ToInt16(jsonObject["acno_sOficinaConsularId"]);

            //Cuando es EDICIÓN 
            if ((lactonotarial.acno_iActoNotarialId != 0) || (lactonotarial.acno_iActuacionId != 0))
            {
                ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();
                lactonotarial = lActoNotarialConsultaBL.obtener(lactonotarial);
            }
            //////

            lactonotarial.acno_iActoNotarialReferenciaId = Convert.ToInt64(jsonObject["acno_iActoNotarialReferenciaId"]);
            lactonotarial.acno_sTipoActoNotarialId = Convert.ToInt16(Enumerador.enmNotarialTipoActo.EXTRAPROTOCOLAR);
            lactonotarial.acno_sSubTipoActoNotarialId = Convert.ToInt16(jsonObject["acno_sSubTipoActoNotarialId"]);
            lactonotarial.acno_bFlagMinuta = ((string)jsonObject["acno_bFlagMinuta"] == "0") ? false : true;
            lactonotarial.acno_vNumeroEscrituraPublica = Convert.ToString(jsonObject["acno_vNumeroEscrituraPublica"]);
            lactonotarial.acno_vDenominacion = Convert.ToString(jsonObject["acno_vDenominacion"]);

            //Log : Insersión
            lactonotarial.acno_dFechaCreacion = DateTime.Now;
            lactonotarial.acno_sUsuarioCreacion = Convert.ToInt16(jsonObject["acno_sUsuarioCreacion"]);
            lactonotarial.acno_vIPCreacion = Convert.ToString(jsonObject["acno_vIPCreacion"]);

            //Log : Modificación
            lactonotarial.acno_dFechaModificacion = DateTime.Now;
            lactonotarial.acno_sUsuarioModificacion = Convert.ToInt16(jsonObject["acno_sUsuarioModificacion"]);
            lactonotarial.acno_vIPModificacion = Convert.ToString(jsonObject["acno_vIPModificacion"]);
            #endregion

            RE_ACTUACION lACTUACION = new RE_ACTUACION();
            #region ACTUACION
            lACTUACION.actu_iPersonaRecurrenteId = Convert.ToInt64(jsonObject["actu_iPersonaRecurrenteId"]);
            lACTUACION.actu_sOficinaConsularId = lactonotarial.acno_sOficinaConsularId;
            lACTUACION.actu_dFechaRegistro = lactonotarial.acno_dFechaCreacion;
            lACTUACION.actu_sUsuarioCreacion = lactonotarial.acno_sUsuarioCreacion;
            lACTUACION.actu_sEstado = (int)Enumerador.enmActuacionEstado.REGISTRADO;
            lACTUACION.actu_vIPCreacion = lactonotarial.acno_vIPCreacion;
            lACTUACION.actu_dFechaCreacion = lactonotarial.acno_dFechaCreacion;
            lACTUACION.actu_FCantidad = 1;
            #endregion

            lactonotarial.ACTUACION = lACTUACION;

            ActoNotarialMantenimiento mnt = new ActoNotarialMantenimiento();
            return serializer.Serialize(mnt.Insertar_ActoNotarial(lactonotarial)).ToString();
        }



        [System.Web.Services.WebMethod(EnableSession = true)]
        public static  string insert_cuerpo(string cuerpo)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var jsonObject = serializer.Deserialize<dynamic>(cuerpo);
            
            CBE_CUERPO lCuerpo = new CBE_CUERPO();
            #region Creando objecto ....
            lCuerpo.ancu_iActoNotarialCuerpoId = Convert.ToInt64(jsonObject["ancu_iActoNotarialCuerpoId"]);
            lCuerpo.ancu_iActoNotarialId = Convert.ToInt64(jsonObject["ancu_iActoNotarialId"]);
            lCuerpo.ancu_vCuerpo = Convert.ToString(jsonObject["ancu_vCuerpo"]);
            lCuerpo.ancu_vFirmaIlegible = "";
            //lCuerpo.ancu_bFlagExtraprotocolarCuerpo = Convert.ToBoolean(jsonObject["ancu_bFlagExtraprotocolarCuerpo"]);
            lCuerpo.ancu_sUsuarioCreacion = Convert.ToInt16(jsonObject["ancu_sUsuarioCreacion"]);
            lCuerpo.ancu_vIPCreacion = Convert.ToString(jsonObject["ancu_vIPCreacion"]);
            lCuerpo.ancu_sUsuarioModificacion = Convert.ToInt16(jsonObject["ancu_sUsuarioModificacion"]);
            lCuerpo.ancu_vIPModificacion = Convert.ToString(jsonObject["ancu_vIPModificacion"]);
            lCuerpo.ActoNotarial.acno_bFlagLeidoAprobado = Convert.ToBoolean(jsonObject["acno_bFlagLeidoAprobado"]);
            lCuerpo.ActoNotarial.acno_iActuacionId = Convert.ToInt64(HttpContext.Current.Session[Constantes.CONST_SESION_ACTUACION_ID]);

            //---adicionado acno_sOficinaConsularId  ==>pipa
            lCuerpo.ActoNotarial.acno_sOficinaConsularId =Convert.ToInt16(jsonObject["acno_sOficinaConsularId"]);
            try
            {
                lCuerpo.ActoNotarial.acno_sTamanoLetra = Convert.ToInt64(jsonObject["acno_sTamanoLetra"]);
            }
            catch
            {
                lCuerpo.ActoNotarial.acno_sTamanoLetra = 7;
            }

            #endregion

            Int64 intActoNotarialId = Convert.ToInt64(jsonObject["ancu_iActoNotarialId"]);
            Int16 intTipoActoNotarial = Convert.ToInt16(jsonObject["TipoActoNotarial"]);
            bool bTieneFirmaTitular = Convert.ToBoolean(jsonObject["ImprimirFirma"]);

            ActualizarParticipante(intActoNotarialId, intTipoActoNotarial, bTieneFirmaTitular);

            ActoNotarialMantenimiento mnt = new ActoNotarialMantenimiento();
            //--=========================adicionado por ==>pipa
            //----30/102020
            JResult result = mnt.Insertar_ActoNotarialCuerpoExtraProtocolar(lCuerpo, null);
            //--------adicionado por Pipa este metodo proviene del evento btnGrabarCuerpo_Click del boton "Aprobar"
            //GrabarCuerpo(Convert.ToInt64(jsonObject["ancu_iActoNotarialId"])); //se comenta por lo que ya no es necesario, el cuerpo ya se grabó previamente

            HttpContext.Current.Session["anep_iSelectedTabId"] = iTabAprobacionPagoIndice;
            if (HttpContext.Current.Session["PasoActualTab"] == null)
            {
                HttpContext.Current.Session["PasoActualTab"] = 1;
            }
            if (Convert.ToInt32(HttpContext.Current.Session["PasoActualTab"]) < iTabAprobacionPagoIndice)
            {
                HttpContext.Current.Session["PasoActualTab"] = iTabAprobacionPagoIndice.ToString();
                
            }
            //=============================
            return serializer.Serialize(result).ToString();
        }

        [System.Web.Services.WebMethod]
        public static string obtener_persona(string idpersona, string tipodocumento, string documento)
        {
            RE_PERSONA lPersona = new RE_PERSONA();
            RE_PERSONAIDENTIFICACION lPersonaIdentificacion = new RE_PERSONAIDENTIFICACION();

            #region Identificación
            PersonaIdentificacionConsultaBL lPersonaIdentificacionConsultaBL = new PersonaIdentificacionConsultaBL();
            if (idpersona != null) lPersonaIdentificacion.peid_iPersonaId = Convert.ToInt64(idpersona);
            if (tipodocumento != null) lPersonaIdentificacion.peid_sDocumentoTipoId = Convert.ToInt16(tipodocumento);
            if (documento != null) lPersonaIdentificacion.peid_vDocumentoNumero = documento.ToString();
            lPersonaIdentificacion = lPersonaIdentificacionConsultaBL.Obtener(lPersonaIdentificacion);
            #endregion

            #region Verificar si existe Persona
            if (lPersonaIdentificacion.peid_iPersonaId != 0)
            {
                PersonaConsultaBL lPersonaConsultaBL = new PersonaConsultaBL();
                lPersona.pers_iPersonaId = lPersonaIdentificacion.peid_iPersonaId;
                lPersona = lPersonaConsultaBL.Obtener(lPersona);
                lPersona.Identificacion = lPersonaIdentificacion;
            }
            #endregion

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(lPersona).ToString();
        }

        public RE_PERSONA obtener_personaCodeBehind(string idpersona, string tipodocumento, string documento)
        {
            RE_PERSONA lPersona = new RE_PERSONA();
            RE_PERSONAIDENTIFICACION lPersonaIdentificacion = new RE_PERSONAIDENTIFICACION();

            //#region Identificación
            //PersonaIdentificacionConsultaBL lPersonaIdentificacionConsultaBL = new PersonaIdentificacionConsultaBL();
            if (idpersona != null) lPersonaIdentificacion.peid_iPersonaId = Convert.ToInt16(idpersona);
            if (tipodocumento != null) lPersonaIdentificacion.peid_sDocumentoTipoId = Convert.ToInt16(tipodocumento);
            if (documento != null) lPersonaIdentificacion.peid_vDocumentoNumero = documento.ToString();
            //lPersonaIdentificacion = lPersonaIdentificacionConsultaBL.Obtener(lPersonaIdentificacion);
            //#endregion

            //#region Verificar si existe Persona
            //if (lPersonaIdentificacion.peid_iPersonaId != 0)
            //{
            PersonaConsultaBL lPersonaConsultaBL = new PersonaConsultaBL();
            lPersona.Identificacion = lPersonaIdentificacion;
            if (lPersonaIdentificacion.peid_iPersonaId != 0)
                lPersona.pers_iPersonaId = lPersonaIdentificacion.peid_iPersonaId;

            lPersona = lPersonaConsultaBL.Obtener(lPersona);
            //}
            //#endregion

            return lPersona;
        }

        private void CargarListadosDesplegables()
        {

            //TAB REGISTRO 
            Util.CargarParametroDropDownList(ddlTipoActoNotarialExtra, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_NOTARIAL_TIPO_ACTO_EXTRAPROTOCOLAR), true);
            ddlTipoActoNotarialExtra.SelectedIndex = 0;

            //TAB PARTICPANTES #1 Poder Fuera de Registro
            Util.CargarDropDownList(ddlRegistroTipoDoc, comun_Part1.ObtenerDocumentoIdentidad(), "Valor", "Id", true);


            Util.CargarParametroDropDownList(ddlRegistroGenero, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_GENERO), true);
            Util.CargarParametroDropDownList(ddlRegistroEstadoCivil, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.ESTADO_CIVIL), true);
            Util.CargarParametroDropDownList(ddlRegistroNacionalidad, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_NACIONALIDAD), true);
            Util.CargarParametroDropDownList(ddlRegistroProfesion, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.OCUPACION), true);
            Util.CargarParametroDropDownList(ddlRegistroTipoParticipante, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_NOTARIAL_TIPO_ACTOR), true);
            Util.CargarParametroDropDownList(ddlRegistroIdioma, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.TRADUCCION_IDIOMA), true);

            Util.CargarParametroDropDownList(ddlSubTipoNotarialExtra, comun_Part1.ObtenerParametrosPorGrupoOrdenadoID(Session, "REGISTRO NOTARIAL- EXTRAPROTOCOLAR SUB-TIPO"), true);
            Util.CargarParametroDropDownList(ddlCondiciones, comun_Part1.ObtenerParametrosPorGrupo(Session, "EXTRAPROTOCOLARES-CONDICIONES-VIAJE DE MENOR"), true, "[SIN CONDICIONES]");

            CargarComboIncapacitados();

            ddlRegistroNacionalidad.SelectedIndex = 0;
            comun_Part3.CargarUbigeo(Session, ddl_UbigeoPais, Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT, string.Empty, string.Empty, true);
            comun_Part3.CargarUbigeo(Session, ddl_UbigeoRegion, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, string.Empty, string.Empty, true);
            comun_Part3.CargarUbigeo(Session, ddl_UbigeoCiudad, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, string.Empty, string.Empty, true);

            // MDIAZ
            //CargarFuncionariosViaje(Comun.ToNullInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), 0);
            ///////////

            CargarUbigeo();
            //comun_Part3.CargarUbigeo(Session, ddl_UbigeoPaisViajeDestino, Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT, string.Empty, string.Empty, true);
            comun_Part3.CargarUbigeo(Session, ddl_UbigeoRegionViajeDestino, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, string.Empty, string.Empty, true);
            comun_Part3.CargarUbigeo(Session, ddl_UbigeoCiudadViajeDestino, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, string.Empty, string.Empty, true);

            //TAB CUERPO
            //TAB INSERTOS
            //TAB APROBACIÓN Y PAGO

            //TAB ARCHIVO DIGITAL
            Util.CargarParametroDropDownList(ddlTipoArchivoAdjunto, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTUACION_TIPO_ADJUNTO), true);

            Util.DropDownListEliminarRegistro(ddlTipoArchivoAdjunto, Convert.ToInt32(Enumerador.enmTipoAdjunto.PDF).ToString());
            Util.DropDownListEliminarRegistro(ddlTipoArchivoAdjunto, Convert.ToInt32(Enumerador.enmTipoAdjunto.FIRMA).ToString());
            Util.DropDownListEliminarRegistro(ddlTipoArchivoAdjunto, Convert.ToInt32(Enumerador.enmTipoAdjunto.HUELLA).ToString());
            Util.DropDownListEliminarRegistro(ddlTipoArchivoAdjunto, Convert.ToInt32(Enumerador.enmTipoAdjunto.OTRO).ToString());
            Util.DropDownListEliminarRegistro(ddlTipoArchivoAdjunto, Convert.ToInt32(Enumerador.enmTipoAdjunto.FOTO).ToString());
            Util.DropDownListEliminarRegistro(ddlTipoArchivoAdjunto, Convert.ToInt32(Enumerador.enmTipoAdjunto.FOTO_PERFIL).ToString());
            Util.DropDownListEliminarRegistro(ddlTipoArchivoAdjunto, Convert.ToInt32(Enumerador.enmTipoAdjunto.HUELLA_INDICE_DERECHO).ToString());
            Util.DropDownListEliminarRegistro(ddlTipoArchivoAdjunto, Convert.ToInt32(Enumerador.enmTipoAdjunto.HUELLA_INDICE_IZQUIERDO).ToString());
            Util.CargarParametroDropDownList(ddlNomBanco, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.BANCO), true);

            DataTable dtTipoPago = new DataTable();
            dtTipoPago = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACREDITACION_TIPO_COBRO);

            DataView dv = dtTipoPago.DefaultView;
            DataTable dtTipoPagoOrdenadoOrdenado = dv.ToTable();
            dtTipoPagoOrdenadoOrdenado.DefaultView.Sort = "torden ASC";

            Util.CargarParametroDropDownList(ddlTipoPago, dtTipoPagoOrdenadoOrdenado, true);

            if (Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Convert.ToInt16(Constantes.CONST_ID_CONSULADO_CARACAS))
            {
                ddlTipoPago.Items.FindByText("PAGO ARUBA").Enabled = false;
                ddlTipoPago.Items.FindByText("PAGO OTRAS ISLAS CARIBEÑAS").Enabled = false;
            }

            lblchkAceptacion.Text = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.AVISOS, Convert.ToInt32(Enumerador.enmNotarialAvisos.CONFORMIDAD_DE_TEXTO), "valor");

            OrdenarTipoActoxUso();
        }

        private void CargarComboIncapacitados()
        {
            ddlOtorganteIncapacitado.Items.Clear();
            ddlOtorganteIncapacitado.DataTextField = "descripcion";
            ddlOtorganteIncapacitado.DataValueField = "id";
            ddlOtorganteIncapacitado.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));

            Int16 iTipoParticipante = Convert.ToInt16(ddlRegistroTipoParticipante.SelectedValue);
            
            if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR).ToString())
            {
                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO))
                {
                    foreach (GridViewRow row in grd_Participantes.Rows)
                    {
                        Int16 iTipParticipanteGrilla = Convert.ToInt16(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_sTipoParticipanteId")].Text);
                        bool bIncapacidadFlag = Convert.ToBoolean(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "pers_bIncapacidadFlag")].Text);

                        if (iTipParticipanteGrilla == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.PADRE) ||
                            iTipParticipanteGrilla == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.MADRE))
                        {
                            if (bIncapacidadFlag)
                            {
                                ddlOtorganteIncapacitado.Items.Add(new ListItem(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "participante")].Text,
                                                                  row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_iActoNotarialParticipanteId")].Text));
                            }
                            //ddlOtorganteIncapacitado.Items.Add(new ListItem(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "participante")].Text,
                            //                                  row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_iActoNotarialParticipanteId")].Text + "," +
                            //                                  row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "peid_sDocumentoTipoId")].Text + "," +
                            //                                  row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "peid_vDocumentoNumero")].Text));
                            
                            
                        }
                    }
                }

                
                    
            }
            if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.PODER_FUERA_REGISTRO).ToString())
            {
                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO))
                {
                    foreach (GridViewRow row in grd_Participantes.Rows)
                    {
                        Int16 iTipParticipanteGrilla = Convert.ToInt16(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_sTipoParticipanteId")].Text);
                        bool bIncapacidadFlag = Convert.ToBoolean(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "pers_bIncapacidadFlag")].Text);
                        if (iTipParticipanteGrilla == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.OTORGANTE))
                        {
                            //ddlOtorganteIncapacitado.Items.Add(new ListItem(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "participante")].Text,
                            //                                  row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_iReferenciaParticipanteId")].Text + "," +
                            //                                  row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "peid_sDocumentoTipoId")].Text + "," +
                            //                                  row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "peid_vDocumentoNumero")].Text));
                            if (bIncapacidadFlag)
                            {
                                ddlOtorganteIncapacitado.Items.Add(new ListItem(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "participante")].Text,
                                                                  row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_iActoNotarialParticipanteId")].Text));
                            }
                        }
                    }
                }

               
            }
        }

        #endregion

        public void OrdenarTipoActoxUso()
        {
            ActoNotarialConsultaBL oActoNotarialConsultaBL = new ActoNotarialConsultaBL();
            DataTable dtUsoSubTipo = oActoNotarialConsultaBL.ObtenerUsoSubTipo(Convert.ToInt64(Enumerador.enmNotarialTipoActo.EXTRAPROTOCOLAR));

            List<int[]> listInt = new List<int[]>();

            for (int j = 1; j < ddlTipoActoNotarialExtra.Items.Count; j++)
            {
                string value = ddlTipoActoNotarialExtra.Items[j].Value;
                int iOrden = Convert.ToInt32(comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.REG_NOTARIAL_TIPO_ACTO_EXTRAPROTOCOLAR, Convert.ToInt32(value), "tOrden"));

                listInt.Add(new int[] { Convert.ToInt32(value), iOrden });
            }

            foreach (int[] arr in listInt)
            {
                ListItem item = ddlTipoActoNotarialExtra.Items.FindByValue(arr[0].ToString());
                ddlTipoActoNotarialExtra.Items.Remove(item);
                ddlTipoActoNotarialExtra.Items.Insert(arr[1], item);
            }
        }


        private DataTable BindListTarifario(string IntSeccionId, ref string strDescripcion)
        {
            DataTable dtTarifario;
            int NroRegistros = 0;

            object[] arrParametros = { 0, this.Txt_TarifaId.Text, "", ((char)Enumerador.enmEstado.ACTIVO).ToString(),
                                       1, 50, 0, 0 };

            dtTarifario = comun_Part2.ObtenerTarifario(Session, ref arrParametros);
            //Session.Remove("dtTarifarioFiltrado");

            if (dtTarifario != null)
            {
                NroRegistros = dtTarifario.Rows.Count;

                if (NroRegistros == 0)
                {
                    LimpiarListaTarifa();
                    LimpiarDatosTarifaPago();
                }
                else
                {
                    //Session.Add("dtTarifarioFiltrado", dtTarifario);
                    ViewState["dtTarifarioFiltrado"] = dtTarifario;
                }
            }

            return dtTarifario;
        }

        private void BuscarTarifario()
        {
            try
            {
                string StrScript = string.Empty;
                string strDescripcionTarifa = string.Empty;
                LimpiarListaTarifa();
                if (this.Txt_TarifaId.Text.Length > 0)
                {
                    //ucCtrlActuacionPago.vTarifaLetra = Txt_TarifaId.Text.Trim().ToUpper();
                    Txt_TarifaId.Text = Txt_TarifaId.Text.Trim().ToUpper();

                    dtTarifarioFiltrado = BindListTarifario(this.Txt_TarifaId.Text, ref strDescripcionTarifa);


                    if (dtTarifarioFiltrado != null)
                    {
                        this.Txt_TarifaDescripcion.Text = strDescripcionTarifa;
                        if (dtTarifarioFiltrado.Rows.Count > 0)
                        {
                            int intSeccionId = Convert.ToInt32(dtTarifarioFiltrado.Rows[0]["tari_sSeccionId"]);
                            if (intSeccionId == (int)Enumerador.enmSeccion.ACTO_NOTARIAL)
                            {
                                if (dtTarifarioFiltrado.Rows.Count == 1)
                                {
                                    this.Txt_TarifaId.Text = dtTarifarioFiltrado.Rows[0]["tari_sNumero"].ToString() + dtTarifarioFiltrado.Rows[0]["tari_vLetra"].ToString().ToUpper();
                                }
                                else
                                {
                                    Lst_Tarifario.Focus();
                                }

                                objTarifarioBE = CargarObjetoTarifario(dtTarifarioFiltrado, 0);
                                CargarListaTarifario(dtTarifarioFiltrado);


                                if (dtTarifarioFiltrado.Rows[0]["tari_sTarifarioId"].ToString() == Constantes.CONST_EXCEPCION_TARIFA_ID_122.ToString())
                                {
                                    Session["NuevoRegistro"] = true;

                                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", "Esta tarifa solo se aplica en el RUNE.", false, 190, 250);
                                    EjecutarScript(StrScript);
                                    this.Txt_TarifaDescripcion.Text = String.Empty;
                                    this.Txt_TarifaId.Text = string.Empty;
                                    return;
                                }

                                CalculoxTarifarioxTipoPagoxCantidad();

                            }
                            else
                            {
                                String strScript = String.Empty;
                                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuación", "La Tarifa Consular no es un acto notarial.");
                                EjecutarScript(strScript);
                            }
                        }
                        else
                        {
                            LimpiarDatosTarifaPago();
                            Txt_TarifaId.Text = string.Empty;

                            String strScript = String.Empty;
                            strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuación", "La Tarifa Consular no Existe.");
                            EjecutarScript(strScript);
                        }
                    }
                }

                updRegPago.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        private void CargarListaTarifario(DataTable dtTarifarioFiltrado)
        {
            if (dtTarifarioFiltrado.Rows.Count > 1)
            {
                this.Lst_Tarifario.DataSource = dtTarifarioFiltrado;
                this.Lst_Tarifario.DataTextField = "tari_vDescripcionCorta";
                this.Lst_Tarifario.DataValueField = "tari_sTarifarioId";
                this.Lst_Tarifario.DataBind();
            }
            else
            {
                this.Txt_TarifaDescripcion.Text = dtTarifarioFiltrado.Rows[0]["tari_vDescripcion"].ToString();
            }
        }

        protected void Lst_Tarifario_SelectedIndexChanged(object sender, EventArgs e)
        {
            string StrScript = string.Empty;



            lblExoneracion.Visible = false;
            ddlExoneracion.Visible = false;
            lblValExoneracion.Visible = false;
            RBNormativa.Visible = false;
            RBSustentoTipoPago.Visible = false;

            lblSustentoTipoPago.Visible = false;
            txtSustentoTipoPago.Visible = false;
            lblValSustentoTipoPago.Visible = false;


            if (Lst_Tarifario.SelectedIndex == -1)
            {
                return;
            }
            
            if (ViewState["dtTarifarioFiltrado"] != null)
            {
                dtTarifarioFiltrado = (DataTable)ViewState["dtTarifarioFiltrado"];

                LimpiarDatosTarifaPago();

                if (Lst_Tarifario.SelectedValue != Convert.ToString(Constantes.CONST_EXCEPCION_TARIFA_ID_122))
                {
                    objTarifarioBE = CargarObjetoTarifario(dtTarifarioFiltrado, Lst_Tarifario.SelectedIndex);

                    if (objTarifarioBE != null)
                    {
                        this.Txt_TarifaId.Text = objTarifarioBE.tari_sNumero + objTarifarioBE.tari_vLetra;
                    }

                    this.Txt_TarifaDescripcion.Text = Lst_Tarifario.SelectedItem.Text;
                    //ucCtrlActuacionPago.vTarifaLetra = Txt_TarifaId.Text.Trim().ToUpper();
                    Txt_TarifaId.Text = Txt_TarifaId.Text.Trim().ToUpper();

                    //ucCtrlActuacionPago.CargarTipoPagoNormaTarifario();
                    CargarTipoPagoNormaTarifario();
                    HabilitaPorTarifa(Lst_Tarifario.SelectedIndex);

                    CalculoxTarifarioxTipoPagoxCantidad(Lst_Tarifario.SelectedIndex);


                    updRegPago.Update();
                }
                else
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", "Esta tarifa solo se aplica en el RUNE.", false, 190, 250);
                    EjecutarScript(StrScript);
                    this.Txt_TarifaDescripcion.Text = String.Empty;
                }
            }

        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            BuscarTarifario();
        }
        protected void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            CalculoxTarifarioxTipoPagoxCantidad();
        }

        private BE.MRE.SI_TARIFARIO CargarObjetoTarifario(DataTable dtTarifarioFiltrado, int intIndiceSeleccionado)
        {
            try
            {
                //BE.MRE.SI_TARIFARIO objTarifarioBE = new BE.MRE.SI_TARIFARIO();
                objTarifarioBE = new BE.MRE.SI_TARIFARIO();

                objTarifarioBE.tari_sTarifarioId = Convert.ToInt16(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_sTarifarioId"]);
                objTarifarioBE.tari_sSeccionId = Convert.ToInt16(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_sSeccionId"]);
                objTarifarioBE.tari_sNumero = Convert.ToInt16(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_sNumero"]);
                objTarifarioBE.tari_vLetra = dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_vLetra"].ToString();
                objTarifarioBE.tari_FCosto = Convert.ToDouble(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_FCosto"]);
                objTarifarioBE.tari_vDescripcion = dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_vDescripcion"].ToString();
                objTarifarioBE.tari_vDescripcionCorta = dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_vDescripcionCorta"].ToString();

                objTarifarioBE.tari_sBasePercepcionId = Convert.ToInt16(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_sBasePercepcionId"]);
                objTarifarioBE.tari_sCalculoTipoId = Convert.ToInt16(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_sCalculoTipoId"]);
                objTarifarioBE.tari_vCalculoFormula = dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_vCalculoFormula"].ToString();

                objTarifarioBE.tari_sTopeUnidadId = Convert.ToInt16(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_sTopeUnidadId"]);
                objTarifarioBE.tari_ITopeCantidad = Convert.ToInt32(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_ITopeCantidad"]);

                if (dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_FMontoExceso"] != System.DBNull.Value)
                {
                    objTarifarioBE.tari_FMontoExceso = Convert.ToDouble(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_FMontoExceso"]);
                }
                else
                {
                    objTarifarioBE.tari_FMontoExceso = 0;
                }

                objTarifarioBE.tari_bTarifarioDependienteFlag = false;
                if (dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_bTarifarioDependienteFlag"] != null)
                    if (dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_bTarifarioDependienteFlag"].ToString() != string.Empty)
                        objTarifarioBE.tari_bTarifarioDependienteFlag = Convert.ToBoolean(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_bTarifarioDependienteFlag"]);

                objTarifarioBE.tari_bHabilitaCantidad = false;
                if (dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_bHabilitaCantidad"] != null)
                    if (dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_bHabilitaCantidad"].ToString() != string.Empty)
                        objTarifarioBE.tari_bHabilitaCantidad = Convert.ToBoolean(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_bHabilitaCantidad"]);

                //ViewState[strVariableTarifario] = objTarifarioBE;
                return objTarifarioBE;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        private string strVariableTarifario = "objTarifarioBE";

        private void HabilitaPorTarifa(int index = 0)
        {
            double decMontoSC = 0;

            dtTarifarioFiltrado = (DataTable)ViewState["dtTarifarioFiltrado"];
            objTarifarioBE = CargarObjetoTarifario(dtTarifarioFiltrado, index);

            //objTarifarioBE = (BE.MRE.SI_TARIFARIO)ViewState[strVariableTarifario];
            
            decMontoSC = (double)objTarifarioBE.tari_FCosto;

            hdn_tari_sTarifarioId.Value = objTarifarioBE.tari_sTarifarioId.ToString();
            hdn_tari_sNumero.Value = objTarifarioBE.tari_sNumero.ToString();
            hdn_tari_vLetra.Value = objTarifarioBE.tari_vLetra.ToString();
            hdn_tari_sCalculoTipoId.Value = objTarifarioBE.tari_sCalculoTipoId.ToString();

            //ucCtrlActuacionPago.bEnabledControl = true;
            //ucCtrlActuacionPago.vTarifaLetra = Txt_TarifaId.Text.Trim().ToUpper();
            Txt_TarifaId.Text = Txt_TarifaId.Text.Trim().ToUpper();


            if (decMontoSC == 0)
            {
                //ucCtrlActuacionPago.bEnabledControl = false;
                //ucCtrlActuacionPago.iTipoPago = Convert.ToInt32(((int)Enumerador.enmTipoCobroActuacion.GRATIS));
                ddlTipoPago.SelectedValue = ((int)Enumerador.enmTipoCobroActuacion.GRATIS).ToString();
                txtCantidad.Text = "1";
                LlenarListaExoneracion();
            }
            else
            {
                if (Txt_TarifaDescripcion.Text == string.Empty)
                {
                    ddlTipoPago.Enabled = false;
                }
                else
                {
                    bool bNoCobrado = ExisteInafecto_Exoneracion(ddlTipoPago.SelectedValue);

                    if (ddlTipoPago.SelectedValue == "0")
                    {
                        Txt_TarifaId.Focus();
                    }
                    else if (Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.GRATIS) ||
                        bNoCobrado)
                    {
                        txtMtoCancelado.Text = "0.00";
                        decMontoSC = 0;
                    }
                }
            }

            //Txt_TarifaProporcional.Text = string.Empty;
            switch (Convert.ToInt32(objTarifarioBE.tari_sCalculoTipoId))
            {
                case (int)Enumerador.enmTipoCalculoTarifario.MONTO_FIJO:
                    {
                        lblCantidad.Text = "Cantidad:";
                        break;
                    }
                case (int)Enumerador.enmTipoCalculoTarifario.PORCENTAJE:
                    {
                        /*El campo cantidad se convierte en monto directo*/
                        /*El label se cambia de texto a monto*/
                        lblCantidad.Text = "Monto:";
                        txtCantidad.MaxLength = 10;

                        //Txt_TarifaProporcional.Text = objTarifarioBE.tari_FCosto.ToString();

                        break;
                    }
                case (int)Enumerador.enmTipoCalculoTarifario.FORMULA:
                    {
                        lblCantidad.Text = "Cantidad:";
                        break;
                    }
                default:
                    break;
            }
        }

        private void CalculoxTarifarioxTipoPagoxCantidad(int index = 0)
        {
            int intCantidad = 1;
            string strScript = string.Empty;
            string strDescripcionTarifa = string.Empty;
            double decMontoSC = 0, decTotalSC = 0;
            double decMontoML = 0, decTotalML = 0;

            // Evalua habilitacion de controles:            
            dtTarifarioFiltrado = (DataTable)ViewState["dtTarifarioFiltrado"];
            objTarifarioBE = CargarObjetoTarifario(dtTarifarioFiltrado, index);

            if (objTarifarioBE == null)
            {
                return;
            }
            else
            {
//                BE.MRE.SI_TARIFARIO objTarifarioBE = (BE.MRE.SI_TARIFARIO)ViewState[strVariableTarifario];
                if (objTarifarioBE.tari_sNumero == 0)
                {
                    return;
                }

                if (string.IsNullOrEmpty(txtCantidad.Text))
                {
                    return;
                }

                decMontoSC = (double)objTarifarioBE.tari_FCosto;
                HabilitaPorTarifa(index);

                if (!string.IsNullOrEmpty(txtCantidad.Text))
                {
                    intCantidad = Convert.ToInt32(txtCantidad.Text);
                }

                if (txtCantidad.Enabled)
                {
                    txtCantidad.Focus();
                }

                // Montos calculados:
                if (intCantidad > 0)
                {
                    decTotalSC = Tarifario.Calculo(objTarifarioBE, intCantidad);
                    decMontoML = CalculaCostoML(decMontoSC, Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]));
                    decTotalML = CalculaCostoML(decTotalSC, Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]));
                }

                // Asignando valores a los controles:
                txtCantidad.Text = intCantidad.ToString();
                string strFormato = ConfigurationManager.AppSettings["FormatoMonto"].ToString();

                if (objTarifarioBE.tari_sCalculoTipoId == (int)Enumerador.enmTipoCalculoTarifario.PORCENTAJE)
                {
                    Txt_MtoSC.Text = decTotalSC.ToString(strFormato);
                    Txt_MontML.Text = decTotalML.ToString(strFormato);

                    Txt_TotSC.Text = decTotalSC.ToString(strFormato);
                    Txt_TotML.Text = decTotalML.ToString(strFormato);
                }
                else
                {
                    Txt_MtoSC.Text = decMontoSC.ToString(strFormato);
                    Txt_MontML.Text = decMontoML.ToString(strFormato);

                    Txt_TotSC.Text = decTotalSC.ToString(strFormato);
                    Txt_TotML.Text = decTotalML.ToString(strFormato);
                }

                updRegPago.Update();
            }
        }

        private double CalculaCostoML(double decMontoSC, double decTipoCambio)
        {
            return (decMontoSC * decTipoCambio);
        }

        private string strVarActuacionDetalleDT = "dtActuacionDetalle";

        private string strVarActuacionDetalleIndice = "intActuacionDetalleIndice";

      

       

        private DataTable CrearTablaActuacionDetalle()
        {
            DataTable DtDetActuaciones = new DataTable();
            DtDetActuaciones.Columns.Clear();

            DataColumn dcTarifarioId = DtDetActuaciones.Columns.Add("acde_sTarifarioId", typeof(int));
            dcTarifarioId.AllowDBNull = true;
            dcTarifarioId.Unique = false;

            DataColumn dcItem = DtDetActuaciones.Columns.Add("acde_sItem", typeof(int));
            dcItem.AllowDBNull = true;
            dcItem.Unique = false;

            DataColumn dcFechaRegistro = DtDetActuaciones.Columns.Add("acde_dFechaRegistro", typeof(string));
            dcFechaRegistro.AllowDBNull = true;
            dcFechaRegistro.Unique = false;

            DataColumn dcRequisitoFlag = DtDetActuaciones.Columns.Add("acde_bRequisitosFlag", typeof(bool));
            dcRequisitoFlag.AllowDBNull = true;
            dcRequisitoFlag.Unique = false;

            DataColumn dcActuacionCorrelativo = DtDetActuaciones.Columns.Add("acde_ICorrelativoActuacion", typeof(long));
            dcActuacionCorrelativo.AllowDBNull = true;
            dcActuacionCorrelativo.Unique = false;

            DataColumn dcTarifarioCorrelativo = DtDetActuaciones.Columns.Add("acde_ICorrelativoTarifario", typeof(long));
            dcTarifarioCorrelativo.AllowDBNull = true;
            dcTarifarioCorrelativo.Unique = false;

            DataColumn dcImpresionFlag = DtDetActuaciones.Columns.Add("acde_bImpresionFlag", typeof(bool));
            dcImpresionFlag.AllowDBNull = true;
            dcImpresionFlag.Unique = false;

            DataColumn dcImpresionFecha = DtDetActuaciones.Columns.Add("acde_dImpresionFecha", typeof(DateTime));
            dcImpresionFecha.AllowDBNull = true;
            dcImpresionFecha.Unique = false;

            DataColumn dcImpresionFuncionario = DtDetActuaciones.Columns.Add("acde_sImpresionFuncionarioId", typeof(int));
            dcImpresionFuncionario.AllowDBNull = true;
            dcImpresionFuncionario.Unique = false;

            DataColumn dcNotas = DtDetActuaciones.Columns.Add("acde_vNotas", typeof(string));
            dcNotas.AllowDBNull = true;
            dcNotas.Unique = false;

            DataColumn dcActuacionDetalleId = DtDetActuaciones.Columns.Add("acde_iActuacionDetalleId", typeof(long));
            DataColumn dcTarifa = DtDetActuaciones.Columns.Add("tari_vNumero", typeof(string));
            DataColumn dcTarifaDescripcion = DtDetActuaciones.Columns.Add("tari_vDescripcionCorta", typeof(string));
            DataColumn dcCosto = DtDetActuaciones.Columns.Add("tari_FCosto", typeof(double));

            DataColumn dcFechaOperacion = DtDetActuaciones.Columns.Add("pago_dFechaOperacion", typeof(DateTime));
            DataColumn dcMonedaLocal = DtDetActuaciones.Columns.Add("pago_sMonedaLocalId", typeof(int));
            DataColumn dcBanco = DtDetActuaciones.Columns.Add("pago_sBancoId", typeof(int));
            DataColumn dcBancoNumeroOperacion = DtDetActuaciones.Columns.Add("pago_vBancoNumeroOperacion", typeof(string));

            DataColumn dcMontoMonedaLocal = DtDetActuaciones.Columns.Add("pago_FMontoMonedaLocal", typeof(double));
            DataColumn dcMontoSolesConsulares = DtDetActuaciones.Columns.Add("pago_FMontoSolesConsulares", typeof(double));
            DataColumn dcTipoCambioBancario = DtDetActuaciones.Columns.Add("pago_FTipCambioBancario", typeof(double));
            DataColumn dcTipoCambioConsular = DtDetActuaciones.Columns.Add("pago_FTipCambioConsular", typeof(double));
            DataColumn dcPagoTipo = DtDetActuaciones.Columns.Add("pago_sPagoTipoId", typeof(int));
            DataColumn dcPagoSustentoTipoPago = DtDetActuaciones.Columns.Add("pago_vSustentoTipoPago", typeof(string));
            DataColumn dcPagoNormaTarifarioId = DtDetActuaciones.Columns.Add("pago_iNormaTarifarioId", typeof(double));


            DataColumn dcTarifaCalculo = DtDetActuaciones.Columns.Add("tari_sCalculoTipoId", typeof(int));
            DataColumn dcTarifaNumero = DtDetActuaciones.Columns.Add("tari_sNumero", typeof(int));
            DataColumn dcTarifaLetra = DtDetActuaciones.Columns.Add("tari_vLetra", typeof(string));
            DataColumn dcTarifaProporcion = DtDetActuaciones.Columns.Add("tari_vProporcion", typeof(string));
            DataColumn dcTarifaCantidad = DtDetActuaciones.Columns.Add("tari_fCantidad", typeof(double));

            DataColumn insu_vCodigoUnicoFabrica = DtDetActuaciones.Columns.Add("insu_vCodigoUnicoFabrica", typeof(string));

            return DtDetActuaciones;
        }

        protected void Txt_TarifaCantidad_TextChanged(object sender, EventArgs e)
        {
            CalculoxTarifarioxTipoPagoxCantidad();
        }

        protected void Btn_Refresh_Click(object sender, EventArgs e)
        {

        }

        private DataTable CrearTablaInsertos(List<RE_ACTONOTARIALDOCUMENTO> documentos)
        {
            ParametroConsultasBL lParametroConsultasBL = new ParametroConsultasBL();
            SI_PARAMETRO lParametro = new SI_PARAMETRO();

            #region creando datatable
            DataTable dt = new DataTable();
            dt.Columns.Add("ando_iActoNotarialId", typeof(Int16));
            dt.Columns.Add("ando_iActoNotarialDocumentoId", typeof(Int16));
            dt.Columns.Add("ando_sTipoInformacionId_desc", typeof(string));
            dt.Columns.Add("ando_sSubTipoInformacionId_desc", typeof(string));
            dt.Columns.Add("ando_vDescripcion", typeof(string));
            dt.Columns.Add("ando_vRutaArchivo", typeof(string));
            dt.Columns.Add("acno_vNumeroEscrituraPublica", typeof(string));
            #endregion

            //lParametroConsultasBL.ob
            foreach (RE_ACTONOTARIALDOCUMENTO documento in documentos)
            {
                DataRow lDataRow = dt.NewRow();
                lDataRow["ando_iActoNotarialId"] = documento.ando_iActoNotarialId;
                lDataRow["ando_iActoNotarialDocumentoId"] = documento.ando_iActoNotarialDocumentoId;
                lParametro.para_sParametroId = documento.ando_sTipoInformacionId;
                lDataRow["ando_sTipoInformacionId_desc"] = lParametroConsultasBL.Obtener(lParametro).para_vDescripcion;
                lParametro.para_sParametroId = documento.ando_sSubTipoInformacionId;
                lDataRow["ando_sSubTipoInformacionId_desc"] = lParametroConsultasBL.Obtener(lParametro).para_vDescripcion;
                lDataRow["ando_vDescripcion"] = documento.ando_vDescripcion.ToString();
                lDataRow["ando_vRutaArchivo"] = documento.ando_vRutaArchivo.ToString();
                lDataRow["acno_vNumeroEscrituraPublica"] = "Este Falta";
                dt.Rows.Add(lDataRow);
            }

            return dt;
        }

        protected void Btn_RegistrarPago_Click(object sender, EventArgs e)
        {

            if (!ValidarRegistroPago())
            { return; }


            List<BE.RE_ACTUACIONDETALLE> lstActuacionDetalle = new List<BE.RE_ACTUACIONDETALLE>();
            List<BE.RE_PAGO> lstPago = new List<BE.RE_PAGO>();

            RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();
            ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();

            lACTONOTARIAL.acno_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
            lACTONOTARIAL = lActoNotarialConsultaBL.obtener(lACTONOTARIAL);
            Int64 lngActuacionId = lACTONOTARIAL.acno_iActuacionId;

            if (lngActuacionId == 0)
            {
                return;
            }

            DataTable dtListaActuacionDetalle = new DataTable();
            dtListaActuacionDetalle = lActoNotarialConsultaBL.ListarActuacionDetalle(lngActuacionId);

            if (dtListaActuacionDetalle.Rows.Count > 0)
            {
                ViewState[Constantes.CONST_SESION_ACTUACIONDET_ID] = dtListaActuacionDetalle.Rows[0]["acde_iActuacionDetalleId"].ToString();
                //Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value] = dtListaActuacionDetalle.Rows[0]["acde_iActuacionDetalleId"].ToString();

                Session[strVariableAccion] = (int)Enumerador.enmTipoOperacion.ACTUALIZACION;
            }
            else
            {
                Session[strVariableAccion] = (int)Enumerador.enmTipoOperacion.REGISTRO;
            }

            DataRow dr;

            DataTable dtAD = new DataTable();

            dtAD = CrearTablaActuacionDetalle();

            #region Actuacion Detalle Insertar
            dr = dtAD.NewRow();

            dr["acde_iActuacionDetalleId"] = 0;
            dr["acde_sTarifarioId"] = hdn_tari_sTarifarioId.Value.ToString();
//            dr["acde_sTarifarioId"] = Lst_Tarifario.SelectedValue;
            
            dr["acde_sItem"] = 1;
            dr["acde_dFechaRegistro"] = DateTime.Now;
            dr["acde_bRequisitosFlag"] = 0;
            dr["acde_ICorrelativoActuacion"] = 0;
            dr["acde_ICorrelativoTarifario"] = 0;
            dr["acde_bImpresionFlag"] = 0;
            dr["acde_dImpresionFecha"] = DateTime.Now;
            dr["acde_vNotas"] = string.Empty;

            dr["acde_iActuacionDetalleId"] = 0;
            dr["tari_vNumero"] = hdn_tari_sNumero.Value + hdn_tari_vLetra.Value;
            dr["tari_vDescripcionCorta"] = Txt_TarifaDescripcion.Text;
            dr["tari_FCosto"] = Txt_MtoSC.Text;
            dr["pago_dFechaOperacion"] = DateTime.Now;
            dr["pago_sMonedaLocalId"] = Convert.ToInt16(Session[Constantes.CONST_SESION_TIPO_MONEDA_ID]);


            if (Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA) ||
                Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA) ||
                Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA))
            {
                if (ddlNomBanco.SelectedIndex > 0)
                    dr["pago_sBancoId"] = Convert.ToInt16(ddlNomBanco.SelectedValue);

                dr["pago_vBancoNumeroOperacion"] = txtNroOperacion.Text.Trim();
                dr["pago_dFechaOperacion"] = Comun.FormatearFecha(ctrFecPago.Text);
                if (Convert.ToDouble(txtMtoCancelado.Text) > 0)
                {
                    if (Comun.IsNumeric(txtMtoCancelado.Text))
                    {
                        if (Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA))
                        {
                            dr["pago_FMontoMonedaLocal"] = Convert.ToDouble(Txt_TotML.Text);
                            dr["pago_FMontoSolesConsulares"] = Convert.ToDouble(Txt_TotSC.Text);
                        }
                        else
                        {
                            dr["pago_FMontoMonedaLocal"] = Convert.ToDouble(Txt_MontML.Text);
                            dr["pago_FMontoSolesConsulares"] = Convert.ToDouble(Txt_MtoSC.Text);
                        }
                    }
                }
            }
            else
            {
                dr["pago_vBancoNumeroOperacion"] = "";
                dr["pago_dFechaOperacion"] = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());

                dr["pago_FMontoMonedaLocal"] = Convert.ToDouble(Txt_MontML.Text);
                dr["pago_FMontoSolesConsulares"] = Convert.ToDouble(Txt_MtoSC.Text);
            }

            if (Convert.ToInt32(hdn_tari_sCalculoTipoId.Value) == (int)Enumerador.enmTipoCalculoTarifario.PORCENTAJE)
            {
                dr["pago_FMontoMonedaLocal"] = Convert.ToDouble(Txt_TotML.Text);
                dr["pago_FMontoSolesConsulares"] = Convert.ToDouble(Txt_TotSC.Text);
            }
            else
            {
                dr["pago_FMontoMonedaLocal"] = Convert.ToDouble(Txt_MontML.Text);
                dr["pago_FMontoSolesConsulares"] = Convert.ToDouble(Txt_MtoSC.Text);
            }

            dr["pago_FTipCambioBancario"] = Session[Constantes.CONST_SESION_TIPO_CAMBIO_BANCARIO].ToString();
            dr["pago_FTipCambioConsular"] = Session[Constantes.CONST_SESION_TIPO_CAMBIO].ToString();
            dr["pago_sPagoTipoId"] = Convert.ToInt16(ddlTipoPago.SelectedValue);

            if (txtSustentoTipoPago.Visible == true)
            {
                dr["pago_vSustentoTipoPago"] = txtSustentoTipoPago.Text.Trim().ToUpper();
            }
            else
            {
                dr["pago_vSustentoTipoPago"] = "";
            }

            Int64 intNormaTarifarioId = 0;

            if (ddlExoneracion.Visible == true)
            {
                dr["pago_iNormaTarifarioId"] = Convert.ToInt64(ddlExoneracion.SelectedValue);
            }
            else
            {
                dr["pago_iNormaTarifarioId"] = intNormaTarifarioId;
            }

            bool bNoCobrado = ExisteInafecto_Exoneracion(ddlTipoPago.SelectedValue);

            if (bNoCobrado ||
                Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.NO_COBRADO) ||
                Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
            {
                dr["pago_FMontoMonedaLocal"] = 0;
                dr["pago_FMontoSolesConsulares"] = 0;
            }

            dr["tari_sCalculoTipoId"] = Convert.ToInt32(hdn_tari_sCalculoTipoId.Value);
            dr["tari_sNumero"] = Convert.ToInt32(hdn_tari_sNumero.Value);
            dr["tari_vLetra"] = Convert.ToString(hdn_tari_vLetra.Value);
            dr["tari_vProporcion"] = Txt_TarifaProporcional.Text;

            if (Convert.ToInt32(hdn_tari_sCalculoTipoId.Value) != (int)Enumerador.enmTipoCalculoTarifario.PORCENTAJE)
                dr["tari_fCantidad"] = "1";
            else
                dr["tari_fCantidad"] =txtCantidad.Text;

            dtAD.Rows.Add(dr);

            
            #endregion

           //Session[strVarActuacionDetalleDT] = dtAD;

           BE.RE_PAGO objPagoBE= new BE.RE_PAGO();

            //DataTable dt = (DataTable)Session[strVarActuacionDetalleDT];
           DataTable dt = dtAD;
            if (dt != null)
            {
                bool bCobrar = true;
                

                if (bNoCobrado || Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.NO_COBRADO) ||
                    Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
                {
                    bCobrar = false;
                }

                #region Actuacion Detalle
                BE.RE_ACTUACIONDETALLE objActuacionDetalleBE;
                
                foreach (DataRow drDetalle in dt.Rows)
                {
                    objActuacionDetalleBE = new BE.RE_ACTUACIONDETALLE();
                                                            
                    objActuacionDetalleBE.acde_iActuacionDetalleId = 0;                    
                    
                    objActuacionDetalleBE.acde_iActuacionId = lngActuacionId;
                    objActuacionDetalleBE.acde_sTarifarioId = Convert.ToInt16(drDetalle["acde_sTarifarioId"]);
                    objActuacionDetalleBE.acde_sItem = 1;
                    objActuacionDetalleBE.acde_dFechaRegistro = DateTime.Now;
                    objActuacionDetalleBE.acde_bRequisitosFlag = false;
                    objActuacionDetalleBE.acde_ICorrelativoActuacion = 0;
                    objActuacionDetalleBE.acde_ICorrelativoTarifario = 0;
                    objActuacionDetalleBE.acde_vNotas = string.Empty;
                    objActuacionDetalleBE.acde_sEstadoId = (int)Enumerador.enmActuacionEstado.REGISTRADO;
                    objActuacionDetalleBE.acde_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    objActuacionDetalleBE.acde_vIPCreacion = Util.ObtenerDireccionIP();
                    objActuacionDetalleBE.acde_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    objActuacionDetalleBE.acde_vIPModificacion = Util.ObtenerDireccionIP();
                    objActuacionDetalleBE.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    lstActuacionDetalle.Add(objActuacionDetalleBE);

                   // objPagoBE = new BE.RE_PAGO();
                    objPagoBE.pago_sPagoTipoId = Convert.ToInt16(ddlTipoPago.SelectedValue);

                    if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.ACTUALIZACION)
                    {
                        //if (HFGUID.Value.Length > 0)
                        //{
                        //    objPagoBE.pago_iActuacionDetalleId = Comun.ToNullInt64(ViewState[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);
                        //}
                        //else
                        //{
                            objPagoBE.pago_iActuacionDetalleId = Comun.ToNullInt64(ViewState[Constantes.CONST_SESION_ACTUACIONDET_ID]);
                        //}
                    }

                    if (txtSustentoTipoPago.Visible == true)
                    {
                        objPagoBE.pago_vSustentoTipoPago = txtSustentoTipoPago.Text.Trim().ToUpper();
                    }
                    else
                    {
                        objPagoBE.pago_vSustentoTipoPago = "";
                    }
                    
                    intNormaTarifarioId = 0;

                    if (ddlExoneracion.Visible == true)
                    {
                        objPagoBE.pago_iNormaTarifarioId = Convert.ToInt64(ddlExoneracion.SelectedValue);
                    }
                    else
                    {
                        objPagoBE.pago_iNormaTarifarioId = intNormaTarifarioId;
                    }


                    objPagoBE.pago_dFechaOperacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                    
                    objPagoBE.pago_sMonedaLocalId = Comun.ObtenerMonedaLocalId(Session, ddlTipoPago.SelectedValue, Txt_TarifaId.Text);

                    if (!bCobrar)
                    {
                        objPagoBE.pago_FMontoMonedaLocal = 0;
                        objPagoBE.pago_FMontoSolesConsulares = 0;
                    }
                    else
                    {
                        objPagoBE.pago_FMontoMonedaLocal = Convert.ToDouble(drDetalle["pago_FMontoMonedaLocal"]);
                        objPagoBE.pago_FMontoSolesConsulares = Convert.ToDouble(drDetalle["pago_FMontoSolesConsulares"]);
                    }

                    objPagoBE.pago_vBancoNumeroOperacion = "";

                    objPagoBE.pago_FTipCambioBancario = Convert.ToDouble(drDetalle["pago_FTipCambioBancario"]);
                    objPagoBE.pago_FTipCambioConsular = Convert.ToDouble(drDetalle["pago_FTipCambioConsular"]);

                    objPagoBE.pago_bPagadoFlag = true;
                    objPagoBE.pago_vNumeroVoucher = "";
                    objPagoBE.pago_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                    objPagoBE.pago_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    objPagoBE.pago_vIPCreacion = Util.ObtenerDireccionIP();
                    objPagoBE.pago_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    objPagoBE.pago_vIPModificacion = Util.ObtenerDireccionIP();
                    objPagoBE.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                    
                    if (Convert.ToInt16(ddlNomBanco.SelectedValue) > 0)
                    {
                        objPagoBE.pago_sBancoId = Convert.ToInt16(ddlNomBanco.SelectedValue);
                    }

                    if (Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA) ||
                        Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA) ||
                        Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA))
                    { 
                        objPagoBE.pago_dFechaOperacion =  Comun.FormatearFecha(ctrFecPago.Text);
                        objPagoBE.pago_vBancoNumeroOperacion = txtNroOperacion.Text.Trim().ToUpper();
                    }
                    if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.ACTUALIZACION)
                    {
                        int intResultado = new ActuacionPagoMantenimientoBL().Actualizar(objPagoBE, Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                    }
                    lstPago.Add(objPagoBE);
                }
                #endregion

                string strScript = string.Empty;
                try
                {

                    ActuacionMantenimientoBL objActuacionBL = new ActuacionMantenimientoBL();

                    if (Convert.ToInt32(Session[strVariableAccion]) != (int)Enumerador.enmTipoOperacion.ACTUALIZACION)
                    {
                        int intResultado = objActuacionBL.InsertarActuacionDetalle(lstActuacionDetalle, lstPago);

                        ActoNotarialMantenimiento actoNotarialMantenimiento = new ActoNotarialMantenimiento();
                        lACTONOTARIAL.acno_sEstadoId = Convert.ToInt16(Enumerador.enmNotarialProtocolarEstado.PAGADA);

                        actoNotarialMantenimiento.ActoNotarialActualizarEstado(lACTONOTARIAL);
                    }                    
                   

                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Actuación Notarial", "Información guardada satisfactoriamente.");

                    // Obtener La consulta por actuacion
                    ActoNotarialConsultaBL objActoNotarialBL = new ActoNotarialConsultaBL();
                    DataTable dtActuacionDetalle = new DataTable();
                    dtActuacionDetalle = objActoNotarialBL.ListarActuacionDetalle(lngActuacionId);

                    if (dtActuacionDetalle.Rows.Count > 0)
                    {
                        ViewState[Constantes.CONST_SESION_ACTUACIONDET_ID] = dtActuacionDetalle.Rows[0]["acde_iActuacionDetalleId"].ToString();
                        //Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value] = dtActuacionDetalle.Rows[0]["acde_iActuacionDetalleId"].ToString();
                    }

                    if (!bCobrar)
                    {
                        //Lbl_TotalGeneral.Text = "0";
                        //Lbl_TotalExtranjera.Text = "0";
                        Txt_TotSC.Text = "0.00";
                        Txt_TotML.Text = "0.00";
                    }
                    string strFormato = ConfigurationManager.AppSettings["FormatoMonto"].ToString();

                    //ucCtrlActuacionPago.vMonto = objPagoBE.pago_FMontoSolesConsulares.ToString(ConfigurationManager.AppSettings["FormatoMonto"].ToString());
                     
                     double decTotalML = Comun.CalculaCostoML(Convert.ToDouble(objPagoBE.pago_FMontoSolesConsulares.ToString()), Convert.ToDouble(objPagoBE.pago_FTipCambioConsular.ToString()));

                     txtMtoCancelado.Text = decTotalML.ToString(strFormato);

                    //Gdv_Tarifa.DataSource = dtActuacionDetalle;
                    //Gdv_Tarifa.DataBind();

                    Session["dtActuacionesRowCount"] = 0;
                    //Session[strVarActuacionDetalleDT] = dtActuacionDetalle;

                    //ctrlToolBar5.btnGrabar.Enabled = false;

                    //ucCtrlActuacionPago.bEnabledControl = false;

                    Txt_TarifaId.Enabled = false;
                    //Btn_AgregarTarifa.Enabled = false;
                    //btnLimpiarTarifa.Enabled = false;

                    Session["anep_iSelectedTabId"] = iTabVinculacionIndice;

                    if (Convert.ToInt32(Session["PasoActualTab"]) < iTabVinculacionIndice)
                    {
                        Session["PasoActualTab"] = iTabVinculacionIndice.ToString();
                        EjecutarScript("SetTabs('" + iTabVinculacionIndice.ToString() + "','" + ddlTipoActoNotarialExtra.SelectedValue.ToString() + "'); EnableTabIndex(" + iTabVinculacionIndice.ToString() + ");");
                    }
                    else
                    {
                        EjecutarScript("EnableTabIndex(" + iTabVinculacionIndice.ToString() + ");", "NavegarTabVinculacion");
                    }

                   // HabilitarBotonesImpresion(true);
                    txtAutoadhesivo.Focus();

                    //-------------------------------------------------------------
                    //Fecha: 11/04/2019
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Habilitar el botón de impresión del certificado
                    //-------------------------------------------------------------
                    int intExtraProtocolarTipoId = Convert.ToInt32(ddlTipoActoNotarialExtra.SelectedValue);
                    HabilitarVistaPrevia(intExtraProtocolarTipoId);
                    updFormato.Update();
                    //-------------------------------------------------------------
                    //btnGrabarVinculacion.Enabled = false;
                    
                    //txtAutoadhesivo.Enabled = false;
                }
                catch (Exception ex)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuación Notarial", ex.Message);
                }
                EjecutarScript(strScript, "MensajePagoActuacion");
                Lst_Tarifario.Enabled = false;
                updRegPago.Update();
            }
        }

       

       

        private void LimpiarListaTarifa()
        {
            Lst_Tarifario.DataSource = null;
            Lst_Tarifario.Items.Clear();
            Lst_Tarifario.ClearSelection();
        }

        private void LimpiarDatosTarifaPago()
        {
            Txt_TarifaProporcional.Text = string.Empty;
            txtCantidad.Text = "1";
            txtCantidad.Enabled = false;
            hdn_tope_min.Value = "0";

            string strFormato = ConfigurationManager.AppSettings["FormatoMonto"].ToString();
            double dblCero = 0;
            Txt_MontML.Text = dblCero.ToString(strFormato);
            Txt_MtoSC.Text = dblCero.ToString(strFormato);
            Txt_TotML.Text = dblCero.ToString(strFormato);
            Txt_TotSC.Text = dblCero.ToString(strFormato);

            txtNroOperacion.Text = "";
            ddlNomBanco.SelectedIndex = 0;
            txtMtoCancelado.Text = "0";
            ctrFecPago.Text = DateTime.Now.ToString(ConfigurationManager.AppSettings["FormatoFechas"]);

            if (ddlTipoPago.Items.Count > 0)
                ddlTipoPago.SelectedIndex = 0;
            //ucCtrlActuacionPago.iTipoPago = 0;
            

        }

        protected void Txt_TarifaId_TextChanged(object sender, EventArgs e)
        {
            lblExoneracion.Visible = false;
            ddlExoneracion.Visible = false;
            lblValExoneracion.Visible = false;
            RBNormativa.Visible = false;
            RBSustentoTipoPago.Visible = false;

            lblSustentoTipoPago.Visible = false;
            txtSustentoTipoPago.Visible = false;
            lblValSustentoTipoPago.Visible = false;

            txtSustentoTipoPago.Text = "";

            BuscarTarifario();
            MostrarDL173_DS076_2005RE();
            CargarTipoPagoNormaTarifario();
            
        
        }

        protected void Btn_ImprimirSupervivencia_Click(object sender, EventArgs e)
        {
            ImprimirSupervivencia();
        }

        protected void Btn_ImprimirViaje_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            ActoNotarialConsultaBL BL = new ActoNotarialConsultaBL();

            dt = (DataTable)BL.ReporteAutorizacionViaje(Convert.ToInt32(ViewState[Constantes.CONST_SESION_ACTONOTARIAL_ID]),
                Convert.ToInt32(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));

            if (dt.Rows.Count > 0)
            {
                #region Formato
                StringBuilder sScript = new StringBuilder();
                sScript.Append("<br />");
                sScript.Append("<p align=\"center\">");
                //sScript.Append("<h2><font face=\"impact\">AUTORIZACIÓN DE VIAJE DE MENOR AL EXTERIOR</font></h2>");
                sScript.Append("<h2><font face=\"impact\">AUTORIZACIÓN DE VIAJE DE MENOR</font></h2>");
                sScript.Append("</p>");

                //string strSubTipoActoExtraProtocolar = dt.Rows[0]["vSubTipoNotarialExtraProtocolar"].ToString();

                //sScript.Append("<p align=\"center\">");
                //sScript.Append("<h4>(" + strSubTipoActoExtraProtocolar + ")</h4>");  
                //sScript.Append("</p>");
                sScript.Append("<br />");
                sScript.Append("<tab></tab>");

                StringBuilder sContenido = new StringBuilder();
                sContenido.Append(ObtenerDocumentoAutorizacionMenor(dt));
                //sContenido.Replace("</p>", "====</p>"); 

                sScript.Append(sContenido);
                #endregion

                #region Firmas

                DataTable dtt = new DataTable();
                //ActoNotarialConsultaBL BL = new ActoNotarialConsultaBL();



                bool bExisteTestigoRuego = false;
                bool bExisteInterprete = false;


                foreach (DataRow Row in dt.Rows)
                {

                    if ((int)Enumerador.enmNotarialTipoParticipante.PADRE == Convert.ToUInt32(Row["sTipoParticipanteId"]))
                    {
                        if (Row["pers_vDescripcionIncapacidad"].ToString().Trim() != String.Empty)
                        {
                            bExisteTestigoRuego = true;

                        }
                    }

                    if ((int)Enumerador.enmNotarialTipoParticipante.MADRE == Convert.ToUInt32(Row["sTipoParticipanteId"]))
                    {
                        if (Row["pers_vDescripcionIncapacidad"].ToString().Trim() != String.Empty)
                        {
                            bExisteTestigoRuego = true;

                        }
                    }

                    if ((int)Enumerador.enmNotarialTipoParticipante.INTERPRETE == Convert.ToUInt32(Row["sTipoParticipanteId"]))
                    {
                        bExisteInterprete = true;
                    }
                }


                List<DocumentoFirma> listObjects = new List<DocumentoFirma>();
                DocumentoFirma objetos = new DocumentoFirma();

                //List<CBE_PARTICIPANTE> loPARTICIPANTES = (List<CBE_PARTICIPANTE>)Session["ParticipanteContainer"];

                foreach (DataRow Row in dt.Rows)
                {
                    objetos = new DocumentoFirma();
                    if ((Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.PADRE)) ||
                        (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.MADRE)) ||
                        //(Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.APODERADO)) ||
                        //(participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.ACOMPANANTE)) ||
                        (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.INTERPRETE) && bExisteInterprete) ||
                        (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO) && bExisteTestigoRuego))
                    {
                        objetos.vNombreCompleto = Row["vParticipante"].ToString();
                        objetos.vNroDocumentoCompleto = Row["vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                        objetos.bIncapacitado = Convert.ToBoolean(Row["pers_bIncapacidadFlag"]);
                        objetos.bImprimirFirma = Convert.ToBoolean(Row["anpa_bFlagFirma"]);
                        objetos.bAplicaHuellaDigital = Convert.ToBoolean(Row["anpa_bFlagHuella"]);

                        listObjects.Add(objetos);
                    }
                }
                objetos = new DocumentoFirma();



                #endregion

                List<string> listPalabrasClaves = new List<string>();
                //listPalabrasClaves.Add("AUTORIZACIÓN DE VIAJE DE MENOR AL EXTERIOR");
                listPalabrasClaves.Add("AUTORIZACIÓN DE VIAJE DE MENOR");
                listPalabrasClaves.Add("CONCLUSIÓN:");
                //listPalabrasClaves.Add("(" + strSubTipoActoExtraProtocolar + ")");


                Comun.CrearDocumentoiTextSharpExtProto(this.Page, sScript.ToString(), Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE].ToString(), Server.MapPath("~/Images/Escudo.JPG"), listObjects, true, listPalabrasClaves, "AutorizacionViaje", true, Convert.ToDouble(HttpContext.Current.Session["TamanoLetra"]));

            }
            
        }

        protected void Btn_ImprimirPoder_Click(object sender, EventArgs e)
        {
            btnVistaPrevia_Click(null, null);
        }

        protected void btn_ImprimirConformidad_Click(object sender, EventArgs e)
        {
            #region Datos de Recurrente

            string vNombreRecurrente = string.Empty;
            string vDocumento = string.Empty;


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
                    if (ViewState["ApePat"].ToString().Trim() != "&nbsp;")
                    {
                        vNombreRecurrente += " " + AplicarInicialMayuscula(ViewState["ApePat"].ToString());
                    }
                }
            }

            if (ViewState["ApeMat"] != null)
            {
                if (ViewState["ApeMat"].ToString().Trim() != string.Empty)
                {
                    if (ViewState["ApeMat"].ToString().Trim() != "&nbsp;")
                    {
                        vNombreRecurrente += " " + AplicarInicialMayuscula(ViewState["ApeMat"].ToString());
                    }
                }
            }


            if (ViewState["ApeCasada"] != null)
            {
                if (ViewState["ApeCasada"].ToString().Trim() != string.Empty)
                {
                    if (ViewState["ApeCasada"].ToString().Trim() != "&nbsp;")
                    {
                        vNombreRecurrente += " " + AplicarInicialMayuscula(ViewState["ApeCasada"].ToString());
                    }
                }
            }

            if (ViewState["DescTipDoc"] != null)
            {
                if (ViewState["DescTipDoc"].ToString().Trim() != string.Empty)
                {
                    if (ViewState["DescTipDoc"].ToString().Contains("OTROS"))
                    {
                        if (ViewState["DescTipDoc_OTRO"] != null)
                        {
                            if (ViewState["DescTipDoc_OTRO"].ToString() != string.Empty)
                            {
                                vDocumento += ViewState["DescTipDoc_OTRO"].ToString();
                            }
                        }
                    }
                    else{
                        vDocumento += ViewState["DescTipDoc"].ToString();
                    }
                    
                }
            }

            if (ViewState["NroDoc"] != null)
            {
                if (ViewState["NroDoc"].ToString().Trim() != string.Empty)
                {
                    vDocumento += " " + ViewState["NroDoc"].ToString();
                }
            }

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
            sContenido.Append(ObtenerDocumentoConformidad(vNombreRecurrente.ToUpper(), vDocumento));
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

            string strRutaPDF = uploadPath + @"\" + "ActaConformidad_" + DateTime.Now.Ticks.ToString() + ".pdf";
            strRutaHtml = uploadPath + @"\" + "ActaConformidad_" + DateTime.Now.Ticks.ToString() + ".html";

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

            CreateFilePDFConformidad(dtTMPReemplazar, strRutaHtml, strRutaPDF, Server.MapPath("~/Images/Escudo.JPG"), listObjects);
            string strScript = string.Empty;

            if (System.IO.File.Exists(strRutaPDF))
            {
                WebClient User = new WebClient();
                Byte[] FileBuffer = User.DownloadData(strRutaPDF);
                if (FileBuffer != null)
                {
                    Session["binaryData"] = FileBuffer;
                    string strUrl = "../Accesorios/VisorPDF.aspx";
                    strScript = "window.open('" + strUrl + "', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";

                    EjecutarScript(strScript);
                }
                User.Dispose();
                //-------------------------------------------------------------------------
                //Fecha: 25/01/2017
                //Autor: Miguel Angel Márquez Beltrán
                //Objetivo: Borrar el archivo temporal de Acta de Conformidad (PDF)
                //-------------------------------------------------------------------------
                if (File.Exists(strRutaPDF))
                {
                    try
                    {
                        File.Delete(strRutaPDF);
                    }
                    catch (Exception ex)
                    {
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTOS NOTARIALES - EXTRAPROTOCOLARES",
                                                "El archivo temporal de Acta de Conformidad (PDF) no se pudo eliminar." +
                                                "\n(" + ex.Message + ")");
                        Comun.EjecutarScript(Page, strScript);
                    }
                }
                //-------------------------------------------------------------------------
            }
            //-------------------------------------------------------------------------
            //Fecha: 25/01/2017
            //Autor: Miguel Angel Márquez Beltrán
            //Objetivo: Borrar el archivo temporal de Acta de Conformidad (HTML)
            //-------------------------------------------------------------------------
            if (File.Exists(strRutaHtml))
            {
                try
                {
                    File.Delete(strRutaHtml);
                }
                catch (Exception ex)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTOS NOTARIALES - EXTRAPROTOCOLARES",
                                            "El archivo temporal de Acta de Conformidad (HTML) no se pudo eliminar." +
                                            "\n(" + ex.Message + ")");
                    Comun.EjecutarScript(Page, strScript);
                }
            }
            #endregion
        }


        protected void grd_Participantes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                LimpiarTabParticipante1();
                Int32 lRowIndex = Convert.ToInt32(e.CommandArgument);


                switch (e.CommandName.ToString())
                {
                    case "Editar":

                        #region Editar
                        hEditarContestigoRuego.Value = "0";
                        Int16 sTipParticipante = Convert.ToInt16(grd_Participantes.Rows[lRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_sTipoParticipanteId")].Text);
                        string vNumeroDocumento = grd_Participantes.Rows[lRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "peid_vDocumentoNumero")].Text;
                        short sDocumentoTipoId = Int16.Parse(grd_Participantes.Rows[lRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "peid_sDocumentoTipoId")].Text);
                        hParticipanteID_Editar.Value = grd_Participantes.Rows[lRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_iActoNotarialParticipanteId")].Text;
                        Int64 ReferenciaParticipanteID = Convert.ToInt64(grd_Participantes.Rows[lRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_iReferenciaParticipanteId")].Text);
                        bool bFlagHuella = Convert.ToBoolean(grd_Participantes.Rows[lRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_bFlagHuella")].Text);
                        bool bIncapacidadFlag = Convert.ToBoolean(grd_Participantes.Rows[lRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "pers_bIncapacidadFlag")].Text);
                        hTipParticipante_Editar.Value = sTipParticipante.ToString();
                        hTipDoc_Editar.Value = sDocumentoTipoId.ToString();
                        hNumDoc_Editar.Value = vNumeroDocumento;
                        CargarComboIncapacitados();

                        #region Validar Enlace con testigo a ruego
                        if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR).ToString()
                            || ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.PODER_FUERA_REGISTRO).ToString())
                        {
                            if (sTipParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.OTORGANTE)
                                || sTipParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.PADRE)
                                || sTipParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.MADRE))
                            {
                                if (bIncapacidadFlag)
                                {
                                    foreach (GridViewRow item in grd_Participantes.Rows)
                                    {
                                        if (hParticipanteID_Editar.Value == item.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_iReferenciaParticipanteId")].Text)
                                        {
                                            //string strScriptt = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", "No puede editar porque se encuentra enlazado a un Testigo a Ruego.");
                                            //EjecutarScript(strScriptt);
                                            //return;

                                            hEditarContestigoRuego.Value = "1";
                                            break;
                                        }
                                        else
                                        {

                                            hEditarContestigoRuego.Value = "0";
                                        }
                                    }

                                }
                            }
                        }
                        #endregion

                        BuscarPersona(sDocumentoTipoId, vNumeroDocumento, sTipParticipante, bFlagHuella, ReferenciaParticipanteID);
                        if (hEditarContestigoRuego.Value == "1")
                        {
                            chkIncapacitado.Enabled = false;
                            txtRegistroTipoIncapacidad.Enabled = false;
                            chkNoHuella.Enabled = false;
                        }
                        else
                        {
                            chkIncapacitado.Enabled = true;
                            txtRegistroTipoIncapacidad.Enabled = true;
                            chkNoHuella.Enabled = true;
                        }

                        Btn_AgregarParticipante.Text = "Actualizar";

                        if (hdn_bCuerpoGrabado.Value == "1" && hdn_bCheckAceptacion.Value == "1")
                        {
                            EjecutarScript("DesactivarAceptacion();");

                            if (ddlSubTipoNotarialExtra.SelectedItem.Text == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE" ||
                                ddlSubTipoNotarialExtra.SelectedItem.Text == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A MADRE" ||
                                ddlSubTipoNotarialExtra.SelectedItem.Text == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE Y MADRE")
                            {
                                if (sTipParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.APODERADO))
                                {
                                    OtrosDatos.Visible = true;
                                    txtEscrituraPublica.Enabled = false;
                                    txtPartidaSunarp.Enabled = false;
                                }
                            }
                            return;
                        }

                        #region Llenar Campos para otorgante
                        txtNumeroEscritura.Text = grd_Participantes.Rows[lRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_vNumeroEscrituraPublica")].Text;
                        txtNumeroPartida.Text = grd_Participantes.Rows[lRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_vNumeroPartida")].Text;
                        #endregion

                        if (hEditarContestigoRuego.Value == "0")
                        {
                            Incapacitado();
                        }

                        MostrarCamposPorTipoParticipante();


                        break;

                        #endregion

                    case "Eliminar":

                        #region Eliminar

                        hParticipanteID_Editar.Value = grd_Participantes.Rows[lRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_iActoNotarialParticipanteId")].Text;

                        ActoNotarialExtraProtocolarMantenimiento _obj2 = new ActoNotarialExtraProtocolarMantenimiento();
                        RE_ACTONOTARIALPARTICIPANTE Entparticipante = LlenarEntidadParticipante(true);
                        Entparticipante.anpa_iActoNotarialParticipanteId = Convert.ToInt64(hParticipanteID_Editar.Value);
                        Entparticipante.anpa_iPersonaId = Convert.ToInt64(grd_Participantes.Rows[lRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_iPersonaId")].Text);
                        _obj2.actualizar(Entparticipante);

                        ParticipanteConsultaBL _obj3 = new ParticipanteConsultaBL();
                        DataTable _dt = new DataTable();
                        _dt = _obj3.ObtenerParticipantesExtraprotocolar(Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value));

                        #region Llena Campos Para Validar
                        if (_dt.Rows.Count > 0)
                        {
                            hExisteInterprete.Value = _dt.Rows[0]["ExisteInterprete"].ToString();
                            hExisteIdiomaExtranjero.Value = _dt.Rows[0]["ExisteIdiomaExtranjero"].ToString();

                            hExistePadre.Value = "0";
                            hExisteMadre.Value = "0";
                            hExisteAcompañante.Value = "0";

                            foreach (DataRow row in _dt.Rows)
                            {
                                if (Convert.ToInt32(row["anpa_sTipoParticipanteId"]) == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.PADRE))
                                {
                                    hExistePadre.Value = "1";
                                }
                                if (Convert.ToInt32(row["anpa_sTipoParticipanteId"]) == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.MADRE))
                                {
                                    hExisteMadre.Value = "1";
                                }
                                if (Convert.ToInt32(row["anpa_sTipoParticipanteId"]) == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.ACOMPANANTE))
                                {
                                    hExisteAcompañante.Value = "1";
                                }
                            }
                        }
                        else
                        {
                            hExisteInterprete.Value = "0";
                            hExisteIdiomaExtranjero.Value = "0";

                            hExistePadre.Value = "0";
                            hExisteMadre.Value = "0";
                            hExisteAcompañante.Value = "0";
                        }
                        #endregion
                        grd_Participantes.DataSource = _dt;
                        grd_Participantes.DataBind();
                        Btn_AgregarParticipante.Text = "Agregar";
                        break;
                        #endregion
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

        //int ObtenerIndiceComboOtorganteIncapacitado(string tipoDoc, string numeroDoc, string idParticipante = null)
        //{

        //    for (int i = 1; i < ddlOtorganteIncapacitado.Items.Count; i++)
        //    {
        //        var aux = ddlOtorganteIncapacitado.Items[i].Value.Split(',');

        //        if (aux.Count() > 0)
        //        {
        //            if (idParticipante == null)
        //            {
        //                if (aux.ElementAt(1) == tipoDoc && aux.ElementAt(2) == numeroDoc)
        //                {
        //                    return i;
        //                }
        //            }
        //            else
        //            {
        //                if (aux.ElementAt(0) == idParticipante)
        //                {
        //                    return i;
        //                }
        //            }
        //        }
        //    }
        //    return 0;
        //}

        protected void Btn_ImprimirAutoadhesivo_Click(object sender, EventArgs e)
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
                //string strUrl = "../Registro/FrmRepAutoadhesivo.aspx";
                //string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=500,height=700,left=100,top=100');";
                string sActuacionDetalle = ViewState[Constantes.CONST_SESION_ACTUACIONDET_ID].ToString();
                string sActuacionExtra = ViewState[Constantes.CONST_SESION_ACTONOTARIAL_ID].ToString();
                string sActuacion = ViewState[Constantes.CONST_SESION_ACTUACION_ID].ToString();
                string strScript = "abrirVentana('../Registro/FrmRepAutoadhesivo.aspx?iDetalleActuacion=" + sActuacionDetalle + "&ActuExtra=" + sActuacionExtra + "&Actuacion=" + sActuacion + "', 'AUTOADHESIVOS', 610, 450, '');";


                //if (Session["dtActuacionDetalle"] != null)
                //{
                //    DataTable dt = new DataTable();
                //    dt = (DataTable)Session["dtActuacionDetalle"];

                //    if (dt.Rows.Count > 0)
                //    {
                //        Session[Constantes.CONST_SESION_ACTUACIONDET_ID] = dt.Rows[0]["acde_iActuacionDetalleId"].ToString();
                //        Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value] = dt.Rows[0]["acde_iActuacionDetalleId"].ToString();
                //    }
                //}
                EjecutarScript(strScript, "1234567");


                Session["FEC_IMPRESION"] = Util.ObtenerFechaActual(
                    Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sDiferenciaHoraria")),
                    Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sHorarioVerano")));
            }
            else
            {
                Int64 intActuacionDetalleId = 0;

                //if (HFGUID.Value.Length > 0)
                //{
                //    intActuacionDetalleId = Convert.ToInt64(ViewState[Constantes.CONST_SESION_ACTUACIONDET_ID]);
                //}
                //else
                //{
                intActuacionDetalleId = Convert.ToInt64(ViewState[Constantes.CONST_SESION_ACTUACIONDET_ID]);
                //}

                DocumentoiTextSharp oDocumentoiTextSharp = new DocumentoiTextSharp(this.Page, string.Empty, HttpContext.Current.Server.MapPath("~/Images/Escudo.JPG"));
                oDocumentoiTextSharp.ActuacionDetalleId = intActuacionDetalleId;
                oDocumentoiTextSharp.CrearAutoAdhesivo();
            }

            
        }


        protected void Button3_Click(object sender, EventArgs e)
        {
            LimpiarTabParticipante1();
        }

        private void HabilitarBotonesImpresion(bool bAccion)
        {            
            txtAutoadhesivo.Enabled = bAccion;
            btnGrabarVinculacion.Enabled = bAccion;
            btnVistaPrevia.Enabled = !bAccion;
            updFormato.Update();
        }

        private void LimpiarTabParticipante1()
        {
            ddlRegistroTipoDoc.SelectedValue = "0";
            txtDocumentoNro.Text = string.Empty;
            txtDescOtroDocumento.Text = string.Empty;
            txtApePaterno.Text = string.Empty;
            txtApeMaterno.Text = string.Empty;
            txtNombres.Text = string.Empty;
            txtApepCas.Text = string.Empty;
            txtFecNac.Text = string.Empty;
            txtRegistroTipoIncapacidad.Text = string.Empty;

            //LblEdad2.Text = string.Empty;
            //LblEdad.Visible = false;
            //LblEdad2.Visible = false;
            chkIncapacitado.Checked = false;

            //BtnCalcularEdad.Visible = false;
            ddlRegistroNacionalidad.SelectedValue = "0";
            ddlRegistroGenero.SelectedValue = "0";
            //ddlPaisOrigen.SelectedValue = System.Web.Configuration.WebConfigurationManager.AppSettings["Pais_PeruId"].ToString();
            ddlPaisOrigen.SelectedIndex = 0;
            RefrescarNacionalidad();
            ddlRegistroTipoDoc.SelectedValue = "0";
            ddlRegistroEstadoCivil.SelectedValue = "0";
            ddlRegistroTipoParticipante.SelectedValue = "0";
            ddlRegistroProfesion.SelectedValue = "0";
            ddlRegistroIdioma.SelectedValue = "0";


            ddl_UbigeoPais.SelectedValue = "0";
            ddl_UbigeoRegion.SelectedValue = "0";
            ddl_UbigeoCiudad.SelectedValue = "0";

            txtDireccion.Text = string.Empty;
            txtCodigoPostal.Text = string.Empty;

            ddlRegistroTipoDoc.Style.Add("border", "solid #888888 1px");
            txtDocumentoNro.Style.Add("border", "solid #888888 1px");
            txtApePaterno.Style.Add("border", "solid #888888 1px");
            txtApeMaterno.Style.Add("border", "solid #888888 1px");
            txtNombres.Style.Add("border", "solid #888888 1px");
            ddlRegistroGenero.Style.Add("border", "solid #888888 1px");
            ddlRegistroEstadoCivil.Style.Add("border", "solid #888888 1px");
            ddlRegistroTipoParticipante.Style.Add("border", "solid #888888 1px");
            ddlRegistroProfesion.Style.Add("border", "solid #888888 1px");
            ddlRegistroIdioma.Style.Add("border", "solid #888888 1px");
            txtRegistroTipoIncapacidad.Style.Add("border", "solid #888888 1px");
            txtDireccion.Style.Add("border", "solid #888888 1px");
            ddl_UbigeoPais.Style.Add("border", "solid #888888 1px");
            ddl_UbigeoRegion.Style.Add("border", "solid #888888 1px");
            ddl_UbigeoCiudad.Style.Add("border", "solid #888888 1px");
            ((TextBox)txtFecNac.FindControl("TxtFecha")).Style.Add("border", "solid #888888 1px");

            ddlRegistroTipoDoc.Enabled = true;
            txtDocumentoNro.Enabled = true;
            txtApePaterno.Enabled = true;
            txtApeMaterno.Enabled = true;
            txtNombres.Enabled = true;
            ddlRegistroTipoDoc.Enabled = true;
            ddlRegistroEstadoCivil.Enabled = true;
            ddlRegistroGenero.Enabled = true;
            ddlRegistroTipoParticipante.Enabled = true;
            imgBuscar.Enabled = true;
            txtFecNac.Enabled = true;
            ddlRegistroNacionalidad.Enabled = true;
            ddl_UbigeoPais.Enabled = true;
            ddl_UbigeoRegion.Enabled = true;
            ddl_UbigeoCiudad.Enabled = true;
            ddlRegistroIdioma.Enabled = true;
            ddlRegistroIdioma.Enabled = true;
            ddlRegistroProfesion.Enabled = true;

            txtDireccion.Enabled = true;
            txtCodigoPostal.Enabled = true;

            LblOtroDocumento.Visible = false;
            txtDescOtroDocumento.Visible = false;
            lblDescOtroDocObl.Visible = false;

            lblValidacionParticipante1.Visible = false;

            chkNoHuella.Checked = false;
            txtNumeroEscritura.Text = "";
            txtNumeroPartida.Text = "";

            ViewState["ExtraIndiceEdicion"] = -1;

            UpdatePanelParticipante.Update();

            //Session["PersonaAlmacenada"] = null;
            //Session["vCamposPersonaAlmacenada"] = null;
        }

        private void LimpiarTabParticipantePrincipales()
        {

            ddlRegistroTipoDoc.SelectedValue = "0";
            txtDocumentoNro.Text = string.Empty;
            txtApePaterno.Text = string.Empty;
            txtApeMaterno.Text = string.Empty;
            txtNombres.Text = string.Empty;
            txtApepCas.Text = string.Empty;
            txtFecNac.Text = string.Empty;

            ddlRegistroTipoDoc.SelectedValue = "0";
            ddlRegistroEstadoCivil.SelectedValue = "0";
            ddlRegistroGenero.SelectedValue = "0";

            ddlRegistroProfesion.SelectedValue = "0";
            ddlRegistroIdioma.SelectedValue = "0";

            ddlRegistroNacionalidad.SelectedValue = "0";
            ddl_UbigeoPais.SelectedValue = "0";
            ddl_UbigeoRegion.SelectedValue = "0";
            ddl_UbigeoCiudad.SelectedValue = "0";


            txtDireccion.Text = string.Empty;
            txtCodigoPostal.Text = string.Empty;

            ddlRegistroTipoDoc.Style.Add("border", "solid #888888 1px");
            txtDocumentoNro.Style.Add("border", "solid #888888 1px");
            txtApePaterno.Style.Add("border", "solid #888888 1px");
            txtApeMaterno.Style.Add("border", "solid #888888 1px");
            txtNombres.Style.Add("border", "solid #888888 1px");
            ddlRegistroGenero.Style.Add("border", "solid #888888 1px");
            ddlRegistroEstadoCivil.Style.Add("border", "solid #888888 1px");

            ddlRegistroProfesion.Style.Add("border", "solid #888888 1px");
            ddlRegistroIdioma.Style.Add("border", "solid #888888 1px");

            txtDireccion.Style.Add("border", "solid #888888 1px");
            ddl_UbigeoPais.Style.Add("border", "solid #888888 1px");
            ddl_UbigeoRegion.Style.Add("border", "solid #888888 1px");
            ddl_UbigeoCiudad.Style.Add("border", "solid #888888 1px");
            ((TextBox)txtFecNac.FindControl("TxtFecha")).Style.Add("border", "solid #888888 1px");


            ddlRegistroTipoDoc.Enabled = true;
            txtDocumentoNro.Enabled = true;
            txtApePaterno.Enabled = true;
            txtApeMaterno.Enabled = true;
            txtNombres.Enabled = true;
            ddlRegistroTipoDoc.Enabled = true;
            ddlRegistroEstadoCivil.Enabled = true;
            ddlRegistroGenero.Enabled = true;

            imgBuscar.Enabled = true;
            txtFecNac.Enabled = true;
            ddlRegistroNacionalidad.Enabled = true;
            ddl_UbigeoPais.Enabled = true;
            ddl_UbigeoRegion.Enabled = true;
            ddl_UbigeoCiudad.Enabled = true;
            ddlRegistroIdioma.Enabled = true;

            ddlRegistroProfesion.Enabled = true;

            txtDireccion.Enabled = true;
            txtCodigoPostal.Enabled = true;

            lblValidacionParticipante1.Visible = false;

            ViewState["ExtraIndiceEdicion"] = -1;

            UpdatePanelParticipante.Update();

            //Session["PersonaAlmacenada"] = null;
            //Session["vCamposPersonaAlmacenada"] = null;

        }

        protected void btnGrabarVinculacion_Click(object sender, EventArgs e)
        {

            try
            {
                short intSeleccionado = 0;

                String strScript = String.Empty;
                if (Convert.ToInt64(hdn_acno_iActoNotarialId.Value) > 0)
                {
                    //if (chkImpresion.Checked)
                    //{
                    Int64 intActDetalleId = 0;

                    if (Session["NuevoRegistro"] != null)
                        if (!Convert.ToBoolean(Session["NuevoRegistro"]))
                            intActDetalleId = Convert.ToInt64(ViewState[Constantes.CONST_SESION_ACTUACIONDET_ID]);

                    String FormatoFecha = ConfigurationManager.AppSettings["FormatoFechas"].ToString();

                    if (FormatoFecha.Trim() == String.Empty)
                    {
                        FormatoFecha = "MMM-dd-yyyy";
                    }

                    DateTime dFecActual = Util.ObtenerFechaActual(
                                                    Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sDiferenciaHoraria")),
                                                    Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sHorarioVerano")));

                    DateTime dFecImpresion = Comun.FormatearFecha("01/01/1800");

                    long lActuacionDetalleId = 0;

                    //if (Gdv_Tarifa.Rows.Count > 0)
                    //{
                    //    lActuacionDetalleId = Convert.ToInt64(Gdv_Tarifa.Rows[intSeleccionado].Cells[Util.ObtenerIndiceColumnaGrilla(Gdv_Tarifa, "acde_iActuacionDetalleId")].Text);
                    //}
                    long lActuacionId = Convert.ToInt64(hdn_acno_iActuacionId.Value);

                    //if (HFGUID.Value.Length > 0)
                    //{
                    //    lActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);
                    //}
                    //else
                    //{
                    lActuacionDetalleId = Convert.ToInt64(ViewState[Constantes.CONST_SESION_ACTUACIONDET_ID]);
                    //}



                    string strMensaje = string.Empty;

                    ActuacionMantenimientoBL oActuacionMantenimientoBL = new ActuacionMantenimientoBL();
                    int intResultado = oActuacionMantenimientoBL.VincularAutoadhesivo(lActuacionId,
                                            lActuacionDetalleId,
                                            (int)Enumerador.enmInsumoTipo.AUTOADHESIVO,
                                            txtAutoadhesivo.Text.Trim(),
                                            dFecActual,
                                            false,
                                             dFecActual,
                                            0, // FUNCIONARIO
                                            Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                                            Convert.ToInt32(Session[Constantes.CONST_SESION_USUARIO_ID]),
                                            ref strMensaje);



                    bool bError = false;
                    if (intResultado > 0)
                    {
                        #region ActualizarEstado

                        RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();
                        ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();

                        lACTONOTARIAL.acno_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
                        lACTONOTARIAL = lActoNotarialConsultaBL.obtener(lACTONOTARIAL);


                        ActoNotarialMantenimiento actoNotarialMantenimiento = new ActoNotarialMantenimiento();
                        lACTONOTARIAL.acno_sEstadoId = Convert.ToInt16(Enumerador.enmNotarialProtocolarEstado.VINCULADA);

                        bError = actoNotarialMantenimiento.ActoNotarialActualizarEstado(lACTONOTARIAL);

                        #endregion
                    }

                    if (strMensaje == string.Empty && !bError)
                    {
                        DataTable dtActuacionInsumo = new DataTable();

                        SGAC.Registro.Actuacion.BL.ActuacionMantenimientoBL objActuacionMantenimientoBL = new SGAC.Registro.Actuacion.BL.ActuacionMantenimientoBL();
                        int IntTotalCount = 0;
                        int IntTotalPages = 0;
                        int intPaginaCantidad = Constantes.CONST_PAGE_SIZE_ADJUNTOS;
                        int PaginaActual = 1;

                        dtActuacionInsumo = objActuacionMantenimientoBL.Obtener_ActuacionInsumo(lActuacionId, PaginaActual, intPaginaCantidad, ref IntTotalCount, ref  IntTotalPages);

                        if (dtActuacionInsumo != null)
                        {
                            if (dtActuacionInsumo.Rows.Count > 0)
                            {
                                Session[Constantes.CONST_ACTUACION_INSUMO_DETALLE_ID + HFGUID.Value] = dtActuacionInsumo.Rows[0]["aide_iActuacionInsumoDetalleId"].ToString();
                                hCodAutoadhesivo.Value = dtActuacionInsumo.Rows[0]["insu_iInsumoId"].ToString();
                                btnGrabarVinculacion.Enabled = false;
                                txtAutoadhesivo.Enabled = false;
                            }
                            else
                            {
                                Session[Constantes.CONST_ACTUACION_INSUMO_DETALLE_ID + HFGUID.Value] = "";
                                btnGrabarVinculacion.Enabled = true;
                                txtAutoadhesivo.Enabled = true;
                            }
                        }


                        //Jonatan -- 20/07/2017 
                        if (txtAutoadhesivo.Text.Length > 0)
                        {
                            ctrlBajaAutoadhesivo1.Activar = true;
                        }
                        else
                        {
                            ctrlBajaAutoadhesivo1.Activar = false;
                        }
                        Session["COD_AUTOADHESIVO"] = txtAutoadhesivo.Text.Trim();
                        if (txtAutoadhesivo.Text.Trim() != string.Empty)
                        {
                            HabilitarBotonesImpresion(false);
                        }
                        btnVistaPrevia.Enabled = true;
                        Btn_ImprimirAutoadhesivo.Enabled = true;
                        strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION,
                            "VINCULACIÓN", "La vinculación se realizó correctamente.", false, 200, 300);

                        EjecutarScript(strScript, "Vinculación Correcta");

                        //  Btn_ImprimirAutoadhesivo.Enabled = false;

                        Session.Remove("IndiceActuacionDetalleSeleccionado");

                        Session["anep_iSelectedTabId"] = iTabDigitalizacionIndice;

                        if (Convert.ToInt32(Session["PasoActualTab"]) < iTabDigitalizacionIndice)
                        {
                            Session["PasoActualTab"] = iTabVinculacionIndice.ToString();
                            EjecutarScript("SetTabs('" + iTabVinculacionIndice.ToString() + "','" + ddlTipoActoNotarialExtra.SelectedValue.ToString() + "'); EnableTabIndex(" + iTabVinculacionIndice.ToString() + ");");
                            //EjecutarScript("EnableTabIndex('" + iTabDigitalizacionIndice.ToString() + "');");
                        }
                        else
                        {
                            EjecutarScript("EnableTabIndex('" + iTabVinculacionIndice.ToString() + "');");
                        }

                        HabilitarTabParticipanteTotal(false);
                        HabilitarTabCuerpo(false);

                        ActoNotarialConsultaBL objActoNotarialBL = new ActoNotarialConsultaBL();
                        DataTable dtActuacionDetalle = new DataTable();
                        dtActuacionDetalle = objActoNotarialBL.ListarActuacionDetalle(lActuacionId);

                        //Gdv_Tarifa.DataSource = dtActuacionDetalle;
                        //Gdv_Tarifa.DataBind();
                        Session["dtActuacionesRowCount"] = dtActuacionDetalle.Rows.Count;
                        ctrlToolBar5.btnGrabar.Enabled = true;
                        //ucCtrlActuacionPago.bEnabledControl = true;
                        updRegPago.Update();

                    }
                    else
                    {
                        strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "VINCULACIÓN", strMensaje, false, 200, 400);
                        EjecutarScript(strScript, "ErrorVinculación");
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
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "VINCULACIÓN", "Verifique los datos del Acto Extraprotoclar.", false, 200, 400);
                    Comun.EjecutarScript(Page, strScript);
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

        private void EjecutarScript(string script, string uniqueId)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), uniqueId, script, true);
        }

        private void EjecutarScript(string script)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPopUp" + DateTime.Now.Ticks.ToString(), script, true);
        }

        private void GrabarTipoActo(bool bModificaTipoActo = false)
        {
            OcultarTodoLosCamposParticipantes();
            if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.PODER_FUERA_REGISTRO).ToString()
                || ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR).ToString()
                || ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.CERTIFICADO_SUPERVIVENCIA).ToString())
            {
                ocultar.Visible = true;
            }
            else
            {
                ocultar.Visible = false;
            }

            if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.CERTIFICADO_SUPERVIVENCIA).ToString())
            {
                chkImprimirFirmaTitular1.Attributes.Add("style", "display:block;");
            }
            else
            {
                chkImprimirFirmaTitular1.Attributes.Add("style", "display:none;");
            }

            if (hdn_actu_iPersonaRecurrenteId.Value == "0")
            {
                EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuación", "Error desde actuación."));
                return;
            }

            RE_ACTONOTARIAL lactonotarial = new RE_ACTONOTARIAL();
            #region Creando objeto
            lactonotarial.acno_iActoNotarialId = Convert.ToInt32(hdn_acno_iActoNotarialId.Value);
            lactonotarial.acno_iActuacionId = Convert.ToInt32(hdn_acno_iActuacionId.Value);
            lactonotarial.acno_sOficinaConsularId = Convert.ToInt16(hdn_acno_sOficinaConsularId.Value);

            //Cuando es EDICIÓN 
            if ((lactonotarial.acno_iActoNotarialId != 0) || (lactonotarial.acno_iActuacionId != 0))
            {
                ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();
                lactonotarial = lActoNotarialConsultaBL.obtener(lactonotarial);
            }
            //////

            lactonotarial.acno_IFuncionarioAutorizadorId = Convert.ToInt32(ddlFuncionario.SelectedValue);
            lactonotarial.acno_iActoNotarialReferenciaId = Convert.ToInt64(hdn_acno_iActoNotarialReferenciaId.Value);
            lactonotarial.acno_sTipoActoNotarialId = Convert.ToInt16(Enumerador.enmNotarialTipoActo.EXTRAPROTOCOLAR);
            lactonotarial.acno_sSubTipoActoNotarialId = Convert.ToInt16(ddlTipoActoNotarialExtra.SelectedValue);
            lactonotarial.acno_bFlagMinuta = false;
            lactonotarial.acno_vNumeroEscrituraPublica = "0";
            lactonotarial.acno_vDenominacion = "EXTRAPROTOCOLAR";
            lactonotarial.acno_sEstadoId = Convert.ToInt16((int)Enumerador.enmNotarialProtocolarEstado.REGISTRADO);

            if (ddlTipoActoNotarialExtra.SelectedItem.Text == "AUTORIZACIÓN DE VIAJE DE MENOR")
            {
                lactonotarial.acno_sSubTipoActoNotarialExtraProtocolarId = Convert.ToInt16(ddlSubTipoNotarialExtra.SelectedValue);
                lactonotarial.acno_sCondicionTipoActoNotarialId = Convert.ToInt16(ddlCondiciones.SelectedValue);
            }


            //Log : Inserción
            lactonotarial.acno_dFechaCreacion = DateTime.Now;
            lactonotarial.acno_sUsuarioCreacion = Convert.ToInt16(hdn_acno_sUsuarioCreacion.Value);
            lactonotarial.acno_vIPCreacion = Convert.ToString(hdn_acno_vIPCreacion.Value);

            //Log : Modificación
            lactonotarial.acno_dFechaModificacion = DateTime.Now;
            lactonotarial.acno_sUsuarioModificacion = Convert.ToInt16(hdn_acno_sUsuarioModificacion.Value);
            lactonotarial.acno_vIPModificacion = Convert.ToString(hdn_acno_vIPModificacion.Value);
            #endregion

            RE_ACTUACION lACTUACION = new RE_ACTUACION();
            #region ACTUACION
            lACTUACION.actu_iPersonaRecurrenteId = Convert.ToInt64(hdn_actu_iPersonaRecurrenteId.Value);
            lACTUACION.actu_sOficinaConsularId = lactonotarial.acno_sOficinaConsularId;
            lACTUACION.actu_dFechaRegistro = lactonotarial.acno_dFechaCreacion;
            lACTUACION.actu_sUsuarioCreacion = lactonotarial.acno_sUsuarioCreacion;
            lACTUACION.actu_sEstado = (int)Enumerador.enmActuacionEstado.REGISTRADO;
            lACTUACION.actu_vIPCreacion = lactonotarial.acno_vIPCreacion;
            lACTUACION.actu_dFechaCreacion = lactonotarial.acno_dFechaCreacion;
            lACTUACION.actu_FCantidad = 1;
            if (Session[Constantes.CONST_SESION_CIUDAD_ITINERANTE].ToString() != "")
            {
                lACTUACION.actu_sCiudadItinerante = Convert.ToInt16(Session[Constantes.CONST_SESION_CIUDAD_CODIGO_ITINERANTE].ToString());
            }

            #endregion

            lactonotarial.ACTUACION = lACTUACION;

            ActoNotarialMantenimiento mnt = new ActoNotarialMantenimiento();

            long lActoNotarialId = mnt.Insertar_ActoNotarialCodeBehind(lactonotarial);

            #region Insertar: Participante Recurrente

            
            if (lactonotarial.acno_iActoNotarialId == 0)
            {
                
                BE.MRE.RE_ACTONOTARIALPARTICIPANTE participante = new RE_ACTONOTARIALPARTICIPANTE();
                participante.anpa_iActoNotarialId = lActoNotarialId;
                participante.anpa_iPersonaId = Comun.ToNullInt64(lACTUACION.actu_iPersonaRecurrenteId);
                participante.anpa_iEmpresaId = Comun.ToNullInt64(lACTUACION.actu_iEmpresaRecurrenteId);
                participante.anpa_sTipoParticipanteId = (Int16)Enumerador.enmNotarialTipoParticipante.RECURRENTE;
                participante.anpa_bFlagFirma = false; participante.anpa_bFlagHuella = false;
                participante.anpa_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                participante.anpa_sUsuarioCreacion = Convert.ToInt16(hdn_acno_sUsuarioCreacion.Value);
                participante.anpa_vIPCreacion = Convert.ToString(hdn_acno_vIPCreacion.Value);

                ActoNotarialExtraProtocolarMantenimiento objParticipanteBL = new ActoNotarialExtraProtocolarMantenimiento();
                objParticipanteBL.insertar(participante);
            }
            #endregion

            if (lActoNotarialId > 0)
            {

                hdn_acno_iActoNotarialId.Value = lActoNotarialId.ToString();
                ViewState[Constantes.CONST_SESION_ACTONOTARIAL_ID] = hdn_acno_iActoNotarialId.Value;

                if (bModificaTipoActo)
                {
                    #region Anulo lo que exista en participantes si es que hay
                    ActoNotarialExtraProtocolarMantenimiento _obj2 = new ActoNotarialExtraProtocolarMantenimiento();
                    RE_ACTONOTARIALPARTICIPANTE Entparticipante = LlenarEntidadParticipante(true);
                    if (grd_Participantes.Rows.Count > 0)
                    {
                        foreach (GridViewRow item in grd_Participantes.Rows)
                        {
                            Entparticipante.anpa_iActoNotarialParticipanteId = Convert.ToInt64(item.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_iActoNotarialParticipanteId")].Text);
                            Entparticipante.anpa_iPersonaId = Convert.ToInt64(item.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_iPersonaId")].Text);
                            _obj2.actualizar(Entparticipante);
                        }
                    }
                    #endregion
                }
                

                if (Session["PasoActualTab"] == null)
                {
                    Session["PasoActualTab"] = iTabParticipanteIndice.ToString();
                }
                else if (Convert.ToInt32(Session["PasoActualTab"]) < iTabParticipanteIndice)
                {
                    Session["PasoActualTab"] = iTabParticipanteIndice.ToString();
                }

                CargarParticipantesTipoActo((Enumerador.enmExtraprotocolarTipo)Convert.ToInt32(ddlTipoActoNotarialExtra.SelectedValue.ToString()));
                SetearTarifario();

                Session["anep_iSelectedTabId"] = iTabParticipanteIndice;


                //if ( Session["ParticipanteContainer"] == null) {


                txtObservacionesCertificador.Text = string.Empty;

                //if(Convert.ToInt32(Session["ParticipanteContainerCount"].ToString())==0){
                //Session["ParticipanteContainer"] = new List<CBE_PARTICIPANTE>();
                //Session["ParticipanteContainerCount"] = null;
                grd_Participantes.DataSource = new DataTable();
                grd_Participantes.DataBind();
                //EjecutarScript("SetTabs('" + iTabParticipanteIndice.ToString() + "','" + ddlTipoActoNotarialExtra.SelectedValue.ToString() + "'); EnableTabIndex(" + iTabParticipanteIndice.ToString() + "); HabilitarPorParticipante(false);", "TabsWea");
                EjecutarScript("SetTabs('" + iTabParticipanteIndice.ToString() + "','" + ddlTipoActoNotarialExtra.SelectedValue.ToString() + "'); EnableTabIndex(" + iTabParticipanteIndice.ToString() + "); ", "TabsWea");
                //}
                LimpiarTabParticipante1();

                EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Actuación", "Registro almacenado satisfactoriamente."));

                // MDIAZ
                //ddlViajeMenorCertificador.SelectedValue = "0";
                Session[Constantes.CONST_SESION_ACTUACION_ID] = lactonotarial.acno_iActuacionId;
                this.hdn_acno_iActuacionId.Value = lactonotarial.acno_iActuacionId.ToString();

                ViewState[Constantes.CONST_SESION_ACTUACION_ID] = lactonotarial.acno_iActuacionId;
                CargarActoNotarial();

            }
        }

        public void SetearTarifario()
        {
            trPoderFueraRegistro.Visible = false;
            trSupervivencia.Visible = false;
            trAutorizacionViajeMenor.Visible = false;
            if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.PODER_FUERA_REGISTRO).ToString())
            {
                trPoderFueraRegistro.Visible = true;
                Txt_TarifaId.Text = "15";

            }
            else if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.CERTIFICADO_SUPERVIVENCIA).ToString())
            {
                trSupervivencia.Visible = true;
                Txt_TarifaId.Text = Constantes.CONST_TARIFA_EXTRA_SUPERVIVENCIA;
            }
            else if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR).ToString())
            {
                trAutorizacionViajeMenor.Visible = true;
                Txt_TarifaId.Text = Constantes.CONST_TARIFA_EXTRA_AUTORIZACION;
            }

            ImageButton1_Click(null, null);
        }

        private void HabilitarTabParticipanteTotal(bool bAction)
        {
            imgBuscar.Enabled = bAction;
            ctrlToolBarParticipante.btnGrabar.Enabled = false;
            ctrlToolBarParticipante.btnCancelar.Enabled = false;

            txtRegistroTipoIncapacidad.Enabled = bAction;
            chkIncapacitado.Enabled = bAction;

            ddlRegistroTipoDoc.Enabled = bAction;
            txtDocumentoNro.Enabled = bAction;
            //txtApePaterno.Enabled = bAction;
            //txtApeMaterno.Enabled = bAction;
            //txtNombres.Enabled = bAction;

            //ddlRegistroGenero.Enabled = bAction;
            //ddlRegistroEstadoCivil.Enabled = bAction;
            //ddlRegistroNacionalidad.Enabled = bAction;

            //txtFecNac.Enabled = bAction;
            //txtDireccion.Enabled = bAction;
            //txtCodigoPostal.Enabled = bAction;
            //ddl_UbigeoPais.Enabled = bAction;
            //ddl_UbigeoRegion.Enabled = bAction;
            //ddl_UbigeoCiudad.Enabled = bAction;

            Btn_AgregarParticipante.Enabled = bAction;
            grd_Participantes.Enabled = bAction;

            ddlRegistroTipoParticipante.Enabled = bAction;
            //ddlRegistroProfesion.Enabled = bAction;
            //ddlRegistroIdioma.Enabled = bAction;

            //ddl_UbigeoPaisViajeDestino.Enabled = bAction;
            //ddl_UbigeoRegionViajeDestino.Enabled = bAction;
            //ddl_UbigeoCiudadViajeDestino.Enabled = bAction;

            //txtItinerario.Enabled = bAction;

            //ddlViajeMenorCertificador.Enabled = bAction; // MDIAZ
            txtObservacionesCertificador.Enabled = bAction;

            UpdParticipante.Update();
        }

        private void HabilitarTabParticipante(bool bAction)
        {
            ddlRegistroTipoDoc.Enabled = bAction;
            txtDocumentoNro.Enabled = bAction;
            txtApePaterno.Enabled = bAction;
            txtApeMaterno.Enabled = bAction;
            txtNombres.Enabled = bAction;
            txtRegistroTipoIncapacidad.Enabled = bAction;
            chkIncapacitado.Enabled = bAction;

            imgBuscar.Enabled = bAction;

            if (ddlRegistroGenero.SelectedIndex == 0)
                ddlRegistroGenero.Enabled = bAction;

            if (ddlRegistroEstadoCivil.SelectedIndex == 0)
                ddlRegistroEstadoCivil.Enabled = bAction;

            if (ddlRegistroNacionalidad.SelectedIndex == 0)
                ddlRegistroNacionalidad.Enabled = bAction;

            if (ddl_UbigeoPais.SelectedIndex == 0)
                ddl_UbigeoPais.Enabled = bAction;

            if (ddl_UbigeoRegion.SelectedIndex == 0)
                ddl_UbigeoRegion.Enabled = bAction;

            if (ddl_UbigeoCiudad.SelectedIndex == 0)
                ddl_UbigeoCiudad.Enabled = bAction;
        }

        private void HabilitarTabCuerpo(bool bAction)
        {
            btnGrabarCuerpo.Enabled = false;
            btnVistaPrevia.Enabled = false;
            btnGrabarCuerpoTemporal.Enabled = false;
            Session["EditorTextoEstado"] = "1";
        }

        //private short ObtenerIndiceListaParticipante(int iRowIndex)
        //{
        //    if (iRowIndex < 0)
        //        return -1;

        //    string vNumeroDocumento = grd_Participantes.Rows[iRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "peid_vDocumentoNumero")].Text;

        //    short sDocumentoTipoId = Int16.Parse(grd_Participantes.Rows[iRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "peid_sDocumentoTipoId")].Text);

        //    List<CBE_PARTICIPANTE> loParticipanteContainer = ((List<CBE_PARTICIPANTE>)Session["ParticipanteContainer"]).Where(x => x.anpa_cEstado != "E").ToList();
        //    var auxParticipanteContainerItem = loParticipanteContainer.Where(
        //        x => x.Identificacion.peid_sDocumentoTipoId == sDocumentoTipoId &&
        //         x.Identificacion.peid_vDocumentoNumero.Trim() == vNumeroDocumento.Trim());

        //    if (auxParticipanteContainerItem.Count() == 1)
        //    {
        //        return (short)loParticipanteContainer.IndexOf(auxParticipanteContainerItem.ElementAt(0));
        //    }
        //    return -1;
        //}


     

        private void CargarParticipantesTipoActo(Enumerador.enmExtraprotocolarTipo tipoActo)
        {
            DataTable dt = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_NOTARIAL_TIPO_ACTOR);

            if (tipoActo == Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR)
            {
                //var dtAux = dt.AsEnumerable().Where(x =>
                //    x["id"].ToString() == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.MADRE).ToString() ||
                //    x["id"].ToString() == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.PADRE).ToString() ||
                //    x["id"].ToString() == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.APODERADO).ToString() ||
                //    x["id"].ToString() == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.ACOMPANANTE).ToString() ||
                //    x["id"].ToString() == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.MENOR).ToString() ||
                //    x["id"].ToString() == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO).ToString() ||
                //    x["id"].ToString() == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.INTERPRETE).ToString());

                ddlRegistroTipoParticipante.Items.Clear();
                ddlRegistroTipoParticipante.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
                ddlRegistroTipoParticipante.Items.Insert(1, new ListItem("PADRE", Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.PADRE).ToString()));
                ddlRegistroTipoParticipante.Items.Insert(2, new ListItem("MADRE", Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.MADRE).ToString()));
                ddlRegistroTipoParticipante.Items.Insert(3, new ListItem("MENOR", Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.MENOR).ToString()));
                ddlRegistroTipoParticipante.Items.Insert(4, new ListItem("ACOMPAÑANTE", Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.ACOMPANANTE).ToString()));
                
                if (ddlSubTipoNotarialExtra.SelectedItem.Text == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A MADRE")
                {
                    ddlRegistroTipoParticipante.Items.Insert(5, new ListItem("APODERADO REPRESENTA A MADRE", Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.APODERADO).ToString()));
                }
                else if (ddlSubTipoNotarialExtra.SelectedItem.Text == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE")
                {
                    ddlRegistroTipoParticipante.Items.Insert(5, new ListItem("APODERADO REPRESENTA A PADRE", Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.APODERADO).ToString()));
                }
                else if (ddlSubTipoNotarialExtra.SelectedItem.Text == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE Y MADRE")
                {
                    ddlRegistroTipoParticipante.Items.Insert(5, new ListItem("APODERADO REPRESENTA A PADRE Y MADRE", Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.APODERADO).ToString()));
                }
                else {
                    ddlRegistroTipoParticipante.Items.Insert(5, new ListItem("APODERADO", Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.APODERADO).ToString()));
                }
                

                ddlRegistroTipoParticipante.Items.Insert(6, new ListItem("INTERPRETE", Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.INTERPRETE).ToString()));
                ddlRegistroTipoParticipante.Items.Insert(7, new ListItem("TESTIGO A RUEGO", Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO).ToString()));


                //Util.CargarParametroDropDownList(ddlRegistroTipoParticipante, dtAux.CopyToDataTable(), true);
                try
                {
                    this.ddlRegistroTipoDoc.Items.Remove(new System.Web.UI.WebControls.ListItem(Convert.ToString(Constantes.CONST_EXCEPCION_CUI), Convert.ToString(Constantes.CONST_EXCEPCION_CUI_ID)));
                }
                catch
                {

                }
                this.ddlRegistroTipoDoc.Items.Add(new System.Web.UI.WebControls.ListItem(Convert.ToString(Constantes.CONST_EXCEPCION_CUI), Convert.ToString(Constantes.CONST_EXCEPCION_CUI_ID)));

            }
            else if (tipoActo == Enumerador.enmExtraprotocolarTipo.PODER_FUERA_REGISTRO)
            {
                var dtAux = dt.AsEnumerable().Where(x =>
                    x["id"].ToString() == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.OTORGANTE).ToString() ||
                    x["id"].ToString() == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.APODERADO).ToString() ||
                    x["id"].ToString() == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.INTERPRETE).ToString() ||
                    x["id"].ToString() == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO).ToString());

                Util.CargarParametroDropDownList(ddlRegistroTipoParticipante, dtAux.CopyToDataTable(), true);
            }
            else if (tipoActo == Enumerador.enmExtraprotocolarTipo.CERTIFICADO_SUPERVIVENCIA)
            {
                var dtAux = dt.AsEnumerable().Where(x =>
                    x["id"].ToString() == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.TITULAR).ToString());

                Util.CargarParametroDropDownList(ddlRegistroTipoParticipante, dtAux.CopyToDataTable(), true);
            }

            if (ddlTipoActoNotarialExtra.SelectedItem != null)
            {

                if (ddlTipoActoNotarialExtra.SelectedItem.Text == "AUTORIZACIÓN DE VIAJE DE MENOR")
                {
                    EjecutarScript("SetLabelActoNotarial('" + ddlTipoActoNotarialExtra.SelectedItem.Text + " - " + ddlSubTipoNotarialExtra.SelectedItem.Text + "');");
                }
                else
                {
                    EjecutarScript("SetLabelActoNotarial('" + ddlTipoActoNotarialExtra.SelectedItem.Text + "');");
                }
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

        private void CargarFuncionarios(int sOfConsularId, int IFuncionarioId)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = funcionario.dtFuncionario(sOfConsularId, IFuncionarioId);

                ddlFuncionario.Items.Clear();
                ddlFuncionario.DataTextField = "vFuncionario";
                ddlFuncionario.DataValueField = "iFuncionarioId";
                ddlFuncionario.DataSource = dt;
                ddlFuncionario.DataBind();
                ddlFuncionario.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ValidarParticipante()
        {
            bool bValida = true;

            #region Otros
            if (ddlRegistroTipoParticipante.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.INTERPRETE).ToString())
            {
                if (ddlRegistroIdioma.SelectedIndex > 0)
                {
                    ddlRegistroIdioma.Style.Add("border", "solid #888888 1px");
                }
                else
                {
                    //ddlRegistroIdioma.Focus();
                    //Btn_AgregarParticipante.Focus();
                    ddlRegistroIdioma.Style.Add("border", "solid Red 1px");
                    bValida = false;
                }
            }
            else
            {
                ddlRegistroIdioma.Style.Add("border", "solid #888888 1px");
            }
            #endregion

            #region Dirección
            if (ddl_UbigeoPais.SelectedIndex > 0 || txtDireccion.Text.Trim() != string.Empty || txtCodigoPostal.Text.Trim() != string.Empty ||
                ddlRegistroTipoParticipante.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.APODERADO).ToString())
            {
                if (txtDireccion.Text.Trim() == string.Empty)
                {
                    txtDireccion.Style.Add("border", "solid Red 1px");
                    bValida = false;
                }
                else
                {
                    txtDireccion.Style.Add("border", "solid #888888 1px");
                }

                if (ddl_UbigeoPais.SelectedValue.ToString() == "0")
                {
                    ddl_UbigeoPais.Style.Add("border", "solid Red 1px");
                    bValida = false;
                }
                else
                {
                    ddl_UbigeoPais.Style.Add("border", "solid #888888 1px");
                }
                if (ddl_UbigeoRegion.SelectedValue.ToString() == "0")
                {
                    ddl_UbigeoRegion.Focus();
                    ddl_UbigeoRegion.Style.Add("border", "solid Red 1px");
                    bValida = false;
                }
                else
                {
                    ddl_UbigeoRegion.Style.Add("border", "solid #888888 1px");
                }
                if (ddl_UbigeoCiudad.SelectedValue.ToString() == "0")
                {
                    ddl_UbigeoCiudad.Focus();
                    ddl_UbigeoCiudad.Style.Add("border", "solid Red 1px");
                    bValida = false;
                }
                else
                {
                    ddl_UbigeoCiudad.Style.Add("border", "solid #888888 1px");
                }
            }
            else
            {
                ddl_UbigeoPais.Style.Add("border", "solid #888888 1px");
                ddl_UbigeoRegion.Style.Add("border", "solid #888888 1px");
                ddl_UbigeoCiudad.Style.Add("border", "solid #888888 1px");
                txtDireccion.Style.Add("border", "solid #888888 1px");
            }

            #endregion

            #region Datos personales

            if (ddlRegistroTipoParticipante.SelectedValue.ToString() != Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.APODERADO).ToString())
            {
                if (ddlRegistroNacionalidad.SelectedIndex > 0)
                {
                    ddlRegistroNacionalidad.Style.Add("border", "solid #888888 1px");
                }
                else
                {
                    ddlRegistroNacionalidad.Focus();
                    ddlRegistroNacionalidad.Style.Add("border", "solid Red 1px");
                    bValida = false;
                }

                if (ddlRegistroTipoParticipante.SelectedValue.ToString() != Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.MENOR).ToString() &&
                    ddlRegistroTipoParticipante.SelectedValue.ToString() != Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.ACOMPANANTE).ToString())
                {
                    if (ddlRegistroEstadoCivil.SelectedIndex > 0)
                    {
                        ddlRegistroEstadoCivil.Style.Add("border", "solid #888888 1px");
                    }
                    else
                    {
                        ddlRegistroEstadoCivil.Focus();
                        ddlRegistroEstadoCivil.Style.Add("border", "solid Red 1px");
                        bValida = false;
                    }
                }
                else
                {
                    ddlRegistroEstadoCivil.Style.Add("border", "solid #888888 1px");
                }

                if (ddlRegistroTipoParticipante.SelectedValue.ToString() != Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.ACOMPANANTE).ToString())
                {
                    if (ddlRegistroGenero.SelectedIndex > 0)
                    {
                        ddlRegistroGenero.Style.Add("border", "solid #888888 1px");
                    }
                    else
                    {
                        ddlRegistroGenero.Focus();
                        ddlRegistroGenero.Style.Add("border", "solid Red 1px");
                        bValida = false;
                    }
                }
                else
                {
                    ddlRegistroGenero.Style.Add("border", "solid #888888 1px");
                }
            }
            else
            {
                ddlRegistroNacionalidad.Style.Add("border", "solid #888888 1px");
                ddlRegistroEstadoCivil.Style.Add("border", "solid #888888 1px");
                ddlRegistroGenero.Style.Add("border", "solid #888888 1px");
            }

            if (ddlRegistroTipoParticipante.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.MENOR).ToString())
            {
                if (txtFecNac.Text == string.Empty)
                {
                    txtFecNac.Focus();
                    txtFecNac.Text = string.Empty;
                    ((TextBox)txtFecNac.FindControl("TxtFecha")).Style.Add("border", "solid Red 1px");
                    bValida = false;
                }
                else
                {
                    ((TextBox)txtFecNac.FindControl("TxtFecha")).Style.Add("border", "solid #888888 1px");
                }
            }

            if (txtNombres.Text.Trim() == string.Empty)
            {
                txtNombres.Focus();
                txtNombres.Style.Add("border", "solid Red 1px");
                bValida = false;
            }
            else
            {
                txtNombres.Style.Add("border", "solid #888888 1px");
            }

            if (Convert.ToInt32(ddlRegistroTipoDoc.SelectedValue) == (int)Enumerador.enmTipoDocumento.DNI ||
                Convert.ToInt32(ddlRegistroTipoDoc.SelectedValue) == (int)Enumerador.enmTipoDocumento.LIBRETA_MILITAR)
            {
                if (txtApeMaterno.Text.Trim() == string.Empty)
                {
                    txtApeMaterno.Focus();
                    txtApeMaterno.Style.Add("border", "solid Red 1px");
                    bValida = false;
                }
                else
                {
                    txtApeMaterno.Style.Add("border", "solid #888888 1px");
                }
            }
            else
            {
                txtApeMaterno.Style.Add("border", "solid #888888 1px");
            }

            if (txtApePaterno.Text.Trim() == string.Empty)
            {
                txtApePaterno.Focus();
                txtApePaterno.Style.Add("border", "solid Red 1px");
                bValida = false;
            }
            else
            {
                txtApePaterno.Style.Add("border", "solid #888888 1px");
            }

            if (txtDocumentoNro.Text.Trim() == string.Empty)
            {
                txtDocumentoNro.Focus();
                txtDocumentoNro.Style.Add("border", "solid Red 1px");
                bValida = false;
            }
            else
            {
                txtDocumentoNro.Style.Add("border", "solid #888888 1px");
            }

            if (ddlRegistroTipoDoc.SelectedValue.ToString() == "0")
            {
                ddlRegistroTipoDoc.Focus();
                ddlRegistroTipoDoc.Style.Add("border", "solid Red 1px");
                bValida = false;
            }
            else
            {
                ddlRegistroTipoDoc.Style.Add("border", "solid #888888 1px");
            }

            #endregion

            #region Tipo Participante

            if (ddlRegistroTipoParticipante.SelectedIndex > 0)
            {
                ddlRegistroTipoParticipante.Style.Add("border", "solid #888888 1px");
            }
            else
            {
                ddlRegistroTipoParticipante.Focus();
                ddlRegistroTipoParticipante.Style.Add("border", "solid Red 1px");
                bValida = false;
            }

            #endregion

            return bValida;
        }

        private bool ValidarParticipantesGrabar()
        {
            bool bValida = true;

            // MDIAZ

            if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR).ToString())
            {

                #region Autorizacion_Viaje_Menor
                #region validar para nuevos formatos (Derecho propio y Representacion)

                if (ddlSubTipoNotarialExtra.SelectedItem.Text == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - MADRE REPRESENTA A PADRE" ||
                    ddlSubTipoNotarialExtra.SelectedItem.Text == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - PADRE REPRESENTA A MADRE")
                {
                    if (txtEscrituraPublica.Text.Length == 0)
                    {
                        txtEscrituraPublica.Focus();
                        txtEscrituraPublica.Style.Add("border", "solid Red 1px");
                        bValida = false;
                    }
                    else if (txtPartidaSunarp.Text.Length == 0)
                    {
                        txtPartidaSunarp.Focus();
                        txtPartidaSunarp.Style.Add("border", "solid Red 1px");
                        bValida = false;
                    }
                }



                ddl_UbigeoPaisViajeDestino.Style.Add("border", "solid #888888 1px");
                ddl_UbigeoRegionViajeDestino.Style.Add("border", "solid #888888 1px");
                ddl_UbigeoCiudadViajeDestino.Style.Add("border", "solid #888888 1px");
                if (hbigeoViajeDestino.Value.Length == 0)
                {
                    ddl_UbigeoPaisViajeDestino.Focus();
                    ddl_UbigeoPaisViajeDestino.Style.Add("border", "solid Red 1px");
                    bValida = false;
                }
                else {
                    if (hbigeoViajeDestino.Value.Length < 5)
                    {
                        ddl_UbigeoRegionViajeDestino.Focus();
                        ddl_UbigeoRegionViajeDestino.Style.Add("border", "solid Red 1px");
                        bValida = false;
                    }
                    else if (hbigeoViajeDestino.Value.Length < 6)
                    {
                        ddl_UbigeoCiudadViajeDestino.Focus();
                        ddl_UbigeoCiudadViajeDestino.Style.Add("border", "solid Red 1px");
                        bValida = false;
                    }
                }
                #endregion
                //if (ddl_UbigeoCiudadViajeDestino.SelectedIndex == 0)
                //{
                //    ddl_UbigeoCiudadViajeDestino.Focus();
                //    ddl_UbigeoCiudadViajeDestino.Style.Add("border", "solid Red 1px");
                //    bValida = false;
                //}
                //else
                //{
                //    ddl_UbigeoCiudadViajeDestino.Style.Add("border", "solid #888888 1px");
                //}

                //if (ddl_UbigeoRegionViajeDestino.SelectedIndex == 0)
                //{
                //    ddl_UbigeoRegionViajeDestino.Focus();
                //    ddl_UbigeoRegionViajeDestino.Style.Add("border", "solid Red 1px");
                //    bValida = false;
                //}
                //else
                //{
                //    ddl_UbigeoRegionViajeDestino.Style.Add("border", "solid #888888 1px");
                //}

                //if (ddl_UbigeoPaisViajeDestino.SelectedIndex == 0)
                //{
                //    ddl_UbigeoPaisViajeDestino.Style.Add("border", "solid Red 1px");
                //    bValida = false;
                //}
                //else
                //{
                //    ddl_UbigeoPaisViajeDestino.Style.Add("border", "solid #888888 1px");
                //}

                if (txtItinerario.Text.Trim() == string.Empty)
                {
                    txtItinerario.Style.Add("border", "solid Red 1px");
                    bValida = false;
                }
                else
                {
                    txtItinerario.Style.Add("border", "solid #888888 1px");
                }
                #endregion
                //-----------------------------------------------------------------------------------------------
            }


            return bValida;
        }

        void CargarDatosRecurrente()
        {
            string strEtiquetaSolicitante = string.Empty;

            if (ViewState["ApePat"] != null)
            {
                strEtiquetaSolicitante += ViewState["ApePat"].ToString() + " ";
            }

            if (ViewState["ApeMat"] != null)
            {
                strEtiquetaSolicitante += ViewState["ApeMat"].ToString() + " ";
            }

            if (ViewState["ApeCasada"] != null)
            {
                if (ViewState["ApeCasada"].ToString() != "&nbsp;")
                {
                    strEtiquetaSolicitante += ViewState["ApeCasada"].ToString() + " ";
                }
            }

            if (ViewState["Nombre"] != null)
            {
                if (ViewState["Nombre"].ToString().Trim() != string.Empty)
                {
                    strEtiquetaSolicitante += ", " + ViewState["Nombre"].ToString() + " ";
                }
            }
            if (ViewState["DescTipDoc"] != null)
            {
                strEtiquetaSolicitante += "- " + ViewState["DescTipDoc"].ToString() + ": ";
            }

            if (ViewState["NroDoc"] != null)
            {
                strEtiquetaSolicitante += ViewState["NroDoc"].ToString();
            }

            lblRecurrente.Text = strEtiquetaSolicitante;
        }

        private void CreateFilePDFConformidad(DataTable TablaText, string HtmlPath, string PdfPath, string imgServerPAth, List<object[]> listFirmas, bool bAplicarCierreTexto = false)
        {
            try
            {
                if (!File.Exists(HtmlPath))
                    return;
                if (File.Exists(PdfPath))
                    File.Delete(PdfPath);

                float fMargenIzquierdaDoc = 80;
                float fMargenDerechaDoc = 80;

                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, fMargenIzquierdaDoc, fMargenDerechaDoc, 100, 80);
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

                for (int k = 0; k < objects.Count; k++)
                {
                    oIElement = (iTextSharp.text.IElement)objects[k];
                    if (objects[k].GetType().FullName == "iTextSharp.text.Paragraph")
                    {
                        oParagraph = new iTextSharp.text.Paragraph();
                        oParagraph.Alignment = ((iTextSharp.text.Paragraph)objects[k]).Alignment;

                        iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
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


                foreach (object[] firma in listFirmas)
                {
                    parrafo = new iTextSharp.text.Paragraph();
                    frase = new iTextSharp.text.Phrase();

                    if (writer.GetVerticalPosition(false) >= 220)
                    {

                        frase.Add(new iTextSharp.text.Chunk("\n\n\n\n\n\n"));
                        parrafo.Add(frase);
                        document.Add(parrafo);
                    }
                    else
                    {

                        while (writer.GetVerticalPosition(false) < 220)
                        {
                            document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n")));
                        }
                    }

                    if ((bool)firma[2])
                    {
                        iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                        cb.Rectangle(document.PageSize.Width - 160, writer.GetVerticalPosition(false) - 10, 70f, 80f);
                        cb.Stroke();

                    }

                    parrafo = new iTextSharp.text.Paragraph();
                    parrafo.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    parrafo.Font = iTextSharp.text.FontFactory.GetFont("Arial");

                    frase = new iTextSharp.text.Phrase();
                    frase.Add(new iTextSharp.text.Chunk("\n" + "                                                                                                        Huella Digital"));
                    frase.Add(new iTextSharp.text.Chunk("\n\n" + "---------------------------------------------------------------"));
                    frase.Add(new iTextSharp.text.Chunk("\n" + firma[0].ToString()));
                    frase.Add(new iTextSharp.text.Chunk("\n" + firma[1].ToString()));
                    parrafo.Add(frase);
                    document.Add(parrafo);

                }


                document.Close();
                oStreamReader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        private string ObtenerTextoTipoDocumento(string value)
        {
            foreach (ListItem item in ddlRegistroTipoDoc.Items)
            {
                if (item.Value.Trim() == value.Trim())
                {
                    return item.Text;
                }
            }

            return string.Empty;
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

        protected void BtnCalcularEdad_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (txtFecNac.Text != string.Empty)
            //    {
            //        LblEdad2.Visible = true;
            //        LblEdad.Visible = true;
            //        DateTime datFecha = new DateTime();
            //        if (!DateTime.TryParse(txtFecNac.Text, out datFecha))
            //        {
            //            datFecha = Comun.FormatearFecha(txtFecNac.Text);
            //        }
            //        LblEdad2.Text = Convert.ToString(Comun.CalcularEdad(datFecha));
            //        EjecutarScript("HabilitarPorParticipante(false);");
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
        }

        #region Digitalizacion
        protected void btnLoadArchivoDigitalizado_Click(object sender, EventArgs e)
        {
            this.Gdv_Adjunto.DataSource = CrearTablaDigitalizacionArchivo(null);
            this.Gdv_Adjunto.DataBind();

            Session.Remove("idDocumentoAdjuntoanxp");

            string strScript = string.Empty;
            strScript = @"$(function(){{
                            LimpiarTabArchivoDigitalizado();
                        }});";
            strScript = string.Format(strScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "LimpiarTabArchivoDigitalizado", strScript, true);
        }

        protected void btnCancelarTab6_Click(object sender, EventArgs e)
        {
            LimpiarCtrlAdjunto();

        }

        protected void MyUserControlUploader2Event_Click(object sender, EventArgs e)
        {

            Label lblNombreArchivoDigitalizado = (Label)ctrlUploader2.FindControl("lblNombreArchivo");
            lblNombreArchivoDigitalizado.Visible = true;
            if (lblNombreArchivoDigitalizado.Text != "") btnVisualizarDigitalizacion.Enabled = true;

            hidNomAdjFile2.Value = ctrlUploader2.FileName;
            Session["iArchivoAdjuntado"] = hidNomAdjFile2.Value;
            Session["RutaArchivoNuevo"] = lblNombreArchivoDigitalizado.Text;


            RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();
            ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();
            lACTONOTARIAL.acno_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
            lACTONOTARIAL.acno_iActuacionId = Convert.ToInt64(this.hdn_acno_iActuacionId.Value);
            lACTONOTARIAL.acno_sTipoActoNotarialId = Convert.ToInt16(Enumerador.enmNotarialTipoActo.EXTRAPROTOCOLAR);
            lACTONOTARIAL = lActoNotarialConsultaBL.obtener(lACTONOTARIAL);

            DataTable dtActuacionDetalle = new DataTable();
            Int64 lngActuacionId = lACTONOTARIAL.acno_iActuacionId;
            dtActuacionDetalle = lActoNotarialConsultaBL.ListarActuacionDetalle(lngActuacionId);

            //Gdv_Tarifa.DataSource = dtActuacionDetalle;
            //Gdv_Tarifa.DataBind();

            Session["dtActuacionesRowCount"] = dtActuacionDetalle.Rows.Count;

            //Session[strVarActuacionDetalleDT] = dtActuacionDetalle;

            if (Convert.ToInt32(Session["PasoActualTab"]) < iTabDigitalizacionIndice)
            {
                Session["PasoActualTab"] = iTabDigitalizacionIndice.ToString();
                EjecutarScript("SetTabs('" + iTabDigitalizacionIndice.ToString() + "','" + ddlTipoActoNotarialExtra.SelectedValue.ToString() + "'); EnableTabIndex(" + iTabDigitalizacionIndice.ToString() + ");");
            }
            else
            {
                EjecutarScript("EnableTabIndex('" + iTabDigitalizacionIndice.ToString() + "');");
            }

            updDigitalizacion.Update();

        }

        protected void btnVisualizarDigitalizacion_Click(object sender, EventArgs e)
        {
            string strScript = string.Empty;

            Label strNombreArchivo = (Label)ctrlUploader2.FindControl("lblNombreArchivo");
            string strArchivo = strNombreArchivo.Text;

            String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];

            //string strRuta = ConfigurationManager.AppSettings["UploadPath"].ToString() + "\\" + strArchivo;

            string strRuta = Path.Combine(uploadPath, strArchivo);

            String[] Ext = strArchivo.Split('.');
            if (Ext.Length > 0)
            {
                Session["strTipoArchivo"] = "." + Ext[1].ToString();
            }
            else
            {
                Session["strTipoArchivo"] = "." + "";
            }
            //-------------------------------------------------------------------------
            //Fecha: 24/01/2017
            //Autor: Miguel Angel Márquez beltrán
            //Objetivo: Obtener el nombre de archivo PDF para guardar en el Disco
            //-------------------------------------------------------------------------                    
            if (Ext[1].ToUpper().Equals("PDF"))
            {
                string strAnio = strArchivo.Substring(0, 4);
                string strMes = strArchivo.Substring(4, 2);
                string strDia = strArchivo.Substring(6, 2);
                String[] strFile = strArchivo.Split('_');
                Int64 iActuacionDetalleId = Convert.ToInt64(strFile[1].ToString());
                string strMision = Documento.GetMisionActuacionDetalle(iActuacionDetalleId);

                string strFilePath = Path.Combine(uploadPath, strMision, strAnio, strMes, strDia, strArchivo);

                if (File.Exists(strFilePath))
                {
                    strRuta = strFilePath;
                }
            }
            //-------------------------------------------------------------------------                    

            if (File.Exists(strRuta))
            {
                try
                {
                    string strUrl = "../Registro/FrmPreviewNotarial.aspx?Ruta=" + strArchivo;
                    strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=700,height=500,left=100,top=100');";
                    Comun.EjecutarScript(Page, strScript);
                }
                catch (Exception ex)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTOS NOTARIALES - PROTOCOLARES",
                        "El archivo se pudo abrir. Vuelva a intentarlo." +
                        "\n(" + ex.Message + ")");
                    Comun.EjecutarScript(Page, strScript);
                }
            }
        }

        protected void Gdv_Adjunto_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 lRowIndex = Convert.ToInt32(e.CommandArgument);
            List<RE_ACTONOTARIALDOCUMENTO> Archivo = new List<RE_ACTONOTARIALDOCUMENTO>();

            if (Session["DocumentoDigitalizadoContainer"] != null)
                Archivo = (List<RE_ACTONOTARIALDOCUMENTO>)Session["DocumentoDigitalizadoContainer"];
            else
            {
                EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "AJUNTOS", "No se puede procesar el archivo adjunto."));
                return;
            }


            switch (e.CommandName.ToString())
            {
                case "Visualizar":

                    Session["strTipoArchivo"] = Path.GetExtension(Archivo[lRowIndex].ando_vRutaArchivo);

                    string nuevaRuta;
                    if (Gdv_Adjunto.Rows[lRowIndex].Cells[2].Text.Contains("@"))
                    {
                        nuevaRuta = Convert.ToString(Gdv_Adjunto.Rows[lRowIndex].Cells[2].Text);
                    }
                    else
                    {
                        nuevaRuta = "";
                    }
                    //string strUrl = "../Registro/frmPreviewExtraProtocolar.aspx?Ruta=" + Convert.ToString(Gdv_Adjunto.Rows[lRowIndex].Cells[2].Text) + "?&RutaNombre=" + ctrlUploader2.RUTAFileName.ToString();
                    //string strUrl = "../Registro/FrmPreviewNotarial.aspx?Ruta=" + Convert.ToString(Gdv_Adjunto.Rows[lRowIndex].Cells[2].Text) + "?&RutaNombre=" + ctrlUploader2.RUTAFileName.ToString();
                    string strUrl = "../Registro/FrmPreviewNotarial.aspx?Ruta=" + Convert.ToString(Gdv_Adjunto.Rows[lRowIndex].Cells[2].Text) + "?&RutaNombre=" + nuevaRuta;
                    string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=700,height=500,left=100,top=100');";
                    Comun.EjecutarScript(Page, strScript);

                    break;

                case "Editar":

                    Label lblNombreArchivoDigitalizado = (Label)ctrlUploader2.FindControl("lblNombreArchivo");
                    lblNombreArchivoDigitalizado.Visible = true;


                    if (hidNomAdjFile2 == null)
                    {
                        hidNomAdjFile2.Value = Gdv_Adjunto.Rows[lRowIndex].Cells[2].Text;
                    }
                    else if (string.IsNullOrEmpty(hidNomAdjFile2.Value))
                    {
                        hidNomAdjFile2.Value = Gdv_Adjunto.Rows[lRowIndex].Cells[2].Text;
                    }

                    Session["idDocumentoAdjuntoanxp"] = Gdv_Adjunto.Rows[lRowIndex].Cells[0].Text;
                    lblNombreArchivoDigitalizado.Text = Server.HtmlDecode(Gdv_Adjunto.Rows[lRowIndex].Cells[2].Text);
                    this.Txt_AdjuntoDescripcion.Text = Server.HtmlDecode(Gdv_Adjunto.Rows[lRowIndex].Cells[3].Text.Replace("&nbsp;", string.Empty).Trim());

                    break;

                case "Eliminar":

                    if (Comun.ToNullInt64(Convert.ToInt64(Gdv_Adjunto.Rows[lRowIndex].Cells[0].Text)) <= 0)
                    {
                        var existe = ((List<RE_ACTONOTARIALDOCUMENTO>)Session["DocumentoDigitalizadoContainer"]).Find(p => p.ando_iActoNotarialDocumentoId == Comun.ToNullInt64(Convert.ToInt64(Gdv_Adjunto.Rows[lRowIndex].Cells[0].Text)));
                        ((List<RE_ACTONOTARIALDOCUMENTO>)Session["DocumentoDigitalizadoContainer"]).Remove(existe);
                    }
                    else
                        ((List<RE_ACTONOTARIALDOCUMENTO>)Session["DocumentoDigitalizadoContainer"]).Find(p => p.ando_iActoNotarialDocumentoId == Comun.ToNullInt64(Convert.ToInt64(Gdv_Adjunto.Rows[lRowIndex].Cells[0].Text))).ando_cEstado = "E";


                    Session["DocumentoDigitalizadoContainer"] = (List<RE_ACTONOTARIALDOCUMENTO>)Archivo;
                    this.Gdv_Adjunto.DataSource = CrearTablaDigitalizacionArchivo(null);
                    this.Gdv_Adjunto.DataBind();
                    break;
            }
        }

        [System.Web.Services.WebMethod]
        public static string adicionar_archivo(string archivo)
        {
            RE_ACTONOTARIALDOCUMENTO lArchivoDigitalizado = new RE_ACTONOTARIALDOCUMENTO();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var jsonObject = serializer.Deserialize<dynamic>(archivo);

            #region OBJETO ARCHIVO (CUANDO ES NUEVO)
            lArchivoDigitalizado.ando_iActoNotarialDocumentoId = 0;
            lArchivoDigitalizado.ando_iActoNotarialId = Convert.ToInt64(jsonObject[0]["ando_iActoNotarialId"]);
            lArchivoDigitalizado.ando_sTipoDocumentoId = Convert.ToInt16(jsonObject[0]["ando_sTipoDocumentoId"]);
            lArchivoDigitalizado.ando_sTipoInformacionId = Convert.ToInt16(jsonObject[0]["ando_sTipoInformacionId"]);
            lArchivoDigitalizado.ando_sSubTipoInformacionId = Convert.ToInt16(jsonObject[0]["ando_sSubTipoInformacionId"]);
            lArchivoDigitalizado.ando_vRutaArchivo = HttpContext.Current.Session["RutaArchivoNuevo"].ToString();
            lArchivoDigitalizado.ando_vDescripcion = jsonObject[0]["ando_vDescripcion"].ToString();
            #endregion

            #region ACTUALIZANDO VARIABLE DE SESSION

            List<RE_ACTONOTARIALDOCUMENTO> ParametrosContainer = new List<RE_ACTONOTARIALDOCUMENTO>();

            if (HttpContext.Current.Session["DocumentoDigitalizadoContainer"] != null)
                ParametrosContainer = (List<RE_ACTONOTARIALDOCUMENTO>)HttpContext.Current.Session["DocumentoDigitalizadoContainer"];


            if (HttpContext.Current.Session["idDocumentoAdjuntoanxp"] == null)
            {

                var existe = ParametrosContainer.FindAll(p => p.ando_iActoNotarialDocumentoId < 0);
                if (existe != null && existe.Count > 0)
                {
                    int iCantidad = (int)(existe.Count + 1);
                    iCantidad = iCantidad * -1;
                    lArchivoDigitalizado.ando_iActoNotarialDocumentoId = iCantidad;
                }
                else if (existe.Count == 0)
                {
                    lArchivoDigitalizado.ando_iActoNotarialDocumentoId = -1;
                }

                ParametrosContainer.Add(lArchivoDigitalizado);
            }
            else
            {

                foreach (var parametro in ParametrosContainer)
                {
                    if (parametro.ando_iActoNotarialDocumentoId.ToString() == HttpContext.Current.Session["idDocumentoAdjuntoanxp"].ToString())
                    {

                        parametro.ando_iActoNotarialId = lArchivoDigitalizado.ando_iActoNotarialId;
                        parametro.ando_sTipoDocumentoId = lArchivoDigitalizado.ando_sTipoDocumentoId;
                        parametro.ando_sTipoInformacionId = lArchivoDigitalizado.ando_sTipoInformacionId;
                        parametro.ando_sSubTipoInformacionId = lArchivoDigitalizado.ando_sSubTipoInformacionId;
                        parametro.ando_vRutaArchivo = lArchivoDigitalizado.ando_vRutaArchivo;
                        parametro.ando_vDescripcion = lArchivoDigitalizado.ando_vDescripcion;
                        break;
                    }
                }
            }

            HttpContext.Current.Session.Remove("iArchivoAdjuntado");
            HttpContext.Current.Session["DocumentoDigitalizadoContainer"] = (List<RE_ACTONOTARIALDOCUMENTO>)ParametrosContainer;
            #endregion

            return "Ok";
        }

        [System.Web.Services.WebMethod]
        public static string insertar_archivo(string larchivoDigitalizado)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var jsonObject = serializer.Deserialize<dynamic>(larchivoDigitalizado);

            List<RE_ACTONOTARIALDOCUMENTO> lArchivo = (List<RE_ACTONOTARIALDOCUMENTO>)HttpContext.Current.Session["DocumentoDigitalizadoContainer"];
            string ActoNotarial = "";
            foreach (RE_ACTONOTARIALDOCUMENTO obj in lArchivo)
            {
                if (obj.ando_cEstado == null)
                {
                    obj.ando_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                }
                else
                {
                    if (!obj.ando_cEstado.Equals("E"))
                    {
                        obj.ando_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                    }
                }
                //Log : Insersión
                obj.ando_dFechaCreacion = DateTime.Today;
                obj.ando_sUsuarioCreacion = Convert.ToInt16(jsonObject["ando_sUsuarioCreacion"]);
                obj.ando_vIPCreacion = Convert.ToString(jsonObject["ando_vIPCreacion"]);
                //obj.ando_iActoNotarialId = Convert.ToString(jsonObject["ando_iActoNotarialId"]);
                ActoNotarial = Convert.ToString(jsonObject["ando_iActoNotarialId"]);
                //Log : Modificación
                obj.ando_dFechaModificacion = DateTime.Today;
                obj.ando_sUsuarioModificacion = Convert.ToInt16(jsonObject["ando_sUsuarioModificacion"]);
                obj.ando_vIPModificacion = Convert.ToString(jsonObject["ando_vIPModificacion"]);
            }

            #region ActualizarEstado

            RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();
            ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();

            lACTONOTARIAL.acno_iActoNotarialId = Convert.ToInt64(ActoNotarial);
            lACTONOTARIAL = lActoNotarialConsultaBL.obtener(lACTONOTARIAL);


            ActoNotarialMantenimiento actoNotarialMantenimiento = new ActoNotarialMantenimiento();
            lACTONOTARIAL.acno_sEstadoId = Convert.ToInt16(Enumerador.enmNotarialProtocolarEstado.VINCULADA);

            actoNotarialMantenimiento.ActoNotarialActualizarEstado(lACTONOTARIAL);

            #endregion

            ActoNotarialMantenimiento mnt = new ActoNotarialMantenimiento();
            return serializer.Serialize(mnt.InsertarActoNotarialDocumento(lArchivo)).ToString();
        }

        private DataTable CrearTablaDigitalizacionArchivo(RE_ACTONOTARIALDOCUMENTO documentoDigitalizado)
        {
            List<RE_ACTONOTARIALDOCUMENTO> ParametrosContainer = (List<RE_ACTONOTARIALDOCUMENTO>)Session["DocumentoDigitalizadoContainer"];
            if (documentoDigitalizado != null)
            {
                if (documentoDigitalizado.ando_iActoNotarialDocumentoId == 0)
                {
                    ParametrosContainer.Add(documentoDigitalizado);
                }
                else
                {
                    foreach (RE_ACTONOTARIALDOCUMENTO lParticipante in ParametrosContainer.Where(p => p.ando_iActoNotarialDocumentoId == documentoDigitalizado.ando_iActoNotarialDocumentoId))
                    {

                    }
                }
            }

            Session["DocumentoDigitalizadoContainer"] = (List<RE_ACTONOTARIALDOCUMENTO>)ParametrosContainer;

            //Actualizar los datos 
            #region creando datatable
            DataTable dt = new DataTable();
            dt.Columns.Add("ando_iActoNotarialDocumentoId", typeof(string));
            dt.Columns.Add("ando_iActoNotarialId", typeof(string));
            dt.Columns.Add("ando_vRutaArchivo", typeof(string));
            dt.Columns.Add("ando_vDescripcion", typeof(string));
            #endregion

            #region pasando a datatable
            foreach (RE_ACTONOTARIALDOCUMENTO p in ParametrosContainer.Where(p => p.ando_cEstado != "E"))
            {
                DataRow lDataRow = dt.NewRow();
                lDataRow["ando_iActoNotarialDocumentoId"] = p.ando_iActoNotarialDocumentoId;
                lDataRow["ando_iActoNotarialId"] = p.ando_iActoNotarialId;
                lDataRow["ando_vRutaArchivo"] = p.ando_vRutaArchivo.ToString();
                lDataRow["ando_vDescripcion"] = p.ando_vDescripcion.ToString();
                dt.Rows.Add(lDataRow);
            }
            #endregion

            return dt;
        }

        public void CrearListDigitalizacionArchivo()
        {
            List<RE_ACTONOTARIALDOCUMENTO> lDigitalizacionArchivos = new List<RE_ACTONOTARIALDOCUMENTO>();
            Session["DocumentoDigitalizadoContainer"] = (List<RE_ACTONOTARIALDOCUMENTO>)lDigitalizacionArchivos;

            RE_ACTONOTARIALDOCUMENTO lDigitalizacionArchivo = new RE_ACTONOTARIALDOCUMENTO();
            lDigitalizacionArchivo.ando_iActoNotarialDocumentoId = 0;
            lDigitalizacionArchivo.ando_iActoNotarialId = 0;
            lDigitalizacionArchivo.ando_vRutaArchivo = "";
            lDigitalizacionArchivo.ando_vDescripcion = "";
        }

        public void InicializarGrillaDigitalizacionArchivo()
        {
            #region creando datatable
            DataTable dt = new DataTable();
            dt.Columns.Add("ando_iActoNotarialDocumentoId", typeof(string));
            dt.Columns.Add("ando_iActoNotarialId", typeof(string));
            dt.Columns.Add("ando_vRutaArchivo", typeof(string));
            dt.Columns.Add("ando_vDescripcion", typeof(string));
            dt.Columns.Add("ando_vUbicacion", typeof(string));
            #endregion

            this.Gdv_Adjunto.DataSource = dt;
            this.Gdv_Adjunto.DataBind();
        }

        protected void Gdv_Adjunto_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Label lblNombreArchivoDigitalizado = (Label)ctrlUploader2.FindControl("lblNombreArchivo");
            lblNombreArchivoDigitalizado.Visible = false;
        }

        protected void Gdv_Adjunto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            LimpiarCtrlAdjunto();
        }


        void LimpiarCtrlAdjunto()
        {
            Txt_AdjuntoDescripcion.Text = string.Empty;

            Label lblNombreArchivo = (Label)ctrlUploader2.FindControl("lblNombreArchivo");
            HtmlTableCell msjeSucess = (HtmlTableCell)ctrlUploader2.FindControl("msjeSucess");
            HtmlTableCell msjeWarning = (HtmlTableCell)ctrlUploader2.FindControl("msjeWarning");
            HtmlTableCell msjeError = (HtmlTableCell)ctrlUploader2.FindControl("msjeError");


            if (lblNombreArchivo != null)
                lblNombreArchivo.Visible = false;

            if (msjeSucess != null)
                msjeSucess.Visible = false;

            if (msjeWarning != null)
                msjeWarning.Visible = false;

            if (msjeError != null)
                msjeError.Visible = false;

            btnVisualizarDigitalizacion.Enabled = false;
        }

        protected void ddlTipoArchivoAdjunto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoArchivoAdjunto != null)
            {
                if (ddlTipoArchivoAdjunto.SelectedItem != null)
                {
                    if (ddlTipoArchivoAdjunto.SelectedItem.ToString() != "0")
                    {
                        HiddenField TipoArchivoDigitalizacion = (HiddenField)ctrlUploader2.FindControl("hd_Extension");
                        string[] tipoActuacion = ddlTipoArchivoAdjunto.SelectedItem.ToString().Split('|');
                        if (tipoActuacion.Length > 1)
                        {
                            TipoArchivoDigitalizacion.Value = tipoActuacion[1];
                        }

                    }
                }
            }
        }


        #endregion

        protected void ddlRegistroTipoDoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            ctrlToolTipoActo.btnGrabar.UseSubmitBehavior = false;
        }

        protected void btnGrabarCuerpo_Click(object sender, EventArgs e)
        {
            if (hdn_acno_iActoNotarialId.Value != "0")
            {
               /* if (Session["vDocumentoActualPFRPrevio"] == null)
                {
                    EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Cuerpo", "No se ha guardado la información del cuerpo."));
                    return;
                }*/

                //GrabarCuerpo(Convert.ToInt32(hdn_acno_iActoNotarialId.Value)); AprobacionPagoIndice
                Session["anep_iSelectedTabId"] = iTabAprobacionPagoIndice;
                if (Session["PasoActualTab"] == null)
                {
                    Session["PasoActualTab"] = 1;
                }
                if (Convert.ToInt32(Session["PasoActualTab"]) < iTabAprobacionPagoIndice)
                {
                    Session["PasoActualTab"] = iTabAprobacionPagoIndice.ToString();
                    //EjecutarScript("DesHabilitarGrabarCuerpo();SetTabs('" + iTabAprobacionPagoIndice.ToString() + "','" + ddlTipoActoNotarialExtra.SelectedValue.ToString() + "');");
                    //EjecutarScript("SetTabs('" + iTabAprobacionPagoIndice.ToString() + "','" + ddlTipoActoNotarialExtra.SelectedValue.ToString() + "'); EnableTabIndex(" + iTabAprobacionPagoIndice.ToString() + ");");
                    EjecutarScript("SetTabs('" + iTabAprobacionPagoIndice.ToString() + "','" + ddlTipoActoNotarialExtra.SelectedValue.ToString() + "'); EnableTabIndex(" + iTabAprobacionPagoIndice.ToString() + "); ", "TabsWea8");

                    //EjecutarScript("DesHabilitarGrabarCuerpo();EnableTabIndex('" + iTabAprobacionPagoIndice.ToString() + "');", ddlTipoActoNotarialExtra.SelectedValue.ToString());
                }
                else
                {
                    EjecutarScript("EnableTabIndex('" + iTabAprobacionPagoIndice.ToString() + "');");
                }
            }
            else
            {
                EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", "Se debe agregar el tipo de acto notarial."));
            }
            //updRegPago.Update();
        }

        protected void btnVistaPrevia_Click(object sender, EventArgs e)
        {
            if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.PODER_FUERA_REGISTRO).ToString())
            {
                //if (txtCuerpo.Text.Trim() == string.Empty)
                //{
                if (hdn_acno_iActoNotarialId.Value != "0")
                {
                    //if (Session["vDocumentoActualPFRPrevio"] == null)
                    //{
                    //    EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "VISTA PREVIA", "No se ha ingresado información al Cuerpo."));
                    //    return;
                    //}

                    if (txtCuerpo.Text == "")
                    {
                        EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "VISTA PREVIA", "No se ha ingresado información al Cuerpo."));
                        return;
                    }
                }
                btnGrabarCuerpoTemporal.Enabled = true;   
                
                # region Imprimir PoderFueraregistro
                StringBuilder sScript = new StringBuilder();

                sScript.Append("<br />");
                sScript.Append("<p align=\"center\">");
                sScript.Append("<h1><font face=\"impact\">PODER FUERA DE REGISTRO</font></h1>");
                sScript.Append("</p>");
                sScript.Append("<br />");

                StringBuilder strScriptAuxiliar = new StringBuilder();

                if (Session["anep_iSelectedTabId"].ToString() == iTabCuerpoIndice.ToString())

                    strScriptAuxiliar.Append(Session["vDocumentoActualPFRPrevio"].ToString());
                //strScriptAuxiliar.Append( txtCuerpo.Text);
                else
                    strScriptAuxiliar.Append(Session["vDocumentoActualPFR"].ToString());


                strScriptAuxiliar.Replace("<p>", "<p align=\"justify\" style=\"background-color: transparent;\">");
                //strScriptAuxiliar.Replace("</p>", "====</p>"); 
                sScript.Append(strScriptAuxiliar);
                if (Session["vCuerpoAuxSession"] != null)
                    txtCuerpo.Text = Session["vCuerpoAuxSession"].ToString();

                if (Session["vTextoAdicionalAuxSession"] != null)
                    txtInfoAdicional.Text = Session["vTextoAdicionalAuxSession"].ToString();

                ImprimirCuerpo(sScript.ToString());

                #endregion
            }
            else if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.CERTIFICADO_SUPERVIVENCIA).ToString())
            {
                btnGrabarCuerpoTemporal.Enabled = true;   
                ImprimirSupervivencia();
            }
            else if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR).ToString())
            {
                btnGrabarCuerpoTemporal.Enabled = true;   
                Btn_ImprimirViaje_Click(null, null);
            }

            

            UpdatePanel4.Update();
        }

        protected void ddl_UbigeoCiudadViajeDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["UbigeoViajeDestino"] != null)
            {
                ViewState.Remove("UbigeoViajeDestino");
            }
            else
            {
                ddl_UbigeoCiudadViajeDestino.Focus();
            }
            //EjecutarScript("HabilitarPorParticipante(false);HabilitarApellidoCasada();");
        }

        protected void ddl_UbigeoCiudad_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_UbigeoCiudad.Focus();
            //EjecutarScript("HabilitarPorParticipante(false);HabilitarApellidoCasada();");

        }

        protected void ddlRegistroTipoDoc_SelectedIndexChanged1(object sender, EventArgs e)
        {
            try
            {
                txtDescOtroDocumento.Text = "";

                if (ddlRegistroTipoDoc.SelectedValue == Convert.ToString((int)Enumerador.enmTipoDocumento.OTROS)
                    || ddlRegistroTipoDoc.SelectedValue == Convert.ToString((int)Enumerador.enmTipoDocumento.PASAPORTE_E))
                {
                    LblOtroDocumento.Visible = true;
                    txtDescOtroDocumento.Visible = true;
                    txtDescOtroDocumento.Enabled = true;
                    lblDescOtroDocObl.Visible = true;

                    if (ddlRegistroTipoDoc.SelectedValue == Convert.ToString((int)Enumerador.enmTipoDocumento.OTROS))
                    {
                        LblOtroDocumento.Text = "Otro documento :";
                    }
                    else
                    {
                        LblOtroDocumento.Text = "Tipo Pasaporte :";
                    }
                }
                else
                {
                    LblOtroDocumento.Visible = false;
                    txtDescOtroDocumento.Visible = false;
                    txtDescOtroDocumento.Enabled = false;
                    lblDescOtroDocObl.Visible = false;
                }
                if (ddlRegistroTipoDoc.SelectedValue != "0")
                {

                    txtDocumentoNro.Text = string.Empty;

                    DataTable dtDocumentoIdentidad = new DataTable();
                    dtDocumentoIdentidad = Comun.ObtenerListaDocumentoIdentidad();

                    //DataTable dtDocumentoIdentidad = (DataTable)Session[Constantes.CONST_SESION_DT_DOCUMENTOIDENTIDAD];


                    foreach (DataRow fila in dtDocumentoIdentidad.Rows)
                    {
                        if (fila["doid_sTipoDocumentoIdentidadId"].ToString() == ddlRegistroTipoDoc.SelectedValue.ToString())
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



                            txtDocumentoNro.MaxLength = iMaxLenght;

                            if (sNacionalidad > 0)
                            {
                                ddlRegistroNacionalidad.SelectedValue = sNacionalidad.ToString();
                                ddlRegistroNacionalidad.Enabled = false;
                            }
                            else
                            {
                                ddlRegistroNacionalidad.SelectedValue = "0";
                                ddlRegistroNacionalidad.Enabled = true;
                            }

                            break;
                        }

                    }

                }
                else
                {
                    ddlRegistroNacionalidad.SelectedValue = "0";
                    ddlRegistroNacionalidad.Enabled = true;
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

        protected void chkImpresion_CheckedChanged(object sender, EventArgs e)
        {
            if (chkImpresion.Checked)
            {
                txtAutoadhesivo.Enabled = true;
                btnGrabarVinculacion.Enabled = true;
            }
            else
            {
                txtAutoadhesivo.Enabled = false;
                btnGrabarVinculacion.Enabled = false;
            }

            updFormato.Update();
        }

        protected void btnDesabilitarAutoahesivo_Click(object sender, EventArgs e)
        {
            Btn_ImprimirAutoadhesivo.Enabled = false;
            chkImpresion.Checked = true;
            
            txtAutoadhesivo.Enabled = false;
            hnd_ImpresionCorrecta.Value = "1";

            if (Convert.ToInt32(Session["PasoActualTab"]) < iTabDigitalizacionIndice)
            {
                Session["PasoActualTab"] = iTabDigitalizacionIndice.ToString();
                EjecutarScript("SetTabs('" + iTabDigitalizacionIndice.ToString() + "','" + ddlTipoActoNotarialExtra.SelectedValue.ToString() + "'); EnableTabIndex(" + iTabDigitalizacionIndice.ToString() + ");");
                //EjecutarScript("EnableTabIndex('" + iTabDigitalizacionIndice.ToString() + "');");
            }
            else
            {
                EjecutarScript("EnableTabIndex('" + iTabDigitalizacionIndice.ToString() + "');");
            }

            updFormato.Update();
        }


        [System.Web.Services.WebMethod]
        public static string VistaPrevia(string TipoActa, string iActoNotarialId, string cuerpo, bool ImprimirFirma = true)
        {
            //ImprimirFirma es asignado por el control: chkImprimirFirmaTitular1.cheked

            string rpt = string.Empty;
            //String sIdiomaCastellano = ConfigurationManager.AppSettings["NotarialIdioma"].ToString();

            String sIdiomaCastellano = HttpContext.Current.Session["NotarialIdioma"].ToString();


            //iActoNotarialId = ViewState[Constantes.CONST_SESION_ACTONOTARIAL_ID].ToString();
            try
            {
                if (iActoNotarialId != "0")
                {

                    if (TipoActa == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.PODER_FUERA_REGISTRO).ToString())
                    {
                        #region Poder_Fuera_Registro

                        StringBuilder sScript = new StringBuilder();
                        sScript.Append("<br />");
                        sScript.Append("<p align=\"center\">");
                        sScript.Append("<h1><font face=\"impact\">PODER FUERA DE REGISTRO</font></h1>");
                        sScript.Append("</p>");
                        sScript.Append("<br />");
                        sScript.Append(cuerpo);

                        DataTable dt = new DataTable();
                        ActoNotarialConsultaBL BL = new ActoNotarialConsultaBL();
                        dt = (DataTable)BL.ReportePoderFueraRegistro(Convert.ToInt32(iActoNotarialId), Convert.ToInt32(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));

                        bool bExisteTestigoRuego = false;
                        bool bExisteInterprete = false;

                        foreach (DataRow Row in dt.Rows)
                        {
                            if ((int)Enumerador.enmNotarialTipoParticipante.OTORGANTE == Convert.ToUInt32(Row["sTipoParticipanteId"]))
                            {
                                if (Row["pers_vDescripcionIncapacidad"].ToString().Trim() != String.Empty)
                                {
                                    bExisteTestigoRuego = true;
                                }
                            }
                            if ((int)Enumerador.enmNotarialTipoParticipante.INTERPRETE == Convert.ToUInt32(Row["sTipoParticipanteId"]))
                            {
                                bExisteInterprete = true;
                            }
                        }

                        List<DocumentoFirma> listObjects = new List<DocumentoFirma>();
                        DocumentoFirma objetos = new DocumentoFirma();
                        //List<CBE_PARTICIPANTE> loPARTICIPANTES = (List<CBE_PARTICIPANTE>)HttpContext.Current.Session["ParticipanteContainer"];

                        foreach (DataRow Row in dt.Rows)
                        {
                            objetos = new DocumentoFirma();
                            if ((Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.OTORGANTE)) ||
                                (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.INTERPRETE) && bExisteInterprete) ||
                                (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO) && bExisteTestigoRuego))
                            {
                                objetos.vNombreCompleto = Row["vParticipante"].ToString();

                                if (Convert.ToInt32(Row["peid_sDocumentoTipoId"].ToString()) == (int)Enumerador.enmTipoDocumento.OTROS)
                                {
                                    objetos.vNroDocumentoCompleto = Row["peid_vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                }
                                else
                                {
                                    objetos.vNroDocumentoCompleto = Row["vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                }

                                objetos.bIncapacitado = Convert.ToBoolean(Row["pers_bIncapacidadFlag"]);
                                objetos.bImprimirFirma = Convert.ToBoolean(Row["anpa_bFlagFirma"]);

                                objetos.bAplicaHuellaDigital = Convert.ToBoolean(Row["anpa_bFlagHuella"]);

                                listObjects.Add(objetos);
                            }
                        }
                        objetos = new DocumentoFirma();
                        List<string> listPalabrasClaves = new List<string>();
                        listPalabrasClaves.Add("PODER FUERA DE REGISTRO");
                        listPalabrasClaves.Add("CONCLUSIÓN:");

                        Comun.CrearDocumentoiTextSharpTamanoLetra(null, sScript.ToString(), HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE].ToString(), HttpContext.Current.Server.MapPath("~/Images/Escudo.JPG"), listObjects, true, listPalabrasClaves, "Poder_Fuera_Registro", false, Convert.ToDouble(HttpContext.Current.Session["TamanoLetra"]));
                        rpt = "OK";
                        //ImprimirCuerpo(sScript.ToString());

                        #endregion
                    }
                    else if (TipoActa == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR).ToString())
                    {
                        #region Autorizacion_Viaje_Menor

                        DataTable dt = new DataTable();
                        ActoNotarialConsultaBL BL = new ActoNotarialConsultaBL();
                        dt = (DataTable)BL.ReporteAutorizacionViaje(Convert.ToInt32(iActoNotarialId), Convert.ToInt32(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                        DataTable dtDatosEscritura = new DataTable();
                        String strObservacion = String.Empty;

                        strObservacion = dt.Rows[0]["acno_vObservaciones"].ToString();
                        string strSubTipoActoExtraProtocolar = dt.Rows[0]["vSubTipoNotarialExtraProtocolar"].ToString();

                        dtDatosEscritura = BL.ActonotarialObtenerDatosPrincipales(Convert.ToInt64(iActoNotarialId), Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));

                        String strOficinaConsularNombre = dtDatosEscritura.Rows[0]["NombreOficinaConsular"].ToString().Trim();
                        String strFuncionarioAutorizador = dtDatosEscritura.Rows[0]["NombreFuncionario"].ToString().Trim();
                        String strCargoFuncionarioAutorizador = dtDatosEscritura.Rows[0]["CargoFuncionario"].ToString().Trim();
                        String strUbigeoOficinaConsular = dtDatosEscritura.Rows[0]["CiudadOficinaConsular"].ToString().Trim();
                        String strUbigeoOficinaConsularCiudad = dtDatosEscritura.Rows[0]["CiudadOficinaConsular"].ToString().Trim() + ", " + dtDatosEscritura.Rows[0]["Provincia"].ToString().Trim();
                        String strFecha = Util.ObtenerFechaParaDocumentoLegalProtocolar(Comun.FormatearFecha(dtDatosEscritura.Rows[0]["Fecha"].ToString())).ToUpper();
                        String StrNumeroDocumentoFuncionario = dtDatosEscritura.Rows[0]["NumeroDocumentoFuncionario"].ToString().Trim();
                        String GeneroFuncionario = dtDatosEscritura.Rows[0]["sGeneroFuncionario"].ToString().Trim();
                        string strSubTipoActoViajeMejor = dtDatosEscritura.Rows[0]["vSubTipoActoNotarialExtraProtocolar"].ToString().Trim();
                        string strNroEscrituraPublica = dtDatosEscritura.Rows[0]["NumeroEscrituraPublica"].ToString().Trim();
                        string strNroPartidaRegistral = dtDatosEscritura.Rows[0]["vPartidaRegistral"].ToString().Trim();

                        StringBuilder sScript = new StringBuilder();
                        StringBuilder sScriptPadre = new StringBuilder();
                        StringBuilder sScriptMadre = new StringBuilder();
                        StringBuilder sScriptMenor = new StringBuilder();
                        StringBuilder sScriptAcompanate = new StringBuilder();
                        StringBuilder sScriptInterprete = new StringBuilder();
                        StringBuilder sScriptApoderado = new StringBuilder();

                        String strPluralSingularComparece = String.Empty;

                        String strGenerpIdentificadoMadre = String.Empty;

                        String strGeneroHijo = String.Empty;
                        String strGeneroIdentificado = String.Empty;
                        string strHijoSoloSola = string.Empty;

                        String strGeneroHijoAcompa = String.Empty;
                        String strGeneroIdentificadoAcompa = String.Empty;

                        String strArticuloEL_LA = String.Empty;
                        String strSingularPluralCOMPARECIENTE = String.Empty;
                        String StrSingularPluralQuien = String.Empty;
                        String StrSingularPluralDeclara = String.Empty;
                        String StrSingularPluralAsume = String.Empty;
                        String strSingularPluralRatifican = String.Empty;
                        String strSingularPluralFirma = String.Empty;
                        String strSingularPluralsON = String.Empty;
                        String strSingularPluralHabilInter = String.Empty;
                        String strGeneroIdentificadoInterprete = String.Empty;
                        string strSingularPluralAutorizan = string.Empty;
                        string strGeneroIdentificadoApoderado = String.Empty;
                        string strGeneroRepresentadoApoderado = String.Empty;

                        String strGenerpLA_ELInterprete = String.Empty;
                        String strPluralSingularInterprete = String.Empty;

                        String strPluralSinguralInterprete = String.Empty;
                        String strPluralSinguralManifiesta = String.Empty;
                        String strPluralSinguralQueda = String.Empty;
                        String strPluralSinguralFacultado = String.Empty;

                        String strPluralSinguralQuienInterprete = String.Empty;
                        String strPluralSinguralDeclaraInterprete = String.Empty;


                        String strIdiomaMadre = String.Empty;
                        String strIdiomaPadre = String.Empty;
                        String strIdioma = String.Empty;
                        bool bIdiomaPadre = false;
                        bool bIdiomaMadre = false;

                        Int16 sContarPadres = 0;
                        Int16 sContarInterprete = 0, sContarMujeresInterprete = 0, sContarHombresInterprete = 0;
                        Int16 sContarMujeresApoderado = 0, sContarHombresApoderado = 0;

                        Int16 intContarHijos = 0;
                        Int16 intNumeroHijos = 0;
                        bool ExisteHijoVarones = false;

                        String HtmlTestigoRuegoParticipante = String.Empty;
                        String strGeneroIdentificadoFuncionario = String.Empty;
                        Boolean ExisteInterorete = false, bExisteTestigoRuego = false, bExisteApoderado = false, bExisteCompania = false;



                        strGeneroIdentificadoFuncionario = "IDENTIFICADO";
                        if (GeneroFuncionario == "2")
                        {
                            strGeneroIdentificadoFuncionario = "IDENTIFICADA";
                        }


                        sScript.Append("<br />");
                        sScript.Append("<p align=\"center\">");
                        //sScript.Append("<h2><font face=\"impact\">AUTORIZACIÓN DE VIAJE DE MENOR AL EXTERIOR</font></h2>");
                        sScript.Append("<h2><font face=\"impact\">AUTORIZACIÓN DE VIAJE DE MENOR</font></h2>");
                        sScript.Append("</p>");

                        //sScript.Append("<p align=\"center\">");
                        //sScript.Append("<h4>(" + strSubTipoActoExtraProtocolar + ")</h4>"); 
                        //sScript.Append("</p>");
                        sScript.Append("<br />");
                        sScript.Append("<tab></tab>");
                        //-----------------------------------------------------------------------------------------

                        sScript.Append("<p align=\"justify\"; style=\"background-color:transparent;\">");

                        sScript.Append("EN LA CIUDAD DE ");
                        sScript.Append(strUbigeoOficinaConsular);
                        //sScript.Append(", EL ");
                        sScript.Append(", ");
                        sScript.Append(strFecha);
                        sScript.Append(", ANTE MÍ, ");
                        sScript.Append(strFuncionarioAutorizador);
                        sScript.Append(", " + strGeneroIdentificadoFuncionario + " CON DOCUMENTO NACIONAL DE IDENTIDAD");
                        sScript.Append(" ");
                        sScript.Append(StrNumeroDocumentoFuncionario);
                        sScript.Append(", ");
                        sScript.Append(strCargoFuncionarioAutorizador);
                        sScript.Append(" EN ");
                        sScript.Append(strUbigeoOficinaConsularCiudad);

                        bool bExistePadre = false;
                        bool bExisteMadre = false;

                        foreach (DataRow Row in dt.Rows)
                        {
                            if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.MENOR))
                            {
                                intNumeroHijos++;

                                if ((int)Enumerador.enmGenero.MASCULINO == Convert.ToUInt16(Row["sGeneroId"].ToString()))
                                {
                                    ExisteHijoVarones = true;
                                }
                            }

                            if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.PADRE))
                            {
                                strGeneroRepresentadoApoderado = "REPRESENTADO";
                                bExistePadre = true;
                            }
                            if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.MADRE))
                            {
                                strGeneroRepresentadoApoderado = "REPRESENTADA";
                                bExisteMadre = true;
                            }
                            if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.ACOMPANANTE))
                            {
                                bExisteCompania = true;
                            }
                        }

                        if (bExistePadre && bExisteMadre)
                        {
                            strGeneroRepresentadoApoderado = "REPRESENTADOS";
                        }

                        Boolean bHuellaMadre = false;
                        Boolean bHuellaPadre = false;

                        foreach (DataRow Row in dt.Rows)
                        {

                            if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.MADRE))
                            {
                                #region Madre
                                if (strSubTipoActoViajeMejor == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - PADRE REPRESENTA A MADRE" ||
                                    strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A MADRE" ||
                                    strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE Y MADRE")
                                {
                                    sScriptMadre.Append(Row["vParticipante"].ToString().ToUpper());
                                    sScriptMadre.Append(", DE NACIONALIDAD " + Row["vNacionalidad"].ToString());
                                    sScriptMadre.Append(", IDENTIFICADA CON  " + Row["vTipoDocumento"].ToString().ToUpper() + " " + Row["vDocumentoNumero"].ToString().ToUpper());
                                }
                                else {
                                    //-------------------------------------------------
                                    //Fecha: 13/09/2017
                                    //Autor: Miguel Márquez Beltrán
                                    //Objetivo: Asignar si hizo check en No Huella  
                                    //-------------------------------------------------

                                    if (Row["anpa_bFlagHuella"].ToString() == "True")
                                    {
                                        bHuellaMadre = true;
                                    }
                                    else
                                    {
                                        bHuellaMadre = false;
                                    }

                                    //-------------------------------------------------

                                    sScriptMadre.Append(Row["vParticipante"].ToString());
                                    sScriptMadre.Append(", DE NACIONALIDAD " + Row["vNacionalidad"].ToString());
                                    sScriptMadre.Append(", IDENTIFICADA CON  " + Row["vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString());
                                    sScriptMadre.Append(", ESTADO CIVIL  " + Row["vEstadoCivil"].ToString());
                                    sScriptMadre.Append(", DE OCUPACIÓN  " + Row["vOcupacion"].ToString());
                                    sScriptMadre.Append(", CON DOMICILIO EN " + Row["vDireccion"].ToString());

                                    if (sIdiomaCastellano != Row["sIdiomaId"].ToString())
                                    {
                                        if (Row["vIdioma"].ToString() != strIdioma)
                                        {
                                            strIdioma = strIdioma + ", " + Row["vIdioma"].ToString();
                                            strIdiomaMadre = Row["vIdioma"].ToString();
                                        }
                                    }
                                    sContarPadres++;
                                    DataTable dtp1 = (DataTable)BL.ReporteAutorizacionViaje(Convert.ToInt32(iActoNotarialId),
                                    Convert.ToInt32(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), Convert.ToInt64(Row["anpa_iActoNotarialParticipanteId"].ToString()));
                                    if (dtp1 != null)
                                    {
                                        if (dtp1.Rows.Count > 0)
                                        {
                                            foreach (DataRow RowTA in dtp1.Rows)
                                            {
                                                if (RowTA["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO))
                                                {
                                                    bExisteTestigoRuego = true;

                                                    sScriptMadre.Append("; QUIEN SE ENCUENTRA INCAPACITADA DE FIRMAR POR " + Row["pers_vDescripcionIncapacidad"].ToString().ToUpper());

                                                    if (bHuellaMadre)
                                                    {
                                                        sScriptMadre.Append(", POR LO QUE IMPRIME SU HUELLA DACTILAR, Y DESIGNA A  ");
                                                    }
                                                    else
                                                    {
                                                        sScriptMadre.Append(", Y DESIGNA A  ");
                                                    }

                                                    sScriptMadre.Append(RowTA["vParticipante"].ToString());
                                                    //sScriptMadre.Append(", DE NACIONALIDAD " + RowTA["vNacionalidad"].ToString());
                                                    //sScriptMadre.Append(", DE OCUPACIÓN " + RowTA["vOcupacion"].ToString());
                                                    sScriptMadre.Append(", CON  " + RowTA["vTipoDocumento"].ToString() + " " + RowTA["vDocumentoNumero"].ToString());
                                                    //sScriptMadre.Append(", ESTADO CIVIL  " + RowTA["vEstadoCivil"].ToString());
                                                    //sScriptMadre.Append(", CON DOMICILIO EN " + RowTA["vDireccion"].ToString());
                                                    sScriptMadre.Append(", QUIEN ACTÚA EN CALIDAD DE TESTIGO A RUEGO");
                                                    sScriptMadre.Append(", DE CONFORMIDAD CON LO ESTABLECIDO EN EL ART.107° DEL DECRETO LEGISLATIVO N° 1049;");
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }

                            if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.PADRE))
                            {
                                #region Padre
                                if (strSubTipoActoViajeMejor == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - MADRE REPRESENTA A PADRE" ||
                                    strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE" ||
                                    strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE Y MADRE")
                                {
                                    sScriptPadre.Append(Row["vParticipante"].ToString().ToUpper());
                                    sScriptPadre.Append(", DE NACIONALIDAD " + Row["vNacionalidad"].ToString().ToUpper());
                                    sScriptPadre.Append(", IDENTIFICADO CON  " + Row["vTipoDocumento"].ToString().ToUpper() + " " + Row["vDocumentoNumero"].ToString().ToUpper());
                                }
                                else {
                                    //-------------------------------------------------
                                    //Fecha: 13/09/2017
                                    //Autor: Miguel Márquez Beltrán
                                    //Objetivo: Asignar si hizo check en No Huella  
                                    //-------------------------------------------------
                                    if (Row["anpa_bFlagHuella"].ToString() == "True")
                                    {
                                        bHuellaPadre = true;
                                    }
                                    else
                                    {
                                        bHuellaPadre = false;
                                    }
                                    //-------------------------------------------------

                                    sScriptPadre.Append(Row["vParticipante"].ToString());
                                    sScriptPadre.Append(", DE NACIONALIDAD " + Row["vNacionalidad"].ToString());
                                    sScriptPadre.Append(", IDENTIFICADO CON  " + Row["vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString());
                                    sScriptPadre.Append(", ESTADO CIVIL  " + Row["vEstadoCivil"].ToString());
                                    sScriptPadre.Append(", DE OCUPACIÓN  " + Row["vOcupacion"].ToString());
                                    sScriptPadre.Append(", CON DOMICILIO EN " + Row["vDireccion"].ToString());
                                    sContarPadres++;

                                    if (sIdiomaCastellano != Row["sIdiomaId"].ToString())
                                    {
                                        if (Row["vIdioma"].ToString() != strIdioma)
                                        {
                                            strIdioma = strIdioma + ", " + Row["vIdioma"].ToString();
                                            strIdiomaPadre = Row["vIdioma"].ToString();
                                        }
                                    }
                                    DataTable dtp1 = (DataTable)BL.ReporteAutorizacionViaje(Convert.ToInt32(iActoNotarialId),
                                    Convert.ToInt32(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), Convert.ToInt64(Row["anpa_iActoNotarialParticipanteId"].ToString()));

                                    if (dtp1 != null)
                                    {
                                        if (dtp1.Rows.Count > 0)
                                        {
                                            foreach (DataRow RowTA in dtp1.Rows)
                                            {
                                                if (RowTA["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO))
                                                {
                                                    bExisteTestigoRuego = true;


                                                    sScriptPadre.Append("; QUIEN SE ENCUENTRA INCAPACITADO DE FIRMAR POR " + Row["pers_vDescripcionIncapacidad"].ToString().ToUpper());

                                                    if (bHuellaPadre)
                                                    {
                                                        sScriptPadre.Append(", POR LO QUE IMPRIME SU HUELLA DACTILAR, Y DESIGNA A  ");
                                                    }
                                                    else
                                                    {
                                                        sScriptPadre.Append(", Y DESIGNA A  ");
                                                    }

                                                    sScriptPadre.Append(dtp1.Rows[0]["vParticipante"].ToString());
                                                    //sScriptPadre.Append(", DE NACIONALIDAD " + dtp1.Rows[0]["vNacionalidad"].ToString());
                                                    //sScriptPadre.Append(", DE OCUPACIÓN " + dtp1.Rows[0]["vOcupacion"].ToString());
                                                    sScriptPadre.Append(", CON  " + dtp1.Rows[0]["vTipoDocumento"].ToString() + " " + dtp1.Rows[0]["vDocumentoNumero"].ToString());
                                                    //sScriptPadre.Append(", ESTADO CIVIL  " + dtp1.Rows[0]["vEstadoCivil"].ToString());
                                                    //sScriptPadre.Append(", CON DOMICILIO EN " + dtp1.Rows[0]["vDireccion"].ToString());
                                                    sScriptPadre.Append(", QUIEN ACTÚA EN CALIDAD DE TESTIGO A RUEGO");
                                                    sScriptPadre.Append(", DE CONFORMIDAD CON LO ESTABLECIDO EN EL ART.107° DEL DECRETO LEGISLATIVO N° 1049;");
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                
                                #endregion
                            }
                            if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.MENOR))
                            {
                                #region Menor Hijo

                                intContarHijos++;

                                if ((int)Enumerador.enmGenero.FEMENINO == Convert.ToUInt16(Row["sGeneroId"].ToString()))
                                {
                                    #region Femenino

                                    if (intContarHijos == 1)
                                    {
                                        if (intNumeroHijos > 1)
                                        {
                                            if (ExisteHijoVarones)
                                            {
                                                strGeneroHijo = "HIJOS";
                                            }
                                            else
                                            {
                                                strGeneroHijo = "HIJAS";
                                            }
                                        }
                                        else
                                        {
                                            strGeneroHijo = "HIJA";
                                        }
                                    }
                                    //else
                                    //{
                                    //    if (intNumeroHijos == intContarHijos)
                                    //    {
                                    //        strGeneroHijo = "Y SU HIJA";
                                    //    }
                                    //    else
                                    //    {
                                    //        strGeneroHijo = "HIJA";
                                    //    }
                                    //}

                                    strGeneroIdentificado = "IDENTIFICADA";
                                    #endregion
                                }
                                else
                                {
                                    #region Masculino

                                    if (intContarHijos == 1)
                                    {
                                        if (intNumeroHijos > 1)
                                        {
                                            if (ExisteHijoVarones)
                                            {
                                                strGeneroHijo = "HIJOS";
                                            }
                                        }
                                        else
                                        {
                                            strGeneroHijo = "HIJO";
                                        }
                                    }
                                    //else
                                    //{
                                    //    if (intNumeroHijos == intContarHijos)
                                    //    {
                                    //        strGeneroHijo = "Y SU HIJO";
                                    //    }
                                    //    else
                                    //    {
                                    //        strGeneroHijo = "HIJO";
                                    //    }
                                    //}
                                    strGeneroIdentificado = "IDENTIFICADO";
                                    #endregion
                                }
                                //sScript.Append("; A FIN DE AUTORIZAR EL VIAJE DE SU MENOR ");
                                if (intContarHijos == 1)
                                {
                                    sScriptMenor.Append(strGeneroHijo + " ");
                                }
                                if (intNumeroHijos > 1)
                                {
                                    if (intNumeroHijos == intContarHijos)
                                    {
                                        sScriptMenor.Append(" Y ");
                                    }
                                    else
                                    {
                                        sScriptMenor.Append(", ");
                                    }
                                }
                                sScriptMenor.Append(Row["vParticipante"].ToString());
                                sScriptMenor.Append(", DE " + Row["Edad"].ToString());
                                sScriptMenor.Append(" DE EDAD, " + strGeneroIdentificado + " CON ");
                                sScriptMenor.Append(Row["vTipoDocumento"].ToString());
                                sScriptMenor.Append(" ");
                                sScriptMenor.Append(Row["vDocumentoNumero"].ToString());

                                if (intNumeroHijos == intContarHijos)
                                {
                                    sScriptMenor.Append(", A ");
                                    sScriptMenor.Append(Row["PaisDestino"].ToString());
                                    sScriptMenor.Append(", ");
                                }
                                
                                #endregion
                            }

                            if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.ACOMPANANTE))
                            {
                                #region Acompañante

                                sScriptAcompanate.Append(" EN COMPAÑÍA DE ");
                                sScriptAcompanate.Append(Row["vParticipante"].ToString());
                                sScriptAcompanate.Append(",");

                                if (Convert.ToInt16(Row["sGeneroId"].ToString()) == (int)Enumerador.enmGenero.FEMENINO)
                                {
                                    sScriptAcompanate.Append(" IDENTIFICADA CON ");
                                }
                                else
                                {
                                    sScriptAcompanate.Append(" IDENTIFICADO CON ");
                                }
                                sScriptAcompanate.Append(Row["vTipoDocumento"].ToString().ToUpper() + " " + Row["vDocumentoNumero"].ToString().ToUpper());
                                sScriptAcompanate.Append(",");
                                #endregion
                            }

                            if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.INTERPRETE))
                            {
                                #region Interprete
                                ExisteInterorete = true;

                                if (Convert.ToInt16(Row["sGeneroId"].ToString()) == (int)Enumerador.enmGenero.FEMENINO)
                                {
                                    sContarMujeresInterprete++;
                                    strGeneroIdentificadoInterprete = "IDENTIFICADA";
                                }
                                else
                                {
                                    sContarHombresInterprete++;
                                    strGeneroIdentificadoInterprete = "IDENTIFICADO";
                                }

                                sScriptInterprete.Append(Row["vParticipante"].ToString().ToString());
                                //sScriptInterprete.Append(", DE NACIONALIDAD " + Row["vNacionalidad"].ToString());
                                //sScriptInterprete.Append(", OCUPACIÓN  " + Row["vOcupacion"].ToString());
                                sScriptInterprete.Append(", " + strGeneroIdentificadoInterprete + " CON  " + Row["vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString());
                                //sScriptInterprete.Append(", ESTADO CIVIL  " + Row["vEstadoCivil"].ToString());
                                //sScriptInterprete.Append(", CON DOMICILIO EN " + Row["vDireccion"].ToString());
                                sScriptInterprete.Append("; ");
                                #endregion
                            }

                            //-----------------------------------------------------------------------
                            //Fecha: 14/09/2017
                            //Autor: Miguel Márquez Beltrán
                            //Objetivo: Incluir el Apoderado en la Autorización de Viaje de Menores
                            //-----------------------------------------------------------------------

                            if (Row["sTipoParticipanteId"].ToString() == Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.APODERADO))
                            {
                                #region Apoderado

                                bExisteApoderado = true;
                                if (Convert.ToInt16(Row["sGeneroId"].ToString()) == (int)Enumerador.enmGenero.FEMENINO)
                                {
                                    sContarMujeresApoderado++;
                                    strGeneroIdentificadoApoderado = "IDENTIFICADA";
                                }
                                else
                                {
                                    sContarHombresApoderado++;
                                    strGeneroIdentificadoApoderado = "IDENTIFICADO";
                                }

                                if (strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A MADRE" ||
                                    strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE" ||
                                    strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE Y MADRE")
                                {
                                    sScriptApoderado.Append(Row["vParticipante"].ToString().ToUpper());
                                    sScriptApoderado.Append(", DE NACIONALIDAD " + Row["vNacionalidad"].ToString().ToUpper());
                                    sScriptApoderado.Append(", " + strGeneroIdentificadoApoderado + " CON  " + Row["vTipoDocumento"].ToString().ToUpper() + " " + Row["vDocumentoNumero"].ToString().ToUpper());
                                    sScriptApoderado.Append(", ESTADO CIVIL  " + Row["vEstadoCivil"].ToString().ToUpper());
                                    sScriptApoderado.Append(", DE OCUPACIÓN  " + Row["vOcupacion"].ToString().ToUpper());
                                    sScriptApoderado.Append(", CON DOMICILIO EN " + Row["vDireccion"].ToString().ToUpper());
                                }
                                else
                                {
                                    sScriptApoderado.Append(", DEBIDAMENTE " + strGeneroRepresentadoApoderado + " POR ");
                                    sScriptApoderado.Append(Row["vParticipante"].ToString().ToUpper());
                                    sScriptApoderado.Append(", " + strGeneroIdentificadoApoderado + " CON  " + Row["vTipoDocumento"].ToString().ToUpper() + " " + Row["vDocumentoNumero"].ToString().ToUpper());
                                    sScriptApoderado.Append(" MEDIANTE PODER POR ESCRITURA PÚBLICA N° " + Row["anpa_vNumeroEscrituraPublica"].ToString().ToUpper());
                                    sScriptApoderado.Append(" INSCRITO EN LA PARTIDA N° " + Row["anpa_vNumeroPartida"].ToString().ToUpper());
                                    sScriptApoderado.Append(" DE LOS REGISTROS PÚBLICOS; ");
                                }

                                #endregion
                            }
                            //-----------------------------------------------------------------------

                        }
                        if (strIdiomaMadre != String.Empty && strIdiomaPadre == String.Empty)
                        {
                            strIdioma = strIdiomaMadre;
                            bIdiomaMadre = true;
                        }
                        else if (strIdiomaMadre == String.Empty && strIdiomaPadre != String.Empty)
                        {
                            strIdioma = strIdiomaPadre;
                            bIdiomaPadre = true;
                        }
                        else if (strIdiomaMadre != String.Empty && strIdiomaPadre != String.Empty)
                        {
                            if (strIdiomaMadre == strIdiomaPadre)
                            { strIdioma = strIdiomaMadre; }
                            else { strIdioma = strIdiomaMadre + ", " + strIdiomaPadre; }
                            bIdiomaPadre = true;
                            bIdiomaMadre = true;
                        }
                        
                        if (sScriptMadre.Length > 0 && sScriptPadre.Length == 0)
                        {
                            sScript.Append("; COMPARECE:");
                            sScript.Append(" " + sScriptMadre);
                            sScript.Append(CrearTextoExtraInterprete(ExisteInterorete, bExisteTestigoRuego, strIdioma, sScriptInterprete.ToString(),true));
                            //sScript.Append(".");
                            strArticuloEL_LA = "LA";
                            strSingularPluralCOMPARECIENTE = "COMPARECIENTE";
                            StrSingularPluralQuien = "QUIEN";
                            StrSingularPluralDeclara = "DECLARA";
                            StrSingularPluralAsume = "ASUME";
                            strSingularPluralRatifican = "RATIFICA";
                            strSingularPluralFirma = "FIRMA";
                            strSingularPluralsON = "ES";
                            strSingularPluralHabilInter = "HÁBIL";
                            strSingularPluralAutorizan = "AUTORIZA";
                        }
                        else if (sScriptMadre.Length == 0 && sScriptPadre.Length > 0)
                        {
                            sScript.Append("; COMPARECE:");
                            sScript.Append(" " + sScriptPadre);
                            sScript.Append(CrearTextoExtraInterprete(ExisteInterorete, bExisteTestigoRuego, strIdioma, sScriptInterprete.ToString(),true));
                            //sScript.Append(".");
                            strArticuloEL_LA = "EL";
                            strSingularPluralCOMPARECIENTE = "COMPARECIENTE";
                            StrSingularPluralQuien = "QUIEN";
                            StrSingularPluralDeclara = "DECLARA";
                            StrSingularPluralAsume = "ASUME";
                            strSingularPluralRatifican = "RATIFICA";
                            strSingularPluralFirma = "FIRMA";
                            strSingularPluralsON = "ES";
                            strSingularPluralHabilInter = "HÁBIL";
                            strSingularPluralAutorizan = "AUTORIZA";
                        }
                        else if (sScriptMadre.Length > 0 && sScriptPadre.Length > 0)
                        {

                            string sStringApoderadoGenero = string.Empty;

                            if (strGeneroIdentificadoApoderado == "IDENTIFICADA")
                            {
                                sStringApoderadoGenero = "FACULTADA";
                            }
                            else
                            {
                                sStringApoderadoGenero = "FACULTADO";
                            }

                            if (strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE Y MADRE")
                            {
                                sScript.Append("; COMPARECE:");
                                sScript.Append(" " + sScriptApoderado);
                                sScript.Append(" QUIEN PROCEDE EN REPRESENTACIÓN DE ");
                                sScript.Append(" " + sScriptPadre);
                                sScript.Append(" Y ");
                                sScript.Append(sScriptMadre);

                                sScript.Append(", DEBIDAMENTE " + sStringApoderadoGenero + " SEGÚN EL PODER INSCRITO EN LA PARTIDA ");
                                sScript.Append(strNroEscrituraPublica);
                                sScript.Append(" DEL REGISTRO DE MANDATOS Y PODERES DE LOS REGISTROS PÚBLICOS DE N° ");
                                sScript.Append(strNroPartidaRegistral);
                            }
                            else if (strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A MADRE")
                            {
                                
                                sScript.Append("; COMPARECEN:");
                                sScript.Append(" " + sScriptPadre);
                                sScript.Append(" Y ");
                                sScript.Append(sScriptApoderado);

                                //sScript.Append(CrearTextoExtraInterprete(ExisteInterorete, bExisteTestigoRuego, strIdiomaPadre, sScriptInterprete.ToString(), true));
                                sScript.Append(" QUIEN PROCEDE EN REPRESENTACIÓN DE ");
                                sScript.Append(sScriptMadre);
                                //sScript.Append(".");


                                sScript.Append(", DEBIDAMENTE " + sStringApoderadoGenero + " SEGÚN EL PODER INSCRITO EN LA PARTIDA ");
                                sScript.Append(strNroEscrituraPublica);
                                sScript.Append(" DEL REGISTRO DE MANDATOS Y PODERES DE LOS REGISTROS PÚBLICOS DE N° ");
                                sScript.Append(strNroPartidaRegistral);
                            }
                            else if (strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE")
                            {
                                sScript.Append("; COMPARECEN:");
                                sScript.Append(" " + sScriptMadre);
                                sScript.Append(" Y ");
                                sScript.Append(sScriptApoderado);

                                //sScript.Append(CrearTextoExtraInterprete(ExisteInterorete, bExisteTestigoRuego, strIdiomaPadre, sScriptInterprete.ToString(), true));
                                sScript.Append(" QUIEN PROCEDE EN REPRESENTACIÓN DE ");
                                sScript.Append(sScriptPadre);
                                //sScript.Append(".");


                                sScript.Append(", DEBIDAMENTE " + sStringApoderadoGenero + " SEGÚN EL PODER INSCRITO EN LA PARTIDA ");
                                sScript.Append(strNroEscrituraPublica);
                                sScript.Append(" DEL REGISTRO DE MANDATOS Y PODERES DE LOS REGISTROS PÚBLICOS DE N° ");
                                sScript.Append(strNroPartidaRegistral);
                            
                            }
                            else if (strSubTipoActoViajeMejor == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - PADRE REPRESENTA A MADRE")
                            {
                                if (bIdiomaPadre)
                                {
                                    sScript.Append("; COMPARECE:");
                                    sScript.Append(" " + sScriptPadre);

                                    sScript.Append(CrearTextoExtraInterprete(ExisteInterorete, bExisteTestigoRuego, strIdiomaPadre, sScriptInterprete.ToString(), true));
                                    sScript.Append(" QUIEN PROCEDE POR DERECHO PROPIO Y EN REPRESENTACIÓN DE ");
                                    sScript.Append(sScriptMadre);
                                    //sScript.Append(".");
                                }
                                else
                                {
                                    sScript.Append("; COMPARECE:");
                                    sScript.Append(" " + sScriptPadre);
                    
                                    sScript.Append(" QUIEN PROCEDE POR DERECHO PROPIO Y EN REPRESENTACIÓN DE ");
                                    sScript.Append(sScriptMadre);
                                }

                                sScript.Append(", DEBIDAMENTE " + "FACULTADO" + " SEGÚN EL PODER INSCRITO EN LA PARTIDA ");
                                sScript.Append(strNroEscrituraPublica);
                                sScript.Append(" DEL REGISTRO DE MANDATOS Y PODERES DE LOS REGISTROS PÚBLICOS DE N° ");
                                sScript.Append(strNroPartidaRegistral);
                            }
                            else if (strSubTipoActoViajeMejor == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - MADRE REPRESENTA A PADRE")
                            {
                                if (bIdiomaMadre)
                                {
                                    sScript.Append("; COMPARECE:");
                                    sScript.Append(" " + sScriptMadre);

                                    sScript.Append(CrearTextoExtraInterprete(ExisteInterorete, bExisteTestigoRuego, strIdiomaMadre, sScriptInterprete.ToString(), true));


                                    sScript.Append(" QUIEN PROCEDE POR DERECHO PROPIO Y EN REPRESENTACIÓN DE ");
                                    sScript.Append(sScriptPadre);
                                }
                                else
                                {
                                    sScript.Append("; COMPARECE:");
                                    sScript.Append(" " + sScriptMadre);

                                    sScript.Append(" QUIEN PROCEDE POR DERECHO PROPIO Y EN REPRESENTACIÓN DE ");
                                    sScript.Append(sScriptPadre);
                                }

                                sScript.Append(", DEBIDAMENTE " + "FACULTADA" + " SEGÚN EL PODER INSCRITO EN LA PARTIDA ");
                                sScript.Append(strNroEscrituraPublica);
                                sScript.Append(" DEL REGISTRO DE MANDATOS Y PODERES DE LOS REGISTROS PÚBLICOS DE N° ");
                                sScript.Append(strNroPartidaRegistral);
                            }
                            else {
                                if (bIdiomaPadre)
                                {
                                    sScript.Append("; COMPARECEN:");
                                    sScript.Append(" " + sScriptMadre);
                                    if (bIdiomaMadre)
                                    {
                                        sScript.Append(CrearTextoExtraInterprete(ExisteInterorete, bExisteTestigoRuego, strIdiomaMadre, sScriptInterprete.ToString(), false));
                                    }
                                    //sScript.Append(";");
                                    sScript.Append(" Y ");
                                    sScript.Append(sScriptPadre);
                                    if (bIdiomaPadre)
                                    {
                                        if (bIdiomaMadre)
                                        { sScript.Append(CrearTextoExtraInterprete(ExisteInterorete, bExisteTestigoRuego, strIdioma, sScriptInterprete.ToString(), true, true)); }
                                        else { sScript.Append(CrearTextoExtraInterprete(ExisteInterorete, bExisteTestigoRuego, strIdioma, sScriptInterprete.ToString(), true)); }
                                    }
                                    //sScript.Append(".");
                                }
                                else if (bIdiomaMadre)
                                {
                                    sScript.Append("; COMPARECEN:");
                                    sScript.Append(" " + sScriptPadre);
                                    if (bIdiomaPadre)
                                    {
                                        sScript.Append(CrearTextoExtraInterprete(ExisteInterorete, bExisteTestigoRuego, strIdiomaPadre, sScriptInterprete.ToString(), false));
                                    }
                                    //sScript.Append(";");
                                    sScript.Append(" Y ");
                                    sScript.Append(sScriptMadre);
                                    if (bIdiomaMadre)
                                    {
                                        if (bIdiomaPadre)
                                        { sScript.Append(CrearTextoExtraInterprete(ExisteInterorete, bExisteTestigoRuego, strIdioma, sScriptInterprete.ToString(), true, true)); }
                                        else { sScript.Append(CrearTextoExtraInterprete(ExisteInterorete, bExisteTestigoRuego, strIdioma, sScriptInterprete.ToString(), true)); }
                                    }
                                    //sScript.Append(".");
                                }
                                else
                                {
                                    sScript.Append("; COMPARECEN:");
                                    sScript.Append(" " + sScriptMadre);
                                    if (bIdiomaMadre)
                                    {
                                        sScript.Append(CrearTextoExtraInterprete(ExisteInterorete, bExisteTestigoRuego, strIdiomaMadre, sScriptInterprete.ToString(), false));
                                    }
                                    //sScript.Append(";");
                                    sScript.Append(" Y ");
                                    sScript.Append(sScriptPadre);
                                    if (bIdiomaPadre)
                                    {
                                        if (bIdiomaPadre)
                                        { sScript.Append(CrearTextoExtraInterprete(ExisteInterorete, bExisteTestigoRuego, strIdioma, sScriptInterprete.ToString(), true, true)); }
                                        else { sScript.Append(CrearTextoExtraInterprete(ExisteInterorete, bExisteTestigoRuego, strIdioma, sScriptInterprete.ToString(), true)); }
                                    }
                                    //sScript.Append(".");
                                }
                            }
                            //cambio 28/02/2019
                            if (strSubTipoActoViajeMejor == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - PADRE REPRESENTA A MADRE")
                            {
                                strArticuloEL_LA = "EL";
                                strSingularPluralCOMPARECIENTE = "COMPARECIENTE";
                                StrSingularPluralQuien = "QUIEN";
                                StrSingularPluralDeclara = "DECLARA";
                                StrSingularPluralAsume = "ASUME";
                                strSingularPluralRatifican = "RATIFICA";
                                strSingularPluralFirma = "FIRMA";
                                strSingularPluralsON = "ES";
                                strSingularPluralHabilInter = "HÁBIL";
                                strSingularPluralAutorizan = "AUTORIZA";
                            }
                            else if (strSubTipoActoViajeMejor == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - MADRE REPRESENTA A PADRE")
                            {
                                strArticuloEL_LA = "LA";
                                strSingularPluralCOMPARECIENTE = "COMPARECIENTE";
                                StrSingularPluralQuien = "QUIEN";
                                StrSingularPluralDeclara = "DECLARA";
                                StrSingularPluralAsume = "ASUME";
                                strSingularPluralRatifican = "RATIFICA";
                                strSingularPluralFirma = "FIRMA";
                                strSingularPluralsON = "ES";
                                strSingularPluralHabilInter = "HÁBIL";
                                strSingularPluralAutorizan = "AUTORIZA";
                            }
                            else if (strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE Y MADRE")
                            {
                                if (strGeneroIdentificadoApoderado == "IDENTIFICADA")
                                {
                                    strArticuloEL_LA = "LA";
                                    strSingularPluralCOMPARECIENTE = "COMPARECIENTE";
                                    StrSingularPluralQuien = "QUIEN";
                                    StrSingularPluralDeclara = "DECLARA";
                                    StrSingularPluralAsume = "ASUME";
                                    strSingularPluralRatifican = "RATIFICA";
                                    strSingularPluralFirma = "FIRMA";
                                    strSingularPluralsON = "ES";
                                    strSingularPluralHabilInter = "HÁBIL";
                                    strSingularPluralAutorizan = "AUTORIZA";
                                }
                                else
                                {
                                    strArticuloEL_LA = "EL";
                                    strSingularPluralCOMPARECIENTE = "COMPARECIENTE";
                                    StrSingularPluralQuien = "QUIEN";
                                    StrSingularPluralDeclara = "DECLARA";
                                    StrSingularPluralAsume = "ASUME";
                                    strSingularPluralRatifican = "RATIFICA";
                                    strSingularPluralFirma = "FIRMA";
                                    strSingularPluralsON = "ES";
                                    strSingularPluralHabilInter = "HÁBIL";
                                    strSingularPluralAutorizan = "AUTORIZA";
                                }
                            }
                            else
                            {
                                strArticuloEL_LA = "LOS";
                                strSingularPluralCOMPARECIENTE = "COMPARECIENTES";
                                StrSingularPluralQuien = "QUIENES";
                                StrSingularPluralDeclara = "DECLARAN";
                                StrSingularPluralAsume = "ASUMEN";
                                strSingularPluralRatifican = "RATIFICAN";
                                strSingularPluralFirma = "FIRMAN";
                                strSingularPluralsON = "SON";
                                strSingularPluralHabilInter = "HÁBILES";
                                strSingularPluralAutorizan = "AUTORIZAN";
                            }
                        }
                        /* INTERPRETE */
                        if (sContarMujeresInterprete == 1 && sContarHombresInterprete == 0)
                        {
                            strGenerpLA_ELInterprete = "LA";
                        }
                        else if (sContarMujeresInterprete >= 1 && sContarHombresInterprete == 0)
                        {
                            strGenerpLA_ELInterprete = "LAS";
                        }

                        else if (sContarMujeresInterprete == 0 && sContarHombresInterprete == 1)
                        {
                            strGenerpLA_ELInterprete = "EL";
                        }
                        else if (sContarMujeresInterprete == 0 && sContarHombresInterprete >= 1)
                        {
                            strGenerpLA_ELInterprete = "LOS";
                        }

                        else if (sContarMujeresInterprete >= 1 && sContarHombresInterprete >= 1)
                        {
                            strGenerpLA_ELInterprete = "LOS";
                        }
                        else
                        {
                            strGenerpLA_ELInterprete = "EL";
                        }

                        String strPluralSingularInterpreteHA = String.Empty;
                        if (sContarInterprete <= 1)
                        {
                            strPluralSingularInterprete = "INTÉRPRETE";
                            strPluralSinguralQuienInterprete = "QUIEN";
                            strPluralSinguralDeclaraInterprete = "DECLARA";
                            strPluralSingularInterpreteHA = "HA";
                        }
                        else
                        {
                            strPluralSingularInterprete = "INTÉRPRETES";
                            strPluralSinguralQuienInterprete = "QUIENES";
                            strPluralSinguralDeclaraInterprete = "DECLARAN";
                            strPluralSingularInterpreteHA = "HAN";
                        }
                        /*JONATAN SILVA CACHAY*/
                        //if (ExisteInterorete)
                        //{
                        //    if (bExisteTestigoRuego)
                        //    {
                        //        sScript.Append(" QUIEN ES HÁBIL");
                        //    }
                        //    else
                        //    {
                        //        sScript.Append(", QUIEN ES HÁBIL");
                        //    }

                        //    sScript.Append(" EN EL IDIOMA " + strIdioma);
                        //    sScript.Append(" Y NO CONOCE EL IDIOMA CASTELLANO, POR LO QUE DESIGNA A ");
                        //    sScript.Append(sScriptInterprete);

                        //    sScript.Append(" QUIEN PROCEDE EN CALIDAD DE INTÉRPRETE DE CONFORMIDAD CON LO ESTABLECIDO EN EL ÁRTICULO 30° DEL DECRETO LEGISLATIVO N° 1049,");
                        //    sScript.Append(" MANIFIESTA SER HÁBIL EN EL IDIOMA " + strIdioma + " Y EL IDIOMA CASTELLANO Y TENER EL CONOCIMIENTO Y EXPERIENCIA SUFICIENTE PARA EFECTUAR LA TRADUCCIÓN QUE LE HA SIDO SOLICITADA.");
                        //}

                        if (ExisteInterorete == true || bExisteTestigoRuego == true)
                        {
                            if (intContarHijos == 1)
                            {
                                sScript.Append(" " + strArticuloEL_LA + " " + strSingularPluralCOMPARECIENTE + " " + strSingularPluralAutorizan + " EL VIAJE A SU MENOR");
                            }
                            else
                            {
                                sScript.Append(" " + strArticuloEL_LA + " " + strSingularPluralCOMPARECIENTE + " " + strSingularPluralAutorizan + " EL VIAJE A SUS MENORES");
                            }
                        }
                        else
                        {
                            if (bExisteApoderado)
                            {
                                if (strSubTipoActoViajeMejor != "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A MADRE" &&
                                    strSubTipoActoViajeMejor != "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE" &&
                                    strSubTipoActoViajeMejor != "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE Y MADRE")
                                {
                                    sScript.Append(sScriptApoderado);
                                }
                            }

                            if (intContarHijos == 1)
                            {
                                sScript.Append(" A FIN DE AUTORIZAR EL VIAJE DE SU MENOR");
                            }
                            else
                            {
                                sScript.Append(" A FIN DE AUTORIZAR EL VIAJE DE SUS MENORES");
                            }
                        }
                        //---------------------------------------
                        if (!bExisteCompania)
                        {
                            if (intContarHijos == 1)
                            {
                                if (ExisteHijoVarones)
                                {
                                    strHijoSoloSola = "QUIEN VIAJA SOLO";
                                }
                                else
                                {
                                    strHijoSoloSola = "QUIEN VIAJA SOLA";
                                }
                            }
                            else
                            {
                                if (ExisteHijoVarones)
                                {
                                    strHijoSoloSola = "QUIENES VIAJAN SOLOS";
                                }
                                else
                                {
                                    strHijoSoloSola = "QUIENES VIAJAN SOLAS";
                                }
                            }
                        }
                        //---------------------------------------

                        sScript.Append(" " + sScriptMenor);

                        if (sScriptAcompanate.Length > 0)
                        { sScript.Append(" " + sScriptAcompanate); }

                        if (!bExisteCompania)
                        {
                            sScript.Append(" " + strHijoSoloSola + ",");
                        }
                        sScript.Append(" DE ACUERDO AL SIGUIENTE ITINERARIO ");
                        sScript.Append(dt.Rows[0]["RutaTitular"].ToString());
                        sScript.Append(";");
                        sScript.Append(" DE CONFORMIDAD CON LO SEÑALADO EN EL ARTÍCULO 111 DEL CÓDIGO");
                        sScript.Append(" DE LOS NIÑOS Y ADOLESCENTES - LEY 27337. LA PRESENTE ACTA DE AUTORIZACIÓN DE VIAJE FUE LEÍDA POR ");
                        sScript.Append(strArticuloEL_LA + " " + strSingularPluralCOMPARECIENTE);
                        //-----------------------------------------------
                        //Autor: Miguel Márquez Beltrán
                        //Fecha: 31/08/2017
                        //Motivo: Cambio del formato de autorización de viaje de menor al exterior
                        //-----------------------------------------------
                        if (strSubTipoActoExtraProtocolar.Equals("PROGENITOR SUPERVIVIENTE") || strSubTipoActoExtraProtocolar.Equals("ÚNICO PROGENITOR QUE REALIZO EL RECONOCIMIENTO"))
                        {
                            if (strSubTipoActoExtraProtocolar.Equals("PROGENITOR SUPERVIVIENTE"))
                            {
                                sScript.Append(", EN SU CALIDAD DE " + strSubTipoActoExtraProtocolar.Trim());
                            }
                            if (strSubTipoActoExtraProtocolar.Equals("ÚNICO PROGENITOR QUE REALIZO EL RECONOCIMIENTO"))
                            {
                                sScript.Append(", EN CALIDAD DE " + strSubTipoActoExtraProtocolar.Trim());
                            }
                        }
                        //-----------------------------------------------
                        sScript.Append(", " + StrSingularPluralQuien);
                        sScript.Append("  " + StrSingularPluralDeclara);
                        sScript.Append(" QUE " + StrSingularPluralAsume);

                        if (bExisteTestigoRuego)
                        {
                            if ((bExistePadre && bHuellaPadre) || (bExisteMadre && bHuellaMadre))
                            {
                                sScript.Append(" TODAS LAS RESPONSABILIDADES QUE DE ELLA EMANEN Y SE " + strSingularPluralRatifican + " EN SU CONTENIDO");
                                sScript.Append(" E IMPRIMEN SU HUELLA DACTILAR EN LA FECHA.");
                            }
                            else
                            {
                                sScript.Append(" TODAS LAS RESPONSABILIDADES QUE DE ELLA EMANEN Y SE " + strSingularPluralRatifican + " EN SU CONTENIDO.");
                            }
                        }
                        else
                        {
                             sScript.Append(" TODAS LAS RESPONSABILIDADES QUE DE ELLA EMANEN Y SE " + strSingularPluralRatifican + " EN SU CONTENIDO Y LO " + strSingularPluralFirma);
                             sScript.Append(" E IMPRIMEN SU HUELLA DACTILAR EN LA FECHA.");
                        }

                        if (ExisteInterorete)
                        {
                            sScript.Append(" POR SU PARTE " + strGenerpLA_ELInterprete + " " + strPluralSingularInterprete + " " + strPluralSinguralDeclaraInterprete + " BAJO SU RESPONSABILIDAD QUE LA TRADUCCIÓN QUE " + strPluralSingularInterpreteHA + " REALIZADO ES CONFORME Y EXACTA.");// + StrSingularPluralQuien + " " + strSingularPluralsON + " " + strSingularPluralHabilInter);
                        }
                        sScript.Append("</p>");
                        sScript.Append("<tab></tab>");


                        if (strObservacion.Trim() != String.Empty)
                        {
                            sScript.Append("<p align=\"justify\"; style=\"background-color:transparent;\">");
                            sScript.Append("OBSERVACIONES: ");
                            sScript.Append(strObservacion.ToUpper());
                            sScript.Append("</p>");

                        }


                        bool bExisteInterprete = false;


                        foreach (DataRow Row in dt.Rows)
                        {

                            if ((int)Enumerador.enmNotarialTipoParticipante.PADRE == Convert.ToUInt32(Row["sTipoParticipanteId"]))
                            {
                                if (Row["pers_vDescripcionIncapacidad"].ToString().Trim() != String.Empty)
                                {
                                    bExisteTestigoRuego = true;

                                }
                            }

                            if ((int)Enumerador.enmNotarialTipoParticipante.MADRE == Convert.ToUInt32(Row["sTipoParticipanteId"]))
                            {
                                if (Row["pers_vDescripcionIncapacidad"].ToString().Trim() != String.Empty)
                                {
                                    bExisteTestigoRuego = true;

                                }
                            }

                            if ((int)Enumerador.enmNotarialTipoParticipante.INTERPRETE == Convert.ToUInt32(Row["sTipoParticipanteId"]))
                            {
                                bExisteInterprete = true;
                            }
                        }

                        List<DocumentoFirma> listObjects = new List<DocumentoFirma>();
                        DocumentoFirma objetos = new DocumentoFirma();

                        //List<CBE_PARTICIPANTE> loPARTICIPANTES = (List<CBE_PARTICIPANTE>)HttpContext.Current.Session["ParticipanteContainer"];

                        string strPP = "";

                        #region ObtenerDatosDePersonaRepresentada
                        string strNombrerepresentado = "";
                        string strDocumentoRepresentado = "";

                        string strNombrereCompleto = "";
                        string strDocumentoCompleto = "";

                        foreach (DataRow Row in dt.Rows)
                        {
                            if (strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE Y MADRE")
                            {
                                if (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.APODERADO))
                                {
                                    strNombrerepresentado = Row["vParticipante"].ToString();
                                    if (Convert.ToInt32(Row["peid_sDocumentoTipoId"].ToString()) == (int)Enumerador.enmTipoDocumento.OTROS)
                                    {
                                        strDocumentoRepresentado = Row["peid_vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                    }
                                    else
                                    {
                                        strDocumentoRepresentado = Row["vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                    }
                                }
                            }
                            if (strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A MADRE")
                            {
                                if (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.MADRE))
                                {
                                    strNombrerepresentado = Row["vParticipante"].ToString();
                                    if (Convert.ToInt32(Row["peid_sDocumentoTipoId"].ToString()) == (int)Enumerador.enmTipoDocumento.OTROS)
                                    {
                                        strDocumentoRepresentado = Row["peid_vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                    }
                                    else
                                    {
                                        strDocumentoRepresentado = Row["vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                    }
                                }
                                if (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.APODERADO))
                                {
                                    strNombrereCompleto = Row["vParticipante"].ToString();
                                    if (Convert.ToInt32(Row["peid_sDocumentoTipoId"].ToString()) == (int)Enumerador.enmTipoDocumento.OTROS)
                                    {
                                        strDocumentoCompleto = Row["peid_vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                    }
                                    else
                                    {
                                        strDocumentoCompleto = Row["vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                    }
                                }
                            }
                            else if (strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE")
                            {
                                if (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.PADRE))
                                {
                                    strNombrerepresentado = Row["vParticipante"].ToString();
                                    if (Convert.ToInt32(Row["peid_sDocumentoTipoId"].ToString()) == (int)Enumerador.enmTipoDocumento.OTROS)
                                    {
                                        strDocumentoRepresentado = Row["peid_vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                    }
                                    else
                                    {
                                        strDocumentoRepresentado = Row["vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                    }
                                }
                                if (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.APODERADO))
                                {
                                    strNombrereCompleto = Row["vParticipante"].ToString();
                                    if (Convert.ToInt32(Row["peid_sDocumentoTipoId"].ToString()) == (int)Enumerador.enmTipoDocumento.OTROS)
                                    {
                                        strDocumentoCompleto = Row["peid_vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                    }
                                    else
                                    {
                                        strDocumentoCompleto = Row["vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                    }
                                }
                            }
                            else if (strSubTipoActoViajeMejor == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - PADRE REPRESENTA A MADRE")
                            {
                                if (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.MADRE))
                                {
                                    strNombrerepresentado = Row["vParticipante"].ToString();
                                    if (Convert.ToInt32(Row["peid_sDocumentoTipoId"].ToString()) == (int)Enumerador.enmTipoDocumento.OTROS)
                                    {
                                        strDocumentoRepresentado = Row["peid_vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                    }
                                    else
                                    {
                                        strDocumentoRepresentado = Row["vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                    }
                                }
                                if (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.PADRE))
                                {
                                    strNombrereCompleto = Row["vParticipante"].ToString();
                                    if (Convert.ToInt32(Row["peid_sDocumentoTipoId"].ToString()) == (int)Enumerador.enmTipoDocumento.OTROS)
                                    {
                                        strDocumentoCompleto = Row["peid_vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                    }
                                    else
                                    {
                                        strDocumentoCompleto = Row["vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                    }
                                }
                            }
                            else if (strSubTipoActoViajeMejor == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - MADRE REPRESENTA A PADRE")
                            {
                                if (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.PADRE))
                                {
                                    strNombrerepresentado = Row["vParticipante"].ToString();
                                    if (Convert.ToInt32(Row["peid_sDocumentoTipoId"].ToString()) == (int)Enumerador.enmTipoDocumento.OTROS)
                                    {
                                        strDocumentoRepresentado = Row["peid_vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                    }
                                    else
                                    {
                                        strDocumentoRepresentado = Row["vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                    }
                                }
                                if (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.MADRE))
                                {
                                    strNombrereCompleto = Row["vParticipante"].ToString();
                                    if (Convert.ToInt32(Row["peid_sDocumentoTipoId"].ToString()) == (int)Enumerador.enmTipoDocumento.OTROS)
                                    {
                                        strDocumentoCompleto = Row["peid_vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                    }
                                    else
                                    {
                                        strDocumentoCompleto = Row["vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                    }
                                }
                            }
                        }
                        #endregion

                        foreach (DataRow Row in dt.Rows)
                        {
                            objetos = new DocumentoFirma();
                            if ((Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.PADRE)) ||
                                (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.MADRE)) ||
                                //(Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.APODERADO)) ||
                                (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.INTERPRETE) && bExisteInterprete) ||
                                (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO) && bExisteTestigoRuego))
                            {

                                if (strSubTipoActoViajeMejor != "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A MADRE" &&
                                    strSubTipoActoViajeMejor != "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE" &&
                                    strSubTipoActoViajeMejor != "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE Y MADRE")
                                {
                                    if (bExisteApoderado)
                                    {
                                        if ((Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.PADRE)) ||
                                            (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.MADRE)))
                                        {
                                            strPP = "P.P. ";
                                        }
                                        else
                                        {
                                            strPP = "";
                                        }
                                    }
                                }
                                
                                objetos.vNombreCompleto = strPP + Row["vParticipante"].ToString();

                                if (Convert.ToInt32(Row["peid_sDocumentoTipoId"].ToString()) == (int)Enumerador.enmTipoDocumento.OTROS)
                                {
                                    objetos.vNroDocumentoCompleto = Row["peid_vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                }
                                else
                                {
                                    objetos.vNroDocumentoCompleto = Row["vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                }


                                objetos.vNombreCompletoRepresentar = strNombrerepresentado;
                                objetos.vNroDocumentoCompletoRepresentar = strDocumentoRepresentado;

                                objetos.vSubTipoActa = strSubTipoActoViajeMejor;
                                objetos.sTipoParticipante = Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString());
                                objetos.bIncapacitado = Convert.ToBoolean(Row["pers_bIncapacidadFlag"]);
                                objetos.bImprimirFirma = Convert.ToBoolean(Row["anpa_bFlagFirma"]);

                                objetos.bAplicaHuellaDigital = Convert.ToBoolean(Row["anpa_bFlagHuella"]);

                                if (strSubTipoActoViajeMejor == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - PADRE REPRESENTA A MADRE")
                                {
                                    if (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.MADRE))
                                    {
                                        objetos.vNombreCompleto = strNombrereCompleto;
                                        objetos.vNroDocumentoCompleto = strDocumentoCompleto;
                                    }
                                }
                                else if (strSubTipoActoViajeMejor == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - MADRE REPRESENTA A PADRE")
                                {
                                    if (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.PADRE))
                                    {
                                        objetos.vNombreCompleto = strNombrereCompleto;
                                        objetos.vNroDocumentoCompleto = strDocumentoCompleto;
                                    }
                                }

                                else if (strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A MADRE")
                                {
                                    if (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.PADRE))
                                    {
                                        listObjects.Add(objetos);
                                    }
                                }
                                else if (strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE")
                                {
                                    if (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.MADRE))
                                    {
                                        listObjects.Add(objetos);
                                    }
                                }
                                if (strSubTipoActoViajeMejor != "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A MADRE" &&
                                        strSubTipoActoViajeMejor != "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE")
                                {
                                    listObjects.Add(objetos);
                                }
                            }
                            else if (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.APODERADO))
                            {
                                if (strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A MADRE" ||
                                        strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE")
                                {
                                    objetos.vNombreCompleto = strPP + Row["vParticipante"].ToString();
                                    if (Convert.ToInt32(Row["peid_sDocumentoTipoId"].ToString()) == (int)Enumerador.enmTipoDocumento.OTROS)
                                    {
                                        objetos.vNroDocumentoCompleto = Row["peid_vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                    }
                                    else
                                    {
                                        objetos.vNroDocumentoCompleto = Row["vTipoDocumento"].ToString() + " " + Row["vDocumentoNumero"].ToString();
                                    }


                                    objetos.vNombreCompletoRepresentar = strNombrerepresentado;
                                    objetos.vNroDocumentoCompletoRepresentar = strDocumentoRepresentado;

                                    objetos.vSubTipoActa = strSubTipoActoViajeMejor;
                                    objetos.sTipoParticipante = Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString());
                                    objetos.bIncapacitado = Convert.ToBoolean(Row["pers_bIncapacidadFlag"]);
                                    objetos.bImprimirFirma = Convert.ToBoolean(Row["anpa_bFlagFirma"]);

                                    objetos.bAplicaHuellaDigital = Convert.ToBoolean(Row["anpa_bFlagHuella"]);


                                    if (strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A MADRE")
                                    {
                                        if (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.MADRE))
                                        {
                                            objetos.vNombreCompleto = strNombrereCompleto;
                                            objetos.vNroDocumentoCompleto = strDocumentoCompleto;
                                        }
                                    }
                                    else if (strSubTipoActoViajeMejor == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE")
                                    {
                                        if (Convert.ToInt16(Row["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.PADRE))
                                        {
                                            objetos.vNombreCompleto = strNombrereCompleto;
                                            objetos.vNroDocumentoCompleto = strDocumentoCompleto;
                                        }
                                    }
                                    listObjects.Add(objetos);
                                }
                            }
                        }
                        objetos = new DocumentoFirma();
                        List<string> listPalabrasClaves = new List<string>();
                        //listPalabrasClaves.Add("AUTORIZACIÓN DE VIAJE DE MENOR AL EXTERIOR");
                        listPalabrasClaves.Add("AUTORIZACIÓN DE VIAJE DE MENOR");
                        
                        listPalabrasClaves.Add("CONCLUSIÓN:");
                        //listPalabrasClaves.Add("(" + strSubTipoActoExtraProtocolar + ")");

                        //                        Comun.CrearDocumentoiTextSharpExtProto(null, sScript.ToString(), HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE].ToString(), HttpContext.Current.Server.MapPath("~/Images/Escudo.JPG"), listObjects, false, null, "Certificado_Supervivencia", false);
                        Comun.CrearDocumentoiTextSharpExtProto(null, sScript.ToString(), HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE].ToString(), HttpContext.Current.Server.MapPath("~/Images/Escudo.JPG"), listObjects, true, listPalabrasClaves, "AutorizacionViaje", false, Convert.ToDouble(HttpContext.Current.Session["TamanoLetra"]));

                        rpt = "OK";

                        #endregion
                    }
                    else if (TipoActa == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.CERTIFICADO_SUPERVIVENCIA).ToString())
                    {
                        #region Certificado_Supervivencia

                        ActoNotarialConsultaBL BL = new ActoNotarialConsultaBL();
                        DataTable dt = new DataTable();
                        dt = (DataTable)BL.ReporteSupervivencia(Convert.ToInt32(iActoNotarialId), Convert.ToInt32(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));

                        if (dt.Rows.Count > 0)
                        {

                            StringBuilder sScript = new StringBuilder();
                            sScript.Append("<br />");
                            sScript.Append("<p align=\"center\">");
                            sScript.Append("<h1><font face=\"impact\">CERTIFICADO DE SUPERVIVENCIA</font></h1>");
                            sScript.Append("</p>");
                            sScript.Append("<br />");
                            sScript.Append("<tab></tab>");

                            string strLetraGenero = "O";
                            string vObservacion = string.Empty;

                            if (dt.Rows[0]["vObservaciones"] != null)
                            {
                                if (dt.Rows[0]["vObservaciones"].ToString() != string.Empty)
                                {
                                    vObservacion = dt.Rows[0]["vObservaciones"].ToString();
                                }
                            }

                            if (dt.Rows[0]["GeneroPersona"].ToString().Equals(Enumerador.enmGenero.FEMENINO.ToString()))
                                strLetraGenero = "A";

                            String IdentificadoGeneroFuncionario = String.Empty;
                            //-------------------------------------------------------------
                            //Fecha: 13/12/2017
                            //Objetivo: Si el valos es 1 es mujer / si es 2 es hombre
                            //-------------------------------------------------------------

                            if (dt.Rows[0]["GeneroFuncionario"].ToString().Trim().Equals("2"))
                                IdentificadoGeneroFuncionario = "IDENTIFICADA";
                            else
                                IdentificadoGeneroFuncionario = "IDENTIFICADO";

                            if (IdentificadoGeneroFuncionario == String.Empty) IdentificadoGeneroFuncionario = "IDENTIFICADO";

                            sScript.Append("<p align=\"justify\"; style=\"background-color:transparent;\">");
                            sScript.Append("EN LA CIUDAD DE ");
                            sScript.Append(dt.Rows[0]["Ciudad"].ToString());
                            //sScript.Append(", EL ");
                            sScript.Append(", ");
                            sScript.Append(Util.ObtenerFechaParaDocumentoLegalProtocolar(Comun.FormatearFecha(dt.Rows[0]["Fecha"].ToString())).ToUpper().Trim());
                            //sScript.Append(", EN EL CONSULADO GENERAL DEL PERÚ EN ");
                            if (dt.Rows[0]["NombreOficinaConsular"].ToString().Contains("SECCIÓN"))
                            {
                                sScript.Append(", EN LA ");
                            }
                            else
                            {
                                sScript.Append(", EN EL ");
                            }
                            sScript.Append(dt.Rows[0]["NombreOficinaConsular"].ToString().ToUpper());
                            //sScript.Append(dt.Rows[0]["OficinaConsular"].ToString().ToUpper());
                            sScript.Append(", YO, ");
                            sScript.Append(dt.Rows[0]["NombreFuncionario"].ToString());
                            sScript.Append(", " + IdentificadoGeneroFuncionario);
                            sScript.Append(" CON DOCUMENTO NACIONAL DE IDENTIDAD ");
                            sScript.Append(dt.Rows[0]["NumeroDocumentoFuncionario"].ToString());
                            sScript.Append(", ");
                            sScript.Append(dt.Rows[0]["CargoFuncionario"].ToString());
                            sScript.Append(" EN ");
                            sScript.Append(dt.Rows[0]["CiudadOficinaFuncionario"].ToString());
                            sScript.Append("; CERTIFICO: QUE HE VERIFICADO LA SUPERVIVENCIA DE ");
                            sScript.Append(dt.Rows[0]["NombrePersona"].ToString());
                            if (dt.Rows[0]["NacionalidadPersona"].ToString() != string.Empty)
                            {
                                sScript.Append(", DE NACIONALIDAD ");
                                sScript.Append(dt.Rows[0]["NacionalidadPersona"].ToString());
                            }
                            sScript.Append(", IDENTIFICAD" + strLetraGenero);
                            sScript.Append(" CON ");
                            sScript.Append(dt.Rows[0]["TipoDocumentoPersona"].ToString());
                            sScript.Append(" ");
                            sScript.Append(dt.Rows[0]["NumeroDocumentoPersona"].ToString());
                            if (dt.Rows[0]["EstadoCivilPersona"].ToString() != string.Empty)
                            {
                                sScript.Append(", DE ESTADO CIVIL ");
                                sScript.Append(dt.Rows[0]["EstadoCivilPersona"].ToString());
                            }
                            if (dt.Rows[0]["ResidenciaPersona"].ToString() != string.Empty)
                            {
                                sScript.Append(", DOMICILIAD");
                                sScript.Append(strLetraGenero);
                                sScript.Append(" EN ");
                                sScript.Append(dt.Rows[0]["ResidenciaPersona"].ToString());
                            }
                            sScript.Append(".");
                            sScript.Append("</p>");

                            if (vObservacion != string.Empty)
                            {
                                sScript.Append("<p align=\"justify\"; style=\"background-color:transparent;\">");
                                sScript.Append(vObservacion);
                                sScript.Append("</p>");
                            }
                            sScript.Append("<br />");
                            sScript.Append("<tab></tab>");

                            List<DocumentoFirma> listObjects = new List<DocumentoFirma>();
                            DocumentoFirma objetos = new DocumentoFirma();
                            objetos.vNombreCompleto = dt.Rows[0]["NombrePersona"].ToString();
                            objetos.vNroDocumentoCompleto = dt.Rows[0]["TipoDocumentoPersona"].ToString() + " " + dt.Rows[0]["NumeroDocumentoPersona"].ToString();

                            objetos.bIncapacitado = false;
                            objetos.bImprimirFirma = true;

                            objetos.bAplicaHuellaDigital = ImprimirFirma;

                            //----------------------------------------------------------------------
                            //Fecha: 07/03/2017
                            //Autor: Miguel Márquez Beltrán
                            //Objetivo: Imprimir la fecha de expedición
                            //----------------------------------------------------------------------                            
                            objetos.sFechaExpedicion = dt.Rows[0]["vFechaExpedicion"].ToString();
                            //----------------------------------------------------------------------                            
                            listObjects.Add(objetos);
                            Comun.CrearDocumentoiTextSharpExtProto(null, sScript.ToString(), HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE].ToString(), HttpContext.Current.Server.MapPath("~/Images/Escudo.JPG"), listObjects, false, null, "Certificado_Supervivencia", false, Convert.ToDouble(HttpContext.Current.Session["TamanoLetra"]));

                            rpt = "OK";

                        #endregion
                        }

                    }
                }
                else
                {
                    rpt = "Se ha encontrado problemas con la obtencion de algunos datos, refresque la pagina por favor.";
                }
                return rpt;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [System.Web.Services.WebMethod]
        public static string InsertCuerpo(string iActoNotarialCuerpoId, string iActoNotarialId, string cuerpo)
        {

            try
            {

                //iActoNotarialId = ViewState[Constantes.CONST_SESION_ACTONOTARIAL_ID].ToString();

                if (iActoNotarialId != "0")
                {
                    if (cuerpo == null)
                    {
                        return "No se ha guardado la información del cuerpo.";
                    }


                    CBE_CUERPO lCuerpo = new CBE_CUERPO();
                    lCuerpo.ancu_iActoNotarialCuerpoId = Convert.ToInt32(iActoNotarialCuerpoId);
                    lCuerpo.ancu_iActoNotarialId = Convert.ToInt64(iActoNotarialId);
                    lCuerpo.ancu_vCuerpo = cuerpo;
                    lCuerpo.ancu_vFirmaIlegible = "";
                    //lCuerpo.ancu_sUsuarioCreacion = Convert.ToInt16(sUsuarioCreacion.Value);
                    //lCuerpo.ancu_vIPCreacion = hdn_acno_vIPCreacion.Value;
                    //lCuerpo.ancu_sUsuarioModificacion = Convert.ToInt16(hdn_acno_sUsuarioModificacion.Value);
                    //lCuerpo.ancu_vIPModificacion = hdn_acno_vIPModificacion.Value;

                    ActoNotarialMantenimiento mnt = new ActoNotarialMantenimiento();
                    mnt.Insertar_ActoNotarialCuerpoExtraProtocolar(lCuerpo, null);

                    if (lCuerpo.ancu_iActoNotarialCuerpoId != 0 && lCuerpo.Message == null)
                    {
                        //hdn_ancu_iActoNotarialCuerpoId.Value = lCuerpo.ancu_iActoNotarialCuerpoId.ToString();
                        //EjecutarScript("EnableTabIndex(iTabAprobacionPagoIndice);" + Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CUERPO", "Registro almacenado correctamente."));
                        //Session["anep_iSelectedTabId"] = iTabAprobacionPagoIndice;

                        //hdn_bCheckAceptacion.Value = "1";
                        //hdn_bCuerpoGrabado.Value = "1";
                        //ddlFuncionario.Enabled = false;
                        //ctrlToolTipoActo.btnGrabar.Enabled = false;
                        //updTipoActo.Update();
                    }
                    else
                    {
                        if (lCuerpo.ancu_iActoNotarialCuerpoId == 0 && lCuerpo.Message != null)
                        {
                            //EjecutarScript("EnableTabIndex(iTabAprobacionPagoIndice);" + Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "CUERPO", lCuerpo.Message));
                            //Session["anep_iSelectedTabId"] = iTabAprobacionPagoIndice;

                            //hdn_bCheckAceptacion.Value = "1";
                            //hdn_bCuerpoGrabado.Value = "1";
                        }
                    }

                    //HabilitarBotonesImpresion(true);

                    //GrabarCuerpo(Convert.ToInt32(hdn_acno_iActoNotarialId.Value));

                    //Session["anep_iSelectedTabId"] = iTabAprobacionPagoIndice;
                    //if (Session["PasoActualTab"] == null)
                    //{
                    //    Session["PasoActualTab"] = 1;
                    //}
                    //if (Convert.ToInt32(Session["PasoActualTab"]) < iTabAprobacionPagoIndice)
                    //{
                    //    Session["PasoActualTab"] = iTabAprobacionPagoIndice.ToString();
                    //    EjecutarScript("SetTabs('" + iTabAprobacionPagoIndice.ToString() + "','" + ddlTipoActoNotarialExtra.SelectedValue.ToString() + "');");
                    //}
                    //else
                    //{
                    //    EjecutarScript("EnableTabIndex(" + iTabAprobacionPagoIndice.ToString() + ");");
                    //}
                }
                else
                {
                    //EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", "Se debe agregar el tipo de acto notarial."));
                    return "Se debe agregar el tipo de acto notarial.";
                }

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }

        protected void btninterprete_Click(object sender, EventArgs e)
        {
            int _indice = 0;

            if (Convert.ToString((int)Enumerador.enmNotarialTipoParticipante.INTERPRETE) == ddlRegistroTipoParticipante.SelectedValue)
            {
                int iIndiceComboCui = Util.ObtenerIndiceCombo(ddlRegistroIdioma, "10108");

                if (iIndiceComboCui >= 0)
                    ddlRegistroIdioma.Items[iIndiceComboCui].Enabled = false;
                foreach (GridViewRow item in grd_Participantes.Rows)
                {
                    if (item.Cells[0].Text == Convert.ToString(Enumerador.enmNotarialTipoParticipante.OTORGANTE))
                    {
                        List<CBE_PARTICIPANTE> Participantes = new List<CBE_PARTICIPANTE>();
                        if (Session["ParticipanteContainer"] != null)
                        {
                            Participantes = ((List<CBE_PARTICIPANTE>)Session["ParticipanteContainer"]).Where(x => x.anpa_cEstado != "E").ToList();
                            ddlRegistroIdioma.SelectedValue = Participantes[_indice].Persona.pers_sIdiomaNatalId.ToString();
                            //ddlRegistroIdioma.Enabled = false;
                            return;
                        }
                    }
                    _indice += 1;
                }

            }
            else
            {
                int iIndiceComboCui = Util.ObtenerIndiceCombo(ddlRegistroIdioma, "10108");

                if (iIndiceComboCui >= 0)
                    ddlRegistroIdioma.Items[iIndiceComboCui].Enabled = true;
            }
            //UpdatePanel4.Update();
        }

        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 29/08/2016
        // Objetivo: Validar que el idioma del otorgante sea el castellano
        //           sino es así, deberá incluir un interprete.
        //------------------------------------------------------------------------

        bool ValidarIdiomaCastellanoOtorgante(List<CBE_PARTICIPANTE> lParticipantes, ref string strIdiomaExtranjero)
        {
            bool bconforme = true;
            string strIdioma = "";
            for (int i = 0; i < lParticipantes.Count; i++)
            {
                if (lParticipantes[i].acpa_sTipoParticipanteId_desc.ToUpper().Trim() == "PADRE" || lParticipantes[i].acpa_sTipoParticipanteId_desc.ToUpper().Trim() == "MADRE" || lParticipantes[i].acpa_sTipoParticipanteId_desc.ToUpper().Trim() == "OTORGANTE")
                {
                    if (lParticipantes[i].anpa_cEstado != "E" && lParticipantes[i].pers_sIdiomaNatalId_desc != "CASTELLANO" && lParticipantes[i].pers_sIdiomaNatalId_desc != null)
                    {
                        strIdioma = lParticipantes[i].pers_sIdiomaNatalId_desc.ToUpper().Trim();
                        bconforme = ValidarIdiomaExtranjeroInterprete(lParticipantes, strIdioma);
                        if (bconforme == false)
                            break;
                    }
                }
            }

            if (bconforme == false)
            { strIdiomaExtranjero = strIdioma; }
            else
            { strIdiomaExtranjero = ""; }

            return bconforme;
        }
        bool ValidarIdiomaExtranjeroInterprete(List<CBE_PARTICIPANTE> lParticipantes, string strIdioma)
        {
            bool bconforme = false;
            for (int i = 0; i < lParticipantes.Count; i++)
            {
                if (lParticipantes[i].acpa_sTipoParticipanteId_desc.ToUpper().Trim() == "INTERPRETE")
                {
                    if (lParticipantes[i].anpa_cEstado != "E" && lParticipantes[i].pers_sIdiomaNatalId_desc == strIdioma)
                    {
                        bconforme = true;
                        break;
                    }
                }
            }

            return bconforme;
        }


        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 09/03/2016
        // Objetivo: Al realizar un cambio del numero de documento y salir del foco
        //          se activa este evento para realizar la busqueda.
        //------------------------------------------------------------------------

        protected void txtDocumentoNro_TextChanged(object sender, EventArgs e)
        {
            ImageClickEventArgs ex = null;

            imgBuscar_Click(sender, ex);
        }

        protected void imgBuscar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                BuscarPersona();
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
        private void BuscarPersona(Int16 TipoDocumento = 0, string NumeroDocumento = "", Int16 sTipParticipante = 0,
            bool? bFlagHuella = null,Int64 iPersonaIncapacitada = 0)
        {
            try
            {
                if (TipoDocumento == 0 && NumeroDocumento == "")
                {
                    TipoDocumento = Convert.ToInt16(ddlRegistroTipoDoc.SelectedValue.ToString());
                    NumeroDocumento = txtDocumentoNro.Text;
                }
                else
                {
                    ddlRegistroTipoParticipante.SelectedValue = sTipParticipante.ToString();
                    ddlRegistroTipoDoc.SelectedValue = TipoDocumento.ToString();
                    txtDocumentoNro.Text = NumeroDocumento;
                }

                if (ddlRegistroTipoDoc.SelectedIndex == 0)
                {
                    ddlRegistroTipoDoc.Style.Add("border", "solid Red 1px");
                    ddlRegistroTipoDoc.Focus();
                    return;
                }

                //Session["iParticipanteResidenciaId"] = null;

                PersonaConsultaBL lPersonaConsultaBL = new PersonaConsultaBL();

                _dtPersona = lPersonaConsultaBL.ObtenerDatosPersona(TipoDocumento, NumeroDocumento);

                //RE_PERSONA lPersona = obtener_personaCodeBehind(null, ddlRegistroTipoDoc.SelectedValue.ToString(), txtDocumentoNro.Text);

                //Session["PersonaAlmacenada"] = null;
                //Session["vCamposPersonaAlmacenada"] = null;

                if (_dtPersona.Rows.Count > 0)
                {
                    LimpiarTabParticipantePrincipales();

                    hCodPersona.Value = _dtPersona.Rows[0]["pers_iPersonaId"].ToString();
                    hPerResidenciaID.Value = _dtPersona.Rows[0]["pere_iResidenciaId"].ToString();

                    ddlRegistroTipoDoc.SelectedValue = _dtPersona.Rows[0]["peid_sDocumentoTipoId"].ToString();
                    ddlRegistroTipoDoc.Enabled = false;

                    txtDescOtroDocumento.Text = _dtPersona.Rows[0]["peid_vTipoDocumento"].ToString();
                    if (ddlRegistroTipoDoc.SelectedValue == Convert.ToString((int)Enumerador.enmTipoDocumento.OTROS)
                        || ddlRegistroTipoDoc.SelectedValue == Convert.ToString((int)Enumerador.enmTipoDocumento.PASAPORTE_E))
                    {
                        LblOtroDocumento.Visible = true;
                        txtDescOtroDocumento.Visible = true;
                        txtDescOtroDocumento.Enabled = true;
                        lblDescOtroDocObl.Visible = true;

                        if (ddlRegistroTipoDoc.SelectedValue == Convert.ToString((int)Enumerador.enmTipoDocumento.OTROS))
                        {
                            LblOtroDocumento.Text = "Otro documento :";
                        }
                        else
                        {
                            LblOtroDocumento.Text = "Tipo Pasaporte :";
                        }
                    }
                    else
                    {
                        LblOtroDocumento.Visible = false;
                        txtDescOtroDocumento.Visible = false;
                        txtDescOtroDocumento.Enabled = false;
                        lblDescOtroDocObl.Visible = false;
                    }

                    txtDocumentoNro.Text = _dtPersona.Rows[0]["peid_vDocumentoNumero"].ToString();
                    txtDocumentoNro.Enabled = false;
                    //-------------------------------------------------

                    txtApePaterno.Text = _dtPersona.Rows[0]["pers_vApellidoPaterno"].ToString();

                    txtApeMaterno.Text = _dtPersona.Rows[0]["pers_vApellidoMaterno"].ToString();

                    txtApepCas.Text = _dtPersona.Rows[0]["pers_vApellidoCasada"].ToString();


                    txtNombres.Text = _dtPersona.Rows[0]["pers_vNombres"].ToString();

                    if (Convert.ToDateTime(_dtPersona.Rows[0]["pers_dNacimientoFecha"]).Year >= 1900)
                    {
                        if (Convert.ToDateTime(_dtPersona.Rows[0]["pers_dNacimientoFecha"]).ToShortDateString() == "01/01/1900")
                        { txtFecNac.Text = ""; }
                        else { txtFecNac.set_Value = Convert.ToDateTime(_dtPersona.Rows[0]["pers_dNacimientoFecha"]); }
                        //Session["vCamposPersonaAlmacenada"] += "nacimiento,";
                    }
                    else
                    {
                        //BtnCalcularEdad.Visible = true;
                    }

                    if (Convert.ToInt16(_dtPersona.Rows[0]["pers_sIdiomaNatalId"].ToString()) > 0)
                    {
                        ddlRegistroIdioma.SelectedValue = _dtPersona.Rows[0]["pers_sIdiomaNatalId"].ToString();
                        //Session["vCamposPersonaAlmacenada"] += "idioma,";
                    }


                    if (Convert.ToInt16(_dtPersona.Rows[0]["pers_sGeneroId"].ToString()) > 0)
                    {
                        ddlRegistroGenero.SelectedValue = _dtPersona.Rows[0]["pers_sGeneroId"].ToString();
                        //Session["vCamposPersonaAlmacenada"] += "genero,";
                    }

                    if (Convert.ToInt16(_dtPersona.Rows[0]["pers_sEstadoCivilId"].ToString()) > 0)
                    {
                        ddlRegistroEstadoCivil.SelectedValue = _dtPersona.Rows[0]["pers_sEstadoCivilId"].ToString();
                        //Session["vCamposPersonaAlmacenada"] += "estadocivil,";
                    }

                    if (Convert.ToInt32(ddlRegistroTipoDoc.SelectedValue) == (int)Enumerador.enmTipoDocumento.PASAPORTE_E)
                    {
                        if (Convert.ToInt16(_dtPersona.Rows[0]["pers_spaisid"].ToString()) > 0)
                        {
                            ddlPaisOrigen.SelectedValue = _dtPersona.Rows[0]["pers_spaisid"].ToString();
                            RefrescarNacionalidad();
                        }
                        else
                        {
                            ddlPaisOrigen.SelectedIndex = 0;
                            ddlRegistroNacionalidad.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt16(_dtPersona.Rows[0]["pers_sNacionalidadId"].ToString()) > 0)
                        {
                            ddlRegistroNacionalidad.SelectedValue = _dtPersona.Rows[0]["pers_sNacionalidadId"].ToString();
                            //Session["vCamposPersonaAlmacenada"] += "nacionalidad,";
                        }
                        if (Convert.ToInt16(_dtPersona.Rows[0]["pers_spaisid"].ToString()) > 0)
                        {
                            ddlPaisOrigen.SelectedValue = _dtPersona.Rows[0]["pers_spaisid"].ToString();
                            RefrescarNacionalidad();
                        }
                        else
                        {
                            ddlPaisOrigen.SelectedIndex = 0;
                            ddlRegistroNacionalidad.SelectedIndex = 0;
                        }
                    }

                    if (sTipParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO))
                    {
                        if (iPersonaIncapacitada != 0)
                        {
                            CargarComboIncapacitados();

                            bool bExisteElemento = false;
                            for (int i = 0; i < ddlOtorganteIncapacitado.Items.Count; i++)
                            {
                                if (ddlOtorganteIncapacitado.Items[i].Value.Equals(iPersonaIncapacitada.ToString()))
                                {
                                    ddlOtorganteIncapacitado.SelectedValue = iPersonaIncapacitada.ToString();
                                    bExisteElemento = true;
                                    break;
                                }
                            }
                            if (!bExisteElemento)
                            {
                                ddlOtorganteIncapacitado.SelectedIndex = 0;
                            }
                            ddlOtorganteIncapacitado.Enabled = true;
                        }
                    }



                    //------------------------------------------------------------------------
                    //Autor: Miguel Márquez Beltrán
                    //Fecha: 19/09/2017
                    //Objetivo: Poner en blanco la incapacidad cuando se busca la persona.
                    //------------------------------------------------------------------------

                    txtRegistroTipoIncapacidad.Enabled = true;
                    txtRegistroTipoIncapacidad.Text = "";

                    if (Convert.ToInt16(_dtPersona.Rows[0]["pers_sOcupacionId"].ToString()) > 0)
                    {
                        ddlRegistroProfesion.SelectedValue = _dtPersona.Rows[0]["pers_sOcupacionId"].ToString();
                        //Session["vCamposPersonaAlmacenada"] += "profesion,";
                    }

                    imgBuscar.Enabled = false;

                    if (_dtPersona.Rows[0]["resi_vResidenciaDireccion"].ToString() != "")
                    {
                        string ubigeo = _dtPersona.Rows[0]["resi_cResidenciaUbigeo"].ToString();
                        if (ubigeo != "")
                        {
                            if (ubigeo.Length == 6)
                            {
                                //Session["UbigeoSupervivencia"] = ubigeo;

                                ddl_UbigeoPais.SelectedValue = ubigeo.Substring(0, 2);
                                ddl_UbigeoPais_SelectedIndexChanged(null, null);

                                ddl_UbigeoRegion.SelectedValue = ubigeo.Substring(2, 2);
                                ddl_UbigeoRegion_SelectedIndexChanged(null, null);

                                ddl_UbigeoCiudad.SelectedValue = ubigeo.Substring(4, 2);
                                //Session["vCamposPersonaAlmacenada"] += "ubigeo,";
                            }
                        }

                        //Session["iParticipanteResidenciaId"] = _dtPersona.Rows[0]["pere_iResidenciaId"].ToString();

                        txtDireccion.Text = _dtPersona.Rows[0]["resi_vResidenciaDireccion"].ToString().Trim();

                        if (!string.IsNullOrEmpty(_dtPersona.Rows[0]["resi_vCodigoPostal"].ToString()))
                        {
                            txtCodigoPostal.Text = _dtPersona.Rows[0]["resi_vCodigoPostal"].ToString().Trim();
                        }
                        else
                        {
                            txtCodigoPostal.Text = string.Empty;
                        }

                        if (txtDireccion.Text != string.Empty)
                        {
                            //---------------------------------------------------------------
                            //Fecha: 03/04/2017
                            //Autor: Miguel Márquez Beltrán
                            //Objetivo: Actualizar la dirección y la ubicación geográfica
                            //---------------------------------------------------------------
                            //txtDireccion.Enabled = false;
                            //Session["vCamposPersonaAlmacenada"] += "domicilio,";
                        }
                        else
                            txtDireccion.Enabled = true;
                    }
                    else
                    {
                        ddl_UbigeoPais.Enabled = true;
                        ddl_UbigeoRegion.Enabled = true;
                        ddl_UbigeoCiudad.Enabled = true;
                    }

                    if (bFlagHuella != null)
                    {
                        if (Convert.ToBoolean(_dtPersona.Rows[0]["pers_bIncapacidadFlag"]))
                        {
                            chkIncapacitado.Checked = true;
                            chkNoHuella.Checked = !((bool)bFlagHuella);
                            txtRegistroTipoIncapacidad.Text = _dtPersona.Rows[0]["pers_vDescripcionIncapacidad"].ToString();
                        }
                        else
                        {
                            chkIncapacitado.Checked = false;
                            txtRegistroTipoIncapacidad.Text = "";
                        }
                    }

                    //Session["PersonaAlmacenada"] = "1";
                    //---------------------------------------------------------------
                    //Fecha: 06/03/2017
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Cambiar el texto de las listas (estado civil y nacionalidad)
                    //         de acuerdo al genero masculino o femenino.
                    //---------------------------------------------------------------                
                    ActualizarGenero();
                    Incapacitado();
                    //Btn_AgregarParticipante.Focus();

                }
                else
                {
                    hCodPersona.Value = "0";
                    //BtnCalcularEdad.Visible = true;
                    string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Actuación Notarial", "El número de documento ingresado no existe.");
                    //strScript += " HabilitarPorParticipante(true);HabilitarApellidoCasada();";
                    //strScript += " HabilitarApellidoCasada();";
                    EjecutarScript(strScript);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void ddlTamanoLetra_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTamanoLetra.SelectedItem.Text.Trim().Length > 0)
            {
                Session["TamanoLetra"] = ddlTamanoLetra.SelectedItem.Text;
            }
        }

        //--------------------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Objetivo: Asignar los datos del participante automaticamente 
        // para el tipo de acto notarial: Certificado de Supervivencia
        //--------------------------------------------------------------------
        private void AsignarParticipanteRecurrente()
        {
            try
            {


                string strTipoDocumento = ViewState["iDocumentoTipoId"].ToString();
                string strNumeroDocumento = ViewState["NroDoc"].ToString();
                if (Convert.ToInt16(strTipoDocumento) == Convert.ToInt16(Enumerador.enmTipoDocumento.CUI))
                {
                    return;
                }
                if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.CERTIFICADO_SUPERVIVENCIA).ToString())
                {
                    BuscarPersona(Convert.ToInt16(strTipoDocumento), strNumeroDocumento, Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.TITULAR));
                }
                else if (ddlTipoActoNotarialExtra.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.PODER_FUERA_REGISTRO).ToString())
                {
                    BuscarPersona(Convert.ToInt16(strTipoDocumento), strNumeroDocumento, Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.OTORGANTE));
                }

                AgregarParticipante();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

        }

        protected void ddlPaisOrigen_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefrescarNacionalidad();
        }

        private void RefrescarNacionalidad()
        {
            //--------------------------------------------------
            //Fecha: 03/04/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Consultar Paises desde un ViewState.
            //--------------------------------------------------  
            DataTable dtPaises = new DataTable();

            dtPaises = (DataTable)ViewState["Paises"];
            //--------------------------------------------------

            LblDescGentilicio.Text = Comun.AsignarGentilicio(dtPaises, ddlPaisOrigen, ddlRegistroGenero).ToUpper();
            Comun.AsignarNacionalidad(Session, ddlRegistroNacionalidad, ddlPaisOrigen);
            string strPaisPeruId = System.Web.Configuration.WebConfigurationManager.AppSettings["Pais_PeruId"].ToString();
            if (ddlPaisOrigen.SelectedValue.Equals(strPaisPeruId))
            {
                ddlRegistroNacionalidad.SelectedValue = Convert.ToString((int)Enumerador.enmNacionalidad.PERUANA);
            }
            else
            {
                ddlRegistroNacionalidad.SelectedValue = Convert.ToString((int)Enumerador.enmNacionalidad.EXTRANJERA);
            }
            ActualizarGenero();
        }

        //--------------------------------------------------------------------
        //-------------------------------------------
        //Fecha: 06/07/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Actualizar el Genero
        //-------------------------------------------
        private void ActualizarGenero()
        {
            string strSexo = ddlRegistroGenero.SelectedItem.Text;
            string strVocal = "";
            if (strSexo == Convert.ToString(Enumerador.enmGenero.MASCULINO))
            {
                strVocal = "O";
            }
            else {
                strVocal = "A";
            }

            foreach(ListItem Item in ddlRegistroEstadoCivil.Items)
            {
                int iLargo = Item.Text.Length;

                if (Item.Text != "- SELECCIONAR -")
                { Item.Text = Item.Text.Substring(0, iLargo - 1) + strVocal; }
                
            }

//            string strScript = string.Empty;
//            strScript = @"$(function(){{
//                                        ObtenerElementosGenero();
//                                    }});";
//            strScript = string.Format(strScript);
//            ScriptManager.RegisterStartupScript(Page, typeof(Page), "Genero", strScript, true);
        }

        private void HabilitarApellidoCasada()
        {
            txtApepCas.Enabled = false;
            if (ddlRegistroGenero.SelectedItem.Text == Convert.ToString(Enumerador.enmGenero.FEMENINO))
            {
                if (ddlRegistroEstadoCivil.SelectedItem.Text == "CASADA")
                {
                    txtApepCas.Enabled = true;
                }
            }
        }
        //-------------------------------------------
        //Fecha: 06/07/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Consultar el gentilicio
        //-------------------------------------------
        protected void ddlRegistroGenero_SelectedIndexChanged(object sender, EventArgs e)
        {
            //---------------------------------------------------
            //Fecha: 03/04/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Asignar el ViewState a DataTable Paises.
            DataTable dtPaises = new DataTable();

            dtPaises = (DataTable)ViewState["Paises"];
            //---------------------------------------------------

            LblDescGentilicio.Text = Comun.AsignarGentilicio(dtPaises, ddlPaisOrigen, ddlRegistroGenero).ToUpper();
//            LblDescGentilicio.Text = Comun.AsignarGentilicio(Session, ddlPaisOrigen, ddlRegistroGenero).ToUpper();
            ActualizarGenero();
            HabilitarApellidoCasada();
            ddlRegistroEstadoCivil.Enabled = true;
        }


        private void ImprimirSupervivencia()
        {
            ActoNotarialConsultaBL BL = new ActoNotarialConsultaBL();
            DataTable dt = new DataTable();
            dt = (DataTable)BL.ReporteSupervivencia(Convert.ToInt32(ViewState[Constantes.CONST_SESION_ACTONOTARIAL_ID]),
                Convert.ToInt32(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));



            if (dt.Rows.Count > 0)
            {
                #region Formato
                StringBuilder sScript = new StringBuilder();
                sScript.Append("<br />");
                sScript.Append("<p align=\"center\">");
                sScript.Append("<h1><font face=\"impact\">CERTIFICADO DE SUPERVIVENCIA</font></h1>");
                sScript.Append("</p>");
                sScript.Append("<br />");
                sScript.Append("<tab></tab>");

                StringBuilder sContenido = new StringBuilder();
                sContenido.Append(ObtenerDocumentoCertificado(dt));
                //sContenido.Replace("</p>", "====</p>"); // MDIAZ

                sScript.Append(sContenido);
                #endregion

                #region Firmas
                List<DocumentoFirma> listObjects = new List<DocumentoFirma>();
                DocumentoFirma objetos = new DocumentoFirma();

                objetos.vNombreCompleto = dt.Rows[0]["NombrePersona"].ToString();
                objetos.vNroDocumentoCompleto = dt.Rows[0]["TipoDocumentoPersona"].ToString() + " " + dt.Rows[0]["NumeroDocumentoPersona"].ToString();


                objetos.bIncapacitado = false;
                objetos.bImprimirFirma = true;

                objetos.bAplicaHuellaDigital = chkImprimirFirmaTitular1.Checked;

                //----------------------------------------------------------------------
                //Fecha: 07/03/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Imprimir la fecha de expedición
                //----------------------------------------------------------------------
                objetos.sFechaExpedicion = dt.Rows[0]["vFechaExpedicion"].ToString();
                //----------------------------------------------------------------------
                listObjects.Add(objetos);

                #endregion

                Comun.CrearDocumentoiTextSharpExtProto(this.Page, sScript.ToString(), Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE].ToString(), Server.MapPath("~/Images/Escudo.JPG"), listObjects, false, null, "Certificado_Supervivencia", true, Convert.ToDouble(HttpContext.Current.Session["TamanoLetra"]));
            }

        }

        protected void ddlTipoActoNotarialExtra_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddlTipoActoNotarialExtra.SelectedValue) == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR))
            {
                ddlSubTipoNotarialExtra.SelectedIndex = 0;
                lbl_subTipoActoNotarialExtra.Visible = true;
                ddlSubTipoNotarialExtra.Visible = true;
                ddlCondiciones.Visible = true;
                lblCondiciones.Visible = true;
                ddlSubTipoNotarialExtra.Focus();
            }
            else
            {
                lbl_subTipoActoNotarialExtra.Visible = false;
                ddlSubTipoNotarialExtra.Visible = false;
                ddlCondiciones.Visible = false;
                lblCondiciones.Visible = false;
                if (ddlTipoActoNotarialExtra.SelectedIndex > 0)
                {
                    ddlFuncionario.Focus();
                }
            }
            // Jonatan Silva Cachay - 07/08/2017
            pintarLista();
        }


        //---------------------------------------------------------------------------
        //Fecha: 17/10/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Grabar FlagFirma del Titular si es Certificado de Supervivencia
        //---------------------------------------------------------------------------

        public static void ActualizarParticipante(Int64 intActoNotarialId, Int16 intTipoActoNotarial, bool bTieneFirmaTitular)
        {
            if (intTipoActoNotarial == Convert.ToInt16(Enumerador.enmExtraprotocolarTipo.CERTIFICADO_SUPERVIVENCIA))
            {
                Int16 intTipoParticipanteid = Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.TITULAR);

                ParticipanteConsultaBL participanteBL = new ParticipanteConsultaBL();
                RE_ACTONOTARIALPARTICIPANTE participanteBE = new RE_ACTONOTARIALPARTICIPANTE();

                participanteBE.anpa_iActoNotarialId = intActoNotarialId;
                participanteBE.anpa_sTipoParticipanteId = intTipoParticipanteid;

                participanteBE = participanteBL.Obtener_ActoNotarial(participanteBE);

                participanteBE.anpa_bFlagFirma = bTieneFirmaTitular;
                participanteBE.anpa_dFechaModificacion = DateTime.Now;
                participanteBE.anpa_vIPModificacion = Util.ObtenerDireccionIP();
                participanteBE.anpa_sUsuarioModificacion = Convert.ToInt16((HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]).ToString());

                
                ActoNotarialExtraProtocolarMantenimiento ACTO_ExtraProtocolarBL = new ActoNotarialExtraProtocolarMantenimiento();

                participanteBE = ACTO_ExtraProtocolarBL.actualizar(participanteBE);
            }
            //---------------------------------------------------------------------------
        }
        //Jonatan -- 20/07/2017 -- reimprimir un autoadhesivo
        void ctrlReimprimirbtn_btnReimprimirHandler()
        {
            if (ctrlReimprimirbtn1.SeImprime == "OK")
            {
                CargarActoNotarial();
                //PintarDatosPestaniaGeneral();
                //btnVistaPrev.Enabled = true;
                ctrlReimprimirbtn1.Activar = chkImpresion.Checked;
            }
            else
            {
                if (ctrlReimprimirbtn1.Activar)
                {
                    Btn_ImprimirAutoadhesivo.Enabled = false;
                }
            }
            //if (HFGUID.Value.Length > 0)
            //{
            //    Response.Redirect("FrmActuacionNotarialExtraProtocolar.aspx?cod=1&GUID=" + HFGUID.Value);
            //}
            //else
            //{
            string codPersona = Request.QueryString["CodPer"].ToString();
            Session[Constantes.CONST_SESION_ACTONOTARIAL_ID] = ViewState[Constantes.CONST_SESION_ACTONOTARIAL_ID];
            Session[Constantes.CONST_SESION_ACTUACION_ID] = ViewState[Constantes.CONST_SESION_ACTUACION_ID];
            if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
            {
                Response.Redirect("FrmActuacionNotarialExtraProtocolar.aspx?cod=1&CodPer=" + codPersona + "&Juridica=1", false);
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
                    Response.Redirect("FrmActuacionNotarialExtraProtocolar.aspx?cod=1&CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                }
                else
                {
                    Response.Redirect("FrmActuacionNotarialExtraProtocolar.aspx?cod=1&CodPer=" + codPersona, false);
                }
            }
            
            //}
        }

        // jonatan -- dar de baja un autoadhesivo
        void ctrlBajaAutoadhesivo_btnAnularAutoahesivo()
        {
            ctrlBajaAutoadhesivo1.CodInsumo = hCodAutoadhesivo.Value;
            Comun.EjecutarScript(this, "Popup(" + hCodAutoadhesivo.Value.ToString() + ");");
        }
        // jonatan -- dar de baja un autoadhesivo
        void ctrlBajaAutoadhesivo_btnAceptarAnularAutoahesivo()
        {
            ctrlBajaAutoadhesivo1.CodInsumo = hCodAutoadhesivo.Value;
            //PintarDatosPestaniaGeneral();
            //String scriptMover = @"$(function(){{ MoveTabIndex(5);}});";
            //ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);
            //updRegPago.Update();
            //if (HFGUID.Value.Length > 0)
            //{
            //    Response.Redirect("FrmActuacionNotarialExtraProtocolar.aspx?cod=0&GUID=" + HFGUID.Value, false);
            //}
            //else
            //{
            string codPersona = Request.QueryString["CodPer"].ToString();
            Session[Constantes.CONST_SESION_ACTONOTARIAL_ID] = ViewState[Constantes.CONST_SESION_ACTONOTARIAL_ID];
            Session[Constantes.CONST_SESION_ACTUACION_ID] = ViewState[Constantes.CONST_SESION_ACTUACION_ID];

            if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
            {
                Response.Redirect("FrmActuacionNotarialExtraProtocolar.aspx?cod=0&CodPer=" + codPersona + "&Juridica=1", false);
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
                    Response.Redirect("FrmActuacionNotarialExtraProtocolar.aspx?cod=0&CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                }
                else
                {
                    Response.Redirect("FrmActuacionNotarialExtraProtocolar.aspx?cod=0&CodPer=" + codPersona, false);
                }
            }
            
            //}
            txtAutoadhesivo.Focus();
        }
        void CargarUltimoInsumo()
        {
            DataTable _dt = new DataTable();
            InsumoConsultaBL _obj = new InsumoConsultaBL();
            _dt = _obj.ConsultarUltimoInsumoUsuario(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]));
            if (_dt.Rows.Count > 0)
            {
                if (txtAutoadhesivo.Text.Length == 0)
                {
                    txtAutoadhesivo.Text = _dt.Rows[0]["INSUMO"].ToString();
                }
            }

        }
        public static string CrearTextoExtraInterprete(bool ExisteInterorete, bool bExisteTestigoRuego, string strIdioma, string sScriptInterprete,bool bTextoCompleto,bool bAmbosPadres = false)
        {
            string strTexto = string.Empty;
            if (ExisteInterorete)
            {
                if (bExisteTestigoRuego)
                {
                    strTexto = " QUIEN ES HÁBIL";
                }
                else
                {
                    strTexto = ", QUIEN ES HÁBIL";
                }
                if (bTextoCompleto)
                {
                    strTexto = strTexto + " EN EL IDIOMA " + strIdioma;
                    if (bAmbosPadres)
                    {
                        strTexto = strTexto + " Y NO CONOCE EL IDIOMA CASTELLANO, POR LO QUE DESIGNAN A ";
                    }
                    else { strTexto = strTexto + " Y NO CONOCE EL IDIOMA CASTELLANO, POR LO QUE DESIGNA A "; }
                    
                    strTexto = strTexto + sScriptInterprete;

                    strTexto = strTexto + " QUIEN PROCEDE EN CALIDAD DE INTÉRPRETE DE CONFORMIDAD CON LO ESTABLECIDO EN EL ÁRTICULO 30° DEL DECRETO LEGISLATIVO N° 1049,";
                    strTexto = strTexto + " MANIFIESTA SER HÁBIL EN EL IDIOMA " + strIdioma + " Y EL IDIOMA CASTELLANO Y TENER EL CONOCIMIENTO Y EXPERIENCIA SUFICIENTE PARA EFECTUAR LA TRADUCCIÓN QUE LE HA SIDO SOLICITADA.";
                }
                else {
                    strTexto = strTexto + " EN EL IDIOMA " + strIdioma;
                    strTexto = strTexto + " Y NO CONOCE EL IDIOMA CASTELLANO ";               
                }
                
            }

            return strTexto;
        }

        protected void ddlRegistroTipoParticipante_SelectedIndexChanged(object sender, EventArgs e)
        {
            MostrarCamposPorTipoParticipante();
            CargarComboIncapacitados();
        }


        // JOnatan Silva C. -- 23/01/2018
        private void MostrarCamposPorTipoParticipante()
        {
            OcultarTodoLosCamposParticipantes();

            Int32 iTipoParticipante = Convert.ToInt32(ddlRegistroTipoParticipante.SelectedValue);
            Int32 iTipoActo = Convert.ToInt32(ddlTipoActoNotarialExtra.SelectedValue);
            Int32 iPais = Convert.ToInt32(ddlPaisOrigen.SelectedValue);
            string strPaisPeruId = System.Web.Configuration.WebConfigurationManager.AppSettings["Pais_PeruId"].ToString();

            if( iTipoActo == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR))
            {
                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.PADRE)
                    || iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.MADRE))
                {
                     DivEstadoCivil.Visible = true;
                     DivDireccion.Visible = true;
                     DivOpcionIncapacitado.Visible = true;
                     DivIncapacitado.Visible = true;
                     DivIdioma.Visible = true;
                     DivOcupacion.Visible = true;
                     DivPaisNacimientoGentilicio.Visible = true;
                     
                }
                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.MENOR))
                {
                    DivEstadoCivil.Visible = true;
                    //DivPaisNacimientoGentilicio.Visible = true;
                    DivFechaNacimiento.Visible = true;
                }
                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.ACOMPANANTE))
                {
                    DivPaisNacimientoGentilicio.Visible = true;
                    DivEstadoCivil.Visible = true;
                }
                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.APODERADO))
                {
                    DivEstadoCivil.Visible = true;
                    DivDireccion.Visible = true;
                    DivIdioma.Visible = true;
                    DivOcupacion.Visible = true;
                    DivPaisNacimientoGentilicio.Visible = true;
                    DivEscrituraPartida.Visible = true;
                }
                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.INTERPRETE))
                {
                    DivEstadoCivil.Visible = true;
                    DivPaisNacimientoGentilicio.Visible = true;
                }
                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO))
                {
                    DivEstadoCivil.Visible = true;
                    DivPaisNacimientoGentilicio.Visible = true;
                    OtorganteIncapacitado.Visible = true;
                }
            }
            else if (iTipoActo == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.PODER_FUERA_REGISTRO))
            {
                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.OTORGANTE))
                {
                    DivEstadoCivil.Visible = true;
                    DivPaisNacimientoGentilicio.Visible = true;
                    DivDireccion.Visible = true;
                    DivIdioma.Visible = true;
                    DivOcupacion.Visible = true;
                    DivIncapacitado.Visible = true;
                    DivOpcionIncapacitado.Visible = true;
                }
                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.APODERADO))
                {
                    DivEstadoCivil.Visible = true;
                    DivPaisNacimientoGentilicio.Visible = true;
                }
                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.INTERPRETE))
                {
                    DivEstadoCivil.Visible = true;
                    DivPaisNacimientoGentilicio.Visible = true;
                }
                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO))
                {
                    DivPaisNacimientoGentilicio.Visible = true;
                    OtorganteIncapacitado.Visible = true;
                    DivEstadoCivil.Visible = true;
                    DivPaisNacimientoGentilicio.Visible = true;
                    DivOcupacion.Visible = true;
                    DivDireccion.Visible = true;
                }
            }
            else if (iTipoActo == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.CERTIFICADO_SUPERVIVENCIA))
            {
                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.TITULAR))
                {
                    DivEstadoCivil.Visible = true;
                    DivPaisNacimientoGentilicio.Visible = true;
                    DivDireccion.Visible = true;
                    DivOcupacion.Visible = true;
                }
            }
        }

        private void OcultarTodoLosCamposParticipantes()
        {
            DivFechaNacimiento.Visible = false;
            DivEstadoCivil.Visible = false;
            DivPaisNacimientoGentilicio.Visible = false;
            DivDireccion.Visible = false;
            DivOpcionIncapacitado.Visible = false;
            DivIncapacitado.Visible = false;
            DivIdioma.Visible = false;
            DivOcupacion.Visible = false;
            OtorganteIncapacitado.Visible = false;
            DivEscrituraPartida.Visible = false;
        }

        private RE_ACTONOTARIALPARTICIPANTE LlenarEntidadParticipante(bool bEliminar = false)
        {
            RE_ACTONOTARIALPARTICIPANTE participante = new RE_ACTONOTARIALPARTICIPANTE();
            //participante.anpa_iActoNotarialParticipanteId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
            participante.anpa_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
            if (hCodPersona.Value != "0" || hCodPersona.Value != null)
            {
                participante.anpa_iPersonaId = Convert.ToInt64(hCodPersona.Value);
            }
            else { participante.anpa_iPersonaId  = 0; }

            participante.anpa_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);

            if (!bEliminar)
            {
                participante.anpa_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                participante.anpa_sTipoParticipanteId = Convert.ToInt16(this.ddlRegistroTipoParticipante.SelectedValue);
                participante.anpa_vNumeroEscrituraPublica = this.txtNumeroEscritura.Text.ToString().ToUpper();
                participante.anpa_vNumeroPartida = this.txtNumeroPartida.Text.ToString().ToUpper();
                if (participante.anpa_sTipoParticipanteId.ToString() == Convert.ToInt64(Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO).ToString())
                {
                    //participante.anpa_vCampoAuxiliar = this.ddlOtorganteIncapacitado.SelectedValue;
                    //var aux = this.ddlOtorganteIncapacitado.SelectedValue.Split(',');

                    //if (aux.Count() > 0)
                    //{
                    //    participante.anpa_iReferenciaParticipanteId = Convert.ToInt64(aux.ElementAt(0).ToString());
                    //}
                    participante.anpa_vCampoAuxiliar = this.ddlOtorganteIncapacitado.SelectedValue;
                    
                    participante.anpa_iReferenciaParticipanteId = Convert.ToInt64(this.ddlOtorganteIncapacitado.SelectedValue);
                    
                }
                if (chkIncapacitado.Checked)
                {
                    // Si es Checked=true entonces es No Huella de lo contrario Si Huella
                    participante.anpa_bFlagHuella = !(chkNoHuella.Checked);
                    participante.anpa_bFlagFirma = false; // No Firma
                }
                else
                {
                    participante.anpa_bFlagHuella = true; // Si Huella
                    participante.anpa_bFlagFirma = true; // Si Firma
                }
            }
            else {
                participante.anpa_cEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
            }

            participante.anpa_sUsuarioCreacion = Convert.ToInt16(this.hdn_acno_sUsuarioCreacion.Value);
            participante.anpa_vIPCreacion = this.hdn_acno_vIPCreacion.Value;
            participante.anpa_sUsuarioModificacion = Convert.ToInt16(this.hdn_acno_sUsuarioModificacion.Value);
            participante.anpa_vIPModificacion = this.hdn_acno_vIPModificacion.Value;
            participante.anpa_dFechaModificacion = DateTime.Now;
            return participante;
        }
        private RE_PERSONA LlenarEntidadPersona()
        {
            string strPaisPeruId = System.Web.Configuration.WebConfigurationManager.AppSettings["Pais_PeruId"].ToString();
            #region llenar Entidad Participante
                RE_PERSONA EntParticipante = new RE_PERSONA();

                if (hCodPersona.Value != "0" || hCodPersona.Value != null)
                {
                    EntParticipante.pers_iPersonaId = Convert.ToInt64(hCodPersona.Value);
                }
                else { EntParticipante.pers_iPersonaId = 0; }

                EntParticipante.pers_sPersonaTipoId = Convert.ToInt16(Enumerador.enmTipoPersona.NATURAL);
                EntParticipante.pers_vApellidoPaterno = this.txtApePaterno.Text.ToString().ToUpper();
                EntParticipante.pers_vApellidoMaterno = this.txtApeMaterno.Text.ToString().ToUpper();
                EntParticipante.pers_vNombres = this.txtNombres.Text.ToString().ToUpper();
                EntParticipante.pers_vApellidoCasada = this.txtApepCas.Text.ToString().ToUpper();
                EntParticipante.pers_sGeneroId = Convert.ToInt16(this.ddlRegistroGenero.SelectedValue);
                EntParticipante.pers_sEstadoCivilId = Convert.ToInt16(this.ddlRegistroEstadoCivil.SelectedValue);
                EntParticipante.pers_sOcupacionId = Convert.ToInt16(this.ddlRegistroProfesion.SelectedValue);
                EntParticipante.Identificacion.peid_sDocumentoTipoId = Convert.ToInt16(this.ddlRegistroTipoDoc.SelectedValue);
                EntParticipante.Identificacion.peid_vDocumentoNumero = txtDocumentoNro.Text;
                EntParticipante.Identificacion.peid_vTipodocumento = this.txtDescOtroDocumento.Text.ToString().ToUpper();
                EntParticipante.pers_sIdiomaNatalId = Convert.ToInt16(this.ddlRegistroIdioma.SelectedValue);
             
                EntParticipante.pers_sPaisId = Convert.ToInt16(ddlPaisOrigen.SelectedValue);

                EntParticipante.Identificacion.peid_sDocumentoTipoId = Convert.ToInt16(ddlRegistroTipoDoc.SelectedValue);
                EntParticipante.Identificacion.peid_vDocumentoNumero = txtDocumentoNro.Text;

                if (Convert.ToInt16(ddlPaisOrigen.SelectedValue) == Convert.ToInt16(strPaisPeruId))
                {
                    EntParticipante.pers_sNacionalidadId = (int)Enumerador.enmNacionalidad.PERUANA;
                    ddlRegistroNacionalidad.SelectedValue = Convert.ToString((int)Enumerador.enmNacionalidad.PERUANA);
                }
                else
                {
                    if (Convert.ToInt16(ddlRegistroTipoDoc.SelectedValue) == Convert.ToInt16(Enumerador.enmTipoDocumento.DNI)
                        || Convert.ToInt16(ddlRegistroTipoDoc.SelectedValue) == Convert.ToInt16(Enumerador.enmTipoDocumento.CUI))
                    {
                        EntParticipante.pers_sNacionalidadId = (int)Enumerador.enmNacionalidad.PERUANA;
                        ddlRegistroNacionalidad.SelectedValue = Convert.ToString((int)Enumerador.enmNacionalidad.PERUANA);
                    }
                    else {
                        EntParticipante.pers_sNacionalidadId = (int)Enumerador.enmNacionalidad.EXTRANJERA;
                        ddlRegistroNacionalidad.SelectedValue = Convert.ToString((int)Enumerador.enmNacionalidad.EXTRANJERA);
                    }
                }

                EntParticipante.pers_bIncapacidadFlag = chkIncapacitado.Checked;

                if (chkIncapacitado.Checked)
                {
                    EntParticipante.pers_vDescripcionIncapacidad = this.txtRegistroTipoIncapacidad.Text;
                }
                else
                {
                    EntParticipante.pers_vDescripcionIncapacidad = string.Empty;
                }

                EntParticipante.pers_dNacimientoFecha = txtFecNac.Value();

                EntParticipante.pers_sUsuarioCreacion = Convert.ToInt16(this.hdn_acno_sUsuarioCreacion.Value);
                EntParticipante.pers_vIPCreacion = this.hdn_acno_vIPCreacion.Value;
                EntParticipante.OficinaConsultar = Convert.ToInt16(hdn_acno_sOficinaConsularId.Value);

                Int32 iTipoParticipante = Convert.ToInt32(ddlRegistroTipoParticipante.SelectedValue);
                Int32 iTipoActo = Convert.ToInt32(ddlTipoActoNotarialExtra.SelectedValue);

                RE_PERSONARESIDENCIA personaResidencia = new RE_PERSONARESIDENCIA();
                if (iTipoActo == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR))
                {
                    if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.PADRE)
                    || iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.MADRE)
                    || iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.APODERADO))
                    {
                        if (hPerResidenciaID.Value != "0" || hPerResidenciaID.Value != null)
                        {
                            personaResidencia.Residencia.resi_iResidenciaId = Convert.ToInt64(hPerResidenciaID.Value);
                        }
                        else {
                            personaResidencia.Residencia.resi_iResidenciaId = 0;
                        }
                        personaResidencia.Residencia.resi_vResidenciaDireccion = txtDireccion.Text;
                        personaResidencia.Residencia.resi_cResidenciaUbigeo = ddl_UbigeoPais.SelectedValue.ToString() + ddl_UbigeoRegion.SelectedValue.ToString() + ddl_UbigeoCiudad.SelectedValue.ToString();
                    }
                    else if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.MENOR))
                    {
                        if (EntParticipante.pers_sEstadoCivilId == 0)
                        {
                            EntParticipante.pers_sEstadoCivilId = Convert.ToInt16(Enumerador.enmEstadoCivil.SOLTERO);
                        }                        
                    }
                    if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.INTERPRETE))
                    {
                        String sIdiomaCastellano = Session["NotarialIdioma"].ToString();
                        Int16 IdiomaPadreMadre = 0;
                        foreach (GridViewRow row in grd_Participantes.Rows)
                        {
                            Int16 iTipoParticipanteGrilla = Convert.ToInt16(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_sTipoParticipanteId")].Text);
                            Int16 iIdiomaGrilla = 0;
                            if (row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "pers_sIdiomaNatalId")].Text != "&nbsp;")
                            { 
                                iIdiomaGrilla = Convert.ToInt16(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "pers_sIdiomaNatalId")].Text); 
                            }
                            

                            if (iTipoParticipanteGrilla == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.PADRE) && iIdiomaGrilla != Convert.ToInt16(sIdiomaCastellano))
                            {
                                IdiomaPadreMadre = Convert.ToInt16(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "pers_sIdiomaNatalId")].Text);
                            }
                            if (IdiomaPadreMadre == 0)
                            {
                                if (iTipoParticipanteGrilla == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.MADRE) && iIdiomaGrilla != Convert.ToInt16(sIdiomaCastellano))
                                {
                                    IdiomaPadreMadre = Convert.ToInt16(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "pers_sIdiomaNatalId")].Text);
                                }
                            }
                        }

                        if (IdiomaPadreMadre != 0)
                        {
                            EntParticipante.pers_sIdiomaNatalId = IdiomaPadreMadre;
                        }
                    }
                }
                if (iTipoActo == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.PODER_FUERA_REGISTRO))
                {
                    if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.OTORGANTE)
                    || iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO))
                    {
                        if (hPerResidenciaID.Value != "0" || hPerResidenciaID.Value != null)
                        {
                            personaResidencia.Residencia.resi_iResidenciaId = Convert.ToInt64(hPerResidenciaID.Value);
                        }
                        else
                        {
                            personaResidencia.Residencia.resi_iResidenciaId = 0;
                        }
                        personaResidencia.Residencia.resi_vResidenciaDireccion = txtDireccion.Text;
                        personaResidencia.Residencia.resi_cResidenciaUbigeo = ddl_UbigeoPais.SelectedValue.ToString() + ddl_UbigeoRegion.SelectedValue.ToString() + ddl_UbigeoCiudad.SelectedValue.ToString();
                    }
                    if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.INTERPRETE))
                    {
                        String sIdiomaCastellano = Session["NotarialIdioma"].ToString();
                        Int16 IdiomaOtorgante = 0;
                        foreach (GridViewRow row in grd_Participantes.Rows)
                        {
                            Int16 iTipoParticipanteGrilla = Convert.ToInt16(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_sTipoParticipanteId")].Text);

                            Int16 iIdiomaGrilla = 0;
                            if (row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "pers_sIdiomaNatalId")].Text != "&nbsp;")
                            {
                                iIdiomaGrilla = Convert.ToInt16(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "pers_sIdiomaNatalId")].Text);
                            }

                            if (iTipoParticipanteGrilla == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.OTORGANTE) && iIdiomaGrilla != Convert.ToInt16(sIdiomaCastellano))
                            {
                                IdiomaOtorgante = Convert.ToInt16(row.Cells[Util.ObtenerIndiceColumnaGrilla(grd_Participantes, "pers_sIdiomaNatalId")].Text);
                            }
                        }
                        if (IdiomaOtorgante != 0)
                        {
                            EntParticipante.pers_sIdiomaNatalId = IdiomaOtorgante;
                        }
                    }
                }
                if (iTipoActo == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.CERTIFICADO_SUPERVIVENCIA))
                {
                    if (hPerResidenciaID.Value != "0" || hPerResidenciaID.Value != null)
                    {
                        personaResidencia.Residencia.resi_iResidenciaId = Convert.ToInt64(hPerResidenciaID.Value);
                    }
                    else
                    {
                        personaResidencia.Residencia.resi_iResidenciaId = 0;
                    }
                    personaResidencia.Residencia.resi_vResidenciaDireccion = txtDireccion.Text;
                    personaResidencia.Residencia.resi_cResidenciaUbigeo = ddl_UbigeoPais.SelectedValue.ToString() + ddl_UbigeoRegion.SelectedValue.ToString() + ddl_UbigeoCiudad.SelectedValue.ToString();
                }
            #endregion
                EntParticipante.Residencias.Add(personaResidencia);
                return EntParticipante;
        }
        private bool ValidarCamposPorTipoParticipanteTramite()
        { 
            Int32 iTipoParticipante = Convert.ToInt32(ddlRegistroTipoParticipante.SelectedValue);
            Int32 iTipoActo = Convert.ToInt32(ddlTipoActoNotarialExtra.SelectedValue);
            if (iTipoParticipante == 0)
            {
                ddlRegistroTipoParticipante.Style.Add("border", "solid Red 1px");
                return false;
            }

            if (iTipoActo == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR))
            {
                #region Autorización_Viaje_Menor

                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.PADRE)
                    || iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.MADRE))
                {
                    if (!ValidarCamposPorTipoParticipante(true, true, true, false, true, true, true, true, true, true, false, false))
                    {
                        return false;
                    }
                }
                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.MENOR))
                {
                    if (!ValidarCamposPorTipoParticipante(true, true, true, true, false, false , false, false, false, false, false, false))
                    {
                        return false;
                    }
                }
                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.ACOMPANANTE))
                {
                    if (!ValidarCamposPorTipoParticipante(true, true, true, false, false, true, false, false, false, false, false, false))
                    {
                        return false;
                    }
                }
                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.APODERADO))
                {
                    if (!ValidarCamposPorTipoParticipante(true, true, true, false,true, true, true, true, true, false, true, false))
                    {
                        return false;
                    }
                }
                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.INTERPRETE))
                {
                    if (!ValidarCamposPorTipoParticipante(true, true, true, false, false, true, false, false, false, false, false, false))
                    {
                        return false;
                    }
                }
                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO))
                {
                    if (!ValidarCamposPorTipoParticipante(true, true, true, false, false, true, false, false, false, false, false, true))
                    {
                        return false;
                    }
                }
                #endregion
            }
            if (iTipoActo == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.PODER_FUERA_REGISTRO))
            {
                #region Poder_Fuera_Registro

                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.OTORGANTE))
                {
                    if (!ValidarCamposPorTipoParticipante(true, true, true, false, true, true, true, true, true, true, false, false))
                    {
                        MostrarCamposPorTipoParticipante();
                        return false;
                    }
                }
                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.APODERADO))
                {
                    if (!ValidarCamposPorTipoParticipante(true, true, true, false,true, true, false, false, false, false, false, false))
                    {
                        return false;
                    }
                }
                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.INTERPRETE))
                {
                    if (!ValidarCamposPorTipoParticipante(true, true, true, false, false, true, false, false, false, false, false, false))
                    {
                        return false;
                    }
                }
                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO))
                {
                    if (!ValidarCamposPorTipoParticipante(true, true, true, false, true, true, true, true, false, false, false,true))
                    {
                        return false;
                    }
                }
                #endregion
            }
            if (iTipoActo == Convert.ToInt32(Enumerador.enmExtraprotocolarTipo.CERTIFICADO_SUPERVIVENCIA))
            {
                if (iTipoParticipante == Convert.ToInt32(Enumerador.enmNotarialTipoParticipante.TITULAR))
                {
                    if (!ValidarCamposPorTipoParticipante(true, true, true, false, true, true, true, true,false, false, false, false))
                    {
                        MostrarCamposPorTipoParticipante();
                        return false;
                    }
                }
            }
            return true;
        }

        private bool ValidarCamposPorTipoParticipante(bool bTipoDocumento, bool bNomApellidos, bool bGenero, bool bFecNac,
            bool bEstadoCivil, bool bPaisGentilicio, bool bDireccion, bool bOcupacion, bool bIdioma, bool bIncapacidad, 
            bool EscrituraPartida, bool OtorganteIncapacidadFirmar)
        {
            if (bTipoDocumento)
            {
                #region Por_Tipo_Documento

                if (ddlRegistroTipoDoc.SelectedIndex == 0)
                {
                    ddlRegistroTipoDoc.Style.Add("border", "solid Red 1px");
                    return false;
                }
                else
                {
                    if (ddlRegistroTipoDoc.SelectedValue == Convert.ToString((int)Enumerador.enmTipoDocumento.OTROS)
                        || ddlRegistroTipoDoc.SelectedValue == Convert.ToString((int)Enumerador.enmTipoDocumento.PASAPORTE_E))
                    {
                        if (txtDescOtroDocumento.Text.Length == 0)
                        {
                            txtDescOtroDocumento.Style.Add("border", "solid Red 1px");
                            return false;
                        }
                    }
                    else {
                        txtDescOtroDocumento.Style.Add("border", "solid #888888 1px");
                    }
                    ddlRegistroTipoDoc.Style.Add("border", "solid #888888 1px");
                    if (txtDocumentoNro.Text.Length == 0)
                    {
                        txtDocumentoNro.Style.Add("border", "solid Red 1px");
                        return false;
                    }
                    else
                    {
                        txtDocumentoNro.Style.Add("border", "solid #888888 1px");
                        if (ddlRegistroTipoDoc.SelectedValue.ToString() == Enumerador.enmTipoDocumento.OTROS.ToString())
                        {
                            if (txtDescOtroDocumento.Text.Length == 0)
                            {
                                txtDescOtroDocumento.Style.Add("border", "solid Red 1px");
                                return false;
                            }
                            else {
                                txtDescOtroDocumento.Style.Add("border", "solid #888888 1px");
                            }
                        }
                    }
                }
                #endregion
            }

            if (bNomApellidos)
            {
                #region Por_Nombre_Apellidos

                if (txtApePaterno.Text.Length == 0)
                {
                    txtApePaterno.Style.Add("border", "solid Red 1px");
                    return false;
                }
                else {
                    txtApePaterno.Style.Add("border", "solid #888888 1px");
                }
                if (txtNombres.Text.Length == 0)
                {
                    ddlRegistroGenero.Style.Add("border", "solid Red 1px");
                    return false;
                }
                else {
                    txtNombres.Style.Add("border", "solid #888888 1px");
                }
                #endregion
            }

            if (bGenero)
            {
                #region Por_Genero
                if (ddlRegistroGenero.SelectedIndex == 0)
                {
                    ddlRegistroGenero.Style.Add("border", "solid Red 1px");
                    return false;
                }
                else {
                    ddlRegistroGenero.Style.Add("border", "solid #888888 1px");
                }
                #endregion
            }

            if (bFecNac)
            {
                #region Por_Fecha_Nacimiento

                if (txtFecNac.Text == "")
                {
                    ((TextBox)txtFecNac.FindControl("TxtFecha")).Style.Add("border", "solid Red 1px");
                    return false;
                }
                else {
                    ((TextBox)txtFecNac.FindControl("TxtFecha")).Style.Add("border", "solid #888888 1px");
                }
                #endregion
            }

            if (bEstadoCivil)
            {
                #region Por_Estado_Civil
                
                if (ddlRegistroEstadoCivil.SelectedIndex == 0)
                {
                    ddlRegistroEstadoCivil.Style.Add("border", "solid Red 1px");
                    return false;
                }
                else {
                    ddlRegistroEstadoCivil.Style.Add("border", "solid #888888 1px");
                }
                #endregion
            }

            if (bPaisGentilicio)
            {
                #region Por_Pais_Origen

                if (ddlPaisOrigen.SelectedIndex == 0)
                {
                    ddlPaisOrigen.Style.Add("border", "solid Red 1px");
                    return false;
                }
                else {
                    ddlPaisOrigen.Style.Add("border", "solid #888888 1px");
                }
                #endregion
            }

            if (bDireccion)
            {
                #region Dirección

                    if (txtDireccion.Text.Trim() == string.Empty)
                    {
                        txtDireccion.Style.Add("border", "solid Red 1px");
                        return false;
                    }
                    else
                    {
                        txtDireccion.Style.Add("border", "solid #888888 1px");
                    }
                    if (ddl_UbigeoPais.SelectedValue.ToString() == "0")
                    {
                        ddl_UbigeoPais.Style.Add("border", "solid Red 1px");
                        return false;
                    }
                    else
                    {
                        ddl_UbigeoPais.Style.Add("border", "solid #888888 1px");
                    }
                    if (ddl_UbigeoRegion.SelectedValue.ToString() == "0")
                    {
                        ddl_UbigeoRegion.Focus();
                        ddl_UbigeoRegion.Style.Add("border", "solid Red 1px");
                        return false;
                    }
                    else
                    {
                        ddl_UbigeoRegion.Style.Add("border", "solid #888888 1px");
                    }

                    if (ddl_UbigeoCiudad.SelectedValue.ToString() == "0")
                    {
                        ddl_UbigeoCiudad.Focus();
                        ddl_UbigeoCiudad.Style.Add("border", "solid Red 1px");
                        return false;
                    }
                    else
                    {
                        ddl_UbigeoCiudad.Style.Add("border", "solid #888888 1px");
                    }

                #endregion
            }

            if (bOcupacion)
            {
                #region Por_Ocupacion

                if (ddlRegistroProfesion.SelectedIndex == 0)
                {
                    ddlRegistroProfesion.Style.Add("border", "solid Red 1px");
                    return false;
                }
                else
                {
                    ddlRegistroProfesion.Style.Add("border", "solid #888888 1px");
                }
                #endregion
            }

            if (bIdioma)
            {
                #region Por_Ocupacion

                if (ddlRegistroIdioma.SelectedIndex == 0)
                {
                    ddlRegistroIdioma.Style.Add("border", "solid Red 1px");
                    return false;
                }
                else 
                {
                    ddlRegistroIdioma.Style.Add("border", "solid #888888 1px");
                }
                #endregion
            }

            if (bIncapacidad)
            {
                #region Por_Incapacidad

                if (chkIncapacitado.Checked)
                {
                    if (txtRegistroTipoIncapacidad.Text.Length == 0)
                    {
                        txtRegistroTipoIncapacidad.Style.Add("border", "solid Red 1px");
                        return false;
                    }
                    else
                    {
                        txtRegistroTipoIncapacidad.Style.Add("border", "solid #888888 1px");
                    }
                }
                #endregion
            }

            if (EscrituraPartida)
            {
                #region Por_Escritura

                if (txtNumeroEscritura.Text.Length == 0)
                {
                    txtNumeroEscritura.Style.Add("border", "solid Red 1px");
                    return false;
                }
                else {
                    txtNumeroEscritura.Style.Add("border", "solid #888888 1px");
                }
                if (txtNumeroPartida.Text.Length == 0)
                {
                    txtNumeroPartida.Style.Add("border", "solid Red 1px");
                    return false;
                }
                else {
                    txtNumeroPartida.Style.Add("border", "solid #888888 1px");
                }
                #endregion
            }

            if (OtorganteIncapacidadFirmar)
            {
                #region Por_Incapacidad_Firmar

                if (ddlOtorganteIncapacitado.SelectedIndex == 0)
                {
                    ddlOtorganteIncapacitado.Style.Add("border", "solid Red 1px");
                    return false;
                }
                else {
                    ddlOtorganteIncapacitado.Style.Add("border", "solid #888888 1px");
                }
                #endregion
            }
            return true;
        }

        private void Incapacitado()
        {
            if (chkIncapacitado.Checked)
            {
                txtRegistroTipoIncapacidad.Enabled = true;
                lblHuella.Visible = true;
                chkNoHuella.Visible = true;
            }
            else {
                txtRegistroTipoIncapacidad.Enabled = false;
                lblHuella.Visible = false;
                chkNoHuella.Visible = false;
            }
        }
        protected void ddlRegistroEstadoCivil_SelectedIndexChanged(object sender, EventArgs e)
        {
            HabilitarApellidoCasada();
        }

        protected void chkIncapacitado_CheckedChanged(object sender, EventArgs e)
        {
            Incapacitado();
        }
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            if (txtAutoadhesivo.Enabled)
            {
                txtAutoadhesivo.Text = "";
            }
        }

        private long ObtenerNormaTarifario(Int16 intTipoPagoId)
        {
            long intNormaTarifarioId = 0;
            string strTarifaLetra = this.Txt_TarifaId.Text.Trim().ToUpper();

            if (strTarifaLetra.Trim().Length > 0 && intTipoPagoId > 0)
            {
                DataTable dtNormaTarifario = new DataTable();
                NormaTarifarioDL objNormaTarifarioBL = new NormaTarifarioDL();
                int IntTotalCount = 0;
                int IntTotalPages = 0;

                dtNormaTarifario = objNormaTarifarioBL.Consultar(intTipoPagoId, -1, strTarifaLetra, "", false, 1, 1, "N", ref IntTotalCount, ref IntTotalPages);

                if (dtNormaTarifario.Rows.Count > 0)
                {
                    intNormaTarifarioId = Convert.ToInt64(dtNormaTarifario.Rows[0]["nota_iNormaTarifarioId"].ToString());
                }
            }
            return intNormaTarifarioId; 
        }

        private bool ExisteInafecto_Exoneracion(string strID)
        {
            bool bExiste = false;
            string strTexto = "";
            string strTipoPagoId = "";

            DataTable datTipoPago = new DataTable();

            datTipoPago = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACREDITACION_TIPO_COBRO);


            for (int i = 0; i < datTipoPago.Rows.Count; i++)
            {
                strTexto = datTipoPago.Rows[i]["descripcion"].ToString().Trim().ToUpper();

                if (strTexto.Contains("EXONERA") || strTexto.Contains("INAFECT"))
                {
                    strTipoPagoId = datTipoPago.Rows[i]["id"].ToString().Trim();

                    if (strID.Trim().Equals(strTipoPagoId))
                    {
                        bExiste = true;
                        break;
                    }
                }
            }
            return bExiste;
        }


       

     
        
        private Boolean ValidarRegistroPago()
        {
            bool iscorrecto = true;
            //-----------------------------------------------------------------
            //Fecha: 21/03/2019
            //Requerimiento: Prueba_11_03_2019 
            //Item: 15
            //Autor: Miguel Márquez Beltrán
            // Objetivo: Deshabilitar la validación del nro. de operación.
            //-----------------------------------------------------------------

            ///*Jonatan Silva Cachay - 21/11/2017 - Validación de Pago: Deposito en cuenta Nro de Operación*/
            //if (Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA) ||
            //    Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA) ||
            //    Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA))
            //{
            //    long lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
            //    if (lngActuacionDetalleId > 0)
            //    {
            //        DataTable _dtVerifica = new DataTable();
            //        ActuacionPagoConsultaBL _obj = new ActuacionPagoConsultaBL();
            //        _dtVerifica = _obj.verificarOperacion(null, Convert.ToInt16(ddlNomBanco.SelectedValue), txtNroOperacion.Text, Convert.ToInt16(ddlTipoPago.SelectedValue), Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), Comun.FormatearFecha(ctrFecPago.Text));

            //        if (_dtVerifica.Rows.Count > 0)
            //        {
            //            string StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", _dtVerifica.Rows[0]["Mensaje"].ToString(), false, 190, 250);
            //            Comun.EjecutarScript(Page, StrScript);
            //            return false;
            //        }
            //    }
            //}

            
            bool bNoCobrado = ExisteInafecto_Exoneracion(ddlTipoPago.SelectedValue);

            if (bNoCobrado || Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
            {

                if (RBNormativa.Visible == true && RBNormativa.Checked)
                {
                    if (ddlExoneracion.Visible == true)
                    {
                        if (ddlExoneracion.SelectedIndex == 0)
                        {
                            ddlExoneracion.Enabled = true;
                            
                            Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", "Seleccione la Ley que exonera el Pago"));
                            return false;
                        }
                    }
                }
                if (RBNormativa.Visible == false)
                {
                    if (ddlExoneracion.Visible == true)
                    {
                        if (ddlExoneracion.SelectedIndex == 0)
                        {
                            ddlExoneracion.Enabled = true;
                            ddlExoneracion.Focus();
                            
                            Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", "Seleccione la Ley que exonera el Pago"));
                            return false;
                        }
                    }
                }

                if (RBSustentoTipoPago.Visible == true && RBSustentoTipoPago.Checked)
                {
                    if (txtSustentoTipoPago.Visible == true && txtSustentoTipoPago.Text.Trim().Length == 0)
                    {
                        txtSustentoTipoPago.Enabled = true;
                        txtSustentoTipoPago.Focus();
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", "Digite el sustento"));
                        return false;
                    }
                }
                if (RBSustentoTipoPago.Visible == false)
                {
                    if (txtSustentoTipoPago.Visible == true && txtSustentoTipoPago.Text.Trim().Length == 0)
                    {
                        txtSustentoTipoPago.Enabled = true;
                        txtSustentoTipoPago.Focus();
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", "Digite el sustento"));
                        return false;
                    }
                }
            }
            

            return iscorrecto;
        }

        //-----------------------------------------------------------//
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 27-10-2016
        // Objetivo: Llenar la lista de exoneraciones
        //-----------------------------------------------------------//
        private void LlenarListaExoneracion()
        {
            if (ddlTipoPago.SelectedIndex <= 0)
            {
                lblSustentoTipoPago.Visible = false;
                txtSustentoTipoPago.Visible = false;
                lblValSustentoTipoPago.Visible = false;
                RBNormativa.Visible = false;
                RBSustentoTipoPago.Visible = false;
                lblExoneracion.Visible = false;
                ddlExoneracion.Visible = false;
                lblValExoneracion.Visible = false;
                return;
            }
            string strTarifaLetra = Txt_TarifaId.Text.Trim().ToUpper();

            DataTable dtExoneracion = new DataTable();

            NormaTarifarioDL objNormaTarifario = new NormaTarifarioDL();
            int IntTotalCount = 0;
            int IntTotalPages = 0;

            string strFecha = Comun.syyyymmdd(DateTime.Now.ToShortDateString());

            Int16 intTipoPagoId = Convert.ToInt16(ddlTipoPago.SelectedValue);

            dtExoneracion = objNormaTarifario.Consultar(intTipoPagoId, -1, strTarifaLetra, strFecha, false, 1000, 1, "N", ref IntTotalCount, ref IntTotalPages);

            Util.CargarDropDownList(ddlExoneracion, dtExoneracion, "norm_vDescripcionCorta", "nota_iNormaTarifarioId", true);

            if (dtExoneracion.Rows.Count > 0)
            {
                #region Si_ExisteRegistros

                if (dtExoneracion.Rows.Count == 1)
                {
                    ddlExoneracion.SelectedIndex = 1;
                    ddlExoneracion.Enabled = false;
                }
                else
                {
                    ddlExoneracion.SelectedIndex = 0;
                    ddlExoneracion.Enabled = true;
                    RBNormativa.Checked = true;
                    txtSustentoTipoPago.Enabled = false;
                    txtSustentoTipoPago.Text = "";
                }
                lblValExoneracion.Visible = true;

                lblExoneracion.Visible = true;
                ddlExoneracion.Visible = true;

                if (Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
                {
                    lblSustentoTipoPago.Visible = true;
                    txtSustentoTipoPago.Visible = true;
                    lblValSustentoTipoPago.Visible = true;
                    RBNormativa.Visible = true;
                    RBSustentoTipoPago.Visible = true;
                }
                else
                {
                    lblSustentoTipoPago.Visible = false;
                    txtSustentoTipoPago.Visible = false;
                    lblValSustentoTipoPago.Visible = false;
                    RBNormativa.Visible = false;
                    RBSustentoTipoPago.Visible = false;
                }
                #endregion
            }
            else
            {
                #region Cuando_No_Existan_registros

                lblExoneracion.Visible = false;
                ddlExoneracion.Visible = false;
                lblSustentoTipoPago.Visible = false;
                txtSustentoTipoPago.Visible = false;
                lblValSustentoTipoPago.Visible = false;
                RBNormativa.Visible = false;
                RBSustentoTipoPago.Visible = false;
                lblValExoneracion.Visible = false;

                #endregion
            }
        }

        //---------------------------------------------------------------------------
        //Fecha: 21/01/2019
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Habilitar la etiqueta: D.L. 173 del D.S. 0076-2005-RE
        //          cuando el tipo de pago sea: Gratuito por Ley 
        //          no tomar en cuenta la tarifa 2 ni la Sección III del Tarifario
        //---------------------------------------------------------------------------

        private void MostrarDL173_DS076_2005RE()
        {
            bool bisSeccionIII = Comun.isSeccionIII(Session, Txt_TarifaId.Text);
            string strTarifa = Txt_TarifaId.Text.Trim().ToUpper();


            if (Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
            {
                if (bisSeccionIII == false && strTarifa != "2")
                {
                    lblExoneracion.Visible = true;
                    //lblNorma173.Visible = true;
                    lblValExoneracion.Visible = false;
                    lblSustentoTipoPago.Visible = true;
                    txtSustentoTipoPago.Visible = true;
                    txtSustentoTipoPago.Enabled = true;
                    lblValSustentoTipoPago.Visible = true;
                    RBNormativa.Visible = false;
                    RBSustentoTipoPago.Visible = false;
                }
                else
                {
                    //lblNorma173.Visible = false;
                }
            }
            else
            {
                //lblNorma173.Visible = false;
                lblSustentoTipoPago.Visible = false;
                txtSustentoTipoPago.Visible = false;
                lblValSustentoTipoPago.Visible = false;
                RBNormativa.Visible = false;
                RBSustentoTipoPago.Visible = false;
            }
            updRegPago.Update();
            //-----------------------------------------------------------------
        }

        private void CargarTipoPagoNormaTarifario()
        {

            int IntTotalCount = 0;
            int IntTotalPages = 0;

            string strTarifaLetra = Txt_TarifaId.Text.Trim().ToUpper();

            //---------------------------------------------------------------------
            DataTable dtNormaTarifario = new DataTable();
            NormaTarifarioDL objNormaTarifarioBL = new NormaTarifarioDL();

            dtNormaTarifario = objNormaTarifarioBL.Consultar(0, -1, strTarifaLetra, "", false, 20000, 1, "N", ref IntTotalCount, ref IntTotalPages);
            DataTable dtTipoPagoSel = dtNormaTarifario.DefaultView.ToTable(true, "nota_sPagoTipoId");
            //---------------------------------------------------------------------
            DataTable dtConsuladoTipoPagoTarifa = new DataTable();
            OficinaConsularTarifarioTipoPagoDL objConsuladoTarifarioTipoPagoBL = new OficinaConsularTarifarioTipoPagoDL();

            Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            dtConsuladoTipoPagoTarifa = objConsuladoTarifarioTipoPagoBL.Consultar(intOficinaConsularId, 0, strTarifaLetra, false, 20000, 1, "N", ref IntTotalCount, ref IntTotalPages);
            DataTable dtConsuladoTipoPagoSel = dtConsuladoTipoPagoTarifa.DefaultView.ToTable(true, "ofpa_sPagoTipoId");
            //----------------------------------------------------
            DataTable dtTipoPago = new DataTable();
            dtTipoPago = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACREDITACION_TIPO_COBRO);
            DataView dv = dtTipoPago.DefaultView;
            DataTable dtTipoPagoOrdenadoOrdenado = dv.ToTable();
            dtTipoPagoOrdenadoOrdenado.DefaultView.Sort = "torden ASC";

            Util.CargarParametroDropDownList(ddlTipoPago, dtTipoPagoOrdenadoOrdenado, true);

            if (dtTipoPagoSel.Rows.Count > 0)
            {
                //-------------------------------------
                Int16 intTipoPagoId = 0;
                bool bExisteTipoPago = false;
                bool bExisteConsuladoTipoPago = false;

                for (int i = 0; i < dtTipoPago.Rows.Count; i++)
                {
                    intTipoPagoId = Convert.ToInt16(dtTipoPago.Rows[i]["id"].ToString());
                    bExisteTipoPago = false;

                    for (int x = 0; x < dtTipoPagoSel.Rows.Count; x++)
                    {
                        if (intTipoPagoId == Convert.ToInt16(dtTipoPagoSel.Rows[x]["nota_sPagoTipoId"].ToString()))
                        {
                            bExisteTipoPago = true;
                            break;
                        }
                    }
                    bExisteConsuladoTipoPago = false;

                    for (int z = 0; z < dtConsuladoTipoPagoSel.Rows.Count; z++)
                    {
                        if (intTipoPagoId == Convert.ToInt16(dtConsuladoTipoPagoSel.Rows[z]["ofpa_sPagoTipoId"].ToString()))
                        {
                            bExisteConsuladoTipoPago = true;
                            break;
                        }
                    }
                    if (bExisteTipoPago == false && bExisteConsuladoTipoPago == false)
                    {
                        ddlTipoPago.Items.Remove(ddlTipoPago.Items.FindByValue(dtTipoPago.Rows[i]["id"].ToString()));
                    }
                }
                //------------------------------------
            }

            if (ddlTipoPago.SelectedIndex == 0)
            {
                lblExoneracion.Visible = false;
                ddlExoneracion.Visible = false;
                lblValExoneracion.Visible = false;
                RBNormativa.Visible = false;
                lblSustentoTipoPago.Visible = false;
                txtSustentoTipoPago.Visible = false;
                lblValSustentoTipoPago.Visible = false;
                RBSustentoTipoPago.Visible = false;
            }
        }

        protected void cmb_TipoPago_SelectedIndexChanged(object sender, EventArgs e)
        {            
            txtCantidad.Text = "1";
            if (Lst_Tarifario.SelectedIndex == -1)
            {
                CalculoxTarifarioxTipoPagoxCantidad();
            }
            else {
                CalculoxTarifarioxTipoPagoxCantidad(Lst_Tarifario.SelectedIndex);
            }
            
            //MostrarMonedaDolar(intTipoPago);
            //bool bNoCobrado = ExisteInafecto_Exoneracion(intTipoPago.ToString());

            //if (bNoCobrado || intTipoPago == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
            //{
            //    Txt_TotSC.Text = "0.00";
            //    Txt_TotML.Text = "0.00";
            //}

            string strDescTipoPagoOrigen = Comun.ObtenerDescripcionTipoPago(Session, HF_TIPO_PAGO.Value);

            Comun.ActualizarControlPago(Session, strDescTipoPagoOrigen, Txt_TarifaId.Text, txtCantidad.Text,
                    ref ctrlToolBar5.btnGrabar, ref ddlTipoPago, ref txtNroOperacion, ref txtAutoadhesivo,
                    ref ddlNomBanco, ref ctrFecPago, ref ddlExoneracion, ref lblExoneracion, ref lblValExoneracion,
                    ref txtSustentoTipoPago, ref lblSustentoTipoPago, ref lblValSustentoTipoPago,
                    ref RBNormativa, ref RBSustentoTipoPago, ref Txt_MontML, ref Txt_MtoSC,
                    ref Txt_TotML, ref Txt_TotSC, ref LblDescMtoML, ref LblDescTotML,
                    ref pnlPagLima, ref txtMtoCancelado);


            if (H_CAN_AUTOADHESIVO.Value == "0")
            {
                ctrlToolBar5.btnGrabar.Enabled = false;
            }
            updRegPago.Update();
        }

        private void CargarDatosTarifaPago()
        {
            //if (ViewState[strVariableTarifario] == null)
            //{
            //    return;
            //}

            BE.RE_TARIFA_PAGO objTarifaPago = new BE.RE_TARIFA_PAGO();

            dtTarifarioFiltrado = (DataTable)ViewState["dtTarifarioFiltrado"];
            objTarifarioBE = CargarObjetoTarifario(dtTarifarioFiltrado, 0);

            //objTarifarioBE = (BE.MRE.SI_TARIFARIO)ViewState[strVariableTarifario];

            objTarifaPago.sTarifarioId = Convert.ToInt32(objTarifarioBE.tari_sTarifarioId);


            objTarifaPago.sTipoActuacion = 0;

            var existe = new SGAC.Registro.Actuacion.BL.ActuacionPagoConsultaBL().ActuacionPagoObtenerDetalle(Convert.ToInt64(ViewState[Constantes.CONST_SESION_ACTUACIONDET_ID]));

            if (existe.Rows.Count > 0)
            {
                objTarifaPago.sTarifarioId = Convert.ToInt16(existe.Rows[0]["sTarifarioId"]);
                objTarifaPago.vTarifa = Convert.ToString(existe.Rows[0]["vTarifa"]);

                objTarifaPago.vTarifaDescripcion = Convert.ToString(existe.Rows[0]["vTarifa"]) + " - " + Convert.ToString(existe.Rows[0]["vTarifaDescripcion"]);
                objTarifaPago.vTarifaDescripcionLarga = Convert.ToString(existe.Rows[0]["descripcion"].ToString());
                objTarifaPago.datFechaRegistro = Comun.FormatearFecha(existe.Rows[0]["acde_dFechaRegistro"].ToString());
                objTarifaPago.tari_sSeccionId = Convert.ToInt32(existe.Rows[0]["tari_sSeccionId"]);
                objTarifaPago.sTipoActuacion = Convert.ToInt16(existe.Rows[0]["sTipoActuacion"]);
                objTarifaPago.datFechaRegistroActuacion = Comun.FormatearFecha(existe.Rows[0]["acde_dFechaRegistro"].ToString());
                objTarifaPago.sTipoPagoId = Convert.ToInt16(existe.Rows[0]["pago_sPagoTipoId"]);
                objTarifaPago.dblCantidad = Convert.ToDouble(existe.Rows[0]["Cantidad"]);
                objTarifaPago.dblMontoSolesConsulares = Convert.ToDouble(existe.Rows[0]["FSolesConsular"]);
                objTarifaPago.dblMontoMonedaLocal = Convert.ToDouble(existe.Rows[0]["FMonedaExtranjera"]);
                objTarifaPago.dblTotalSolesConsulares = Convert.ToDouble(existe.Rows[0]["FTOTALSOLESCONSULARES"]);
                objTarifaPago.dblTotalMonedaLocal = Convert.ToDouble(existe.Rows[0]["FTOTALMONEDALocal"]);
                objTarifaPago.vObservaciones = Convert.ToString(existe.Rows[0]["acde_vNotas"]);
                objTarifaPago.vMonedaLocal = Convert.ToString(existe.Rows[0]["vMonedaLocal"]);

                if (objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA ||
                    objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA ||
                    objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA)
                {
                    if (existe.Rows[0]["pago_sBancoId"].ToString() != string.Empty)
                    {
                        if (Convert.ToInt32(existe.Rows[0]["pago_sBancoId"]) != 0)
                        {
                            objTarifaPago.vNumeroOperacion = Convert.ToString(existe.Rows[0]["pago_vBancoNumeroOperacion"]);
                            objTarifaPago.sBancoId = Convert.ToInt16(existe.Rows[0]["pago_sBancoId"]);
                            objTarifaPago.datFechaPago = Comun.FormatearFecha(Convert.ToString(existe.Rows[0]["pago_dFechaOperacion"]));
                            objTarifaPago.dblMontoCancelado = Convert.ToDouble(existe.Rows[0]["FMonedaExtranjera"]);
                        }
                    }
                }

                //----------------------------------------//
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 27-10-2016
                // Objetivo: Visualizar la clasificación de la tarifa
                //           Visualizar la Norma de la tarifa
                //----------------------------------------//
                objTarifaPago.dblClasificacion = Convert.ToDouble(existe.Rows[0]["acde_sClasificacionTarifaId"]);
                objTarifaPago.dblNormaTarifario = Convert.ToDouble(existe.Rows[0]["pago_iNormaTarifarioId"]);
                objTarifaPago.vSustentoTipoPago = Convert.ToString(existe.Rows[0]["pago_vSustentoTipoPago"]).ToUpper();


                Session[Constantes.CONST_SESION_OBJ_TARIFA_PAGO] = objTarifaPago;
            }
            else
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTUACIÓN CONSULAR", "No hay datos de pago."));
                return;
            }
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
    }
}



public class ClaseInterprete
{
    public ClaseInterprete()
    {
        
    }
    public string idioma { get; set; }

    private StringBuilder _DatosPersonales;

    public void SetDatosPersonales(StringBuilder sb)
    {
        _DatosPersonales = new StringBuilder(sb.ToString());
    }

    public StringBuilder GetDatosPersonales()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(_DatosPersonales.ToString());

        return sb;
    }
        
}
        