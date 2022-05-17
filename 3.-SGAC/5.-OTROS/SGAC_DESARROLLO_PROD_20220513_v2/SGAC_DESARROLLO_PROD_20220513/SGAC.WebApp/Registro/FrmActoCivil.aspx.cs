using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using iTextSharp.text;
using SGAC.Accesorios;
using SGAC.BE;
using SGAC.Controlador;
using SGAC.Registro.Actuacion.BL;
using SGAC.Registro.Persona.BL;
using SGAC.WebApp.Accesorios;
using SGAC.WebApp.Accesorios.SharedControls;
using Microsoft.Reporting.WebForms;
using Microsoft.Security.Application;
using SGAC.Almacen.BL;
using SGAC.Configuracion.Sistema.BL;

namespace SGAC.WebApp.Registro
{
    public partial class FrmActoCivil : MyBasePage
    {
       
        #region CAMPOS

        private string strVariableAccion = "ActoCivil_Accion";
        private string strRegistroCivilId = "REGISTRO_CIVIL_ID";
        private string strVariableTarifario = "objTarifarioBE";

        private string strVariableActDT = "TRAMITE_DT";

        DataTable dtActuaciones = null;

        private BE.MRE.SI_TARIFARIO objTarifarioBE;

        private static bool cargado = false;
        private static bool edicion = false;
        private static int edicion_rowindex = -1;
        #endregion CAMPOS

        #region Enumerador
        public enum ModoProceso
        {
            Binario = 0,
            Texto = 1,
            Automatico = 2
        };
        public enum ErrorCorreccion
        {
            Nivel0 = 0,
            Nivel1 = 1,
            Nivel2 = 2,
            Nivel3 = 3,
            Nivel4 = 4,
            Nivel5 = 5,
            Nivel6 = 6,
            Nivel7 = 7,
            Nivel8 = 8
        };
        public enum Fuente
        {
            MW6_PDF417R3 = 1,
            MW6_PDF417R4 = 2,
            MW6_PDF417R5 = 3,
            MW6_PDF417R6 = 4
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ctrlToolBarRegistro.VisibleIButtonGrabar = true;
                ctrlToolBarRegistro.VisibleIButtonCancelar = true;
                ctrlToolBarRegistro.btnGrabar.Enabled = false;

                ctrlToolBarRegistro.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolBarRegistro_btnGrabarHandler);
                ctrlToolBarRegistro.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarRegistro_btnCancelarHandler);

                ctrlToolbarFormato.VisibleIButtonGrabar = true;
                ctrlToolbarFormato.VisibleIButtonCancelar = true;
                ctrlToolbarFormato.VisibleIButtonEliminar = true;
                ctrlToolbarFormato.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolbarFormato_btnGrabarHandler);
                ctrlToolbarFormato.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolbarFormato_btnCancelarHandler);
                ctrlToolbarFormato.btnEliminarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEliminarClick(ctrlToolbarFormato_btnEliminarHandler);
                ctrlToolbarFormato.btnGrabar.Enabled = true;

                //Button BtnEliminarE = (Button)ctrlToolbarFormato.FindControl("btnEliminar");
                //BtnEliminarE.OnClientClick = "return confirm('¿Desea eliminar a la persona?');";

                // Jonatan -- 20/07/2017 -- Botones controles de usuario, reimpresión y anulación de autoadhesivos
                ctrlReimprimirbtn1.btnReimprimirHandler += new Accesorios.SharedControls.ctrlReimprimirbtn.OnButtonReimprimirClick(ctrlReimprimirbtn_btnReimprimirHandler);
                ctrlBajaAutoadhesivo1.btnAnularHandler += new Accesorios.SharedControls.ctrlBajaAutoadhesivo.OnButtonAnularClick(ctrlBajaAutoadhesivo_btnAnularAutoahesivo);
                ctrlBajaAutoadhesivo1.btnAceptarAnularHandler += new Accesorios.SharedControls.ctrlBajaAutoadhesivo.OnButtonAceptarAnulacionClick(ctrlBajaAutoadhesivo_btnAceptarAnularAutoahesivo);
                //------------------------------------------------------

                BtnGrabAnotacion.OnClientClick = "return ValidarRegistroAnotacion();";
                //ctrlToolbarFormato.btnGrabar.OnClientClick = "return ValidarGrabar();";
                BtnVistaPrevia.OnClientClick = "return ValidarGrabar();";
                ctrlToolBarRegistro.btnGrabar.OnClientClick = "return ValidarRegistroActuacion()";
                btnGrabarVinculacion.OnClientClick = "return ValidarVinculacion();";
                btnAceptar.OnClientClick = "return ActoCivil_Participantes();";
                //ctrFecPago.Text = DateTime.Now.ToString(ConfigurationManager.AppSettings["FormatoFechas"]);

                lblValidacionParticipante.Text = HF_TextoValidacion.Value;
                this.ctrFecRegistro.StartDate = new DateTime(1900, 1, 1);
                this.ctrFecRegistro.EndDate = DateTime.Now.AddDays(1);
                txtFecNac.EndDate = DateTime.Now;

                this.CtrldFecNacimientoParticipante.StartDate = new DateTime(1900, 1, 1);
                this.CtrldFecNacimientoParticipante.EndDate = DateTime.Today;

                ctrFecRegistro.Ejecutar_Scrip = true;
                txtFecNac.Ejecutar_Scrip = true;
                CtrldFecNacimientoParticipante.Ejecutar_Scrip = true;
                //CtrldFecNacimientoParticipante.Ejecutar_Scrip = true;
                ctrlAdjunto.Click += new EventHandler(ctrlAdjunto_Click);

                ctrlUbigeo1.Click += new EventHandler(Ubigeo_Click);

                ctrlAdjunto.IsCivil = true;

                string strRegistroInicial = Convert.ToString(Request.QueryString["bIni"]);

                if (strRegistroInicial == "1")
                {
                    if (Session["InicioTramite"].ToString() == "2")
                    {
                        hInicioTramite.Value = "2";
                        //Session["InicioTramite"] = "0";
                    }
                    else
                    {
                        Session["InicioTramite"] = "1";
                    }
                }
                else
                {
                    Session["InicioTramite"] = "2";
                }

                //-------------------------------------------
                if (!Page.IsPostBack)
                {
                    string codPersona = "0";
                    if (Request.QueryString["CodPer"] != null)
                    {
                        codPersona = Util.DesEncriptar(Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString()));

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
                        HF_iPersonaID.Value = codPersona.ToString();
                        GetDataPersona(Convert.ToInt64(codPersona));
                    }
                    if (Request.QueryString["GUID"] != null)
                    {
                        HFGUID.Value = Sanitizer.GetSafeHtmlFragment(Request.QueryString["GUID"].ToString());
                    }
                    else
                    {
                        HFGUID.Value = "";
                    }
                    ctrlAdjunto.GUID = HFGUID.Value;
                    ctrlReimprimirbtn1.GUID = HFGUID.Value;

                    if (Session["strBusqueda"] != null)
                    {
                        Session.Remove("strBusqueda");
                    }

                    //---------------------------------------------------------//
                    // Autor: Miguel Angel Márquez Beltrán
                    // Fecha: 18-06-2019
                    // Objetivo: Activar el botón grabar por defecto al inicio.
                    //           y deshactivar Vista previa al inicio.
                    //---------------------------------------------------------//
                    ctrlToolbarFormato.btnGrabar.Enabled = true;
                    BtnVistaPrevia.Enabled = false;
                    cbxAfirmarTexto.Enabled = false;
                    btnActa.Enabled = false;
                    updVinculacion.Update();
                    //updFormato.Update();
                    //---------------------------------------------------------//


                    this.hCUI.Value = "";
                    //----------------------------------------//          
                    // Autor: Miguel Angel Márquez Beltrán
                    // Fecha: 05-03-2019
                    // Objetivo: Ocultar los controles de clasificación
                    //----------------------------------------//
                    lblExoneracion.Visible = false;
                    ddlExoneracion.Visible = false;

                    //----------------------------------------//
                    // Autor: Miguel Angel Márquez Beltrán
                    // Fecha: 05-03-2019
                    // Objetivo: Ocultar controles de Sustento
                    //----------------------------------------//
                    lblSustentoTipoPago.Visible = false;
                    txtSustentoTipoPago.Visible = false;
                    lblValSustentoTipoPago.Visible = false;
                    RBNormativa.Visible = false;
                    RBSustentoTipoPago.Visible = false;
                    lblValExoneracion.Visible = false;
                    //----------------------------------------//

                    lbltienendocumento.Visible = false;
                    rbNo.Visible = false;
                    rbSi.Visible = false;

                    HabilitaCamposPagoActuacion(false);
                    dtActuaciones = (DataTable)Session[strVariableActDT];


                    HF_ACTUACIONDET_ID.Value = Convert.ToString(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
                    //HF_ACTUACIONDETALLE_REFERENCIA.Value = Convert.ToString(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);


                    Session["TIPO_ACTO_PARTICIPANTE"] = (int)Enumerador.enmTipoActa.NACIMIENTO;
                    Session["bCivil"] = 1;
                    CargarListadosDesplegables();
                    CargarDatosIniciales();
                    PintarDatosPestaniaRegistro();
                    MostrarDL173_DS076_2005RE();
                    BloquearParaTarifasGratuitas();
                    if (Session["InicioTramite"].ToString().Equals("2")) // viene de la pantalla frmActuación entonces no debe cargar nada ya que solo tiene pago
                    {
                        CargarDatosRegistroCivil();

                    }
                    else
                    {
                        mtLayoutCivil();
                        //    Session["Participante"] = (List<BE.RE_PARTICIPANTE>)objActuacion.PARTICIPANTE_Container;
                        //    this.Grd_Participantes.DataSource = mtParticipanteContainerToTable("");
                        //    this.Grd_Participantes.DataBind();
                    }

                    if (Convert.ToInt32(Session["iACTUACION_ID" + HFGUID.Value]) > 0)
                    {
                        HF_ACT_ID.Value = Session["iACTUACION_ID" + HFGUID.Value].ToString();
                        Session.Remove("iACTUACION_ID" + HFGUID.Value);
                    }

                    if (Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]) > 0)
                    {
                        HF_ACTUACIONDET_ID.Value = Convert.ToString(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);
                        if (Session["InicioTramite"].Equals("2")) // viene de la pantalla frmActuación entonces no debe cargar nada ya que solo tiene pago
                        {
                            CargarDatosActuacionDetalle();
                        }
                    }

                    //if (HFGUID.Value.Length > 0)
                    //{
                    //    if (Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]) > 0)
                    //    {
                    //        HF_iPersonaID.Value = Session["iPersonaId" + HFGUID.Value].ToString();
                    //    }
                    //}
                    //else
                    //{

                    //}

                    DeshabilitarBotonVinculacion();

                    Session["iOperAnot"] = true;
                    HFAutodhesivo.Value = "0";

                    if ((Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.ACTUALIZACION) ||
                        (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.CONSULTA))
                    {
                        LstTarifario.Visible = false;
                        imgBuscarTarifarioM.Visible = false;
                        if ((Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.ACTUALIZACION ||
                            Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.CONSULTA) || Grd_Participantes.Rows.Count <= 0)
                        {
                            // Los botones se muestran despues de la digitalización
                            if (Convert.ToBoolean(Session["ACT_DIGITALIZA"]))
                            {
                                btnAnotacion.Visible = true;
                                btnCopiaCert.Visible = true;
                                HabilitaControlesTabFormato(false);
                                HabilitaControlesTabAnotacion(false);
                                ctrlToolBarRegistro.btnGrabar.Enabled = false;
                                ctrlToolbarFormato.btnGrabar.Enabled = false;
                                HFAutodhesivo.Value = "1";
                                //BtnVistaPrevia.Enabled = false;
                            }

                            if (Session["COD_AUTOADHESIVO"].ToString() != string.Empty)
                            {
                                if (txtIdTarifa.Text != Constantes.CONST_EXCEPCION_TARIFA_ID_1.ToString())
                                {
                                    ctrlToolBarRegistro.btnGrabar.Enabled = false;
                                    ctrlToolbarFormato.btnGrabar.Enabled = false;
                                    HabilitaControlesTabFormato(false);

                                    //HabilitaControlesTabAnotacion(false);
                                    Grd_Participantes.Enabled = false;
                                    btnAceptar.Enabled = false;
                                    btnCancelar.Enabled = false;
                                    HFAutodhesivo.Value = "1";
                                    cbxAfirmarTexto.Checked = true;
                                    cbxAfirmarTexto.Enabled = false;
                                }
                                updAnotaciones.Update();
                            }
                            if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
                            {
                                Grd_Participantes.Enabled = false;
                                btnAceptar.Enabled = false;
                                btnCancelar.Enabled = false;
                                Grd_Participantes.Enabled = false;
                                btnAceptar.Enabled = false;
                                btnCancelar.Enabled = false;
                                HabilitaControlesTabFormato(false);
                                HabilitaControlesTabAnotacion(false);
                                ctrlToolBarRegistro.btnGrabar.Enabled = false;
                                ctrlToolbarFormato.btnGrabar.Enabled = false;
                                ctrlToolbarFormato.btnEliminar.Enabled = false;
                                BtnGrabAnotacion.Enabled = false;
                                HabilitaControl(false);
                            }
                            updbotones.Update();
                            updRegPago.Update();
                            updFormato.Update();
                        }
                        else
                        {
                            ctrlToolBarRegistro.btnGrabar.Enabled = false;
                            txtObservaciones.Enabled = false;
                            ctrlToolbarFormato.btnGrabar.Enabled = false;
                            btnGrabarVinculacion.Enabled = false;
                            ctrlAdjunto.BtnGrabActAdj.Enabled = false;
                            BtnGrabAnotacion.Enabled = false;
                            //cbxAfirmarTexto.Enabled = false;
                            HabilitaControlesTabFormato(false);
                            HabilitaControl(false);

                            ctrlAdjunto.isConsultar = true;

                        }
                    }

                    Session["OperRegCiv"] = true;

                    #region FORMATO
                    if (txtIdTarifa.Text != Constantes.CONST_EXCEPCION_TARIFA_ID_1.ToString())
                    {

                        string strScript = string.Empty;
                        if (txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_ID_2.ToString())
                        {
                            ctrFecRegistro.set_Value = DateTime.Now;
                            ctrFecRegistro.Enabled = true;
                            updAnotaciones.Update();

                            btnActa.Visible = false;
                            ctrlToolbarFormato.btnGrabar.Enabled = false;
                            HabilitaControlesTabFormato(false);

                            ddlActaTarifa.Visible = false;
                            lblCO_ddlActaTarifa.Visible = false;
                            lblTipoActaTarifa.Visible = false;
                        }
                        else
                        {
                            if (txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_3A)
                            {
                                btnActa.Visible = true;
                                ddlActaTarifa.Visible = false;

                                btnActa.Visible = false;
                                ctrlToolbarFormato.btnGrabar.Enabled = false;
                                HabilitaControlesTabFormato(false);
                                txtNombresTitular.Enabled = false;
                                txtApePatTitular.Enabled = false;
                                txtApeMatTitular.Enabled = false;
                                ddlTipoActa.Enabled = false;
                                ddlActaTarifa.Visible = false;
                                lblCO_ddlActaTarifa.Visible = false;
                                lblTipoActaTarifa.Visible = false;
                            }
                            else
                            {
                                btnActa.Visible = false;
                                ctrlToolbarFormato.btnGrabar.Enabled = false;
                                HabilitaControlesTabFormato(true);
                                //--------------------------------
                                // Fecha: 24/10/2016
                                // Autor: Miguel Márquez Beltrán
                                // Objetivo: Ocultar Tipo de Acta.
                                //--------------------------------
                                //ddlActaTarifa.Visible = true;
                                //lblCO_ddlActaTarifa.Visible = true;
                                //lblTipoActaTarifa.Visible = true;                            
                                //--------------------------------

                                txtApePatTitular.Enabled = false;
                                txtApeMatTitular.Enabled = false;
                                txtNombresTitular.Enabled = false;
                                ddlTipoActa.Enabled = true;

                            }
                        }
                    }
                    else
                    {
                        //if (Convert.ToInt16(ddlTipoActa.SelectedValue) == Convert.ToInt16(Enumerador.enmTipoActa.NACIMIENTO))
                        //{
                        //    btnSolicitudInscr.Visible = true;
                        //    updVinculacion.Update();
                        //}
                        //else
                        //{
                        //    btnSolicitudInscr.Visible = false;
                        //    updVinculacion.Update();
                        //}
                        btnActa.Visible = true;
                        ddlActaTarifa.Visible = false;
                    }
                    #endregion

                    Cargar_Actuacion();
                    HabilitarTag();
                    ddl_TipoDatoParticipante.Enabled = false;

                    Load();
                    HabilitarControlParticipanteRune(false);


                    HF_ValoresDocumentoIdentidad.Value = string.Empty;

                    DataTable dt = new DataTable();

                    dt = Comun.ObtenerListaDocumentoIdentidad();

                    //DataTable dt = (DataTable)Session[Constantes.CONST_SESION_DT_DOCUMENTOIDENTIDAD];

                    foreach (DataRow dr in dt.Rows)
                    {
                        HF_ValoresDocumentoIdentidad.Value += dr["doid_sTipoDocumentoIdentidadId"].ToString() + "," +
                            dr["doid_sDigitosMinimo"].ToString() + "," + dr["doid_sDigitos"].ToString() + "," +
                         dr["doid_bNumero"].ToString() + "," + dr["doid_sTipoNacionalidad"].ToString() + "," +
                         dr["vMensajeError"].ToString() + "|";
                    }

                    // Jonatan -- 20/07/2017
                    if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
                    {
                        ctrlReimprimirbtn1.Activar = false;
                        ctrlBajaAutoadhesivo1.Activar = false;
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


                    int iIndiceComboTipoVinculoParticipante = Util.ObtenerIndiceComboPorText(ddl_TipoVinculoParticipante, "EL TITULAR");

                    if (iIndiceComboTipoVinculoParticipante >= 0)
                    {
                        ddl_TipoVinculoParticipante.Items[iIndiceComboTipoVinculoParticipante].Enabled = false;
                    }
                }
                else {
                    if (Session["COD_AUTOADHESIVO"].ToString().Length > 0 && Session["InicioTramite"].ToString().Equals("2"))
                    {
                        if (Session["ACTUALIZA"] != null)
                        {
                            if (Session["ACTUALIZA"].ToString() == "")
                            {
                                Session["ACTUALIZA"] = null;
                                BindGridActuacionesInsumoDetalle(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]));
                                String scriptMover = String.Empty;
                                scriptMover = @"$(function(){{MoveTabIndex(4);}});";
                                scriptMover = string.Format(scriptMover);
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);
                            }
                        }
                    }
                }

                Enable_vinculo();
                if (Session["InicioTramite"].ToString().Equals("2")) // viene de la pantalla frmActuación entonces no debe cargar nada ya que solo tiene pago
                {
                    ValidarDocumentos();
                }
                VerificarVariablesSesion();
                MostrarEdad();
                //----------------------------------------//
                // Autor: Jonatan Silva Cachay
                // Fecha: 14-02-2017
                // Objetivo: Verificar Enabled
                //----------------------------------------//            
                ctrlToolBarRegistro.btnGrabar.Enabled = true;
                if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
                {
                    ctrlToolBarRegistro.btnGrabar.Enabled = false;
                    updRegPago.Update();
                    ctrlAdjunto.BtnGrabActAdj.Enabled = false;
                    updActuacionAdjuntar.Update();
                }
                //----------------------------------------------------------
                //Fecha: 18/06/2019
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Activar el botón Vista Previa de 
                //acuerdo a la validación del número de participantes.
                //----------------------------------------------------------
                BtnVistaPrevia.Enabled = ValidarParticipantesRegistroCivil();
                cbxAfirmarTexto.Enabled = BtnVistaPrevia.Enabled;
                btnActa.Enabled = BtnVistaPrevia.Enabled;

                updVinculacion.Update();
                //----------------------------------------------------------
                //string strRegistroInicial_0 = Convert.ToString(Request.QueryString["bIni"]);

                if (strRegistroInicial == "1")
                {
                    if (hInicioTramite.Value == "2")
                    {
                        Session["InicioTramite"] = "2";
                    }
                    else
                    {
                        Session["InicioTramite"] = "1";
                    }
                }
                else
                { Session["InicioTramite"] = "2"; }

                if (HF_TARIFA_2.Value == txtIdTarifa.Text)
                {
                    BtnActaConformidad.Visible = false;
                }
                else
                {
                    BtnActaConformidad.Visible = true;
                }
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("The operation is not valid for the state") || ex.ToString().Contains("La operación no es válida para el estado"))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", "HTTP 404 - intentelo de nuevo", true);
                }
                else {
                    Session["_LastException"] = ex;
                    Response.Redirect("../PageError/GenericErrorPage.aspx");
                }
            }
        }

        void CargarUltimoInsumo()
        {
            DataTable _dt = new DataTable();
            InsumoConsultaBL _obj = new InsumoConsultaBL();
            _dt = _obj.ConsultarUltimoInsumoUsuario(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]));
            if (_dt.Rows.Count > 0)
            {
                if (txtCodAutoadhesivo.Text.Length == 0)
                {
                    txtCodAutoadhesivo.Text = _dt.Rows[0]["INSUMO"].ToString();
                }
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
            //    Response.Redirect("FrmActoCivil.aspx?cod=1&GUID=" + HFGUID.Value);
            //}
            //else
            //{
            string codPersona = Request.QueryString["CodPer"].ToString();
            if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
            {
                Response.Redirect("FrmActoCivil.aspx?cod=1&CodPer=" + codPersona + "&Juridica=1", false);
            }
            else
            { // PERSONA NATURAL
                Response.Redirect("FrmActoCivil.aspx?cod=1&CodPer=" + codPersona, false);
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
            //    Response.Redirect("FrmActoCivil.aspx?cod=0&GUID=" + HFGUID.Value, false);
            //}
            //else
            //{
            string codPersona = Request.QueryString["CodPer"].ToString();
            if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
            {
                Response.Redirect("FrmActoCivil.aspx?cod=3&CodPer=" + codPersona + "&Juridica=1", false);
            }
            else
            { // PERSONA NATURAL
                Response.Redirect("FrmActoCivil.aspx?cod=3&CodPer=" + codPersona, false);
            }
            
            //}
        }
  
        protected void txtIdTarifa_TextChanged(object sender, EventArgs e)
        {
            ddlTipoPago.SelectedIndex = 0;
            BuscarTarifario();
        }
       
        private void ValidarDocumentos()
        {
            string vTipoDoc = ddl_TipoDocParticipante.SelectedValue;
            if (vTipoDoc == "")
            { vTipoDoc = "0"; }
            if (Convert.ToInt16(vTipoDoc) > 0)
            {
                DataTable dt = new DataTable();

                dt = Comun.ObtenerListaDocumentoIdentidad();

                //DataTable dt = (DataTable)Session[Constantes.CONST_SESION_DT_DOCUMENTOIDENTIDAD];

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["doid_sTipoDocumentoIdentidadId"].ToString() == vTipoDoc)
                    {
                        if (dr["doid_sDigitos"].ToString() == string.Empty)
                            txtNroDocParticipante.MaxLength = 8;//Convert.ToInt16(dr["doid_sDigitos"]);
                        else
                            txtNroDocParticipante.MaxLength = Convert.ToInt16(dr["doid_sDigitos"]);


                        if (dr["doid_sTipoNacionalidad"].ToString() != string.Empty)
                            ddl_NacParticipante.Enabled = false;
                        else
                            ddl_NacParticipante.Enabled = true;
                    }
                }
            }
        }

        #region Actuacion

        protected void btnAnotacion_Click(object sender, EventArgs e)
        {
            hidEventNuevo.Value = "ev";

            txtIdTarifa.Text = Constantes.CONST_EXCEPCION_TARIFA_ID_2.ToString();
            BuscarTarifario();

            Comun.EjecutarScript(Page, Util.HabilitarTab(3) + Util.HabilitarTab(0) + Util.DeshabilitarTab(1) + Util.DeshabilitarTab(2) + Util.DeshabilitarTab(3) + Util.DeshabilitarTab(4));
            // Anotación
            ctrFecRegistro.set_Value = DateTime.Now;
            ctrFecRegistro.Enabled = true;

            cmb_TipoAnotacion.Enabled = true;
            txtDescAnotacion.Enabled = true;
            BtnGrabAnotacion.Enabled = true;

            LimpiarActuacionDetalle();
            ddlTipoPago.Enabled = true;
            ctrlToolBarRegistro.btnGrabar.Enabled = true;

            txtCodAutoadhesivo.Enabled = true;
            btnLimpiar.Enabled = true;
            btnVistaPrev.Enabled = false;
            chkImpresion.Enabled = false;

            updAnotaciones.Update();
            updbotones.Update();
            updActuacionAdjuntar.Update();
            updVinculacion.Update();
            updFormato.Update();
        }

        protected void btnCopiaCertificada_Click(object sender, EventArgs e)
        {
            hidEventNuevo.Value = "ev";

            txtIdTarifa.Text = Constantes.CONST_EXCEPCION_TARIFA_ID_3.ToString();
            BuscarTarifario();

            LimpiarActuacionDetalle();

            Comun.EjecutarScript(Page, Util.HabilitarTab(0) + Util.DeshabilitarTab(1) + Util.DeshabilitarTab(2) + Util.DeshabilitarTab(3) + Util.DeshabilitarTab(4));
            ctrlToolBarRegistro.btnGrabar.Enabled = true;

            ddlTipoPago.Enabled = true;

            txtCodAutoadhesivo.Enabled = true;
            btnLimpiar.Enabled = true;
            btnVistaPrev.Enabled = false;
            chkImpresion.Enabled = false;

            updAnotaciones.Update();
            updbotones.Update();
            updActuacionAdjuntar.Update();
            updVinculacion.Update();
            updFormato.Update();
        }

        private void LimpiarActuacionDetalle()
        {
            Session[strRegistroCivilId] = 0;
            Session["COD_AUTOADHESIVO"] = string.Empty;
            Session["ACTDET_REFERNCIA"] = Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value];
            Session["ActoCivil_Accion"] = Enumerador.enmTipoOperacion.REGISTRO;
            Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value] = 0;

            hifRegistroCivil.Value = "0";

            // Adjuntos
            ctrlAdjunto.HabilitarAdjuntos(true);
            updActuacionAdjuntar.Update();

            // Limpiar            
            btnGrabarVinculacion.Enabled = true;
            btnActa.Visible = false;
            chkImpresion.Checked = false;
            //txtCodAutoadhesivo.Enabled = true;
            txtCodAutoadhesivo.Text = string.Empty;
            //btnVistaPrev.Enabled = true;
            Grd_ActInsDet.DataSource = null;
            Grd_ActInsDet.DataBind();

            // Registro
            ddlActuacionTipo.SelectedValue = ddlTipoActa.SelectedValue;
            updRegPago.Update();

            // Formato = informativo
            HabilitaControlesTabFormato(false);
        }

        private void HabilitarControlParticipanteRune(Boolean estado)
        {
            //ddl_NacParticipante.Enabled = estado;
            txtNomParticipante.Enabled = estado;
            txtApeMatParticipante.Enabled = estado;
            txtApePatParticipante.Enabled = estado;
           // txtDireccionParticipante.Enabled = estado;
           // ctrlUbigeo1.HabilitaControl(estado);

            CmbEstCiv.Enabled = estado;

            CtrldFecNacimientoParticipante.Enabled = estado;
        }

        private void HabilitarControlParticipanteEspeciales(Boolean estado)
        {
            ddl_TipoDocParticipante.Enabled = estado;
            txtNroDocParticipante.Enabled = estado;
            ddl_NacParticipante.Enabled = estado;
            txtNomParticipante.Enabled = estado;
            txtApeMatParticipante.Enabled = estado;
            txtApePatParticipante.Enabled = estado;
          //  txtDireccionParticipante.Enabled = estado;
           // ctrlUbigeo1.HabilitaControl(estado);
        }

        private void LimpiarControlParticipanteRune()
        {
            ddl_NacParticipante.SelectedValue = "0";
            txtNomParticipante.Text = String.Empty;
            txtApeMatParticipante.Text = String.Empty;
            txtApePatParticipante.Text = String.Empty;
            ctrlUbigeo1.UbigeoRefresh();
        }

        private void ctrlToolBarRegistro_btnCancelarHandler()
        {
            //if (HFGUID.Value.Length > 0)
            //{
            //    Session["iPersonaId" + HFGUID.Value] = Session["iCodPersonaId" + HFGUID.Value];
            //}
            //else
            //{
            //    Session["iPersonaId"] = Session["iCodPersonaId"];
            //}
            Session.Remove("Participante");

            //if (HFGUID.Value.Length > 0)
            //{
            //    Response.Redirect("~/Registro/FrmTramite.aspx?GUID=" + HFGUID.Value);
            //}
            //else
            //{
            string codPersona = Request.QueryString["CodPer"].ToString();

            Response.Redirect("~/Registro/FrmTramite.aspx?CodPer="+codPersona,false);
            //}
        }

        private void ctrlToolBarRegistro_btnGrabarHandler()
        {
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
            //        _dtVerifica = _obj.verificarOperacion(lngActuacionDetalleId, Convert.ToInt16(ddlNomBanco.SelectedValue), txtNroOperacion.Text, Convert.ToInt16(ddlTipoPago.SelectedValue), Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), Comun.FormatearFecha(ctrFecPago.Text));

            //        if (_dtVerifica.Rows.Count > 0)
            //        {
            //            string StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", _dtVerifica.Rows[0]["Mensaje"].ToString(), false, 190, 250);
            //            Comun.EjecutarScript(Page, StrScript);
            //            return;
            //        }
            //    }
            //}
            
            bool bCumpleCondicion = false;
            bool bNoCobrado = ExisteInafecto_Exoneracion(hTipoPago.Value);


            if (bNoCobrado || Convert.ToInt32(hTipoPago.Value) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
            {
                if (RBNormativa.Visible == true && RBNormativa.Checked)
                {
                    if (ddlExoneracion.Visible == true)
                    {
                        if (ddlExoneracion.SelectedIndex == 0)
                        {
                            ddlExoneracion.Enabled = true;
                            ddlExoneracion.Focus();
                            Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", "Seleccione la Ley que exonera el Pago"));
                            return;
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
                            return;
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
                        return;
                    }
                }
                if (RBSustentoTipoPago.Visible == false)
                {
                    if (txtSustentoTipoPago.Visible == true && txtSustentoTipoPago.Text.Trim().Length == 0)
                    {
                        txtSustentoTipoPago.Enabled = true;
                        txtSustentoTipoPago.Focus();
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", "Digite el sustento"));
                        return;
                    }
                }
            }
            //----------------------------------------//
            // Autor: Jonatan Silva Cachay
            // Fecha: 14-02-2017
            // Objetivo: Verificar Enabled
            //----------------------------------------//

           
            if (ddlTipoPago.Items.FindByText("PAGO ARUBA") != null)
            {
                if (Convert.ToInt32(hTipoPago.Value) == Convert.ToInt32(ddlTipoPago.Items.FindByText("PAGO ARUBA").Value))
                {
                    bCumpleCondicion = true;
                }
            }
            if (ddlTipoPago.Items.FindByText("PAGO OTRAS ISLAS CARIBEÑAS") != null)
            {
                if (Convert.ToInt32(hTipoPago.Value) == Convert.ToInt32(ddlTipoPago.Items.FindByText("PAGO OTRAS ISLAS CARIBEÑAS").Value))
                {
                    bCumpleCondicion = true;
                }
            }

            if (bCumpleCondicion == true)
            {
                if (txtCodAutoadhesivo.Text.Length > 0 && txtCodAutoadhesivo.Enabled == false && bNoCobrado == false)
                {
                    string StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", "No se puede modificar el Tipo de Pago porque el autodesivo ya fue vinculado.", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }
            }
            if (hidEventNuevo.Value == "ev")
            {
                if (ValidaRegistroNuevaTarifa())
                {
                    RegistrarActuacion();

                    #region Tags
                    String scriptMover = String.Empty;
                    if (txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_ID_1.ToString())
                    {
                        scriptMover = @"$(function(){{EnableTabIndex(0);EnableTabIndex(1);EnableTabIndex(2); DisableTabIndex(3);EnableTabIndex(4); MoveTabIndex(0);}});";
                    }
                    else
                    {
                        if (txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_ID_2.ToString())
                        {
                            scriptMover = @"$(function(){{EnableTabIndex(0);DisableTabIndex(1);EnableTabIndex(2); EnableTabIndex(3);EnableTabIndex(4); MoveTabIndex(0);}});";
                        }
                        else
                        {
                            scriptMover = @"$(function(){{EnableTabIndex(0);DisableTabIndex(1);EnableTabIndex(2); DisableTabIndex(3);EnableTabIndex(4); MoveTabIndex(0);}});";
                        }

                        txtCodAutoadhesivo.Text = String.Empty;
                        Session["COD_AUTOADHESIVO"] = String.Empty;
                        chkImpresion.Checked = false;
                        //btnVistaPrev.Enabled = true;
                        ddlActaTarifa.Visible = true;
                        ddlActaTarifa.Enabled = false;
                        lblTipoActaTarifa.Visible = false;
                        lblCO_ddlActaTarifa.Visible = false;
                        Grd_Anotaciones.Enabled = true;

                        updAnotaciones.Update();
                        updVinculacion.Update();
                        updActuacionAdjuntar.Update();

                    }
                    scriptMover = string.Format(scriptMover);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);
                    #endregion
                }
                else
                {
                    string strScript = string.Empty;
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTUACIÓN", "Falta validar algunos campos.");
                    Comun.EjecutarScript(Page, strScript);
                }
            }
            else
            {
                ActualizarActuacionDetalle();
                
            }

            DeshabilitarBotonVinculacion();
        }

        private void ActualizarActuacionDetalle()
        {
            string StrScript = string.Empty;
            int IntRpta = 0;
            long lngActuacionDetalleId = 0;

            if (HFGUID.Value.Length > 0)
            {
                lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);
            }
            else
            {
                lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
            }

            if (lngActuacionDetalleId > 0)
            {
                BE.RE_ACTUACION ObjActuacBE = new RE_ACTUACION();
                BE.RE_ACTUACIONDETALLE ObjActuacDetBE = new RE_ACTUACIONDETALLE();

                ObjActuacDetBE.acde_iActuacionDetalleId = lngActuacionDetalleId;
                ObjActuacDetBE.acde_vNotas = txtObservaciones.Text.ToUpper().Replace("'", "''");
                ObjActuacBE.actu_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                ObjActuacBE.actu_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                ObjActuacBE.actu_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                object[] miArray = { ObjActuacBE, ObjActuacDetBE };

                Int16 intTipoPago = 0;
                if (ctrlToolBarRegistro.btnGrabar.Enabled)
                {
                    intTipoPago = Convert.ToInt16(ddlTipoPago.SelectedValue);
                    hTipoPago.Value = ddlTipoPago.SelectedValue.ToString();

                    Int64 intNormaTarifarioId = 0;

                    if (ddlExoneracion.Visible == true)
                    {
                        intNormaTarifarioId = Convert.ToInt64(ddlExoneracion.SelectedValue);
                    }
                    


                    #region Actualizar Pago
                    BE.RE_PAGO ObjPagoBE = new BE.RE_PAGO();
                    ObjPagoBE.pago_iNormaTarifarioId = intNormaTarifarioId;

                    if (txtSustentoTipoPago.Visible == true)
                    {
                        ObjPagoBE.pago_vSustentoTipoPago = txtSustentoTipoPago.Text.Trim();
                    }
                    else
                    {
                        ObjPagoBE.pago_vSustentoTipoPago = "";
                    }
                    
                    ObjPagoBE.pago_sPagoTipoId = intTipoPago;
                    
                    ObjPagoBE.pago_sMonedaLocalId = Comun.ObtenerMonedaLocalId(Session, ddlTipoPago.SelectedValue, txtIdTarifa.Text);

                    if (HFGUID.Value.Length > 0)
                    {
                        ObjPagoBE.pago_iActuacionDetalleId = Comun.ToNullInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);
                    }
                    else
                    {
                        ObjPagoBE.pago_iActuacionDetalleId = Comun.ToNullInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
                    }
                    ObjPagoBE.pago_FTipCambioBancario = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO_BANCARIO]);
                    ObjPagoBE.pago_FTipCambioConsular = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]);

                    ObjPagoBE.pago_sUsuarioModificacion = Comun.ToNullInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    ObjPagoBE.pago_vIPModificacion = Util.ObtenerDireccionIP();

                    if (Convert.ToInt32(ObjPagoBE.pago_sPagoTipoId) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA) ||
                        Convert.ToInt32(ObjPagoBE.pago_sPagoTipoId) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA) ||
                        Convert.ToInt32(ObjPagoBE.pago_sPagoTipoId) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA))
                    {
                    
                        if (ddlNomBanco.SelectedIndex > 0)
                            ObjPagoBE.pago_sBancoId = Convert.ToInt16(ddlNomBanco.SelectedValue);

                        ObjPagoBE.pago_vBancoNumeroOperacion = txtNroOperacion.Text.Trim();
                        ObjPagoBE.pago_dFechaOperacion = Comun.FormatearFecha(ctrFecPago.Text);

                        if (Convert.ToDouble(txtMtoCancelado.Text) > 0)
                        {
                            if (Comun.IsNumeric(txtMtoCancelado.Text))
                            {
                                if (Convert.ToInt32(ObjPagoBE.pago_sPagoTipoId) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA))
                                {
                                    ObjPagoBE.pago_FMontoMonedaLocal = Convert.ToDouble(txtTotalML.Text);
                                    ObjPagoBE.pago_FMontoSolesConsulares = Convert.ToDouble(txtTotalSC.Text);
                                }
                                else
                                {
                                    ObjPagoBE.pago_FMontoMonedaLocal = Convert.ToDouble(txtMontoML.Text);
                                    ObjPagoBE.pago_FMontoSolesConsulares = Convert.ToDouble(txtMontoSC.Text);
                                }
                            }
                            else
                            {
                                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", "El formato del monto de pago es incorrecto.", false, 190, 320);
                                Comun.EjecutarScript(Page, StrScript);
                                return;
                            }
                        }
                        else
                        {
                            StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", "No ha colocado el monto de pago.", false, 190, 320);
                            Comun.EjecutarScript(Page, StrScript);
                            return;
                        }
                    }
                    else
                    {
                        ObjPagoBE.pago_vBancoNumeroOperacion = "";
                        ObjPagoBE.pago_FTipCambioBancario = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO_BANCARIO]);
                        ObjPagoBE.pago_FTipCambioConsular = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]);
                        ObjPagoBE.pago_dFechaOperacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());

                        ObjPagoBE.pago_FMontoMonedaLocal = Convert.ToDouble(txtMontoML.Text);
                        ObjPagoBE.pago_FMontoSolesConsulares = Convert.ToDouble(txtMontoSC.Text);

                    }

                    if (Convert.ToInt32(ObjPagoBE.pago_sPagoTipoId) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.NO_COBRADO) ||
                        Convert.ToInt32(ObjPagoBE.pago_sPagoTipoId) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
                    {
                        ObjPagoBE.pago_FMontoMonedaLocal = 0;
                        ObjPagoBE.pago_FMontoSolesConsulares = 0;
                    }

                    ObjPagoBE.pago_bPagadoFlag = false;
                    ObjPagoBE.pago_vComentario = "";
                    IntRpta = new ActuacionPagoMantenimientoBL().Actualizar(ObjPagoBE, Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                    
                    //--------------------------------------------------------
                    if (IntRpta > 0)
                    {
                        //ddlTipoPago.Enabled = false;
                        txtNroOperacion.Enabled = false;
                        ddlNomBanco.Enabled = false;
                        ctrFecPago.Enabled = false;

                        updRegPago.Update();
                    }
                    else
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", "Ocurrió un problema en la actualización del pago.", false, 190, 250);
                        Comun.EjecutarScript(Page, StrScript);
                        return;
                    }
                    #endregion
                }

                ActuacionMantenimientoBL objBL = new ActuacionMantenimientoBL();
                IntRpta = objBL.Actualizar(ObjActuacBE, ObjActuacDetBE);
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

        private void RegistrarActuacion()
        {
            string StrScript = string.Empty;
            long lngActuacionId = 0;
            long lngActuacionDetalleId = 0;

            BE.RE_PAGO ObjPagoBE = new BE.RE_PAGO();
            BE.RE_ACTUACION ObjActuacBE = new BE.RE_ACTUACION();
            BE.MRE.SI_TARIFARIO objTarifarioBE = new BE.MRE.SI_TARIFARIO();
            BE.RE_ACTUACIONDETALLE ObjActuacDetBE = new BE.RE_ACTUACIONDETALLE();

            if (Session[strVariableTarifario] != null)
            {
                objTarifarioBE = (BE.MRE.SI_TARIFARIO)Session[strVariableTarifario];

                ObjActuacBE.actu_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                ObjActuacBE.actu_FCantidad = Convert.ToInt32(txtCantidad.Text);

                //if (HFGUID.Value.Length > 0)
                //{
                //    ObjActuacBE.actu_iPersonaRecurrenteId = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
                //}
                //else
                //{
                    ObjActuacBE.actu_iPersonaRecurrenteId = Convert.ToInt64(ViewState["iPersonaId"]);
                //}
                ObjActuacBE.actu_IFuncionarioId = null;
                ObjActuacBE.actu_dFechaRegistro = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                ObjActuacBE.actu_sEstado = (int)Enumerador.enmActuacionEstado.REGISTRADO;
                ObjActuacBE.actu_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                ObjActuacBE.actu_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                if (objTarifarioBE.tari_sCalculoTipoId == (int)Enumerador.enmTipoCalculoTarifario.MONTO_FIJO)
                {
                    for (int i = 1; i <= ObjActuacBE.actu_FCantidad; i++)
                    {
                        DataRow rowDetAct;

                        rowDetAct = ((DataTable)Session["DtDetActuaciones" + HFGUID.Value]).NewRow();

                        rowDetAct["sTarifarioId"] = objTarifarioBE.tari_sTarifarioId;
                        rowDetAct["sItem"] = i;
                        rowDetAct["dFechaRegistro"] = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                        rowDetAct["bRequisitosFlag"] = 0;
                        rowDetAct["sVinculacionInsumoId"] = 0;
                        rowDetAct["dVinculacionFecha"] = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                        rowDetAct["ICorrelativoActuacion"] = 0;
                        rowDetAct["ICorrelativoTarifario"] = 0;
                        rowDetAct["sFuncionarioFirmanteId"] = DBNull.Value;
                        rowDetAct["sFuncionarioContactoId"] = DBNull.Value;
                        rowDetAct["bImpresionFlag"] = 0;
                        rowDetAct["dImpresionFecha"] = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                        rowDetAct["sImpresionFuncionarioId"] = DBNull.Value;

                        ((DataTable)Session["DtDetActuaciones" + HFGUID.Value]).Rows.Add(rowDetAct);
                    }
                }
                else
                {
                    DataRow rowDetAct;
                    rowDetAct = ((DataTable)Session["DtDetActuaciones" + HFGUID.Value]).NewRow();

                    rowDetAct["sTarifarioId"] = objTarifarioBE.tari_sTarifarioId;
                    rowDetAct["sItem"] = 1;
                    rowDetAct["dFechaRegistro"] = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                    rowDetAct["bRequisitosFlag"] = 0;
                    rowDetAct["sVinculacionInsumoId"] = 0;
                    rowDetAct["dVinculacionFecha"] = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                    rowDetAct["ICorrelativoActuacion"] = 0;
                    rowDetAct["ICorrelativoTarifario"] = 0;
                    rowDetAct["sFuncionarioFirmanteId"] = DBNull.Value;
                    rowDetAct["sFuncionarioContactoId"] = DBNull.Value;
                    rowDetAct["bImpresionFlag"] = 0;
                    rowDetAct["dImpresionFecha"] = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                    rowDetAct["sImpresionFuncionarioId"] = DBNull.Value;

                    ((DataTable)Session["DtDetActuaciones" + HFGUID.Value]).Rows.Add(rowDetAct);
                }

                #region Cargar Datos Pago Actuación

                ObjPagoBE.pago_sPagoTipoId = Convert.ToInt16(ddlTipoPago.SelectedValue);
                hTipoPago.Value = ddlTipoPago.SelectedValue.ToString();
                

                ObjPagoBE.pago_sMonedaLocalId = Comun.ObtenerMonedaLocalId(Session, ddlTipoPago.SelectedValue, txtIdTarifa.Text);


                if (Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA) ||
                    Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA) ||
                    Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA))
                {
                
                    if (ddlNomBanco.SelectedIndex > 0)
                        ObjPagoBE.pago_sBancoId = Convert.ToInt16(ddlNomBanco.SelectedValue);

                    ObjPagoBE.pago_vBancoNumeroOperacion = txtNroOperacion.Text.Trim();

                    if (ctrFecPago.Value() != DateTime.MinValue)
                    {
                        ObjPagoBE.pago_dFechaOperacion = ctrFecPago.Value();
                    }

                    if (txtMtoCancelado.Text.Length > 0)
                    {
                        if (Comun.IsNumeric(txtMtoCancelado.Text))
                        {
                            //ObjPagoBE.pago_FMontoMonedaLocal = Convert.ToDouble(txtMtoCancelado.Text);
                            //ObjPagoBE.pago_FMontoSolesConsulares = Convert.ToDouble(txtTotalSC.Text);

                            ObjPagoBE.pago_FMontoMonedaLocal = Convert.ToDouble(txtMontoML.Text);
                            ObjPagoBE.pago_FMontoSolesConsulares = Convert.ToDouble(txtMontoSC.Text);
                        }
                    }
                    txtNroOperacion.Enabled = false;
                    ddlNomBanco.Enabled = false;
                    ctrFecPago.Enabled = false;
                    txtMtoCancelado.Enabled = false;

                    updRegPago.Update();
                }
                else
                {
                    ObjPagoBE.pago_vBancoNumeroOperacion = "";
                    ObjPagoBE.pago_FTipCambioBancario = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO_BANCARIO]);
                    ObjPagoBE.pago_FTipCambioConsular = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]);
                    ObjPagoBE.pago_dFechaOperacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                    //ObjPagoBE.pago_FMontoMonedaLocal = Convert.ToDouble(txtTotalML.Text);
                    //ObjPagoBE.pago_FMontoSolesConsulares = Convert.ToDouble(txtTotalSC.Text);
                    // dblTotalSolesConsulares
                    ObjPagoBE.pago_FMontoMonedaLocal = Convert.ToDouble(txtMontoML.Text);
                    ObjPagoBE.pago_FMontoSolesConsulares = Convert.ToDouble(txtMontoSC.Text);

                }

                if (Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.NO_COBRADO) || Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
                {
                    ObjPagoBE.pago_FMontoMonedaLocal = 0;
                    ObjPagoBE.pago_FMontoSolesConsulares = 0;
                }

                ObjPagoBE.pago_bPagadoFlag = false;
                ObjPagoBE.pago_vComentario = "";

                BE.RE_TARIFA_PAGO oRE_TARIFA_PAGO = new RE_TARIFA_PAGO();

                #endregion Cargar Datos Pago Actuación


                object[] miArray = { ObjActuacBE,
					 (DataTable)Session["DtDetActuaciones" + HFGUID.Value],
					 ObjPagoBE,
					 lngActuacionId };

                Proceso MiProc = new Proceso();
                MiProc.Invocar(ref miArray, "SGAC.BE.RE_ACTUACION", Enumerador.enmAccion.INSERTAR);

                lngActuacionId = Convert.ToInt64(miArray[3]);
                HF_ACT_ID.Value = lngActuacionId.ToString();
                lngActuacionDetalleId = Convert.ToInt64(((BE.RE_PAGO)miArray[2]).pago_iActuacionDetalleId);
            }

            if (lngActuacionDetalleId > 0)
            {
                Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value] = lngActuacionId;

                if (HFGUID.Value.Length > 0)
                {
                    Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value] = lngActuacionDetalleId;
                }
                else
                {
                    Session[Constantes.CONST_SESION_ACTUACIONDET_ID] = lngActuacionDetalleId;
                }

                SGAC.Registro.Actuacion.BL.ActuacionAdjuntoConsultaBL oActuacionAdjuntoConsultaBL = new ActuacionAdjuntoConsultaBL();
                List<BE.MRE.RE_ACTUACIONADJUNTO> lActuacionAdjunto = new List<BE.MRE.RE_ACTUACIONADJUNTO>();

                lActuacionAdjunto = oActuacionAdjuntoConsultaBL.ActuacionAdjuntoObtenerDigitalizados(Convert.ToInt64(HF_ACTUACIONDET_ID.Value));

                foreach (BE.MRE.RE_ACTUACIONADJUNTO oRE_ACTUACIONADJUNTO in lActuacionAdjunto)
                {
                    RE_ACTUACIONADJUNTO ObjAdjActBE = new RE_ACTUACIONADJUNTO();
                    ObjAdjActBE.acad_iActuacionDetalleId = lngActuacionDetalleId;
                    ObjAdjActBE.acad_sAdjuntoTipoId = 4201;
                    ObjAdjActBE.acad_vNombreArchivo = oRE_ACTUACIONADJUNTO.vNombreArchivo;
                    ObjAdjActBE.acad_vDescripcion = "COPIA DEL DOCUMENTO DIGITALIZADO DE LA TARIFA 1";

                    ObjAdjActBE.acad_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    ObjAdjActBE.acad_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    ObjAdjActBE.acad_bBloqueoAdjunto = true;
                    ActuacionAdjuntoMantenimientoBL oActuacionAdjuntoMantenimientoBL = new ActuacionAdjuntoMantenimientoBL();
                    Int32 IntRpta = oActuacionAdjuntoMantenimientoBL.Insertar(ObjAdjActBE, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));

                    ctrlAdjunto.CargarGrillaAdjuntos(lngActuacionDetalleId);
                }

                ctrlToolbarFormato.btnGrabar.Enabled = false;
                LimpiarVinculacion();

                ctrlToolBarRegistro.btnGrabar.Enabled = false;
                ddlTipoPago.Enabled = false;
                txtCantidad.Enabled = false;

                btnAnotacion.Visible = false;
                btnCopiaCert.Visible = false;
                ddlTipoActa.Enabled = false;
                hidEventNuevo.Value = "";

                HFAutodhesivo.Value = "0";
                txtCodAutoadhesivo.Text = String.Empty;
                btnVistaPrev.Enabled = false;
                chkImpresion.Checked = false;
                btnGrabarVinculacion.Enabled = true;

                HF_ACT_ID.Value = lngActuacionId.ToString();
                Cargar_Actuacion();

                BE.RE_TARIFA_PAGO objTarifaPago = ObtenerDatosTarifaPago(Convert.ToInt64(lngActuacionDetalleId));
                Session.Add(Constantes.CONST_SESION_OBJ_TARIFA_PAGO, objTarifaPago);

                PintarDatosPestaniaRegistro();
                updVinculacion.Update();

                if (txtIdTarifa.Text.Trim() != Constantes.CONST_EXCEPCION_TARIFA_ID_1.ToString() && txtIdTarifa.Text.Trim() != Constantes.CONST_EXCEPCION_TARIFA_3A)
                {
                    btnAnotacion.Visible = false;
                    btnCopiaCert.Visible = false;
                }

                updbotones.Update();
                updRegPago.Update();
                updbotones.Update();
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "ACTUACION", Constantes.CONST_MENSAJE_EXITO);
            }
            else
            {
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTUACION", Constantes.CONST_MENSAJE_OPERACION_FALLIDA);
            }

            Comun.EjecutarScript(Page, StrScript);
        }

        private bool ValidaRegistroNuevaTarifa()
        {
            bool bolValidado = true;

            if (txtCantidad.Text.Trim().Length == 0)
                bolValidado = false;
            if (ddlTipoPago.SelectedIndex < 1)
                bolValidado = false;
            else if (
                Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA) ||
                Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA) ||
                Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA))
            {
                if (txtNroOperacion.Text.Trim().Length == 0)
                    bolValidado = false;
                else if (ddlNomBanco.SelectedIndex < 1)
                    bolValidado = false;
                else if (txtMtoCancelado.Text.Trim().Length == 0)
                    bolValidado = false;
            }

            return bolValidado;
        }

        #endregion Actuacion

        #region Tarifario

        private void BuscarTarifario()
        {
            Tarifa tarifa = new Tarifa();
            DataTable dtTarifario = new DataTable();
            object[] arrParametros = { 0,
                                       txtIdTarifa.Text,
                                       "",
                                       ((char)Enumerador.enmEstado.ACTIVO).ToString(),
                                       1,   50,       0,        0 };

            dtTarifario = comun_Part2.ObtenerTarifario(Session, ref arrParametros);
            objTarifarioBE = tarifa.GetTarifario(dtTarifario, 0);
            Session[strVariableTarifario] = objTarifarioBE;

            string strFormato = ConfigurationManager.AppSettings["FormatoMonto"].ToString();
            double dblCero = 0;
            txtDescTarifa.Text = objTarifarioBE.tari_vDescripcionCorta;
            txtMontoSC.Text = dblCero.ToString(strFormato); ;
            txtMontoML.Text = dblCero.ToString(strFormato);
            txtTotalSC.Text = dblCero.ToString(strFormato);
            txtTotalML.Text = dblCero.ToString(strFormato);

            #region Limpiar datos del pago

            lblCantidad.Text = "Cantidad:";
            txtCantidad.Text = "1";
            txtCantidad.Enabled = false;

            txtNroOperacion.Text = "";
            ddlNomBanco.SelectedIndex = 0;
            txtMtoCancelado.Text = "0";
            txtObservaciones.Text = string.Empty;

            ctrFecPago.Text = DateTime.Now.ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
            ddlTipoPago.SelectedValue = "0";

            #endregion Limpiar datos del pago

            CalculoxTarifarioxTipoPagoxCantidad();

            updRegPago.Update();
        }

        private void CalculoxTarifarioxTipoPagoxCantidad()
        {
            int intCantidad = 1;
            string strScript = string.Empty;
            string strDescripcionTarifa = string.Empty;
            double decMontoSC = 0, decTotalSC = 0;
            double decMontoML = 0, decTotalML = 0;

            objTarifarioBE = (BE.MRE.SI_TARIFARIO)Session[strVariableTarifario];

            txtCantidad.Enabled = (bool)objTarifarioBE.tari_bHabilitaCantidad;
            decMontoSC = (double)objTarifarioBE.tari_FCosto;

            HabilitaPorTarifa();
            if (!string.IsNullOrEmpty(txtCantidad.Text))
            {
                intCantidad = Convert.ToInt32(txtCantidad.Text);
            }

            if (txtCantidad.Enabled)
            {
                txtCantidad.Focus();
            }

            if (intCantidad > 0)
            {
                decTotalSC = Tarifario.Calculo(objTarifarioBE, intCantidad);
                decMontoML = CalculaCostoML(decMontoSC, Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]));
                decTotalML = CalculaCostoML(decTotalSC, Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]));
            }

            txtCantidad.Text = intCantidad.ToString();
            string strFormato = ConfigurationManager.AppSettings["FormatoMonto"].ToString();

            txtMontoSC.Text = decMontoSC.ToString(strFormato);
            txtMontoML.Text = decMontoML.ToString(strFormato);

            txtTotalSC.Text = decTotalSC.ToString(strFormato);
            txtTotalML.Text = decTotalML.ToString(strFormato);

            if (Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA) ||
                Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA) ||
                Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA))
            {
                HabilitaDatosPago();
                pnlPagLima.Visible = true;

                txtMtoCancelado.Text = (Convert.ToDouble(txtCantidad.Text) * Convert.ToDouble(txtMontoML.Text)).ToString(strFormato);
            }
            else if (Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.NO_COBRADO) || Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
            {
                pnlPagLima.Visible = false;
                txtTotalSC.Text = "0";
                txtTotalML.Text = "0";
            }
            else
            {
                pnlPagLima.Visible = false;
            }
        }

        protected void cmb_TipoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            BE.RE_TARIFA_PAGO objTarifaPago = new RE_TARIFA_PAGO();
            objTarifaPago = (BE.RE_TARIFA_PAGO)Session[Constantes.CONST_SESION_OBJ_TARIFA_PAGO];
            if (objTarifaPago != null)
            {
                string strDescTipoPagoOrigen = Comun.ObtenerDescripcionTipoPago(Session, objTarifaPago.sTipoPagoId.ToString());


                Comun.ActualizarControlPago(Session, strDescTipoPagoOrigen, txtIdTarifa.Text, txtCantidad.Text,
                    ref ctrlToolBarRegistro.btnGrabar, ref ddlTipoPago, ref txtNroOperacion, ref txtCodAutoadhesivo,
                    ref ddlNomBanco, ref ctrFecPago, ref ddlExoneracion, ref lblExoneracion, ref lblValExoneracion,
                    ref txtSustentoTipoPago, ref lblSustentoTipoPago, ref lblValSustentoTipoPago,
                    ref RBNormativa, ref RBSustentoTipoPago, ref txtMontoML, ref txtMontoSC,
                    ref txtTotalML, ref txtTotalSC, ref LblDescMtoML, ref LblDescTotML,
                    ref pnlPagLima, ref txtMtoCancelado);
            }            


            updRegPago.Update();
        }

        protected void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            CalculoxTarifarioxTipoPagoxCantidad();
        }

        #endregion Tarifario

        #region Formato

        protected void ddlTipoActa_SelectedIndexChanged(object sender, EventArgs e)
        {
            tablaInscripcionOficio.Visible = false;
            tablaReconocimientoAdopcion.Visible = false;
            tablaReconstitucionReposicion.Visible = false;            
            txtNumeroActaAnterior.Text = "";
            txtTitularActa.Text = "";
            ddl_Genero.Enabled = true;
            if (Convert.ToInt32(this.ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.NACIMIENTO)
            {
                tablaCUI.Visible = true;
                chksinCUI.Checked = true;
                chkconCUI.Checked = false;
                pnlActaAnterior.Visible = false;
                ddl_Genero.Enabled = true;
                txtNombresTitular.Enabled = true;
                txtApePatTitular.Enabled = true;
                txtApeMatTitular.Enabled = true;
                txtNroCUI.Enabled = true;
                txtNroCUI.Text = "";
            }
            else
            {
                if (Convert.ToInt32(this.ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.DEFUNCION)
                {
                   tablaInscripcionOficio.Visible = true;
                   chkInscripcionOficio.Checked = false;
                } 
                tablaCUI.Visible = false;                
                pnlActaAnterior.Visible = false;
                tablaReconstitucionReposicion.Visible = true;
                chkReconstitucionReposicion.Checked = false;
                imgBuscarCUI.Visible = false;
            }
               
            //---------------------------------------------------------------------
            //Fecha: 25/09/2018
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Limpiar la observación.
            //---------------------------------------------------------------------
            txtCivilObservaciones.Text = "";
            //----------------------------------------------------------
            //ticket 354
            if ((Convert.ToInt32(this.ddlTipoActa.SelectedValue) != (int)Enumerador.enmTipoActa.DEFUNCION))
            {
                txtLugarOcurrencia.MaxLength = 50;
            }
            else {
                txtLugarOcurrencia.MaxLength = 70;
                ddl_Genero.Enabled = false;
                txtNombresTitular.Enabled = false;
                txtApePatTitular.Enabled = false;
                txtApeMatTitular.Enabled = false;
            }
            ddl_NacParticipante.Visible = true;
            Label49.Visible = true;
            Label65.Visible = true;
            limpiar_cabezera();
            if (txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_ID_1.ToString())
            {
                mtLayoutCivil();
                LimpiarDatosParticipante();
              //  this.ctrlUbigeo1.HabilitaControl(false);
                ctrlUbigeo1.ClearControl();
                ctrlUbigeo1.UbigeoRefresh();

                ddlActaTarifa.SelectedValue = ddlTipoActa.SelectedValue;

                if (Convert.ToInt32(this.ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.NACIMIENTO || (Convert.ToInt32(this.ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.DEFUNCION))
                {
                    ddl_TipoDatoParticipante.Visible = true;
                    ddl_TipoDatoParticipante.Enabled = false;
                    lblPartTipoVinc.Visible = true;
                    ddl_TipoVinculoParticipante.Enabled = true;
                    // se quitara al padre y madre de la lista de vinculos======
                    Enable_vinculo();
                    //==========================================================
                }
                else
                {
                    ddl_TipoDatoParticipante.Visible = false;
                    ddl_TipoDatoParticipante.Enabled = false;
                    lblPartTipoVinc.Visible = false;
                    Enable_estado();
                    //ddl_TipoVinculoParticipante.Enabled = false;
                }


                lblEstadoCivil.Visible = false;
                CmbEstCiv.Visible = false;
                lbldFecNacParticipante.Visible = false;
                CtrldFecNacimientoParticipante.Visible = false;
                lblObligaFecNacimientoParticipante.Visible = false;
                //HFEsRune_nacimiento_titular.Value = "0";
                txtFecNac.Enabled = true;
                txtFecNac.Text = String.Empty;
                ddl_Genero.SelectedValue = "0";

                lbltienendocumento.Visible = false;
                rbSi.Visible = false;
                rbNo.Visible = false;
                rbSi.Checked = true;
                rbNo.Checked = false;
                HF_TIENE_DOCUMENTO.Value = "0";

                ddl_TipoDocParticipante.Enabled = true;
                txtNroDocParticipante.Enabled = true;
            }

            int iIndiceComboTipoVinculoParticipante = Util.ObtenerIndiceComboPorText(ddl_TipoVinculoParticipante, "EL TITULAR");

            if (iIndiceComboTipoVinculoParticipante >= 0)
            {
                ddl_TipoVinculoParticipante.Items[iIndiceComboTipoVinculoParticipante].Enabled = false;
            }

            Activar_Ubicacion(true);
            if ((Convert.ToInt32(this.ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.MATRIMONIO))
            {
                CtrldFecNacimientoParticipante.AutoPostBack = false;
            }
            else
            {
                CtrldFecNacimientoParticipante.AutoPostBack = true;
            }
            updFormato.Update();
        }

        

        private void mtLayoutCivil()
        {
            ConfigurarTabFormato(Convert.ToInt32(this.ddlTipoActa.SelectedValue));
            this.ddlActuacionTipo.SelectedValue = ddlTipoActa.SelectedValue;

            if (Convert.ToInt32(this.ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.NACIMIENTO)
            {
                tablaCUI.Visible = true;
                if (this.hifRegistroCivil.Value.Equals("0"))
                {
                    this.chksinCUI.Checked = false;
                    this.chkconCUI.Checked = true;
                }
                Session["TIPO_ACTO_PARTICIPANTE"] = (int)Enumerador.enmTipoActa.NACIMIENTO;
                this.lblCUI.Visible = true;
                this.txtNroCUI.Visible = true;
                this.lblCO_txtNroCUI.Visible = true;

                this.lblHoraTitular.Visible = true;
                this.txtHora.Visible = true;

                this.Label12.Visible = true;
                this.ddl_Genero.Visible = true;

                this.Label69.Visible = true;
                this.txtNombresTitular.Visible = true;

                this.Label68.Visible = true;
                this.txtApePatTitular.Visible = true;

                this.Label68.Visible = true;
                this.txtApeMatTitular.Visible = true;


                Grd_Participantes.DataSource = new DataTable();
                Grd_Participantes.DataBind();
                Session["Participante"] = new List<RE_PARTICIPANTE>();

                ddl_TipoDatoParticipante.Visible = true;
            }
            else if (Convert.ToInt32(this.ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.MATRIMONIO)
            {
                tablaCUI.Visible = false;
                if (this.hifRegistroCivil.Value.Equals("0"))
                {
                    pnlActaAnterior.Visible = false;
                    tablaReconstitucionReposicion.Visible = true;
                    chkReconstitucionReposicion.Checked = false;
                }
                Session["TIPO_ACTO_PARTICIPANTE"] = (int)Enumerador.enmTipoActa.MATRIMONIO;
                this.lblCUI.Visible = false;
                this.txtNroCUI.Visible = false;
                this.lblCO_txtNroCUI.Visible = false;
                ddl_TipoDatoParticipante.Visible = false;
                Grd_Participantes.DataSource = new DataTable();
                Grd_Participantes.DataBind();
                Session["Participante"] = new List<RE_PARTICIPANTE>();
            }
            else if (Convert.ToInt32(this.ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.DEFUNCION)
            {
                tablaCUI.Visible = false;
                if (this.hifRegistroCivil.Value.Equals("0"))
                {
                    pnlActaAnterior.Visible = false;
                    tablaReconstitucionReposicion.Visible = true;
                    chkReconstitucionReposicion.Checked = false;
                    tablaInscripcionOficio.Visible = true;
                }
                ddl_TipoDatoParticipante.Visible = false;
                Session["TIPO_ACTO_PARTICIPANTE"] = (int)Enumerador.enmTipoActa.DEFUNCION;
                this.lblCUI.Visible = false;
                this.txtNroCUI.Visible = false;
                this.lblCO_txtNroCUI.Visible = false;

                Grd_Participantes.DataSource = new DataTable();
                Grd_Participantes.DataBind();
                Session["Participante"] = new List<RE_PARTICIPANTE>();
            }
            mtParticipanteInitialize();
            updFormato.Update();
        }

        private DateTime FechaHora(string fecha, string hora = "")
        {
            DateTime loFecha;
            String loFechaHora = (fecha + ((hora.Length != 0) ? " " + hora : string.Empty));
            try
            {
                loFecha = Comun.FormatearFecha(loFechaHora);
            }
            catch
            {
                loFecha = new DateTime();
            }
            return loFecha;
        }

        private List<BE.RE_PARTICIPANTE> Participantes(BE.MRE.RE_REGISTROCIVIL civil)
        {
            List<BE.RE_PARTICIPANTE> loParticipanteContainer = (List<BE.RE_PARTICIPANTE>)Session["Participante"];

            #region SOLO SI ES NACIMIENTO
            foreach (BE.RE_PARTICIPANTE participante in loParticipanteContainer.Where(p => p.sTipoParticipanteId == (int)Enumerador.enmParticipanteNacimiento.TITULAR))
            {
                if (Convert.ToInt32(Session["TIPO_ACTO_PARTICIPANTE"]) == (int)Enumerador.enmTipoActa.NACIMIENTO)
                {
                    participante.pers_dNacimientoFecha = Comun.FormatearFecha(civil.reci_dFechaHoraOcurrenciaActo.ToString());
                    participante.sGeneroId = Convert.ToInt32(this.ddl_Genero.SelectedValue);
                    participante.pers_cNacimientoLugar = civil.reci_cOcurrenciaUbigeo;

                    txtNombresTitular.Text = participante.vNombres;
                    txtApePatTitular.Text = participante.vPrimerApellido;
                    txtApeMatTitular.Text = participante.vSegundoApellido;
                }
            }
          

            foreach (BE.RE_PARTICIPANTE participante in loParticipanteContainer.Where(p => p.sTipoParticipanteId == (int)Enumerador.enmParticipanteNacimiento.DECLARANTE_1))
            {
                if (Convert.ToInt32(Session["TIPO_ACTO_PARTICIPANTE"]) == (int)Enumerador.enmTipoActa.NACIMIENTO)
                {
                    participante.pers_dNacimientoFecha = null;
                    participante.pers_cNacimientoLugar = null;
                }
            }
            foreach (BE.RE_PARTICIPANTE participante in loParticipanteContainer.Where(p => p.sTipoParticipanteId == (int)Enumerador.enmParticipanteNacimiento.DECLARANTE_2))
            {
                if (Convert.ToInt32(Session["TIPO_ACTO_PARTICIPANTE"]) == (int)Enumerador.enmTipoActa.NACIMIENTO)
                {
                    participante.pers_dNacimientoFecha = null;
                    participante.pers_cNacimientoLugar = null;
                }
            }
            #endregion

            #region SOLO SI ES DEFUNCION
            foreach (BE.RE_PARTICIPANTE participante in loParticipanteContainer.Where(p => p.sTipoParticipanteId == (int)Enumerador.enmParticipanteDefuncion.TITULAR))
            {
                if (Convert.ToInt32(Session["TIPO_ACTO_PARTICIPANTE"]) == (int)Enumerador.enmTipoActa.DEFUNCION)
                {
                    participante.pers_bFallecidoFlag = true;
                    participante.pers_dFechaDefuncion = FechaHora(this.txtFecNac.Text, this.txtHora.Text);
                    participante.pers_cUbigeoDefuncion = civil.reci_cOcurrenciaUbigeo;
                    participante.sGeneroId = Convert.ToInt32(this.ddl_Genero.SelectedValue);

                    txtNombresTitular.Text = participante.vNombres;
                    txtApePatTitular.Text = participante.vPrimerApellido;
                    txtApeMatTitular.Text = participante.vSegundoApellido;
                }
            }
            #endregion

            Session["Participante"] = (List<BE.RE_PARTICIPANTE>)loParticipanteContainer;

            return loParticipanteContainer;
        }


        private List<BE.RE_PARTICIPANTE> ParticipantesAgregarRecurrente(BE.MRE.RE_REGISTROCIVIL civil)
        {
            List<BE.RE_PARTICIPANTE> loParticipanteContainer = (List<BE.RE_PARTICIPANTE>)Session["Participante"];
            Int32 ContarRecurrente = 0;
            BE.RE_PARTICIPANTE oRE_PARTICIPANTE = new RE_PARTICIPANTE();
            BE.MRE.RE_PERSONA oRE_PERSONA = new BE.MRE.RE_PERSONA();
            PersonaConsultaBL oPersonaConsultaBL = new PersonaConsultaBL();
            oRE_PERSONA = oPersonaConsultaBL.USP_RE_PERSONA_OBTENERXIDPERSONA(Convert.ToInt64(HF_iPersonaID.Value));


            if ((int)Enumerador.enmTipoActa.NACIMIENTO == Convert.ToInt32(ddlTipoActa.SelectedValue))
            {
                oRE_PARTICIPANTE.sTipoParticipanteId = (int)Enumerador.enmParticipanteNacimiento.RECURRENTE;
                oRE_PARTICIPANTE.vTipoParticipante = Enumerador.enmParticipanteNacimiento.RECURRENTE.ToString();

            }
            else if ((int)Enumerador.enmTipoActa.MATRIMONIO == Convert.ToInt32(ddlTipoActa.SelectedValue))
            {
                oRE_PARTICIPANTE.sTipoParticipanteId = (int)Enumerador.enmParticipanteMatrimonio.RECURRENTE;
                oRE_PARTICIPANTE.vTipoParticipante = Enumerador.enmParticipanteMatrimonio.RECURRENTE.ToString();
            }
            else if ((int)Enumerador.enmTipoActa.DEFUNCION == Convert.ToInt32(ddlTipoActa.SelectedValue))
            {
                oRE_PARTICIPANTE.sTipoParticipanteId = (int)Enumerador.enmParticipanteDefuncion.RECURRENTE;
                oRE_PARTICIPANTE.vTipoParticipante = Enumerador.enmParticipanteDefuncion.RECURRENTE.ToString();
            }


            oRE_PARTICIPANTE.iPersonaId = oRE_PERSONA.pers_iPersonaId;
            oRE_PARTICIPANTE.sTipoDocumentoId = oRE_PERSONA.Identificacion.peid_sDocumentoTipoId;

            oRE_PARTICIPANTE.vNumeroDocumento = oRE_PERSONA.Identificacion.peid_vDocumentoNumero;
            oRE_PARTICIPANTE.sNacionalidadId = oRE_PERSONA.pers_sNacionalidadId;
            oRE_PARTICIPANTE.vNombres = oRE_PERSONA.pers_vNombres;
            oRE_PARTICIPANTE.vPrimerApellido = oRE_PERSONA.pers_vApellidoMaterno;
            oRE_PARTICIPANTE.vSegundoApellido = oRE_PERSONA.pers_vApellidoPaterno;

            oRE_PARTICIPANTE.sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            oRE_PARTICIPANTE.sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            oRE_PARTICIPANTE.sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            foreach (GridViewRow row in Grd_Participantes.Rows)
            {
                String iParticipanteID = row.Cells[0].Text.ToString();

                if ((iParticipanteID == HF_PARTICIPANTE_RECURRENTE_DEFUNCION.Value) ||
                    (iParticipanteID == HF_PARTICIPANTE_RECURRENTE_MATRIMONIO.Value) ||
                    (iParticipanteID == HF_PARTICIPANTE_RECURRENTE_NACIMIENTO.Value)
                    )
                {
                    ContarRecurrente += 1;
                }
            }

            if (ContarRecurrente == 0)
            {
                loParticipanteContainer.Add(oRE_PARTICIPANTE);
            }

            return loParticipanteContainer;
        }
        private bool ValidarRegistroFormato()
        {
            bool resultado = true;
            if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.NACIMIENTO)
            {
                #region Nacimiento

                if (txtLibroRegCiv.Text.Length == 0)
                {
                    txtLibroRegCiv.Focus();
                    txtLibroRegCiv.Style.Add("border", "solid Red 1px");
                    resultado = false;
                }
                else
                {
                    txtLibroRegCiv.Style.Add("border", "solid #888888 1px");
                }

                if (txtNroActa.Text.Length == 0)
                {
                    txtNroActa.Focus();
                    txtNroActa.Style.Add("border", "solid Red 1px");
                    resultado = false;
                }
                else
                {
                    txtNroActa.Style.Add("border", "solid #888888 1px");
                }
                if (chkconCUI.Checked)
                {
                    if (txtNroCUI.Text.Length == 0)
                    {
                        txtNroCUI.Focus();
                        txtNroCUI.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtNroCUI.Style.Add("border", "solid #888888 1px");
                    }

                    if (txtNombresTitular.Text.Length == 0)
                    {
                        txtNombresTitular.Focus();
                        txtNombresTitular.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtNombresTitular.Style.Add("border", "solid #888888 1px");
                    }

                    if (txtApePatTitular.Text.Length == 0)
                    {
                        txtApePatTitular.Focus();
                        txtApePatTitular.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtApePatTitular.Style.Add("border", "solid #888888 1px");
                    }

                    if (txtApeMatTitular.Text.Length == 0)
                    {
                        txtApeMatTitular.Focus();
                        txtApeMatTitular.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtApeMatTitular.Style.Add("border", "solid #888888 1px");
                    }

                    if (ddl_Genero.SelectedIndex == 0)
                    {
                        ddl_Genero.Focus();
                        ddl_Genero.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        ddl_Genero.Style.Add("border", "solid #888888 1px");
                    }
                }

                

                if (ddl_TipoOcurrencia.SelectedIndex == 0)
                {
                    ddl_TipoOcurrencia.Focus();
                    ddl_TipoOcurrencia.Style.Add("border", "solid Red 1px");
                    resultado = false;
                }
                else
                {
                    ddl_TipoOcurrencia.Style.Add("border", "solid #888888 1px");
                }
                if (txtLugarOcurrencia.Text.Length == 0)
                {
                    txtLugarOcurrencia.Focus();
                    txtLugarOcurrencia.Style.Add("border", "solid Red 1px");
                    resultado = false;
                }
                else
                {
                    txtLugarOcurrencia.Style.Add("border", "solid #888888 1px");
                }
                if (ddl_DeptOcurrencia.SelectedIndex == 0)
                {
                    ddl_DeptOcurrencia.Focus();
                    ddl_DeptOcurrencia.Style.Add("border", "solid Red 1px");
                    resultado = false;
                }
                else
                {
                    ddl_DeptOcurrencia.Style.Add("border", "solid #888888 1px");
                }
                if (ddl_ProvOcurrencia.SelectedIndex == 0)
                {
                    ddl_ProvOcurrencia.Focus();
                    ddl_ProvOcurrencia.Style.Add("border", "solid Red 1px");
                    resultado = false;
                }
                else
                {
                    ddl_ProvOcurrencia.Style.Add("border", "solid #888888 1px");
                }
                if (ddl_DistOcurrencia.SelectedIndex == 0)
                {
                    ddl_DistOcurrencia.Focus();
                    ddl_DistOcurrencia.Style.Add("border", "solid Red 1px");
                    resultado = false;
                }
                else
                {
                    ddl_DistOcurrencia.Style.Add("border", "solid #888888 1px");
                }
                #endregion
            }
            else {

                if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.DEFUNCION)
                {
                    #region Defunción

                    if (txtLibroRegCiv.Text.Length == 0)
                    {
                        txtLibroRegCiv.Focus();
                        txtLibroRegCiv.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtLibroRegCiv.Style.Add("border", "solid #888888 1px");
                    }
                    if (txtNroActa.Text.Length == 0)
                    {
                        txtNroActa.Focus();
                        txtNroActa.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtNroActa.Style.Add("border", "solid #888888 1px");
                    }
                    if (txtFecNac.Text.Length == 0)
                    {
                        txtFecNac.Focus();
                        //txtFecNac.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        //txtFecNac.Style.Add("border", "solid #888888 1px");
                    }
                    if (txtHora.Text.Length == 0)
                    {
                        txtHora.Focus();
                        txtHora.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtHora.Style.Add("border", "solid #888888 1px");
                    }
                    if (txtLugarOcurrencia.Text.Length == 0)
                    {
                        txtLugarOcurrencia.Focus();
                        txtLugarOcurrencia.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtLugarOcurrencia.Style.Add("border", "solid #888888 1px");
                    }
                    if (ddl_TipoOcurrencia.SelectedIndex == 0)
                    {
                        ddl_TipoOcurrencia.Focus();
                        ddl_TipoOcurrencia.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        ddl_TipoOcurrencia.Style.Add("border", "solid #888888 1px");
                    }
                    if (ddl_DeptOcurrencia.SelectedIndex == 0)
                    {
                        ddl_DeptOcurrencia.Focus();
                        ddl_DeptOcurrencia.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        ddl_DeptOcurrencia.Style.Add("border", "solid #888888 1px");
                    }
                    if (ddl_ProvOcurrencia.SelectedIndex == 0)
                    {
                        ddl_ProvOcurrencia.Focus();
                        ddl_ProvOcurrencia.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        ddl_ProvOcurrencia.Style.Add("border", "solid #888888 1px");
                    }
                    if (ddl_DistOcurrencia.SelectedIndex == 0)
                    {
                        ddl_DistOcurrencia.Focus();
                        ddl_DistOcurrencia.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        ddl_DistOcurrencia.Style.Add("border", "solid #888888 1px");
                    }

                    #endregion
                }
                else {
                    if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.MATRIMONIO)
                    {
                        #region Matrimonio

                        if (txtLibroRegCiv.Text.Length == 0)
                        {
                            txtLibroRegCiv.Focus();
                            txtLibroRegCiv.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtLibroRegCiv.Style.Add("border", "solid #888888 1px");
                        }
                        if (txtNroActa.Text.Length == 0)
                        {
                            txtNroActa.Focus();
                            txtNroActa.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtNroActa.Style.Add("border", "solid #888888 1px");
                        }
                        //======04/11/2020; Autor:Pipa
                        //======se comenta este segmento para por lo que esto dos campos no es obligatorio
                        /*if (txtNroExpediente.Text.Length == 0)
                        {
                            txtNroExpediente.Focus();
                            txtNroExpediente.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtNroExpediente.Style.Add("border", "solid #888888 1px");
                        }
                        if (txtCargoCelebrante.Text.Length == 0)
                        {
                            txtCargoCelebrante.Focus();
                            txtCargoCelebrante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtCargoCelebrante.Style.Add("border", "solid #888888 1px");
                        }*/
                        if (ddl_DeptOcurrencia.SelectedIndex == 0)
                        {
                            ddl_DeptOcurrencia.Focus();
                            ddl_DeptOcurrencia.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            ddl_DeptOcurrencia.Style.Add("border", "solid #888888 1px");
                        }
                        if (ddl_ProvOcurrencia.SelectedIndex == 0)
                        {
                            ddl_ProvOcurrencia.Focus();
                            ddl_ProvOcurrencia.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            ddl_ProvOcurrencia.Style.Add("border", "solid #888888 1px");
                        }
                        if (ddl_DistOcurrencia.SelectedIndex == 0)
                        {
                            ddl_DistOcurrencia.Focus();
                            ddl_DistOcurrencia.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            ddl_DistOcurrencia.Style.Add("border", "solid #888888 1px");
                        }
                        #endregion
                    }
                }
            }

            return resultado;
        }
        private void ctrlToolbarFormato_btnGrabarHandler()
        {
            String StrScript = String.Empty; //Variables para los Mensaje 
            String DniInvalidos = String.Empty; //Variable para Obtener por WEBCONFIG los DNI Invalidos
            
            String MensajeParticipantes = String.Empty; //Mensaje de Error Para los Participante Obligatorios
            String iCodTipoParticipante = String.Empty; //Codigo del Participante
            String ooParticipante = String.Empty; //Descripcion del Participante
            Session["InicioTramite"] = "2";
            if (txtFecNac.Value() == DateTime.MinValue)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTUACIÓN", "Falta ingresar la fecha"));
                return;
            }

            if (!ValidarRegistroFormato())
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTUACIÓN", "Ingrese todo los campos obligatorios"));
                return;
            }

            long lngActuacionDetalleId = 0;

            lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
    

            if (lngActuacionDetalleId == 0)
            {
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO CIVIL", "POR FAVOR REGISTRE NUEVAMENTE EL TRÁMITE", false, 190, 350);
                                Comun.EjecutarScript(Page, StrScript);
                                return;
            }


            string strScript = string.Empty;
           
            BE.MRE.RE_REGISTROCIVIL ObjRegCivBE = new BE.MRE.RE_REGISTROCIVIL();
            ActoCivilConsultaBL funActoCivil = new ActoCivilConsultaBL();

            if (Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != 0)
            {
                #region OBJETO REGISTRO CIVIL ...

                //ObjRegCivBE.reci_iRegistroCivilId = Convert.ToInt64(hifRegistroCivil.Value);
                //---------------------------------------------------------------------------------------------------
                // Autor: Miguel Márquez Beltrán
                //Fecha:29/09/2016
                //Objetivo: Asignar un valor valido al atributo: ObjRegCivBE.reci_iRegistroCivilId para el 
                //          registro civil del titular.
                //---------------------------------------------------------------------------------------------------
                if (Convert.ToInt64(hifRegistroCivil.Value) > 0)
                {
                    ObjRegCivBE.reci_iRegistroCivilId = Convert.ToInt64(hifRegistroCivil.Value);
                }
                else
                {
                    hifRegistroCivil.Value = Session[strRegistroCivilId].ToString();
                    ObjRegCivBE.reci_iRegistroCivilId = Convert.ToInt64(Session[strRegistroCivilId].ToString());
                }
                //---------------------------------------------------------------------------------------------------
                                
                ObjRegCivBE.reci_iActuacionDetalleId = lngActuacionDetalleId;
                ObjRegCivBE.reci_sTipoActaId = Convert.ToInt16(this.ddlTipoActa.SelectedValue);
                ObjRegCivBE.reci_vNumeroActa = Convert.ToString(this.txtNroActa.Text);
                ObjRegCivBE.reci_dFechaRegistro = Comun.FormatearFecha(this.txtFechaRegistro.Text);
                ObjRegCivBE.reci_vLibro = this.txtLibroRegCiv.Text.Trim().ToUpper();
                ObjRegCivBE.reci_vNumeroCUI = ((ddlTipoActa.SelectedValue == Convert.ToString((int)Enumerador.enmTipoActa.NACIMIENTO)) ? this.txtNroCUI.Text : string.Empty);

                #region SOLO PARA ACTO NACIMIENTO
                if ((ddlTipoActa.SelectedValue == Convert.ToString((int)Enumerador.enmTipoActa.NACIMIENTO)) || (ddlTipoActa.SelectedValue == Convert.ToString((int)Enumerador.enmTipoActa.DEFUNCION)))
                {
                    ObjRegCivBE.reci_sOcurrenciaTipoId = Convert.ToInt16(this.ddl_TipoOcurrencia.SelectedValue);
                    ObjRegCivBE.reci_vOcurrenciaLugar = this.txtLugarOcurrencia.Text.ToUpper();
                    ObjRegCivBE.reci_dFechaHoraOcurrenciaActo = FechaHora(this.txtFecNac.Text, this.txtHora.Text);                    
                }
                #endregion

                ObjRegCivBE.reci_cOcurrenciaUbigeo = this.ddl_DeptOcurrencia.SelectedValue + this.ddl_ProvOcurrencia.SelectedValue + this.ddl_DistOcurrencia.SelectedValue;
                ObjRegCivBE.reci_IOcurrenciaCentroPobladoId = null;
                //ObjRegCivBE.reci_cOficinaRegistralUbigeo = this.ddl_ContDepOffiReg.SelectedValue + this.ddl_PaisProvOffiReg.SelectedValue + this.ddl_CiuDistOffiReg.SelectedValue;
                ObjRegCivBE.reci_cOficinaRegistralUbigeo = "000000";
                ObjRegCivBE.reci_IOficinaRegistralCentroPobladoId = null;

                #region SOLO PARA ACTO MATRIMINIO
                if (this.ddlTipoActa.SelectedValue == Convert.ToString((int)Enumerador.enmTipoActa.MATRIMONIO))
                {
                    ObjRegCivBE.reci_vNumeroExpedienteMatrimonio = this.txtNroExpediente.Text.ToUpper();
                    ObjRegCivBE.reci_vCargoCelebrante = this.txtCargoCelebrante.Text.ToUpper();
                    ObjRegCivBE.reci_dFechaHoraOcurrenciaActo = this.txtFecNac.Value();
                }
                #endregion

                
                ObjRegCivBE.reci_IAprobacionUsuarioId = 0;
                ObjRegCivBE.reci_vIPAprobacion = string.Empty;
                ObjRegCivBE.reci_dFechaAprobacion = Comun.FormatearFecha(null);
                ObjRegCivBE.reci_bDigitalizadoFlag = false;
                ObjRegCivBE.reci_bAnotacionFlag = false;
                
                ObjRegCivBE.reci_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                ObjRegCivBE.reci_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                ObjRegCivBE.reci_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                ObjRegCivBE.reci_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                ObjRegCivBE.reci_vObservaciones = txtCivilObservaciones.Text.Trim().ToUpper();
                ObjRegCivBE.reci_cConCUI = "N";
                
                //------------------------------------------------------------
                //Fecha: 01/09/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Asignar valor al atributo reci_bInscripcionOficio
                //------------------------------------------------------------
                ObjRegCivBE.reci_bInscripcionOficio = false;

                if (this.ddlTipoActa.SelectedValue == Convert.ToString((int)Enumerador.enmTipoActa.DEFUNCION))
                {
                    if (tablaInscripcionOficio.Visible == true)
                    {
                        ObjRegCivBE.reci_bInscripcionOficio = chkInscripcionOficio.Checked;
                    }
                }
                //------------------------------------------------------------

                if (chkconCUI.Visible == true)
                {
                    if (chkconCUI.Checked == true)
                    {
                        ObjRegCivBE.reci_cConCUI = "S";
                    }
                }
                if (chksinCUI.Visible == true)
                {
                    if (chksinCUI.Checked == true)
                    {
                        ObjRegCivBE.reci_cConCUI = "N";
                    }
                }
                if (tablaReconocimientoAdopcion.Visible == true)
                {
                    if (chkReconocimientoAdopcion.Checked)
                    {
                        ObjRegCivBE.reci_cReconocimientoAdopcion = "S";
                    }
                    else
                    {
                        ObjRegCivBE.reci_cReconocimientoAdopcion = "N";
                    }
                }
                else
                {
                    ObjRegCivBE.reci_cReconocimientoAdopcion = "N";
                }

                if (tablaReconstitucionReposicion.Visible == true)
                {
                    if (chkReconstitucionReposicion.Checked)
                    {
                        ObjRegCivBE.reci_cReconstitucionReposicion = "S";
                    }
                    else
                    {
                        ObjRegCivBE.reci_cReconstitucionReposicion = "N";
                    }
                }
                else
                {
                    ObjRegCivBE.reci_cReconstitucionReposicion = "N";
                }

                if (tablaReconocimientoAdopcion.Visible == true || tablaReconstitucionReposicion.Visible == true)
                {
                    if (chkReconocimientoAdopcion.Checked || chkReconstitucionReposicion.Checked)
                    {
                        if (txtNumeroActaAnterior.Text.Trim().Length == 0)
                        {
                            ObjRegCivBE.reci_iNumeroActaAnterior = null;
                        }
                        else
                        {
                            ObjRegCivBE.reci_iNumeroActaAnterior = Convert.ToInt32(txtNumeroActaAnterior.Text.Trim());
                        }
                        ObjRegCivBE.reci_vTitular = txtTitularActa.Text.Trim().ToUpper();
                    }
                    else
                    {
                        ObjRegCivBE.reci_iNumeroActaAnterior = null;
                        ObjRegCivBE.reci_vTitular = "";
                    }
                }
                else
                {
                    ObjRegCivBE.reci_iNumeroActaAnterior = null;
                    ObjRegCivBE.reci_vTitular = "";
                }
                //--------------------------------------------------
                #endregion Registro Civil

                //Proceso MiProc = new Proceso();
                ActoCivilMantenimientoBL BL = new ActoCivilMantenimientoBL();
                int intResultado = 0;

                #region ACTUALIZA ACTO CIVIL
                if ((ObjRegCivBE.reci_iActuacionDetalleId != 0) && (ObjRegCivBE.reci_iRegistroCivilId > 0))           
                    {
                    intResultado = BL.Actualizar(ObjRegCivBE,
                                                 Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                                                 Participantes(ObjRegCivBE), null);


                    if (intResultado > 0)
                    {
                        if (chkconCUI.Checked)
                        {
                            if (Convert.ToInt32(Session["TIPO_ACTO_PARTICIPANTE"]) == (int)Enumerador.enmTipoActa.NACIMIENTO)
                            {
                                string valorTitular = Grd_Participantes.Rows[0].Cells[9].Text;
                                ActualizarTitular(Convert.ToInt64(valorTitular));
                            }
                        }
                        CargarDatosRegistroCivil();
                    }
                }
                #endregion

                #region INSERTA ACTO CIVIL
                if (((Convert.ToInt32(Session["ActoCivil_Accion"]).Equals((int)Enumerador.enmTipoOperacion.REGISTRO)) && (ObjRegCivBE.reci_iActuacionDetalleId != 0))
                    || ((Convert.ToInt32(Session["ActoCivil_Accion"]).Equals((int)Enumerador.enmTipoOperacion.ACTUALIZACION)) && ObjRegCivBE.reci_iActuacionDetalleId != 0 && ObjRegCivBE.reci_iRegistroCivilId < 0)                    
                    || (txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_ID_1.ToString() && Convert.ToInt32(Session["ActoCivil_Accion"]).Equals((int)Enumerador.enmTipoOperacion.CONSULTA)))
                {
                    #region validación CUI
                    if (Convert.ToInt32(Session["TIPO_ACTO_PARTICIPANTE"]) == (int)Enumerador.enmTipoActa.NACIMIENTO)
                    {
                        if (txtNroCUI.Text.Trim().Length != 0)
                        {
                            DataTable dtRegCivil = new DataTable();
                            dtRegCivil = funActoCivil.ObtenerPorCUI(txtNroCUI.Text);
                            if (dtRegCivil.Rows.Count != 0)
                            {
                                txtNroCUI.Enabled = true;
                                imgBuscarCUI.Enabled = true;
                                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "REGISTRO CIVIL", "El número de CUI ingresado ya existe, ingrese otro");
                                Comun.EjecutarScript(Page, strScript);
                                return;
                            }
                        }
                    }
                    #endregion

                    long LonRegistroCivilId = 0;

                    intResultado = BL.Insertar(ObjRegCivBE, Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), Participantes(ObjRegCivBE), ref LonRegistroCivilId);
                    if (intResultado > 0)
                    {
                        hifRegistroCivil.Value = LonRegistroCivilId.ToString();
                        Session[strRegistroCivilId] = LonRegistroCivilId.ToString();
                        Session["ActoCivil_Accion"] = Enumerador.enmTipoOperacion.ACTUALIZACION;
                        if (chkconCUI.Checked)
                        {
                            RegistrarTitular();
                        }
                        CargarDatosRegistroCivil();
                    }
                }
                #endregion

                if (intResultado > 0)
                {
                    
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "REGISTRO CIVIL", Constantes.CONST_MENSAJE_EXITO);

                    //----------------------------------------------------------
                    //Fecha: 18/06/2019
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Activar el botón Vista Previa de 
                    //acuerdo a la validación del número de participantes.
                    //----------------------------------------------------------
                    BtnVistaPrevia.Enabled = ValidarParticipantesRegistroCivil();
                    cbxAfirmarTexto.Enabled = BtnVistaPrevia.Enabled;
                    btnActa.Enabled = BtnVistaPrevia.Enabled;
                    btnAgregarParticipante.Enabled = true;
                    
                    updVinculacion.Update();
                    //----------------------------------------------------------
                }
                else
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO CIVIL", Constantes.CONST_MENSAJE_OPERACION_FALLIDA);
                }
                Comun.EjecutarScript(Page, strScript);
            }


            
        }

        private void ctrlToolbarFormato_btnEliminarHandler()
        {
            string strScript = string.Empty;
            try
            {
                ActoCivilMantenimientoBL _obj = new ActoCivilMantenimientoBL();
                BE.MRE.RE_REGISTROCIVIL ObjRegCivBE = new BE.MRE.RE_REGISTROCIVIL();

                if (Convert.ToInt64(hifRegistroCivil.Value) > 0)
                {
                    ObjRegCivBE.reci_iRegistroCivilId = Convert.ToInt64(hifRegistroCivil.Value);
                }
                else
                {
                    hifRegistroCivil.Value = Session[strRegistroCivilId].ToString();
                    ObjRegCivBE.reci_iRegistroCivilId = Convert.ToInt64(Session[strRegistroCivilId].ToString());
                }
                ObjRegCivBE.reci_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                ObjRegCivBE.reci_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                _obj.Eliminar(ObjRegCivBE,Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));

                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "REGISTRO CIVIL", Constantes.CONST_MENSAJE_ELIMINADO);
                ddlTipoActa.SelectedValue = Convert.ToString((int)Enumerador.enmTipoActa.NACIMIENTO);
                Session["InicioTramite"] = "1";
                hInicioTramite.Value = "1";
                Session.Remove("Participante");
                hifRegistroCivil.Value = "0";
                Session[strRegistroCivilId] = 0;
                btnAgregarParticipante.Enabled = false;
                chkconCUI.Enabled = true;
                chksinCUI.Enabled = true;
                Session["ActoCivil_Accion"] = Enumerador.enmTipoOperacion.REGISTRO;
            }
            catch {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO CIVIL", Constantes.CONST_MENSAJE_OPERACION_FALLIDA);
            }
            Comun.EjecutarScript(Page, strScript);

            ddlTipoActa_SelectedIndexChanged(null, null);
            ddlTipoActa.Enabled = true;
            chkconCUI.Checked = true;
            chkconCUI_CheckedChanged(null, null);
        }
        private void ctrlToolbarFormato_btnCancelarHandler()
        {
            //if (HFGUID.Value.Length > 0)
            //{
            //    Session["iPersonaId" + HFGUID.Value] = Session["iCodPersonaId" + HFGUID.Value];
            //}
            //else
            //{
                //Session["iPersonaId"] = Session["iCodPersonaId"];
            //}
            Session.Remove("Participante");

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

        #region CombosLugarOcurrencia

        protected void ddl_DeptOcurrencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_DeptOcurrencia.SelectedIndex > 0)
            {
                ddl_ProvOcurrencia.Enabled = true;

                if (ViewState["Ubigeo"] != null)
                {
                    beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                    obeUbigeoListas = (beUbigeoListas)ViewState["Ubigeo"];
                    List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
                    lbeUbicaciongeografica = Comun.obtenerListaUbiGeo("02", ddl_DeptOcurrencia.SelectedValue, "", obeUbigeoListas.Ubigeo02);
                    lbeUbicaciongeografica.Insert(0, new beUbicaciongeografica { Ubi02 = "0", Provincia = "-- SELECCIONE --" });
                    ddl_ProvOcurrencia.DataSource = lbeUbicaciongeografica;
                    ddl_ProvOcurrencia.DataValueField = "Ubi02";
                    ddl_ProvOcurrencia.DataTextField = "Provincia";
                    ddl_ProvOcurrencia.DataBind();
                }
                ddl_ProvOcurrencia.Focus();
                if (ddl_ProvOcurrencia.Enabled == true)
                {
                    ddl_DeptOcurrencia.Focus();
                }
            }
            else
            {
                this.ddl_ProvOcurrencia.Items.Clear();
                this.ddl_ProvOcurrencia.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- SELECCIONAR -", "0"));

            }
            this.ddl_DistOcurrencia.Items.Clear();
            this.ddl_DistOcurrencia.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- SELECCIONAR -", "0"));
            
            if (HF_ESRune.Value == "0" || HF_ESRune.Value == String.Empty)
            {
                HabilitarControlParticipanteRune(true);
            }
            else
            {
             //   ctrlUbigeo1.HabilitaControl(false);
            }
            if (rbSi.Checked)
            {
                if (HF_ESRune.Value == "0" || HF_ESRune.Value == String.Empty)
                {
                    HabilitarControlParticipanteRune(true);
                }
                else
                {
                    //ddl_TipoDocParticipante.Enabled = true;
                    txtNroDocParticipante.Enabled = true;
                }
            }
            else
            {
                ddl_TipoDocParticipante.Enabled = false;
                txtNroDocParticipante.Enabled = false;
            }

            if (ddl_TipoParticipante.SelectedIndex > 0)
            {
            if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteDefuncion.DECLARANTE)
                            || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteDefuncion.REGISTRADOR_CIVIL)
                            || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteMatrimonio.CELEBRANTE)
                            || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteMatrimonio.REGISTRADOR_CIVIL)
                            || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteNacimiento.REGISTRADOR_CIVIL)
                            || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteNacimiento.DECLARANTE_1)
                            || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteNacimiento.DECLARANTE_2)
                            )
            {
               // txtDireccionParticipante.Enabled = false;
               // ctrlUbigeo1.HabilitaControl(false);
            }
            }
            
            
            ddl_DeptOcurrencia.Focus();
            updFormato.Update();

            //Llenar_titular_ubicacion_nacimiento();
        }



        protected void ddl_ProvOcurrencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_ProvOcurrencia.SelectedIndex > 0)
            {
        
                ddl_DistOcurrencia.Enabled = true;

                if (ViewState["Ubigeo"] != null)
                {
                    beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                    obeUbigeoListas = (beUbigeoListas)ViewState["Ubigeo"];
                    List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
                    lbeUbicaciongeografica = Comun.obtenerListaUbiGeo("03", ddl_DeptOcurrencia.SelectedValue, ddl_ProvOcurrencia.SelectedValue, obeUbigeoListas.Ubigeo03);
                    lbeUbicaciongeografica.Insert(0, new beUbicaciongeografica { Ubi03 = "00", Distrito = "-- SELECCIONE --" });
                    ddl_DistOcurrencia.DataSource = lbeUbicaciongeografica;
                    ddl_DistOcurrencia.DataValueField = "Ubi03";
                    ddl_DistOcurrencia.DataTextField = "Distrito";
                    ddl_DistOcurrencia.DataBind();
                    ddl_DistOcurrencia.Enabled = (ddl_ProvOcurrencia.SelectedValue.Equals("00") ? false : true);
                    ddl_DistOcurrencia.Focus();
                }

                ddl_ProvOcurrencia.Focus();

                if (ddl_DistOcurrencia.Enabled == true)
                {
                    ddl_ProvOcurrencia.Focus();
                }
            }
            else
            {
                //----------------------------------------------------
                //Fecha: 03/04/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Limpiar el control ddl_DistOcurrencia
                //----------------------------------------------------
                Comun.limpiaCombo("--SELECCIONAR--", ddl_DistOcurrencia);
                //----------------------------------------------------
                //ddl_DistOcurrencia.DataSource = new DataTable();
                //ddl_DistOcurrencia.DataBind();
                //ddl_DistOcurrencia.SelectedIndex = -1;
            }
            
            if (HF_ESRune.Value == "0" || HF_ESRune.Value == String.Empty)
            {
                HabilitarControlParticipanteRune(true);
            }
            else {
                // ctrlUbigeo1.HabilitaControl(false); 
            }
            if (rbSi.Checked)
            {
                if (HF_ESRune.Value == "0" || HF_ESRune.Value == String.Empty)
                {
                    HabilitarControlParticipanteRune(true);
                }
                else
                {
                    //ddl_TipoDocParticipante.Enabled = true;
                    txtNroDocParticipante.Enabled = true;
                }
            }
            else
            {
                ddl_TipoDocParticipante.Enabled = false;
                txtNroDocParticipante.Enabled = false;
            }


            if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteDefuncion.DECLARANTE)
                            || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteDefuncion.REGISTRADOR_CIVIL)
                            || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteMatrimonio.CELEBRANTE)
                            || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteMatrimonio.REGISTRADOR_CIVIL)
                            || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteNacimiento.REGISTRADOR_CIVIL)
                            || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteNacimiento.DECLARANTE_1)
                            || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteNacimiento.DECLARANTE_2)
                            )
            {
                //txtDireccionParticipante.Enabled = false;
                //ctrlUbigeo1.HabilitaControl(false);
            }


            ddl_ProvOcurrencia.Focus();
            updFormato.Update();
        }

        #endregion CombosLugarOcurrencia

        #region CombosOficinaRegistral



        #endregion CombosOficinaRegistral

        #endregion Formato

        #region ANOTACIONES  Por Favor Leer Descripcion!!!!!!!

        private void CargarGrillaAnotaciones(long LonActuacionDetalleId)
        {
            Grd_Anotaciones.DataSource = null;
            Grd_Anotaciones.DataBind();

            Proceso MiProc = new Proceso();
            DataTable dtAnotaciones = new DataTable();

            int IntTotalCount = 0;
            int IntTotalPages = 0;

            int intPaginaCantidad = Constantes.CONST_PAGE_SIZE_ADJUNTOS;
            int PaginaActual = CtrlPageBarActAnotacion.PaginaActual;

            object[] arrParametros = {   LonActuacionDetalleId,
                                         PaginaActual,
                                         intPaginaCantidad,
                                         IntTotalCount,IntTotalPages };

            dtAnotaciones = (DataTable)MiProc.Invocar(ref arrParametros, "SGAC.BE.RE_ANOTACION", Enumerador.enmAccion.CONSULTAR);

            if (dtAnotaciones.Rows.Count > 0)
            {
                Grd_Anotaciones.DataSource = dtAnotaciones;
                Grd_Anotaciones.DataBind();

                CtrlPageBarActAnotacion.TotalResgistros = Convert.ToInt32(arrParametros[3]);  //IntTotalCount;
                CtrlPageBarActAnotacion.TotalPaginas = Convert.ToInt32(arrParametros[3]);  //IntTotalPages;

                CtrlPageBarActAnotacion.Visible = false;
                if (CtrlPageBarActAnotacion.TotalResgistros > intPaginaCantidad)
                {
                    CtrlPageBarActAnotacion.Visible = true;
                }
                dtAnotaciones = null;
            }
        }

        protected void BtnGrabAnotacion_Click(object sender, EventArgs e)
        {
            string StrScript = string.Empty;
            Int32 iCaracteresMaximo = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["iCaracteresMaximo"].ToString());
            long lngActuacionDetalleId = 0;

            if (HFGUID.Value.Length > 0)
            {
                lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);
            }
            else
            {
                lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
            }
            
            if (lngActuacionDetalleId == 0)
            {
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuación", "Debe crear la actuación antes de guardar Anotación.");
                Comun.EjecutarScript(Page, StrScript);
                return;
            }

            if (ddlTipoActaAnotacion.SelectedIndex == 0)
            {
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuación", "Seleccione el Tipo de Acta.");
                Comun.EjecutarScript(Page, StrScript);
                return;
            }

            if (txtTitular.Text.Trim().Length == 0)
            {
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuación", "Seleccione o digite el nombre completo del titular.");
                Comun.EjecutarScript(Page, StrScript);
                return;
            }

            if (cmb_TipoAnotacion.SelectedIndex == 0)
            {
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuación", "Seleccione el Tipo de Anotación.");
                Comun.EjecutarScript(Page, StrScript);
                return;
            }

            String strTexSinHtml = String.Empty;
            strTexSinHtml = txtDescAnotacion.Text;
            strTexSinHtml = Regex.Replace(strTexSinHtml, "<.*?>", String.Empty);
            if (strTexSinHtml.Length >= iCaracteresMaximo)
            {
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuación", "Ha Superado el Limite de Caracteres Permitidos.");
                Comun.EjecutarScript(Page, StrScript);
                return;
            }
            

            RE_ACTUACIONANOTACION ObjAnotBE = new RE_ACTUACIONANOTACION();
            ActuacionAnotacionMantenimientoBL bl = new ActuacionAnotacionMantenimientoBL();
            Proceso MiProc = new Proceso();

            int IntRpta = 0;

            if (Convert.ToBoolean(Session["iOperAnot"]))
            {
                if (txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_ID_2.ToString())
                {
                    if (Grd_Anotaciones.Rows.Count > 0)
                    {
                        txtNumeroActa.Text = "";
                        txtTitular.Text = "";
                        ddlTipoActaAnotacion.SelectedIndex = 0;
                        cmb_TipoAnotacion.SelectedValue = "0";
                        txtDescAnotacion.Text = "";
                        ctrFecRegistro.set_Value = DateTime.Now;
                        ctrFecRegistro.Enabled = true;
                        updAnotaciones.Update();
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuación", "Error. La tarifa 2 sólo permite el registro de una anotación.");
                        Comun.EjecutarScript(Page, StrScript);
                        return;
                    }
                }

                ObjAnotBE.anot_iActuacionDetalleId = lngActuacionDetalleId;
                ObjAnotBE.anot_sTipoAnotacionId = Convert.ToInt16(cmb_TipoAnotacion.SelectedValue);                                
                ObjAnotBE.anot_dFechaRegistro = ctrFecRegistro.Value();
                ObjAnotBE.anot_vComentarios = txtDescAnotacion.Text;

                if (txtDescAnotacion.Text.Trim().ToUpper().Length <= 100)
                    ObjAnotBE.vDescripcionCorta = txtDescAnotacion.Text;
                else
                    ObjAnotBE.vDescripcionCorta = txtDescAnotacion.Text.Trim().ToUpper().Substring(0, 100);

                ObjAnotBE.anot_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                ObjAnotBE.anot_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                if (txtNumeroActa.Text.Trim().Length == 0)
                {
                    ObjAnotBE.anot_iNumeroActaAnterior = null;
                }
                else
                {
                    ObjAnotBE.anot_iNumeroActaAnterior = Convert.ToInt32(txtNumeroActa.Text.Trim());
                }
                ObjAnotBE.anot_vTitular = txtTitular.Text.Trim().ToUpper();

                ObjAnotBE.anot_sTipoActaId = Convert.ToInt16(ddlTipoActaAnotacion.SelectedValue);
                //----------------------------------------                

                IntRpta = bl.Insertar(ObjAnotBE,
                                      Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));


                txtNumeroActa.Text = "";
                txtTitular.Text = "";
                ddlTipoActaAnotacion.SelectedIndex = 0;
                cmb_TipoAnotacion.SelectedValue = "0";
                txtDescAnotacion.Text = "";
                ctrFecRegistro.set_Value = DateTime.Now;
                ctrFecRegistro.Enabled = true;

                if (HFGUID.Value.Length > 0)
                {
                    CargarGrillaAnotaciones(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]));
                }
                else
                {
                    CargarGrillaAnotaciones(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]));
                }

                LimpiarVinculacion();
                if (IntRpta <= 0)
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuación", Constantes.CONST_MENSAJE_OPERACION_FALLIDA, false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }
                else
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Actuación", "Se ha guardo la anotación de la actuación.", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                }
            }
            else
            {

                if (txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_ID_2.ToString())
                {
                    if (Grd_Anotaciones.Rows.Count > 1)
                    {
                        if (Convert.ToBoolean(Session["isVista"]) != true)
                        {
                            cmb_TipoAnotacion.SelectedValue = "0";
                            txtDescAnotacion.Text = "";
                            ctrFecRegistro.set_Value = DateTime.Now;
                            ctrFecRegistro.Enabled = true;
                            updAnotaciones.Update();
                            StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuación", "Error. La tarifa 2 sólo permite el registro de una anotación.");
                            Comun.EjecutarScript(Page, StrScript);
                            return;
                        }

                    }
                    if (Convert.ToBoolean(Session["isVista"]) == true)
                    {
                        cmb_TipoAnotacion.SelectedValue = "0";
                        txtDescAnotacion.Text = "";
                        ctrFecRegistro.set_Value = DateTime.Now;
                        ctrFecRegistro.Enabled = true;
                        updAnotaciones.Update();
                        return;
                    }
                }

                ObjAnotBE.anot_iActuacionAnotacionId = Convert.ToInt32(Session["IActuacionAnotacionId"]);

                if (HFGUID.Value.Length > 0)
                {
                    ObjAnotBE.anot_iActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);
                }
                else
                {
                    ObjAnotBE.anot_iActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
                }

                ObjAnotBE.anot_sTipoAnotacionId = Convert.ToInt16(cmb_TipoAnotacion.SelectedValue);
                ObjAnotBE.anot_vComentarios = txtDescAnotacion.Text;

                if (strTexSinHtml.Length <= 100)
                    ObjAnotBE.vDescripcionCorta = strTexSinHtml;
                else
                    ObjAnotBE.vDescripcionCorta = strTexSinHtml.Trim().ToUpper().Substring(0, 100);

                ObjAnotBE.anot_dFechaRegistro = ctrFecRegistro.Value();
                ObjAnotBE.anot_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                ObjAnotBE.anot_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                if (txtNumeroActa.Text.Trim().Length == 0)
                {
                    ObjAnotBE.anot_iNumeroActaAnterior = 0;
                }
                else
                {
                    ObjAnotBE.anot_iNumeroActaAnterior = Convert.ToInt32(txtNumeroActa.Text.Trim());
                }
                ObjAnotBE.anot_vTitular = txtTitular.Text.Trim().ToUpper();

                if (ddlTipoActaAnotacion.SelectedIndex > 0)
                {
                    ObjAnotBE.anot_sTipoActaId = Convert.ToInt16(ddlTipoActaAnotacion.SelectedValue);
                }
                //--------------------------------------------------
                IntRpta = bl.Actualizar(ObjAnotBE,
                                        Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));

                txtNumeroActa.Text = "";
                txtTitular.Text = "";
                ddlTipoActaAnotacion.SelectedIndex = 0;

                cmb_TipoAnotacion.SelectedValue = "0";
                txtDescAnotacion.Text = "";
                ctrFecRegistro.set_Value = DateTime.Now;
                ctrFecRegistro.Enabled = true;

                if (HFGUID.Value.Length > 0)
                {
                    CargarGrillaAnotaciones(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]));
                }
                else
                {
                    CargarGrillaAnotaciones(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]));
                }

                LimpiarVinculacion();
                if (IntRpta > 0)
                {
                    txtCodAutoadhesivo.Enabled = true;
                    btnLimpiar.Enabled = true;
                    txtCodAutoadhesivo.Focus();
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Actuación", "Se ha modificado la anotación de la actuación.", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                }
                else
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuación", "Error. No se pudo realizar la operación", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);

                    return;
                }


                updAnotaciones.Update();
                updVinculacion.Update();

            }

            //if (HFGUID.Value.Length > 0)
            //{
            //    Response.Redirect("FrmActoCivil.aspx?GUID=" + HFGUID.Value);
            //}
            //else
            //{
                string codPersona = Request.QueryString["CodPer"].ToString();
                if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                {
                    Response.Redirect("FrmActoCivil.aspx?CodPer=" + codPersona + "&Juridica=1", false);
                }
                else
                { // PERSONA NATURAL
                    Response.Redirect("FrmActoCivil.aspx?CodPer=" + codPersona, false);
                }
                
            //}
            Session["iOperAnot"] = true;


        }

        protected void Grd_Anotaciones_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[7].ColumnSpan = 4;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
            }
        }

        protected void Grd_Anotaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ActuacionAnotacionMantenimientoBL bl = new ActuacionAnotacionMantenimientoBL();
            string StrScript = string.Empty;

            int Index = Convert.ToInt32(e.CommandArgument);

            Session["iOperAnot"] = false;


            String scriptMover = String.Empty;
            if (txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_ID_1.ToString())
            {
                scriptMover = @"$(function(){{EnableTabIndex(0);EnableTabIndex(1);EnableTabIndex(2); DisableTabIndex(3);EnableTabIndex(4); MoveTabIndex(3);}});";
            }
            else
            {
                if (txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_ID_2.ToString())
                {
                    scriptMover = @"$(function(){{EnableTabIndex(0);DisableTabIndex(1);EnableTabIndex(2); EnableTabIndex(3);EnableTabIndex(4); MoveTabIndex(3);}});";
                }
                else
                {
                    scriptMover = @"$(function(){{EnableTabIndex(0);DisableTabIndex(1);EnableTabIndex(2); DisableTabIndex(3);EnableTabIndex(4); MoveTabIndex(3);}});";
                }
            }

            if (e.CommandName == "ImprimirAnotacion")
            {
                if (Session["ACTUALIZA"].ToString() != "")
                {
                        Session["ACTUALIZA"] = null;
                        Session["REGISTRO_RPT"] = Enumerador.enmRegistroReporte.ANOTACION;
                        Session["ANOTACION_TIPO"] = Convert.ToString(Page.Server.HtmlDecode(Grd_Anotaciones.Rows[Index].Cells[4].Text));
                        Session["ANOTACION_DESC"] = Convert.ToString(Page.Server.HtmlDecode(Grd_Anotaciones.Rows[Index].Cells[5].Text));
                        HFEsConsulta.Value = "1";

                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);

                        string strUrl = "../Registro/FrmRepAnotacion.aspx";
                        string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=720,height=500,left=100,top=10');";
                        ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), "OpenPopup", strScript, true);
                        ctrFecRegistro.Enabled = true;

                        return;
                }
                else{
                    if (Session["COD_AUTOADHESIVO"].ToString() == txtCodAutoadhesivo.Text)
                    {
                        BindGridActuacionesInsumoDetalle(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]));
                        scriptMover = @"$(function(){{EnableTabIndex(0);DisableTabIndex(1);EnableTabIndex(2); EnableTabIndex(3);EnableTabIndex(4); MoveTabIndex(4);}});";
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);
                    }
                }
            }

            if (e.CommandName == "Consultar")
            {
                Session["IActuacionAnotacionId"] = Convert.ToString(Grd_Anotaciones.Rows[Index].Cells[0].Text);

                ddlTipoActaAnotacion.SelectedValue = Convert.ToString(Grd_Anotaciones.Rows[Index].Cells[11].Text);
                txtNumeroActa.Text = Convert.ToString(Grd_Anotaciones.Rows[Index].Cells[13].Text);
                txtTitular.Text = Convert.ToString(Grd_Anotaciones.Rows[Index].Cells[12].Text);
                cmb_TipoAnotacion.SelectedValue = Convert.ToString(Grd_Anotaciones.Rows[Index].Cells[2].Text);
                txtDescAnotacion.Text = Convert.ToString(Page.Server.HtmlDecode(Grd_Anotaciones.Rows[Index].Cells[5].Text));
                ctrFecRegistro.Text = Comun.FormatearFecha(Convert.ToString(Grd_Anotaciones.Rows[Index].Cells[3].Text).ToString()).ToString("MMM-dd-yyyy");
                HFEsConsulta.Value = "1";
                Comun.EjecutarScript(Page, Util.DeshabilitarTab(1) + Util.ActivarTab(3, "Anotaciones") + Util.MoverTab(3));

                if (HFGUID.Value.Length > 0)
                {
                    CargarGrillaAdjuntos(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]));
                }
                else
                {
                    CargarGrillaAdjuntos(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]));
                }
                
                ctrFecRegistro.Enabled = true;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);
                Session["isVista"] = true;
                return;
            }
            else if (e.CommandName == "Editar")
            {
                Session["isVista"] = false;
                Session["IActuacionAnotacionId"] = Convert.ToString(Grd_Anotaciones.Rows[Index].Cells[0].Text);

                txtNumeroActa.Text = Convert.ToString(Grd_Anotaciones.Rows[Index].Cells[13].Text);
                ddlTipoActaAnotacion.SelectedValue = Convert.ToString(Grd_Anotaciones.Rows[Index].Cells[11].Text);
                txtTitular.Text = Convert.ToString(Grd_Anotaciones.Rows[Index].Cells[12].Text);
                cmb_TipoAnotacion.SelectedValue = Convert.ToString(Grd_Anotaciones.Rows[Index].Cells[2].Text);
                txtDescAnotacion.Text = Convert.ToString(Page.Server.HtmlDecode(Grd_Anotaciones.Rows[Index].Cells[5].Text));
                ctrFecRegistro.Text = Comun.FormatearFecha(Convert.ToString(Grd_Anotaciones.Rows[Index].Cells[3].Text).ToString()).ToString("MMM-dd-yyyy");
                ctrFecRegistro.Enabled = true;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);
                HFEsConsulta.Value = "0";

            }
            else if (e.CommandName == "Eliminar")
            {

                RE_ACTUACIONANOTACION ObjAnotBE = new RE_ACTUACIONANOTACION();
                Proceso MiProc = new Proceso();
                HFEsConsulta.Value = "0";
                int IntRpta = 0;

                ObjAnotBE.anot_iActuacionAnotacionId = Convert.ToInt64(Convert.ToString(Grd_Anotaciones.Rows[Index].Cells[0].Text));
                ObjAnotBE.anot_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                ObjAnotBE.anot_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                IntRpta = bl.Eliminar(ObjAnotBE,
                                      Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);
                if (IntRpta > 0)
                {
                    Session["iOperAnot"] = true;
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "RUNE", "Se ha eliminado la anotación del connacional.", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);

                    if (HFGUID.Value.Length > 0)
                    {
                        CargarGrillaAnotaciones(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]));
                    }
                    else
                    {
                        CargarGrillaAnotaciones(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]));
                    }

                }
                else
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "RUNE", "Error. No se pudo realizar la operación", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }


                cmb_TipoAnotacion.SelectedValue = "0";
                txtDescAnotacion.Text = "";
                ctrFecRegistro.Text = DateTime.Now.ToString("MMM-dd-yyyy");
                ctrFecRegistro.Enabled = true;
                updAnotaciones.Update();
            }
        }

        protected void Grd_Anotaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            ImageButton imgImprimir = e.Row.FindControl("btnPrint") as ImageButton;
            ScriptManager.GetCurrent(this).RegisterPostBackControl(imgImprimir);
        }

        protected void CtrlPageBarActAnotacion_Click(object sender, EventArgs e)
        {
            if (HFGUID.Value.Length > 0)
            {
                CargarGrillaAnotaciones(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]));
            }
            else
            {
                CargarGrillaAnotaciones(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]));
            }
            updAnotaciones.Update();
        }

        protected void ctrlPagAdjuntosAnotaciones_Click(object sender, EventArgs e)
        {
            if (HFGUID.Value.Length > 0)
            {
                CargarGrillaAdjuntos(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]));
            }
            else
            {
                CargarGrillaAdjuntos(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]));
            }
            updAnotaciones.Update();
        }

        private void HabilitaControlesTabAnotacion(bool bHabilitado = true)
        {
            ctrFecRegistro.Enabled = bHabilitado;
            cmb_TipoAnotacion.Enabled = bHabilitado;
            txtDescAnotacion.Enabled = bHabilitado;
            BtnGrabAnotacion.Enabled = bHabilitado;
        }

        #endregion ANOTACIONES  Por Favor Leer Descripcion!!!!!!!

        #region Adjuntos

        protected void MyUserControlUploader1Event_Click(object sender, EventArgs e)
        {
            string StrScript = string.Empty;

            Session["NuevoRegistro"] = false;

            StrScript = @"$(function(){{
                            MoveTabIndex(2);
                        }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoveTabIndex2", StrScript, true);
        }

        #endregion Adjuntos

        #region Vinculación

        // Eventos para el manejo de la impresión del Autuadhesivo
        private void BindGridActuacionesInsumoDetalle(Int64 iActuacionDetalleId)
        {
            Grd_ActInsDet.DataSource = null;
            Grd_ActInsDet.DataBind();

            SGAC.Registro.Actuacion.BL.ActuacionMantenimientoBL objActuacionMantenimientoBL = new SGAC.Registro.Actuacion.BL.ActuacionMantenimientoBL();
            DataTable dtActuacionInsumoDetalle = new DataTable();

            Session["CodAutoadhesivo"] = null;

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_PAGE_SIZE_ADJUNTOS;
            int PaginaActual = CtrlPageBarActuacionInsumoDetalle.PaginaActual;
            dtActuacionInsumoDetalle = objActuacionMantenimientoBL.Obtener_ActuacionInsumoDetalle(iActuacionDetalleId, PaginaActual, intPaginaCantidad, ref IntTotalCount, ref  IntTotalPages); // objActuacionMantenimientoBL.Obtener_ActuacionInsumoDetalle(iActuacionDetalleId, ctrlPaginadorAct.PaginaActual.ToString(), intPaginaCantidad, IntTotalCount, IntTotalPages);

            if (dtActuacionInsumoDetalle.Rows.Count > 0)
            {
                hCodAutoadhesivo.Value = dtActuacionInsumoDetalle.Rows[0]["insu_iInsumoId"].ToString();
            }
            else { hCodAutoadhesivo.Value = ""; }

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
                if (dtActuacionInsumoDetalle.Rows.Count == 1)
                {
                    txtCodAutoadhesivo.Text = dtActuacionInsumoDetalle.Rows[0]["insu_vCodigoUnicoFabrica"].ToString();

                    ctrlAdjunto.SetCodigoVinculacion(txtCodAutoadhesivo.Text);
                    Session["CodAutoadhesivo"] = txtCodAutoadhesivo.Text;

                    txtCodAutoadhesivo.Enabled = true;
                    btnLimpiar.Enabled = true;
                    string strFlag = dtActuacionInsumoDetalle.Rows[0]["aide_bFlagImpresion"].ToString();
                    if (strFlag.Equals("SI"))
                    {
                        chkImpresion.Checked = true;
                        txtCodAutoadhesivo.Enabled = false;
                        btnLimpiar.Enabled = false;
                        hdn_ImpresionCorrecta.Value = "1";
                    }
                    else
                    {
                        if (txtCodAutoadhesivo.Text.Trim() == String.Empty)
                        {
                            txtCodAutoadhesivo.Enabled = true;
                            btnLimpiar.Enabled = true;
                        }
                        else
                        {
                            txtCodAutoadhesivo.Enabled = false;
                            btnLimpiar.Enabled = false;
                        }
                        chkImpresion.Checked = false;
                        btnVistaPrev.Enabled = true;
                        hdn_ImpresionCorrecta.Value = "";
                    }

                    btnGrabarVinculacion.Enabled = false;


                    Session[Constantes.CONST_ACTUACION_INSUMO_DETALLE_ID + HFGUID.Value] = dtActuacionInsumoDetalle.Rows[0]["aide_iActuacionInsumoDetalleId"].ToString();
                }

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
                btnLimpiar.Enabled = true;
                btnGrabarVinculacion.Enabled = true;
                btnVistaPrev.Enabled = false;
            }
        }

        private Int64 RegistrarRegistroCivilOtrasTarifas()
        {
            long lngActuacionDetalleId = 0;

            if (HFGUID.Value.Length > 0)
            {
                lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);
            }
            else
            {
                lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
            }


            if (lngActuacionDetalleId == 0)
                return 0;

            BE.MRE.RE_REGISTROCIVIL ObjRegCivBE = new BE.MRE.RE_REGISTROCIVIL();

            ObjRegCivBE.reci_iRegistroCivilId = Convert.ToInt64(hifRegistroCivil.Value);
            ObjRegCivBE.reci_iActuacionDetalleId = lngActuacionDetalleId;
            ObjRegCivBE.reci_sTipoActaId = Convert.ToInt16(this.ddlActaTarifa.SelectedValue);
            ObjRegCivBE.reci_vNumeroActa = "0";
            ObjRegCivBE.reci_dFechaRegistro = FechaHora(this.txtFechaRegistro.Text);

            if (txtIdTarifa.Text.Trim().Contains(Constantes.CONST_EXCEPCION_TARIFA_ID_2.ToString()))
                ObjRegCivBE.reci_bAnotacionFlag = true;
            else
                ObjRegCivBE.reci_bAnotacionFlag = false;

            ObjRegCivBE.reci_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            ObjRegCivBE.reci_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
            ObjRegCivBE.reci_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            ObjRegCivBE.reci_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

            List<RE_PARTICIPANTE> lstParticipantes = new List<RE_PARTICIPANTE>();

            Proceso MiProc = new Proceso();
            ActoCivilMantenimientoBL BL = new ActoCivilMantenimientoBL();
            int intResultado = 0;
            #region ACTUALIZA ACTO CIVIL
            if ((Convert.ToInt32(Session["ActoCivil_Accion"]).Equals((int)Enumerador.enmTipoOperacion.ACTUALIZACION))
                && (ObjRegCivBE.reci_iActuacionDetalleId != 0)
                && (ObjRegCivBE.reci_iRegistroCivilId > 0))
            {
                intResultado = BL.Actualizar(ObjRegCivBE,
                                             Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), lstParticipantes, null);
            }
            #endregion

            #region INSERTA ACTO CIVIL
            if (((Convert.ToInt32(Session["ActoCivil_Accion"]).Equals((int)Enumerador.enmTipoOperacion.REGISTRO))
                && (ObjRegCivBE.reci_iActuacionDetalleId != 0))
                || ObjRegCivBE.reci_iRegistroCivilId < 1)
            {
                long LonRegistroCivilId = 0;
                intResultado = BL.Insertar(ObjRegCivBE, Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), lstParticipantes/*Participantes(ObjRegCivBE)*/, ref LonRegistroCivilId);
                if (intResultado > 0)
                {
                    ObjRegCivBE.reci_iRegistroCivilId = LonRegistroCivilId;
                    hifRegistroCivil.Value = LonRegistroCivilId.ToString();
                    Session[strRegistroCivilId] = LonRegistroCivilId.ToString();
                    Session["ActoCivil_Accion"] = Enumerador.enmTipoOperacion.ACTUALIZACION;
                }
            }
            #endregion

            if (intResultado > 0)
            {
                this.ddlTipoActa.Enabled = false;
            }
            updFormato.Update();

            return ObjRegCivBE.reci_iRegistroCivilId;
        }

        protected void btnGrabarVinculacion_Click(object sender, EventArgs e)
        {
            String strScript = String.Empty;

            if (Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]) == 0)
            {

                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO CIVIL", "Se perdió la sesión. Seleccione otra vez el trámite.");
                Comun.EjecutarScript(Page, strScript);

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


            #region Otras Tarifas civiles excepto la Tarifa 1


            if (!txtIdTarifa.Text.Trim().Equals(Constantes.CONST_EXCEPCION_TARIFA_ID_1.ToString()))
            {

                if (txtIdTarifa.Text == Convert.ToString(Constantes.CONST_EXCEPCION_TARIFA_ID_2))
                {
                    if (Grd_Anotaciones.Rows.Count == 0)
                    {
                        strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO CIVIL", Constantes.CONST_MENSAJE_NO_TIENE_ANOTACIONES);
                        Comun.EjecutarScript(Page, strScript);
                        return;
                    }
                }

                long lngRegistroCivilId = RegistrarRegistroCivilOtrasTarifas();

                if (lngRegistroCivilId < 1)
                {
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO CIVIL", Constantes.CONST_MENSAJE_OPERACION_FALLIDA);
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }
            }


            #endregion


            #region Tags
            String scriptMover = String.Empty;
            if (txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_3A.ToString())
            {
                scriptMover = @"$(function(){{EnableTabIndex(0);EnableTabIndex(1);DisableTabIndex(2); DisableTabIndex(3);DisableTabIndex(4); MoveTabIndex(1);}});";
            }
            else
            {
                if (txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_ID_2.ToString())
                {
                    scriptMover = @"$(function(){{EnableTabIndex(0);DisableTabIndex(1);EnableTabIndex(2); EnableTabIndex(3);EnableTabIndex(4); MoveTabIndex(4);}});";
                }
                else
                {
                    if (txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_ID_1.ToString())
                    {
                        scriptMover = @"$(function(){{EnableTabIndex(0);EnableTabIndex(1);EnableTabIndex(2); DisableTabIndex(3);EnableTabIndex(4); MoveTabIndex(4);}});";
                    }
                    else
                    {
                        scriptMover = @"$(function(){{EnableTabIndex(0);DisableTabIndex(1);EnableTabIndex(2); DisableTabIndex(3);EnableTabIndex(4); MoveTabIndex(4);}});";
                    }
                }
            }

            scriptMover = string.Format(scriptMover);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);
            #endregion

            if (Convert.ToInt64(hifRegistroCivil.Value) > 0 || txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_ID_2.ToString())
            {
                //if (chkImpresion.Checked)
                //{
                Int64 intActDetalleId = 0;

                if (Session["NuevoRegistro"] != null)
                    if (!Convert.ToBoolean(Session["NuevoRegistro"]))
                        intActDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);

                String FormatoFecha = ConfigurationManager.AppSettings["FormatoFechas"].ToString();

                if (FormatoFecha.Trim() == String.Empty)
                {
                    FormatoFecha = "MMM-dd-yyyy";
                }

                DateTime dFecActual = Util.ObtenerFechaActual(
                                                Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sDiferenciaHoraria")),
                                                Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sHorarioVerano")));

                DateTime dFecImpresion = Comun.FormatearFecha("01/01/1800");


                string strMensaje = string.Empty;

                ActuacionMantenimientoBL oActuacionMantenimientoBL = new ActuacionMantenimientoBL();
                int intResultado = oActuacionMantenimientoBL.VincularAutoadhesivo(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value]),
                                            Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]),
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
                    //txtCodAutoadhesivo.Enabled = false;
                    chkImpresion.Enabled = false;

                    Session["COD_AUTOADHESIVO"] = txtCodAutoadhesivo.Text.Trim();
                    if (txtCodAutoadhesivo.Text.Trim() != string.Empty)
                    {
                        //ctrlToolBarRegistro.btnGrabar.Enabled = false;
                        //ctrlToolbarFormato.btnGrabar.Enabled = false;
                        //HabilitaControlesTabFormato(false);
                        //cbxAfirmarTexto.Checked = true;
                        //cbxAfirmarTexto.Enabled = false;
                        //Grd_Participantes.Enabled = false;
                        //btnAceptar.Enabled = false;
                        //btnCancelar.Enabled = false;
                        //HFAutodhesivo.Value = "1";

                        txtCodAutoadhesivo.Enabled = false;
                        btnLimpiar.Enabled = false;
                        btnVistaPrev.Enabled = false;
                    }

                    #region Tipo Adjunto
                    ctrlAdjunto.CargarTipoArchivo();
                    Cargar_Actuacion();

                    HabilitaControlesTabAnotacion(false);
                    BtnGrabAnotacion.Enabled = false;

                    ctrlAdjunto.IsCivil = true;
                    updActuacionAdjuntar.Update();
                    updFormato.Update();
                    updRegPago.Update();
                    updAnotaciones.Update();
                    updActuacionAdjuntar.Update();
                    #endregion

                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION,
                        "VINCULACIÓN", "La vinculación se realizó correctamente.");

                    HF_HABILITAR_CHECK_IMPRESION.Value = "false";
                    txtCodAutoadhesivo.Enabled = false;
                    btnLimpiar.Enabled = false;
                    btnGrabarVinculacion.Enabled = false;
                    btnVistaPrev.Enabled = true;
                    
                    //Comun.EjecutarScript(Page, strScript + Util.ActivarTab(2, "Adjuntos") + Util.MoverTab(2));
                    if (HFGUID.Value.Length > 0)
                    {
                        BindGridActuacionesInsumoDetalle(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]));
                    }
                    else
                    {
                        BindGridActuacionesInsumoDetalle(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]));
                    }
                }
                else
                {
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "VINCULACIÓN", strMensaje, false, 200, 400);
                    Comun.EjecutarScript(Page, strScript);
                }
                //}
                //else
                //{
                //    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "VINCULACIÓN", "Falta Validar Impresión Correcta.", false, 200, 400);
                //    Comun.EjecutarScript(Page, strScript);
                //}
            }
            else
            {
                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "VINCULACIÓN", "Debe Grabar el Registro Civil antes de realizar la vinculación.", false, 200, 400);
                Comun.EjecutarScript(Page, strScript);
                return;
            }
            ddlTipoPago.SelectedValue = hTipoPago.Value.ToString();
            
            updRegPago.Update();
        }

        private void VerificarVariablesSesion()
        {
            chkImpresion.Enabled = Convert.ToBoolean(HF_HABILITAR_CHECK_IMPRESION.Value);
        }

        protected void ctrlPagActuacionInsumoDetalle_Click(object sender, EventArgs e)
        {
            BindGridActuacionesInsumoDetalle(Convert.ToInt64(Session["ActuacionDetalleId" + HFGUID.Value]));
            updVinculacion.Update();
        }

        protected void chkImpresion_CheckedChanged(object sender, EventArgs e)
        {
            if (chkImpresion.Checked)
            {
                //txtCodAutoadhesivo.Enabled = true;
                //btnGrabarVinculacion.Enabled = true;
            }
            else
            {
                //txtCodAutoadhesivo.Enabled = false;
                //btnGrabarVinculacion.Enabled = false;
            }

            updVinculacion.Update();
        }

        private void LimpiarVinculacion()
        {
            btnGrabarVinculacion.Enabled = true;
            chkImpresion.Checked = false;
            txtCodAutoadhesivo.Text = string.Empty;
            btnVistaPrev.Enabled = false;
            Grd_ActInsDet.DataSource = null;
            Grd_ActInsDet.DataBind();
        }

        #endregion Vinculación


        #region Metodos

        private void CargarDatosIniciales()
        {
            #region Datos Personales

            lblDestino.Text = string.Empty;
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




            #endregion Datos Personales

            btnAnotacion.Visible = false;
            btnCopiaCert.Visible = false;

            hidEventNuevo.Value = "";
            ddlActuacionTipo.SelectedValue = ddlTipoActa.SelectedValue;

            Session["COD_AUTOADHESIVO"] = string.Empty;

            pnlDatosAdicioActaMatrim.Visible = false;
            lblDestino.Text = strEtiquetaSolicitante;
            tablaReconocimientoAdopcion.Visible = false;
            tablaReconstitucionReposicion.Visible = false;
            tablaInscripcionOficio.Visible = false;
            pnlActaAnterior.Visible = false;
            //---------------------------------------------------
            // Autor: Miguel Márquez Beltrán
            // Fecha: 05/10/2016
            // Objetivo: Mostrar la fecha actual del consulado
            //---------------------------------------------------
            
            //txtFechaRegistro.Text = DateTime.Now.ToString(ConfigurationManager.AppSettings["FormatoFechas"]);

            txtFechaRegistro.Text = Comun.FormatearFecha((Accesorios.Comun.ObtenerFechaActualTexto(HttpContext.Current.Session))).ToString("MMM-dd-yyyy");
            //---------------------------------------------------

            lblDatosTitular.Text = "DATOS DE NACIMIENTO";
            lblFechaTitular.Text = "Fecha Nac.:";
            lblHoraTitular.Text = "Hora Nac.: ";
            lblDatosOcurrencia.Text = "LUGAR DE OCURRENCIA";
            long lngActuacionDetalleId = 0;


            lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
  
            if (Session["InicioTramite"] != "1")
            {
                BindGridActuacionesInsumoDetalle(lngActuacionDetalleId);
                CargarGrillaAdjuntos(lngActuacionDetalleId);
            }

            Session[strRegistroCivilId] = 0;

            #region Oficina Registral

            string strUbigeo = string.Empty;
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                strUbigeo = Constantes.CONST_OFICINACONSULAR_LIMA_UBIGEO;
            }
            else
            {
                strUbigeo = comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_cUbigeoCodigo").ToString();
            }
            #endregion Oficina Registral
        }

        private void CargarListadosDesplegables()
        {
            Util.CargarParametroDropDownList(ddlActuacionTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_CIVIL_TIPO_RECONOCIMIENTO), true);
            //------------------------------------------------------------------------------
            DataTable dtTipoActa = new DataTable();

            dtTipoActa = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_CIVIL_TIPO_RECONOCIMIENTO);

            Util.CargarParametroDropDownList(ddlTipoActa, dtTipoActa, false);
            Util.CargarParametroDropDownList(ddlTipoActaAnotacion, dtTipoActa, true);
            Util.CargarParametroDropDownList(ddlActaTarifa, dtTipoActa, false);
            //------------------------------------------------------------------------------            
            Util.CargarParametroDropDownList(ddlTipoPago, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACREDITACION_TIPO_COBRO), true);
            Util.CargarParametroDropDownList(ddlNomBanco, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.BANCO), true);
            Util.CargarParametroDropDownList(ddl_Genero, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_GENERO), true);
            Util.CargarParametroDropDownList(ddlGenero_Titular, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_GENERO), true);

            Util.CargarParametroDropDownList(ddl_TipoOcurrencia, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_CIVIL_NACIMIENTO_LUGAR), true);

            //----------------------------------------------------

            Util.CargarParametroDropDownList(cmb_TipoAnotacion, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTUACION_TIPO_ANOTACION), true);

            Util.CargarParametroDropDownList(CmbEstCiv, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.ESTADO_CIVIL), true);

            if (Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Convert.ToInt16(Constantes.CONST_ID_CONSULADO_CARACAS))
            {
                ddlTipoPago.Items.FindByText("PAGO ARUBA").Enabled = false;
                ddlTipoPago.Items.FindByText("PAGO OTRAS ISLAS CARIBEÑAS").Enabled = false;
            }

            beUbigeoListas obeUbigeoListas = new beUbigeoListas();
            UbigeoConsultasBL objUbigeoBL = new UbigeoConsultasBL();

            obeUbigeoListas = objUbigeoBL.obtenerUbiGeo();

            ViewState["Ubigeo"] = obeUbigeoListas;
            if (obeUbigeoListas != null)
            {
                if (obeUbigeoListas.Ubigeo01.Count > 0)
                {
                    obeUbigeoListas.Ubigeo01.Insert(0, new beUbicaciongeografica { Ubi01 = "0", Departamento = "-- SELECCIONE --" });

                    ddl_DeptOcurrencia.DataSource = obeUbigeoListas.Ubigeo01;
                    ddl_DeptOcurrencia.DataValueField = "Ubi01";
                    ddl_DeptOcurrencia.DataTextField = "Departamento";
                    ddl_DeptOcurrencia.DataBind();
                }
            }

            this.ddl_DistOcurrencia.Items.Clear();
            this.ddl_DistOcurrencia.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- SELECCIONAR -", "0"));
            this.ddl_ProvOcurrencia.Items.Clear();
            this.ddl_ProvOcurrencia.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- SELECCIONAR -", "0"));
        }

        private DataTable CrearTablaActuacionDetalle()
        {
            DataTable DtDetActuaciones = new DataTable();
            DtDetActuaciones.Columns.Clear();

            DataColumn dcTarifarioId = DtDetActuaciones.Columns.Add("sTarifarioId", typeof(int));
            dcTarifarioId.AllowDBNull = true;
            dcTarifarioId.Unique = false;

            DataColumn dcItem = DtDetActuaciones.Columns.Add("sItem", typeof(int));
            dcItem.AllowDBNull = true;
            dcItem.Unique = false;

            DataColumn dcFechaRegistro = DtDetActuaciones.Columns.Add("dFechaRegistro", typeof(string));
            dcFechaRegistro.AllowDBNull = true;
            dcFechaRegistro.Unique = false;

            DataColumn dcRequisitoFlag = DtDetActuaciones.Columns.Add("bRequisitosFlag", typeof(bool));
            dcRequisitoFlag.AllowDBNull = true;
            dcRequisitoFlag.Unique = false;

            DataColumn dcVinculacionInsumo = DtDetActuaciones.Columns.Add("sVinculacionInsumoId", typeof(int));
            dcVinculacionInsumo.AllowDBNull = true;
            dcVinculacionInsumo.Unique = false;

            DataColumn dcVinculacionFecha = DtDetActuaciones.Columns.Add("dVinculacionFecha", typeof(string));
            dcVinculacionFecha.AllowDBNull = true;
            dcVinculacionFecha.Unique = false;

            DataColumn dcActuacionCorrelativo = DtDetActuaciones.Columns.Add("ICorrelativoActuacion", typeof(long));
            dcActuacionCorrelativo.AllowDBNull = true;
            dcActuacionCorrelativo.Unique = false;

            DataColumn dcTarifarioCorrelativo = DtDetActuaciones.Columns.Add("ICorrelativoTarifario", typeof(long));
            dcTarifarioCorrelativo.AllowDBNull = true;
            dcTarifarioCorrelativo.Unique = false;

            DataColumn dcFuncionarioFir = DtDetActuaciones.Columns.Add("sFuncionarioFirmanteId", typeof(int));
            dcFuncionarioFir.AllowDBNull = true;
            dcFuncionarioFir.Unique = false;

            DataColumn dcFuncionarioCont = DtDetActuaciones.Columns.Add("sFuncionarioContactoId", typeof(int));
            dcFuncionarioCont.AllowDBNull = true;
            dcFuncionarioCont.Unique = false;

            DataColumn dcImpresionFlag = DtDetActuaciones.Columns.Add("bImpresionFlag", typeof(bool));
            dcImpresionFlag.AllowDBNull = true;
            dcImpresionFlag.Unique = false;

            DataColumn dcImpresionFecha = DtDetActuaciones.Columns.Add("dImpresionFecha", typeof(DateTime));
            dcImpresionFecha.AllowDBNull = true;
            dcImpresionFecha.Unique = false;

            DataColumn dcImpresionFuncionario = DtDetActuaciones.Columns.Add("sImpresionFuncionarioId", typeof(int));
            dcImpresionFuncionario.AllowDBNull = true;
            dcImpresionFuncionario.Unique = false;

            DataColumn dcNotas = DtDetActuaciones.Columns.Add("vNotas", typeof(string));
            dcNotas.AllowDBNull = true;
            dcNotas.Unique = false;

            return DtDetActuaciones;
        }

        private void LimpiarControlesTabFormato(int IntTarifa)
        {
            txtLibroRegCiv.Text = "";
            txtNroActa.Text = "";
            txtNroCUI.Text = "";
            txtFechaRegistro.Text = "";

            txtNroExpediente.Text = "";
            txtCargoCelebrante.Text = "";

            txtHora.Text = "";
            //ddl_Genero.SelectedIndex = -1;
            txtNombresTitular.Text = "";
            txtApePatTitular.Text = "";
            txtApeMatTitular.Text = "";

            ddl_TipoOcurrencia.SelectedIndex = -1;
            txtLugarOcurrencia.Text = "";


            txtObservaciones.Text = string.Empty;
        }

        private void ConfigurarTabFormato(int IntTipoActa)
        {
            switch (IntTipoActa)
            {
                case (int)Enumerador.enmTipoActa.NACIMIENTO:
                                       
                    ddlTipoActa.SelectedValue = Convert.ToString((int)Enumerador.enmTipoActa.NACIMIENTO);

                    pnlRegCivEncabezado.Visible = true;

                    pnlDatosAdicioActaMatrim.Visible = false;

                    TitDatosTitular.Visible = true;
                    pnlDatosTitular.Visible = true;

                    lblDatosTitular.Text = "DATOS DE NACIMIENTO";
                    lblFechaTitular.Text = "Fecha Nac.:";
                    lblHoraTitular.Text = "Hora Nac.: ";

                    lblNacLugarTipo.Visible = true;
                    ddl_TipoOcurrencia.Visible = true;
                    lblNacLugar.Visible = true;
                    txtLugarOcurrencia.Visible = true;
                    lblCO_ContDepOffiReg0.Visible = true;
                    lblCO_ContDepOffiReg3.Visible = true;

                    txtNroCUI.Visible = true;
                    lblCUI.Visible = true;
                    lblCO_txtNroCUI.Visible = true;

                    btnActa.Text = "Acta de Nacimiento";

                    break;

                case (int)Enumerador.enmTipoActa.MATRIMONIO:

                    ddlTipoActa.SelectedValue = Convert.ToString((int)Enumerador.enmTipoActa.MATRIMONIO);

                    pnlRegCivEncabezado.Visible = true;

                    pnlDatosAdicioActaMatrim.Visible = true;

                    this.lblDatosTitular.Text = "DATOS DE MATRIMONIO";
                    this.lblFechaTitular.Text = "Fecha Celebracion:";
                    this.lblHoraTitular.Visible = false;
                    this.txtHora.Visible = false;
                    this.lblCO_ContDepOffiReg19.Visible = false;
                    this.LblFechaValija1.Visible = false;
                    this.Label12.Visible = false;
                    this.ddl_Genero.Visible = false;
                    this.lblCO_ddl_Genero.Visible = false;
                    this.Label69.Visible = false;
                    this.txtNombresTitular.Visible = false;
                    this.Label67.Visible = false;
                    this.txtApePatTitular.Visible = false;
                    this.Label68.Visible = false;
                    this.txtApeMatTitular.Visible = false;

                    lblNacLugarTipo.Visible = false;
                    ddl_TipoOcurrencia.Visible = false;
                    lblNacLugar.Visible = false;
                    txtLugarOcurrencia.Visible = false;
                    lblCO_ContDepOffiReg0.Visible = false;
                    lblCO_ContDepOffiReg3.Visible = false;

                    txtNroCUI.Visible = false;
                    lblCUI.Visible = false;
                    lblCO_txtNroCUI.Visible = false;

                    btnActa.Text = "Acta de Matrimonio";
                    imgBuscarCUI.Visible = false;
                    break;

                case (int)Enumerador.enmTipoActa.DEFUNCION:

                    ddlTipoActa.SelectedValue = Convert.ToString((int)Enumerador.enmTipoActa.DEFUNCION);

                    pnlRegCivEncabezado.Visible = true;

                    pnlDatosAdicioActaMatrim.Visible = false;

                    TitDatosTitular.Visible = true;
                    pnlDatosTitular.Visible = true;

                    lblDatosTitular.Text = "DATOS DEL FALLECIDO";
                    lblFechaTitular.Text = "Fecha Fallec.:";
                    lblHoraTitular.Text = "Hora Fallec.:";

                    lblNacLugarTipo.Visible = true;
                    ddl_TipoOcurrencia.Visible = true;
                    lblNacLugar.Visible = true;
                    txtLugarOcurrencia.Visible = true;
                    lblCO_ContDepOffiReg0.Visible = true;
                    lblCO_ContDepOffiReg3.Visible = true;

                    txtNroCUI.Visible = false;
                    lblCUI.Visible = false;
                    lblCO_txtNroCUI.Visible = false;

                    this.lblHoraTitular.Visible = true;
                    this.txtHora.Visible = true;

                    this.Label12.Visible = true;
                    this.ddl_Genero.Visible = true;
                    this.lblCO_ddl_Genero.Visible = true;
                    this.Label69.Visible = true;
                    this.txtNombresTitular.Visible = true;
                    this.Label67.Visible = true;
                    this.txtApePatTitular.Visible = true;
                    this.Label68.Visible = true;
                    this.txtApeMatTitular.Visible = true;

                    btnActa.Text = "Acta de Defunción";
                    imgBuscarCUI.Visible = false;
                    break;
            }

            updFormato.Update();
            updVinculacion.Update();
        }

        private void ConfigurarTabVinculo(int IntNroTarifa, int IntTipoActa)
        {
            switch (IntNroTarifa)
            {
                case Constantes.CONST_EXCEPCION_TARIFA_ID_1: // 1 - INSCRIPCION NACIMIENTO, MATRIMONIO O DEFUNCION
                    {
                        if (IntTipoActa == (int)Enumerador.enmTipoActa.NACIMIENTO)
                        {
                            btnActa.Text = "Acta Nacimiento";
                        }

                        if (IntTipoActa == (int)Enumerador.enmTipoActa.MATRIMONIO)
                        {
                            btnActa.Text = "Acta Matrimonio";
                        }

                        if (IntTipoActa == (int)Enumerador.enmTipoActa.DEFUNCION)
                        {
                            btnActa.Text = "Acta Defunción";
                        }

                        break;
                    }
                case Constantes.CONST_EXCEPCION_TARIFA_ID_2: // 2 - INSCRIPCION SENTENCIAS O ANOTACIONES EST. CIVIL
                    {
                        btnActa.Text = "Anotación";
                        break;
                    }
                case Constantes.CONST_EXCEPCION_TARIFA_ID_3: // 2* - ANOTACIÓN EN RECTIFICACIÓN
                    {
                        btnActa.Text = "Anotación - (Rectificación)";
                        break;
                    }
                case Constantes.CONST_EXCEPCION_TARIFA_ID_4: // 3 - COPIA CERTIFICADA AS. ESTADO CIVIL
                    {
                        btnActa.Text = "Copia Certificada";
                        break;
                    }
                case Constantes.CONST_EXCEPCION_TARIFA_ID_5: // 3A - 1º COPIA CERTIFICADA AS. ESTADO CIVIL - GRATIS
                    {
                        if (IntTipoActa == (int)Enumerador.enmTipoActa.NACIMIENTO)
                        {
                            btnActa.Text = "Acta Nacimiento";
                        }

                        if (IntTipoActa == (int)Enumerador.enmTipoActa.MATRIMONIO)
                        {
                            btnActa.Text = "Acta Matrimonio";
                        }

                        if (IntTipoActa == (int)Enumerador.enmTipoActa.DEFUNCION)
                        {
                            btnActa.Text = "Acta Defunción";
                        }

                        break;
                    }
                case Constantes.CONST_EXCEPCION_TARIFA_ID_6: // 3B - REG.MILITAR
                    {
                        btnActa.Text = "Constancia de Inscripción";
                        break;
                    }
                default:
                    break;
            }
        }

        private void PintarDatosPestaniaRegistro()
        {
            BE.RE_TARIFA_PAGO objTarifaPago = new RE_TARIFA_PAGO();
            objTarifaPago = (BE.RE_TARIFA_PAGO)Session[Constantes.CONST_SESION_OBJ_TARIFA_PAGO];

            txtIdTarifa.Text = objTarifaPago.vTarifa;

            // Título según tarifa
            lblTituloTarifa.Text = objTarifaPago.vTarifaDescripcion;

            txtDescTarifa.Text = objTarifaPago.vTarifaDescripcion;
            txtMontoSC.Text = string.Format("{0:0.00}", objTarifaPago.dblMontoSolesConsulares);
            txtMontoML.Text = string.Format("{0:0.00}", objTarifaPago.dblMontoMonedaLocal);
            txtTotalSC.Text = string.Format("{0:0.00}", objTarifaPago.dblTotalSolesConsulares);
            txtTotalML.Text = string.Format("{0:0.00}", objTarifaPago.dblTotalMonedaLocal);
            LblFecha.Text = objTarifaPago.datFechaRegistroActuacion.ToString(ConfigurationManager.AppSettings["FormatoFechas"]);

            ddlTipoPago.SelectedValue = objTarifaPago.sTipoPagoId.ToString();
            hTipoPago.Value = objTarifaPago.sTipoPagoId.ToString();
            txtCantidad.Text = objTarifaPago.dblCantidad.ToString();
            
            
            
            if (objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA ||
                objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA ||
                objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA)
            {
                txtNroOperacion.Text = objTarifaPago.vNumeroOperacion;
                ddlNomBanco.SelectedValue = objTarifaPago.sBancoId.ToString();
                ctrFecPago.Text = objTarifaPago.datFechaPago.ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                
                string strFormato = ConfigurationManager.AppSettings["FormatoMonto"].ToString();

                txtMtoCancelado.Text = (Convert.ToDouble(txtCantidad.Text) * Convert.ToDouble(txtMontoSC.Text)).ToString(strFormato);
                
                HabilitaCamposPagoActuacion(false);
                pnlPagLima.Visible = true;
                txtNroOperacion.Enabled = false;
                ddlNomBanco.Enabled = false;
                ctrFecPago.Enabled = false;
                txtMtoCancelado.Enabled = false;

            }

            txtSustentoTipoPago.Text = objTarifaPago.vSustentoTipoPago;

            if (objTarifaPago != null)
            {
                string strDescTipoPagoOrigen = Comun.ObtenerDescripcionTipoPago(Session, objTarifaPago.sTipoPagoId.ToString());


                Comun.ActualizarControlPago(Session, strDescTipoPagoOrigen, txtIdTarifa.Text, txtCantidad.Text,
                    ref ctrlToolBarRegistro.btnGrabar, ref ddlTipoPago, ref txtNroOperacion, ref txtCodAutoadhesivo,
                    ref ddlNomBanco, ref ctrFecPago, ref ddlExoneracion, ref lblExoneracion, ref lblValExoneracion,
                    ref txtSustentoTipoPago, ref lblSustentoTipoPago, ref lblValSustentoTipoPago,
                    ref RBNormativa, ref RBSustentoTipoPago, ref txtMontoML, ref txtMontoSC,
                    ref txtTotalML, ref txtTotalSC, ref LblDescMtoML, ref LblDescTotML,
                    ref pnlPagLima, ref txtMtoCancelado);
            }   


            //----------------------------------------//
            // Autor: Miguel Angel Márquez Beltrán
            // Fecha: 05/03/2019
            // Objetivo: Visualizar la Norma de la tarifa
            //----------------------------------------//                

            if (ddlTipoPago.SelectedItem.Text == "GRATUITO POR LEY" ||
                ddlTipoPago.SelectedItem.Text == "INAFECTO POR INDIGENCIA")
            {
                ddlExoneracion.SelectedValue = objTarifaPago.dblNormaTarifario.ToString();   
            }
            
            txtObservaciones.Text = objTarifaPago.vObservaciones;
            

            if (ddlActuacionTipo.SelectedIndex > 0)
            {
                ConfigurarTabFormato((int)Enumerador.enmTipoActa.NACIMIENTO);
            }
                        
            updRegPago.Update();
        }

        private void HabilitaDatosPago(bool bolHabilitar = true)
        {
            txtNroOperacion.Enabled = bolHabilitar;
            ddlNomBanco.Enabled = bolHabilitar;
            txtMtoCancelado.Enabled = bolHabilitar;
            ctrFecPago.Enabled = bolHabilitar;
        }
        private void BloquearParaTarifasGratuitas()
        {
            double decMontoSC = 0;
            decMontoSC = Convert.ToDouble(txtMontoSC.Text);
            if (decMontoSC == 0 && ddlTipoPago.SelectedValue == ((int)Enumerador.enmTipoCobroActuacion.GRATIS).ToString())
            {
                txtSustentoTipoPago.Enabled = false;
                txtSustentoTipoPago.Text = "DS 045-2003-RE TARIFA DE DERECHOS CONSULARES";

                if (ddlExoneracion.Items.Count == 2)
                {
                    ddlExoneracion.Enabled = false;
                }
                else if (ddlExoneracion.Items.Count > 2)
                {
                    ddlExoneracion.Enabled = true;
                }
            }
        }
        private void HabilitaPorTarifa()
        {
            double decMontoSC = 0;
            objTarifarioBE = (BE.MRE.SI_TARIFARIO)Session[strVariableTarifario];
            decMontoSC = (double)objTarifarioBE.tari_FCosto;

            if (decMontoSC == 0)
            {
                ddlTipoPago.Enabled = false;
                ddlTipoPago.SelectedValue = ((int)Enumerador.enmTipoCobroActuacion.GRATIS).ToString();
                
                txtCantidad.Text = "1";
            }
            else
            {
                if (txtDescTarifa.Text == string.Empty)
                {
                    ddlTipoPago.Enabled = false;
                }
                else
                {
                    if (ddlTipoPago.SelectedValue == "0")
                    {
                        ddlTipoPago.Focus();
                    }
                    else if (Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.GRATIS) || Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.NO_COBRADO))
                    {
                        txtMtoCancelado.Text = "0.00";
                        decMontoSC = 0;
                    }
                }
            }

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

        private double CalculaCostoML(double decMontoSC, double decTipoCambio)
        {
            return (decMontoSC * decTipoCambio);
        }

        private void LimpiarDatosTarifaPago()
        {
            lblCantidad.Text = "Cantidad:";
            txtCantidad.Text = "1";
            txtCantidad.Enabled = false;
            ddlTipoPago.Enabled = false;

            string strFormato = ConfigurationManager.AppSettings["FormatoMonto"].ToString();
            double dblCero = 0;
            txtMontoSC.Text = dblCero.ToString(strFormato);
            txtMontoML.Text = dblCero.ToString(strFormato);
            txtTotalSC.Text = dblCero.ToString(strFormato);
            txtTotalML.Text = dblCero.ToString(strFormato);

            txtNroOperacion.Text = "";
            ddlNomBanco.SelectedIndex = 0;
            txtMtoCancelado.Text = "0";

            ctrFecPago.Text = DateTime.Now.ToString(ConfigurationManager.AppSettings["FormatoFechas"]);

            ddlTipoPago.SelectedValue = "0";
        }

        #region Habilitar

        private void HabilitaCamposPagoActuacion(bool bolHabilitar = true)
        {
            txtIdTarifa.Enabled = bolHabilitar;
            LstTarifario.Enabled = bolHabilitar;
            imgBuscarTarifarioM.Enabled = bolHabilitar;
            //ddlActuacionTipo.Enabled = bolHabilitar;
            ddlTipoPago.Enabled = bolHabilitar;
            txtCantidad.Enabled = bolHabilitar;
            pnlPagLima.Visible = bolHabilitar;
            //txtObservaciones.Enabled = bolHabilitar;
            lblLeyendaTarifa.Visible = bolHabilitar;
            lblLeyenda.Visible = bolHabilitar;
        }

        private void HabilitaControlesTabFormato(bool bHabilitado)
        {
            //BtnVistaPrevia.Enabled = true;
           
            
            txtLibroRegCiv.Enabled = bHabilitado;
            txtNroActa.Enabled = bHabilitado;
            txtNroCUI.Enabled = bHabilitado;
            txtFechaRegistro.Enabled = bHabilitado;
            txtFecNac.Enabled = bHabilitado;
            
            txtNroExpediente.Enabled = bHabilitado;
            txtCargoCelebrante.Enabled = bHabilitado;

            txtHora.Enabled = bHabilitado;
            ddl_Genero.Enabled = bHabilitado;
            txtNombresTitular.Enabled = bHabilitado;
            txtApePatTitular.Enabled = bHabilitado;
            txtApeMatTitular.Enabled = bHabilitado;

            ddl_TipoOcurrencia.Enabled = bHabilitado;
            txtLugarOcurrencia.Enabled = bHabilitado;
            ddl_DeptOcurrencia.Enabled = bHabilitado;
            ddl_ProvOcurrencia.Enabled = bHabilitado;
            ddl_DistOcurrencia.Enabled = bHabilitado;
            ddl_CentroPobladoOcurrencia.Enabled = bHabilitado;


            HabilitaControl(bHabilitado);
        }

        #endregion Habilitar

        private void CargarDatosActuacionDetalle()
        {
            long lngActuacionDetalleId = Convert.ToInt64(HF_ACTUACIONDET_ID.Value);
            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_PAGE_SIZE_ADJUNTOS;
            int PaginaActual = 0;

            #region Adjuntos

            ctrlAdjunto.Grd_Archivos.DataSource = null;
            ctrlAdjunto.Grd_Archivos.DataBind();

            DataTable dtAdjuntos = new DataTable();
            //Proceso MiProc = new Proceso();

            //object[] miArray = { lngActuacionDetalleId,
            //            "1",
            //            intPaginaCantidad,
            //            IntTotalCount, IntTotalPages };

            //dtAdjuntos = (DataTable)MiProc.Invocar(ref miArray,
            //                                       "SGAC.BE.RE_ACTUACIONADJUNTO",
            //                                       Enumerador.enmAccion.OBTENER);

            ActuacionAdjuntoConsultaBL obj = new ActuacionAdjuntoConsultaBL();
            dtAdjuntos = obj.ActuacionAdjuntosObtener(lngActuacionDetalleId,
                        "1",
                        intPaginaCantidad,
                        ref IntTotalCount,ref IntTotalPages);

            if (dtAdjuntos.Rows.Count > 0)
            {
                ctrlAdjunto.Grd_Archivos.DataSource = dtAdjuntos;
                ctrlAdjunto.Grd_Archivos.DataBind();
            }

            #endregion Adjuntos

            #region Anotacion
            DataTable dtAnotaciones = new DataTable();

            PaginaActual = CtrlPageBarActAnotacion.PaginaActual;

            //object[] arrParametros = {   lngActuacionDetalleId, 
            //                             PaginaActual, 
            //                             intPaginaCantidad,
            //                             IntTotalCount,IntTotalPages };
            //dtAnotaciones = (DataTable)MiProc.Invocar(ref arrParametros, "SGAC.BE.RE_ANOTACION", Enumerador.enmAccion.CONSULTAR);

            ActuacionAnotacionConsultaBL objAnota = new ActuacionAnotacionConsultaBL();
            dtAnotaciones = objAnota.Obtener(lngActuacionDetalleId,
                                         PaginaActual,
                                         intPaginaCantidad,
                                         ref IntTotalCount, ref IntTotalPages);

            if (dtAnotaciones.Rows.Count > 0)
            {
                Grd_Anotaciones.DataSource = dtAnotaciones;
                Grd_Anotaciones.DataBind();
                dtAnotaciones = null;
            }
            #endregion

            #region Vinculación Insumo

            Grd_ActInsDet.DataSource = null;
            Grd_ActInsDet.DataBind();

            ActuacionMantenimientoBL objActuacionMantenimientoBL = new ActuacionMantenimientoBL();
            DataTable dtActuacionInsumoDetalle = new DataTable();

            PaginaActual = CtrlPageBarActuacionInsumoDetalle.PaginaActual;
            dtActuacionInsumoDetalle = objActuacionMantenimientoBL.Obtener_ActuacionInsumoDetalle(lngActuacionDetalleId, PaginaActual, intPaginaCantidad, ref IntTotalCount, ref  IntTotalPages); // objActuacionMantenimientoBL.Obtener_ActuacionInsumoDetalle(iActuacionDetalleId, ctrlPaginadorAct.PaginaActual.ToString(), intPaginaCantidad, IntTotalCount, IntTotalPages);

            if (dtActuacionInsumoDetalle.Rows.Count > 0)
            {
                txtCodAutoadhesivo.Text = dtActuacionInsumoDetalle.Rows[0]["insu_vCodigoUnicoFabrica"].ToString();
                Session["COD_AUTOADHESIVO"] = txtCodAutoadhesivo.Text.Trim();

                //txtCodAutoadhesivo.Enabled = false;
                ctrlAdjunto.SetCodigoVinculacion(txtCodAutoadhesivo.Text);

                //if (txtCodAutoadhesivo.Text.Trim() != string.Empty)
                //btnVistaPrev.Enabled = false;
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
                string strFlag = dtActuacionInsumoDetalle.Rows[0]["aide_bFlagImpresion"].ToString();
                if (strFlag.Equals("SI"))
                {
                    chkImpresion.Checked = true;
                    chkImpresion.Enabled = false;
                    btnVistaPrev.Enabled = true;
                    hdn_ImpresionCorrecta.Value = "1";
                    txtCodAutoadhesivo.Enabled = false;
                    btnLimpiar.Enabled = false;
                }
                else
                {
                    btnVistaPrev.Enabled = true;
                    chkImpresion.Checked = false;
                    btnVistaPrev.Enabled = true;
                    hdn_ImpresionCorrecta.Value = "";
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
                txtCodAutoadhesivo.Enabled = true;
                btnLimpiar.Enabled = true;
                ctrlBajaAutoadhesivo1.Activar = false;
                txtCodAutoadhesivo.Enabled = true;
                btnGrabarVinculacion.Enabled = true;
                btnVistaPrev.Enabled = false;
            }
            #endregion Vinculación Insumo
        }

        private void CargarGrillaAdjuntos(long LonActuacionDetalleId)
        {
            ctrlAdjunto.Grd_Archivos.DataSource = null;
            ctrlAdjunto.Grd_Archivos.DataBind();

            DataTable dtAdjuntos = new DataTable();
            //Proceso MiProc = new Proceso();

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_PAGE_SIZE_ADJUNTOS;

            //object[] miArray = { LonActuacionDetalleId,
            //                    "1",
            //                    intPaginaCantidad,
            //                    IntTotalCount, IntTotalPages };

            //dtAdjuntos = (DataTable)MiProc.Invocar(ref miArray,
            //                                       "SGAC.BE.RE_ACTUACIONADJUNTO",
            //                                       Enumerador.enmAccion.OBTENER);

            ActuacionAdjuntoConsultaBL obj = new ActuacionAdjuntoConsultaBL();
            dtAdjuntos = obj.ActuacionAdjuntosObtener(LonActuacionDetalleId,
                                "1",
                                intPaginaCantidad,
                                ref IntTotalCount,ref IntTotalPages);


            if (dtAdjuntos.Rows.Count > 0)
            {
                ctrlAdjunto.Grd_Archivos.DataSource = dtAdjuntos;
                ctrlAdjunto.Grd_Archivos.DataBind();
            }
        }
        private void CargarParticipantes()
        {
            Int64 lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
        }
        private void CargarDatosRegistroCivil()
        {
            long lngActuacionDetalleId = 0;

            if (HFGUID.Value.Length > 0)
            {
                lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);
            }
            else
            {
                lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
            }

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
            DataTable dtParticipantes = new DataTable();
            BE.RE_ACTUACIONMILITAR objActuacion = objBL.ObtenerDatosActoCivil(lngActuacionDetalleId, lngPersonaId,out dtParticipantes);

            this.hifRegistroCivil.Value = objActuacion.REGISTROCIVIL.reci_iRegistroCivilId.ToString();

            if (objActuacion.REGISTROCIVIL.acde_iReferenciaId != null)
            {
                this.HF_iReferenciaId.Value = Convert.ToString(objActuacion.REGISTROCIVIL.acde_iReferenciaId);
            }

            if (objActuacion.REGISTROCIVIL.reci_iRegistroCivilId_iReferenciaId != null)
            {
                this.HF_RECI_AUX.Value = Convert.ToString(objActuacion.REGISTROCIVIL.reci_iRegistroCivilId_iReferenciaId);
            }

            Session[strRegistroCivilId] = objActuacion.REGISTROCIVIL.reci_iRegistroCivilId.ToString();

            if (objActuacion.REGISTROCIVIL.reci_iRegistroCivilId > 0)
            {
                btnAgregarParticipante.Enabled = true;
            }
            else {
                btnAgregarParticipante.Enabled = false;
            }
            this.ddlTipoActa.SelectedValue = (objActuacion.REGISTROCIVIL.reci_sTipoActaId).ToString();

            if (Convert.ToInt32(this.ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.NACIMIENTO)
            {
                #region Acta_Nacimiento

                this.txtNumeroActaAnterior.Text = "";
                this.txtTitularActa.Text = "";
                tablaCUI.Visible = true;

                if (objActuacion.REGISTROCIVIL.reci_cConCUI.Equals("S"))
                {
                    this.chkconCUI.Checked = true;
                    this.chksinCUI.Checked = false;

                    tablaReconocimientoAdopcion.Visible = false;
                    pnlActaAnterior.Visible = false;
                    ddl_TipoParticipante.SelectedIndex = 0;
                    txtNroCUI.Enabled = true;
                }
                if (objActuacion.REGISTROCIVIL.reci_cConCUI.Equals("N"))
                {
                    this.chkconCUI.Checked = false;
                    this.chksinCUI.Checked = true;
                    
                    tablaReconocimientoAdopcion.Visible = true;
                    chkReconocimientoAdopcion.Checked = false;
                    ddl_TipoParticipante.SelectedIndex = 0;
                    ddl_TipoDocParticipante.SelectedIndex = 0;
                    ddl_TipoDocParticipante.Enabled = true;
                    ddl_NacParticipante.Enabled = true;
                    txtNroCUI.Enabled = false;
                    imgBuscarCUI.Enabled = false;
                }
                
                if (objActuacion.REGISTROCIVIL.reci_cReconocimientoAdopcion.Equals("S"))
                {
                    tablaReconocimientoAdopcion.Visible = true;
                    chkReconocimientoAdopcion.Checked = true;
                    pnlActaAnterior.Visible = true;
                    
                    if (objActuacion.REGISTROCIVIL.reci_iNumeroActaAnterior != null)
                    {
                        this.txtNumeroActaAnterior.Text = objActuacion.REGISTROCIVIL.reci_iNumeroActaAnterior.ToString();
                    }
                    this.txtTitularActa.Text = objActuacion.REGISTROCIVIL.reci_vTitular.ToString();
                    
                    
                }
                else
                {
                    this.chkReconocimientoAdopcion.Checked = false;
                   pnlActaAnterior.Visible = false;
                }

                if (objActuacion.REGISTROCIVIL.reci_iRegistroCivilId > 0)
                {
                    txtNroCUI.Enabled = false;
                    imgBuscarCUI.Enabled = false;
                    chkconCUI.Enabled = false;
                    chksinCUI.Enabled = false;
                }
                else
                {
                    txtNroCUI.Enabled = true;
                    chkconCUI.Enabled = true;
                    chksinCUI.Enabled = true;
                }


                if (chksinCUI.Checked)
                {
                    if (objActuacion.REGISTROCIVIL.reci_dFechaHoraOcurrenciaActo != DateTime.MinValue)
                    {
                        this.txtFecNac.Text = Comun.FormatearFecha(objActuacion.REGISTROCIVIL.reci_dFechaHoraOcurrenciaActo.ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                        this.txtHora.Text = Comun.FormatearFecha(objActuacion.REGISTROCIVIL.reci_dFechaHoraOcurrenciaActo.ToString()).ToString("HH:mm");
                    }
                    if (objActuacion.REGISTROCIVIL.reci_cOcurrenciaUbigeo.Trim() != string.Empty)
                    {
                        this.ddl_DeptOcurrencia.SelectedValue = objActuacion.REGISTROCIVIL.reci_cOcurrenciaUbigeo.Substring(0, 2);

                        //----------------------------------------------------
                        //Fecha: 03/04/2017
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: Cargar la provincia
                        //----------------------------------------------------
                        //Comun.CargarUbigeo(Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_DeptOcurrencia.SelectedValue, "", "", true, "--SELECCIONAR--", "", Enumerador.enmNacionalidad.NINGUNA, ddl_ProvOcurrencia);
                        if (ViewState["Ubigeo"] != null)
                        {
                            beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                            obeUbigeoListas = (beUbigeoListas)ViewState["Ubigeo"];
                            List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
                            lbeUbicaciongeografica = Comun.obtenerListaUbiGeo("02", ddl_DeptOcurrencia.SelectedValue, "", obeUbigeoListas.Ubigeo02);
                            lbeUbicaciongeografica.Insert(0, new beUbicaciongeografica { Ubi02 = "0", Provincia = "-- SELECCIONE --" });
                            ddl_ProvOcurrencia.DataSource = lbeUbicaciongeografica;
                            ddl_ProvOcurrencia.DataValueField = "Ubi02";
                            ddl_ProvOcurrencia.DataTextField = "Provincia";
                            ddl_ProvOcurrencia.DataBind();
                        }
                        //----------------------------------------------------
                        //Comun.CargarUbigeo(Session, ddl_ProvOcurrencia, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_DeptOcurrencia.SelectedValue, string.Empty, true);

                        if (objActuacion.REGISTROCIVIL.reci_cOcurrenciaUbigeo.Substring(2, 2).Equals("00"))
                        {
                            ddl_ProvOcurrencia.SelectedIndex = 0;
                        }
                        else
                        {
                            this.ddl_ProvOcurrencia.SelectedValue = objActuacion.REGISTROCIVIL.reci_cOcurrenciaUbigeo.Substring(2, 2);
                        }

                        //----------------------------------------------------
                        //Fecha: 03/04/2017
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: Cargar el Distrito
                        //----------------------------------------------------
                        if (this.ddl_ProvOcurrencia.SelectedIndex > 0)
                        {
                            //Comun.CargarUbigeo(Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddl_DeptOcurrencia.SelectedValue, ddl_ProvOcurrencia.SelectedValue, "", true, "--SELECCIONAR--", "", Enumerador.enmNacionalidad.NINGUNA, ddl_DistOcurrencia);
                            if (ViewState["Ubigeo"] != null)
                            {
                                beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                                obeUbigeoListas = (beUbigeoListas)ViewState["Ubigeo"];
                                List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
                                lbeUbicaciongeografica = Comun.obtenerListaUbiGeo("03", ddl_DeptOcurrencia.SelectedValue, ddl_ProvOcurrencia.SelectedValue, obeUbigeoListas.Ubigeo03);
                                lbeUbicaciongeografica.Insert(0, new beUbicaciongeografica { Ubi03 = "00", Distrito = "-- SELECCIONE --" });
                                ddl_DistOcurrencia.DataSource = lbeUbicaciongeografica;
                                ddl_DistOcurrencia.DataValueField = "Ubi03";
                                ddl_DistOcurrencia.DataTextField = "Distrito";
                                ddl_DistOcurrencia.DataBind();
                                ddl_DistOcurrencia.Enabled = (ddl_ProvOcurrencia.SelectedValue.Equals("00") ? false : true);
                                ddl_DistOcurrencia.Focus();
                            }
                            this.ddl_DistOcurrencia.SelectedValue = objActuacion.REGISTROCIVIL.reci_cOcurrenciaUbigeo.Substring(4, 2);
                        }
                        else
                        {
                            Util.CargarParametroDropDownList(ddl_DistOcurrencia, new DataTable(), true);
                            this.ddl_DistOcurrencia.SelectedIndex = 0;
                        }
                        //----------------------------------------------------                        
                    }

                    // bloquear controles
                    txtNombresTitular.Enabled = false;
                    txtApeMatTitular.Enabled = false;
                    txtApePatTitular.Enabled = false;
                    ddl_Genero.Enabled = false;
                }

                
                
                #endregion
            }

            if (Convert.ToInt32(this.ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.MATRIMONIO || Convert.ToInt32(this.ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.DEFUNCION)
            {
                #region Acta_Matrimonio_Defuncion

                tablaCUI.Visible = false;
                this.txtNumeroActaAnterior.Text = "";
                this.txtTitularActa.Text = "";
                tablaReconstitucionReposicion.Visible = true;
                ddl_Genero.Enabled = false;
                txtNombresTitular.Enabled = false;
                txtApeMatTitular.Enabled = false;
                txtApePatTitular.Enabled = false;
                if (objActuacion.REGISTROCIVIL.reci_cReconstitucionReposicion.Equals("S"))
                {
                    this.chkReconstitucionReposicion.Checked = true;
                    pnlActaAnterior.Visible = true;
                    if (objActuacion.REGISTROCIVIL.reci_iNumeroActaAnterior != null)
                    {
                        this.txtNumeroActaAnterior.Text = objActuacion.REGISTROCIVIL.reci_iNumeroActaAnterior.ToString();
                    }
                    this.txtTitularActa.Text = objActuacion.REGISTROCIVIL.reci_vTitular.ToString();

                }
                else
                {
                    this.chkReconstitucionReposicion.Checked = false;
                    pnlActaAnterior.Visible = false;
                }

                
                #endregion
            }
            
            
            this.ddlActaTarifa.SelectedValue = (objActuacion.REGISTROCIVIL.reci_sTipoActaId).ToString(); // Otras tarifas a parte de 1
            this.ddlTipoActa.Enabled = (objActuacion.REGISTROCIVIL.reci_sTipoActaId == -1) ? true : false;
            this.ddlActaTarifa.Enabled = (objActuacion.REGISTROCIVIL.reci_sTipoActaId == -1) ? true : false;

            if (objActuacion.REGISTROCIVIL.reci_sTipoActaId != -1)
            {
                Session["TIPO_ACTO_PARTICIPANTE"] = objActuacion.REGISTROCIVIL.reci_sTipoActaId;
            }

            this.txtLibroRegCiv.Text = objActuacion.REGISTROCIVIL.reci_vLibro;
            this.txtNroActa.Text = (objActuacion.REGISTROCIVIL.reci_vNumeroActa == string.Empty) ? string.Empty.ToString() : objActuacion.REGISTROCIVIL.reci_vNumeroActa.ToString();
            this.txtNroExpediente.Text = objActuacion.REGISTROCIVIL.reci_vNumeroExpedienteMatrimonio;
            this.txtCargoCelebrante.Text = objActuacion.REGISTROCIVIL.reci_vCargoCelebrante;

            if (objActuacion.REGISTROCIVIL.reci_dFechaRegistro != DateTime.MinValue)
            {
                this.txtFechaRegistro.Text = Comun.FormatearFecha(objActuacion.REGISTROCIVIL.reci_dFechaRegistro.ToString()).ToString("MMM-dd-yyyy");
            }
            else
            {
                //---------------------------------------------------
                // Autor: Miguel Márquez Beltrán
                // Fecha: 05/10/2016
                // Objetivo: Mostrar la fecha actual del consulado
                //---------------------------------------------------
                //this.txtFechaRegistro.Text = DateTime.Now.ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                this.txtFechaRegistro.Text = Comun.FormatearFecha(Accesorios.Comun.ObtenerFechaActualTexto(HttpContext.Current.Session)).ToString("MMM-dd-yyyy");
            }

            if (objActuacion.REGISTROCIVIL.reci_sOcurrenciaTipoId > 0)
                this.ddl_TipoOcurrencia.SelectedValue = (objActuacion.REGISTROCIVIL.reci_sOcurrenciaTipoId).ToString();
            else
                this.ddl_TipoOcurrencia.SelectedIndex = 0;

            this.txtLugarOcurrencia.Text = objActuacion.REGISTROCIVIL.reci_vOcurrenciaLugar.ToString();

            //-----------------------------------------------
            //Autor: Miguel Márquez Beltrán
            //Fecha: 03/10/2016
            //Objetivo: Activar Lugar de ocurrencia
            //-----------------------------------------------
            //if ((Int32)objActuacion.REGISTROCIVIL.reci_sTipoActaId == (Int32)Enumerador.enmTipoActa.NACIMIENTO )
            //{                                
            //        desactivar_ubiOcurrencia(false);
            //}


            //SOLO SI ES MATRIMONIO -- SE TOMA DE CIVIL
            if (Convert.ToInt32(Session["TIPO_ACTO_PARTICIPANTE"]) == (int)Enumerador.enmTipoActa.MATRIMONIO)
            {
                #region Participante_Matrimonio

                if (objActuacion.REGISTROCIVIL.reci_dFechaHoraOcurrenciaActo != DateTime.MinValue)
                {
                    this.txtFecNac.Text = Comun.FormatearFecha(objActuacion.REGISTROCIVIL.reci_dFechaHoraOcurrenciaActo.ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                }

                if (objActuacion.REGISTROCIVIL.reci_cOcurrenciaUbigeo.Trim() != string.Empty)
                {
                    this.ddl_DeptOcurrencia.SelectedValue = objActuacion.REGISTROCIVIL.reci_cOcurrenciaUbigeo.Substring(0, 2);

                    //----------------------------------------------------
                    //Fecha: 03/04/2017
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Cargar la provincia
                    //----------------------------------------------------
                    //Comun.CargarUbigeo(Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_DeptOcurrencia.SelectedValue, "", "", true, "--SELECCIONAR--", "", Enumerador.enmNacionalidad.NINGUNA, ddl_ProvOcurrencia);

                    if (ViewState["Ubigeo"] != null)
                    {
                        beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                        obeUbigeoListas = (beUbigeoListas)ViewState["Ubigeo"];
                        List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
                        lbeUbicaciongeografica = Comun.obtenerListaUbiGeo("02", ddl_DeptOcurrencia.SelectedValue, "", obeUbigeoListas.Ubigeo02);
                        lbeUbicaciongeografica.Insert(0, new beUbicaciongeografica { Ubi02 = "0", Provincia = "-- SELECCIONE --" });
                        ddl_ProvOcurrencia.DataSource = lbeUbicaciongeografica;
                        ddl_ProvOcurrencia.DataValueField = "Ubi02";
                        ddl_ProvOcurrencia.DataTextField = "Provincia";
                        ddl_ProvOcurrencia.DataBind();
                    }
                    //----------------------------------------------------

                    //Comun.CargarUbigeo(Session, ddl_ProvOcurrencia, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_DeptOcurrencia.SelectedValue, string.Empty, true);
                    
                    this.ddl_ProvOcurrencia.SelectedValue = objActuacion.REGISTROCIVIL.reci_cOcurrenciaUbigeo.Substring(2, 2);

                    //----------------------------------------------------
                    //Fecha: 03/04/2017
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Cargar el Distrito
                    //----------------------------------------------------
                    //Comun.CargarUbigeo(Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddl_DeptOcurrencia.SelectedValue, ddl_ProvOcurrencia.SelectedValue, "", true, "--SELECCIONAR--", "", Enumerador.enmNacionalidad.NINGUNA, ddl_DistOcurrencia);
                    if (ViewState["Ubigeo"] != null)
                    {
                        beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                        obeUbigeoListas = (beUbigeoListas)ViewState["Ubigeo"];
                        List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
                        lbeUbicaciongeografica = Comun.obtenerListaUbiGeo("03", ddl_DeptOcurrencia.SelectedValue, ddl_ProvOcurrencia.SelectedValue, obeUbigeoListas.Ubigeo03);
                        lbeUbicaciongeografica.Insert(0, new beUbicaciongeografica { Ubi03 = "00", Distrito = "-- SELECCIONE --" });
                        ddl_DistOcurrencia.DataSource = lbeUbicaciongeografica;
                        ddl_DistOcurrencia.DataValueField = "Ubi03";
                        ddl_DistOcurrencia.DataTextField = "Distrito";
                        ddl_DistOcurrencia.DataBind();
                        ddl_DistOcurrencia.Enabled = (ddl_ProvOcurrencia.SelectedValue.Equals("00") ? false : true);
                        ddl_DistOcurrencia.Focus();
                    }
                    //----------------------------------------------------
                    //Comun.CargarUbigeo(Session, ddl_DistOcurrencia, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddl_DeptOcurrencia.SelectedValue, ddl_ProvOcurrencia.SelectedValue, true);
                    this.ddl_DistOcurrencia.SelectedValue = objActuacion.REGISTROCIVIL.reci_cOcurrenciaUbigeo.Substring(4, 2);
                }

           

                lblEstadoCivil.Enabled = false;
                CmbEstCiv.Enabled = false;
                lbldFecNacParticipante.Enabled = false;
                CtrldFecNacimientoParticipante.Enabled = false;
                CtrldFecNacimientoParticipante.EnabledText = false;
                #endregion
            }

            if (objActuacion.TITULAR != null)
            {
                this.txtNombresTitular.Text = nullToString(objActuacion.TITULAR.vNombres);
                this.txtApePatTitular.Text = nullToString(objActuacion.TITULAR.vPrimerApellido);
                this.txtApeMatTitular.Text = nullToString(objActuacion.TITULAR.vSegundoApellido);
                this.ddl_Genero.SelectedValue = Convert.ToInt16(objActuacion.TITULAR.sGeneroId).ToString();

                //SOLO SI ES NACIMIENTO
                if (Convert.ToInt32(Session["TIPO_ACTO_PARTICIPANTE"]) == (int)Enumerador.enmTipoActa.NACIMIENTO && chkconCUI.Checked)
                {
                    #region Participante_Nacimiento

                    this.txtNroCUI.Text = objActuacion.REGISTROCIVIL.reci_vNumeroCUI;
                    this.hCUI.Value = objActuacion.REGISTROCIVIL.reci_vNumeroCUI;
                    if (objActuacion.TITULAR.pers_dNacimientoFecha != DateTime.MinValue)
                    {
                        this.txtFecNac.Text = Comun.FormatearFecha(objActuacion.TITULAR.pers_dNacimientoFecha.ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                        this.txtHora.Text = Comun.FormatearFecha(objActuacion.TITULAR.pers_dNacimientoFecha.ToString()).ToString("HH:mm");
                    }
                    if (objActuacion.TITULAR.pers_cNacimientoLugar.Trim() != string.Empty)
                    {
                        this.ddl_DeptOcurrencia.SelectedValue = objActuacion.TITULAR.pers_cNacimientoLugar.Substring(0, 2);

                        //----------------------------------------------------
                        //Fecha: 03/04/2017
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: Cargar la provincia
                        //----------------------------------------------------
                        //Comun.CargarUbigeo(Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_DeptOcurrencia.SelectedValue, "", "", true, "--SELECCIONAR--", "", Enumerador.enmNacionalidad.NINGUNA, ddl_ProvOcurrencia);
                        if (ViewState["Ubigeo"] != null)
                        {
                            beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                            obeUbigeoListas = (beUbigeoListas)ViewState["Ubigeo"];
                            List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
                            lbeUbicaciongeografica = Comun.obtenerListaUbiGeo("02", ddl_DeptOcurrencia.SelectedValue, "", obeUbigeoListas.Ubigeo02);
                            lbeUbicaciongeografica.Insert(0, new beUbicaciongeografica { Ubi02 = "0", Provincia = "-- SELECCIONE --" });
                            ddl_ProvOcurrencia.DataSource = lbeUbicaciongeografica;
                            ddl_ProvOcurrencia.DataValueField = "Ubi02";
                            ddl_ProvOcurrencia.DataTextField = "Provincia";
                            ddl_ProvOcurrencia.DataBind();
                        }
                        //----------------------------------------------------
                        //Comun.CargarUbigeo(Session, ddl_ProvOcurrencia, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_DeptOcurrencia.SelectedValue, string.Empty, true);

                        if (objActuacion.TITULAR.pers_cNacimientoLugar.Substring(2, 2).Equals("00"))
                        {
                            ddl_ProvOcurrencia.SelectedIndex = 0;
                        }
                        else
                        {
                            this.ddl_ProvOcurrencia.SelectedValue = objActuacion.TITULAR.pers_cNacimientoLugar.Substring(2, 2);
                        }
                        
                        //----------------------------------------------------
                        //Fecha: 03/04/2017
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: Cargar el Distrito
                        //----------------------------------------------------
                        if (this.ddl_ProvOcurrencia.SelectedIndex > 0)
                        {
                            //Comun.CargarUbigeo(Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddl_DeptOcurrencia.SelectedValue, ddl_ProvOcurrencia.SelectedValue, "", true, "--SELECCIONAR--", "", Enumerador.enmNacionalidad.NINGUNA, ddl_DistOcurrencia);
                            if (ViewState["Ubigeo"] != null)
                            {
                                beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                                obeUbigeoListas = (beUbigeoListas)ViewState["Ubigeo"];
                                List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
                                lbeUbicaciongeografica = Comun.obtenerListaUbiGeo("03", ddl_DeptOcurrencia.SelectedValue, ddl_ProvOcurrencia.SelectedValue, obeUbigeoListas.Ubigeo03);
                                lbeUbicaciongeografica.Insert(0, new beUbicaciongeografica { Ubi03 = "00", Distrito = "-- SELECCIONE --" });
                                ddl_DistOcurrencia.DataSource = lbeUbicaciongeografica;
                                ddl_DistOcurrencia.DataValueField = "Ubi03";
                                ddl_DistOcurrencia.DataTextField = "Distrito";
                                ddl_DistOcurrencia.DataBind();
                                ddl_DistOcurrencia.Enabled = (ddl_ProvOcurrencia.SelectedValue.Equals("00") ? false : true);
                                ddl_DistOcurrencia.Focus();
                            }
                            this.ddl_DistOcurrencia.SelectedValue = objActuacion.TITULAR.pers_cNacimientoLugar.Substring(4, 2);
                        }
                        else
                        {
                            Util.CargarParametroDropDownList(ddl_DistOcurrencia, new DataTable(), true);
                            this.ddl_DistOcurrencia.SelectedIndex = 0;
                        }
                        //----------------------------------------------------                        
                    }
                    
                    #endregion
                }

                //SOLO SI ES DEFUNCION
                if (Convert.ToInt32(Session["TIPO_ACTO_PARTICIPANTE"]) == (int)Enumerador.enmTipoActa.DEFUNCION)
                {
                    #region participante_defuncion

                    tablaInscripcionOficio.Visible = true;
                    chkInscripcionOficio.Checked = false;

                    if (objActuacion.REGISTROCIVIL.reci_bInscripcionOficio != null)
                    {
                        if (objActuacion.REGISTROCIVIL.reci_bInscripcionOficio == true)
                        {
                            chkInscripcionOficio.Checked = true;
                        }
                    }
                    

                    if (objActuacion.REGISTROCIVIL.reci_bInscripcionOficio == true)
                    {

                    }
                    else
                    { 
                    }

                    if (objActuacion.REGISTROCIVIL.reci_dFechaHoraOcurrenciaActo != DateTime.MinValue)
                    {
                        this.txtFecNac.Text = Comun.FormatearFecha(objActuacion.REGISTROCIVIL.reci_dFechaHoraOcurrenciaActo.ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"]); 
                        this.txtHora.Text = Comun.FormatearFecha(objActuacion.REGISTROCIVIL.reci_dFechaHoraOcurrenciaActo.ToString()).ToString("HH:mm");
                    }
                    if (objActuacion.REGISTROCIVIL.reci_cOcurrenciaUbigeo != null)
                    {
                        if (objActuacion.REGISTROCIVIL.reci_cOcurrenciaUbigeo.Trim() != string.Empty)
                        {
                            this.ddl_DeptOcurrencia.SelectedValue = objActuacion.REGISTROCIVIL.reci_cOcurrenciaUbigeo.Substring(0, 2);

                            //----------------------------------------------------
                            //Fecha: 03/04/2017
                            //Autor: Miguel Márquez Beltrán
                            //Objetivo: Cargar la provincia
                            //----------------------------------------------------
                            //Comun.CargarUbigeo(Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_DeptOcurrencia.SelectedValue, "", "", true, "--SELECCIONAR--", "", Enumerador.enmNacionalidad.NINGUNA, ddl_ProvOcurrencia);

                            if (ViewState["Ubigeo"] != null)
                            {
                                beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                                obeUbigeoListas = (beUbigeoListas)ViewState["Ubigeo"];
                                List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
                                lbeUbicaciongeografica = Comun.obtenerListaUbiGeo("02", ddl_DeptOcurrencia.SelectedValue, "", obeUbigeoListas.Ubigeo02);
                                lbeUbicaciongeografica.Insert(0, new beUbicaciongeografica { Ubi02 = "0", Provincia = "-- SELECCIONE --" });
                                ddl_ProvOcurrencia.DataSource = lbeUbicaciongeografica;
                                ddl_ProvOcurrencia.DataValueField = "Ubi02";
                                ddl_ProvOcurrencia.DataTextField = "Provincia";
                                ddl_ProvOcurrencia.DataBind();
                            }
                            //----------------------------------------------------

                            //Comun.CargarUbigeo(Session, ddl_ProvOcurrencia, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_DeptOcurrencia.SelectedValue, string.Empty, true);
                            this.ddl_ProvOcurrencia.SelectedValue = objActuacion.REGISTROCIVIL.reci_cOcurrenciaUbigeo.Substring(2, 2);

                            //----------------------------------------------------
                            //Fecha: 03/04/2017
                            //Autor: Miguel Márquez Beltrán
                            //Objetivo: Cargar el Distrito
                            //----------------------------------------------------
                            //Comun.CargarUbigeo(Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddl_DeptOcurrencia.SelectedValue, ddl_ProvOcurrencia.SelectedValue, "", true, "--SELECCIONAR--", "", Enumerador.enmNacionalidad.NINGUNA, ddl_DistOcurrencia);
                            if (ViewState["Ubigeo"] != null)
                            {
                                beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                                obeUbigeoListas = (beUbigeoListas)ViewState["Ubigeo"];
                                List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
                                lbeUbicaciongeografica = Comun.obtenerListaUbiGeo("03", ddl_DeptOcurrencia.SelectedValue, ddl_ProvOcurrencia.SelectedValue, obeUbigeoListas.Ubigeo03);
                                lbeUbicaciongeografica.Insert(0, new beUbicaciongeografica { Ubi03 = "00", Distrito = "-- SELECCIONE --" });
                                ddl_DistOcurrencia.DataSource = lbeUbicaciongeografica;
                                ddl_DistOcurrencia.DataValueField = "Ubi03";
                                ddl_DistOcurrencia.DataTextField = "Distrito";
                                ddl_DistOcurrencia.DataBind();
                                ddl_DistOcurrencia.Enabled = (ddl_ProvOcurrencia.SelectedValue.Equals("00") ? false : true);
                                ddl_DistOcurrencia.Focus();
                            }
                            //----------------------------------------------------                        
                            //Comun.CargarUbigeo(Session, ddl_DistOcurrencia, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddl_DeptOcurrencia.SelectedValue, ddl_ProvOcurrencia.SelectedValue, true);

                            this.ddl_DistOcurrencia.SelectedValue = objActuacion.REGISTROCIVIL.reci_cOcurrenciaUbigeo.Substring(4, 2);
                        }
                    }

                    #endregion
                }
            }
            txtCivilObservaciones.Text = objActuacion.REGISTROCIVIL.reci_vObservaciones;

            // LAYOUT - X ACTO CIVIL
            mtLayoutCivil();

            // PARTICIPANTES
            ViewState["Participante"] = dtParticipantes; //(List<BE.RE_PARTICIPANTE>)objActuacion.PARTICIPANTE_Container;

            lblEstadoCivil.Visible = false;
            CmbEstCiv.Visible = false;
            lbldFecNacParticipante.Visible = false;
            CtrldFecNacimientoParticipante.Visible = false;
            lblObligaFecNacimientoParticipante.Visible = false;
            this.Grd_Participantes.DataSource = dtParticipantes;
            this.Grd_Participantes.DataBind();
            updFormato.Update();
        }

        #endregion

        #region Participantes
        public void HabilitaControl(bool bHabilitado = true)
        {
            this.ddl_TipoParticipante.Enabled = bHabilitado;
            this.ddl_TipoDatoParticipante.Enabled = bHabilitado;
            this.ddl_TipoVinculoParticipante.Enabled = bHabilitado;
            this.ddl_TipoDocParticipante.Enabled = bHabilitado;

            this.txtNroDocParticipante.Enabled = bHabilitado;
            this.ddl_NacParticipante.Enabled = bHabilitado;
            this.txtNomParticipante.Enabled = bHabilitado;
            this.txtApePatParticipante.Enabled = bHabilitado;
            this.txtApeMatParticipante.Enabled = bHabilitado;
            //this.txtDireccionParticipante.Enabled = bHabilitado;
            this.btnAceptar.Enabled = bHabilitado;
            this.btnCancelar.Enabled = bHabilitado;
            this.Grd_Participantes.Enabled = bHabilitado;

            //this.ctrlUbigeo1.HabilitaControl(false);
        }

        private string nullToString(object value)
        {
            return value == null ? "" : value.ToString();
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
            dt.Columns.Add("vDocumentoTipo", typeof(string));
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
            btnCancelar_Click(sender, e);
            //Tomando de la variable de session//
            List<BE.RE_PARTICIPANTE> loParticipanteContainer = (List<BE.RE_PARTICIPANTE>)Session["Participante"];
            //
            int lRowIndex = Convert.ToInt32(e.CommandArgument);

            //GridRow
            string TipoParticipante = Grd_Participantes.Rows[lRowIndex].Cells[0].Text;
            string NmrpDocumento = Grd_Participantes.Rows[lRowIndex].Cells[7].Text;
            txtPersonaId.Text = (Grd_Participantes.Rows[lRowIndex].Cells[9].Text != string.Empty) ? Grd_Participantes.Rows[lRowIndex].Cells[9].Text : "0";

            string strNombres = Grd_Participantes.Rows[lRowIndex].Cells[13].Text;
            string strPaterno = Grd_Participantes.Rows[lRowIndex].Cells[14].Text;

            if (e.CommandName == "Editar")
            {
                
                Int32 sTipoParticipanteID = Convert.ToInt32(Grd_Participantes.Rows[lRowIndex].Cells[0].Text);
                if (sTipoParticipanteID == (Int32)Enumerador.enmParticipanteNacimiento.TITULAR)
                {
                    if (chkconCUI.Checked)
                    {
                        string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "REGISTRO CIVIL", "LA EDICIÓN DEL PARTICIPANTE TITULAR SE REALIZA DESDE LOS CAMPOS DEL ACTA");
                        Comun.EjecutarScript(Page, strScript);
                        return;
                    }
                }
                Int32 sTipoDato = Convert.ToInt32(Grd_Participantes.Rows[lRowIndex].Cells[17].Text);

                String sTipoDoc = Grd_Participantes.Rows[lRowIndex].Cells[4].Text.ToString();
                String NroDni = Grd_Participantes.Rows[lRowIndex].Cells[5].Text.ToString();

                ddl_TipoParticipante.SelectedValue = sTipoParticipanteID.ToString();
                ddl_TipoParticipante_SelectedIndexChanged(null,null);
                ddl_TipoDatoParticipante.SelectedValue = sTipoDato.ToString();
                ddl_TipoDatoParticipante_SelectedIndexChanged(null, null);
                string noHTML = Regex.Replace(Grd_Participantes.Rows[lRowIndex].Cells[5].Text.ToString(), @"<[^>]+>|&nbsp;", "").Trim();

                ddl_NacParticipante.SelectedValue = Grd_Participantes.Rows[lRowIndex].Cells[10].Text.ToString();
                txtNomParticipante.Text = Server.HtmlDecode(Grd_Participantes.Rows[lRowIndex].Cells[11].Text);
                txtApePatParticipante.Text = Server.HtmlDecode(Grd_Participantes.Rows[lRowIndex].Cells[12].Text);

                //============04/11/2020; Autor: Pipa
                //============motivo:se registra un participante con la opcion sin documento, 
                //============luego al editar salia marcado rbSi.Checked = true; y obligaba ingresar nro de documento
                if (txtNroDocParticipante.Text.Equals(""))
                {
                    rbSi.Checked = false;
                    rbNo.Checked = true;
                }
                else
                {
                    rbSi.Checked = true;
                    rbNo.Checked = false;
                    
                }
               

                string strapemat = Regex.Replace(Server.HtmlDecode(Grd_Participantes.Rows[lRowIndex].Cells[13].Text.ToString()), @"<[^>]+>|&nbsp;", "").Trim();

                txtApeMatParticipante.Text = strapemat;

                txtPersonaId.Text = Grd_Participantes.Rows[lRowIndex].Cells[9].Text.ToString();
                
                string estadoCivil = Regex.Replace(Grd_Participantes.Rows[lRowIndex].Cells[20].Text.ToString(), @"<[^>]+>|&nbsp;", "").Trim();
                if(estadoCivil!= "")
                {
                    CmbEstCiv.SelectedValue = estadoCivil.ToString();
                }

                
                string Genero = Regex.Replace(Grd_Participantes.Rows[lRowIndex].Cells[19].Text.ToString(), @"<[^>]+>|&nbsp;", "").Trim();
                if (Genero != "")
                {
                    ddlGenero_Titular.SelectedValue = Genero.ToString();
                }
                string fecha = Regex.Replace(Grd_Participantes.Rows[lRowIndex].Cells[21].Text.ToString(), @"<[^>]+>|&nbsp;", "").Trim();
                if (fecha != "")
                {
                    CtrldFecNacimientoParticipante.Text = Comun.FormatearFecha(fecha).ToShortDateString();
                }
                
                if (noHTML.Length == 0)
                {
                    txtNroDocParticipante.Text = String.Empty;
                    ddl_TipoDocParticipante.SelectedValue = "0";
                    lblValidacionParticipante.Visible = false;
                }
                else
                {
                    txtNroDocParticipante.Text = NroDni;
                    if (Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.DNI
                                || Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.LIBRETA_MILITAR
                                || Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.CARNET_EXTRANJERIA
                                || Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.CUI)
                    {
                        ddl_TipoDocParticipante.SelectedValue = sTipoDoc;
                    }
                    else
                    {
                        ddl_TipoDocParticipante.SelectedValue = Convert.ToInt16(Enumerador.enmTipoDocumento.OTROS).ToString();
                    }
                }
                string ubigeo = "", direccion ="";

                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (Int32)Enumerador.enmParticipanteDefuncion.TITULAR ||
                    Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (Int32)Enumerador.enmParticipanteMatrimonio.DON ||
                    Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (Int32)Enumerador.enmParticipanteMatrimonio.DONIA)
                {
                    ubigeo = Regex.Replace(Grd_Participantes.Rows[lRowIndex].Cells[22].Text.ToString(), @"<[^>]+>|&nbsp;", "").Trim();
                }
                else {
                    ubigeo = Regex.Replace(Grd_Participantes.Rows[lRowIndex].Cells[14].Text.ToString(), @"<[^>]+>|&nbsp;", "").Trim();

                    HiddenField hDireccion = (HiddenField)Grd_Participantes.Rows[lRowIndex].FindControl("hDireccion");
                    //direccion = Regex.Replace(Grd_Participantes.Rows[lRowIndex].Cells[15].Text.ToString(), @"<[^>]+>|&nbsp;", "").Trim();
                    direccion = hDireccion.Value;
                }

                if (ubigeo != null)
                {
                    if (ubigeo.Length == 6)
                    {
                        if (ubigeo != "000000")
                        {
                            txtDireccionParticipante.Text = direccion;
                            ctrlUbigeo1.setUbigeo(ubigeo);
                        }
                    }
                }
                ViewState["Editar"] = true;
                txtParticipanteID.Text = Grd_Participantes.Rows[lRowIndex].Cells[1].Text;

                string javaScript = "abrirPopupParticipantes();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", javaScript, true);
            }
            if (e.CommandName == "Eliminar")
            {
                Int32 sTipoParticipanteID = Convert.ToInt32(Grd_Participantes.Rows[lRowIndex].Cells[0].Text);
                if (sTipoParticipanteID != (Int32)Enumerador.enmParticipanteNacimiento.TITULAR)
                {
                    if (sTipoParticipanteID == (Int32)Enumerador.enmParticipanteNacimiento.PADRE)
                    {
                        foreach (GridViewRow row in Grd_Participantes.Rows)
                        {
                            Int32 sTipoDato = Convert.ToInt32(row.Cells[17].Text);

                            if (sTipoDato == (int)Enumerador.enmParticipanteTipoDatos.PADRE)
                            {
                                string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "REGISTRO CIVIL", "NO SE PUEDE ELIMINAR EL PADRE - ESTA ASOCIADO A UN DECLARANTE");
                                Comun.EjecutarScript(Page, strScript);
                                return;
                            }
                        }
                    }
                        if (sTipoParticipanteID == (Int32)Enumerador.enmParticipanteNacimiento.MADRE)
                    {
                        foreach (GridViewRow row in Grd_Participantes.Rows)
                        {
                            Int32 sTipoDato = Convert.ToInt32(row.Cells[17].Text);

                            if (sTipoDato == (int)Enumerador.enmParticipanteTipoDatos.MADRE)
                            {
                                string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "REGISTRO CIVIL", "NO SE PUEDE ELIMINAR LA MADRE - ESTA ASOCIADO A UN DECLARANTE");
                                Comun.EjecutarScript(Page, strScript);
                                return;
                            }
                        }
                    }
                    ActuacionMantenimientoBL obj = new ActuacionMantenimientoBL();
                    ActuacionConsultaBL ObjParticipante = new ActuacionConsultaBL();
                    Int64 sParticipante = Convert.ToInt32(Grd_Participantes.Rows[lRowIndex].Cells[16].Text);
                    obj.EliminarParticipante(sParticipante);

                    Int64 lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
                    DataTable dtParticipantes = ObjParticipante.ObtenerParticipantes(lngActuacionDetalleId);

                    this.Grd_Participantes.DataSource = dtParticipantes;
                    this.Grd_Participantes.DataBind();
                    string strScript2 = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "REGISTRO CIVIL", Constantes.CONST_MENSAJE_ELIMINADO);
                    Comun.EjecutarScript(Page, strScript2);
                }
                else
                {
                    string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "REGISTRO CIVIL", "NO SE PUEDE ELIMINAR EL TITULAR - DEBE DE ANULAR EL ACTA");
                    Comun.EjecutarScript(Page, strScript);
                }
                
            }
            //--------------------------------------
            //Fecha: 18/10/2016
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Se adiciono la condicional
            //--------------------------------------
            if (e.CommandName == "Editar")
            {
                ControlesEditarParticipantes();
            }
            
            txtFecNac.EnabledIcon = true;
            txtFecNac.Enabled = true;

            //----------------------------------------------------------
            updFormato.Update();
        }
        private void ControlesEditarParticipantes()
        {
            this.ddl_TipoParticipante.Enabled = false;
            ddl_TipoDatoParticipante.Enabled = false;
            ddl_TipoVinculoParticipante.Enabled = false;
            ddl_TipoDocParticipante.Enabled = false;
            ddl_NacParticipante.Enabled = false;
            txtNroDocParticipante.Enabled = false;
            txtNomParticipante.Enabled = true;
            txtApePatParticipante.Enabled = true;
            txtApeMatParticipante.Enabled = true;
            txtDireccionParticipante.Enabled = true;
            CmbEstCiv.Enabled = true;
            CtrldFecNacimientoParticipante.Enabled = true;
            this.rbSi.Enabled = false;
            this.rbNo.Enabled = false;
            if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteMatrimonio.DON) ||
                    Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteMatrimonio.DONIA))
            {
                if (ddl_NacParticipante.SelectedIndex == 0)
                {
                    ddl_NacParticipante.Enabled = true;
                }
                else
                {
                    ddl_NacParticipante.Enabled = false;
                }
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Llenar_titular_ubicacion_nacimiento();

            ddl_TipoDocParticipante.Enabled = true;
            txtNroDocParticipante.Enabled = true;
            LimpiarDatosParticipante();
            //this.ctrlUbigeo1.HabilitaControl(false);
            ctrlUbigeo1.ClearControl();
            ctrlUbigeo1.UbigeoRefresh();
            HF_ESRune.Value = "0";
            edicion = false;
            edicion_rowindex = -1;

            ddlGenero_Titular.SelectedIndex = 0;
            ddl_NacParticipante.Visible = true;
            Label49.Visible = true;
            Label65.Visible = true;
            if (chkconCUI.Checked)
            {
                txtNroCUI.Enabled = false;
                imgBuscarCUI.Enabled = false;
            }
            txtFecNac.EnabledIcon = true;
            txtFecNac.Enabled = true;
            RegresarFormatoDatosParticipante();
        }
        private bool validarObligatiriedadCamposParticipante()
        {
            bool resultado = true;

            if (ddl_TipoParticipante.SelectedIndex == 0)
            {
                resultado = false;
            }
            if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (Int32)Enumerador.enmTipoActa.DEFUNCION)
            {
                #region VALIDACION DEFUNCION
                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (Int32)Enumerador.enmParticipanteDefuncion.TITULAR)
                {
                    if (ddl_TipoDocParticipante.SelectedIndex == 0)
                    {
                        ddl_TipoDocParticipante.Focus();
                        ddl_TipoDocParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        ddl_TipoDocParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (txtNroDocParticipante.Text.Length == 0)
                    {
                        txtNroDocParticipante.Focus();
                        txtNroDocParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtNroDocParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (txtNomParticipante.Text.Length == 0)
                    {
                        txtNomParticipante.Focus();
                        txtNomParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtNomParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (txtApePatParticipante.Text.Length == 0)
                    {
                        txtApePatParticipante.Focus();
                        txtApePatParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtApePatParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (ddlGenero_Titular.SelectedIndex == 0)
                    {
                        ddlGenero_Titular.Focus();
                        ddlGenero_Titular.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        ddlGenero_Titular.Style.Add("border", "solid #888888 1px");
                    }
                    if (CtrldFecNacimientoParticipante.Value() == DateTime.MinValue)
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTUACIÓN", "Falta ingresar la fecha"));
                        resultado = false;
                    }
                }
                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (Int32)Enumerador.enmParticipanteDefuncion.PADRE ||
                    Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (Int32)Enumerador.enmParticipanteDefuncion.MADRE)
                {
                    if (rbSi.Checked)
                    {
                        if (ddl_TipoDocParticipante.SelectedIndex == 0)
                        {
                            ddl_TipoDocParticipante.Focus();
                            ddl_TipoDocParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            ddl_TipoDocParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        if (txtNroDocParticipante.Text.Length == 0)
                        {
                            txtNroDocParticipante.Focus();
                            txtNroDocParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtNroDocParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        if (txtNomParticipante.Text.Length == 0)
                        {
                            txtNomParticipante.Focus();
                            txtNomParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtNomParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        if (txtApePatParticipante.Text.Length == 0)
                        {
                            txtApePatParticipante.Focus();
                            txtApePatParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtApePatParticipante.Style.Add("border", "solid #888888 1px");
                        }
                    }
                    else
                    {
                        if (txtNomParticipante.Text.Length == 0)
                        {
                            txtNomParticipante.Focus();
                            txtNomParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtNomParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        if (txtApePatParticipante.Text.Length == 0)
                        {
                            txtApePatParticipante.Focus();
                            txtApePatParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtApePatParticipante.Style.Add("border", "solid #888888 1px");
                        }
                    }
                    if (ddl_NacParticipante.SelectedIndex == 0)
                    {
                        ddl_NacParticipante.Focus();
                        ddl_NacParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        ddl_NacParticipante.Style.Add("border", "solid #888888 1px");
                    }
                }
                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (Int32)Enumerador.enmParticipanteDefuncion.REGISTRADOR_CIVIL)
                {
                    if (ddl_TipoDocParticipante.SelectedIndex == 0)
                    {
                        ddl_TipoDocParticipante.Focus();
                        ddl_TipoDocParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        ddl_TipoDocParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (txtNroDocParticipante.Text.Length == 0)
                    {
                        txtNroDocParticipante.Focus();
                        txtNroDocParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtNroDocParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (txtNomParticipante.Text.Length == 0)
                    {
                        txtNomParticipante.Focus();
                        txtNomParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtNomParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (txtApePatParticipante.Text.Length == 0)
                    {
                        txtApePatParticipante.Focus();
                        txtApePatParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtApePatParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (ddl_NacParticipante.SelectedIndex == 0)
                    {
                        ddl_NacParticipante.Focus();
                        ddl_NacParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        ddl_NacParticipante.Style.Add("border", "solid #888888 1px");
                    }
                }
                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (Int32)Enumerador.enmParticipanteDefuncion.DECLARANTE)
                {
                    if (chkInscripcionOficio.Checked == false)
                        {
                        #region validar declarante

                        if (ddl_TipoDatoParticipante.SelectedIndex > 0)
                    {
                        if (txtNomParticipante.Text.Length == 0)
                        {
                            txtNomParticipante.Focus();
                            txtNomParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtNomParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        if (txtApePatParticipante.Text.Length == 0)
                        {
                            txtApePatParticipante.Focus();
                            txtApePatParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtApePatParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        #endregion
                        }
                 }
                    else
                    {
                        if (ddl_TipoVinculoParticipante.SelectedIndex == 0)
                        {
                            ddl_TipoVinculoParticipante.Focus();
                            ddl_TipoVinculoParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            ddl_TipoVinculoParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        if (ddl_TipoDocParticipante.SelectedIndex == 0)
                        {
                            ddl_TipoDocParticipante.Focus();
                            ddl_TipoDocParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            ddl_TipoDocParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        //--------------------------------------------------
                        //Fecha: 29/04/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Validar el Nro. Documento.
                        //--------------------------------------------------
                        if (txtNroDocParticipante.Text.Trim().Length == 0)
                        {
                            txtNroDocParticipante.Focus();
                            txtNroDocParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtNroDocParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        //--------------------------------------------------

                        if (ddl_NacParticipante.SelectedIndex == 0)
                        {
                            ddl_NacParticipante.Focus();
                            ddl_NacParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            ddl_NacParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        if (txtNomParticipante.Text.Length == 0)
                        {
                            txtNomParticipante.Focus();
                            txtNomParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtNomParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        if (txtApePatParticipante.Text.Length == 0)
                        {
                            txtApePatParticipante.Focus();
                            txtApePatParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtApePatParticipante.Style.Add("border", "solid #888888 1px");
                        }
                    }
                }
                #endregion
            }
            if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (Int32)Enumerador.enmTipoActa.MATRIMONIO)
            {
                #region VALIDACION MATRIMONIO
                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (Int32)Enumerador.enmParticipanteMatrimonio.CELEBRANTE)
                {
                    if (rbSi.Checked)
                    {
                        if (ddl_TipoDocParticipante.SelectedIndex == 0)
                        {
                            ddl_TipoDocParticipante.Focus();
                            ddl_TipoDocParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            ddl_TipoDocParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        if (txtNroDocParticipante.Text.Length == 0)
                        {
                            txtNroDocParticipante.Focus();
                            txtNroDocParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtNroDocParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        if (txtNomParticipante.Text.Length == 0)
                        {
                            txtNomParticipante.Focus();
                            txtNomParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtNomParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        if (txtApePatParticipante.Text.Length == 0)
                        {
                            txtApePatParticipante.Focus();
                            txtApePatParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtApePatParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        if (ddl_NacParticipante.SelectedIndex == 0)
                        {
                            ddl_NacParticipante.Focus();
                            ddl_NacParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            ddl_NacParticipante.Style.Add("border", "solid #888888 1px");
                        }
                    }
                    else
                    {
                        if (txtNomParticipante.Text.Length == 0)
                        {
                            txtNomParticipante.Focus();
                            txtNomParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtNomParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        if (txtApePatParticipante.Text.Length == 0)
                        {
                            txtApePatParticipante.Focus();
                            txtApePatParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtApePatParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        if (ddl_NacParticipante.SelectedIndex == 0)
                        {
                            ddl_NacParticipante.Focus();
                            ddl_NacParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            ddl_NacParticipante.Style.Add("border", "solid #888888 1px");
                        }
                    }
                }
                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (Int32)Enumerador.enmParticipanteMatrimonio.DONIA ||
                Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (Int32)Enumerador.enmParticipanteMatrimonio.DON)
                {
                    if (ddl_TipoDocParticipante.SelectedIndex == 0)
                    {
                        ddl_TipoDocParticipante.Focus();
                        ddl_TipoDocParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        ddl_TipoDocParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (txtNroDocParticipante.Text.Length == 0)
                    {
                        txtNroDocParticipante.Focus();
                        txtNroDocParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtNroDocParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (txtNomParticipante.Text.Length == 0)
                    {
                        txtNomParticipante.Focus();
                        txtNomParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtNomParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (txtApePatParticipante.Text.Length == 0)
                    {
                        txtApePatParticipante.Focus();
                        txtApePatParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtApePatParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (ddl_NacParticipante.SelectedIndex == 0)
                    {
                        ddl_NacParticipante.Focus();
                        ddl_NacParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        ddl_NacParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (CmbEstCiv.SelectedIndex == 0)
                    {
                        CmbEstCiv.Focus();
                        CmbEstCiv.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        CmbEstCiv.Style.Add("border", "solid #888888 1px");
                    }
                    if (CtrldFecNacimientoParticipante.Value() == DateTime.MinValue)
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTUACIÓN", "Falta ingresar la fecha"));
                        resultado = false;
                    }
                }
                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (Int32)Enumerador.enmParticipanteMatrimonio.REGISTRADOR_CIVIL)
                
                {
                    if (ddl_TipoDocParticipante.SelectedIndex == 0)
                    {
                        ddl_TipoDocParticipante.Focus();
                        ddl_TipoDocParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        ddl_TipoDocParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (txtNroDocParticipante.Text.Length == 0)
                    {
                        txtNroDocParticipante.Focus();
                        txtNroDocParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtNroDocParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (txtNomParticipante.Text.Length == 0)
                    {
                        txtNomParticipante.Focus();
                        txtNomParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtNomParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (txtApePatParticipante.Text.Length == 0)
                    {
                        txtApePatParticipante.Focus();
                        txtApePatParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtApePatParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (ddl_NacParticipante.SelectedIndex == 0)
                    {
                        ddl_NacParticipante.Focus();
                        ddl_NacParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        ddl_NacParticipante.Style.Add("border", "solid #888888 1px");
                    }
                }
                #endregion
            }
            if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (Int32)Enumerador.enmTipoActa.NACIMIENTO)
            {
                #region VALIDACION NACIMIENTO
                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (Int32)Enumerador.enmParticipanteNacimiento.TITULAR)
                {
                    if (txtNroDocParticipante.Text.Length == 0)
                    {
                        txtNroDocParticipante.Focus();
                        txtNroDocParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtNroDocParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (txtNomParticipante.Text.Length == 0)
                    {
                        txtNomParticipante.Focus();
                        txtNomParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtNomParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (txtApePatParticipante.Text.Length == 0)
                    {
                        txtApePatParticipante.Focus();
                        txtApePatParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtApePatParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (txtApeMatParticipante.Text.Length == 0)
                    {
                        txtApeMatParticipante.Focus();
                        txtApeMatParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtApeMatParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (chksinCUI.Checked)
                    {
                        if (ddlGenero_Titular.SelectedIndex == 0)
                        {
                            ddlGenero_Titular.Focus();
                            ddlGenero_Titular.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            ddlGenero_Titular.Style.Add("border", "solid #888888 1px");
                        }
                    }

                }
                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (Int32)Enumerador.enmParticipanteNacimiento.PADRE ||
                    Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (Int32)Enumerador.enmParticipanteNacimiento.MADRE)
                {
                    if (rbSi.Checked)
                    {
                        if (ddl_TipoDocParticipante.SelectedIndex == 0)
                        {
                            ddl_TipoDocParticipante.Focus();
                            ddl_TipoDocParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            ddl_TipoDocParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        if (txtNroDocParticipante.Text.Length == 0)
                        {
                            txtNroDocParticipante.Focus();
                            txtNroDocParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtNroDocParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        if (txtNomParticipante.Text.Length == 0)
                        {
                            txtNomParticipante.Focus();
                            txtNomParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtNomParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        if (txtApePatParticipante.Text.Length == 0)
                        {
                            txtApePatParticipante.Focus();
                            txtApePatParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtApePatParticipante.Style.Add("border", "solid #888888 1px");
                        }
                    }
                    else
                    {
                        if (txtNomParticipante.Text.Length == 0)
                        {
                            txtNomParticipante.Focus();
                            txtNomParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtNomParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        if (txtApePatParticipante.Text.Length == 0)
                        {
                            txtApePatParticipante.Focus();
                            txtApePatParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtApePatParticipante.Style.Add("border", "solid #888888 1px");
                        }
                    }
                    if (ddl_NacParticipante.SelectedIndex == 0)
                    {
                        ddl_NacParticipante.Focus();
                        ddl_NacParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        ddl_NacParticipante.Style.Add("border", "solid #888888 1px");
                    }
                }
                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (Int32)Enumerador.enmParticipanteNacimiento.REGISTRADOR_CIVIL)
                {
                    if (ddl_TipoDocParticipante.SelectedIndex == 0)
                    {
                        ddl_TipoDocParticipante.Focus();
                        ddl_TipoDocParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        ddl_TipoDocParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (txtNroDocParticipante.Text.Length == 0)
                    {
                        txtNroDocParticipante.Focus();
                        txtNroDocParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtNroDocParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (txtNomParticipante.Text.Length == 0)
                    {
                        txtNomParticipante.Focus();
                        txtNomParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtNomParticipante.Style.Add("border", "solid #888888 1px");
                    }
                    if (txtApePatParticipante.Text.Length == 0)
                    {
                        txtApePatParticipante.Focus();
                        txtApePatParticipante.Style.Add("border", "solid Red 1px");
                        resultado = false;
                    }
                    else
                    {
                        txtApePatParticipante.Style.Add("border", "solid #888888 1px");
                    }
                }
                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (Int32)Enumerador.enmParticipanteNacimiento.DECLARANTE_1 ||
                    Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (Int32)Enumerador.enmParticipanteNacimiento.DECLARANTE_2)
                {
                    if (ddl_TipoDatoParticipante.SelectedIndex > 0)
                    {
                        if (txtNomParticipante.Text.Length == 0)
                        {
                            txtNomParticipante.Focus();
                            txtNomParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtNomParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        if (txtApePatParticipante.Text.Length == 0)
                        {
                            txtApePatParticipante.Focus();
                            txtApePatParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtApePatParticipante.Style.Add("border", "solid #888888 1px");
                        }
                    }
                    else
                    {
                        if (ddl_TipoVinculoParticipante.SelectedIndex == 0)
                        {
                            ddl_TipoVinculoParticipante.Focus();
                            ddl_TipoVinculoParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            ddl_TipoVinculoParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        if (ddl_TipoDocParticipante.SelectedIndex == 0)
                        {
                            ddl_TipoDocParticipante.Focus();
                            ddl_TipoDocParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            ddl_TipoDocParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        //--------------------------------------------------
                        //Fecha: 29/04/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Validar el Nro. Documento.
                        //--------------------------------------------------
                        if (txtNroDocParticipante.Text.Trim().Length == 0)
                        {
                            txtNroDocParticipante.Focus();
                            txtNroDocParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtNroDocParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        //--------------------------------------------------
                        if (ddl_NacParticipante.SelectedIndex == 0)
                        {
                            ddl_NacParticipante.Focus();
                            ddl_NacParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            ddl_NacParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        if (txtNomParticipante.Text.Length == 0)
                        {
                            txtNomParticipante.Focus();
                            txtNomParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtNomParticipante.Style.Add("border", "solid #888888 1px");
                        }
                        if (txtApePatParticipante.Text.Length == 0)
                        {
                            txtApePatParticipante.Focus();
                            txtApePatParticipante.Style.Add("border", "solid Red 1px");
                            resultado = false;
                        }
                        else
                        {
                            txtApePatParticipante.Style.Add("border", "solid #888888 1px");
                        }
                    }
                }
                #endregion
            }

            return resultado;
        }
        private void RegistrarParticipante()
        {

            
            RE_PARTICIPANTE objParticipante = new RE_PARTICIPANTE();
            Int64 lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
            objParticipante.iActuacionDetId = lngActuacionDetalleId;
            objParticipante.vPrimerApellido = txtApePatParticipante.Text.ToUpper();
            objParticipante.vSegundoApellido = txtApeMatParticipante.Text.ToUpper();
            objParticipante.vNombres = txtNomParticipante.Text.ToUpper();
            objParticipante.sNacionalidadId = Convert.ToInt16(ddl_NacParticipante.SelectedValue);
            objParticipante.sTipoParticipanteId = Convert.ToInt16(ddl_TipoParticipante.SelectedValue);
            objParticipante.sTipoDocumentoId = Convert.ToInt16(ddl_TipoDocParticipante.SelectedValue);
            objParticipante.sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            objParticipante.sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            objParticipante.vUbigeo = ctrlUbigeo1.getResidenciaUbigeo();
            objParticipante.sTipoVinculoId = Convert.ToInt16(ddl_TipoVinculoParticipante.SelectedValue);
            if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (Int32)Enumerador.enmTipoActa.DEFUNCION)
            {
                objParticipante.sGeneroId = Convert.ToInt16(ddlGenero_Titular.SelectedValue);
                if (CtrldFecNacimientoParticipante.Text != string.Empty)
                {
                    DateTime datFecha = new DateTime();
                    if (!DateTime.TryParse(CtrldFecNacimientoParticipante.Text, out datFecha))
                    {
                        datFecha = Comun.FormatearFecha(CtrldFecNacimientoParticipante.Text);
                    }
                    objParticipante.pers_dNacimientoFecha = datFecha;
                }
                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteDefuncion.TITULAR))
                {
                    objParticipante.pers_dFechaDefuncion = FechaHora(this.txtFecNac.Text, this.txtHora.Text);
                    objParticipante.pers_cUbigeoDefuncion = this.ddl_DeptOcurrencia.SelectedValue + this.ddl_ProvOcurrencia.SelectedValue + this.ddl_DistOcurrencia.SelectedValue;
                    objParticipante.pers_bFallecidoFlag = true;
                }
                
            }
            else {
                if ((Convert.ToInt32(ddlTipoActa.SelectedValue) == (Int32)Enumerador.enmTipoActa.NACIMIENTO))
                {
                    if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteNacimiento.TITULAR))
                    {
                        if (chkconCUI.Checked)
                        {
                            //---------------------------------------------------
                            //Fecha: 04/03/2021
                            //Autor: Miguel Márquez Beltrán
                            //Motivo: Registrar el genero del titular.
                            //---------------------------------------------------
                            if (ddlGenero_Titular.SelectedIndex > 0)
                            {
                                objParticipante.sGeneroId = Convert.ToInt16(ddlGenero_Titular.SelectedValue);
                            }
                            //---------------------------------------------------
                        }
                        objParticipante.pers_cNacimientoLugar = this.ddl_DeptOcurrencia.SelectedValue + this.ddl_ProvOcurrencia.SelectedValue + this.ddl_DistOcurrencia.SelectedValue;
                        objParticipante.pers_dNacimientoFecha = FechaHora(this.txtFecNac.Text, this.txtHora.Text);

                        //-----------------------------------------
                        // Fecha: 22/03/2022
                        // Autor: Miguel Márquez Beltrán
                        // Motivo: Asignar el país de origen.
                        //-----------------------------------------
                        if (ddl_DeptOcurrencia.SelectedIndex > 0 && ddl_ProvOcurrencia.SelectedIndex > 0)
                        {
                            string strPaisOrigen = "0";
                            strPaisOrigen = Comun.AsignarPaisOrigen(Session, ddl_DeptOcurrencia, ddl_ProvOcurrencia);
                            if (strPaisOrigen != "0")
                            {
                                objParticipante.pers_sPaisId = Convert.ToInt16(strPaisOrigen);
                            }
                        }
                        if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteDefuncion.TITULAR))
                        {
                            if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (Int32)Enumerador.enmTipoActa.NACIMIENTO)
                            {
                                //--------------------------------------------
                                // Fecha: 19/04/2022.
                                // Documento: ECPP-SGAC-18042022.
                                // Autor: Miguel Márquez Beltrán.
                                // Motivo: Asignar el país de Perú.
                                //         para el titular con CUI/sin CUI 
                                //         para el Tipo de Acta Nacimiento. 
                                //--------------------------------------------
                                string strPaisOrigen = "0";
                                strPaisOrigen = System.Web.Configuration.WebConfigurationManager.AppSettings["Pais_PeruId"].ToString();
                                if (strPaisOrigen != "0")
                                {
                                    objParticipante.pers_sPaisId = Convert.ToInt16(strPaisOrigen);
                                }
                                //-----------------------------------------

                            }
                        }
                        //-----------------------------------------
                    }
                }
            }
            if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (Int32)Enumerador.enmTipoActa.MATRIMONIO)
            {
                if (CtrldFecNacimientoParticipante.Text != string.Empty)
                {
                    DateTime datFecha = new DateTime();
                    if (!DateTime.TryParse(CtrldFecNacimientoParticipante.Text, out datFecha))
                    {
                        datFecha = Comun.FormatearFecha(CtrldFecNacimientoParticipante.Text);
                    }
                    objParticipante.pers_dNacimientoFecha = datFecha;
                }
                if (CmbEstCiv.SelectedIndex > 0)
                {
                    objParticipante.pers_sEstadoCivilId = Convert.ToInt32(CmbEstCiv.SelectedValue);
                }
                objParticipante.pers_cNacimientoLugar = ctrlUbigeo1.getResidenciaUbigeo();
            }
            objParticipante.vDireccion = txtDireccionParticipante.Text.ToUpper();
            objParticipante.vNumeroDocumento = txtNroDocParticipante.Text.ToUpper();
            objParticipante.iPersonaId = Convert.ToInt64(txtPersonaId.Text);
                       

            objParticipante.sTipoDatoId = Convert.ToInt16(ddl_TipoDatoParticipante.SelectedValue);
            objParticipante.sTipoVinculoId = Convert.ToInt16(ddl_TipoVinculoParticipante.SelectedValue);
            ActoCivilMantenimientoBL obj = new ActoCivilMantenimientoBL();
            obj.InsertarParticipantes(objParticipante);

            ActuacionConsultaBL ObjParticipante = new ActuacionConsultaBL();

            DataTable dtParticipantes = ObjParticipante.ObtenerParticipantes(lngActuacionDetalleId);

            this.Grd_Participantes.DataSource = dtParticipantes;
            this.Grd_Participantes.DataBind();
            string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "REGISTRO CIVIL", Constantes.CONST_MENSAJE_EXITO);
            Comun.EjecutarScript(Page, strScript);
            updFormato.Update();
        }
        private void ActualizarTitular(Int64 idPersona = 0)
        {
            RE_ACTUACIONPARTICIPANTE objParticipante = new RE_ACTUACIONPARTICIPANTE();
            RE_PERSONAIDENTIFICACION objIdentificacion = new RE_PERSONAIDENTIFICACION();
            RE_PERSONA objPersona = new RE_PERSONA();
            if( Convert.ToInt32(ddlTipoActa.SelectedValue) == (Int32)Enumerador.enmTipoActa.NACIMIENTO)
            {
                Int64 lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
                string strParticipanteId = Grd_Participantes.Rows[0].Cells[1].Text;
                objParticipante.acpa_iActuacionParticipanteId = Convert.ToInt64(strParticipanteId);
                objParticipante.acpa_iActuacionDetalleId = lngActuacionDetalleId;
                objParticipante.acpa_iPersonaId = idPersona;
                objParticipante.acpa_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objParticipante.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                objParticipante.vDirecionParticipante = "";
                objParticipante.vUbigeo = this.ddl_DeptOcurrencia.SelectedValue + this.ddl_ProvOcurrencia.SelectedValue + this.ddl_DistOcurrencia.SelectedValue;
                objParticipante.acpa_sTipoParticipanteId = (Int32)Enumerador.enmParticipanteNacimiento.TITULAR;
                objParticipante.acpa_sTipoDatoId = 0;
                objParticipante.acpa_sTipoVinculoId = 0;

                objIdentificacion.peid_sDocumentoTipoId = (Int16)Enumerador.enmTipoDocumento.CUI; ;
                objIdentificacion.peid_vDocumentoNumero = txtNroCUI.Text;

                objPersona.pers_sNacionalidadId = (int)Enumerador.enmNacionalidad.PERUANA;
                objPersona.pers_vNombres = txtNombresTitular.Text.ToUpper();
                objPersona.pers_vApellidoPaterno = txtApePatTitular.Text.ToUpper();
                objPersona.pers_vApellidoMaterno = txtApeMatTitular.Text.ToUpper();
                objPersona.pers_dNacimientoFecha = FechaHora(this.txtFecNac.Text, this.txtHora.Text);
                objPersona.pers_sGeneroId = Convert.ToInt16(ddl_Genero.SelectedValue);
                objPersona.pers_cNacimientoLugar = this.ddl_DeptOcurrencia.SelectedValue + this.ddl_ProvOcurrencia.SelectedValue + this.ddl_DistOcurrencia.SelectedValue;

                //--------------------------------------------
                // Fecha: 19/04/2022.
                // Documento: ECPP-SGAC-18042022.
                // Autor: Miguel Márquez Beltrán.
                // Motivo: Asignar el país de Perú.
                //         para el titular con CUI/sin CUI 
                //         para el Tipo de Acta Nacimiento. 
                //--------------------------------------------
                string strPaisOrigen = "0";
                strPaisOrigen = System.Web.Configuration.WebConfigurationManager.AppSettings["Pais_PeruId"].ToString();
                if (strPaisOrigen != "0")
                {
                    objPersona.pers_sPaisId = Convert.ToInt16(strPaisOrigen);
                }
                //--------------------------------------------

                ParticipanteMantenimientoBL obj = new ParticipanteMantenimientoBL();
                obj.ActualizarParticipante(objParticipante, objPersona, objIdentificacion);
            }
        }
        private void ActualizarTitularSINCUI(Int64 idPersona = 0)
        {
            RE_ACTUACIONPARTICIPANTE objParticipante = new RE_ACTUACIONPARTICIPANTE();
            RE_PERSONAIDENTIFICACION objIdentificacion = new RE_PERSONAIDENTIFICACION();
            RE_PERSONA objPersona = new RE_PERSONA();
            if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (Int32)Enumerador.enmTipoActa.NACIMIENTO)
            {
                Int64 lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
                string strParticipanteId = txtParticipanteID.Text;
                objParticipante.acpa_iActuacionParticipanteId = Convert.ToInt64(strParticipanteId);
                objParticipante.acpa_iActuacionDetalleId = lngActuacionDetalleId;
                objParticipante.acpa_iPersonaId = idPersona;
                objParticipante.acpa_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objParticipante.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                objParticipante.vDirecionParticipante = "";
                objParticipante.vUbigeo = this.ddl_DeptOcurrencia.SelectedValue + this.ddl_ProvOcurrencia.SelectedValue + this.ddl_DistOcurrencia.SelectedValue;
                objParticipante.acpa_sTipoParticipanteId = (Int32)Enumerador.enmParticipanteNacimiento.TITULAR;
                objParticipante.acpa_sTipoDatoId = 0;
                objParticipante.acpa_sTipoVinculoId = 0;

                objIdentificacion.peid_sDocumentoTipoId = Convert.ToInt16(ddl_TipoDocParticipante.SelectedValue.ToString());
                objIdentificacion.peid_vDocumentoNumero = txtNroDocParticipante.Text;

                objPersona.pers_sNacionalidadId = (int)Enumerador.enmNacionalidad.PERUANA;
                objPersona.pers_vNombres = txtNomParticipante.Text.ToUpper();
                objPersona.pers_vApellidoPaterno = txtApePatParticipante.Text.ToUpper();
                objPersona.pers_vApellidoMaterno = txtApeMatParticipante.Text.ToUpper();
                objPersona.pers_dNacimientoFecha = FechaHora(this.txtFecNac.Text, this.txtHora.Text);
                //---------------------------------------------------
                //Fecha: 04/03/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Registrar el genero del titular.
                //---------------------------------------------------
                //if (ddl_Genero.SelectedIndex > 0)
                if (ddlGenero_Titular.SelectedIndex > 0)
                {
                    objPersona.pers_sGeneroId = Convert.ToInt16(ddlGenero_Titular.SelectedValue);
                }
                //---------------------------------------------------
                objPersona.pers_cNacimientoLugar = this.ddl_DeptOcurrencia.SelectedValue + this.ddl_ProvOcurrencia.SelectedValue + this.ddl_DistOcurrencia.SelectedValue;

                //--------------------------------------------
                // Fecha: 19/04/2022.
                // Documento: ECPP-SGAC-18042022.
                // Autor: Miguel Márquez Beltrán.
                // Motivo: Asignar el país de Perú.
                //         para el titular con CUI/sin CUI 
                //         para el Tipo de Acta Nacimiento. 
                //--------------------------------------------
                string strPaisOrigen = "0";
                strPaisOrigen = System.Web.Configuration.WebConfigurationManager.AppSettings["Pais_PeruId"].ToString();
                if (strPaisOrigen != "0")
                {
                    objPersona.pers_sPaisId = Convert.ToInt16(strPaisOrigen);
                }
                //-----------------------------------------
                ParticipanteMantenimientoBL obj = new ParticipanteMantenimientoBL();
                obj.ActualizarParticipante(objParticipante, objPersona, objIdentificacion);

                txtApeMatTitular.Text = txtApeMatParticipante.Text.ToUpper();
                txtApePatTitular.Text = txtApePatParticipante.Text.ToUpper();
                txtNombresTitular.Text = txtNomParticipante.Text.ToUpper();

                ActuacionConsultaBL ObjParticipante = new ActuacionConsultaBL();
                DataTable dtParticipantes = ObjParticipante.ObtenerParticipantes(lngActuacionDetalleId);

                this.Grd_Participantes.DataSource = dtParticipantes;
                this.Grd_Participantes.DataBind();
                string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "REGISTRO CIVIL", Constantes.CONST_MENSAJE_EXITO);
                Comun.EjecutarScript(Page, strScript);
                updFormato.Update();
            }
        }
        private void ActualizarParticipante(Int64 idPersona = 0)
        {
            RE_ACTUACIONPARTICIPANTE objParticipante = new RE_ACTUACIONPARTICIPANTE();
            RE_PERSONAIDENTIFICACION objIdentificacion = new RE_PERSONAIDENTIFICACION();
            RE_PERSONA objPersona = new RE_PERSONA();

            Int64 lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
            string strParticipanteId = txtParticipanteID.Text;
            objParticipante.acpa_iActuacionParticipanteId = Convert.ToInt64(strParticipanteId);
            objParticipante.acpa_iActuacionDetalleId = lngActuacionDetalleId;
            objParticipante.acpa_iPersonaId = idPersona;
            objParticipante.acpa_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            objParticipante.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            objParticipante.vDirecionParticipante = txtDireccionParticipante.Text.ToUpper();
            objParticipante.vUbigeo = ctrlUbigeo1.getResidenciaUbigeo();
            objParticipante.acpa_sTipoParticipanteId = Convert.ToInt16(ddl_TipoParticipante.SelectedValue);
            objParticipante.acpa_sTipoDatoId = Convert.ToInt16(ddl_TipoDatoParticipante.SelectedValue);
            objParticipante.acpa_sTipoVinculoId = Convert.ToInt16(ddl_TipoVinculoParticipante.SelectedValue);

            objIdentificacion.peid_sDocumentoTipoId = Convert.ToInt16(ddl_TipoDocParticipante.SelectedValue.ToString());
            objIdentificacion.peid_vDocumentoNumero = txtNroDocParticipante.Text.ToUpper();

            objPersona.pers_sNacionalidadId = Convert.ToInt16(ddl_NacParticipante.SelectedValue);
            objPersona.pers_vNombres = txtNomParticipante.Text.ToUpper();
            objPersona.pers_vApellidoPaterno = txtApePatParticipante.Text.ToUpper();
            objPersona.pers_vApellidoMaterno = txtApeMatParticipante.Text.ToUpper();

            if (CmbEstCiv.SelectedIndex > 0)
            {
                objPersona.pers_sEstadoCivilId = Convert.ToInt16(CmbEstCiv.SelectedValue);
            }
            if (ddl_Genero.SelectedIndex > 0)
            {
                objPersona.pers_sGeneroId = Convert.ToInt16(ddl_Genero.SelectedValue);
            }
            //objPersona.pers_cNacimientoLugar = this.ddl_DeptOcurrencia.SelectedValue + this.ddl_ProvOcurrencia.SelectedValue + this.ddl_DistOcurrencia.SelectedValue;

            if (CtrldFecNacimientoParticipante.Text != string.Empty)
            {
                DateTime datFecha = new DateTime();
                if (!DateTime.TryParse(CtrldFecNacimientoParticipante.Text, out datFecha))
                {
                    datFecha = Comun.FormatearFecha(CtrldFecNacimientoParticipante.Text);
                }
                objPersona.pers_dNacimientoFecha = datFecha;
            }
            if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (Int32)Enumerador.enmTipoActa.NACIMIENTO)
            {
                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteNacimiento.TITULAR))
                {
                    //--------------------------------------------
                    // Fecha: 19/04/2022.
                    // Documento: ECPP-SGAC-18042022.
                    // Autor: Miguel Márquez Beltrán.
                    // Motivo: Asignar el país de Perú.
                    //         para el titular con CUI/sin CUI 
                    //         para el Tipo de Acta Nacimiento. 
                    //--------------------------------------------
                    string strPaisOrigen = "0";
                    strPaisOrigen = System.Web.Configuration.WebConfigurationManager.AppSettings["Pais_PeruId"].ToString();
                    if (strPaisOrigen != "0")
                    {
                        objPersona.pers_sPaisId = Convert.ToInt16(strPaisOrigen);
                    }
                    //-----------------------------------------
                }
            }
            if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (Int32)Enumerador.enmTipoActa.DEFUNCION)
            {
                
                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteDefuncion.TITULAR))
                {
                    objPersona.pers_dFechaDefuncion = FechaHora(this.txtFecNac.Text, this.txtHora.Text);
                    objPersona.pers_cUbigeoDefuncion = this.ddl_DeptOcurrencia.SelectedValue + this.ddl_ProvOcurrencia.SelectedValue + this.ddl_DistOcurrencia.SelectedValue;
                    objPersona.pers_bFallecidoFlag = true;
                    objPersona.pers_cNacimientoLugar = ctrlUbigeo1.getResidenciaUbigeo();
                }

            }
            
                ParticipanteMantenimientoBL obj = new ParticipanteMantenimientoBL();
                obj.ActualizarParticipante(objParticipante, objPersona, objIdentificacion);

                
                ActuacionConsultaBL ObjParticipante = new ActuacionConsultaBL();
                DataTable dtParticipantes = ObjParticipante.ObtenerParticipantes(lngActuacionDetalleId);

                this.Grd_Participantes.DataSource = dtParticipantes;
                this.Grd_Participantes.DataBind();
                string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "REGISTRO CIVIL", Constantes.CONST_MENSAJE_EXITO);
                Comun.EjecutarScript(Page, strScript);
                updFormato.Update();
            
        }
        private void RegistrarTitular()
        {
            RE_PARTICIPANTE objParticipante = new RE_PARTICIPANTE();
            if( Convert.ToInt32(ddlTipoActa.SelectedValue) == (Int32)Enumerador.enmTipoActa.NACIMIENTO)
            {
                Int64 lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
                if(txtPersonaId.Text.Length>1)
                {
                    objParticipante.iPersonaId = Convert.ToInt64(txtPersonaId.Text);
                }
                objParticipante.iActuacionDetId = lngActuacionDetalleId;
                objParticipante.vPrimerApellido = txtApePatTitular.Text.ToUpper();
                objParticipante.vSegundoApellido = txtApeMatTitular.Text.ToUpper();
                objParticipante.vNombres = txtNombresTitular.Text.ToUpper();
                objParticipante.sNacionalidadId = (int)Enumerador.enmNacionalidad.PERUANA;
                objParticipante.sTipoParticipanteId = (Int32)Enumerador.enmParticipanteNacimiento.TITULAR;
                objParticipante.sTipoDocumentoId = (Int16)Enumerador.enmTipoDocumento.CUI;
                objParticipante.sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                objParticipante.sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                //objParticipante.vUbigeo = this.ddl_DeptOcurrencia.SelectedValue + this.ddl_ProvOcurrencia.SelectedValue + this.ddl_DistOcurrencia.SelectedValue;
                objParticipante.pers_dNacimientoFecha = FechaHora(this.txtFecNac.Text, this.txtHora.Text);
                objParticipante.sGeneroId = Convert.ToInt16(ddl_Genero.SelectedValue);
                objParticipante.vDireccion = "";
                objParticipante.vNumeroDocumento = txtNroCUI.Text;
                objParticipante.pers_cNacimientoLugar = this.ddl_DeptOcurrencia.SelectedValue + this.ddl_ProvOcurrencia.SelectedValue + this.ddl_DistOcurrencia.SelectedValue;

                //--------------------------------------------
                // Fecha: 19/04/2022.
                // Documento: ECPP-SGAC-18042022.
                // Autor: Miguel Márquez Beltrán.
                // Motivo: Asignar el país de Perú.
                //         para el titular con CUI/sin CUI 
                //         para el Tipo de Acta Nacimiento. 
                //--------------------------------------------
                string strPaisOrigen = "0";
                strPaisOrigen = System.Web.Configuration.WebConfigurationManager.AppSettings["Pais_PeruId"].ToString();
                if (strPaisOrigen != "0")
                {
                    objParticipante.pers_sPaisId = Convert.ToInt16(strPaisOrigen);
                }
                //-----------------------------------------

                ActoCivilMantenimientoBL obj = new ActoCivilMantenimientoBL();
                obj.InsertarParticipantes(objParticipante);
                hTitularId.Value = objParticipante.iPersonaId.ToString();
            }
        }

        
        private string ValidarRegistroParticipante()
        {
            ActuacionConsultaBL ObjParticipante = new ActuacionConsultaBL();
            RE_PARTICIPANTE objParticipante = new RE_PARTICIPANTE();
            Int64 lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
            objParticipante.iActuacionDetId = lngActuacionDetalleId;
            objParticipante.sTipoParticipanteId = Convert.ToInt16(ddl_TipoParticipante.SelectedValue);
            DataTable dt = new DataTable();
            if (txtParticipanteID.Text != "0")
            {
                dt = ObjParticipante.VerificarRegistroParticipantes(lngActuacionDetalleId, objParticipante.sTipoParticipanteId,Convert.ToInt64(txtPersonaId.Text));
            }
            else {
                 dt = ObjParticipante.VerificarRegistroParticipantes(lngActuacionDetalleId, objParticipante.sTipoParticipanteId,0);
            }
            //ViewState["ParticipanteID"] = null;
            string resultado = dt.Rows[0]["RESULTADO"].ToString();
            return resultado;
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddl_NacParticipante.SelectedIndex == 0)
                {
                    ddl_NacParticipante.Enabled = true;
                }
                else
                {
                    ddl_NacParticipante.Enabled = false;
                }
                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteMatrimonio.DON) ||
                    Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteMatrimonio.DONIA))
                {
                    if (Convert.ToInt16(CmbEstCiv.SelectedValue) == (Int16)Enumerador.enmEstadoCivil.CASADO)
                    {
                        string strScript2 = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION,
                                                        "PARTICIPANTE", "NO SE PUEDE AGREGAR UN PARTICIPANTE DON / DOÑA CON ESTADO CIVIL CASADO");
                        Comun.EjecutarScript(Page, strScript2);
                        return;
                    }
                }
                string resultado = ValidarRegistroParticipante();

                if (resultado.Length > 0)
                {
                    Int64 lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
                    ActuacionConsultaBL ObjParticipante = new ActuacionConsultaBL();
                    DataTable dtParticipantes = ObjParticipante.ObtenerParticipantes(lngActuacionDetalleId);
                    this.Grd_Participantes.DataSource = dtParticipantes;
                    this.Grd_Participantes.DataBind();
                    string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION,
                        "PARTICIPANTE", resultado);
                    Comun.EjecutarScript(Page, strScript);
                    updVinculacion.Update();
                    return;
                }
                bool ActualizaTitularSinCUI = false;
                if (validarObligatiriedadCamposParticipante())
                {
                    if (ViewState["Editar"] == null)
                    {
                        ViewState["Editar"] = false;
                    }
                    if (Convert.ToBoolean(ViewState["Editar"]) == true)
                    {
                        if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteNacimiento.TITULAR))
                        {
                            if (chksinCUI.Checked)
                            {
                                string valorTitular = txtPersonaId.Text;
                                ActualizarTitularSINCUI(Convert.ToInt64(valorTitular));
                                ActualizaTitularSinCUI = true;
                            }
                        }
                    }
                    
                    if (ActualizaTitularSinCUI == false)
                    {
                        if (Convert.ToBoolean(ViewState["Editar"]) == true)
                        {
                            string valorTitular = txtPersonaId.Text;
                            ActualizarParticipante(Convert.ToInt64(valorTitular));
                        }
                        else {
                            RegistrarParticipante();
                            if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteDefuncion.TITULAR))
                            {
                                ddl_Genero.Enabled = false;
                                ddl_Genero.SelectedValue = ddlGenero_Titular.SelectedValue;
                            }
                        }
                    }
                    ViewState["Editar"] = false;
                    btnCancelar_Click(sender, e);
                    //----------------------------------------------------------
                    //Fecha: 18/06/2019
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Activar el botón Vista Previa de 
                    //acuerdo a la validación del número de participantes.
                    //----------------------------------------------------------
                    BtnVistaPrevia.Enabled = ValidarParticipantesRegistroCivil();
                    cbxAfirmarTexto.Enabled = BtnVistaPrevia.Enabled;
                    btnActa.Enabled = BtnVistaPrevia.Enabled;
                    ddl_TipoParticipante.Enabled = true;
                    updVinculacion.Update();
                    txtParticipanteID.Text = "0";
                }
                else {
                    if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteNacimiento.DECLARANTE_1)
                            || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteNacimiento.DECLARANTE_2))
                    {
                        if (txtNroDocParticipante.Text.Length == 0)
                        {
                            string strScript2 = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION,
                                                    "PARTICIPANTE", "DEBE INGRESAR EL NRO. DOCUMENTO");
                            Comun.EjecutarScript(Page, strScript2);
                            return;
                        }
                    }
                    else {
                        string strScript2 = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION,
                                                        "PARTICIPANTE", "COMPLETE LOS DATOS OBLIGATORIOS");
                        Comun.EjecutarScript(Page, strScript2);
                        return;
                    }
                }
                
                //----------------------------------------------------------
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

        public void Llenar_titular_ubicacion_nacimiento()
        {
            if (Session["Participante"] != null && Convert.ToInt32(ddlTipoActa.SelectedValue) == (Int32)Enumerador.enmTipoActa.NACIMIENTO)
            {
                int contador = 0;
                List<RE_PARTICIPANTE> loparticipante = (List<RE_PARTICIPANTE>)Session["Participante"];
                foreach (RE_PARTICIPANTE item in (loparticipante.Where(p => (Int32)p.sTipoParticipanteId == (Int32)Enumerador.enmParticipanteNacimiento.TITULAR && p.cEstado != "E")))
                {
                    contador++;
                }
                if (contador>0)
                {
                    //-----------------------------------------------
                    //Autor: Miguel Márquez Beltrán
                    //Fecha: 03/10/2016
                    //Objetivo: Activar Lugar de ocurrencia
                    //-----------------------------------------------
                 
                    //desactivar_ubiOcurrencia(false);
                }
                else
                {
                    desactivar_ubiOcurrencia(true);
                }
            }
        }

        public void llenar_ubigeo(string LugarNacimiento)
        {
            if (LugarNacimiento.Length == 6)
            {
                this.ddl_DeptOcurrencia.SelectedValue = LugarNacimiento.Substring(0, 2);

                //----------------------------------------------------
                //Fecha: 03/04/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Cargar la provincia
                //----------------------------------------------------
                Comun.CargarUbigeo(Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_DeptOcurrencia.SelectedValue, "", "", true, "--SELECCIONAR--", "", Enumerador.enmNacionalidad.NINGUNA, ddl_ProvOcurrencia);
                //----------------------------------------------------

                //Comun.CargarUbigeo(Session, ddl_ProvOcurrencia, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_DeptOcurrencia.SelectedValue, string.Empty, true);

                if (LugarNacimiento.Substring(2, 2) == "00")
                {
                    this.ddl_ProvOcurrencia.SelectedIndex = 0;
                    this.ddl_DistOcurrencia.SelectedIndex = 0;
                }
                else
                {
                    this.ddl_ProvOcurrencia.SelectedValue = LugarNacimiento.Substring(2, 2);
                    //----------------------------------------------------
                    //Fecha: 03/04/2017
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Cargar el Distrito
                    //----------------------------------------------------
                    Comun.CargarUbigeo(Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddl_DeptOcurrencia.SelectedValue, ddl_ProvOcurrencia.SelectedValue, "", true, "--SELECCIONAR--", "", Enumerador.enmNacionalidad.NINGUNA, ddl_DistOcurrencia);
                    //----------------------------------------------------                        
                    this.ddl_DistOcurrencia.SelectedValue = LugarNacimiento.Substring(4, 2);
                }
                
            }

        }

        public void desactivar_ubiOcurrencia(Boolean activar) {
            ddl_DeptOcurrencia.Enabled = activar;
            ddl_ProvOcurrencia.Enabled = activar;
            ddl_DistOcurrencia.Enabled = activar;
        }


        protected void imgBuscar_Click(object sender, ImageClickEventArgs e)
        {
            string loFechaNacimiento = this.txtFecNac.Text;
            #region Buscar persona
            EnPersona objEn = new EnPersona();

            if (ddl_TipoDocParticipante.SelectedIndex == 0)
            {
                string strScript2 = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION,
                                        "PARTICIPANTE", "DEBE INGRESAR EL TIPO DE DOCUMENTO");
                Comun.EjecutarScript(Page, strScript2);
                return;
            }
            objEn.iPersonaId = 0;
            objEn.sDocumentoTipoId = Convert.ToInt32(ddl_TipoDocParticipante.SelectedValue);
            if (ddl_TipoDatoParticipante.SelectedItem != null)
            {
                objEn.vDocumentoTipo = ddl_TipoDatoParticipante.SelectedItem.Text;
            }
            objEn.vDocumentoNumero = txtNroDocParticipante.Text.ToUpper();
            if (ddl_TipoVinculoParticipante.SelectedIndex > 0)
            {
                objEn.sTipoVinculoId = Convert.ToInt32(ddl_TipoVinculoParticipante.SelectedValue);
            }
            if (ddl_TipoDatoParticipante.SelectedIndex > 0)
            {
                objEn.sTipoDatoId = Convert.ToInt32(ddl_TipoDatoParticipante.SelectedValue);
            }
            object[] arrParametros = { objEn };
            objEn = SGAC.WebApp.Accesorios.Persona.oPersona(arrParametros);
            #endregion

            #region Pintar Datos Persona
            string strScript = string.Empty;
            txtPersonaId.Text = "0";
            if (objEn != null)
            {
                txtPersonaId.Text = objEn.iPersonaId.ToString();

                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteNacimiento.TITULAR))
                {
                    if (chksinCUI.Checked)
                    {
                        ddl_NacParticipante.SelectedValue = Convert.ToInt16(Enumerador.enmNacionalidad.PERUANA).ToString();
                        ddlGenero_Titular.SelectedValue = objEn.sGeneroId.ToString();
                    }
                    else
                    {
                        ddl_NacParticipante.SelectedValue = objEn.sNacionalidadId.ToString();
                    }
                }
                else{
                    ddl_NacParticipante.SelectedValue = objEn.sNacionalidadId.ToString();
                }

                if (Convert.ToInt16(ddl_TipoDocParticipante.SelectedValue) == Convert.ToInt16(Enumerador.enmTipoDocumento.DNI))
                {
                    ddl_NacParticipante.SelectedValue = Convert.ToInt16(Enumerador.enmNacionalidad.PERUANA).ToString();
                }

                txtNomParticipante.Text = objEn.vNombres;
                txtApePatParticipante.Text = objEn.vApellidoPaterno;
                txtApeMatParticipante.Text = objEn.vApellidoMaterno;
                txtDireccionParticipante.Text = objEn.vDireccion;
                this.txtFecNac.Text = loFechaNacimiento;
                if (objEn.vNombres != string.Empty && objEn.vNombres != null)
                {
                    txtNomParticipante.Enabled = false;
                    txtApeMatParticipante.Enabled = false;
                    txtApePatParticipante.Enabled = false;
                }
                else
                {
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION,
                        "PARTICIPANTE", "El número de documento no esta registrado en el sistema.");
                    Comun.EjecutarScript(Page, strScript);
                    txtNomParticipante.Enabled = true;
                    txtApeMatParticipante.Enabled = true;
                    txtApePatParticipante.Enabled = true;
                    ddlGenero_Titular.Enabled = true;
                    //if (Convert.ToInt32(this.ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.NACIMIENTO)
                    //{
                    //    if (chkconCUI.Checked)
                    //    {
                    //        ddl_Genero.Enabled = true;
                    //    }
                    //}
                }
                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteDefuncion.TITULAR))
                {
                    ddlGenero_Titular.SelectedValue = objEn.sGeneroId.ToString();
                }
                if (objEn.dFecNacimiento == null || objEn.dFecNacimiento =="0")
                { 
                    CtrldFecNacimientoParticipante.Text = String.Empty;
                    CtrldFecNacimientoParticipante.Enabled = true;
                }
                else {
                    CtrldFecNacimientoParticipante.Text = Comun.FormatearFecha(objEn.dFecNacimiento.ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"]); 
                    DateTime datFecha = new DateTime();
                    if (!DateTime.TryParse(txtFecNac.Text, out datFecha))
                    {
                        datFecha = Comun.FormatearFecha(txtFecNac.Text);
                    }
                    DateTime datFecha2 = new DateTime();
                    if (!DateTime.TryParse(objEn.dFecNacimiento, out datFecha))
                    {
                        datFecha2 = Comun.FormatearFecha(objEn.dFecNacimiento);
                    }
                    CtrldFecNacimientoParticipante.Enabled = false;
                    LblEdad2.Text = ObtenerEdad(datFecha2, datFecha);
                    
                }
                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteMatrimonio.DON) ||
                    Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteMatrimonio.DONIA))
                {
                    ddlGenero_Titular.SelectedValue = objEn.sGeneroId.ToString();
                }
                //if (txtApeMatParticipante.Text != string.Empty)
                //{
                //    txtApeMatParticipante.Enabled = true;
                //}
                string HFDep = objEn.iDptoContId.ToString(),
                    HFProv = objEn.iProvPaisId.ToString(),
                    HFdist = objEn.iDistCiuId.ToString();

                if (objEn.sEstadoCivilId != null)
                {
                    CmbEstCiv.SelectedValue = objEn.sEstadoCivilId.ToString();
                }
                
                if (HFDep.Length == 1)
                {
                    HFDep = "0" + HFDep;
                }
                if (HFProv.Length == 1)
                {
                    HFProv = "0" + HFProv;
                }
                if (HFdist.Length == 1)
                {
                    HFdist = "0" + HFdist;
                }

                string ubigeo = HFDep + HFProv + HFdist;

                if (lblUbigeoParticipantes.Text == "LUGAR DE NACIMIENTO")
                {
                    if (objEn.cNacimientoLugar != null)
                    {
                        ubigeo = objEn.cNacimientoLugar;
                        if (ubigeo.Length > 0)
                        {
                            ctrlUbigeo1.setUbigeo(objEn.cNacimientoLugar);
                        }
                    }
                }
                else {
                    ctrlUbigeo1.setUbigeo(ubigeo);
                }
                

                //ctrlUbigeo1.setDepartamentoId(objEn.iDptoContId.ToString());
                //ctrlUbigeo1.setCiudadId(objEn.iDptoContId.ToString(), objEn.iProvPaisId.ToString());
                //ctrlUbigeo1.setDistrito(objEn.iDptoContId.ToString(), objEn.iProvPaisId.ToString(), objEn.iDistCiuId.ToString());
            }

            if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteMatrimonio.DON) ||
                    Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteMatrimonio.DONIA))
            {
                if (ddl_NacParticipante.SelectedIndex == 0)
                {
                    ddl_NacParticipante.Enabled = true;
                }
                else
                {
                    ddl_NacParticipante.Enabled = false;
                }
            }

            if (ddl_NacParticipante.SelectedIndex == 0)
            {
                ddl_NacParticipante.Enabled = true;
            }
            else
            {
                ddl_NacParticipante.Enabled = false;
            }
            #endregion
        }

        private void LimpiarDatosParticipante()
        {
            this.ddl_TipoParticipante.SelectedIndex = 0;
            if (this.ddl_TipoDatoParticipante.SelectedIndex != -1) { this.ddl_TipoDatoParticipante.SelectedIndex = 0; };
            this.ddl_TipoVinculoParticipante.SelectedIndex = 0;
            this.ddl_TipoDocParticipante.SelectedIndex = 0;
            this.ddl_NacParticipante.SelectedIndex = 0;
            this.ddl_NacParticipante.Enabled = false;
            this.txtNroDocParticipante.Text = string.Empty;
            this.txtNomParticipante.Text = string.Empty;
            this.txtNomParticipante.Enabled = false;
            this.txtApePatParticipante.Text = string.Empty;
            this.txtApePatParticipante.Enabled = false;
            this.txtApeMatParticipante.Text = string.Empty;
            this.txtApeMatParticipante.Enabled = false;
            this.txtDireccionParticipante.Text = string.Empty;
            //this.txtDireccionParticipante.Enabled = false;
            this.txtPersonaId.Text = string.Empty;
            this.txtPersonaId.Text = "0";
            this.LblEdad2.Text = String.Empty;
            this.CmbEstCiv.SelectedValue = "0";
            this.CtrldFecNacimientoParticipante.Text = String.Empty;

            this.LblEdad2.Visible = false;
            this.CmbEstCiv.Visible = false;
            this.CtrldFecNacimientoParticipante.Visible = false;
            lblObligaFecNacimientoParticipante.Visible = false;
            this.LblEdad.Visible = false;
            this.BtnCalcularEded.Visible = false;
            this.lbldFecNacParticipante.Visible = false;
            this.lblEstadoCivil.Visible = false;
            this.lbltienendocumento.Visible = false;
            this.rbSi.Visible = false;
            this.rbNo.Visible = false;

            this.ctrlUbigeo1.ClearControl();
            //this.ctrlUbigeo1.HabilitaControl(false);

            updFormato.Update();
        }


        private void LimpiarDatosParticipanteRune()
        {

            this.ddl_TipoDatoParticipante.SelectedIndex = 0;
            this.ddl_TipoDatoParticipante.Enabled = false;
            this.ddl_TipoVinculoParticipante.SelectedIndex = 0;
            this.ddl_TipoDocParticipante.SelectedIndex = 0;
            this.ddl_NacParticipante.SelectedIndex = 0;
            this.ddl_NacParticipante.Enabled = false;
            this.txtNroDocParticipante.Text = string.Empty;
            this.txtNomParticipante.Text = string.Empty;
            this.txtNomParticipante.Enabled = false;
            this.txtApePatParticipante.Text = string.Empty;
            this.txtApePatParticipante.Enabled = false;
            this.txtApeMatParticipante.Text = string.Empty;
            this.txtApeMatParticipante.Enabled = false;
            this.txtDireccionParticipante.Text = string.Empty;
            //this.txtDireccionParticipante.Enabled = false;
            this.txtPersonaId.Text = string.Empty;

            this.LblEdad2.Text = String.Empty;
            this.CmbEstCiv.SelectedValue = "0";
            this.CtrldFecNacimientoParticipante.Text = String.Empty;

            this.LblEdad2.Visible = false;
            this.CmbEstCiv.Visible = false;
            this.CtrldFecNacimientoParticipante.Visible = false;
            lblObligaFecNacimientoParticipante.Visible = false;
            this.BtnCalcularEded.Visible = false;
            this.lbldFecNacParticipante.Visible = false;
            this.lblEstadoCivil.Visible = false;
            this.txtPersonaId.Text = "0";
            this.rbSi.Checked = true;
            this.ctrlUbigeo1.ClearControl();

            updFormato.Update();
        }

        private DataTable mtParticipanteContainerToTable(string strTipoParticipanteId)
        {
            DataTable dt = CrearTablaParticipante();
            int ItemRow = 1;

            //Tomando de la variable de SESSION
            List<BE.RE_PARTICIPANTE> loParticipanteContainer = (List<BE.RE_PARTICIPANTE>)Session["Participante"];
            //
            #region Creando DataTable
            foreach (RE_PARTICIPANTE item in loParticipanteContainer.Where(p => p.cEstado != "E")) //
            {
                // ERROR 2015
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

                string Stipo_doc = "";
                if (item.sTipoDocumentoId == (Int16)Enumerador.enmTipoDocumento.CUI || item.sTipoDocumentoId == (Int16)Enumerador.enmTipoDocumento.DNI || item.sTipoDocumentoId == (Int16)Enumerador.enmTipoDocumento.CARNET_EXTRANJERIA || item.sTipoDocumentoId == (Int16)Enumerador.enmTipoDocumento.LIBRETA_MILITAR)
                {
                    Stipo_doc = item.vTipoDocumento;
                }
                else if (item.sTipoDocumentoId.ToString().Trim() == string.Empty || item.sTipoDocumentoId <= 0 )
                {
                    Stipo_doc = " ";            
                }
                else
                {
                    Stipo_doc = Enumerador.enmTipoDocumento.OTROS.ToString();                    
                }
                dr["vDocumentoCompleto"] = Stipo_doc + " - " + item.vNumeroDocumento.ToString();
                dr["sNacionalidadId"] = item.sNacionalidadId;
                dr["vResidenciaDireccion"] = item.vDireccion;
                dr["cResidenciaUbigeo"] = item.vUbigeo;
                dr["ICentroPobladoId"] = item.ICentroPobladoId;
                dr["cEstado"] = item.cEstado;
                dr["vNombreCompleto"] = item.vPrimerApellido + " " + item.vSegundoApellido + "," + item.vNombres;

                dt.Rows.Add(dr);
            }
            //--------------------------------------------------------------
            // Autor: Miguel Márquez Beltrán
            // Fecha: 23/09/2016
            // Objetivo: Actualizar los datos del participante vinculado 
            //          por el tipo y número de documento.
            //---------------------------------------------------------------            
            string strsDocumentoTipoId = "";
            string strDocumentoNumero = "";
            string strApellidoPaterno = "";
            string strApellidoMaterno = "";
            string strNombres = "";
            string strNacionalidadId = "";
            string strResidenciaDireccion = "";
            string strResidenciaUbigeo = "";
            string strCentroPobladoId = "";
            string strNombreCompleto = "";
            bool bexiste = false;

            if (dt.Rows.Count > 1)
            {
                if (strTipoParticipanteId.Trim().Length > 0)
                {                                        
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["sTipoParticipanteId"].ToString() == strTipoParticipanteId)
                        {
                            strApellidoPaterno = dt.Rows[i]["vApellidoPaterno"].ToString();
                            strApellidoMaterno = dt.Rows[i]["vApellidoMaterno"].ToString();
                            strNombres = dt.Rows[i]["vNombres"].ToString();
                            strNacionalidadId = dt.Rows[i]["sNacionalidadId"].ToString();
                            strResidenciaDireccion = dt.Rows[i]["vResidenciaDireccion"].ToString();
                            strResidenciaUbigeo = dt.Rows[i]["cResidenciaUbigeo"].ToString();
                            strCentroPobladoId = dt.Rows[i]["ICentroPobladoId"].ToString();
                            strNombreCompleto = dt.Rows[i]["vNombreCompleto"].ToString();
                            strsDocumentoTipoId = dt.Rows[i]["sDocumentoTipoId"].ToString();
                            strDocumentoNumero = dt.Rows[i]["vDocumentoNumero"].ToString();
                            bexiste = true;
                            break;
                        }
                    }
                    if (bexiste)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (strDocumentoNumero.Length > 0 && dt.Rows[i]["sDocumentoTipoId"].ToString() == strsDocumentoTipoId && dt.Rows[i]["vDocumentoNumero"].ToString() == strDocumentoNumero)                               
                            {
                                dt.Rows[i]["vApellidoPaterno"] = strApellidoPaterno;
                                dt.Rows[i]["vApellidoMaterno"] = strApellidoMaterno;
                                dt.Rows[i]["vNombres"] = strNombres;
                                dt.Rows[i]["sNacionalidadId"] = strNacionalidadId;
                                dt.Rows[i]["vResidenciaDireccion"] = strResidenciaDireccion;
                                dt.Rows[i]["cResidenciaUbigeo"] = strResidenciaUbigeo;
                                dt.Rows[i]["ICentroPobladoId"] = strCentroPobladoId;
                                dt.Rows[i]["vNombreCompleto"] = strNombreCompleto;
                            }
                        }
                    }
                }
            }
            if (bexiste)
            {
                //---------------------------------------------------------
                // Fecha: 18/10/2016
                // Autor: Miguel Márquez Beltrán
                // Objetivo: Realizar la busqueda solo si tiene documento
                //---------------------------------------------------------
                if (strDocumentoNumero.Length > 0)
                {
                    //---------------------------------------------------------
                    foreach (RE_PARTICIPANTE item in loParticipanteContainer.Where(p => p.sTipoDocumentoId == Convert.ToInt16(strsDocumentoTipoId) && p.vNumeroDocumento == strDocumentoNumero)) //
                    {
                        item.vPrimerApellido = strApellidoPaterno;
                        item.vSegundoApellido = strApellidoMaterno;
                        item.vNombres = strNombres;
                        item.sNacionalidadId = Convert.ToInt32(strNacionalidadId);
                        item.vDireccion = strResidenciaDireccion;
                        item.vUbigeo = strResidenciaUbigeo;
                        item.ICentroPobladoId = Convert.ToInt32(strCentroPobladoId);
                        item.vNombresCompletos = strNombreCompleto;
                    }
                }
                Session["Participante"] = (List<BE.RE_PARTICIPANTE>)loParticipanteContainer;
            }
            ////--------------------------------------
            ////Fecha: 18/10/2016
            ////Autor: Miguel Márquez Beltrán
            ////--------------------------------------

            //Session["Participante"] = (List<BE.RE_PARTICIPANTE>)loParticipanteContainer;
            ////---------------------------------------------------------------
            #endregion
            return dt;
        }

        private DataTable mtParticipanteContainerToTable_Aux()
        {
            DataTable dt = CrearTablaParticipante();
            int ItemRow = 1;

            //Tomando de la variable de SESSION
            List<BE.RE_PARTICIPANTE> loParticipanteContainer = (List<BE.RE_PARTICIPANTE>)Session["ParticipanteAux"];
            //
            #region Creando DataTable
            foreach (RE_PARTICIPANTE item in loParticipanteContainer.Where(p => p.cEstado != "E")) //
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
            this.btnAceptar.OnClientClick = "return ActoCivil_Participantes()";
            DataTable dtTipDoc = new DataTable();
            DataTable dtTipDocAux = new DataTable();
            dtTipDoc = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.DOCUMENTO_IDENTIDAD);


            ddl_TipoDocParticipante.Items.Clear();

            //var dtAux = dtTipDoc.AsEnumerable().Where(x =>
            //        x["id"].ToString() == Convert.ToInt32(Enumerador.enmTipoDocumento.DNI).ToString() ||
            //        x["id"].ToString() == Convert.ToInt32(Enumerador.enmTipoDocumento.LIBRETA_MILITAR).ToString() ||
            //        x["id"].ToString() == Convert.ToInt32(Enumerador.enmTipoDocumento.CARNET_EXTRANJERIA).ToString() ||
            //        x["id"].ToString() == Convert.ToInt32(Enumerador.enmTipoDocumento.OTROS).ToString());

            //dtTipDocAux = dtAux.CopyToDataTable();

            DataView dv = dtTipDoc.DefaultView;
            dv.RowFilter = " Valor in ('DNI','LM/BOL','CE','OTROS', 'CUI')";
            DataTable dtOrdenado = dv.ToTable();
            dtOrdenado.DefaultView.Sort = "Id ASC";


            //Util.CargarParametroDropDownList(ddl_TipoDocParticipante, dtAux.CopyToDataTable(), true);

            Util.CargarDropDownList(ddl_TipoDocParticipante, dtOrdenado, "Valor", "Id", true);

            int lDatoParticipante = Convert.ToInt32(Session["TIPO_ACTO_PARTICIPANTE"]);
            if (lDatoParticipante == (int)Enumerador.enmTipoActa.NACIMIENTO)
            {
                this.ddl_TipoDocParticipante.Items.Add(new System.Web.UI.WebControls.ListItem(Constantes.CONST_EXCEPCION_CUI, Convert.ToString(Constantes.CONST_EXCEPCION_CUI_ID)));
                
                DataTable dtTipoParticipante = new DataTable();

                dtTipoParticipante = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_CIVIL_PARTICIPANTE_NACIMIENTO);
                dtTipoParticipante.DefaultView.Sort = "id ASC";

                Util.CargarParametroDropDownList(ddl_TipoParticipante, dtTipoParticipante, true);
                ddl_TipoDatoParticipante.Visible = true;
                lblPartTipoVinc.Visible = true;
            }
            else if (lDatoParticipante == (int)Enumerador.enmTipoActa.MATRIMONIO)
            {
                DataTable dtTipoParticipante = new DataTable();
                dtTipoParticipante = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_CIVIL_PARITICPANTE_MATRIMONIO);
                dtTipoParticipante.DefaultView.Sort = "id ASC";

                Util.CargarParametroDropDownList(ddl_TipoParticipante, dtTipoParticipante, true);
                ddl_TipoDatoParticipante.Visible = false;
                lblPartTipoVinc.Visible = false;
            }
            else if (lDatoParticipante == (int)Enumerador.enmTipoActa.DEFUNCION)
            {
                DataTable dtTipoParticipante = new DataTable();
                dtTipoParticipante = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_CIVIL_PARTICIPANTE_DEFUNCION);
                dtTipoParticipante.DefaultView.Sort = "id ASC";

                Util.CargarParametroDropDownList(ddl_TipoParticipante, dtTipoParticipante, true);
                ddl_TipoDatoParticipante.Visible = true;
                lblPartTipoVinc.Visible = true;
            }
            DataTable dtTipoDatoParticipante = new DataTable();
            dtTipoDatoParticipante = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTO_CIVIL_PARTICIPANTE_TIPO_DATO);
            dtTipoDatoParticipante.DefaultView.Sort = "id ASC"; 

            Util.CargarParametroDropDownList(ddl_TipoDatoParticipante, dtTipoDatoParticipante, true);
            //------------------------------------
            DataTable dtTipoVinculoParticipante = new DataTable();
            dtTipoVinculoParticipante = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_TIPO_VINCULO);
            dtTipoVinculoParticipante.DefaultView.Sort = "id ASC";

            Util.CargarParametroDropDownList(ddl_TipoVinculoParticipante, dtTipoVinculoParticipante, true);
            //---------------------------------------
            DataTable dtNacParticipante = new DataTable();
            dtNacParticipante = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_NACIONALIDAD);
            dtNacParticipante.DefaultView.Sort = "id ASC"; 

            Util.CargarParametroDropDownList(ddl_NacParticipante,dtNacParticipante , true);
            //-----------------------------------------
            ctrlUbigeo1.UbigeoRefresh();
        }

        private bool ExistTipoParticipante(Int16 tp)
        {
            List<BE.RE_PARTICIPANTE> loParticipanteContainer = (List<BE.RE_PARTICIPANTE>)Session["Participante"];
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

        


        [System.Web.Services.WebMethod]
        public static string GetPersonExist(Int32 tipo, string documento, string strGUID)
        {
            #region Buscar persona
            EnPersona objEn = new EnPersona();
            ActoCivilConsultaBL oActoCivilConsultaBL = new ActoCivilConsultaBL();
            Int32 Rsta = 0;
            objEn.iPersonaId = 0;
            //objEn.vDocumentoTipo = tipo;
            objEn.sDocumentoTipoId = tipo;
            objEn.vDocumentoNumero = documento;
            objEn.sResidenciaTipoId = 2252;
            object[] arrParametros = { objEn };
            objEn = SGAC.WebApp.Accesorios.Persona.oPersona(arrParametros);

            
            //HttpContext.Current.Session["iPersonaId" + strGUID] = objEn.iPersonaId;
            
            
            #endregion

            string person = new JavaScriptSerializer().Serialize(objEn);

            return person;
        }


        #endregion

        protected void btnBuscarParticipante_Click(object sender, EventArgs e)
        {
            EnPersona objEn = new EnPersona();
            ActoCivilConsultaBL oActoCivilConsultaBL = new ActoCivilConsultaBL();
            Int32 Rsta = 0;
            objEn.iPersonaId = 0;
            //objEn.vDocumentoTipo = tipo;
            objEn.sDocumentoTipoId = Convert.ToInt32( ddl_TipoDocParticipante.SelectedValue);
            objEn.vDocumentoNumero = txtNroDocParticipante.Text;
            objEn.sResidenciaTipoId = 2252;
            object[] arrParametros = { objEn };
            objEn = SGAC.WebApp.Accesorios.Persona.oPersona(arrParametros);

            HF_ESRune.Value = "1";
            string Ubigeo = string.Empty;
            String LugarNacimiento = String.Empty;
            string sNacionalidadId = objEn.sNacionalidadId.ToString(); 
            string dFechaNacimiento = objEn.pers_dNacimientoFechaCorta.ToString(); 

            string HFDep,HFProv,HFdist;
            HFDep = objEn.iDptoContId.ToString();
            HFProv = objEn.iProvPaisId.ToString();
            HFdist = objEn.iDptoContId.ToString();

            if (HFDep.Length == 1)
            {
                HFDep = "0" + HFDep;
            }
            if (HFProv.Length == 1)
            {
                HFProv = "0" + HFProv;
            }
            if (HFdist.Length == 1)
            {
                HFdist = "0" + HFdist;
            }
            Ubigeo = HFDep + HFProv + HFdist;

            if (txtPersonaId.Text == "0")
            {
                ddl_NacParticipante.Enabled = true;
                txtNomParticipante.Enabled = true;
                txtApePatParticipante.Enabled = true;
                txtApeMatParticipante.Enabled = true;

                ddl_NacParticipante.SelectedIndex = 0;
                txtNomParticipante.Text = String.Empty;

                txtApePatParticipante.Text = String.Empty;
                txtApeMatParticipante.Text = String.Empty;

                txtDireccionParticipante.Text = String.Empty;

                ctrlUbigeo1.UbigeoRefresh();

            }
            else
            {

                HF_ESRune.Value = "1";

                ddl_NacParticipante.Enabled = false;
                txtNomParticipante.Enabled = false;
                txtApePatParticipante.Enabled = false;
                if (txtApeMatParticipante.Text != string.Empty)
                    txtApeMatParticipante.Enabled = false;

            }
            LugarNacimiento = objEn.cNacimientoLugar;

            if (Convert.ToInt32(Session["TIPO_ACTO_PARTICIPANTE"]) == (int)Enumerador.enmTipoActa.NACIMIENTO)
            {

                if (dFechaNacimiento.Trim().Length != 0 && dFechaNacimiento != "0")
                {
                    this.txtFecNac.Text = Comun.FormatearFecha(dFechaNacimiento).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                }
                else
                { txtFecNac.Enabled = true; txtHora.Enabled = true; }

                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteNacimiento.TITULAR))
                {

                    // Para este participante se cargara el lugar de nacimiento y no de residencia
                    if (LugarNacimiento.Length == 6)
                    {
                        if (LugarNacimiento != "000000")
                        {

                                this.ddl_DeptOcurrencia.SelectedValue = LugarNacimiento.Substring(0, 2);

                                //----------------------------------------------------
                                //Fecha: 03/04/2017
                                //Autor: Miguel Márquez Beltrán
                                //Objetivo: Cargar la provincia
                                //----------------------------------------------------
                                Comun.CargarUbigeo(Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_DeptOcurrencia.SelectedValue, "", "", true, "--SELECCIONAR--", "", Enumerador.enmNacionalidad.NINGUNA, ddl_ProvOcurrencia);
                                //----------------------------------------------------

                                //Comun.CargarUbigeo(Session, ddl_ProvOcurrencia, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_DeptOcurrencia.SelectedValue, string.Empty, true);
                                if (LugarNacimiento.Substring(2, 2).Equals("00"))
                                {
                                    this.ddl_ProvOcurrencia.SelectedIndex = 0;
                                }
                                else
                                {
                                    this.ddl_ProvOcurrencia.SelectedValue = LugarNacimiento.Substring(2, 2);
                                }
                                //----------------------------------------------------
                                //Fecha: 03/04/2017
                                //Autor: Miguel Márquez Beltrán
                                //Objetivo: Cargar el Distrito
                                //----------------------------------------------------

                                if (this.ddl_ProvOcurrencia.SelectedIndex > 0)
                                {
                                    Comun.CargarUbigeo(Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddl_DeptOcurrencia.SelectedValue, ddl_ProvOcurrencia.SelectedValue, "", true, "--SELECCIONAR--", "", Enumerador.enmNacionalidad.NINGUNA, ddl_DistOcurrencia);
                                    //----------------------------------------------------    
                                    //Comun.CargarUbigeo(Session, ddl_DistOcurrencia, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddl_DeptOcurrencia.SelectedValue, ddl_ProvOcurrencia.SelectedValue, true);
                                    if (LugarNacimiento.Substring(4, 2).Equals("00"))
                                    {
                                        this.ddl_DistOcurrencia.SelectedIndex = 0;
                                    }
                                    else
                                    {
                                        this.ddl_DistOcurrencia.SelectedValue = LugarNacimiento.Substring(4, 2);
                                    }
                                }
                                else
                                {
                                    this.ddl_DistOcurrencia.SelectedIndex = 0;
                                }
                            //}
                        }
                        else
                        { 
                            ctrlUbigeo1.ClearControl(); 
                            ctrlUbigeo1.UbigeoRefresh(); 
                        }
                    }
                

                    //--------------------------------------------------------------
                    //Autor: Miguel Márquez Beltrán
                    //Fecha: 05/10/2016
                    //Objetivo: Permitir editar la fecha de nacimiento y el genero
                    //--------------------------------------------------------------                   

                    if (objEn.dFecNacimiento == null)
                    { txtFecNac.Text = String.Empty; }


                    if (txtApeMatParticipante.Text == string.Empty)
                    { txtApeMatParticipante.Enabled = true; }
                    else
                    { txtApeMatParticipante.Enabled = false; }

                }
                else
                {                    
                    if (Ubigeo.Length == 6)
                    {
                        if (Ubigeo != "000000")
                        {
                            ctrlUbigeo1.setUbigeo(Ubigeo);
                        }
                        else
                        {

                            ctrlUbigeo1.ClearControl();
                            ctrlUbigeo1.UbigeoRefresh();
                        }
                    }
           
                    if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteNacimiento.REGISTRADOR_CIVIL))
                    {
                        ddl_NacParticipante.Enabled = false;

                    }
                    if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteNacimiento.DECLARANTE_1) || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteNacimiento.DECLARANTE_2))
                    {
                        if (ddl_TipoDatoParticipante.SelectedValue == "0")
                        {
                            ddl_TipoVinculoParticipante.Enabled = true;
                            ddl_TipoDatoParticipante.Enabled = false;
                        }
                        else
                        {
                            ddl_TipoVinculoParticipante.Enabled = false;
                            ddl_TipoDatoParticipante.Enabled = true;
                        }
                        if (ddl_TipoVinculoParticipante.SelectedValue == "0" && ddl_TipoDatoParticipante.SelectedValue == "0")
                        {
                            ddl_TipoVinculoParticipante.Enabled = true;
                            ddl_TipoDatoParticipante.Enabled = true;
                        }
                    }


                }
                txtFecNac.Enabled = true;
                txtHora.Enabled = true;
            }

            if (Convert.ToInt32(Session["TIPO_ACTO_PARTICIPANTE"]) == (int)Enumerador.enmTipoActa.MATRIMONIO)
            {
                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteMatrimonio.DON) || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteMatrimonio.DONIA))
                {
                    // Para este participantes se cargara el lugar de nacimiento y no de recidencia
                    if (LugarNacimiento.Length == 6)
                    {
                        if (LugarNacimiento != "000000")
                        {
                            ctrlUbigeo1.setUbigeo(LugarNacimiento);
                        }
                        else
                        {
                            ctrlUbigeo1.ClearControl();
                            ctrlUbigeo1.UbigeoRefresh();
                        }
                    }


                    if (CmbEstCiv.SelectedValue != "0" && Convert.ToInt16(CmbEstCiv.SelectedValue) != (Int16)Enumerador.enmEstadoCivil.CASADO)
                    { CmbEstCiv.Enabled = false; }
                    else
                    {
                        CmbEstCiv.SelectedValue = "0";
                        CmbEstCiv.Enabled = true;
                    }
                }
                else
                {
                    if (Ubigeo.Length == 6)
                    {
                        if (Ubigeo != "000000")
                        {
                            ctrlUbigeo1.setUbigeo(Ubigeo);
                        }
                        else
                        {
                            ctrlUbigeo1.ClearControl();
                            ctrlUbigeo1.UbigeoRefresh();

                        }
                    }

                }
                ddl_NacParticipante.Enabled = true;
            }

            if (Convert.ToInt32(Session["TIPO_ACTO_PARTICIPANTE"]) == (int)Enumerador.enmTipoActa.DEFUNCION)
            {
                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteDefuncion.TITULAR))
                {
                    // Para este participante se cargara el lugar de nacimiento y no de recidencia
                    if (LugarNacimiento.Length == 6)
                    {
                        if (LugarNacimiento != "000000")
                        {
                            ctrlUbigeo1.setUbigeo(LugarNacimiento);
                        }
                        else
                        {
                            ctrlUbigeo1.ClearControl();
                            ctrlUbigeo1.UbigeoRefresh();

                        }
                    }

                }
                else
                {
                    if (Ubigeo.Length == 6)
                    {
                        if (Ubigeo != "000000")
                        {
                            ctrlUbigeo1.setUbigeo(Ubigeo);
                        }
                        else
                        {
                            ctrlUbigeo1.ClearControl();
                            ctrlUbigeo1.UbigeoRefresh();
                        }
                    }

                }

                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteDefuncion.DECLARANTE)) {
                    if (ddl_TipoDatoParticipante.SelectedValue == "0")
                    {
                        ddl_TipoVinculoParticipante.Enabled = true;
                        ddl_TipoDatoParticipante.Enabled = false;
                    }
                    else
                    {
                        ddl_TipoVinculoParticipante.Enabled = false;
                        ddl_TipoDatoParticipante.Enabled = true;
                    }
                    if (ddl_TipoVinculoParticipante.SelectedValue == "0" && ddl_TipoDatoParticipante.SelectedValue == "0")
                    {
                        ddl_TipoVinculoParticipante.Enabled = true;
                        ddl_TipoDatoParticipante.Enabled = true;
                    }
                }

                ddl_NacParticipante.Enabled = true;
            }


            if (sNacionalidadId == "0")
                ddl_NacParticipante.Enabled = true;
            else
                ddl_NacParticipante.Enabled = false;

            updFormato.Update();
            updRegPago.Update();

        }

        protected void Tramite_Click(object sender, EventArgs e)
        {
            try
            {
                //if (HFGUID.Value.Length > 0)
                //{
                //    Session["iPersonaId" + HFGUID.Value] = Session["iCodPersonaId" + HFGUID.Value];
                //}
                //else
                //{
                    //ViewState["iPersonaId"] = Session["iCodPersonaId"];
                //}

                Session.Remove("Participante");

                string codPersona = Request.QueryString["CodPer"].ToString();
                //if (HFGUID.Value.Length > 0)
                //{
                //    Response.Redirect("~/Registro/FrmTramite.aspx?GUID=" + HFGUID.Value,false);
                //}
                //else
                //{
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

        protected void ctrlAdjunto_Click(object sender, EventArgs e)
        {
            Cargar_Actuacion();
            #region Tags
            String scriptMover = String.Empty;
            if (txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_3A.ToString())
            {
                scriptMover = @"$(function(){{EnableTabIndex(0);EnableTabIndex(1);DisableTabIndex(2); DisableTabIndex(3);DisableTabIndex(4); MoveTabIndex(2);}});";

                if (Convert.ToBoolean(Session["ACT_DIGITALIZA"]) == true)
                {
                    btnAnotacion.Visible = true;
                    btnCopiaCert.Visible = true;
                    HFAutodhesivo.Value = "1";
                    BtnVistaPrevia.Enabled = false;
                    updbotones.Update();
                    updRegPago.Update();
                    updFormato.Update();

                }

            }
            else
            {

                if (txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_ID_2.ToString())
                {
                    //scriptMover = @"$(function(){{ EnableTabIndex(3) MoveTabIndex(4); DisableTabIndex(1);}});";
                    scriptMover = @"$(function(){{EnableTabIndex(0);DisableTabIndex(1);EnableTabIndex(2); EnableTabIndex(3);EnableTabIndex(4); MoveTabIndex(2);}});";
                }
                else
                {
                    if (txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_ID_1.ToString())
                    {
                        scriptMover = @"$(function(){{EnableTabIndex(0);EnableTabIndex(1);EnableTabIndex(2); DisableTabIndex(3);EnableTabIndex(4); MoveTabIndex(2);}});";

                        if (Convert.ToBoolean(Session["ACT_DIGITALIZA"]) == true)
                        {
                            btnAnotacion.Visible = true;
                            btnCopiaCert.Visible = true;
                            HFAutodhesivo.Value = "1";
                            BtnVistaPrevia.Enabled = false;
                            updbotones.Update();
                            updRegPago.Update();
                            updFormato.Update();
                        }

                    }
                    else
                    {
                        scriptMover = @"$(function(){{EnableTabIndex(0);DisableTabIndex(1);EnableTabIndex(2); DisableTabIndex(3);EnableTabIndex(4); MoveTabIndex(2);}});";
                    }
                }

            }

            if (hdn_ImpresionCorrecta.Value == "1")
            {
                chkImpresion.Enabled = false;
            }
            scriptMover = string.Format(scriptMover);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);
            updVinculacion.Update();
            updActuacionAdjuntar.Update();
            #endregion

        }

        private void HabilitarTag()
        {

            #region Tags
            String scriptMover = String.Empty;
            if (Grd_ActInsDet.Rows.Count > 0)
            {
                if (txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_3A.ToString())
                {
                    scriptMover = @"$(function(){{EnableTabIndex(0);EnableTabIndex(1);DisableTabIndex(2); DisableTabIndex(3);EnableTabIndex(4); MoveTabIndex(4);}});";
                }
                else
                    if (txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_ID_1.ToString())
                    {
                        scriptMover = @"$(function(){{EnableTabIndex(0);EnableTabIndex(1);EnableTabIndex(2); DisableTabIndex(3);EnableTabIndex(4); MoveTabIndex(4);}});";
                    }
                    else if (txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_ID_2.ToString())
                    {
                        scriptMover = @"$(function(){{EnableTabIndex(0);DisableTabIndex(1);EnableTabIndex(2); EnableTabIndex(3);EnableTabIndex(4); MoveTabIndex(4);}});";
                    }
                    else
                    {
                        scriptMover = @"$(function(){{EnableTabIndex(0);EnableTabIndex(1);EnableTabIndex(2); EnableTabIndex(3);EnableTabIndex(4); MoveTabIndex(3);}});";
                    }                                
            }
            else {
                if (txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_3A.ToString())
                {
                    scriptMover = @"$(function(){{EnableTabIndex(0);EnableTabIndex(1);DisableTabIndex(2); DisableTabIndex(3);DisableTabIndex(4); MoveTabIndex(1);}});";

                }
                else
                {
                    if (txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_ID_1.ToString())
                    {
                        string valor = Convert.ToString(Request.QueryString["cod"]);
                        if (valor == "3") //Baja
                        {
                            //String scriptMover = String.Empty;
                            scriptMover = @"$(function(){{EnableTabIndex(0);EnableTabIndex(1);EnableTabIndex(2); DisableTabIndex(3);EnableTabIndex(4); MoveTabIndex(4);}});";
                        }
                        else {
                            scriptMover = @"$(function(){{EnableTabIndex(0);EnableTabIndex(1);EnableTabIndex(2); DisableTabIndex(3);EnableTabIndex(4); MoveTabIndex(1);}});";
                        }                        
                    }

                    else if (txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_ID_2.ToString())
                    {
                        scriptMover = @"$(function(){{EnableTabIndex(0);DisableTabIndex(1);EnableTabIndex(2); EnableTabIndex(3);EnableTabIndex(4); MoveTabIndex(3);}});";
                    }
                    else
                    {
                        scriptMover = @"$(function(){{EnableTabIndex(0);DisableTabIndex(1);EnableTabIndex(2); DisableTabIndex(3);EnableTabIndex(4); MoveTabIndex(4);}});";
                    }
                }
            }
            


            scriptMover = string.Format(scriptMover);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);
            updActuacionAdjuntar.Update();
            #endregion

        }

        /// <summary>
        /// En el Evento Load Cargar los Siguiente:
        /// Combo de Notacion
        /// Util.CargarParametroDropDownList(cmb_TipoAnotacion, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTUACION_TIPO_ANOTACION), true);
        /// 
        /// Tambien los Evento de JavaScript al Ejecutar un evento Click fuera del IsPostBack
        /// BtnGrabAnotacion.OnClientClick = "return ValidarRegistroAnotacion();";
        /// Asegurarse que Funcion JavaScript Exista en tu formulario
        /// 
        /// <%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="ctrlPageBar" TagPrefix="uc2" %> 
        /// 
        /// </summary>
        public void Load()
        {

            String CaracterEspecial1 = String.Empty;
            CaracterEspecial1 = ConfigurationManager.AppSettings["ValidarText"].ToString();
            HF_Validar_Caracteres.Value = CaracterEspecial1;

            //HF_ACTA_NACIMIENTO.Value = Convert.ToString((int)Enumerador.enmTipoActa.NACIMIENTO);
            //HF_ACTA_MATRIMONIO.Value = Convert.ToString((int)Enumerador.enmTipoActa.MATRIMONIO);
            //HF_ACTA_DEFUNCION.Value = Convert.ToString((int)Enumerador.enmTipoActa.DEFUNCION);

            //HF_PARTICIPANTE_MADRE.Value = Convert.ToString((int)Enumerador.enmParticipanteNacimiento.MADRE);
            //HF_PARTICIPANTE_PADRE.Value = Convert.ToString((int)Enumerador.enmParticipanteNacimiento.PADRE);

            //HF_PARTICIPANTE_DON.Value = Convert.ToString((int)Enumerador.enmParticipanteMatrimonio.DON);
            //HF_PARTICIPANTE_DONIA.Value = Convert.ToString((int)Enumerador.enmParticipanteMatrimonio.DONIA);

            //HF_PARTICIPANTE_DECLARANTE1.Value = Convert.ToString((int)Enumerador.enmParticipanteNacimiento.DECLARANTE_1);
            //HF_PARTICIPANTE_DECLARANTE2.Value = Convert.ToString((int)Enumerador.enmParticipanteNacimiento.DECLARANTE_2);


            HF_PARTICIPANTE_RECURRENTE_MATRIMONIO.Value = Convert.ToString((int)Enumerador.enmParticipanteMatrimonio.RECURRENTE);
            HF_PARTICIPANTE_RECURRENTE_NACIMIENTO.Value = Convert.ToString((int)Enumerador.enmParticipanteNacimiento.RECURRENTE);
            HF_PARTICIPANTE_RECURRENTE_DEFUNCION.Value = Convert.ToString((int)Enumerador.enmParticipanteDefuncion.RECURRENTE);

            //HF_PARTICIPANTE_NACIMIENTO_TITULAR.Value = Convert.ToString((int)Enumerador.enmParticipanteNacimiento.TITULAR);
            //HF_PARTICIPANTE_REGISTRADOR_NACIMIENTO.Value = Convert.ToString((int)Enumerador.enmParticipanteNacimiento.REGISTRADOR_CIVIL);

            //HF_PARTICIPANTE_REGISTRADOR_MATRIMONIO.Value = Convert.ToString((int)Enumerador.enmParticipanteMatrimonio.REGISTRADOR_CIVIL);
            HF_PARTICIPANTE_CELEBRANTE_MATRIMONIO.Value = Convert.ToString((int)Enumerador.enmParticipanteMatrimonio.CELEBRANTE);

            HF_PARTICIPANTE_DECLARANTE_DEFUNCION.Value = Convert.ToString((int)Enumerador.enmParticipanteDefuncion.DECLARANTE);
            HF_PARTICIPANTE_MADRE_DEFUNCION.Value = Convert.ToString((int)Enumerador.enmParticipanteDefuncion.MADRE);
            HF_PARTICIPANTE_PADRE_DEFUNCION.Value = Convert.ToString((int)Enumerador.enmParticipanteDefuncion.PADRE);
            HF_PARTICIPANTE_REGISTRADORCIVIL_DEFUNCION.Value = Convert.ToString((int)Enumerador.enmParticipanteDefuncion.REGISTRADOR_CIVIL);
            //HF_PARTICIPANTE_TITULAR_DEFUNCION.Value = Convert.ToString((int)Enumerador.enmParticipanteDefuncion.TITULAR);

            //HF_NACIONALIDAD_EXTRANJERA.Value = Convert.ToString((int)Enumerador.enmNacionalidad.EXTRANJERA);
            HF_NACIONALIDAD_PERUANA.Value = Convert.ToString((int)Enumerador.enmNacionalidad.PERUANA);
            HF_PAGADO_EN_LIMA.Value = Convert.ToString((int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA);

            //HF_TipoDoc_DNI.Value = Convert.ToString((int)Enumerador.enmTipoDocumento.DNI);
            //HF_TipoDoc_CE.Value = Convert.ToString((int)Enumerador.enmTipoDocumento.CARNET_EXTRANJERIA);
            //HF_TipoDoc_LM.Value = Convert.ToString((int)Enumerador.enmTipoDocumento.LIBRETA_MILITAR);
            //HF_TipoDoc_CUI.Value = Convert.ToString((int)Enumerador.enmTipoDocumento.CUI);

            HF_TARIFA_1.Value = Constantes.CONST_EXCEPCION_TARIFA_ID_1.ToString();
            HF_TARIFA_2.Value = Constantes.CONST_EXCEPCION_TARIFA_ID_2.ToString();
            HF_TARIFA_3.Value = Constantes.CONST_EXCEPCION_TARIFA_ID_3.ToString();
            HF_TARIFA_4.Value = Constantes.CONST_EXCEPCION_TARIFA_ID_4.ToString();

            hdn_CONFORMIDAD_DE_TEXTO.Value = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.AVISOS, Convert.ToInt32(Enumerador.enmNotarialAvisos.CONFORMIDAD_DE_TEXTO), "valor");
            cbxAfirmarTexto.Text = hdn_CONFORMIDAD_DE_TEXTO.Value;

            //HF_estado_casado.Value = Convert.ToInt16(Enumerador.enmEstadoCivil.CASADO).ToString();

            HF_TIENE_DOCUMENTO.Value = "1";

            HF_ESRune.Value = "0";

            HFModoVistaAutoadhesivo.Value = ConfigurationManager.AppSettings["ModoVistaAutoadhesivo"].ToString();
            buscarExisteCUI();

            //----------------------------------------------
        }

        protected void ddl_TipoParticipante_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_NacParticipante.Visible = true;// validación del ticket 354
            Label49.Visible = true; //validación del ticket 354
            Label65.Visible = true;//validación del ticket 354


            rbSi.Checked = true;
            rbNo.Checked = false;

            lblEstadoCivil.Enabled = false;
            CmbEstCiv.Enabled = false;
            lbldFecNacParticipante.Enabled = false;
            CtrldFecNacimientoParticipante.Enabled = false;
            lblEstadoCivil.Visible = false;
            CmbEstCiv.Visible = false;
            lbldFecNacParticipante.Visible = false;
            CtrldFecNacimientoParticipante.Visible = false;
            lblObligaFecNacimientoParticipante.Visible = false;
            CmbEstCiv.SelectedValue = "0";
            CtrldFecNacimientoParticipante.Text = String.Empty;

            lbltienendocumento.Visible = false;
            rbSi.Visible = false;
            rbNo.Visible = false;

            LblEdad2.Visible = false;
            LblEdad.Visible = false;
            Label446.Visible = false;
            BtnCalcularEded.Visible = false;
            if (Convert.ToInt32(this.ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.DEFUNCION)
            {
                if (ddl_TipoParticipante.SelectedItem.Text == "TITULAR")
                {
                    lblGenero_Titular.Visible = true;
                    ddlGenero_Titular.Visible = true;
                    lblObligatorioGenero.Visible = true;
                }
                else {
                    lblGenero_Titular.Visible = false;
                    ddlGenero_Titular.Visible = false;
                    lblObligatorioGenero.Visible = false;
                }
            }
            else {
                lblGenero_Titular.Visible = false;
                ddlGenero_Titular.Visible = false;
            }
            if (ddl_TipoParticipante.SelectedValue != "0")
            {
                int iIndiceComboTipoVinculoParticipante = Util.ObtenerIndiceComboPorText(ddl_TipoVinculoParticipante, "EL TITULAR");

                if (iIndiceComboTipoVinculoParticipante >= 0)
                {
                    ddl_TipoVinculoParticipante.Items[iIndiceComboTipoVinculoParticipante].Enabled = false;
                }
                LimpiarDatosParticipanteRune();
                if (Convert.ToInt32(this.ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.NACIMIENTO)
                {

                    if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteNacimiento.TITULAR)
                    {
                        ddl_NacParticipante.SelectedValue = Convert.ToInt16(Enumerador.enmNacionalidad.PERUANA).ToString();
                        ddl_NacParticipante.Enabled = false;
                        lblUbigeoParticipantes.Text = "LUGAR DE NACIMIENTO";
                        txtDireccionParticipante.Visible = false;
                        lblDireccionParticipante.Visible = false;
                        ddl_TipoDatoParticipante.Enabled = false;
                        ddl_TipoVinculoParticipante.Enabled = false;
                        ddl_TipoVinculoParticipante.SelectedValue = "0";
                        Label446.Visible = true;

                        Activar_Ubicacion(false);
                        int iIndiceComboCui = Util.ObtenerIndiceCombo(ddl_TipoDocParticipante, Convert.ToInt16(Enumerador.enmTipoDocumento.CUI).ToString());
                        if (chkconCUI.Checked)
                        {
                            if (iIndiceComboCui >= 0)
                            {
                                ddl_TipoDocParticipante.Items[iIndiceComboCui].Enabled = true;
                                ddl_TipoDocParticipante.SelectedIndex = iIndiceComboCui;
                                ddl_TipoDocParticipante.Enabled = false;
                            }
                        }
                        else
                        {
                            if (iIndiceComboCui >= 0)
                            {
                              //  ddl_TipoDocParticipante.Items[iIndiceComboCui].Enabled = false;
                                ddl_TipoDocParticipante.Enabled = true;
                            }
                            ddlGenero_Titular.Visible = true;
                            lblGenero_Titular.Visible = true;
                        }
                        
                    }
                    else
                    {
                        ddl_NacParticipante.Enabled = true;
                     
                        int iIndiceComboCui = Util.ObtenerIndiceCombo(ddl_TipoDocParticipante, Convert.ToInt16(Enumerador.enmTipoDocumento.CUI).ToString());
                        if (iIndiceComboCui >= 0)
                        {
                            if (chkconCUI.Checked)
                            {
                                ddl_TipoDocParticipante.Items[iIndiceComboCui].Enabled = false;
                            }
                            ddl_TipoDocParticipante.Enabled = true;
                        }
                        Activar_Ubicacion(true);
                        
                        if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteNacimiento.DECLARANTE_1 || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteNacimiento.DECLARANTE_2)
                        {
                            ddl_TipoDatoParticipante.Enabled = true;
                            Label14.Visible = true;
                        }
                        else
                        {
                            ddl_TipoDatoParticipante.Enabled = false;

                            Label14.Visible = false;
                        }

                        lblUbigeoParticipantes.Text = "LUGAR DE RESIDENCIA";
                        txtDireccionParticipante.Visible = true;
                        lblDireccionParticipante.Visible = true;
                        if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteNacimiento.REGISTRADOR_CIVIL)
                        {
                            Activar_Ubicacion(false);
                            txtDireccionParticipante.Visible = false;
                            lblDireccionParticipante.Visible = false;
                        }

                       // int iIndiceCombo = Util.ObtenerIndiceCombo(ddl_TipoDocParticipante, Convert.ToInt16(Enumerador.enmTipoDocumento.CUI).ToString());
                        int iIndiceComboTipoDatoParticipante = Util.ObtenerIndiceComboPorText(ddl_TipoDatoParticipante, "IGUAL TITULAR");
                        
                        
                        if (iIndiceComboTipoDatoParticipante >= 0)
                        {
                            if (chksinCUI.Checked)
                            {
                                ddl_TipoDatoParticipante.Items[iIndiceComboTipoDatoParticipante].Enabled = true;
                            }
                            else {
                                ddl_TipoDatoParticipante.Items[iIndiceComboTipoDatoParticipante].Enabled = false;
                            }
                        }
             

                        ddl_TipoDocParticipante.Enabled = true;
                        ddl_TipoVinculoParticipante.Enabled = true;
                        ddl_TipoVinculoParticipante.SelectedValue = "0";


                        //si en nacimiento es padre o madre el tipo de vinculo debe estar desactivado
                        if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteNacimiento.PADRE || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteNacimiento.MADRE || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteNacimiento.REGISTRADOR_CIVIL)
                        {
                            ddl_TipoVinculoParticipante.Enabled = false;
                            ddl_TipoVinculoParticipante.SelectedValue = "0";
                        }


                        //Si es padre o madre podra declarar que no posee documento
                        if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteNacimiento.PADRE || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteNacimiento.MADRE)
                        {
                            lbltienendocumento.Visible = true;
                            rbSi.Visible = true;
                            rbNo.Visible = true;
                            rbSi.Enabled = true;
                            rbNo.Enabled = true;
                        }
                        

                    }


                }
                else
                {

                    if (Convert.ToInt32(this.ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.MATRIMONIO)
                    {
                        
                        int iIndiceComboTipoDatoParticipante = Util.ObtenerIndiceComboPorText(ddl_TipoDatoParticipante, "IGUAL TITULAR");
                        if (iIndiceComboTipoDatoParticipante >= 0)
                        {
                            ddl_TipoDatoParticipante.Items[iIndiceComboTipoDatoParticipante].Enabled = false;
                        }
                        Activar_Ubicacion(true);
                        ddl_TipoVinculoParticipante.Enabled = false;
                        if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteMatrimonio.DON || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteMatrimonio.DONIA)
                        {
                            lblUbigeoParticipantes.Text = "LUGAR DE NACIMIENTO";
                            txtDireccionParticipante.Visible = false;
                            lblDireccionParticipante.Visible = false;


                            lblEstadoCivil.Visible = true;
                            CmbEstCiv.Visible = true;


                            lblEstadoCivil.Enabled = true;
                            CmbEstCiv.Enabled = true;
                            lbldFecNacParticipante.Enabled = true;
                            CmbEstCiv.SelectedValue = "0";
                            ddl_NacParticipante.Enabled = true;

                            //------Fecha de nacimiento----
                            lbldFecNacParticipante.Visible = true;
                            CtrldFecNacimientoParticipante.Visible = true;
                            lblObligaFecNacimientoParticipante.Visible = true;
                            CtrldFecNacimientoParticipante.Enabled = true;
                            CtrldFecNacimientoParticipante.Text = String.Empty;
                            //LblEdad2.Visible = true;
                            //LblEdad.Visible = true;
                            //BtnCalcularEded.Visible = true;
                            //-----------------------------
                        }
                        else
                        {
                            lblUbigeoParticipantes.Text = "LUGAR DE RESIDENCIA";
                            txtDireccionParticipante.Visible = true;
                            lblDireccionParticipante.Visible = true;

                            CmbEstCiv.SelectedValue = "0";
                            CtrldFecNacimientoParticipante.Text = String.Empty;

                            CmbEstCiv.Enabled = false;
                            CtrldFecNacimientoParticipante.Enabled = false;


                            lblEstadoCivil.Enabled = false;
                            CmbEstCiv.Enabled = false;
                            lbldFecNacParticipante.Enabled = false;
                            CtrldFecNacimientoParticipante.Enabled = false;

                            CmbEstCiv.SelectedValue = "0";
                            CtrldFecNacimientoParticipante.Text = String.Empty;


                            if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteMatrimonio.CELEBRANTE)
                            {
                                lbltienendocumento.Visible = true;
                                rbSi.Visible = true;
                                rbNo.Visible = true;
                                rbSi.Enabled = true;
                                rbNo.Enabled = true;
                                ddl_NacParticipante.SelectedValue = Convert.ToString((int)Enumerador.enmNacionalidad.EXTRANJERA);
                                Activar_Ubicacion(false);
                                txtDireccionParticipante.Visible = false;
                                lblDireccionParticipante.Visible = false;
                            }
                            if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteMatrimonio.REGISTRADOR_CIVIL)
                            {
                                Activar_Ubicacion(false);
                                txtDireccionParticipante.Visible = false;
                                lblDireccionParticipante.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        int iIndiceComboTipoDatoParticipante = Util.ObtenerIndiceComboPorText(ddl_TipoDatoParticipante, "IGUAL TITULAR");
                        if (iIndiceComboTipoDatoParticipante >= 0)
                        {
                            ddl_TipoDatoParticipante.Items[iIndiceComboTipoDatoParticipante].Enabled = false;
                        }
                        Activar_Ubicacion(true);
                        lblEstadoCivil.Enabled = false;
                        CmbEstCiv.Enabled = false;
                        lbldFecNacParticipante.Enabled = false;
                        CtrldFecNacimientoParticipante.Enabled = false;

                        lblEstadoCivil.Visible = false;
                        CmbEstCiv.Visible = false;
                        lbldFecNacParticipante.Visible = false;
                        CtrldFecNacimientoParticipante.Visible = false;
                        lblObligaFecNacimientoParticipante.Visible = false;

                        CmbEstCiv.SelectedValue = "0";
                        CtrldFecNacimientoParticipante.Text = String.Empty;

                        if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteDefuncion.TITULAR)
                        {
                            lblUbigeoParticipantes.Text = "LUGAR DE NACIMIENTO";
                            txtDireccionParticipante.Visible = false;
                            lblDireccionParticipante.Visible = false;

                            //-------------------------------------------
                            /* 
                            validación del ticket 354 esto ser retirada para cumplir con el requerimiento 
                            del cliente el cual indica que una titular de defuncion siempre debe ser de 
                            nacionalidad peruana por mas que su documento diga lo contrario
                            */
                            ddl_NacParticipante.SelectedValue = Convert.ToInt16(Enumerador.enmNacionalidad.PERUANA).ToString();
                            ddl_NacParticipante.Visible = false;
                            Label49.Visible = false;
                            Label65.Visible = false;
                            //-------------------------------------------

                            ddl_TipoVinculoParticipante.Enabled = false;


                            //------Fecha de nacimiento ON----
                            lbldFecNacParticipante.Visible = true;
                            CtrldFecNacimientoParticipante.Visible = true;
                            lblObligaFecNacimientoParticipante.Visible = true;
                            CtrldFecNacimientoParticipante.Enabled = true;
                            CtrldFecNacimientoParticipante.Text = String.Empty;
                            //LblEdad2.Visible = true;
                            //LblEdad.Visible = true;
                            //BtnCalcularEded.Visible = true;
                            //-----------------------------

                        }
                        else
                        {

                            //------Fecha de nacimiento OF----
                            lbldFecNacParticipante.Visible = false;
                            CtrldFecNacimientoParticipante.Visible = false;
                            lblObligaFecNacimientoParticipante.Visible = false;
                            CtrldFecNacimientoParticipante.Text = String.Empty;
                            LblEdad2.Visible = false;
                            LblEdad.Visible = false;
                            BtnCalcularEded.Visible = false;
                            //-----------------------------


                            lblUbigeoParticipantes.Text = "LUGAR DE RESIDENCIA";
                            txtDireccionParticipante.Visible = true;
                            lblDireccionParticipante.Visible = true;

                            if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteDefuncion.PADRE || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteDefuncion.MADRE)
                            {
                                ddl_TipoVinculoParticipante.Enabled = false;
                                lbltienendocumento.Visible = true;
                                rbSi.Visible = true;
                                rbNo.Visible = true;
                                rbSi.Enabled = true;
                                rbNo.Enabled = true;
                            }
                            else if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteDefuncion.REGISTRADOR_CIVIL)
                            {
                                ddl_TipoVinculoParticipante.Enabled = false;
                            }
                            else if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteDefuncion.DECLARANTE)
                            {
                                ddl_TipoDatoParticipante.Enabled = true;
                                ddl_TipoVinculoParticipante.Enabled = true;
                            }
                            else
                            {
                                ddl_TipoDatoParticipante.Enabled = false;
                                LimpiarDatosParticipanteRune();
                                ddl_TipoVinculoParticipante.Enabled = true;
                            }
                            if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteDefuncion.REGISTRADOR_CIVIL)
                            {
                                Activar_Ubicacion(false);
                                txtDireccionParticipante.Visible = false;
                                lblDireccionParticipante.Visible = false;
                            }
                        }
                    }

                    //int iIndiceCombo = Util.ObtenerIndiceCombo(ddl_TipoDocParticipante, Convert.ToInt16(Enumerador.enmTipoDocumento.CUI).ToString());

                    //if (iIndiceCombo >= 0)
                    //    ddl_TipoDocParticipante.Items[iIndiceCombo].Enabled = false;

                    ddl_TipoDocParticipante.Enabled = true;

                    //ddl_TipoVinculoParticipante.Enabled = true;
                    ddl_TipoVinculoParticipante.SelectedValue = "0";
                }


                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteDefuncion.DECLARANTE)
                                            || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteDefuncion.REGISTRADOR_CIVIL)
                                            || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteMatrimonio.CELEBRANTE)
                                            || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteMatrimonio.REGISTRADOR_CIVIL)
                                            || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteNacimiento.REGISTRADOR_CIVIL)
                                            || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteNacimiento.DECLARANTE_1)
                                            || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == Convert.ToInt32(Enumerador.enmParticipanteNacimiento.DECLARANTE_2)
                                            )
                {
                    //txtDireccionParticipante.Enabled = false;
                    //ctrlUbigeo1.HabilitaControl(false);
                }

            }
            else
            {
                ddl_TipoDatoParticipante.SelectedValue = "0";
                //ddl_Genero.SelectedValue = "0";

                lbltienendocumento.Visible = false;
                rbSi.Visible = false;
                rbNo.Visible = false;
                rbSi.Checked = true;
                rbNo.Checked = false;
                HF_TIENE_DOCUMENTO.Value = "0";
                lblEstadoCivil.Visible = false;
                CmbEstCiv.Visible = false;
                lbldFecNacParticipante.Visible = false;
                CtrldFecNacimientoParticipante.Visible = false;
                lblObligaFecNacimientoParticipante.Visible = false;
                LblEdad2.Visible = false;
                LblEdad.Visible = false;
                BtnCalcularEded.Visible = false;

                HabilitarControlParticipanteRune(false);
            }


            updFormato.Update();
        }

        protected void gdvActuaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            String strScript = String.Empty;

            if (e.CommandName == "EditarAct")
            {

                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "ACTO CIVIL", "OPERACION EN MANTENIMIENTO");
                Comun.EjecutarScript(Page, strScript);
                return;
            }
        }

        private BE.RE_TARIFA_PAGO ObtenerDatosTarifaPago(Int64 lngActuacionDetalleId)
        {
            Proceso objProceso = new Proceso();
            object[] arrParametros = { lngActuacionDetalleId };

            DataTable dtPago = (DataTable)objProceso.Invocar(ref arrParametros,
                                                             "SGAC.BE.AC_PAGO",
                                                             "LEERREGISTRO");

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
                objTarifaPago.vMonedaLocal = dr["vMonedaLocal"].ToString();
                objTarifaPago.vObservaciones = dr["acde_vNotas"].ToString();

                if (objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA ||
                    objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA ||
                    objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA)
                {
                    objTarifaPago.vNumeroOperacion = Convert.ToString(dr["pago_vBancoNumeroOperacion"]);
                    objTarifaPago.sBancoId = Convert.ToInt16(dr["pago_sBancoId"]);
                    objTarifaPago.datFechaPago = Comun.FormatearFecha(dr["pago_dFechaOperacion"].ToString());
                    objTarifaPago.dblMontoCancelado = Convert.ToDouble(dr["FTOTALSOLESCONSULARES"]);
                }
                objTarifaPago.dblClasificacion = Convert.ToDouble(dr["acde_sClasificacionTarifaId"]);
                //--------------------------------------------     
                objTarifaPago.dblNormaTarifario = Convert.ToDouble(dr["pago_iNormaTarifarioId"]);
                objTarifaPago.vSustentoTipoPago = dr["pago_vSustentoTipoPago"].ToString();

            }
            return objTarifaPago;
        }


        private void buscarExisteCUI()
        {
            //foreach (GridViewRow row in Grd_Participantes.Rows)
            //{
            //    Int32 iTipoDocumentoID = Convert.ToInt32(row.Cells[6].Text);

            //    if (iTipoDocumentoID == Constantes.CONST_EXCEPCION_CUI_ID)
            //    {
            //        HF_CUI_PARTICiPANTE.Value = row.Cells[7].Text.Trim().ToString();
            //    }
            //}
        }

        public void desactivar_participante_declarante()
        {
            ddl_TipoDocParticipante.Enabled = false; txtNroDocParticipante.Enabled = false;
            ddl_NacParticipante.Enabled = false;
            txtNomParticipante.Enabled = false;
            txtApePatParticipante.Enabled = false;
            txtApeMatParticipante.Enabled = false;
            //ctrlUbigeo1.HabilitaControl(false);
            //txtDireccionParticipante.Enabled = false;
        }

        public void activar_participante_declarante()
        {

            ddl_NacParticipante.Enabled = true;
            txtNomParticipante.Enabled = true;
            txtApePatParticipante.Enabled = true;
            txtApeMatParticipante.Enabled = true;
            //txtDireccionParticipante.Enabled = true;
            //ctrlUbigeo1.HabilitaControl(true);
        }

        public void limpiar_participante()
        {
            ddl_NacParticipante.SelectedValue = "0";
            txtNomParticipante.Text = String.Empty;
            txtApePatParticipante.Text = String.Empty;
            txtApeMatParticipante.Text = String.Empty;
            txtDireccionParticipante.Text = String.Empty;
            ctrlUbigeo1.UbigeoRefresh();
        }

        protected void ddl_TipoDatoParticipante_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Siempre que no exista padre o madre se limpiara los campos
            ddl_TipoDocParticipante.SelectedValue = "0";
            ddl_TipoDocParticipante.Enabled = true; txtNroDocParticipante.Enabled = true;
            txtNroDocParticipante.Text = string.Empty;
            HabilitarControlParticipanteRune(false);
            //ctrlUbigeo1.HabilitaControl(true);

            limpiar_participante();
            HF_ESRune.Value = "0";



            if (Convert.ToInt32(this.ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.NACIMIENTO)
            {

                #region Tipo Acto: Nacimiento
                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteNacimiento.DECLARANTE_1
                    || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteNacimiento.DECLARANTE_2)
                {
                    if (ddl_TipoDatoParticipante.SelectedValue != "0")
                    {
                        if (Convert.ToInt32(ddl_TipoDatoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteTipoDatos.PADRE)
                        {
                            desactivar_participante_declarante();
                            #region Verificar que no se registre 2 veces Padre
                                foreach (GridViewRow row in Grd_Participantes.Rows)
                                {
                                 string sTipoDatoID = Regex.Replace(row.Cells[17].Text.ToString(), @"<[^>]+>|&nbsp;", "").Trim();
                                 Int32 sTipoParticipanteID = Convert.ToInt32(row.Cells[0].Text);
                                 if (sTipoDatoID != "")
                                 {
                                     if (Convert.ToInt32(sTipoDatoID) == (int)Enumerador.enmParticipanteTipoDatos.PADRE)
                                     {
                                         if(ViewState["Editar"] == null)
                                         {
                                            ViewState["Editar"]  = false;
                                         }
                                         if (Convert.ToBoolean(ViewState["Editar"]) == false)
                                         {
                                             string StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Acto Civil", "No puede ingresar 2 veces al Declarante Padre", false, 190, 250);
                                             Comun.EjecutarScript(Page, StrScript);
                                             return;
                                         }
                                     }
                                 }
                                }
                            #endregion

                            #region Participante Padre
                            foreach (GridViewRow row in Grd_Participantes.Rows)
                            {
                                Int32 sTipoParticipanteID = Convert.ToInt32(row.Cells[0].Text);
                               
                                if (sTipoParticipanteID == (int)Enumerador.enmParticipanteNacimiento.PADRE)
                                {
                                    String sTipoDoc = row.Cells[4].Text.ToString();
                                    String NroDni = row.Cells[5].Text.ToString();
                                    

                                    string noHTML = Regex.Replace(row.Cells[5].Text.ToString(), @"<[^>]+>|&nbsp;", "").Trim();

                                    ddl_NacParticipante.SelectedValue = row.Cells[10].Text.ToString();
                                    txtNomParticipante.Text = Server.HtmlDecode(row.Cells[11].Text.ToString());
                                    txtApePatParticipante.Text = Server.HtmlDecode(row.Cells[12].Text.ToString());
                                    txtApeMatParticipante.Text = Server.HtmlDecode(row.Cells[13].Text.ToString());

                                    txtPersonaId.Text = row.Cells[9].Text.ToString();

                                    if (noHTML.Length == 0)
                                    {
                                        txtNroDocParticipante.Text = String.Empty;
                                        ddl_TipoDocParticipante.SelectedValue = "0";
                                        lblValidacionParticipante.Visible = false;
                                    }
                                    else
                                    {
                                        txtNroDocParticipante.Text = NroDni;
                                        if (Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.DNI
                                        || Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.LIBRETA_MILITAR
                                        || Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.CARNET_EXTRANJERIA
                                        || Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.CUI)
                                        {
                                            ddl_TipoDocParticipante.SelectedValue = sTipoDoc;
                                        }
                                        else
                                        {
                                            ddl_TipoDocParticipante.SelectedValue = Convert.ToInt16(Enumerador.enmTipoDocumento.OTROS).ToString();
                                        }
                                    }

                                    string ubigeo = Regex.Replace(Server.HtmlDecode(row.Cells[14].Text.ToString()), @"<[^>]+>|&nbsp;", "").Trim();
                                    string direccion = Regex.Replace(Server.HtmlDecode(row.Cells[15].Text.ToString()), @"<[^>]+>|&nbsp;", "").Trim(); 
                                    if (ubigeo != null)
                                    {

                                        if (ubigeo.Length == 6)
                                        {
                                            if (ubigeo != "000000")
                                            {
                                                txtDireccionParticipante.Text = direccion;
                                                ctrlUbigeo1.setUbigeo(ubigeo);

                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        else if (Convert.ToInt32(ddl_TipoDatoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteTipoDatos.MADRE)
                        {
                            desactivar_participante_declarante();
                            #region Verificar que no se registre 2 veces Padre
                            foreach (GridViewRow row in Grd_Participantes.Rows)
                            {
                                string sTipoDatoID = Regex.Replace(row.Cells[17].Text.ToString(), @"<[^>]+>|&nbsp;", "").Trim();
                                Int32 sTipoParticipanteID = Convert.ToInt32(row.Cells[0].Text);
                                if (sTipoDatoID != "")
                                {
                                    if (Convert.ToInt32(sTipoDatoID) == (int)Enumerador.enmParticipanteTipoDatos.MADRE)
                                    {
                                        if (ViewState["Editar"] == null)
                                        {
                                            ViewState["Editar"] = false;
                                        }
                                        if (Convert.ToBoolean(ViewState["Editar"]) == false)
                                        {
                                            string StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Acto Civil", "No puede ingresar 2 veces al Declarante Padre", false, 190, 250);
                                            Comun.EjecutarScript(Page, StrScript);
                                            return;
                                        }
                                    }
                                }
                            }
                            #endregion
                            #region Participante Madre
                            foreach (GridViewRow row in Grd_Participantes.Rows)
                            {
                                Int32 sTipoParticipanteID = Convert.ToInt32(row.Cells[0].Text);

                                if (sTipoParticipanteID == (int)Enumerador.enmParticipanteNacimiento.MADRE)
                                {
                                    String sTipoDoc = row.Cells[4].Text.ToString();
                                    String NroDni = row.Cells[5].Text.ToString();


                                    string noHTML = Regex.Replace(row.Cells[5].Text.ToString(), @"<[^>]+>|&nbsp;", "").Trim();

                                    ddl_NacParticipante.SelectedValue = row.Cells[10].Text.ToString();
                                    txtNomParticipante.Text = Server.HtmlDecode(row.Cells[11].Text.ToString());
                                    txtApePatParticipante.Text = Server.HtmlDecode(row.Cells[12].Text.ToString());
                                    txtApeMatParticipante.Text = Server.HtmlDecode(row.Cells[13].Text.ToString());

                                    txtPersonaId.Text = row.Cells[9].Text.ToString();

                                    if (noHTML.Length == 0)
                                    {
                                        txtNroDocParticipante.Text = String.Empty;
                                        ddl_TipoDocParticipante.SelectedValue = "0";
                                        lblValidacionParticipante.Visible = false;
                                    }
                                    else
                                    {
                                        txtNroDocParticipante.Text = NroDni;
                                        if (Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.DNI
                                        || Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.LIBRETA_MILITAR
                                        || Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.CARNET_EXTRANJERIA
                                        || Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.CUI)
                                        {
                                            ddl_TipoDocParticipante.SelectedValue = sTipoDoc;
                                        }
                                        else
                                        {
                                            ddl_TipoDocParticipante.SelectedValue = Convert.ToInt16(Enumerador.enmTipoDocumento.OTROS).ToString();
                                        }
                                    }

                                    string ubigeo = Regex.Replace(Server.HtmlDecode(row.Cells[14].Text.ToString()), @"<[^>]+>|&nbsp;", "").Trim();
                                    string direccion = Regex.Replace(row.Cells[15].Text.ToString(), @"<[^>]+>|&nbsp;", "").Trim();
                                    if (ubigeo != null)
                                    {

                                        if (ubigeo.Length == 6)
                                        {
                                            if (ubigeo != "000000")
                                            {
                                                txtDireccionParticipante.Text = direccion;
                                                ctrlUbigeo1.setUbigeo(ubigeo);

                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        else if (ddl_TipoDatoParticipante.SelectedItem.Text == "IGUAL TITULAR")
                        {
                            desactivar_participante_declarante();
                            #region Participante Titular
                            foreach (GridViewRow row in Grd_Participantes.Rows)
                            {
                                Int32 sTipoParticipanteID = Convert.ToInt32(row.Cells[0].Text);

                                if (sTipoParticipanteID == (int)Enumerador.enmParticipanteNacimiento.TITULAR)
                                {
                                    String sTipoDoc = row.Cells[4].Text.ToString();
                                    String NroDni = row.Cells[5].Text.ToString();


                                    string noHTML = Regex.Replace(row.Cells[5].Text.ToString(), @"<[^>]+>|&nbsp;", "").Trim();

                                    ddl_NacParticipante.SelectedValue = row.Cells[10].Text.ToString();
                                    txtNomParticipante.Text = Server.HtmlDecode(row.Cells[11].Text.ToString());
                                    txtApePatParticipante.Text = Server.HtmlDecode(row.Cells[12].Text.ToString());
                                    txtApeMatParticipante.Text = Server.HtmlDecode(row.Cells[13].Text.ToString());

                                    txtPersonaId.Text = row.Cells[9].Text.ToString();

                                    if (noHTML.Length == 0)
                                    {
                                        txtNroDocParticipante.Text = String.Empty;
                                        ddl_TipoDocParticipante.SelectedValue = "0";
                                        lblValidacionParticipante.Visible = false;
                                    }
                                    else
                                    {
                                        txtNroDocParticipante.Text = NroDni;

                                        if (Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.DNI
                                                                        || Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.LIBRETA_MILITAR
                                                                        || Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.CARNET_EXTRANJERIA
                                                                        || Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.CUI)
                                        {
                                            ddl_TipoDocParticipante.SelectedValue = sTipoDoc;
                                        }
                                        else
                                        {
                                            ddl_TipoDocParticipante.SelectedValue = Convert.ToInt16(Enumerador.enmTipoDocumento.OTROS).ToString();
                                        }
                                    }

                                    string ubigeo = Regex.Replace(Server.HtmlDecode(row.Cells[14].Text.ToString()), @"<[^>]+>|&nbsp;", "").Trim();
                                    string direccion = Regex.Replace(row.Cells[15].Text.ToString(), @"<[^>]+>|&nbsp;", "").Trim();
                                    if (ubigeo != null)
                                    {

                                        if (ubigeo.Length == 6)
                                        {
                                            if (ubigeo != "000000")
                                            {
                                                txtDireccionParticipante.Text = direccion;
                                                ctrlUbigeo1.setUbigeo(ubigeo);

                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        ddl_TipoVinculoParticipante.Enabled = false;
                    }
                    else
                    {
                        ddl_TipoVinculoParticipante.Enabled = true;
                        activar_participante_declarante();
                        limpiar_participante();
                    }
                }
                #endregion
            }
            else
            {

                if (Convert.ToInt32(this.ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.DEFUNCION)
                {
                    if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteDefuncion.DECLARANTE)
                    {
                        desactivar_participante_declarante();
                        #region Participante Declarante
                        if (ddl_TipoDatoParticipante.SelectedValue != "0")
                        {
                            if (ddl_TipoDatoParticipante.SelectedValue != "0")
                            {
                                if (Convert.ToInt32(ddl_TipoDatoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteTipoDatos.PADRE)
                                {
                                    desactivar_participante_declarante();
                                    #region Participante Padre
                                    foreach (GridViewRow row in Grd_Participantes.Rows)
                                    {
                                        Int32 sTipoParticipanteID = Convert.ToInt32(row.Cells[0].Text);

                                        if (sTipoParticipanteID == (int)Enumerador.enmParticipanteDefuncion.PADRE)
                                        {
                                            String sTipoDoc = row.Cells[4].Text.ToString();
                                            String NroDni = row.Cells[5].Text.ToString();


                                            string noHTML = Regex.Replace(row.Cells[5].Text.ToString(), @"<[^>]+>|&nbsp;", "").Trim();

                                            ddl_NacParticipante.SelectedValue = row.Cells[10].Text.ToString();
                                            txtNomParticipante.Text = Server.HtmlDecode(row.Cells[11].Text.ToString());
                                            txtApePatParticipante.Text = Server.HtmlDecode(row.Cells[12].Text.ToString());
                                            txtApeMatParticipante.Text = Server.HtmlDecode(row.Cells[13].Text.ToString());

                                            txtPersonaId.Text = row.Cells[9].Text.ToString();

                                            if (noHTML.Length == 0)
                                            {
                                                txtNroDocParticipante.Text = String.Empty;
                                                ddl_TipoDocParticipante.SelectedValue = "0";
                                                lblValidacionParticipante.Visible = false;
                                            }
                                            else
                                            {
                                                txtNroDocParticipante.Text = NroDni;

                                                if (Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.DNI
                                                                                || Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.LIBRETA_MILITAR
                                                                                || Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.CARNET_EXTRANJERIA
                                                                                || Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.CUI)
                                                {
                                                    ddl_TipoDocParticipante.SelectedValue = sTipoDoc;
                                                }
                                                else
                                                {
                                                    ddl_TipoDocParticipante.SelectedValue = Convert.ToInt16(Enumerador.enmTipoDocumento.OTROS).ToString();
                                                }
                                            }

                                            string ubigeo = Regex.Replace(Server.HtmlDecode(row.Cells[14].Text.ToString()), @"<[^>]+>|&nbsp;", "").Trim();
                                            string direccion = Regex.Replace(row.Cells[15].Text.ToString(), @"<[^>]+>|&nbsp;", "").Trim();
                                            if (ubigeo != null)
                                            {

                                                if (ubigeo.Length == 6)
                                                {
                                                    if (ubigeo != "000000")
                                                    {
                                                        txtDireccionParticipante.Text = direccion;
                                                        ctrlUbigeo1.setUbigeo(ubigeo);

                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                else if (Convert.ToInt32(ddl_TipoDatoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteTipoDatos.MADRE)
                                {
                                    desactivar_participante_declarante();
                                    #region Participante Madre
                                    foreach (GridViewRow row in Grd_Participantes.Rows)
                                    {
                                        Int32 sTipoParticipanteID = Convert.ToInt32(row.Cells[0].Text);

                                        if (sTipoParticipanteID == (int)Enumerador.enmParticipanteDefuncion.MADRE)
                                        {
                                            String sTipoDoc = row.Cells[4].Text.ToString();
                                            String NroDni = row.Cells[5].Text.ToString();


                                            string noHTML = Regex.Replace(row.Cells[5].Text.ToString(), @"<[^>]+>|&nbsp;", "").Trim();

                                            ddl_NacParticipante.SelectedValue = row.Cells[10].Text.ToString();
                                            txtNomParticipante.Text = Server.HtmlDecode(row.Cells[11].Text.ToString());
                                            txtApePatParticipante.Text = Server.HtmlDecode(row.Cells[12].Text.ToString());
                                            txtApeMatParticipante.Text = Server.HtmlDecode(row.Cells[13].Text.ToString());

                                            txtPersonaId.Text = row.Cells[9].Text.ToString();

                                            if (noHTML.Length == 0)
                                            {
                                                txtNroDocParticipante.Text = String.Empty;
                                                ddl_TipoDocParticipante.SelectedValue = "0";
                                                lblValidacionParticipante.Visible = false;
                                            }
                                            else
                                            {
                                                txtNroDocParticipante.Text = NroDni;

                                                if (Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.DNI
                                                                                || Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.LIBRETA_MILITAR
                                                                                || Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.CARNET_EXTRANJERIA
                                                                                || Convert.ToInt16(sTipoDoc) == (Int16)Enumerador.enmTipoDocumento.CUI)
                                                {
                                                    ddl_TipoDocParticipante.SelectedValue = sTipoDoc;
                                                }
                                                else
                                                {
                                                    ddl_TipoDocParticipante.SelectedValue = Convert.ToInt16(Enumerador.enmTipoDocumento.OTROS).ToString();
                                                }
                                            }

                                            string ubigeo = Regex.Replace(Server.HtmlDecode(row.Cells[14].Text.ToString()), @"<[^>]+>|&nbsp;", "").Trim();
                                            string direccion = Regex.Replace(row.Cells[15].Text.ToString(), @"<[^>]+>|&nbsp;", "").Trim();
                                            if (ubigeo != null)
                                            {

                                                if (ubigeo.Length == 6)
                                                {
                                                    if (ubigeo != "000000")
                                                    {
                                                        txtDireccionParticipante.Text = direccion;
                                                        ctrlUbigeo1.setUbigeo(ubigeo);

                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                ddl_TipoVinculoParticipante.Enabled = false;
                            }
                            else
                            {
                                ddl_TipoVinculoParticipante.Enabled = true;
                                activar_participante_declarante();
                                limpiar_participante();
                            }
                        }
                        else
                        {
                            ddl_TipoVinculoParticipante.Enabled = true;
                            activar_participante_declarante();
                            limpiar_participante();
                        }
                        #endregion
                    }
                }
            }
            if (ddl_TipoDatoParticipante.SelectedValue == "0")
            {
                ddl_TipoDocParticipante.Enabled = true; txtNroDocParticipante.Enabled = true;
            }

            updFormato.Update();
        }

        public int CalcularEdad(DateTime birthDate)
        {

            if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteDefuncion.TITULAR) {
                DateTime now = Comun.FormatearFecha(txtFecNac.Text); ;
                int age = now.Year - birthDate.Year;
                if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                    age--;
                return age;
            }
            else
            {
                DateTime now = DateTime.Today;
                int age = now.Year - birthDate.Year;
                if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                    age--;
                return age;
            }
            


        }

        protected void BtnCalcularEded_Click(object sender, EventArgs e)
        {
            try
            {
                if (CtrldFecNacimientoParticipante.Text != string.Empty)
                {
                    LblEdad2.Visible = true;
                    LblEdad.Visible = true;
                    DateTime datFecha = new DateTime();
                    if (!DateTime.TryParse(CtrldFecNacimientoParticipante.Text, out datFecha))
                    {
                        datFecha = Comun.FormatearFecha(CtrldFecNacimientoParticipante.Text);
                    }
                    //LblEdad2.Text = Convert.ToString(CalcularEdad(datFecha));
                    LblEdad2.Text = ObtenerEdad(datFecha);
                }

                //if (HFEsRune.Value == "0")
                //    HabilitarControlParticipanteRune(true);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                updFormato.Update();
            }
        }

        protected void rbSi_CheckedChanged(object sender, EventArgs e)
        {
            HabilitarSegunExistenciaDocumento();
        }

        private void HabilitarSegunExistenciaDocumento()
        {

            if (rbSi.Checked)
            {
                ddl_NacParticipante.Enabled = true;
                //HabilitarControlParticipanteRune(true);
                ddl_TipoDocParticipante.Enabled = true;
                txtNroDocParticipante.Enabled = true;

                ddl_NacParticipante.Enabled = false;
                txtApeMatParticipante.Enabled = false;
                txtApePatParticipante.Enabled = false;
                txtNomParticipante.Enabled = false;
                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteMatrimonio.CELEBRANTE)
                {
                    lbltienendocumento.Visible = true;
                    rbSi.Visible = true;
                    rbNo.Visible = true;
                    ddl_NacParticipante.SelectedValue = Convert.ToString((int)Enumerador.enmNacionalidad.EXTRANJERA);
                }

                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteNacimiento.PADRE || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteNacimiento.MADRE)
                {
                    ddl_NacParticipante.SelectedValue = Convert.ToString((int)Enumerador.enmNacionalidad.NINGUNA);                    
                }
                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteDefuncion.PADRE || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteDefuncion.MADRE)
                {
                    ddl_NacParticipante.SelectedValue = Convert.ToString((int)Enumerador.enmNacionalidad.NINGUNA);
                }

            }
            else
            {
                //HabilitarControlParticipanteRune(false);
                ddl_TipoDocParticipante.SelectedIndex = 0;
                ddl_TipoDocParticipante.Enabled = false;
                txtNroDocParticipante.Enabled = false;
                txtApeMatParticipante.Enabled = true;
                txtApePatParticipante.Enabled = true;
                txtNomParticipante.Enabled = true;
                ddl_NacParticipante.Enabled = true;
                ddl_NacParticipante.SelectedValue = Convert.ToString((int)Enumerador.enmNacionalidad.EXTRANJERA);
                //HFEsRune.Value = "0";
                if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteMatrimonio.CELEBRANTE)
                {
                    lbltienendocumento.Visible = true;
                    rbSi.Visible = true;
                    rbNo.Visible = true;
                    ddl_NacParticipante.SelectedValue = Convert.ToString((int)Enumerador.enmNacionalidad.EXTRANJERA);
                }
            }
        }

        protected void Ubigeo_Click(object sender, EventArgs e)
        {
            if (ddl_Genero.SelectedValue == "0")
            {
                ddl_Genero.Enabled = true;
            }
            if (HF_ESRune.Value != "1")
            {
                ddl_Genero.Enabled = true;
            }

            if (HF_ESRune.Value == "0" || HF_ESRune.Value == String.Empty)
            {
                HabilitarControlParticipanteRune(true);
            }
            else { 
            //    ctrlUbigeo1.HabilitaControl(true); 
            }
            if (rbSi.Checked)
            {
                if (HF_ESRune.Value == "0" || HF_ESRune.Value == String.Empty)
                {
                    HabilitarControlParticipanteRune(true);
                }
                else
                {
                    //ddl_TipoDocParticipante.Enabled = true;
                    txtNroDocParticipante.Enabled = true;
                }
            }
            else
            {
                ddl_TipoDocParticipante.Enabled = false;
                txtNroDocParticipante.Enabled = false;
            }

            if (ddl_NacParticipante.SelectedValue != "0" && ddl_TipoDocParticipante.SelectedValue != Convert.ToString((int)Enumerador.enmTipoDocumento.OTROS))
            {
                if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.NACIMIENTO)
                {
                    if (Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteNacimiento.PADRE || Convert.ToInt32(ddl_TipoParticipante.SelectedValue) == (int)Enumerador.enmParticipanteNacimiento.MADRE)
                    {

                        //ddl_NacParticipante.SelectedValue = Convert.ToString((int)Enumerador.enmNacionalidad.EXTRANJERA);
                        //ddl_NacParticipante.Enabled = false;
                        ddl_NacParticipante.Enabled = true;
                    }
                }
                else {
                    ddl_NacParticipante.Enabled = false;
                }
                
           
                
            }

        }

        [System.Web.Services.WebMethod]
        public static Boolean Imprimir(Int64 HF_REFISTROCIVIL, Int64 HF_ACTUDETALLE, Int32 intTipoActa)
        {
            try
            {
                StringBuilder sScript = new StringBuilder();
                Boolean Resultado = false;
                String vTipoActa = String.Empty;

                string strRutaHtml = string.Empty;
                string strArchivoPDF = string.Empty;

                String localfilepath = String.Empty;
                String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];

                switch (intTipoActa)
                {
                    case (int)Enumerador.enmTipoActa.NACIMIENTO:
                        vTipoActa = Enumerador.enmTipoActa.NACIMIENTO.ToString();
                        break;
                    case (int)Enumerador.enmTipoActa.MATRIMONIO:
                        vTipoActa = Enumerador.enmTipoActa.MATRIMONIO.ToString();
                        break;
                    case (int)Enumerador.enmTipoActa.DEFUNCION:
                        vTipoActa = Enumerador.enmTipoActa.DEFUNCION.ToString();
                        break;
                }
                string strRutaPDF = uploadPath + @"\" + vTipoActa.Trim() + DateTime.Now.Ticks.ToString() + ".pdf";

                strRutaHtml = uploadPath + @"\" + vTipoActa.Trim() + DateTime.Now.Ticks.ToString() + ".html";
                DataTable dt = new Reportes.dsActoCivil().Tables[vTipoActa];

                dt = new SGAC.Registro.Actuacion.BL.ActoCivilConsultaBL().Consultar_Formato(HF_ACTUDETALLE, HF_REFISTROCIVIL);


                StreamWriter str = new StreamWriter(strRutaHtml, true, Encoding.Default);

                switch (intTipoActa)
                {
                    case (int)Enumerador.enmTipoActa.NACIMIENTO:
                        str.Write("<p align=\"justify\" style=\"background-color: transparent;\">Hola</p>");
                        str.Dispose();

                        CreateFilePDFNacimiento(dt, strRutaHtml, strRutaPDF);

                        if (System.IO.File.Exists(strRutaPDF))
                        {
                            //WebClient User = new WebClient();
                            //Byte[] FileBuffer = User.DownloadData(strRutaPDF);

                            //if (FileBuffer != null)
                            //{
                            //    HttpContext.Current.Session["binaryData"] = FileBuffer;
                                Resultado = true;
                            //}                                                        
                        }

                        if (File.Exists(strRutaHtml))
                        {
                            File.Delete(strRutaHtml);
                        }

                        break;
                    case (int)Enumerador.enmTipoActa.MATRIMONIO:

                        str.Write("<p align=\"justify\" style=\"background-color: transparent;\">Hola</p>");
                        str.Dispose();


                        CreateFilePDFMatrimonio(dt, strRutaHtml, strRutaPDF);

                        if (System.IO.File.Exists(strRutaPDF))
                        {
                            //WebClient User = new WebClient();
                            //Byte[] FileBuffer = User.DownloadData(strRutaPDF);
                            //if (FileBuffer != null)
                            //{
                            //    HttpContext.Current.Session["binaryData"] = FileBuffer;
                                Resultado = true;
                            //}
                        }

                        if (File.Exists(strRutaHtml))
                        {
                            File.Delete(strRutaHtml);
                        }

                        break;
                    case (int)Enumerador.enmTipoActa.DEFUNCION:

                        str.Write("<p align=\"justify\" style=\"background-color: transparent;\">Hola</p>");
                        str.Dispose();

                        CreateFilePDFDefuncion(dt, strRutaHtml, strRutaPDF);
                        if (System.IO.File.Exists(strRutaPDF))
                        {
                            //WebClient User = new WebClient();
                            //Byte[] FileBuffer = User.DownloadData(strRutaPDF);
                            //if (FileBuffer != null)
                            //{
                            //    HttpContext.Current.Session["binaryData"] = FileBuffer;
                                Resultado = true;

                            //}
                        }

                        if (File.Exists(strRutaHtml))
                        {
                            File.Delete(strRutaHtml);
                        }
                        break;
                }


                //------------------------
                //Autor: Miguel Angel Márquez Beltrán
                //Fecha: 27/09/2016
                //Objetivo: Asignar a la sesion el nombre y la ruta del archivo.
                //-------------------------
               
                HttpContext.Current.Session["rutaPDF"] = strRutaPDF;
                //if (Resultado)
                //{
                //    WebClient User = new WebClient();
                //    Byte[] FileBuffer = User.DownloadData(strRutaPDF);
                //    if (FileBuffer != null)
                //    {
                //        HttpContext.Current.Session["binaryData"] = FileBuffer;
                //    }
                //}
                return Resultado;
            }
            catch (Exception ex)
            {
                throw ex;
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


        private static void CreateFilePDFMatrimonio(DataTable dt, string HtmlPath, string PdfPath)
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

                iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                iTextSharp.text.pdf.BaseFont bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);


                var vNavegador = HttpContext.Current.Session["vNavegadorActivo"].ToString();


                cb.SetFontAndSize(bfTimes, 11);

                float ejex = (float)11.6;
                float fOrigenX = 68;
                float fOrigenY = document.PageSize.Height - 71 - 20;


                if (vNavegador.IndexOf("firefox") == -1)
                {
                    fOrigenX -= 1;
                    fOrigenY += 9;
                }
                else
                {
                    fOrigenY += 4;
                }

                #region PosicionesY

                float fechaPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMFechaCelebracionY"].ToString());
                float ubigeoLugarDepProvPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMLugarNacimientoUbi01Y"].ToString());
                float ubigeoLugarDistCpoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMLugarNacimientoUbi03Y"].ToString());

                float celebranteIdentidad = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMCelebranteIdentidadY"].ToString());
                float celebranteNombrePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMCelebranteNombresY"].ToString());

                float celebrantePrimerApellidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMCelebrantePrimerApellidoY"].ToString());
                float celebranteSegundoApellidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMCelebranteSegundoApellidoY"].ToString());
                float celebranteCargoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMCelebranteCargoY"].ToString());
                float celebranteExpedientePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMCelebranteExpedienteY"].ToString());

                float conyuge1NombrePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMConyuge1NombresY"].ToString());
                float conyuge1PrimerApellidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMConyuge1PrimerApellidoY"].ToString());
                float conyuge1SegundoApellidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMConyuge1SegundoApellidoY"].ToString());
                float conyuge1IdentiNacionalPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMConyuge1DocTipoY"].ToString());
                float conyuge1EdadEdoCivilPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMConyuge1EdadY"].ToString());
                float conyuge1DepProvPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMConyuge1NacimientoUbi01Y"].ToString());
                float conyuge1DistCpoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMConyuge1NacimientoUbi03Y"].ToString());

                float conyuge2NombrePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMConyuge2NombresY"].ToString());
                float conyuge2PrimerApellidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMConyuge2PrimerApellidoY"].ToString());
                float conyuge2SegundoApellidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMConyuge2SegundoApellidoY"].ToString());
                float conyuge2IdentiNacionalPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMConyuge2DocTipoY"].ToString());
                float conyuge2EdadEdoCivilPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMConyuge2EdadY"].ToString());
                float conyuge2DepProvPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMConyuge2NacimientoUbi01Y"].ToString());
                float conyuge2DistCpoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMConyuge2NacimientoUbi03Y"].ToString());

                float fechaRegistroPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMFechaRegistroY"].ToString());

                float registradorDepProvPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMOfiRegistralUbi01Y"].ToString());
                float registradorDistCpoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMOfiRegistralUbi03Y"].ToString());

                float registradorNombrePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMRegistradorNombresY"].ToString());
                float registradorIdentidadPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMRegistradorDocNumeroY"].ToString());

                float observacionesPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FMObservacionesY"].ToString());

                #endregion

                cb.BeginText();

                //Fecha de Celebracion
                DateTime fecha = Comun.FormatearFecha(dt.Rows[0]["Fecha"].ToString());
                //string vFecha = fecha.Day.ToString().PadLeft(2, '0') + "   " + fecha.Month.ToString().PadLeft(2, '0') + "  " + fecha.Year.ToString(); //IDM-MODIFICADO

                _EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 9)
                    + float.Parse(ConfigurationManager.AppSettings["FMFechaCelebracionX"].ToString())
                    , fechaPosY, ejex, fecha.Day.ToString().PadLeft(2, '0'), cb, document);
                _EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 9)
                    + float.Parse(ConfigurationManager.AppSettings["FMFechaCelebracionMesX"].ToString())
                    , fechaPosY, ejex, fecha.Month.ToString().PadLeft(2, '0'), cb, document);
                _EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 9)
                    + float.Parse(ConfigurationManager.AppSettings["FMFechaCelebracionAnioX"].ToString())
                    , fechaPosY, ejex, fecha.Year.ToString(), cb, document);

                //Lugar Ubigeo Departamento
                string ubigeoDepCod = dt.Rows[0]["LugarUbigeoDep"].ToString().PadLeft(2, '0');
                string ubigeoDepDesc = dt.Rows[0]["LugarUbigeoDepDetalle"].ToString();

                if (ubigeoDepDesc.Length == 0) ubigeoDepDesc = "----------";
                else ubigeoDepDesc = Util.GenerarLineasubigeoizquierda(ubigeoDepDesc);
                ubigeoDepCod = EsCodigo00(ubigeoDepCod);

                //string ubigeoDep = ubigeoDepCod + "    " + ubigeoDepDesc; // IDM-MODIFICADO            

                EscribirLetraxLetraMatrimonio(fOrigenX + float.Parse(ConfigurationManager.AppSettings["FMLugarUbi01X"].ToString())
                    , ubigeoLugarDepProvPosY, ejex, ubigeoDepCod, cb, document);
                EscribirLetraxLetraMatrimonio(fOrigenX + float.Parse(ConfigurationManager.AppSettings["FMLugarUbi01DescX"].ToString())
                    , ubigeoLugarDepProvPosY, ejex, ubigeoDepDesc, cb, document);

                //Lugar Ubigeo Provincia
                string ubigeoProvCod = dt.Rows[0]["LugarUbigeoPrv"].ToString().PadLeft(2, '0');
                string ubigeoProvDesc = dt.Rows[0]["LugarUbigeoPrvDetalle"].ToString();

                if (ubigeoProvDesc.Length == 0) ubigeoProvDesc = "----------";
                else ubigeoProvDesc = Util.GenerarLineasubigeoderecha(ubigeoProvDesc);// +"----";

                ubigeoProvCod = EsCodigo00(ubigeoProvCod);

                //string ubigeoProv = ubigeoProvCod + "    " + ubigeoProvDesc; // IDM-MODIFICADO    

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 25)
                    + float.Parse(ConfigurationManager.AppSettings["FMLugarUbi02X"].ToString())
                    , ubigeoLugarDepProvPosY, ejex, ubigeoProvCod, cb, document);
                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 25)
                    + float.Parse(ConfigurationManager.AppSettings["FMLugarUbi02DescX"].ToString())
                    , ubigeoLugarDepProvPosY, ejex, ubigeoProvDesc, cb, document);

                //Lugar Ubigeo Distrito
                string ubigeoDistCod = dt.Rows[0]["LugarUbigeoDst"].ToString().PadLeft(2, '0');
                string ubigeoDistDesc = dt.Rows[0]["LugarUbigeoDstDetalle"].ToString();

                if (ubigeoDistDesc.Length == 0) ubigeoDistDesc = "----------";
                else ubigeoDistDesc = Util.GenerarLineasubigeoizquierda(ubigeoDistDesc);// +"-----";

                ubigeoDistCod = EsCodigo00(ubigeoDistCod);

                //string ubigeoDist = ubigeoDistCod + "    " + ubigeoDistDesc; // IDM-MODIFICADO   

                EscribirLetraxLetraMatrimonio(fOrigenX
                    + float.Parse(ConfigurationManager.AppSettings["FMLugarUbi03X"].ToString())
                    , ubigeoLugarDistCpoPosY, ejex, ubigeoDistCod, cb, document);
                EscribirLetraxLetraMatrimonio(fOrigenX
                    + float.Parse(ConfigurationManager.AppSettings["FMLugarUbi03DescX"].ToString())
                    , ubigeoLugarDistCpoPosY, ejex, ubigeoDistDesc, cb, document);

                //Lugar Ubigeo Centros Poblados
                string ubigeoCpoCod = dt.Rows[0]["LugarUbigeoCpo"].ToString().PadLeft(2, '0');
                string ubigeoCpoDesc = dt.Rows[0]["LugarUbigeoCpoDetalle"].ToString();

                if (ubigeoCpoDesc.Length == 0) ubigeoCpoDesc = "----------";
                else ubigeoCpoDesc = Util.GenerarLineasubigeoderecha(ubigeoCpoDesc);// +"----------";


                ubigeoCpoCod = EsCodigo00(ubigeoCpoCod);

                //string ubigeoCpo = ubigeoCpoCod + "    " + ubigeoCpoDesc; // IDM-MODIFICADO

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 25)
                    + float.Parse(ConfigurationManager.AppSettings["FMLugarCentroPobladoX"].ToString())
                    , ubigeoLugarDistCpoPosY, ejex, ubigeoCpoCod, cb, document);
                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 25)
                    + float.Parse(ConfigurationManager.AppSettings["FMLugarCentroPobladoDescX"].ToString())
                    , ubigeoLugarDistCpoPosY, ejex, ubigeoCpoDesc, cb, document);

                //Documento Identidad

                string celTipoDocumento = (dt.Rows[0]["CelebranteTipoDocumento"].ToString().Trim() == "" ? "--" : dt.Rows[0]["CelebranteTipoDocumento"].ToString());
                string celNroDocumento = Util.GenerarLineasDocumento(dt.Rows[0]["CelebranteNumeroDocumento"].ToString());
                //string celDocumento = celTipoDocumento + "       " + celNroDocumento;// IDM-MODIFICADO

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 26)
                    + float.Parse(ConfigurationManager.AppSettings["FMCelebranteTipoDocX"].ToString())
                    , celebranteIdentidad, ejex, celTipoDocumento, cb, document);
                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 26)
                    + float.Parse(ConfigurationManager.AppSettings["FMCelebranteNumeroDocX"].ToString())
                    , celebranteIdentidad, ejex, celNroDocumento, cb, document);

                //Nombre

                string vNombre = dt.Rows[0]["CelebrantePreNombres"].ToString();

                if (vNombre.Length == 0) vNombre = "----------";
                else vNombre = vNombre + "------";

                EscribirLetraxLetraMatrimonio(fOrigenX +
                    +float.Parse(ConfigurationManager.AppSettings["FMCelebranteNombresX"].ToString())
                    , celebranteNombrePosY, ejex, vNombre, cb, document);


                //Primer Apellido

                string vPrimerApellido = dt.Rows[0]["CelebrantePrimerApellido"].ToString();

                if (vPrimerApellido.Length == 0) vPrimerApellido = "----------";
                else vPrimerApellido = vPrimerApellido + "------";

                EscribirLetraxLetraMatrimonio(fOrigenX +
                    +float.Parse(ConfigurationManager.AppSettings["FMCelebrantePrimerApellidoX"].ToString())
                    , celebrantePrimerApellidoPosY, ejex, vPrimerApellido, cb, document);


                //Segundo Apellido

                string vSegundoApellido = dt.Rows[0]["CelebranteSegundoApellido"].ToString();

                if (vSegundoApellido.Trim().Length == 0) vSegundoApellido = "----------";
                else vSegundoApellido = vSegundoApellido + "------";

                EscribirLetraxLetraMatrimonio(fOrigenX +
                +float.Parse(ConfigurationManager.AppSettings["FMCelebranteSegundoApellidoX"].ToString())
                , celebranteSegundoApellidoPosY, ejex, vSegundoApellido, cb, document);


                //Cargo

                string vCargo = dt.Rows[0]["CelebranteCargo"].ToString();

                if (vCargo.Trim().Length == 0) vCargo = "----------";
                else vCargo = vCargo + "------";

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 4)
                    + float.Parse(ConfigurationManager.AppSettings["FMCelebranteCargoX"].ToString())
                    , celebranteCargoPosY, ejex, vCargo, cb, document);


                //Expediente

                string vExpediente = dt.Rows[0]["CelebranteExpediente"].ToString();

                if (vExpediente.Trim().Length == 0) vExpediente = "----------";
                else vExpediente = vExpediente + "------";

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 6)
                    + float.Parse(ConfigurationManager.AppSettings["FMCelebranteExpedienteX"].ToString())
                    , celebranteExpedientePosY, ejex, vExpediente, cb, document);


                //Conyuge1 Don PreNombre

                string vConyuge1Nombre = dt.Rows[0]["Conyuge1Prenombres"].ToString();

                if (vConyuge1Nombre.Trim().Length == 0) vConyuge1Nombre = "----------";
                else vConyuge1Nombre = vConyuge1Nombre + "------";

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 0)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge1NombresX"].ToString())
                , conyuge1NombrePosY, ejex, vConyuge1Nombre, cb, document);


                //Conyuge1 Don PrimerApellido

                string vConyuge1PrimerApellido = dt.Rows[0]["Conyuge1PrimerApellido"].ToString();

                if (vConyuge1PrimerApellido.Trim().Length == 0) vConyuge1PrimerApellido = "----------";
                else vConyuge1PrimerApellido = vConyuge1PrimerApellido + "------";

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 0)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge1PrimerApellidoX"].ToString())
                    , conyuge1PrimerApellidoPosY, ejex, vConyuge1PrimerApellido, cb, document);


                //Conyuge1 Don SegundoApellido

                string vConyuge1SegundoApellido = dt.Rows[0]["Conyuge1SegundoApellido"].ToString();

                if (vConyuge1SegundoApellido.Trim().Length == 0) vConyuge1SegundoApellido = "----------";
                else vConyuge1SegundoApellido = vConyuge1SegundoApellido + "------";

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 0)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge1SegundoApellidoX"].ToString())
                    , conyuge1SegundoApellidoPosY, ejex, vConyuge1SegundoApellido, cb, document);


                //Conyuge1 Don Documento Identidad

                string vConyuge1TipoDocumento = dt.Rows[0]["Conyuge1TipoDocumento"].ToString();
                string vConyuge1NumeroDocumento = Util.GenerarLineasDocumento(dt.Rows[0]["Conyuge1NumeroDocumento"].ToString());

                if (vConyuge1NumeroDocumento.Trim().Length == 0) vConyuge1NumeroDocumento = "----------";
                else vConyuge1NumeroDocumento = vConyuge1NumeroDocumento + "------";

                //string vConyuge1Documento = vConyuge1TipoDocumento + "              " + vConyuge1NumeroDocumento;// IDM-MODIFICADO

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 8)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge1DocTipoX"].ToString())
                    , conyuge1IdentiNacionalPosY, ejex, vConyuge1TipoDocumento, cb, document);
                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 8)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge1DocNumeroX"].ToString())
                    , conyuge1IdentiNacionalPosY, ejex, vConyuge1NumeroDocumento, cb, document);

                //Conyuge1 Nacionalidad

                string vConyuge1Nacionalidad = dt.Rows[0]["Conyuge1Nacionalidad"].ToString();
                string vConyuge1NacionalidadTexto = dt.Rows[0]["Conyuge1NacionalidadTexto"].ToString();

                if (vConyuge1Nacionalidad.Trim().Length == 0) vConyuge1Nacionalidad = "--";


                if (vConyuge1NacionalidadTexto.Trim().Length == 0) vConyuge1NacionalidadTexto = "----------";
                else vConyuge1NacionalidadTexto = vConyuge1NacionalidadTexto + "------";

                //string vConyuge1NacionalidadCompleta = vConyuge1Nacionalidad + "              " + vConyuge1NacionalidadTexto;// IDM-MODIFICADO

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 29)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge1NacionalidadX"].ToString())
                , conyuge1IdentiNacionalPosY, ejex, vConyuge1Nacionalidad, cb, document);
                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 29)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge1NacionalidadDescX"].ToString())
                , conyuge1IdentiNacionalPosY, ejex, vConyuge1NacionalidadTexto, cb, document);


                //Conyuge1 Edad

                string vConyuge1Edad = dt.Rows[0]["Conyuge1Edad"].ToString().PadLeft(3, ' ');

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 1)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge1EdadX"].ToString())
                    , conyuge1EdadEdoCivilPosY, ejex, vConyuge1Edad, cb, document);


                //Conyuge1 Estado Civil

                string vConyuge1EstadoCivil = dt.Rows[0]["Conyuge1EstadoCivil"].ToString();

                if (vConyuge1EstadoCivil.Trim().Length == 0) vConyuge1EstadoCivil = "----------";
                else vConyuge1EstadoCivil = vConyuge1EstadoCivil + "------";

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 29)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge1EstadoCivilX"].ToString())
                    , conyuge1EdadEdoCivilPosY, ejex, vConyuge1EstadoCivil, cb, document);


                //Lugar Ubigeo Departamento Conyuge1
                string ubigeoDepCodConyuge1 = dt.Rows[0]["Conyuge1UbigeoDep"].ToString().PadLeft(2, '0');
                string ubigeoDepDescConyuge1 = dt.Rows[0]["Conyuge1UbigeoDepDetalle"].ToString();

                if (ubigeoDepDescConyuge1.Trim().Length == 0) ubigeoDepDescConyuge1 = "----------";
                else ubigeoDepDescConyuge1 = Util.GenerarLineasubigeoizquierda(ubigeoDepDescConyuge1);// +"-----";


                ubigeoDepCodConyuge1 = EsCodigo00(ubigeoDepCodConyuge1);

                //string ubigeoDepConyuge1 = ubigeoDepCodConyuge1 + "      " + ubigeoDepDescConyuge1;// IDM-MODIFICADO

                EscribirLetraxLetraMatrimonio(fOrigenX
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge1NacimientoUbi01X"].ToString())
                    , conyuge1DepProvPosY, ejex, ubigeoDepCodConyuge1, cb, document);
                EscribirLetraxLetraMatrimonio(fOrigenX
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge1NacimientoUbi01DescX"].ToString())
                    , conyuge1DepProvPosY, ejex, ubigeoDepDescConyuge1, cb, document);


                //Lugar Ubigeo Provincia
                string ubigeoProvCodConyuge1 = dt.Rows[0]["Conyuge1UbigeoPrv"].ToString().PadLeft(2, '0');
                string ubigeoProvDescConyuge1 = dt.Rows[0]["Conyuge1UbigeoPrvDetalle"].ToString();

                if (ubigeoProvDescConyuge1.Trim().Length == 0) ubigeoProvDescConyuge1 = "----------";
                else ubigeoProvDescConyuge1 = Util.GenerarLineasubigeoderecha(ubigeoProvDescConyuge1);// +"-----";

                ubigeoProvCodConyuge1 = EsCodigo00(ubigeoProvCodConyuge1);

                //string ubigeoProvConyuge1 = ubigeoProvCodConyuge1 + "      " + ubigeoProvDescConyuge1;// IDM-MODIFICADO

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 25)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge1NacimientoUbi02X"].ToString())
                    , conyuge1DepProvPosY, ejex, ubigeoProvCodConyuge1, cb, document);
                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 25)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge1NacimientoUbi02DescX"].ToString())
                    , conyuge1DepProvPosY, ejex, ubigeoProvDescConyuge1, cb, document);

                //Lugar Ubigeo Distrito
                string ubigeoDistCodConyuge1 = dt.Rows[0]["Conyuge1UbigeoDst"].ToString().PadLeft(2, '0');
                string ubigeoDistDescConyuge1 = dt.Rows[0]["Conyuge1UbigeoDstDetalle"].ToString();

                if (ubigeoDistDescConyuge1.Trim().Length == 0) ubigeoDistDescConyuge1 = "----------";
                else ubigeoDistDescConyuge1 = Util.GenerarLineasubigeoizquierda(ubigeoDistDescConyuge1);// +"-----";

                ubigeoDistCodConyuge1 = EsCodigo00(ubigeoDistCodConyuge1);

                //string ubigeoDistConyuge1 = ubigeoDistCodConyuge1 + "      " + ubigeoDistDescConyuge1;// IDM-MODIFICADO

                EscribirLetraxLetraMatrimonio(fOrigenX
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge1NacimientoUbi03X"].ToString())
                    , conyuge1DistCpoPosY, ejex, ubigeoDistCodConyuge1, cb, document);
                EscribirLetraxLetraMatrimonio(fOrigenX
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge1NacimientoUbi03DescX"].ToString())
                    , conyuge1DistCpoPosY, ejex, ubigeoDistDescConyuge1, cb, document);


                //Lugar Ubigeo Centros Poblados
                string ubigeoCpoCodConyuge1 = dt.Rows[0]["Conyuge1UbigeoCpo"].ToString().PadLeft(2, '0');
                string ubigeoCpoDescConyuge1 = dt.Rows[0]["Conyuge1UbigeoCpoDetalle"].ToString();

                if (ubigeoCpoDescConyuge1.Trim().Length == 0) ubigeoCpoDescConyuge1 = "----------";
                else ubigeoCpoDescConyuge1 = Util.GenerarLineasubigeoderecha(ubigeoCpoDescConyuge1);// +"-----";

                ubigeoCpoCodConyuge1 = EsCodigo00(ubigeoCpoCodConyuge1);

                //string ubigeoCpoConyuge1 = ubigeoCpoCodConyuge1 + "      " + ubigeoCpoDescConyuge1; // IDM-MODIFICADO

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 25)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge1CentroPobladoX"].ToString())
                    , conyuge1DistCpoPosY, ejex, ubigeoCpoCodConyuge1, cb, document);
                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 25)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge1CentroPobladoDescX"].ToString())
                    , conyuge1DistCpoPosY, ejex, ubigeoCpoDescConyuge1, cb, document);

                //Conyuge2 Don PreNombre

                string vConyuge2Nombre = dt.Rows[0]["Conyuge2Prenombres"].ToString();

                if (vConyuge2Nombre.Trim().Length == 0) vConyuge2Nombre = "----------";
                else vConyuge2Nombre = vConyuge2Nombre + "-----";

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 0)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge2NombresX"].ToString())
                    , conyuge2NombrePosY, ejex, vConyuge2Nombre, cb, document);


                //Conyuge2 Doña PrimerApellido

                string vConyuge2PrimerApellido = dt.Rows[0]["Conyuge2PrimerApellido"].ToString();

                if (vConyuge2PrimerApellido.Trim().Length == 0) vConyuge2PrimerApellido = "----------";
                else vConyuge2PrimerApellido = vConyuge2PrimerApellido + "-----";

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 0)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge2PrimerApellidoX"].ToString())
                    , conyuge2PrimerApellidoPosY, ejex, vConyuge2PrimerApellido, cb, document);


                //Conyuge2 Don SegundoApellido

                string vConyuge2SegundoApellido = dt.Rows[0]["Conyuge2SegundoApellido"].ToString();

                if (vConyuge2SegundoApellido.Trim().Length == 0) vConyuge2SegundoApellido = "----------";
                else vConyuge2SegundoApellido = vConyuge2SegundoApellido + "-----";

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 0)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge2SegundoApellidoX"].ToString())
                    , conyuge2SegundoApellidoPosY, ejex, vConyuge2SegundoApellido, cb, document);


                //Conyuge2 Don Documento Identidad

                string vConyuge2TipoDocumento = dt.Rows[0]["Conyuge2TipoDocumento"].ToString();
                string vConyuge2NumeroDocumento = dt.Rows[0]["Conyuge2NumeroDocumento"].ToString();

                if (vConyuge2TipoDocumento.Trim().Length == 0) vConyuge2TipoDocumento = "--";
                if (vConyuge2NumeroDocumento.Trim().Length == 0) vConyuge2NumeroDocumento = "----------";
                else vConyuge2NumeroDocumento = Util.GenerarLineasDocumento(vConyuge2NumeroDocumento);

                //string vConyuge2Documento = vConyuge2TipoDocumento + "             " + vConyuge2NumeroDocumento;// IDM-MODIFICADO

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 8)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge2DocTipoX"].ToString())
                    , conyuge2IdentiNacionalPosY, ejex, vConyuge2TipoDocumento, cb, document);
                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 8)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge2DocNumeroX"].ToString())
                    , conyuge2IdentiNacionalPosY, ejex, vConyuge2NumeroDocumento, cb, document);


                //Conyuge2 Nacionalidad

                string vConyuge2Nacionalidad = dt.Rows[0]["Conyuge2Nacionalidad"].ToString();
                string vConyuge2NacionalidadTexto = dt.Rows[0]["Conyuge2NacionalidadTexto"].ToString();

                if (vConyuge2Nacionalidad.Trim().Length == 0) vConyuge2Nacionalidad = "--";
                if (vConyuge2NacionalidadTexto.Trim().Length == 0) vConyuge2NacionalidadTexto = "----------";

                //string vConyuge2NacionalidadCompleta = vConyuge2Nacionalidad + "                " + vConyuge2NacionalidadTexto;// IDM-MODIFICADO

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 29)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge2NacionalidadX"].ToString())
                    , conyuge2IdentiNacionalPosY, ejex, vConyuge2Nacionalidad, cb, document);
                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 29)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge2NacionalidadDescX"].ToString())
                    , conyuge2IdentiNacionalPosY, ejex, vConyuge2NacionalidadTexto, cb, document);

                //Conyuge2 Edad

                string vConyuge2Edad = dt.Rows[0]["Conyuge2Edad"].ToString().PadLeft(3, ' ');

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 2)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge2EdadX"].ToString())
                    , conyuge2EdadEdoCivilPosY, ejex, vConyuge2Edad, cb, document);


                //Conyuge2 Estado Civil

                string vConyuge2EstadoCivil = dt.Rows[0]["Conyuge2EstadoCivil"].ToString();

                if (vConyuge2EstadoCivil.Trim().Length == 0) vConyuge2EstadoCivil = "----------";
                else vConyuge2EstadoCivil = vConyuge2EstadoCivil + "------";

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 29)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge2EstadoCivilX"].ToString())
                    , conyuge2EdadEdoCivilPosY, ejex, vConyuge2EstadoCivil, cb, document);


                //Lugar Ubigeo Departamento Conyuge2
                string ubigeoDepCodConyuge2 = dt.Rows[0]["Conyuge2UbigeoDep"].ToString().PadLeft(2, '0');
                string ubigeoDepDescConyuge2 = dt.Rows[0]["Conyuge2UbigeoDepDetalle"].ToString();

                if (ubigeoDepDescConyuge2.Trim().Length == 0) ubigeoDepDescConyuge2 = "----------";
                else ubigeoDepDescConyuge2 = Util.GenerarLineasubigeoizquierda(ubigeoDepDescConyuge2);// +"-----";

                ubigeoDepCodConyuge2 = EsCodigo00(ubigeoDepCodConyuge2);

                //string ubigeoDepConyuge2 = ubigeoDepCodConyuge2 + "        " + ubigeoDepDescConyuge2;// IDM-MODIFICADO

                EscribirLetraxLetraMatrimonio(fOrigenX
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge2NacimientoUbi01X"].ToString())
                    , conyuge2DepProvPosY, ejex, ubigeoDepCodConyuge2, cb, document);
                EscribirLetraxLetraMatrimonio(fOrigenX
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge2NacimientoUbi01DescX"].ToString())
                    , conyuge2DepProvPosY, ejex, ubigeoDepDescConyuge2, cb, document);

                //Lugar Ubigeo Provincia
                string ubigeoProvCodConyuge2 = dt.Rows[0]["Conyuge2UbigeoPrv"].ToString().PadLeft(2, '0');
                string ubigeoProvDescConyuge2 = dt.Rows[0]["Conyuge2UbigeoPrvDetalle"].ToString();

                ubigeoProvCodConyuge2 = EsCodigo00(ubigeoProvCodConyuge2);

                if (ubigeoProvDescConyuge2.Trim().Length == 0) ubigeoProvDescConyuge2 = "----------";
                else ubigeoProvDescConyuge2 = Util.GenerarLineasubigeoderecha(ubigeoProvDescConyuge2);// +"-----";
                //string ubigeoProvConyuge2 = ubigeoProvCodConyuge2 + "        " + ubigeoProvDescConyuge2;// IDM-MODIFICADO

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 25)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge2NacimientoUbi02X"].ToString())
                    , conyuge2DepProvPosY, ejex, ubigeoProvCodConyuge2, cb, document);
                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 25)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge2NacimientoUbi02DescX"].ToString())
                    , conyuge2DepProvPosY, ejex, ubigeoProvDescConyuge2, cb, document);

                //Lugar Ubigeo Distrito
                string ubigeoDistCodConyuge2 = dt.Rows[0]["Conyuge2UbigeoDst"].ToString().PadLeft(2, '0');
                string ubigeoDistDescConyuge2 = dt.Rows[0]["Conyuge2UbigeoDstDetalle"].ToString();

                ubigeoDistCodConyuge2 = EsCodigo00(ubigeoDistCodConyuge2);

                if (ubigeoDistDescConyuge2.Trim().Length == 0) ubigeoDistDescConyuge2 = "----------";
                else ubigeoDistDescConyuge2 = Util.GenerarLineasubigeoizquierda(ubigeoDistDescConyuge2);// +"-----";
                //string ubigeoDistConyuge2 = ubigeoDistCodConyuge2 + "        " + ubigeoDistDescConyuge2;// IDM-MODIFICADO

                EscribirLetraxLetraMatrimonio(fOrigenX
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge2NacimientoUbi03X"].ToString())
                    , conyuge2DistCpoPosY, ejex, ubigeoDistCodConyuge2, cb, document);
                EscribirLetraxLetraMatrimonio(fOrigenX
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge2NacimientoUbi03DescX"].ToString())
                    , conyuge2DistCpoPosY, ejex, ubigeoDistDescConyuge2, cb, document);

                //Lugar Ubigeo Centros Poblados
                string ubigeoCpoCodConyuge2 = dt.Rows[0]["Conyuge2UbigeoCpo"].ToString().PadLeft(2, '0');
                string ubigeoCpoDescConyuge2 = dt.Rows[0]["Conyuge2UbigeoCpoDetalle"].ToString();

                ubigeoCpoCodConyuge2 = EsCodigo00(ubigeoCpoCodConyuge2);

                if (ubigeoCpoDescConyuge2.Trim().Length == 0) ubigeoCpoDescConyuge2 = "----------";
                else ubigeoCpoDescConyuge2 = Util.GenerarLineasubigeoderecha(ubigeoCpoDescConyuge2);// +"-----";
                //string ubigeoCpoConyuge2 = ubigeoCpoCodConyuge2 + "        " + ubigeoCpoDescConyuge2; //IDM-MODIFICADO

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 25)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge2CentroPobladoX"].ToString())
                    , conyuge2DistCpoPosY, ejex, ubigeoCpoCodConyuge2, cb, document);
                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 25)
                    + float.Parse(ConfigurationManager.AppSettings["FMConyuge2CentroPobladoDescX"].ToString())
                    , conyuge2DistCpoPosY, ejex, ubigeoCpoDescConyuge2, cb, document);

                //Fecha de Celebracion
                DateTime fechaRegistro = Comun.FormatearFecha(dt.Rows[0]["RegistroFecha"].ToString());
                //string vFechaRegistro = fechaRegistro.Day.ToString().PadLeft(2, '0') + "   " + fechaRegistro.Month.ToString().PadLeft(2, '0') + "  " + fechaRegistro.Year.ToString();//IDM-MODIFICADO

                _EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 9)
                    + float.Parse(ConfigurationManager.AppSettings["FMFechaRegistroX"].ToString())
                    , fechaRegistroPosY, ejex, fechaRegistro.Day.ToString().PadLeft(2, '0'), cb, document);
                _EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 9)
                    + float.Parse(ConfigurationManager.AppSettings["FMFechaRegistroMesX"].ToString())
                    , fechaRegistroPosY, ejex, fechaRegistro.Month.ToString().PadLeft(2, '0'), cb, document);
                _EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 9)
                    + float.Parse(ConfigurationManager.AppSettings["FMFechaRegistroAnioX"].ToString())
                    , fechaRegistroPosY, ejex, fechaRegistro.Year.ToString(), cb, document);

                //Registro Ubigeo Departamento
                string ubigeoDepCodRegistro = dt.Rows[0]["RegistroUbigeoDep"].ToString().PadLeft(2, '0');
                string ubigeoDepDescRegistro = dt.Rows[0]["RegistroUbigeoDepDetalle"].ToString();

                ubigeoDepCodRegistro = EsCodigo00(ubigeoDepCodRegistro);

                if (ubigeoDepDescRegistro.Trim().Length == 0) ubigeoDepDescRegistro = "----------";
                else ubigeoDepDescRegistro = Util.GenerarLineasubigeoizquierda(ubigeoDepDescRegistro);// +"-----";
                //string ubigeoDepRegistro = ubigeoDepCodRegistro + "    " + ubigeoDepDescRegistro;// IDM-MODIFICADO

                EscribirLetraxLetraMatrimonio(fOrigenX
                    + float.Parse(ConfigurationManager.AppSettings["FMOfiRegistralUbi01X"].ToString())
                    , registradorDepProvPosY, ejex, ubigeoDepCodRegistro, cb, document);
                EscribirLetraxLetraMatrimonio(fOrigenX
                    + float.Parse(ConfigurationManager.AppSettings["FMOfiRegistralUbi01DescX"].ToString())
                    , registradorDepProvPosY, ejex, ubigeoDepDescRegistro, cb, document);

                //Registro Ubigeo Provincia
                string ubigeoProvCodRegistro = dt.Rows[0]["RegistroUbigeoPrv"].ToString().PadLeft(2, '0');
                string ubigeoProvDescRegistro = dt.Rows[0]["RegistroUbigeoPrvDetalle"].ToString();

                ubigeoProvCodRegistro = EsCodigo00(ubigeoProvCodRegistro);

                if (ubigeoProvDescRegistro.Trim().Length == 0) ubigeoProvDescRegistro = "----------";
                else ubigeoProvDescRegistro = Util.GenerarLineasubigeoderecha(ubigeoProvDescRegistro);// +"-----";
                //string ubigeoProvRegistro = ubigeoProvCodRegistro + "    " + ubigeoProvDescRegistro;// IDM-MODIFICADO

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 25)
                    + float.Parse(ConfigurationManager.AppSettings["FMOfiRegistralUbi02X"].ToString())
                    , registradorDepProvPosY, ejex, ubigeoProvCodRegistro, cb, document);
                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 25)
                    + float.Parse(ConfigurationManager.AppSettings["FMOfiRegistralUbi02DescX"].ToString())
                    , registradorDepProvPosY, ejex, ubigeoProvDescRegistro, cb, document);

                //Registro Ubigeo Distrito
                string ubigeoDistCodRegistro = dt.Rows[0]["RegistroUbigeoDst"].ToString().PadLeft(2, '0');
                string ubigeoDistDescRegistro = dt.Rows[0]["RegistroUbigeoDstDetalle"].ToString();

                ubigeoDistCodRegistro = EsCodigo00(ubigeoDistCodRegistro);

                if (ubigeoDistDescRegistro.Trim().Length == 0) ubigeoDistDescRegistro = "----------";
                else ubigeoDistDescRegistro = Util.GenerarLineasubigeoizquierda(ubigeoDistDescRegistro);// +"-----";
                //string ubigeoDistRegistro = ubigeoDistCodRegistro + "    " + ubigeoDistDescRegistro;// IDM-MODIFICADO

                EscribirLetraxLetraMatrimonio(fOrigenX
                    + float.Parse(ConfigurationManager.AppSettings["FMOfiRegistralUbi03X"].ToString())
                    , registradorDistCpoPosY, ejex, ubigeoDistCodRegistro, cb, document);
                EscribirLetraxLetraMatrimonio(fOrigenX
                    + float.Parse(ConfigurationManager.AppSettings["FMOfiRegistralUbi03DescX"].ToString())
                    , registradorDistCpoPosY, ejex, ubigeoDistDescRegistro, cb, document);

                //Registro Ubigeo Centros Poblados
                string ubigeoCpoCodRegistro = dt.Rows[0]["RegistroUbigeoCpo"].ToString().PadLeft(2, '0');
                string ubigeoCpoDescRegistro = dt.Rows[0]["RegistroUbigeoCpoDetalle"].ToString();

                ubigeoCpoCodRegistro = EsCodigo00(ubigeoCpoCodRegistro);

                if (ubigeoCpoDescRegistro.Trim().Length == 0) ubigeoCpoDescRegistro = "----------";
                else ubigeoCpoDescRegistro = Util.GenerarLineasubigeoderecha(ubigeoCpoDescRegistro);// +"-----";
                //string ubigeoCpoRegistro = ubigeoCpoCodRegistro + "    " + ubigeoCpoDescRegistro; // IDM-MODIFICADO

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 25)
                    + float.Parse(ConfigurationManager.AppSettings["FMOfiRegistralCentroPobladoX"].ToString())
                    , registradorDistCpoPosY, ejex, ubigeoCpoCodRegistro, cb, document);
                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 25)
                    + float.Parse(ConfigurationManager.AppSettings["FMOfiRegistralCentroPobladoDescX"].ToString())
                    , registradorDistCpoPosY, ejex, ubigeoCpoDescRegistro, cb, document);

                //Registrador Civil Nombre Completo
                string vRegistradorPrenombres = dt.Rows[0]["RegistradorPrenombres"].ToString();
                string vRegistradorPrimerApellido = dt.Rows[0]["RegistradorPrimerApellido"].ToString();
                string vRegistradorSegundoApellido = dt.Rows[0]["RegistradorSegundoApellido"].ToString();

                string vRegistradorNombreCompleto = vRegistradorPrimerApellido + " " + vRegistradorSegundoApellido + " " + vRegistradorPrenombres;

                if (vRegistradorNombreCompleto.Trim().Length == 0) vRegistradorNombreCompleto = "----------";
                else vRegistradorNombreCompleto = vRegistradorNombreCompleto + "-----";

                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 8)
                    + float.Parse(ConfigurationManager.AppSettings["FMRegistradorNombresX"].ToString())
                    , registradorNombrePosY, ejex, vRegistradorNombreCompleto, cb, document);

                //Registrador Don Documento Identidad

                string vRegistradorNumeroDocumento = dt.Rows[0]["RegistradorNumeroDocumento"].ToString();

                if (vRegistradorNumeroDocumento.Trim().Length == 0) vRegistradorNumeroDocumento = "----------";
                else vRegistradorNumeroDocumento = Util.GenerarLineasDocumento(vRegistradorNumeroDocumento);
                EscribirLetraxLetraMatrimonio(fOrigenX + (ejex * 2)
                    + float.Parse(ConfigurationManager.AppSettings["FMRegistradorDocNumeroX"].ToString())
                    , registradorIdentidadPosY, ejex, vRegistradorNumeroDocumento, cb, document);

                //Registrador Don Documento Identidad

                string vObservaciones = dt.Rows[0]["Observaciones"].ToString();

                if (vObservaciones.Trim().Length == 0) vObservaciones = "";
                else vObservaciones = vObservaciones + "-----";
                EscribirLetraxLetraMatrimonioObservacion(fOrigenX + (ejex * 5)
                    + float.Parse(ConfigurationManager.AppSettings["FMObservacionesX"].ToString())
                    , observacionesPosY, ejex, vObservaciones, cb, document);

                cb.EndText();

                document.Close();
                oStreamReader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void CreateFilePDFNacimiento(DataTable dt, string HtmlPath, string PdfPath)
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

                //var ms = new MemoryStream();

                //StreamReader oStreamReader = new StreamReader(ms, System.Text.Encoding.Default);
                
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
                iTextSharp.text.pdf.BaseFont bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);

                cb.SetFontAndSize(bfTimes, 11);


                float ejex = (float)11.6;
                float fOrigenX = -39;
                float fOrigenY_ = document.PageSize.Height - 70 - 18;
                float fOrigenX_ = document.PageSize.Width;
                float fOrigenY = 18;

                #region PosicionesY

                float CuiPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNCuiY"].ToString());
                float CuiPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNCuiX"].ToString());

                float fechaPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNFechaY"].ToString());
                float fechaPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNFechaX"].ToString());
                float HoraPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNHoraY"].ToString());
                float HoraPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNHoraX"].ToString());

                float TipoLugarPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNLugarTipoY"].ToString());
                float TipoLugarPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNLugarTipoX"].ToString());

                float LugarPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNLugarOcurrenciaY"].ToString());
                float LugarPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNLugarOcurrenciaX"].ToString());

                float LugarUbioDepPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNLugarUbi01X"].ToString());
                float LugarUbioDepPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNLugarUbi01Y"].ToString());

                float LugarUbigeoDepDetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNLugarUbi01DescX"].ToString());
                float LugarUbigeoDepDetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNLugarUbi01DescY"].ToString());

                float LugarUbigeoPrvPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNLugarUbi02X"].ToString());
                float LugarUbigeoPrvPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNLugarUbi02Y"].ToString());

                float LugarUbigeoPrvdetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNLugarUbi02DescX"].ToString());
                float LugarUbigeoPrvdetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNLugarUbi02DescY"].ToString());

                float LugarUbigeoDstPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNLugarUbi03X"].ToString());
                float LugarUbigeoDstPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNLugarUbi03Y"].ToString());

                float LugarUbigeoDstDetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNLugarUbi03DescX"].ToString());
                float LugarUbigeoDstDetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNLugarUbi03DescY"].ToString());

                float LugarUbigeoCPPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNLugarUbi04X"].ToString());
                float LugarUbigeoCPPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNLugarUbi04Y"].ToString());

                float LugarUbigeoCPDetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNLugarUbi04DescX"].ToString());
                float LugarUbigeoCPDetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNLugarUbi04DescY"].ToString());



                float SexoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNGeneroX"].ToString());
                float SexoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNGeneroY"].ToString());

                float SexoDetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNGeneroDescX"].ToString());
                float SexoDetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNGeneroDescY"].ToString());

                float PreNombresPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNNombresX"].ToString());
                float PreNombresPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNNombresY"].ToString());

                float PrimerApellidoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNPrimerApellidoX"].ToString());
                float PrimerApellidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNPrimerApellidoY"].ToString());

                float SegundoApellidoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNSegundoApellidoX"].ToString());
                float SegundoApellidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNSegundopellidoY"].ToString());

                float PadrePreNombresPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNPadreNombresX"].ToString());
                float PadrePreNombresPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNPadreNombresY"].ToString());

                float PadrePrimerApellidoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNPadrePrimerApellidoX"].ToString());
                float PadrePrimerApellidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNPadrePrimerApellidoY"].ToString());

                float PadreSegundoApellidoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNPadreSegundopellidoX"].ToString());
                float PadreSegundoApellidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNPadreSegundopellidoY"].ToString());

                float PadreTipoDocumentoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNPadreDocTipoX"].ToString());
                float PadreTipoDocumentoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNPadreDocTipoY"].ToString());

                float PadreNumeroDocumentoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNPadreDocNumeroX"].ToString());
                float PadreNumeroDocumentoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNPadreDocNumeroY"].ToString());

                float PadreNacionalidadPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNPadreNacionalidadX"].ToString());
                float PadreNacionalidadPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNPadreNacionalidadY"].ToString());


                float MadrePreNombresPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNMadreNombresX"].ToString());
                float MadrePreNombresPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNMadreNombresY"].ToString());

                float MadrePrimerApellidoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNMadrePrimerApellidoX"].ToString());
                float MadrePrimerApellidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNMadrePrimerApellidoY"].ToString());

                float MadreSegundoApellidoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNMadreSegundoApellidoX"].ToString());
                float MadreSegundoApellidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNMadreSegundopellidoY"].ToString());

                float MadreTipoDocumentoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNMadreDocTipoX"].ToString());
                float MadreTipoDocumentoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNMadreDocTipoY"].ToString());

                float MadreNumeroDocumentoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNMadreDocNumeroX"].ToString());
                float MadreNumeroDocumentoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNMadreDocNumeroY"].ToString());

                float MadreNacionalidadPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNMadreNacionalidadX"].ToString());
                float MadreNacionalidadPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNMadreNacionalidadY"].ToString());

                float DireccionPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNDireccionX"].ToString());
                float DireccionPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNDireccionY"].ToString());

                float DireccionUbigeoDepPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNDireccionUbi01X"].ToString());
                float DireccionUbigeoDepPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNDireccionUbi01Y"].ToString());

                float DireccionUbigeoDepDetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNDireccionUbi01DescX"].ToString());
                float DireccionUbigeoDepDetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNDireccionUbi01DescY"].ToString());

                float DireccionUbigeoPrvPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNDireccionUbi02X"].ToString());
                float DireccionUbigeoPrvPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNDireccionUbi02Y"].ToString());

                float DireccionUbigeoPrvDetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNDireccionUbi02DescX"].ToString());
                float DireccionUbigeoPrvDetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNDireccionUbi02DescY"].ToString());

                float DireccionUbigeoDstPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNDireccionUbi03X"].ToString());
                float DireccionUbigeoDstPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNDireccionUbi03Y"].ToString());

                float DireccionUbigeoDstDetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNDireccionUbi03DescX"].ToString());
                float DireccionUbigeoDstDetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNDireccionUbi03DescY"].ToString());


                float DireccionUbigeoCPPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNDireccionUbi04X"].ToString());
                float DireccionUbigeoCPPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNDireccionUbi04Y"].ToString());

                float DireccionUbigeoCPDetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNDireccionUbi04DescX"].ToString());
                float DireccionUbigeoCPDetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNDireccionUbi04DescY"].ToString());

                float RegistroFechaPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNRegistroFechaX"].ToString());
                float RegistroFechaPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNRegistroFechaY"].ToString());

                float RegisTroUbigeoDepPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNRegistroUbi01X"].ToString());
                float RegisTroUbigeoDepPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNRegistroUbi01Y"].ToString());

                float RegistroUbigeoDetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNRegistroUbi01DescX"].ToString());
                float RegistroUbigeoDetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNRegistroUbi01DescY"].ToString());

                float RegistroUbigeoPrvPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNRegistroUbi02X"].ToString());
                float RegistroUbigeoPrvPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNRegistroUbi02Y"].ToString());

                float RegistroUbigeoPrvDetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNRegistroUbi02DescX"].ToString());
                float RegistroUbigeoPrvDetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNRegistroUbi02DescY"].ToString());

                float RegistroUbigeoDstPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNRegistroUbi03X"].ToString());
                float RegistroUbigeoDstPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNRegistroUbi03Y"].ToString());

                float RegistroUbigeoDstDetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNRegistroUbi03DescX"].ToString());
                float RegistroUbigeoDstDetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNRegistroUbi03DescY"].ToString());

                float RegistroUbigeoCPPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNRegistroUbi04X"].ToString());
                float RegistroUbigeoCPPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNRegistroUbi04Y"].ToString());

                float RegistroUbigeoCPDetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNRegistroUbi04DescX"].ToString());
                float RegistroUbigeoCPDetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNRegistroUbi04DescY"].ToString());

                float Declarante1VinculoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNDeclarante1VinculoX"].ToString());
                float Declarante1VinculoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNDeclarante1VinculoY"].ToString());

                float Declarante1PrenombresPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNDeclarante1NombresX"].ToString());
                float Declarante1PrenombresPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNDeclarante1NombresY"].ToString());

                float Declarante1PrimerApellidoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNDeclarante1PrimerApellidoX"].ToString());
                float Declarante1PrimerApellidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNDeclarante1PrimerApellidoY"].ToString());

                float Declarante1SegundoApellidoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNDeclarante1SegundoApellidoX"].ToString());
                float Declarante1SegundoApellidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNDeclarante1SegundopellidoY"].ToString());

                float Declarante1TipoDocumentoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNDeclarante1DocTipoX"].ToString());
                float Declarante1TipoDocumentoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNDeclarante1DocTipoY"].ToString());

                float Declarante1NumeroDocumentoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNDeclarante1DocNumeroX"].ToString());
                float Declarante1NumeroDocumentoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNDeclarante1DocNumeroY"].ToString());

                float Declarante2VinculoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNDeclarante2VinculoX"].ToString());
                float Declarante2VinculoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNDeclarante2VinculoY"].ToString());

                float Declarante2PrenombresPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNDeclarante2NombresX"].ToString());
                float Declarante2PrenombresPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNDeclarante2NombresY"].ToString());

                float Declarante2PrimerApellidoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNDeclarante2PrimerApellidoX"].ToString());
                float Declarante2PrimerApellidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNDeclarante2PrimerApellidoY"].ToString());

                float Declarante2SegundoApellidoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNDeclarante2SegundoApellidoX"].ToString());
                float Declarante2SegundoApellidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNDeclarante2SegundopellidoY"].ToString());

                float Declarante2TipoDocumentoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNDeclarante2DocTipoX"].ToString());
                float Declarante2TipoDocumentoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNDeclarante2DocTipoY"].ToString());

                float Declarante2NumeroDocumentoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNDeclarante2DocNumeroX"].ToString());
                float Declarante2NumeroDocumentoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNDeclarante2DocNumeroY"].ToString());

                float RegistradorNombresCompletosPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNRegistradorNombresX"].ToString());
                float RegistradorNombresCompletosPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNRegistradorNombresY"].ToString());


                float RegistradorNumeroDocumentoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNRegistradorDocNumeroX"].ToString());
                float RegistradorNumeroDocumentoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNRegistradorDocNumeroY"].ToString());

                float ObservacionesPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNObservacionesX"].ToString());
                float ObservacionesPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNObservacionesY"].ToString());

                float ObservacionesPosX_2 = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNObservacionesX_2"].ToString());
                float ObservacionesPosY_2 = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNObservacionesY_2"].ToString());

                float ObservacionesPosX_3 = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNObservacionesX_2"].ToString());
                float ObservacionesPosY_3 = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNObservacionesY_2"].ToString()) + 15;

                float FirmaDeclaranteX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNFirmaDeclaranteX"].ToString());
                float FirmaDeclaranteY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNFirmaDeclaranteY"].ToString());

                float HuellaDeclaranteX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FNHuellaDeclaranteX"].ToString());
                float HuellaDeclaranteY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FNHuellaDeclaranteY"].ToString());

                #endregion

                cb.BeginText();

                String Cui = String.Empty;
                String Lugar = String.Empty;
                String TipoLugar = String.Empty;
                String LugarUbioDep = String.Empty;
                String LugarUbigeoDepDetalle = String.Empty;
                String LugarUbigeoPrv = String.Empty;
                String LugarUbigeoPrvdetalle = String.Empty;
                String LugarUbigeoDst = String.Empty;
                String LugarUbigeoDstDetalle = String.Empty;

                String LugarUbigeocpo = String.Empty;
                String LugarUbigeocpoDetalle = String.Empty;

                String Sexo = String.Empty;
                String SexoDetalle = String.Empty;


                String PreNombres = String.Empty;
                String PrimerApellido = String.Empty;
                String SegundoApellido = String.Empty;

                String PadrePreNombres = String.Empty;
                String PadrePrimerApellido = String.Empty;
                String PadreSegundoApellido = String.Empty;
                String PadreTipoDocumento = String.Empty;
                String PadreNumeroDocumento = String.Empty;
                String PadreNacionalidad = String.Empty;


                String MadrePreNombres = String.Empty;
                String MadrePrimerApellido = String.Empty;
                String MadreSegundoApellido = String.Empty;
                String MadreTipoDocumento = String.Empty;
                String MadreNumeroDocumento = String.Empty;
                String MadreNacionalidad = String.Empty;
                String DireccionUbigeoDep = String.Empty;
                String DireccionUbigeoDepDetalle = String.Empty;
                String DireccionUbigeoPrv = String.Empty;
                String DireccionUbigeoPrvDetalle = String.Empty;
                String DireccionUbigeoDst = String.Empty;
                String DireccionUbigeoDstDetalle = String.Empty;

                String DireccionUbigeoCpo = String.Empty;
                String DireccionUbigeoCpoDetalle = String.Empty;


                String Direccion = String.Empty;

                String RegistroFecha = String.Empty;
                String RegisTroUbigeoDep = String.Empty;
                String RegistroUbigeoDetalle = String.Empty;
                String RegistroUbigeoPrv = String.Empty;
                String RegistroUbigeoPrvDetalle = String.Empty;
                String RegistroUbigeoDst = String.Empty;
                String RegistroUbigeoDstDetalle = String.Empty;
                String RegistroUbigeoCpo = String.Empty;
                String RegistroUbigeoCpoDetalle = String.Empty;

                String Declarante1Vinculo = String.Empty;
                String Declarante1Prenombres = String.Empty;
                String Declarante1PrimerApellido = String.Empty;
                String Declarante1SegundoApellido = String.Empty;
                String Declarante1TipoDocumento = String.Empty;
                String Declarante1NumeroDocumento = String.Empty;

                String Declarante2Vinculo = String.Empty;
                String Declarante2Prenombres = String.Empty;
                String Declarante2PrimerApellido = String.Empty;
                String Declarante2SegundoApellido = String.Empty;
                String Declarante2TipoDocumento = String.Empty;
                String Declarante2NumeroDocumento = String.Empty;

                String RegistradorPrenombres = String.Empty;
                String RegistradorPrimerApellido = String.Empty;
                String RegistradorSegundoApellido = String.Empty;

                String RegistradorNombresCompletos = String.Empty;
                String RegistradorNumeroDocumento = String.Empty;

                String Observaciones = String.Empty;

                //CUI  26-04-2016 se oculto el CUI 
                //Cui =  (dt.Rows[0]["CUI"].ToString()== null)?"":dt.Rows[0]["CUI"].ToString();
                //_EscribirLetraxLetra(CuiPosX + (ejex * 7), CuiPosY, ejex, Cui, cb, document);

                //Fecha de Nacimiento
                DateTime fecha = Comun.FormatearFecha(dt.Rows[0]["Fecha"].ToString());
                string vFecha = fecha.Day.ToString().PadLeft(2, '0') + " " + fecha.Month.ToString().PadLeft(2, '0') + " " + fecha.Year.ToString();
                //string vHora = fecha.Hour.ToString().PadLeft(2, '0') + " " + fecha.Minute.ToString().PadLeft(2, '0') + " " + fecha.ToString("tt").ToUpper().Replace(".", "").Trim();

                TimeSpan timespan = new TimeSpan(fecha.Hour,fecha.Minute,fecha.Second);
                DateTime time = DateTime.Today.Add(timespan);
                string displayTime = time.ToString("hh:mm tt");
                string vHora = displayTime.Replace(":", " ").Replace(". ","").Replace(".","").ToUpper().Trim();

                _EscribirLetraxLetra(fechaPosX + (ejex * (float)8.5), fechaPosY, ejex, vFecha, cb, document);
                _EscribirLetraxLetra(HoraPosX + (ejex * 8), HoraPosY, ejex, vHora, cb, document);

                //Lugar u Tipo Lugar
                if (dt.Rows[0]["Lugar"] != null) Lugar = dt.Rows[0]["Lugar"].ToString();
                if (dt.Rows[0]["LugarTipo"] != null) TipoLugar = dt.Rows[0]["LugarTipo"].ToString();

                if (Lugar.Trim().Length == 0) Lugar = "----------";
                else Lugar = Lugar + "-----";

                EscribirLetraxLetra(LugarPosX + (ejex * 9), LugarPosY + 5, ejex, Lugar, cb, document);
                EscribirLetraxLetra(TipoLugarPosX + (ejex * 9), TipoLugarPosY + 5, ejex, TipoLugar, cb, document);

                //Lugar de Ocurrencia
                if (dt.Rows[0]["LugarUbigeoDep"] != null) LugarUbioDep = dt.Rows[0]["LugarUbigeoDep"].ToString().PadLeft(2, '0');
                if (dt.Rows[0]["LugarUbigeoDepDetalle"] != null) LugarUbigeoDepDetalle = dt.Rows[0]["LugarUbigeoDepDetalle"].ToString();
                if (dt.Rows[0]["LugarUbigeoPrv"] != null) LugarUbigeoPrv = dt.Rows[0]["LugarUbigeoPrv"].ToString().PadLeft(2, '0');
                if (dt.Rows[0]["LugarUbigeoPrvdetalle"] != null) LugarUbigeoPrvdetalle = dt.Rows[0]["LugarUbigeoPrvdetalle"].ToString();
                if (dt.Rows[0]["LugarUbigeoDst"] != null) LugarUbigeoDst = dt.Rows[0]["LugarUbigeoDst"].ToString().PadLeft(2, '0');
                if (dt.Rows[0]["LugarUbigeoDstDetalle"] != null) LugarUbigeoDstDetalle = dt.Rows[0]["LugarUbigeoDstDetalle"].ToString();

                if (dt.Rows[0]["LugarUbigeoCpo"] != null) LugarUbigeocpo = dt.Rows[0]["LugarUbigeoCpo"].ToString().PadLeft(2, '0');

                if (dt.Rows[0]["LugarUbigeoCpoDetalle"] != null) LugarUbigeocpoDetalle = dt.Rows[0]["LugarUbigeoCpoDetalle"].ToString();



                if (LugarUbigeoDepDetalle.Trim().Length == 0) LugarUbigeoDepDetalle = "----------";//----------
                else LugarUbigeoDepDetalle = Util.GenerarLineasubigeoizquierda(LugarUbigeoDepDetalle);// +"-----";

                if (LugarUbigeoPrvdetalle.Trim().Length == 0) LugarUbigeoPrvdetalle = "----------";
                else LugarUbigeoPrvdetalle = Util.GenerarLineasubigeoderecha(LugarUbigeoPrvdetalle);// +"-----";

                if (LugarUbigeoDstDetalle.Trim().Length == 0) LugarUbigeoDstDetalle = "----------";
                else LugarUbigeoDstDetalle = Util.GenerarLineasubigeoizquierda(LugarUbigeoDstDetalle);// +"-----";

                if (LugarUbigeocpoDetalle.Trim().Length == 0) LugarUbigeocpoDetalle = "----------";
                else LugarUbigeocpoDetalle = Util.GenerarLineasubigeoderecha(LugarUbigeocpoDetalle);// + "-----";


                if (LugarUbigeocpo == "00") LugarUbigeocpo = "--";

                EscribirLetraxLetra(LugarUbioDepPosX + (ejex * 9), LugarUbioDepPosY + 6, ejex, LugarUbioDep, cb, document);
                EscribirLetraxLetra(LugarUbigeoDepDetallePosX + (ejex * 9), LugarUbigeoDepDetallePosY + 6, ejex, LugarUbigeoDepDetalle, cb, document);
                EscribirLetraxLetra(LugarUbigeoPrvPosX + (ejex * 9), LugarUbigeoPrvPosY + 6, ejex, LugarUbigeoPrv, cb, document);
                EscribirLetraxLetra(LugarUbigeoPrvdetallePosX + (ejex * 9), LugarUbigeoPrvdetallePosY + 6, ejex, LugarUbigeoPrvdetalle, cb, document);
                EscribirLetraxLetra(LugarUbigeoDstPosX + (ejex * 9), LugarUbigeoDstPosY + 6, ejex, LugarUbigeoDst, cb, document);
                EscribirLetraxLetra(LugarUbigeoDstDetallePosX + (ejex * 9), LugarUbigeoDstDetallePosY + 6, ejex, LugarUbigeoDstDetalle, cb, document);

                EscribirLetraxLetra(LugarUbigeoCPPosX + (ejex * 9), LugarUbigeoCPPosY + 6, ejex, LugarUbigeocpo, cb, document);
                EscribirLetraxLetra(LugarUbigeoCPDetallePosX + (ejex * 9), LugarUbigeoCPDetallePosY + 6, ejex, LugarUbigeocpoDetalle, cb, document);

                //Sexo
                if (dt.Rows[0]["Sexo"] != null) Sexo = dt.Rows[0]["Sexo"].ToString();
                if (dt.Rows[0]["SexoDetalle"] != null) SexoDetalle = dt.Rows[0]["SexoDetalle"].ToString();

                if (SexoDetalle.Trim().Length == 0) SexoDetalle = "----------";
                else SexoDetalle = SexoDetalle + "-----";


                EscribirLetraxLetra(SexoPosX + (ejex * 9), SexoPosY + 4, ejex, Sexo, cb, document);
                EscribirLetraxLetra(SexoDetallePosX + (ejex * 9), SexoDetallePosY + 4, ejex, SexoDetalle, cb, document);

                //Titular
                if (dt.Rows[0]["PreNombres"] != null) PreNombres = dt.Rows[0]["PreNombres"].ToString();
                if (dt.Rows[0]["PrimerApellido"] != null) PrimerApellido = dt.Rows[0]["PrimerApellido"].ToString();
                if (dt.Rows[0]["SegundoApellido"] != null) SegundoApellido = dt.Rows[0]["SegundoApellido"].ToString();

                if (PreNombres.Trim().Length == 0) PreNombres = "----------";
                else PreNombres = PreNombres + "-----";

                if (PrimerApellido.Trim().Length == 0) PrimerApellido = "----------";
                else PrimerApellido = PrimerApellido + "-----";

                if (SegundoApellido.Trim().Length == 0) SegundoApellido = "----------";
                else SegundoApellido = SegundoApellido + "-----";

                EscribirLetraxLetra(PreNombresPosX + (ejex * 9), PreNombresPosY + 4, ejex, PreNombres, cb, document);
                EscribirLetraxLetra(PrimerApellidoPosX + (ejex * 9), PrimerApellidoPosY + 4, ejex, PrimerApellido, cb, document);
                EscribirLetraxLetra(SegundoApellidoPosX + (ejex * 9), SegundoApellidoPosY + 4, ejex, SegundoApellido, cb, document);


                //Padre
                if (dt.Rows[0]["PadrePreNombres"] != null) PadrePreNombres = dt.Rows[0]["PadrePreNombres"].ToString();
                if (dt.Rows[0]["PadrePrimerApellido"] != null) PadrePrimerApellido = dt.Rows[0]["PadrePrimerApellido"].ToString();
                if (dt.Rows[0]["PadreSegundoApellido"] != null) PadreSegundoApellido = dt.Rows[0]["PadreSegundoApellido"].ToString();
                if (dt.Rows[0]["PadreTipoDocumento"] != null) PadreTipoDocumento = dt.Rows[0]["PadreTipoDocumento"].ToString();
                if (dt.Rows[0]["PadreNumeroDocumento"] != null) PadreNumeroDocumento = dt.Rows[0]["PadreNumeroDocumento"].ToString();
                if (dt.Rows[0]["PadreNacionalidad"] != null) PadreNacionalidad = dt.Rows[0]["PadreNacionalidad"].ToString();

                if (PadrePreNombres.Trim().Length == 0) PadrePreNombres = "----------";
                else PadrePreNombres = PadrePreNombres + "-----";

                if (PadrePrimerApellido.Trim().Length == 0) PadrePrimerApellido = "----------";
                else PadrePrimerApellido = PadrePrimerApellido + "-----";

                //if (SegundoApellido.Trim().Length == 0) SegundoApellido = "----------";
                //else SegundoApellido = SegundoApellido + "-----";

                if (PadreSegundoApellido.Trim().Length == 0) PadreSegundoApellido = "----------";
                else PadreSegundoApellido = PadreSegundoApellido + "-----";

                if (PadreTipoDocumento.Trim().Length == 0) PadreTipoDocumento = "--";

                if (PadreTipoDocumento.Equals(Constantes.CONST_DOCUMENTO_PADRE_MADRE)) PadreTipoDocumento = "--";

                if (PadreNumeroDocumento.Trim().Length == 0) { PadreNumeroDocumento = "----------"; PadreTipoDocumento = "--"; }
                
                else PadreNumeroDocumento = Util.GenerarLineasDocumento(PadreNumeroDocumento);// +"-----";

                if (PadreNacionalidad.Trim().Length == 0) PadreNacionalidad = "--";

                EscribirLetraxLetra(PadrePreNombresPosX + (ejex * 9), PadrePreNombresPosY + 4, ejex, PadrePreNombres, cb, document);
                EscribirLetraxLetra(PadrePrimerApellidoPosX + (ejex * 9), PadrePrimerApellidoPosY + 4, ejex, PadrePrimerApellido, cb, document);
                EscribirLetraxLetra(PadreSegundoApellidoPosX + (ejex * 9), PadreSegundoApellidoPosY + 4, ejex, PadreSegundoApellido, cb, document);
                EscribirLetraxLetra(PadreTipoDocumentoPosX + (ejex * 9), PadreTipoDocumentoPosY, ejex, PadreTipoDocumento, cb, document);
                EscribirLetraxLetra(PadreNumeroDocumentoPosX + (ejex * 9), PadreNumeroDocumentoPosY, ejex, PadreNumeroDocumento, cb, document);
                EscribirLetraxLetra(PadreNacionalidadPosX + (ejex * (float)8.5), PadreNacionalidadPosY, ejex, PadreNacionalidad, cb, document);

                //MADRE
                if (dt.Rows[0]["MadrePreNombres"] != null) MadrePreNombres = dt.Rows[0]["MadrePreNombres"].ToString();
                if (dt.Rows[0]["MadrePrimerApellido"] != null) MadrePrimerApellido = dt.Rows[0]["MadrePrimerApellido"].ToString();
                if (dt.Rows[0]["MadreSegundoApellido"] != null) MadreSegundoApellido = dt.Rows[0]["MadreSegundoApellido"].ToString();
                if (dt.Rows[0]["MadreTipoDocumento"] != null) MadreTipoDocumento = dt.Rows[0]["MadreTipoDocumento"].ToString();
                if (dt.Rows[0]["MadreNumeroDocumento"] != null) MadreNumeroDocumento = dt.Rows[0]["MadreNumeroDocumento"].ToString();
                if (dt.Rows[0]["MadreNacionalidad"] != null) MadreNacionalidad = dt.Rows[0]["MadreNacionalidad"].ToString();


                if (dt.Rows[0]["Direccion"] != null) Direccion = dt.Rows[0]["Direccion"].ToString();

                if (dt.Rows[0]["DireccionUbigeoDep"] != null || dt.Rows[0]["DireccionUbigeoDep"].ToString() != "0" && dt.Rows[0]["DireccionUbigeoDep"].ToString() != "00" || dt.Rows[0]["DireccionUbigeoDep"].ToString().Trim() != "") DireccionUbigeoDep = dt.Rows[0]["DireccionUbigeoDep"].ToString().PadLeft(2, '0');
                if (dt.Rows[0]["DireccionUbigeoDepDetalle"] != null && dt.Rows[0]["DireccionUbigeoDepDetalle"].ToString() != "0" && dt.Rows[0]["DireccionUbigeoDepDetalle"].ToString() != "00") DireccionUbigeoDepDetalle = dt.Rows[0]["DireccionUbigeoDepDetalle"].ToString();
                if (dt.Rows[0]["DireccionUbigeoPrv"] != null && dt.Rows[0]["DireccionUbigeoPrv"].ToString() != "0" && dt.Rows[0]["DireccionUbigeoPrv"].ToString() != "00" && dt.Rows[0]["DireccionUbigeoPrv"].ToString().Trim() != "") DireccionUbigeoPrv = dt.Rows[0]["DireccionUbigeoPrv"].ToString().PadLeft(2, '0');
                if (dt.Rows[0]["DireccionUbigeoPrvDetalle"] != null && dt.Rows[0]["DireccionUbigeoPrvDetalle"].ToString() != "0" && dt.Rows[0]["DireccionUbigeoPrvDetalle"].ToString() != "00") DireccionUbigeoPrvDetalle = dt.Rows[0]["DireccionUbigeoPrvDetalle"].ToString();
                if (dt.Rows[0]["DireccionUbigeoDst"] != null && dt.Rows[0]["DireccionUbigeoDst"].ToString() != "0" && dt.Rows[0]["DireccionUbigeoDst"].ToString() != "00" && dt.Rows[0]["DireccionUbigeoDst"].ToString() != "00") DireccionUbigeoDst = dt.Rows[0]["DireccionUbigeoDst"].ToString().PadLeft(2, '0');
                if (dt.Rows[0]["DireccionUbigeoDstDetalle"] != null && dt.Rows[0]["DireccionUbigeoDstDetalle"].ToString() != "0" && dt.Rows[0]["DireccionUbigeoDstDetalle"].ToString() != "00") DireccionUbigeoDstDetalle = dt.Rows[0]["DireccionUbigeoDstDetalle"].ToString();

                if (dt.Rows[0]["DireccionUbigeoCpo"] != null && dt.Rows[0]["DireccionUbigeoCpo"].ToString() != "0" && dt.Rows[0]["DireccionUbigeoCpo"].ToString() != "00" && dt.Rows[0]["DireccionUbigeoCpo"].ToString() != "00") DireccionUbigeoCpo = dt.Rows[0]["DireccionUbigeoCpo"].ToString().PadLeft(2, '0');
                if (dt.Rows[0]["DireccionUbigeoCpoDetalle"] != null && dt.Rows[0]["DireccionUbigeoCpoDetalle"].ToString() != "0" && dt.Rows[0]["DireccionUbigeoCpoDetalle"].ToString() != "00") DireccionUbigeoCpoDetalle = dt.Rows[0]["DireccionUbigeoCpoDetalle"].ToString();


                if (MadrePreNombres.Trim().Length == 0) MadrePreNombres = "----------";
                else MadrePreNombres = MadrePreNombres + "-----";

                if (MadrePrimerApellido.Trim().Length == 0) MadrePrimerApellido = "----------";
                else MadrePrimerApellido = MadrePrimerApellido + "-----";

                if (MadreSegundoApellido.Trim().Length == 0) MadreSegundoApellido = "----------";
                else MadreSegundoApellido = MadreSegundoApellido + "-----";

                if (MadreTipoDocumento.Trim().Length == 0) MadreTipoDocumento = "--";
                else MadreTipoDocumento = MadreTipoDocumento + "";

                if (MadreTipoDocumento.Equals(Constantes.CONST_DOCUMENTO_PADRE_MADRE)) MadreTipoDocumento = "--";

                if (MadreNumeroDocumento.Trim().Length == 0) { MadreNumeroDocumento = "----------"; MadreTipoDocumento = "--"; }
                else { MadreNumeroDocumento = Util.GenerarLineasDocumento(MadreNumeroDocumento); }// +"-----";

                if (MadreNacionalidad.Trim().Length == 0) MadreNacionalidad = "--";
                  
                if (Direccion.Trim().Length == 0) Direccion = "----------";
                else Direccion = Direccion + "-----";
                 
                if (DireccionUbigeoDepDetalle.Trim().Length == 0) DireccionUbigeoDepDetalle = "----------";
                else DireccionUbigeoDepDetalle = Util.GenerarLineasubigeoizquierda(DireccionUbigeoDepDetalle);// +"-----";

                if (DireccionUbigeoPrvDetalle.Trim().Length == 0) DireccionUbigeoPrvDetalle = "----------";
                else DireccionUbigeoPrvDetalle = Util.GenerarLineasubigeoderecha(DireccionUbigeoPrvDetalle);// +"-----";

                if (DireccionUbigeoDstDetalle.Trim().Length == 0) DireccionUbigeoDstDetalle = "------";
                else DireccionUbigeoDstDetalle = Util.GenerarLineasubigeoizquierda(DireccionUbigeoDstDetalle);// +"---";

                if (DireccionUbigeoCpoDetalle.Trim().Length == 0) DireccionUbigeoCpoDetalle = "------";
                else DireccionUbigeoCpoDetalle = Util.GenerarLineasubigeoderecha(DireccionUbigeoCpoDetalle);// +"---";

                if (DireccionUbigeoCpo == "00" || DireccionUbigeoCpo.Trim() == "") DireccionUbigeoCpo = "--";
                if (DireccionUbigeoDep == "00" || DireccionUbigeoDep.Trim() == "") DireccionUbigeoDep = "--";
                if (DireccionUbigeoPrv == "00" || DireccionUbigeoPrv.Trim() == "") DireccionUbigeoPrv = "--";
                if (DireccionUbigeoDst == "00" || DireccionUbigeoDst.Trim() == "") DireccionUbigeoDst = "--";


                EscribirLetraxLetra(MadrePreNombresPosX + (ejex * 9), MadrePreNombresPosY, ejex, MadrePreNombres, cb, document);
                EscribirLetraxLetra(MadrePrimerApellidoPosX + (ejex * 9), MadrePrimerApellidoPosY, ejex, MadrePrimerApellido, cb, document);
                EscribirLetraxLetra(MadreSegundoApellidoPosX + (ejex * 9), MadreSegundoApellidoPosY, ejex, MadreSegundoApellido, cb, document);
                EscribirLetraxLetra(MadreTipoDocumentoPosX + (ejex * 9), MadreTipoDocumentoPosY, ejex, MadreTipoDocumento, cb, document);
                EscribirLetraxLetra(MadreNumeroDocumentoPosX + (ejex * 9), MadreNumeroDocumentoPosY, ejex, MadreNumeroDocumento, cb, document);
                EscribirLetraxLetra(MadreNacionalidadPosX + (ejex * 9), MadreNacionalidadPosY, ejex, MadreNacionalidad, cb, document);

                EscribirLetraxLetra(DireccionPosX + (ejex * 9), DireccionPosY, ejex, Direccion, cb, document);

                EscribirLetraxLetra(DireccionUbigeoDepPosX + (ejex * 9), DireccionUbigeoDepPosY, ejex, DireccionUbigeoDep, cb, document);
                EscribirLetraxLetra(DireccionUbigeoDepDetallePosX + (ejex * 9), DireccionUbigeoDepDetallePosY, ejex, DireccionUbigeoDepDetalle, cb, document);
                EscribirLetraxLetra(DireccionUbigeoPrvPosX + (ejex * 9), DireccionUbigeoPrvPosY, ejex, DireccionUbigeoPrv, cb, document);

                EscribirLetraxLetra(DireccionUbigeoPrvDetallePosX + (ejex * 9), DireccionUbigeoPrvDetallePosY, ejex, DireccionUbigeoPrvDetalle, cb, document);


                EscribirLetraxLetra(DireccionUbigeoDstPosX + (ejex * 9), DireccionUbigeoDstPosY + 4, ejex, DireccionUbigeoDst, cb, document);
                EscribirLetraxLetra(DireccionUbigeoDstDetallePosX + (ejex * 9), DireccionUbigeoDstDetallePosY + 4, ejex, DireccionUbigeoDstDetalle, cb, document);

                EscribirLetraxLetra(DireccionUbigeoCPPosX + (ejex * 9), DireccionUbigeoCPPosY + 4, ejex, DireccionUbigeoCpo, cb, document);
                EscribirLetraxLetra(DireccionUbigeoCPDetallePosX + (ejex * 9), DireccionUbigeoCPDetallePosY + 4, ejex, DireccionUbigeoCpoDetalle, cb, document);



                //REGISTRADOR
                DateTime fechaRegistrador = Comun.FormatearFecha(dt.Rows[0]["RegistroFecha"].ToString());
                RegistroFecha = fechaRegistrador.Day.ToString().PadLeft(2, '0') + " " + fechaRegistrador.Month.ToString().PadLeft(2, '0') + " " + fechaRegistrador.Year.ToString();

                if (dt.Rows[0]["RegistroUbigeoDep"] != null) RegisTroUbigeoDep = dt.Rows[0]["RegistroUbigeoDep"].ToString().PadLeft(2, '0');
                if (RegisTroUbigeoDep.Trim().Length == 0) RegisTroUbigeoDep = "--";

                if (dt.Rows[0]["RegistroUbigeoDepDetalle"] != null) RegistroUbigeoDetalle = dt.Rows[0]["RegistroUbigeoDepDetalle"].ToString();
                if (RegistroUbigeoDetalle.Trim().Length == 0) RegistroUbigeoDetalle = "----------";
                else RegistroUbigeoDetalle = Util.GenerarLineasubigeoizquierda(RegistroUbigeoDetalle);// +"-----";

                if (dt.Rows[0]["RegistroUbigeoPrv"] != null) RegistroUbigeoPrv = dt.Rows[0]["RegistroUbigeoPrv"].ToString().PadLeft(2, '0');
                if (RegistroUbigeoPrv.Trim().Length == 0) RegistroUbigeoPrv = "--";

                if (dt.Rows[0]["RegistroUbigeoPrvDetalle"] != null) RegistroUbigeoPrvDetalle = dt.Rows[0]["RegistroUbigeoPrvDetalle"].ToString();
                if (RegistroUbigeoPrvDetalle.Trim().Length == 0) RegistroUbigeoPrvDetalle = "----------";
                else RegistroUbigeoPrvDetalle = Util.GenerarLineasubigeoderecha(RegistroUbigeoPrvDetalle);// +"------";

                if (dt.Rows[0]["RegistroUbigeoDst"] != null) RegistroUbigeoDst = dt.Rows[0]["RegistroUbigeoDst"].ToString().PadLeft(2, '0');
                if (RegistroUbigeoDst.Trim().Length == 0) RegistroUbigeoDst = "--";

                if (dt.Rows[0]["RegistroUbigeoDstDetalle"] != null) RegistroUbigeoDstDetalle = dt.Rows[0]["RegistroUbigeoDstDetalle"].ToString();
                if (RegistroUbigeoDstDetalle.Trim().Length == 0) RegistroUbigeoDstDetalle = "----------";
                else RegistroUbigeoDstDetalle = Util.GenerarLineasubigeoizquierda(RegistroUbigeoDstDetalle);// +"-----";

                //if (dt.Rows[0]["RegistroUbigeoDstDetalle"] != null) RegistroUbigeoDstDetalle = dt.Rows[0]["RegistroUbigeoDstDetalle"].ToString();
                //if (RegistroUbigeoDstDetalle.Trim().Length == 0) RegistroUbigeoDstDetalle = "----------";
                //else RegistroUbigeoDstDetalle = RegistroUbigeoDstDetalle + "-----";

                if (dt.Rows[0]["RegistroUbigeoCpo"] != null) RegistroUbigeoCpo = dt.Rows[0]["RegistroUbigeoCpo"].ToString();
                if (RegistroUbigeoCpo.Trim().Length == 0) RegistroUbigeoCpo = "--";


                if (dt.Rows[0]["RegistroUbigeoCpoDetalle"] != null) RegistroUbigeoCpoDetalle = dt.Rows[0]["RegistroUbigeoCpoDetalle"].ToString();
                if (RegistroUbigeoCpoDetalle.Trim().Length == 0) RegistroUbigeoCpoDetalle = "----------";
                else RegistroUbigeoCpoDetalle = Util.GenerarLineasubigeoderecha(RegistroUbigeoCpoDetalle);// +"-----";

                if (dt.Rows[0]["RegistradorPrenombres"] != null) RegistradorPrenombres = dt.Rows[0]["RegistradorPrenombres"].ToString();
                if (dt.Rows[0]["RegistradorPrimerApellido"] != null) RegistradorPrimerApellido = dt.Rows[0]["RegistradorPrimerApellido"].ToString();
                if (dt.Rows[0]["RegistradorSegundoApellido"] != null) RegistradorSegundoApellido = dt.Rows[0]["RegistradorSegundoApellido"].ToString();


                if (dt.Rows[0]["RegistradorNumeroDocumento"] != null) RegistradorNumeroDocumento = dt.Rows[0]["RegistradorNumeroDocumento"].ToString();

                RegistradorNombresCompletos = RegistradorPrimerApellido + " " + RegistradorSegundoApellido + " " + RegistradorPrenombres;

                if (RegistradorNombresCompletos.Trim().Length == 0) RegistradorNombresCompletos = "----------";
                else RegistradorNombresCompletos = RegistradorNombresCompletos + "-----";

                if (RegistradorNumeroDocumento.Trim().Length == 0) RegistradorNumeroDocumento = "----------";
                else RegistradorNumeroDocumento = Util.GenerarLineasDocumento(RegistradorNumeroDocumento);// +"-----";

                _EscribirLetraxLetra(RegistroFechaPosX + (ejex * 9), RegistroFechaPosY, ejex, RegistroFecha, cb, document);
                EscribirLetraxLetra(RegisTroUbigeoDepPosX + (ejex * 9), RegisTroUbigeoDepPosY, ejex, RegisTroUbigeoDep, cb, document);
                EscribirLetraxLetra(RegistroUbigeoDetallePosX + (ejex * 9), RegistroUbigeoDetallePosY, ejex, RegistroUbigeoDetalle, cb, document);
                EscribirLetraxLetra(RegistroUbigeoPrvPosX + (ejex * 9), RegistroUbigeoPrvPosY, ejex, RegistroUbigeoPrv, cb, document);
                EscribirLetraxLetra(RegistroUbigeoPrvDetallePosX + (ejex * 9), RegistroUbigeoPrvDetallePosY, ejex, RegistroUbigeoPrvDetalle, cb, document);


                EscribirLetraxLetra(RegistroUbigeoDstPosX + (ejex * 9), RegistroUbigeoDstPosY, ejex, RegistroUbigeoDst, cb, document);
                EscribirLetraxLetra(RegistroUbigeoDstDetallePosX + (ejex * 9), RegistroUbigeoDstDetallePosY, ejex, RegistroUbigeoDstDetalle, cb, document);

                EscribirLetraxLetra(RegistroUbigeoCPPosX + (ejex * 9), RegistroUbigeoCPPosY, ejex, RegistroUbigeoCpo, cb, document);
                EscribirLetraxLetra(RegistroUbigeoCPDetallePosX + (ejex * 9), RegistroUbigeoCPDetallePosY, ejex, RegistroUbigeoCpoDetalle, cb, document);

                EscribirLetraxLetra(RegistradorNombresCompletosPosX + (ejex * 9), RegistradorNombresCompletosPosY, ejex, RegistradorNombresCompletos, cb, document);
                EscribirLetraxLetra(RegistradorNumeroDocumentoPosX + (ejex * 9), RegistradorNumeroDocumentoPosY, ejex, RegistradorNumeroDocumento, cb, document);


                //DECLARANTE 1

                if (dt.Rows[0]["Declarante1Vinculo"] != null && dt.Rows[0]["Declarante1Vinculo"].ToString() != "0" && dt.Rows[0]["Declarante1Vinculo"].ToString() != "00") Declarante1Vinculo = dt.Rows[0]["Declarante1Vinculo"].ToString();
                if (dt.Rows[0]["Declarante1Prenombres"] != null && dt.Rows[0]["Declarante1Prenombres"].ToString() != "0" && dt.Rows[0]["Declarante1Prenombres"].ToString() != "00") Declarante1Prenombres = dt.Rows[0]["Declarante1Prenombres"].ToString();
                if (dt.Rows[0]["Declarante1PrimerApellido"] != null && dt.Rows[0]["Declarante1PrimerApellido"].ToString() != "0" && dt.Rows[0]["Declarante1PrimerApellido"].ToString() != "00") Declarante1PrimerApellido = dt.Rows[0]["Declarante1PrimerApellido"].ToString();
                if (dt.Rows[0]["Declarante1SegundoApellido"] != null && dt.Rows[0]["Declarante1SegundoApellido"].ToString() != "0" && dt.Rows[0]["Declarante1SegundoApellido"].ToString() != "00") Declarante1SegundoApellido = dt.Rows[0]["Declarante1SegundoApellido"].ToString();
                if (dt.Rows[0]["Declarante1TipoDocumento"] != null && dt.Rows[0]["Declarante1TipoDocumento"].ToString() != "0" && dt.Rows[0]["Declarante1TipoDocumento"].ToString() != "00") Declarante1TipoDocumento = dt.Rows[0]["Declarante1TipoDocumento"].ToString();
                if (dt.Rows[0]["Declarante1NumeroDocumento"] != null && dt.Rows[0]["Declarante1NumeroDocumento"].ToString() != "0" && dt.Rows[0]["Declarante1NumeroDocumento"].ToString() != "00") Declarante1NumeroDocumento = dt.Rows[0]["Declarante1NumeroDocumento"].ToString();

                if (Declarante1Vinculo.Trim().Length == 0) Declarante1Vinculo = "----------";
                else Declarante1Vinculo = Util.GenerarLineasVinculo(Declarante1Vinculo);// +"-----";

                if (Declarante1Prenombres.Trim().Length == 0) Declarante1Prenombres = "----------";
                else Declarante1Prenombres = Declarante1Prenombres + "-----";

                if (Declarante1PrimerApellido.Trim().Length == 0) Declarante1PrimerApellido = "----------";
                else Declarante1PrimerApellido = Declarante1PrimerApellido + "-----";

                if (Declarante1SegundoApellido.Trim().Length == 0) Declarante1SegundoApellido = "----------";
                else Declarante1SegundoApellido = Declarante1SegundoApellido + "-----";

                if (Declarante1TipoDocumento.Trim().Length == 0) Declarante1TipoDocumento = "--";
                else Declarante1TipoDocumento = Declarante1TipoDocumento + "";


                if (Declarante1NumeroDocumento.Trim().Length == 0) { Declarante1NumeroDocumento = "----------"; Declarante1TipoDocumento = "--"; }
                else { Declarante1NumeroDocumento = Util.GenerarLineasDocumento(Declarante1NumeroDocumento); }// +"-----";

                EscribirLetraxLetra(Declarante1VinculoPosX + (ejex * 9), Declarante1VinculoPosY, ejex, Declarante1Vinculo, cb, document);
                EscribirLetraxLetra(Declarante1PrenombresPosX + (ejex * 9), Declarante1PrenombresPosY, ejex, Declarante1Prenombres, cb, document);
                EscribirLetraxLetra(Declarante1PrimerApellidoPosX + (ejex * 9), Declarante1PrimerApellidoPosY, ejex, Declarante1PrimerApellido, cb, document);
                EscribirLetraxLetra(Declarante1SegundoApellidoPosX + (ejex * 9), Declarante1SegundoApellidoPosY, ejex, Declarante1SegundoApellido, cb, document);
                EscribirLetraxLetra(Declarante1TipoDocumentoPosX + (ejex * 9), Declarante1TipoDocumentoPosY, ejex, Declarante1TipoDocumento, cb, document);
                EscribirLetraxLetra(Declarante1NumeroDocumentoPosX + (ejex * 9), Declarante1NumeroDocumentoPosY, ejex, Declarante1NumeroDocumento, cb, document);

                //DECLARANTE 2

                if (dt.Rows[0]["Declarante2Vinculo"] != null && dt.Rows[0]["Declarante2Vinculo"].ToString() != "0" && dt.Rows[0]["Declarante2Vinculo"].ToString() != "00") Declarante2Vinculo = dt.Rows[0]["Declarante2Vinculo"].ToString();
                if (dt.Rows[0]["Declarante2Prenombres"] != null && dt.Rows[0]["Declarante2Prenombres"].ToString() != "0" && dt.Rows[0]["Declarante2Prenombres"].ToString() != "00") Declarante2Prenombres = dt.Rows[0]["Declarante2Prenombres"].ToString();
                if (dt.Rows[0]["Declarante2PrimerApellido"] != null && dt.Rows[0]["Declarante2PrimerApellido"].ToString() != "0" && dt.Rows[0]["Declarante2PrimerApellido"].ToString() != "00") Declarante2PrimerApellido = dt.Rows[0]["Declarante2PrimerApellido"].ToString();
                if (dt.Rows[0]["Declarante2SegundoApellido"] != null && dt.Rows[0]["Declarante2SegundoApellido"].ToString() != "0" && dt.Rows[0]["Declarante2SegundoApellido"].ToString() != "00") Declarante2SegundoApellido = dt.Rows[0]["Declarante2SegundoApellido"].ToString();
                if (dt.Rows[0]["Declarante2TipoDocumento"] != null && dt.Rows[0]["Declarante2TipoDocumento"].ToString() != "0" && dt.Rows[0]["Declarante2TipoDocumento"].ToString() != "00") Declarante2TipoDocumento = dt.Rows[0]["Declarante2TipoDocumento"].ToString();
                if (dt.Rows[0]["Declarante2NumeroDocumento"] != null && dt.Rows[0]["Declarante2NumeroDocumento"].ToString() != "0" && dt.Rows[0]["Declarante2NumeroDocumento"].ToString() != "00") Declarante2NumeroDocumento = dt.Rows[0]["Declarante2NumeroDocumento"].ToString();

                if (Declarante2Vinculo.Trim().Length == 0) Declarante2Vinculo = "----------";
                else Declarante2Vinculo = Util.GenerarLineasVinculo(Declarante2Vinculo);// +"-----";

                if (Declarante2Prenombres.Trim().Length == 0) Declarante2Prenombres = "----------";
                else Declarante2Prenombres = Declarante2Prenombres + "-----";

                if (Declarante2PrimerApellido.Trim().Length == 0) Declarante2PrimerApellido = "----------";
                else Declarante2PrimerApellido = Declarante2PrimerApellido + "-----";

                if (Declarante2SegundoApellido.Trim().Length == 0) Declarante2SegundoApellido = "----------";
                else Declarante2SegundoApellido = Declarante2SegundoApellido + "-----";

                if (Declarante2TipoDocumento.Trim().Length == 0) Declarante2TipoDocumento = "--";
                else Declarante2TipoDocumento = Declarante2TipoDocumento + "";



                if (Declarante2NumeroDocumento.Trim().Length == 0) { Declarante2NumeroDocumento = "----------"; Declarante2TipoDocumento = "--"; }
                else { Declarante2NumeroDocumento = Util.GenerarLineasDocumento(Declarante2NumeroDocumento); }// +"-----";

                EscribirLetraxLetra(Declarante2VinculoPosX + (ejex * 9), Declarante2VinculoPosY, ejex, Declarante2Vinculo, cb, document);
                EscribirLetraxLetra(Declarante2PrenombresPosX + (ejex * 9), Declarante2PrenombresPosY, ejex, Declarante2Prenombres, cb, document);
                EscribirLetraxLetra(Declarante2PrimerApellidoPosX + (ejex * 9), Declarante2PrimerApellidoPosY, ejex, Declarante2PrimerApellido, cb, document);
                EscribirLetraxLetra(Declarante2SegundoApellidoPosX + (ejex * 9), Declarante2SegundoApellidoPosY, ejex, Declarante2SegundoApellido, cb, document);
                EscribirLetraxLetra(Declarante2TipoDocumentoPosX + (ejex * 9), Declarante2TipoDocumentoPosY, ejex, Declarante2TipoDocumento, cb, document);
                EscribirLetraxLetra(Declarante2NumeroDocumentoPosX + (ejex * 9), Declarante2NumeroDocumentoPosY, ejex, Declarante2NumeroDocumento, cb, document);

                //OBSERVACION  
                if (dt.Rows[0]["Observaciones"] != null) Observaciones = dt.Rows[0]["Observaciones"].ToString();

                if (Observaciones.Trim().Length == 0) Observaciones = "";
                else Observaciones = Observaciones + "-----";

                string Observaciones_2 = string.Empty;
                if (Observaciones == "LEY N° 30738 LEY DE REFORMA DEL ARTÍCULO N° 52 DE LA CONSTITUCIÓN POLÍTICA DEL PERÚ-----")
                {
                    cb.SetFontAndSize(bfTimes, 8);
                    //Observaciones = "LEY N° 30738 LEY DE REFORMA DEL ARTICULO N° 52 DE LA CONSTITUCIÓN";
                    //Observaciones_2 = "POLÍTICA DEL PERÚ" + "-----";
                }
                //--------------------------------------------------------------
                //Fecha: 23/08/2018
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Dividir la observación de dos lineas de 69 caracteres
                //--------------------------------------------------------------
                string strObservacion_1 = "";
                string strObservacion_2 = "";
                string strObservacion_3 = "";
                string strPalabra = "";
                string strPalabra_2 = "";
                string[] strListaObservacion = Observaciones.Split(' ');

                for (int i = 0; i < strListaObservacion.Length; i++)
                {
                    strPalabra = strPalabra + strListaObservacion[i] + " ";

                    if (strPalabra.Length <= 70)
                    {
                        strObservacion_1 = strPalabra;
                    }
                    else
                    {
                        strPalabra_2 = strPalabra_2 + strListaObservacion[i] + " ";
                        if (strPalabra_2.Length <= 80)
                        {
                            strObservacion_2 = strPalabra_2;
                        }
                        else {
                            strObservacion_3 = strObservacion_3 + strListaObservacion[i] + " ";
                        }
                    }                                        
                }

                EscribirLetraxLetra(ObservacionesPosX + (ejex * 9), ObservacionesPosY, ejex, strObservacion_1, cb, document);
                EscribirLetraxLetra(ObservacionesPosX_2 + (ejex * 9), ObservacionesPosY_2, ejex, strObservacion_2, cb, document);
                EscribirLetraxLetra(ObservacionesPosX_2 + (ejex * 9), ObservacionesPosY_3, ejex, strObservacion_3, cb, document);
                cb.SetFontAndSize(bfTimes, 11);
                if ((Declarante1NumeroDocumento == "----------" && Declarante1Prenombres == "----------") || (Declarante2NumeroDocumento == "----------" && Declarante2Prenombres == "----------"))
                {
                    string firma_huella_declarante = "----------";

                    EscribirLetraxLetraNacimientoObservacion(FirmaDeclaranteX + (ejex * 9), FirmaDeclaranteY, ejex, firma_huella_declarante, cb, document);
                    EscribirLetraxLetraNacimientoObservacion(HuellaDeclaranteX + (ejex * 9), HuellaDeclaranteY, ejex, firma_huella_declarante, cb, document);
                }


//                cb.SetTextMatrix(ObservacionesPosX, document.PageSize.Height - ObservacionesPosY);
//                cb.ShowText(Observaciones.ToString());


                //Miguel Márquez Beltrán
                //Fecha: 26/09/2016

                //Byte[] FileBuffer = ms.ToArray();
                //if (FileBuffer != null)
                //{
                //    System.Web.HttpContext.Current.Session["binaryData"] = FileBuffer;
                //}
                //--------------------------
                cb.EndText();
                document.Close();
                oStreamReader.Close();
                writer.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void CreateFilePDFDefuncion(DataTable dt, string HtmlPath, string PdfPath)
        {
            int iValorY = 5;
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

                iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                iTextSharp.text.pdf.BaseFont bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);

                cb.SetFontAndSize(bfTimes, 11);


                float ejex = (float)11.6;
                float fOrigenX = -39;
                float fOrigenY_ = document.PageSize.Height - 70 - 18;
                float fOrigenX_ = document.PageSize.Width;
                float fOrigenY = 18;

                #region PosicionesY


                float fechaPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDFechaY"].ToString());
                float fechaPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDFechaX"].ToString());

                float HoraPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDHoraY"].ToString());
                float HoraPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDHoraX"].ToString());

                float TipoLugarPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDLugarTipoY"].ToString());
                float TipoLugarPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDLugarTipoX"].ToString());

                float LugarPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDLugarOcurrenciaY"].ToString());
                float LugarPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDLugarOcurrenciaX"].ToString());

                float LugarUbioDepPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi01X"].ToString());
                float LugarUbioDepPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi01Y"].ToString());

                float LugarUbigeoDepDetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi01DescX"].ToString());
                float LugarUbigeoDepDetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi01DescY"].ToString());

                float LugarUbigeoPrvPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi02X"].ToString());
                float LugarUbigeoPrvPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi02Y"].ToString());

                float LugarUbigeoPrvdetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi02DescX"].ToString());
                float LugarUbigeoPrvdetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi02DescY"].ToString());

                float LugarUbigeoDstPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi03X"].ToString());
                float LugarUbigeoDstPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi03Y"].ToString());

                float LugarUbigeoDstDetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi03DescX"].ToString());
                float LugarUbigeoDstDetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi03DescY"].ToString());

                float LugarUbigeoCPPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi04X"].ToString());
                float LugarUbigeoCPPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi04Y"].ToString());

                float LugarUbigeoCPDetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi04DescX"].ToString());
                float LugarUbigeoCPDetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi04DescY"].ToString());


                float PreNombresFallecidoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDNombresFallecidoX"].ToString());
                float PreNombresFallecidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDNombresFallecidoY"].ToString());

                float PrimerApellidoFallecidoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDPrimerApellidoFallecidoX"].ToString());
                float PrimerApellidoFallecidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDPrimerApellidoFallecidoY"].ToString());

                float SegundoApellidoFallecidoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDSegundoApellidoFallecidoX"].ToString());
                float SegundoApellidoFallecidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDSegundopellidoFallecidoY"].ToString());



                float FallecidoTipoDocumentoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDFallecidoDocTipoX"].ToString());
                float FallecidoTipoDocumentoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDFallecidoDocTipoY"].ToString());

                float FallecidoNumeroDocumentoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDFallecidoDocNumeroX"].ToString());
                float FallecidoNumeroDocumentoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDFallecidoDocNumeroY"].ToString());

                float FallecidoEdadPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDFallecidoEdadX"].ToString());
                float FallecidoEdadPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDFallecidoEdadY"].ToString());


                float FallecidoTipoNacionalidadPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDFallecidoTipoNacionalidadX"].ToString());
                float FallecidoTipoNacionalidadPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDFallecidoTipoNacionalidadY"].ToString());

                float FallecidoNacionalidadPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDFallecidoNacionalidadX"].ToString());
                float FallecidoNacionalidadPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDFallecidoNacionalidadY"].ToString());



                float FallecidoLugarUbioDepPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi01FallecidoX"].ToString());
                float FallecidoLugarUbioDepPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi01FallecidoY"].ToString());

                float FallecidoLugarUbigeoDepDetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi01DescFallecidoX"].ToString());
                float FallecidoLugarUbigeoDepDetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi01DescFallecidoY"].ToString());

                float FallecidoLugarUbigeoPrvPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi02FallecidoX"].ToString());
                float FallecidoLugarUbigeoPrvPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi02FallecidoY"].ToString());

                float FallecidoLugarUbigeoPrvdetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi02DescFallecidoX"].ToString());
                float FallecidoLugarUbigeoPrvdetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi02DescFallecidoY"].ToString());

                float FallecidoLugarUbigeoDstPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi03FallecidoX"].ToString());
                float FallecidoLugarUbigeoDstPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi03FallecidoY"].ToString());

                float FallecidoLugarUbigeoDstDetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi03DescFallecidoX"].ToString());
                float FallecidoLugarUbigeoDstDetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi03DescFallecidoY"].ToString());


                float FallecidoLugarUbigeoCPPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi04FallecidoX"].ToString());
                float FallecidoLugarUbigeoCPPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi04FallecidoY"].ToString());

                float FallecidoLugarUbigeoCPDetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi04DescFallecidoX"].ToString());
                float FallecidoLugarUbigeoCPDetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDLugarUbi04DescFallecidoY"].ToString());



                float PadrePreNombresPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDPadreNombresX"].ToString());
                float PadrePreNombresPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDPadreNombresY"].ToString());

                float PadrePrimerApellidoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDPadrePrimerApellidoX"].ToString());
                float PadrePrimerApellidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDPadrePrimerApellidoY"].ToString());

                float PadreSegundoApellidoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDPadreSegundopellidoX"].ToString());
                float PadreSegundoApellidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDPadreSegundopellidoY"].ToString());

                float MadrePreNombresPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDMadreNombresX"].ToString());
                float MadrePreNombresPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDMadreNombresY"].ToString());

                float MadrePrimerApellidoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDMadrePrimerApellidoX"].ToString());
                float MadrePrimerApellidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDMadrePrimerApellidoY"].ToString());

                float MadreSegundoApellidoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDMadreSegundoApellidoX"].ToString());
                float MadreSegundoApellidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDMadreSegundopellidoY"].ToString());


                float RegistroFechaPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDRegistroFechaX"].ToString());
                float RegistroFechaPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDRegistroFechaY"].ToString());

                float RegisTroUbigeoDepPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDRegistroUbi01X"].ToString());
                float RegisTroUbigeoDepPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDRegistroUbi01Y"].ToString());

                float RegistroUbigeoDetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDRegistroUbi01DescX"].ToString());
                float RegistroUbigeoDetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDRegistroUbi01DescY"].ToString());

                float RegistroUbigeoPrvPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDRegistroUbi02X"].ToString());
                float RegistroUbigeoPrvPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDRegistroUbi02Y"].ToString());

                float RegistroUbigeoPrvDetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDRegistroUbi02DescX"].ToString());
                float RegistroUbigeoPrvDetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDRegistroUbi02DescY"].ToString());

                float RegistroUbigeoDstPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDRegistroUbi03X"].ToString());
                float RegistroUbigeoDstPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDRegistroUbi03Y"].ToString());

                float RegistroUbigeoDstDetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDRegistroUbi03DescX"].ToString());
                float RegistroUbigeoDstDetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDRegistroUbi03DescY"].ToString());


                float RegistroUbigeoCPPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDRegistroUbi04X"].ToString());
                float RegistroUbigeoCPPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDRegistroUbi04Y"].ToString());

                float RegistroUbigeoCPDetallePosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDRegistroUbi04DescX"].ToString());
                float RegistroUbigeoCPDetallePosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDRegistroUbi04DescY"].ToString());


                float Declarante1PrenombresPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDDeclarante1NombresX"].ToString());
                float Declarante1PrenombresPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDDeclarante1NombresY"].ToString());

                float Declarante1PrimerApellidoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDDeclarante1PrimerApellidoX"].ToString());
                float Declarante1PrimerApellidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDDeclarante1PrimerApellidoY"].ToString());

                float Declarante1SegundoApellidoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDDeclarante1SegundoApellidoX"].ToString());
                float Declarante1SegundoApellidoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDDeclarante1SegundopellidoY"].ToString());

                float Declarante1TipoDocumentoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDDeclarante1DocTipoX"].ToString());
                float Declarante1TipoDocumentoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDDeclarante1DocTipoY"].ToString());

                float Declarante1NumeroDocumentoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDDeclarante1DocNumeroX"].ToString());
                float Declarante1NumeroDocumentoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDDeclarante1DocNumeroY"].ToString());


                float RegistradorNombresCompletosPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDRegistradorNombresX"].ToString());
                float RegistradorNombresCompletosPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDRegistradorNombresY"].ToString());


                float RegistradorNumeroDocumentoPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDRegistradorDocNumeroX"].ToString());
                float RegistradorNumeroDocumentoPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDRegistradorDocNumeroY"].ToString());

                float ObservacionesPosX = fOrigenX + float.Parse(ConfigurationManager.AppSettings["FDObservacionesX"].ToString());
                float ObservacionesPosY = fOrigenY + float.Parse(ConfigurationManager.AppSettings["FDObservacionesY"].ToString());

                #endregion

                cb.BeginText();

                String Lugar = String.Empty;
                String TipoLugar = String.Empty;
                String LugarUbioDep = String.Empty;
                String LugarUbigeoDepDetalle = String.Empty;
                String LugarUbigeoPrv = String.Empty;
                String LugarUbigeoPrvdetalle = String.Empty;
                String LugarUbigeoDst = String.Empty;
                String LugarUbigeoDstDetalle = String.Empty;

                String LugarUbigeoCpo = String.Empty;
                String LugarUbigeoCpoDetalle = String.Empty;

                String Sexo = String.Empty;
                String SexoDetalle = String.Empty;

                String PreNombresFallecido = String.Empty;
                String PrimerApellidoFallecido = String.Empty;
                String SegundoApellidoFallecido = String.Empty;
                String TipoDocumentoFallecido = String.Empty;
                String NumeroDocumentoFallecido = String.Empty;
                String EdadFallecido = String.Empty;
                String TipoNacionalidadFallecido = String.Empty;
                String NacionalidadFallecido = String.Empty;

                String FallecidoLugarUbioDep = String.Empty;
                String FallecidoLugarUbigeoDepDetalle = String.Empty;
                String FallecidoLugarUbigeoPrv = String.Empty;
                String FallecidoLugarUbigeoPrvdetalle = String.Empty;
                String FallecidoLugarUbigeoDst = String.Empty;
                String FallecidoLugarUbigeoDstDetalle = String.Empty;

                String FallecidoLugarUbigeoCpo = String.Empty;
                String FallecidoLugarUbigeoCpoDetalle = String.Empty;



                String PadrePreNombres = String.Empty;
                String PadrePrimerApellido = String.Empty;
                String PadreSegundoApellido = String.Empty;

                String MadrePreNombres = String.Empty;
                String MadrePrimerApellido = String.Empty;
                String MadreSegundoApellido = String.Empty;

                String RegistroFecha = String.Empty;
                String RegisTroUbigeoDep = String.Empty;
                String RegistroUbigeoDetalle = String.Empty;
                String RegistroUbigeoPrv = String.Empty;
                String RegistroUbigeoPrvDetalle = String.Empty;
                String RegistroUbigeoDst = String.Empty;
                String RegistroUbigeoDstDetalle = String.Empty;

                String RegistroUbigeoCpo = String.Empty;
                String RegistroUbigeoCpoDetalle = String.Empty;

                String Declarante1Vinculo = String.Empty;
                String Declarante1Prenombres = String.Empty;
                String Declarante1PrimerApellido = String.Empty;
                String Declarante1SegundoApellido = String.Empty;
                String Declarante1TipoDocumento = String.Empty;
                String Declarante1NumeroDocumento = String.Empty;

                String RegistradorPrenombres = String.Empty;
                String RegistradorPrimerApellido = String.Empty;
                String RegistradorSegundoApellido = String.Empty;

                String RegistradorNombresCompletos = String.Empty;
                String RegistradorNumeroDocumento = String.Empty;

                String Observaciones = String.Empty;

                //Fecha de Nacimiento
                DateTime fecha = Comun.FormatearFecha(dt.Rows[0]["Fecha"].ToString());
                string vFecha = fecha.Day.ToString().PadLeft(2, '0') + " " + fecha.Month.ToString().PadLeft(2, '0') + " " + fecha.Year.ToString();
                //string vHora = fecha.Hour.ToString().PadLeft(2, '0') + " " + fecha.Minute.ToString().PadLeft(2, '0') + " " + fecha.ToString("tt").ToUpper().Replace(".", "").Trim();

                TimeSpan timespan = new TimeSpan(fecha.Hour, fecha.Minute, fecha.Second);
                DateTime time = DateTime.Today.Add(timespan);
                string displayTime = time.ToString("hh:mm tt");
                string vHora = displayTime.Replace(":", " ").Replace(". ", "").Replace(".", "").ToUpper().Trim();

                _EscribirLetraxLetra(fechaPosX + (ejex * (float)8.5), fechaPosY, ejex, vFecha, cb, document);
                _EscribirLetraxLetra(HoraPosX + (ejex * 8), HoraPosY, ejex, vHora, cb, document);

                //Lugar u Tipo Lugar
                if (dt.Rows[0]["LugarOcurrencia"] != null) Lugar = dt.Rows[0]["LugarOcurrencia"].ToString();
                if (dt.Rows[0]["LugarTipo"] != null) TipoLugar = dt.Rows[0]["LugarTipo"].ToString();

                if (Lugar.Trim().Length == 0) Lugar = "----------";
                else Lugar = Lugar + "-----";

                EscribirLetraxLetra(LugarPosX + (ejex * 9), LugarPosY + 5, ejex, Lugar, cb, document);
                EscribirLetraxLetra(TipoLugarPosX + (ejex * 9), TipoLugarPosY + 5, ejex, TipoLugar, cb, document);

                //Lugar de Ocurrencia
                if (dt.Rows[0]["LugarUbigeoDep"] != null) LugarUbioDep = dt.Rows[0]["LugarUbigeoDep"].ToString().PadLeft(2, '0');
                if (dt.Rows[0]["LugarUbigeoDepDetalle"] != null) LugarUbigeoDepDetalle = dt.Rows[0]["LugarUbigeoDepDetalle"].ToString();
                if (dt.Rows[0]["LugarUbigeoPrv"] != null) LugarUbigeoPrv = dt.Rows[0]["LugarUbigeoPrv"].ToString().PadLeft(2, '0');
                if (dt.Rows[0]["LugarUbigeoPrvdetalle"] != null) LugarUbigeoPrvdetalle = dt.Rows[0]["LugarUbigeoPrvdetalle"].ToString();
                if (dt.Rows[0]["LugarUbigeoDst"] != null) LugarUbigeoDst = dt.Rows[0]["LugarUbigeoDst"].ToString().PadLeft(2, '0');
                if (dt.Rows[0]["LugarUbigeoDstDetalle"] != null) LugarUbigeoDstDetalle = dt.Rows[0]["LugarUbigeoDstDetalle"].ToString();

                if (dt.Rows[0]["LugarUbigeoCpo"] != null) LugarUbigeoCpo = dt.Rows[0]["LugarUbigeoCpo"].ToString().PadLeft(2, '0');

                if (dt.Rows[0]["LugarUbigeoCpoDetalle"] != null) LugarUbigeoCpoDetalle = dt.Rows[0]["LugarUbigeoCpoDetalle"].ToString();

                if (LugarUbigeoDepDetalle.Trim().Length == 0) LugarUbigeoDepDetalle = "----------";
                else LugarUbigeoDepDetalle = Util.GenerarLineasubigeoizquierda(LugarUbigeoDepDetalle);// +"-----";

                if (LugarUbigeoPrvdetalle.Trim().Length == 0) LugarUbigeoPrvdetalle = "----------";
                else LugarUbigeoPrvdetalle = Util.GenerarLineasubigeoderecha(LugarUbigeoPrvdetalle);// +"-----";

                if (LugarUbigeoDstDetalle.Trim().Length == 0) LugarUbigeoDstDetalle = "----------";
                else LugarUbigeoDstDetalle = Util.GenerarLineasubigeoizquierda(LugarUbigeoDstDetalle);// +"-----";

                if (LugarUbigeoCpoDetalle.Trim().Length == 0) LugarUbigeoCpoDetalle = "----------";
                else LugarUbigeoCpoDetalle = Util.GenerarLineasubigeoderecha(LugarUbigeoCpoDetalle);// +"-----";

                if (LugarUbigeoCpo == "00") LugarUbigeoCpo = "--";

                EscribirLetraxLetra(LugarUbioDepPosX + (ejex * 9), LugarUbioDepPosY + 6, ejex, LugarUbioDep, cb, document);
                EscribirLetraxLetra(LugarUbigeoDepDetallePosX + (ejex * 9), LugarUbigeoDepDetallePosY + 6, ejex, LugarUbigeoDepDetalle, cb, document);
                EscribirLetraxLetra(LugarUbigeoPrvPosX + (ejex * 9), LugarUbigeoPrvPosY + 6, ejex, LugarUbigeoPrv, cb, document);
                EscribirLetraxLetra(LugarUbigeoPrvdetallePosX + (ejex * 9), LugarUbigeoPrvdetallePosY + 6, ejex, LugarUbigeoPrvdetalle, cb, document);
                EscribirLetraxLetra(LugarUbigeoDstPosX + (ejex * 9), LugarUbigeoDstPosY + 6, ejex, LugarUbigeoDst, cb, document);
                EscribirLetraxLetra(LugarUbigeoDstDetallePosX + (ejex * 9), LugarUbigeoDstDetallePosY + 6, ejex, LugarUbigeoDstDetalle, cb, document);

                EscribirLetraxLetra(LugarUbigeoCPPosX + (ejex * 9), LugarUbigeoCPPosY + 6, ejex, LugarUbigeoCpo, cb, document);
                EscribirLetraxLetra(LugarUbigeoCPDetallePosX + (ejex * 9), LugarUbigeoCPDetallePosY + 6, ejex, LugarUbigeoCpoDetalle, cb, document);

                //Fallecido
                if (dt.Rows[0]["PreNombres"] != null) PreNombresFallecido = dt.Rows[0]["PreNombres"].ToString();
                if (dt.Rows[0]["PrimerApellido"] != null) PrimerApellidoFallecido = dt.Rows[0]["PrimerApellido"].ToString();
                if (dt.Rows[0]["SegundoApellido"] != null) SegundoApellidoFallecido = dt.Rows[0]["SegundoApellido"].ToString();

                if (dt.Rows[0]["TipoDocumento"] != null) TipoDocumentoFallecido = dt.Rows[0]["TipoDocumento"].ToString();
                if (dt.Rows[0]["NumeroDocumento"] != null) NumeroDocumentoFallecido = dt.Rows[0]["NumeroDocumento"].ToString();

                //----------------------------------------------------------------
                // Autor del cambio: Miguel Márquez Beltrán
                // Fecha del cambio: 13/09/2016
                // Objetivo: Mostrar Calcular meses o dias del menor de un año
                //-----------------------------------------------------------------
                DateTime dFechaNacimiento = new DateTime();
                DateTime dFechaFallecimiento = new DateTime();

                if (dt.Rows[0]["pers_dNacimientoFecha"] != null && dt.Rows[0]["pers_dFechaDefuncion"] != null)
                {
                    dFechaNacimiento = Comun.FormatearFecha(dt.Rows[0]["pers_dNacimientoFecha"].ToString());                
                    dFechaFallecimiento = Comun.FormatearFecha(dt.Rows[0]["pers_dFechaDefuncion"].ToString());

                    EdadFallecido = Comun.DiferenciaFechas(dFechaFallecimiento, dFechaNacimiento, "--");
                }
                if (dt.Rows[0]["Edad"] != null)
                {
                    EdadFallecido = dt.Rows[0]["Edad"].ToString();
                }
                //if (dt.Rows[0]["Edad"] != null)
                //{
                //    EdadFallecido = dt.Rows[0]["Edad"].ToString();
                //}

                if (dt.Rows[0]["Nacionalidad"] != null) TipoNacionalidadFallecido = Constantes.CONST_VALOR_PERUANO.ToString(); //dt.Rows[0]["Nacionalidad"].ToString(); se retiro esto por el ticket 354 el cual siempre mostrara 1 por la forzacion de la nacionalidad a peruana
                if (dt.Rows[0]["NacionalidadTexto"] != null) NacionalidadFallecido = Enumerador.enmNacionalidad.PERUANA.ToString();// dt.Rows[0]["NacionalidadTexto"].ToString();  se retiro esto por el ticket 354 el cual siempre mostrara 1 por la forzacion de la nacionalidad a peruana

                if (PreNombresFallecido.Trim().Length == 0) PreNombresFallecido = "----------";
                else PreNombresFallecido = PreNombresFallecido + "-----";
                if (PrimerApellidoFallecido.Trim().Length == 0) PrimerApellidoFallecido = "----------";
                else PrimerApellidoFallecido = PrimerApellidoFallecido + "-----";
                if (SegundoApellidoFallecido.Trim().Length == 0) SegundoApellidoFallecido = "----------";
                else SegundoApellidoFallecido = SegundoApellidoFallecido + "-----";
                if (TipoDocumentoFallecido.Trim().Length == 0) TipoDocumentoFallecido = "-";
                if (NumeroDocumentoFallecido.Trim().Length == 0) NumeroDocumentoFallecido = "----------";
                else NumeroDocumentoFallecido = Util.GenerarLineasDocumento(NumeroDocumentoFallecido);// +"-----";
                if (EdadFallecido.Trim().Length == 0) EdadFallecido = "--";
                if (EdadFallecido == "0") EdadFallecido = "--";
                if (TipoNacionalidadFallecido.Trim().Length == 0) TipoNacionalidadFallecido = "-";
                if (NacionalidadFallecido.Trim().Length == 0) NacionalidadFallecido = "----------";
                else NacionalidadFallecido = NacionalidadFallecido + "-----";



                EscribirLetraxLetra(PreNombresFallecidoPosX + (ejex * 9), PreNombresFallecidoPosY + 4, ejex, PreNombresFallecido, cb, document);
                EscribirLetraxLetra(PrimerApellidoFallecidoPosX + (ejex * 9), PrimerApellidoFallecidoPosY + 4, ejex, PrimerApellidoFallecido, cb, document);
                EscribirLetraxLetra(SegundoApellidoFallecidoPosX + (ejex * 9), SegundoApellidoFallecidoPosY + 4, ejex, SegundoApellidoFallecido, cb, document);

                EscribirLetraxLetra(FallecidoTipoDocumentoPosX + (ejex * 9), FallecidoTipoDocumentoPosY + 4, ejex, TipoDocumentoFallecido, cb, document);
                EscribirLetraxLetra(FallecidoNumeroDocumentoPosX + (ejex * 9), FallecidoNumeroDocumentoPosY + 4, ejex, NumeroDocumentoFallecido, cb, document);
                EscribirLetraxLetra(FallecidoEdadPosX + (ejex * 9), FallecidoEdadPosY + 4, ejex, EdadFallecido, cb, document);

                EscribirLetraxLetra(FallecidoTipoNacionalidadPosX + (ejex * 9), FallecidoTipoNacionalidadPosY + 4, ejex, TipoNacionalidadFallecido, cb, document);
                EscribirLetraxLetra(FallecidoNacionalidadPosX + (ejex * 9), FallecidoNacionalidadPosY + 4, ejex, NacionalidadFallecido, cb, document);



                // Fallecido Ubigeo
                if (dt.Rows[0]["NacUbigeoDep"] != null) FallecidoLugarUbioDep = dt.Rows[0]["NacUbigeoDep"].ToString().PadLeft(2, '0');
                if (dt.Rows[0]["NacUbigeoDepDetalle"] != null) FallecidoLugarUbigeoDepDetalle = dt.Rows[0]["NacUbigeoDepDetalle"].ToString();
                if (dt.Rows[0]["NacUbigeoPrv"] != null) FallecidoLugarUbigeoPrv = dt.Rows[0]["NacUbigeoPrv"].ToString().PadLeft(2, '0');
                if (dt.Rows[0]["NacUbigeoPrvDetalle"] != null) FallecidoLugarUbigeoPrvdetalle = dt.Rows[0]["NacUbigeoPrvDetalle"].ToString();
                if (dt.Rows[0]["NacUbigeoDst"] != null) FallecidoLugarUbigeoDst = dt.Rows[0]["NacUbigeoDst"].ToString().PadLeft(2, '0');
                if (dt.Rows[0]["NacUbigeoDstDetalle"] != null) FallecidoLugarUbigeoDstDetalle = dt.Rows[0]["NacUbigeoDstDetalle"].ToString();

                if (dt.Rows[0]["NacUbigeoCpo"] != null) FallecidoLugarUbigeoCpo = dt.Rows[0]["NacUbigeoCpo"].ToString().PadLeft(2, '0');
                if (dt.Rows[0]["NacUbigeoCpoDetalle"] != null) FallecidoLugarUbigeoCpoDetalle = dt.Rows[0]["NacUbigeoCpoDetalle"].ToString();




                if (FallecidoLugarUbigeoDepDetalle.Trim().Length == 0) FallecidoLugarUbigeoDepDetalle = "----------";
                else FallecidoLugarUbigeoDepDetalle = Util.GenerarLineasubigeoizquierda(FallecidoLugarUbigeoDepDetalle);// +"-----";

                if (FallecidoLugarUbigeoPrvdetalle.Trim().Length == 0) FallecidoLugarUbigeoPrvdetalle = "----------";
                else FallecidoLugarUbigeoPrvdetalle = Util.GenerarLineasubigeoderecha(FallecidoLugarUbigeoPrvdetalle);// +"-----";

                if (FallecidoLugarUbigeoDstDetalle.Trim().Length == 0) FallecidoLugarUbigeoDstDetalle = "----------";
                else FallecidoLugarUbigeoDstDetalle = Util.GenerarLineasubigeoizquierda(FallecidoLugarUbigeoDstDetalle);// +"-----";

                if (FallecidoLugarUbigeoCpoDetalle.Trim().Length == 0) FallecidoLugarUbigeoCpoDetalle = "----------";
                else FallecidoLugarUbigeoCpoDetalle = Util.GenerarLineasubigeoderecha(FallecidoLugarUbigeoCpoDetalle);// +"-----";

                if (FallecidoLugarUbigeoCpo == "00") FallecidoLugarUbigeoCpo = "--";

                EscribirLetraxLetra(FallecidoLugarUbioDepPosX + (ejex * 9), FallecidoLugarUbioDepPosY + 6, ejex, FallecidoLugarUbioDep, cb, document);
                EscribirLetraxLetra(FallecidoLugarUbigeoDepDetallePosX + (ejex * 9), FallecidoLugarUbigeoDepDetallePosY + 6, ejex, FallecidoLugarUbigeoDepDetalle, cb, document);
                EscribirLetraxLetra(FallecidoLugarUbigeoPrvPosX + (ejex * 9), FallecidoLugarUbigeoPrvPosY + 6, ejex, FallecidoLugarUbigeoPrv, cb, document);
                EscribirLetraxLetra(FallecidoLugarUbigeoPrvdetallePosX + (ejex * 9), FallecidoLugarUbigeoPrvdetallePosY + 6, ejex, FallecidoLugarUbigeoPrvdetalle, cb, document);
                EscribirLetraxLetra(FallecidoLugarUbigeoDstPosX + (ejex * 9), FallecidoLugarUbigeoDstPosY + 6, ejex, FallecidoLugarUbigeoDst, cb, document);
                EscribirLetraxLetra(FallecidoLugarUbigeoDstDetallePosX + (ejex * 9), FallecidoLugarUbigeoDstDetallePosY + 6, ejex, FallecidoLugarUbigeoDstDetalle, cb, document);

                EscribirLetraxLetra(FallecidoLugarUbigeoCPPosX + (ejex * 9), FallecidoLugarUbigeoCPPosY + 6, ejex, FallecidoLugarUbigeoCpo, cb, document);
                EscribirLetraxLetra(FallecidoLugarUbigeoCPDetallePosX + (ejex * 9), FallecidoLugarUbigeoCPDetallePosY + 6, ejex, FallecidoLugarUbigeoCpoDetalle, cb, document);




                //Padre
                if (dt.Rows[0]["PadrePreNombres"] != null) PadrePreNombres = dt.Rows[0]["PadrePreNombres"].ToString();
                if (dt.Rows[0]["PadrePrimerApellido"] != null) PadrePrimerApellido = dt.Rows[0]["PadrePrimerApellido"].ToString();
                if (dt.Rows[0]["PadreSegundoApellido"] != null) PadreSegundoApellido = dt.Rows[0]["PadreSegundoApellido"].ToString();

                if (PadrePreNombres.Trim().Length == 0) PadrePreNombres = "----------";
                else PadrePreNombres = PadrePreNombres + "-----";

                if (PadrePrimerApellido.Trim().Length == 0) PadrePrimerApellido = "----------";
                else PadrePrimerApellido = PadrePrimerApellido + "-----";

                if (PadreSegundoApellido.Trim().Length == 0) PadreSegundoApellido = "----------";
                else PadreSegundoApellido = PadreSegundoApellido + "-----";


                EscribirLetraxLetra(PadrePreNombresPosX + (ejex * 9), PadrePreNombresPosY + 4, ejex, PadrePreNombres, cb, document);
                EscribirLetraxLetra(PadrePrimerApellidoPosX + (ejex * 9), PadrePrimerApellidoPosY + 4, ejex, PadrePrimerApellido, cb, document);
                EscribirLetraxLetra(PadreSegundoApellidoPosX + (ejex * 9), PadreSegundoApellidoPosY + 4, ejex, PadreSegundoApellido, cb, document);

                //MADRE
                if (dt.Rows[0]["MadrePreNombres"] != null) MadrePreNombres = dt.Rows[0]["MadrePreNombres"].ToString();
                if (dt.Rows[0]["MadrePrimerApellido"] != null) MadrePrimerApellido = dt.Rows[0]["MadrePrimerApellido"].ToString();
                if (dt.Rows[0]["MadreSegundoApellido"] != null) MadreSegundoApellido = dt.Rows[0]["MadreSegundoApellido"].ToString();


                if (MadrePreNombres.Trim().Length == 0) MadrePreNombres = "----------";
                else MadrePreNombres = MadrePreNombres + "-----";

                if (MadrePrimerApellido.Trim().Length == 0) MadrePrimerApellido = "----------";
                else MadrePrimerApellido = MadrePrimerApellido + "-----";

                if (MadreSegundoApellido.Trim().Length == 0) MadreSegundoApellido = "----------";
                else MadreSegundoApellido = MadreSegundoApellido + "-----";


                EscribirLetraxLetra(MadrePreNombresPosX + (ejex * 9), MadrePreNombresPosY, ejex, MadrePreNombres, cb, document);
                EscribirLetraxLetra(MadrePrimerApellidoPosX + (ejex * 9), MadrePrimerApellidoPosY, ejex, MadrePrimerApellido, cb, document);
                EscribirLetraxLetra(MadreSegundoApellidoPosX + (ejex * 9), MadreSegundoApellidoPosY, ejex, MadreSegundoApellido, cb, document);




                //REGISTRADOR
                DateTime fechaRegistrador = Comun.FormatearFecha(dt.Rows[0]["RegistroFecha"].ToString());
                RegistroFecha = fechaRegistrador.Day.ToString().PadLeft(2, '0') + " " + fechaRegistrador.Month.ToString().PadLeft(2, '0') + " " + fechaRegistrador.Year.ToString();

                if (dt.Rows[0]["RegistroUbigeoDep"] != null) RegisTroUbigeoDep = dt.Rows[0]["RegistroUbigeoDep"].ToString().PadLeft(2, '0');
                if (RegisTroUbigeoDep.Trim().Length == 0) RegisTroUbigeoDep = "--";


                if (dt.Rows[0]["RegistroUbigeoDepDetalle"] != null) RegistroUbigeoDetalle = dt.Rows[0]["RegistroUbigeoDepDetalle"].ToString();
                if (RegistroUbigeoDetalle.Trim().Length == 0) RegistroUbigeoDetalle = "----------";
                else RegistroUbigeoDetalle = Util.GenerarLineasubigeoizquierda(RegistroUbigeoDetalle);// +"-----";

                if (dt.Rows[0]["RegistroUbigeoPrv"] != null) RegistroUbigeoPrv = dt.Rows[0]["RegistroUbigeoPrv"].ToString().PadLeft(2, '0');
                if (RegistroUbigeoPrv.Trim().Length == 0) RegistroUbigeoPrv = "--";

                if (dt.Rows[0]["RegistroUbigeoPrvDetalle"] != null) RegistroUbigeoPrvDetalle = dt.Rows[0]["RegistroUbigeoPrvDetalle"].ToString();
                if (RegistroUbigeoPrvDetalle.Trim().Length == 0) RegistroUbigeoPrvDetalle = "----------";
                else RegistroUbigeoPrvDetalle = Util.GenerarLineasubigeoderecha(RegistroUbigeoPrvDetalle);// +"-----";

                if (dt.Rows[0]["RegistroUbigeoDst"] != null) RegistroUbigeoDst = dt.Rows[0]["RegistroUbigeoDst"].ToString().PadLeft(2, '0');
                if (RegistroUbigeoDst.Trim().Length == 0) RegistroUbigeoDst = "--";

                if (dt.Rows[0]["RegistroUbigeoDstDetalle"] != null) RegistroUbigeoDstDetalle = dt.Rows[0]["RegistroUbigeoDstDetalle"].ToString();
                if (RegistroUbigeoDstDetalle.Trim().Length == 0) RegistroUbigeoDstDetalle = "----------";
                else RegistroUbigeoDstDetalle = Util.GenerarLineasubigeoizquierda(RegistroUbigeoDstDetalle);// +"-----";


                if (dt.Rows[0]["RegistroUbigeoCpo"] != null) RegistroUbigeoCpo = dt.Rows[0]["RegistroUbigeoCpo"].ToString().PadLeft(2, '0');
                if (RegistroUbigeoCpo.Trim().Length == 0) RegistroUbigeoCpo = "--";
                if (RegistroUbigeoCpo.Trim() == "00") RegistroUbigeoCpo = "--";

                if (dt.Rows[0]["RegistroUbigeoCpoDetalle"] != null) RegistroUbigeoCpoDetalle = dt.Rows[0]["RegistroUbigeoCpoDetalle"].ToString();
                if (RegistroUbigeoCpoDetalle.Trim().Length == 0) RegistroUbigeoCpoDetalle = "----------";
                else RegistroUbigeoCpoDetalle = Util.GenerarLineasubigeoderecha(RegistroUbigeoCpoDetalle);// +"-----";


                if (dt.Rows[0]["RegistradorPrenombres"] != null) RegistradorPrenombres = dt.Rows[0]["RegistradorPrenombres"].ToString();
                if (dt.Rows[0]["RegistradorPrimerApellido"] != null) RegistradorPrimerApellido = dt.Rows[0]["RegistradorPrimerApellido"].ToString();
                if (dt.Rows[0]["RegistradorSegundoApellido"] != null) RegistradorSegundoApellido = dt.Rows[0]["RegistradorSegundoApellido"].ToString();


                if (dt.Rows[0]["RegistradorNumeroDocumento"] != null) RegistradorNumeroDocumento = dt.Rows[0]["RegistradorNumeroDocumento"].ToString();

                RegistradorNombresCompletos = RegistradorPrimerApellido + " " + RegistradorSegundoApellido + " " + RegistradorPrenombres;

                if (RegistradorNombresCompletos.Trim().Length == 0) RegistradorNombresCompletos = "----------";
                else RegistradorNombresCompletos = RegistradorNombresCompletos + "-----";

                if (RegistradorNumeroDocumento.Trim().Length == 0) RegistradorNumeroDocumento = "----------";
                else RegistradorNumeroDocumento = Util.GenerarLineasDocumento(RegistradorNumeroDocumento);// +"-----";

                _EscribirLetraxLetra(RegistroFechaPosX + (ejex * 9), RegistroFechaPosY, ejex, RegistroFecha, cb, document);
                EscribirLetraxLetra(RegisTroUbigeoDepPosX + (ejex * 9), RegisTroUbigeoDepPosY, ejex, RegisTroUbigeoDep, cb, document);
                EscribirLetraxLetra(RegistroUbigeoDetallePosX + (ejex * 9), RegistroUbigeoDetallePosY, ejex, RegistroUbigeoDetalle, cb, document);
                EscribirLetraxLetra(RegistroUbigeoPrvPosX + (ejex * 9), RegistroUbigeoPrvPosY, ejex, RegistroUbigeoPrv, cb, document);
                EscribirLetraxLetra(RegistroUbigeoPrvDetallePosX + (ejex * 9), RegistroUbigeoPrvDetallePosY, ejex, RegistroUbigeoPrvDetalle, cb, document);

                EscribirLetraxLetra(RegistroUbigeoDstPosX + (ejex * 9), RegistroUbigeoDstPosY, ejex, RegistroUbigeoDst, cb, document);
                EscribirLetraxLetra(RegistroUbigeoDstDetallePosX + (ejex * 9), RegistroUbigeoDstDetallePosY, ejex, RegistroUbigeoDstDetalle, cb, document);

                EscribirLetraxLetra(RegistroUbigeoCPPosX + (ejex * 9), RegistroUbigeoCPPosY, ejex, RegistroUbigeoCpo, cb, document);
                EscribirLetraxLetra(RegistroUbigeoCPDetallePosX + (ejex * 9), RegistroUbigeoCPDetallePosY, ejex, RegistroUbigeoCpoDetalle, cb, document);


                EscribirLetraxLetra(RegistradorNombresCompletosPosX + (ejex * 9), RegistradorNombresCompletosPosY, ejex, RegistradorNombresCompletos, cb, document);
                EscribirLetraxLetra(RegistradorNumeroDocumentoPosX + (ejex * 9), RegistradorNumeroDocumentoPosY, ejex, RegistradorNumeroDocumento, cb, document);


                //DECLARANTE 1

                //if (dt.Rows[0]["Declarante1Vinculo"] != null && dt.Rows[0]["Declarante1Vinculo"].ToString() != "0" && dt.Rows[0]["Declarante1Vinculo"].ToString() != "00") Declarante1Vinculo = dt.Rows[0]["Declarante1Vinculo"].ToString();
                if (dt.Rows[0]["DeclarantePrenombres"] != null && dt.Rows[0]["DeclarantePrenombres"].ToString() != "0" && dt.Rows[0]["DeclarantePrenombres"].ToString() != "00") Declarante1Prenombres = dt.Rows[0]["DeclarantePrenombres"].ToString();
                if (dt.Rows[0]["DeclarantePrimerApellido"] != null && dt.Rows[0]["DeclarantePrimerApellido"].ToString() != "0" && dt.Rows[0]["DeclarantePrimerApellido"].ToString() != "00") Declarante1PrimerApellido = dt.Rows[0]["DeclarantePrimerApellido"].ToString();
                if (dt.Rows[0]["DeclaranteSegundoApellido"] != null && dt.Rows[0]["DeclaranteSegundoApellido"].ToString() != "0" && dt.Rows[0]["DeclaranteSegundoApellido"].ToString() != "00") Declarante1SegundoApellido = dt.Rows[0]["DeclaranteSegundoApellido"].ToString();
                if (dt.Rows[0]["DeclaranteTipoDocumento"] != null && dt.Rows[0]["DeclaranteTipoDocumento"].ToString() != "0" && dt.Rows[0]["DeclaranteTipoDocumento"].ToString() != "00") Declarante1TipoDocumento = dt.Rows[0]["DeclaranteTipoDocumento"].ToString();
                if (dt.Rows[0]["DeclaranteNumeroDocumento"] != null && dt.Rows[0]["DeclaranteNumeroDocumento"].ToString() != "0" && dt.Rows[0]["DeclaranteNumeroDocumento"].ToString() != "00") Declarante1NumeroDocumento = dt.Rows[0]["DeclaranteNumeroDocumento"].ToString();

                if (Declarante1Vinculo.Trim().Length == 0) Declarante1Vinculo = "----------";
                else Declarante1Vinculo = Declarante1Vinculo + "-----";

                if (Declarante1Prenombres.Trim().Length == 0) Declarante1Prenombres = "----------";
                else Declarante1Prenombres = Declarante1Prenombres + "-----";

                if (Declarante1PrimerApellido.Trim().Length == 0) Declarante1PrimerApellido = "----------";
                else Declarante1PrimerApellido = Declarante1PrimerApellido + "-----";

                if (Declarante1SegundoApellido.Trim().Length == 0) Declarante1SegundoApellido = "----------";
                else Declarante1SegundoApellido = Declarante1SegundoApellido + "-----";

                if (Declarante1TipoDocumento.Trim().Length == 0) Declarante1TipoDocumento = "--";
                else Declarante1TipoDocumento = Declarante1TipoDocumento + ""; ;

                if (Declarante1NumeroDocumento.Trim().Length == 0) Declarante1NumeroDocumento = "----------";
                else Declarante1NumeroDocumento = Util.GenerarLineasDocumento(Declarante1NumeroDocumento);// +"-----";

                //EscribirLetraxLetra(Declarante1VinculoPosX + (ejex * 9), Declarante1VinculoPosY, ejex, Declarante1Vinculo, cb, document);
                EscribirLetraxLetra(Declarante1PrenombresPosX + (ejex * 9), Declarante1PrenombresPosY, ejex, Declarante1Prenombres, cb, document);
                EscribirLetraxLetra(Declarante1PrimerApellidoPosX + (ejex * 9), Declarante1PrimerApellidoPosY, ejex, Declarante1PrimerApellido, cb, document);
                EscribirLetraxLetra(Declarante1SegundoApellidoPosX + (ejex * 9), Declarante1SegundoApellidoPosY, ejex, Declarante1SegundoApellido, cb, document);
                EscribirLetraxLetra(Declarante1TipoDocumentoPosX + (ejex * 9), Declarante1TipoDocumentoPosY, ejex, Declarante1TipoDocumento, cb, document);
                EscribirLetraxLetra(Declarante1NumeroDocumentoPosX + (ejex * 9), Declarante1NumeroDocumentoPosY, ejex, Declarante1NumeroDocumento, cb, document);

                //OBSERVACION  
                if (dt.Rows[0]["Observaciones"] != null) Observaciones = dt.Rows[0]["Observaciones"].ToString();

                if (Observaciones.Trim().Length == 0) Observaciones = "";
                else Observaciones = Observaciones + "-----";

                EscribirLetraxLetraNacimientoObservacion(ObservacionesPosX + (ejex * 9), ObservacionesPosY, ejex, Observaciones, cb, document);

                cb.EndText();

                document.Close();
                oStreamReader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void EscribirLetraxLetra(float ejeXInicio, float ejeYInicio, float ejeXDistancia, string palabra, iTextSharp.text.pdf.PdfContentByte cb, iTextSharp.text.Document document, int limiteLetra = 0)
        {
            cb.SetTextMatrix(ejeXInicio, document.PageSize.Height - ejeYInicio);
            cb.ShowText(palabra.ToString());
        }

        private static void _EscribirLetraxLetra(float ejeXInicio, float ejeYInicio, float ejeXDistancia, string palabra, iTextSharp.text.pdf.PdfContentByte cb, iTextSharp.text.Document document, int limiteLetra = 0)
        {
            float cont = ejeXInicio;
            foreach (char letra in palabra)
            {
                cb.SetTextMatrix(cont, document.PageSize.Height - ejeYInicio);
                cb.ShowText(letra.ToString());
                cont += ejeXDistancia;
            }
        }

        private static void EscribirLetraxLetraMatrimonio(float ejeXInicio, float ejeYInicio, float ejeXDistancia, string palabra_, iTextSharp.text.pdf.PdfContentByte cb, iTextSharp.text.Document document, int limiteLetra = 0)
        {

            cb.SetTextMatrix(ejeXInicio, ejeYInicio);
            cb.ShowText(palabra_.ToString());

        }

        private static void EscribirLetraxLetraMatrimonioObservacion(float ejeXInicio, float ejeYInicio, float ejeXDistancia, string palabra_, iTextSharp.text.pdf.PdfContentByte cb, iTextSharp.text.Document document, int limiteLetra = 0)
        {
            float pos = 0;
            float tamPalabra = 0;
            float ancho = 230f;
            float posxAcumulado = tamPalabra;
            string texto = string.Empty;

            iTextSharp.text.Font fontConsulado = iTextSharp.text.FontFactory.GetFont("Arial", 6);
            foreach (string palabra in palabra_.Split(' '))
            {
                tamPalabra = new iTextSharp.text.Chunk(palabra.Trim(), fontConsulado).GetWidthPoint();

                if (posxAcumulado + tamPalabra > ancho)
                {

                    cb.SetTextMatrix(ejeXInicio, ejeYInicio + pos);
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
                cb.SetTextMatrix(ejeXInicio, ejeYInicio + pos);
                cb.ShowText(texto.Trim());
            }


        }

        private static void EscribirLetraxLetraNacimientoObservacion(float ejeXInicio, float ejeYInicio, float ejeXDistancia, string palabra_, iTextSharp.text.pdf.PdfContentByte cb, iTextSharp.text.Document document, int limiteLetra = 0)
        {

            float pos = 0;
            float tamPalabra = 0;
            float ancho = 230f;
            float posxAcumulado = tamPalabra;
            string texto = string.Empty;

            iTextSharp.text.Font fontConsulado = iTextSharp.text.FontFactory.GetFont("Arial", 6);
            foreach (string palabra in palabra_.Split(' '))
            {
                tamPalabra = new iTextSharp.text.Chunk(palabra.Trim(), fontConsulado).GetWidthPoint();

                if (posxAcumulado + tamPalabra > ancho)
                {

                    cb.SetTextMatrix(ejeXInicio, document.PageSize.Height - ejeYInicio - pos);
                    cb.ShowText(texto.Trim());
                    texto = string.Empty;

                    pos += 10;
                    posxAcumulado = 0;
                }

                posxAcumulado += tamPalabra;
                posxAcumulado += new iTextSharp.text.Chunk(" ", fontConsulado).GetWidthPoint();
                texto += " " + palabra;
            }

            if (texto.Trim() != string.Empty)
            {
                cb.SetTextMatrix(ejeXInicio, document.PageSize.Height - ejeYInicio - pos);
                cb.ShowText(texto.Trim());
            }


        }

        private static void _EscribirLetraxLetraMatrimonio(float ejeXInicio, float ejeYInicio, float ejeXDistancia, string palabra, iTextSharp.text.pdf.PdfContentByte cb, iTextSharp.text.Document document, int limiteLetra = 0)
        {
            float cont = ejeXInicio;
            float contAplica = cont;
            foreach (char letra in palabra)
            {
                if (letra.ToString().ToLower() == "i")
                    contAplica = cont + (ejeXDistancia - 4) / 3;
                else
                    contAplica = cont;

                cb.SetTextMatrix(contAplica, ejeYInicio);
                cb.ShowText(letra.ToString());
                cont += ejeXDistancia - 4;
            }
        }

        private static string EsCodigo00(string codigo)
        {
            if (codigo.Trim() == "00")
                return "--";

            return codigo;
        }

        protected void ddl_Genero_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void BtnVistaPrevia_Click(object sender, EventArgs e)
        {
            String StrScript = String.Empty; //Variables para los Mensaje 
            String DniInvalidos = String.Empty; //Variable para Obtener por WEBCONFIG los DNI Invalidos
            Int16 ContarCodParti = 0;  //Contar la Cantidad de Participante obligatorios a registrar obtenido del WEBConfing
            Int16 ContarAux = 0, ContarPadresPeruanos = 0, ContarDeclarante = 0;
            Int16 intCantDeclarantes = 0;// Contador
            String MensajeParticipantes = String.Empty; //Mensaje de Error Para los Participante Obligatorios
            String iCodTipoParticipante = String.Empty; //Codigo del Participante
            String ooParticipante = String.Empty; //Descripcion del Participante
            string vNacionalidad = string.Empty;
            string strTipoDocumento = string.Empty;
            string strNumerodocumento = string.Empty;

            Session["InicioTramite"] = "2";
            if (txtFecNac.Value() == DateTime.MinValue)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTUACIÓN", "Falta ingresar la fecha"));
                return;
            }
            if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.NACIMIENTO)
            {
                DniInvalidos = ConfigurationManager.AppSettings["DniInvalidos"].ToString();
                String[] data = DniInvalidos.Trim().Split(',');

                foreach (String odata in data)
                {

                    if (odata.Trim() == txtNroCUI.Text.Trim())
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO CIVIL", "Error. Nro. CUI Inválido.", false, 190, 250);
                        Comun.EjecutarScript(Page, StrScript);
                        return;
                    }
                }
                if (tablaReconocimientoAdopcion.Visible == true)
                {
                    if (chkReconocimientoAdopcion.Checked)
                    {
                        if (txtTitularActa.Text.Trim().Length == 0)
                        {
                            StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO CIVIL", "Falta ingresar el nombre del Titular del Acta anterior.", false, 190, 250);
                            Comun.EjecutarScript(Page, StrScript);
                            return;
                        }
                    }
                }
            }

            if (Grd_Participantes.Rows.Count > 0)
            {
                
                if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.NACIMIENTO)
                {
                    #region Nacimiento

                    String ValNacimineto = ConfigurationManager.AppSettings["Nacimiento"].ToString();
                    String[] CodParti = ValNacimineto.Trim().Split(',');

                    ContarCodParti = Convert.ToInt16(CodParti.Length);
                    int countPadre = 0;
                    int countDeclarante = 0;


                    foreach (String onjcaract in CodParti)
                    {

                        foreach (GridViewRow row in Grd_Participantes.Rows)
                        {
                            iCodTipoParticipante = row.Cells[0].Text;
                            //ooParticipante = row.Cells[5].Text;


                            if (onjcaract == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.DECLARANTE_1) || onjcaract == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.DECLARANTE_2))
                            {
                                if (iCodTipoParticipante == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.DECLARANTE_1) || iCodTipoParticipante == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.DECLARANTE_2))
                                {
                                    ContarAux++;
                                    break;
                                }
                            }

                            if (onjcaract == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.MADRE) || onjcaract == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.PADRE))
                            {
                                if (iCodTipoParticipante == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.MADRE) || iCodTipoParticipante == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.PADRE))
                                {
                                    ContarAux++;
                                    break;
                                }
                            }

                            if (iCodTipoParticipante == onjcaract)
                            {

                                ContarAux++;
                                break;
                            }


                        }

                        if (ContarAux == 0)
                        {
                            string strDescripcion = string.Empty;
                            strDescripcion = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.REG_CIVIL_PARTICIPANTE_NACIMIENTO, Convert.ToInt32(onjcaract), "descripcion");


                            if (strDescripcion == Enumerador.enmParticipanteNacimiento.PADRE.ToString() || strDescripcion == Enumerador.enmParticipanteNacimiento.MADRE.ToString())
                            {
                                countPadre++;
                            }
                            else if (strDescripcion == Constantes.CONST_VALIDACION_DECLARANTE1 || strDescripcion == Constantes.CONST_VALIDACION_DECLARANTE2)
                            {
                                countDeclarante++;
                            }
                            else
                            {

                                MensajeParticipantes += "," + strDescripcion;
                            }
                        }
                        else
                        {
                            ContarAux = 0;
                        }

                    }

                    if (countPadre == Constantes.CONST_VALIDACION_PADRES)
                    {
                        MensajeParticipantes += ", " + Enumerador.enmParticipanteNacimiento.PADRE.ToString() + " ó " + Enumerador.enmParticipanteNacimiento.MADRE.ToString();
                    }
                    if (countDeclarante == Constantes.CONST_VALIDACION_DECLARANTE)
                    {
                        MensajeParticipantes += ", " + Constantes.CONST_VALIDACION_DECLARANTE1 + " ó " + Constantes.CONST_VALIDACION_DECLARANTE2;
                    }
                    #endregion
                }
                else
                {
                    string doc;
                    if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.MATRIMONIO)
                    {
                        #region Matrimonio
                       
                        
                        String ValMatrimonio = ConfigurationManager.AppSettings["Matrimonio"].ToString();
                        String[] CodParti = ValMatrimonio.Trim().Split(',');
                        ContarCodParti = Convert.ToInt16(CodParti.Length);

                        foreach (String onjcaract in CodParti)
                        {

                            foreach (GridViewRow row in Grd_Participantes.Rows)
                            {
                                iCodTipoParticipante = row.Cells[0].Text;
                                ooParticipante = row.Cells[5].Text;
                                //doc = row.Cells[].Text;//sDocumentoTipoId                            
                                if (iCodTipoParticipante == onjcaract)
                                {
                                    ContarAux++;
                                    break;
                                }

                            }

                            if (ContarAux == 0)
                            {
                                string strDescripcion = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.REG_CIVIL_PARITICPANTE_MATRIMONIO, Convert.ToInt32(onjcaract), "descripcion");
                                MensajeParticipantes += "," + strDescripcion;

                            }
                            else
                            {
                                ContarAux = 0;
                            }
                        }
                        #endregion

                    }
                    else
                    {
                        if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.DEFUNCION)
                        {
                            #region Defuncion

                            string ValDefuncion = ConfigurationManager.AppSettings["Defuncion"].ToString();
                            if (chkInscripcionOficio.Checked == true)
                            {
                                int intDeclarante = (Int32)Enumerador.enmParticipanteDefuncion.DECLARANTE;

                                ValDefuncion = ValDefuncion.Replace("," + intDeclarante.ToString(), "");
                            }
                            String[] CodParti = ValDefuncion.Trim().Split(',');
                            ContarCodParti = Convert.ToInt16(CodParti.Length);

                            foreach (String onjcaract in CodParti)
                            {
                                foreach (GridViewRow row in Grd_Participantes.Rows)
                                {
                                    iCodTipoParticipante = row.Cells[0].Text;
                                    ooParticipante = row.Cells[5].Text;

                                    if (iCodTipoParticipante == onjcaract)
                                    {
                                        ContarAux++;
                                        break;
                                    }
                                }

                                if (ContarAux == 0)
                                {
                                    string strDescripcion = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.REG_CIVIL_PARTICIPANTE_DEFUNCION, Convert.ToInt32(onjcaract), "descripcion");
                                    MensajeParticipantes += "," + strDescripcion;
                                }
                                else
                                {
                                    ContarAux = 0;
                                }
                            }

                            #endregion
                        }
                    }

                }
            }
            else
            {
                if (Grd_Participantes.Rows.Count == 0)
                {

                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO CIVIL", "Error. Debe ingresar participantes.", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    return;

                }
            }

            if (MensajeParticipantes.Trim().Length > 0)
            {

                MensajeParticipantes = MensajeParticipantes.Trim().Substring(1);
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO CIVIL", "DEBE INGRESAR: " + MensajeParticipantes, false, 190, 350);
                Comun.EjecutarScript(Page, StrScript);
                return;
            }


            if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.NACIMIENTO)
            {
                #region Nacimiento
                //---------------------------------------------------
                //Fecha: 25/03/2022
                //Autor: Miguel Márquez Beltrán
                //Motivo: Solo validar cuando el titular 
                //         haya nacido en el exterior.
                //---------------------------------------------------
                string strDptoPaisTitular = this.ddl_DeptOcurrencia.SelectedValue;
                short intDptoPaisTitular = 0;
                if (strDptoPaisTitular.Trim().Length > 0)
                {
                    intDptoPaisTitular = Convert.ToInt16(strDptoPaisTitular);
                }
                //-------------------------------------
                //Solo válido para paises extranjeros.
                //-------------------------------------
                if (intDptoPaisTitular > 90)
                {
                #region Validar_Padres_Titular
                foreach (GridViewRow row in Grd_Participantes.Rows)
                {
                    #region Participantes

                    iCodTipoParticipante = row.Cells[0].Text;
                    //ooParticipante = row.Cells[5].Text;
                    vNacionalidad = row.Cells[10].Text;
                    //validacion padres por lo menos extrangero y peruano
                    if (iCodTipoParticipante == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.PADRE))
                    {
                        if (vNacionalidad == Convert.ToString((int)Enumerador.enmNacionalidad.PERUANA))
                        {
                            ContarPadresPeruanos++;
                        }
                    }
                    if (iCodTipoParticipante == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.MADRE))
                    {
                        if (vNacionalidad == Convert.ToString((int)Enumerador.enmNacionalidad.PERUANA))
                        {
                            ContarPadresPeruanos++;
                        }
                    }

                    

                    //validacion declarante por lo menos un peruano
                    if (iCodTipoParticipante == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.DECLARANTE_1))
                    {
                        intCantDeclarantes++;
                        if (vNacionalidad == Convert.ToString((int)Enumerador.enmNacionalidad.PERUANA))
                        {
                            ContarDeclarante++;
                        }
                    }
                    if (iCodTipoParticipante == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.DECLARANTE_2))
                    {
                        intCantDeclarantes++;
                        if (vNacionalidad == Convert.ToString((int)Enumerador.enmNacionalidad.PERUANA))
                        {
                            ContarDeclarante++;
                        }
                    }
                    #endregion
                }
               

                if (ContarPadresPeruanos == 0)
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO CIVIL", "Uno de los Padres debe de ser de nacionalidad Peruana", false, 190, 350);
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }

                //if (intCantDeclarantes == 2)
                //{
                //    if (ContarDeclarante == 0)
                //    {
                //        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO CIVIL", "Uno de los Declarantes debe de ser de nacionalidad Peruana", false, 190, 350);
                //        Comun.EjecutarScript(Page, StrScript);
                //        return;
                //    }
                //}
                    #endregion
                }
                #endregion
            }

            if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.MATRIMONIO)
            {
                #region Matrimonio

                if (tablaReconstitucionReposicion.Visible == true)
                {
                    if (chkReconstitucionReposicion.Checked)
                    {
                        if (txtTitularActa.Text.Trim().Length == 0)
                        {
                            StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO CIVIL", "Falta ingresar el nombre del Titular del Acta anterior.", false, 190, 250);
                            Comun.EjecutarScript(Page, StrScript);
                            return;
                        }
                    }
                }
                #region Participantes

                foreach (GridViewRow row in Grd_Participantes.Rows)
                {
                    iCodTipoParticipante = row.Cells[0].Text;
                    //ooParticipante = row.Cells[5].Text;
                    vNacionalidad = row.Cells[10].Text;
                    if (iCodTipoParticipante == Convert.ToString((int)Enumerador.enmParticipanteMatrimonio.DON))
                    {
                        if (vNacionalidad == Convert.ToString((int)Enumerador.enmNacionalidad.PERUANA))
                        {
                            ContarPadresPeruanos++;
                        }
                    }
                    if (iCodTipoParticipante == Convert.ToString((int)Enumerador.enmParticipanteMatrimonio.DONIA))
                    {
                        if (vNacionalidad == Convert.ToString((int)Enumerador.enmNacionalidad.PERUANA))
                        {
                            ContarPadresPeruanos++;
                        }
                    }
                }
                #endregion
                if (ContarPadresPeruanos == 0)
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO CIVIL", "Uno de los Cónyunges debe de ser de nacionalidad Peruana", false, 190, 350);
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }
                #endregion
            }
            long lngActuacionDetalleId = 0;

            if (HFGUID.Value.Length > 0)
            {
                lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);
            }
            else
            {
                lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
            }

            if (lngActuacionDetalleId == 0)
                return;

            string strScript = string.Empty;
            List<BE.RE_PARTICIPANTE> loParticipanteContainer = (List<BE.RE_PARTICIPANTE>)Session["Participante"];
            BE.MRE.RE_REGISTROCIVIL ObjRegCivBE = new BE.MRE.RE_REGISTROCIVIL();
            ActoCivilConsultaBL funActoCivil = new ActoCivilConsultaBL();

            if (Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != 0)
            {
                #region OBJETO REGISTRO CIVIL ...

                //---------------------------------------------------------------------------------------------------
                // Autor: Miguel Márquez Beltrán
                //Fecha:03/10/2016
                //Objetivo: Asignar un valor valido al atributo: ObjRegCivBE.reci_iRegistroCivilId para el 
                //          registro civil del titular.
                //---------------------------------------------------------------------------------------------------

                if (Convert.ToInt64(hifRegistroCivil.Value) > 0)
                {
                    ObjRegCivBE.reci_iRegistroCivilId = Convert.ToInt64(hifRegistroCivil.Value);
                }
                else
                {
                    hifRegistroCivil.Value = Session[strRegistroCivilId].ToString();
                    ObjRegCivBE.reci_iRegistroCivilId = Convert.ToInt64(Session[strRegistroCivilId].ToString());
                }

                ObjRegCivBE.reci_iActuacionDetalleId = lngActuacionDetalleId;                                                
                ObjRegCivBE.reci_sTipoActaId = Convert.ToInt16(this.ddlTipoActa.SelectedValue);
                ObjRegCivBE.reci_vNumeroActa = Convert.ToString(this.txtNroActa.Text);
                

                //---------------------------------------------------
                // Autor: Miguel Márquez Beltrán
                // Fecha: 05/10/2016
                // Objetivo: Mostrar la fecha actual del consulado
                //---------------------------------------------------
                //ObjRegCivBE.reci_dFechaRegistro = Comun.FormatearFecha(this.txtFechaRegistro.Text);
                ObjRegCivBE.reci_dFechaRegistro = Comun.FormatearFecha((Accesorios.Comun.ObtenerFechaActualTexto(HttpContext.Current.Session)));
                //---------------------------------------------------

                ObjRegCivBE.reci_vLibro = this.txtLibroRegCiv.Text.Trim().ToUpper();
                ObjRegCivBE.reci_vNumeroCUI = ((ddlTipoActa.SelectedValue == Convert.ToString((int)Enumerador.enmTipoActa.NACIMIENTO)) ? this.txtNroCUI.Text : string.Empty);

                #region SOLO PARA ACTA DE NACIMIENTO
                if ((ddlTipoActa.SelectedValue == Convert.ToString((int)Enumerador.enmTipoActa.NACIMIENTO)) || (ddlTipoActa.SelectedValue == Convert.ToString((int)Enumerador.enmTipoActa.DEFUNCION)))
                {
                    ObjRegCivBE.reci_sOcurrenciaTipoId = Convert.ToInt16(this.ddl_TipoOcurrencia.SelectedValue);
                    ObjRegCivBE.reci_vOcurrenciaLugar = this.txtLugarOcurrencia.Text.ToUpper();
                    ObjRegCivBE.reci_dFechaHoraOcurrenciaActo = FechaHora(this.txtFecNac.Text, this.txtHora.Text);                    
                }
                #endregion

                ObjRegCivBE.reci_cOcurrenciaUbigeo = this.ddl_DeptOcurrencia.SelectedValue + this.ddl_ProvOcurrencia.SelectedValue + this.ddl_DistOcurrencia.SelectedValue;
                ObjRegCivBE.reci_IOcurrenciaCentroPobladoId = null;
                string strUbigeo = string.Empty;
                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
                {
                    strUbigeo = Constantes.CONST_OFICINACONSULAR_LIMA_UBIGEO;
                }
                else
                {
                    strUbigeo = comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_cUbigeoCodigo").ToString();
                }
                ObjRegCivBE.reci_cOficinaRegistralUbigeo = strUbigeo;
                ObjRegCivBE.reci_IOficinaRegistralCentroPobladoId = null;

                #region SOLO PARA ACTA DE MATRIMONIO
                if (this.ddlTipoActa.SelectedValue == Convert.ToString((int)Enumerador.enmTipoActa.MATRIMONIO))
                {
                    ObjRegCivBE.reci_vNumeroExpedienteMatrimonio = this.txtNroExpediente.Text.ToUpper();
                    ObjRegCivBE.reci_vCargoCelebrante = this.txtCargoCelebrante.Text.ToUpper();
                    ObjRegCivBE.reci_dFechaHoraOcurrenciaActo = this.txtFecNac.Value();
                }
                #endregion

                ObjRegCivBE.reci_IAprobacionUsuarioId = 0;
                ObjRegCivBE.reci_vIPAprobacion = string.Empty;
                ObjRegCivBE.reci_dFechaAprobacion = Comun.FormatearFecha(null);
                ObjRegCivBE.reci_bDigitalizadoFlag = false;
                ObjRegCivBE.reci_bAnotacionFlag = false;


                //if (hValidaLey30738.Value == "1")
                //{
                //    //ObjRegCivBE.reci_vObservaciones = "LEY N° 30738 LEY DE REFORMA DEL ARTÍCULO N° 52 DE LA CONSTITUCIÓN POLÍTICA DEL PERÚ";
                //    ObjRegCivBE.reci_vObservaciones = "";
                //}
                //else {
                    ObjRegCivBE.reci_vObservaciones = txtCivilObservaciones.Text.Trim().ToUpper();
                //}
                
                ObjRegCivBE.reci_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                ObjRegCivBE.reci_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                ObjRegCivBE.reci_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                ObjRegCivBE.reci_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                ObjRegCivBE.reci_cConCUI = "N";

                if (chkconCUI.Visible == true)
                {
                    if (chkconCUI.Checked == true)
                    {
                        ObjRegCivBE.reci_cConCUI = "S";
                    }
                }
                if (chksinCUI.Visible == true)
                {
                    if (chksinCUI.Checked == true)
                    {
                        ObjRegCivBE.reci_cConCUI = "N";
                    }
                }
                if (tablaReconocimientoAdopcion.Visible == true)
                {
                    if (chkReconocimientoAdopcion.Checked)
                    {
                        ObjRegCivBE.reci_cReconocimientoAdopcion = "S";
                    }
                    else
                    {
                        ObjRegCivBE.reci_cReconocimientoAdopcion = "N";
                    }
                }
                else
                {
                    ObjRegCivBE.reci_cReconocimientoAdopcion = "N";
                }

                if (tablaReconstitucionReposicion.Visible == true)
                {
                    if (chkReconstitucionReposicion.Checked)
                    {
                        ObjRegCivBE.reci_cReconstitucionReposicion = "S";
                    }
                    else
                    {
                        ObjRegCivBE.reci_cReconstitucionReposicion = "N";
                    }
                }
                else
                {
                    ObjRegCivBE.reci_cReconstitucionReposicion = "N";
                }


                if (tablaReconocimientoAdopcion.Visible == true || tablaReconstitucionReposicion.Visible == true)
                {
                    if (chkReconocimientoAdopcion.Checked || chkReconstitucionReposicion.Checked)
                    {
                        if (txtNumeroActaAnterior.Text.Trim().Length == 0)
                        {
                            ObjRegCivBE.reci_iNumeroActaAnterior = null;
                        }
                        else
                        {
                            ObjRegCivBE.reci_iNumeroActaAnterior = Convert.ToInt32(txtNumeroActaAnterior.Text.Trim());
                        }
                        ObjRegCivBE.reci_vTitular = txtTitularActa.Text.Trim().ToUpper();
                    }
                    else
                    {
                        ObjRegCivBE.reci_iNumeroActaAnterior = null;
                        ObjRegCivBE.reci_vTitular = "";
                    }
                }
                else
                {
                    ObjRegCivBE.reci_iNumeroActaAnterior = null;
                    ObjRegCivBE.reci_vTitular = "";
                }
                //--------------------------------------------------


                #endregion Registro Civil

                #region Validar Tipos Participantes (Mientras se esperan las reglas)
//                List<RE_PARTICIPANTE> lstParticipantes = Participantes(ObjRegCivBE);

                if (Grd_Participantes.Rows.Count == 0)
                {
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO CIVIL", "Debe ingresar participantes.");
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }
                #endregion

                ActoCivilMantenimientoBL BL = new ActoCivilMantenimientoBL();
                int intResultado = 0;

                #region ACTUALIZA ACTO CIVIL

                if ((ObjRegCivBE.reci_iActuacionDetalleId != 0)
                    && (ObjRegCivBE.reci_iRegistroCivilId > 0))
                {
                    #region validación CUI
                    if (Convert.ToInt32(Session["TIPO_ACTO_PARTICIPANTE"]) == (int)Enumerador.enmTipoActa.NACIMIENTO)
                    {
                        if (hCUI.Value.Length > 0 && hCUI.Value.Trim() != txtNroCUI.Text.Trim())
                        {
                            DataTable dtRegCivil = new DataTable();
                            dtRegCivil = funActoCivil.ObtenerPorCUI(txtNroCUI.Text);
                            if (dtRegCivil.Rows.Count != 0)
                            {
                                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "REGISTRO CIVIL", "El número de CUI ingresado ya existe, ingrese otro");
                                Comun.EjecutarScript(Page, strScript);
                                return;
                            }
                        }
                    }
                    #endregion
                    intResultado = BL.Actualizar(ObjRegCivBE,
                                                 Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                                                 Participantes(ObjRegCivBE), null);
                }
                #endregion

                #region INSERTA ACTO CIVIL
                if (((Convert.ToInt32(Session["ActoCivil_Accion"]).Equals((int)Enumerador.enmTipoOperacion.REGISTRO)) && (ObjRegCivBE.reci_iActuacionDetalleId != 0))
                    || ((Convert.ToInt32(Session["ActoCivil_Accion"]).Equals((int)Enumerador.enmTipoOperacion.ACTUALIZACION)) && ObjRegCivBE.reci_iActuacionDetalleId != 0 && ObjRegCivBE.reci_iRegistroCivilId < 0)
                    || (txtIdTarifa.Text == Constantes.CONST_EXCEPCION_TARIFA_ID_1.ToString() && Convert.ToInt32(Session["ActoCivil_Accion"]).Equals((int)Enumerador.enmTipoOperacion.CONSULTA)))
                {
                    if (intResultado <= 0)
                    {
                        #region validación CUI
                        if (Convert.ToInt32(Session["TIPO_ACTO_PARTICIPANTE"]) == (int)Enumerador.enmTipoActa.NACIMIENTO)
                        {
                            if (txtNroCUI.Text.Trim().Length != 0)
                            {
                                DataTable dtRegCivil = new DataTable();
                                dtRegCivil = funActoCivil.ObtenerPorCUI(txtNroCUI.Text);
                                if (dtRegCivil.Rows.Count != 0)
                                {
                                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "REGISTRO CIVIL", "El número de CUI ingresado ya existe, ingrese otro");
                                    Comun.EjecutarScript(Page, strScript);
                                    return;
                                }
                            }
                        }
                        #endregion

                        long LonRegistroCivilId = 0;

                        intResultado = BL.Insertar(ObjRegCivBE, Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), Participantes(ObjRegCivBE), ref LonRegistroCivilId);
                        if (intResultado > 0)
                        {
                            hifRegistroCivil.Value = LonRegistroCivilId.ToString();
                            Session[strRegistroCivilId] = LonRegistroCivilId.ToString();
                            Session["ActoCivil_Accion"] = Enumerador.enmTipoOperacion.ACTUALIZACION;

                            CargarDatosRegistroCivil();

                            //intResultado = BL.Actualizar(ObjRegCivBE,
                            //                       Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                            //                       ParticipantesAgregarRecurrente(ObjRegCivBE), null);

                            //CargarDatosRegistroCivil();
                        }
                    }
                    
                }
                
                #endregion

                bool bgenerodocumento = false;

                if (intResultado > 0)
                {

                    cbxAfirmarTexto.Enabled = true;
                    ESTADOVISTAPREVIA.Value = "1";
                    Int64 RegistroCivil = Convert.ToInt64(hifRegistroCivil.Value);
                    Int64 iActuacionDetalle = Convert.ToInt64(HF_ACTUACIONDET_ID.Value);
                    Int32 iTipoActaId = Convert.ToInt32(ddlTipoActa.SelectedValue);
                    Boolean bol = Imprimir(RegistroCivil, iActuacionDetalle, iTipoActaId);

                    if (bol)
                    {

                        //string strUrl = "../Accesorios/VisorPDF.aspx";
                       // string strUrl = "../Accesorios/VisPDF.aspx";
                       // strScript = "window.open('" + strUrl + "', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";
                        bgenerodocumento = true;
                    }
                }
                else
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO CIVIL", "Ocurrió un problema al generarse la vista previa. Favor de volver a generar.");
                }
                //Comun.EjecutarScript(Page, strScript);
                //-----------------------------------------------------
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 28/09/2016
                // Objetivo: Descargar el archivo de la vista previa.
                //-----------------------------------------------------
                if (bgenerodocumento)
                {
                    ddlTipoActa.Enabled = false;
                    cbxAfirmarTexto.Enabled = true;
                    updFormato.Update();
                    Response.Redirect("../Accesorios/VisPDF.aspx", false);
                }
                else
                {
                    Comun.EjecutarScript(Page, strScript);
                }
                
                                                
            }
        }
        private Boolean ImprimirActaConformidad(Int32 intTipoActa)
        {
            Boolean Resultado = false;

            #region Datos de Recurrente

            String vNombreRecurrente = string.Empty;
            String vDocumento = string.Empty;


            if (ViewState["Nombre"] != null)
            {
                if (ViewState["Nombre"].ToString().Trim() != string.Empty)
                {
                    vNombreRecurrente += AplicarInicialMayuscula(ViewState["Nombre"].ToString().ToUpper());
                }
            }

            if (ViewState["ApePat"] != null)
            {
                if (ViewState["ApePat"].ToString().Trim() != string.Empty)
                {
                    vNombreRecurrente += " " + AplicarInicialMayuscula(ViewState["ApePat"].ToString().ToUpper());
                }
            }

            if (ViewState["ApeMat"] != null)
            {
                if (ViewState["ApeMat"].ToString().Trim() != string.Empty)
                {
                    vNombreRecurrente += " " + AplicarInicialMayuscula(ViewState["ApeMat"].ToString().ToUpper());
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
                    vDocumento += ViewState["DescTipDoc"].ToString().ToUpper();
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
            sContenido.Append(ObtenerDocumentoConformidad(vNombreRecurrente, vDocumento, intTipoActa));

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
            objetos[0] = vNombreRecurrente;
            objetos[1] = vDocumento;
            objetos[2] = true;

            listObjects.Add(objetos);


            #endregion

            DocumentoiTextSharp.CreateFilePDFConformidad(dtTMPReemplazar, strRutaHtml, strRutaPDF, HttpContext.Current.Server.MapPath("~/Images/Escudo.PNG"), listObjects);

            if (File.Exists(strRutaHtml))
            {
                File.Delete(strRutaHtml);
            }

            if (System.IO.File.Exists(strRutaPDF))
            {
                //WebClient User = new WebClient();
                //Byte[] FileBuffer = User.DownloadData(strRutaPDF);
                //if (FileBuffer != null)
                //{
                //    HttpContext.Current.Session["binaryData"] = FileBuffer;
                Resultado = true;
                //}
            }
            //------------------------
            //Autor: Miguel Angel Márquez Beltrán
            //Fecha: 27/09/2016
            //Objetivo: Asignar a la sesion el nombre y la ruta del archivo.
            //-------------------------

            HttpContext.Current.Session["rutaPDF"] = strRutaPDF;

            #endregion


            return Resultado;

        }
        [System.Web.Services.WebMethod]
        public static Boolean ImprimirActaConformidad(Int32 intTipoActa, string strGUID)
        {
            Boolean Resultado = false;

            #region Datos de Recurrente

            String vNombreRecurrente = string.Empty;
            String vDocumento = string.Empty;

            FrmActoCivil page = HttpContext.Current.Handler as FrmActoCivil;
            page.ViewState["Modules"] = null;

            if (HttpContext.Current.Session["Nombres" + strGUID] != null)
            {
                if (HttpContext.Current.Session["Nombres" + strGUID].ToString().Trim() != string.Empty)
                {
                    vNombreRecurrente += AplicarInicialMayuscula(HttpContext.Current.Session["Nombres" + strGUID].ToString().ToUpper());
                }
            }

            if (HttpContext.Current.Session["ApePat" + strGUID] != null)
            {
                if (HttpContext.Current.Session["ApePat" + strGUID].ToString().Trim() != string.Empty)
                {
                    vNombreRecurrente += " " + AplicarInicialMayuscula(HttpContext.Current.Session["ApePat" + strGUID].ToString().ToUpper());
                }
            }

            if (HttpContext.Current.Session["ApeMat" + strGUID] != null)
            {
                if (HttpContext.Current.Session["ApeMat" + strGUID].ToString().Trim() != string.Empty)
                {
                    vNombreRecurrente += " " + AplicarInicialMayuscula(HttpContext.Current.Session["ApeMat" + strGUID].ToString().ToUpper());
                }
            }

            if (HttpContext.Current.Session["ApeCasada" + strGUID] != null)
            {
                if (HttpContext.Current.Session["ApeCasada" + strGUID].ToString().Trim() != string.Empty)
                {
                    if (HttpContext.Current.Session["ApeCasada" + strGUID].ToString().Trim() != "&nbsp;")
                    {
                        vNombreRecurrente += " " + AplicarInicialMayuscula(HttpContext.Current.Session["ApeCasada" + strGUID].ToString());
                    }
                }
            }

            if (HttpContext.Current.Session["DescTipDoc" + strGUID] != null)
            {
                if (HttpContext.Current.Session["DescTipDoc" + strGUID].ToString().Trim() != string.Empty)
                {
                    vDocumento += HttpContext.Current.Session["DescTipDoc" + strGUID].ToString().ToUpper();
                }
            }

            if (HttpContext.Current.Session["NroDoc" + strGUID] != null)
            {
                if (HttpContext.Current.Session["NroDoc" + strGUID].ToString().Trim() != string.Empty)
                {
                    vDocumento += " " + HttpContext.Current.Session["NroDoc" + strGUID].ToString();
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
            sContenido.Append(ObtenerDocumentoConformidad(vNombreRecurrente, vDocumento, intTipoActa, strGUID));

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
            objetos[0] = vNombreRecurrente;
            objetos[1] = vDocumento;
            objetos[2] = true;

            listObjects.Add(objetos);


            #endregion

            DocumentoiTextSharp.CreateFilePDFConformidad(dtTMPReemplazar, strRutaHtml, strRutaPDF, HttpContext.Current.Server.MapPath("~/Images/Escudo.PNG"), listObjects);

            if (File.Exists(strRutaHtml))
            {
                File.Delete(strRutaHtml);
            }

            if (System.IO.File.Exists(strRutaPDF))
            {
                //WebClient User = new WebClient();
                //Byte[] FileBuffer = User.DownloadData(strRutaPDF);
                //if (FileBuffer != null)
                //{
                //    HttpContext.Current.Session["binaryData"] = FileBuffer;
                    Resultado = true;
                //}
            }
            //------------------------
            //Autor: Miguel Angel Márquez Beltrán
            //Fecha: 27/09/2016
            //Objetivo: Asignar a la sesion el nombre y la ruta del archivo.
            //-------------------------

            HttpContext.Current.Session["rutaPDF"] = strRutaPDF;

            #endregion


            return Resultado;

        }

        [System.Web.Services.WebMethod]
        public static Boolean GetGenerarAutoAdhesivosPDF(string strGUID)
        {

            DocumentoiTextSharp oDocumentoiTextSharp = new DocumentoiTextSharp(null, string.Empty, HttpContext.Current.Server.MapPath("~/Images/Escudo.JPG"));
            Int64 iActuacionDetalleId = 0;

            if (strGUID.Length > 0)
            {
                iActuacionDetalleId = Convert.ToInt64(HttpContext.Current.Session[Constantes.CONST_SESION_ACTUACIONDET_ID + strGUID]);
            }
            else
            {
                iActuacionDetalleId = Convert.ToInt64(HttpContext.Current.Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
            }
            
            oDocumentoiTextSharp.bGenerarDocumentoAutomaticamente = false;
            oDocumentoiTextSharp.ActuacionDetalleId = iActuacionDetalleId;

            oDocumentoiTextSharp.CrearAutoAdhesivo();

            return true;
        }

        private static String AplicarInicialMayuscula(string palabra)
        {

            if (palabra.Trim() != string.Empty)
            {
                palabra = palabra.ToLower();
                return palabra[0].ToString().ToUpper() + palabra.Substring(1, palabra.Length - 1); ;
            }

            return string.Empty;
        }
        private String ObtenerDocumentoConformidad(String vNombre, String vDocumento, Int32 TipoActa)
        {

            bool bEsMujer = false;

            if (ViewState["PER_GENERO"].ToString() == Convert.ToInt32(Enumerador.enmGenero.FEMENINO).ToString())
            {
                bEsMujer = true;
            }


            StringBuilder sScript = new StringBuilder();

            sScript.Append("<p align=\"justify\"; style=\"background-color:transparent; font-family:arial;\" >");
            sScript.Append("Yo, ");
            sScript.Append(vNombre.ToUpper());
            sScript.Append(", identificad");

            if (bEsMujer)
                sScript.Append("a");
            else
                sScript.Append("o");

            sScript.Append(" con ");
            sScript.Append(vDocumento.ToUpper());
            sScript.Append(", ");
            sScript.Append("declaro que he leído y revisado en su detalle el documento de ");

            if (TipoActa == (int)Enumerador.enmTipoActa.NACIMIENTO)
            {
                sScript.Append("ACTA DE NACIMIENTO");
            }
            else if (TipoActa == (int)Enumerador.enmTipoActa.MATRIMONIO)
            {
                sScript.Append("ACTA DE MATRIMONIO");

            }
            else if (TipoActa == (int)Enumerador.enmTipoActa.DEFUNCION)
            {
                sScript.Append("ACTA DE DEFUNCIÓN");
            }

            sScript.Append(", que he tenido a la vista y me ha sido entregado en la fecha,");
            sScript.Append(" manifestando mi conformidad con su contenido.");
            sScript.Append("</p>");
            sScript.Append("<br />");


            sScript.Append("<p align=\"right\"; style=\"background-color:transparent; font-family:arial;\">");
            DateTime dt_Fecha = Comun.FormatearFecha((Accesorios.Comun.ObtenerFechaActualTexto(HttpContext.Current.Session)));
            string str_Fecha = dt_Fecha.ToString("dd") + " de " + AplicarInicialMayuscula(dt_Fecha.ToString("MMMM")) + " de " + dt_Fecha.ToString("yyyy");


            if (HttpContext.Current.Session["CiudadOficinaConsular"] != null)
            {

                str_Fecha = HttpContext.Current.Session["CiudadOficinaConsular"].ToString().ToUpper() + ", " + str_Fecha;
            }

            sScript.Append(str_Fecha);
            sScript.Append("</p>");
            sScript.Append("<br />");
            sScript.Append("<tab></tab>");


            return sScript.ToString();
        }
        private static String ObtenerDocumentoConformidad(String vNombre, String vDocumento, Int32 TipoActa, string strGUID)
        {

            bool bEsMujer = false;
            
            if (HttpContext.Current.Session["PER_GENERO" + strGUID].ToString() == Convert.ToInt32(Enumerador.enmGenero.FEMENINO).ToString())
            {
                bEsMujer = true;
            }


            StringBuilder sScript = new StringBuilder();

            sScript.Append("<p align=\"justify\"; style=\"background-color:transparent; font-family:arial;\" >");
            sScript.Append("Yo, ");
            sScript.Append(vNombre.ToUpper());
            sScript.Append(", identificad");

            if (bEsMujer)
                sScript.Append("a");
            else
                sScript.Append("o");

            sScript.Append(" con ");
            sScript.Append(vDocumento.ToUpper());
            sScript.Append(", ");
            sScript.Append("declaro que he leído y revisado en su detalle el documento de ");

            if (TipoActa == (int)Enumerador.enmTipoActa.NACIMIENTO)
            {
                sScript.Append("ACTA DE NACIMIENTO");
            }
            else if (TipoActa == (int)Enumerador.enmTipoActa.MATRIMONIO)
            {
                sScript.Append("ACTA DE MATRIMONIO");

            }
            else if (TipoActa == (int)Enumerador.enmTipoActa.DEFUNCION)
            {
                sScript.Append("ACTA DE DEFUNCIÓN");
            }

            sScript.Append(", que he tenido a la vista y me ha sido entregado en la fecha,");
            sScript.Append(" manifestando mi conformidad con su contenido.");
            sScript.Append("</p>");
            sScript.Append("<br />");


            sScript.Append("<p align=\"right\"; style=\"background-color:transparent; font-family:arial;\">");
            DateTime dt_Fecha = Comun.FormatearFecha((Accesorios.Comun.ObtenerFechaActualTexto(HttpContext.Current.Session)));
            string str_Fecha = dt_Fecha.ToString("dd") + " de " + AplicarInicialMayuscula(dt_Fecha.ToString("MMMM")) + " de " + dt_Fecha.ToString("yyyy");


            if (HttpContext.Current.Session["CiudadOficinaConsular"] != null)
            {

                str_Fecha = HttpContext.Current.Session["CiudadOficinaConsular"].ToString().ToUpper() + ", " + str_Fecha;
            }

            sScript.Append(str_Fecha);
            sScript.Append("</p>");
            sScript.Append("<br />");
            sScript.Append("<tab></tab>");


            return sScript.ToString();
        }
        
        protected void cbxAfirmarTexto_CheckedChanged(object sender, EventArgs e)
        {
            Int16 ContarCodParti = 0;  //Contar la Cantidad de Participante obligatorios a registrar obtenido del WEBConfing
            String iCodTipoParticipante = String.Empty; //Codigo del Participante
            String ooParticipante = String.Empty; //Descripcion del Participante
            Int16 ContarAux = 0;
            String MensajeParticipantes = String.Empty; //Mensaje de Error Para los Participante Obligatorios

            string StrScript;
            if (Grd_Participantes.Rows.Count > 0)
            {

                if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.NACIMIENTO)
                {
                    #region Nacimiento

                    String ValNacimineto = ConfigurationManager.AppSettings["Nacimiento"].ToString();
                    String[] CodParti = ValNacimineto.Trim().Split(',');

                    ContarCodParti = Convert.ToInt16(CodParti.Length);
                    int countPadre = 0;
                    int countDeclarante = 0;


                    foreach (String onjcaract in CodParti)
                    {

                        foreach (GridViewRow row in Grd_Participantes.Rows)
                        {
                            iCodTipoParticipante = row.Cells[0].Text;
                            //ooParticipante = row.Cells[5].Text;


                            if (onjcaract == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.DECLARANTE_1) || onjcaract == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.DECLARANTE_2))
                            {
                                if (iCodTipoParticipante == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.DECLARANTE_1) || iCodTipoParticipante == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.DECLARANTE_2))
                                {
                                    ContarAux++;
                                    break;
                                }
                            }

                            if (onjcaract == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.MADRE) || onjcaract == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.PADRE))
                            {
                                if (iCodTipoParticipante == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.MADRE) || iCodTipoParticipante == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.PADRE))
                                {
                                    ContarAux++;
                                    break;
                                }
                            }

                            if (iCodTipoParticipante == onjcaract)
                            {

                                ContarAux++;
                                break;
                            }


                        }

                        if (ContarAux == 0)
                        {
                            string strDescripcion = string.Empty;
                            strDescripcion = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.REG_CIVIL_PARTICIPANTE_NACIMIENTO, Convert.ToInt32(onjcaract), "descripcion");


                            if (strDescripcion == Enumerador.enmParticipanteNacimiento.PADRE.ToString() || strDescripcion == Enumerador.enmParticipanteNacimiento.MADRE.ToString())
                            {
                                countPadre++;
                            }
                            else if (strDescripcion == Constantes.CONST_VALIDACION_DECLARANTE1 || strDescripcion == Constantes.CONST_VALIDACION_DECLARANTE2)
                            {
                                countDeclarante++;
                            }
                            else
                            {

                                MensajeParticipantes += "," + strDescripcion;
                            }
                        }
                        else
                        {
                            ContarAux = 0;
                        }

                    }

                    if (countPadre == Constantes.CONST_VALIDACION_PADRES)
                    {
                        MensajeParticipantes += ", " + Enumerador.enmParticipanteNacimiento.PADRE.ToString() + " ó " + Enumerador.enmParticipanteNacimiento.MADRE.ToString();
                    }
                    if (countDeclarante == Constantes.CONST_VALIDACION_DECLARANTE)
                    {
                        MensajeParticipantes += ", " + Constantes.CONST_VALIDACION_DECLARANTE1 + " ó " + Constantes.CONST_VALIDACION_DECLARANTE2;
                    }
                    #endregion
                }
                else
                {
                    if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.MATRIMONIO)
                    {
                        #region Matrimonio


                        String ValMatrimonio = ConfigurationManager.AppSettings["Matrimonio"].ToString();
                        String[] CodParti = ValMatrimonio.Trim().Split(',');
                        ContarCodParti = Convert.ToInt16(CodParti.Length);

                        foreach (String onjcaract in CodParti)
                        {

                            foreach (GridViewRow row in Grd_Participantes.Rows)
                            {
                                iCodTipoParticipante = row.Cells[0].Text;
                                ooParticipante = row.Cells[5].Text;
                                //doc = row.Cells[].Text;//sDocumentoTipoId                            
                                if (iCodTipoParticipante == onjcaract)
                                {
                                    ContarAux++;
                                    break;
                                }

                            }

                            if (ContarAux == 0)
                            {
                                string strDescripcion = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.REG_CIVIL_PARITICPANTE_MATRIMONIO, Convert.ToInt32(onjcaract), "descripcion");
                                MensajeParticipantes += "," + strDescripcion;

                            }
                            else
                            {
                                ContarAux = 0;
                            }
                        }
                        #endregion

                    }
                    else
                    {
                        if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.DEFUNCION)
                        {
                            #region Defuncion

                            string ValDefuncion = ConfigurationManager.AppSettings["Defuncion"].ToString();
                            if (chkInscripcionOficio.Checked == true)
                            {
                                int intDeclarante = (Int32)Enumerador.enmParticipanteDefuncion.DECLARANTE;

                                ValDefuncion = ValDefuncion.Replace("," + intDeclarante.ToString(), "");
                            }
                            String[] CodParti = ValDefuncion.Trim().Split(',');
                            ContarCodParti = Convert.ToInt16(CodParti.Length);

                            foreach (String onjcaract in CodParti)
                            {
                                foreach (GridViewRow row in Grd_Participantes.Rows)
                                {
                                    iCodTipoParticipante = row.Cells[0].Text;
                                    ooParticipante = row.Cells[5].Text;

                                    if (iCodTipoParticipante == onjcaract)
                                    {
                                        ContarAux++;
                                        break;
                                    }
                                }

                                if (ContarAux == 0)
                                {
                                    string strDescripcion = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.REG_CIVIL_PARTICIPANTE_DEFUNCION, Convert.ToInt32(onjcaract), "descripcion");
                                    MensajeParticipantes += "," + strDescripcion;
                                }
                                else
                                {
                                    ContarAux = 0;
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
            else
            {
                if (Grd_Participantes.Rows.Count == 0)
                {
                    cbxAfirmarTexto.Checked = false;
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO CIVIL", "Error. Debe ingresar participantes.", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    return;

                }
            }

            if (MensajeParticipantes.Trim().Length > 0)
            {
                cbxAfirmarTexto.Checked = false;
                MensajeParticipantes = MensajeParticipantes.Trim().Substring(1);
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO CIVIL", "DEBE INGRESAR: " + MensajeParticipantes, false, 190, 350);
                Comun.EjecutarScript(Page, StrScript);
                return;
            }
            string strScript = string.Empty;
            updFormato.Update();

            if (cbxAfirmarTexto.Checked)
            {
                if (Session[strVariableAccion].ToString() == Enumerador.enmTipoOperacion.ACTUALIZACION.ToString())
                {
                    strScript += Util.HabilitarTab(4);
                }
            }
            else
            {
                strScript += Util.DeshabilitarTab(4);
            }
            Comun.EjecutarScript(Page, strScript);
        }

        protected void rbNo_CheckedChanged(object sender, EventArgs e)
        {
            HabilitarSegunExistenciaDocumento();
        }

        #region metodos generales 


        public void limpiar_cabezera(){
            txtLibroRegCiv.Text = string.Empty;
            txtNroActa.Text = string.Empty;
            txtNroCUI.Text = string.Empty;
            txtNroExpediente.Text = string.Empty;
            txtCargoCelebrante.Text = string.Empty;
            txtHora.Text = string.Empty;
            ddl_TipoOcurrencia.SelectedValue = "0";
            txtLugarOcurrencia.Text = string.Empty;
            ddl_DeptOcurrencia.SelectedIndex = 0;
            ddl_ProvOcurrencia.SelectedIndex = 0;
            ddl_DistOcurrencia.SelectedIndex = 0;
            txtNombresTitular.Text = string.Empty;
            txtApePatTitular.Text = string.Empty;
            txtApeMatTitular.Text = string.Empty;


        }

        protected void btnDesabilitarAutoahesivo_Click(object sender, EventArgs e)
        {
            btnVistaPrev.Enabled = false;
            chkImpresion.Checked = true;
            chkImpresion.Enabled = false;
            txtCodAutoadhesivo.Enabled = false;
            btnLimpiar.Enabled = false;
            hdn_ImpresionCorrecta.Value = "1";
            if (HFGUID.Value.Length > 0)
            {
                BindGridActuacionesInsumoDetalle(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]));
            }
            else
            {
                BindGridActuacionesInsumoDetalle(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]));
            }
            updVinculacion.Update();
        }


        private void Enable_vinculo()
        {
            //  ticket 368 el tipo de dato no debe mostrarse PADRE y MADRE tanto en nacimiento y defuncion
            if (Convert.ToInt32(this.ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.NACIMIENTO || Convert.ToInt32(this.ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.DEFUNCION)
            {
                foreach (System.Web.UI.WebControls.ListItem item in ddl_TipoVinculoParticipante.Items)
                {
                    if (item.Value.ToString() == Convert.ToString((Int16)Enumerador.enmVinculo.PADRE) || item.Value.ToString() == Convert.ToString((Int16)Enumerador.enmVinculo.MADRE))
                    {
                        item.Enabled = false;
                    }
                }
            }
        }

        private void Enable_estado()
        {
            //  ticket 368 el tipo de dato no debe mostrarse PADRE y MADRE tanto en nacimiento y defuncion
            if (Convert.ToInt32(this.ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.MATRIMONIO )
            {
                foreach (System.Web.UI.WebControls.ListItem item in CmbEstCiv.Items)
                {
                    if (item.Value.ToString() == Convert.ToString((Int16)Enumerador.enmEstadoCivil.CASADO) )
                    {
                        item.Enabled = false;
                    }
                }
            }
        }
        private void Cargar_Actuacion()
        {

            long iPersonaID = Convert.ToInt64(HF_iPersonaID.Value);
            long iActuacionID = Convert.ToInt64(HF_ACT_ID.Value);

            ActuacionConsultaBL oActuacionConsultaBL = new ActuacionConsultaBL();
            gdvActuaciones.DataSource = oActuacionConsultaBL.ActuacionDetalleObtener_Actuacion(iPersonaID, iActuacionID);
            gdvActuaciones.DataBind();
        }
        public void Activar_Ubicacion(Boolean activar)
        {
            ctrlUbigeo1.Visible = activar;
            lblUbigeoParticipantes.Visible = activar;
        }

        static DataTable CrearTmpTabla()
        {
            DataTable dtTablaTemporal = new DataTable();

            dtTablaTemporal.Columns.Add("strCadenaBuscar", typeof(string));
            dtTablaTemporal.Columns.Add("strCadenaReemplazar", typeof(string));

            return dtTablaTemporal;
        }


        #endregion

        protected void btn_deabilidardefuncion_Click(object sender, EventArgs e)
        {
            CtrldFecNacimientoParticipante.Enabled = true;
            HabilitarControlParticipanteRune(true);
            //txtNomParticipante.Enabled = true;
            //txtApeMatParticipante.Enabled = true;
            //txtApePatParticipante.Enabled = true;
            ddl_Genero.Enabled = true;
        }

        //----------------------------------------------
        // Autor: Miguel Márquez Beltrán
        //Fecha:13/10/2016
        //Objetivo: Deshabilitar el calculo de la edad
        //----------------------------------------------
        private string ObtenerEdad(DateTime dFechaNac,bool numerico = false)
        {
            DateTime datFechaHoy = new DateTime();
            datFechaHoy = Comun.FormatearFecha((Accesorios.Comun.ObtenerFechaActualTexto(HttpContext.Current.Session)));

            string strEdad = Comun.DiferenciaFechas(datFechaHoy, dFechaNac, "--", numerico);

            return strEdad;
        }
        private string ObtenerEdad(DateTime dFechaNac, DateTime dFechaFallece)
        {
            string strEdad = Comun.DiferenciaFechas(dFechaFallece, dFechaNac, "--");

            return strEdad;
        }

        private void HabilitarControlesPersonaNoExiste()
        {
            //----------------------------------------------
            // Autor: Jonatan Silva Cachay
            //Fecha: 08/05/2017
            //Objetivo: habilitar Controles
            //----------------------------------------------
            string striPersonaId = "";

            //if (HFGUID.Value.Length > 0)
            //{
            //    striPersonaId = HttpContext.Current.Session["iPersonaId" + HFGUID.Value].ToString();
            //}
            //else
            //{
                striPersonaId = ViewState["iPersonaId"].ToString();
            //}

            if (striPersonaId == "0")
            {

                txtNomParticipante.Enabled = true;
                txtApeMatParticipante.Enabled = true;
                txtApePatParticipante.Enabled = true;
                
                DropDownList _ddlContDep = ctrlUbigeo1.FindControl("ddl_ContDepParticipanteAP") as DropDownList;
                DropDownList _ddlPaisCiudad = ctrlUbigeo1.FindControl("ddl_PaisCiudadParticipanteAP") as DropDownList; 
                DropDownList _ddlCuidadDistrito = ctrlUbigeo1.FindControl("ddl_CiudadDistritoParticipanteAP") as DropDownList; 
                DropDownList _ddlCentroPoblado = ctrlUbigeo1.FindControl("ddl_CentroPobladoParticipanteAP") as DropDownList;
                _ddlContDep.Enabled = true;
                _ddlPaisCiudad.Enabled = true;
                _ddlCuidadDistrito.Enabled = true;
                _ddlCentroPoblado.Enabled = true;

            }

        }

        private void MostrarEdad()
        {
            //----------------------------------------------
            // Autor: Miguel Márquez Beltrán
            //Fecha:14/10/2016
            //Objetivo: Calcular la edad
            //----------------------------------------------
            if (CtrldFecNacimientoParticipante.Text.Length == 0) { return; }

            if (ddlTipoActa.SelectedItem.Value == Convert.ToString((int)Enumerador.enmTipoActa.MATRIMONIO))
            {
                if (ddl_TipoParticipante.SelectedItem.Value == Convert.ToString((int)Enumerador.enmParticipanteMatrimonio.DON) || ddl_TipoParticipante.SelectedItem.Value == Convert.ToString((int)Enumerador.enmParticipanteMatrimonio.DONIA))
                {
                    DateTime datFechaNac = new DateTime();
                    if (!DateTime.TryParse(CtrldFecNacimientoParticipante.Text, out datFechaNac))
                    {
                        datFechaNac = Comun.FormatearFecha(CtrldFecNacimientoParticipante.Text);
                    }
                    LblEdad2.Text = ObtenerEdad(datFechaNac);
                    HabilitarControlesPersonaNoExiste();
                }                
            }
            if (txtFecNac.Text.Length == 0) { return; }
            if (ddlTipoActa.SelectedItem.Value == Convert.ToString((int)Enumerador.enmTipoActa.DEFUNCION))
            {
                if (ddl_TipoParticipante.SelectedItem.Value == Convert.ToString((int)Enumerador.enmParticipanteDefuncion.TITULAR))
                {                    
                    DateTime datFechaNac = new DateTime();
                    if (!DateTime.TryParse(CtrldFecNacimientoParticipante.Text, out datFechaNac))
                    {
                        datFechaNac = Comun.FormatearFecha(CtrldFecNacimientoParticipante.Text);
                    }
                    DateTime datFechaFallece = new DateTime();
                    if (!DateTime.TryParse(txtFecNac.Text, out datFechaFallece))
                    {
                        datFechaFallece = Comun.FormatearFecha(txtFecNac.Text);
                    }

                    LblEdad2.Text = ObtenerEdad(datFechaNac, datFechaFallece);
                }
            }                
        }

        protected void btnSolicitudInscr_Click(object sender, EventArgs e)
        {
            //Obtener Datos de Impresión
            long iActuacionID = Convert.ToInt64(HF_ACT_ID.Value);
            int intCodParametro = Convert.ToInt16(comun_Part1.ObtenerParametroDatoPorCampo(Session, "IMPRESION-TIPO DOCUMENTO", "SOLICITUD DE INSCRIPCIÓN RENIEC", "id"));

            SGAC.Reportes.BL.DataImpresionBL Obj = new SGAC.Reportes.BL.DataImpresionBL();
            DataTable _dt = Obj.ObtenerDataDeImpresion(iActuacionID, intCodParametro);


            if (_dt.Rows.Count == 0)
            {
                string StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Acto Civil", "No se ha generado la data de impresión", false, 190, 250);
                Comun.EjecutarScript(Page, StrScript);
                return;
            }
            else
            {
                string sNombreOficinaConsular = comun_Part2.ObtenerNombreOficinaPorId(Session, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString()));
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

                ReportParameter[] parameters = new ReportParameter[1];
                parameters[0] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());

                
                
                Session["DtDatos"] = _dt;
                Session["objParametroReportes"] = parameters;
                Session["DataSet"] = "DtSolicitudInscripcion";
                string strUrl = "../Reportes/frmVisorReporte.aspx";
                string Script = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=700,height=700,left=100,top=100');";
                Comun.EjecutarScript(Page, Script);
            }
        }

        protected void imgBuscarActa_Click(object sender, ImageClickEventArgs e)
        {
            if (txtNumeroActa.Text.Trim().Length > 0)
            {

                DataTable dtActasTitulares = new DataTable();

                ActoCivilConsultaBL objActoCivilConsultaBL = new ActoCivilConsultaBL();

                int intRegistroCivilId = 0;
                string strNumeroActa = txtNumeroActa.Text.Trim();
                string strApPaterno = "";
                string strApMaterno = "";
                string strNombres = "";

                int IntTotalCount = 0;
                int IntTotalPages = 0;
                int intPaginaCantidad = Constantes.CONST_PAGE_SIZE_ADJUNTOS;
                int PaginaActual = 1;

                dtActasTitulares = objActoCivilConsultaBL.Consultar_Actas_Titulares(PaginaActual, intPaginaCantidad, ref IntTotalCount, ref IntTotalPages, intRegistroCivilId, strNumeroActa, strApPaterno, strApMaterno, strNombres);

                if (dtActasTitulares.Rows.Count > 0)
                {
                    if (dtActasTitulares.Rows.Count == 1)
                    {
                        txtTitular.Text = dtActasTitulares.Rows[0]["TITULAR"].ToString();
                    }
                    else
                    {
                        string strConyuges = "";
                        for (int i = 0; i < dtActasTitulares.Rows.Count; i++)
                        {
                            strConyuges = strConyuges + dtActasTitulares.Rows[0]["TITULAR"].ToString().Trim() + "/";
                        }
                        strConyuges = strConyuges.Substring(0, strConyuges.Length - 1);
                        if (strConyuges.Length > 200)
                        {
                            strConyuges = strConyuges.Substring(0, 200);
                        }
                        txtTitular.Text = strConyuges;
                    }
                    ddlTipoActaAnotacion.SelectedValue = dtActasTitulares.Rows[0]["reci_sTipoActaId"].ToString();
                    txtTitular.Enabled = false;                    
                }
                else
                {
                    txtTitular.Enabled = true;                    
                    string StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Acto Civil", "No se encontraron datos", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);

                }
            }
            else
            {
                txtTitular.Text = "";
                txtTitular.Enabled = true;
                ddlTipoActaAnotacion.SelectedIndex = 0;
                ddlTipoActaAnotacion.Enabled = true;

            }
        }

        protected void btnBuscarTitular_Click(object sender, EventArgs e)
        {            
            Session["ES_ANOTACION_O_TARIFA_1"] = "ANOTACION";
            Grd_BusquedaActas.DataSource = null;
            Grd_BusquedaActas.DataBind();
            txtBuscarApPaterno.Focus();
            CtrlPageBarActas.Visible = false;
            CtrlPageBarActas.InicializarPaginador();
            updBusquedaTitular.Update();

            ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "limpiarCamposBusquedaActas();abrirPopupBusquedaActas();", true);
        }
       

        protected void btnFiltrarTitulares_Click(object sender, EventArgs e)
        {

            int intRegistroCivilId = 0;
            string strNumeroActa = "";
            string strApPaterno = txtBuscarApPaterno.Text.Trim();
            string strApMaterno = txtBuscarApMaterno.Text.Trim();
            string strNombres = txtBuscarNombre.Text.Trim();

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_PAGE_SIZE_ADJUNTOS;
            int PaginaActual = CtrlPageBarActas.PaginaActual;


            if (strApPaterno.Length >= 3 || strApMaterno.Length >= 3 || strNombres.Length >= 3)
            {                
                DataTable dtActasTitulares = new DataTable();

                ActoCivilConsultaBL objActoCivilConsultaBL = new ActoCivilConsultaBL();

                Int16 intTipoActaId = 0;

                if (Session["ES_ANOTACION_O_TARIFA_1"].Equals("TARIFA_1"))
                {
                    intTipoActaId = Convert.ToInt16(this.ddlTipoActa.SelectedValue.ToString());
                    dtActasTitulares = objActoCivilConsultaBL.Consultar_Actas_Titulares(PaginaActual, intPaginaCantidad, ref IntTotalCount, ref IntTotalPages, intRegistroCivilId, strNumeroActa, strApPaterno, strApMaterno, strNombres, intTipoActaId);
                }
                else
                {
                    intTipoActaId = Convert.ToInt16(this.ddlTipoActaAnotacion.SelectedValue.ToString());
                    dtActasTitulares = objActoCivilConsultaBL.Consultar_Actas_Titulares(PaginaActual, intPaginaCantidad, ref IntTotalCount, ref IntTotalPages, intRegistroCivilId, strNumeroActa, strApPaterno, strApMaterno, strNombres, intTipoActaId);
                }

                dtActasTitulares = objActoCivilConsultaBL.Consultar_Actas_Titulares(PaginaActual, intPaginaCantidad, ref IntTotalCount, ref IntTotalPages, intRegistroCivilId, strNumeroActa, strApPaterno, strApMaterno, strNombres, intTipoActaId);

                if (dtActasTitulares.Rows.Count > 0)
                {
                    Grd_BusquedaActas.DataSource = dtActasTitulares;
                    Grd_BusquedaActas.DataBind();

                    CtrlPageBarActas.TotalResgistros = IntTotalCount;
                    CtrlPageBarActas.TotalPaginas = IntTotalPages;

                    CtrlPageBarActas.Visible = false;

                    if (CtrlPageBarActas.TotalResgistros > intPaginaCantidad)
                    {
                        CtrlPageBarActas.Visible = true;
                    }
                }
                else
                {
                    Grd_BusquedaActas.DataSource = null;
                    Grd_BusquedaActas.DataBind();
                    CtrlPageBarActas.Visible = false;
                    string StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Búsqueda", "No se encontraron datos", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                }
            }
            else
            {
                Grd_BusquedaActas.DataSource = null;
                Grd_BusquedaActas.DataBind();
                string StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Búsqueda", Constantes.CONST_VALIDACION_MIN_3_CARACTERES, false, 190, 250);
                Comun.EjecutarScript(Page, StrScript);                
            }            
        }

        protected void Grd_BusquedaActas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Select")
            {
                if (Session["ES_ANOTACION_O_TARIFA_1"].Equals("ANOTACION"))
                {
                    long iRegistroCivilId = Convert.ToInt64(Grd_BusquedaActas.Rows[index].Cells[0].Text);
                    string strTipoActaId = Grd_BusquedaActas.Rows[index].Cells[1].Text;
                    string strLibro = Grd_BusquedaActas.Rows[index].Cells[2].Text.Trim();
                    string strNumeroActa = Grd_BusquedaActas.Rows[index].Cells[3].Text.Trim();
                    string strNroDocumento = Grd_BusquedaActas.Rows[index].Cells[4].Text.Trim();
                    string strTitular = HttpUtility.HtmlDecode(Grd_BusquedaActas.Rows[index].Cells[5].Text.Trim());

                    txtNumeroActa.Text = strNumeroActa;
                    txtTitular.Text = strTitular;
                    ddlTipoActaAnotacion.SelectedValue = strTipoActaId;                    
                    txtTitular.Enabled = false;
                    updAnotaciones.Update();
                }
                if (Session["ES_ANOTACION_O_TARIFA_1"].Equals("TARIFA_1"))
                {
                    string strNumeroActaAnterior = Grd_BusquedaActas.Rows[index].Cells[3].Text.Trim();
                    string strTitular = HttpUtility.HtmlDecode(Grd_BusquedaActas.Rows[index].Cells[5].Text.Trim());

                    txtNumeroActaAnterior.Text = strNumeroActaAnterior;
                    txtTitularActa.Text = strTitular;
                    txtTitularActa.Enabled = false;

                    updFormato.Update();
                }
                ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "cerrarPopupBusquedaActas();", true);
            }
        }
        protected void Grd_BusquedaActas_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                e.Row.ToolTip = "Haga Click para seleccionar la fila.";
                e.Row.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink(this.Grd_BusquedaActas, "Select$" + e.Row.RowIndex);
            }
        }
        protected void CtrlPageBarActas_Click(object sender, EventArgs e)
        {
            btnFiltrarTitulares_Click(sender, e);
            updBusquedaTitular.Update();
        }
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            if (txtCodAutoadhesivo.Enabled)
            {
                txtCodAutoadhesivo.Text = "";
            }
        }



protected void imgBuscarActaAnterior_Click(object sender, ImageClickEventArgs e)
        {
            if (txtNumeroActaAnterior.Text.Trim().Length > 0)
            {

                DataTable dtActasTitulares = new DataTable();

                ActoCivilConsultaBL objActoCivilConsultaBL = new ActoCivilConsultaBL();

                int intRegistroCivilId = 0;
                string strNumeroActaAnterior = txtNumeroActaAnterior.Text.Trim();
                string strApPaterno = "";
                string strApMaterno = "";
                string strNombres = "";

                int IntTotalCount = 0;
                int IntTotalPages = 0;
                int intPaginaCantidad = Constantes.CONST_PAGE_SIZE_ADJUNTOS;
                int PaginaActual = 1;

                dtActasTitulares = objActoCivilConsultaBL.Consultar_Actas_Titulares(PaginaActual, intPaginaCantidad, ref IntTotalCount, ref IntTotalPages, intRegistroCivilId, strNumeroActaAnterior, strApPaterno, strApMaterno, strNombres);

                if (dtActasTitulares.Rows.Count > 0)
                {
                    if (dtActasTitulares.Rows.Count == 1)
                    {
                        txtTitularActa.Text = dtActasTitulares.Rows[0]["TITULAR"].ToString();
                    }
                    else
                    {
                        string strConyuges = "";
                        for (int i = 0; i < dtActasTitulares.Rows.Count; i++)
                        {
                            strConyuges = strConyuges + dtActasTitulares.Rows[0]["TITULAR"].ToString().Trim() + "/";
                        }
                        strConyuges = strConyuges.Substring(0, strConyuges.Length - 1);
                        if (strConyuges.Length > 200)
                        {
                            strConyuges = strConyuges.Substring(0, 200);
                        }
                        txtTitularActa.Text = strConyuges;
                    }

                    txtTitularActa.Enabled = false;
                    //-----------------------------------------
                }
                else
                {
                    txtTitularActa.Enabled = true;
                    
                    string StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Acto Civil", "No se encontraron datos", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                }
            }
            else
            {
                txtTitularActa.Text = "";
                txtTitularActa.Enabled = true;
                ddlTipoActaAnotacion.SelectedIndex = 0;
                ddlTipoActaAnotacion.Enabled = true;
            }
        }

        protected void btnBuscarTitularActa_Click(object sender, EventArgs e)
        {
            Session["ES_ANOTACION_O_TARIFA_1"] = "TARIFA_1";
            
            Grd_BusquedaActas.DataSource = null;
            Grd_BusquedaActas.DataBind();
            txtBuscarApPaterno.Focus();
            CtrlPageBarActas.Visible = false;
            CtrlPageBarActas.InicializarPaginador();
            updBusquedaTitular.Update();

            ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "limpiarCamposBusquedaActas();abrirPopupBusquedaActas();", true);

        }

        protected void chkReconocimientoAdopcion_CheckedChanged(object sender, EventArgs e)
        {

            if (chkReconocimientoAdopcion.Checked)
            {
                pnlActaAnterior.Visible = true;
                txtNumeroActaAnterior.Focus();
                updFormato.Update();
            }
            else
            {
                txtNumeroActaAnterior.Text = "";
                txtTitularActa.Text = "";                
                pnlActaAnterior.Visible = false;
                updFormato.Update();
            }
        }

        protected void chkconCUI_CheckedChanged(object sender, EventArgs e)
        {
            chksinCUI.Checked = !(chkconCUI.Checked);
            chksinCUI_CheckedChanged(sender, e);
            imgBuscarCUI.Visible = true;
            imgBuscarCUI.Enabled = true;
        }

        protected void chksinCUI_CheckedChanged(object sender, EventArgs e)
        {
            chkconCUI.Checked = !(chksinCUI.Checked);
            imgBuscarCUI.Visible = false;
            imgBuscarCUI.Enabled = false;
            if (chksinCUI.Checked)
            {                
                pnlActaAnterior.Visible = false;
                tablaReconocimientoAdopcion.Visible = true;
                chkReconocimientoAdopcion.Checked = false;
                ddl_TipoParticipante.SelectedIndex = 0;
                ddl_TipoDocParticipante.SelectedIndex = 0;
                ddl_TipoDocParticipante.Enabled = true;
                ddl_NacParticipante.Enabled = true;

                //int iIndiceComboCui = Util.ObtenerIndiceCombo(ddl_TipoDocParticipante, Convert.ToInt16(Enumerador.enmTipoDocumento.CUI).ToString());
                //if (iIndiceComboCui >= 0)
                //{
                //    ddl_TipoDocParticipante.Items[iIndiceComboCui].Enabled = false;
                //}
                txtNumeroActaAnterior.Text = "";
                txtNroCUI.Text = "";
                txtTitularActa.Text = "";
                txtNroCUI.Enabled = false;
                imgBuscarCUI.Enabled = false;
                txtApePatTitular.Enabled = false;
                txtApeMatTitular.Enabled = false;
                txtNombresTitular.Enabled = false;
                ddl_Genero.Enabled = false;
            }
            else
            {                
                tablaReconocimientoAdopcion.Visible = false;
                pnlActaAnterior.Visible = false;
                ddl_TipoParticipante.SelectedIndex = 0;
                txtNumeroActaAnterior.Text = "";
                txtTitularActa.Text = "";
                txtNroCUI.Enabled = true;
                txtApePatTitular.Enabled = true;
                txtApeMatTitular.Enabled = true;
                txtNombresTitular.Enabled = true;
                ddl_Genero.Enabled = true;
            }
            updFormato.Update();
        }
        protected void chkReconstitucionReposicion_CheckedChanged(object sender, EventArgs e)
        {            
            if (chkReconstitucionReposicion.Checked)
            {
                pnlActaAnterior.Visible = true;
                txtNumeroActaAnterior.Focus();
            }
            else
            {
                txtNumeroActaAnterior.Text = "";
                txtTitularActa.Text = "";
                pnlActaAnterior.Visible = false;
                chksinCUI.Checked = true;
            }
            updFormato.Update();
        }
        protected void ddlTipoActaAnotacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTitular.Text = "";
            txtTitular.Enabled = true;
            txtNumeroActa.Text = "";
            updAnotaciones.Update();
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

            string strTarifaLetra = txtIdTarifa.Text.Trim().ToUpper();

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

        private void MostrarDL173_DS076_2005RE()
        {
            //---------------------------------------------------------------------------
            //Fecha: 21/01/2019
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Habilitar la etiqueta: D.L. 173 del D.S. 0076-2005-RE
            //          cuando el tipo de pago sea: Gratuito por Ley 
            //          no tomar en cuenta la tarifa 2 ni la Sección III del Tarifario
            //---------------------------------------------------------------------------
            bool bisSeccionIII = Comun.isSeccionIII(Session, txtIdTarifa.Text);
            string strTarifa = txtIdTarifa.Text.Trim().ToUpper();


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

     
        private bool ExisteInafecto_Exoneracion(string strID)
        {
            bool bExiste = false;
            string strTexto = "";
            string strTipoPagoId = "";

            for (int i = 0; i < ddlTipoPago.Items.Count; i++)
            {
                strTexto = ddlTipoPago.Items[i].Text.Trim().ToUpper();

                if (strTexto.Contains("EXONERA") || strTexto.Contains("INAFECT"))
                {
                    strTipoPagoId = ddlTipoPago.Items[i].Value.Trim();

                    if (strID.Trim().Equals(strTipoPagoId))
                    {
                        bExiste = true;
                        break;
                    }
                }
            }
            return bExiste;
        }


        private void DeshabilitarBotonVinculacion()
        {
            //-------------------------------------------------------------------
            //Fecha: 12/04/2019
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Deshabilitar el botón grabar de la ficha de vinculación.
            //-------------------------------------------------------------------
            if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
            {
                btnGrabarVinculacion.Enabled = false;
                return;
            }
            if (HF_iPersonaID != null && HF_ACT_ID != null)
            {
                ActuacionConsultaBL objBL = new ActuacionConsultaBL();
                DataTable dtActuacionConsulta = new DataTable();


                long intPersonaId = Convert.ToInt64(HF_iPersonaID.Value);
                long intActuacionId = Convert.ToInt64(HF_ACT_ID.Value);

                dtActuacionConsulta = objBL.ActuacionDetalleObtener_Actuacion(intPersonaId, intActuacionId);
                if (dtActuacionConsulta.Rows.Count > 0)
                {
                    string strFechaRegistro = Comun.ObtenerFechaActualTexto(Session);
                    if (dtActuacionConsulta.Rows[0]["dFechaRegistro"].ToString().Trim() != string.Empty)
                    {
                        strFechaRegistro = dtActuacionConsulta.Rows[0]["dFechaRegistro"].ToString().Trim();

                    }
                    if (Comun.CalcularDiasHabilesModificacion(Session, Page, strFechaRegistro) == false)
                    {
                        btnGrabarVinculacion.Enabled = false;

                    }
                }
            }
            //----------------------------------------------------------           
        }

        private bool ValidarParticipantesRegistroCivil()
        {
            try
            {
                bool bValidado = true;

                Int16 ContarCodParti = 0;  //Contar la Cantidad de Participante obligatorios a registrar obtenido del WEBConfing
                String iCodTipoParticipante = String.Empty; //Codigo del Participante
                String ooParticipante = String.Empty; //Descripcion del Participante
                Int16 ContarAux = 0;
                String MensajeParticipantes = String.Empty; //Mensaje de Error Para los Participante Obligatorios

                if (Grd_Participantes.Rows.Count > 0)
                {

                    if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.NACIMIENTO)
                    {
                        #region Nacimiento

                        String ValNacimineto = ConfigurationManager.AppSettings["Nacimiento"].ToString();
                        String[] CodParti = ValNacimineto.Trim().Split(',');

                        ContarCodParti = Convert.ToInt16(CodParti.Length);
                        int countPadre = 0;
                        int countDeclarante = 0;


                        foreach (String onjcaract in CodParti)
                        {

                            foreach (GridViewRow row in Grd_Participantes.Rows)
                            {
                                iCodTipoParticipante = row.Cells[0].Text;
                                ooParticipante = row.Cells[5].Text;


                                if (onjcaract == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.DECLARANTE_1) || onjcaract == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.DECLARANTE_2))
                                {
                                    if (iCodTipoParticipante == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.DECLARANTE_1) || iCodTipoParticipante == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.DECLARANTE_2))
                                    {
                                        ContarAux++;
                                        break;
                                    }
                                }

                                if (onjcaract == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.MADRE) || onjcaract == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.PADRE))
                                {
                                    if (iCodTipoParticipante == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.MADRE) || iCodTipoParticipante == Convert.ToString((int)Enumerador.enmParticipanteNacimiento.PADRE))
                                    {
                                        ContarAux++;
                                        break;
                                    }
                                }

                                if (iCodTipoParticipante == onjcaract)
                                {

                                    ContarAux++;
                                    break;
                                }


                            }

                            if (ContarAux == 0)
                            {
                                string strDescripcion = string.Empty;
                                strDescripcion = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.REG_CIVIL_PARTICIPANTE_NACIMIENTO, Convert.ToInt32(onjcaract), "descripcion");


                                if (strDescripcion == Enumerador.enmParticipanteNacimiento.PADRE.ToString() || strDescripcion == Enumerador.enmParticipanteNacimiento.MADRE.ToString())
                                {
                                    countPadre++;
                                }
                                else if (strDescripcion == Constantes.CONST_VALIDACION_DECLARANTE1 || strDescripcion == Constantes.CONST_VALIDACION_DECLARANTE2)
                                {
                                    countDeclarante++;
                                }
                                else
                                {

                                    MensajeParticipantes += "," + strDescripcion;
                                }
                            }
                            else
                            {
                                ContarAux = 0;
                            }

                        }

                        if (countPadre == Constantes.CONST_VALIDACION_PADRES)
                        {
                            MensajeParticipantes += ", " + Enumerador.enmParticipanteNacimiento.PADRE.ToString() + " ó " + Enumerador.enmParticipanteNacimiento.MADRE.ToString();
                        }
                        if (countDeclarante == Constantes.CONST_VALIDACION_DECLARANTE)
                        {
                            MensajeParticipantes += ", " + Constantes.CONST_VALIDACION_DECLARANTE1 + " ó " + Constantes.CONST_VALIDACION_DECLARANTE2;
                        }
                        #endregion
                    }
                    else
                    {
                        string doc;
                        if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.MATRIMONIO)
                        {
                            #region Matrimonio


                            String ValMatrimonio = ConfigurationManager.AppSettings["Matrimonio"].ToString();
                            String[] CodParti = ValMatrimonio.Trim().Split(',');
                            ContarCodParti = Convert.ToInt16(CodParti.Length);

                            foreach (String onjcaract in CodParti)
                            {

                                foreach (GridViewRow row in Grd_Participantes.Rows)
                                {
                                    iCodTipoParticipante = row.Cells[0].Text;
                                    ooParticipante = row.Cells[5].Text;
                                    //doc = row.Cells[].Text;//sDocumentoTipoId                            
                                    if (iCodTipoParticipante == onjcaract)
                                    {
                                        ContarAux++;
                                        break;
                                    }

                                }

                                if (ContarAux == 0)
                                {
                                    string strDescripcion = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.REG_CIVIL_PARITICPANTE_MATRIMONIO, Convert.ToInt32(onjcaract), "descripcion");
                                    MensajeParticipantes += "," + strDescripcion;

                                }
                                else
                                {
                                    ContarAux = 0;
                                }
                            }
                            #endregion

                        }
                        else
                        {
                            if (Convert.ToInt32(ddlTipoActa.SelectedValue) == (int)Enumerador.enmTipoActa.DEFUNCION)
                            {
                                #region Defuncion

                                string ValDefuncion = ConfigurationManager.AppSettings["Defuncion"].ToString();

                                if (chkInscripcionOficio.Checked == true)
                                {
                                    int intDeclarante = (Int32)Enumerador.enmParticipanteDefuncion.DECLARANTE;

                                    ValDefuncion = ValDefuncion.Replace("," + intDeclarante.ToString(), "");
                                }


                                String[] CodParti = ValDefuncion.Trim().Split(',');
                                ContarCodParti = Convert.ToInt16(CodParti.Length);

                                foreach (String onjcaract in CodParti)
                                {
                                    foreach (GridViewRow row in Grd_Participantes.Rows)
                                    {
                                        iCodTipoParticipante = row.Cells[0].Text;
                                        ooParticipante = row.Cells[5].Text;

                                        if (iCodTipoParticipante == onjcaract)
                                        {
                                            ContarAux++;
                                            break;
                                        }
                                    }

                                    if (ContarAux == 0)
                                    {
                                        string strDescripcion = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.REG_CIVIL_PARTICIPANTE_DEFUNCION, Convert.ToInt32(onjcaract), "descripcion");
                                        MensajeParticipantes += "," + strDescripcion;
                                    }
                                    else
                                    {
                                        ContarAux = 0;
                                    }
                                }



                                #endregion
                            }
                        }
                    }
                }
                else
                {
                    if (Grd_Participantes.Rows.Count == 0)
                    {
                        return false;
                    }
                }

                if (MensajeParticipantes.Trim().Length > 0)
                {
                    return false;
                }


                return bValidado;
            }
            catch (Exception ex)
            {
                throw ex;
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

        protected void BtnActaConformidad_Click(object sender, EventArgs e)
        {
            ImprimirActaConformidad(Convert.ToInt32(ddlTipoActa.SelectedValue));

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "window", "window.open('../Accesorios/VistaPreviaPDF.aspx', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height')", true);
        }

        protected void btnAgregarParticipante_Click(object sender, EventArgs e)
        {
            ViewState["Editar"] = false;
            btnCancelar_Click(null, null);
            ddl_TipoParticipante.Enabled = true;
            string script = "abrirPopupParticipantes();";

            ScriptManager.RegisterStartupScript(this, typeof(Page), "popup", script, true);
        }
        private void RegresarFormatoDatosParticipante()
        {
            ddl_TipoDocParticipante.Style.Add("border", "solid #888888 1px");
            txtNroDocParticipante.Style.Add("border", "solid #888888 1px");
            txtNomParticipante.Style.Add("border", "solid #888888 1px");
            txtApePatParticipante.Style.Add("border", "solid #888888 1px");
            ddlGenero_Titular.Style.Add("border", "solid #888888 1px");
            ddl_TipoDocParticipante.Style.Add("border", "solid #888888 1px");
            ddl_TipoVinculoParticipante.Style.Add("border", "solid #888888 1px");
            ddl_NacParticipante.Style.Add("border", "solid #888888 1px");
            CmbEstCiv.Style.Add("border", "solid #888888 1px");

        }
        protected void txtNroDocParticipante_TextChanged(object sender, EventArgs e)
        {
            imgBuscar_Click(null, null);
        }

        protected void imgBuscarCUI_Click(object sender, ImageClickEventArgs e)
        {
            #region Buscar persona
            EnPersona objEn = new EnPersona();
            objEn.iPersonaId = 0;
            objEn.sDocumentoTipoId = (Int16)Enumerador.enmTipoDocumento.CUI;
            objEn.vDocumentoNumero = txtNroCUI.Text.ToUpper();
            if (txtNroCUI.Text.Length == 0)
            {
                string StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Acto Civil", "Ingrese el número de CUI", false, 190, 250);
                Comun.EjecutarScript(Page, StrScript);
                return;
            }
            object[] arrParametros = { objEn };
            objEn = SGAC.WebApp.Accesorios.Persona.oPersona(arrParametros);
            #endregion
            txtPersonaId.Text = "0";
            if (objEn != null)
            {
                if (objEn.iPersonaId == 0)
                {
                    string StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Acto Civil", "No se encontró información", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }
                txtPersonaId.Text = objEn.iPersonaId.ToString();
                txtNombresTitular.Text = objEn.vNombres;
                txtApePatTitular.Text = objEn.vApellidoPaterno;
                txtApeMatTitular.Text = objEn.vApellidoMaterno;
                ddl_Genero.SelectedValue = objEn.sGeneroId.ToString();
                if (objEn.dFecNacimiento != null && objEn.dFecNacimiento != "0")
                {
                    this.txtFecNac.Text = Comun.FormatearFecha(objEn.dFecNacimiento).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                    this.txtHora.Text = Comun.FormatearFecha(objEn.dFecNacimiento.ToString()).ToString("HH:mm");
                }
                txtNroCUI.Enabled = false;
                imgBuscarCUI.Enabled = false;
            }
            else {
                txtNroCUI.Enabled = true;
                imgBuscarCUI.Enabled = true;
                string StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Acto Civil", "No se encontró información", false, 190, 250);
                Comun.EjecutarScript(Page, StrScript);
                return;
            }
        }

        protected void chkInscripcionOficio_CheckedChanged(object sender, EventArgs e)
        {
            if (chkInscripcionOficio.Checked)
            {
                txtCivilObservaciones.Text = "Inscripción de Oficio.";
            }
            else {
                txtCivilObservaciones.Text = "";
            }
        }
}
}
