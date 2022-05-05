using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;
using System.Configuration;
using System.Net;
using System.Threading;
using System.Web.Script.Serialization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Web.Configuration;
using SGAC.Accesorios;
using SGAC.BE;
using SGAC.Configuracion.Sistema.BL;
using SGAC.Controlador;
using SGAC.WebApp.Accesorios;

using SGAC.Registro.Actuacion.BL;
using SGAC.Registro.Persona.BL;
using Microsoft.Security.Application;
using SGAC.Almacen.BL;
using SGAC.Configuracion.Maestro.BL;

namespace SGAC.WebApp.Registro
{
    public partial class FrmActoGeneral : MyBasePage
    {
        #region Campos
        private string strVariableAccion = "Actuacion_Accion";
        private string strVariableTarifario = "objTarifarioBE";
        private BE.MRE.SI_TARIFARIO objTarifarioBE;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlToolBarRegistro.VisibleIButtonGrabar = true;
            ctrlToolBarRegistro.VisibleIButtonCancelar = true;
            ctrlToolBarRegistro.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolBarRegistro_btnGrabarHandler);
            ctrlToolBarRegistro.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarRegistro_btnCancelarHandler);

            //------------------------------------------------------
            // Autor: Miguel Márquez Beltrán
            // Fecha: 07/11/2016
            // Objetivo: Registro de la ficha Registral
            //------------------------------------------------------
            ctrlToolBarFicha.VisibleIButtonNuevo = true;
            ctrlToolBarFicha.VisibleIButtonGrabar = true;
            //ctrlToolBarFicha.VisibleIButtonCancelar = true;
            ctrlToolBarFicha.VisibleIButtonPrint = true;
            ctrlToolBarFicha.VisibleIButtonEliminar = true;

            //------------------------------------------------------

            EstadoNormalBarraFichaRegistral(true);

            ctrlToolBarFicha.btnNuevoHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonNuevoClick(ctrlToolBarFicha_btnNuevoHandler);
            ctrlToolBarFicha.btnEliminarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEliminarClick(ctrlToolBarFicha_btnEliminarHandler);
            ctrlToolBarFicha.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolBarFicha_btnGrabarHandler);
            //ctrlToolBarFicha.btnPrintHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonPrintClick(ctrlToolBarFicha_btnPrintHandler);
            
            //------------------------------------------------------

            ctrlToolBarFicha.btnImprimir.OnClientClick = "return abrirPopupImpresionPrevia()";
            btnGrabarAntecedente.Visible = true;
            btnAnularAntecedente.Visible = true;

            //------------------------------------------------------
            // Jonatan -- 20/07/2017 -- Botones controles de usuario, reimpresión y anulación de autoadhesivos
            ctrlReimprimirbtn1.btnReimprimirHandler += new Accesorios.SharedControls.ctrlReimprimirbtn.OnButtonReimprimirClick(ctrlReimprimirbtn_btnReimprimirHandler);
            ctrlBajaAutoadhesivo1.btnAnularHandler += new Accesorios.SharedControls.ctrlBajaAutoadhesivo.OnButtonAnularClick(ctrlBajaAutoadhesivo_btnAnularAutoahesivo);
            ctrlBajaAutoadhesivo1.btnAceptarAnularHandler += new Accesorios.SharedControls.ctrlBajaAutoadhesivo.OnButtonAceptarAnulacionClick(ctrlBajaAutoadhesivo_btnAceptarAnularAutoahesivo);
            //------------------------------------------------------
            ctrFechaEnvio.AllowFutureDate = true;
            btnGrabarVinculacion.OnClientClick = "return ValidarAutoadhesivo()";
            ctrlToolBarRegistro.btnGrabar.OnClientClick = "return ValidarRegistroActuacion()";

            ctrlAdjunto.isGeneral = true;

            txtFechaVencimientoMSIAP.AllowFutureDate = true;
            //----------------------------------------------------------

            var s_IsJudicial = Request.QueryString["vClass"];

            if (s_IsJudicial != null)
            {
                ctrlAdjunto.IsJudicial = true;
            }
            if (Page.Request.Params["__EVENTTARGET"] == "ACTUALIZA")
            {
                if (tablaFileUpLoadFoto.Visible == true)
                {
                    if (FileUploadFoto.HasFile == true)
                    {
                        Session["UploadFotoAntecedentePenal"] = FileUploadFoto;
                        int s_Maximo = Constantes.CONST_TAMANIO_MAX_ADJUNTO_FOTO_KB;
                        lblMensajeFileUploadFoto.Text = "elija imagenes (*.jpg) hasta " + s_Maximo.ToString() + "kb";
                        lblMensajeErrorUpLoadFoto.Text = "";
                        CargarArchivo();
                        Comun.EjecutarScript(Page, Util.HabilitarTab(5) + Util.ActivarTab(6, "Vinculación"));
                        BindGridActuacionesInsumoDetalle(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]));
                        if (btnVistaPrev.Enabled)
                        {
                                ctrlReimprimirbtn1.Activar = false;
                        }
                        else
                        {
                            ctrlReimprimirbtn1.Activar = chkImpresion.Checked;
                        }
                    }
                }
            }

            //if (!Page.IsPostBack || Page.Request.Params["__EVENTTARGET"] == "ACTUALIZA")
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
                else {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Formulario", "alert('Por favor evitar abrir 2 pestañas, o copiar el formulario');", true);
                    Response.Redirect("../Default.aspx", false);
                    return;
                }
                


                if (Session["strBusqueda"] != null)
                {
                    Session.Remove("strBusqueda");
                }
                //----------------------------------------//
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 19/07/2018
                //----------------------------------------//
                btnGrabarAntecedente.Enabled = true;

                //----------------------------------------//
                CargarDropDownList();
                CargarDatosIniciales();
                //----------------------------------------//
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 15-08-2016
                // Objetivo: Ocultar los controles de clasificación
                // Referencia: Requerimiento No.001_2.doc
                //----------------------------------------//
                lblClasificacion.Visible = false;
                ddlClasificacion.Visible = false;
                ddlClasificacion.SelectedIndex = 0;
                //----------------------------------------//          
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 15-08-2016
                // Objetivo: Ocultar los controles de clasificación
                //----------------------------------------//
                lblExoneracion.Visible = false;
                ddlExoneracion.Visible = false;

                //----------------------------------------//
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 25-02-2019
                // Objetivo: Ocultar controles de Sustento
                //----------------------------------------//
                lblSustentoTipoPago.Visible = false;
                txtSustentoTipoPago.Visible = false;
                lblValSustentoTipoPago.Visible = false;
                RBNormativa.Visible = false;
                RBSustentoTipoPago.Visible = false;
                lblValExoneracion.Visible = false;

                //----------------------------------------------------------//
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 09-11-2016
                // Objetivo: Inicializar las sesiones de la Ficha Registral
                //----------------------------------------------------------//
                Session["FichaRegistral"] = null;
                //----------------------------------------------------------//
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 09-12-2016
                // Objetivo: Inicializar la sesión de participante de 
                //           la ficha registral
                //----------------------------------------------------------//
                Session["FichaRegistralParticipante"] = null;
                //----------------------------------------------------------//
                PintarDatosPestaniaGeneral();
                Session["iOperAnot"] = true;


                DeshabilitarBotonVinculacion();
                if (!EsMayorEdad())
                {
                    chkModEstCivil.Visible = false;
                    chkModInterdiccion.Visible = false;
                }

                if (Convert.ToString(Session["strActo"]).Equals("Judicial"))
                {
                    btn_acta_diligenciamiento.Visible = true;
                }

                //----------------------------------------------------------//
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 12-12-2016
                // Objetivo: Asignar parametros iniciales del Doc. Identidad
                //----------------------------------------------------------//
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
                //----------------------------------------//
                // Autor: Jonatan Silva Cachay
                // Fecha: 14-02-2017
                // Objetivo: Verificar Enabled
                //----------------------------------------//

                ctrlToolBarFicha.btnImprimir.Enabled = false;
                ctrlToolBarFicha.btnGrabar.Enabled = true;
                //----------------------------------------------------------//
                // Fin de PostBack
                //----------------------------------------------------------//
                CargarDatosEditar();
                if (Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value] == null) { return; }
                Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value].ToString();
                // Jonatan -- 20/07/2017
                if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
                {
                    ctrlReimprimirbtn1.Activar = true;
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

                VerificarTarficaReniecConFIcha();
                BloquearParaTarifasGratuitas();
                //------------------------------------------------------------------------------
                //Fecha: 08/03/2019
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Habilitar Nro. Operación, Nombre del banco y la fecha de pago.
                //------------------------------------------------------------------------------
                if ((Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.ACTUALIZACION))
                {
                    if (Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA) ||
                                        Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA) ||
                                        Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA))
                    {
                        txtNroOperacion.Enabled = true;
                        ddlNomBanco.Enabled = true;
                        ctrFecPago.Enabled = true;
                    }

                    if (Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
                    {
                        ctrlToolBarRegistro.btnGrabar.Enabled = true;
                    }
                }

                if (txtIdTarifa.Text.Contains("58B"))
                {
                    //------------------------------
                    //Fecha: 26/10/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: LLenar lista de idiomas.
                    //------------------------------
                    string strIdioma = Session[Constantes.CONST_SESION_IDIOMA_TEXTO].ToString();
                    string strIdiomaId = Session[Constantes.CONST_SESION_IDIOMA_ID].ToString();
                    ddlIdiomas.Items.Clear();
                    ddlIdiomas.Items.Insert(0, new ListItem("CASTELLANO", "0"));
                    if (strIdioma.ToUpper() != "CASTELLANO")
                    {
                        ddlIdiomas.Items.Insert(1, new ListItem(strIdioma, strIdiomaId));
                    }
                    //------------------------------

                    btnConstanciaInscripcion.Visible = true;
                }
                else
                {
                    btnConstanciaInscripcion.Visible = false;
                }
                //--------------------------------------------------------------------
            }
            else
            {
                
            }
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlAdjunto.BtnGrabActAdj, btnAgregarParticipante, btnGrabarAntecedente, btnGrabarVinculacion, btnAnularAntecedente, btnRegistrarAntecedentesPenales,btnNoAntecedentes,
                                      ctrlToolBarRegistro.btnEditar, ctrlToolBarRegistro.btnEliminar, ctrlToolBarRegistro.btnGrabar, ctrlToolBarRegistro.btnNuevo, 
                                      ctrlToolBarFicha.btnEditar, ctrlToolBarFicha.btnEliminar, ctrlToolBarFicha.btnGrabar, ctrlToolBarFicha.btnNuevo};
                Comun.ModoLectura(ref arrButtons);
                GridView[] arrGridView = { GridViewParticipante, grvDocAdjuntosReniec, gdvAntecedentesPenales };
                Comun.ModoLectura(ref arrGridView);
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
            Session["IngresoReimprimir"] = 0;
            if (ctrlReimprimirbtn1.SeImprime == "OK")
            {
                PintarDatosPestaniaGeneral();
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

            //if (Request.QueryString["GUID"] != null)
            //{
            //    HFGUID.Value = Sanitizer.GetSafeHtmlFragment(Request.QueryString["GUID"].ToString());
            //    Response.Redirect("FrmActoGeneral.aspx?GUID=" + HFGUID.Value + "&cod=1");
            //}
            //else
            //{
            string codPersona = Request.QueryString["CodPer"].ToString();
            if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
            {
                Response.Redirect("FrmActoGeneral.aspx?cod=1&CodPer=" + codPersona + "&Juridica=1", false);
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
                    Response.Redirect("FrmActoGeneral.aspx?cod=1&CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                }
                else
                {
                    Response.Redirect("FrmActoGeneral.aspx?cod=1&CodPer=" + codPersona, false);
                }
            }
            
            //}
        }

        // jonatan -- dar de baja un autoadhesivo
        void ctrlBajaAutoadhesivo_btnAnularAutoahesivo()
        {            
            //----------------------------------------------------------------
            //Fecha: 10/08/2018
            //Autor: Miguel Márquez Beltrán
            //Motivo: Validación para la Baja del AutoAdhesivo
            //Condición: 
            //      1.- Cuando se da la Baja al autoadhesivo consular, debe de validarse si esta Asignado al usuario.
            //      2.- También podrá realizar la Baja el usuario con el perfil de Administrador del consulado.
            //      3.- En caso contrario no debe de permitir dar de Baja, debe de visualizarse el mensaje.
            //----------------------------------------------------------------
            long lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);                       
            
            ActuacionMantenimientoBL objActuacionMantenimientoBL = new ActuacionMantenimientoBL();
            DataTable dtActuacionMantenimiento = new DataTable();
            int ITotalRecords = 0;
            int ITotalPages = 0;
            string strUsuarioActuacion = "";
            int intOficinaConsularId = 0;
            string strCodigoUnicoFabrica = "";

            dtActuacionMantenimiento = objActuacionMantenimientoBL.Obtener_ActuacionInsumoDetalleAll(lngActuacionDetalleId, 1, 1, ref ITotalRecords, ref ITotalPages);
            if (dtActuacionMantenimiento.Rows.Count > 0)
            {
                strUsuarioActuacion = dtActuacionMantenimiento.Rows[0]["usuario"].ToString();
                strCodigoUnicoFabrica = dtActuacionMantenimiento.Rows[0]["insu_vCodigoUnicoFabrica"].ToString();

                ActuacionConsultaBL objActuacionConsultaBL = new ActuacionConsultaBL();
                DataTable dtActuacionConsulta = new DataTable();
                dtActuacionConsulta = objActuacionConsultaBL.ActuacionesObtenerPorAutoadhesivo(strCodigoUnicoFabrica, 1, 1, ref ITotalRecords, ref ITotalPages);
                
                if (dtActuacionConsulta.Rows.Count > 0)
                {
                    intOficinaConsularId = Convert.ToInt32(dtActuacionConsulta.Rows[0]["sOficinaConsularId"].ToString());
                }
            }
            
            string strUsuario = Session[Constantes.CONST_SESION_USUARIO].ToString();
            string strUsuarioRol = Session[Constantes.CONST_SESION_USUARIO_ROL].ToString();

            if (intOficinaConsularId == (int)Session[Constantes.CONST_SESION_OFICINACONSULAR_ID])
            {
                // SOLO EL ADMINISTRADOR O EL QUE VINCULO EL AUTOADHESIVO PUEDE DARLE DE BAJA
                if (strUsuarioRol.Equals("SGAC_ADMINISTRADOR") || strUsuarioRol.Equals("SGAC_ADMINISTRADOR_CONSULADO_SCI") || strUsuario.Equals(strUsuarioActuacion))
                {
                    ctrlBajaAutoadhesivo1.CodInsumo = hCodAutoadhesivo.Value;

                    Comun.EjecutarScript(this, "Popup(" + hCodAutoadhesivo.Value.ToString() + ");");
                }
                else
                {
                    string StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Validación - Baja del AutoAdhesivo", "No se permite dar de Baja al AutoAdhesivo Vinculado.", false, 190, 320);
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }
            }
            else
            {
                string StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Validación - Baja del AutoAdhesivo", "No se permite dar de Baja al AutoAdhesivo Vinculado.", false, 190, 320);
                Comun.EjecutarScript(Page, StrScript);
                return;
            }
                                   
        }

        // jonatan -- dar de baja un autoadhesivo
        void ctrlBajaAutoadhesivo_btnAceptarAnularAutoahesivo()
        {
            ctrlBajaAutoadhesivo1.CodInsumo = hCodAutoadhesivo.Value;
           
            string codPersona = Request.QueryString["CodPer"].ToString();
            if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
            {
                Response.Redirect("FrmActoGeneral.aspx?cod=0&CodPer=" + codPersona + "&Juridica=1", false);
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
                    Response.Redirect("FrmActoGeneral.aspx?cod=0&CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                }
                else
                {
                    Response.Redirect("FrmActoGeneral.aspx?cod=0&CodPer=" + codPersona, false);
                }
            }
           
            //}
            txtCodAutoadhesivo.Focus();
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

        void ctrlToolBarRegistro_btnGrabarHandler()
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


           
            
            bool bNoCobrado = ExisteInafecto_Exoneracion(this.ddlTipoPago.SelectedValue);

            if (bNoCobrado || Convert.ToInt32(this.ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
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
            

            ActualizarActuacionDetalle();
            CheckNormativa();
            DeshabilitarBotonVinculacion();
        }

        private void CheckNormativa()
        {
            if (RBNormativa.Visible == true)
            {
                if (RBNormativa.Checked)
                {
                    txtSustentoTipoPago.Enabled = false;
                    ddlExoneracion.Enabled = true;
                }
                else
                {
                    txtSustentoTipoPago.Enabled = true;
                    ddlExoneracion.Enabled = false;
                }
            }
        }

        private void CargarDropDownList()
        {
            try
            {
                //-----------------------------------------------------------------------------------
                // Fecha: 25/07/2018
                // Autor: Miguel Márquez Beltrán
                // Objetivo: Limitar el ingreso de caracteres en la caja de texto.
                //-----------------------------------------------------------------------------------

                txtObservaciones.Attributes.Add("onkeypress", " ValidarCaracteres(this, 500);");
                txtObservaciones.Attributes.Add("onkeyup", " ValidarCaracteres(this, 500);");

                DataTable dtTipoPago = new DataTable();
                dtTipoPago = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACREDITACION_TIPO_COBRO);

                //DataView dv = dtTipoPago.DefaultView;
                //DataTable dtTipoPagoOrdenadoOrdenado = dv.ToTable();
                //dtTipoPagoOrdenadoOrdenado.DefaultView.Sort = "torden ASC";

                Util.CargarParametroDropDownList(ddlTipoPago, dtTipoPago, true);

                Util.CargarParametroDropDownList(ddlNomBanco, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.BANCO), true);
                Util.CargarParametroDropDownList(ddlClasificacion, comun_Part1.ObtenerParametrosPorGrupo(Session, SGAC.Accesorios.Constantes.CONST_ACTUACION_CLASIFICACION_TARIFA), true);


                Util.CargarParametroDropDownList(CmbEstCiv, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.ESTADO_CIVIL), true);
                Util.CargarParametroDropDownList(CmbGenero, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_GENERO), true);
                Util.CargarParametroDropDownList(CmbGradInst, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_GRADO_INSTRUCCION), true);

                Util.CargarParametroDropDownList(ddlDeclarante, comun_Part1.ObtenerParametrosPorGrupo(Session, SGAC.Accesorios.Constantes.CONST_TIPO_DECLARANTE_RENIEC), true);
                Util.CargarParametroDropDownList(ddlTutorG, comun_Part1.ObtenerParametrosPorGrupo(Session, SGAC.Accesorios.Constantes.CONST_TIPO_TUTOR_GUARDADOR_RENIEC), true);


                Util.CargarParametroDropDownList(ddlTramiteMSIAP, comun_Part1.ObtenerParametrosPorGrupo(Session, "ANTECEDENTES-PENALES"), true);


                if (Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Convert.ToInt16(Constantes.CONST_ID_CONSULADO_CARACAS))
                {
                    ddlTipoPago.Items.FindByText("PAGO ARUBA").Enabled = false;
                    ddlTipoPago.Items.FindByText("PAGO OTRAS ISLAS CARIBEÑAS").Enabled = false;
                }

                BE.RE_TARIFA_PAGO objTarifaPago = new RE_TARIFA_PAGO();
                objTarifaPago = (BE.RE_TARIFA_PAGO)Session[Constantes.CONST_SESION_OBJ_TARIFA_PAGO];

                //----------------------------------------------
                //Fecha: 08/02/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Validar el objeto objTarifaPago
                //----------------------------------------------
                if (objTarifaPago != null)
                {
                    if (objTarifaPago.tari_sSeccionId == 8)
                    {
                        CargarDropDownListFichaRegistral();
                    }
                }
                //-----------------------------------------------------------------------
                //Autor: Miguel Márquez Beltrán
                //Fecha: 19/07/2018
                //Objetivo: Cargar valores iniciales para Antecedentes Penales
                //          y la tarifa: 20B y la clasificación: 
                //          Certificación de Antecedentes Penales
                //-----------------------------------------------------------------------
                if (objTarifaPago != null)
                {

                }
                //----------------------------------------------
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        private void CargarDropDownListFichaRegistral()
        {
            //--------------------------------------------------------//
            // Autor: Miguel Angel Márquez Beltrán
            // Fecha: 07-12-2016
            // Objetivo: Listas desplegables de la ficha registral
            //--------------------------------------------------------//
            txtObservacionFicha.Attributes.Add("onkeypress", " ValidarCaracteres(this, 500);");
            txtObservacionFicha.Attributes.Add("onkeyup", " ValidarCaracteres(this, 500);");

            comun_Part3.CargarUbigeo(Session, ddl_DeptDomicilio, Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT, string.Empty, string.Empty, true);
            comun_Part3.CargarUbigeo(Session, ddl_ProvDomicilio, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, string.Empty, string.Empty, true);
            comun_Part3.CargarUbigeo(Session, ddl_DistDomicilio, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, string.Empty, string.Empty, true);
            Util.CargarParametroDropDownList(ddl_LocalidadDomicilio, new DataTable(), true);

            comun_Part3.CargarUbigeo(Session, ddl_DeptNacimiento, Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT, string.Empty, string.Empty, true);
            comun_Part3.CargarUbigeo(Session, ddl_ProvNacimiento, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, string.Empty, string.Empty, true);
            comun_Part3.CargarUbigeo(Session, ddl_DistNacimiento, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, string.Empty, string.Empty, true);
            Util.CargarParametroDropDownList(ddl_LocalidadNacimiento, new DataTable(), true);

            EstablecerSecuenciaEstadoFicha(ddlEstadoFicha, "REGISTRADO");
            //-----------------------------------------------------------------
            //Objetivo: Llenar la lista de tipo de documento del participante
            //-----------------------------------------------------------------
            DataTable dtTipDoc = new DataTable();
            dtTipDoc = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.DOCUMENTO_IDENTIDAD);
            ddl_TipoDocParticipante.Items.Clear();
            DataView dv = dtTipDoc.DefaultView;
            //dv.RowFilter = " Valor in ('DNI','LM/BOL','CE','OTROS')";
            DataTable dtOrdenado = dv.ToTable();
            dtOrdenado.DefaultView.Sort = "Id ASC";
            Util.CargarDropDownList(ddl_TipoDocParticipante, dtOrdenado, "Valor", "Id", true);
            ListItem lListItem = new ListItem(Convert.ToString(Enumerador.enmTipoDocumento.CUI), Convert.ToString((int)Enumerador.enmTipoDocumento.CUI));
            ddl_TipoDocParticipante.Items.Add(lListItem);
            //-----------------------------------------------------------------
            CargarTipoParticipante();

            DataTable dtCodigoLocal = new DataTable();
            OficinaConsularConsultasBL objOficinaConsularBL = new OficinaConsularConsultasBL();

            dtCodigoLocal = objOficinaConsularBL.ListaCodigoLocal("", "");
            Util.CargarDropDownList(ddlLocalDestino, dtCodigoLocal, "OFCO_VNOMBREMOSTRAR", "OFCO_VCODIGOLOCAL", true);
            if (ddlLocalDestino.Items.Count > 0)
            {
                ddlLocalDestino.SelectedValue = Session[Constantes.CONST_SESION_OFICINACONSULAR_CODIGOLOCAL].ToString();
            }

            //btnAgregarFicha.Visible = false;
            //btnAceptarFicha.Visible = false;

            //ctrlToolBarFicha.btnGrabar.Enabled = false;
            ctrlToolBarFicha.btnImprimir.Enabled = false;
            //pnlParticipantes.Style.Add("display", "none");
            //--------------------------------------------------------//
        }

        private void CargarDatosIniciales()
        {
            try
            {
                // Datos Personales
                lblDestino.Text = string.Empty;
                string strEtiquetaSolicitante = string.Empty;

                //if (HFGUID.Value.Length > 0)
                //{
                //    if (Session["ApePat" + HFGUID.Value] != null)
                //        strEtiquetaSolicitante += Session["ApePat" + HFGUID.Value].ToString() + " ";
                //    if (Session["ApeMat" + HFGUID.Value] != null)
                //        strEtiquetaSolicitante += Session["ApeMat" + HFGUID.Value].ToString() + " ";
                //    if (Session["ApeCasada" + HFGUID.Value] != null)
                //    {
                //        if (Session["ApeCasada" + HFGUID.Value].ToString() != "&nbsp;")
                //        {
                //            strEtiquetaSolicitante += Session["ApeCasada" + HFGUID.Value].ToString() + " ";
                //        }
                //    }
                //    if (Session["Nombres" + HFGUID.Value] != null)
                //    {
                //        if (Session["Nombres" + HFGUID.Value].ToString().Trim() != string.Empty)
                //            strEtiquetaSolicitante += ", " + Session["Nombres" + HFGUID.Value].ToString() + " ";
                //    }

                //    if (Session["DescTipDoc" + HFGUID.Value] != null)
                //        strEtiquetaSolicitante += "- " + Session["DescTipDoc" + HFGUID.Value].ToString() + ": ";
                //    if (Session["NroDoc" + HFGUID.Value] != null)
                //        strEtiquetaSolicitante += Session["NroDoc" + HFGUID.Value].ToString();
                //}
                //else
                //{
                    if (ViewState["ApePat"] != null)
                        strEtiquetaSolicitante += ViewState["ApePat"].ToString() + " ";
                    if (ViewState["ApeMat"] != null)
                        strEtiquetaSolicitante += ViewState["ApeMat"].ToString() + " ";
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
                            strEtiquetaSolicitante += ", " + ViewState["Nombre"].ToString() + " ";
                    }

                    if (ViewState["DescTipDoc"] != null)
                        strEtiquetaSolicitante += "- " + ViewState["DescTipDoc"].ToString() + ": ";
                    if (ViewState["NroDoc"] != null)
                        strEtiquetaSolicitante += ViewState["NroDoc"].ToString();
                //}

                if (strEtiquetaSolicitante == "")
                {
                    if (ViewState["iPersonaId"] != null)
                    {
                        EnPersona objPersona = new EnPersona();
                        objPersona.iPersonaId = Convert.ToInt64(ViewState["iPersonaId"].ToString());
                        object[] arrParametros = { objPersona };

                        objPersona = SGAC.WebApp.Accesorios.Persona.oPersona(arrParametros);
                        if (objPersona != null)
                        {
                            ViewState["DescTipDoc"] = objPersona.vDocumentoTipo;
                            ViewState["NroDoc"] = objPersona.vDocumentoNumero;
                            ViewState["ApePat"] = objPersona.vApellidoPaterno;
                            ViewState["ApeMat"] = objPersona.vApellidoMaterno;
                            ViewState["ApeCasada"] = objPersona.vApellidoCasada;
                            ViewState["Nombre"] = objPersona.vNombres;
                        }

                        if (ViewState["ApePat"] != null)
                            strEtiquetaSolicitante += ViewState["ApePat"].ToString() + " ";
                        if (ViewState["ApeMat"] != null)
                            strEtiquetaSolicitante += ViewState["ApeMat"].ToString() + " ";
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
                                strEtiquetaSolicitante += ", " + ViewState["Nombre"].ToString() + " ";
                        }

                        if (ViewState["DescTipDoc"] != null)
                            strEtiquetaSolicitante += "- " + ViewState["DescTipDoc"].ToString() + ": ";
                        if (ViewState["NroDoc"] != null)
                            strEtiquetaSolicitante += ViewState["NroDoc"].ToString();
                    }
                }

                lblDestino.Text = strEtiquetaSolicitante;

                if (rbSemi.Checked)
                {
                    DivSemiAutomatico.Visible = true;
                }
                else { DivSemiAutomatico.Visible = false; }

                //ctrFechaEnvio.AllowFutureDate = true;
                ctrlToolBarFicha.btnNuevo.Enabled = false;
                ctrlToolBarFicha.btnNuevo.Enabled = false;
                hNuevo.Value = "0";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        private void PintarDatosPestaniaGeneral()
        {
            try
            {
                //if (Convert.ToString(Session["strActo"]).Equals("Judicial"))
                //{
                CargarDatosTarifaPago();
                //}

                PintarDatosPestaniaRegistro();

                long lngActuacionDetalleId = 0;
                if (Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value] != null)
                {
                    lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);
                    if (lngActuacionDetalleId == 0)
                    {
                        lngActuacionDetalleId = Convert.ToInt64(Session["ACTUACIONDETALLE"].ToString());
                    }
                }

                BindGridActuacionesInsumoDetalle(lngActuacionDetalleId);
                CargarGrillaAdjuntos(lngActuacionDetalleId);

                if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.REGISTRO)
                {

                }
                else
                {
                    if ((Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.ACTUALIZACION))
                    {
                        ctrlToolBarRegistro.btnGrabar.Enabled = true;
                        ctrlAdjunto.BtnGrabActAdj.Visible = true;
                        txtObservaciones.Enabled = true;

                    }
                    else if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
                    {
                        BE.RE_TARIFA_PAGO objTarifaPago = new RE_TARIFA_PAGO();
                        objTarifaPago = (BE.RE_TARIFA_PAGO)Session[Constantes.CONST_SESION_OBJ_TARIFA_PAGO];

                        if (objTarifaPago.vTarifa == "20B")
                        {
                            ctrlToolBarRegistro.btnGrabar.Enabled = true;
                        }
                        else
                        {
                            ctrlToolBarRegistro.btnGrabar.Enabled = false;
                        }

                        txtObservaciones.Enabled = false;
                        ctrlAdjunto.BtnGrabActAdj.Enabled = false;
                        btnGrabarVinculacion.Enabled = false;
                        ddlTipoPago.Enabled = false;
                    }

                    if (Convert.ToString(Session["strActo"]).Equals("Judicial"))
                    {
                        if (txtCodAutoadhesivo.Text.Trim() != string.Empty)
                        {
                            btnGrabarVinculacion.Enabled = false;
                            ctrlToolBarRegistro.btnGrabar.Enabled = false;
                        }
                        else
                            btnGrabarVinculacion.Enabled = true;
                    }
                }     
            }
            catch (Exception ex)
            {
                throw ex;
            }
                   
        }

        private void ActualizarActuacionDetalle()
        {
            string StrScript = string.Empty;
            int IntRpta = 0;
            long lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);
            if (lngActuacionDetalleId > 0)
            {

                BE.MRE.SI_TARIFARIO objTarifarioBE = new BE.MRE.SI_TARIFARIO();
                string strTarifaId = txtIdTarifa.Text.Trim().ToUpper();
                objTarifarioBE = Comun.ObtenerTarifario(Session, strTarifaId);

                BE.RE_ACTUACION ObjActuacBE = new RE_ACTUACION();
                BE.RE_ACTUACIONDETALLE ObjActuacDetBE = new RE_ACTUACIONDETALLE();

                ObjActuacDetBE.acde_iActuacionDetalleId = lngActuacionDetalleId;
                ObjActuacDetBE.acde_vNotas = txtObservaciones.Text.ToUpper().Replace("'", "''");
                ObjActuacBE.actu_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                ObjActuacBE.actu_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                ObjActuacBE.actu_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                
                object[] miArray = { ObjActuacBE, ObjActuacDetBE };

                Int16 intTipoPago = 0;
                if (ctrlToolBarRegistro.btnGrabar.Enabled || hModificaTemporal.Value == "1")
                {
                    intTipoPago = Convert.ToInt16(ddlTipoPago.SelectedValue);
                    hTipoPago.Value = ddlTipoPago.SelectedValue.ToString();
                    #region Actualizar Pago
                    BE.RE_PAGO ObjPagoBE = new BE.RE_PAGO();
                    ObjPagoBE.pago_sPagoTipoId = intTipoPago;                    

                    ObjPagoBE.pago_sMonedaLocalId = Comun.ObtenerMonedaLocalId(Session, ddlTipoPago.SelectedValue, txtIdTarifa.Text);

                    ObjPagoBE.pago_iActuacionDetalleId = Comun.ToNullInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);
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
                                ObjPagoBE.pago_FMontoMonedaLocal = Convert.ToDouble(txtMontoML.Text);

                                if (objTarifarioBE.tari_sCalculoTipoId != (int)Enumerador.enmTipoCalculoTarifario.MONTO_FIJO)
                                { ObjPagoBE.pago_FMontoSolesConsulares = Convert.ToDouble(txtTotalSC.Text); }
                                else
                                {
                                    ObjPagoBE.pago_FMontoMonedaLocal = Convert.ToDouble(txtMontoML.Text);
                                    ObjPagoBE.pago_FMontoSolesConsulares = Convert.ToDouble(txtMontoSC.Text);
                                }

                                
                                if (Convert.ToInt32(ObjPagoBE.pago_sPagoTipoId) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA))
                                 {
                                     ObjPagoBE.pago_FMontoMonedaLocal = Convert.ToDouble(txtTotalML.Text);
                                     ObjPagoBE.pago_FMontoSolesConsulares = Convert.ToDouble(txtTotalSC.Text); 
                                 }
                                 else{
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

                        if (objTarifarioBE.tari_sCalculoTipoId != (int)Enumerador.enmTipoCalculoTarifario.MONTO_FIJO)
                        {
                            ObjPagoBE.pago_FMontoMonedaLocal = Convert.ToDouble(txtTotalML.Text);
                            ObjPagoBE.pago_FMontoSolesConsulares = Convert.ToDouble(txtTotalSC.Text);
                        }
                        else
                        {
                            ObjPagoBE.pago_FMontoMonedaLocal = Convert.ToDouble(txtMontoML.Text);
                            ObjPagoBE.pago_FMontoSolesConsulares = Convert.ToDouble(txtMontoSC.Text);
                        }

                        if (CalcularTarifarioEspecial(Convert.ToDouble(txtCantidad.Text)) != 0)
                        {

                            double decTotalSC = CalcularTarifarioEspecial(Convert.ToDouble(txtCantidad.Text));
                            double decTotalML = CalculaCostoML(decTotalSC, Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]));
                            string strFormato = ConfigurationManager.AppSettings["FormatoMonto"].ToString();

                            txtTotalSC.Text = decTotalSC.ToString(strFormato);
                            txtTotalML.Text = decTotalML.ToString(strFormato);
                            ObjPagoBE.pago_FMontoMonedaLocal = Convert.ToDouble(txtTotalML.Text);
                            ObjPagoBE.pago_FMontoSolesConsulares = Convert.ToDouble(txtTotalSC.Text);
                        }
                        else {
                            ObjPagoBE.pago_FMontoMonedaLocal = Convert.ToDouble(txtMontoML.Text);
                            ObjPagoBE.pago_FMontoSolesConsulares = Convert.ToDouble(txtMontoSC.Text);
                        }
                    }

                    bool bNoCobrado = ExisteInafecto_Exoneracion(ObjPagoBE.pago_sPagoTipoId.ToString());

                    if (bNoCobrado ||
                        Convert.ToInt32(ObjPagoBE.pago_sPagoTipoId) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.NO_COBRADO) ||
                        Convert.ToInt32(ObjPagoBE.pago_sPagoTipoId) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
                    {
                        ObjPagoBE.pago_FMontoMonedaLocal = 0;
                        ObjPagoBE.pago_FMontoSolesConsulares = 0;
                    }


                    if (txtSustentoTipoPago.Visible == true)
                    {
                        ObjPagoBE.pago_vSustentoTipoPago = txtSustentoTipoPago.Text.Trim().ToUpper();
                    }
                    else
                    {
                        ObjPagoBE.pago_vSustentoTipoPago = "";
                    }

                    Int64 intNormaTarifarioId = 0;

                    if (ddlExoneracion.Visible == true)
                    {
                        intNormaTarifarioId = Convert.ToInt64(ddlExoneracion.SelectedValue);
                    }
                    ObjPagoBE.pago_iNormaTarifarioId = intNormaTarifarioId;


                    ObjPagoBE.pago_bPagadoFlag = false;
                    ObjPagoBE.pago_vComentario = "";
                    IntRpta = new ActuacionPagoMantenimientoBL().Actualizar(ObjPagoBE,Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                    if (IntRpta > 0)
                    {
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
                if (txtIdTarifa.Text == "20B")
                {
                    Int16 intClasificacion = 0;
                    
                    intClasificacion = Convert.ToInt16(ddlClasificacion.SelectedValue);
                    
                    IntRpta = objBL.Actualizar(ObjActuacBE, ObjActuacDetBE, intClasificacion); 
                }
                else {
                    IntRpta = objBL.Actualizar(ObjActuacBE, ObjActuacDetBE);
                }
                tablaFileUpLoadFoto.Visible = false;
                if (IntRpta > 0)
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Actuaciones", Constantes.CONST_MENSAJE_EXITO, false, 190, 250);
                    

                    //-----------------------------------------------------------------------
                    //Autor: Miguel Márquez Beltrán
                    //Fecha: 19/07/2018
                    //Objetivo: Habilitar la pestaña de Antecedentes Penales
                    //          para la tarifa: 20B y la clasificación: 
                    //          Certificación de Antecedentes Penales
                    //-----------------------------------------------------------------------

                    if (txtIdTarifa.Text == "20B")
                    {
                        if (ddlClasificacion.SelectedItem.Text.ToUpper().Contains(Constantes.CONST_CERTIFICADO_ANTECEDENTES_PENALES))
                        {
                            Session["AntecedentePenalDetalleId"] = "0";
                            CargarDatosAntecedentespenales();                            
                            Comun.EjecutarScript(Page, Util.HabilitarTab(5) + StrScript);
                        }
                        else
                        {
                         
                            //-------------------------------------------------------------
                            //Fecha: 30/07/2018
                            //Autor: Miguel Márquez Beltrán
                            //Objetivo: Anular Antecedentes Penales por Actuación Detalle
                            //-------------------------------------------------------------
                            long lngAntecedentePenalId = 0;

                            if (hanpe_iAntecedentePenalId.Value.Length > 0)
                            {
                                lngAntecedentePenalId = Convert.ToInt64(hanpe_iAntecedentePenalId.Value);
                            }

                            
                            bool bAnulacionExitosa = false;
                            if (lngAntecedentePenalId > 0)
                            {
                                bAnulacionExitosa = AnularAntecedentesPenales();

                                if (bAnulacionExitosa == false)
                                {
                                    Comun.EjecutarScript(Page, Util.HabilitarTab(6) + Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ANTECEDENTE PENAL", Constantes.CONST_MENSAJE_OPERACION_FALLIDA, false, 190, 250));
                                }
                                else
                                {
                                    updAntecedente.Update();
                                    Comun.EjecutarScript(Page, Util.DeshabilitarTab(5) + Util.HabilitarTab(6) + StrScript);
                                }
                            }
                            else
                            {                                
                                Comun.EjecutarScript(Page, Util.DeshabilitarTab(5) + Util.HabilitarTab(6) + StrScript);
                            }
                            //-------------------------------------------------------------                            
                        }
                    }
                    else
                    {
                        Comun.EjecutarScript(Page, StrScript);
                    }
                    //-----------------------------------------------------------------------

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

        #region Registro
        private void CargarDatosTarifaPago()
        {
            BE.RE_TARIFA_PAGO objTarifaPago = new RE_TARIFA_PAGO();
            objTarifaPago.sTarifarioId = Convert.ToInt32(Session["IntTarifarioId"]);
            

            objTarifaPago.sTipoActuacion = 0;

            var existe = new SGAC.Registro.Actuacion.BL.ActuacionPagoConsultaBL().ActuacionPagoObtenerDetalle(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]));

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


                int intCalculoTipoId = Comun.ToNullInt32(existe.Rows[0]["tari_sCalculoTipoId"]);

                switch (intCalculoTipoId)
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

                Session[Constantes.CONST_SESION_OBJ_TARIFA_PAGO] = objTarifaPago;
            }
            else
            { 
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTUACIÓN CONSULAR", "No hay datos de pago."));
                return;
            }
        }

        private void PintarDatosPestaniaRegistro()
        {
            BE.RE_TARIFA_PAGO objTarifaPago = new RE_TARIFA_PAGO();
            objTarifaPago = (BE.RE_TARIFA_PAGO)Session[Constantes.CONST_SESION_OBJ_TARIFA_PAGO];

            if (objTarifaPago != null )
            {
                lblTituloTarifa.Text = objTarifaPago.vTarifaDescripcion;

                txtIdTarifa.Text = objTarifaPago.vTarifa;
                CargarTipoPagoNormaTarifario();
                //***
                //----------------------------------------//
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 15-08-2016
                // Objetivo: Visualizar la clasificación de la tarifa
                // Referencia: Requerimiento No.001_2.doc
                //----------------------------------------//
                if (Convert.ToString(Session["strActo"]) != "Judicial")
                {
                    if (objTarifaPago.dblClasificacion > 0)
                    {
                        lblClasificacion.Visible = true;
                        ddlClasificacion.Visible = true;
                        ddlClasificacion.SelectedValue = objTarifaPago.dblClasificacion.ToString();
                    }
                    else
                    {
                        lblClasificacion.Visible = false;
                        ddlClasificacion.Visible = false;
                        ddlClasificacion.SelectedIndex = -1;
                    }
                    if (txtIdTarifa.Text == "20B")
                    {
                        ctrlToolBarRegistro.btnGrabar.Enabled = true;
                        lblClasificacion.Visible = true;
                        ddlClasificacion.Visible = true;
                        ddlClasificacion.SelectedValue = objTarifaPago.dblClasificacion.ToString();
                        ddlClasificacion.Enabled = true;
                        //-----------------------------------------------------------------------------------
                        // Fecha: 25/07/2018
                        // Autor: Miguel Márquez Beltrán
                        // Objetivo: Limitar el ingreso de caracteres en la caja de texto.
                        //-----------------------------------------------------------------------------------

                        txtObservacionMSIAP.Attributes.Add("onkeypress", " ValidarCaracteres(this, 500);");
                        txtObservacionMSIAP.Attributes.Add("onkeyup", " ValidarCaracteres(this, 500);");
                    }
                    else {
                        ddlClasificacion.Enabled = false;
                    }
                    
                }

                string strFormato = ConfigurationManager.AppSettings["FormatoMonto"].ToString();


                txtDescTarifa.Text = objTarifaPago.vTarifaDescripcion;
                txtMontoSC.Text = objTarifaPago.dblMontoSolesConsulares.ToString(strFormato);
                txtMontoML.Text = objTarifaPago.dblMontoMonedaLocal.ToString(strFormato);
                txtTotalSC.Text = objTarifaPago.dblTotalSolesConsulares.ToString(strFormato);
                txtTotalML.Text = objTarifaPago.dblTotalMonedaLocal.ToString(strFormato);

                LblFecha.Text = objTarifaPago.datFechaRegistroActuacion.ToString(ConfigurationManager.AppSettings["FormatoFechas"]);


                ddlTipoPago.SelectedValue = objTarifaPago.sTipoPagoId.ToString();
                hTipoPago.Value = objTarifaPago.sTipoPagoId.ToString();
                if (objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA ||
                    objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA ||
                    objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA)
                {
                    txtNroOperacion.Text = objTarifaPago.vNumeroOperacion;
                    ddlNomBanco.SelectedValue = objTarifaPago.sBancoId.ToString();
                    ctrFecPago.set_Value = objTarifaPago.datFechaPago;
                }
                
                txtCantidad.Text = objTarifaPago.dblCantidad.ToString(strFormato);
                txtSustentoTipoPago.Text = objTarifaPago.vSustentoTipoPago;
                txtMtoCancelado.Text = objTarifaPago.dblMontoCancelado.ToString(strFormato);

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
                    try
                    {
                    ddlExoneracion.SelectedValue = objTarifaPago.dblNormaTarifario.ToString();
                    }
                    catch
                    {
                    }
                }
                                               
                txtObservaciones.Text = objTarifaPago.vObservaciones;
                                                                            
                LstTarifario.Visible = false;

                txtIdTarifa.Enabled = false;
                txtCantidad.Enabled = false;
                if (ddlTipoPago.SelectedIndex > 0)
                {
                    //---------------------------------------------------
                    // Autor: Miguel Márquez Beltrán
                    // Fecha: 07/11/2016
                    // Objetivo: Visualizar la pestaña Ficha Registral
                    //---------------------------------------------------
                    if (objTarifaPago.tari_sSeccionId == 8)
                    {
                        #region SiEsSeccion8

                        if (Session["ACTO_GENERAL_MRE"] != null)
                        {
                            if (Session["ACTO_GENERAL_MRE"].Equals("1"))
                            {
                                    //----------------------------------------------------------------------
                                    //Fecha: 17/02/2017
                                    //Autor: Miguel Márquez Beltrán
                                    //Objetivo: Activar la pestaña Ficha Registral y la de vinculación.
                                    //----------------------------------------------------------------------
                                    Comun.EjecutarScript(Page, Util.HabilitarTab(4) + Util.HabilitarTab(6));                                   
                            }
                            else
                            {
                                // JOnatan Silva Cachay
                                // cuando es reintegro no se activa la pensaña de ficha registral
                                if (objTarifaPago.vTarifa != "84")
                                {
                                    Comun.EjecutarScript(Page, Util.HabilitarTab(4));
                                }
                            }
                            Session["ACTO_GENERAL_MRE"] = "";
                        }
                        else
                        {
                            if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.REGISTRO || Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.ACTUALIZACION || Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
                            {
                                // JOnatan Silva Cachay
                                // cuando es reintegro no se activa la pensaña de Ficha Registral
                                if (objTarifaPago.vTarifa != "84")
                                {
                                    Comun.EjecutarScript(Page, Util.HabilitarTab(4));                                        
                                }
                            }
                        }
                        


                        CtrlPageBarFichaRegistral.PageSize = Constantes.CONST_CANT_REGISTRO;
                        ctrFechaFicha.set_Value = Comun.FormatearFecha((Accesorios.Comun.ObtenerFechaActualTexto(HttpContext.Current.Session)));
                        txtFechaNacimiento.EndDate = DateTime.Now;
                        txtFechaNacimiento.EnabledText = true;
                        txtFechaNacimiento.EnabledIcon = true;
                        txtFechaNacimiento.Enabled = true;

                        cargarFichasRegistrales();
                        //---------------------------------------------
                        // Objetivo: Quitar la visibilidad de adicionar participante, 
                        //           editar participante o cancelar. 
                        //---------------------------------------------
                        //btnGrabarParticipante.Visible = false;
                        //btnEditarParticipante.Visible = false;
                        //btnCancelarParticipante.Visible = false;
                        //-------------------------------------------------
                        // Objetivo: Asignar caracteres para su validación
                        //          en la función validarcaracterespecial()
                        //-------------------------------------------------
                        String CaracterEspecial1 = String.Empty;
                        CaracterEspecial1 = ConfigurationManager.AppSettings["ValidarText"].ToString();
                        HFValidarTexto.Value = CaracterEspecial1;
                        #endregion
                        //---------------------------------------------
                    }
                    //-----------------------------------------------------------------------
                    //Autor: Miguel Márquez Beltrán
                    //Fecha: 19/07/2018
                    //Objetivo: Habilitar la pestaña de Antecedentes Penales
                    //          para la tarifa: 20B y la clasificación: 
                    //          Certificación de Antecedentes Penales
                    //-----------------------------------------------------------------------
                    if (objTarifaPago.vTarifa == "20B")
                    {
                        #region Tarifa_20B

                        DataTable dtClasificacionTarifa = new DataTable();
                        dtClasificacionTarifa = comun_Part1.ObtenerParametrosPorGrupo(Session, SGAC.Accesorios.Constantes.CONST_ACTUACION_CLASIFICACION_TARIFA);

                        int intClasificacionId = Convert.ToInt32(objTarifaPago.dblClasificacion);
                        string strClasificacion = "";

                        for (int i = 0; i < dtClasificacionTarifa.Rows.Count; i++)
                        {
                            if (Convert.ToInt32(dtClasificacionTarifa.Rows[i]["id"].ToString()) == intClasificacionId)
                            {
                                strClasificacion = dtClasificacionTarifa.Rows[i]["descripcion"].ToString().ToUpper();
                                break;
                            }
                        }
                        if (strClasificacion.Contains(Constantes.CONST_CERTIFICADO_ANTECEDENTES_PENALES))
                        {
                            CargarDatosAntecedentespenales();
                            
                            if (Session["IngresoVistaPrevia"] == null)
                            {
                                Session["IngresoVistaPrevia"] = "";
                            }
                            if (Session["IngresoReimprimir"] == null)
                            {
                                Session["IngresoReimprimir"] = 0;
                            }
                            int intReimprimir = Convert.ToInt32(Session["IngresoReimprimir"].ToString());
                            intReimprimir = intReimprimir + 1;
                            Session["IngresoReimprimir"] = intReimprimir;
                            if (Session["ACTO_GENERAL_MRE"].ToString() == "1" || Session["IngresoVistaPrevia"].ToString() == "S" || intReimprimir > 1)
                            {
                                Session["IngresoVistaPrevia"] = "N";
                                Session["IngresoReimprimir"] = "1";
                                Comun.EjecutarScript(Page, Util.HabilitarTab(5) + Util.ActivarTab(6, "Vinculación"));                                
                            }
                            else
                            {
                                //Cuando edita
                                if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.REGISTRO ||
                                    Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.ACTUALIZACION)
                                {
                                    Comun.EjecutarScript(Page, Util.HabilitarTab(5) + Util.ActivarTab(5, "Antecedentes Penales"));
                                }
                            }
                        }
                        else
                        {

                            Comun.EjecutarScript(Page, Util.DeshabilitarSoloTab(5) + Util.HabilitarTab(6));
                        }
                        #endregion
                    }
                    else
                    {
                        Comun.EjecutarScript(Page, Util.HabilitarTab(6));
                    }


                }
                else
                {
                    Comun.EjecutarScript(Page, Util.HabilitarTab(0));
                }

                //if (Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA) ||
                //    Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA) ||
                //    Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA))
                //{
                //    HabilitaDatosPago(false);
                //    pnlPagLima.Visible = true;
                //    txtMtoCancelado.Text = (Convert.ToDouble(txtCantidad.Text) * Convert.ToDouble(txtMontoML.Text)).ToString(strFormato);
                //}
                //else
                //{
                //    pnlPagLima.Visible = false;
                //}

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

        

        #endregion

        #region Adjuntos
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
            dtAdjuntos = obj.ActuacionAdjuntosObtener(LonActuacionDetalleId, "1", intPaginaCantidad,ref IntTotalCount,ref IntTotalPages);

            if (dtAdjuntos.Rows.Count > 0)
            {
                ctrlAdjunto.Grd_Archivos.DataSource = dtAdjuntos;
                ctrlAdjunto.Grd_Archivos.DataBind();
            }
        }
        #endregion

        #region Vinculación LEER POR FAVOR LA DESCRIPCION XD!!!!!!

        #region  Descripcion
        // En el Evento Load Cargar los Siguiente:
     
        //Tambien los Evento de JavaScript al Ejecutar un evento Click fuera del IsPostBack
        //           BtnGrabAnotacion.OnClientClick = "return ValidarRegistroAnotacion();";
        //  Asegurarse que Funcion JavaScript Exista en tu formulario
        #endregion

        // Eventos para el manejo de la impresión del Autuadhesivo 
        private void BindGridActuacionesInsumoDetalle(Int64 iActuacionDetalleId)
        {
            try
            {
                Grd_ActInsDet.DataSource = null;
                Grd_ActInsDet.DataBind();

                ActuacionMantenimientoBL objActuacionMantenimientoBL = new ActuacionMantenimientoBL();
                DataTable dtActuacionInsumoDetalle = new DataTable();

                ctrlAdjunto.SetCodigoVinculacion(string.Empty);

                int IntTotalCount = 0;
                int IntTotalPages = 0;
                int intPaginaCantidad = Constantes.CONST_PAGE_SIZE_ADJUNTOS;
                int PaginaActual = CtrlPageBarActuacionInsumoDetalle.PaginaActual;
                dtActuacionInsumoDetalle = objActuacionMantenimientoBL.Obtener_ActuacionInsumoDetalle(iActuacionDetalleId, PaginaActual, intPaginaCantidad, ref IntTotalCount, ref  IntTotalPages); // objActuacionMantenimientoBL.Obtener_ActuacionInsumoDetalle(iActuacionDetalleId, ctrlPaginadorAct.PaginaActual.ToString(), intPaginaCantidad, IntTotalCount, IntTotalPages);

                if (dtActuacionInsumoDetalle.Rows.Count > 0)
                {
                    txtCodAutoadhesivo.Text = dtActuacionInsumoDetalle.Rows[0]["insu_vCodigoUnicoFabrica"].ToString();
                    ctrlAdjunto.SetCodigoVinculacion(txtCodAutoadhesivo.Text);
                    Session["COD_AUTOADHESIVO"] = txtCodAutoadhesivo.Text.Trim();

                    string strFlag = dtActuacionInsumoDetalle.Rows[0]["aide_bFlagImpresion"].ToString();
                    Session[Constantes.CONST_ACTUACION_INSUMO_DETALLE_ID] = dtActuacionInsumoDetalle.Rows[0]["aide_iActuacionInsumoDetalleId"].ToString();
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
                        hnd_ImpresionCorrecta.Value = "1";
                    }
                    else
                    {
                        btnVistaPrev.Enabled = true;
                        chkImpresion.Checked = false;
                        btnVistaPrev.Enabled = true;
                        hnd_ImpresionCorrecta.Value = "";
                    }
                    txtCodAutoadhesivo.Enabled = false;
                    btnLimpiar.Enabled = false;
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
                    //txtCodAutoadhesivo.Text = "";
                    txtCodAutoadhesivo.Focus();
                    txtCodAutoadhesivo.Enabled = true;
                    ctrlBajaAutoadhesivo1.Activar = false;
                    txtCodAutoadhesivo.Enabled = true;
                    btnGrabarVinculacion.Enabled = true;
                    btnVistaPrev.Enabled = false;
                    btnLimpiar.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        protected void btnVistaPrev_Click(object sender, EventArgs e)
        {
            try
            {
                Session["IngresoVistaPrevia"] = "S";

                bool bModoHTML = true;
                String StrScript = String.Empty;
                if (txtIdTarifa.Enabled)
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", "Debe Primero Registrar el Pago de la Actuación", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }
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
                    #region Vista Previa Autoadhesivo
                    string strScript = "abrirVentana('../Registro/FrmRepAutoadhesivo.aspx?GUID=" + HFGUID.Value + "', 'AUTOADHESIVOS', 610, 450, '');";
                    //string strUrl = "../Registro/FrmRepAutoadhesivo.aspx";
                    //string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=500,height=700,left=100,top=100');";
                    Comun.EjecutarScript(Page, strScript);

                    Session["FEC_IMPRESION"] = Util.ObtenerFechaActual(
                        Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sDiferenciaHoraria")),
                        Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sHorarioVerano")));
                    #endregion
                }
                else
                {
                    Int64 intActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);

                    DocumentoiTextSharp oDocumentoiTextSharp = new DocumentoiTextSharp(this.Page, string.Empty, HttpContext.Current.Server.MapPath("~/Images/Escudo.JPG"));
                    oDocumentoiTextSharp.ActuacionDetalleId = intActuacionDetalleId;
                    oDocumentoiTextSharp.CrearAutoAdhesivo();
                }

                if (btnGrabarVinculacion.Visible)
                {
                    if (txtCodAutoadhesivo.Text.Trim() == string.Empty)
                    {
                        txtCodAutoadhesivo.Enabled = false;
                        btnLimpiar.Enabled = false;
                        chkImpresion.Enabled = true;
                    }
                    else
                    {
                        chkImpresion.Enabled = false;
                        txtCodAutoadhesivo.Enabled = false;
                        btnLimpiar.Enabled = false;
                    }
                }
                else
                {
                    chkImpresion.Enabled = false;
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
        protected void btnGrabarVinculacion_Click(object sender, EventArgs e)
        {
            try
            {

            String StrScript = String.Empty;
            
            if (txtIdTarifa.Enabled)
            {
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", "Debe Primero Registrar el Pago de la Actuación", false, 190, 250);
                Comun.EjecutarScript(Page, StrScript);
                
                String scriptMover = @"$(function(){{ MoveTabIndex(0);}});";
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);
                Comun.EjecutarScript(Page, "cerrarPopupEspera();"); 
                return;
            }

            ctrlCargando1.Visible = true;

            Int64 intActDetalleId = 0;

            if (Session["NuevoRegistro"] != null)
                if (!Convert.ToBoolean(Session["NuevoRegistro"]))
                    intActDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);

            String FormatoFecha = ConfigurationManager.AppSettings["FormatoFechas"].ToString();

            if (FormatoFecha.Trim() == String.Empty)
            {
                FormatoFecha = "MMM-dd-yyyy";
            }


            //*----------------------------------*
            //*Fecha: 03/12/2019
            //*Autor: Miguel Márquez Beltrán
            //*Motivo: Usar la sesión creada.
            //*----------------------------------*
            Double intDiferenciaHoraria = Convert.ToDouble(Session[Constantes.CONST_SESION_DIFERENCIA_HORARIA].ToString());
            Double intHorarioVerano = Convert.ToDouble(Session[Constantes.CONST_SESION_HORARIO_VERANO].ToString());

            //DateTime dFecActual = Util.ObtenerFechaActual(
            //                                Convert.ToInt16(Comun.ObtenerDatoOficinaConsular(Session, "ofco_sDiferenciaHoraria")),
            //                                Convert.ToInt16(Comun.ObtenerDatoOficinaConsular(Session, "ofco_sHorarioVerano")));

            DateTime dFecActual = Util.ObtenerFechaActual(intDiferenciaHoraria, intHorarioVerano);


            Session["ACTUACIONDETALLE"] = Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value];
            string strMensaje= string.Empty;

            Int16 CiudadItinerante = 0;

            if (Session[Constantes.CONST_SESION_CIUDAD_ITINERANTE].ToString() != "")
            {
                CiudadItinerante = Convert.ToInt16(Session[Constantes.CONST_SESION_CIUDAD_CODIGO_ITINERANTE]);
            }

            ActuacionMantenimientoBL oActuacionMantenimientoBL = new ActuacionMantenimientoBL();
            int intResultado = oActuacionMantenimientoBL.VincularAutoadhesivo(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value]),
                                            Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]),
                                            (int)Enumerador.enmInsumoTipo.AUTOADHESIVO,
                                            txtCodAutoadhesivo.Text.Trim().ToUpper(),
                                            dFecActual,
                                            false,
                                             dFecActual,
                                            0, // FUNCIONARIO
                                            Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                                            Convert.ToInt32(Session[Constantes.CONST_SESION_USUARIO_ID]),
                                            ref strMensaje, Enumerador.enmNotarialTipoFormato.OTROS, CiudadItinerante);

            String strScript = String.Empty;
            if (strMensaje.Trim() == string.Empty)
            {     
                btnGrabarVinculacion.Enabled = false;
                txtCodAutoadhesivo.Enabled = false;
                btnLimpiar.Enabled = false;
                chkImpresion.Enabled = false;

                Session["COD_AUTOADHESIVO"] = txtCodAutoadhesivo.Text.Trim();

                #region Tipo Adjunto
                ctrlAdjunto.SetCodigoVinculacion(txtCodAutoadhesivo.Text.Trim());
                ctrlAdjunto.CargarTipoArchivo();
                updActuacionAdjuntar.Update();
                //updVinculacion.Update();

                //btnVistaPrev.Enabled = false;
                updVinculacion.Update();
                #endregion

                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION,
                    "VINCULACIÓN", "La vinculación se realizó correctamente.");
                Comun.EjecutarScript(Page, strScript);
                Comun.EjecutarScript(Page, "cerrarPopupEspera();"); 
                //ctrlToolBarRegistro.btnGrabar.Enabled = false;

                BindGridActuacionesInsumoDetalle(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]));

                //HFGUID.Value = Sanitizer.GetSafeHtmlFragment(Request.QueryString["GUID"].ToString());
                //Response.Redirect("FrmActoGeneral.aspx?GUID=" + HFGUID.Value);
            }
            else
            {
                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "VINCULACIÓN", strMensaje, false, 200, 400);
                Comun.EjecutarScript(Page, strScript);
                Comun.EjecutarScript(Page, "cerrarPopupEspera();"); 
            }
            ctrlCargando1.Visible = false;
            ddlTipoPago.SelectedValue = hTipoPago.Value.ToString();
            
            updRegPago.Update();
            
            Comun.EjecutarScript(Page, "cerrarPopupEspera();");
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
        protected void ctrlPagActuacionInsumoDetalle_Click(object sender, EventArgs e)
        {
            BindGridActuacionesInsumoDetalle(Convert.ToInt64(Session["ActuacionDetalleId" + HFGUID.Value]));
            updVinculacion.Update();
            
        }

        protected void chkImpresion_CheckedChanged(object sender, EventArgs e)
        {
            if (chkImpresion.Checked)
            {
                txtCodAutoadhesivo.Enabled = true;
                btnLimpiar.Enabled = true;
            }
            else
            {
                txtCodAutoadhesivo.Enabled = false;
                btnLimpiar.Enabled = false;
            }
            updVinculacion.Update();
        }
        #endregion

        protected void txtCodAutoadhesivo_TextChanged(object sender, EventArgs e)
        {
          
        }

        protected void txtIdTarifa_TextChanged(object sender, EventArgs e)
        {

        }

        DataTable CrearTmpTabla()
        {
            DataTable dtTablaTemporal = new DataTable();

            dtTablaTemporal.Columns.Add("strCadenaBuscar", typeof(string));
            dtTablaTemporal.Columns.Add("strCadenaReemplazar", typeof(string));

            return dtTablaTemporal;
        }

        protected void btn_acta_diligenciamiento_Click(object sender, EventArgs e)
        {
            try
            {

                var CLActas = new ActaJudicialConsultaBL().Obtener(Convert.ToInt64(Session["iActoJudicialNotificacionId"]));

                string s_Cuerpo = string.Empty;
                string s_Resultado = string.Empty;
                string s_Observacion = string.Empty;

                try
                {
                    var existe_acta = (from dr in CLActas.AsEnumerable()
                                       where dr.Field<Int16>("acjd_sTipoActaId") == Convert.ToInt16(Enumerador.enmJudicialTipoActa.ACTA_DILIGENCIAMIENTO)
                                       select dr).CopyToDataTable();


                    s_Cuerpo = Convert.ToString(existe_acta.Rows[0]["acjd_vCuerpoActa"]);
                    s_Resultado = Convert.ToString(existe_acta.Rows[0]["acjd_vResultado"]);
                    s_Observacion = Convert.ToString(existe_acta.Rows[0]["acjd_vObservaciones"]);
                }
                catch(Exception ex)
                {
                    #region Registro Incidencia
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_vValoresTabla = "ACTO JUDICIAL - Acta Diligenciamiento FORMATO",
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

                    s_Cuerpo = string.Empty;
                    s_Resultado = string.Empty;
                    s_Observacion = string.Empty;
                }
                 
                var dtTMPReemplazar = CrearTmpTabla();
                    
                DataRow row = dtTMPReemplazar.NewRow(); row["strCadenaBuscar"] = "[strCuerpo]"; row["strCadenaReemplazar"] = s_Cuerpo; dtTMPReemplazar.Rows.Add(row);
                DataRow row1 = dtTMPReemplazar.NewRow(); row1["strCadenaBuscar"] = "[strResultado]"; row1["strCadenaReemplazar"] = s_Resultado; dtTMPReemplazar.Rows.Add(row1);
                DataRow row2 = dtTMPReemplazar.NewRow(); row2["strCadenaBuscar"] = "[strObservacion]"; row2["strCadenaReemplazar"] = (s_Observacion != "" ? "" : s_Observacion) /* El tiket 361  nos indica que no deberia mostrase la observacion */; dtTMPReemplazar.Rows.Add(row2);
                DataRow row4 = dtTMPReemplazar.NewRow(); row4["strCadenaBuscar"] = "[Consulado]"; row4["strCadenaReemplazar"] = Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE]; dtTMPReemplazar.Rows.Add(row4);
                DataRow row3 = dtTMPReemplazar.NewRow(); row3["strCadenaBuscar"] = "[Logo]"; dtTMPReemplazar.Rows.Add(row3);

                Util.DataTableVarcharMayusculas(dtTMPReemplazar);

                string strRutaHtml = string.Empty;
                string strArchivoPDF = string.Empty;

                strRutaHtml = Server.MapPath("~/Registro/Plantillas/acta-diligenciamiento.html");
                strArchivoPDF = "acta-diligenciamiento.pdf";

                String localfilepath = String.Empty;
                String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];

                string strRutaPDF = uploadPath + @"\" + strArchivoPDF;

                //UIConvert.GenerarPDF(dtTMPReemplazar, strRutaHtml, strRutaPDF);
                CreateFilePDFConformidad(dtTMPReemplazar, strRutaHtml, strRutaPDF, HttpContext.Current.Server.MapPath("~/Images/Escudo.PNG"));
                if (System.IO.File.Exists(strRutaPDF))
                {
                    new Descarga().Download(strRutaPDF, strArchivoPDF, false);
                }
            }
            catch (Exception ex)
            {
                #region Registro Incidencia
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_vValoresTabla = "ACTO JUDICIAL",
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

                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, fMargenIzquierdaDoc, fMargenDerechaDoc, 80, 80);

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

                //---------------------------------------------------------------
                // Fecha: 11/12/2019
                // Autor: Miguel Márquez Beltrán
                // Motivo: No debe aparecer el escudo en el Acta de Conformidad.
                //---------------------------------------------------------------

                //#region Imagen

                //iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imgServerPAth);
                //img.SetAbsolutePosition(40f, document.PageSize.Height - 130);
                //img.ScaleAbsoluteHeight(100f);
                //img.ScaleAbsoluteWidth(63f);
                //document.Add(img);

                //#endregion

                //#region Consulado Imagen

                //// PdfContentByte cb = writer.DirectContent;
                //iTextSharp.text.pdf.BaseFont bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);
                //iTextSharp.text.Font fontConsulado = iTextSharp.text.FontFactory.GetFont("Arial", 6);


                //cb.BeginText();


                //cb.SetFontAndSize(bfTimes, 6);

                //string texto = string.Empty;

                //float pos = 0;
                //float tamPalabra = 0;
                //float ancho = 80f;
                //String NombreConsulado = String.Empty;
                //NombreConsulado = HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE].ToString();

                //if (NombreConsulado.ToUpper().Contains("CONSULADO GENERAL DEL PERÚ"))
                //{
                //    ancho = new iTextSharp.text.Chunk("CONSULADO GENERAL DEL PERÚ", fontConsulado).GetWidthPoint() + 5;
                //    NombreConsulado = NombreConsulado.ToUpper().Replace("PERÚ EN", "PERÚ");
                //}




                //int iPosicionComa = NombreConsulado.IndexOf(",");

                //if (iPosicionComa >= 0)
                //    NombreConsulado = NombreConsulado.Substring(0, iPosicionComa);



                //float posxAcumulado = tamPalabra;

                //foreach (string palabra in NombreConsulado.Split(' '))
                //{
                //    tamPalabra = new iTextSharp.text.Chunk(palabra.Trim(), fontConsulado).GetWidthPoint();

                //    if (posxAcumulado + tamPalabra > ancho)
                //    {

                //        cb.SetTextMatrix(40f + (57.77f / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f), document.PageSize.Height - 140 + pos);
                //        cb.ShowText(texto.Trim());
                //        texto = string.Empty;

                //        pos -= 10;
                //        posxAcumulado = 0;
                //    }

                //    posxAcumulado += tamPalabra;
                //    posxAcumulado += new iTextSharp.text.Chunk(" ", fontConsulado).GetWidthPoint();
                //    texto += " " + palabra;
                //}

                //if (texto.Trim() != string.Empty)
                //{
                //    cb.SetTextMatrix(40f + (57.77f / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f), document.PageSize.Height - 140 + pos);
                //    cb.ShowText(texto.Trim());
                //}

                //cb.EndText();

                //#endregion

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

                if (listFirmas != null)
                {
                    frase.Add(new iTextSharp.text.Chunk("\n\n\n\n\n"));
                    parrafo.Add(frase);
                    document.Add(parrafo);

                    parrafo = new iTextSharp.text.Paragraph();
                    frase = new iTextSharp.text.Phrase();

                    frase.Add(new iTextSharp.text.pdf.draw.LineSeparator(1F, 40.0F, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_LEFT, 1));
                    parrafo.Add(frase);
                    document.Add(parrafo);

                    parrafo = new iTextSharp.text.Paragraph();
                    frase = new iTextSharp.text.Phrase();
                    frase.Add(new iTextSharp.text.Chunk("\n" + listFirmas[0][0].ToString().Replace(",", "")));
                    frase.Add(new iTextSharp.text.Chunk("\n" + listFirmas[0][1].ToString()));
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

        private double CalcularTarifarioEspecial(double intCantidad)
        {
            double douMonto = 0;
            switch (txtIdTarifa.Text)
            {
                case "7C":
                    {
                        // TARIFA 7C
                        if (intCantidad > 1)
                            douMonto = 150 * 2 + (intCantidad - 2) * 8;
                        else
                            douMonto = 150;
                        break;
                    }
                case "17A":
                    {
                        // TARIFA 17A
                        douMonto = 16 + (intCantidad - 1) * 8;
                        break;
                    }
                case "17C":
                    {
                        // TARIFA 17C
                        douMonto = 16 + (intCantidad - 1) * 8;
                        break;
                    }
                case "30":
                    {
                        // TARIFA 30
                        if (intCantidad > 1)
                            douMonto = 80 + (intCantidad - 2) * 20;
                        else
                            douMonto = 80;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return douMonto;
        }
        private double CalculaCostoML(double decMontoSC, double decTipoCambio)
        {
            return (decMontoSC * decTipoCambio);
        }
        protected void ddlTipoPago_SelectedIndexChanged(object sender, EventArgs e)
        {

            BE.RE_TARIFA_PAGO objTarifaPago = new BE.RE_TARIFA_PAGO();
            objTarifaPago = (BE.RE_TARIFA_PAGO)Session[Constantes.CONST_SESION_OBJ_TARIFA_PAGO];
            if (objTarifaPago != null )
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

        protected void btnDesabilitarAutoahesivo_Click(object sender, EventArgs e)
        {
            btnVistaPrev.Enabled = false;
            chkImpresion.Checked = true;
            chkImpresion.Enabled = false;
            txtCodAutoadhesivo.Enabled = false;
            btnLimpiar.Enabled = false;
            hnd_ImpresionCorrecta.Value = "1";
            BindGridActuacionesInsumoDetalle(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]));
            updVinculacion.Update();
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

        //-----------------------------------------------------------//
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 08-11-2016
        // Objetivo: Registrar la Ficha Registral
        //-----------------------------------------------------------//

         protected void btnAgregarFicha_Click(object sender, EventArgs e)
        {
            AgregarFicha();
        }
         private void CargarDatosEditar()
         {
             DataTable dtFichas = new DataTable();

             if (Session["FichaRegistral"] == null)
             {
                 dtFichas = CrearDataTable();
             }
             else
             {
                 dtFichas = (DataTable)Session["FichaRegistral"];
             }
             if (dtFichas.Rows.Count > 0)
             {
                 string strFichaRegistralId = dtFichas.Rows[0]["FIRE_IFICHAREGISTRALID"].ToString();
                 

                 HFFichaRegistralId.Value = strFichaRegistralId;

                 if (rbSemi.Checked)
                 {
                     DivSemiAutomatico.Visible = true;
                 }
                 else { DivSemiAutomatico.Visible = false; }

                 string strNumeroFicha = dtFichas.Rows[0]["FIRE_VNUMEROFICHA"].ToString();

                 EditarFichaRegistral(strNumeroFicha);
                 
                 ConsultarParticipantesFichaRegistral(Convert.ToInt32(strFichaRegistralId));

                 txtNroFicha.Focus();

                 
             }
             
         }

         private void VerificarTarficaReniecConFIcha()
         {
             string strTarifasSInFichaReniec = "";
             strTarifasSInFichaReniec = comun_Part1.ObtenerParametroDatoPorCampo(Session, SGAC.Accesorios.Constantes.CONST_RESTRICCIONES_FICHA_RENIEC, SGAC.Accesorios.Constantes.CONST_TARIFAS_SIN_FICHAS_REGISTRALES, "valor");

             string[] separadas;

             separadas = strTarifasSInFichaReniec.Split(',');

             foreach (string s in separadas)
             {
                 if (txtIdTarifa.Text.Contains(s))
                 {
                     txtNroFicha.Enabled = false;
                     break;
                 }
                 else
                 {
                     txtNroFicha.Enabled = true;
                 }
             }
         }
        protected void GridViewFicha_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strFichaRegistralId = "";
            string strNumeroFicha = "";
            //int index = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Select")
            {
                GridViewRow gvrModificar;
                gvrModificar = (GridViewRow)((TableCell)((ImageButton)e.CommandSource).Parent).Parent;

                Label lblFichaRegistralId = (Label)gvrModificar.FindControl("lblFichaRegistralId");

                strFichaRegistralId = lblFichaRegistralId.Text.Trim();

                HFFichaRegistralId.Value = strFichaRegistralId;

                Label lblNumeroFicha = (Label)gvrModificar.FindControl("lblNumeroFicha");
                strNumeroFicha = lblNumeroFicha.Text.Trim();

                ConsultarFichaRegistral(strNumeroFicha);

                Label lblEstadoFicha = (Label)gvrModificar.FindControl("lblEstadoFicha");
                hFichaRegistralId.Value = strFichaRegistralId;
                hNumeroFicha.Value = strNumeroFicha;

                if (Convert.ToInt64(HFFichaRegistralId.Value) > 0)
                {
                    ConsultarParticipantesFichaRegistral(Convert.ToInt64(HFFichaRegistralId.Value));
                    DataTable dtFichaRegistral = (DataTable)Session["FichaRegistral"];
                    if (dtFichaRegistral.Rows.Count > 1)
                    {
                       // Comun.EjecutarScript(Page, "ActivarGrilla();");
                    }
                    else
                    {
                        Comun.EjecutarScript(Page, "OcultarGrilla();");
                        updFicha.Update();
                    }
                    //if (lblEstadoFicha.Text.Trim().ToUpper() != "ANULADO")
                    //{
                    //    EstadoBarraFichaRegistral(false);
                    //    //btnGrabarParticipante.Visible = true;
                    //    btnCancelarParticipante.Visible = true;
                    //    ctrlToolBarRegistro.btnCancelar.Enabled = true;
                    //    ctrlToolBarFicha.btnImprimir.Enabled = true;
                    //}
                    //else
                    //{
                    //    //btnGrabarParticipante.Visible = false;
                    //    //btnCancelarParticipante.Visible = false;
                    //}
                    ////pnlParticipantes.Style.Add("display", "block");
                }
                //else
                //{
                //    //btnGrabarParticipante.Visible = false;
                //    //btnCancelarParticipante.Visible = false;
                //    //pnlParticipantes.Style.Add("display", "none");
                //    //ctrlToolBarFicha.btnImprimir.Enabled = false;
                //}
            }

            //if (e.CommandName == "Anular")
            //{
            //    GridViewRow gvrModificar;
            //    gvrModificar = (GridViewRow)((TableCell)((ImageButton)e.CommandSource).Parent).Parent;

            //    Label lblFichaRegistralId = (Label)gvrModificar.FindControl("lblFichaRegistralId");

            //    strFichaRegistralId = lblFichaRegistralId.Text.Trim();

            //    Label lblNumeroFicha = (Label)gvrModificar.FindControl("lblNumeroFicha");
            //    strNumeroFicha = lblNumeroFicha.Text.Trim();
            //    //pnlParticipantes.Style.Add("display", "none");
            //    ctrlToolBarFicha.btnImprimir.Enabled = false;
            //    AnularFichaRegistral(strFichaRegistralId, strNumeroFicha);
                
            //}
            //if (e.CommandName == "Editar")
            //{
            //    GridViewRow gvrModificar;
            //    gvrModificar = (GridViewRow)((TableCell)((ImageButton)e.CommandSource).Parent).Parent;

            //    Label lblFichaRegistralId = (Label)gvrModificar.FindControl("lblFichaRegistralId");

            //    strFichaRegistralId = lblFichaRegistralId.Text.Trim();

            //    HFFichaRegistralId.Value = strFichaRegistralId;

            //    Label lblNumeroFicha = (Label)gvrModificar.FindControl("lblNumeroFicha");
            //    strNumeroFicha = lblNumeroFicha.Text.Trim();

            //    EditarFichaRegistral(strNumeroFicha);
            //    EstadoNormalBarraFichaRegistral(false);

            //    //---------------------------------------------
            //    // Objetivo: Quitar la visibilidad de adicionar participante, 
            //    //           editar participante o cancelar. 
            //    //---------------------------------------------
            //    //btnGrabarParticipante.Visible = false;
            //    btnEditarParticipante.Visible = false;
            //    //btnCancelarParticipante.Visible = false;
            //    //---------------------------------------------
            //    ctrlToolBarRegistro.btnCancelar.Enabled = true;
            //    ctrlToolBarFicha.btnImprimir.Enabled = false;
            //    Session["FichaRegistralParticipante"] = null;
            //    GridViewParticipante.DataSource = null;
            //    GridViewParticipante.DataBind();
            //    //pnlParticipantes.Style.Add("display", "none");
            //    txtNroFicha.Focus();
            //}
        }

        protected void GridViewFicha_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Int64 intFichaRegistralId = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string fichaRegistral = Convert.ToString(Request.QueryString["fregi"]);
                if (fichaRegistral != null)
                {
                    fichaRegistral = Util.DesEncriptar(fichaRegistral);
                    HFNumeroFichaRegistral.Value = fichaRegistral;
                }
                
                Label lblFichaRegistralId = (Label)e.Row.FindControl("lblFichaRegistralId");
                intFichaRegistralId = Convert.ToInt64(lblFichaRegistralId.Text.Trim());
                //ImageButton btnSeleccionar = (ImageButton)e.Row.FindControl("btnSeleccionar");
                Label lblEstadoFicha = (Label)e.Row.FindControl("lblEstadoFicha");

                Label lblNumeroFicha = (Label)e.Row.FindControl("lblNumeroFicha");
                if (lblNumeroFicha.Text == fichaRegistral)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#c6efce");
                }

                string strEstado = lblEstadoFicha.Text;
                //if (intFichaRegistralId == 0)
                //{
                //    btnSeleccionar.Visible = false;
                //}
                //else
                //{
                //    if (strEstado == "ANULADO")
                //    {
                //        btnSeleccionar.Visible = false; 
                //    }
                //    else { btnSeleccionar.Visible = true; }
                    
                //}
                //Label lblEstadoFicha = (Label)e.Row.FindControl("lblEstadoFicha");
                //ImageButton btnEditar = (ImageButton)e.Row.FindControl("btnEditar");
                //ImageButton btnAnular = (ImageButton)e.Row.FindControl("btnAnular");

                if (lblEstadoFicha.Text.Equals("ANULADO") || lblEstadoFicha.Text.Equals("REPROCESO") || lblEstadoFicha.Text.Equals("REIMPRESIÓN") || lblEstadoFicha.Text.Equals("RECUPERADO"))
                {
                    //btnEditar.Visible = false;
                    //btnAnular.Visible = false;
                }
                else
                {
                    //btnEditar.Visible = true;
                    //btnAnular.Visible = true;
                }
            }
        }   

        protected void ctrlPageBarFichaRegistral_Click(object sender, EventArgs e)
        {
            cargarFichasRegistrales();
        }

        protected void btnAceptarFicha_Click(object sender, EventArgs e)
        {
            AgregarFicha();
        }

        private void AgregarFicha()
        {
            //EDITAR 
            DataTable dtFichas = new DataTable();

            string strNumeroFicha = HFNumeroFichaRegistral.Value;

            if (Session["FichaRegistral"] == null)
            {
                dtFichas = CrearDataTable();
            }
            else
            {
                dtFichas = (DataTable)Session["FichaRegistral"];
            }

            if (dtFichas.Rows.Count > 0)
            {
                if (!(validarFicha()))
                {
                    ctrlToolBarFicha.btnNuevo.Enabled = false;
                    hNuevo.Value = "0";
                    //ctrlToolBarFicha.btnGrabar.Enabled = false;
                    ctrlToolBarFicha.btnImprimir.Enabled = false;
                    ctrlToolBarFicha.btnCancelar.Enabled = true;
                    return;
                }
                if (strNumeroFicha.Length == 0)
                {
                    strNumeroFicha = dtFichas.Rows[0]["FIRE_VNUMEROFICHA"].ToString();
                }
                if (txtNroFicha.Text.Trim().Length == 0)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Ficha Registral", "Digite el Número de la Ficha Registral."));
                    return;
                }
                if (ctrFechaFicha.Text.Trim().Length == 0)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Ficha Registral", "Seleccione la fecha de registro."));
                    return;
                }

                

                if (dtFichas.Rows.Count > 0)
                {
                    for (int i = 0; i < dtFichas.Rows.Count; i++)
                    {
                        if (dtFichas.Rows[i]["FIRE_VNUMEROFICHA"].ToString().Equals(strNumeroFicha))
                        {
                            long lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);
                            DateTime dFechaEstado = ctrFechaFicha.Value();
                            string strObservacion = txtObservacionFicha.Text.Trim().ToUpper();
                            Int16 intEstadoId = Convert.ToInt16(ddlEstadoFicha.SelectedValue);
                            string strEstado = ddlEstadoFicha.SelectedItem.Text.Trim();
                            string strCodigoLocalDestinoId = ddlLocalDestino.SelectedValue;

                            dtFichas.Rows[i]["FIRE_VNUMEROFICHA"] = txtNroFicha.Text.Trim();
                            dtFichas.Rows[i]["FIRE_VCODIGOLOCALDESTINO"] = strCodigoLocalDestinoId;
                            dtFichas.Rows[i]["FIRE_DFECHAESTADO"] = dFechaEstado;
                            dtFichas.Rows[i]["FIRE_VOBSERVACION"] = strObservacion;
                            dtFichas.Rows[i]["FIRE_SESTADOID"] = intEstadoId;
                            dtFichas.Rows[i]["ESTA_VDESCRIPCIONCORTA"] = strEstado;
                            break;
                        }
                    }
                    Session["FichaRegistral"] = dtFichas;
                    GridViewFicha.DataSource = dtFichas;
                    GridViewFicha.DataBind();

                    HFNumeroFichaRegistral.Value = "";
                    txtNroFicha.Text = "";
                    txtObservacionFicha.Text = "";
                    ddlEstadoFicha.SelectedIndex = 0;
                    ddlLocalDestino.SelectedIndex = 0;
                    EstadoNormalBarraFichaRegistral(true);

                }
                ctrlToolBarFicha.btnGrabar.Enabled = true;
                //btnAceptarFicha.Visible = false;
                updFicha.Update();
            }
            //agregar ficha a grilla temporal
            else {
                if (!(validarFicha()))
                {
                    ctrlToolBarFicha.btnNuevo.Enabled = false;
                    hNuevo.Value = "0";
                    //ctrlToolBarFicha.btnGrabar.Enabled = false;
                    ctrlToolBarFicha.btnImprimir.Enabled = false;
                    ctrlToolBarFicha.btnCancelar.Enabled = true;
                    return;
                }

                if (Session["FichaRegistral"] == null)
                {
                    dtFichas = CrearDataTable();
                }
                else
                {
                    dtFichas = (DataTable)Session["FichaRegistral"];
                }
                long lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);
                strNumeroFicha = txtNroFicha.Text.Trim();
                DateTime dFechaEstado = ctrFechaFicha.Value();
                string strObservacion = txtObservacionFicha.Text.Trim().ToUpper();
                Int16 intEstadoId = Convert.ToInt16(ddlEstadoFicha.SelectedValue);
                string strEstado = ddlEstadoFicha.SelectedItem.Text.Trim();
                string strCodigoLocal = comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "vCodigoLocal").ToString();
                string strCodigoLocalDestino = ddlLocalDestino.SelectedValue;

                DataRow dr = dtFichas.NewRow();

                dr["FIRE_IFICHAREGISTRALID"] = 0;
                dr["INUMERO"] = dtFichas.Rows.Count + 1;
                dr["FIRE_IACTUACIONDETALLEID"] = lngActuacionDetalleId;
                dr["FIRE_VNUMEROFICHA"] = strNumeroFicha;
                dr["FIRE_VCODIGOLOCAL"] = strCodigoLocal;
                dr["FIRE_VCODIGOLOCALDESTINO"] = strCodigoLocalDestino;
                dr["FIRE_DFECHAESTADO"] = dFechaEstado;
                dr["FIRE_VOBSERVACION"] = strObservacion;
                dr["FIRE_VNUMEROGUIA"] = string.Empty;
                dr["FIRE_SESTADOID"] = intEstadoId;
                dr["ESTA_VDESCRIPCIONCORTA"] = strEstado;

                dtFichas.Rows.Add(dr);

                Session["FichaRegistral"] = dtFichas;
                GridViewFicha.DataSource = dtFichas;
                GridViewFicha.DataBind();

                //ctrlToolBarFicha.btnNuevo.Enabled = true;
                ctrlToolBarFicha.btnGrabar.Enabled = true;
                ctrlToolBarFicha.btnImprimir.Enabled = false;
                ctrlToolBarFicha.btnCancelar.Enabled = true;

                txtNroFicha.Text = "";
                txtObservacionFicha.Text = "";
                ddlEstadoFicha.SelectedIndex = 0;
                ddlLocalDestino.SelectedIndex = 0;
                //btnAgregarFicha.Visible = false;
                updFicha.Update();
            
            }

            
        }

        private bool validarFicha()
        {
            if (txtNroFicha.Text.Trim().Length == 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Ficha Registral", "Digite el Número de la Ficha Registral."));
                return false;
            }
            if (ctrFechaFicha.Text.Trim().Length == 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Ficha Registral", "Seleccione la fecha de registro."));
                return false;
            }

            if (Comun.EsFecha(ctrFechaFicha.Text.Trim()) == false)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Ficha Registral", Constantes.CONST_VALIDACION_FECHA_NO_VALIDA));                
                return false;
            }

            if (ddlLocalDestino.SelectedIndex == 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Ficha Registral", "Seleccione el Local de Destino."));
                return false;
            }

            string strNumeroFicha = "";
            bool bBuscarFichaGrilla = false;

            if (HFNumeroFichaRegistral.Value.Length > 0)
            {
                strNumeroFicha = HFNumeroFichaRegistral.Value.Trim();
            }

            if (strNumeroFicha.Length == 0)
            {
                strNumeroFicha = txtNroFicha.Text.Trim();
                bBuscarFichaGrilla = true;
            }
            else
            {
                strNumeroFicha = txtNroFicha.Text.Trim();
                if (HFNumeroFichaRegistral.Value.Trim() != strNumeroFicha)
                {  bBuscarFichaGrilla = true; }
            }
            if (bBuscarFichaGrilla == true)
            {
                if (ExisteFichaRegistral(strNumeroFicha))
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Ficha Registral", "El Número de la Ficha Registral ya existe en la lista."));
                    return false;
                }

                DataTable dtFichaRegistral = new DataTable();

                FichaRegistralBL objFichaRegistralBL = new FichaRegistralBL();
                int intTotalRecords = 0;
                int intTotalPages = 0;
                dtFichaRegistral = objFichaRegistralBL.Consultar(0, 0, strNumeroFicha, "", "", 0, 1, 1, ref intTotalRecords, ref intTotalPages);
                if (dtFichaRegistral.Rows.Count > 0)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Ficha Registral", "El Número de la Ficha Registral ya ha sido registrado."));
                    hRepetido.Value = "1";
                    txtNroFicha.Text = HFNumeroFichaRegistral.Value.Trim();
                    return false;
                }
                dtFichaRegistral.Dispose();
            }
            return true;
        }

        private bool validarGrillaFichas()
        {
            bool existeFicha = true;
            if (Session["FichaRegistral"] == null)
            {
                existeFicha = false;
            }
            else
            {
                DataTable dtFichas = new DataTable();

                dtFichas = (DataTable)Session["FichaRegistral"];

                if (dtFichas.Rows.Count == 0)
                {
                    dtFichas.Dispose();
                    existeFicha = false;
                }
            }
            if (existeFicha == false)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Ficha Registral", "Deberá registrar por lo menos una Ficha Registral."));
            }

            if (ctrFechaEnvio.Text.Length > 0)
            {
                if (Comun.EsFecha(ctrFechaEnvio.Text.Trim()) == false)
                {                    
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Ficha Registral", "La fecha de envió no es válida."));
                    existeFicha = false;
                }
            }

            return existeFicha;
        }

        private DataTable CrearDataTable()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("INUMERO", typeof(Int64));
            dt.Columns.Add("FIRE_IFICHAREGISTRALID", typeof(Int64));
            dt.Columns.Add("FIRE_IACTUACIONDETALLEID", typeof(Int64));
            dt.Columns.Add("FIRE_VNUMEROFICHA", typeof(String));
            dt.Columns.Add("FIRE_VCODIGOLOCAL", typeof(String));
            dt.Columns.Add("FIRE_VCODIGOLOCALDESTINO", typeof(String));
            dt.Columns.Add("FIRE_DFECHAESTADO", typeof(DateTime));
            dt.Columns.Add("FIRE_VOBSERVACION", typeof(String));
            dt.Columns.Add("FIRE_VNUMEROGUIA", typeof(String));
            dt.Columns.Add("FIRE_SESTADOID", typeof(Int16));
            dt.Columns.Add("ESTA_VDESCRIPCIONCORTA", typeof(String));

            dt.Columns.Add("para_vDescripcion", typeof(String));
            dt.Columns.Add("fire_dFechaEnvio", typeof(DateTime));
            dt.Columns.Add("fire_vNumeroOficio", typeof(String));                
            return dt;
        }

       
        private void AnularFichaRegistral(string strFichaRegistralId, string strNumeroFicha)
        {
            DataTable dtFichas = new DataTable();
            bool bExisteError = false;

            if (validarGrillaFichas())
            {                
                dtFichas = (DataTable)Session["FichaRegistral"];

                for (int i = 0; i < dtFichas.Rows.Count; i++)
                {
                    if (dtFichas.Rows[i]["FIRE_VNUMEROFICHA"].ToString().Equals(strNumeroFicha))
                    {
                        Int16 intEstadoId = ObtenerEstadoId("ANULADO");
                        long intFichaRegistralId = Convert.ToInt64(dtFichas.Rows[i]["FIRE_IFICHAREGISTRALID"].ToString());

                        dtFichas.Rows[i]["FIRE_SESTADOID"] = intEstadoId;
                        dtFichas.Rows[i]["ESTA_VDESCRIPCIONCORTA"] = "ANULADO";
                        //ctrlToolBarFicha.btnGrabar.Enabled = true;
                        //--------------------------------------------
                        FichaRegistralBL objFichaRegistralBL = new FichaRegistralBL();
                        SGAC.BE.MRE.RE_FICHAREGISTRAL objFichaBE = new BE.MRE.RE_FICHAREGISTRAL();
                        objFichaBE.fire_iFichaRegistralId = intFichaRegistralId;
                        objFichaBE.fire_sEstadoId = 0; //ANULADO --- SE AGREGA EL VALOR ANULADO POR BASE DE DATOS
                        objFichaBE.fire_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        objFichaBE.fire_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                        objFichaBE.OficinaConsultar  = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]); 

                        objFichaRegistralBL.Anular(objFichaBE);
                        if (objFichaRegistralBL.isError)
                        {
                            bExisteError = true;
                        }
                
                        //--------------------------------------------
                        break;
                    }
                }
            }

            if (!(bExisteError))
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "FICHA REGISTRAL", Constantes.CONST_MENSAJE_ELIMINADO, false, 190, 250));
                ctrlToolBarFicha.btnEliminar.Enabled = false;
                ctrlToolBarFicha.btnImprimir.Enabled = false;
                btnAgregarParticipante.Enabled = false;
                btnDocAdjuntosFicha.Enabled = false;
                Session["FichaRegistral"] = null;
                GridViewFicha.DataSource = null;
                GridViewFicha.DataBind();
                GridViewParticipante.DataSource = null;
                GridViewParticipante.DataBind();
                txtNroFicha.Text = "";
                txtObservaciones.Text = "";
                ctrFechaEnvio.Text = "";
                txtHojaRemOfi.Text = "";
                txtObservacionFicha.Text = "";
                updFicha.Update();
            }
            else
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "FICHA REGISTRAL", Constantes.CONST_MENSAJE_OPERACION_FALLIDA, false, 190, 250));
            }
        }

        private void EditarFichaRegistral(string strNumeroFicha)
        {
            DataTable dtFichas = new DataTable();

            dtFichas = (DataTable)Session["FichaRegistral"];

            if (dtFichas.Rows.Count > 0)
            {
                for (int i = 0; i < dtFichas.Rows.Count; i++)
                {
                    if (dtFichas.Rows[i]["FIRE_VNUMEROFICHA"].ToString().Equals(strNumeroFicha))
                    {
                        txtNroFicha.Text = strNumeroFicha;
                        HFNumeroFichaRegistral.Value = strNumeroFicha;
                        ctrFechaFicha.set_Value = Comun.FormatearFecha(dtFichas.Rows[i]["FIRE_DFECHAESTADO"].ToString());

                        string strValor = dtFichas.Rows[i]["ESTA_VDESCRIPCIONCORTA"].ToString().Trim();
                        hEstadoInicial.Value = strValor;

                        if (strValor == "REGISTRADO")
                        {
                            EstablecerSecuenciaEstadoFicha(ddlEstadoFicha, strValor);
                            ddlEstadoFicha.SelectedValue = ddlEstadoFicha.Items.FindByText(strValor).Value;
                        }
                        else {
                            strValor = dtFichas.Rows[i]["FIRE_SESTADOID"].ToString().Trim();
                            hEstadoInicial.Value = strValor;
                            EstablecerSecuenciaEstadoFicha(ddlEstadoFicha, strValor);
                            ddlEstadoFicha.SelectedValue = strValor;
                        }
                        
                        ddlLocalDestino.SelectedValue = dtFichas.Rows[i]["FIRE_VCODIGOLOCALDESTINO"].ToString().Trim();
                        txtObservacionFicha.Text = dtFichas.Rows[i]["FIRE_VOBSERVACION"].ToString().Trim().ToUpper();
                        hObservación.Value = dtFichas.Rows[i]["FIRE_VOBSERVACION"].ToString().Trim().ToUpper();
                        if (dtFichas.Rows[i]["para_vDescripcion"].ToString().Trim().ToUpper() != "REGISTRO MANUAL")
                        {
                            rbSemi.Checked = true;
                            rbManual.Checked = false;
                            if (dtFichas.Rows[i]["fire_dFechaEnvio"].ToString().Length > 0)
                            {
                                ctrFechaEnvio.set_Value = Comun.FormatearFecha(dtFichas.Rows[i]["fire_dFechaEnvio"].ToString());
                            }
                            else { ctrFechaEnvio.Text = ""; }
                            
                            txtHojaRemOfi.Text = dtFichas.Rows[i]["fire_vNumeroOficio"].ToString().Trim().ToUpper();
                            DivSemiAutomatico.Visible = true;
                        }
                        else {
                            rbSemi.Checked = false;
                            rbManual.Checked = true;
                            ctrFechaEnvio.Text = "";
                            txtHojaRemOfi.Text = "";
                            DivSemiAutomatico.Visible = false;
                        }

                        //ctrlToolBarFicha.btnNuevo.Enabled = false;
                        ctrlToolBarFicha.btnGrabar.Enabled = true;
                        //btnAceptarFicha.Visible = true;
                        //btnAgregarFicha.Visible = false;
                        
                        break;
                    }
                }
            }
        }

        private void ConsultarFichaRegistral(string strNumeroFicha)
        {
            DataTable dtFichas = new DataTable();

            dtFichas = (DataTable)Session["FichaRegistral"];

            if (dtFichas.Rows.Count > 0)
            {
                for (int i = 0; i < dtFichas.Rows.Count; i++)
                {
                    if (dtFichas.Rows[i]["FIRE_VNUMEROFICHA"].ToString().Equals(strNumeroFicha))
                    {
                        txtNroFicha.Text = strNumeroFicha;
                        HFNumeroFichaRegistral.Value = strNumeroFicha;
                        ctrFechaFicha.set_Value = Comun.FormatearFecha(dtFichas.Rows[i]["FIRE_DFECHAESTADO"].ToString());
                        string strValor = dtFichas.Rows[i]["FIRE_SESTADOID"].ToString().Trim();
                        hEstadoInicial.Value = strValor;
                        EstablecerSecuenciaEstadoFicha(ddlEstadoFicha, strValor);

                        ddlEstadoFicha.SelectedValue = strValor;
                        txtObservacionFicha.Text = dtFichas.Rows[i]["FIRE_VOBSERVACION"].ToString().Trim().ToUpper();
                        hObservación.Value = dtFichas.Rows[i]["FIRE_VOBSERVACION"].ToString().Trim().ToUpper();
                        string strCodigoLocalDestinoId = dtFichas.Rows[i]["FIRE_VCODIGOLOCALDESTINO"].ToString().Trim();
                        if (strCodigoLocalDestinoId.Length > 0)
                        {
                            ddlLocalDestino.SelectedValue = strCodigoLocalDestinoId;
                        }
                        else
                        {
                            ddlLocalDestino.SelectedIndex = 0;
                        }

                        //ctrlToolBarFicha.btnNuevo.Enabled = false;
                        ctrlToolBarFicha.btnGrabar.Enabled = true;
                        //btnAceptarFicha.Visible = false;
                        //btnAgregarFicha.Visible = false;
                        break;
                    }
                }
            }            
        }
       
        void ctrlToolBarFicha_btnNuevoHandler()
        {
            if (ddlLocalDestino.Items.Count > 0)
            {
                ddlLocalDestino.SelectedValue = Session[Constantes.CONST_SESION_OFICINACONSULAR_CODIGOLOCAL].ToString();
            }
            txtNroFicha.Text = "";
            ctrFechaFicha.set_Value = Comun.FormatearFecha((Accesorios.Comun.ObtenerFechaActualTexto(HttpContext.Current.Session)));
            txtObservacionFicha.Text = "";
            ddlEstadoFicha.SelectedIndex = 0;
            txtNroFicha.Focus();
            //btnAgregarFicha.Visible = true;
            //EstadoNormalBarraFichaRegistral(false);
            //---------------------------------------------
            // Objetivo: Quitar la visibilidad de adicionar participante, 
            //           editar participante o cancelar. 
            //---------------------------------------------
            //btnGrabarParticipante.Visible = false;
            //btnEditarParticipante.Visible = false;
            //btnCancelarParticipante.Visible = false;
            //---------------------------------------------       
            EstablecerSecuenciaEstadoFicha(ddlEstadoFicha, "RECUPERADO CON FICHA");
            Session["FichaRegistralParticipante"] = null;
            GridViewParticipante.DataSource = null;
            GridViewParticipante.DataBind();
            LimpiarDatosParticipantes();
            
            ctrlToolBarFicha.btnGrabar.Enabled = true;
            ctrlToolBarFicha.btnImprimir.Enabled = false;
            ctrlToolBarFicha.btnEliminar.Enabled = false;
            btnAgregarParticipante.Enabled = false;
            btnDocAdjuntosFicha.Enabled = false;
            hNuevo.Value = "1";

        }
        void ctrlToolBarFicha_btnEliminarHandler()
        {
            ctrlToolBarFicha.btnImprimir.Enabled = false;
            DataTable dtFichas = new DataTable();
            dtFichas = (DataTable)Session["FichaRegistral"];
            string strFichaRegistralId = "";
            string strNumeroFicha = "";
            if (dtFichas.Rows.Count > 1)
            {
                strFichaRegistralId = hFichaRegistralId.Value;
                strNumeroFicha = hNumeroFicha.Value;
            }
            else {
                 strFichaRegistralId = dtFichas.Rows[0]["FIRE_IFICHAREGISTRALID"].ToString();
                 strNumeroFicha = dtFichas.Rows[0]["FIRE_VNUMEROFICHA"].ToString();
            }
            
            AnularFichaRegistral(strFichaRegistralId, strNumeroFicha);
            cargarFichasRegistrales();
            if (ddlLocalDestino.Items.Count > 0)
            {
                ddlLocalDestino.SelectedValue = Session[Constantes.CONST_SESION_OFICINACONSULAR_CODIGOLOCAL].ToString();
            }
            EstablecerSecuenciaEstadoFicha(ddlEstadoFicha, "REGISTRADO");
        }
        void ctrlToolBarFicha_btnGrabarHandler()
        {
            if (rbSemi.Checked)
            {
                //if (ctrFechaEnvio.Text.Length == 0 || txtHojaRemOfi.Text.Length == 0)
                //{
                //    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "FICHA REGISTRAL", "INGRESE LOS DATOS DEL ENVIO", false, 190, 250));
                //    return;
                //}
            }
            hRepetido.Value = "";
            AgregarFicha();
            if (hRepetido.Value == "1")
            {
                return;
            }
            if (validarGrillaFichas())
            {
                string strNumFicha = "";
                bool registrado = false;
                DataTable dtFichas = new DataTable();
                dtFichas = (DataTable)Session["FichaRegistral"];
                
                BE.MRE.RE_FICHAREGISTRAL objFichaRegistralBE;

                ArrayList listaFichasInsertar = new ArrayList();
                ArrayList listaFichasActualizar = new ArrayList();

                ArrayList listaFichas = new ArrayList();

                long intFichaRegistralId = 0;

                #region Bucle de Fichas

                for (int i = 0; i < dtFichas.Rows.Count; i++)
                {
                    objFichaRegistralBE = new BE.MRE.RE_FICHAREGISTRAL();

                    if (hNuevo.Value == "1")
                    {
                        intFichaRegistralId = 0;
                    }
                    else {
                        intFichaRegistralId = Convert.ToInt64(dtFichas.Rows[i]["FIRE_IFICHAREGISTRALID"].ToString());
                    }

                    objFichaRegistralBE.fire_iFichaRegistralId = intFichaRegistralId;
                    objFichaRegistralBE.fire_iActuacionDetalleId = Convert.ToInt64(dtFichas.Rows[i]["FIRE_IACTUACIONDETALLEID"].ToString());
                    objFichaRegistralBE.fire_vNumeroFicha = dtFichas.Rows[i]["FIRE_VNUMEROFICHA"].ToString();
                    objFichaRegistralBE.fire_vCodigoLocal = dtFichas.Rows[i]["FIRE_VCODIGOLOCAL"].ToString();
                    objFichaRegistralBE.fire_vCodigoLocalDestino = dtFichas.Rows[i]["FIRE_VCODIGOLOCALDESTINO"].ToString();
                    objFichaRegistralBE.fire_dFechaEstado = Comun.FormatearFecha(dtFichas.Rows[i]["FIRE_DFECHAESTADO"].ToString());
                    objFichaRegistralBE.fire_vObservacion = dtFichas.Rows[i]["FIRE_VOBSERVACION"].ToString();
                    objFichaRegistralBE.fire_vNumeroGuia = dtFichas.Rows[i]["FIRE_VNUMEROGUIA"].ToString();
                    objFichaRegistralBE.fire_sEstadoId = Convert.ToInt16(dtFichas.Rows[i]["FIRE_SESTADOID"].ToString());
                    strNumFicha = dtFichas.Rows[i]["FIRE_VNUMEROFICHA"].ToString();
                    if (intFichaRegistralId == 0)
                    {
                        objFichaRegistralBE.fire_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        objFichaRegistralBE.fire_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    }
                    else
                    {
                        objFichaRegistralBE.fire_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        objFichaRegistralBE.fire_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    }

                    objFichaRegistralBE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                    if (rbManual.Checked)
                    {
                        objFichaRegistralBE.fire_vTipoRegistro = Constantes.CONST_REGISTRO_MANUAL;
                        objFichaRegistralBE.fire_vNumeroOficio = "";
                        objFichaRegistralBE.fire_dFechaEnvio = null;
                    }
                    else { objFichaRegistralBE.fire_vTipoRegistro = Constantes.CONST_REGISTRO_SEMIAUTOMATICO;
                    objFichaRegistralBE.fire_vNumeroOficio = txtHojaRemOfi.Text;
                    DateTime datFecha = new DateTime();
                    if (ctrFechaEnvio.Text.Length > 0)
                    {
                        if (!DateTime.TryParse(ctrFechaEnvio.Text, out datFecha))
                        {
                            datFecha = Comun.FormatearFecha(ctrFechaEnvio.Text);
                        }
                        objFichaRegistralBE.fire_dFechaEnvio = datFecha;
                    }
                    }
                    

                    listaFichas.Add(objFichaRegistralBE);
                }

                #endregion

                FichaRegistralBL objFichaRegistralBL = new FichaRegistralBL();

                objFichaRegistralBL.Grabar(listaFichas);

                if (objFichaRegistralBL.isError)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "FICHA REGISTRAL", Constantes.CONST_MENSAJE_OPERACION_FALLIDA, false, 190, 250));
                }
                else
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "FICHA REGISTRAL", Constantes.CONST_MENSAJE_EXITO, false, 190, 250));
                    //ctrlToolBarFicha.btnNuevo.Enabled = true;
                    txtNroFicha.Text = "";
                    txtObservacionFicha.Text = "";
                    ddlEstadoFicha.SelectedIndex = 0;
                    //ddlLocalDestino.SelectedIndex = 0;
                    //ctrlToolBarFicha.btnGrabar.Enabled = false;
                    //ctrlToolBarFicha.btnNuevo.Enabled = true;
                    //btnAgregarFicha.Visible = false;
                    //btnAceptarFicha.Visible = false;
                    HFNumeroFichaRegistral.Value = strNumFicha;
                    
                    if (ValidarTarifaMenorEdad())
                    {
                        lblTitular.Text = "¿" + lblDestino.Text;
                        registrado = true;
                    }
                }
                cargarFichasRegistrales();
                CargarPersonaTitularMayorEdad();
                CargarDatosEditar();

                if (registrado)
                {
                    bool bTitular = false;
                    if (Session["FichaRegistralParticipante"] == null)
                    {
                        //Comun.EjecutarScript(Page, "abrirPopupPregunta();");
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "abrirPopupPregunta();", true);
                    }
                    else {
                        
                        DataTable _dt = new DataTable();
                        _dt = (DataTable)Session["FichaRegistralParticipante"];
                        
                        for (int i = 0; i <= _dt.Rows.Count - 1; i++)
                        {
                            if (_dt.Rows[i]["VTIPO_PARTICIPANTE"].ToString() == "TITULAR")
                            {
                                bTitular = true;
                                break;
                            }
                        }
                        if (!bTitular)
                        { ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "abrirPopupPregunta();", true); }
                    }
                }    
            }
            //if (ddlLocalDestino.Items.Count > 0)
            //{
            //    ddlLocalDestino.SelectedValue = Session[Constantes.CONST_SESION_OFICINACONSULAR_CODIGOLOCAL].ToString();
            //}
        }

        //void ctrlToolBarFicha_btnCancelarHandler()
        //{            
        //    HFNumeroFichaRegistral.Value = "";
        //    ctrFechaFicha.set_Value = Comun.FormatearFecha((Accesorios.Comun.ObtenerFechaActualTexto(HttpContext.Current.Session)));
        //    txtNroFicha.Text = "";
        //    txtObservacionFicha.Text = "";
        //    ddlEstadoFicha.SelectedIndex = 0;
        //    ddlLocalDestino.SelectedIndex = 0;
        //    cargarFichasRegistrales();
        //    EstadoNormalBarraFichaRegistral(true);
            
        //    //btnAgregarFicha.Visible = false;
        //    //btnAceptarFicha.Visible = false;
        //    ctrlToolBarFicha.btnNuevo.Enabled = true;
        //    //ctrlToolBarFicha.btnGrabar.Enabled = false;
        //    ctrlToolBarFicha.btnImprimir.Enabled = false;
        //    //---------------------------------------------
        //    // Objetivo: Quitar la visibilidad de adicionar participante, 
        //    //           editar participante o cancelar. 
        //    //---------------------------------------------
        //    //btnGrabarParticipante.Visible = false;
        //    btnEditarParticipante.Visible = false;
        //    btnCancelarParticipante.Visible = false;            
        //    //---------------------------------------------
        //    EstablecerSecuenciaEstadoFicha(ddlEstadoFicha, "");
        //    Session["FichaRegistralParticipante"] = null;
        //    GridViewParticipante.DataSource = null;
        //    GridViewParticipante.DataBind();
        //    LimpiarDatosParticipantes();
            
        //}

        //----------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 21/12/2016
        // Objetivo: Imprimir la ficha registral
        //----------------------------------------
        void ctrlToolBarFicha_btnPrintHandler()
        {
            if (validarGrillaFichas())
            {
                if (Session["FichaRegistralParticipante"] == null)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Ficha Participantes", "Deberá registrar por lo menos una Participante"));
                    return;
                }

                FichaRegistralBL objFichaRegistralBL = new FichaRegistralBL();
                DataTable dtFichaRegistral = new DataTable();
                DataTable dtDocFichaRegistral = new DataTable();
                ObtenerDatosGrilla();
                long iFichaRegistralId = Convert.ToInt64(HFFichaRegistralId.Value);
                int iTipoParticipanteId = 0;
                string strCodigoLocal = comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "vCodigoLocal").ToString(); 
                string strTipoDocTitular = "";
                string strNroDocTitular = "";
                string strFechaRegistro_DD = "";
                string strFechaRegistro_MM = "";
                string strFechaRegistro_YYYY = "";
                string strApPaternoTitular = "";
                string strApMaternoTitular = "";
                string strNombresTitular = "";
                string strDirDptoTitular = "";
                string strDirCodDptoTitular = "";
                string strDirProvTitular = "";
                string strDirCodProvTitular = "";
                string strDirDistTitular = "";
                string strDirCodDistTitular = "";
                string strDireccionTitular = "";
                string strCodigoPostalResidencia = "";
                string strSenasParticularesTitular = "";
                string strFecNacTitular_DD = "";
                string strFecNacTitular_MM = "";
                string strFecNacTitular_YYYY = "";
                string strNacDptoTitular = "";
                string strNacCodDptoTitular = "";
                string strNacProvTitular = "";
                string strNacCodProvTitular = "";
                string strNacDistTitular = "";
                string strNacCodDistTitular = "";
                string strApPaternoPadre = "";
                string strApMaternoPadre = "";
                string strNombresPadre = "";
                string strApPaternoMadre = "";
                string strApMaternoMadre = "";
                string strNombresMadre = "";
                string strApPaternoConyuge = "";
                string strApMaternoConyuge = "";
                string strNombresConyuge = "";
                string strApPaternoDeclarante = "";
                string strApMaternoDeclarante = "";
                string strNombresDeclarante = "";
                string strTelefonoTitular = "";
                string strCodigoLocalDestino = "";
                string strApeCasadaTitular = "";
                string strCorreoElectronico = "";
                /*Nuevos Campos*/

                string strEstadoCivil = "";
                string strGENERO = "";
                string strGRADO_INSTRUCCION = "";
                string strEstaturaMetros = "";
                string strEstaturaCentimetros = "";
                string strANIO = "";
                string strEstudioCompleto = "";
                string strDiscapacidad = "";
                string strInterdicto = "";
                string strNombreCurador = "";
                string strDonaOrganos = "";
                string strTIPO_DECLARANTE = "";
                string strTIPO_TUTOR = "";

                //--------------------------------------------------
                //Fecha: 03/03/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Adicionar nuevos campos.
                //---------------------------------------------------
                string strTipoDocPadre = "";
                string strNroDocPadre = "";
                string strTipoDocMadre = "";
                string strNroDocMadre = "";
                string strTipoDocConyuge = "";
                string strNroDocConyuge = "";
                string strTipoDocDeclarante = "";
                string strNroDocDeclarante = "";
                //---------------------------------------------------

                dtFichaRegistral = objFichaRegistralBL.Reporte(iFichaRegistralId);
                dtDocFichaRegistral = objFichaRegistralBL.ObtenerDocumentosFichaRegistral(iFichaRegistralId);
                #region DatosFichaRegistral
                if (dtFichaRegistral.Rows.Count > 0)
                {
                    if (EsMayorEdad())
                    {
                        for (int i = 0; i < dtFichaRegistral.Rows.Count; i++)
                        {
                            iTipoParticipanteId = Convert.ToInt32(dtFichaRegistral.Rows[i]["STIPOPARTICIPANTEID"].ToString());
                            strFechaRegistro_DD = dtFichaRegistral.Rows[i]["FECHAREGISTRO_DD"].ToString();
                            strFechaRegistro_MM = dtFichaRegistral.Rows[i]["FECHAREGISTRO_MM"].ToString();
                            strFechaRegistro_YYYY = dtFichaRegistral.Rows[i]["FECHAREGISTRO_YYYY"].ToString();
                            strCodigoLocalDestino = dtFichaRegistral.Rows[i]["CODIGO_LOCAL_DESTINO"].ToString();

                            if (iTipoParticipanteId == (int)Enumerador.enmFichaTipoParticipanteMayor.TITULAR)
                            {
                                strTipoDocTitular = dtFichaRegistral.Rows[i]["VTIPO_DOCUMENTO"].ToString();
                                strNroDocTitular = dtFichaRegistral.Rows[i]["VNUMERO_DOCUMENTO"].ToString();
                                strApPaternoTitular = dtFichaRegistral.Rows[i]["VAPELLIDO_PATERNO"].ToString();
                                strApMaternoTitular = dtFichaRegistral.Rows[i]["VAPELLIDO_MATERNO"].ToString();
                                strApeCasadaTitular = dtFichaRegistral.Rows[i]["VAPELLIDOCASADA"].ToString();
                                strNombresTitular = dtFichaRegistral.Rows[i]["VNOMBRES"].ToString();
                                if (chkModLugDomicilio.Checked)
                                {
                                    strDirDptoTitular = dtFichaRegistral.Rows[i]["DES_DIR_DPTO"].ToString();
                                    strDirCodDptoTitular = dtFichaRegistral.Rows[i]["COD_DIR_DPTO"].ToString();
                                    strDirProvTitular = dtFichaRegistral.Rows[i]["DES_DIR_PROV"].ToString();
                                    strDirCodProvTitular = dtFichaRegistral.Rows[i]["COD_DIR_PROV"].ToString();
                                    strDirDistTitular = dtFichaRegistral.Rows[i]["DES_DIR_DIST"].ToString();
                                    strDirCodDistTitular = dtFichaRegistral.Rows[i]["COD_DIR_DIST"].ToString();
                                }
                                if (chkModDireccion.Checked)
                                {
                                   strDireccionTitular = dtFichaRegistral.Rows[i]["DIRECCION"].ToString();
                                   strCodigoPostalResidencia = dtFichaRegistral.Rows[i]["COD_POSTAL_RESIDENCIA"].ToString();
                                }
                                if (chkModFecNac.Checked)
                                {
                                    strFecNacTitular_DD = dtFichaRegistral.Rows[i]["FECHANACIMIENTO_DD"].ToString();
                                    strFecNacTitular_MM = dtFichaRegistral.Rows[i]["FECHANACIMIENTO_MM"].ToString();
                                    strFecNacTitular_YYYY = dtFichaRegistral.Rows[i]["FECHANACIMIENTO_YYYY"].ToString();
                                }
                                if (chkModLugNac.Checked)
                                {
                                    strNacDptoTitular = dtFichaRegistral.Rows[i]["DES_NAC_DPTO"].ToString();
                                    strNacCodDptoTitular = dtFichaRegistral.Rows[i]["COD_NAC_DPTO"].ToString();
                                    strNacProvTitular = dtFichaRegistral.Rows[i]["DES_NAC_PROV"].ToString();
                                    strNacCodProvTitular = dtFichaRegistral.Rows[i]["COD_NAC_PROV"].ToString();
                                    strNacDistTitular = dtFichaRegistral.Rows[i]["DES_NAC_DIST"].ToString();
                                    strNacCodDistTitular = dtFichaRegistral.Rows[i]["COD_NAC_DIST"].ToString();
                                }
                                if (chkModTelefono.Checked)
                                {
                                    strTelefonoTitular = dtFichaRegistral.Rows[i]["TELEFONO_DIR"].ToString();
                                }
                                if (chkModEmail.Checked)
                                {
                                    strCorreoElectronico = dtFichaRegistral.Rows[i]["VCORREO_ELECTRONICO"].ToString();
                                }
                                if (chkModObservacion.Checked)
                                {
                                    strSenasParticularesTitular = dtFichaRegistral.Rows[i]["VSENASPARTICULARES"].ToString();
                                }
                                if (chkModEstCivil.Checked)
                                {
                                    strEstadoCivil = dtFichaRegistral.Rows[i]["ESTADO_CIVIL"].ToString();
                                }
                                /*Nuevos campos*/
                                if (chkModGenero.Checked)
                                {
                                    strGENERO = dtFichaRegistral.Rows[i]["GENERO"].ToString();
                                }
                                if (chkModGradoInstr.Checked)
                                {
                                    strGRADO_INSTRUCCION = dtFichaRegistral.Rows[i]["GRADO_INSTRUCCION"].ToString();
                                    strANIO = dtFichaRegistral.Rows[i]["ANIO"].ToString();
                                    strEstudioCompleto = dtFichaRegistral.Rows[i]["cEstudioCompleto"].ToString();
                                }
                                if (chkModEstatura.Checked)
                                {
                                    strEstaturaMetros = dtFichaRegistral.Rows[i]["estaturaMetros"].ToString();
                                    strEstaturaCentimetros = dtFichaRegistral.Rows[i]["estaturaCentimetros"].ToString();
                                }
                                if (chkModDiscapacidad.Checked)
                                {
                                    strDiscapacidad = dtFichaRegistral.Rows[i]["cDiscapacidad"].ToString();
                                }
                                if (chkModInterdiccion.Checked)
                                {
                                    strInterdicto = dtFichaRegistral.Rows[i]["cInterdicto"].ToString();
                                    strNombreCurador = dtFichaRegistral.Rows[i]["vNombreCurador"].ToString();
                                }
                                if (chkDonaOrganos.Checked)
                                {
                                    strDonaOrganos = dtFichaRegistral.Rows[i]["cDonaOrganos"].ToString();
                                }                                
                            }
                            if (iTipoParticipanteId == (int)Enumerador.enmFichaTipoParticipanteMayor.PADRE)
                            {
                                //--------------------------------------------------
                                //Fecha: 03/03/2021
                                //Autor: Miguel Márquez Beltrán
                                //Motivo: Adicionar nuevos campos.
                                //---------------------------------------------------
                                strTipoDocPadre = dtFichaRegistral.Rows[i]["VTIPO_DOCUMENTO"].ToString();
                                strNroDocPadre = dtFichaRegistral.Rows[i]["VNUMERO_DOCUMENTO"].ToString();
                                //---------------------------------------------------
                                strApPaternoPadre = dtFichaRegistral.Rows[i]["VAPELLIDO_PATERNO"].ToString();
                                strApMaternoPadre = dtFichaRegistral.Rows[i]["VAPELLIDO_MATERNO"].ToString();
                                strNombresPadre = dtFichaRegistral.Rows[i]["VNOMBRES"].ToString();
                            }
                            if (iTipoParticipanteId == (int)Enumerador.enmFichaTipoParticipanteMayor.MADRE)
                            {
                                //--------------------------------------------------
                                //Fecha: 03/03/2021
                                //Autor: Miguel Márquez Beltrán
                                //Motivo: Adicionar nuevos campos.
                                //---------------------------------------------------
                                strTipoDocMadre = dtFichaRegistral.Rows[i]["VTIPO_DOCUMENTO"].ToString();
                                strNroDocMadre = dtFichaRegistral.Rows[i]["VNUMERO_DOCUMENTO"].ToString();
                                //---------------------------------------------------

                                strApPaternoMadre = dtFichaRegistral.Rows[i]["VAPELLIDO_PATERNO"].ToString();
                                strApMaternoMadre = dtFichaRegistral.Rows[i]["VAPELLIDO_MATERNO"].ToString();
                                strNombresMadre = dtFichaRegistral.Rows[i]["VNOMBRES"].ToString();
                            }
                            if (iTipoParticipanteId == (int)Enumerador.enmFichaTipoParticipanteMayor.CONYUGE)
                            {
                                //--------------------------------------------------
                                //Fecha: 03/03/2021
                                //Autor: Miguel Márquez Beltrán
                                //Motivo: Adicionar nuevos campos.
                                //---------------------------------------------------
                                strTipoDocConyuge = dtFichaRegistral.Rows[i]["VTIPO_DOCUMENTO"].ToString();
                                strNroDocConyuge = dtFichaRegistral.Rows[i]["VNUMERO_DOCUMENTO"].ToString();
                                //---------------------------------------------------
                                strApPaternoConyuge = dtFichaRegistral.Rows[i]["VAPELLIDO_PATERNO"].ToString();
                                strApMaternoConyuge = dtFichaRegistral.Rows[i]["VAPELLIDO_MATERNO"].ToString();
                                strNombresConyuge = dtFichaRegistral.Rows[i]["VNOMBRES"].ToString();
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dtFichaRegistral.Rows.Count; i++)
                        {
                            iTipoParticipanteId = Convert.ToInt32(dtFichaRegistral.Rows[i]["STIPOPARTICIPANTEID"].ToString());
                            strFechaRegistro_DD = dtFichaRegistral.Rows[i]["FECHAREGISTRO_DD"].ToString();
                            strFechaRegistro_MM = dtFichaRegistral.Rows[i]["FECHAREGISTRO_MM"].ToString();
                            strFechaRegistro_YYYY = dtFichaRegistral.Rows[i]["FECHAREGISTRO_YYYY"].ToString();
                            strCodigoLocalDestino = dtFichaRegistral.Rows[i]["CODIGO_LOCAL_DESTINO"].ToString();

                            if (iTipoParticipanteId == (int)Enumerador.enmFichaTipoParticipanteMenor.TITULAR)
                            {
                                strTipoDocTitular = dtFichaRegistral.Rows[i]["VTIPO_DOCUMENTO"].ToString();
                                strNroDocTitular = dtFichaRegistral.Rows[i]["VNUMERO_DOCUMENTO"].ToString();
                                strApPaternoTitular = dtFichaRegistral.Rows[i]["VAPELLIDO_PATERNO"].ToString();
                                strApMaternoTitular = dtFichaRegistral.Rows[i]["VAPELLIDO_MATERNO"].ToString();
                                strNombresTitular = dtFichaRegistral.Rows[i]["VNOMBRES"].ToString();

                                if (chkModLugDomicilio.Checked)
                                {
                                    strDirDptoTitular = dtFichaRegistral.Rows[i]["DES_DIR_DPTO"].ToString();
                                    strDirCodDptoTitular = dtFichaRegistral.Rows[i]["COD_DIR_DPTO"].ToString();
                                    strDirProvTitular = dtFichaRegistral.Rows[i]["DES_DIR_PROV"].ToString();
                                    strDirCodProvTitular = dtFichaRegistral.Rows[i]["COD_DIR_PROV"].ToString();
                                    strDirDistTitular = dtFichaRegistral.Rows[i]["DES_DIR_DIST"].ToString();
                                    strDirCodDistTitular = dtFichaRegistral.Rows[i]["COD_DIR_DIST"].ToString();
                                }
                                if (chkModDireccion.Checked)
                                {
                                    strDireccionTitular = dtFichaRegistral.Rows[i]["DIRECCION"].ToString();
                                    strCodigoPostalResidencia = dtFichaRegistral.Rows[i]["COD_POSTAL_RESIDENCIA"].ToString();
                                }
                                if (chkModFecNac.Checked)
                                {
                                    strFecNacTitular_DD = dtFichaRegistral.Rows[i]["FECHANACIMIENTO_DD"].ToString();
                                    strFecNacTitular_MM = dtFichaRegistral.Rows[i]["FECHANACIMIENTO_MM"].ToString();
                                    strFecNacTitular_YYYY = dtFichaRegistral.Rows[i]["FECHANACIMIENTO_YYYY"].ToString();
                                }
                                if (chkModLugNac.Checked)
                                {
                                    strNacDptoTitular = dtFichaRegistral.Rows[i]["DES_NAC_DPTO"].ToString();
                                    strNacCodDptoTitular = dtFichaRegistral.Rows[i]["COD_NAC_DPTO"].ToString();
                                    strNacProvTitular = dtFichaRegistral.Rows[i]["DES_NAC_PROV"].ToString();
                                    strNacCodProvTitular = dtFichaRegistral.Rows[i]["COD_NAC_PROV"].ToString();
                                    strNacDistTitular = dtFichaRegistral.Rows[i]["DES_NAC_DIST"].ToString();
                                    strNacCodDistTitular = dtFichaRegistral.Rows[i]["COD_NAC_DIST"].ToString();
                                }
                                if (chkModTelefono.Checked)
                                {
                                    strTelefonoTitular = dtFichaRegistral.Rows[i]["TELEFONO_DIR"].ToString();
                                }
                                if (chkModEmail.Checked)
                                {
                                    strCorreoElectronico = dtFichaRegistral.Rows[i]["VCORREO_ELECTRONICO"].ToString();
                                }
                                if (chkModObservacion.Checked)
                                {
                                    strSenasParticularesTitular = dtFichaRegistral.Rows[i]["VSENASPARTICULARES"].ToString();
                                }

                                /*Nuevos campos*/
                                if (chkModEstCivil.Checked)
                                {
                                    strEstadoCivil = dtFichaRegistral.Rows[i]["ESTADO_CIVIL"].ToString();
                                }
                                if (chkModGenero.Checked)
                                {
                                    strGENERO = dtFichaRegistral.Rows[i]["GENERO"].ToString();
                                }
                                if (chkModGradoInstr.Checked)
                                {
                                    strGRADO_INSTRUCCION = dtFichaRegistral.Rows[i]["GRADO_INSTRUCCION"].ToString();
                                    strANIO = dtFichaRegistral.Rows[i]["ANIO"].ToString();
                                    strEstudioCompleto = dtFichaRegistral.Rows[i]["cEstudioCompleto"].ToString();
                                }
                                if (chkModEstatura.Checked)
                                {
                                    strEstaturaMetros = dtFichaRegistral.Rows[i]["estaturaMetros"].ToString();
                                    strEstaturaCentimetros = dtFichaRegistral.Rows[i]["estaturaCentimetros"].ToString();
                                }
                                if (chkModDiscapacidad.Checked)
                                {
                                    strDiscapacidad = dtFichaRegistral.Rows[i]["cDiscapacidad"].ToString();
                                }
                                if (chkModInterdiccion.Checked)
                                {
                                    strInterdicto = dtFichaRegistral.Rows[i]["cInterdicto"].ToString();
                                    strNombreCurador = dtFichaRegistral.Rows[i]["vNombreCurador"].ToString();
                                }
                                if (chkDonaOrganos.Checked)
                                {
                                    strDonaOrganos = dtFichaRegistral.Rows[i]["cDonaOrganos"].ToString();
                                }
                                
                                strTIPO_DECLARANTE = dtFichaRegistral.Rows[i]["TIPO_DECLARANTE"].ToString();
                                strTIPO_TUTOR = dtFichaRegistral.Rows[i]["TIPO_TUTOR"].ToString();
                            }
                            if (iTipoParticipanteId == (int)Enumerador.enmFichaTipoParticipanteMenor.PADRE)
                            {
                                //--------------------------------------------------
                                //Fecha: 03/03/2021
                                //Autor: Miguel Márquez Beltrán
                                //Motivo: Adicionar nuevos campos.
                                //---------------------------------------------------
                                strTipoDocPadre = dtFichaRegistral.Rows[i]["VTIPO_DOCUMENTO"].ToString();
                                strNroDocPadre = dtFichaRegistral.Rows[i]["VNUMERO_DOCUMENTO"].ToString();
                                //---------------------------------------------------

                                strApPaternoPadre = dtFichaRegistral.Rows[i]["VAPELLIDO_PATERNO"].ToString();
                                strApMaternoPadre = dtFichaRegistral.Rows[i]["VAPELLIDO_MATERNO"].ToString();
                                strNombresPadre = dtFichaRegistral.Rows[i]["VNOMBRES"].ToString();
                            }
                            if (iTipoParticipanteId == (int)Enumerador.enmFichaTipoParticipanteMenor.MADRE)
                            {
                                //--------------------------------------------------
                                //Fecha: 03/03/2021
                                //Autor: Miguel Márquez Beltrán
                                //Motivo: Adicionar nuevos campos.
                                //---------------------------------------------------
                                strTipoDocMadre = dtFichaRegistral.Rows[i]["VTIPO_DOCUMENTO"].ToString();
                                strNroDocMadre = dtFichaRegistral.Rows[i]["VNUMERO_DOCUMENTO"].ToString();
                                //---------------------------------------------------

                                strApPaternoMadre = dtFichaRegistral.Rows[i]["VAPELLIDO_PATERNO"].ToString();
                                strApMaternoMadre = dtFichaRegistral.Rows[i]["VAPELLIDO_MATERNO"].ToString();
                                strNombresMadre = dtFichaRegistral.Rows[i]["VNOMBRES"].ToString();
                            }
                            if (iTipoParticipanteId == (int)Enumerador.enmFichaTipoParticipanteMenor.DECLARANTE)
                            {
                                //--------------------------------------------------
                                //Fecha: 03/03/2021
                                //Autor: Miguel Márquez Beltrán
                                //Motivo: Adicionar nuevos campos.
                                //---------------------------------------------------
                                strTipoDocDeclarante = dtFichaRegistral.Rows[i]["VTIPO_DOCUMENTO"].ToString();
                                strNroDocDeclarante = dtFichaRegistral.Rows[i]["VNUMERO_DOCUMENTO"].ToString();
                                //---------------------------------------------------

                                strApPaternoDeclarante = dtFichaRegistral.Rows[i]["VAPELLIDO_PATERNO"].ToString();
                                strApMaternoDeclarante = dtFichaRegistral.Rows[i]["VAPELLIDO_MATERNO"].ToString();
                                strNombresDeclarante = dtFichaRegistral.Rows[i]["VNOMBRES"].ToString();
                                strTIPO_DECLARANTE = dtFichaRegistral.Rows[i]["TIPO_DECLARANTE"].ToString();
                                strTIPO_TUTOR = dtFichaRegistral.Rows[i]["TIPO_TUTOR"].ToString();
                            }
                        }
                    }
                }
                #endregion

                SGAC.BE.MRE.Custom.CBE_FICHAREGISTRAL objFichaRegistralBE = new BE.MRE.Custom.CBE_FICHAREGISTRAL();


                objFichaRegistralBE.strCodigoLocal = strCodigoLocal;
                objFichaRegistralBE.strTipoDocTitular = strTipoDocTitular;
                objFichaRegistralBE.strNroDocTitular = strNroDocTitular;
                objFichaRegistralBE.strFechaRegistro_DD = strFechaRegistro_DD;
                objFichaRegistralBE.strFechaRegistro_MM = strFechaRegistro_MM;
                objFichaRegistralBE.strFechaRegistro_YYYY = strFechaRegistro_YYYY;
                objFichaRegistralBE.strApPaternoTitular = strApPaternoTitular;
                objFichaRegistralBE.strApMaternoTitular = strApMaternoTitular;
                objFichaRegistralBE.strNombresTitular = strNombresTitular;
                objFichaRegistralBE.strDirDptoTitular = strDirDptoTitular;
                objFichaRegistralBE.strDirCodDptoTitular = strDirCodDptoTitular;
                objFichaRegistralBE.strDirProvTitular = strDirProvTitular;
                objFichaRegistralBE.strDirCodProvTitular = strDirCodProvTitular;
                objFichaRegistralBE.strDirDistTitular = strDirDistTitular;
                objFichaRegistralBE.strDirCodDistTitular = strDirCodDistTitular;
                objFichaRegistralBE.strDireccionTitular = strDireccionTitular;
                objFichaRegistralBE.strCodigoPostalResidencia = strCodigoPostalResidencia;
                objFichaRegistralBE.strSenasParticularesTitular = strSenasParticularesTitular;
                objFichaRegistralBE.strFecNacTitular_DD = strFecNacTitular_DD;
                objFichaRegistralBE.strFecNacTitular_MM = strFecNacTitular_MM;
                objFichaRegistralBE.strFecNacTitular_YYYY = strFecNacTitular_YYYY;
                objFichaRegistralBE.strNacDptoTitular = strNacDptoTitular;
                objFichaRegistralBE.strNacCodDptoTitular = strNacCodDptoTitular;
                objFichaRegistralBE.strNacProvTitular = strNacProvTitular;
                objFichaRegistralBE.strNacCodProvTitular = strNacCodProvTitular;
                objFichaRegistralBE.strNacDistTitular = strNacDistTitular;
                objFichaRegistralBE.strNacCodDistTitular = strNacCodDistTitular;

                //--------------------------------------------------
                //Fecha: 03/03/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Adicionar nuevos campos.
                //---------------------------------------------------
                objFichaRegistralBE.strTipoDocPadre = strTipoDocPadre;
                objFichaRegistralBE.strNroDocPadre = strNroDocPadre;
                objFichaRegistralBE.strTipoDocMadre = strTipoDocMadre;
                objFichaRegistralBE.strNroDocMadre = strNroDocMadre;
                objFichaRegistralBE.strTipoDocConyuge = strTipoDocConyuge;
                objFichaRegistralBE.strNroDocConyuge = strNroDocConyuge;
                objFichaRegistralBE.strTipoDocDeclarante = strTipoDocDeclarante;
                objFichaRegistralBE.strNroDocDeclarante = strNroDocDeclarante;
                //---------------------------------------------------
                objFichaRegistralBE.strApPaternoPadre = strApPaternoPadre;
                objFichaRegistralBE.strApMaternoPadre = strApMaternoPadre;
                objFichaRegistralBE.strNombresPadre = strNombresPadre;
                objFichaRegistralBE.strApPaternoMadre = strApPaternoMadre;
                objFichaRegistralBE.strApMaternoMadre = strApMaternoMadre;
                objFichaRegistralBE.strNombresMadre = strNombresMadre;
                objFichaRegistralBE.strApPaternoConyuge = strApPaternoConyuge;
                objFichaRegistralBE.strApMaternoConyuge = strApMaternoConyuge;
                objFichaRegistralBE.strNombresConyuge = strNombresConyuge;
                objFichaRegistralBE.strApPaternoDeclarante = strApPaternoDeclarante;
                objFichaRegistralBE.strApMaternoDeclarante = strApMaternoDeclarante;
                objFichaRegistralBE.strNombresDeclarante = strNombresDeclarante;
                objFichaRegistralBE.strTelefonoTitular = strTelefonoTitular;
                objFichaRegistralBE.strCodigoLocalDestino = strCodigoLocalDestino;
                objFichaRegistralBE.strApCasadaTitular = strApeCasadaTitular;
                objFichaRegistralBE.strCorreoElectronicoTitular = strCorreoElectronico;
                /*nuevos campos*/
                objFichaRegistralBE.strEstadoCivil =strEstadoCivil;
                objFichaRegistralBE.strGENERO =strGENERO;
                objFichaRegistralBE.strGRADO_INSTRUCCION=strGRADO_INSTRUCCION;
                objFichaRegistralBE.strEstaturaMetros=strEstaturaMetros;
                objFichaRegistralBE.strEstaturaCentimetros=strEstaturaCentimetros;
                objFichaRegistralBE.strANIO=strANIO;
                objFichaRegistralBE.strEstudioCompleto=strEstudioCompleto;
                objFichaRegistralBE.strDiscapacidad=strDiscapacidad;
                objFichaRegistralBE.strInterdicto=strInterdicto;
                objFichaRegistralBE.strNombreCurador=strNombreCurador;
                objFichaRegistralBE.strDonaOrganos=strDonaOrganos;
                objFichaRegistralBE.strTIPO_DECLARANTE=strTIPO_DECLARANTE;
                objFichaRegistralBE.strTIPO_TUTOR = strTIPO_TUTOR;

                Boolean bol = false;

                if (EsMayorEdad())
                {
                    bol = ImprimirFichaRegistral(objFichaRegistralBE, "FichaRegistralMayorEdad", true, dtDocFichaRegistral);
                }
                else
                {
                    bol = ImprimirFichaRegistral(objFichaRegistralBE, "FichaRegistralMenorEdad", false, dtDocFichaRegistral);
                }
                

                if (bol)
                {
                     Response.Redirect("../Accesorios/VisPDF.aspx", false);
                }
                else
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "FICHA REGISTRAL", "Ocurrió un problema al generarse la vista previa. Favor de volver a generar."));
                }
            }
        }
        private bool ValidarTarifaMenorEdad()
        {
            bool resultado = false;
            string strTarifasMenorEdad = ConfigurationManager.AppSettings.Get("TarifaParticipanteFichaRegistralMenorEdad");
            string[] TarifasMenorEdad;

            TarifasMenorEdad = strTarifasMenorEdad.Split(',');
            foreach (string tarifa in TarifasMenorEdad)
            {
                if (txtIdTarifa.Text == tarifa)
                {
                    resultado = true;
                    break;
                }
            }
            return resultado;
        }

        private void CargarPersonaTitularMenorEdad()
        {
            bool resultado = false;
            string strTarifasMenorEdad = ConfigurationManager.AppSettings.Get("TarifaParticipanteFichaRegistralMenorEdad");
            string[] TarifasMenorEdad;

            TarifasMenorEdad = strTarifasMenorEdad.Split(',');
            foreach (string tarifa in TarifasMenorEdad)
            {
                if (txtIdTarifa.Text == tarifa)
                {
                    resultado = true;
                    break;
                }
            }
            if (resultado)
            {
                ObtenerDatosGrilla();
                bool bTitular = false;
                if (Session["FichaRegistralParticipante"] != null)
                {
                    DataTable _dt = new DataTable();
                    _dt = (DataTable)Session["FichaRegistralParticipante"];

                    for (int i = 0; i <= _dt.Rows.Count - 1; i++)
                    {
                        if (_dt.Rows[i]["VTIPO_PARTICIPANTE"].ToString() == "TITULAR")
                        {
                            bTitular = true;
                            break;
                        }
                    }
                    if (bTitular)
                    { return; }
                }

                BE.MRE.RE_FICHAREGISTRALPARTICIPANTE objFichaParticipanteBE = new BE.MRE.RE_FICHAREGISTRALPARTICIPANTE();
                FichaRegistralParticipanteBL objFichaParticipanteBL = new FichaRegistralParticipanteBL();

                long iPersonaId = 0;

                //if (HFGUID.Value.Length > 0)
                //{
                //    iPersonaId = Convert.ToInt64(Vie["iPersonaId" + HFGUID.Value].ToString());
                //}
                //else
                //{
                    iPersonaId = Convert.ToInt64(ViewState["iPersonaId"].ToString());
                //}
                


                int iTipoParticipanteId = Convert.ToInt16(Enumerador.enmFichaTipoParticipanteMenor.TITULAR);
                long iFichaRegistralId = Convert.ToInt64(HFFichaRegistralId.Value);

                objFichaParticipanteBE.fipa_iFichaRegistralId = iFichaRegistralId;
                objFichaParticipanteBE.fipa_sTipoParticipanteId = iTipoParticipanteId;
                objFichaParticipanteBE.fipa_iPersonaId = iPersonaId;
                objFichaParticipanteBE.fipa_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objFichaParticipanteBE.fipa_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                objFichaParticipanteBE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                //objFichaParticipanteBE.PERSONA = objPersonaBE;

                objFichaParticipanteBL.AgregarParticipante(ref objFichaParticipanteBE);

                if (!(objFichaParticipanteBE.Error))
                {
                    //Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Ficha Registral - Participante", Constantes.CONST_MENSAJE_EXITO));
                    //--------------------------------------------------
                    ConsultarParticipantesFichaRegistral(iFichaRegistralId);
                    LimpiarDatosParticipantes();
                }
                else
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Ficha Registral - Participante", Constantes.CONST_MENSAJE_OPERACION_FALLIDA));
                }
            }
            
        }

        private void CargarPersonaTitularMayorEdad()
        {
            bool resultado = false;
            string strTarifasMayorEdad = ConfigurationManager.AppSettings.Get("TarifaParticipanteFichaRegistralMayorEdad");
            string[] TarifasMayorEdad;

            TarifasMayorEdad = strTarifasMayorEdad.Split(',');
            foreach (string tarifa in TarifasMayorEdad)
            {
                if (txtIdTarifa.Text ==tarifa)
                {
                    resultado = true;
                    break;
                }
            }

            if (resultado)
            {
                ObtenerDatosGrilla();
                
                if (Session["FichaRegistralParticipante"] != null)
                {
                    return;
                }


                BE.MRE.RE_FICHAREGISTRALPARTICIPANTE objFichaParticipanteBE = new BE.MRE.RE_FICHAREGISTRALPARTICIPANTE();
                FichaRegistralParticipanteBL objFichaParticipanteBL = new FichaRegistralParticipanteBL();

                long iPersonaId = 0;

                //if (HFGUID.Value.Length > 0)
                //{
                //    iPersonaId = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value].ToString());
                //}
                //else
                //{
                    iPersonaId = Convert.ToInt64(ViewState["iPersonaId"].ToString());
                //}
                                

                int iTipoParticipanteId = Convert.ToInt16(Enumerador.enmFichaTipoParticipanteMayor.TITULAR);
                long iFichaRegistralId = Convert.ToInt64(HFFichaRegistralId.Value);

                objFichaParticipanteBE.fipa_iFichaRegistralId = iFichaRegistralId;
                objFichaParticipanteBE.fipa_sTipoParticipanteId = iTipoParticipanteId;
                objFichaParticipanteBE.fipa_iPersonaId = iPersonaId;
                objFichaParticipanteBE.fipa_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objFichaParticipanteBE.fipa_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                objFichaParticipanteBE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                //objFichaParticipanteBE.PERSONA = objPersonaBE;

                objFichaParticipanteBL.AgregarParticipante(ref objFichaParticipanteBE);

                if (!(objFichaParticipanteBE.Error))
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Ficha Registral - Participante", Constantes.CONST_MENSAJE_EXITO));
                    //--------------------------------------------------
                    ConsultarParticipantesFichaRegistral(iFichaRegistralId);
                    LimpiarDatosParticipantes();
                }
                else
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Ficha Registral - Participante", Constantes.CONST_MENSAJE_OPERACION_FALLIDA));
                }
            }

        }
        private void cargarFichasRegistrales()
        {
            long lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad =  Constantes.CONST_CANT_REGISTRO;

            int PaginaActual = CtrlPageBarFichaRegistral.PaginaActual;

            GridViewFicha.DataSource = null;
            GridViewFicha.DataBind();

            FichaRegistralBL objFichaRegistralBL = new FichaRegistralBL();
            DataTable dtFichaRegistral = new DataTable();

            dtFichaRegistral = objFichaRegistralBL.Consultar(0, lngActuacionDetalleId, "", "", "", 0, PaginaActual, intPaginaCantidad, ref IntTotalCount, ref IntTotalPages);

            if (dtFichaRegistral.Rows.Count > 0)
            {
                    GridViewFicha.DataSource = dtFichaRegistral;
                    GridViewFicha.DataBind();

                    CtrlPageBarFichaRegistral.TotalResgistros = IntTotalCount;
                    CtrlPageBarFichaRegistral.TotalPaginas = IntTotalPages;

                    CtrlPageBarFichaRegistral.Visible = false;
                    ctrlToolBarFicha.btnEliminar.Enabled = true;
                    btnAgregarParticipante.Enabled = true;
                    btnDocAdjuntosFicha.Enabled = true;
                    if (CtrlPageBarFichaRegistral.TotalResgistros > intPaginaCantidad)
                    {
                        CtrlPageBarFichaRegistral.Visible = true;
                    }
                    ctrlToolBarFicha.btnImprimir.Enabled = true;

                    if (dtFichaRegistral.Rows[0]["ESTA_VDESCRIPCIONCORTA"].ToString() == "OBSERVADO")
                    {
                        ctrlToolBarFicha.btnNuevo.Enabled = true;
                        
                    }
                    else 
                    { 
                        ctrlToolBarFicha.btnNuevo.Enabled = false;
                        hNuevo.Value = "0";
                    }
                    
            }
            else
            {
                ctrlToolBarFicha.btnImprimir.Enabled = false;
                ctrlToolBarFicha.btnEliminar.Enabled = false;
                btnAgregarParticipante.Enabled = false;
                btnDocAdjuntosFicha.Enabled = false;
                //ctrlToolBarFicha_btnNuevoHandler();
            }
            Session["FichaRegistral"] = dtFichaRegistral;
            int iFichaRegistralId = 0;
            if (dtFichaRegistral.Rows.Count > 0)
            {
                iFichaRegistralId = Convert.ToInt32(dtFichaRegistral.Rows[0]["FIRE_IFICHAREGISTRALID"].ToString());
            }
            else {
                iFichaRegistralId = 0;
            }
            ConsultarParticipantesFichaRegistral(iFichaRegistralId);
            ListarDocumentosAdjuntosReniec(iFichaRegistralId);
            if (dtFichaRegistral.Rows.Count > 1)
            {
                //Comun.EjecutarScript(Page, "ActivarGrilla();");
            }
            else
            {
                Comun.EjecutarScript(Page, "OcultarGrilla();");
                updFicha.Update(); 
            }
                    
        }

        private Int16 ObtenerEstadoId(string strEstado)
        {
            Int16 intEstadoId = 0;

            for (int i = 0; i < ddlEstadoFicha.Items.Count; i++)
            {
                if (ddlEstadoFicha.Items[i].Text.Equals(strEstado))
                {
                    intEstadoId = Convert.ToInt16(ddlEstadoFicha.Items[i].Value);
                    break;
                }
            }

            return intEstadoId;
        }

        private bool ExisteFichaRegistral(string strNumeroFicha)
        {
            bool bExiste = false;

            if (Session["FichaRegistral"] != null)
            {
                DataTable dtFichas = new DataTable();
                dtFichas = (DataTable)Session["FichaRegistral"];

                for (int i = 0; i < dtFichas.Rows.Count; i++)
                {
                    if (dtFichas.Rows[i]["FIRE_VNUMEROFICHA"].ToString().Equals(strNumeroFicha))
                    {
                        bExiste = true;
                        break;
                    }
                }
            }
            return bExiste;
        }


        //--------------------------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 09/12/2016
        // Objetivo: Eventos del Ubigeo del domicilio y lugar de nacimiento
        //---------------------------------------------------------------------------

        protected void ddl_DeptDomicilio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_DeptDomicilio.SelectedIndex > 0)
            {
                ddl_ProvDomicilio.Enabled = true;

                comun_Part3.CargarUbigeo(Session, ddl_ProvDomicilio, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_DeptDomicilio.SelectedValue.ToString(), "", true);

                ddl_DistDomicilio.Items.Clear();
                ddl_DistDomicilio.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));

                if (ddl_ProvDomicilio.Enabled == true)
                {
                    ddl_DeptDomicilio.Focus();
                }
            }
            else
            {
                ddl_ProvDomicilio.Items.Clear();
                ddl_ProvDomicilio.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));

                ddl_DistDomicilio.Items.Clear();
                ddl_DistDomicilio.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
            }

            //if (hNuevoParticipante.Value != "1")
            //{
            //    Comun.EjecutarScript(Page, "desabilitarBotonesEditar();");
            //}
            
        }

        protected void ddl_ProvDomicilio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_ProvDomicilio.SelectedIndex > 0)
            {
                comun_Part3.CargarUbigeo(Session, ddl_DistDomicilio, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddl_DeptDomicilio.SelectedValue.ToString(), ddl_ProvDomicilio.SelectedValue.ToString(), true);
                if (ddl_DistDomicilio.Enabled == true)
                {
                    ddl_ProvDomicilio.Focus();
                }
            }
            else
            {
                ddl_DistDomicilio.Items.Clear();
                ddl_DistDomicilio.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
            }
            //if (hNuevoParticipante.Value != "1")
            //{
            //    Comun.EjecutarScript(Page, "desabilitarBotonesEditar();");
            //}
        }
       
        protected void ddl_DeptNacimiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_DeptNacimiento.SelectedIndex > 0)
            {
                ddl_ProvNacimiento.Enabled = true;

                comun_Part3.CargarUbigeo(Session, ddl_ProvNacimiento, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_DeptNacimiento.SelectedValue.ToString(), "", true);

                ddl_DistNacimiento.Items.Clear();
                ddl_DistNacimiento.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));

                if (ddl_ProvNacimiento.Enabled == true)
                {
                    ddl_DeptNacimiento.Focus();
                }
            }
            else
            {
                ddl_ProvNacimiento.Items.Clear();
                ddl_ProvNacimiento.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));

                ddl_DistNacimiento.Items.Clear();
                ddl_DistNacimiento.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
            }
            //if (hNuevoParticipante.Value != "1")
            //{
            //    Comun.EjecutarScript(Page, "desabilitarBotonesEditar();");
            //}
        }

        protected void ddl_ProvNacimiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_ProvNacimiento.SelectedIndex > 0)
            {
                comun_Part3.CargarUbigeo(Session, ddl_DistNacimiento, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddl_DeptNacimiento.SelectedValue.ToString(), ddl_ProvNacimiento.SelectedValue.ToString(), true);
                if (ddl_DistNacimiento.Enabled == true)
                {
                    ddl_ProvNacimiento.Focus();
                }
            }
            else
            {
                ddl_DistNacimiento.Items.Clear();
                ddl_DistNacimiento.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
            }
            //if (hNuevoParticipante.Value != "1")
            //{
            //    Comun.EjecutarScript(Page, "desabilitarBotonesEditar();");
            //}
        }

        private void ObtenerDatosGrilla()
        {
            DataTable dtFichas = new DataTable();
            dtFichas = (DataTable)Session["FichaRegistral"];
            HFFichaRegistralId.Value = dtFichas.Rows[0]["FIRE_IFICHAREGISTRALID"].ToString();
        }


        private void NuevoParticipante()
        {
            try
            {
                ObtenerDatosGrilla();
                bool bolExisteParticipante = false;
                bool bolExisteParticipanteCodigo = false;
                bool bolExisteParticipanteDeclarante = false;

                if (Session["FichaRegistralParticipante"] != null)
                {
                    DataTable dtParticipantes = new DataTable();

                    dtParticipantes = (DataTable)Session["FichaRegistralParticipante"];

                    for (int i = 0; i < dtParticipantes.Rows.Count; i++)
                    {
                        if (dtParticipantes.Rows[i]["ITIPO_PARTICIPANTEID"].ToString().Equals(ddl_TipoParticipante.SelectedValue))
                        {
                            bolExisteParticipante = true;
                        }
                        if (dtParticipantes.Rows[i]["IPERSONAID"].ToString().Equals(hfPersona.Value))
                        {
                            if (ddl_TipoParticipante.SelectedItem.Text == "DECLARANTE")
                            {
                                if (dtParticipantes.Rows[i]["VTIPO_PARTICIPANTE"].ToString().Equals("DECLARANTE"))
                                {
                                    bolExisteParticipanteDeclarante = true;
                                }
                            }
                            else
                            {
                                bolExisteParticipanteCodigo = true;
                            }
                        }
                    }
                }

                if (bolExisteParticipante)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Ficha Registral - Participante", "El tipo de participante ya existe"));
                    return;
                }

                if (bolExisteParticipanteDeclarante)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Ficha Registral - Participante", "El participante ya existe"));
                    return;
                }

                if (bolExisteParticipanteCodigo)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Ficha Registral - Participante", "El participante ya existe"));
                    return;
                }

                if (EsMayorEdad())
                {
                    if (!(validarParticipanteFichaMayorEdad()))
                    { return; }
                }
                else
                {
                    if (!(validarParticipanteFichaMenorEdad()))
                    { return; }
                }

                //------------------------------------------------------------------------
                //Fecha: 16/04/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Validar el registro del tipo de participante, Tipo de documento, 
                //        nro. documento, primer apellido, nombres y genero en relación
                //        al tipo de participante.
                //------------------------------------------------------------------------
                if (validarDatosParticipantes())
                { return; }
                //------------------------------------------------------------------------

                BE.MRE.RE_PERSONA objPersonaBE = new BE.MRE.RE_PERSONA();
                BE.MRE.RE_PERSONAIDENTIFICACION objPersonaIdentificacionBE = new BE.MRE.RE_PERSONAIDENTIFICACION();
                BE.MRE.RE_PERSONARESIDENCIA objPersonaResidenciaBE = new BE.MRE.RE_PERSONARESIDENCIA();
                BE.MRE.RE_RESIDENCIA objResidenciaBE = new BE.MRE.RE_RESIDENCIA();
                BE.MRE.RE_FICHAREGISTRALPARTICIPANTE objFichaParticipanteBE = new BE.MRE.RE_FICHAREGISTRALPARTICIPANTE();
                FichaRegistralParticipanteBL objFichaParticipanteBL = new FichaRegistralParticipanteBL();


                long iPersonaId = Convert.ToInt64(hfPersona.Value);
                long iResidenciaId = Convert.ToInt64(HFpere_iResidenciaId.Value);
                Int16 iTipoDocumentoId = Convert.ToInt16(ddl_TipoDocParticipante.SelectedValue);
                string strResUbigeo = getUbigeo(ddl_DeptDomicilio, ddl_ProvDomicilio, ddl_DistDomicilio);
                string strNacUbigeo = getUbigeo(ddl_DeptNacimiento, ddl_ProvNacimiento, ddl_DistNacimiento);
                int iTipoParticipanteId = Convert.ToInt32(ddl_TipoParticipante.SelectedValue);
                long iFichaRegistralId = Convert.ToInt64(HFFichaRegistralId.Value);
                Int16 sEstadoCivil = Convert.ToInt16(CmbEstCiv.SelectedValue);
                Int16 sGradoInstruccion = Convert.ToInt16(CmbGradInst.SelectedValue);
                Int16 sGenero = Convert.ToInt16(CmbGenero.SelectedValue);
                
                objPersonaIdentificacionBE.peid_iPersonaId = iPersonaId;
                objPersonaIdentificacionBE.peid_sDocumentoTipoId = iTipoDocumentoId;
                objPersonaIdentificacionBE.peid_vDocumentoNumero = txtNroDocParticipante.Text.Trim();
                if (iTipoDocumentoId == (int)Enumerador.enmTipoDocumento.DNI)
                {
                    objPersonaIdentificacionBE.peid_bActivoEnRune = true;
                }
                else
                {
                    objPersonaIdentificacionBE.peid_bActivoEnRune = false;
                }
                objPersonaResidenciaBE.pere_iPersonaId = iPersonaId;
                objPersonaResidenciaBE.pere_iResidenciaId = iResidenciaId;
                objPersonaResidenciaBE.Residencia.resi_iResidenciaId = iResidenciaId;
                objPersonaResidenciaBE.Residencia.resi_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objPersonaResidenciaBE.Residencia.resi_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objPersonaResidenciaBE.Residencia.resi_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                objPersonaResidenciaBE.Residencia.resi_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                objPersonaResidenciaBE.Residencia.resi_vResidenciaDireccion = TxtDirDir.Text.ToUpper().Trim();
                objPersonaResidenciaBE.Residencia.resi_cResidenciaUbigeo = strResUbigeo;
                objPersonaResidenciaBE.Residencia.resi_sResidenciaTipoId = (int)Enumerador.enmTipoResidencia.RESIDENCIA;
                objPersonaResidenciaBE.Residencia.resi_vResidenciaTelefono = TxtTelfDir.Text.Trim();
                objPersonaResidenciaBE.Residencia.resi_vCodigoPostal = txtCodPostal.Text;

                objPersonaBE.pers_iPersonaId = iPersonaId;
                objPersonaBE.pers_sPersonaTipoId = (int)Enumerador.enmTipoPersona.NATURAL;
                objPersonaBE.pers_vApellidoPaterno = txtApePatParticipante.Text.ToUpper().Trim();
                objPersonaBE.pers_vApellidoMaterno = txtApeMatParticipante.Text.ToUpper().Trim();
                objPersonaBE.pers_vApellidoCasada = txtApeCasadaParticipante.Text.ToUpper().Trim();
                objPersonaBE.pers_vNombres = txtNomParticipante.Text.ToUpper().Trim();
                //objPersonaBE.pers_vLugarNacimiento = TxtDirNac.Text.ToUpper().Trim();
                objPersonaBE.pers_cNacimientoLugar = strNacUbigeo;
                if (txtFechaNacimiento.Text.Length > 0)
                {
                    DateTime datFecha = new DateTime();
                    if (!DateTime.TryParse(txtFechaNacimiento.Text, out datFecha))
                    {
                        datFecha = Comun.FormatearFecha(txtFechaNacimiento.Text);
                    }

                    objPersonaBE.pers_dNacimientoFecha = datFecha;
                }
                objPersonaBE.pers_dFechaDefuncion = null;

                objPersonaBE.pers_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objPersonaBE.pers_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                objPersonaBE.pers_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objPersonaBE.pers_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                objPersonaBE.pers_vCorreoElectronico = "";
                objPersonaBE.pers_sGeneroId = sGenero;
                objPersonaBE.pers_vObservaciones = "";
                objPersonaBE.pers_vEstatura = txtEstatura.Text;
                objPersonaBE.pers_cEstudioCompleto = ddlCompleto.SelectedValue.ToString();
                if (txtAnio.Text.Length == 0)
                {
                    objPersonaBE.pers_sAnioEstudio = 0;
                }
                else
                {
                    objPersonaBE.pers_sAnioEstudio = Convert.ToInt16(txtAnio.Text);
                }
                objPersonaBE.pers_cDiscapacidad = SelecionDiscapacidad();
                objPersonaBE.pers_cInterdicto = SelecionInterdicto();
                objPersonaBE.pers_cDonaOrganos = SelecionDonaOrganos();
                objPersonaBE.pers_vNombreCurador = txtCurador.Text;
                objPersonaBE.pers_sTipoDeclarante = Convert.ToInt16(ddlDeclarante.SelectedValue);
                objPersonaBE.pers_sTipoTutorGuardador = Convert.ToInt16(ddlTutorG.SelectedValue);

                if (ddl_TipoParticipante.SelectedValue == Enumerador.enmFichaTipoParticipanteMenor.TITULAR.ToString() ||
                    ddl_TipoParticipante.SelectedValue == Enumerador.enmFichaTipoParticipanteMayor.TITULAR.ToString())
                {
                    objPersonaBE.pers_sNacionalidadId = Convert.ToInt16(Enumerador.enmNacionalidad.PERUANA);
                }
                else
                {
                    if (Convert.ToInt16(ddl_TipoDocParticipante.SelectedValue) == Convert.ToInt16(Enumerador.enmTipoDocumento.DNI))
                    {
                        objPersonaBE.pers_sNacionalidadId = Convert.ToInt16(Enumerador.enmNacionalidad.PERUANA);
                    }
                    else { objPersonaBE.pers_sNacionalidadId = Convert.ToInt16(Enumerador.enmNacionalidad.NINGUNA); }
                }
                objPersonaBE.pers_sEstadoCivilId = sEstadoCivil;
                objPersonaBE.pers_sPaisId = 0;
                objPersonaBE.pers_sGradoInstruccionId = sGradoInstruccion;
                objPersonaBE.pers_sOcupacionId = 0;
                objPersonaBE.pers_sProfesionId = 0;
                objPersonaBE.bGenera58A = false;
                //objPersonaBE.pers_vApellidoCasada = "";
                objPersonaBE.pers_bFallecidoFlag = false;
                objPersonaBE.pers_vSenasParticulares = TxtSenasParticulares.Text.Trim().ToUpper();
                objPersonaBE.pers_vCorreoElectronico = TxtEmail.Text.ToUpper();
                objPersonaBE.REGISTROUNICO.reun_vEmergenciaNombre = "";
                objPersonaBE.REGISTROUNICO.reun_sEmergenciaRelacionId = 0;
                objPersonaBE.REGISTROUNICO.reun_vEmergenciaDireccionLocal = "";
                objPersonaBE.REGISTROUNICO.reun_vEmergenciaCodigoPostal = "";
                objPersonaBE.REGISTROUNICO.reun_vEmergenciaTelefono = "";
                objPersonaBE.REGISTROUNICO.reun_vEmergenciaDireccionPeru = "";
                objPersonaBE.REGISTROUNICO.reun_vEmergenciaCorreoElectronico = "";
                objPersonaBE.REGISTROUNICO.reun_cViveExteriorDesde = "00";
                objPersonaBE.REGISTROUNICO.reun_bPiensaRetornarAlPeru = false;
                objPersonaBE.REGISTROUNICO.reun_cCuandoRetornaAlPeru = "00";
                objPersonaBE.REGISTROUNICO.reun_bAfiliadoSeguroSocial = false;
                objPersonaBE.REGISTROUNICO.reun_bAfiliadoAFP = false;
                objPersonaBE.REGISTROUNICO.reun_bAportaSeguroSocial = false;
                objPersonaBE.REGISTROUNICO.reun_bBeneficiadoExterior = false;
                objPersonaBE.REGISTROUNICO.reun_sOcupacionPeru = 0;
                objPersonaBE.REGISTROUNICO.reun_sOcupacionExtranjero = 0;
                objPersonaBE.REGISTROUNICO.reun_vNombreConvenio = "";


                objPersonaBE.Identificacion = objPersonaIdentificacionBE;
                objPersonaBE.Residencias.Add(objPersonaResidenciaBE);

                objFichaParticipanteBE.fipa_iFichaRegistralId = iFichaRegistralId;
                objFichaParticipanteBE.fipa_sTipoParticipanteId = iTipoParticipanteId;
                objFichaParticipanteBE.fipa_iPersonaId = iPersonaId;
                objFichaParticipanteBE.fipa_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objFichaParticipanteBE.fipa_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                objFichaParticipanteBE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                objFichaParticipanteBE.fipa_bConsignaApellidoMaterno = chkConsignaApeCas.Checked;
                objFichaParticipanteBE.PERSONA = objPersonaBE;

                objFichaParticipanteBL.insertar(ref objFichaParticipanteBE);

                if (!(objFichaParticipanteBE.Error))
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Ficha Registral - Participante", Constantes.CONST_MENSAJE_EXITO));
                    //--------------------------------------------------
                    ConsultarParticipantesFichaRegistral(iFichaRegistralId);
                    LimpiarDatosParticipantes();
                }
                else
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Ficha Registral - Participante", Constantes.CONST_MENSAJE_OPERACION_FALLIDA));
                }
            }
            catch (Exception ex)
            {
                #region Registro Incidencia
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_vValoresTabla = "PARTICIPANTE RENIEC",
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
        }

        private void EditarParticipante()
        {
            try
            {
                ObtenerDatosGrilla();
                bool bolExisteParticipante = false;
                bool bolExisteParticipanteCodigo = false;
                bool bolExisteParticipanteDeclarante = false;

                if (Session["FichaRegistralParticipante"] != null)
                {
                    DataTable dtParticipantes = new DataTable();

                    dtParticipantes = (DataTable)Session["FichaRegistralParticipante"];

                    foreach (DataRow dr in dtParticipantes.Rows)
                    {
                        if (dr["IPARTICIPANTEID"].ToString() == hNuevoParticipanteEDITARID.Value)
                            dr.Delete();
                    }
                    dtParticipantes.AcceptChanges();
                    for (int i = 0; i < dtParticipantes.Rows.Count; i++)
                    {
                        if (dtParticipantes.Rows[i]["ITIPO_PARTICIPANTEID"].ToString().Equals(ddl_TipoParticipante.SelectedValue))
                        {
                            bolExisteParticipante = true;
                        }
                        if (dtParticipantes.Rows[i]["IPERSONAID"].ToString().Equals(hfPersona.Value))
                        {
                            if (ddl_TipoParticipante.SelectedItem.Text == "DECLARANTE")
                            {
                                if (dtParticipantes.Rows[i]["VTIPO_PARTICIPANTE"].ToString().Equals("DECLARANTE"))
                                {
                                    bolExisteParticipanteDeclarante = true;
                                }
                            }
                            else
                            {
                                bolExisteParticipanteCodigo = true;
                            }
                        }
                    }
                }

                if (bolExisteParticipante)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Ficha Registral - Participante", "El tipo de participante ya existe"));
                    return;
                }

                if (bolExisteParticipanteDeclarante)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Ficha Registral - Participante", "El participante ya existe"));
                    return;
                }

                if (bolExisteParticipanteCodigo)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Ficha Registral - Participante", "El participante ya existe"));
                    return;
                }
                if (EsMayorEdad())
                {
                    if (!(validarParticipanteFichaMayorEdad()))
                    { return; }
                }
                else
                {
                    if (!(validarParticipanteFichaMenorEdad()))
                    { return; }
                }

                //----------------------------------------------------------
                //Fecha: 25/02/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Validar si existe la dirección.
                //----------------------------------------------------------
                if (DIV_LUGAR_DOMICILIO.Visible == true)
                {
                    string strDireccion = TxtDirDir.Text.Trim().ToUpper();
                    if (strDireccion != hd_DirDir.Value.ToString().Trim().ToUpper())
                    {
                        if (validarExisteDireccion(strDireccion))
                        {
                            Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Ficha Registral - Participante", "La Dirección ya existe"));
                            return;
                        }
                    }
                }
                //----------------------------------------------------------
                //------------------------------------------------------------------------
                //Fecha: 16/04/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Validar el registro del tipo de participante, Tipo de documento, 
                //        nro. documento, primer apellido, nombres y genero en relación
                //        al tipo de participante.
                //------------------------------------------------------------------------
                if (validarDatosParticipantes())
                { return; }
                //------------------------------------------------------------------------

                BE.MRE.RE_PERSONA objPersonaBE = new BE.MRE.RE_PERSONA();
                BE.MRE.RE_PERSONAIDENTIFICACION objPersonaIdentificacionBE = new BE.MRE.RE_PERSONAIDENTIFICACION();
                BE.MRE.RE_PERSONARESIDENCIA objPersonaResidenciaBE = new BE.MRE.RE_PERSONARESIDENCIA();
                BE.MRE.RE_RESIDENCIA objResidenciaBE = new BE.MRE.RE_RESIDENCIA();
                BE.MRE.RE_FICHAREGISTRALPARTICIPANTE objFichaParticipanteBE = new BE.MRE.RE_FICHAREGISTRALPARTICIPANTE();
                FichaRegistralParticipanteBL objFichaParticipanteBL = new FichaRegistralParticipanteBL();


                long iPersonaId = Convert.ToInt64(hfPersona.Value);

                if (HFpere_iResidenciaId.Value == "")
                {
                    HFpere_iResidenciaId.Value = "0";
                }

                long iResidenciaId = 0;

                if (hNuevaDireccion.Value == "1")
                {
                    iResidenciaId = 0;
                }
                else
                {
                    iResidenciaId = Convert.ToInt64(HFpere_iResidenciaId.Value);
                }

                Int16 iTipoDocumentoId = Convert.ToInt16(ddl_TipoDocParticipante.SelectedValue);
                string strResUbigeo = getUbigeo(ddl_DeptDomicilio, ddl_ProvDomicilio, ddl_DistDomicilio);
                string strNacUbigeo = getUbigeo(ddl_DeptNacimiento, ddl_ProvNacimiento, ddl_DistNacimiento);
                DateTime dFechaNacimiento = txtFechaNacimiento.Value();
                int iTipoParticipanteId = Convert.ToInt32(ddl_TipoParticipante.SelectedValue);
                long iFichaRegistralId = Convert.ToInt64(HFFichaRegistralId.Value);
                Int16 sEstadoCivil = Convert.ToInt16(CmbEstCiv.SelectedValue);
                Int16 sGradoInstruccion = Convert.ToInt16(CmbGradInst.SelectedValue);
                Int16 sGenero = Convert.ToInt16(CmbGenero.SelectedValue);

                objPersonaIdentificacionBE.peid_iPersonaId = iPersonaId;
                objPersonaIdentificacionBE.peid_sDocumentoTipoId = iTipoDocumentoId;
                objPersonaIdentificacionBE.peid_vDocumentoNumero = txtNroDocParticipante.Text.Trim();
                objPersonaResidenciaBE.pere_iPersonaId = iPersonaId;
                objPersonaResidenciaBE.pere_iResidenciaId = iResidenciaId;
                objPersonaResidenciaBE.Residencia.resi_iResidenciaId = iResidenciaId;
                objPersonaResidenciaBE.Residencia.resi_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objPersonaResidenciaBE.Residencia.resi_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objPersonaResidenciaBE.Residencia.resi_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                objPersonaResidenciaBE.Residencia.resi_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                objPersonaResidenciaBE.Residencia.resi_vResidenciaDireccion = TxtDirDir.Text.ToUpper().Trim();
                objPersonaResidenciaBE.Residencia.resi_cResidenciaUbigeo = strResUbigeo;
                objPersonaResidenciaBE.Residencia.resi_sResidenciaTipoId = (int)Enumerador.enmTipoResidencia.RESIDENCIA;
                objPersonaResidenciaBE.Residencia.resi_vResidenciaTelefono = TxtTelfDir.Text.Trim();
                objPersonaResidenciaBE.Residencia.resi_vCodigoPostal = txtCodPostal.Text;

                objPersonaBE.pers_iPersonaId = iPersonaId;
                objPersonaBE.pers_sPersonaTipoId = (int)Enumerador.enmTipoPersona.NATURAL;
                objPersonaBE.pers_vApellidoPaterno = txtApePatParticipante.Text.ToUpper().Trim();
                objPersonaBE.pers_vApellidoMaterno = txtApeMatParticipante.Text.ToUpper().Trim();
                objPersonaBE.pers_vApellidoCasada = txtApeCasadaParticipante.Text.ToUpper().Trim();
                objPersonaBE.pers_vNombres = txtNomParticipante.Text.ToUpper().Trim();
                //objPersonaBE.pers_vLugarNacimiento = TxtDirNac.Text.ToUpper().Trim();
                objPersonaBE.pers_cNacimientoLugar = strNacUbigeo;
                objPersonaBE.pers_dNacimientoFecha = dFechaNacimiento;
                objPersonaBE.pers_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objPersonaBE.pers_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                objPersonaBE.pers_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objPersonaBE.pers_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                objPersonaBE.pers_vCorreoElectronico = TxtEmail.Text.ToUpper();
                objPersonaBE.pers_sGeneroId = sGenero;
                objPersonaBE.pers_vObservaciones = "";
                objPersonaBE.pers_vEstatura = txtEstatura.Text;
                objPersonaBE.pers_cEstudioCompleto = ddlCompleto.SelectedValue.ToString();
                if (txtAnio.Text.Length == 0)
                {
                    objPersonaBE.pers_sAnioEstudio = 0;
                }
                else {
                    objPersonaBE.pers_sAnioEstudio = Convert.ToInt16(txtAnio.Text);
                }
                
                objPersonaBE.pers_cDiscapacidad = SelecionDiscapacidad();
                objPersonaBE.pers_cInterdicto = SelecionInterdicto();
                objPersonaBE.pers_cDonaOrganos = SelecionDonaOrganos();
                objPersonaBE.pers_vNombreCurador = txtCurador.Text;
                objPersonaBE.pers_sTipoDeclarante = Convert.ToInt16(ddlDeclarante.SelectedValue);
                objPersonaBE.pers_sTipoTutorGuardador = Convert.ToInt16(ddlTutorG.SelectedValue);

                if (ddl_TipoParticipante.SelectedValue == Enumerador.enmFichaTipoParticipanteMenor.TITULAR.ToString() ||
                    ddl_TipoParticipante.SelectedValue == Enumerador.enmFichaTipoParticipanteMayor.TITULAR.ToString())
                {
                    objPersonaBE.pers_sNacionalidadId = Convert.ToInt16(Enumerador.enmNacionalidad.PERUANA);
                }
                else
                {
                    if (Convert.ToInt16(ddl_TipoDocParticipante.SelectedValue) == Convert.ToInt16(Enumerador.enmTipoDocumento.DNI))
                    {
                        objPersonaBE.pers_sNacionalidadId = Convert.ToInt16(Enumerador.enmNacionalidad.PERUANA);
                    }
                    else { objPersonaBE.pers_sNacionalidadId = Convert.ToInt16(Enumerador.enmNacionalidad.NINGUNA); }
                }

                objPersonaBE.pers_sEstadoCivilId = sEstadoCivil;
                objPersonaBE.pers_sGradoInstruccionId = sGradoInstruccion;
                objPersonaBE.pers_sOcupacionId = 0;
                objPersonaBE.pers_sProfesionId = 0;
                objPersonaBE.bGenera58A = false;
                //objPersonaBE.pers_vApellidoCasada = "";
                objPersonaBE.pers_bFallecidoFlag = false;
                objPersonaBE.pers_vSenasParticulares = TxtSenasParticulares.Text.Trim().ToUpper();
                objPersonaBE.pers_vCorreoElectronico = TxtEmail.Text.ToUpper();
                objPersonaBE.REGISTROUNICO.reun_vEmergenciaNombre = "";
                objPersonaBE.REGISTROUNICO.reun_sEmergenciaRelacionId = 0;
                objPersonaBE.REGISTROUNICO.reun_vEmergenciaDireccionLocal = "";
                objPersonaBE.REGISTROUNICO.reun_vEmergenciaCodigoPostal = "";
                objPersonaBE.REGISTROUNICO.reun_vEmergenciaTelefono = "";
                objPersonaBE.REGISTROUNICO.reun_vEmergenciaDireccionPeru = "";
                objPersonaBE.REGISTROUNICO.reun_vEmergenciaCorreoElectronico = "";
                objPersonaBE.REGISTROUNICO.reun_cViveExteriorDesde = "00";
                objPersonaBE.REGISTROUNICO.reun_bPiensaRetornarAlPeru = false;
                objPersonaBE.REGISTROUNICO.reun_cCuandoRetornaAlPeru = "00";
                objPersonaBE.REGISTROUNICO.reun_bAfiliadoSeguroSocial = false;
                objPersonaBE.REGISTROUNICO.reun_bAfiliadoAFP = false;
                objPersonaBE.REGISTROUNICO.reun_bAportaSeguroSocial = false;
                objPersonaBE.REGISTROUNICO.reun_bBeneficiadoExterior = false;
                objPersonaBE.REGISTROUNICO.reun_sOcupacionPeru = 0;
                objPersonaBE.REGISTROUNICO.reun_sOcupacionExtranjero = 0;
                objPersonaBE.REGISTROUNICO.reun_vNombreConvenio = "";

                objPersonaBE.Identificacion = objPersonaIdentificacionBE;
                objPersonaBE.Residencias.Add(objPersonaResidenciaBE);

                objFichaParticipanteBE.fipa_iFichaRegistralParticipanteId = Convert.ToInt64(HFParticipanteId.Value);
                objFichaParticipanteBE.fipa_sTipoParticipanteId = iTipoParticipanteId;
                objFichaParticipanteBE.fipa_iPersonaId = iPersonaId;
                objFichaParticipanteBE.fipa_cEstadoId = "A";
                objFichaParticipanteBE.fipa_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objFichaParticipanteBE.fipa_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                objFichaParticipanteBE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                objFichaParticipanteBE.fipa_bConsignaApellidoMaterno = chkConsignaApeCas.Checked;

                objFichaParticipanteBE.PERSONA = objPersonaBE;
                
                objFichaParticipanteBL.actualizar(ref objFichaParticipanteBE);

                if (!(objFichaParticipanteBE.Error))
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Ficha Registral - Participante", Constantes.CONST_MENSAJE_EXITO));
                    //--------------------------------------------------
                    ConsultarParticipantesFichaRegistral(iFichaRegistralId);
                    LimpiarDatosParticipantes();
                }
                else
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Ficha Registral - Participante", Constantes.CONST_MENSAJE_OPERACION_FALLIDA));
                }
            }
            catch (Exception ex){
                #region Registro Incidencia
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_vValoresTabla = "PARTICIPANTE RENIEC",
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
            //if (hNuevoParticipante.Value != "1")
            //{
            //    Comun.EjecutarScript(Page, "desabilitarBotonesEditar();");
            //}  
        }
        //--------------------------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 09/12/2016
        // Objetivo: Adiciona a la tabla temporal los datos del participante.
        //---------------------------------------------------------------------------
        protected void btnNuevoParticipante_Click(object sender, EventArgs e)
        {
            NuevoParticipante();
        }

        //--------------------------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 09/12/2016
        // Objetivo: Actualizar los datos del participante.
        // Autor: Jonatan Silva Cachay
        // Fecha: 28/09/2017
        // Objetivo: se utiliza un solo boton para realizar la actualización o registro
        //---------------------------------------------------------------------------

        protected void btnEditarParticipante_Click(object sender, EventArgs e)
        {
            if (hNuevoParticipante.Value == "0")
            {
                EditarParticipante();
                Comun.EjecutarScript(Page, "ActivarNuevoParticipante();");
            }
            else {
                NuevoParticipante();
            }
        }

        //-----------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 09/12/2016
        // Objetivo: Inicializa los constroles de participantes
        //-----------------------------------------------------------

        protected void btnCancelarParticipante_Click(object sender, EventArgs e)
        {
            LimpiarDatosParticipantes();
            hNuevoParticipante.Value = "1";
            //ctrlToolBarFicha.btnCancelar.Enabled = true;
            //EstablecerSecuenciaEstadoFicha(ddlEstadoFicha, "");
            //btnEditarParticipante.Visible = false;
            //btnGrabarParticipante.Visible = true;
            
            //if (hNuevoParticipante.Value != "1")
            //{
            //    Comun.EjecutarScript(Page, "desabilitarBotonesEditar();");
            //}
            //else {
            //    btnEditarParticipante.Visible = false;
            //    Comun.EjecutarScript(Page, "desabilitarBotonesGrabar();");
            //}
        }

        //----------------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 09/12/2016
        // Objetivo: Validar datos del participante para un menor de edad. 
        //----------------------------------------------------------------

        private bool validarParticipanteFichaMenorEdad()
        {
            //----------------------------------------------
            // Datos del Particpante de la Ficha Registral
            //----------------------------------------------
            if (ddl_TipoParticipante.SelectedIndex == 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Participante", "Seleccione el tipo de participante"));
                ddl_TipoParticipante.Focus();
                return false;
            }


            int intTipoParticipante = Convert.ToInt32(ddl_TipoParticipante.SelectedValue);
            switch (intTipoParticipante)
            {
                case (int)Enumerador.enmFichaTipoParticipanteMenor.TITULAR:

                     string strTitular_TipoDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Titular.TipoDocumento"].ToString();
                     string strTitular_NumeroDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Titular.NumeroDocumento"].ToString();
                     string strTitular_PrimerApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Titular.PrimerApellido"].ToString();
                     string strTitular_SegundoApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Titular.SegundoApellido"].ToString();
                     string strTitular_Nombres = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Titular.Nombres"].ToString();
                     string strTitular_Domicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Titular.Domicilio"].ToString();
                     string strTitular_UbigeoDomicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Titular.UbigeoDomicilio"].ToString();
                     string strTitular_FechaNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Titular.FechaNacimiento"].ToString();
                     string strTitular_LugarNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Titular.LugarNacimiento"].ToString();
                     string strTitular_UbigeoNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Titular.UbigeoNacimiento"].ToString();

                    if (!(ValidacionFichaParticipantes(strTitular_TipoDocumento, strTitular_NumeroDocumento, strTitular_PrimerApellido, strTitular_SegundoApellido, strTitular_Nombres,
                                                strTitular_Domicilio, strTitular_UbigeoDomicilio, strTitular_FechaNacimiento, strTitular_LugarNacimiento, strTitular_UbigeoNacimiento)))
                    { return false;}

                    break;
                case (int)Enumerador.enmFichaTipoParticipanteMenor.PADRE:

                     string strPadre_TipoDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Padre.TipoDocumento"].ToString();
                     string strPadre_NumeroDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Padre.NumeroDocumento"].ToString();
                     string strPadre_PrimerApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Padre.PrimerApellido"].ToString();
                     string strPadre_SegundoApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Padre.SegundoApellido"].ToString();
                     string strPadre_Nombres = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Padre.Nombres"].ToString();
                     string strPadre_Domicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Padre.Domicilio"].ToString();
                     string strPadre_UbigeoDomicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Padre.UbigeoDomicilio"].ToString();
                     string strPadre_FechaNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Padre.FechaNacimiento"].ToString();
                     string strPadre_LugarNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Padre.LugarNacimiento"].ToString();
                     string strPadre_UbigeoNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Padre.UbigeoNacimiento"].ToString();

                     if (!(ValidacionFichaParticipantes(strPadre_TipoDocumento, strPadre_NumeroDocumento, strPadre_PrimerApellido, strPadre_SegundoApellido, strPadre_Nombres,
                                                strPadre_Domicilio, strPadre_UbigeoDomicilio, strPadre_FechaNacimiento, strPadre_LugarNacimiento, strPadre_UbigeoNacimiento)))
                    { return false;}

                    break;
                case (int)Enumerador.enmFichaTipoParticipanteMenor.MADRE:
                    string strMadre_TipoDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Madre.TipoDocumento"].ToString();
                    string strMadre_NumeroDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Madre.NumeroDocumento"].ToString();
                    string strMadre_PrimerApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Madre.PrimerApellido"].ToString();
                    string strMadre_SegundoApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Madre.SegundoApellido"].ToString();
                    string strMadre_Nombres = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Madre.Nombres"].ToString();
                    string strMadre_Domicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Madre.Domicilio"].ToString();
                    string strMadre_UbigeoDomicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Madre.UbigeoDomicilio"].ToString();
                    string strMadre_FechaNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Madre.FechaNacimiento"].ToString();
                    string strMadre_LugarNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Madre.LugarNacimiento"].ToString();
                    string strMadre_UbigeoNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Madre.UbigeoNacimiento"].ToString();

                    if (!(ValidacionFichaParticipantes(strMadre_TipoDocumento, strMadre_NumeroDocumento, strMadre_PrimerApellido, strMadre_SegundoApellido, strMadre_Nombres,
                                                strMadre_Domicilio, strMadre_UbigeoDomicilio, strMadre_FechaNacimiento, strMadre_LugarNacimiento, strMadre_UbigeoNacimiento)))
                    { return false;}
                    break;

                case (int)Enumerador.enmFichaTipoParticipanteMenor.DECLARANTE:
                     string strDeclarante_TipoDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Declarante.TipoDocumento"].ToString();
                     string strDeclarante_NumeroDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Declarante.NumeroDocumento"].ToString();
                     string strDeclarante_PrimerApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Declarante.PrimerApellido"].ToString();
                     string strDeclarante_SegundoApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Declarante.SegundoApellido"].ToString();
                     string strDeclarante_Nombres = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Declarante.Nombres"].ToString();
                     string strDeclarante_Domicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Declarante.Domicilio"].ToString();
                     string strDeclarante_UbigeoDomicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Declarante.UbigeoDomicilio"].ToString();
                     string strDeclarante_FechaNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Declarante.FechaNacimiento"].ToString();
                     string strDeclarante_LugarNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Declarante.LugarNacimiento"].ToString();
                     string strDeclarante_UbigeoNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Declarante.UbigeoNacimiento"].ToString();

                     if (!(ValidacionFichaParticipantes(strDeclarante_TipoDocumento, strDeclarante_NumeroDocumento, strDeclarante_PrimerApellido, strDeclarante_SegundoApellido, strDeclarante_Nombres,
                                                 strDeclarante_Domicilio, strDeclarante_UbigeoDomicilio, strDeclarante_FechaNacimiento, strDeclarante_LugarNacimiento, strDeclarante_UbigeoNacimiento)))
                     { return false; }
                     break;

                default:
                    break;
            }

   

            return true;
        }


        //----------------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 15/12/2016
        // Objetivo: Validar datos del participante para un mayor de edad. 
        //----------------------------------------------------------------

        private bool validarParticipanteFichaMayorEdad()
        {
            //----------------------------------------------
            // Datos del Particpante de la Ficha Registral
            //----------------------------------------------
            if (ddl_TipoParticipante.SelectedIndex == 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Participante", "Seleccione el tipo de participante"));
                ddl_TipoParticipante.Focus();
                return false;
            }


            int intTipoParticipante = Convert.ToInt32(ddl_TipoParticipante.SelectedValue);
            switch (intTipoParticipante)
            {
                case (int)Enumerador.enmFichaTipoParticipanteMayor.TITULAR:

                    string strTitular_TipoDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Titular.TipoDocumento"].ToString();
                    string strTitular_NumeroDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Titular.NumeroDocumento"].ToString();
                    string strTitular_PrimerApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Titular.PrimerApellido"].ToString();
                    string strTitular_SegundoApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Titular.SegundoApellido"].ToString();
                    string strTitular_Nombres = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Titular.Nombres"].ToString();
                    string strTitular_Domicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Titular.Domicilio"].ToString();
                    string strTitular_UbigeoDomicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Titular.UbigeoDomicilio"].ToString();
                    string strTitular_FechaNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Titular.FechaNacimiento"].ToString();
                    string strTitular_LugarNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Titular.LugarNacimiento"].ToString();
                    string strTitular_UbigeoNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Titular.UbigeoNacimiento"].ToString();

                    if (!(ValidacionFichaParticipantes(strTitular_TipoDocumento, strTitular_NumeroDocumento, strTitular_PrimerApellido, strTitular_SegundoApellido, strTitular_Nombres,
                                                strTitular_Domicilio, strTitular_UbigeoDomicilio, strTitular_FechaNacimiento, strTitular_LugarNacimiento, strTitular_UbigeoNacimiento)))
                    { return false; }

                    break;
                case (int)Enumerador.enmFichaTipoParticipanteMayor.PADRE:

                    string strPadre_TipoDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Padre.TipoDocumento"].ToString();
                    string strPadre_NumeroDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Padre.NumeroDocumento"].ToString();
                    string strPadre_PrimerApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Padre.PrimerApellido"].ToString();
                    string strPadre_SegundoApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Padre.SegundoApellido"].ToString();
                    string strPadre_Nombres = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Padre.Nombres"].ToString();
                    string strPadre_Domicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Padre.Domicilio"].ToString();
                    string strPadre_UbigeoDomicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Padre.UbigeoDomicilio"].ToString();
                    string strPadre_FechaNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Padre.FechaNacimiento"].ToString();
                    string strPadre_LugarNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Padre.LugarNacimiento"].ToString();
                    string strPadre_UbigeoNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Padre.UbigeoNacimiento"].ToString();

                    if (!(ValidacionFichaParticipantes(strPadre_TipoDocumento, strPadre_NumeroDocumento, strPadre_PrimerApellido, strPadre_SegundoApellido, strPadre_Nombres,
                                               strPadre_Domicilio, strPadre_UbigeoDomicilio, strPadre_FechaNacimiento, strPadre_LugarNacimiento, strPadre_UbigeoNacimiento)))
                    { return false; }

                    break;
                case (int)Enumerador.enmFichaTipoParticipanteMayor.MADRE:
                    string strMadre_TipoDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Madre.TipoDocumento"].ToString();
                    string strMadre_NumeroDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Madre.NumeroDocumento"].ToString();
                    string strMadre_PrimerApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Madre.PrimerApellido"].ToString();
                    string strMadre_SegundoApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Madre.SegundoApellido"].ToString();
                    string strMadre_Nombres = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Madre.Nombres"].ToString();
                    string strMadre_Domicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Madre.Domicilio"].ToString();
                    string strMadre_UbigeoDomicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Madre.UbigeoDomicilio"].ToString();
                    string strMadre_FechaNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Madre.FechaNacimiento"].ToString();
                    string strMadre_LugarNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Madre.LugarNacimiento"].ToString();
                    string strMadre_UbigeoNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Madre.UbigeoNacimiento"].ToString();

                    if (!(ValidacionFichaParticipantes(strMadre_TipoDocumento, strMadre_NumeroDocumento, strMadre_PrimerApellido, strMadre_SegundoApellido, strMadre_Nombres,
                                                strMadre_Domicilio, strMadre_UbigeoDomicilio, strMadre_FechaNacimiento, strMadre_LugarNacimiento, strMadre_UbigeoNacimiento)))
                    { return false; }
                    break;
                case (int)Enumerador.enmFichaTipoParticipanteMayor.CONYUGE:
                    string strConyuge_TipoDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Conyuge.TipoDocumento"].ToString();
                    string strConyuge_NumeroDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Conyuge.NumeroDocumento"].ToString();
                    string strConyuge_PrimerApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Conyuge.PrimerApellido"].ToString();
                    string strConyuge_SegundoApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Conyuge.SegundoApellido"].ToString();
                    string strConyuge_Nombres = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Conyuge.Nombres"].ToString();
                    string strConyuge_Domicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Conyuge.Domicilio"].ToString();
                    string strConyuge_UbigeoDomicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Conyuge.UbigeoDomicilio"].ToString();
                    string strConyuge_FechaNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Conyuge.FechaNacimiento"].ToString();
                    string strConyuge_LugarNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Conyuge.LugarNacimiento"].ToString();
                    string strConyuge_UbigeoNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Conyuge.UbigeoNacimiento"].ToString();

                    if (!(ValidacionFichaParticipantes(strConyuge_TipoDocumento, strConyuge_NumeroDocumento, strConyuge_PrimerApellido, strConyuge_SegundoApellido, strConyuge_Nombres,
                                                strConyuge_Domicilio, strConyuge_UbigeoDomicilio, strConyuge_FechaNacimiento, strConyuge_LugarNacimiento, strConyuge_UbigeoNacimiento)))
                    { return false; }
                    break;

                default:
                    break;
            }



            return true;
        }
        
        //----------------------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 09/12/2016
        // Objetivo: Grilla de participantes con las opciones de editar y anular.
        //------------------------------------------------------------------------

        protected void GridViewParticipante_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strTipoParticipanteId = "";

            if (e.CommandName == "Anular")
            {
                GridViewRow gvrModificar;
                gvrModificar = (GridViewRow)((TableCell)((ImageButton)e.CommandSource).Parent).Parent;

                Label lblFichaTipoParticipanteId = (Label)gvrModificar.FindControl("lblFichaTipoParticipanteId");

                strTipoParticipanteId = lblFichaTipoParticipanteId.Text;
                AnularFichaParticipante(strTipoParticipanteId);
            }

            if (e.CommandName == "Editar")
            {
                Comun.EjecutarScript(this, "abrirPopup();");

                GridViewRow gvrModificar;
                gvrModificar = (GridViewRow)((TableCell)((ImageButton)e.CommandSource).Parent).Parent;

                Label lblFichaTipoParticipanteId = (Label)gvrModificar.FindControl("lblFichaTipoParticipanteId");
                Label lblFichaParticipanteId = (Label)gvrModificar.FindControl("lblFichaRegistralParticipanteId");
                
                strTipoParticipanteId = lblFichaTipoParticipanteId.Text;
                hNuevoParticipanteEDITARID.Value = lblFichaParticipanteId.Text;
                EditarFichaParticipante(strTipoParticipanteId);
                hNuevoParticipante.Value = "0";

                bool esMayorEdad = EsMayorEdad();

                if (ddl_TipoParticipante.SelectedItem.Text != "TITULAR")
                {
                    if (ddl_TipoParticipante.SelectedItem.Text == "DECLARANTE")
                    {
                        DIV_TIPO_DECLARANTE.Visible = true;
                    }
                    else
                    {
                        DIV_LUGAR_DOMICILIO.Visible = false;
                        DIV_LUGAR_NACIMIENTO.Visible = false;
                        DIV_OTROS.Visible = false;
                        DIV_TIPO_DECLARANTE.Visible = false;
                    }

                }
                else
                {
                    if (esMayorEdad)
                    {
                        //chkConsignaApeCas.Visible = true;
                        lblNota.Visible = true;
                    }
                    DIV_LUGAR_DOMICILIO.Visible = true;
                    DIV_LUGAR_NACIMIENTO.Visible = true;
                    DIV_OTROS.Visible = true;
                    DIV_TIPO_DECLARANTE.Visible = false;
                }
                BloquearNombresBusqueda();
                Comun.EjecutarScript(this, "CmbGenero_VisibleApellidoCasada();chkConsignaApeCas_VerificarChecked();ObtenerElementosGenero();");
            }
        }


        //----------------------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 09/12/2016
        // Objetivo: Eliminar el registro del participante de la grilla
        //------------------------------------------------------------------------
        
        private void AnularFichaParticipante(string strTipoParticipanteId)
        {
            DataTable dtFichaParticipante = new DataTable();
            long iParticipanteId = 0;
            long iPersonaId = 0;

            if (validarGrillaParticipantes())
            {
                dtFichaParticipante = (DataTable)Session["FichaRegistralParticipante"];


                for (int i = 0; i < dtFichaParticipante.Rows.Count; i++)
                {
                    if (dtFichaParticipante.Rows[i]["ITIPO_PARTICIPANTEID"].ToString().Equals(strTipoParticipanteId))
                    {
                        iParticipanteId = Convert.ToInt64(dtFichaParticipante.Rows[i]["IPARTICIPANTEID"].ToString());
                        iPersonaId = Convert.ToInt64(dtFichaParticipante.Rows[i]["IPERSONAID"].ToString());
                        dtFichaParticipante.Rows.RemoveAt(i);
                        break;
                    }
                }
                if (iParticipanteId > 0)
                {
                    FichaRegistralParticipanteBL objFichaParticipanteBL = new FichaRegistralParticipanteBL();
                    BE.MRE.RE_FICHAREGISTRALPARTICIPANTE objFichaParticipanteBE = new BE.MRE.RE_FICHAREGISTRALPARTICIPANTE();
                    objFichaParticipanteBE.fipa_iFichaRegistralParticipanteId = iParticipanteId;
                    objFichaParticipanteBE.fipa_sTipoParticipanteId = Convert.ToInt32(strTipoParticipanteId);
                    objFichaParticipanteBE.fipa_iPersonaId = iPersonaId;
                    objFichaParticipanteBE.fipa_cEstadoId = "E";
                    objFichaParticipanteBE.fipa_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    objFichaParticipanteBE.fipa_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    objFichaParticipanteBE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                    objFichaParticipanteBL.anular(ref objFichaParticipanteBE);

                    if (!(objFichaParticipanteBE.Error))
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Ficha Registral - Participante", Constantes.CONST_MENSAJE_EXITO_ANULAR));

                        Session["FichaRegistralParticipante"] = dtFichaParticipante;
                        GridViewParticipante.DataSource = dtFichaParticipante;
                        GridViewParticipante.DataBind();
                    }
                    else
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Ficha Registral - Participante", Constantes.CONST_MENSAJE_OPERACION_FALLIDA));
                    }
                }

            }
            
        }

        //----------------------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 09/12/2016
        // Objetivo: Asignar a los controles los campos de la fila seleccionada
        //------------------------------------------------------------------------

        private void EditarFichaParticipante(string strTipoParticipanteId)
        {
            DataTable dtFichaParticipante = new DataTable();
            dtFichaParticipante = (DataTable)Session["FichaRegistralParticipante"];
            if (dtFichaParticipante.Rows.Count > 0)
            {
                for (int i = 0; i < dtFichaParticipante.Rows.Count; i++)
                {
                    if (dtFichaParticipante.Rows[i]["ITIPO_PARTICIPANTEID"].ToString().Equals(strTipoParticipanteId))
                    {
                        HFFichaTipoParticipanteId.Value = strTipoParticipanteId;

                        ddl_TipoParticipante.SelectedValue = dtFichaParticipante.Rows[i]["ITIPO_PARTICIPANTEID"].ToString();

                        if (dtFichaParticipante.Rows[i]["ITIPO_DOCUMENTO"].ToString().Length == 0)
                        {
                            ddl_TipoDocParticipante.SelectedIndex = 0;
                        }
                        else
                        {
                            ddl_TipoDocParticipante.SelectedValue = dtFichaParticipante.Rows[i]["ITIPO_DOCUMENTO"].ToString();
                        }

                        if (dtFichaParticipante.Rows[i]["ESTADOCIVIL"].ToString().Length == 0)
                        {
                            CmbEstCiv.SelectedIndex = 0;
                        }
                        else {
                            CmbEstCiv.SelectedValue = dtFichaParticipante.Rows[i]["ESTADOCIVIL"].ToString();
                        }
                        if (dtFichaParticipante.Rows[i]["GENERO"].ToString().Length == 0)
                        {
                            CmbGenero.SelectedIndex = 0;
                        }
                        else
                        {
                            CmbGenero.SelectedValue = dtFichaParticipante.Rows[i]["GENERO"].ToString();
                        }
                        if (dtFichaParticipante.Rows[i]["GRADOINSTRUCCION"].ToString().Length == 0)
                        {
                            CmbGradInst.SelectedIndex = 0;
                        }
                        else
                        {
                            CmbGradInst.SelectedValue = dtFichaParticipante.Rows[i]["GRADOINSTRUCCION"].ToString();
                        }
                       
                        txtEstatura.Text = dtFichaParticipante.Rows[i]["ESTATURA"].ToString();
                        txtAnio.Text = dtFichaParticipante.Rows[i]["ANIOESTUDIO"].ToString();
                        ddlCompleto.SelectedValue = dtFichaParticipante.Rows[i]["CONCLUIDO"].ToString();

                        txtNroDocParticipante.Text =  dtFichaParticipante.Rows[i]["VNUMERO_DOCUMENTO"].ToString().Trim();
                        txtApePatParticipante.Text = dtFichaParticipante.Rows[i]["VAPELLIDO_PATERNO"].ToString().Trim();
                        txtApeMatParticipante.Text = dtFichaParticipante.Rows[i]["VAPELLIDO_MATERNO"].ToString().Trim();
                        txtApeCasadaParticipante.Text = dtFichaParticipante.Rows[i]["VAPECASADA"].ToString().Trim();
                        hApeCasadaParticipante.Value = dtFichaParticipante.Rows[i]["VAPECASADA"].ToString().Trim();

                        txtNomParticipante.Text = dtFichaParticipante.Rows[i]["VNOMBRES"].ToString().Trim();
                        HFParticipanteId.Value = dtFichaParticipante.Rows[i]["IPARTICIPANTEID"].ToString().Trim();
                        HFpere_iResidenciaId.Value = dtFichaParticipante.Rows[i]["IRESIDENCIAID"].ToString();
                        
                        hfPersona.Value = dtFichaParticipante.Rows[i]["IPERSONAID"].ToString().Trim();
                        
                        TxtDirDir.Text = dtFichaParticipante.Rows[i]["VDIRECCION"].ToString().Trim();
                        //---------------------------------------------------
                        //Fecha: 25/02/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Asignar la dirección al control hd_DirDir
                        //----------------------------------------------------
                        hd_DirDir.Value = dtFichaParticipante.Rows[i]["VDIRECCION"].ToString().Trim();
                        //----------------------------------------------------
                        
                        TxtTelfDir.Text = dtFichaParticipante.Rows[i]["VTELEFONO"].ToString().Trim(); 

                        string strResUbigeo = dtFichaParticipante.Rows[i]["CDIRECCION_UBIGEO"].ToString().Trim();
                        setUbigeo(ddl_DeptDomicilio, ddl_ProvDomicilio, ddl_DistDomicilio, strResUbigeo);
                        //Comun.FormatearFecha(objActuacion.REGISTROCIVIL.reci_dFechaHoraOcurrenciaActo.ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                        if (dtFichaParticipante.Rows[i]["DFECHA_NACIMIENTO"].ToString().Length > 0)
                        {
                            string strFechaNacimiento = dtFichaParticipante.Rows[i]["DFECHA_NACIMIENTO"].ToString();
                            txtFechaNacimiento.set_Value = Comun.FormatearFecha(strFechaNacimiento);
                        }
                        else
                        {
                            txtFechaNacimiento.Text = "";
                        }
                        //TxtDirNac.Text = dtFichaParticipante.Rows[i]["VLUGAR_NACIMIENTO"].ToString().Trim();
                        string strNacUbigeo = dtFichaParticipante.Rows[i]["CNACIMIENTO_UBIGEO"].ToString().Trim();
                        setUbigeo(ddl_DeptNacimiento, ddl_ProvNacimiento, ddl_DistNacimiento, strNacUbigeo);

                        TxtSenasParticulares.Text = dtFichaParticipante.Rows[i]["VSENASPARTICULARES"].ToString().Trim();
                        TxtEmail.Text = dtFichaParticipante.Rows[i]["VCORREO_ELECTRONICO"].ToString().Trim();

                        txtCodPostal.Text = dtFichaParticipante.Rows[i]["CODIGO_POSTAL"].ToString().Trim();

                        SetearDiscapacidad(dtFichaParticipante.Rows[i]["DISCAPACIDAD"].ToString());
                        SetearInterdicto(dtFichaParticipante.Rows[i]["INTERDICTO"].ToString());
                        SetearDonaOrganos(dtFichaParticipante.Rows[i]["DONAORGANOS"].ToString());
                        txtCurador.Text = dtFichaParticipante.Rows[i]["NOMCURADOR"].ToString();
                        chkConsignaApeCas.Checked = Convert.ToBoolean(dtFichaParticipante.Rows[i]["bCONSIGNAMADRE"]);
                        break;
                    }
                }               
            }
            //btnGrabarParticipante.Visible = false;
            btnEditarParticipante.Visible = true;
            //btnCancelarParticipante.Visible = true;
            ctrlToolBarFicha.btnCancelar.Enabled = true;
            updFicha.Update();
        }
        //------------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 09/12/2016
        // Objetivo: Validar contenido de la grilla de participantes
        //------------------------------------------------------------

        private bool validarGrillaParticipantes()
        {
            bool existeFichaParticipante = true;

            if (Session["FichaRegistralParticipante"] == null)
            {
                existeFichaParticipante = false;
            }
            else
            {
                DataTable dtFichaParticipante = new DataTable();

                dtFichaParticipante = (DataTable)Session["FichaRegistralParticipante"];

                if (dtFichaParticipante.Rows.Count == 0)
                {
                    dtFichaParticipante.Dispose();
                    existeFichaParticipante = false;
                }
            }
            if (existeFichaParticipante == false)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Ficha Registral", "Deberá registrar por lo menos un Participante."));
            }
            return existeFichaParticipante;
        }

        //------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 09/12/2016
        // Objetivo: Obtener el código de ubigeo
        //------------------------------------------------------

        private string getUbigeo(DropDownList ddlDpto, DropDownList ddlProv, DropDownList ddlDist)
        {
            string lReturn = null;
            if (ddlDpto.SelectedIndex > 0)
            {
                int intCodigoDepa = Convert.ToInt32(ddlDpto.SelectedValue);
                int intCodigoProv = Convert.ToInt32(ddlProv.SelectedValue);
                int intCodigoDist = Convert.ToInt32(ddlDist.SelectedValue);
                lReturn = intCodigoDepa.ToString("00") + intCodigoProv.ToString("00") + intCodigoDist.ToString("00");
            }
            return lReturn;
        }

        //------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 09/12/2016
        // Objetivo: Asignar a una variable el código de ubigeo
        //------------------------------------------------------

        private void setUbigeo(DropDownList ddlDpto, DropDownList ddlProv, DropDownList ddlDist, string strUbigeo)
        {
            if (strUbigeo.Length == 6)
            {
                ddlDpto.SelectedValue = strUbigeo.Substring(0, 2);
                comun_Part3.CargarUbigeo(Session, ddlProv, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddlDpto.SelectedValue, string.Empty, true);
                if (strUbigeo.Substring(2, 2) != "00")
                {
                    ddlProv.SelectedValue = strUbigeo.Substring(2, 2);

                    comun_Part3.CargarUbigeo(Session, ddlDist, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddlDpto.SelectedValue, ddlProv.SelectedValue, true);
                    ddlDist.SelectedValue = strUbigeo.Substring(4, 2);
                }
                else
                {                                        
                    ddlProv.SelectedIndex = 0;                    
                    ddlDist.Items.Clear();
                    ddlDist.Items.Insert(0, new ListItem("- SELECCIONAR - ", "0"));
                    ddlDist.SelectedIndex = 0;
                }
            }
        }

        //------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 09/12/2016
        // Objetivo: Cargar el tipo de participante 
        // dependiendo de la tarifa
        //------------------------------------------------------

        private void CargarTipoParticipante()
        {
            BE.RE_TARIFA_PAGO objTarifaPago = new RE_TARIFA_PAGO();
            objTarifaPago = (BE.RE_TARIFA_PAGO)Session[Constantes.CONST_SESION_OBJ_TARIFA_PAGO];

            if (objTarifaPago != null)
            {
                string strTarifa = objTarifaPago.vTarifa;
                string strTarifasMayorEdad = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdad"].ToString();
                string strTarifasMenorEdad = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdad"].ToString();

                if (strTarifasMayorEdad.Trim().Length > 0)
                {
                    string[] arrTarifasMayorEdad = strTarifasMayorEdad.Trim().Split(',');

                    if (Util.ContieneItemArreglo(arrTarifasMayorEdad, strTarifa))
                    {
                        Util.CargarParametroDropDownList(ddl_TipoParticipante, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.FICHA_REGISTRAL_PARTICIPANTE_MAYOR), true);
                        Util.CargarParametroDropDownList(ddlDocAdjunto, comun_Part1.ObtenerParametrosPorGrupo(Session, SGAC.Accesorios.Constantes.CONST_DOC_ADJUNTOS_RENIEC_MAYOR), true);
                    }
                }

                if (strTarifasMenorEdad.Trim().Length > 0)
                {
                    string[] arrTarifasMenorEdad = strTarifasMenorEdad.Trim().Split(',');

                    if (Util.ContieneItemArreglo(arrTarifasMenorEdad, strTarifa))
                    {
                        Util.CargarParametroDropDownList(ddl_TipoParticipante, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.FICHA_REGISTRAL_PARTICIPANTE_MENOR), true);
                        Util.CargarParametroDropDownList(ddlDocAdjunto, comun_Part1.ObtenerParametrosPorGrupo(Session, SGAC.Accesorios.Constantes.CONST_DOC_ADJUNTOS_RENIEC_MENOR), true);
                        CmbEstCiv.Enabled = false;
                    }
                }
            }

        }


        //------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 15/12/2016
        // Objetivo: Verificar la existencia de la persona.
        //------------------------------------------------------

        [System.Web.Services.WebMethod]
        public static string GetPersonExist(Int32 tipo, String documento, String TipoParticipante)
        {
            #region Buscar persona
            EnPersona objEn = new EnPersona();
            objEn.iPersonaId = 0;
            objEn.sDocumentoTipoId = tipo;
            objEn.vDocumentoNumero = documento;
            objEn.sResidenciaTipoId = 2252;
            objEn.sTipoParticipanteId = Convert.ToInt16(TipoParticipante);
            object[] arrParametros = { objEn };
            objEn = SGAC.WebApp.Accesorios.Persona.oPersona(arrParametros);
           
            #endregion

            string person = new JavaScriptSerializer().Serialize(objEn);

            return person;
        }
        //--------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 14/12/2016
        // Objetivo: Asignar los datos de ubicación 
        //          geográfica a los controles.
        //--------------------------------------------

        protected void btnBuscarParticipante_Click(object sender, EventArgs e)
        {
            string strUbigeo = string.Empty;
            string strLNUbigeo = string.Empty;
            String LugarNacimiento = String.Empty;
            string dFechaNacimiento = HFFechaNacimiento.Value;


            if (hfPersona.Value.Trim() == "0")
            {
                txtNomParticipante.Enabled = true;
                txtApePatParticipante.Enabled = true;
                txtApeMatParticipante.Enabled = true;

                txtNomParticipante.Text = String.Empty;
                txtApePatParticipante.Text = String.Empty;
                txtApeMatParticipante.Text = String.Empty;
                //TxtDirNac.Text = String.Empty;
                //TxtDirNac.Enabled = true;
                TxtDirDir.Text = String.Empty;
                TxtDirDir.Enabled = true;
                hd_DirDir.Value = string.Empty;
                TxtTelfDir.Text = String.Empty;
                TxtTelfDir.Enabled = true;
                TxtSenasParticulares.Text = String.Empty;
                TxtSenasParticulares.Enabled = true;
                TxtEmail.Text = String.Empty;
                TxtEmail.Enabled = true;
            }
            else
            {
                txtNomParticipante.Enabled = false;
                txtApePatParticipante.Enabled = false;
                if (txtApeMatParticipante.Text != string.Empty)
                    txtApeMatParticipante.Enabled = false;
            }
            if (txtApeMatParticipante.Text == string.Empty)
            { txtApeMatParticipante.Enabled = true; }
            else
            { txtApeMatParticipante.Enabled = false; }

            //-----------------------------------------------------------
            // Datos de Nacimiento
            //-----------------------------------------------------------
            if (dFechaNacimiento.Trim().Length != 0 && dFechaNacimiento != "0")
            {
                txtFechaNacimiento.Text = Comun.FormatearFecha(dFechaNacimiento).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
            }
            else
            {
                txtFechaNacimiento.Enabled = true;
            }


            
            //-----------------------------------------------------------
            // Datos de Residencia
            //-----------------------------------------------------------
            if (HFDep.Value.Length == 1)
            {
                HFDep.Value = "0" + HFDep.Value;
            }
            if (HFProv.Value.Length == 1)
            {
                HFProv.Value = "0" + HFProv.Value;
            }
            if (HFdist.Value.Length == 1)
            {
                HFdist.Value = "0" + HFdist.Value;
            }
            strUbigeo = HFDep.Value + HFProv.Value + HFdist.Value;
            if (strUbigeo.Length == 6)
            {
                if (strUbigeo != "000000")
                {
                    ddl_DeptDomicilio.SelectedValue = HFDep.Value;
                    comun_Part3.CargarUbigeo(Session, ddl_ProvDomicilio, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_DeptDomicilio.SelectedValue, string.Empty, true);
                    if (HFProv.Value != "00")
                    {
                        ddl_ProvDomicilio.SelectedValue = HFProv.Value;
                    }

                    comun_Part3.CargarUbigeo(Session, ddl_DistDomicilio, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddl_DeptDomicilio.SelectedValue, ddl_ProvDomicilio.SelectedValue, true);
                    if (HFdist.Value != "00")
                    {
                        ddl_DistDomicilio.SelectedValue = HFdist.Value;
                    }
                }
                else
                {
                    this.ddl_DeptDomicilio.SelectedIndex = 0;
                }
            }
            else
            {
                this.ddl_DeptDomicilio.SelectedIndex = 0;
            }

            //-----------------------------------------------------------
            //Autor: Miguel Márquez Beltrán
            //Fecha: 26/01/2017
            //Objetivo: Asignar el Ubigeo del Lugar de Nacimiento
            //-----------------------------------------------------------            
            strLNUbigeo = HFLNDep.Value + HFLNProv.Value + HFLNdist.Value;
            if (strLNUbigeo.Length == 6)
            {
                if (strLNUbigeo != "000000")
                {
                    ddl_DeptNacimiento.SelectedValue = HFLNDep.Value;
                    comun_Part3.CargarUbigeo(Session, ddl_ProvNacimiento, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_DeptNacimiento.SelectedValue, string.Empty, true);
                    if (HFLNProv.Value != "00")
                    {
                        ddl_ProvNacimiento.SelectedValue = HFLNProv.Value;
                    }

                    comun_Part3.CargarUbigeo(Session, ddl_DistNacimiento, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddl_DeptNacimiento.SelectedValue, ddl_ProvNacimiento.SelectedValue, true);
                    if (HFLNdist.Value != "00")
                    {
                        ddl_DistNacimiento.SelectedValue = HFLNdist.Value;
                    }
                }
                else
                {
                    this.ddl_DeptNacimiento.SelectedIndex = 0;
                }
            }
            else
            {
                this.ddl_DeptNacimiento.SelectedIndex = 0;
            }
            //if (hNuevoParticipante.Value != "1")
            //{
            //    Comun.EjecutarScript(Page, "desabilitarBotonesEditar();");
            //}
        }

        //--------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 15/12/2016
        // Objetivo: Estado de las opciones
        //          de la ficha registral
        //--------------------------------------------

        private void EstadoBarraFichaRegistral(bool bEstado)
        {
            //ctrlToolBarFicha.btnNuevo.Enabled = bEstado;
            ctrlToolBarFicha.btnGrabar.Enabled = bEstado;
            ctrlToolBarFicha.btnImprimir.Enabled = bEstado;
            ctrlToolBarFicha.btnCancelar.Enabled = !bEstado;            
        }

        //--------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 15/12/2016
        // Objetivo: Estado de todas las opciones
        //          de la ficha registral
        //--------------------------------------------

        private void EstadoNormalBarraFichaRegistral(bool bEstado)
        {
            //ctrlToolBarFicha.btnNuevo.Enabled = bEstado;
            ctrlToolBarFicha.btnGrabar.Enabled = bEstado;
            ctrlToolBarFicha.btnImprimir.Enabled = bEstado;
            ctrlToolBarFicha.btnCancelar.Enabled = !bEstado;            
        }

        //----------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 15/12/2016
        // Objetivo: Validación de la ficha de participantes
        //----------------------------------------------------

        private bool ValidacionFichaParticipantes(string strTarifas_TipoDocumento, string strTarifas_NumeroDocumento, string strTarifas_PrimerApellido, string strTarifas_SegundoApellido,
                                                  string strTarifas_Nombres, string strTarifas_Domicilio, string strTarifas_UbigeoDomicilio, string strTarifas_FechaNacimiento,
                                                  string strTarifas_LugarNacimiento, string strTarifas_UbigeoNacimiento)
        {
            bool bCorrecto = true;
            
            BE.RE_TARIFA_PAGO objTarifaPago = new RE_TARIFA_PAGO();
            objTarifaPago = (BE.RE_TARIFA_PAGO)Session[Constantes.CONST_SESION_OBJ_TARIFA_PAGO];
            string strTarifa = objTarifaPago.vTarifa.Trim().ToUpper();

            string[] arrTarifas_TipoDocumento = strTarifas_TipoDocumento.Trim().Split(',');
            if (Util.ContieneItemArreglo(arrTarifas_TipoDocumento, strTarifa))
            {
                if (ddl_TipoDocParticipante.SelectedIndex == 0)
                {
                    //Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Participante", "Seleccione el tipo de documento del participante"));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Participante", "alert('Seleccione el tipo de documento del participante');", true); 
                    ddl_TipoDocParticipante.Focus();
                    return false;
                }
            }

            string[] arrTarifas_NumeroDocumento = strTarifas_NumeroDocumento.Trim().Split(',');
            if (Util.ContieneItemArreglo(arrTarifas_NumeroDocumento, strTarifa))
            {
                if (txtNroDocParticipante.Text.Trim() == "")
                {
                    //Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Participante", "Digite el Número de documento del Participante"));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Participante", "alert('Digite el Número de documento del Participante');", true); 
                    txtNroDocParticipante.Focus();
                    return false;
                }
            }

            //string[] arrTarifas_PrimerApellido = strTarifas_PrimerApellido.Trim().Split(',');
            //if (Util.ContieneItemArreglo(arrTarifas_PrimerApellido, strTarifa))
            //{
            //    if (txtApePatParticipante.Text.Trim() == "")
            //    {
            //        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Participante", "Digite el Primer Apellido del Participante"));
            //        txtApePatParticipante.Focus();
            //        return false;
            //    }
            //}

            //string[] arrTarifas_SegundoApellido = strTarifas_SegundoApellido.Trim().Split(',');
            //if (Util.ContieneItemArreglo(arrTarifas_SegundoApellido, strTarifa))
            //{
            //    if (txtApeMatParticipante.Text.Trim() == "" && ddl_TipoDocParticipante.Text == "DNI")
            //    {
            //        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Participante", "Digite el Segundo Apellido del Participante"));
            //        txtApePatParticipante.Focus();
            //        return false;
            //    }
            //}

            //string[] arrTarifas_Nombres = strTarifas_Nombres.Trim().Split(',');
            //if (Util.ContieneItemArreglo(arrTarifas_Nombres, strTarifa))
            //{
            //    if (txtNomParticipante.Text.Trim() == "")
            //    {
            //        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Participante", "Digite el Nombre del Participante"));
            //        txtNomParticipante.Focus();
            //        return false;
            //    }
            //}

            //string[] arrTarifas_Domicilio = strTarifas_Domicilio.Trim().Split(',');
            //if (Util.ContieneItemArreglo(arrTarifas_Domicilio, strTarifa))
            //{
            //    if (TxtDirDir.Text.Trim() == "")
            //    {
            //        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Participante", "Digite la Dirección del Participante"));
            //        TxtDirDir.Focus();
            //        return false;
            //    }
            //}

            //string[] arrTarifas_UbigeoDomicilio = strTarifas_UbigeoDomicilio.Trim().Split(',');
            //if (Util.ContieneItemArreglo(arrTarifas_UbigeoDomicilio, strTarifa))
            //{
            //    if (ddl_DeptDomicilio.SelectedIndex == 0)
            //    {
            //        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Participante", "Seleccione el Dpto./Continente del domicilio"));
            //        ddl_DeptDomicilio.Focus();
            //        return false;
            //    }

            //    if (ddl_ProvDomicilio.SelectedIndex == 0)
            //    {
            //        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Participante", "Seleccione el Prov./País del domicilio"));
            //        ddl_ProvDomicilio.Focus();
            //        return false;
            //    }
            //    if (ddl_DistDomicilio.SelectedIndex == 0)
            //    {
            //        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Participante", "Seleccione el Dist./Ciudad/Estado del domicilio"));
            //        return false;
            //    }
            //}

            //string[] arrTarifas_FechaNacimiento = strTarifas_FechaNacimiento.Trim().Split(',');
            //if (Util.ContieneItemArreglo(arrTarifas_FechaNacimiento, strTarifa))
            //{
            //    if (txtFechaNacimiento.Text.Trim() == "")
            //    {
            //        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Participante", "Seleccione la fecha de nacimiento del participante"));
            //        txtFechaNacimiento.Focus();
            //        return false;
            //    }
            //}
           

            //string[] arrTarifas_UbigeoNacimiento = strTarifas_UbigeoNacimiento.Trim().Split(',');
            //if (Util.ContieneItemArreglo(arrTarifas_UbigeoNacimiento, strTarifa))
            //{
            //    if (ddl_DeptNacimiento.SelectedIndex == 0)
            //    {
            //        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Participante", "Seleccione el Dpto./Continente de Nacimiento"));
            //        ddl_DeptNacimiento.Focus();
            //        return false;
            //    }

            //    if (ddl_ProvNacimiento.SelectedIndex == 0)
            //    {
            //        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Participante", "Seleccione el Prov./País del Nacimiento"));
            //        ddl_ProvNacimiento.Focus();
            //        return false;
            //    }
            //    if (ddl_DistNacimiento.SelectedIndex == 0)
            //    {
            //        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Participante", "Seleccione el Dist./Ciudad/Estado del Nacimiento"));
            //        ddl_DistNacimiento.Focus();
            //        return false;
            //    }
            //}

            return bCorrecto;
        }

        //----------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 15/12/2016
        // Objetivo: Verificar de acuerdo a la tarifa si
        //         es un tipo de participante mayor de edad
        //----------------------------------------------------
       
        private bool EsMayorEdad()
        {
            bool bMayor = false;

            BE.RE_TARIFA_PAGO objTarifaPago = new RE_TARIFA_PAGO();
            objTarifaPago = (BE.RE_TARIFA_PAGO)Session[Constantes.CONST_SESION_OBJ_TARIFA_PAGO];

            if (objTarifaPago != null)
            {
                string strTarifa = objTarifaPago.vTarifa;
                string strTarifasMayorEdad = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdad"].ToString();
                string strTarifasMenorEdad = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdad"].ToString();

                if (strTarifasMayorEdad.Trim().Length > 0)
                {
                    string[] arrTarifasMayorEdad = strTarifasMayorEdad.Trim().Split(',');

                    if (Util.ContieneItemArreglo(arrTarifasMayorEdad, strTarifa))
                    {
                        return true;
                    }
                }

                if (strTarifasMenorEdad.Trim().Length > 0)
                {
                    string[] arrTarifasMenorEdad = strTarifasMenorEdad.Trim().Split(',');

                    if (Util.ContieneItemArreglo(arrTarifasMenorEdad, strTarifa))
                    {
                        return false;
                    }
                }
            }
            return bMayor;
        }

        //--------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 16/12/2016
        // Objetivo: Consultar la participantes de la ficha registral
        //--------------------------------------------------------------

        private void ConsultarParticipantesFichaRegistral(long iFichaRegistralId)
        {
            //--------------------------------------------------
            DataTable dtFichaParticipante = new DataTable();
            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = 7;
            int intPaginaActual = 1;

            FichaRegistralParticipanteBL objFichaParticipanteBL = new FichaRegistralParticipanteBL();

            dtFichaParticipante = objFichaParticipanteBL.Consultar(iFichaRegistralId, intPaginaActual, intPaginaCantidad, ref IntTotalCount, ref IntTotalPages);
            if (dtFichaParticipante.Rows.Count > 0)
            {
                lblListaParticipantes.Visible = true;
                ctrlToolBarFicha.btnImprimir.Enabled = true;
                Session["FichaRegistralParticipante"] = dtFichaParticipante;

                //if (dtFichaParticipante.Rows.Count == 1)
                //{
                //    string strDireccion = dtFichaParticipante.Rows[0]["VDIRECCION"].ToString();
                //    if (strDireccion.Length == 0)
                //    {
                //        hNuevaDireccion.Value = "1";
                //        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "FICHA REGISTRAL", "INGRESE LA DIRECCIÓN DEL PARTICIPANTE", false, 190, 250));
                //    }
                //    else { hNuevaDireccion.Value = "0"; }
                //}
                GridViewParticipante.DataSource = dtFichaParticipante;
                GridViewParticipante.DataBind();
            }
            else
            {
                lblListaParticipantes.Visible = false;
                Session["FichaRegistralParticipante"] = null;
                GridViewParticipante.DataSource = null;
                GridViewParticipante.DataBind();                
            }
            //updFicha.Update();
        }

        //--------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 22/12/2016
        // Objetivo: Limpiar los datos del participante
        //--------------------------------------------------------------

        private void LimpiarDatosParticipantes(bool busquedaSinExito = false)
        {
            //----------------------------------------------
            // Datos del Participante de la Ficha Registral
            //----------------------------------------------
            if (!busquedaSinExito)
            {
                ddl_TipoParticipante.SelectedIndex = 0;
                ddl_TipoDocParticipante.SelectedIndex = 0;
                txtNroDocParticipante.Text = "";
            }
            
            txtApePatParticipante.Text = "";
            txtApePatParticipante.Enabled = true;
            txtApeMatParticipante.Text = "";
            txtApeMatParticipante.Enabled = true;
            txtApeCasadaParticipante.Text = "";
            hApeCasadaParticipante.Value = "";
            txtApeCasadaParticipante.Enabled = true;
            txtNomParticipante.Text = "";
            txtNomParticipante.Enabled = true;
            txtFechaNacimiento.Text = "";
            ddlDeclarante.ClearSelection();
            ddlTutorG.ClearSelection();
            //---------------------
            // Lugar de Domicilio
            //---------------------
            TxtDirDir.Text = "";
            hd_DirDir.Value = "";
            TxtTelfDir.Text = "";
            TxtSenasParticulares.Text = "";
            TxtEmail.Text = "";
            ddl_DeptDomicilio.ClearSelection();
            ddl_ProvDomicilio.ClearSelection();
            ddl_DistDomicilio.ClearSelection();
            //---------------------
            // Lugar de Nacimiento
            //---------------------
            //TxtDirNac.Text = "";
            ddl_DeptNacimiento.ClearSelection();
            ddl_ProvNacimiento.ClearSelection();
            ddl_DistNacimiento.ClearSelection();

            //-----------------
            // CAMPOS NUEVOS
            //-----------------
            CmbEstCiv.ClearSelection();
            CmbGenero.ClearSelection();
            CmbGradInst.ClearSelection();
            txtAnio.Text = "";
            ddlCompleto.ClearSelection();
            txtEstatura.Text = "";


            rbSi_Discapacidad.Checked = false;
            rbNO_Discapacidad.Checked = false;
            rbSi_Interdicto.Checked = false;
            rbNO_Interdicto.Checked = false;
            txtCurador.Text = "";
            rbSi_Donador.Checked = false;
            rbNO_Donador.Checked = false;

            HFFichaTipoParticipanteId.Value = "0";
            HFParticipanteId.Value = "0";
            hfPersona.Value = "0";
            HFpere_iResidenciaId.Value = "0";
            txtCodPostal.Text = "";
        }

        
        //--------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 22/12/2016
        // Objetivo: Establecer el estado de la ficha registral
        //--------------------------------------------------------------

        private void EstablecerSecuenciaEstadoFicha(DropDownList ddlEstado_Ficha, string strValor)
        {                  
            //ddlEstado_Ficha

            //REGISTRADO
            //ANULADO
            //ENVIADO
            //OBSERVADO
            //RECUPERADO
            //REPROCESO
            //REIMPRESIÓN
            //INCOMPLETO

            ddlEstado_Ficha.Items.Clear();

            DataTable dtEstado = new DataTable();

            //dtEstado = Comun.ObtenerParametrosPorGrupo((DataTable)Session[Constantes.CONST_SESION_DT_ESTADO], SGAC.Accesorios.Constantes.CONST_FICHA_ESTADO);
            dtEstado = comun_Part1.ObtenerParametrosPorGrupoMRE(SGAC.Accesorios.Constantes.CONST_FICHA_ESTADO);

            ListItem listaItems = new ListItem();

            string strtexto = "";
            string strvalue = "";
            string strTextoInicial = "";

            for (int i = 0; i < dtEstado.Rows.Count; i++)
            {
                strtexto = dtEstado.Rows[i]["VALOR"].ToString().Trim();
                strvalue = dtEstado.Rows[i]["ID"].ToString().Trim();
                if (strValor == "RECUPERADO CON FICHA")
                {
                    if (dtEstado.Rows[i]["VALOR"].ToString().Trim().Equals("RECUPERADO CON FICHA"))
                    {
                        listaItems = new ListItem(strtexto, strvalue);
                        ddlEstado_Ficha.Items.Add(listaItems);
                    }
                }
                else
                {
                    if (strValor == "REGISTRADO")
                    {
                        if (dtEstado.Rows[i]["VALOR"].ToString().Trim().Equals("REGISTRADO"))
                        {
                            listaItems = new ListItem(strtexto, strvalue);
                            ddlEstado_Ficha.Items.Add(listaItems);
                        }
                    }
                    else
                    {
                        if (strValor.Length > 0)
                        {
                            if (dtEstado.Rows[i]["ID"].ToString().Trim() == strValor)
                            {
                                strTextoInicial = dtEstado.Rows[i]["VALOR"].ToString().Trim();
                            }
                        }
                        else
                        {
                            if (dtEstado.Rows[i]["VALOR"].ToString().Trim().Equals("REGISTRADO"))
                            {
                                listaItems = new ListItem(strtexto, strvalue);
                                ddlEstado_Ficha.Items.Add(listaItems);
                            }
                            else
                            {
                                if (dtEstado.Rows[i]["VALOR"].ToString().Trim().Equals("RECUPERADO CON FICHA"))
                                {
                                    listaItems = new ListItem(strtexto, strvalue);
                                    ddlEstado_Ficha.Items.Add(listaItems);
                                }

                            }
                        }
                    }
                    
                }
            }
            if (strValor.Length > 0)
            {
                #region AsignarEstados
                for (int i = 0; i < dtEstado.Rows.Count; i++)
                {
                    strtexto = dtEstado.Rows[i]["VALOR"].ToString().Trim();
                    strvalue = dtEstado.Rows[i]["ID"].ToString().Trim();

                    if (strTextoInicial == "REGISTRADO")
                    {
                        if (strtexto == "REGISTRADO" || strtexto == "ANULADO" || strtexto == "INCOMPLETO" || strtexto == "ENVIADO")
                        {
                            listaItems = new ListItem(strtexto, strvalue);
                            ddlEstado_Ficha.Items.Add(listaItems);
                        }
                    }
                    if (strTextoInicial == "INCOMPLETO")
                    {
                        if (strtexto == "INCOMPLETO" || strtexto == "REGISTRADO")
                        {
                            listaItems = new ListItem(strtexto, strvalue);
                            ddlEstado_Ficha.Items.Add(listaItems);
                        }
                    }
                    if (strTextoInicial == "ENVIADO")
                    {
                        if (strtexto == "ENVIADO" || strtexto == "OBSERVADO" || strtexto == "REPROCESO" || strtexto == "REIMPRESIÓN" ) //|| strtexto == "REGISTRADO")
                        {
                            listaItems = new ListItem(strtexto, strvalue);
                            ddlEstado_Ficha.Items.Add(listaItems);
                        }
                    }
                    if (strTextoInicial == "ANULADO")
                    {
                        if (strtexto == "ANULADO")
                        {
                            listaItems = new ListItem(strtexto, strvalue);
                            ddlEstado_Ficha.Items.Add(listaItems);
                        }
                    }
                    if (strTextoInicial == "REPROCESO")
                    {
                        if (strtexto == "REPROCESO")
                        {
                            listaItems = new ListItem(strtexto, strvalue);
                            ddlEstado_Ficha.Items.Add(listaItems);
                        }
                    }
                    if (strTextoInicial == "REIMPRESIÓN")
                    {
                        if (strtexto == "REIMPRESIÓN")
                        {
                            listaItems = new ListItem(strtexto, strvalue);
                            ddlEstado_Ficha.Items.Add(listaItems);
                        }
                    }
                    if (strTextoInicial == "RECUPERADO")
                    {
                        if (strtexto == "RECUPERADO")
                        {
                            listaItems = new ListItem(strtexto, strvalue);
                            ddlEstado_Ficha.Items.Add(listaItems);
                        }
                    }


                    if (strTextoInicial == "OBSERVADO")
                    {
                        if (strtexto == "OBSERVADO" || strtexto == "RECUPERADO")
                        {
                            listaItems = new ListItem(strtexto, strvalue);
                            ddlEstado_Ficha.Items.Add(listaItems);
                        }
                    }
                    if (strTextoInicial == "RECUPERADO CON FICHA")
                    {
                        if (strtexto == "RECUPERADO CON FICHA")
                        {
                            listaItems = new ListItem(strtexto, strvalue);
                            ddlEstado_Ficha.Items.Add(listaItems);
                        }
                    }
                    
            #endregion
             }
            }
        }

        //------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 11/01/2017
        // Objetivo: Imprimir la ficha Registral
        //------------------------------------------------------

        [System.Web.Services.WebMethod]
        private static Boolean ImprimirFichaRegistral(SGAC.BE.MRE.Custom.CBE_FICHAREGISTRAL objFichaRegistralBE, string strNombreFile, bool bolEsMayorEdad,DataTable _dtDocumentos)
        {
            try
            {

                StringBuilder sScript = new StringBuilder();
                Boolean Resultado = false;
                string strRutaHtml = string.Empty;

                String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];

                string strRutaPDF = uploadPath + @"\" + strNombreFile + "_" + DateTime.Now.Ticks.ToString() + ".pdf";
                strRutaHtml = uploadPath + @"\" + strNombreFile + "_" + DateTime.Now.Ticks.ToString() + ".html";

                StreamWriter str = new StreamWriter(strRutaHtml, true, Encoding.Default);

                str.Write("<p align=\"justify\" style=\"background-color: transparent;\">Hola</p>");
                str.Dispose();

                if (bolEsMayorEdad)
                {
                    CreateFilePDFFichaRegistralMayorEdad(objFichaRegistralBE, strRutaHtml, strRutaPDF, _dtDocumentos);
                }
                else
                {
                    CreateFilePDFFichaRegistralMenorEdad(objFichaRegistralBE, strRutaHtml, strRutaPDF, _dtDocumentos);
                }

                if (System.IO.File.Exists(strRutaPDF))
                {
                    Resultado = true;
                }
                if (File.Exists(strRutaHtml))
                {
                    File.Delete(strRutaHtml);
                }
                
                HttpContext.Current.Session["rutaPDF"] = strRutaPDF;

                return Resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 11/01/2017
        // Objetivo: Crea el archivo PDF de la ficha Registral 
        //           para mayores de edad.
        //------------------------------------------------------

        private static void CreateFilePDFFichaRegistralMayorEdad(SGAC.BE.MRE.Custom.CBE_FICHAREGISTRAL objFichaRegistralBE, string HtmlPath, string PdfPath,DataTable _dtDocumentos)
        {
            try
            {
                if (!File.Exists(HtmlPath))
                    return;
                if (File.Exists(PdfPath))
                    File.Delete(PdfPath);

                float fMargenIzquierdaDoc = 80;
                float fMargenDerechaDoc = 80;


                /*PARAMETRO DE MARGENES*/
                string sTipoDocumento = "0";
                sTipoDocumento = comun_Part1.ObtenerParametroDatoPorCampo(HttpContext.Current.Session, SGAC.Accesorios.Constantes.CONST_DOCUMENTOS_IMPRESION, SGAC.Accesorios.Constantes.CONST_DOC_FICHA_MAYOR_EDAD, "id");
                TablaMaestraConsultaBL obj = new TablaMaestraConsultaBL();
                DataTable dt = obj.ConsultarMargenesDocumento(Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), Convert.ToInt16(sTipoDocumento));

                Int16 MargenX_1 = 0;
                Int16 MargenY_1 = 0;
                Int16 MargenX_2 = 0;
                Int16 MargenY_2 = 0;
                Int16 MargenX_3 = 0;
                Int16 MargenY_3 = 0;
                Int16 MargenX_4 = 0;
                Int16 MargenY_4 = 0;

                foreach (DataRow row in dt.Rows)
                {
                    if (row["mado_sSeccion"].ToString() == "1")
                    {
                        MargenX_1 = Convert.ToInt16(row["mado_sMargenIzquierdo"].ToString());
                        MargenY_1 = Convert.ToInt16(row["mado_sMargenSuperior"].ToString());
                    }
                    if (row["mado_sSeccion"].ToString() == "2")
                    {
                        MargenX_2 = Convert.ToInt16(row["mado_sMargenIzquierdo"].ToString());
                        MargenY_2 = Convert.ToInt16(row["mado_sMargenSuperior"].ToString());
                    }
                    if (row["mado_sSeccion"].ToString() == "3")
                    {
                        MargenX_3 = Convert.ToInt16(row["mado_sMargenIzquierdo"].ToString());
                        MargenY_3 = Convert.ToInt16(row["mado_sMargenSuperior"].ToString());
                    }
                    if (row["mado_sSeccion"].ToString() == "4")
                    {
                        MargenX_4 = Convert.ToInt16(row["mado_sMargenIzquierdo"].ToString());
                        MargenY_4 = Convert.ToInt16(row["mado_sMargenSuperior"].ToString());
                    }
                }

                /*FIN PARAMETROS MARGENES*/

                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, fMargenIzquierdaDoc, fMargenDerechaDoc, 100, 80);
                StreamReader oStreamReader = new StreamReader(HtmlPath, System.Text.Encoding.Default);


                iTextSharp.text.FontFactory.RegisterDirectories();

                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(PdfPath, FileMode.Create));

                document.Open();

                document.NewPage();

                

                iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                iTextSharp.text.pdf.BaseFont bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);
                
                cb.SetFontAndSize(bfTimes, 10);
                cb.BeginText();
                //------------------------------------------


                #region parte superior primera cara
                float iMayorEdadTitularCodigoLocal_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.CodigoLocal_X"].ToString());
                float iMayorEdadTitularCodigoLocal_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.CodigoLocal_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularCodigoLocal_X, iMayorEdadTitularCodigoLocal_Y + MargenY_1, objFichaRegistralBE.strCodigoLocal, cb, document);

                /*Jonatan 31/05/2017 - Se agrega dni en el formato*/
                if (objFichaRegistralBE.strTipoDocTitular == "DNI")
                {
                    float iMayorEdadTitularDNI_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.DNI_X"].ToString());
                    float iMayorEdadTitularDNI_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.DNI_Y"].ToString());

                    EscribirLetraxLetra(iMayorEdadTitularDNI_X + MargenX_1, iMayorEdadTitularDNI_Y + MargenY_1, objFichaRegistralBE.strNroDocTitular, cb, document);

                    float iMayorEdadTitularDNI_Desglosable_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.DNI_DESGLOSABLE_X"].ToString());
                    float iMayorEdadTitularDNI_Desglosable_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.DNI_DESGLOSABLE_Y"].ToString());

                    EscribirLetraxLetra(iMayorEdadTitularDNI_Desglosable_X + MargenX_1, iMayorEdadTitularDNI_Desglosable_Y + MargenY_1, objFichaRegistralBE.strNroDocTitular, cb, document);
                }

                float iMayorEdadTitularApellidos_DESGLOSABLE_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.Apellidos_DESGLOSABLE_X"].ToString());
                float iMayorEdadTitularApellidos_DESGLOSABLE_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.Apellidos_DESGLOSABLE_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularApellidos_DESGLOSABLE_X + MargenX_1, iMayorEdadTitularApellidos_DESGLOSABLE_Y + MargenY_1, objFichaRegistralBE.strApPaternoTitular + " " + objFichaRegistralBE.strApMaternoTitular + " " + objFichaRegistralBE.strApCasadaTitular, cb, document);

                float iMayorEdadTitularNombres_DESGLOSABLE_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.Nombres_DESGLOSABLE_X"].ToString());
                float iMayorEdadTitularNombres_DESGLOSABLE_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.Nombres_DESGLOSABLE_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularNombres_DESGLOSABLE_X + MargenX_1, iMayorEdadTitularNombres_DESGLOSABLE_Y + MargenY_1, objFichaRegistralBE.strNombresTitular, cb, document);

                //------------------------------------------
                float iMayorEdadTitularFechaRegistro_dia_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.FechaRegistro_dia_X"].ToString());
                float iMayorEdadTitularFechaRegistro_dia_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.FechaRegistro_dia_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularFechaRegistro_dia_X + MargenX_1, iMayorEdadTitularFechaRegistro_dia_Y + MargenY_1, objFichaRegistralBE.strFechaRegistro_DD, cb, document);

                float iMayorEdadTitularFechaRegistro_mes_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.FechaRegistro_mes_X"].ToString());
                float iMayorEdadTitularFechaRegistro_mes_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.FechaRegistro_mes_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularFechaRegistro_mes_X + MargenX_1, iMayorEdadTitularFechaRegistro_mes_Y + MargenY_1, objFichaRegistralBE.strFechaRegistro_MM, cb, document);

                float iMayorEdadTitularFechaRegistro_anio_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.FechaRegistro_anio_X"].ToString());
                float iMayorEdadTitularFechaRegistro_anio_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.FechaRegistro_anio_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularFechaRegistro_anio_X + MargenX_1, iMayorEdadTitularFechaRegistro_anio_Y + MargenY_1, objFichaRegistralBE.strFechaRegistro_YYYY, cb, document);
                //------------------------------------------
                float iMayorEdadTitularApPaterno_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.ApPaterno_X"].ToString());
                float iMayorEdadTitularApPaterno_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.ApPaterno_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularApPaterno_X + MargenX_1, iMayorEdadTitularApPaterno_Y + MargenY_1, objFichaRegistralBE.strApPaternoTitular, cb, document);

                float iMayorEdadTitularApMaterno_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.ApMaterno_X"].ToString());
                float iMayorEdadTitularApMaterno_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.ApMaterno_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularApMaterno_X + MargenX_1, iMayorEdadTitularApMaterno_Y + MargenY_1, objFichaRegistralBE.strApMaternoTitular, cb, document);

                //JONATAN
                float iMayorEdadTitularApCasada_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.ApCasada_X"].ToString());
                float iMayorEdadTitularApCasada_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.ApCasada_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularApCasada_X + MargenX_1, iMayorEdadTitularApCasada_Y + MargenY_1, objFichaRegistralBE.strApCasadaTitular, cb, document);

                float iMayorEdadTitularNombres_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.Nombres_X"].ToString());
                float iMayorEdadTitularNombres_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.Nombres_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularNombres_X + MargenX_1, iMayorEdadTitularNombres_Y + MargenY_1, objFichaRegistralBE.strNombresTitular, cb, document);
                //------------------------------------------

                float iMayorEdadTitularDirDpto_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.DirDpto_X"].ToString());
                float iMayorEdadTitularDirDpto_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.DirDpto_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularDirDpto_X + MargenX_1, iMayorEdadTitularDirDpto_Y + MargenY_1, objFichaRegistralBE.strDirDptoTitular, cb, document);

                float iMayorEdadTitularDirCodDpto_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.DirCodDpto_X"].ToString());
                float iMayorEdadTitularDirCodDpto_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.DirCodDpto_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularDirCodDpto_X + MargenX_1, iMayorEdadTitularDirCodDpto_Y + MargenY_1, objFichaRegistralBE.strDirCodDptoTitular, cb, document);

                float iMayorEdadTitularDirProv_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.DirProv_X"].ToString());
                float iMayorEdadTitularDirProv_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.DirProv_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularDirProv_X + MargenX_1, iMayorEdadTitularDirProv_Y + MargenY_1, objFichaRegistralBE.strDirProvTitular, cb, document);

                float iMayorEdadTitularDirCodProv_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.DirCodProv_X"].ToString());
                float iMayorEdadTitularDirCodProv_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.DirCodProv_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularDirCodProv_X + MargenX_1, iMayorEdadTitularDirCodProv_Y + MargenY_1, objFichaRegistralBE.strDirCodProvTitular, cb, document);

                float iMayorEdadTitularDirDist_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.DirDist_X"].ToString());
                float iMayorEdadTitularDirDist_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.DirDist_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularDirDist_X + MargenX_1, iMayorEdadTitularDirDist_Y + MargenY_1, objFichaRegistralBE.strDirDistTitular, cb, document);

                float iMayorEdadTitularDirCodDist_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.DirCodDist_X"].ToString());
                float iMayorEdadTitularDirCodDist_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.DirCodDist_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularDirCodDist_X + MargenX_1, iMayorEdadTitularDirCodDist_Y + MargenY_1, objFichaRegistralBE.strDirCodDistTitular, cb, document);

                //------------------------------------------
                float iMayorEdadTitularDireccion_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.Direccion_X"].ToString());
                float iMayorEdadTitularDireccion_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.Direccion_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularDireccion_X + MargenX_1, iMayorEdadTitularDireccion_Y + MargenY_1, objFichaRegistralBE.strDireccionTitular, cb, document);

                //------------------------------------------
                float iMayorEdadCodigoPostal_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.CodigoPostal_X"].ToString());
                float iMayorEdadCodigoPostal_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.CodigoPostal_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadCodigoPostal_X + MargenX_1, iMayorEdadCodigoPostal_Y + MargenY_1, objFichaRegistralBE.strCodigoPostalResidencia, cb, document);


                //------------------------------------------
                float iMayorEdadTitularSenasParticulares_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.SenasParticulares_X"].ToString());
                float iMayorEdadTitularSenasParticulares_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.SenasParticulares_Y"].ToString());

                cb.SetFontAndSize(bfTimes, 7);
                EscribirLetraxLetra(iMayorEdadTitularSenasParticulares_X + MargenX_1, iMayorEdadTitularSenasParticulares_Y + MargenY_1, objFichaRegistralBE.strSenasParticularesTitular, cb, document);
                           
                #endregion


                #region parte inferior primera cara
                //------------------------------------------   

                /*Nuevos Campos*/
                //------------------------------------------
                /*
                 --ESTADO CIVIL
                 */
                DataTable dtImpresionCompleta = new DataTable();

                dtImpresionCompleta = comun_Part1.ObtenerParametrosPorGrupo(HttpContext.Current.Session, SGAC.Accesorios.Constantes.CONST_IMPRESION_COMPLETA);

                //DataTable dtImpresionCompleta = Comun.ObtenerParametrosPorGrupo((DataTable)HttpContext.Current.Session[Constantes.CONST_SESION_DT_PARAMETRO], SGAC.Accesorios.Constantes.CONST_IMPRESION_COMPLETA);

                if (dtImpresionCompleta.Rows[0]["descripcion"].ToString() == "SI")
                {
                    float iMayorEdadTitularEstadoCivil_X = 0;
                    float iMayorEdadTitularEstadoCivil_Y = 0;
                    if (objFichaRegistralBE.strEstadoCivil == Enumerador.enmEstadoCivil.SOLTERO.ToString())
                    {
                        iMayorEdadTitularEstadoCivil_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.EstadoCivil_SOLTERO_X"].ToString());
                        iMayorEdadTitularEstadoCivil_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.EstadoCivil_SOLTERO_Y"].ToString());
                    }
                    if (objFichaRegistralBE.strEstadoCivil == Enumerador.enmEstadoCivil.CASADO.ToString())
                    {
                        iMayorEdadTitularEstadoCivil_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.EstadoCivil_CASADO_X"].ToString());
                        iMayorEdadTitularEstadoCivil_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.EstadoCivil_CASADO_Y"].ToString());
                    }
                    if (objFichaRegistralBE.strEstadoCivil == Enumerador.enmEstadoCivil.VIUDO.ToString())
                    {
                        iMayorEdadTitularEstadoCivil_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.EstadoCivil_VIUDO_X"].ToString());
                        iMayorEdadTitularEstadoCivil_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.EstadoCivil_VIUDO_Y"].ToString());
                    }
                    if (objFichaRegistralBE.strEstadoCivil == Enumerador.enmEstadoCivil.DIVORCIADO.ToString())
                    {
                        iMayorEdadTitularEstadoCivil_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.EstadoCivil_DIVORCIADO_X"].ToString());
                        iMayorEdadTitularEstadoCivil_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.EstadoCivil_DIVORCIADO_Y"].ToString());
                    }
                    EscribirLetraxLetra(iMayorEdadTitularEstadoCivil_X + MargenX_2, iMayorEdadTitularEstadoCivil_Y + MargenY_2, "x", cb, document);

                    //------------------------------------------
                    /*
                     --GRADO INSTRUCCIÓN
                     */
                    float iMayorEdadTitularGradoInstruccion_X = 0;
                    float iMayorEdadTitularGradoInstruccion_Y = 0;
                    if (objFichaRegistralBE.strGRADO_INSTRUCCION == "PRIMARIA")
                    {
                        iMayorEdadTitularGradoInstruccion_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.GradoInstruccion_PRIMARIA_X"].ToString());
                        iMayorEdadTitularGradoInstruccion_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.GradoInstruccion_PRIMARIA_Y"].ToString());
                    }
                    if (objFichaRegistralBE.strGRADO_INSTRUCCION == "SECUNDARIA")
                    {
                        iMayorEdadTitularGradoInstruccion_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.GradoInstruccion_SECUNDARIA_X"].ToString());
                        iMayorEdadTitularGradoInstruccion_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.GradoInstruccion_SECUNDARIA_Y"].ToString());
                    }
                    if (objFichaRegistralBE.strGRADO_INSTRUCCION == "SUPERIOR")
                    {
                        iMayorEdadTitularGradoInstruccion_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.GradoInstruccion_SUPERIOR_X"].ToString());
                        iMayorEdadTitularGradoInstruccion_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.GradoInstruccion_SUPERIOR_Y"].ToString());
                    }
                    if (objFichaRegistralBE.strGRADO_INSTRUCCION == "ILETRADO")
                    {
                        iMayorEdadTitularGradoInstruccion_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.GradoInstruccion_ILETRADO_X"].ToString());
                        iMayorEdadTitularGradoInstruccion_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.GradoInstruccion_ILETRADO_Y"].ToString());
                    }
                    if (objFichaRegistralBE.strGRADO_INSTRUCCION == "TECNICA")
                    {
                        iMayorEdadTitularGradoInstruccion_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.GradoInstruccion_TECNICA_X"].ToString());
                        iMayorEdadTitularGradoInstruccion_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.GradoInstruccion_TECNICA_Y"].ToString());
                    }
                    if (objFichaRegistralBE.strGRADO_INSTRUCCION == "ESPECIAL")
                    {
                        iMayorEdadTitularGradoInstruccion_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.GradoInstruccion_ESPECIAL_X"].ToString());
                        iMayorEdadTitularGradoInstruccion_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.GradoInstruccion_ESPECIAL_Y"].ToString());
                    }
                    EscribirLetraxLetra(iMayorEdadTitularGradoInstruccion_X + MargenX_2, iMayorEdadTitularGradoInstruccion_Y + MargenY_2, "x", cb, document);

                    //------------------------------------------

                    /*
                    --AÑO ESTUDIO Y ESTUDIO COMPLETO
                    */

                    if (objFichaRegistralBE.strANIO != "0")
                    {
                        float iMayorEdadTitularAnioEstudio_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.AnioEstudio_X"].ToString());
                        float iMayorEdadTitularAnioEstudio_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.AnioEstudio_Y"].ToString());
                        EscribirLetraxLetra(iMayorEdadTitularAnioEstudio_X + MargenX_2, iMayorEdadTitularAnioEstudio_Y + MargenY_2, objFichaRegistralBE.strANIO, cb, document);
                    }


                    if (objFichaRegistralBE.strEstudioCompleto == "S")
                    {
                        float iMayorEdadTitularEstudioCompleto_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.EstudioCompleto_X"].ToString());
                        float iMayorEdadTitularEstudioCompleto_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.EstudioCompleto_Y"].ToString());

                        EscribirLetraxLetra(iMayorEdadTitularEstudioCompleto_X + MargenX_2, iMayorEdadTitularEstudioCompleto_Y + MargenY_2, "x", cb, document);
                    }

                    if (objFichaRegistralBE.strEstaturaMetros != "0")
                    {
                        float iMayorEdadTitularEstaturaMetros_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.EstaturaMetros_X"].ToString());
                        float iMayorEdadTitularEstaturaMetros_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.EstaturaMetros_Y"].ToString());

                        EscribirLetraxLetra(iMayorEdadTitularEstaturaMetros_X + MargenX_2, iMayorEdadTitularEstaturaMetros_Y + MargenY_2, objFichaRegistralBE.strEstaturaMetros, cb, document);
                    }
                    if (objFichaRegistralBE.strEstaturaCentimetros != "0")
                    {
                        float iMayorEdadTitularEstaturaCentimetros_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.EstaturaCentimetros_X"].ToString());
                        float iMayorEdadTitularEstaturaCentimetros_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.EstaturaCentimetros_Y"].ToString());

                        EscribirLetraxLetra(iMayorEdadTitularEstaturaCentimetros_X + MargenX_2, iMayorEdadTitularEstaturaCentimetros_Y + MargenY_2, objFichaRegistralBE.strEstaturaCentimetros, cb, document);
                    }

                    if (objFichaRegistralBE.strGENERO == "MASCULINO")
                    {
                        float iMayorEdadTitularGenero_Masculino_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.GeneroMasculino_X"].ToString());
                        float iMayorEdadTitularGenero_Masculino_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.GeneroMasculino_Y"].ToString());

                        EscribirLetraxLetra(iMayorEdadTitularGenero_Masculino_X + MargenX_2, iMayorEdadTitularGenero_Masculino_Y + MargenY_2, "x", cb, document);
                    }
                    if (objFichaRegistralBE.strGENERO == "FEMENINO")
                    {
                        float iMayorEdadTitularGenero_Femenino_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.GeneroFemenino_X"].ToString());
                        float iMayorEdadTitularGenero_Femenino_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.GeneroFemenino_Y"].ToString());

                        EscribirLetraxLetra(iMayorEdadTitularGenero_Femenino_X + MargenX_2, iMayorEdadTitularGenero_Femenino_Y + MargenY_2, "x", cb, document);
                    }

                    if (objFichaRegistralBE.strDiscapacidad == "S")
                    {
                        float iMayorEdadTitularDiscapacidad_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.Discapacidad_SI_X"].ToString());
                        float iMayorEdadTitularDiscapacidad_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.Discapacidad_SI_Y"].ToString());

                        EscribirLetraxLetra(iMayorEdadTitularDiscapacidad_X + MargenX_2, iMayorEdadTitularDiscapacidad_Y + MargenY_2, "x", cb, document);
                    }
                    if (objFichaRegistralBE.strDiscapacidad == "N")
                    {
                        float iMayorEdadTitularDiscapacidad_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.Discapacidad_NO_X"].ToString());
                        float iMayorEdadTitularDiscapacidad_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.Discapacidad_NO_Y"].ToString());

                        EscribirLetraxLetra(iMayorEdadTitularDiscapacidad_X + MargenX_2, iMayorEdadTitularDiscapacidad_Y + MargenY_2, "x", cb, document);
                    }

                    if (objFichaRegistralBE.strInterdicto == "S")
                    {
                        float iMayorEdadTitularInterdicto_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.Interdiccion_SI_X"].ToString());
                        float iMayorEdadTitularInterdicto_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.Interdiccion_SI_Y"].ToString());

                        EscribirLetraxLetra(iMayorEdadTitularInterdicto_X + MargenX_2, iMayorEdadTitularInterdicto_Y + MargenY_2, "x", cb, document);
                    }
                    if (objFichaRegistralBE.strInterdicto == "N")
                    {
                        float iMayorEdadTitularInterdicto_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.Interdiccion_NO_X"].ToString());
                        float iMayorEdadTitularInterdicto_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.Interdiccion_NO_Y"].ToString());

                        EscribirLetraxLetra(iMayorEdadTitularInterdicto_X + MargenX_2, iMayorEdadTitularInterdicto_Y + MargenY_2, "x", cb, document);
                    }

                    float iMayorCurador_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.Curador_X"].ToString());
                    float iMayorCurador_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.Curador_Y"].ToString());

                    EscribirLetraxLetra(iMayorCurador_X + MargenX_2, iMayorCurador_Y + MargenY_2, objFichaRegistralBE.strNombreCurador, cb, document);
                    //------------------------------------------


                    //------------------------------------------
                    //DOCUMENTOS ADJUNTOS
                    //------------------------------------------
                    if (_dtDocumentos.Rows.Count > 0)
                    {
                        if (_dtDocumentos.Rows[0]["1"].ToString() == "1") //DOCUMENTO
                        {
                            float iMayorEdadDodAdjunto_LIBRETA_MILITAR_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.LIBRETA_MILITAR_X"].ToString());
                            float iMayorEdadDodAdjunto_LIBRETA_MILITAR_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.LIBRETA_MILITAR_Y"].ToString());

                            EscribirLetraxLetra(iMayorEdadDodAdjunto_LIBRETA_MILITAR_X + MargenX_2, iMayorEdadDodAdjunto_LIBRETA_MILITAR_Y + MargenY_2, "x", cb, document);

                            float iMayorEdadDodAdjunto_NUM_LIBRETA_MILITAR_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.NUM_LIBRETA_MILITAR_X"].ToString());
                            float iMayorEdadDodAdjunto_NUM_LIBRETA_MILITAR_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.NUM_LIBRETA_MILITAR_Y"].ToString());

                            EscribirLetraxLetra(iMayorEdadDodAdjunto_NUM_LIBRETA_MILITAR_X + MargenX_2, iMayorEdadDodAdjunto_NUM_LIBRETA_MILITAR_Y + MargenY_2, _dtDocumentos.Rows[1]["1"].ToString(), cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["2"].ToString() == "1") //DOCUMENTO
                        {
                            float iMayorEdadDodAdjunto_ACTA_NAC_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.ACTA_NAC_X"].ToString());
                            float iMayorEdadDodAdjunto_ACTA_NAC_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.ACTA_NAC_Y"].ToString());

                            EscribirLetraxLetra(iMayorEdadDodAdjunto_ACTA_NAC_X + MargenX_2, iMayorEdadDodAdjunto_ACTA_NAC_Y + MargenY_2, "x", cb, document);

                            float iMayorEdadDodAdjunto_NUM_ACTA_NAC_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.NUM_ACTA_NAC_X"].ToString());
                            float iMayorEdadDodAdjunto_NUM_ACTA_NAC_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.NUM_ACTA_NAC_Y"].ToString());

                            EscribirLetraxLetra(iMayorEdadDodAdjunto_NUM_ACTA_NAC_X + MargenX_2, iMayorEdadDodAdjunto_NUM_ACTA_NAC_Y + MargenY_2, _dtDocumentos.Rows[1]["2"].ToString(), cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["3"].ToString() == "1") //DOCUMENTO
                        {
                            float iMayorEdadDodAdjunto_NATURALIZACION_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.NATURALIZACION_X"].ToString());
                            float iMayorEdadDodAdjunto_NATURALIZACION_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.NATURALIZACION_Y"].ToString());

                            EscribirLetraxLetra(iMayorEdadDodAdjunto_NATURALIZACION_X + MargenX_2, iMayorEdadDodAdjunto_NATURALIZACION_Y + MargenY_2, "x", cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["4"].ToString() == "1") //DOCUMENTO
                        {
                            float iMayorEdadDodAdjunto_ACTA_BAUTIZO_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.ACTA_BAUTIZO_X"].ToString());
                            float iMayorEdadDodAdjunto_ACTA_BAUTIZO_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.ACTA_BAUTIZO_Y"].ToString());

                            EscribirLetraxLetra(iMayorEdadDodAdjunto_ACTA_BAUTIZO_X + MargenX_2, iMayorEdadDodAdjunto_ACTA_BAUTIZO_Y + MargenY_2, "x", cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["5"].ToString() == "1") //DOCUMENTO
                        {
                            float iMayorEdadDodAdjunto_ACTA_MATRI_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.ACTA_MATRI_X"].ToString());
                            float iMayorEdadDodAdjunto_ACTA_MATRI_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.ACTA_MATRI_Y"].ToString());

                            EscribirLetraxLetra(iMayorEdadDodAdjunto_ACTA_MATRI_X + MargenX_2, iMayorEdadDodAdjunto_ACTA_MATRI_Y + MargenY_2, "x", cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["6"].ToString() == "1") //DOCUMENTO
                        {
                            float iMayorEdadDodAdjunto_DEFUNCION_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.DEFUNCION_X"].ToString());
                            float iMayorEdadDodAdjunto_DEFUNCION_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.DEFUNCION_Y"].ToString());

                            EscribirLetraxLetra(iMayorEdadDodAdjunto_DEFUNCION_X + MargenX_2, iMayorEdadDodAdjunto_DEFUNCION_Y + MargenY_2, "x", cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["7"].ToString() == "1") //DOCUMENTO
                        {
                            float iMayorEdadDodAdjunto_RES_JUDICIAL_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.RES_JUDICIAL_X"].ToString());
                            float iMayorEdadDodAdjunto_RES_JUDICIAL_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.RES_JUDICIAL_Y"].ToString());

                            EscribirLetraxLetra(iMayorEdadDodAdjunto_RES_JUDICIAL_X + MargenX_2, iMayorEdadDodAdjunto_RES_JUDICIAL_Y + MargenY_2, "x", cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["8"].ToString() == "1") //DOCUMENTO
                        {
                            float iMayorEdadDodAdjunto_PAGO_BN_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.PAGO_BN_X"].ToString());
                            float iMayorEdadDodAdjunto_PAGO_BN_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.PAGO_BN_Y"].ToString());

                            EscribirLetraxLetra(iMayorEdadDodAdjunto_PAGO_BN_X + MargenX_2, iMayorEdadDodAdjunto_PAGO_BN_Y + MargenY_2, "x", cb, document);

                            float iMayorEdadDodAdjunto_NUM_PAGO_BN_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.NUM_PAGO_BN_X"].ToString());
                            float iMayorEdadDodAdjunto_NUM_PAGO_BN_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.NUM_PAGO_BN_Y"].ToString());

                            EscribirLetraxLetra(iMayorEdadDodAdjunto_NUM_PAGO_BN_X + MargenX_2, iMayorEdadDodAdjunto_NUM_PAGO_BN_Y + MargenY_2, _dtDocumentos.Rows[1]["8"].ToString(), cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["9"].ToString() == "1") //DOCUMENTO
                        {
                            float iMayorEdadDodAdjunto_CERTIFICADO_ESTUDIO_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.CERTIFICADO_ESTUDIO_X"].ToString());
                            float iMayorEdadDodAdjunto_CERTIFICADO_ESTUDIO_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.CERTIFICADO_ESTUDIO_Y"].ToString());

                            EscribirLetraxLetra(iMayorEdadDodAdjunto_CERTIFICADO_ESTUDIO_X + MargenX_2, iMayorEdadDodAdjunto_CERTIFICADO_ESTUDIO_Y + MargenY_2, "x", cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["10"].ToString() == "1") //DOCUMENTO
                        {
                            float iMayorEdadDodAdjunto_RECIBO_SERVICIO_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.RECIBO_SERVICIO_X"].ToString());
                            float iMayorEdadDodAdjunto_RECIBO_SERVICIO_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.RECIBO_SERVICIO_Y"].ToString());

                            EscribirLetraxLetra(iMayorEdadDodAdjunto_RECIBO_SERVICIO_X + MargenX_2, iMayorEdadDodAdjunto_RECIBO_SERVICIO_Y + MargenY_2, "x", cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["11"].ToString() == "1") //DOCUMENTO
                        {
                            float iMayorEdadDodAdjunto_DECLARACION_JURADA_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.DECLARACION_JURADA_X"].ToString());
                            float iMayorEdadDodAdjunto_DECLARACION_JURADA_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.DECLARACION_JURADA_Y"].ToString());

                            EscribirLetraxLetra(iMayorEdadDodAdjunto_DECLARACION_JURADA_X + MargenX_2, iMayorEdadDodAdjunto_DECLARACION_JURADA_Y + MargenY_2, "x", cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["12"].ToString() == "1") //DOCUMENTO
                        {
                            float iMayorEdadDodAdjunto_CARNET_IDENTIDAD_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.CARNET_IDENTIDAD_X"].ToString());
                            float iMayorEdadDodAdjunto_CARNET_IDENTIDAD_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.CARNET_IDENTIDAD_Y"].ToString());

                            EscribirLetraxLetra(iMayorEdadDodAdjunto_CARNET_IDENTIDAD_X + MargenX_2, iMayorEdadDodAdjunto_CARNET_IDENTIDAD_Y + MargenY_2, "x", cb, document);

                            float iMayorEdadDodAdjunto_NUM_CARNET_IDENTIDAD_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.NUM_CARNET_IDENTIDAD_X"].ToString());
                            float iMayorEdadDodAdjunto_NUM_CARNET_IDENTIDAD_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.NUM_CARNET_IDENTIDAD_Y"].ToString());

                            EscribirLetraxLetra(iMayorEdadDodAdjunto_NUM_CARNET_IDENTIDAD_X + MargenX_2, iMayorEdadDodAdjunto_NUM_CARNET_IDENTIDAD_Y + MargenY_2, _dtDocumentos.Rows[1]["12"].ToString(), cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["13"].ToString() == "1") //DOCUMENTO
                        {
                            float iMayorEdadDodAdjunto_OTRO_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.OTRO_X"].ToString());
                            float iMayorEdadDodAdjunto_OTRO_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.OTRO_Y"].ToString());

                            EscribirLetraxLetra(iMayorEdadDodAdjunto_OTRO_X + MargenX_2, iMayorEdadDodAdjunto_OTRO_Y + MargenY_2, "x", cb, document);

                            float iMayorEdadDodAdjunto_DESC_OTRO_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.DESC_OTRO_X"].ToString());
                            float iMayorEdadDodAdjunto_DESC_OTRO_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Documento.DESC_OTRO_Y"].ToString());

                            EscribirLetraxLetra(iMayorEdadDodAdjunto_DESC_OTRO_X + MargenX_2, iMayorEdadDodAdjunto_DESC_OTRO_Y + MargenY_2, _dtDocumentos.Rows[1]["13"].ToString(), cb, document);
                        }
                    }
                }
                #endregion
                

                cb.EndText();
                
                document.NewPage();
                
                cb.BeginText();
                cb.SetFontAndSize(bfTimes, 10);

                #region parte superior segunda cara
                //------------------------------------------
                float iMayorEdadTitularFechaNacimiento_dia_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.FechaNacimiento_dia_X"].ToString());
                float iMayorEdadTitularFechaNacimiento_dia_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.FechaNacimiento_dia_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularFechaNacimiento_dia_X + MargenX_3, iMayorEdadTitularFechaNacimiento_dia_Y + MargenY_3, objFichaRegistralBE.strFecNacTitular_DD, cb, document);

                float iMayorEdadTitularFechaNacimiento_mes_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.FechaNacimiento_mes_X"].ToString());
                float iMayorEdadTitularFechaNacimiento_mes_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.FechaNacimiento_mes_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularFechaNacimiento_mes_X + MargenX_3, iMayorEdadTitularFechaNacimiento_mes_Y + MargenY_3, objFichaRegistralBE.strFecNacTitular_MM, cb, document);

                float iMayorEdadTitularFechaNacimiento_anio_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.FechaNacimiento_anio_X"].ToString());
                float iMayorEdadTitularFechaNacimiento_anio_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.FechaNacimiento_anio_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularFechaNacimiento_anio_X + MargenX_3, iMayorEdadTitularFechaNacimiento_anio_Y + MargenY_3, objFichaRegistralBE.strFecNacTitular_YYYY, cb, document);
                //------------------------------------------   

                /*Nuevos Campos*/
                //------------------------------------------
                /*
                 --DONA ORGANOS
                 */
                if (dtImpresionCompleta.Rows[0]["descripcion"].ToString() == "SI")
                {
                    if (objFichaRegistralBE.strDonaOrganos == "S")
                    {
                        float iMayorEdadTitularDonaOrganos_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.DonaOrganos_SI_X"].ToString());
                        float iMayorEdadTitularDonaOrganos_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.DonaOrganos_SI_Y"].ToString());

                        EscribirLetraxLetra(iMayorEdadTitularDonaOrganos_X + MargenX_3, iMayorEdadTitularDonaOrganos_Y + MargenY_3, "x", cb, document);
                    }
                    if (objFichaRegistralBE.strDonaOrganos == "N")
                    {
                        float iMayorEdadTitularDonaOrganos_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.DonaOrganos_NO_X"].ToString());
                        float iMayorEdadTitularDonaOrganos_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.DonaOrganos_NO_Y"].ToString());

                        EscribirLetraxLetra(iMayorEdadTitularDonaOrganos_X + MargenX_3, iMayorEdadTitularDonaOrganos_Y + MargenY_3, "x", cb, document);
                    }
                }
                //FIN

                //------------------------------------------                
                float iMayorEdadTitularNacDpto_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.NacDpto_X"].ToString());
                float iMayorEdadTitularNacDpto_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.NacDpto_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularNacDpto_X + MargenX_3, iMayorEdadTitularNacDpto_Y + MargenY_3, objFichaRegistralBE.strNacDptoTitular, cb, document);

                float iMayorEdadTitularNacCodDpto_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.NacCodDpto_X"].ToString());
                float iMayorEdadTitularNacCodDpto_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.NacCodDpto_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularNacCodDpto_X + MargenX_3, iMayorEdadTitularNacCodDpto_Y + MargenY_3, objFichaRegistralBE.strNacCodDptoTitular, cb, document);

                float iMayorEdadTitularNacProv_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.NacProv_X"].ToString());
                float iMayorEdadTitularNacProv_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.NacProv_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularNacProv_X + MargenX_3, iMayorEdadTitularNacProv_Y + MargenY_3, objFichaRegistralBE.strNacProvTitular, cb, document);

                float iMayorEdadTitularNacCodProv_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.NacCodProv_X"].ToString());
                float iMayorEdadTitularNacCodProv_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.NacCodProv_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularNacCodProv_X + MargenX_3, iMayorEdadTitularNacCodProv_Y + MargenY_3, objFichaRegistralBE.strNacCodProvTitular, cb, document);

                float iMayorEdadTitularNacDist_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.NacDist_X"].ToString());
                float iMayorEdadTitularNacDist_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.NacDist_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularNacDist_X + MargenX_3, iMayorEdadTitularNacDist_Y + MargenY_3, objFichaRegistralBE.strNacDistTitular, cb, document);

                float iMayorEdadTitularNacCodDist_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.NacCodDist_X"].ToString());
                float iMayorEdadTitularNacCodDist_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.NacCodDist_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularNacCodDist_X + MargenX_3, iMayorEdadTitularNacCodDist_Y + MargenY_3, objFichaRegistralBE.strNacCodDistTitular, cb, document);
                //-----------------------------------------------------------
                //Fecha: 04/03/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Imprimir el tipo y numero de documento del Padre
                //-----------------------------------------------------------
                string strTipoDocumento = "";

                float iMayorEdadPadreTipoDocumento_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Padre.TipoDocumento_X"].ToString());
                float iMayorEdadPadreTipoDocumento_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Padre.TipoDocumento_Y"].ToString());

                strTipoDocumento = obtenerTipodocumento(objFichaRegistralBE.strTipoDocPadre);

                EscribirLetraxLetra(iMayorEdadPadreTipoDocumento_X + MargenX_3, iMayorEdadPadreTipoDocumento_Y + MargenY_3, strTipoDocumento, cb, document);

                float iMayorEdadPadreNumeroDocumento_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Padre.NumeroDocumento_X"].ToString());
                float iMayorEdadPadreNumeroDocumento_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Padre.NumeroDocumento_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadPadreNumeroDocumento_X + MargenX_3, iMayorEdadPadreNumeroDocumento_Y + MargenY_3, objFichaRegistralBE.strNroDocPadre, cb, document);
                //-----------------------------------------------------------

                float iMayorEdadPadreApPaterno_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Padre.ApPaterno_X"].ToString());
                float iMayorEdadPadreApPaterno_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Padre.ApPaterno_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadPadreApPaterno_X + MargenX_3, iMayorEdadPadreApPaterno_Y + MargenY_3, objFichaRegistralBE.strApPaternoPadre, cb, document);

                float iMayorEdadPadreApMaterno_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Padre.ApMaterno_X"].ToString());
                float iMayorEdadPadreApMaterno_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Padre.ApMaterno_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadPadreApMaterno_X + MargenX_3, iMayorEdadPadreApMaterno_Y + MargenY_3, objFichaRegistralBE.strApMaternoPadre, cb, document);

                float iMayorEdadPadreNombres_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Padre.Nombres_X"].ToString());
                float iMayorEdadPadreNombres_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Padre.Nombres_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadPadreNombres_X + MargenX_3, iMayorEdadPadreNombres_Y + MargenY_3, objFichaRegistralBE.strNombresPadre, cb, document);

                //-----------------------------------------------------------
                //Fecha: 04/03/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Imprimir el tipo y numero de documento de la Madre
                //-----------------------------------------------------------
                float iMayorEdadMadreTipoDocumento_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Madre.TipoDocumento_X"].ToString());
                float iMayorEdadMadreTipoDocumento_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Madre.TipoDocumento_Y"].ToString());

                strTipoDocumento = obtenerTipodocumento(objFichaRegistralBE.strTipoDocMadre);

                EscribirLetraxLetra(iMayorEdadMadreTipoDocumento_X + MargenX_3, iMayorEdadMadreTipoDocumento_Y + MargenY_3, strTipoDocumento, cb, document);
                
                float iMayorEdadMadreNumeroDocumento_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Madre.NumeroDocumento_X"].ToString());
                float iMayorEdadMadreNumeroDocumento_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Madre.NumeroDocumento_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadMadreNumeroDocumento_X + MargenX_3, iMayorEdadMadreNumeroDocumento_Y + MargenY_3, objFichaRegistralBE.strNroDocMadre, cb, document);
                //-----------------------------------------------------------

                //------------------------------------------
                float iMayorEdadMadreApPaterno_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Madre.ApPaterno_X"].ToString());
                float iMayorEdadMadreApPaterno_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Madre.ApPaterno_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadMadreApPaterno_X + MargenX_3, iMayorEdadMadreApPaterno_Y + MargenY_3, objFichaRegistralBE.strApPaternoMadre, cb, document);

                float iMayorEdadMadreApMaterno_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Madre.ApMaterno_X"].ToString());
                float iMayorEdadMadreApMaterno_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Madre.ApMaterno_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadMadreApMaterno_X + MargenX_3, iMayorEdadMadreApMaterno_Y + MargenY_3, objFichaRegistralBE.strApMaternoMadre, cb, document);

                float iMayorEdadMadreNombres_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Madre.Nombres_X"].ToString());
                float iMayorEdadMadreNombres_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Madre.Nombres_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadMadreNombres_X + MargenX_3, iMayorEdadMadreNombres_Y + MargenY_3, objFichaRegistralBE.strNombresMadre, cb, document);

                //-----------------------------------------------------------
                //Fecha: 04/03/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Imprimir el tipo y numero de documento del Conyuge.
                //-----------------------------------------------------------
                float iMayorEdadConyugeTipoDocumento_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Conyuge.TipoDocumento_X"].ToString());
                float iMayorEdadConyugeTipoDocumento_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Conyuge.TipoDocumento_Y"].ToString());

                strTipoDocumento = obtenerTipodocumento(objFichaRegistralBE.strTipoDocConyuge);

                EscribirLetraxLetra(iMayorEdadConyugeTipoDocumento_X + MargenX_3, iMayorEdadConyugeTipoDocumento_Y + MargenY_3, strTipoDocumento, cb, document);
                
                float iMayorEdadConyugeNumeroDocumento_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Conyuge.NumeroDocumento_X"].ToString());
                float iMayorEdadConyugeNumeroDocumento_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Conyuge.NumeroDocumento_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadConyugeNumeroDocumento_X + MargenX_3, iMayorEdadConyugeNumeroDocumento_Y + MargenY_3, objFichaRegistralBE.strNroDocConyuge, cb, document);
                //-----------------------------------------------------------

                //------------------------------------------
                float iMayorEdadConyugeApPaterno_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Conyuge.ApPaterno_X"].ToString());
                float iMayorEdadConyugeApPaterno_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Conyuge.ApPaterno_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadConyugeApPaterno_X + MargenX_3, iMayorEdadConyugeApPaterno_Y + MargenY_3, objFichaRegistralBE.strApPaternoConyuge, cb, document);

                float iMayorEdadConyugeApMaterno_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Conyuge.ApMaterno_X"].ToString());
                float iMayorEdadConyugeApMaterno_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Conyuge.ApMaterno_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadConyugeApMaterno_X + MargenX_3, iMayorEdadConyugeApMaterno_Y + MargenY_3, objFichaRegistralBE.strApMaternoConyuge, cb, document);

                float iMayorEdadConyugeNombres_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Conyuge.Nombres_X"].ToString());
                float iMayorEdadConyugeNombres_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Conyuge.Nombres_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadConyugeNombres_X + MargenX_3, iMayorEdadConyugeNombres_Y + MargenY_3, objFichaRegistralBE.strNombresConyuge, cb, document);
                //------------------------------------------

                float iMayorEdadTitularCodLocalDestino_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.CodLocalDestino_X"].ToString());
                float iMayorEdadTitularCodLocalDestino_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.CodLocalDestino_Y"].ToString());

                if (objFichaRegistralBE.strCodigoLocalDestino != "0")
                {
                    EscribirLetraxLetra(iMayorEdadTitularCodLocalDestino_X + MargenX_3, iMayorEdadTitularCodLocalDestino_Y + MargenY_3, objFichaRegistralBE.strCodigoLocalDestino, cb, document);
                }
                //------------------------------------------
                float iMayorEdadTitularTelefono_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.Telefono_X"].ToString());
                float iMayorEdadTitularTelefono_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.Telefono_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularTelefono_X + MargenX_3, iMayorEdadTitularTelefono_Y + MargenY_3, objFichaRegistralBE.strTelefonoTitular, cb, document);

                //------------------------------------------
                float iMayorEdadTitularCorreo_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.Correo_X"].ToString());
                float iMayorEdadTitularCorreo_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MayorEdad.Titular.Correo_Y"].ToString());

                EscribirLetraxLetra(iMayorEdadTitularCorreo_X + MargenX_3, iMayorEdadTitularCorreo_Y + MargenY_3, objFichaRegistralBE.strCorreoElectronicoTitular, cb, document);

                #endregion
                
                //------------------------------------------
                cb.EndText();
                //------------------------------------------
                document.Close();
                oStreamReader.Close();
                writer.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 11/01/2017
        // Objetivo: Crea el archivo PDF de la ficha Registral 
        //           para menores de edad.
        //------------------------------------------------------

        private static void CreateFilePDFFichaRegistralMenorEdad(SGAC.BE.MRE.Custom.CBE_FICHAREGISTRAL objFichaRegistralBE, string HtmlPath, string PdfPath, DataTable _dtDocumentos,bool NuevoFormatoY = false)
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
                
                /*PARAMETRO DE MARGENES*/
                string sTipoDocumento = "0";
                sTipoDocumento = comun_Part1.ObtenerParametroDatoPorCampo(HttpContext.Current.Session, SGAC.Accesorios.Constantes.CONST_DOCUMENTOS_IMPRESION, SGAC.Accesorios.Constantes.CONST_DOC_FICHA_MENOR_EDAD, "id");
                TablaMaestraConsultaBL obj = new TablaMaestraConsultaBL();
                DataTable dt = obj.ConsultarMargenesDocumento(Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), Convert.ToInt16(sTipoDocumento));

                Int16 MargenX_1 = 0;
                Int16 MargenY_1 = 0;
                Int16 MargenX_2 = 0;
                Int16 MargenY_2 = 0;
                Int16 MargenX_3 = 0;
                Int16 MargenY_3 = 0;
                Int16 MargenX_4 = 0;
                Int16 MargenY_4 = 0;

                foreach (DataRow row in dt.Rows)
                {
                     if(row["mado_sSeccion"].ToString() == "1"){
                           MargenX_1 =  Convert.ToInt16(row["mado_sMargenIzquierdo"].ToString());
                           MargenY_1 =  Convert.ToInt16(row["mado_sMargenSuperior"].ToString());
                     }
                    if(row["mado_sSeccion"].ToString() == "2"){
                           MargenX_2 =  Convert.ToInt16(row["mado_sMargenIzquierdo"].ToString());
                           MargenY_2 =  Convert.ToInt16(row["mado_sMargenSuperior"].ToString());
                     }
                    if(row["mado_sSeccion"].ToString() == "3"){
                           MargenX_3 =  Convert.ToInt16(row["mado_sMargenIzquierdo"].ToString());
                           MargenY_3 =  Convert.ToInt16(row["mado_sMargenSuperior"].ToString());
                     }
                    if(row["mado_sSeccion"].ToString() == "4"){
                           MargenX_4 =  Convert.ToInt16(row["mado_sMargenIzquierdo"].ToString());
                           MargenY_4 =  Convert.ToInt16(row["mado_sMargenSuperior"].ToString());
                     }
                }

                /*FIN PARAMETROS MARGENES*/

                iTextSharp.text.FontFactory.RegisterDirectories();

                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(PdfPath, FileMode.Create));

                document.Open();

                document.NewPage();



                iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                iTextSharp.text.pdf.BaseFont bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);

                cb.SetFontAndSize(bfTimes, 10);
                cb.BeginText();

                //------------------------------------------
                float iMenorEdadTitularCodigoLocal_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.CodigoLocal_X"].ToString());
                float iMenorEdadTitularCodigoLocal_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.CodigoLocal_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularCodigoLocal_X + MargenX_1, iMenorEdadTitularCodigoLocal_Y + MargenY_1, objFichaRegistralBE.strCodigoLocal, cb, document);

                
                #region parte superior primera cara
                /*JOnatan 31/05/2017 - se agrega DNI al formato*/
                if (objFichaRegistralBE.strTipoDocTitular == "DNI")
                {
                    float iMenorEdadDNI_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.DNI_X"].ToString());
                    float iMenorEdadDNI_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.DNI_Y"].ToString());

                    EscribirLetraxLetra(iMenorEdadDNI_X + MargenX_1, iMenorEdadDNI_Y + MargenY_1, objFichaRegistralBE.strNroDocTitular, cb, document);

                    float iMenorEdadTitularDNI_Desglosable_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.DNI_DESGLOSABLE_X"].ToString());
                    float iMenorEdadTitularDNI_Desglosable_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.DNI_DESGLOSABLE_Y"].ToString());

                    EscribirLetraxLetra(iMenorEdadTitularDNI_Desglosable_X + MargenX_1, iMenorEdadTitularDNI_Desglosable_Y + MargenY_1, objFichaRegistralBE.strNroDocTitular, cb, document);
                }


                float iMenorEdadTitularApellidos_DESGLOSABLE_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.Apellidos_DESGLOSABLE_X"].ToString());
                float iMenorEdadTitularApellidos_DESGLOSABLE_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.Apellidos_DESGLOSABLE_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularApellidos_DESGLOSABLE_X + MargenX_1, iMenorEdadTitularApellidos_DESGLOSABLE_Y + MargenY_1, objFichaRegistralBE.strApPaternoTitular + " " + objFichaRegistralBE.strApMaternoTitular, cb, document);

                float iMenorEdadTitularNombres_DESGLOSABLE_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.Nombres_DESGLOSABLE_X"].ToString());
                float iMenorEdadTitularNombres_DESGLOSABLE_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.Nombres_DESGLOSABLE_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularNombres_DESGLOSABLE_X + MargenX_1, iMenorEdadTitularNombres_DESGLOSABLE_Y + MargenY_1, objFichaRegistralBE.strNombresTitular, cb, document);


                //------------------------------------------
                float iMenorEdadTitularFechaRegistro_dia_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.FechaRegistro_dia_X"].ToString());
                float iMenorEdadTitularFechaRegistro_dia_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.FechaRegistro_dia_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularFechaRegistro_dia_X + MargenX_1, iMenorEdadTitularFechaRegistro_dia_Y + MargenY_1, objFichaRegistralBE.strFechaRegistro_DD, cb, document);

                float iMenorEdadTitularFechaRegistro_mes_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.FechaRegistro_mes_X"].ToString());
                float iMenorEdadTitularFechaRegistro_mes_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.FechaRegistro_mes_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularFechaRegistro_mes_X + MargenX_1, iMenorEdadTitularFechaRegistro_mes_Y + MargenY_1, objFichaRegistralBE.strFechaRegistro_MM, cb, document);

                float iMenorEdadTitularFechaRegistro_anio_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.FechaRegistro_anio_X"].ToString());
                float iMenorEdadTitularFechaRegistro_anio_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.FechaRegistro_anio_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularFechaRegistro_anio_X + MargenX_1, iMenorEdadTitularFechaRegistro_anio_Y + MargenY_1, objFichaRegistralBE.strFechaRegistro_YYYY, cb, document);
                //------------------------------------------
                float iMenorEdadTitularApPaterno_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.ApPaterno_X"].ToString());
                float iMenorEdadTitularApPaterno_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.ApPaterno_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularApPaterno_X + MargenX_1, iMenorEdadTitularApPaterno_Y + MargenY_1, objFichaRegistralBE.strApPaternoTitular, cb, document);

                float iMenorEdadTitularApMaterno_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.ApMaterno_X"].ToString());
                float iMenorEdadTitularApMaterno_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.ApMaterno_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularApMaterno_X + MargenX_1, iMenorEdadTitularApMaterno_Y + MargenY_1, objFichaRegistralBE.strApMaternoTitular, cb, document);

                float iMenorEdadTitularNombres_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.Nombres_X"].ToString());
                float iMenorEdadTitularNombres_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.Nombres_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularNombres_X + MargenX_1, iMenorEdadTitularNombres_Y + MargenY_1, objFichaRegistralBE.strNombresTitular, cb, document);
                //------------------------------------------

                float iMenorEdadTitularDirDpto_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.DirDpto_X"].ToString());
                float iMenorEdadTitularDirDpto_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.DirDpto_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularDirDpto_X + MargenX_1, iMenorEdadTitularDirDpto_Y + MargenY_1, objFichaRegistralBE.strDirDptoTitular, cb, document);

                float iMenorEdadTitularDirCodDpto_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.DirCodDpto_X"].ToString());
                float iMenorEdadTitularDirCodDpto_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.DirCodDpto_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularDirCodDpto_X + MargenX_1, iMenorEdadTitularDirCodDpto_Y + MargenY_1, objFichaRegistralBE.strDirCodDptoTitular, cb, document);

                float iMenorEdadTitularDirProv_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.DirProv_X"].ToString());
                float iMenorEdadTitularDirProv_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.DirProv_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularDirProv_X + MargenX_1, iMenorEdadTitularDirProv_Y + MargenY_1, objFichaRegistralBE.strDirProvTitular, cb, document);

                float iMenorEdadTitularDirCodProv_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.DirCodProv_X"].ToString());
                float iMenorEdadTitularDirCodProv_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.DirCodProv_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularDirCodProv_X + MargenX_1, iMenorEdadTitularDirCodProv_Y + MargenY_1, objFichaRegistralBE.strDirCodProvTitular, cb, document);

                float iMenorEdadTitularDirDist_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.DirDist_X"].ToString());
                float iMenorEdadTitularDirDist_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.DirDist_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularDirDist_X + MargenX_1, iMenorEdadTitularDirDist_Y + MargenY_1, objFichaRegistralBE.strDirDistTitular, cb, document);

                float iMenorEdadTitularDirCodDist_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.DirCodDist_X"].ToString());
                float iMenorEdadTitularDirCodDist_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.DirCodDist_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularDirCodDist_X + MargenX_1, iMenorEdadTitularDirCodDist_Y + MargenY_1, objFichaRegistralBE.strDirCodDistTitular, cb, document);

                //------------------------------------------
                float iMenorEdadTitularDireccion_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.Direccion_X"].ToString());
                float iMenorEdadTitularDireccion_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.Direccion_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularDireccion_X + MargenX_1, iMenorEdadTitularDireccion_Y + MargenY_1, objFichaRegistralBE.strDireccionTitular, cb, document);

                //------------------------------------------
                float iMenorEdadCodigoPostal_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.CodigoPostal_X"].ToString());
                float iMenorEdadCodigoPostal_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.CodigoPostal_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadCodigoPostal_X + MargenX_1, iMenorEdadCodigoPostal_Y + MargenY_1, objFichaRegistralBE.strCodigoPostalResidencia, cb, document);

                //------------------------------------------
                float iMenorEdadTitularSenasParticulares_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.SenasParticulares_X"].ToString());
                float iMenorEdadTitularSenasParticulares_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.SenasParticulares_Y"].ToString());

                cb.SetFontAndSize(bfTimes, 7);
                EscribirLetraxLetra(iMenorEdadTitularSenasParticulares_X + MargenX_1, iMenorEdadTitularSenasParticulares_Y + MargenY_1, objFichaRegistralBE.strSenasParticularesTitular, cb, document);

                //------------------------------------------    

                //------------------------------------------
                #endregion

                #region parte inferior primera cara
                /*Nuevos Campos*/
                //------------------------------------------

                //------------------------------------------
                /*
                 --GRADO INSTRUCCIÓN
                 */

                DataTable dtImpresionCompleta = new DataTable();

                dtImpresionCompleta = comun_Part1.ObtenerParametrosPorGrupo(HttpContext.Current.Session, SGAC.Accesorios.Constantes.CONST_IMPRESION_COMPLETA);
                //DataTable dtImpresionCompleta = Comun.ObtenerParametrosPorGrupo((DataTable)HttpContext.Current.Session[Constantes.CONST_SESION_DT_PARAMETRO], SGAC.Accesorios.Constantes.CONST_IMPRESION_COMPLETA);

                if (dtImpresionCompleta.Rows[0]["descripcion"].ToString() == "SI")
                {
                    float iMenorEdadTitularGradoInstruccion_X = 0;
                    float iMenorEdadTitularGradoInstruccion_Y = 0;
                    if (objFichaRegistralBE.strGRADO_INSTRUCCION == "INICIAL")
                    {
                        iMenorEdadTitularGradoInstruccion_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.GradoInstruccion_INICIAL_X"].ToString());
                        iMenorEdadTitularGradoInstruccion_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.GradoInstruccion_INICIAL_Y"].ToString());
                    }
                    if (objFichaRegistralBE.strGRADO_INSTRUCCION == "PRIMARIA")
                    {
                        iMenorEdadTitularGradoInstruccion_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.GradoInstruccion_PRIMARIA_X"].ToString());
                        iMenorEdadTitularGradoInstruccion_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.GradoInstruccion_PRIMARIA_Y"].ToString());
                    }
                    if (objFichaRegistralBE.strGRADO_INSTRUCCION == "SECUNDARIA")
                    {
                        iMenorEdadTitularGradoInstruccion_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.GradoInstruccion_SECUNDARIA_X"].ToString());
                        iMenorEdadTitularGradoInstruccion_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.GradoInstruccion_SECUNDARIA_Y"].ToString());
                    }
                    if (objFichaRegistralBE.strGRADO_INSTRUCCION == "SUPERIOR")
                    {
                        iMenorEdadTitularGradoInstruccion_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.GradoInstruccion_SUPERIOR_X"].ToString());
                        iMenorEdadTitularGradoInstruccion_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.GradoInstruccion_SUPERIOR_Y"].ToString());
                    }
                    if (objFichaRegistralBE.strGRADO_INSTRUCCION == "ILETRADO")
                    {
                        iMenorEdadTitularGradoInstruccion_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.GradoInstruccion_ILETRADO_X"].ToString());
                        iMenorEdadTitularGradoInstruccion_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.GradoInstruccion_ILETRADO_Y"].ToString());
                    }
                    if (objFichaRegistralBE.strGRADO_INSTRUCCION == "TECNICA")
                    {
                        iMenorEdadTitularGradoInstruccion_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.GradoInstruccion_TECNICA_X"].ToString());
                        iMenorEdadTitularGradoInstruccion_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.GradoInstruccion_TECNICA_Y"].ToString());
                    }
                    if (objFichaRegistralBE.strGRADO_INSTRUCCION == "ESPECIAL")
                    {
                        iMenorEdadTitularGradoInstruccion_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.GradoInstruccion_ESPECIAL_X"].ToString());
                        iMenorEdadTitularGradoInstruccion_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.GradoInstruccion_ESPECIAL_Y"].ToString());
                    }
                    EscribirLetraxLetra(iMenorEdadTitularGradoInstruccion_X + MargenX_2, iMenorEdadTitularGradoInstruccion_Y + MargenY_2, "x", cb, document);

                    //------------------------------------------

                    /*
                    --AÑO ESTUDIO Y ESTUDIO COMPLETO
                    */

                    if (objFichaRegistralBE.strANIO != "0")
                    {
                        float iMenorEdadTitularAnioEstudio_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.AnioEstudio_X"].ToString());
                        float iMenorEdadTitularAnioEstudio_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.AnioEstudio_Y"].ToString());
                        EscribirLetraxLetra(iMenorEdadTitularAnioEstudio_X + MargenX_2, iMenorEdadTitularAnioEstudio_Y + MargenY_2, objFichaRegistralBE.strANIO, cb, document);
                    }


                    if (objFichaRegistralBE.strEstudioCompleto == "S")
                    {
                        float iMenorEdadTitularEstudioCompleto_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.EstudioCompleto_X"].ToString());
                        float iMenorEdadTitularEstudioCompleto_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.EstudioCompleto_Y"].ToString());

                        EscribirLetraxLetra(iMenorEdadTitularEstudioCompleto_X + MargenX_2, iMenorEdadTitularEstudioCompleto_Y + MargenY_2, "x", cb, document);
                    }

                    if (objFichaRegistralBE.strEstaturaMetros != "0")
                    {
                        float iMenorEdadTitularEstaturaMetros_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.EstaturaMetros_X"].ToString());
                        float iMenorEdadTitularEstaturaMetros_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.EstaturaMetros_Y"].ToString());

                        EscribirLetraxLetra(iMenorEdadTitularEstaturaMetros_X + MargenX_2, iMenorEdadTitularEstaturaMetros_Y + MargenY_2, objFichaRegistralBE.strEstaturaMetros, cb, document);
                    }
                    if (objFichaRegistralBE.strEstaturaCentimetros != "0")
                    {
                        float iMenorEdadTitularEstaturaCentimetros_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.EstaturaCentimetros_X"].ToString());
                        float iMenorEdadTitularEstaturaCentimetros_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.EstaturaCentimetros_Y"].ToString());

                        EscribirLetraxLetra(iMenorEdadTitularEstaturaCentimetros_X + MargenX_2, iMenorEdadTitularEstaturaCentimetros_Y + MargenY_2, objFichaRegistralBE.strEstaturaCentimetros, cb, document);
                    }

                    if (objFichaRegistralBE.strGENERO == "MASCULINO")
                    {
                        float iMenorEdadTitularGenero_Masculino_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.GeneroMasculino_X"].ToString());
                        float iMenorEdadTitularGenero_Masculino_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.GeneroMasculino_Y"].ToString());

                        EscribirLetraxLetra(iMenorEdadTitularGenero_Masculino_X + MargenX_2, iMenorEdadTitularGenero_Masculino_Y + MargenY_2, "x", cb, document);
                    }
                    if (objFichaRegistralBE.strGENERO == "FEMENINO")
                    {
                        float iMenorEdadTitularGenero_Femenino_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.GeneroFemenino_X"].ToString());
                        float iMenorEdadTitularGenero_Femenino_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.GeneroFemenino_Y"].ToString());

                        EscribirLetraxLetra(iMenorEdadTitularGenero_Femenino_X + MargenX_2, iMenorEdadTitularGenero_Femenino_Y + MargenY_2, "x", cb, document);
                    }

                    if (objFichaRegistralBE.strDiscapacidad == "S")
                    {
                        float iMenorEdadTitularDiscapacidad_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.Discapacidad_SI_X"].ToString());
                        float iMenorEdadTitularDiscapacidad_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.Discapacidad_SI_Y"].ToString());

                        EscribirLetraxLetra(iMenorEdadTitularDiscapacidad_X + MargenX_2, iMenorEdadTitularDiscapacidad_Y + MargenY_2, "x", cb, document);
                    }
                    if (objFichaRegistralBE.strDiscapacidad == "N")
                    {
                        float iMenorEdadTitularDiscapacidad_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.Discapacidad_NO_X"].ToString());
                        float iMenorEdadTitularDiscapacidad_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.Discapacidad_NO_Y"].ToString());

                        EscribirLetraxLetra(iMenorEdadTitularDiscapacidad_X + MargenX_2, iMenorEdadTitularDiscapacidad_Y + MargenY_2, "x", cb, document);
                    }
                    //------------------------------------------
                    //DOCUMENTOS ADJUNTOS
                    //------------------------------------------
                    if (_dtDocumentos.Rows.Count > 0)
                    {
                        if (_dtDocumentos.Rows[0]["1"].ToString() == "1") //DOCUMENTO
                        {
                            float iMenorEdadDodAdjunto_DI_DECLARANTE_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.DI_DECLARANTE_X"].ToString());
                            float iMenorEdadDodAdjunto_DI_DECLARANTE_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.DI_DECLARANTE_Y"].ToString());

                            EscribirLetraxLetra(iMenorEdadDodAdjunto_DI_DECLARANTE_X + MargenX_2, iMenorEdadDodAdjunto_DI_DECLARANTE_Y + MargenY_2, "x", cb, document);

                            float iMenorEdadDodAdjunto_NUM_DI_DECLARANTE_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.NUM_DI_DECLARANTE_X"].ToString());
                            float iMenorEdadDodAdjunto_NUM_DI_DECLARANTE_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.NUM_DI_DECLARANTE_Y"].ToString());

                            EscribirLetraxLetra(iMenorEdadDodAdjunto_NUM_DI_DECLARANTE_X + MargenX_2, iMenorEdadDodAdjunto_NUM_DI_DECLARANTE_Y + MargenY_2, _dtDocumentos.Rows[1]["1"].ToString(), cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["2"].ToString() == "1") //DOCUMENTO
                        {
                            float iMenorEdadDodAdjunto_ACTA_NAC_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.ACTA_NAC_X"].ToString());
                            float iMenorEdadDodAdjunto_ACTA_NAC_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.ACTA_NAC_Y"].ToString());

                            EscribirLetraxLetra(iMenorEdadDodAdjunto_ACTA_NAC_X + MargenX_2, iMenorEdadDodAdjunto_ACTA_NAC_Y + MargenY_2, "x", cb, document);

                            float iMenorEdadDodAdjunto_NUM_ACTA_NAC_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.NUM_ACTA_NAC_X"].ToString());
                            float iMenorEdadDodAdjunto_NUM_ACTA_NAC_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.NUM_ACTA_NAC_Y"].ToString());

                            EscribirLetraxLetra(iMenorEdadDodAdjunto_NUM_ACTA_NAC_X + MargenX_2, iMenorEdadDodAdjunto_NUM_ACTA_NAC_Y + MargenY_2, _dtDocumentos.Rows[1]["2"].ToString(), cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["3"].ToString() == "1") //DOCUMENTO
                        {
                            float iMenorEdadDodAdjunto_DNI_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.DNI_X"].ToString());
                            float iMenorEdadDodAdjunto_DNI_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.DNI_Y"].ToString());

                            EscribirLetraxLetra(iMenorEdadDodAdjunto_DNI_X + MargenX_2, iMenorEdadDodAdjunto_DNI_Y + MargenY_2, "x", cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["4"].ToString() == "1") //DOCUMENTO
                        {
                            float iMenorEdadDodAdjunto_ACTA_PADRE_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.ACTA_PADRE_X"].ToString());
                            float iMenorEdadDodAdjunto_ACTA_PADRE_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.ACTA_PADRE_Y"].ToString());

                            EscribirLetraxLetra(iMenorEdadDodAdjunto_ACTA_PADRE_X + MargenX_2, iMenorEdadDodAdjunto_ACTA_PADRE_Y + MargenY_2, "x", cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["5"].ToString() == "1") //DOCUMENTO
                        {
                            float iMenorEdadDodAdjunto_ACTA_MADRE_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.ACTA_MADRE_X"].ToString());
                            float iMenorEdadDodAdjunto_ACTA_MADRE_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.ACTA_MADRE_Y"].ToString());

                            EscribirLetraxLetra(iMenorEdadDodAdjunto_ACTA_MADRE_X + MargenX_2, iMenorEdadDodAdjunto_ACTA_MADRE_Y + MargenY_2, "x", cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["6"].ToString() == "1") //DOCUMENTO
                        {
                            float iMenorEdadDodAdjunto_RECIBO_BN_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.RECIBO_BN_X"].ToString());
                            float iMenorEdadDodAdjunto_RECIBO_BN_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.RECIBO_BN_Y"].ToString());

                            EscribirLetraxLetra(iMenorEdadDodAdjunto_RECIBO_BN_X + MargenX_2, iMenorEdadDodAdjunto_RECIBO_BN_Y + MargenY_2, "x", cb, document);

                            float iMenorEdadDodAdjunto_NUM_RECIBO_BN_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.NUM_RECIBO_BN_X"].ToString());
                            float iMenorEdadDodAdjunto_NUM_RECIBO_BN_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.NUM_RECIBO_BN_Y"].ToString());

                            EscribirLetraxLetra(iMenorEdadDodAdjunto_NUM_RECIBO_BN_X + MargenX_2, iMenorEdadDodAdjunto_NUM_RECIBO_BN_Y + MargenY_2, _dtDocumentos.Rows[1]["6"].ToString(), cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["7"].ToString() == "1") //DOCUMENTO
                        {
                            float iMenorEdadDodAdjunto_CI_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.CI_X"].ToString());
                            float iMenorEdadDodAdjunto_CI_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.CI_Y"].ToString());

                            EscribirLetraxLetra(iMenorEdadDodAdjunto_CI_X + MargenX_2, iMenorEdadDodAdjunto_CI_Y + MargenY_2, "x", cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["8"].ToString() == "1") //DOCUMENTO
                        {
                            float iMenorEdadDodAdjunto_CE_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.CE_X"].ToString());
                            float iMenorEdadDodAdjunto_CE_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.CE_Y"].ToString());

                            EscribirLetraxLetra(iMenorEdadDodAdjunto_CE_X + MargenX_2, iMenorEdadDodAdjunto_CE_Y + MargenY_2, "x", cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["9"].ToString() == "1") //DOCUMENTO
                        {
                            float iMenorEdadDodAdjunto_CERTIFICADO_ESTUDIO_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.CERTIFICADO_ESTUDIO_X"].ToString());
                            float iMenorEdadDodAdjunto_CERTIFICADO_ESTUDIO_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.CERTIFICADO_ESTUDIO_Y"].ToString());

                            EscribirLetraxLetra(iMenorEdadDodAdjunto_CERTIFICADO_ESTUDIO_X + MargenX_2, iMenorEdadDodAdjunto_CERTIFICADO_ESTUDIO_Y + MargenY_2, "x", cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["10"].ToString() == "1") //DOCUMENTO
                        {
                            float iMenorEdadDodAdjunto_RECIBO_SERVICIO_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.RECIBO_SERVICIO_X"].ToString());
                            float iMenorEdadDodAdjunto_RECIBO_SERVICIO_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.RECIBO_SERVICIO_Y"].ToString());

                            EscribirLetraxLetra(iMenorEdadDodAdjunto_RECIBO_SERVICIO_X + MargenX_2, iMenorEdadDodAdjunto_RECIBO_SERVICIO_Y + MargenY_2, "x", cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["11"].ToString() == "1") //DOCUMENTO
                        {
                            float iMenorEdadDodAdjunto_DECLARACION_JURADA_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.DECLARACION_JURADA_X"].ToString());
                            float iMenorEdadDodAdjunto_DECLARACION_JURADA_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.DECLARACION_JURADA_Y"].ToString());

                            EscribirLetraxLetra(iMenorEdadDodAdjunto_DECLARACION_JURADA_X + MargenX_2, iMenorEdadDodAdjunto_DECLARACION_JURADA_Y + MargenY_2, "x", cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["12"].ToString() == "1") //DOCUMENTO
                        {
                            float iMenorEdadDodAdjunto_PSP_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.PSP_X"].ToString());
                            float iMenorEdadDodAdjunto_PSP_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.PSP_Y"].ToString());

                            EscribirLetraxLetra(iMenorEdadDodAdjunto_PSP_X + MargenX_2, iMenorEdadDodAdjunto_PSP_Y + MargenY_2, "x", cb, document);
                        }

                        if (_dtDocumentos.Rows[0]["13"].ToString() == "1") //DOCUMENTO
                        {
                            float iMenorEdadDodAdjunto_OTRO_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.OTRO_X"].ToString());
                            float iMenorEdadDodAdjunto_OTRO_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.OTRO_Y"].ToString());

                            EscribirLetraxLetra(iMenorEdadDodAdjunto_OTRO_X + MargenX_2, iMenorEdadDodAdjunto_OTRO_Y + MargenY_2, "x", cb, document);

                            float iMenorEdadDodAdjunto_NUM_OTRO_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.DESC_OTRO_X"].ToString());
                            float iMenorEdadDodAdjunto_NUM_OTRO_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Documento.DESC_OTRO_Y"].ToString());

                            EscribirLetraxLetra(iMenorEdadDodAdjunto_NUM_OTRO_X + MargenX_2, iMenorEdadDodAdjunto_NUM_OTRO_Y + MargenY_2, _dtDocumentos.Rows[1]["13"].ToString(), cb, document);
                        }
                    }
                }
                #endregion
                

                cb.EndText();

                document.NewPage();

                cb.BeginText();
                cb.SetFontAndSize(bfTimes, 10);

                #region parte superior segunda cara
                //------------------------------------------
                float iMenorEdadTitularFechaNacimiento_dia_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.FechaNacimiento_dia_X"].ToString());
                float iMenorEdadTitularFechaNacimiento_dia_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.FechaNacimiento_dia_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularFechaNacimiento_dia_X + MargenX_3, iMenorEdadTitularFechaNacimiento_dia_Y + MargenY_3, objFichaRegistralBE.strFecNacTitular_DD, cb, document);

                float iMenorEdadTitularFechaNacimiento_mes_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.FechaNacimiento_mes_X"].ToString());
                float iMenorEdadTitularFechaNacimiento_mes_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.FechaNacimiento_mes_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularFechaNacimiento_mes_X + MargenX_3, iMenorEdadTitularFechaNacimiento_mes_Y + MargenY_3, objFichaRegistralBE.strFecNacTitular_MM, cb, document);

                float iMenorEdadTitularFechaNacimiento_anio_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.FechaNacimiento_anio_X"].ToString());
                float iMenorEdadTitularFechaNacimiento_anio_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.FechaNacimiento_anio_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularFechaNacimiento_anio_X + MargenX_3, iMenorEdadTitularFechaNacimiento_anio_Y + MargenY_3, objFichaRegistralBE.strFecNacTitular_YYYY, cb, document);

                //------------------------------------------                
                float iMenorEdadTitularNacDpto_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.NacDpto_X"].ToString());
                float iMenorEdadTitularNacDpto_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.NacDpto_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularNacDpto_X + MargenX_3, iMenorEdadTitularNacDpto_Y + MargenY_3, objFichaRegistralBE.strNacDptoTitular, cb, document);

                float iMenorEdadTitularNacCodDpto_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.NacCodDpto_X"].ToString());
                float iMenorEdadTitularNacCodDpto_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.NacCodDpto_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularNacCodDpto_X + MargenX_3, iMenorEdadTitularNacCodDpto_Y + MargenY_3, objFichaRegistralBE.strNacCodDptoTitular, cb, document);

                float iMenorEdadTitularNacProv_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.NacProv_X"].ToString());
                float iMenorEdadTitularNacProv_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.NacProv_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularNacProv_X + MargenX_3, iMenorEdadTitularNacProv_Y + MargenY_3, objFichaRegistralBE.strNacProvTitular, cb, document);

                float iMenorEdadTitularNacCodProv_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.NacCodProv_X"].ToString());
                float iMenorEdadTitularNacCodProv_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.NacCodProv_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularNacCodProv_X + MargenX_3, iMenorEdadTitularNacCodProv_Y + MargenY_3, objFichaRegistralBE.strNacCodProvTitular, cb, document);

                float iMenorEdadTitularNacDist_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.NacDist_X"].ToString());
                float iMenorEdadTitularNacDist_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.NacDist_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularNacDist_X + MargenX_3, iMenorEdadTitularNacDist_Y + MargenY_3, objFichaRegistralBE.strNacDistTitular, cb, document);

                float iMenorEdadTitularNacCodDist_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.NacCodDist_X"].ToString());
                float iMenorEdadTitularNacCodDist_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.NacCodDist_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularNacCodDist_X + MargenX_3, iMenorEdadTitularNacCodDist_Y + MargenY_3, objFichaRegistralBE.strNacCodDistTitular, cb, document);
                //--------------------------------------------
                //Fecha: 03/03/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Imprimir el tipo de documento
                //          y el nro. de documento del Padre.
                //--------------------------------------------
                string strTipoDocumento = "";

                float iMenorEdadPadreTipoDocumento_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Padre.TipoDocumento_X"].ToString());
                float iMenorEdadPadreTipoDocumento_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Padre.TipoDocumento_Y"].ToString());
                    
                strTipoDocumento = obtenerTipodocumento(objFichaRegistralBE.strTipoDocPadre);

                EscribirLetraxLetra(iMenorEdadPadreTipoDocumento_X + MargenX_3, iMenorEdadPadreTipoDocumento_Y + MargenY_3, strTipoDocumento, cb, document);

                float iMenorEdadPadreNumeroDocumento_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Padre.NumeroDocumento_X"].ToString());
                float iMenorEdadPadreNumeroDocumento_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Padre.NumeroDocumento_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadPadreNumeroDocumento_X + MargenX_3, iMenorEdadPadreNumeroDocumento_Y + MargenY_3, objFichaRegistralBE.strNroDocPadre, cb, document);
                //------------------------------------------

                float iMenorEdadPadreApPaterno_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Padre.ApPaterno_X"].ToString());
                float iMenorEdadPadreApPaterno_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Padre.ApPaterno_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadPadreApPaterno_X + MargenX_3, iMenorEdadPadreApPaterno_Y + MargenY_3, objFichaRegistralBE.strApPaternoPadre, cb, document);

                float iMenorEdadPadreApMaterno_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Padre.ApMaterno_X"].ToString());
                float iMenorEdadPadreApMaterno_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Padre.ApMaterno_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadPadreApMaterno_X + MargenX_3, iMenorEdadPadreApMaterno_Y + MargenY_3, objFichaRegistralBE.strApMaternoPadre, cb, document);

                float iMenorEdadPadreNombres_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Padre.Nombres_X"].ToString());
                float iMenorEdadPadreNombres_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Padre.Nombres_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadPadreNombres_X + MargenX_3, iMenorEdadPadreNombres_Y + MargenY_3, objFichaRegistralBE.strNombresPadre, cb, document);

                //--------------------------------------------
                //Fecha: 03/03/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Imprimir el tipo de documento
                //          y el nro. de documento del Madre.
                //--------------------------------------------
                
               
                float iMenorEdadMadreTipoDocumento_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Madre.TipoDocumento_X"].ToString());
                float iMenorEdadMadreTipoDocumento_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Madre.TipoDocumento_Y"].ToString());

                strTipoDocumento = obtenerTipodocumento(objFichaRegistralBE.strTipoDocMadre);

                EscribirLetraxLetra(iMenorEdadMadreTipoDocumento_X + MargenX_3, iMenorEdadMadreTipoDocumento_Y + MargenY_3, strTipoDocumento, cb, document);
               
                float iMenorEdadMadreNumeroDocumento_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Madre.NumeroDocumento_X"].ToString());
                float iMenorEdadMadreNumeroDocumento_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Madre.NumeroDocumento_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadMadreNumeroDocumento_X + MargenX_3, iMenorEdadMadreNumeroDocumento_Y + MargenY_3, objFichaRegistralBE.strNroDocMadre, cb, document);
                //------------------------------------------

                float iMenorEdadMadreApPaterno_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Madre.ApPaterno_X"].ToString());
                float iMenorEdadMadreApPaterno_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Madre.ApPaterno_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadMadreApPaterno_X + MargenX_3, iMenorEdadMadreApPaterno_Y + MargenY_3, objFichaRegistralBE.strApPaternoMadre, cb, document);

                float iMenorEdadMadreApMaterno_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Madre.ApMaterno_X"].ToString());
                float iMenorEdadMadreApMaterno_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Madre.ApMaterno_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadMadreApMaterno_X + MargenX_3, iMenorEdadMadreApMaterno_Y + MargenY_3, objFichaRegistralBE.strApMaternoMadre, cb, document);

                float iMenorEdadMadreNombres_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Madre.Nombres_X"].ToString());
                float iMenorEdadMadreNombres_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Madre.Nombres_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadMadreNombres_X + MargenX_3, iMenorEdadMadreNombres_Y + MargenY_3, objFichaRegistralBE.strNombresMadre, cb, document);

                //-------------------------------------------------
                //Fecha: 03/03/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Imprimir el tipo de documento
                //          y el nro. de documento del Declarante.
                //-------------------------------------------------
                                
                float iMenorEdadDeclaranteTipoDocumento_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Declarante.TipoDocumento_X"].ToString());
                float iMenorEdadDeclaranteTipoDocumento_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Declarante.TipoDocumento_Y"].ToString());

                strTipoDocumento = obtenerTipodocumento(objFichaRegistralBE.strTipoDocDeclarante);

                EscribirLetraxLetra(iMenorEdadDeclaranteTipoDocumento_X + MargenX_3, iMenorEdadDeclaranteTipoDocumento_Y + MargenY_3, strTipoDocumento, cb, document);
                
                float iMenorEdadDeclaranteNumeroDocumento_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Declarante.NumeroDocumento_X"].ToString());
                float iMenorEdadDeclaranteNumeroDocumento_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Declarante.NumeroDocumento_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadDeclaranteNumeroDocumento_X + MargenX_3, iMenorEdadDeclaranteNumeroDocumento_Y + MargenY_3, objFichaRegistralBE.strNroDocDeclarante, cb, document);
                //------------------------------------------

                float iMenorEdadDeclaranteApPaterno_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Declarante.ApPaterno_X"].ToString());
                float iMenorEdadDeclaranteApPaterno_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Declarante.ApPaterno_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadDeclaranteApPaterno_X + MargenX_3, iMenorEdadDeclaranteApPaterno_Y + MargenY_3, objFichaRegistralBE.strApPaternoDeclarante, cb, document);

                float iMenorEdadDeclaranteApMaterno_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Declarante.ApMaterno_X"].ToString());
                float iMenorEdadDeclaranteApMaterno_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Declarante.ApMaterno_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadDeclaranteApMaterno_X + MargenX_3, iMenorEdadDeclaranteApMaterno_Y + MargenY_3, objFichaRegistralBE.strApMaternoDeclarante, cb, document);

                float iMenorEdadDeclaranteNombres_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Declarante.Nombres_X"].ToString());
                float iMenorEdadDeclaranteNombres_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Declarante.Nombres_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadDeclaranteNombres_X + MargenX_3, iMenorEdadDeclaranteNombres_Y + MargenY_3, objFichaRegistralBE.strNombresDeclarante, cb, document);
                //------------------------------------------

                float iMenorEdadTitularCodLocalDestino_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.CodLocalDestino_X"].ToString());
                float iMenorEdadTitularCodLocalDestino_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.CodLocalDestino_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularCodLocalDestino_X + MargenX_3, iMenorEdadTitularCodLocalDestino_Y + MargenY_3, objFichaRegistralBE.strCodigoLocalDestino, cb, document);

                //------------------------------------------
                float iMenorEdadTitularTelefono_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.Telefono_X"].ToString());
                float iMenorEdadTitularTelefono_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.Telefono_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularTelefono_X + MargenX_3, iMenorEdadTitularTelefono_Y + MargenY_3, objFichaRegistralBE.strTelefonoTitular, cb, document);
                //------------------------------------------

                float iMenorEdadTitularCorreo_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.Correo_X"].ToString());
                float iMenorEdadTitularCorreo_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Titular.Correo_Y"].ToString());

                EscribirLetraxLetra(iMenorEdadTitularCorreo_X + MargenX_3, iMenorEdadTitularCorreo_Y + MargenY_3, objFichaRegistralBE.strCorreoElectronicoTitular, cb, document);

                //------------------------------------------   

                /*Nuevos Campos*/
                //------------------------------------------
                /*
                 --TIPO DECLARANTE
                 */
                if (dtImpresionCompleta.Rows[0]["descripcion"].ToString() == "SI")
                {
                    cb.SetFontAndSize(bfTimes, 7);
                    float iMenorEdadDeclaranteTipo_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Declarante.TipoDeclarante_X"].ToString());
                    float iMenorEdadDeclaranteTipo_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Declarante.TipoDeclarante_Y"].ToString());

                    EscribirLetraxLetra(iMenorEdadDeclaranteTipo_X, iMenorEdadDeclaranteTipo_Y, objFichaRegistralBE.strTIPO_DECLARANTE, cb, document);
                    /*
                     --TIPO Tutor
                     */
                    float iMenorEdadDeclaranteTutor_X = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Declarante.TUTOR_X"].ToString());
                    float iMenorEdadDeclaranteTutor_Y = float.Parse(ConfigurationManager.AppSettings["FichaRegistral.MenorEdad.Declarante.TUTOR_Y"].ToString());

                    EscribirLetraxLetra(iMenorEdadDeclaranteTutor_X, iMenorEdadDeclaranteTutor_Y, objFichaRegistralBE.strTIPO_TUTOR, cb, document);
                }
                #endregion

                //FIN
                //------------------------------------------
                cb.EndText();
                //------------------------------------------
                document.Close();
                oStreamReader.Close();
                writer.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 11/01/2017
        // Objetivo: Asignar texto al documento PDF de la ficha Registral 
        //------------------------------------------------------

        private static void EscribirLetraxLetra(float ejeXInicio, float ejeYInicio, string palabra, iTextSharp.text.pdf.PdfContentByte cb, iTextSharp.text.Document document)
        {
            cb.SetTextMatrix(ejeXInicio, document.PageSize.Height - ejeYInicio);
            cb.ShowText(palabra.ToString());
        }

        //------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 11/01/2017
        // Objetivo: Establecer los campos obligatorios 
        // de la ficha registral para menores de edad.         
        //------------------------------------------------------

        private void EstablecerCamposObligatoriosMenorEdad()
        {
            

            if (ddl_TipoParticipante.SelectedIndex == 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Participante", "Seleccione el tipo de participante"));
                ddl_TipoParticipante.Focus();
                return;
            }


            int intTipoParticipante = Convert.ToInt32(ddl_TipoParticipante.SelectedValue);
            switch (intTipoParticipante)
            {
                case (int)Enumerador.enmFichaTipoParticipanteMenor.TITULAR:
                     string strTitular_TipoDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Titular.TipoDocumento"].ToString();
                     string strTitular_NumeroDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Titular.NumeroDocumento"].ToString();
                     string strTitular_PrimerApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Titular.PrimerApellido"].ToString();
                     string strTitular_SegundoApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Titular.SegundoApellido"].ToString();
                     string strTitular_Nombres = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Titular.Nombres"].ToString();
                     string strTitular_Domicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Titular.Domicilio"].ToString();
                     string strTitular_UbigeoDomicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Titular.UbigeoDomicilio"].ToString();
                     string strTitular_FechaNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Titular.FechaNacimiento"].ToString();
                     string strTitular_LugarNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Titular.LugarNacimiento"].ToString();
                     string strTitular_UbigeoNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Titular.UbigeoNacimiento"].ToString();
                     
                     EstablecerAsteriscosVisibles(strTitular_TipoDocumento, strTitular_NumeroDocumento, strTitular_PrimerApellido, strTitular_SegundoApellido, strTitular_Nombres,
                                                strTitular_Domicilio, strTitular_UbigeoDomicilio, strTitular_FechaNacimiento, strTitular_LugarNacimiento, strTitular_UbigeoNacimiento);
                     break;
                case (int)Enumerador.enmFichaTipoParticipanteMenor.PADRE:
                     string strPadre_TipoDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Padre.TipoDocumento"].ToString();
                     string strPadre_NumeroDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Padre.NumeroDocumento"].ToString();
                     string strPadre_PrimerApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Padre.PrimerApellido"].ToString();
                     string strPadre_SegundoApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Padre.SegundoApellido"].ToString();
                     string strPadre_Nombres = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Padre.Nombres"].ToString();
                     string strPadre_Domicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Padre.Domicilio"].ToString();
                     string strPadre_UbigeoDomicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Padre.UbigeoDomicilio"].ToString();
                     string strPadre_FechaNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Padre.FechaNacimiento"].ToString();
                     string strPadre_LugarNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Padre.LugarNacimiento"].ToString();
                     string strPadre_UbigeoNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Padre.UbigeoNacimiento"].ToString();
                     
                     EstablecerAsteriscosVisibles(strPadre_TipoDocumento, strPadre_NumeroDocumento, strPadre_PrimerApellido, strPadre_SegundoApellido, strPadre_Nombres,
                                                strPadre_Domicilio, strPadre_UbigeoDomicilio, strPadre_FechaNacimiento, strPadre_LugarNacimiento, strPadre_UbigeoNacimiento);
                     break;
                case (int)Enumerador.enmFichaTipoParticipanteMenor.MADRE:
                     string strMadre_TipoDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Madre.TipoDocumento"].ToString();
                     string strMadre_NumeroDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Madre.NumeroDocumento"].ToString();
                     string strMadre_PrimerApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Madre.PrimerApellido"].ToString();
                     string strMadre_SegundoApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Madre.SegundoApellido"].ToString();
                     string strMadre_Nombres = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Madre.Nombres"].ToString();
                     string strMadre_Domicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Madre.Domicilio"].ToString();
                     string strMadre_UbigeoDomicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Madre.UbigeoDomicilio"].ToString();
                     string strMadre_FechaNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Madre.FechaNacimiento"].ToString();
                     string strMadre_LugarNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Madre.LugarNacimiento"].ToString();
                     string strMadre_UbigeoNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Madre.UbigeoNacimiento"].ToString();
                     
                     EstablecerAsteriscosVisibles(strMadre_TipoDocumento, strMadre_NumeroDocumento, strMadre_PrimerApellido, strMadre_SegundoApellido, strMadre_Nombres,
                                                strMadre_Domicilio, strMadre_UbigeoDomicilio, strMadre_FechaNacimiento, strMadre_LugarNacimiento, strMadre_UbigeoNacimiento);
                     break;
                case (int)Enumerador.enmFichaTipoParticipanteMenor.DECLARANTE:
                     string strDeclarante_TipoDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Declarante.TipoDocumento"].ToString();
                     string strDeclarante_NumeroDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Declarante.NumeroDocumento"].ToString();
                     string strDeclarante_PrimerApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Declarante.PrimerApellido"].ToString();
                     string strDeclarante_SegundoApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Declarante.SegundoApellido"].ToString();
                     string strDeclarante_Nombres = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Declarante.Nombres"].ToString();
                     string strDeclarante_Domicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Declarante.Domicilio"].ToString();
                     string strDeclarante_UbigeoDomicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Declarante.UbigeoDomicilio"].ToString();
                     string strDeclarante_FechaNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Declarante.FechaNacimiento"].ToString();
                     string strDeclarante_LugarNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Declarante.LugarNacimiento"].ToString();
                     string strDeclarante_UbigeoNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMenorEdadValidar.Declarante.UbigeoNacimiento"].ToString();

                     EstablecerAsteriscosVisibles(strDeclarante_TipoDocumento, strDeclarante_NumeroDocumento, strDeclarante_PrimerApellido, strDeclarante_SegundoApellido, strDeclarante_Nombres,
                                                 strDeclarante_Domicilio, strDeclarante_UbigeoDomicilio, strDeclarante_FechaNacimiento, strDeclarante_LugarNacimiento, strDeclarante_UbigeoNacimiento);
                     break;
                default:
                     break;
            }

        }

        //------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 11/01/2017
        // Objetivo: Establecer los campos obligatorios 
        // de la ficha registral para mayores de edad.         
        //------------------------------------------------------
        private void EstablecerCamposObligatoriosMayorEdad()
        {


            if (ddl_TipoParticipante.SelectedIndex == 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Participante", "Seleccione el tipo de participante"));
                ddl_TipoParticipante.Focus();
                return;
            }


            int intTipoParticipante = Convert.ToInt32(ddl_TipoParticipante.SelectedValue);
            switch (intTipoParticipante)
            {
                case (int)Enumerador.enmFichaTipoParticipanteMayor.TITULAR:
                     string strTitular_TipoDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Titular.TipoDocumento"].ToString();
                    string strTitular_NumeroDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Titular.NumeroDocumento"].ToString();
                    string strTitular_PrimerApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Titular.PrimerApellido"].ToString();
                    string strTitular_SegundoApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Titular.SegundoApellido"].ToString();
                    string strTitular_Nombres = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Titular.Nombres"].ToString();
                    string strTitular_Domicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Titular.Domicilio"].ToString();
                    string strTitular_UbigeoDomicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Titular.UbigeoDomicilio"].ToString();
                    string strTitular_FechaNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Titular.FechaNacimiento"].ToString();
                    string strTitular_LugarNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Titular.LugarNacimiento"].ToString();
                    string strTitular_UbigeoNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Titular.UbigeoNacimiento"].ToString();

                    EstablecerAsteriscosVisibles(strTitular_TipoDocumento, strTitular_NumeroDocumento, strTitular_PrimerApellido, strTitular_SegundoApellido, strTitular_Nombres,
                                               strTitular_Domicilio, strTitular_UbigeoDomicilio, strTitular_FechaNacimiento, strTitular_LugarNacimiento, strTitular_UbigeoNacimiento);
                    break;
                case (int)Enumerador.enmFichaTipoParticipanteMayor.PADRE:
                    string strPadre_TipoDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Padre.TipoDocumento"].ToString();
                    string strPadre_NumeroDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Padre.NumeroDocumento"].ToString();
                    string strPadre_PrimerApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Padre.PrimerApellido"].ToString();
                    string strPadre_SegundoApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Padre.SegundoApellido"].ToString();
                    string strPadre_Nombres = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Padre.Nombres"].ToString();
                    string strPadre_Domicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Padre.Domicilio"].ToString();
                    string strPadre_UbigeoDomicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Padre.UbigeoDomicilio"].ToString();
                    string strPadre_FechaNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Padre.FechaNacimiento"].ToString();
                    string strPadre_LugarNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Padre.LugarNacimiento"].ToString();
                    string strPadre_UbigeoNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Padre.UbigeoNacimiento"].ToString();

                    EstablecerAsteriscosVisibles(strPadre_TipoDocumento, strPadre_NumeroDocumento, strPadre_PrimerApellido, strPadre_SegundoApellido, strPadre_Nombres,
                                               strPadre_Domicilio, strPadre_UbigeoDomicilio, strPadre_FechaNacimiento, strPadre_LugarNacimiento, strPadre_UbigeoNacimiento);
                    break;
                case (int)Enumerador.enmFichaTipoParticipanteMayor.MADRE:
                     string strMadre_TipoDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Madre.TipoDocumento"].ToString();
                    string strMadre_NumeroDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Madre.NumeroDocumento"].ToString();
                    string strMadre_PrimerApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Madre.PrimerApellido"].ToString();
                    string strMadre_SegundoApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Madre.SegundoApellido"].ToString();
                    string strMadre_Nombres = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Madre.Nombres"].ToString();
                    string strMadre_Domicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Madre.Domicilio"].ToString();
                    string strMadre_UbigeoDomicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Madre.UbigeoDomicilio"].ToString();
                    string strMadre_FechaNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Madre.FechaNacimiento"].ToString();
                    string strMadre_LugarNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Madre.LugarNacimiento"].ToString();
                    string strMadre_UbigeoNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Madre.UbigeoNacimiento"].ToString();

                    EstablecerAsteriscosVisibles(strMadre_TipoDocumento, strMadre_NumeroDocumento, strMadre_PrimerApellido, strMadre_SegundoApellido, strMadre_Nombres,
                                               strMadre_Domicilio, strMadre_UbigeoDomicilio, strMadre_FechaNacimiento, strMadre_LugarNacimiento, strMadre_UbigeoNacimiento);
                    break;
                case (int)Enumerador.enmFichaTipoParticipanteMayor.CONYUGE:
                    string strConyuge_TipoDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Conyuge.TipoDocumento"].ToString();
                    string strConyuge_NumeroDocumento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Conyuge.NumeroDocumento"].ToString();
                    string strConyuge_PrimerApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Conyuge.PrimerApellido"].ToString();
                    string strConyuge_SegundoApellido = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Conyuge.SegundoApellido"].ToString();
                    string strConyuge_Nombres = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Conyuge.Nombres"].ToString();
                    string strConyuge_Domicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Conyuge.Domicilio"].ToString();
                    string strConyuge_UbigeoDomicilio = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Conyuge.UbigeoDomicilio"].ToString();
                    string strConyuge_FechaNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Conyuge.FechaNacimiento"].ToString();
                    string strConyuge_LugarNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Conyuge.LugarNacimiento"].ToString();
                    string strConyuge_UbigeoNacimiento = ConfigurationManager.AppSettings["TarifaParticipanteFichaRegistralMayorEdadValidar.Conyuge.UbigeoNacimiento"].ToString();

                    EstablecerAsteriscosVisibles(strConyuge_TipoDocumento, strConyuge_NumeroDocumento, strConyuge_PrimerApellido, strConyuge_SegundoApellido, strConyuge_Nombres,
                                                strConyuge_Domicilio, strConyuge_UbigeoDomicilio, strConyuge_FechaNacimiento, strConyuge_LugarNacimiento, strConyuge_UbigeoNacimiento);
                    break;
                default:
                    break;
            }

        }

        //------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 11/01/2017
        // Objetivo: Poner los asteriscos visibles para
        //          los campos obligatorios.
        //------------------------------------------------------

        private void EstablecerAsteriscosVisibles(string strTarifas_TipoDocumento, string strTarifas_NumeroDocumento, string strTarifas_PrimerApellido, string strTarifas_SegundoApellido,
                                                  string strTarifas_Nombres, string strTarifas_Domicilio, string strTarifas_UbigeoDomicilio, string strTarifas_FechaNacimiento,
                                                  string strTarifas_LugarNacimiento, string strTarifas_UbigeoNacimiento)
        {
            BE.RE_TARIFA_PAGO objTarifaPago = new RE_TARIFA_PAGO();
            objTarifaPago = (BE.RE_TARIFA_PAGO)Session[Constantes.CONST_SESION_OBJ_TARIFA_PAGO];
            string strTarifa = objTarifaPago.vTarifa.Trim().ToUpper();

            string[] arrTarifas_TipoDocumento = strTarifas_TipoDocumento.Trim().Split(',');

            if (Util.ContieneItemArreglo(arrTarifas_TipoDocumento, strTarifa))
            {
                lbl_asterisco_02.Visible = true;
            }
            string[] arrTarifas_NumeroDocumento = strTarifas_NumeroDocumento.Trim().Split(',');
            if (Util.ContieneItemArreglo(arrTarifas_NumeroDocumento, strTarifa))
            {
                lbl_asterisco_03.Visible = true;
            }
            string[] arrTarifas_PrimerApellido = strTarifas_PrimerApellido.Trim().Split(',');
            if (Util.ContieneItemArreglo(arrTarifas_PrimerApellido, strTarifa))
            {
                lbl_asterisco_04.Visible = true;
            }
            string[] arrTarifas_SegundoApellido = strTarifas_SegundoApellido.Trim().Split(',');
            if (Util.ContieneItemArreglo(arrTarifas_SegundoApellido, strTarifa))
            {
                lbl_asterisco_05.Visible = true;
            }
            string[] arrTarifas_Nombres = strTarifas_Nombres.Trim().Split(',');
            if (Util.ContieneItemArreglo(arrTarifas_Nombres, strTarifa))
            {
                lbl_asterisco_06.Visible = true;
            }
            string[] arrTarifas_Domicilio = strTarifas_Domicilio.Trim().Split(',');
            if (Util.ContieneItemArreglo(arrTarifas_Domicilio, strTarifa))
            {
                //lbl_asterisco_07.Visible = true;
            }
            string[] arrTarifas_UbigeoDomicilio = strTarifas_UbigeoDomicilio.Trim().Split(',');
            if (Util.ContieneItemArreglo(arrTarifas_UbigeoDomicilio, strTarifa))
            {
                //lbl_asterisco_08.Visible = true;
                //lbl_asterisco_09.Visible = true;
                //lbl_asterisco_10.Visible = true;
            }
            string[] arrTarifas_FechaNacimiento = strTarifas_FechaNacimiento.Trim().Split(',');
            if (Util.ContieneItemArreglo(arrTarifas_FechaNacimiento, strTarifa))
            {
                //lbl_asterisco_11.Visible = true;
            }
            string[] arrTarifas_UbigeoNacimiento = strTarifas_UbigeoNacimiento.Trim().Split(',');
            if (Util.ContieneItemArreglo(arrTarifas_UbigeoNacimiento, strTarifa))
            {
                //lbl_asterisco_12.Visible = true;
                //lbl_asterisco_13.Visible = true;
                //lbl_asterisco_14.Visible = true;
            }
        }

        //--------------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 11/01/2017
        // Objetivo: Evento que actualiza los asteriscos para 
        //  los campos obligatorios al cambiar el tipo de participante
        //--------------------------------------------------------------

        protected void ddl_TipoParticipante_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_asterisco_01.Visible = true;
            lbl_asterisco_02.Visible = false;
            lbl_asterisco_03.Visible = false;
            lbl_asterisco_04.Visible = false;
            lbl_asterisco_05.Visible = false;
            lbl_asterisco_06.Visible = false;
            lbl_asterisco_07.Visible = false;
            lbl_asterisco_08.Visible = false;
            lbl_asterisco_09.Visible = false;
            lbl_asterisco_10.Visible = false;
            lbl_asterisco_11.Visible = false;
            lbl_asterisco_12.Visible = false;
            lbl_asterisco_13.Visible = false;
            lbl_asterisco_14.Visible = false;
            bool esMayorEdad = EsMayorEdad();
            if (esMayorEdad)
            {
                EstablecerCamposObligatoriosMayorEdad();
            }
            else
            {
                EstablecerCamposObligatoriosMenorEdad();
            }
            ddl_TipoDocParticipante.SelectedValue = Convert.ToString((int)Enumerador.enmTipoDocumento.DNI);
            txtNroDocParticipante.Focus();
            //if (hNuevoParticipante.Value != "1")
            //{
            //    Comun.EjecutarScript(Page, "desabilitarBotonesEditar();");
            //}
            //chkConsignaApeCas.Visible = false;
            lblNota.Visible = false;
            if (ddl_TipoParticipante.SelectedItem.Text != "TITULAR")
            {
                if (ddl_TipoParticipante.SelectedItem.Text == "DECLARANTE")
                {
                    DIV_TIPO_DECLARANTE.Visible = true;
                    DIV_OTROS.Visible = false;
                }
                else
                {
                    DIV_LUGAR_DOMICILIO.Visible = false;
                    DIV_LUGAR_NACIMIENTO.Visible = false;
                    DIV_OTROS.Visible = false;
                    DIV_TIPO_DECLARANTE.Visible = false;
                }
                
            }
            else {
                if (esMayorEdad)
                {
                    //chkConsignaApeCas.Visible = true;
                    lblNota.Visible = true;
                }
                DIV_LUGAR_DOMICILIO.Visible = true;
                DIV_LUGAR_NACIMIENTO.Visible = true;
                DIV_OTROS.Visible = true;
                DIV_TIPO_DECLARANTE.Visible = false;
            }

            
        }


        protected void rbManual_CheckedChanged(object sender, EventArgs e)
        {
            if (rbManual.Checked)
            {
                DivSemiAutomatico.Visible = false;
            }
            else
            {
                DivSemiAutomatico.Visible = true;
            }
        }

        protected void rbSemi_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSemi.Checked)
            {
                DivSemiAutomatico.Visible = true;
            }
            else
            {
                DivSemiAutomatico.Visible = false;
            }
        }

        protected void btnAceptarTitular_Click(object sender, EventArgs e)
        {
            CargarPersonaTitularMenorEdad();
        }

        protected void ddlEstadoFicha_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEstadoFicha.SelectedValue != hEstadoInicial.Value)
            {
                if (txtObservacionFicha.Text == hObservación.Value)
                {
                    txtObservacionFicha.Text = "";
                }
            }
            else {
                txtObservacionFicha.Text = hObservación.Value;
            }
        }
        //-------------------------------------------------------
        //Fecha: 19/07/2018
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Agregar el método de grabar y anular
        //-------------------------------------------------------

        private bool validarDatosMSIAP()
        {
            if (ctrFechaSolicitudMSIAP.Text.Trim().Length == 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Antecedentes Penales", "Seleccione la fecha de Solicitud."));
                return false;
            }

            if (ctrFechaRespuestaMSIAP.Text.Length > 0)
            {
                if (Comun.EsFecha(ctrFechaRespuestaMSIAP.Text.Trim()) == false)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Antecedentes Penales", "La fecha de respuesta MSIAP no es válida."));
                    return false;
                }
            }
            if (ddlTramiteMSIAP.SelectedIndex == 0)
            {
                   Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Antecedentes Penales", "Seleccione el tipo de trámite para Antecedente Penal."));
                   return false;
            }
            //-----------------------------------------------------
            //Fecha: 10/12/2019
            //Autor: Miguel Márquez Beltrán
            //Motivo: Validar que solo exista un antecedente Penal
            //          por actuación detalle. 
            //-----------------------------------------------------
            long lngAntecedentePenalId = 0;
            long lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);
            short intOficinaConsular = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            if (hanpe_iAntecedentePenalId.Value != "")
            {
                lngAntecedentePenalId = Convert.ToInt64(hanpe_iAntecedentePenalId.Value);
            }
            if (lngAntecedentePenalId == 0)
            {
                Antecedente_PenalBL objBL = new Antecedente_PenalBL();
                DataTable dt = new DataTable();
                dt = objBL.Consultar(lngActuacionDetalleId, intOficinaConsular);
                if (dt.Rows.Count > 0)
                {
                   Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Antecedentes Penales", "Ya existe registrado un Antecedente Penal."));
                   return false;
                }
            }

            //if (ddlUsuarioAutorizadoMSIAP.SelectedIndex == 0)
            //{
            //    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Antecedentes Penales", "Seleccione al usuario autorizado."));
            //    return false;
            //}
            //if (txtNumeroSolicutdWebMSIAP.Text.Trim().Length == 0)
            //{
            //    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Antecedentes Penales", "Digite el Número de Solicitud Web."));
            //    return false;
            //}
            //if (txtOficioRespuestaMSIAP.Text.Trim().Length == 0)
            //{
            //    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Antecedentes Penales", "Digite el Oficio de Respuesta."));
            //    return false;
            //}
            //if (ctrFechaRespuestaMSIAP.Text.Trim().Length == 0)
            //{
            //    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Antecedentes Penales", "Seleccione la fecha de respuesta."));
            //    return false;
            //}

            return true;
        }

              

        private bool AnularAntecedentesPenales()
        {
            bool bAnulacionExitosa = false;
            Antecedente_PenalBL objBL = new Antecedente_PenalBL();
            SGAC.BE.MRE.RE_ANTECEDENTE_PENAL objBE = new BE.MRE.RE_ANTECEDENTE_PENAL();


            long lngAntecedentePenalId = Convert.ToInt64(hanpe_iAntecedentePenalId.Value);
            short intOficinaConsular = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            objBE.anpe_iAntecedentePenalId = lngAntecedentePenalId;
            objBE.OficinaConsultar = intOficinaConsular;
            objBE.anpe_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            objBE.anpe_vIpModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

            objBL.Anular(objBE);

            if (objBL.isError)
            {
                bAnulacionExitosa = false;
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ANTECEDENTE PENAL", Constantes.CONST_MENSAJE_OPERACION_FALLIDA, false, 190, 250));
            }
            else
            {
                bAnulacionExitosa = true;
               // Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "ANTECEDENTE PENAL", Constantes.CONST_MENSAJE_ELIMINADO, false, 190, 250));
                limpiarDatosMSIAP();
                btnAnularAntecedente.Enabled = false;
            }
            return bAnulacionExitosa;
        }

        private void CargarDatosAntecedentespenales()
        {            
            btnGrabarAntecedente.Enabled = true;

            long lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);           

            CargarFuncionarios(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), 0);
            //------------------------------------------------------------------------------------------------
            limpiarDatosMSIAP();

            short intOficinaConsular = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            Antecedente_PenalBL objBL = new Antecedente_PenalBL();
            DataTable dtAntecedentepenal = new DataTable();

            dtAntecedentepenal = objBL.Consultar(lngActuacionDetalleId, intOficinaConsular, "", "", "");

            if (dtAntecedentepenal.Rows.Count > 0)
            {
                hanpe_iAntecedentePenalId.Value = dtAntecedentepenal.Rows[0]["anpe_iAntecedentePenalId"].ToString();
                ctrFechaSolicitudMSIAP.set_Value = Comun.FormatearFecha(dtAntecedentepenal.Rows[0]["anpe_dFechaSolicitud"].ToString());
                string strUsuarioFuncionarioId = dtAntecedentepenal.Rows[0]["anpe_iFuncionarioId"].ToString();
                if (strUsuarioFuncionarioId == "0")
                {
                    ddlUsuarioAutorizadoMSIAP.SelectedIndex = 0;
                }
                else
                {
                    ddlUsuarioAutorizadoMSIAP.SelectedValue = strUsuarioFuncionarioId;
                }
                
                txtNumeroSolicutdWebMSIAP.Text = dtAntecedentepenal.Rows[0]["anpe_vNumeroSolicitud"].ToString();
                txtOficioRespuestaMSIAP.Text = dtAntecedentepenal.Rows[0]["anpe_vNumeroOficioRpta"].ToString();

                if (dtAntecedentepenal.Rows[0]["anpe_dFechaRespuesta"].ToString().Length == 0)
                {
                    ctrFechaRespuestaMSIAP.Text = "";
                }
                else
                {
                    ctrFechaRespuestaMSIAP.set_Value = Comun.FormatearFecha(dtAntecedentepenal.Rows[0]["anpe_dFechaRespuesta"].ToString());
                }

                txtObservacionMSIAP.Text = dtAntecedentepenal.Rows[0]["anpe_vObservacion"].ToString();
                ddlTramiteMSIAP.SelectedValue = dtAntecedentepenal.Rows[0]["anpe_sSolicitaParaId"].ToString();
                //---------------------------------------------------                 
                btnAnularAntecedente.Enabled = true;
                btnRegistrarAntecedentesPenales.Visible = true;
                btnNoAntecedentes.Visible = true;
                cargarDetalleAntecedentesPenales();
            }
            else
            {
                btnRegistrarAntecedentesPenales.Visible = false;
                btnNoAntecedentes.Visible = false;
                btnAnularAntecedente.Enabled = false;
                Session["AntecedentePenalDetalleId"] = "0";
                lblTituloAntecedentesPenalesDetalle.Visible = false;
                CtrlPaginadorAntecedentesPenales.Visible = false;
            }
            updAntecedente.Update();
        }

        private void limpiarDatosMSIAP()
        {
            hanpe_iAntecedentePenalId.Value = "0";
            
            ctrFechaSolicitudMSIAP.Text = "";
            ddlUsuarioAutorizadoMSIAP.SelectedIndex = 0;
            txtNumeroSolicutdWebMSIAP.Text = "";
            txtOficioRespuestaMSIAP.Text = "";
            ctrFechaRespuestaMSIAP.Text = "";
            ctrFechaRespuestaMSIAP.AllowFutureDate = true;
            txtObservacionMSIAP.Text = "";

            ActuacionConsultaBL ActConsultaDetBL = new ActuacionConsultaBL();
            DataTable dtActuacionDet = new DataTable();
            long lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);
            dtActuacionDet = ActConsultaDetBL.ObtenerDatosPorActuacionDetalle(lngActuacionDetalleId);
            if (dtActuacionDet.Rows.Count > 0)
            {
                ctrFechaSolicitudMSIAP.set_Value = Comun.FormatearFecha(dtActuacionDet.Rows[0]["acde_dFechaRegistro"].ToString());
            }

        }

        private void CargarFuncionarios(int sOfConsularId, int IFuncionarioId)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = funcionario.dtFuncionario(sOfConsularId, IFuncionarioId);

                ddlUsuarioAutorizadoMSIAP.DataSource = null;
                ddlUsuarioAutorizadoMSIAP.Items.Clear();
                ddlUsuarioAutorizadoMSIAP.DataTextField = "vFuncionario";
                ddlUsuarioAutorizadoMSIAP.DataValueField = "iFuncionarioId";
                ddlUsuarioAutorizadoMSIAP.DataSource = dt;
                ddlUsuarioAutorizadoMSIAP.DataBind();
                ddlUsuarioAutorizadoMSIAP.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnGrabarAntecedente_Click(object sender, EventArgs e)
        {
            if (validarDatosMSIAP())
            {
                Antecedente_PenalBL objBL = new Antecedente_PenalBL();
                SGAC.BE.MRE.RE_ANTECEDENTE_PENAL objBE = new BE.MRE.RE_ANTECEDENTE_PENAL();

                long lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);

                DateTime dFechaSolicitud = ctrFechaSolicitudMSIAP.Value();

                string strNumeroSolicitud = txtNumeroSolicutdWebMSIAP.Text.Trim().ToUpper();
                string strOficiorespuesta = txtOficioRespuestaMSIAP.Text.Trim().ToUpper();

                string strObservacion = txtObservacionMSIAP.Text.Trim().ToUpper();

                long lngAntecedentePenalId = 0;

                if (hanpe_iAntecedentePenalId.Value != "")
                {
                    lngAntecedentePenalId = Convert.ToInt64(hanpe_iAntecedentePenalId.Value);
                }

                short intOficinaConsular = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                //-------------------------------------------------------------
                objBE.anpe_iAntecedentePenalId = lngAntecedentePenalId;
                objBE.OficinaConsultar = intOficinaConsular;
                objBE.anpe_iActuacionDetalleId = lngActuacionDetalleId;
                objBE.anpe_dFechaSolicitud = Comun.FormatearFecha(dFechaSolicitud.ToShortDateString());

                if (ddlUsuarioAutorizadoMSIAP.SelectedIndex > 0)
                {
                    int intUsuarioAutorizadorId = Convert.ToInt32(ddlUsuarioAutorizadoMSIAP.SelectedValue);
                    objBE.anpe_iFuncionarioId = intUsuarioAutorizadorId;
                }

                objBE.anpe_vNumeroSolicitud = strNumeroSolicitud;

                objBE.anpe_vNumeroOficioRpta = strOficiorespuesta;

                if (ctrFechaRespuestaMSIAP.Text.Length > 0)
                {
                    DateTime dFechaRespuesta = ctrFechaRespuestaMSIAP.Value();
                    objBE.anpe_dFechaRespuesta = Comun.FormatearFecha(dFechaRespuesta.ToShortDateString());
                }

                objBE.anpe_vObservacion = strObservacion;
                objBE.anpe_sSolicitaParaId = Convert.ToInt16(ddlTramiteMSIAP.SelectedValue);
                objBE.anpe_cRegistraAntecedentesPenales = "N";

                if (objBE.anpe_iAntecedentePenalId == 0)
                {
                    objBE.anpe_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    objBE.anpe_vIpCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                }
                else
                {
                    objBE.anpe_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    objBE.anpe_vIpModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                }
                objBL.Grabar(objBE);
                //-------------------------------------------------------------
                if (objBL.isError)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ANTECEDENTE PENAL", Constantes.CONST_MENSAJE_OPERACION_FALLIDA, false, 190, 250));
                }
                else
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "ANTECEDENTE PENAL", Constantes.CONST_MENSAJE_EXITO, false, 190, 250));
                    btnAnularAntecedente.Enabled = true;
                    btnRegistrarAntecedentesPenales.Visible = true;
                    btnNoAntecedentes.Visible = true;
                    hanpe_iAntecedentePenalId.Value = objBE.anpe_iAntecedentePenalId.ToString();
                    BotonesAntecedentesPenalesId.Visible = true;
                    tablaFileUpLoadFoto.Visible = true;
                    
                    updVinculacion.Update();
                }
            }
        }

        protected void btnAnularAntecedente_Click(object sender, EventArgs e)
        {
            Antecedente_PenalBL objBL = new Antecedente_PenalBL();
            SGAC.BE.MRE.RE_ANTECEDENTE_PENAL objBE = new BE.MRE.RE_ANTECEDENTE_PENAL();


            long lngAntecedentePenalId = Convert.ToInt64(hanpe_iAntecedentePenalId.Value);
            short intOficinaConsular = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            if (lngAntecedentePenalId == 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Antecedentes Penales", "No existe registro de Antecedentes Penales."));
                return;
            }

            objBE.anpe_iAntecedentePenalId = lngAntecedentePenalId;
            objBE.OficinaConsultar = intOficinaConsular;
            objBE.anpe_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            objBE.anpe_vIpModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

            objBL.Anular(objBE);

            if (objBL.isError)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ANTECEDENTE PENAL", Constantes.CONST_MENSAJE_OPERACION_FALLIDA, false, 190, 250));
            }
            else
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "ANTECEDENTE PENAL", Constantes.CONST_MENSAJE_ELIMINADO, false, 190, 250));
                limpiarDatosMSIAP();
                btnAnularAntecedente.Enabled = false;
                hanpe_iAntecedentePenalId.Value = "0";
                Session["AntecedentePenalDetalleId"] = "0";
                cargarDetalleAntecedentesPenales();
                BotonesAntecedentesPenalesId.Visible = false;
                tablaFileUpLoadFoto.Visible = false;
                btnRegistrarAntecedentesPenales.Visible = false;
                btnNoAntecedentes.Visible = false;
                updVinculacion.Update();
                updAntecedente.Update();
            }

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            if (txtCodAutoadhesivo.Enabled)
            {
                txtCodAutoadhesivo.Text = "";
            }
        }

        protected void imbBuscarPersona_Click(object sender, ImageClickEventArgs e)
        {
            BuscarPersona();
        }

        private void BuscarPersona()
        {
            try
            {
                Int16 sTipDoc = 0;
                string sNroDocumento = string.Empty;

                if (ValidarBusquedaPersona())
                {
                    sTipDoc = Convert.ToInt16(ddl_TipoDocParticipante.SelectedValue);
                    sNroDocumento = txtNroDocParticipante.Text;
                }

                PersonaConsultaBL _obj = new PersonaConsultaBL();
                DataTable dt = new DataTable();

                dt = _obj.ObtenerDatosPersona(sTipDoc, sNroDocumento);
                

                if (dt.Rows.Count == 0)
                {
                    LimpiarDatosParticipantes(true);
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "RENIEC", "La persona no esta registrada en el sistema.", false, 190, 250));
                }
                else {
                    hfPersona.Value = dt.Rows[0]["pers_iPersonaId"].ToString();
                    //----------------------------------------------------------
                    //Fecha: 25/02/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Asignar el ID de la residencia y la dirección
                    //----------------------------------------------------------
                    HFpere_iResidenciaId.Value = dt.Rows[0]["pere_iResidenciaId"].ToString();
                    hd_DirDir.Value = dt.Rows[0]["resi_vResidenciaDireccion"].ToString();
                    //----------------------------------------------------------
                    LlenarDatosDeBusqueda(dt);
                    BloquearNombresBusqueda();
                }                                
            }

            catch (Exception ex)
            {
                #region Registro Incidencia
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_vValoresTabla = "ACTO GENERAL",
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
            
        }

        private void LlenarDatosDeBusqueda(DataTable dt)
        {
            txtApePatParticipante.Text = dt.Rows[0]["pers_vApellidoPaterno"].ToString();
            txtApeMatParticipante.Text = dt.Rows[0]["pers_vApellidoMaterno"].ToString();
            txtApeCasadaParticipante.Text = dt.Rows[0]["pers_vApellidoCasada"].ToString();
            hApeCasadaParticipante.Value = dt.Rows[0]["pers_vApellidoCasada"].ToString();
            txtNomParticipante.Text = dt.Rows[0]["pers_vNombres"].ToString();
            CmbEstCiv.SelectedValue = dt.Rows[0]["pers_sEstadoCivilId"].ToString();
            CmbGenero.SelectedValue = dt.Rows[0]["pers_sGeneroId"].ToString();
            CmbGradInst.SelectedValue = dt.Rows[0]["pers_sGradoInstruccionId"].ToString();
            txtAnio.Text = dt.Rows[0]["pers_sAnioEstudio"].ToString();
            ddlCompleto.SelectedValue = dt.Rows[0]["pers_cEstudioCompleto"].ToString();
            txtEstatura.Text = dt.Rows[0]["pers_vEstatura"].ToString();


            TxtDirDir.Text = dt.Rows[0]["resi_vResidenciaDireccion"].ToString();           
            //--------------------------------------------------
            //Fecha: 25/02/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Asignar la dirección al control hidden.
            //--------------------------------------------------
            hd_DirDir.Value = dt.Rows[0]["resi_vResidenciaDireccion"].ToString();
            //--------------------------------------------------
            txtFechaNacimiento.Text = Comun.FormatearFecha(dt.Rows[0]["pers_dNacimientoFecha"].ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"]); 
            TxtTelfDir.Text = dt.Rows[0]["resi_vResidenciaTelefono"].ToString();
            TxtSenasParticulares.Text = dt.Rows[0]["pers_vSenasParticulares"].ToString();
            TxtEmail.Text = dt.Rows[0]["pers_vCorreoElectronico"].ToString();
            txtCodPostal.Text = dt.Rows[0]["resi_vCodigoPostal"].ToString();
            //ddl_DeptDomicilio.SelectedValue = dt.Rows[0]["iDptoContId"].ToString();
            //ddl_ProvDomicilio.SelectedValue = dt.Rows[0]["iProvPaisId"].ToString();
            //ddl_DistDomicilio.SelectedValue = dt.Rows[0]["iDistCiuId"].ToString();
            #region Datos de Domicilio

           
            string strUbigeo = dt.Rows[0]["resi_cResidenciaUbigeo"].ToString();
            

            if (strUbigeo.Length == 6)
            {

                HFDep.Value = strUbigeo.Substring(0, 2);
                HFProv.Value = strUbigeo.Substring(2, 2);
                HFdist.Value = strUbigeo.Substring(4, 2);

                if (strUbigeo != "000000")
                {
                    ddl_DeptDomicilio.SelectedValue = HFDep.Value;
                    comun_Part3.CargarUbigeo(Session, ddl_ProvDomicilio, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_DeptDomicilio.SelectedValue, string.Empty, true);
                    if (HFProv.Value != "00")
                    {
                        ddl_ProvDomicilio.SelectedValue = HFProv.Value;
                    }

                    comun_Part3.CargarUbigeo(Session, ddl_DistDomicilio, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddl_DeptDomicilio.SelectedValue, ddl_ProvDomicilio.SelectedValue, true);
                    if (HFdist.Value != "00")
                    {
                        ddl_DistDomicilio.SelectedValue = HFdist.Value;
                    }
                }
                else
                {
                    this.ddl_DeptDomicilio.SelectedIndex = 0;
                }
            }
            else
            {
                this.ddl_DeptDomicilio.SelectedIndex = 0;
            }

            #endregion
            


            #region Datos de nacimiento
            string strLNUbigeo = dt.Rows[0]["pers_cNacimientoLugar"].ToString(); 

            if (strLNUbigeo.Length == 6)
            {
                if (strLNUbigeo != "000000")
                {
                    HFLNDep.Value = strLNUbigeo.Substring(0, 2);
                    HFLNProv.Value = strLNUbigeo.Substring(2, 2);
                    HFLNdist.Value = strLNUbigeo.Substring(4, 2);

                    ddl_DeptNacimiento.SelectedValue = HFLNDep.Value;
                    comun_Part3.CargarUbigeo(Session, ddl_ProvNacimiento, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_DeptNacimiento.SelectedValue, string.Empty, true);
                    if (HFLNProv.Value != "00")
                    {
                        ddl_ProvNacimiento.SelectedValue = HFLNProv.Value;
                    }

                    comun_Part3.CargarUbigeo(Session, ddl_DistNacimiento, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddl_DeptNacimiento.SelectedValue, ddl_ProvNacimiento.SelectedValue, true);
                    if (HFLNdist.Value != "00")
                    {
                        ddl_DistNacimiento.SelectedValue = HFLNdist.Value;
                    }
                }
                else
                {
                    this.ddl_DeptNacimiento.SelectedIndex = 0;
                }
            }
            else
            {
                this.ddl_DeptNacimiento.SelectedIndex = 0;
            }
            #endregion
        }

        private bool ValidarBusquedaPersona()
        {
            bool respuesta = false;
            #region validar que se elija tipo de participante y tipo de documento
            if (ddl_TipoParticipante.SelectedIndex > 0)
            {
                ddl_TipoParticipante.Style.Add("border", "solid #888888 1px");
                if (ddl_TipoDocParticipante.SelectedIndex > 0)
                {
                    ddl_TipoDocParticipante.Style.Add("border", "solid #888888 1px");
                    if (txtNroDocParticipante.Text.Trim().Length > 0)
                    {
                        txtNroDocParticipante.Style.Add("border", "solid #888888 1px");
                        respuesta = true;
                    }
                    else {
                        
                        txtNroDocParticipante.Style.Add("border", "solid Red 1px");
                    }
                }
                else
                {
                    ddl_TipoDocParticipante.Style.Add("border", "solid Red 1px");
                }
            }
            else {
                ddl_TipoParticipante.Style.Add("border", "solid Red 1px");
            }
            #endregion
            return respuesta;
        }
        private void BloquearNombresBusqueda()
        {
            txtNomParticipante.Enabled = false;
            txtApeMatParticipante.Enabled = false;
            txtApePatParticipante.Enabled = false;

            if (txtApeCasadaParticipante.Text.Length == 0)
            {
                if (CmbGenero.SelectedItem.Text == Convert.ToString(Enumerador.enmGenero.FEMENINO))
                {
                    txtApeCasadaParticipante.Enabled = true;
                }
                else {
                    txtApeCasadaParticipante.Enabled = false;
                }
                
            }
            else {
                txtApeCasadaParticipante.Enabled = false;
            }
        }
        private string SelecionDiscapacidad()
        {
            string sResultado = string.Empty;

            if (rbSi_Discapacidad.Checked)
            {
                sResultado = "S";
            }
            if (rbNO_Discapacidad.Checked)
            {
                sResultado = "N";
            }
            return sResultado;
        }
        private string SelecionInterdicto()
        {
            string sResultado = string.Empty;

            if (rbSi_Interdicto.Checked)
            {
                sResultado = "S";
            }
            if (rbNO_Interdicto.Checked)
            {
                sResultado = "N";
            }
            return sResultado;
        }
        private string SelecionDonaOrganos()
        {
            string sResultado = string.Empty;

            if (rbSi_Donador.Checked)
            {
                sResultado = "S";
            }
            if (rbNO_Donador.Checked)
            {
                sResultado = "N";
            }
            return sResultado;
        }

        private void SetearDiscapacidad(string sDiscapacidad)
        {
            if (sDiscapacidad == "S")
            {
                rbSi_Discapacidad.Checked = true;
                rbNO_Discapacidad.Checked = false;
            }
            if (sDiscapacidad == "N")
            {
                rbSi_Discapacidad.Checked = false;
                rbNO_Discapacidad.Checked = true;
            }
        }

        private void SetearInterdicto(string sInterdicto)
        {
            if (sInterdicto == "S")
            {
                rbSi_Interdicto.Checked = true;
                rbNO_Interdicto.Checked = false;
            }
            if (sInterdicto == "N")
            {
                rbSi_Interdicto.Checked = false;
                rbNO_Interdicto.Checked = true;
            }
        }

        private void SetearDonaOrganos(string sDonaOrganos)
        {
            if (sDonaOrganos == "S")
            {
                rbSi_Donador.Checked = true;
                rbNO_Donador.Checked = false;
            }
            if (sDonaOrganos == "N")
            {
                rbSi_Donador.Checked = false;
                rbNO_Donador.Checked = true;
            }
        }

        protected void btnAgregarDocAdj_Click(object sender, EventArgs e)
        {
            long iFichaRegistralId = Convert.ToInt64(HFFichaRegistralId.Value);
            if (hDocAdjuntoEditar.Value == "0")
            {
                // NUEVO
                if (VerificarGrillaDOcAdjuntos(hDocAdjuntoDDL.Value))
                {
                    RegistrarDocAdjunto();
                }
            }
            else { 
                //EDITA
                if (VerificarGrillaDOcAdjuntos(hDocAdjuntoDDL.Value))
                {
                    ActualizarDocAdjunto();                
                }
            }
            limpiarDatosDocAdjuntos();
            ListarDocumentosAdjuntosReniec(iFichaRegistralId);

            if (hDocAdjuntoEditar.Value == "0")
            {
                Comun.EjecutarScript(this, "abrirPopupDocAdjuntos();");
            }
        }
        private void RegistrarDocAdjunto()
        {
            try
            {
                FichaRegistralParticipanteBL _obj = new FichaRegistralParticipanteBL();
                long iFichaRegistralId = Convert.ToInt64(HFFichaRegistralId.Value);
                Int16 Usuario = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                string vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                string Resultado = _obj.AgregarDocumentoAdjuntoFicha(iFichaRegistralId, Convert.ToInt16(ddlDocAdjunto.SelectedValue), txtNumeroDocAdj.Text, Usuario, vIPCreacion);

                if (Resultado != "OK")
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "RENIEC - DOC ADJUNTOS", Constantes.CONST_MENSAJE_OPERACION_FALLIDA, false, 190, 250));
                }
                else
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "RENIEC - DOC ADJUNTOS", Constantes.CONST_MENSAJE_EXITO, false, 190, 250));
                }
            }
            catch (Exception ex)
            {
                #region Registro Incidencia
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_vValoresTabla = "DOCUMENTO ADJUNTO",
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
        }
        private void ActualizarDocAdjunto()
        {
            try
            {
                FichaRegistralParticipanteBL _obj = new FichaRegistralParticipanteBL();
                long iDocAdjunto = Convert.ToInt64(hDocAdjuntoEditar.Value);
                Int16 Usuario = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                string vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                string Resultado = _obj.ActualizarDocumentoAdjuntoFicha(iDocAdjunto, Convert.ToInt16(ddlDocAdjunto.SelectedValue), txtNumeroDocAdj.Text, "A", Usuario, vIPCreacion);

                if (Resultado != "OK")
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "RENIEC - DOC ADJUNTOS", Constantes.CONST_MENSAJE_OPERACION_FALLIDA, false, 190, 250));
                }
                else
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "RENIEC - DOC ADJUNTOS", Constantes.CONST_MENSAJE_EXITO, false, 190, 250));
                }
            }
            catch (Exception ex)
            {
                #region Registro Incidencia
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_vValoresTabla = "DOCUMENTO ADJUNTO",
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
        }

        private void AnularDocAdjuntoFicha(long ID)
        {
            try
            {
                FichaRegistralParticipanteBL _obj = new FichaRegistralParticipanteBL();
                
                Int16 Usuario = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                string vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                string Resultado = _obj.ActualizarDocumentoAdjuntoFicha(ID, 0, "", "E", Usuario, vIPCreacion);

                if (Resultado != "OK")
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "RENIEC - DOC ADJUNTOS", Constantes.CONST_MENSAJE_OPERACION_FALLIDA, false, 190, 250));
                }
                else
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "RENIEC - DOC ADJUNTOS", Constantes.CONST_MENSAJE_ELIMINADO, false, 190, 250));
                }
            }
            catch (Exception ex)
            {
                #region Registro Incidencia
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_vValoresTabla = "DOCUMENTO ADJUNTO",
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
        }
        private void ListarDocumentosAdjuntosReniec(long iFichaRegistralId)
        { 
            FichaRegistralParticipanteBL _obj = new FichaRegistralParticipanteBL();
            
            DataTable _dt = new DataTable();
            _dt = _obj.ConsultarDocAdjuntos(iFichaRegistralId);
            if (_dt.Rows.Count > 0)
            {
                lblDocAdjuntos.Visible = true;
            }
            else {
                lblDocAdjuntos.Visible = false;
            }
            grvDocAdjuntosReniec.DataSource = _dt;
            grvDocAdjuntosReniec.DataBind();
            updFicha.Update();
        }

        protected void grvDocAdjuntosReniec_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long strID = 0;

            if (e.CommandName == "Anular")
            {
                GridViewRow gvrModificar;
                gvrModificar = (GridViewRow)((TableCell)((ImageButton)e.CommandSource).Parent).Parent;

                Label iFichaRegistralDocumentoID = (Label)gvrModificar.FindControl("lbl_iFichaRegistralDocumentoID");

                strID = Convert.ToInt64(iFichaRegistralDocumentoID.Text);
                AnularDocAdjuntoFicha(strID);
                long iFichaRegistralId = Convert.ToInt64(HFFichaRegistralId.Value);
                ListarDocumentosAdjuntosReniec(iFichaRegistralId);
            }

            if (e.CommandName == "Editar")
            {
                Comun.EjecutarScript(this, "abrirPopupDocAdjuntos();");

                GridViewRow gvrModificar;
                gvrModificar = (GridViewRow)((TableCell)((ImageButton)e.CommandSource).Parent).Parent;

                Label iFichaRegistralDocumentoID = (Label)gvrModificar.FindControl("lbl_iFichaRegistralDocumentoID");
                Label lblDOCADJUNTO = (Label)gvrModificar.FindControl("lblDOCADJUNTO");
                Label lblNUMERODOCADJUNTO = (Label)gvrModificar.FindControl("lblNUMERODOCADJUNTO");

                strID = Convert.ToInt64(iFichaRegistralDocumentoID.Text);

                hDocAdjuntoEditar.Value = strID.ToString();
                hDocAdjuntoDDL.Value = lblDOCADJUNTO.Text;
                ddlDocAdjunto.SelectedValue = lblDOCADJUNTO.Text;
                txtNumeroDocAdj.Text = lblNUMERODOCADJUNTO.Text;
            }
        }
        private void limpiarDatosDocAdjuntos()
        {
            ddlDocAdjunto.SelectedValue = "0";
            txtNumeroDocAdj.Text = "";
        }

        private bool VerificarGrillaDOcAdjuntos(string tipoDocActual)
        {

            foreach (GridViewRow row in grvDocAdjuntosReniec.Rows)
            {
                Label lblDOCADJUNTO = (Label)row.FindControl("lblDOCADJUNTO");
                
                if (lblDOCADJUNTO.Text == ddlDocAdjunto.SelectedValue)
                {
                    if (tipoDocActual != lblDOCADJUNTO.Text)
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "RENIEC - DOC ADJUNTOS", "YA SE HA INGRESADO ESE TIPO DE DOCUMENTO", false, 190, 250));
                        return false;
                    }
                    
                }
            }
            return true;
        }

        protected void txtNroDocParticipante_TextChanged(object sender, EventArgs e)
        {
            if (txtNroDocParticipante.Text.Length >= 8)
            {
                BuscarPersona();
            }
        }



    

        protected void GridViewParticipante_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;


            ImageButton btnEditar = e.Row.FindControl("btnEditar") as ImageButton;

            ImageButton btnAnular = e.Row.FindControl("btnAnular") as ImageButton;


            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                ImageButton[] arrImageButtons = { btnEditar, btnAnular };
                Comun.ModoLectura(ref arrImageButtons);
            }
        }

        protected void grvDocAdjuntosReniec_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;


            ImageButton btnEditar = e.Row.FindControl("btnEditar") as ImageButton;

            ImageButton btnAnular = e.Row.FindControl("btnAnular") as ImageButton;


            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                ImageButton[] arrImageButtons = { btnEditar, btnAnular };
                Comun.ModoLectura(ref arrImageButtons);
            }
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


        private void CargarTipoPagoNormaTarifario()
        {

            int IntTotalCount = 0;
            int IntTotalPages = 0;

            string strTarifaLetra = txtIdTarifa.Text.Trim().ToUpper();

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

            //if (Session["iPersonaId" + HFGUID.Value] != null && Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value] != null)
            //{
            if (ViewState["iPersonaId"] != null && Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value] != null)
            {
                ActuacionConsultaBL objBL = new ActuacionConsultaBL();
                DataTable dtActuacionConsulta = new DataTable();


                long intPersonaId = Convert.ToInt64(ViewState["iPersonaId"].ToString());
                long intActuacionId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value].ToString());

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

       
        private bool validarDatosAntecedentesPenalesDetalle()
        {
            string Script = string.Empty;

            if (Session["AntecedentePenalDetalleId"].ToString().Length == 0)
            { Session["AntecedentePenalDetalleId"] = "0"; }
            Int64 intAntecedentePenalDetalleId = Convert.ToInt64(Session["AntecedentePenalDetalleId"].ToString());
            if (intAntecedentePenalDetalleId > 0)
            {
                Script = "CambiarTexto_btnAgregarAntecedentePenal();";
            }

            if (txtOrganoJurisdiccionalMSIAP.Text.Trim().Length == 0)
            {
                Script += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Antecedentes Penales", "Digite el Organo Jurisdiccional.");

                Comun.EjecutarScript(Page, Script);
                return false;
            }

            if (txtNumeroExpedienteMSIAP.Text.Trim().Length == 0)
            {
                Script +=  Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Antecedentes Penales", "Digite el número de expediente.");
                Comun.EjecutarScript(Page, Script);
                return false;
            }
            if (txtFechaSentenciaMSIAP.Text.Trim().Length == 0)
            {
                Script +=Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Antecedentes Penales", "Seleccione la fecha de sentencia.");
                Comun.EjecutarScript(Page, Script);
                return false;
            }
            if (txtDelitoMSIAP.Text.Trim().Length == 0)
            {
                Script  += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Antecedentes Penales", "Digite el delito.");
                Comun.EjecutarScript(Page, Script);
                return false;
            }
            //------------------------------------------------------
            //Autor: Miguel Márquez
            //Fecha: 19/04/2022
            //Motivo: Según requerimiento será opcional.
            //------------------------------------------------------
            //------------------------------------------------------
            //if (txtDuracionAniosMSIAP.Text.Trim().Length == 0)
            //{
            //    Script += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Antecedentes Penales", "Digite la cantidad de años.");
            //    Comun.EjecutarScript(Page, Script);
            //    return false;
            //}
            //if (txtDuracionMesesMSIAP.Text.Trim().Length == 0)
            //{
            //    Script += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Antecedentes Penales", "Digite la cantidad de meses.");
            //    Comun.EjecutarScript(Page, Script);
            //    return false;
            //}
            //if (txtDuracionDiasMSIAP.Text.Trim().Length == 0)
            //{
            //    Script += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Antecedentes Penales", "Digite la cantidad de días.");
            //    Comun.EjecutarScript(Page, Script);
            //    return false;
            //}
            //else {
            //    if (txtDuracionAniosMSIAP.Text == "0")
            //    {
            //        if (txtDuracionMesesMSIAP.Text == "0")
            //        {
            //            if (txtDuracionDiasMSIAP.Text == "0")
            //            {
            //                Script += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Antecedentes Penales", "La pena debe ser mínimo 1 dia.");
            //                Comun.EjecutarScript(Page, Script);
            //                return false;
            //            }
            //        }
            //    }
            //}
            //if (txtFechaVencimientoMSIAP.Text.Trim().Length == 0)
            //{
            //    Script += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Antecedentes Penales", "Seleccione la fecha de vencimiento.");
            //    Comun.EjecutarScript(Page, Script);
            //    return false;
            //}
            if (txtTipoPena.Text.Trim().Length == 0)
            {
                Script += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Antecedentes Penales", "Digite el tipo de pena.");
                Comun.EjecutarScript(Page, Script);
                return false;
            }
            bool bBuscarExpediente = false;
            if (intAntecedentePenalDetalleId > 0)
            {
                if (HF_NroExpediente.Value.Trim().ToUpper() != txtNumeroExpedienteMSIAP.Text.Trim().ToUpper())
                {
                    bBuscarExpediente = true;
                }
            }
            else
            {
                bBuscarExpediente = true;
            }
            if (bBuscarExpediente == true)
            {
                Int64 intAntecedentePenalId = Convert.ToInt64(hanpe_iAntecedentePenalId.Value);
                int IntTotalCount = 0;
                int IntTotalPages = 0;
                Antecedente_Penal_DetalleBL objAntecedentePenalDetalleBL = new Antecedente_Penal_DetalleBL();

                DataTable dtAntecedentePenalDetalle = new DataTable();
                dtAntecedentePenalDetalle = objAntecedentePenalDetalleBL.Consultar(0, intAntecedentePenalId, txtNumeroExpedienteMSIAP.Text.Trim(), "A",
                                                            1, 1, "N", ref IntTotalPages, ref IntTotalCount);
                if (dtAntecedentePenalDetalle.Rows.Count > 0)
                {
                    Script += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Antecedentes Penales", "Ya se encuentra registrado el número de expediente.");
                    Comun.EjecutarScript(Page, Script);
                    return false;
                }
                dtAntecedentePenalDetalle.Dispose();
            }

            return true;
        }

        protected void btnAgregarAntecedentePenal_Click(object sender, EventArgs e)
        {
            string StrScript = string.Empty;
            bool bEsCorrecto = false;

            if (Session["AntecedentePenalDetalleId"].ToString().Length == 0)
            { Session["AntecedentePenalDetalleId"] = "0"; }
            Int64 intAntecedentePenalDetalleId = Convert.ToInt64(Session["AntecedentePenalDetalleId"].ToString());

            if (validarDatosAntecedentesPenalesDetalle())
            {

                Int64 intAntecedentePenalId = Convert.ToInt64(hanpe_iAntecedentePenalId.Value);

                BE.MRE.RE_ANTECEDENTE_PENAL_DETALLE AntecedenteDetalleBE = new BE.MRE.RE_ANTECEDENTE_PENAL_DETALLE();
                BE.MRE.RE_ANTECEDENTE_PENAL_DETALLE BE = new BE.MRE.RE_ANTECEDENTE_PENAL_DETALLE();
                Antecedente_Penal_DetalleBL objAntecedenteDetalleBL = new Antecedente_Penal_DetalleBL();

                AntecedenteDetalleBE.apde_iAntecedentePenalDetalleId = intAntecedentePenalDetalleId;
                AntecedenteDetalleBE.apde_iAntecedentePenalId = intAntecedentePenalId;
                AntecedenteDetalleBE.apde_vOrganoJurisdiccional = txtOrganoJurisdiccionalMSIAP.Text.Trim().ToUpper();
                AntecedenteDetalleBE.apde_vNumeroExpediente = txtNumeroExpedienteMSIAP.Text.Trim().ToUpper();
                AntecedenteDetalleBE.apde_dFechaSentencia = Comun.FormatearFecha(txtFechaSentenciaMSIAP.Text);
                AntecedenteDetalleBE.apde_vDelito = txtDelitoMSIAP.Text.Trim().ToUpper();
                AntecedenteDetalleBE.apde_cDuracion_Anios = txtDuracionAniosMSIAP.Text.Trim();
                AntecedenteDetalleBE.apde_cDuracion_Meses = txtDuracionMesesMSIAP.Text.Trim();
                AntecedenteDetalleBE.apde_cDuracion_Dias = txtDuracionDiasMSIAP.Text.Trim();

                if (txtFechaVencimientoMSIAP.Text.Length > 0)
                {
                    AntecedenteDetalleBE.apde_dFechaVencimiento = Comun.FormatearFecha(txtFechaVencimientoMSIAP.Text);
                }
                
                AntecedenteDetalleBE.apde_vTipoPena = txtTipoPena.Text.Trim().ToUpper();
                AntecedenteDetalleBE.apde_cEstado = "A";
                AntecedenteDetalleBE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                if (intAntecedentePenalDetalleId == 0)
                {
                    AntecedenteDetalleBE.apde_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    AntecedenteDetalleBE.apde_vIpCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    BE = objAntecedenteDetalleBL.insertar(AntecedenteDetalleBE);
                    if (BE.Error)
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Antecedentes Penales", Constantes.CONST_MENSAJE_OPERACION_FALLIDA, false, 190, 250);
                        bEsCorrecto = false;
                    }
                    else
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Antecedentes Penales", Constantes.CONST_MENSAJE_EXITO, false, 190, 250);
                        bEsCorrecto = true;
                    }
                }
                else
                {
                    AntecedenteDetalleBE.apde_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    AntecedenteDetalleBE.apde_vIpModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    objAntecedenteDetalleBL.actualizar(AntecedenteDetalleBE);

                    if (objAntecedenteDetalleBL.isError == true)
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Antecedentes Penales", Constantes.CONST_MENSAJE_OPERACION_FALLIDA, false, 190, 250);
                        bEsCorrecto = false;
                    }
                    else
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Antecedentes Penales", Constantes.CONST_MENSAJE_EXITO, false, 190, 250);
                        bEsCorrecto = true;
                    }
                }
                //----------------------------------------------
                cargarDetalleAntecedentesPenales();

                if (bEsCorrecto)
                {
                    Comun.EjecutarScript(this, StrScript + ";abrirPopupAntecedentesPenales()");
                    Session["AntecedentePenalDetalleId"] = "0";
                }
                
            }
        }
        
        protected void gdvAntecedentesPenales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int64 intAntecedentePenalDetalleId = 0;
            if (e.CommandName == "Editar")
            {
                GridViewRow gvrModificar;
                gvrModificar = (GridViewRow)((TableCell)((ImageButton)e.CommandSource).Parent).Parent;
                Label lblAntecedentePenalDetalleId = (Label)gvrModificar.FindControl("lblAntecedentePenalDetalleId");
                intAntecedentePenalDetalleId = Convert.ToInt64(lblAntecedentePenalDetalleId.Text);
                Session["AntecedentePenalDetalleId"] = intAntecedentePenalDetalleId.ToString();
                LeerAntecedentePenalDetalle();
                Comun.EjecutarScript(this, "CambiarTexto_btnAgregarAntecedentePenal();abrirEditarPopupAntecedentesPenales();");
                
            }
            if (e.CommandName == "Anular")
            {
                
                GridViewRow gvrModificar;
                gvrModificar = (GridViewRow)((TableCell)((ImageButton)e.CommandSource).Parent).Parent;
                Label lblAntecedentePenalDetalleId = (Label)gvrModificar.FindControl("lblAntecedentePenalDetalleId");
                intAntecedentePenalDetalleId = Convert.ToInt64(lblAntecedentePenalDetalleId.Text);
                Session["AntecedentePenalDetalleId"] = intAntecedentePenalDetalleId.ToString();
                AnularAntecedentePenalDetalle();
            }
        }
       
        protected void CtrlPaginadorAntecedentesPenales_Click(object sender, EventArgs e)
        {
            cargarDetalleAntecedentesPenales();
        }
    
        private void cargarDetalleAntecedentesPenales()
        {
            Int64 intAntecedentePenalId = Convert.ToInt64(hanpe_iAntecedentePenalId.Value);
            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad =  Constantes.CONST_CANT_REGISTRO;
            int PaginaActual = CtrlPaginadorAntecedentesPenales.PaginaActual;

            if (intAntecedentePenalId > 0)
            {
                BotonesAntecedentesPenalesId.Visible = true;
                tablaFileUpLoadFoto.Visible = true;
                

                Antecedente_Penal_DetalleBL objAntecedentePenalDetalleBL = new Antecedente_Penal_DetalleBL();

                DataTable dtAntecedentePenalDetalle = new DataTable();

                dtAntecedentePenalDetalle = objAntecedentePenalDetalleBL.Consultar(0, intAntecedentePenalId, "", "A",
                                                                        intPaginaCantidad, PaginaActual, "S", ref IntTotalPages, ref IntTotalCount);

                if (dtAntecedentePenalDetalle.Rows.Count > 0)
                {
                    lblTituloAntecedentesPenalesDetalle.Visible = true;
                    gdvAntecedentesPenales.DataSource = dtAntecedentePenalDetalle;
                    gdvAntecedentesPenales.DataBind();

                    CtrlPaginadorAntecedentesPenales.TotalResgistros = IntTotalCount;
                    CtrlPaginadorAntecedentesPenales.TotalPaginas = IntTotalPages;

                    CtrlPaginadorAntecedentesPenales.Visible = false;

                    if (CtrlPaginadorAntecedentesPenales.TotalResgistros > intPaginaCantidad)
                    {
                        CtrlPaginadorAntecedentesPenales.Visible = true;
                    }
                }
                else
                {
                    Session["AntecedentePenalDetalleId"] = "0";
                    lblTituloAntecedentesPenalesDetalle.Visible = false;
                    gdvAntecedentesPenales.DataSource = null;
                    gdvAntecedentesPenales.DataBind();
                    CtrlPaginadorAntecedentesPenales.Visible = false;
                }
            }
            else
            {
                lblTituloAntecedentesPenalesDetalle.Visible = false;
                gdvAntecedentesPenales.DataSource = null;
                gdvAntecedentesPenales.DataBind();
                CtrlPaginadorAntecedentesPenales.Visible = false;
                BotonesAntecedentesPenalesId.Visible = false;
                tablaFileUpLoadFoto.Visible = false;
                
            }
            //updAntecedentesPenales.Update();
            updAntecedente.Update();
        }
       
        private void LeerAntecedentePenalDetalle()
        {
            Int64 intAntecedentePenalDetalleId = Convert.ToInt64(Session["AntecedentePenalDetalleId"].ToString());
            int IntTotalCount = 0;
            int IntTotalPages = 0;

            Antecedente_Penal_DetalleBL objAntecedentePenalDetalleBL = new Antecedente_Penal_DetalleBL();

            DataTable dtAntecedentePenalDetalle = new DataTable();

            dtAntecedentePenalDetalle = objAntecedentePenalDetalleBL.Consultar(intAntecedentePenalDetalleId, 0, "", "A",
                                                                1, 1, "S", ref IntTotalPages, ref IntTotalCount);
            HF_NroExpediente.Value = "";
            if (dtAntecedentePenalDetalle.Rows.Count > 0)
            {
                txtOrganoJurisdiccionalMSIAP.Text = dtAntecedentePenalDetalle.Rows[0]["apde_vOrganoJurisdiccional"].ToString();
                txtNumeroExpedienteMSIAP.Text = dtAntecedentePenalDetalle.Rows[0]["apde_vNumeroExpediente"].ToString();
                HF_NroExpediente.Value = dtAntecedentePenalDetalle.Rows[0]["apde_vNumeroExpediente"].ToString();
                if (dtAntecedentePenalDetalle.Rows[0]["apde_dFechaSentencia"].ToString().Length == 0)
                {
                    txtFechaSentenciaMSIAP.Text = "";
                }
                else
                {
                    txtFechaSentenciaMSIAP.set_Value = Comun.FormatearFecha(dtAntecedentePenalDetalle.Rows[0]["apde_dFechaSentencia"].ToString());
                }
                txtDelitoMSIAP.Text = dtAntecedentePenalDetalle.Rows[0]["apde_vDelito"].ToString();
                txtDuracionAniosMSIAP.Text = dtAntecedentePenalDetalle.Rows[0]["apde_cDuracion_Anios"].ToString();
                txtDuracionMesesMSIAP.Text = dtAntecedentePenalDetalle.Rows[0]["apde_cDuracion_Meses"].ToString();
                txtDuracionDiasMSIAP.Text = dtAntecedentePenalDetalle.Rows[0]["apde_cDuracion_Dias"].ToString();
                if (dtAntecedentePenalDetalle.Rows[0]["apde_dFechaVencimiento"].ToString().Length == 0)
                {
                    txtFechaVencimientoMSIAP.Text = "";
                }
                else
                {
                    txtFechaVencimientoMSIAP.set_Value = Comun.FormatearFecha(dtAntecedentePenalDetalle.Rows[0]["apde_dFechaVencimiento"].ToString());
                }
                txtTipoPena.Text = dtAntecedentePenalDetalle.Rows[0]["apde_vTipoPena"].ToString();
                updAntecedentesPenales.Update();
            }
        }

        private void AnularAntecedentePenalDetalle()
        {
            string StrScript = string.Empty;

            Int64 intAntecedentePenalDetalleId = Convert.ToInt64(Session["AntecedentePenalDetalleId"].ToString());

            BE.MRE.RE_ANTECEDENTE_PENAL_DETALLE AntecedenteDetalleBE = new BE.MRE.RE_ANTECEDENTE_PENAL_DETALLE();
            BE.MRE.RE_ANTECEDENTE_PENAL_DETALLE BE = new BE.MRE.RE_ANTECEDENTE_PENAL_DETALLE();
            Antecedente_Penal_DetalleBL objAntecedenteDetalleBL = new Antecedente_Penal_DetalleBL();

            AntecedenteDetalleBE.apde_iAntecedentePenalDetalleId = intAntecedentePenalDetalleId;
            AntecedenteDetalleBE.apde_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            AntecedenteDetalleBE.apde_vIpModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
            AntecedenteDetalleBE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            objAntecedenteDetalleBL.anular(AntecedenteDetalleBE);

            if (objAntecedenteDetalleBL.isError == true)
            {
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Antecedentes Penales", Constantes.CONST_MENSAJE_OPERACION_FALLIDA, false, 190, 250);
                Comun.EjecutarScript(this, StrScript);  
            }
            else
            {
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Antecedentes Penales", Constantes.CONST_MENSAJE_EXITO, false, 190, 250);
                cargarDetalleAntecedentesPenales();
                Comun.EjecutarScript(this, StrScript + ";LimpiarAntecedentePenalDetalle()");
                Session["AntecedentePenalDetalleId"] = "0";
            }
                                  
        }

        protected void btnImpresionCertificadoAntecedentesPenales_Click(object sender, EventArgs e)
        {
            string strRPT = "";
            string StrScript = string.Empty;
            Int64 intAntecedentePenalId = Convert.ToInt64(hanpe_iAntecedentePenalId.Value);
            string strIdioma = Session[Constantes.CONST_SESION_IDIOMA_TEXTO].ToString();
            string strRutaFoto = "";
            if (strIdioma.Trim().Length == 0)
            {
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Antecedentes Penales", "NO SE HA REGISTRADO EL IDIOMA DEL PAÍS", false, 190, 250);
                Comun.EjecutarScript(this, StrScript);
                return;
            }

            strRutaFoto = HF_RutaFoto.Value;
            
            Session["UploadFotoAntecedentePenal"] = null;
            if (intAntecedentePenalId > 0)
            {
                Antecedente_PenalBL objAntecedentePenalBL = new Antecedente_PenalBL();
                DataTable dtAntecedentePenal = new DataTable();
                Int16 intTipoDocumentoId = 1;

                //if (HFGUID.Value.Length > 0)
                //{
                //    intTipoDocumentoId = Convert.ToInt16(Session["iDocumentoTipoId" + HFGUID.Value].ToString());
                //}
                //else
                //{
                    intTipoDocumentoId = Convert.ToInt16(ViewState["iDocumentoTipoId"].ToString());
                //}


                dtAntecedentePenal = objAntecedentePenalBL.ReporteCertificadoConsular(intAntecedentePenalId, intTipoDocumentoId);

                if (dtAntecedentePenal.Rows.Count > 0)
                {
                    if (dtAntecedentePenal.Rows[0]["ID"].ToString().Equals("0"))
                    {
                        //----------------------------------------------
                        // NO TIENE ANTECEDENTES PENALES
                        //----------------------------------------------
                        if (strIdioma.Equals("CASTELLANO"))
                        { strRPT = "CAST_NR"; }
                        else
                        { strRPT = "OTR_NR"; }
                    }
                    else
                    {
                        //----------------------------------------------
                        // SI TIENE ANTECEDENTES PENALES
                        //----------------------------------------------
                        if (strIdioma.Equals("CASTELLANO"))
                        { strRPT = "CAST_SR"; }
                        else
                        { strRPT = "OTR_SR"; }
                    }
                    Session["dtDatos"] = dtAntecedentePenal;
                    Session["IdOficinaConsular_contabilidad"] = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    Comun.VerVistaPreviaCertificadoAntecedentePenal(Session, Page, strRPT, Enumerador.enmReporteContable.CERTIFICADO_ANTECEDENTE_PENAL, strRutaFoto);
                }
                else
                {
                    //----------------------------------------------
                    // NO EXISTE EL REGISTRO DE ANTECEDENTES PENALES
                    //----------------------------------------------
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Antecedentes Penales", "NO SE HA REGISTRADO EL ANTECEDENTE PENAL", false, 190, 250);
                    Comun.EjecutarScript(this, StrScript);
                    return;
                }
            }
        }

        protected void btn_ImprimirConformidad_Click(object sender, EventArgs e)
        {
            #region Datos de Recurrente

            string vNombreRecurrente = string.Empty;
            string vDocumento = string.Empty;

            //string strGUID = "";

            //if (HFGUID.Value.Length > 0)
            //{
            //    strGUID = HFGUID.Value;
            //}

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
                    else
                    {
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
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "CERTIFICADO DE ANTECEDENTES PENALES",
                                                "El archivo temporal de Acta de Conformidad (PDF) no se pudo eliminar." +
                                                "\n(" + ex.Message + ")");
                        Comun.EjecutarScript(Page, strScript);
                    }
                }
                //-------------------------------------------------------------------------
            }
            
            if (File.Exists(strRutaHtml))
            {
                try
                {
                    File.Delete(strRutaHtml);
                }
                catch (Exception ex)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "CERTIFICADO DE ANTECEDENTES PENALES",
                                            "El archivo temporal de Acta de Conformidad (HTML) no se pudo eliminar." +
                                            "\n(" + ex.Message + ")");
                    Comun.EjecutarScript(Page, strScript);
                }
            }
            #endregion
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
            
            sScript.Append("Certificado de Antecedentes Penales");            

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

        private Boolean CargarArchivo()
        {
            Boolean Resultado = false;
            String strScript = String.Empty;
            HF_RutaFoto.Value = "";
            try
            {
                String localfilepath = String.Empty;
                String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
                String uploadFileName = String.Empty;

                if (Session["UploadFotoAntecedentePenal"] != null)
                {                    
                    FileUploadFoto = (FileUpload)Session["UploadFotoAntecedentePenal"];
                    Session["UploadFotoAntecedentePenal"] = null;
                }

                if (FileUploadFoto.HasFile)
                {
                    string strLocalFilePath = FileUploadFoto.FileName;
                    string strFileName = Path.GetFileName(strLocalFilePath);
                    
                    String caracteres = ConfigurationManager.AppSettings["validarchars"].ToString();
                    Int32 SizeFile = strLocalFilePath.Length;

                    string[] caract = caracteres.Split(',');

                    foreach (string onjcaract in caract)
                    {
                        for (int i = 0; i < SizeFile; i++)
                        {
                            String var = String.Empty;
                            var = strLocalFilePath.Substring(i, 1);
                            if (var == onjcaract)
                            {
                                lblMensajeErrorUpLoadFoto.Text = "Error al Adjunto, el archivo contiene caracteres Invalidos";
                                return false;
                            }
                        }
                    }
                    //--------------------------------
                    String extension = System.IO.Path.GetExtension(strFileName);
                    int fileSizeInBytes = FileUploadFoto.PostedFile.ContentLength;
                    int fileSizeInKB = fileSizeInBytes / Constantes.CONST_TAMANIO_MAX_ADJUNTO_KB;
                    int s_Maximo = Constantes.CONST_TAMANIO_MAX_ADJUNTO_FOTO_KB;                    

                    if (fileSizeInKB <= s_Maximo)
                    {
                        if (extension.ToUpper().Equals(".JPG"))
                        {
                            #region AdjuntarFoto

                            String strMensaje = String.Empty;
                            if (Directory.Exists(uploadPath)) { }
                            else
                            {
                                try
                                {
                                    // Crear carpeta
                                    //strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Ruta del Archivo", uploadPath, false, 200, 400);
                                    //Comun.EjecutarScript(Page, strScript);
                                    Directory.CreateDirectory(uploadPath);
                                }
                                catch
                                {
                                    lblMensajeErrorUpLoadFoto.Text = "No se encuentra o no existe el directorio de Adjuntos.";
                                    return false;
                                }
                            }
                            //----------------------------
                            if (HFGUID.Value.Length > 0)
                            {
                                uploadFileName = Documento.GetUniqueUploadFileName(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]), uploadPath, ref strFileName);
                            }
                            else
                            {
                                uploadFileName = Documento.GetUniqueUploadFileName(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]), uploadPath, ref strFileName);
                            }
                            try
                            {
                                FileUploadFoto.SaveAs(uploadFileName);
                                HF_RutaFoto.Value = uploadFileName;
                                //strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTAR", System.Configuration.ConfigurationManager.AppSettings["UploadSucessMsje"], false, 200, 400);
                                //Comun.EjecutarScript(Page, strScript);
                                Resultado = true;
                            }
                            catch (Exception ex)
                            {
                                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTAR", ex.Message, false, 200, 400);
                                Comun.EjecutarScript(Page, strScript);
                                return false;
                            }
                            #endregion
                        }
                        else
                        {
                            lblMensajeErrorUpLoadFoto.Text = System.Configuration.ConfigurationManager.AppSettings["UploadInvalidFileMsje"].ToString();
                            Resultado = false;
                        }
                    }
                    else
                    {
                        lblMensajeErrorUpLoadFoto.Text = System.Configuration.ConfigurationManager.AppSettings["UploadInvalidSizeMsje"].ToString();
                        Resultado = false;
                    }
                }
                //updVinculacion.Update();
                return Resultado;
            }
            catch (Exception ex)
            {
                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTAR", ex.Message, false, 200, 400);
                Comun.EjecutarScript(Page, strScript);

                return false;
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

        private void EjecutarScript(string script)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPopUp" + DateTime.Now.Ticks.ToString(), script, true);
        }

        [System.Web.Services.WebMethod]
        public static void SetSession(string variable, string valor)
        {
            HttpContext.Current.Session[variable] = valor;
        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            ctrlToolBarFicha_btnPrintHandler();
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

        protected void Tramite_Click(object sender, EventArgs e)
        { 
            string codPersona = Request.QueryString["CodPer"].ToString();
            if (Request.QueryString["Juridica"] != null) // si es persona juridica
            {
                Response.Redirect("FrmTramite.aspx?CodPer=" + codPersona + "&Juridica=1", false);
            }
            else{
                string codTipoDocEncriptada = "";
                string codNroDocumentoEncriptada = "";

                if (Request.QueryString["CodTipoDoc"] != null && Request.QueryString["codNroDoc"] != null)
                {
                    codTipoDocEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString());
                    codNroDocumentoEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["codNroDoc"].ToString());
                }

                if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                {
                    Response.Redirect("FrmTramite.aspx?CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                }
                else
                {
                    Response.Redirect("FrmTramite.aspx?CodPer=" + codPersona, false);
                }
            }                        
        }
        //----------------------------------------------------------
        //Fecha: 25/02/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Validar si existe la dirección.
        //----------------------------------------------------------
        private bool validarExisteDireccion(string strDireccion)
        {
            bool bExisteDireccion = false;

            DataTable dt = new DataTable();
            PersonaResidenciaConsultaBL ConsultaResidenciasBL = new PersonaResidenciaConsultaBL();
            long iPersonaId = Convert.ToInt64(hfPersona.Value);
            dt = ConsultaResidenciasBL.Obtener(iPersonaId);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (strDireccion == dt.Rows[i]["vResidenciaDireccion"].ToString().Trim().ToUpper())
                {
                    bExisteDireccion = true;
                    break;
                }
            }
            return bExisteDireccion;
        }
        //------------------------------------------------------------------------
        //Fecha: 30/04/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Validar el genero en relación al tipo de participante.
        //------------------------------------------------------------------------

        private bool validarDatosParticipantes()
        {
                     
            if (CmbGenero.SelectedIndex > 0)
            {
                if (ddl_TipoParticipante.SelectedValue == Convert.ToString((int)Enumerador.enmFichaTipoParticipanteMenor.MADRE)
                    || ddl_TipoParticipante.SelectedValue == Convert.ToString((int)Enumerador.enmFichaTipoParticipanteMayor.MADRE))
                {
                    if (CmbGenero.SelectedItem.Text == Convert.ToString(Enumerador.enmGenero.MASCULINO))
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Participante", "El tipo de participante no corresponde al genero Masculino"));
                        CmbGenero.Focus();

                        return true;
                    }
                }
                if (ddl_TipoParticipante.SelectedValue == Convert.ToString((int)Enumerador.enmFichaTipoParticipanteMenor.PADRE)
                    || ddl_TipoParticipante.SelectedValue == Convert.ToString((int)Enumerador.enmFichaTipoParticipanteMayor.PADRE))
                {
                    if (CmbGenero.SelectedItem.Text == Convert.ToString(Enumerador.enmGenero.FEMENINO))
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Participante", "El tipo de participante no corresponde al genero Femenino"));
                        CmbGenero.Focus();

                        return true;
                    }
                }
            }

            return false;
        }
        protected void btnRegistroSUNARP_Click(object sender, EventArgs e)
        {
            //--------------------------------------------------------------------------
            //Fecha: 07/10/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Se redirecciona al formulario: FrmSolicitudInscripcionSUNARP
            //--------------------------------------------------------------------------
            string codPersona = Request.QueryString["CodPer"].ToString();
            if (Request.QueryString["Juridica"] != null) // si es persona juridica
            {
                Response.Redirect("FrmSolicitudInscripcionSUNARP.aspx?CodPer=" + codPersona + "&Juridica=1", false);
            }
            else
            {
                string codTipoDocEncriptada = "";
                string codNroDocumentoEncriptada = "";

                if (Request.QueryString["CodTipoDoc"] != null && Request.QueryString["codNroDoc"] != null)
                {
                    codTipoDocEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString());
                    codNroDocumentoEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["codNroDoc"].ToString());
                }
                if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                {
                    Response.Redirect("FrmSolicitudInscripcionSUNARP.aspx?CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                }
                else
                {
                    Response.Redirect("FrmSolicitudInscripcionSUNARP.aspx?CodPer=" + codPersona, false);
                }
            }
        }

        protected void ImprimirConstancia(object sender, EventArgs e)
        {
            try
            {
                //ViewState.Add("AccionDocumento", "Nuevo");

                string codPersona = Util.DesEncriptar(Request.QueryString["CodPer"].ToString());

                PersonaConsultaBL objPersonaBL = new PersonaConsultaBL();
                DataSet dsRune = new DataSet();

                string nombrereporte = string.Empty;
                string ruta = string.Empty;

                BE.RE_PERSONA objBE = new BE.RE_PERSONA();

                objBE.pers_iPersonaId = Convert.ToInt64(codPersona);

                dsRune = objPersonaBL.Persona_Imprimir_Rune(objBE);

                if (dsRune.Tables[1].Rows.Count > 0)
                {
                    if (dsRune.Tables[1].Rows[0]["pers_sNacionalidadId"].ToString() == Convert.ToString((Int32)Enumerador.enmNacionalidad.EXTRANJERA))
                    {
                        string StrScript = string.Empty;
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "RUNE", "Para imprimir la Constancia de Inscripción tiene que ser PERUANO", false, 190, 250);
                        Comun.EjecutarScript(Page, StrScript);
                        return;
                    }
                }

                if (ddlIdiomas.SelectedItem.Text.ToUpper() == "CASTELLANO")
                {
                    Session["strNombreArchivo"] = "crConstanciaInscripcion_Idioma_CAST.rdlc";
                }
                else {
                    Session["strNombreArchivo"] = "crConstanciaInscripcion_Idioma_OTR.rdlc";
                }
                Session["DtDatos"] = dsRune;
                Session["REGISTRO_RPT"] = Enumerador.enmRegistroReporte.FILIACION;

                //-------------------------------------------------------------------------
                //Autor: Miguel Márquez Beltrán
                //Fecha: 30/09/2016
                //Objetivo: Crear una sesion para permitir actualizar el formato del RUNE
                //-------------------------------------------------------------------------
                Session["printRUNE"] = "1";
                //-------------------------------------------------------------------------
                Session["MontoLocal"] = txtTotalML.Text;
                Session["SolesConsulares"] = txtTotalSC.Text;

                string strUrl = "../Registro/FrmReporteRune.aspx";
                string strScript = "window.open('" + strUrl + "', 'popup_window', 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=yes,width=750,height=800,left=150,top=10');";

                Comun.EjecutarScript(Page, strScript);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //--------------------------------------------
        //Fecha: 15/03/2022
        //Autor: Miguel Márquez Beltrán
        //Motivo: Obtener el nro. de tipo documento 
        //          para los formatos.
        //--------------------------------------------
        private static string obtenerTipodocumento(string strDescTipodocumento)
        {
            string strTipoDocumento = "";
            
            if (strDescTipodocumento == "DNI") { strTipoDocumento = "1"; }
            if (strDescTipodocumento == "CI") { strTipoDocumento = "3"; }
            if (strDescTipodocumento == "CE") { strTipoDocumento = "4"; }
            if (strDescTipodocumento == "PASAPORTE P" || strDescTipodocumento == "PASAP EXTR") { strTipoDocumento = "5"; }

            return strTipoDocumento;
        }

        //-------------------------------------------------------------------------
    }
}
