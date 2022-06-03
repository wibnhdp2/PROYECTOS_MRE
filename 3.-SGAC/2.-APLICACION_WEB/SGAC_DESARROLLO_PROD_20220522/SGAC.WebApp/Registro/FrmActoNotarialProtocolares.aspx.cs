using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Data;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Net;
using System.Threading;

using SGAC.Almacen.BL;
using SGAC.BE.MRE;
using SGAC.Configuracion.Sistema.BL;
using SGAC.Registro.Actuacion.BL;
using SGAC.Configuracion.Maestro.BL;
using SGAC.Registro.Persona.BL;
using SGAC.Accesorios;
using SGAC.WebApp.Accesorios;
using System.Configuration;
using SGAC.Controlador;
using SGAC.BE.MRE.Custom;
using SGAC.Reportes.BL;
using System.Drawing;
using System.Web.Services;
using Microsoft.Security.Application;
using SUNARP.Registro.Inscripcion.BL;

namespace SGAC.WebApp.Registro
{
    public partial class FrmActoNotarialProtocolares : MyBasePage
    {
        protected static List<string> json = new List<string>();
        public static string strClaveGUID { get; set; }

        const string CON_CentralAnterior_1 = "<p style='text-align:left;'><b>SEÑOR(A) CÓNSUL:</b></p>";
        const string CON_CentralAnterior_2 = "SÍRVASE USTED EXTENDER EN SU REGISTRO DE ESCRITURAS PÚBLICAS UNA EN QUE CONSTE ";
        const string CON_CentralAnterior_3 = " QUE OTORGA ";
        const string CON_CentralAnterior_4 = " QUE OTORGAN ";
        const string CON_Oficina_Registral_Lima = "2";
        #region Clases
        //[Serializable]
        //class participantes
        //{
        //    public string vEmpresa { get; set; }
        //    public long pers_iPersonaId { get; set; }
        //    public string vTipoParticipanteId { get; set; }
        //    public string vDescTipDoc { get; set; }
        //    public string vDescLargaTipDoc { get; set; }
        //    public string vNroDocumento { get; set; }
        //    public Int16 pers_sNacionalidadId { get; set; }
        //    public string vNacionalidad { get; set; }
        //    public Int16 pers_sEstadoCivilId { get; set; }
        //    public string vEstadoCivil { get; set; }
        //    public string vDireccion { get; set; }
        //    public string cResidenciaUbigeo { get; set; }
        //    public string DptoCont { get; set; }
        //    public string ProvPais { get; set; }
        //    public string DistCiu { get; set; }
        //    public string vOcupacion { get; set; }
        //    public string vProfesion { get; set; }
        //    public string vTipoDocumento { get; set; }
        //    public Int16 iGenero { get; set; }
        //    public bool pers_bIncapacidadFlag { get; set; }
        //    public string pers_vDescripcionIncapacidad { get; set; }
        //    public long anpa_iReferenciaParticipanteId { get; set; }
        //    public long anpa_iActoNotarialParticpanteId { get; set; }
        //    public Int16 anpa_sTipoParticipanteId { get; set; }
        //    public Int16 pers_sIdiomaNatalId { get; set; }
        //    public string vIdioma { get; set; }
        //    public string vGentilicio { get; set; }
        //    public Int16 peid_sDocumentoTipoId { get; set; }
        //    public string peid_vTipoDocumento { get; set; }
        //    public Int16 acno_sTipoActoNotarialId { get; set; }
        //    public Int16 acno_sSubTipoActoNotarialId { get; set; }
        //    public string vNumeroEscrituraPublica { get; set; }
        //    public string vNumeroPartida { get; set; }
        //    public DateTime? anpa_dFechaSuscripcion { get; set; }
        //    public string vNacionalidadPais { get; set; }
        //    public bool anpa_bFlagFirma { get; set; }
        //    public bool anpa_bFlagHuella { get; set; }
        //}

        #endregion

        protected void Tramite_Click(object sender, EventArgs e)
        {
            Session.Remove("Participante");
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            //-------------------------------------------------------
            //Fecha: 22/05/2020
            //Autor: Miguel Márquez Beltrán
            //Obs.: Nro. 8. El botón “limpiar” no tiene ninguna acción en esa pantalla.
            //Acción: Se oculta el botón limpiar.
            //-------------------------------------------------------
            //ctrlToolBar5.VisibleIButtonCancelar = true;
            //ctrlToolBar5.btnCancelar.Text = "    Limpiar";
            //ctrlToolBar5.btnCancelar.CssClass = "btnLimpiar";
            //-------------------------------------------------------

            ctrlToolBar5.VisibleIButtonGrabar = true;
            ctrlToolBar5.btnGrabar.Click += new EventHandler(btnGrabar5_Click);
            ctrlToolBar5.btnGrabar.OnClientClick = "return ValidarPago();";
            //cbxAfirmarTexto.CheckedChanged +=cbxAfirmarTexto_CheckedChanged;
            //----------------------------------------------------------------------------------------------------
            //Fecha: 22/05/2020
            //Autor: Miguel Márquez Beltrán
            //Obs.:Nro.1. Actos Notariales: No se muestra el botón de baja, reimpresión del autoadhesivo consular.
            //----------------------------------------------------------------------------------------------------            
            ctrlReimprimirbtn1.btnReimprimirHandler += new Accesorios.SharedControls.ctrlReimprimirbtn.OnButtonReimprimirClick(ctrlReimprimirbtn_btnReimprimirHandler);
            ctrlBajaAutoadhesivo1.btnAnularHandler += new Accesorios.SharedControls.ctrlBajaAutoadhesivo.OnButtonAnularClick(ctrlBajaAutoadhesivo_btnAnularAutoahesivo);
            ctrlBajaAutoadhesivo1.btnAceptarAnularHandler += new Accesorios.SharedControls.ctrlBajaAutoadhesivo.OnButtonAceptarAnulacionClick(ctrlBajaAutoadhesivo_btnAceptarAnularAutoahesivo);
            //----------------------------------------------------------------------------------------------------


            //btnAgregarParticipante.OnClientClick = "return ValidarIngresoParticipante();";
            btnAgregarParticipanteNew.OnClientClick = "return ValidarIngresoParticipante();";

            Btn_AgregarTarifa.OnClientClick = "return ValidarActuacionDetalle();";

            ctrFecPago.Text = DateTime.Now.ToString(ConfigurationManager.AppSettings["FormatoFechas"]);

            txtRegistradorNombres.Style.Add("display", "none");
            lblRegistrador.Style.Add("display", "none");
            txtTextoNormativo.Style.Add("display", "none");
            //ddl_peid_sDocumentoTipoId.AutoPostBack = true;
            ctrlReimprimirbtn1.GUID = HFGUID.Value;

            if (!Page.IsPostBack)
            {
                #region PostBack
                if (Session["tab_activa_vinculacion"] == null || Session["tab_activa_vinculacion"].ToString() == "NO")
                { Session["tab_activa_vinculacion"] = "NO"; }
                gdv_Normas.Visible = false;
                llenarTablaTarifasActoProtocolar();

                String CaracterEspecialRune = String.Empty;
                CaracterEspecialRune = ConfigurationManager.AppSettings["validarCaracterRune"].ToString();
                HFValidarTextoRune.Value = CaracterEspecialRune;

                List<CBE_PARTICIPANTE> lParticipantes = new List<CBE_PARTICIPANTE>();
                HttpContext.Current.Session["ParticipanteContainer"] = (List<CBE_PARTICIPANTE>)lParticipantes;

                ctrlBajaAutoadhesivo1.Activar = false;
                Session["dtActuacionDetalle"] = null;
                ddlTipoPago.Enabled = false;
                string codPersona = Util.DesEncriptar(Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString()));

                DataTable dtTipoParticipantes = new DataTable();
                dtTipoParticipantes = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REGISTRO_NOTARIAL_PROTOCOLAR_TIPO_PARTICIPANTE);
                Session["vwparticipantes"] = dtTipoParticipantes;
                //PARA_VVALOR (INICIA - RECIBE)

                CargaListasDesplegables();

                //--------------------------------------------------
                //Fecha: 16/03/2017
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
                //ViewState["Paises"] = dtPaises;
                Util.CargarDropDownList(ddlPaisOrigen, dtPaises, "PAIS_VNOMBRE", "PAIS_SPAISID", true);

                //-------------------------------------------------------
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
                        ddl_UbigeoPais.DataSource = obeUbigeoListas.Ubigeo01;
                        ddl_UbigeoPais.DataValueField = "Ubi01";
                        ddl_UbigeoPais.DataTextField = "Departamento";
                        ddl_UbigeoPais.DataBind();
                    }
                }
                //-------------------------------------------------------
                tablaOtroDocumento.Style.Add("display", "none");
                txtDescOtroDocumento.Enabled = false;

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
                    //-----------------------------------------------------
                    string codPersonaEncr = Request.QueryString["CodPer"].ToString();
                    if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                    {
                        Tramite.PostBackUrl = "~/Registro/FrmTramite.aspx?CodPer=" + codPersonaEncr + "&Juridica=1";
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
                            Tramite.PostBackUrl = "~/Registro/FrmTramite.aspx?CodPer=" + codPersonaEncr + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada;
                        }
                        else
                        {
                            Tramite.PostBackUrl = "~/Registro/FrmTramite.aspx?CodPer=" + codPersonaEncr;
                        }
                    }
                }
                hdn_CONFORMIDAD_DE_TEXTO.Value = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.AVISOS, Convert.ToInt32(Enumerador.enmNotarialAvisos.CONFORMIDAD_DE_TEXTO), "valor");
                SessionLugarSession();
                cbxAfirmarTexto.Text = hdn_CONFORMIDAD_DE_TEXTO.Value;
                //----------------------------------------------
                //Fecha: 02/12/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Deshabilitar el check de aprobado
                //        Radio Button Apoderado por defecto.
                //----------------------------------------------
                //--vpipa check
                string StrScript = string.Empty;
                StrScript = @"$('#MainContent_cbxAfirmarTexto').prop('disabled', true);$('#MainContent_Btn_AfirmarTextoLeido').prop('disabled', true);"; 
                StrScript = string.Format(StrScript);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitarChec", StrScript, true);
                //cbxAfirmarTexto.Enabled = false;

                rbOtros.Checked = false;
                rbApoderado.Checked = true;
                rbApoderado.Enabled = false;
                rbOtros.Enabled = false;
                ddl_Apoderado.Enabled = false;
                ddl_TipoDocrepresentante.Enabled = false;
                txtRepresentanteNombres.Enabled = false;
                txtRepresentanteNroDoc.Enabled = false;
                ddl_GerenoPresentante.Enabled = false;
                //-------------------------------------
                LoadHidden();


                if (Session["strBusqueda"] != null)
                {
                    Session.Remove("strBusqueda");
                }

                CargaDatosIniciales();

                if (Comun.ToNullInt64(hdn_actu_iPersonaRecurrenteId.Value) == 0)
                {
                    ddl_pers_sPersonaTipoId.SelectedValue = Convert.ToString(ViewState["iTipoId"]);
                    // ddl_pers_sPersonaTipoId_SelectedIndexChanged(sender, e);

                    string scriptMover = string.Empty;
                    switch (Comun.ToNullInt64(ddl_pers_sPersonaTipoId.SelectedValue))
                    {
                        case (Int64)Enumerador.enmTipoPersona.JURIDICA:
                            ddl_empr_sTipoDocumentoId.SelectedValue = Convert.ToString(ViewState["iDocumentoTipoId"]);
                            txt_empr_vNumeroDocumento.Text = Convert.ToString(ViewState["NroDoc"]);
                            txt_empr_vRazonSocial.Text = Convert.ToString(ViewState["ApePat"]);
                            break;
                        case (Int64)Enumerador.enmTipoPersona.NATURAL:
                            string strTipoDocumento = Convert.ToString(ViewState["iDocumentoTipoId"]);
                            //ddl_peid_sDocumentoTipoId.SelectedValue = Convert.ToString(ViewState["iDocumentoTipoId"]);
                            buscarValorTipoDocumento(ref ddl_peid_sDocumentoTipoId, strTipoDocumento);
                            txt_peid_vDocumentoNumero.Text = Convert.ToString(ViewState["NroDoc"]);
                            break;
                    }
                }
                //-------------------------------------------------
                //Fecha: 17/04/2020
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Reemplazar la session por ViewState
                //-------------------------------------------------
                ViewState["ModoEdicionProtocolar"] = false;
                //Session["ModoEdicionProtocolar"] = false;

                //CargaListasDesplegables();
                if (ddl_pers_sPersonaTipoId.Items.Count > 1)
                {
                    ddl_pers_sPersonaTipoId.SelectedIndex = 2;
                    //ddl_pers_sPersonaTipoId_SelectedIndexChanged(sender, e);
                }
                
                
                ModoEdicion();
                //--------------------------------------------------------------------
                //Fecha: 14/12/2021
                //Autor:  Miguel Márquez Beltrán
                //Motivo: Limpia los datos si ya existe en la grilla.
                //--------------------------------------------------------------------
                string vNumeroDocumento = txt_peid_vDocumentoNumero.Text.ToUpper();
                string vTipoDocumento = ddl_peid_sDocumentoTipoId.Text.ToUpper();
                if (vNumeroDocumento.Length > 0 && vTipoDocumento.Length > 0)
                {
                    bool bExisteExtranjero = false;

                    for (int i = 0; i < grd_Participantes.Rows.Count; i++)
                    {
                        if (grd_Participantes.Rows[i].Cells[2].Text == vTipoDocumento &&
                           grd_Participantes.Rows[i].Cells[3].Text == vNumeroDocumento)
                        {
                            string strScript2 = string.Empty;
                            strScript2 = @"$(function(){{tab_02_Limpiar();}});";
                            strScript2 = string.Format(strScript2);
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "deshabilitarAux", strScript2, true);
                        }
                        
                        if (ObtenerIniciaRecibe(grd_Participantes.Rows[i].Cells[0].Text) == "INICIA")
                        {
                            if (grd_Participantes.Rows[i].Cells[6].Text != "PERUANA")
                            {
                                bExisteExtranjero = true;
                            }
                        }
                    }
                    if (bExisteExtranjero == true)
                    {
                        tablaIncisoCArticulo55.Style.Add("display", "block");
                    }
                    else
                    {
                        tablaIncisoCArticulo55.Style.Add("display", "none");
                    }
                    updIncisoCArticulo55.Update();     
                }
                //----------------------------------------
                string valor = "";
                if (Convert.ToInt32(Session["Actuacion_Accion"]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
                {
                    ctrlReimprimirbtn1.Activar = false;
                    ctrlBajaAutoadhesivo1.Activar = false;

                    btnGrabarVinculacion.Enabled = false;
                    btnLimpiarVinc.Enabled = false;//--vpipa
                }
                else
                {                    
                    valor = Convert.ToString(Request.QueryString["cod"]);
                    if (valor == "1")
                    {
                        ctrlReimprimirbtn1.Activar = false;

                        //--------------------------------------------
                        //Fecha: 06/01/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Desactivar el botón autoadhesivo y
                        //          activar cuando se haya grabado.
                        //--------------------------------------------
                        //btnAutoadhesivo.Enabled = true;

                    }
                    else
                    {
                        ctrlReimprimirbtn1.Activar = chkImpresionCorrecta.Checked;
                    }


                    CargarUltimoInsumo();
                }
                //-----------------------------------------------------------------------
                //Fecha: 02/03/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Asignar las normas filtradas
                //-----------------------------------------------------------------------
                //if (Cmb_TipoActoNotarial.SelectedIndex > 0)
                //{
                //    AsignarNormasporTipoActoNotarial();
                //}
                //-----------------------------------------------------------------------
                ////--------------------------------------------------
                ////Fecha: 16/03/2017
                ////Autor: Miguel Márquez Beltrán
                ////Objetivo: Consultar la tabla PS_SISTEMA.SI_Pais
                ////--------------------------------------------------
                //DataTable dtPaises = new DataTable();
                //dtPaises = Comun.ConsultarPaises();
                ////--------------------------------------------------
                ////Fecha: 03/04/2020
                ////Autor: Miguel Márquez Beltrán
                ////Motivo: Asignar Paises a un ViewState.
                ////--------------------------------------------------
                //ViewState["Paises"] = dtPaises;
                //Util.CargarDropDownList(ddlPaisOrigen, dtPaises, "PAIS_VNOMBRE", "PAIS_SPAISID", true);

                //--------------------------------------------------
                CargarComboApoderados();

                SeleccionarComboApoderado(Convert.ToInt16(ddl_TipoDocrepresentante.SelectedValue), txtRepresentanteNroDoc.Text.ToString());


                int intEstadoId = Consultar_Estado_Cs(Convert.ToInt64(hdn_acno_iActoNotarialId.Value), Convert.ToInt64(hdn_acno_iActuacionId.Value));
                if (intEstadoId == (int)Enumerador.enmNotarialProtocolarEstado.APROBADA ||
                    intEstadoId == (int)Enumerador.enmNotarialProtocolarEstado.PAGADA ||
                    intEstadoId == (int)Enumerador.enmNotarialProtocolarEstado.DIGITALIZADA)
                {
                    ddl_Apoderado.Enabled = false;

                    /*//se adiciona calculoVisual(); para mostrar los montos al editar calculoVisual();
                    string strscript2 = @"$(function(){{ calculoVisual();  }});";
                    strscript2 = string.Format(strscript2);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DesHabilitarTabs", strscript2, true);*/

                }
                if (intEstadoId == (int)Enumerador.enmNotarialProtocolarEstado.ASOCIADA ||
                    intEstadoId == (int)Enumerador.enmNotarialProtocolarEstado.TRANSCRITA)
                {
                    rbApoderado.Enabled = true;
                    rbOtros.Enabled = true;
                    rbApoderado.Checked = true;
                    ddl_Apoderado.Enabled = true;
                }
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

                hdn_Tipo_Participante_Editando.Value = "-1";
                ActualizarGenero();

                btnActualizarFechaOtorgante.Enabled = false;
                this.ctrFechaSuscripcion.StartDate = new DateTime(1900, 1, 1);
                this.ctrFechaSuscripcion.AllowFutureDate = true;
                //----------------------------------------------------
                //Fecha: 17/11/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: documento de observación_EP_17112021
                //          Permitir fechas futuras en la fecha de 
                //          suscripción.
                //----------------------------------------------------
                //this.ctrFechaSuscripcion.EndDate = DateTime.Now;
                //----------------------------------------------------
                //----------------------------------------//                
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 27-02-2019
                // Objetivo: Ocultar controles de exoneración
                //----------------------------------------//
                lblExoneracion.Visible = false;
                ddlExoneracion.Visible = false;
                //----------------------------------------//
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 27-02-2019
                // Objetivo: Ocultar controles de Sustento
                //----------------------------------------//
                lblSustentoTipoPago.Visible = false;
                txtSustentoTipoPago.Visible = false;
                lblValSustentoTipoPago.Visible = false;
                RBNormativa.Visible = false;
                RBSustentoTipoPago.Visible = false;
                lblValExoneracion.Visible = false;
               
                updDatosParticipantes.Update();

                txtFindOficinaRegistral.Attributes.Add("style", "visibility:hidden");
                txtFindOficinaConsularPri.Attributes.Add("style", "visibility:hidden");
                this.ctrFechaExpedicionPri.StartDate = new DateTime(1900, 1, 1);
                this.ctrFechaExpedicionPri.EndDate = DateTime.Now;

                //--------------------------------------------------
                //Fecha: 26/11/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Deshabilitar las pestañas al iniciar
                //          un nuevo registro protocolar.
                //--------------------------------------------------
                if (Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value) == 0)
                {
                    string strscript2 = string.Empty;
                    strscript2 = @"$(function(){{
                                            Desabilitar_Tab(1);Desabilitar_Tab(2);Desabilitar_Tab(3);Desabilitar_Tab(4);Desabilitar_Tab(5); 
                                            }});";
                    strscript2 = string.Format(strscript2);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DesHabilitarTabs", strscript2, true);
                }
                if (intEstadoId == (int)Enumerador.enmNotarialProtocolarEstado.PAGADA)
                {
                    string strScript = string.Empty;
                    strScript = @"$(function(){{
                                            Desabilitar_Tab(5);
                                        }});";
                    strScript = string.Format(strScript);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DeshabilitarTabDigitalizacion", strScript, true);

                }
                //--------------------------------------------------
                #endregion

                //----------------------------------------//
            }
            else
            {
                if (Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value) > 0)
                {
                    if (this.hdf_ActoNotarialPrimigeniaId.Value.Length > 0)
                    {
                        if (Convert.ToInt64(this.hdf_ActoNotarialPrimigeniaId.Value) > 0)
                        {
                            string strScript = string.Empty;
                            strScript = @"$(function(){{
                                            SoloHabilitarTablaPrimigenia(); 
                                            }});";
                            strScript = string.Format(strScript);
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitarTablaPrimigenia", strScript, true);

                        }
                    }
                }
                if (Session["tab_activa_vinculacion"].ToString() == "SI")
                {
                    string strScript = @"$(function(){{ HabilitarTabVinculacion(); }});";

                    strScript = string.Format(strScript);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitarTabVinculacion", strScript, true);
                    Session["tab_activa_vinculacion"] = "NO";
                }
            }
            Thread.Sleep(0);
        }

        private void LoadHidden()
        {
            HF_OTORGANTE.Value = Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE);
            HF_APODERADO.Value = Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.APODERADO);
            HF_INTERPRETE.Value = Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE);
            HF_TESTIGO_A_RUEGO.Value = Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO);
            HF_VENDEDOR.Value = Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR);
            HF_COMPRADOR.Value = Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.COMPRADOR);
            HF_ANTICIPANTE.Value = Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE);
            HF_ANTICIPADO.Value = Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPADO);

            HF_DONANTE.Value = Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE);
            HF_DONATARIO.Value = Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.DONATARIO);

            HF_NACIONALIDAD_EXTRANJERA.Value = Convert.ToString((int)Enumerador.enmNacionalidad.EXTRANJERA);
            HF_NACIONALIDAD_PERUANA.Value = Convert.ToString((int)Enumerador.enmNacionalidad.PERUANA);

            HF_PAGADO_EN_LIMA.Value = Convert.ToString((int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA);

            HF_GRATIS.Value = Convert.ToString((int)Enumerador.enmTipoCobroActuacion.GRATIS);
            HF_NOCOBRADO.Value = Convert.ToString((int)Enumerador.enmTipoCobroActuacion.NO_COBRADO);
            HF_EFECTIVO.Value = Convert.ToString((int)Enumerador.enmTipoCobroActuacion.EFECTIVO);

            HF_TIPOACTO_COMPRA_VENTA.Value = Convert.ToString((int)Enumerador.enmProtocolarTipo.COMPRA_VENTA);
            //HF_FORMATOPROTOCOLARES.Value = ConfigurationManager.AppSettings["FormatoProtocolar"].ToString();

            hdn_AccionActualizar.Value = Convert.ToString((int)Enumerador.enmTipoOperacion.ACTUALIZACION);
            hdn_AccionConsultar.Value = Convert.ToString((int)Enumerador.enmTipoOperacion.CONSULTA);

            if (Request.QueryString["class"] != null)
                hdn_AccionOperacion.Value = Request.QueryString["class"].ToString();
            else
                hdn_AccionOperacion.Value = "1054";
        }

        #region Pestaña: Registro
        [System.Web.Services.WebMethod]
        public static string tab_registro(string iActuacionId, string iActoNotarialId)
        {
            ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();

            RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();
            lACTONOTARIAL.acno_iActoNotarialId = Convert.ToInt64(iActoNotarialId);
            lACTONOTARIAL.acno_iActuacionId = Convert.ToInt64(iActuacionId);
            lACTONOTARIAL.acno_sTipoActoNotarialId = Convert.ToInt16(Enumerador.enmNotarialTipoActo.PROTOCOLAR);
            lACTONOTARIAL = lActoNotarialConsultaBL.obtener(lACTONOTARIAL);

            JavaScriptSerializer serialize = new JavaScriptSerializer();
            return serialize.Serialize(lACTONOTARIAL).ToString();
        }

        [System.Web.Services.WebMethod]
        public static string insert_registro(string actonotarial)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var jsonObject = serializer.Deserialize<dynamic>(actonotarial);

            RE_ACTONOTARIAL lactonotarial = new RE_ACTONOTARIAL();
            #region Creando objeto
            lactonotarial.acno_iActoNotarialId = Convert.ToInt64(jsonObject["acno_iActoNotarialId"]);
            lactonotarial.acno_iActuacionId = Convert.ToInt64(jsonObject["acno_iActuacionId"]);
            lactonotarial.acno_sOficinaConsularId = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            //Cuando es EDICIÓN 
            if ((lactonotarial.acno_iActoNotarialId != 0) || (lactonotarial.acno_iActuacionId != 0))
            {
                ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();
                lactonotarial = lActoNotarialConsultaBL.obtener(lactonotarial);
            }

            lactonotarial.acno_sTipoActoNotarialId = Convert.ToInt16(Enumerador.enmNotarialTipoActo.PROTOCOLAR);
            lactonotarial.acno_sSubTipoActoNotarialId = Convert.ToInt16(jsonObject["acno_sSubTipoActoNotarialId"]);
            lactonotarial.acno_bFlagMinuta = Convert.ToBoolean(jsonObject["acno_bFlagMinuta"]);
            lactonotarial.acno_vDenominacion = Convert.ToString(jsonObject["acno_vDenominacion"]);
            lactonotarial.acno_IFuncionarioAutorizadorId = Convert.ToInt32(jsonObject["acno_IFuncionarioAutorizadorId"]);

            //------------------------------------------------------------------------------------------------------------------
            //Campos para el subFlujo de Rectificaciones
            //------------------------------------------------------------------------------------------------------------------
            lactonotarial.acno_vNumeroEscrituraPublica = Convert.ToString(jsonObject["acno_vNumeroEscrituraPublica"]); // Para la vinculacion de un Nro de escritura anterior
            lactonotarial.acno_sAccionSubTipoActoNotarialId = Convert.ToInt16(jsonObject["acno_sAccionSubTipoActoNotarialId"]); // Para el registro del accion realizado con la escritura publica
            lactonotarial.acno_iActoNotarialReferenciaId = Convert.ToInt64(jsonObject["acno_iActoNotarialReferenciaId"]); // Para la vinculacion de un Nro de acto notarial anterior
            //------------------------------------------------------------------------------------------------------------------

            lactonotarial.acno_sEstadoId = (Int16)Enumerador.enmNotarialProtocolarEstado.REGISTRADO;

            //Log : Insersión
            lactonotarial.acno_dFechaCreacion = DateTime.Today;
            lactonotarial.acno_dFechaExtension = DateTime.Today;
            lactonotarial.acno_sUsuarioCreacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            lactonotarial.acno_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();

            //Log : Modificación
            lactonotarial.acno_dFechaModificacion = DateTime.Today;
            lactonotarial.acno_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            lactonotarial.acno_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
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
            if (HttpContext.Current.Session[Constantes.CONST_SESION_CIUDAD_ITINERANTE] != null)
            {
                if (HttpContext.Current.Session[Constantes.CONST_SESION_CIUDAD_ITINERANTE].ToString() != "")
                {
                    lACTUACION.actu_sCiudadItinerante = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_CIUDAD_CODIGO_ITINERANTE].ToString());
                }
            }
            #endregion

            lactonotarial.ACTUACION = lACTUACION;

            //----------------------------------------------------------------------
            HttpContext.Current.Session["dtDatosEscritura"] = null;
            //----------------------------------------------------------------------
            ActoNotarialMantenimiento mnt = new ActoNotarialMantenimiento();

            string strValidarTablaPrimigenia = jsonObject["ValidarTablaPrimigenia"];
            string strFechaExpedicionPri = "";

            RE_ACTONOTARIAL_PRIMIGENIA ActoNotarialPrimigeniaBE = new RE_ACTONOTARIAL_PRIMIGENIA();
            if (strValidarTablaPrimigenia == "S")
            {
                DateTime datFecha = new DateTime();

                strFechaExpedicionPri = Convert.ToString(jsonObject["FechaExpedicionPri"]);

                if (strFechaExpedicionPri.Length == 0)
                {
                    datFecha = DateTime.MinValue;
                }
                else
                {
                    if (!DateTime.TryParse(strFechaExpedicionPri, out datFecha))
                    {
                        datFecha = Comun.FormatearFecha(strFechaExpedicionPri);
                    }  
                }

                ActoNotarialPrimigeniaBE.anpr_iActoNotarialId = Convert.ToInt64(jsonObject["acno_iActoNotarialId"]);
                ActoNotarialPrimigeniaBE.anpr_cAnioEscritura = Convert.ToString(jsonObject["AnioEscrituraPri"]);
                ActoNotarialPrimigeniaBE.anpr_vNumeroEscrituraPublica = Convert.ToString(jsonObject["NumeroEscrituraPublicaPri"]);
                ActoNotarialPrimigeniaBE.anpr_sOficinaConsularId = Convert.ToInt16(jsonObject["OficinaConsularPri"]);

                ActoNotarialPrimigeniaBE.anpr_dFechaExpedicion = datFecha;
                                
                ActoNotarialPrimigeniaBE.anpr_vTipoActoNotarial = Convert.ToString(jsonObject["TipoActoNotarialPri"]);
                ActoNotarialPrimigeniaBE.anpr_vNotaria = Convert.ToString(jsonObject["Notaria"]);
                ActoNotarialPrimigeniaBE.anpr_iActoNotarialPrimigeniaId = Convert.ToInt64(jsonObject["ActoNotarialPrimigeniaId"]);
                ActoNotarialPrimigeniaBE.anpr_cEstado = "A";
                ActoNotarialPrimigeniaBE.anpr_sUsuarioCreacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                ActoNotarialPrimigeniaBE.anpr_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                ActoNotarialPrimigeniaBE.anpr_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
                ActoNotarialPrimigeniaBE.anpr_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
                ActoNotarialPrimigeniaBE.OficinaConsultar = lactonotarial.acno_sOficinaConsularId;
                if (jsonObject["acno_iActoNotarialReferenciaPriId"] == "")
                {
                    ActoNotarialPrimigeniaBE.iActoNotarialReferencialId = 0;
                }
                else
                {
                    ActoNotarialPrimigeniaBE.iActoNotarialReferencialId = Convert.ToInt64(jsonObject["acno_iActoNotarialReferenciaPriId"]);
                }
            }
            //----------------------------------------------------
            //Fecha: 10/01/2022
            //Motivo: Inicializar los participantes.
            //Autor: Miguel Márquez Beltrán
            //----------------------------------------------------
            List<CBE_PARTICIPANTE> lParticipantes = new List<CBE_PARTICIPANTE>();
            HttpContext.Current.Session["ParticipanteContainer"] = (List<CBE_PARTICIPANTE>)lParticipantes;
            //----------------------------------------------------
            return serializer.Serialize(mnt.Insertar_ActoNotarial(lactonotarial, strValidarTablaPrimigenia, ActoNotarialPrimigeniaBE)).ToString();
        }

        #endregion

        #region Pestaña: Participante



        [System.Web.Services.WebMethod]
        public static string Consultar_Estado(Int64 hdn_acno_iActoNotarialId, Int64 hdn_acno_iActuacionId)
        {
            RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();
            ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();

            string s_Valor = string.Empty;
            if (hdn_acno_iActoNotarialId != 0)
            {
                lACTONOTARIAL.acno_iActoNotarialId = hdn_acno_iActoNotarialId;
                lACTONOTARIAL.acno_iActuacionId = hdn_acno_iActuacionId;
                lACTONOTARIAL.acno_sTipoActoNotarialId = Convert.ToInt16(Enumerador.enmNotarialTipoActo.PROTOCOLAR);
                lACTONOTARIAL = lActoNotarialConsultaBL.obtener(lACTONOTARIAL);

                s_Valor = lACTONOTARIAL.acno_sEstadoId.ToString();
            }
            else
            {
                s_Valor = "";
            }

            return s_Valor;
        }

        public int Consultar_Estado_Cs(Int64 hdn_acno_iActoNotarialId, Int64 hdn_acno_iActuacionId)
        {
            RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();
            ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();

            int intValor = 0;
            if (hdn_acno_iActoNotarialId != 0)
            {
                lACTONOTARIAL.acno_iActoNotarialId = hdn_acno_iActoNotarialId;
                lACTONOTARIAL.acno_iActuacionId = hdn_acno_iActuacionId;
                lACTONOTARIAL.acno_sTipoActoNotarialId = Convert.ToInt16(Enumerador.enmNotarialTipoActo.PROTOCOLAR);
                lACTONOTARIAL = lActoNotarialConsultaBL.obtener(lACTONOTARIAL);

                intValor = lACTONOTARIAL.acno_sEstadoId;
            }
            else
            {
                intValor = 0;
            }

            return intValor;
        }



        protected void grd_Participantes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 lRowIndex = Convert.ToInt32(e.CommandArgument);
            List<CBE_PARTICIPANTE> Participantes = (List<CBE_PARTICIPANTE>)Session["ParticipanteContainer"];

            if (Participantes.Count == 0)
            { return; }

            Participantes = Participantes.Where(x => x.anpa_cEstado == "A").ToList();
            int indiceParticipante = lRowIndex;
            Int64 iPersonaID = 0;
            switch (e.CommandName.ToString())
            {
                case "Editar":
                    #region Editar

                    //if (Participantes[lRowIndex].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE) ||
                    //    Participantes[lRowIndex].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR) ||
                    //    Participantes[lRowIndex].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE) ||
                    //    Participantes[lRowIndex].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE))

                    if (ObtenerIniciaRecibe(Participantes[lRowIndex].acpa_sTipoParticipanteId_desc)=="INICIA")
                    {
                        #region Validación_de_Testigo_A_Ruego
                        if (Participantes.Where(x => x.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO) && x.anpa_cEstado != "E" && x.anpa_iReferenciaParticipanteId == Participantes[lRowIndex].anpa_iActoNotarialParticipanteIdAux).Count() > 0)
                        {
                            string strScriptt = "";

                            //if (Participantes[lRowIndex].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE))
                            //    strScriptt = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", "No puede editar el otorgante porque se encuentra enlazado a un Testigo a Ruego.");

                            //if (Participantes[lRowIndex].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR))
                            //    strScriptt = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", "No puede editar el vendedor porque se encuentra enlazado a un Testigo a Ruego.");

                            //if (Participantes[lRowIndex].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE))
                            //    strScriptt = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", "No puede editar el anticipante porque se encuentra enlazado a un Testigo a Ruego.");

                            //if (Participantes[lRowIndex].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE))
                            //    strScriptt = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", "No puede editar el donante porque se encuentra enlazado a un Testigo a Ruego.");

                            if (ObtenerIniciaRecibe(Participantes[lRowIndex].anpa_sTipoParticipanteId) == "INICIA")
                            {
                                string strNombreParticipante = ObtenerNombreParticipante(Participantes[lRowIndex].anpa_sTipoParticipanteId);
                                strScriptt = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", "No puede editar el " + strNombreParticipante + " porque se encuentra enlazado a un Testigo a Ruego.");
                            }

                            EjecutarScript(strScriptt);
                            return;
                        }
                        #endregion
                        #region Validación_de_Interprete
                        if (Participantes.Where(x => x.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE) && x.anpa_cEstado != "E" && x.anpa_iReferenciaParticipanteId == Participantes[lRowIndex].anpa_iActoNotarialParticipanteIdAux).Count() > 0)
                        {
                            string strScriptt = "";

                            //if (Participantes[lRowIndex].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE))
                            //    strScriptt = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", "No puede editar el otorgante porque se encuentra enlazado a un Interprete.");

                            //if (Participantes[lRowIndex].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR))
                            //    strScriptt = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", "No puede editar el vendedor porque se encuentra enlazado a un Interprete.");

                            //if (Participantes[lRowIndex].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE))
                            //    strScriptt = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", "No puede editar el donante porque se encuentra enlazado a un Interprete.");

                            //if (Participantes[lRowIndex].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE))
                            //    strScriptt = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", "No puede editar el anticipante porque se encuentra enlazado a un Interprete.");

                            if (ObtenerIniciaRecibe(Participantes[lRowIndex].anpa_sTipoParticipanteId) == "INICIA")
                            {
                                string strNombreParticipante = ObtenerNombreParticipante(Participantes[lRowIndex].anpa_sTipoParticipanteId);
                                strScriptt = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", "No puede editar el " + strNombreParticipante + " porque se encuentra enlazado a un Interprete.");
                            }

                            EjecutarScript(strScriptt);
                            return;
                        }
                        #endregion
                    }
                    String strScrip = String.Empty;
                    strScrip += @"$(function(){{ ";
                    //-------------------------------------------------------------

                    this.ddl_anpa_sTipoParticipanteId.SelectedValue = Participantes[lRowIndex].anpa_sTipoParticipanteId.ToString();
                    this.ddl_pers_sGeneroId.SelectedValue = "0";
                    this.ddl_pers_sEstadoCivilId.SelectedValue = "0";
                    this.ddl_pers_sProfesionId.SelectedValue = "0";
                    this.ddl_pers_sIdiomaNatalId.SelectedValue = "0";
                    this.txt_resi_vResidenciaDireccion.Text = "";
                    //this.ddl_UbigeoPais.SelectedValue = "0";
                    //this.ddl_UbigeoRegion.SelectedValue = "0";
                    //this.ddl_UbigeoCiudad.SelectedValue = "0";
                    this.txt_resi_vCodigoPostal.Text = "";
                    this.ddlPaisOrigen.SelectedValue = "0";

                    hdn_Tipo_Participante_Editando.Value = Participantes[lRowIndex].anpa_sTipoParticipanteId.ToString();
                    Session["ActoNotarialParticipanteId"] = grd_Participantes.Rows[lRowIndex].Cells[7].Text;
                    ViewState["ExtraIndiceEdicion"] = indiceParticipante;
                    hdn_IndiceGrillaParticipantesEdicion.Value = "1";
                    HF_REGISTRO_NUEVO.Value = "0";

                    string strTipoDocumento = Participantes[lRowIndex].Identificacion.peid_sDocumentoTipoId.ToString();
                    buscarValorTipoDocumento(ref ddl_peid_sDocumentoTipoId, strTipoDocumento);

                    //this.ddl_peid_sDocumentoTipoId.SelectedValue = Participantes[lRowIndex].Identificacion.peid_sDocumentoTipoId.ToString();

                    //--------------------------------------------
                    //Fecha: 30/07/2020
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Activar Otro documento.
                    //--------------------------------------------
                    if (Convert.ToInt32(ddl_peid_sDocumentoTipoId.SelectedValue) == (int)Enumerador.enmTipoDocumento.OTROS)
                    {
                        txtDescOtroDocumento.Text = Participantes[lRowIndex].Identificacion.peid_vTipodocumento.ToString();
                    }
                    else
                    {
                        txtDescOtroDocumento.Text = "";
                    }
                    strScrip += " ActivarOtrodocumento(); ";
                    //--------------------------------------------

                    this.txt_peid_vDocumentoNumero.Text = Participantes[lRowIndex].Identificacion.peid_vDocumentoNumero.ToString();

                    this.ddl_pers_sNacionalidadId.SelectedValue = Participantes[lRowIndex].Persona.pers_sNacionalidadId.ToString();
                    this.txt_pers_vApellidoPaterno.Text = Participantes[lRowIndex].Persona.pers_vApellidoPaterno.ToString().Trim();
                    this.txt_pers_vApellidoMaterno.Text = Participantes[lRowIndex].Persona.pers_vApellidoMaterno.ToString().Trim();
                    this.txt_pers_vNombres.Text = Participantes[lRowIndex].Persona.pers_vNombres.ToString().Trim();

                    //-------------------------------------------------------------
                    //Fecha: 05/01/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Se adiciona el apellido de casada.
                    //-------------------------------------------------------------
                    this.txt_pers_vApellidoCasada.Text = Participantes[lRowIndex].Persona.pers_vApellidoCasada.ToString().Trim();
                   
                    strScrip += " validarApellidoCasada(); ";
                    
                    //--------------------------------------------------------
                    //Fecha: 15/03/2017
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Validaciones de los campos
                    //--------------------------------------------------------
                    if (Participantes[lRowIndex].Persona.pers_sGeneroId.ToString().Length > 0)
                    {
                        this.ddl_pers_sGeneroId.SelectedValue = Participantes[lRowIndex].Persona.pers_sGeneroId.ToString();
                        strScrip += " ObtenerElementosGenero(); ";
                    }
                    if (Participantes[lRowIndex].Persona.pers_sPaisId.ToString().Length > 0)
                    {
                        this.ddlPaisOrigen.SelectedValue = Participantes[lRowIndex].Persona.pers_sPaisId.ToString();
                        this.LblDescNacionalidadCopia.Text = Participantes[lRowIndex].Persona.vNacionalidad;
                    }
                    if (Participantes[lRowIndex].Persona.pers_sEstadoCivilId.ToString().Length > 0)
                    {
                        this.ddl_pers_sEstadoCivilId.SelectedValue = Participantes[lRowIndex].Persona.pers_sEstadoCivilId.ToString();
                    }
                    if (Participantes[lRowIndex].Persona.pers_sOcupacionId.ToString().Length > 0)
                    {
                        this.ddl_pers_sProfesionId.SelectedValue = Participantes[lRowIndex].Persona.pers_sOcupacionId.ToString();
                    }
                    if (Participantes[lRowIndex].Persona.pers_sIdiomaNatalId.ToString().Length > 0)
                    {
                        this.ddl_pers_sIdiomaNatalId.SelectedValue = Participantes[lRowIndex].Persona.pers_sIdiomaNatalId.ToString();
                    }
                    //--------------------------------------------------------
                    if (Participantes[lRowIndex].Persona.pers_bIncapacidadFlag)
                    {
                        chkIncapacitado.Checked = true;
                        txtRegistroTipoIncapacidad.Text = Participantes[lRowIndex].Persona.pers_vDescripcionIncapacidad.ToString();
                        strScrip += " MostrarHuella(); ";

                        if (Participantes[lRowIndex].anpa_bFlagHuella)
                        {
                            chkNoHuella.Checked = true;

                        }
                        else
                        {
                            chkNoHuella.Checked = false;
                        }                         
                    }
                    else
                    {
                        strScrip += " OcultarHuella(); ";
                        chkIncapacitado.Checked = false;
                        txtRegistroTipoIncapacidad.Text = String.Empty;
                        chkIncapacitado.Enabled = true;
                        txtRegistroTipoIncapacidad.Enabled = true;
                        chkNoHuella.Checked = false;
                    }
                    //-------------------------------------------------
                    //Fecha: 17/04/2020
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Reemplazar la session por ViewState
                    //-------------------------------------------------

                    //if (Convert.ToBoolean(Session["ModoEdicionProtocolar"]) == false)
                    if (Convert.ToBoolean(ViewState["ModoEdicionProtocolar"]) == false)
                    {
                        if (Participantes[lRowIndex].Residencia.resi_cResidenciaUbigeo != null) // MDIAZ 24/08/2015 - 23:46
                        {
                            if (Participantes[lRowIndex].Residencia.resi_cResidenciaUbigeo.Length > 0)
                            {
                                if (Participantes[lRowIndex].Residencia.resi_iResidenciaId != 0)
                                    hdn_acpa_residenciaId.Value = Participantes[lRowIndex].Residencia.resi_iResidenciaId.ToString();

                                this.txt_resi_vResidenciaDireccion.Text = Participantes[lRowIndex].Residencia.resi_vResidenciaDireccion.ToString();
                                this.txt_resi_vCodigoPostal.Text = Participantes[lRowIndex].Residencia.resi_vCodigoPostal.ToString();

                                iPersonaID = Convert.ToInt64(Participantes[lRowIndex].anpa_iPersonaId.ToString());

                                if (Participantes[lRowIndex].Residencia.resi_cResidenciaUbigeo.Length == 6)
                                {                                 
                                    string ubi01 = Participantes[lRowIndex].Residencia.resi_cResidenciaUbigeo.Substring(0, 2);
                                    string ubi02 = Participantes[lRowIndex].Residencia.resi_cResidenciaUbigeo.Substring(2, 2);
                                    string ubi03 = Participantes[lRowIndex].Residencia.resi_cResidenciaUbigeo.Substring(4, 2);

                                    hdn_acpa_pais.Value = ubi01;
                                    hdn_acpa_provincia.Value = ubi02;
                                    hdn_acpa_distrito.Value = ubi03;

                                    hubigeo.Value = ubi01 + ubi02 + ubi03;

                                    //---------------------------------------------------------
                                    //Fecha: 10/12/2021
                                    //Autor: Miguel Márquez Beltrán
                                    //Motivo: Mostrar la entrada de código postal 
                                    //          cuando el país sea Estados Unidos de América.
                                    //---------------------------------------------------------
                                    string strScript = string.Empty;
                                    
                                    if (ubi01 + ubi02 == "9213")
                                    {
                                        strScript = @"$(function(){{
                                            HabilitarCodigoPostal(); 
                                            }});";
                                    }
                                    else
                                    {
                                        strScript = @"$(function(){{
                                            DeshabilitarCodigoPostal(); 
                                            }});";
                                    }
                                    strScript = string.Format(strScript);
                                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DesHabilitarCodigoPostal", strScript, true);
                                    //-----------------------------------------------------

                                    comun_Part3.CargarUbigeo(Session, this.ddl_UbigeoPais, Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT, string.Empty, string.Empty, true);
                                    this.ddl_UbigeoPais.SelectedValue = ubi01;
                                    comun_Part3.CargarUbigeo(Session, this.ddl_UbigeoRegion, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ubi01, string.Empty, true);
                                    this.ddl_UbigeoRegion.SelectedValue = ubi02;
                                    comun_Part3.CargarUbigeo(Session, this.ddl_UbigeoCiudad, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ubi01, ubi02, true);
                                    this.ddl_UbigeoCiudad.SelectedValue = ubi03;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Participantes[lRowIndex].Persona._ResidenciaTop != null)
                        {
                            if (Participantes[lRowIndex].Persona._ResidenciaTop.resi_iResidenciaId != 0)
                                hdn_acpa_residenciaId.Value = Participantes[lRowIndex].Persona._ResidenciaTop.resi_iResidenciaId.ToString();

                            this.txt_resi_vResidenciaDireccion.Text = Participantes[lRowIndex].Persona._ResidenciaTop.resi_vResidenciaDireccion.ToString();
                            this.txt_resi_vCodigoPostal.Text = Participantes[lRowIndex].Persona._ResidenciaTop.resi_vCodigoPostal.ToString();

                            string strUbigeo = Participantes[lRowIndex].Persona._ResidenciaTop.resi_cResidenciaUbigeo;
                            string ubi01 = "";
                            string ubi02 = "";
                            string ubi03 = "";

                            if (strUbigeo != null)
                            {
                                if (strUbigeo.Length > 1)
                                {
                                    ubi01 = Participantes[lRowIndex].Persona._ResidenciaTop.resi_cResidenciaUbigeo.Substring(0, 2);
                                }
                                if (strUbigeo.Length > 3)
                                {
                                    ubi02 = Participantes[lRowIndex].Persona._ResidenciaTop.resi_cResidenciaUbigeo.Substring(2, 2);
                                }
                                if (strUbigeo.Length == 6)
                                {
                                    ubi03 = Participantes[lRowIndex].Persona._ResidenciaTop.resi_cResidenciaUbigeo.Substring(4, 2);
                                }

                                hubigeo.Value = ubi01 + ubi02 + ubi03;
                            }
                            //---------------------------------------------------------
                            //Fecha: 10/12/2021
                            //Autor: Miguel Márquez Beltrán
                            //Motivo: Mostrar la entrada de código postal 
                            //          cuando el país sea Estados Unidos de América.
                            //---------------------------------------------------------
                            string strScript = string.Empty;

                            if (ubi01 + ubi02 == "9213")
                            {
                                strScript = @"$(function(){{
                                            HabilitarCodigoPostal(); 
                                            }});";
                            }
                            else
                            {
                                strScript = @"$(function(){{
                                            DeshabilitarCodigoPostal(); 
                                            }});";
                            }
                            strScript = string.Format(strScript);
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "DesHabilitarCodigoPostal", strScript, true);
                            //---------------------------------------------------------
                            iPersonaID = Convert.ToInt64(Participantes[lRowIndex].anpa_iPersonaId.ToString());

                            // UBIGEO ...
                            if (ubi01.Length == 2)
                            {
                                comun_Part3.CargarUbigeo(Session, this.ddl_UbigeoPais, Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT, string.Empty, string.Empty, true);
                                this.ddl_UbigeoPais.SelectedValue = ubi01;

                                if (ubi02.Length == 2)
                                {
                                    comun_Part3.CargarUbigeo(Session, this.ddl_UbigeoRegion, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ubi01, string.Empty, true);
                                    this.ddl_UbigeoRegion.SelectedValue = ubi02;

                                    if (ubi03.Length == 2)
                                    {
                                        comun_Part3.CargarUbigeo(Session, this.ddl_UbigeoCiudad, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ubi01, ubi02, true);
                                        this.ddl_UbigeoCiudad.SelectedValue = ubi03;
                                    }
                                }
                            }
                        }
                    }

                    // SE DESBLOQUEAN LOS CONTROLES
                    //-------------------------------------------------
                    //Fecha: 17/04/2020
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Reemplazar la session por ViewState
                    //-------------------------------------------------

                    //if (Convert.ToBoolean(Session["ModoEdicionProtocolar"]) == false)

                    if (Convert.ToBoolean(ViewState["ModoEdicionProtocolar"]) == false)
                    {
                        #region ModoEdicionProtocolar_True

                        this.ddl_UbigeoPais.Enabled = true;
                        this.ddl_UbigeoRegion.Enabled = true;
                        this.ddl_UbigeoCiudad.Enabled = true;
                        this.ddl_pers_sNacionalidadId.Enabled = true;
                        this.txt_pers_vApellidoPaterno.Enabled = true;
                        this.txt_pers_vApellidoMaterno.Enabled = true;
                        this.txt_pers_vNombres.Enabled = true;
                        //-------------------------------------------------------------
                        //Fecha: 05/01/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Se adiciona el apellido de casada.
                        //-------------------------------------------------------------
                        this.txt_pers_vApellidoCasada.Enabled = true;
                        this.txtDescOtroDocumento.Enabled = true;
                        this.ddl_pers_sGeneroId.Enabled = true;
                        this.ddl_pers_sEstadoCivilId.Enabled = true;
                        this.txt_resi_vResidenciaDireccion.Enabled = true;

                        if (iPersonaID == 0)
                        {
                            this.ddl_peid_sDocumentoTipoId.Enabled = true;
                            this.txt_peid_vDocumentoNumero.Enabled = true;
                        }
                        else
                        {
                            this.ddl_pers_sProfesionId.Enabled = true;
                            this.ddl_pers_sIdiomaNatalId.Enabled = true;
                        }
                        #endregion
                    }
                    else
                    {
                        this.txtDescOtroDocumento.Enabled = true;

                        this.txt_pers_vApellidoPaterno.Enabled = true;

                        this.txt_pers_vApellidoMaterno.Enabled = true;

                        this.txt_pers_vNombres.Enabled = true;
                        //-------------------------------------------------------------
                        //Fecha: 05/01/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Se adiciona el apellido de casada.
                        //-------------------------------------------------------------
                        this.txt_pers_vApellidoCasada.Enabled = true;
                        this.ddl_pers_sGeneroId.Enabled = true;

                        this.ddl_pers_sEstadoCivilId.Enabled = true;
                        this.txt_resi_vResidenciaDireccion.Enabled = true;

                        this.ddl_UbigeoPais.Enabled = true;
                        this.ddl_UbigeoRegion.Enabled = true;
                        this.ddl_UbigeoCiudad.Enabled = true;
                    }

                    ddl_Participante_Discapacidad.Items.Clear();
                    ListItem lListItem0 = new ListItem("-- SELECCIONAR --", "0");
                    ddl_Participante_Discapacidad.Items.Add(lListItem0);
                    //lblParticipanteDiscapacidad.Visible = false;
                    //==========================================================
                    //Fecha: 20/04/2020
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Inicializar la lista desplegable de Interpretes.
                    //==========================================================
                    ddl_Participante_Interprete.Items.Clear();
                    ddl_Participante_Interprete.Items.Add(lListItem0);
                    lblParticipanteConInterprete.Visible = false;
                    ddl_Participante_Interprete.Visible = false;
                    //==========================================================

                    //if (ddl_anpa_sTipoParticipanteId.SelectedValue == Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE) ||
                    //    ddl_anpa_sTipoParticipanteId.SelectedValue == Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR) ||
                    //    ddl_anpa_sTipoParticipanteId.SelectedValue == Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE) ||
                    //    ddl_anpa_sTipoParticipanteId.SelectedValue == Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE))

                    if (ObtenerIniciaRecibe(Convert.ToInt16(ddl_anpa_sTipoParticipanteId.SelectedValue)) == "INICIA")
                    {
                        strScrip += " MostrasIncapacidad(); ";
                        strScrip += " OcularListaIncapacidad(); ";
                        strScrip += " OcularListaInterpretes(); ";
                    }
                    else
                    {
                        strScrip += " OcultarIncapacidad(); ";

                        if (ddl_anpa_sTipoParticipanteId.SelectedValue == Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO))
                        {
                            strScrip += " MostrarlistaIncapacidad(); ";

                            //---------------------------------------------------------------
                            //Fecha: 30/07/2020
                            //Autor: Miguel Márquez Beltrán
                            //Motivo: LLenar la lista desplegable con los participantes 
                            //          que tengan discapacidad.
                            //---------------------------------------------------------------
                            llenarParticipanteDiscapacidad();
                            //---------------------------------------------------------------
                            if (ddl_Participante_Discapacidad.Items.Count > 1)
                            {
                                for (int i = 0; i < ddl_Participante_Discapacidad.Items.Count; i++)
                                {
                                    if (ddl_Participante_Discapacidad.Items[i].Value == Participantes[lRowIndex].anpa_iReferenciaParticipanteId.ToString())
                                    {
                                        ddl_Participante_Discapacidad.SelectedValue = Participantes[lRowIndex].anpa_iReferenciaParticipanteId.ToString();
                                        break;
                                    }
                                }
                            }

                        }
                        else
                        {
                            strScrip += " OcularListaIncapacidad(); ";

                        }

                        #region LlenarParticipanteInterprete
                        if (ddl_anpa_sTipoParticipanteId.SelectedValue == Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE))
                        {
                            foreach (GridViewRow row in grd_Participantes.Rows)
                            {
                                string str_Participante = row.Cells[0].Text.ToString();
                                string Participante = row.Cells[4].Text.ToString();
                                string str_idioma = row.Cells[7].Text.ToString();
                                Int64 Aux = Convert.ToInt64(row.Cells[12].Text.ToString());
                                if (str_idioma != "CASTELLANO")
                                {
                                    //if (str_Participante == "OTORGANTE" || str_Participante == "VENDEDOR" || str_Participante == "ANTICIPANTE" || str_Participante == "DONANTE")

                                    if (ObtenerIniciaRecibe(str_Participante) == "INICIA")
                                    {
                                        ListItem lListItem = new ListItem(Participante, Convert.ToString(Aux));

                                        ddl_Participante_Interprete.Items.Add(lListItem);
                                        HF_IREFERENCIAINTERPRETE.Value = Convert.ToString(Aux);
                                    }
                                }
                            }

                        }
                        #endregion

                        if (ddl_Participante_Interprete.Items.Count > 1)
                        {
                            for (int i = 0; i < ddl_Participante_Interprete.Items.Count; i++)
                            {
                                if (ddl_Participante_Interprete.Items[i].Value == Participantes[lRowIndex].anpa_iReferenciaParticipanteId.ToString())
                                {
                                    ddl_Participante_Interprete.SelectedValue = Participantes[lRowIndex].anpa_iReferenciaParticipanteId.ToString();
                                    break;
                                }
                            }
                        }
                    }
                    strScrip += "}});";
                    strScrip = String.Format(strScrip);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "deshabilitarAux", strScrip, true);

                    ddl_empr_sTipoDocumentoId.Enabled = false;
                    txt_empr_vNumeroDocumento.Enabled = false;
                    txt_empr_vRazonSocial.Enabled = false;
                    //--------------------------------------------------
                    //Fecha: 26/11/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Deshabilitar Tipo de documento y 
                    //          número de documento al editar.
                    //--------------------------------------------------
                    ddl_peid_sDocumentoTipoId.Enabled = false;
                    txt_peid_vDocumentoNumero.Enabled = false;
                    //--------------------------------------------------
                    //btnAgregarParticipante.Text = "  Actualizar";
                    btnAgregarParticipanteNew.Text = "  Actualizar";
                    updDatosParticipantes.Update();
                    updParticipantesOtogarntes.Update();

                    #endregion
                    break;
                case "Eliminar":
                    #region Eliminar

                    //if (indiceParticipante < 0)
                    //{
                    //    string strScriptt = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", Constantes.CONST_MENSAJE_PERDIDA_SESSION_ACTUACION);
                    //    EjecutarScript(strScriptt);
                    //    return;
                    //}


                    string TipoParicip = grd_Participantes.Rows[lRowIndex].Cells[0].Text;
                    string TipoDcmento = grd_Participantes.Rows[lRowIndex].Cells[1].Text;
                    string NmroDcmento = grd_Participantes.Rows[lRowIndex].Cells[3].Text;

                    //if (Participantes[indiceParticipante].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE) ||
                    //    Participantes[indiceParticipante].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR) ||
                    //    Participantes[indiceParticipante].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE) ||
                    //    Participantes[indiceParticipante].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE))

                    if (ObtenerIniciaRecibe(Participantes[indiceParticipante].anpa_sTipoParticipanteId)=="INICIA")
                    {
                        #region Validación_de_Testigo_a_Ruego
                        if (Participantes.Where(x => x.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO) && x.anpa_cEstado != "E" &&
                            x.anpa_iReferenciaParticipanteId == Participantes[indiceParticipante].anpa_iActoNotarialParticipanteIdAux).Count() > 0)
                        {
                            strScrip = "";
                            //if (Participantes[indiceParticipante].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE))
                            //    strScrip = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", "No puede eliminar el otorgante porque se encuentra enlazado a un Testigo a Ruego.");

                            //if (Participantes[indiceParticipante].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR))
                            //    strScrip = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", "No puede eliminar el vendedor porque se encuentra enlazado a un Testigo a Ruego.");

                            //if (Participantes[indiceParticipante].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE))
                            //    strScrip = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", "No puede eliminar el donante porque se encuentra enlazado a un Testigo a Ruego.");

                            //if (Participantes[indiceParticipante].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE))
                            //    strScrip = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", "No puede eliminar el anticipante porque se encuentra enlazado a un Testigo a Ruego.");

                            if (ObtenerIniciaRecibe(Participantes[indiceParticipante].anpa_sTipoParticipanteId) == "INICIA")
                            {
                                string strNombreParticipante = ObtenerNombreParticipante(Participantes[indiceParticipante].anpa_sTipoParticipanteId);
                                strScrip = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", "No puede eliminar el " + strNombreParticipante + " porque se encuentra enlazado a un Testigo a Ruego.");
                            }

                            EjecutarScript(strScrip);
                            return;
                        }
                        #endregion
                        #region Validación_de_Interprete
                        if (Participantes.Where(x => x.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE) && x.anpa_cEstado != "E" &&
                           x.anpa_iReferenciaParticipanteId == Participantes[indiceParticipante].anpa_iActoNotarialParticipanteIdAux).Count() > 0)
                        {
                            strScrip = "";
                            //if (Participantes[indiceParticipante].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE))
                            //    strScrip = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", "No puede eliminar el otorgante porque se encuentra enlazado a un Interprete.");

                            //if (Participantes[indiceParticipante].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR))
                            //    strScrip = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", "No puede eliminar el vendedor porque se encuentra enlazado a un Interprete.");

                            //if (Participantes[indiceParticipante].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE))
                            //    strScrip = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", "No puede eliminar el donante porque se encuentra enlazado a un Interprete.");

                            //if (Participantes[indiceParticipante].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE))
                            //    strScrip = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", "No puede eliminar el anticipante porque se encuentra enlazado a un Interprete.");

                            if (ObtenerIniciaRecibe(Participantes[indiceParticipante].anpa_sTipoParticipanteId) == "INICIA")
                            {
                                string strNombreParticipante = ObtenerNombreParticipante(Participantes[indiceParticipante].anpa_sTipoParticipanteId);
                                strScrip = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", "No puede eliminar el " + strNombreParticipante + " porque se encuentra enlazado a un Interprete.");
                            }
                            EjecutarScript(strScrip);
                            return;
                        }
                        #endregion
                    }

                    List<CBE_PARTICIPANTE> loEliminar = new List<CBE_PARTICIPANTE>();

                    foreach (CBE_PARTICIPANTE p in Participantes.Where(p => p.anpa_cEstado != "E"))
                    {
                        if ((p.acpa_sTipoParticipanteId_desc.ToString() == TipoParicip) &&
                            (p.peid_sDocumentoTipoId_desc.ToString() == TipoDcmento) &&
                            (p.Identificacion.peid_vDocumentoNumero.ToString() == NmroDcmento)
                            )
                        {
                            if (p.anpa_iActoNotarialParticipanteId != 0)
                            {
                                p.anpa_cEstado = "E";
                            }
                            else
                            {
                                loEliminar.Add(p);
                            }
                        }
                    }

                    foreach (CBE_PARTICIPANTE p in loEliminar)
                    {
                        Participantes.Remove(p);
                    }

                    //-------------------------------------------------
                    //Fecha: 24/07/2020
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Eliminar al participante.
                    //-------------------------------------------------
                    for (int i = 0; i < Participantes.Count; i++)
                    {
                        if (Participantes[i].anpa_cEstado == "E")
                        {
                            //----------------------------------------

                            RE_ACTONOTARIALPARTICIPANTE ParticipanteBE = new RE_ACTONOTARIALPARTICIPANTE();
                            ParticipanteBE.anpa_iActoNotarialParticipanteId = Participantes[i].anpa_iActoNotarialParticipanteId;
                            ParticipanteBE.anpa_iActoNotarialId = Participantes[i].anpa_iActoNotarialId;
                            ParticipanteBE.anpa_iPersonaId = Participantes[i].Persona.pers_iPersonaId;
                            ParticipanteBE.anpa_cEstado = "E";
                            ParticipanteBE.anpa_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                            ParticipanteBE.anpa_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP(); ;
                            ParticipanteBE.anpa_dFechaModificacion = DateTime.Today;
                            ParticipanteBE.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                            ParticipanteBE.HostName = Util.ObtenerHostName();


                            ActoNotarialMantenimiento BL = new ActoNotarialMantenimiento();
                            BL.AnularParticipante(ParticipanteBE); 
                            //----------------------------------------

                            Participantes.RemoveAt(i);
                        }
                    }
                    //-----------------------------------------------------------------------------------
                    //Fecha: 24/09/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Si existe un extranjero se debe activar RichTextBoxDL1049Art55IncisoC
                    //-----------------------------------------------------------------------------------

                    bool bExisteExtranjero = false;

                    for (int i = 0; i < Participantes.Count; i++)
                    {
                        //if (Participantes[i].anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE) ||
                        //    Participantes[i].anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR) ||
                        //    Participantes[i].anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE) ||
                        //    Participantes[i].anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE))

                        if (ObtenerIniciaRecibe(Participantes[i].anpa_sTipoParticipanteId) == "INICIA")
                        {
                            if (Participantes[i].Persona.vNacionalidad != "PERUANA")
                            {
                                bExisteExtranjero = true;
                                break;
                            }
                        }
                    }
                    if (bExisteExtranjero == true)
                    {
                        tablaIncisoCArticulo55.Style.Add("display", "block");
                    }
                    else
                    {
                        tablaIncisoCArticulo55.Style.Add("display", "none");
                    }
                    updIncisoCArticulo55.Update();
                    //-----------------------------------------------------
                    //Fecha: 24/09/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Activar Cmb_TipoActoNotarial cuando 
                    //        no existan participantes.
                    //-----------------------------------------------------
                    string strScript2 = string.Empty;
                    if (Participantes.Count == 0)
                    {
                        Cmb_TipoActoNotarial.Enabled = true;
                        strScript2 = "$('#btnCncl_tab1').attr('disabled',false);Habilitar_Tab(0);";
                    }
                    
                    updTipoActoNotarial.Update();
                    //-----------------------------------------------------
                    
                    updParticipantesOtogarntes.Update();
                    Session["ParticipanteContainer"] = (List<CBE_PARTICIPANTE>)Participantes;

                    DataTable dtParticipantes = new DataTable();

                    dtParticipantes = CrearTablaParticipante(null);

                    this.grd_Participantes.DataSource = dtParticipantes;
                    this.grd_Participantes.DataBind();

                    this.grd_Otorgantes.DataSource = ObtenerOtorgantesOrdenados(dtParticipantes);
                    this.grd_Otorgantes.DataBind();
                    updGrillaOtorgantes.Update();

                    ViewState["ActoNotarialParticipanteId"] = "0";
                    ViewState["ExtraIndiceEdicion"] = -1;
                    hdn_IndiceGrillaParticipantesEdicion.Value = "0";

                    strScript2 =strScript2+ @"$(function(){{tab_02_Limpiar();}});";
                    strScript2 = string.Format(strScript2);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "deshabilitarAux", strScript2, true);

                    #endregion
                    break;
            }
        }
        protected void grd_Presentantes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 lRowIndex = Convert.ToInt32(e.CommandArgument);
            List<CBE_PARTICIPANTE> Participantes = (List<CBE_PARTICIPANTE>)Session["ParticipanteContainer"];

            if (Participantes == null)
            { return; }

            if (Participantes.Count == 0)
            { return; }

            Participantes = Participantes.Where(x => x.anpa_cEstado == "A").ToList();
            int indiceParticipante = lRowIndex;
            switch (e.CommandName.ToString())
            {
                //case "Editar":
                //    #region Editar
                //    string tipoParticipante = GridViewPresentante.Rows[lRowIndex].Cells[1].Text;
                //    if (tipoParticipante == "2")
                //    { //solo para otros
                //        rbOtros.Checked = true;
                //        HF_presentanteId.Value = GridViewPresentante.Rows[lRowIndex].Cells[0].Text;
                //        txtRepresentanteNombres.Text = GridViewPresentante.Rows[lRowIndex].Cells[3].Text;
                //        ddl_TipoDocrepresentante.SelectedValue = GridViewPresentante.Rows[lRowIndex].Cells[4].Text;
                //        txtRepresentanteNroDoc.Text = GridViewPresentante.Rows[lRowIndex].Cells[6].Text;
                //        ddl_GerenoPresentante.SelectedValue = GridViewPresentante.Rows[lRowIndex].Cells[7].Text;
                //        updParte.Update();
                //    }
                    
                //    #endregion
                //    break;
                case "Eliminar":
                    #region Eliminar

                    //if (indiceParticipante < 0)
                    //{
                    //    string strScriptt = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", Constantes.CONST_MENSAJE_PERDIDA_SESSION_ACTUACION);
                    //    EjecutarScript(strScriptt);
                    //    return;
                    //}
                    CBE_PRESENTANTE b= new CBE_PRESENTANTE();
                    //Int32 lRowIndex = Convert.ToInt32(e.CommandArgument);
                    b.anpr_iActoNotarialPresentanteId = Convert.ToInt64(GridViewPresentante.Rows[lRowIndex].Cells[0].Text);
                    b.anpr_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
                    b.anpr_sUsuario= Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                    b.anpr_vIP = SGAC.Accesorios.Util.ObtenerDireccionIP();
                    ActoNotarialMantenimiento BL = new ActoNotarialMantenimiento();
                    Int64 result = BL.AnularPresentante(b);
                    if (result == 1)
                    {
                        //EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PRESENTANTE", "Se eliminó correctamente."), "eliminacionPresentante");
                        //-----presentantes
                        CBE_PRESENTANTE lp = new CBE_PRESENTANTE();
                        ParticipanteConsultaBL lParticipanteConsultaBL = new ParticipanteConsultaBL();
                        List<CBE_PRESENTANTE> lPresentante = lParticipanteConsultaBL.listaPresentante(b);
                        Session["PresentanteContainer"] = lPresentante;
                        this.GridViewPresentante.DataSource = lPresentante;
                        this.GridViewPresentante.DataBind();
                        UpdateGridPresentantes.Update();

                        HF_presentanteId.Value = "0";
                        ddl_TipoDocrepresentante.SelectedIndex = 0;
                        txtRepresentanteNroDoc.Text = "";
                        txtRepresentanteNombres.Text = "";
                        ddl_GerenoPresentante.SelectedIndex = 0;
                        updParte.Update();
                    }
                    else
                    {
                        EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PRESENTANTE", "No es posible eliminar el participante"), "eliminacionPresentante");
                    }
                    #endregion
                    break;
            }
        }

        [System.Web.Services.WebMethod]
        public static string insert_participante(string participante, string TipoActaProtocolar)
        {

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var jsonObject = serializer.Deserialize<dynamic>(participante);

            String TipoActaProtocar = serializer.Deserialize<dynamic>(TipoActaProtocolar);


            RE_ACTONOTARIALPARTICIPANTE lPARTICIPANTE = new RE_ACTONOTARIALPARTICIPANTE();
            lPARTICIPANTE.anpa_iActoNotarialId = Convert.ToInt64(jsonObject["anpa_iActoNotarialId"]);

            lPARTICIPANTE.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            lPARTICIPANTE.anpa_sUsuarioCreacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            lPARTICIPANTE.anpa_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();

            lPARTICIPANTE.anpa_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            lPARTICIPANTE.anpa_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();

            String sMensaje = String.Empty;
            ActoNotarialMantenimiento mnt = new ActoNotarialMantenimiento();

            if (Convert.ToInt64(lPARTICIPANTE.anpa_iActoNotarialId) != 0)
            {
                List<CBE_PARTICIPANTE> ParticipantesContainer = new List<CBE_PARTICIPANTE>();

                List<CBE_PARTICIPANTE> ParticipantesContainerActualizar = new List<CBE_PARTICIPANTE>();
                List<CBE_PARTICIPANTE> ParticipantesContainerInsertar = new List<CBE_PARTICIPANTE>();
                List<CBE_PARTICIPANTE> lParticipantes = (List<CBE_PARTICIPANTE>)HttpContext.Current.Session["ParticipanteContainer"];

                //if (lParticipantes.Where(x => (x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE) ||
                //    x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR) ||
                //    x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE))
                //    && x.anpa_cEstado != "E").Count() == 0)

                if (lParticipantes.Where(x => (ObtenerIniciaRecibe(x.anpa_sTipoParticipanteId) == "INICIA")
                    && x.anpa_cEstado != "E").Count() == 0)
               
                {                    
                    return "Debe existir al menos un tipo de participante que INICIE el Acto Protocolar.";
                }
                else
                {
                    //if (lParticipantes.Where(x => x.Persona.pers_bIncapacidadFlag == true &&
                    //    (x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE) ||
                    //    x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR) ||
                    //    x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE))
                    //    && x.anpa_cEstado != "E").Count() >= 1)

                    if (lParticipantes.Where(x => x.Persona.pers_bIncapacidadFlag == true &&
                        (ObtenerIniciaRecibe(x.anpa_sTipoParticipanteId) == "INICIA")
                        && x.anpa_cEstado != "E").Count() >= 1)
                    {

                        if (lParticipantes.Where(x => x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO) && x.anpa_cEstado != "E").Count() == 0)
                        {
                            return "Debe existir al menos un tipo de participante TESTIGO A RUEGO.";
                        }
                    }
                }


                //------------------------------------------------------------------------
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 31/08/2016
                // Objetivo: Validar que el idioma del otorgante sea el castellano
                //           sino es así, deberá incluir un interprete.
                //------------------------------------------------------------------------
                if (ValidarIdiomaCastellanoOtorgante(lParticipantes) == false)
                {
                    return "Debe existir al menos un tipo de participante INTERPRETE.";
                }

                //if (lParticipantes.Where(x => (x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.APODERADO) ||
                //    x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.COMPRADOR) ||
                //    x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPADO))
                //    && x.anpa_cEstado != "E").Count() == 0)

                if (lParticipantes.Where(x => (ObtenerIniciaRecibe(x.anpa_sTipoParticipanteId) == "RECIBE")
                    && x.anpa_cEstado != "E").Count() == 0)
                {
                    //if (TipoActaProtocar == Convert.ToString(Convert.ToInt32(Enumerador.enmProtocolarTipo.COMPRA_VENTA)))
                    //    return "Debe existir al menos un tipo de participante COMPRADOR.";

                    //if (TipoActaProtocar == Convert.ToString(Convert.ToInt32(Enumerador.enmProtocolarTipo.ANTICIPO_HERENCIA)))
                    //    return "Debe existir al menos un tipo de participante ANTICIPANTE.";


                    return "Debe existir al menos un tipo de participante que RECIBE en el Acto Protocolar.";
                }



                var lCount = lParticipantes.Where(p => p.anpa_iActoNotarialParticipanteId == 0).ToList();
                if (lCount.Count == 0)
                {
                    #region Actualizando Participante

                    foreach (CBE_PARTICIPANTE lItem in lParticipantes.Where(p => p.anpa_iActoNotarialParticipanteId != 0).OrderByDescending(s => s.anpa_sTipoParticipanteId).ToList())
                    {
                        lItem.anpa_sUsuarioModificacion = lPARTICIPANTE.anpa_sUsuarioModificacion;
                        lItem.anpa_vIPModificacion = lPARTICIPANTE.anpa_vIPModificacion;
                        lItem.anpa_dFechaModificacion = DateTime.Now;
                        lItem.OficinaConsultar = lPARTICIPANTE.OficinaConsultar;
                        ParticipantesContainer.Add(lItem);
                    }
                    //-------------------------------------------------
                    //Fecha: 07/09/2017
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Validar el contenedor de participantes
                    //-------------------------------------------------
                    if (ParticipantesContainer.Count > 0)
                    {
                        mnt.ActualizarActoNotarialParticipante(ParticipantesContainer, ref sMensaje);
                    }
                    #endregion
                }
                else
                {
                    #region Insertando Participante


                    foreach (CBE_PARTICIPANTE lItem in lParticipantes.Where(p => p.anpa_iActoNotarialParticipanteId != 0).OrderByDescending(s => s.anpa_sTipoParticipanteId).ToList())
                    {
                        lItem.anpa_sUsuarioModificacion = lPARTICIPANTE.anpa_sUsuarioModificacion;
                        lItem.anpa_vIPModificacion = lPARTICIPANTE.anpa_vIPModificacion;
                        lItem.anpa_dFechaModificacion = DateTime.Now;
                        lItem.OficinaConsultar = lPARTICIPANTE.OficinaConsultar;
                        ParticipantesContainerActualizar.Add(lItem);

                    }

                    //-------------------------------------------------
                    //Fecha: 07/09/2017
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Validar el contenedor de participantes
                    //-------------------------------------------------
                    if (ParticipantesContainerActualizar.Count > 0)
                    {
                        mnt.ActualizarActoNotarialParticipante(ParticipantesContainerActualizar, ref sMensaje);
                    }

                    foreach (CBE_PARTICIPANTE lItem in (lParticipantes.Where(p => p.anpa_iActoNotarialParticipanteId == 0).OrderByDescending(s => s.anpa_sTipoParticipanteId).ToList()))
                    {
                        if (lItem.Persona.pers_iPersonaId == 0)
                        {
                            lItem.Persona.pers_sUsuarioCreacion = lPARTICIPANTE.anpa_sUsuarioCreacion;
                            lItem.Persona.pers_vIPCreacion = lPARTICIPANTE.anpa_vIPCreacion;
                            lItem.Persona.HostName = "";
                        }

                        lItem.anpa_vIPCreacion = lPARTICIPANTE.anpa_vIPCreacion;
                        lItem.anpa_sUsuarioCreacion = lPARTICIPANTE.anpa_sUsuarioCreacion;
                        lItem.anpa_dFechaCreacion = DateTime.Now;
                        lItem.anpa_dFechaModificacion = DateTime.Now;
                        lItem.anpa_iEmpresaId = lItem.Empresa.empr_iEmpresaId;
                        lItem.OficinaConsultar = lPARTICIPANTE.OficinaConsultar;

                        ParticipantesContainerInsertar.Add(lItem);
                    }

                    //-------------------------------------------------
                    //Fecha: 07/09/2017
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Validar el contenedor de participantes
                    //-------------------------------------------------
                    if (ParticipantesContainerInsertar.Count > 0)
                    {
                        mnt.InsertarActoNotarialParticipante_Persona(ParticipantesContainerInsertar, ref sMensaje);
                    }

                    #endregion
                }

            }


            if (sMensaje != String.Empty)
                return "OK";
            else
                return sMensaje;
        }

   

        private DataTable CrearTablaParticipante(CBE_PARTICIPANTE participante)
        {
            List<CBE_PARTICIPANTE> ParametrosContainer = (List<CBE_PARTICIPANTE>)Session["ParticipanteContainer"];
            if (participante != null)
            {
                if (ViewState["ActoNotarialParticipanteId"].ToString() != "0" && ViewState["ActoNotarialParticipanteId"].ToString() != string.Empty)
                    participante.anpa_iActoNotarialParticipanteId = Convert.ToInt64(ViewState["ActoNotarialParticipanteId"].ToString());


                if (participante != null)
                {
                    ParametrosContainer.Add(participante);
                }
            }

            //----------------------------------------------------------------
            //Fecha: 25/09/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Asignar la Fecha de extensión del Acto Notarial
            //          a la fecha de suscripcíón del participante Otorgante
            //----------------------------------------------------------------
            for (int i = 0; i < ParametrosContainer.Count; i++)
            {

                //if (ParametrosContainer[i].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE)
                //|| ParametrosContainer[i].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR)
                //|| ParametrosContainer[i].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE)
                //|| ParametrosContainer[i].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE)
                //|| ParametrosContainer[i].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO)
                //|| ParametrosContainer[i].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE))

                if (ObtenerIniciaRecibe(ParametrosContainer[i].anpa_sTipoParticipanteId) == "INICIA"
                || ParametrosContainer[i].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO)
                || ParametrosContainer[i].anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE))
                {
                    if (ParametrosContainer[i].anpa_dFechaSuscripcion == null || ParametrosContainer[i].anpa_dFechaSuscripcion == DateTime.MinValue)
                    {
                        if (hdn_acno_dFechaExtension.Value.Length > 0)
                        {
                            ParametrosContainer[i].anpa_dFechaSuscripcion = Comun.FormatearFecha(hdn_acno_dFechaExtension.Value);
                        }
                    }
                }
            }

            //---------------------------------------------------            

            Session["ParticipanteContainer"] = (List<CBE_PARTICIPANTE>)ParametrosContainer;

            #region creando datatable
            DataTable dt = new DataTable();
            dt.Columns.Add("anpa_iActoNotarialParticipanteId", typeof(string));
            dt.Columns.Add("acpa_sTipoParticipanteId_desc", typeof(string));
            dt.Columns.Add("peid_sDocumentoTipoId_desc", typeof(string));
            dt.Columns.Add("peid_sDocumentoTipoId", typeof(string));
            dt.Columns.Add("peid_vDocumentoNumero", typeof(string));
            dt.Columns.Add("participante", typeof(string));
            dt.Columns.Add("pers_sNacionalidadId_desc", typeof(string));
            dt.Columns.Add("pers_bIncapacidadFlag", typeof(Boolean));
            dt.Columns.Add("anpa_iActoNotarialParticipanteIdAux", typeof(Int64));
            dt.Columns.Add("anpa_sReferenciaParticipanteId", typeof(Int64));
            dt.Columns.Add("anpa_sTipoParticipanteId", typeof(Int16));
            //-------------------------------------------------------------------
            //Fecha: 09/07/2020
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Asignar la columna Nacionalidad del participante
            //-------------------------------------------------------------------
            dt.Columns.Add("vNacionalidad", typeof(string));
            dt.Columns.Add("pers_sIdiomaNatalId_desc", typeof(string));

            //-------------------------------------------------------------------
            //Fecha: 20/09/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Asignar la columna Fecha de suscripción
            //-------------------------------------------------------------------
            dt.Columns.Add("anpa_dFechaSuscripcion", typeof(DateTime));

            //-------------------------------------------------------------------
            //Fecha: 30/07/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Adicionar la descripción de Incapacidad.
            //-------------------------------------------------------------------
            dt.Columns.Add("pers_vDescripcionIncapacidad", typeof(string));
            
            dt.Columns.Add("anpa_cEstado", typeof(string));

            //-----------------------------------------------------------------
            //Fecha: 01/12/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Mostrar la dirección y el ubigeo.
            //-----------------------------------------------------------------
            dt.Columns.Add("resi_vResidenciaDireccion", typeof(string));
            dt.Columns.Add("resi_cResidenciaUbigeo", typeof(string));
            //-----------------------------------------------------------------

            #endregion

            #region pasando a datatable
            bool bExisteParticipante = false;

            foreach (CBE_PARTICIPANTE p in ParametrosContainer.Where(p => p.anpa_cEstado != "E"))
            {
                if (ExisteParticipante(dt, p.Identificacion.peid_sDocumentoTipoId.ToString(),p.Identificacion.peid_vDocumentoNumero.ToString()))
                {
                    bExisteParticipante = true;
                }
                else
                {
                    bExisteParticipante = false;
                }
                if (bExisteParticipante == false)
                {
                #region AdicionarParticipanteSiNoExiste
                    DataRow lDataRow = dt.NewRow();
                    lDataRow["anpa_iActoNotarialParticipanteId"] = p.anpa_iActoNotarialParticipanteId.ToString();
                    lDataRow["acpa_sTipoParticipanteId_desc"] = p.acpa_sTipoParticipanteId_desc.ToString();
                    lDataRow["peid_sDocumentoTipoId_desc"] = p.peid_sDocumentoTipoId_desc.ToString();
                    lDataRow["peid_sDocumentoTipoId"] = p.Identificacion.peid_sDocumentoTipoId.ToString();
                    lDataRow["peid_vDocumentoNumero"] = p.Identificacion.peid_vDocumentoNumero.ToString();
                    lDataRow["participante"] = p.Persona.pers_vApellidoPaterno.ToString() + " " + p.Persona.pers_vApellidoMaterno.ToString() + ", " + p.Persona.pers_vNombres.ToString();
                    lDataRow["pers_sNacionalidadId_desc"] = (p.Persona.pers_sNacionalidadId == 0) ? "" : p.pers_sNacionalidadId_desc.ToString();
                    lDataRow["pers_bIncapacidadFlag"] = (p.Persona.pers_bIncapacidadFlag) ? true : false;
                    lDataRow["anpa_iActoNotarialParticipanteIdAux"] = (p.anpa_iActoNotarialParticipanteId.ToString() != "0") ? p.anpa_iActoNotarialParticipanteId.ToString() : p.anpa_iActoNotarialParticipanteIdAux.ToString();
                    lDataRow["anpa_sReferenciaParticipanteId"] = (p.anpa_iReferenciaParticipanteId != null) ? p.anpa_iReferenciaParticipanteId.ToString() : "0";

                    lDataRow["anpa_sTipoParticipanteId"] = (p.anpa_sTipoParticipanteId.ToString() != "0") ? p.anpa_sTipoParticipanteId.ToString() : p.anpa_sTipoParticipanteId.ToString();
                    //-------------------------------------------------------------------
                    //Fecha: 09/07/2020
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Asignar la columna Nacionalidad del participante
                    //-------------------------------------------------------------------
                    lDataRow["vNacionalidad"] = (p.Persona.vNacionalidad != null) ? p.Persona.vNacionalidad.ToString().ToUpper() : "";
                    lDataRow["pers_sIdiomaNatalId_desc"] = (p.pers_sIdiomaNatalId_desc != null) ? p.pers_sIdiomaNatalId_desc.ToString().ToUpper() : "";

                    //-------------------------------------------------------------------
                    //Fecha: 20/09/2017
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Asignar la columna Fecha de suscripción
                    //-------------------------------------------------------------------

                    if (p.anpa_dFechaSuscripcion == null || p.anpa_dFechaSuscripcion == DateTime.MinValue)
                    {
                        if (hdn_acno_dFechaExtension.Value.Length > 0)
                            lDataRow["anpa_dFechaSuscripcion"] = hdn_acno_dFechaExtension.Value;
                    }
                    else
                    {
                        lDataRow["anpa_dFechaSuscripcion"] = p.anpa_dFechaSuscripcion.ToString();
                    }
                    //-------------------------------------------------------------------
                    //Fecha: 30/07/2020
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Adicionar la descripción de Incapacidad.
                    //-------------------------------------------------------------------
                    lDataRow["pers_vDescripcionIncapacidad"] = p.Persona.pers_vDescripcionIncapacidad;

                    lDataRow["anpa_cEstado"] = p.anpa_cEstado;

                    //-----------------------------------------------------------------
                    //Fecha: 01/12/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Mostrar la dirección y el ubigeo.
                    //-----------------------------------------------------------------
                    lDataRow["resi_vResidenciaDireccion"] = p.Residencia.resi_vResidenciaDireccion;
                    lDataRow["resi_cResidenciaUbigeo"] = p.Residencia.resi_cResidenciaUbigeo;
                    //-----------------------------------------------------------------


                    dt.Rows.Add(lDataRow);
                    #endregion
                }
            }
            #endregion

            return dt;
        }

        [System.Web.Services.WebMethod]
        public static string[] Retornar_persona(Int64 iPersona)
        {
            string s_Datos = string.Empty;

            var s_Persona = new SGAC.Registro.Persona.BL.PersonaConsultaBL().PersonaGetById(iPersona);
            if (s_Persona.Rows.Count > 0)
            {
                DataTable dtTipoDocumento = new DataTable();
                dtTipoDocumento = comun_Part1.ObtenerDocumentoIdentidad();
                bool bExisteDocumento = false;
                string strTipoDocumentoId = Convert.ToString(s_Persona.Rows[0]["sDocumentoTipoId"]);
                for (int i = 0; i < dtTipoDocumento.Rows.Count; i++)
                {
                    if (dtTipoDocumento.Rows[i]["Id"].ToString().Equals(strTipoDocumentoId))
                    {
                        bExisteDocumento = true;
                        break;
                    }
                }
                if (bExisteDocumento)
                {
                    s_Datos = Convert.ToString(s_Persona.Rows[0]["vNroDocumento"]) + "|" + Convert.ToString(s_Persona.Rows[0]["sDocumentoTipoId"]) + "|" + Convert.ToString(s_Persona.Rows[0]["iPersonaId"]);
                }
                else
                {
                    s_Datos = Convert.ToString(s_Persona.Rows[0]["vNroDocumento"]) + "|0|" + Convert.ToString(s_Persona.Rows[0]["iPersonaId"]);
                }
            }

            return s_Datos.Split('|');
        }

        [System.Web.Services.WebMethod]
        public static string obtener_empresa(string tipodocumento, string documento)
        {
            RE_EMPRESA lEmpresa = new RE_EMPRESA(Convert.ToInt16(tipodocumento), documento);
            EmpresaConsultaBL mnt = new EmpresaConsultaBL();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(mnt.Obtener(lEmpresa)).ToString();
        }

        [System.Web.Services.WebMethod]
        public static string obtener_representeslegales(string idempresa)
        {
            RepresentanteLegalConsultaBL lRepresentanteLegalConsultaBL = new RepresentanteLegalConsultaBL();
            RE_EMPRESA lEmpresa = new RE_EMPRESA();
            lEmpresa.empr_iEmpresaId = Convert.ToInt64(idempresa);
            lRepresentanteLegalConsultaBL.listado(lEmpresa);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(lRepresentanteLegalConsultaBL.listado(lEmpresa).Where(p => p.rele_cEstado != "E")).ToString();
        }

        [System.Web.Services.WebMethod]
        public static string obtener_persona(string idpersona, string tipodocumento, string documento, string ActoNotarialId)
        {
            RE_PERSONA lPersona = new RE_PERSONA();

            RE_PERSONAIDENTIFICACION lPersonaIdentificacion = new RE_PERSONAIDENTIFICACION();

            #region Identificación
            //PersonaIdentificacionConsultaBL lPersonaIdentificacionConsultaBL = new PersonaIdentificacionConsultaBL();
            if (idpersona != null) lPersonaIdentificacion.peid_iPersonaId = Convert.ToInt16(idpersona);
            if (tipodocumento != null) lPersonaIdentificacion.peid_sDocumentoTipoId = Convert.ToInt16(tipodocumento);
            if (documento != null) lPersonaIdentificacion.peid_vDocumentoNumero = documento.ToString();
            //lPersonaIdentificacion = lPersonaIdentificacionConsultaBL.Obtener(lPersonaIdentificacion);
            #endregion

            #region Verificar si existe Persona
            //if (lPersonaIdentificacion.peid_iPersonaId != 0)
            //{
            PersonaConsultaBL lPersonaConsultaBL = new PersonaConsultaBL();
            lPersona.pers_iPersonaId = lPersonaIdentificacion.peid_iPersonaId;
            lPersona.Identificacion = lPersonaIdentificacion;
            lPersona = lPersonaConsultaBL.Obtener(lPersona);

            //-----------------------------------------------
            //Fecha: 07/07/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Asignar la Nacionalidad y el País.
            //-----------------------------------------------
            lPersona.vNacionalidad = "";
            lPersona.pers_sPaisId = 0;
            if (lPersona.pers_iPersonaId > 0)
            {
                DataTable dtNacionalidad = new DataTable();
                dtNacionalidad = lPersonaConsultaBL.PersonaListarUltimaNacionalidad(lPersona.pers_iPersonaId);
                if (dtNacionalidad.Rows.Count > 0)
                {
                    lPersona.vNacionalidad = dtNacionalidad.Rows[0]["pena_vNacionalidad"].ToString();
                    lPersona.pers_sPaisId = Convert.ToInt16(dtNacionalidad.Rows[0]["pena_sPais"].ToString());
                }
            }
            //-----------------------------------------------
            //lPersona.Identificacion = lPersonaIdentificacion;            
            //}
            //-----------------------------------------------------------------
            //Fecha: 03/12/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Consultar el idioma diferente al castellano
            //          del primer participante.
            //------------------------------------------------

            Int64 intActoNotarialId = 0;

            if (ActoNotarialId.Length > 0)
            { 
                intActoNotarialId = Convert.ToInt64(ActoNotarialId);
                RE_ACTONOTARIALPARTICIPANTE participanteBE = new RE_ACTONOTARIALPARTICIPANTE();
                ActoNotarialConsultaBL ActoNotarialBL = new ActoNotarialConsultaBL();
                participanteBE = ActoNotarialBL.ObtenerIdiomaPrimerParticipanteOtorgante(intActoNotarialId);
                if (participanteBE.Error == false)
                {
                    if (participanteBE.IdiomaId > 0)
                    {
                        lPersona.pers_sIdiomaNatalId = participanteBE.IdiomaId;
                    }
                }
            }
            //-----------------------------------------------------------------------------
            #endregion

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(lPersona).ToString();
        }

        [System.Web.Services.WebMethod]
        public static string obtener_provincias(string ubigeo)
        {
            SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
            DataTable lDataTable = objBL.Consultar(ubigeo, null, "01");
            DataView lDataView = lDataTable.DefaultView;
            lDataView.Sort = "ubge_vProvincia  ASC";

            List<CBE_DROPDOWNLIST> loProvincias = (lDataView.ToTable(true, "ubge_vProvincia", "ubge_cUbi02").AsEnumerable().Select( x => new CBE_DROPDOWNLIST { ValueField = x.ItemArray[1].ToString(), TextField = x.ItemArray[0].ToString() })).ToList();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(loProvincias).ToString();
        }

        [System.Web.Services.WebMethod]
        public static string obtener_distritos(string departamento, string provincia)
        {
            SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
            DataTable lDataTable = objBL.Consultar(departamento, provincia, null);
            DataView lDataView = lDataTable.DefaultView;

            lDataView.Sort = "ubge_vDistrito  ASC";

            List<CBE_DROPDOWNLIST> loDistritos = (lDataView.ToTable(true, "ubge_vDistrito", "ubge_cUbi03").AsEnumerable().Select(
                                   x => new CBE_DROPDOWNLIST { ValueField = x.ItemArray[1].ToString(), TextField = x.ItemArray[0].ToString() })
                                   ).ToList();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(loDistritos).ToString();
        }

        private void Habilitar_Tab(int s_Estado)
        {
            string script = @"$(function(){{";
            script += "pageLoadedHandler();}});";
            script = string.Format(script);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "Habilitar_Tab", script, true);
        }

        [System.Web.Services.WebMethod]
        public static string ValidarParticipanteTestigoRuego(Int32 sTipoParticipanteId, Int32 sTipoActoNotarialId)
        {
            try
            {

                List<CBE_PARTICIPANTE> lParticipantes = (List<CBE_PARTICIPANTE>)HttpContext.Current.Session["ParticipanteContainer"];
                if (sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO))
                {
                    //-------------------------------------------------                    
                    // Fecha: 05/09/2017
                    // Autor: Miguel Márquez Beltrán
                    // Objetivo: Obtener el tipo de acto notarial
                    //          y validar el testigo a ruego por 
                    //          tipo de acto notarial.
                    //-------------------------------------------------                    

                    //if (sTipoActoNotarialId == (int)Enumerador.enmProtocolarTipo.COMPRA_VENTA)
                    //{
                    //    if (lParticipantes.Where(x => x.Persona.pers_bIncapacidadFlag == true && x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR) && x.anpa_cEstado != "E").Count() == 0)
                    //        return "No se ha registrado un Vendedor con incapacidad de firmar.";
                    //}
                    //else if (sTipoActoNotarialId == (int)Enumerador.enmProtocolarTipo.ANTICIPO_HERENCIA)
                    //{
                    //    if (lParticipantes.Where(x => x.Persona.pers_bIncapacidadFlag == true && x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE) && x.anpa_cEstado != "E").Count() == 0)
                    //        return "No se ha registrado un Anticipante con incapacidad de firmar.";
                    //}
                    //else if (sTipoActoNotarialId == (int)Enumerador.enmProtocolarTipo.DONACION)
                    //{
                    //    if (lParticipantes.Where(x => x.Persona.pers_bIncapacidadFlag == true && x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE) && x.anpa_cEstado != "E").Count() == 0)
                    //        return "No se ha registrado un Donante con incapacidad de firmar.";
                    //}
                    //else
                    //{
                    //    if (lParticipantes.Where(x => x.Persona.pers_bIncapacidadFlag == true && x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE) && x.anpa_cEstado != "E").Count() == 0)
                    //        return "No se ha registrado un Otorgante con incapacidad de firmar.";
                    //}

                    if (lParticipantes.Where(x => x.Persona.pers_bIncapacidadFlag == true && ObtenerIniciaRecibe(x.anpa_sTipoParticipanteId) == "INICIA" && x.anpa_cEstado != "E").Count() == 0)
                            return "No se ha registrado un participante que INICIA el Acto Protocolar con incapacidad para firmar.";
                    //-------------------------------------------------                    
                }
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion

        [System.Web.Services.WebMethod]
        public static string ValidarParticipanteInterprete(Int32 sTipoParticipanteId, Int32 sTipoActoNotarialId)
        {
            try
            {

                List<CBE_PARTICIPANTE> lParticipantes = (List<CBE_PARTICIPANTE>)HttpContext.Current.Session["ParticipanteContainer"];
                if (sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE))
                {

                    //if (sTipoActoNotarialId == (int)Enumerador.enmProtocolarTipo.COMPRA_VENTA)
                    //{
                    //    if (lParticipantes.Where(x => x.pers_sIdiomaNatalId_desc != "CASTELLANO" && x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR) && x.anpa_cEstado != "E").Count() == 0)
                    //        return "No se ha registrado un Vendedor que requiera un Interprete.";
                    //}
                    //else if (sTipoActoNotarialId == (int)Enumerador.enmProtocolarTipo.ANTICIPO_HERENCIA)
                    //{
                    //    if (lParticipantes.Where(x => x.pers_sIdiomaNatalId_desc != "CASTELLANO" && x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE) && x.anpa_cEstado != "E").Count() == 0)
                    //        return "No se ha registrado un Anticipante que requiera un Interprete.";
                    //}
                    //else
                    //{
                    //    if (lParticipantes.Where(x => x.pers_sIdiomaNatalId_desc != "CASTELLANO" && x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE) && x.anpa_cEstado != "E").Count() == 0)
                    //        return "No se ha registrado un Otorgante que requiera un Interprete.";
                    //}
                    if (lParticipantes.Where(x => x.pers_sIdiomaNatalId_desc != "CASTELLANO" && ObtenerIniciaRecibe(x.anpa_sTipoParticipanteId) == "INICIA" && x.anpa_cEstado != "E").Count() == 0)
                        return "No se ha registrado un participante que INICIA el Acto Protocolar que requiera un Interprete.";

                    //-------------------------------------------------                    
                }
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        #region Pestaña: Cuerpo
        [System.Web.Services.WebMethod]
        public static string insert_cuerpo(string cuerpo)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var jsonObject = serializer.Deserialize<dynamic>(cuerpo);

            CBE_CUERPO lCuerpo = new CBE_CUERPO();
            #region Creando objeto ....
            lCuerpo.ancu_iActoNotarialCuerpoId = Convert.ToInt64(jsonObject["ancu_iActoNotarialCuerpoId"]);
            lCuerpo.ancu_iActoNotarialId = Convert.ToInt64(jsonObject["ancu_iActoNotarialId"]);

          
            //------------------------------------------------------

            lCuerpo.ancu_vCuerpo = Convert.ToString(jsonObject["ancu_vCuerpo"]);

            lCuerpo.ancu_vTextoCentral = Convert.ToString(jsonObject["ancu_vTextoCentral"]);

            lCuerpo.ancu_vTextoAdicional = Convert.ToString(jsonObject["ancu_vTextoAdicional"]);
            lCuerpo.ancu_vTextoNormativo = Convert.ToString(jsonObject["ancu_vTextoNormativo"]);
            lCuerpo.ancu_vDL1049Articulo55C = Convert.ToString(jsonObject["ancu_vDL1049Articulo55C"]);

            lCuerpo.ancu_vFirmaIlegible = Convert.ToString(jsonObject["ancu_vFirmaIlegible"]);

            lCuerpo.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            lCuerpo.ancu_bFlagExtraprotocolarCuerpo = false;

            lCuerpo.ancu_sUsuarioCreacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            lCuerpo.ancu_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();

            lCuerpo.ancu_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            lCuerpo.ancu_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();

            lCuerpo.ActoNotarial.acno_iActoNotarialId = lCuerpo.ancu_iActoNotarialId;

            ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();
            RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();

            lACTONOTARIAL.acno_iActoNotarialId = lCuerpo.ancu_iActoNotarialId;
            lACTONOTARIAL = lActoNotarialConsultaBL.obtener(lACTONOTARIAL);

            lACTONOTARIAL.acno_vAutorizacionTipo = Convert.ToString(jsonObject["acno_sAutorizacionTipoId"]);
            lACTONOTARIAL.acno_sAutorizacionDocumentoTipoId = 1; //D.N.I.
            lACTONOTARIAL.acno_vAutorizacionDocumentoNumero = Convert.ToString(jsonObject["acno_vAutorizacionDocumentoNumero"]);

            lACTONOTARIAL.acno_vNumeroColegiatura = Convert.ToString(jsonObject["acno_vNumeroColegiatura"]);

            lACTONOTARIAL.acno_vNumeroLibro = Convert.ToString(jsonObject["acno_vNumeroLibro"]);
            lACTONOTARIAL.acno_vNumeroFojaInicial = Convert.ToInt16(jsonObject["acno_vNumeroFojaInicial"]);
            lACTONOTARIAL.acno_vNumeroFojaFinal = Convert.ToInt16(jsonObject["acno_vNumeroFojaFinal"]);
            lACTONOTARIAL.acno_sNumeroHojas = Convert.ToInt16(jsonObject["acno_sNumeroHojas"]);
            lACTONOTARIAL.acno_nCostoEP = Convert.ToDouble(jsonObject["acno_nCostoEP"]);
            lACTONOTARIAL.acno_nCostoParte2 = Convert.ToDouble(jsonObject["acno_nCostoParte2"]);
            lACTONOTARIAL.acno_nCostoTestimonio = Convert.ToDouble(jsonObject["acno_nCostoTestimonio"]);

           
            lACTONOTARIAL.acno_vNumeroEscrituraPublica = Convert.ToString(jsonObject["acno_vNumeroEscrituraPublica"]);


            lACTONOTARIAL.acno_vNumeroOficio = Convert.ToString(jsonObject["acno_vNroOficio"]);
            lACTONOTARIAL.acno_vRegistradorNombre = Convert.ToString(jsonObject["acno_vRegistrador"]);
            
            lACTONOTARIAL.acno_iOficinaRegistralId = Convert.ToInt16(jsonObject["acno_iOficinaRegistralId"]);

            //lACTONOTARIAL.acno_vPresentanteNombre = Convert.ToString(jsonObject["acno_vPresentante"]);
            //lACTONOTARIAL.acno_sPresentanteTipoDocumento = Convert.ToInt16(jsonObject["acno_iTipoDocRepresentante"]);
            //lACTONOTARIAL.acno_vPresentanteNumeroDocumento = Convert.ToString(jsonObject["acno_vNumeroDocumentoRepresentante"]);
            //lACTONOTARIAL.acno_sPresentanteGenero = Convert.ToInt16(jsonObject["acno_sPresentanteGenero"]);

            lACTONOTARIAL.acno_vNombreColegiatura = Convert.ToString(jsonObject["acno_vNombreColegiatura"]);


            if (jsonObject["acno_iNumeroActoNotarial"] != String.Empty)
                lACTONOTARIAL.acno_iNumeroActoNotarial = Convert.ToInt32(jsonObject["acno_iNumeroActoNotarial"]);
            else
                lACTONOTARIAL.acno_iNumeroActoNotarial = 0;

            lCuerpo.ActoNotarial = lACTONOTARIAL;
            #endregion

            #region Imagen
            List<BE.MRE.RE_ACTONOTARIALDOCUMENTO> lstImagenes = (List<BE.MRE.RE_ACTONOTARIALDOCUMENTO>)HttpContext.Current.Session["ImagenesContainer"];

            String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
            String uploadFile = string.Empty;
            List<BE.MRE.RE_ACTONOTARIALDOCUMENTO> lstImagenesEliminadas = new List<BE.MRE.RE_ACTONOTARIALDOCUMENTO>();
            lstImagenesEliminadas = (List<BE.MRE.RE_ACTONOTARIALDOCUMENTO>)HttpContext.Current.Session["Imagenes_eliminadas"];
            foreach (BE.MRE.RE_ACTONOTARIALDOCUMENTO Imagen in lstImagenesEliminadas)
            {
                uploadFile = Imagen.ando_vRutaArchivo;
                if (uploadFile.Length > 0)
                {
                    if (System.IO.File.Exists(uploadPath + "/" + uploadFile))
                    {
                        System.IO.File.Delete(uploadPath + "/" + uploadFile);
                    }
                }
                if (Imagen.ando_iActoNotarialDocumentoId != 0)
                    lstImagenes.Add(Imagen);
            }
            #endregion

            //--------------------------------------------------------------
            ActoNotarialMantenimiento mnt;

            string strMensaje = "";

            strMensaje = ActualizarFechaSuscripcionOtorgantes();


            string strFechaConclusionFirma = Convert.ToString(jsonObject["acno_dFechaConclusionFirma"]);

            if (strFechaConclusionFirma.Length > 0)
            {
                RE_ACTONOTARIAL actoNotarial = new RE_ACTONOTARIAL();

                actoNotarial.acno_iActoNotarialId = Convert.ToInt64(jsonObject["ancu_iActoNotarialId"]); ;
                actoNotarial.acno_sOficinaConsularId = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                actoNotarial.acno_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                actoNotarial.acno_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
                actoNotarial.acno_dFechaConclusionFirma = Comun.FormatearFecha(jsonObject["acno_dFechaConclusionFirma"]);

                mnt = new ActoNotarialMantenimiento();

                strMensaje = mnt.ActualizarFechaConclusion(actoNotarial);
            }
            //--------------------------------------------------------------            



            mnt = new ActoNotarialMantenimiento();
            return serializer.Serialize(mnt.Insertar_ActoNotarialCuerpo(lCuerpo, lstImagenes)).ToString();
        }

        [System.Web.Services.WebMethod]
        public static string llenar_introduccion_escritura(string cuerpo)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var jsonObject = serializer.Deserialize<dynamic>(cuerpo);


            string strTextoActoNotarial = Convert.ToString(jsonObject["textoActoNotarial"]);
            string strTexto = string.Empty;
            
            if (strTextoActoNotarial.IndexOf("RECTIFICACIÓN") > -1)
            {
                strTexto = llenar_introduccion_escritura_Rectificacion(cuerpo);
            }
            else
            {
                strTexto = llenar_introduccion_escritura_Regular(cuerpo);
            }
            return strTexto;
        }
        //-----------------------------------------------------------
        [System.Web.Services.WebMethod]
        public static string llenar_conclusion_escritura(string cuerpo)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var jsonObject = serializer.Deserialize<dynamic>(cuerpo);

            string strTextoActoNotarial = Convert.ToString(jsonObject["textoActoNotarial"]);
            string strTexto = string.Empty;

            if (strTextoActoNotarial.IndexOf("RECTIFICACIÓN") > -1)
            {
                strTexto = llenar_conclusion_escritura_Rectificacion(cuerpo);
            }
            else
            {
                strTexto = llenar_conclusion_escritura_Regular(cuerpo);
            }
            return strTexto;
        }

        [System.Web.Services.WebMethod]
        public static string llenar_final_escritura(string cuerpo)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var jsonObject = serializer.Deserialize<dynamic>(cuerpo);

            string strTextoActoNotarial = Convert.ToString(jsonObject["textoActoNotarial"]);
            string strTexto = string.Empty;

            if (strTextoActoNotarial.IndexOf("RECTIFICACIÓN") > -1)
            {
                strTexto = llenar_final_escritura_Rectificacion(cuerpo);
            }
            else
            {
                strTexto = llenar_final_escritura_Regular(cuerpo);
            }

            return strTexto;
        }
        [System.Web.Services.WebMethod]
        public static string obtenerTextoNormativo(string cuerpo)
        {

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var jsonObject = serializer.Deserialize<dynamic>(cuerpo);
            
            //string idss = Convert.ToString(jsonObject["ids"]);
            //string[] words = idss.Split('-');
            string striActoNotarialId = Convert.ToString(jsonObject["iActoNotarialId"]);
            Int64 iActoNotarialId = Convert.ToInt64(striActoNotarialId);

            ActoNotarial_NormasConsultaBL objActoNotarialNormasConsultaBL = new ActoNotarial_NormasConsultaBL();
            DataTable dtActoNotarialNormas = new DataTable();
            RE_ACTONOTARIAL_NORMA actoNotarial_normaBE = new RE_ACTONOTARIAL_NORMA();
            
            actoNotarial_normaBE.anra_iActoNotarialId = iActoNotarialId;
            dtActoNotarialNormas = objActoNotarialNormasConsultaBL.ObtenerNormas(actoNotarial_normaBE);

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            for (int i = 0; i < dtActoNotarialNormas.Rows.Count; i++)
            {
                row = new Dictionary<string, object>();
                //norm_vDescripcion
                string ColumnName = "norm_vDescripcion";
                string val = "" + Convert.ToString((dtActoNotarialNormas.Rows[i][ColumnName].ToString())).ToUpper();
                row.Add(ColumnName, val);
                val = val.Replace("<P>", "");
                val = val.Replace("</P>", "");
                row.Add("norm_vDescripcion_aux", val);
                rows.Add(row);
            }

            //-----------------------------------------------------------------

            //ArticuloLeyConsultaBL objBL = new ArticuloLeyConsultaBL();
            //DataTable dtArticulo = new DataTable();
            //string strArticulo = string.Empty;
                        
            //List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            //Dictionary<string, object> row;
            //foreach (string id in words)
            //{
            //    dtArticulo = objBL.ObtenerTextoLey(Comun.ToNullInt32(id));
            //    foreach (DataRow dr in dtArticulo.Rows)
            //    {
            //        row = new Dictionary<string, object>();
            //        foreach (DataColumn col in dtArticulo.Columns)
            //        {
            //            string ColumnName = col.ColumnName;
            //            string val = "" + Convert.ToString((dr[col])).ToUpper();
            //            row.Add(ColumnName, val);
            //            if (ColumnName.Equals("arle_vDescripcionLarga"))
            //            {
            //                val = val.Replace("<P>", "");
            //                val = val.Replace("</P>", "");
            //                row.Add("arle_vDescripcionLarga_aux", val);
            //            }
            //        }
            //        rows.Add(row);
            //    }
            //}

            //-----------------------------------------------------------------
            return serializer.Serialize(rows);
            
            //return "{\"items\":[" + strArticulo+"]}";
        }
        

        protected void Btn_Aprobar_Click(object sender, EventArgs e)
        {
            String StrScript = String.Empty;


            //string ides = hf_idsNormativos.Value;
            //--------------------------------------------------
            //Fecha: 09/12/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Validar el stock de insumos.
            //--------------------------------------------------
            int intStock = 0;
            
            ActuacionConsultaBL objActuacionBL = new ActuacionConsultaBL();

            intStock = objActuacionBL.ObtenerSaldoAutoadhesivos(
            Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
            Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]),
            Convert.ToInt16(Enumerador.enmInsumoTipo.AUTOADHESIVO));
            
            if (intStock <= 0)
            {
                //StrScript = SGAC.Accesorios.Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Acto Notarial", Constantes.CONST_MENSAJE_SALDO_INSUFICIENTE, false, 200, 250);
                //Comun.EjecutarScript(Page, StrScript);
                return;
            }
            //--------------------------------------------------
            RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();
            ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();

            lACTONOTARIAL.acno_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
            lACTONOTARIAL = lActoNotarialConsultaBL.obtener(lACTONOTARIAL);

            String Mensaje = String.Empty;
            
            Int32 FojaInicial = 0, FojaFinal = 0, acno_iNumeroEscritura = 0;
            String iNumeroLibro = String.Empty;

            int intCantidadFojas = Convert.ToInt32(HF_NUMERO_PAGINA_DOCUMENTO.Value);
            if (intCantidadFojas <= 1)
            {                
                StrScript = SGAC.Accesorios.Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Acto Notarial", "Vuelva a generar la Vista Previa", false, 200, 250);
                //Comun.EjecutarScript(Page, StrScript);
                Comun.EjecutarScript(Page, "$('#btnTraerCuerpo').prop('disabled', false);$('#btnCncl_tab3').prop('disabled', false);$('#MainContent_Btn_VistaPreviaAprobar').prop('disabled', false);" + StrScript);

                return;
            }
            Int16 ESCRITURA_PUBLICA= Convert.ToInt16((int)Enumerador.enmNotarialLibroTipo.ESCRITURA_PUBLICA);
            Int16 CONST_SESION_OFICINACONSULAR_ID=Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            Int32 NUMERO_PAGINA_DOCUMENTO=Convert.ToInt32(HF_NUMERO_PAGINA_DOCUMENTO.Value);
            Int32 resp = lActoNotarialConsultaBL.ValidarFojas(Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value), ESCRITURA_PUBLICA, CONST_SESION_OFICINACONSULAR_ID, NUMERO_PAGINA_DOCUMENTO, 1,
                ref Mensaje, ref FojaInicial, ref FojaFinal, ref acno_iNumeroEscritura, ref iNumeroLibro);
            if (resp == 0)
            {
                StrScript = SGAC.Accesorios.Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Acto Notarial", Mensaje, false, 190, 250);
                Comun.EjecutarScript(Page, StrScript);
                return;
            }

            //-----------------------------------------------------
            //Fecha: 27/03/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Actualizar el cuerpo de la escritura.
            //-----------------------------------------------------
            RE_ACTONOTARIALCUERPO lACTONOTARIALCUERPO = new RE_ACTONOTARIALCUERPO();
            ActoNotarialCuerpoConsultaBL lActoNotarialCuerpoConsultaBL = new ActoNotarialCuerpoConsultaBL();
            lACTONOTARIALCUERPO.ancu_iActoNotarialId = lACTONOTARIAL.acno_iActoNotarialId;
            lACTONOTARIALCUERPO = lActoNotarialCuerpoConsultaBL.obtener(lACTONOTARIALCUERPO);
            if (lACTONOTARIALCUERPO.ancu_iActoNotarialCuerpoId != 0)
            {
                Documento odoc = new Documento();

                string strLetrasNumeroEscrituraPublica = odoc.ConvertirNumeroLetras(acno_iNumeroEscritura.ToString(), true);
                int intLibro = 0;

                try
                {
                    intLibro = int.Parse(iNumeroLibro);
                }
                catch (Exception)
                {
                    intLibro = 0;
                    throw;
                }

                //string strLibroRomanos = Util.ObtenerRomanosMax3999(intLibro);
                string strLibroRomanos = intLibro.ToString();

                string strAnioEscritura = lACTONOTARIAL.acno_dFechaExtension.Year.ToString();
                StringBuilder sScript = new StringBuilder();
                sScript.Append("<p style='text-align:justify;font-weight:bold;'>");
                sScript.Append("ESCRITURA PÚBLICA NÚMERO " + strLetrasNumeroEscrituraPublica + "(" + acno_iNumeroEscritura.ToString() + ")" + " (LIBRO " + strLibroRomanos + ", AÑO " + strAnioEscritura + ").");
                sScript.Append("</p>");


                ActoNotarialMantenimiento ActoNotarialBL = new ActoNotarialMantenimiento();
                CBE_CUERPO lCuerpo = new CBE_CUERPO();

                lCuerpo.ancu_iActoNotarialCuerpoId = lACTONOTARIALCUERPO.ancu_iActoNotarialCuerpoId;
                lCuerpo.ancu_iActoNotarialId = lACTONOTARIALCUERPO.ancu_iActoNotarialId;

                //----------------------------------------------------------------------
                //Objetivo: Reemplazar el primer parrafo.
                //----------------------------------------------------------------------
                string strTexto = lACTONOTARIALCUERPO.ancu_vCuerpo;
                int intIndice_Fin = strTexto.IndexOf("<p", 1);
                string strLineaOld = strTexto.Substring(0, intIndice_Fin);
                string strCuerpo = strTexto.Replace(strLineaOld, sScript.ToString());
                //----------------------------------------------------------------------
                string strEscrituraPublicaNumeroOld = "REGISTRO DE ESCRITURAS PÚBLICAS NÚMERO CERO";

                string strEscrituraPublicaNumeroNuevo = "REGISTRO DE ESCRITURAS PÚBLICAS NÚMERO " + strLetrasNumeroEscrituraPublica;

                strCuerpo = strCuerpo.Replace(strEscrituraPublicaNumeroOld, strEscrituraPublicaNumeroNuevo);
                //----------------------------------------------------------------------
                //Objetivo: Reemplazar LIBR000 por el número del libro en romanos
                //----------------------------------------------------------------------
                strCuerpo = strCuerpo.Replace("LIBR000", strLibroRomanos);

                string strCadFojas = "";
                string strCadFojasLetras = "";

                for (int i = FojaInicial; i <= FojaFinal; i++)
                {
                    strCadFojas = strCadFojas + i.ToString() + ", ";
                    strCadFojasLetras = strCadFojasLetras + odoc.ConvertirNumeroLetras(i.ToString(), true) + ", ";
                }

                strCadFojas = strCadFojas.Substring(0, strCadFojas.Length - 2);
                strCadFojasLetras = strCadFojasLetras.Substring(0, strCadFojasLetras.Length - 2);

                strCuerpo = strCuerpo.Replace("FOJAS CERO (0)", "FOJAS " + strCadFojasLetras + " (" + strCadFojas + ")");
                //----------------------------------------------------------------------
                lCuerpo.ancu_vCuerpo = strCuerpo;
                lCuerpo.ancu_vTextoCentral = RichTextBox.Text.Trim();
                lCuerpo.ancu_vTextoAdicional = RichTextBoxAdicional.Text.Trim();
                lCuerpo.ancu_vDL1049Articulo55C = RichTextBoxDL1049Art55IncisoC.Text.Trim();
                lCuerpo.ancu_vTextoNormativo = lACTONOTARIALCUERPO.ancu_vTextoNormativo;
                lCuerpo.ancu_vFirmaIlegible = txt_ancu_vFirmaIlegible.Text.Trim();
                lCuerpo.ancu_cEstado = lACTONOTARIALCUERPO.ancu_cEstado;
                lCuerpo.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                lCuerpo.ancu_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                lCuerpo.ancu_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
                lCuerpo.ancu_sUsuarioCreacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                lCuerpo.ancu_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();

                lACTONOTARIAL.acno_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                lACTONOTARIAL.acno_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
                lACTONOTARIAL.acno_vNumeroLibro = iNumeroLibro.ToString();
                lACTONOTARIAL.acno_vNumeroFojaInicial = Convert.ToInt16(FojaInicial.ToString());
                lACTONOTARIAL.acno_vNumeroFojaFinal = Convert.ToInt16(FojaFinal.ToString());
                lACTONOTARIAL.acno_vNumeroEscrituraPublica = acno_iNumeroEscritura.ToString();
                lACTONOTARIAL.acno_nCostoEP = Convert.ToDouble(txtCostoEP.Text);
                lACTONOTARIAL.acno_nCostoParte2 = Convert.ToDouble(txtCostoParte2.Text);
                lACTONOTARIAL.acno_nCostoTestimonio = Convert.ToDouble(txtCostoTestimonio.Text);


                lCuerpo.ActoNotarial = lACTONOTARIAL;

                ActoNotarialBL.Insertar_ActoNotarialCuerpo(lCuerpo, null);
            }



            //-----------------------------------------------------

            txtNroFojaIni.Text = FojaInicial.ToString();
            txtNroFojaFinal.Text = FojaFinal.ToString();
            txtNroEscritura.Text = acno_iNumeroEscritura.ToString();
            txtNroLibro.Text = iNumeroLibro.ToString();


            if (lACTONOTARIAL.acno_sEstadoId == (Int16)Enumerador.enmNotarialProtocolarEstado.TRANSCRITA || lACTONOTARIAL.acno_sEstadoId == (Int16)Enumerador.enmNotarialProtocolarEstado.ASOCIADA)
            {

                ActoNotarialMantenimiento bl = new ActoNotarialMantenimiento();
                RE_ACTONOTARIAL actonotarialBE = new RE_ACTONOTARIAL();
                actonotarialBE.acno_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
                actonotarialBE.acno_sEstadoId = (int)Enumerador.enmNotarialProtocolarEstado.APROBADA;
                actonotarialBE.acno_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                actonotarialBE.acno_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                actonotarialBE.acno_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();

                if (bl.ActoNotarialActualizarEstado(actonotarialBE) == false)
                {
                    StrScript = string.Empty;
                    StrScript = @"$(function(){{
                            HabilitarTabPagos();
                           }});";
                    StrScript = string.Format(StrScript);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitarTabPagos", StrScript, true);
                }
            }

            Habilitar_Tab((int)Enumerador.enmNotarialProtocolarEstado.APROBADA);

            //Btn_Aprobar.Enabled = false;
            
            //------------------------------------------
            // Fecha: 07/06/2021
            // Autor: Miguel Márquez Beltrán
            // Motivo: Deshabilitar Check de Aprobar
            //------------------------------------------
            //vpipa check
            //cbxAfirmarTexto.Enabled = false;
            ctrlToolBar5.btnGrabar.Enabled = false;
            StrScript = "HabilitarTabPagos();$('#MainContent_btnCncl_tab3').prop('disabled', true); $('#btnTraerCuerpo').prop('disabled', true); $('#MainContent_Btn_VistaPreviaAprobar').prop('disabled', true);";
            StrScript = StrScript+@"$('#MainContent_Btn_AfirmarTextoLeido').prop('disabled', true);$('#MainContent_cbxAfirmarTexto').prop('disabled', true);";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitarChec", StrScript, true);
            //------------------------------------------
            //AsignarNormasporTipoActoNotarial();
            //-------------------------------------------------
            //Fecha: 13/01/2022
            //Autor: Miguel Márquez Beltrán
            //Motivo: Deshabilitar el recuadro de presentante.
            //-------------------------------------------------
            rbApoderado.Enabled = false;
            ddl_Apoderado.Enabled = false;
            rbOtros.Enabled = false;
            ddl_TipoDocrepresentante.Enabled = false;
            txtRepresentanteNroDoc.Enabled = false;
            txtRepresentanteNombres.Enabled = false;
            ddl_GerenoPresentante.Enabled = false;
            updParte.Update();
            //-------------------------------------------------

            //-----------------------------------------------------------------------------------------------------
            // Setear el tarifario con las tarifas correspondientes al tipo de acto seleccionado en el Tab Pagos
            //-----------------------------------------------------------------------------------------------------
            DataTable dtTarifario;
            Session.Remove("dtTarifarioFiltrado");

            //-----------------------------------------------
            //Fecha: 06/12/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Consultar las tarifas de la sección 2.
            //-----------------------------------------------
            //dtTarifario = cargar_tarifas(HF_ACTONOTARIAL_POR_TARIFA.Value);
            
            dtTarifario = cargar_tarifas();

            //------------------------------------------
            //Fecha: 11/11/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: El control se realiza desde
            //          tipo de actos por tarifa.
            //------------------------------------------
            //if (hdn_acno_iActoNotarialReferenciaId.Value == "0") // Acción de Registro
            //{
            //    dtTarifario = cargar_tarifas(HF_ACTONOTARIAL_POR_TARIFA.Value);
            //}
            //else // Acción de Rectificación
            //{
            //    dtTarifario = cargar_tarifas(HF_ACTONOTARIAL_POR_TARIFA.Value, ddlAccionR.SelectedValue);
            //}

            Session.Add("dtTarifarioFiltrado", dtTarifario);

            this.Lst_Tarifario.DataSource = dtTarifario;
            this.Lst_Tarifario.DataTextField = "tari_vDescripcionCorta";
            this.Lst_Tarifario.DataValueField = "tari_sTarifarioId";
            this.Lst_Tarifario.DataBind();

            Txt_TarifaId.Enabled = true;
            Txt_TarifaDescripcion.Enabled = false;

            ModoEdicion();

            //--vpipa check
            StrScript = @"$('#MainContent_Btn_AfirmarTextoLeido').prop('disabled', true);$('#MainContent_cbxAfirmarTexto').prop('disabled', true);";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitarChec", StrScript, true);
            //cbxAfirmarTexto.Enabled = false;
            UpdUbigeoRegistrador.Update();
            updCuerpoCabecera.Update();
            updParte.Update();
            updRegPago.Update();
        }

        #region Sección Imagen

        [System.Web.Services.WebMethod]
        public static string argregar_imagen(string imagen)
        {
            RE_ACTONOTARIALDOCUMENTO lImagenAdjunta = new RE_ACTONOTARIALDOCUMENTO();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var jsonObject = serializer.Deserialize<dynamic>(imagen);

            lImagenAdjunta.ando_iActoNotarialDocumentoId = 0;
            lImagenAdjunta.ando_iActoNotarialId = Convert.ToInt64(jsonObject[0]["ando_iActoNotarialId"]);
            lImagenAdjunta.ando_sTipoDocumentoId = Convert.ToInt16(jsonObject[0]["ando_sTipoDocumentoId"]);
            lImagenAdjunta.ando_sTipoInformacionId = Convert.ToInt16(jsonObject[0]["ando_sTipoInformacionId"]);
            lImagenAdjunta.ando_sSubTipoInformacionId = Convert.ToInt16(jsonObject[0]["ando_sSubTipoInformacionId"]);
            lImagenAdjunta.ando_vRutaArchivo = jsonObject[0]["ando_vRutaArchivo"].ToString();
            lImagenAdjunta.ando_vDescripcion = jsonObject[0]["ando_vDescripcion"].ToString();

            List<RE_ACTONOTARIALDOCUMENTO> ParametrosContainer = new List<RE_ACTONOTARIALDOCUMENTO>();

            if (HttpContext.Current.Session["ImagenesContainer"] != null)
            {
                ParametrosContainer = (List<RE_ACTONOTARIALDOCUMENTO>)HttpContext.Current.Session["ImagenesContainer"];
            }

            ParametrosContainer.Add(lImagenAdjunta);

            HttpContext.Current.Session["ImagenesContainer"] = (List<RE_ACTONOTARIALDOCUMENTO>)ParametrosContainer;

            return "Ok";
        }

        private short ObtenerIndice(int iRowIndex)
        {
            if (iRowIndex < 0)
            {
                return -1;
            }

            return 1;
        }

        protected void gdvImagenes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 lRowIndex = Convert.ToInt32(e.CommandArgument);
            List<RE_ACTONOTARIALDOCUMENTO> Archivo = (List<RE_ACTONOTARIALDOCUMENTO>)Session["ImagenesContainer"];

            int indice;

            if (Archivo != null)
                indice = Archivo.Count;
            else
                indice = -1;



            switch (e.CommandName.ToString())
            {
                case "Visualizar":

                    //if (indice <= 0)
                    //{
                    //    string strScriptt = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", Constantes.CONST_MENSAJE_PERDIDA_SESSION_ACTUACION);
                    //    EjecutarScript(strScriptt);
                    //    return;
                    //}

                    Session["strTipoArchivo"] = ".jpg";

                    // Verificar si la imagen existe...

                    string strUrl = "../Registro/FrmPreviewNotarial.aspx?Ruta=" + Convert.ToString(gdvImagenes.Rows[lRowIndex].Cells[2].Text);
                    string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=700,height=500,left=100,top=100');";
                    Comun.EjecutarScript(Page, strScript);

                    break;
                case "Eliminar":

                    if (indice <= 0)
                    {
                        string strScriptt = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", Constantes.CONST_MENSAJE_PERDIDA_SESSION_ACTUACION);
                        EjecutarScript(strScriptt);
                        return;
                    }


                    if (((List<RE_ACTONOTARIALDOCUMENTO>)Session["ImagenesContainer"]).Count > lRowIndex)
                    {
                        // 1. Guardo objeto a eliminar
                        BE.MRE.RE_ACTONOTARIALDOCUMENTO Imagen = ((List<RE_ACTONOTARIALDOCUMENTO>)Session["ImagenesContainer"])[lRowIndex];
                        Imagen.ando_cEstado = "E";
                        // 2. Elimino en la lista de imágenes
                        ((List<RE_ACTONOTARIALDOCUMENTO>)Session["ImagenesContainer"]).RemoveAt(lRowIndex);
                        // 3. Guardo en lista de eliminados
                        string strNombreImagen = Convert.ToString(Page.Server.HtmlDecode(gdvImagenes.Rows[lRowIndex].Cells[2].Text)).Trim();
                        ((List<BE.MRE.RE_ACTONOTARIALDOCUMENTO>)Session["Imagenes_eliminadas"]).Add(Imagen);
                        hdn_imagen_nombre.Value = "";
                    }
                    else
                    {
                        return;
                    }

                    this.gdvImagenes.DataSource = CrearTablaImagenes(null);
                    this.gdvImagenes.DataBind();

                    System.Web.UI.HtmlControls.HtmlTableCell msjeSucess = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader2.FindControl("msjeSucess");
                    msjeSucess.Visible = false;

                    System.Web.UI.HtmlControls.HtmlTableCell msjeWarning = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader2.FindControl("msjeWarning");
                    msjeWarning.Visible = false;

                    System.Web.UI.HtmlControls.HtmlTableCell msjeError = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader2.FindControl("msjeError");
                    msjeError.Visible = false;

                    break;
            }
        }

        private DataTable CrearTablaImagenes(RE_ACTONOTARIALDOCUMENTO documentoDigitalizado)
        {
            List<RE_ACTONOTARIALDOCUMENTO> ParametrosContainer = (List<RE_ACTONOTARIALDOCUMENTO>)Session["ImagenesContainer"];
            if (documentoDigitalizado != null)
            {
                if (documentoDigitalizado.ando_iActoNotarialDocumentoId == 0)
                {
                    ParametrosContainer.Add(documentoDigitalizado);
                }
            }

            Session["ImagenesContainer"] = (List<RE_ACTONOTARIALDOCUMENTO>)ParametrosContainer;

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

        protected void btnCargarImagenes_Click(object sender, EventArgs e)
        {
            this.gdvImagenes.DataSource = CrearTablaImagenes(null);
            this.gdvImagenes.DataBind();

            if (txtImagenTitulo.Text.Trim() != string.Empty)
            {
                txtImagenTitulo.Text = string.Empty;
                lblValidacionImagen.Visible = false;
            }
            else
            {
                lblValidacionImagen.Visible = true;
            }

            //btnVisualizarDigitalizacion.Enabled = false;

            updImagenes.Update();
        }

        #endregion



        private static Int16 ObtenerCantidadFojasPorFormato(Int64 intActoNotarialId, Int64 intActuacionId)
        {
            Int16 intCantidadFojas = 0;
            try
            {                

                ActoNotarialConsultaBL objActuacionConsulta = new ActoNotarialConsultaBL();

                RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();
                lACTONOTARIAL.acno_iActoNotarialId = Convert.ToInt64(intActoNotarialId);
                lACTONOTARIAL.acno_iActuacionId = Convert.ToInt64(intActuacionId);
                lACTONOTARIAL.acno_sTipoActoNotarialId = Convert.ToInt16(Enumerador.enmNotarialTipoActo.PROTOCOLAR);
                lACTONOTARIAL = objActuacionConsulta.obtener(lACTONOTARIAL);

                Int16 intFojaInicial = Convert.ToInt16(lACTONOTARIAL.acno_vNumeroFojaInicial.ToString());
                Int16 intFojaFinal = Convert.ToInt16(lACTONOTARIAL.acno_vNumeroFojaFinal.ToString());
                Int16 intNumeroFojas = Convert.ToInt16(intFojaFinal - intFojaInicial + 2);
                //------------------------------------------------------------
                //Se adiciona una foja adicional por el parte o testimonio.
                //------------------------------------------------------------

                intCantidadFojas = intNumeroFojas;
            }
            catch (Exception ex)
            {
                throw;
            }

            return intCantidadFojas;
        }
        #endregion

        #region Pestaña: Pago
        private string strVariableTarifario = "objTarifarioBE";
        private string strVarActuacionDetalleDT = "dtActuacionDetalle";
        private string strVarActuacionDetalleIndice = "intActuacionDetalleIndice";

        protected void Gdv_Tarifa_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int intSeleccionado = Convert.ToInt32(e.CommandArgument);
            Session[strVarActuacionDetalleIndice] = intSeleccionado;

            String strScript = String.Empty;
            Int32 lRowIndex = Convert.ToInt32(e.CommandArgument);
            //int indice;

            //if (((DataTable)Session[strVarActuacionDetalleDT]) != null)
            //    indice = ((DataTable)Session[strVarActuacionDetalleDT]).Rows.Count;
            //else
            //    indice = -1;

            if (e.CommandName == "Eliminar")
            {
                #region Eliminar

                #region Si acde_iActuacionDetalleId = '0'

                if (this.Gdv_Tarifa.Rows[lRowIndex].Cells[0].Text == "0")
                {

                    #region Validación al quere eliminar la tarifa 17B mientras exista la tarifa: 17C.

                    DataTable dtActuaciones = ((DataTable)Session[strVarActuacionDetalleDT]).Copy();
                    String Tarifa = this.Gdv_Tarifa.Rows[lRowIndex].Cells[5].Text.ToString();

                    if (Tarifa == Constantes.CONST_EXCEPCION_TARIFA_17B)
                    {
                        foreach (GridViewRow row in Gdv_Tarifa.Rows)
                        {
                            String TarifaAux = row.Cells[5].Text.ToString();

                            if (TarifaAux.Trim() == Constantes.CONST_EXCEPCION_TARIFA_17C)
                            {
                                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO PROTOCOLAR", "NO PUEDE ELIMINAR UNA TARIFA 17B, MIENTRAS EXISTA UNA TARIFA 17C");
                                Comun.EjecutarScript(Page, strScript);
                                updRegPago.Update();
                                Session[strVarActuacionDetalleIndice] = -1;
                                return;
                            }
                        }
                    }
                    #endregion

                    #region Validación al querer eliminar: 12C, 13B o 13C.

                    String TarifaID = this.Gdv_Tarifa.Rows[lRowIndex].Cells[1].Text.ToString();
                    if (TarifaID == Constantes.CONST_PROTOCOLAR_ID_TARIFA_12B.ToString()
                        || TarifaID == Constantes.CONST_PROTOCOLAR_ID_TARIFA_12C.ToString()
                        || TarifaID == Constantes.CONST_PROTOCOLAR_ID_TARIFA_13B.ToString()
                        || TarifaID == Constantes.CONST_PROTOCOLAR_ID_TARIFA_13C.ToString())
                    {
                        strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO PROTOCOLAR", "NO PUEDE ELIMINAR ESTA TARIFA POR QUE DEPENDE DE OTRA TARIFA");
                        Comun.EjecutarScript(Page, strScript);
                        updRegPago.Update();
                        Session[strVarActuacionDetalleIndice] = -1;
                        return;
                    }
                    #endregion

                    DataTable dtaux = new DataTable();
                    dtaux = dtActuaciones.Clone();

                    if (TarifaID == Constantes.CONST_PROTOCOLAR_ID_TARIFA_12A.ToString())
                    {
                        #region Tarifa 12A

                        //int intTotalOtorgante = ObtenerCantidadParticipantesPorTipo(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE);
                        //int intTotalApoderado = ObtenerCantidadParticipantesPorTipo(Enumerador.enmNotarialProtocolarTipoParticipante.APODERADO);
                        int intTotalOtorgante = ObtenerCantidadParticipantesPorTipo("INICIA");
                        int intTotalApoderado = ObtenerCantidadParticipantesPorTipo("RECIBE");
                        int intTotalParticipa = intTotalOtorgante + intTotalApoderado;

                        DataRow desRow1 = dtaux.NewRow();
                        for (int i = 0; i < dtActuaciones.Rows.Count; i++)
                        {
                            if (i >= lRowIndex && i <= lRowIndex + intTotalParticipa)
                            {
                            }
                            else
                            {
                                desRow1 = dtaux.NewRow();
                                desRow1.ItemArray = ((object[])dtActuaciones.Rows[i].ItemArray.Clone());
                                dtaux.Rows.Add(desRow1);
                            }
                        }
                        #endregion
                    }
                    else if (TarifaID == Constantes.CONST_PROTOCOLAR_ID_TARIFA_13A.ToString())
                    {
                        #region Tarifa 13A

                        //int intTotalOtorgante = ObtenerCantidadParticipantesPorTipo(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE);
                        //int intTotalApoderado = ObtenerCantidadParticipantesPorTipo(Enumerador.enmNotarialProtocolarTipoParticipante.APODERADO);
                        int intTotalOtorgante = ObtenerCantidadParticipantesPorTipo("INICIA");
                        int intTotalApoderado = ObtenerCantidadParticipantesPorTipo("RECIBE");
                        int intTotalParticipa = intTotalOtorgante + intTotalApoderado;

                        DataRow desRow = dtaux.NewRow();
                        for (int i = 0; i < dtActuaciones.Rows.Count; i++)
                        {
                            if (i >= lRowIndex && i <= lRowIndex + intTotalParticipa)
                            {
                            }
                            else
                            {
                                desRow = dtaux.NewRow();
                                desRow.ItemArray = ((object[])dtActuaciones.Rows[i].ItemArray.Clone());
                                dtaux.Rows.Add(desRow);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region Diferente a 12A y 13A
                        DataRow desRow = dtaux.NewRow();
                        for (int i = 0; i < dtActuaciones.Rows.Count; i++)
                        {
                            if (i != lRowIndex)
                            {
                                desRow = dtaux.NewRow();
                                desRow.ItemArray = ((object[])dtActuaciones.Rows[i].ItemArray.Clone());
                                dtaux.Rows.Add(desRow);
                            }
                        }
                        #endregion
                    }

                    Session[strVarActuacionDetalleIndice] = -1;
                    Session[strVarActuacionDetalleDT] = dtaux;

                    Gdv_Tarifa.DataSource = dtaux;

                    if (dtaux.Rows.Count > 0)
                    {
                        ddlTipoPago.Enabled = true;
                    }
                    else
                    {
                        ddlTipoPago.SelectedIndex = 0;
                        ddlTipoPago_SelectedIndexChanged(sender, null);
                        ddlTipoPago.Enabled = false;
                    }

                    Gdv_Tarifa.DataBind();
                    CalcularTotalPago();
                    bool bNoCobrado = ExisteInafecto_Exoneracion(ddlTipoPago.SelectedValue);
                    if (bNoCobrado || Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
                    {
                        Txt_TotSC.Text = "0.00";
                        Txt_TotML.Text = "0.00";
                    }
                }
                #endregion
                
                #endregion

            }
        }

        protected void Gdv_Tarifa_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[4].Text.Trim() != "&nbsp;")
                {
                    e.Row.Cells[4].Text = (Comun.FormatearFecha(e.Row.Cells[4].Text)).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                }
            }
        }

        protected void btnLimpiarTarifa_Click(object sender, EventArgs e)
        {
            Txt_TarifaId.Text = string.Empty;
            Txt_TarifaDescripcion.Text = string.Empty;

            hdn_tari_sCalculoTipoId.Value = "0";
            hdn_tari_sNumero.Value = "0";
            hdn_tari_sTarifarioId.Value = "0";
            hdn_tari_vLetra.Value = string.Empty;

            LimpiarDatosTarifaPago();
            LimpiarListaTarifa();

            Session[strVarActuacionDetalleIndice] = -1;
        }

        protected void ddlTipoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Txt_VoucherNro.Enabled = true;
            //Label24.Visible = true;
            //bool bNoCobrado = ExisteInafecto_Exoneracion(ddlTipoPago.SelectedValue);
            //if (bNoCobrado == false)
            //{
            //    CalcularTotalPago();
            //}
            CalcularTotalPago();

            //if (Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA) ||
            //    Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA) ||
            //    Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA))
            //{
            //    pnlPagLima.Visible = true;
            //}
            //else if (bNoCobrado || Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.NO_COBRADO) || Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.GRATIS) || Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.EFECTIVO))
            //{
            //    pnlPagLima.Visible = false;
            //    Txt_VoucherNro.Enabled = false;
            //    Txt_VoucherNro.Text = String.Empty;
            //    Label24.Visible = false;

            //    if (bNoCobrado || Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
            //    {
            //        Txt_TotSC.Text = "0.00";
            //        Txt_TotML.Text = "0.00";
            //        Lbl_TotalGeneral.Text = "0.00";
            //        Lbl_TotalExtranjera.Text = "0.00";
            //    }
            //}
            //else
            //{
            //    pnlPagLima.Visible = false;
            //}
            //LlenarListaExoneracion();
            //RBNormativa.Checked = true;
            //RBSustentoTipoPago.Checked = false;


            string strDescTipoPagoOrigen = Comun.ObtenerDescripcionTipoPago(Session, HF_TIPO_PAGO_ID.Value);

            Comun.ActualizarControlPago(Session, strDescTipoPagoOrigen, Txt_TarifaId.Text, Txt_TarifaCantidad.Text,
                ref ctrlToolBar5.btnGrabar, ref ddlTipoPago, ref txtNroOperacion, ref txtCodigoInsumo,
                ref ddlNomBanco, ref ctrFecPago, ref ddlExoneracion, ref lblExoneracion, ref lblValExoneracion,
                ref txtSustentoTipoPago, ref lblSustentoTipoPago, ref lblValSustentoTipoPago,
                ref RBNormativa, ref RBSustentoTipoPago, ref Txt_MontML, ref Txt_MtoSC,
                ref Txt_TotML, ref Txt_TotSC, ref LblDescMtoML, ref LblDescTotML,
                ref pnlPagLima, ref txtMtoCancelado);

            updRegPago.Update();
        }

        protected void Btn_AgregarTarifa_Click(object sender, EventArgs e)
        {
            

            DataTable dt = new DataTable();
            dt = (DataTable)Session["dtActuacionDetalle"];

            if (dt == null)
            {
                dt = CrearTablaActuacionDetalle();
            }

            DataRow dr;
            int intActuacionDetallePosicion = Convert.ToInt32(Session[strVarActuacionDetalleIndice]);

            String strScript = String.Empty;
            Int32 ContadorParte = 0;

            #region Validaciones
            if (Txt_TarifaCantidad.Text.Trim() == string.Empty)
            {
                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO PROTOCOLAR", "Falta ingresar la cantidad.");
                Comun.EjecutarScript(Page, strScript);
                updRegPago.Update();
                return;
            }

            if (hdn_tari_sNumero.Value + hdn_tari_vLetra.Value != "17B")
            {
                Double intMonto = 0;

                intMonto = Convert.ToDouble(this.Txt_MontML.Text.Trim());

                if (intMonto == 0)
                {
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO PROTOCOLAR", "Monto no puede ser cero.");
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }
            }
            if (Convert.ToInt32(HF_ACTONOTARIAL_POR_TARIFA.Value) == (int)Enumerador.enmProtocolarTipo.PODER_ESPECIAL)
            {
                if (Convert.ToInt32(hdn_tari_sTarifarioId.Value) == Constantes.CONST_PROTOCOLAR_ID_TARIFA_13B ||
                    Convert.ToInt32(hdn_tari_sTarifarioId.Value) == Constantes.CONST_PROTOCOLAR_ID_TARIFA_13C)
                {
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO PROTOCOLAR", "Esta tarifa se agrega desde la tarifa 13A.");
                    Comun.EjecutarScript(Page, strScript);
                    updRegPago.Update();
                    return;
                }
            }
            else if (Convert.ToInt32(HF_ACTONOTARIAL_POR_TARIFA.Value) == (int)Enumerador.enmProtocolarTipo.PODER_GENERAL_AMPLIO_ABSOLUTO)
            {
                if (Convert.ToInt32(hdn_tari_sTarifarioId.Value) == Constantes.CONST_PROTOCOLAR_ID_TARIFA_12B ||
                    Convert.ToInt32(hdn_tari_sTarifarioId.Value) == Constantes.CONST_PROTOCOLAR_ID_TARIFA_12C)
                {
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO PROTOCOLAR", "Esta tarifa se agrega desde la tarifa 12A.");
                    Comun.EjecutarScript(Page, strScript);
                    updRegPago.Update();
                    return;
                }
            }

            // Tarifas Únicas
            // 12A, 13A, 17B
            foreach (GridViewRow row in Gdv_Tarifa.Rows)
            {
                // Celda 5: Tarifa número y letra
                String Tarifa = row.Cells[5].Text.ToString();
                
                if (Tarifa.Trim() == Constantes.CONST_EXCEPCION_TARIFA_17B)
                {
                    ContadorParte += 1;
                }
            }


            //----------------------------------------------------------
            //Fecha: 25/05/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Validar las tárifas únicas 12A, 13A y 17B.
            //----------------------------------------------------------
            if (hdn_tari_sNumero.Value + hdn_tari_vLetra.Value == "12A" ||
                hdn_tari_sNumero.Value + hdn_tari_vLetra.Value == "13A" ||
                hdn_tari_sNumero.Value + hdn_tari_vLetra.Value == "17B")
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["tari_vNumero"].ToString() == hdn_tari_sNumero.Value + hdn_tari_vLetra.Value)
                    {
                        strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO PROTOCOLAR", "La tarifa ya se encuentra registrada.");
                        Comun.EjecutarScript(Page, strScript);
                        updRegPago.Update();
                        return;
                    }                    
                }
            }
            //----------------------------------------------------------

            // Tarifas con dependencias
            // 17B -> 17C        
            //if (Txt_TarifaId.Text == Constantes.CONST_EXCEPCION_TARIFA_17C)
            if (hdn_tari_sNumero.Value + hdn_tari_vLetra.Value == Constantes.CONST_EXCEPCION_TARIFA_17C)
            {
                if (ContadorParte == 0 && Lst_Tarifario.Items[1].Enabled)
                {
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO PROTOCOLAR", "No puede ingresar la tarifa 17C si no ha ingresado antes la tarifa 17B.");
                    Comun.EjecutarScript(Page, strScript);
                    updRegPago.Update();
                    return;
                }
            }
            //DILIGENCIA FUERA DE LA OFICINA CONSULAR
            //if (hdn_tari_sNumero.Value.ToString().Equals("30"))
            //{
            //    CalculoxTarifarioxTipoPagoxCantidad();
            //}
            #endregion

            CargarTipoPagoNormaTarifario();

            Int32 intActoNotarialTipo = Convert.ToInt32(HF_ACTONOTARIAL_POR_TARIFA.Value);
            if (intActuacionDetallePosicion < 0)
            {   // INSERTAR
                #region Actuacion Detalle Insertar
                dr = dt.NewRow();

                dr["acde_iActuacionDetalleId"] = 0;
                dr["acde_sTarifarioId"] = hdn_tari_sTarifarioId.Value.ToString();
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
                dr["pago_sMonedaLocalId"] = Session[Constantes.CONST_SESION_TIPO_MONEDA_ID].ToString();

                if (Convert.ToInt32(hdn_tari_sCalculoTipoId.Value) != (int)Enumerador.enmTipoCalculoTarifario.MONTO_FIJO)
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

                dr["tari_sCalculoTipoId"] = Convert.ToInt32(hdn_tari_sCalculoTipoId.Value);
                dr["tari_sNumero"] = Convert.ToInt32(hdn_tari_sNumero.Value);
                dr["tari_vLetra"] = Convert.ToString(hdn_tari_vLetra.Value);
                dr["tari_vProporcion"] = Txt_TarifaProporcional.Text.Trim();

                if (Convert.ToInt32(hdn_tari_sCalculoTipoId.Value) != (int)Enumerador.enmTipoCalculoTarifario.MONTO_FIJO)
                {
                    dr["tari_fCantidad"] = "1";
                }
                else
                {
                    dr["tari_fCantidad"] = Txt_TarifaCantidad.Text;
                }

                dt.Rows.Add(dr);

                #region tarifa 12A y 13A
                if (intActoNotarialTipo == (int)Enumerador.enmProtocolarTipo.PODER_GENERAL_AMPLIO_ABSOLUTO ||
                            intActoNotarialTipo == (int)Enumerador.enmProtocolarTipo.PODER_ESPECIAL)
                {
                    if (Convert.ToInt32(hdn_tari_sTarifarioId.Value) == Constantes.CONST_PROTOCOLAR_ID_TARIFA_13A
                        || Convert.ToInt32(hdn_tari_sTarifarioId.Value) == Constantes.CONST_PROTOCOLAR_ID_TARIFA_12A)
                    {
                        //Inserta la tarifa 12A y 13A de acuerdo a la cantidad de otorgantes y apoderados.
                        CargarActuacionesPorParticipantes(Convert.ToInt32(HF_ACTONOTARIAL_POR_TARIFA.Value), ref dt);
                    }
                }
                #endregion

                // Cantidad : Mas de 1                 
                if (Convert.ToInt32(hdn_tari_sCalculoTipoId.Value) == (int)Enumerador.enmTipoCalculoTarifario.MONTO_FIJO)
                {
                    #region Tipo de Calculo es Monto Fijo

                    int intCantidad = Convert.ToInt32(Txt_TarifaCantidad.Text);
                    DataRow drNuevo;
                    for (int i = 1; i < intCantidad; i++)
                    {
                        drNuevo = dt.NewRow();
                        drNuevo["acde_iActuacionDetalleId"] = 0;
                        drNuevo["acde_sTarifarioId"] = hdn_tari_sTarifarioId.Value.ToString();
                        drNuevo["acde_sItem"] = 1;
                        drNuevo["acde_dFechaRegistro"] = DateTime.Now;
                        drNuevo["acde_bRequisitosFlag"] = 0;
                        drNuevo["acde_ICorrelativoActuacion"] = 0;
                        drNuevo["acde_ICorrelativoTarifario"] = 0;
                        drNuevo["acde_bImpresionFlag"] = 0;
                        drNuevo["acde_dImpresionFecha"] = DateTime.Now;
                        drNuevo["acde_vNotas"] = string.Empty;

                        drNuevo["acde_iActuacionDetalleId"] = 0;
                        drNuevo["tari_vNumero"] = hdn_tari_sNumero.Value + hdn_tari_vLetra.Value;
                        drNuevo["tari_vDescripcionCorta"] = Txt_TarifaDescripcion.Text;
                        drNuevo["tari_FCosto"] = Txt_MtoSC.Text;
                        drNuevo["pago_dFechaOperacion"] = DateTime.Now;
                        drNuevo["pago_sMonedaLocalId"] = Session[Constantes.CONST_SESION_TIPO_MONEDA_ID].ToString();

                        if (Convert.ToInt32(hdn_tari_sCalculoTipoId.Value) != (int)Enumerador.enmTipoCalculoTarifario.MONTO_FIJO)
                        {
                            drNuevo["pago_FMontoMonedaLocal"] = Convert.ToDouble(Txt_TotML.Text);
                            drNuevo["pago_FMontoSolesConsulares"] = Convert.ToDouble(Txt_TotSC.Text);
                        }
                        else
                        {
                            drNuevo["pago_FMontoMonedaLocal"] = Convert.ToDouble(Txt_MontML.Text);
                            drNuevo["pago_FMontoSolesConsulares"] = Convert.ToDouble(Txt_MtoSC.Text);
                        }

                        drNuevo["pago_FTipCambioBancario"] = Session[Constantes.CONST_SESION_TIPO_CAMBIO_BANCARIO].ToString();
                        drNuevo["pago_FTipCambioConsular"] = Session[Constantes.CONST_SESION_TIPO_CAMBIO].ToString();
                        drNuevo["pago_sPagoTipoId"] = Convert.ToInt16(ddlTipoPago.SelectedValue);

                        drNuevo["tari_sCalculoTipoId"] = Convert.ToInt32(hdn_tari_sCalculoTipoId.Value);
                        drNuevo["tari_sNumero"] = Convert.ToInt32(hdn_tari_sNumero.Value);
                        drNuevo["tari_vLetra"] = Convert.ToString(hdn_tari_vLetra.Value);
                        drNuevo["tari_vProporcion"] = Txt_TarifaProporcional.Text.Trim();
                        drNuevo["tari_fCantidad"] = "1";

                        dt.Rows.Add(drNuevo);

                        #region tarifa 12A y 13A - MDIAZ
                        if (intActoNotarialTipo == (int)Enumerador.enmProtocolarTipo.PODER_GENERAL_AMPLIO_ABSOLUTO ||
                                    intActoNotarialTipo == (int)Enumerador.enmProtocolarTipo.PODER_ESPECIAL)
                        {
                            if (Convert.ToInt32(hdn_tari_sTarifarioId.Value) == Constantes.CONST_PROTOCOLAR_ID_TARIFA_13A
                                || Convert.ToInt32(hdn_tari_sTarifarioId.Value) == Constantes.CONST_PROTOCOLAR_ID_TARIFA_12A)
                            {
                                CargarActuacionesPorParticipantes(Convert.ToInt32(HF_ACTONOTARIAL_POR_TARIFA.Value), ref dt);
                            }
                        }
                        #endregion
                    }

                    Lbl_TotalGeneral.Text = (Convert.ToDouble(Lbl_TotalGeneral.Text) + Convert.ToDouble(dr["pago_FMontoSolesConsulares"]) * intCantidad).ToString();
                    txtMtoCancelado.Text = (Convert.ToDouble(Lbl_TotalGeneral.Text) + Convert.ToDouble(dr["pago_FMontoSolesConsulares"]) * intCantidad).ToString();
                    Lbl_TotalExtranjera.Text = (Convert.ToDouble(Lbl_TotalExtranjera.Text) + Convert.ToDouble(dr["pago_FMontoMonedaLocal"]) * intCantidad).ToString();
                    #endregion
                }
                else
                {
                    txtMtoCancelado.Text = (Convert.ToDouble(Lbl_TotalGeneral.Text) + Convert.ToDouble(dr["pago_FMontoSolesConsulares"])).ToString();
                    Lbl_TotalGeneral.Text = (Convert.ToDouble(Lbl_TotalGeneral.Text) + Convert.ToDouble(dr["pago_FMontoSolesConsulares"])).ToString();
                    Lbl_TotalExtranjera.Text = (Convert.ToDouble(Lbl_TotalExtranjera.Text) + Convert.ToDouble(dr["pago_FMontoMonedaLocal"])).ToString();
                }
                #endregion
            }

            Gdv_Tarifa.DataSource = dt;
            Gdv_Tarifa.DataBind();

            Session[strVarActuacionDetalleDT] = dt;

            Session[strVarActuacionDetalleIndice] = -1;
            //Txt_TarifaId.Text = string.Empty;
            //Txt_TarifaDescripcion.Text = string.Empty;

           // LimpiarDatosTarifaPago();

            CalcularTotalPago();
            ddlTipoPago.Enabled = true;
            updRegPago.Update();
        }

        protected void Txt_TarifaCantidad_TextChanged(object sender, EventArgs e)
        {
            CalculoxTarifarioxTipoPagoxCantidad();
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            BuscarTarifario();
        }

        protected void Lst_Tarifario_SelectedIndexChanged(object sender, EventArgs e)
        {
            string StrScript = string.Empty;

            if (Lst_Tarifario.SelectedIndex == -1)
            {
                return;
            }

            lblExoneracion.Visible = false;
            ddlExoneracion.Visible = false;
            lblValExoneracion.Visible = false;
            RBNormativa.Visible = false;
            RBSustentoTipoPago.Visible = false;

            lblSustentoTipoPago.Visible = false;
            txtSustentoTipoPago.Visible = false;
            txtSustentoTipoPago.Text = "";
            lblValSustentoTipoPago.Visible = false;


            if (Session["dtTarifarioFiltrado"] != null)
            {
                DataTable dtTarifarioFiltrado = (DataTable)Session["dtTarifarioFiltrado"];

                LimpiarDatosTarifaPago();

                if (Lst_Tarifario.SelectedValue != Convert.ToString(Constantes.CONST_EXCEPCION_TARIFA_ID_122))
                {
                    CargarObjetoTarifario(dtTarifarioFiltrado, Lst_Tarifario.SelectedIndex);

                    BE.MRE.SI_TARIFARIO objTarifarioBE = new BE.MRE.SI_TARIFARIO();
                    objTarifarioBE = (BE.MRE.SI_TARIFARIO)Session[strVariableTarifario];

                    this.Txt_TarifaId.Text = objTarifarioBE.tari_sNumero + objTarifarioBE.tari_vLetra;
                    this.Txt_TarifaDescripcion.Text = Lst_Tarifario.SelectedItem.Text;

                    CalculoxTarifarioxTipoPagoxCantidad();

                    if (Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA) ||
                        Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA) ||
                        Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA))
                    {
                        pnlPagLima.Visible = true;
                    }
                    else if (Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.NO_COBRADO) || Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
                    {
                        pnlPagLima.Visible = false;
                    }

                    Txt_TarifaCantidad.Enabled = true;
                    if (Convert.ToInt16(objTarifarioBE.tari_bHabilitaCantidad) == 0)
                    {
                        Txt_TarifaCantidad.Text = "1";
                        Txt_TarifaCantidad.Enabled = false;
                    }

                    #region Caso: Parte o Testimonio
                    Int64 intActoNotarialId = 0;
                    Int16 intCantidadFojas = 0;
                    Int64 intActuacionId = 0;
                    intActoNotarialId = Convert.ToInt64(hdn_acno_iActoNotarialId.Value);
                    intActuacionId = Convert.ToInt64(hdn_acno_iActuacionId.Value); 

                    Txt_TarifaCantidad.Enabled = true;

                    if (intActoNotarialId != 0)
                    {
                        if (Txt_TarifaId.Text.Equals(Constantes.CONST_EXCEPCION_TARIFA_17A))
                        {
                            //if (hdn_cant_fojas_escritura.Value == "0")
                            //{
                                intCantidadFojas = ObtenerCantidadFojasPorFormato(intActoNotarialId, intActuacionId);
                                hdn_cant_fojas_escritura.Value = intCantidadFojas.ToString();
                                hdn_cant_fojas_testimonio.Value = intCantidadFojas.ToString();
                            //}

                            Txt_TarifaCantidad.Text = hdn_cant_fojas_testimonio.Value;
                            Txt_TarifaCantidad_TextChanged(null, null);
                            //----------------------------------------
                            //Fecha: 19/08/2020
                            //Autor: Miguel Márquez Beltrán
                            //Motivo: Deshabilitar el texto cantidad.
                            //----------------------------------------
                            Txt_TarifaCantidad.Enabled = false;
                            //----------------------------------------
                        }
                        else if (Txt_TarifaId.Text.Equals(Constantes.CONST_EXCEPCION_TARIFA_17C))
                        {
                            //if (hdn_cant_fojas_parte.Value == "0")
                            //{
                                intCantidadFojas = ObtenerCantidadFojasPorFormato(intActoNotarialId, intActuacionId);
                                hdn_cant_fojas_parte.Value = intCantidadFojas.ToString();
                            //}

                            Txt_TarifaCantidad.Text = hdn_cant_fojas_parte.Value;
                            //Txt_TarifaCantidad_TextChanged(null, null);
                            CalculoxTarifarioxTipoPagoxCantidad();
                            //----------------------------------------
                            //Fecha: 19/08/2020
                            //Autor: Miguel Márquez Beltrán
                            //Motivo: Deshabilitar el texto cantidad.
                            //----------------------------------------
                            Txt_TarifaCantidad.Enabled = false;
                            //----------------------------------------

                        }
                    }
                    #endregion


                    if (Gdv_Tarifa.Rows.Count > 0)
                    {
                        ddlTipoPago.Enabled = true;
                    }
                    else
                    {
                        ddlTipoPago.SelectedIndex = 0;
                        ddlTipoPago.Enabled = false;
                        lblExoneracion.Visible = false;
                        ddlExoneracion.Enabled = false;
                        ddlExoneracion.Visible = false;
                        lblValExoneracion.Visible = false;
                        lblSustentoTipoPago.Visible = false;
                        txtSustentoTipoPago.Enabled = false;
                        txtSustentoTipoPago.Visible = false;
                        lblValSustentoTipoPago.Visible = false;
                    }


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

        protected void Txt_TarifaId_TextChanged(object sender, EventArgs e)
        {
            lblExoneracion.Visible = false;
            ddlExoneracion.Visible = false;
            lblValExoneracion.Visible = false;
            RBNormativa.Visible = false;
            RBSustentoTipoPago.Visible = false;

            lblSustentoTipoPago.Visible = false;
            txtSustentoTipoPago.Visible = false;
            txtSustentoTipoPago.Text = "";
            lblValSustentoTipoPago.Visible = false;

            BuscarTarifario();
        }

        protected void btn_ToolBar_Cuerpo_Click(object sender, EventArgs e)
        {
            try
            {
                if (Comun.ToNullInt64(hdn_acno_iActoNotarialId.Value) == 0)
                {
                    string strScript = string.Empty;
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Registro Notarial : Cuerpo", "Debe grabase el acto notarial previamente", false, 160, 300);
                    Comun.EjecutarScript(Page, strScript);
                }
                else
                {

                    CBE_CUERPO lCuerpo = new CBE_CUERPO();
                    #region Creando objecto ....
                    lCuerpo.ancu_iActoNotarialCuerpoId = Comun.ToNullInt64(hdn_ancu_iActoNotarialCuerpoId.Value);
                    lCuerpo.ancu_iActoNotarialId = Comun.ToNullInt64(hdn_acno_iActoNotarialId.Value);

                    lCuerpo.ancu_vCuerpo = RichTextBox.Text;

                    lCuerpo.ancu_vFirmaIlegible = txt_ancu_vFirmaIlegible.Text;
                    lCuerpo.ancu_bFlagExtraprotocolarCuerpo = false;
                    lCuerpo.ancu_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    lCuerpo.ancu_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
                    lCuerpo.ancu_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    lCuerpo.ancu_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();

                    lCuerpo.ActoNotarial.acno_iActoNotarialId = lCuerpo.ancu_iActoNotarialId;

                    ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();
                    RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();
                    lACTONOTARIAL.acno_iActoNotarialId = lCuerpo.ancu_iActoNotarialId;
                    lACTONOTARIAL = lActoNotarialConsultaBL.obtener(lACTONOTARIAL);
                    lACTONOTARIAL.acno_vAutorizacionTipo = txt_acno_sAutorizacionTipoId.Text;
                    lACTONOTARIAL.acno_vNumeroColegiatura = txt_acno_vNumeroColegiatura.Text;
                    lACTONOTARIAL.acno_sAutorizacionDocumentoTipoId = 1; // D.N.I.
                    lACTONOTARIAL.acno_vAutorizacionDocumentoNumero = txtAutorizacionNroDocumento.Text;

                    lCuerpo.ActoNotarial = lACTONOTARIAL;
                    #endregion

                    #region Imagenes
                    List<BE.MRE.RE_ACTONOTARIALDOCUMENTO> lstImagenes = (List<BE.MRE.RE_ACTONOTARIALDOCUMENTO>)HttpContext.Current.Session["ImagenesContainer"];
                    #endregion

                    ActoNotarialMantenimiento mnt = new ActoNotarialMantenimiento();
                    var s_Valor = mnt.Insertar_ActoNotarialCuerpo(lCuerpo, lstImagenes);
                    if (s_Valor.Identity != 0 && s_Valor.Message != null)
                    {
                        hdn_ancu_iActoNotarialCuerpoId.Value = s_Valor.Identity.ToString();
                        string strScript = string.Empty;
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Registro Notarial : Cuerpo", "El registro ha sido grabado correctamente", false, 160, 300);
                        Comun.EjecutarScript(Page, strScript);
                    }
                }
            }
            catch (Exception ex)
            {
                string strScript = string.Empty;
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Registro Notarial : Cuerpo", ex.Message);
                Comun.EjecutarScript(Page, strScript);
            }
        }

        void btnGrabar5_Click(object sender, EventArgs e)
        {
            Btn_RegistrarPago_Click(null, null);
            if (Convert.ToInt32(Session["Actuacion_Accion"]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
            {
                btnGrabarVinculacion.Enabled = false;
                btnLimpiarVinc.Enabled = false;//--vpipa
            }
        }

        protected void Btn_RegistrarPago_Click(object sender, EventArgs e)
        {
            //---------revalidar la disponibilidad de fojas, por lo que pudo haber ocupado las fojas por otro projecto posterior
            //autor: pipa
            //Fecha: 02/02/2022
            string strMensajeFojas = string.Empty;
            String iNumeroLibro = String.Empty;
            Int32 Fojainicial_actual = 0, FojaFinal = 0, acno_iNumeroEscritura = 0;
            Int16 NumeroHojas = Convert.ToInt16(this.HF_NroHojas.Value);
            Int16 NroFojaInicial_reservado = Convert.ToInt16(this.txtNroFojaIni.Text);
            Int16 NroFojaFinal_reservado = Convert.ToInt16(this.txtNroFojaFinal.Text);

            Int64 acno_iActoNotarialId=Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
            ActoNotarialConsultaBL bActoNotarialConsulta = new ActoNotarialConsultaBL();
            Int16 escritura_publica =Convert.ToInt16((int)Enumerador.enmNotarialLibroTipo.ESCRITURA_PUBLICA);
            Int16 oficinaConsularId=Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
            int resp = bActoNotarialConsulta.ValidarFojas(acno_iActoNotarialId, escritura_publica, oficinaConsularId,  NumeroHojas, 1,
                                                            ref strMensajeFojas, ref Fojainicial_actual, ref FojaFinal, ref acno_iNumeroEscritura, ref iNumeroLibro);

            if (NroFojaInicial_reservado != Fojainicial_actual)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTO NOTARIAL: PAGOS", "Las Fojas han sido ocupadas por otro proyecto, porfavor vuelva a realizar vista previa para recalcular la fojas"));
                return;
            }
            //-------------------------------------------------------
            //Fecha: 25/05/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Validar la selección obligatoria del tipo de pago.
            //-------------------------------------------------------
            if (Convert.ToInt16(ddlTipoPago.SelectedValue) <= 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTO NOTARIAL: PAGOS", "Seleccione el Tipo de Pago"));
                return;
            }
            //-------------------------------------------------------

            string strScript = string.Empty;
            Int32 ContadorTarifa = 0;
            List<BE.RE_ACTUACIONDETALLE> lstActuacionDetalle = new List<BE.RE_ACTUACIONDETALLE>();
            List<BE.RE_PAGO> lstPago = new List<BE.RE_PAGO>();

            RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();
            ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();

            lACTONOTARIAL.acno_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
            lACTONOTARIAL = lActoNotarialConsultaBL.obtener(lACTONOTARIAL);
            Int64 lngActuacionId = lACTONOTARIAL.acno_iActuacionId;

            hdn_acno_iActuacionId.Value = lngActuacionId.ToString();
            Session[Constantes.CONST_SESION_ACTUACION_ID] = lngActuacionId;

            if (lngActuacionId == 0)
            {
                return;
            }
            

            #region Validación Datos Solicitud Parte/Testimonio
            if (Convert.ToInt32(hdn_AccionOperacion.Value) == (int)Enumerador.enmNotarialAccion.SOLICITUD)
            {
                if (ddlAdicionalFuncionario.SelectedIndex == 0)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTO NOTARIAL: PAGOS", "No ha seleccionado al Funcionario Responsable.", false, 190, 250);
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }
                else if (ddlAdicionalTipoDoc.SelectedIndex == 0)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTO NOTARIAL: PAGOS", "No ha ingresado Tipo de Documento del Prepresentante.", false, 190, 250);
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }
                else if (txtAdicionalNumDoc.Text.Trim() == string.Empty)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTO NOTARIAL: PAGOS", "No ha ingresado Número de Documento del Presentante.", false, 190, 250);
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }
                else if ((Convert.ToInt32(ddlAdicionalTipoDoc.SelectedValue) == (int)Enumerador.enmTipoDocumento.DNI) && txtAdicionalNumDoc.Text.Length != 8)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTO NOTARIAL: PAGOS", "No ha ingresado Número de Documento válido.", false, 190, 250);
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }
                else if (txtAdicionalNombres.Text.Trim() == string.Empty)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTO NOTARIAL: PAGOS", "No ha ingresado Nombres y Apellidos del Presentante.", false, 190, 250);
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }
                else if (ddlAdicionalGenero.SelectedIndex == 0)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTO NOTARIAL: PAGOS", "No ha ingresado Género del Presentante.", false, 190, 250);
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }
            }
            #endregion

            DataTable dt = (DataTable)Session[strVarActuacionDetalleDT];
            if (dt != null)
            {
                #region Validación
                BE.RE_ACTUACIONDETALLE objActuacionDetalleBE;
                BE.RE_PAGO objPagoBE;

                foreach (DataRow dr in dt.Rows)
                {
                    if ((Convert.ToInt16(dr["acde_sTarifarioId"]) != Constantes.CONST_PROTOCOLAR_ID_TARIFA_17A) &&
                      (Convert.ToInt16(dr["acde_sTarifarioId"]) != Constantes.CONST_PROTOCOLAR_ID_TARIFA_17B) &&
                      (Convert.ToInt16(dr["acde_sTarifarioId"]) != Constantes.CONST_PROTOCOLAR_ID_TARIFA_17C))
                    {
                        ContadorTarifa++;
                    }
                }

                if (ContadorTarifa == 0)
                {
                    // Validación : No se hace cuando es una SOLICITUD DE parte/testimonio
                    if (Convert.ToInt32(hdn_AccionOperacion.Value) != (int)Enumerador.enmNotarialAccion.SOLICITUD)
                    {
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTO NOTARIAL: PAGOS", "FALTA AGREGAR TARIFA DE ESCRITURAS PÚBLICAS.", false, 190, 250);
                        Comun.EjecutarScript(Page, strScript);
                        return;
                    }
                }

                if (RBNormativa.Checked)
                {
                    if (ddlExoneracion.Visible == true && ddlExoneracion.Enabled == true)
                    {
                        if (ddlExoneracion.SelectedIndex == 0)
                        {
                            Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTO NOTARIAL: PAGOS", "Seleccione la Ley que exonera el Pago"));
                            return;
                        }
                    }
                }

                if (RBSustentoTipoPago.Checked)
                {
                    if (txtSustentoTipoPago.Visible == true && txtSustentoTipoPago.Text.Trim().Length == 0)
                    {
                        txtSustentoTipoPago.Enabled = true;
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTO NOTARIAL: PAGOS", "Digite el sustento"));
                        return;
                    }
                }
                //---------------------------------------------------------------------------
                bool bisSeccionIII = Comun.isSeccionIII(Session, Txt_TarifaId.Text);
                string strTarifa = Txt_TarifaId.Text.Trim().ToUpper();
                if (Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
                {
                    if (bisSeccionIII == false && strTarifa != "2")
                    {
                        if (txtSustentoTipoPago.Visible == true && txtSustentoTipoPago.Enabled == true && txtSustentoTipoPago.Text.Trim().Length == 0)
                        {
                            Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTO NOTARIAL: PAGOS", "Digite el sustento"));
                            return;
                        }
                    }
                }
                //---------------------------------------------------------------------------
                #endregion

                #region Carga de Lista de Actuación detalle y Lista de Pagos
                foreach (DataRow dr in dt.Rows)
                {
                    objActuacionDetalleBE = new BE.RE_ACTUACIONDETALLE();
                    objActuacionDetalleBE.acde_iActuacionDetalleId = 0;
                    objActuacionDetalleBE.acde_iActuacionId = lngActuacionId;
                    objActuacionDetalleBE.acde_sTarifarioId = Convert.ToInt16(dr["acde_sTarifarioId"]);
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

                    objPagoBE = new BE.RE_PAGO();
                    objPagoBE.pago_sPagoTipoId = Convert.ToInt16(ddlTipoPago.SelectedValue);
                    objPagoBE.pago_dFechaOperacion = DateTime.Now;

                    objPagoBE.pago_sMonedaLocalId = Comun.ObtenerMonedaLocalId(Session, ddlTipoPago.SelectedValue, dr["tari_vNumero"].ToString());


                    bool bNoCobrado = ExisteInafecto_Exoneracion(ddlTipoPago.SelectedValue);

                    if (bNoCobrado || Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.NO_COBRADO) || Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
                    {
                        objPagoBE.pago_FMontoMonedaLocal = 0;
                        objPagoBE.pago_FMontoSolesConsulares = 0;
                    }
                    else
                    {
                        objPagoBE.pago_FMontoMonedaLocal = Convert.ToDouble(dr["pago_FMontoMonedaLocal"]);
                        objPagoBE.pago_FMontoSolesConsulares = Convert.ToDouble(dr["pago_FMontoSolesConsulares"]);
                    }

                    objPagoBE.pago_FTipCambioBancario = Convert.ToDouble(dr["pago_FTipCambioBancario"]);
                    objPagoBE.pago_FTipCambioConsular = Convert.ToDouble(dr["pago_FTipCambioConsular"]);
                    objPagoBE.pago_bPagadoFlag = true;
                    objPagoBE.pago_vNumeroVoucher = Txt_VoucherNro.Text.Trim();
                    objPagoBE.pago_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                    objPagoBE.pago_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    objPagoBE.pago_vIPCreacion = Util.ObtenerDireccionIP();
                    objPagoBE.pago_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    objPagoBE.pago_vIPModificacion = Util.ObtenerDireccionIP();
                    objPagoBE.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                    objPagoBE.pago_vBancoNumeroOperacion = Convert.ToString(txtNroOperacion.Text.Trim().ToUpper());
                    objPagoBE.pago_sBancoId = Convert.ToInt16(ddlNomBanco.SelectedValue);

                    if (txtSustentoTipoPago.Visible == true)
                    {
                        objPagoBE.pago_vSustentoTipoPago = txtSustentoTipoPago.Text.Trim().ToUpper();
                    }
                    else
                    {
                        objPagoBE.pago_vSustentoTipoPago = "";
                    }

                    Int64 intNormaTarifarioId = 0;

                    if (ddlExoneracion.Visible == true)
                    {
                        intNormaTarifarioId = Convert.ToInt64(ddlExoneracion.SelectedValue);
                    }

                    objPagoBE.pago_iNormaTarifarioId = intNormaTarifarioId;

                    if (Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA) ||
                        Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA) ||
                        Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA))
                    {
                        if (ctrFecPago.Text != String.Empty)
                        {
                            objPagoBE.pago_dFechaOperacion = Comun.FormatearFecha(ctrFecPago.Text);
                        }
                    }

                    lstPago.Add(objPagoBE);
                }
                #endregion

                try
                {
                    #region Registro Actuacion Detalle
                    Int64 lonActuacionDetalleId = 0;
                    ActuacionMantenimientoBL objActuacionBL = new ActuacionMantenimientoBL();

                    Int16 intCantFojasParte = 0;
                    Int16 intCantFojasEscritura = 0;

                    intCantFojasParte = Convert.ToInt16(hdn_cant_fojas_parte.Value);
                    intCantFojasEscritura = Convert.ToInt16(hdn_cant_fojas_escritura.Value);

                    #region Solicitud Parte/Testimonio
                    if (Convert.ToInt32(hdn_AccionOperacion.Value) == (int)Enumerador.enmNotarialAccion.SOLICITUD)
                    {
                        lACTONOTARIAL.acno_vNumeroOficio = string.Empty;
                        lACTONOTARIAL.acno_IFuncionarioAutorizadorId = Convert.ToInt32(ddlAdicionalFuncionario.SelectedValue);
                        lACTONOTARIAL.acno_sPresentanteTipoDocumento = Convert.ToInt16(ddlAdicionalTipoDoc.SelectedValue);
                        lACTONOTARIAL.acno_vPresentanteNumeroDocumento = txtAdicionalNumDoc.Text.Trim();
                        lACTONOTARIAL.acno_vPresentanteNombre = txtAdicionalNombres.Text.Trim();
                        lACTONOTARIAL.acno_sPresentanteGenero = Convert.ToInt16(ddlAdicionalGenero.SelectedValue);
                    }
                    #endregion

                    int intResultado = objActuacionBL.InsertarActuacionDetalle(lstActuacionDetalle, lstPago, ref lonActuacionDetalleId, lACTONOTARIAL, intCantFojasParte, intCantFojasEscritura);
                    #endregion

                    updFormato.Update();
                    Session[Constantes.CONST_SESION_ACTUACIONDET_ID] = lonActuacionDetalleId;

                    // Obtener el estado y validar su cambio
                    if (lACTONOTARIAL.acno_sEstadoId == (Int16)Enumerador.enmNotarialProtocolarEstado.APROBADA)
                    {
                        ActoNotarialMantenimiento bl = new ActoNotarialMantenimiento();
                        RE_ACTONOTARIAL actonotarialBE = new RE_ACTONOTARIAL();
                        actonotarialBE.acno_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
                        actonotarialBE.acno_sEstadoId = (int)Enumerador.enmNotarialProtocolarEstado.PAGADA;
                        actonotarialBE.acno_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                        actonotarialBE.acno_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        actonotarialBE.acno_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
                        actonotarialBE.sTipoLibroId = escritura_publica;
                        actonotarialBE.acno_sNumeroHojas = NumeroHojas;
                        actonotarialBE.iNumeroEscrituraPublica = acno_iNumeroEscritura;

                        if (bl.ActoNotarialActualizarEstado(actonotarialBE) == false)
                        {
                            strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Actuación Notarial", "Información guardada satisfactoriamente.");

                            // Obtener La consulta por actuacion
                            ActoNotarialConsultaBL objActoNotarialBL = new ActoNotarialConsultaBL();
                            DataTable dtActuacionDetalle = new DataTable();
                            dtActuacionDetalle = objActoNotarialBL.ListarActuacionDetalle(lngActuacionId);

                            ctrlToolBar5.btnGrabar.Enabled = false;
                            ctrlToolBar5.btnCancelar.Enabled = false;
                            btnCancelarAprobacion.Enabled = false;
                            Gdv_Tarifa.DataSource = dtActuacionDetalle;
                            Gdv_Tarifa.DataBind();

                            #region Actualizar totales

                            bool bNoCobrado = ExisteInafecto_Exoneracion(ddlTipoPago.SelectedValue);


                            if (bNoCobrado || Convert.ToInt32(ddlTipoPago.SelectedValue) == (int)Enumerador.enmTipoCobroActuacion.GRATIS ||
                                Convert.ToInt32(ddlTipoPago.SelectedValue) == (int)Enumerador.enmTipoCobroActuacion.NO_COBRADO)
                            {
                                Txt_TotSC.Text = "0.00";
                                Txt_TotML.Text = "0.00";
                            }
                            #endregion

                            Session["HabilitarImpresion"] = "1";

                            ddlTipoPago.Enabled = false;
                            Txt_VoucherNro.Enabled = false;
                            Txt_TarifaId.Enabled = false;
                            Btn_AgregarTarifa.Enabled = false;
                            btnLimpiarTarifa.Enabled = false;
                            btnPresentanteAgregar.Enabled = false;
                            GridViewPresentante.Enabled = false;

                            btnAgregarNorma.Enabled = false;
                            gdv_Normas.Enabled = false;

                            strScript = string.Empty;
                            strScript = @"$(function(){{
                                            HabilitarTabVinculacion();
                                        }});";
                            strScript = string.Format(strScript);
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitarTabVinculacion", strScript, true);

                            Int64 intActuacionId = Convert.ToInt64(this.hdn_acno_iActuacionId.Value);
                            BindGridActuacionesInsumoDetalle(intActuacionId);
                            //-------------------------------------------------------------------------
                            //Fecha: 02/04/2020
                            //Autor: Miguel Márquez Beltrán
                            //Motivo: Seleccionar el primer registro de la grilla.
                            //-------------------------------------------------------------------------
                            if (gdvVinculacion.Rows.Count > 0)
                            {
                                gdvVinculacion.SelectedIndex = 0;

                                GridViewRow row = gdvVinculacion.Rows[0];
                                ImageButton imgbutton = (ImageButton)row.FindControl("btnSeleccionar");

                                gdvVinculacion_RowCommand(gdvVinculacion, new GridViewCommandEventArgs(imgbutton, new CommandEventArgs("Select", imgbutton.CommandArgument)));
                            }
                            //-------------------------------------------------------------------------

                        }
                    }
                    else if (lACTONOTARIAL.acno_sEstadoId == (Int16)Enumerador.enmNotarialProtocolarEstado.DIGITALIZADA &&
                        Convert.ToInt32(hdn_AccionOperacion.Value) == (int)Enumerador.enmNotarialAccion.SOLICITUD)
                    {
                        #region Deshabilitar Tab de Aprobación y Pago
                        ctrlToolBar5.btnGrabar.Enabled = false;
                        ctrlToolBar5.btnCancelar.Enabled = false;
                        Btn_AgregarTarifa.Enabled = false;
                        btnLimpiarTarifa.Enabled = false;
                        Gdv_Tarifa.Enabled = false;
                        ddlAdicionalFuncionario.Enabled = false;
                        ddlAdicionalTipoDoc.Enabled = false;
                        ddlAdicionalGenero.Enabled = false;
                        txtAdicionalNumDoc.Enabled = false;
                        txtAdicionalNombres.Enabled = false;

                        Txt_TarifaCantidad.Enabled = false;
                        ddlTipoPago.Enabled = false;
                        #endregion

                        Int64 intActuacionId = Convert.ToInt64(this.hdn_acno_iActuacionId.Value);
                        BindGridActuacionesInsumoDetalle(intActuacionId, true);
                    }


                }
                catch (Exception ex)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuación Notarial", ex.Message);
                }

                EjecutarScript(strScript, "MensajePagoActuacionX");

                strScript = string.Empty;
                strScript = @"$(function(){{
                                    HabilitarTabVinculacion();
                                }});";
                strScript = string.Format(strScript);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitarTabVinculacion", strScript, true);
                //Page.Response.Redirect(Page.Request.Url.ToString(), false);
                //Context.ApplicationInstance.CompleteRequest();
            }

            if (Convert.ToInt32(Session["Actuacion_Accion"]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
            {
                btnGrabarVinculacion.Enabled = false;
                btnLimpiarVinc.Enabled = false;//--vpipa
            }

        }

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

            DataColumn dcMontoMonedaLocal = DtDetActuaciones.Columns.Add("pago_FMontoMonedaLocal", typeof(double));
            DataColumn dcMontoSolesConsulares = DtDetActuaciones.Columns.Add("pago_FMontoSolesConsulares", typeof(double));
            DataColumn dcTipoCambioBancario = DtDetActuaciones.Columns.Add("pago_FTipCambioBancario", typeof(double));
            DataColumn dcTipoCambioConsular = DtDetActuaciones.Columns.Add("pago_FTipCambioConsular", typeof(double));
            DataColumn dcPagoTipo = DtDetActuaciones.Columns.Add("pago_sPagoTipoId", typeof(int));

            DataColumn dcTarifaCalculo = DtDetActuaciones.Columns.Add("tari_sCalculoTipoId", typeof(int));
            DataColumn dcTarifaNumero = DtDetActuaciones.Columns.Add("tari_sNumero", typeof(int));
            DataColumn dcTarifaLetra = DtDetActuaciones.Columns.Add("tari_vLetra", typeof(string));
            DataColumn dcTarifaProporcion = DtDetActuaciones.Columns.Add("tari_vProporcion", typeof(string));
            DataColumn dcTarifaCantidad = DtDetActuaciones.Columns.Add("tari_fCantidad", typeof(double));

            return DtDetActuaciones;
        }

        private void CalcularTotalPago()
        {
            double dblTotalSolesConsulares = 0;
            double dblTotalMonedaLocal = 0;
            if ((DataTable)Session[strVarActuacionDetalleDT] != null)
            {
                DataTable dt = ((DataTable)Session[strVarActuacionDetalleDT]).Copy();
                foreach (DataRow dr in dt.Rows)
                {
                    dblTotalSolesConsulares += Convert.ToDouble(dr["pago_FMontoSolesConsulares"]);
                    dblTotalMonedaLocal += Convert.ToDouble(dr["pago_FMontoMonedaLocal"]);
                }
            }

            txtMtoCancelado.Text = dblTotalSolesConsulares.ToString(ConfigurationManager.AppSettings["FormatoMonto"].ToString());
            Lbl_TotalGeneral.Text = dblTotalSolesConsulares.ToString(ConfigurationManager.AppSettings["FormatoMonto"].ToString());
            Lbl_TotalExtranjera.Text = dblTotalMonedaLocal.ToString(ConfigurationManager.AppSettings["FormatoMonto"].ToString());
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
            Txt_TarifaCantidad.Text = "1";

            string strFormato = ConfigurationManager.AppSettings["FormatoMonto"].ToString();
            double dblCero = 0;
            Txt_MontML.Text = dblCero.ToString(strFormato);
            Txt_MtoSC.Text = dblCero.ToString(strFormato);
            Txt_TotML.Text = dblCero.ToString(strFormato);
            Txt_TotSC.Text = dblCero.ToString(strFormato);
        }

        private void CargarObjetoTarifario(DataTable dtTarifarioFiltrado, int intIndiceSeleccionado)
        {
            BE.MRE.SI_TARIFARIO objTarifarioBE = new BE.MRE.SI_TARIFARIO();

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

            Session[strVariableTarifario] = objTarifarioBE;
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

        private double CalculaCostoML(double decMontoSC, double decTipoCambio)
        {
            return (decMontoSC * decTipoCambio);
        }

        private void HabilitaPorTarifa()
        {
            double decMontoSC = 0;
            BE.MRE.SI_TARIFARIO objTarifarioBE = (BE.MRE.SI_TARIFARIO)Session[strVariableTarifario];
            decMontoSC = (double)objTarifarioBE.tari_FCosto;

            hdn_tari_sTarifarioId.Value = objTarifarioBE.tari_sTarifarioId.ToString();
            hdn_tari_sNumero.Value = objTarifarioBE.tari_sNumero.ToString();
            hdn_tari_vLetra.Value = objTarifarioBE.tari_vLetra.ToString();
            hdn_tari_sCalculoTipoId.Value = objTarifarioBE.tari_sCalculoTipoId.ToString();

            ddlTipoPago.Enabled = true;
            if (decMontoSC == 0)
            {
            }
            else
            {
                if (Txt_TarifaDescripcion.Text == string.Empty)
                {
                    ddlTipoPago.Enabled = false;
                }
                else
                {
                    if (ddlTipoPago.SelectedValue == "0")
                    {
                        Txt_TarifaId.Focus();
                    }
                }
            }

            Txt_TarifaProporcional.Text = string.Empty;
            switch (Convert.ToInt32(objTarifarioBE.tari_sCalculoTipoId))
            {
                case (int)Enumerador.enmTipoCalculoTarifario.MONTO_FIJO:
                    {
                        lblTarifaConsular2.Text = "Cantidad:";
                        break;
                    }
                case (int)Enumerador.enmTipoCalculoTarifario.PORCENTAJE:
                    {
                        /*El campo cantidad se convierte en monto directo*/
                        /*El label se cambia de texto a monto*/
                        lblTarifaConsular2.Text = "Monto:";
                        Txt_TarifaCantidad.MaxLength = 10;

                        // Porcentaje
                        Txt_TarifaProporcional.Text = objTarifarioBE.tari_FCosto.ToString();

                        break;
                    }
                case (int)Enumerador.enmTipoCalculoTarifario.FORMULA:
                    {
                        lblTarifaConsular2.Text = "Cantidad:";
                        break;
                    }
                default:
                    break;
            }
        }

        private void CalculoxTarifarioxTipoPagoxCantidad()
        {
            int intCantidad = 1;
            string strScript = string.Empty;
            string strDescripcionTarifa = string.Empty;
            double decMontoSC = 0, decTotalSC = 0;
            double decMontoML = 0, decTotalML = 0;

            if (Txt_TarifaCantidad.Text.Trim() == string.Empty)
                return;

            // Evalua habilitacion de controles:            
            if (Session[strVariableTarifario] == null)
            {
                return;
            }
            else
            {
                BE.MRE.SI_TARIFARIO objTarifarioBE = (BE.MRE.SI_TARIFARIO)Session[strVariableTarifario];
                if (objTarifarioBE.tari_sNumero == 0)
                {
                    return;
                }
                // Tarifario:
                if (string.IsNullOrEmpty(Txt_TarifaCantidad.Text))
                {
                    return;
                }

                // Tarifa de la Actuación:
                decMontoSC = (double)objTarifarioBE.tari_FCosto;

                HabilitaPorTarifa();

                if (!string.IsNullOrEmpty(Txt_TarifaCantidad.Text))
                {
                    intCantidad = Convert.ToInt32(Txt_TarifaCantidad.Text);
                }

                //if (Txt_TarifaCantidad.Enabled)
                //{
                //    Txt_TarifaCantidad.Focus();
                //}

                // Montos calculados:
                if (intCantidad > 0)
                {
                    decTotalSC = Tarifario.Calculo(objTarifarioBE, intCantidad);
                    decMontoML = CalculaCostoML(decMontoSC, Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]));
                    decTotalML = CalculaCostoML(decTotalSC, Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]));
                }

                // Asignando valores a los controles:
                Txt_TarifaCantidad.Text = intCantidad.ToString();
                string strFormato = ConfigurationManager.AppSettings["FormatoMonto"].ToString();

                if (objTarifarioBE.tari_sCalculoTipoId != (int)Enumerador.enmTipoCalculoTarifario.MONTO_FIJO)
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

        private void EjecutarScript(string script, string uniqueId)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), uniqueId, script, true);
        }

        private void EjecutarScript(string script)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPopUp", script, true);
        }

        private void BuscarTarifario()
        {
            string StrScript = string.Empty;
            string strDescripcionTarifa = string.Empty;

            LimpiarListaTarifa();

            lblExoneracion.Visible = false;
            ddlExoneracion.Visible = false;
            lblValExoneracion.Visible = false;
            RBNormativa.Visible = false;
            RBSustentoTipoPago.Visible = false;

            lblSustentoTipoPago.Visible = false;
            txtSustentoTipoPago.Visible = false;
            txtSustentoTipoPago.Text = "";
            lblValSustentoTipoPago.Visible = false;

            if (this.Txt_TarifaId.Text.Length > 0)
            {
                DataTable dtTarifarioFiltrado = (DataTable)Session["dtTarifarioFiltrado"];
                dtTarifarioFiltrado = BindListTarifario(this.Txt_TarifaId.Text, ref strDescripcionTarifa);

                if (dtTarifarioFiltrado != null)
                {
                    this.Txt_TarifaDescripcion.Text = strDescripcionTarifa;

                    if (dtTarifarioFiltrado.Rows.Count > 0)
                    {
                        int intSeccionId = Convert.ToInt32(dtTarifarioFiltrado.Rows[0]["tari_sSeccionId"]);
                        if (intSeccionId == (int)Enumerador.enmSeccion.ACTO_NOTARIAL)
                        {
                            CargarObjetoTarifario(dtTarifarioFiltrado, 0);
                            CargarListaTarifario(dtTarifarioFiltrado);

                            if (dtTarifarioFiltrado.Rows.Count == 1)
                            {
                                this.Txt_TarifaId.Text = dtTarifarioFiltrado.Rows[0]["tari_sNumero"].ToString() + dtTarifarioFiltrado.Rows[0]["tari_vLetra"].ToString().ToUpper();
                                CalculoxTarifarioxTipoPagoxCantidad();
                            }
                            Txt_TarifaCantidad.Enabled = true;
                            if (dtTarifarioFiltrado.Rows[0]["tari_sTarifarioId"].ToString() == Constantes.CONST_EXCEPCION_TARIFA_ID_122.ToString())
                            {
                                Session["NuevoRegistro"] = true;

                                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", "Esta tarifa solo se aplica en el RUNE.", false, 190, 250);
                                EjecutarScript(StrScript);

                                this.Txt_TarifaDescripcion.Text = String.Empty;
                                this.Txt_TarifaId.Text = string.Empty;

                                return;
                            }
                            else
                            {
                                if (dtTarifarioFiltrado.Rows[0]["tari_sTarifarioId"].ToString() == Constantes.CONST_PROTOCOLAR_ID_TARIFA_17A.ToString() || dtTarifarioFiltrado.Rows[0]["tari_sTarifarioId"].ToString() == Constantes.CONST_PROTOCOLAR_ID_TARIFA_17B.ToString())
                                {
                                    Txt_TarifaCantidad.Text = "1";
                                    Txt_TarifaCantidad.Enabled = false;
                                }
                            }
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

            Lst_Tarifario.Enabled = true;
            Txt_TarifaId.Focus();

            updRegPago.Update();
        }

        private DataTable BindListTarifario(string IntSeccionId, ref string strDescripcion)
        {
            DataTable dtTarifario;
            int NroRegistros = 0;

            object[] arrParametros = { 0, this.Txt_TarifaId.Text, "", ((char)Enumerador.enmEstado.ACTIVO).ToString(),
                                       1, 50, 0, 0 };

            dtTarifario = comun_Part2.ObtenerTarifario(Session, ref arrParametros);
            Session.Remove("dtTarifarioFiltrado");

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
                    Session.Add("dtTarifarioFiltrado", dtTarifario);
                }
            }

            return dtTarifario;
        }

        
        private void CargarActuacionesPorParticipantes(int intTipoActo, ref DataTable dt)
        {

            //Int32 intCantidadOtorgante = ObtenerCantidadParticipantesPorTipo(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE);
            //Int32 intCantidadAporado = ObtenerCantidadParticipantesPorTipo(Enumerador.enmNotarialProtocolarTipoParticipante.APODERADO);
            Int32 intCantidadOtorgante = ObtenerCantidadParticipantesPorTipo("INICIA");
            Int32 intCantidadAporado = ObtenerCantidadParticipantesPorTipo("RECIBE");

            if (intCantidadOtorgante == 1 && intCantidadAporado == 1)
                return;

            DataRow drNuevo;
            BE.MRE.SI_TARIFARIO objTarifa = new BE.MRE.SI_TARIFARIO();
            TarifarioConsultasBL objBL = new TarifarioConsultasBL();

            double dblTipoCambioConsular = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO].ToString());

            #region Otorgantes
            if (intTipoActo == (int)Enumerador.enmProtocolarTipo.PODER_GENERAL_AMPLIO_ABSOLUTO)
            {
                objTarifa = objBL.ObtenerTarifaPorId(Constantes.CONST_PROTOCOLAR_ID_TARIFA_12B);
            }
            else if (intTipoActo == (int)Enumerador.enmProtocolarTipo.PODER_ESPECIAL)
            {
                objTarifa = objBL.ObtenerTarifaPorId(Constantes.CONST_PROTOCOLAR_ID_TARIFA_13B);
            }
            else if (intTipoActo == (int)Enumerador.enmProtocolarTipo.ANTICIPO_HERENCIA)
            {
                objTarifa = objBL.ObtenerTarifaPorId(Constantes.CONST_PROTOCOLAR_ID_TARIFA_11B);
            }

            if (objTarifa.tari_sTarifarioId != 0)
            {
                for (int i = 1; i < intCantidadOtorgante; i++)
                {
                    #region Actuacion
                    drNuevo = dt.NewRow();
                    drNuevo["acde_iActuacionDetalleId"] = 0;
                    drNuevo["acde_sTarifarioId"] = objTarifa.tari_sTarifarioId;
                    drNuevo["acde_sItem"] = 1;
                    drNuevo["acde_dFechaRegistro"] = DateTime.Now;
                    drNuevo["acde_bRequisitosFlag"] = 0;
                    drNuevo["acde_ICorrelativoActuacion"] = 0;
                    drNuevo["acde_ICorrelativoTarifario"] = 0;
                    drNuevo["acde_bImpresionFlag"] = 0;
                    drNuevo["acde_dImpresionFecha"] = DateTime.Now;
                    drNuevo["acde_vNotas"] = string.Empty;

                    drNuevo["acde_iActuacionDetalleId"] = 0;
                    drNuevo["tari_vNumero"] = objTarifa.tari_sNumero + objTarifa.tari_vLetra;
                    drNuevo["tari_vDescripcionCorta"] = objTarifa.tari_vDescripcionCorta;
                    drNuevo["tari_FCosto"] = objTarifa.tari_FCosto;
                    drNuevo["pago_dFechaOperacion"] = DateTime.Now;
                    drNuevo["pago_sMonedaLocalId"] = Session[Constantes.CONST_SESION_TIPO_MONEDA_ID].ToString();

                    drNuevo["pago_FMontoMonedaLocal"] = objTarifa.tari_FCosto * dblTipoCambioConsular;
                    drNuevo["pago_FMontoSolesConsulares"] = objTarifa.tari_FCosto;

                    drNuevo["pago_FTipCambioBancario"] = Session[Constantes.CONST_SESION_TIPO_CAMBIO_BANCARIO].ToString();
                    drNuevo["pago_FTipCambioConsular"] = Session[Constantes.CONST_SESION_TIPO_CAMBIO].ToString();
                    drNuevo["pago_sPagoTipoId"] = Convert.ToInt16(ddlTipoPago.SelectedValue);

                    drNuevo["tari_sCalculoTipoId"] = objTarifa.tari_sCalculoTipoId;
                    drNuevo["tari_sNumero"] = objTarifa.tari_sNumero;
                    drNuevo["tari_vLetra"] = objTarifa.tari_vLetra;
                    drNuevo["tari_vProporcion"] = "";
                    drNuevo["tari_fCantidad"] = "1";

                    dt.Rows.Add(drNuevo);
                    #endregion
                }
            }
            #endregion

            #region Apoderados
            objTarifa = new BE.MRE.SI_TARIFARIO();
            if (intTipoActo == (int)Enumerador.enmProtocolarTipo.PODER_GENERAL_AMPLIO_ABSOLUTO)
            {
                objTarifa = objBL.ObtenerTarifaPorId(Constantes.CONST_PROTOCOLAR_ID_TARIFA_12C);
            }
            else if (intTipoActo == (int)Enumerador.enmProtocolarTipo.PODER_ESPECIAL)
            {
                objTarifa = objBL.ObtenerTarifaPorId(Constantes.CONST_PROTOCOLAR_ID_TARIFA_13C);
            }

            if (objTarifa.tari_sTarifarioId != 0)
            {
                for (int i = 1; i <= intCantidadAporado; i++)
                {
                    #region Actuacion
                    drNuevo = dt.NewRow();
                    drNuevo["acde_iActuacionDetalleId"] = 0;
                    drNuevo["acde_sTarifarioId"] = objTarifa.tari_sTarifarioId;
                    drNuevo["acde_sItem"] = 1;
                    drNuevo["acde_dFechaRegistro"] = DateTime.Now;
                    drNuevo["acde_bRequisitosFlag"] = 0;
                    drNuevo["acde_ICorrelativoActuacion"] = 0;
                    drNuevo["acde_ICorrelativoTarifario"] = 0;
                    drNuevo["acde_bImpresionFlag"] = 0;
                    drNuevo["acde_dImpresionFecha"] = DateTime.Now;
                    drNuevo["acde_vNotas"] = string.Empty;

                    drNuevo["acde_iActuacionDetalleId"] = 0;
                    drNuevo["tari_vNumero"] = objTarifa.tari_sNumero + objTarifa.tari_vLetra;
                    drNuevo["tari_vDescripcionCorta"] = objTarifa.tari_vDescripcionCorta;
                    drNuevo["tari_FCosto"] = objTarifa.tari_FCosto;
                    drNuevo["pago_dFechaOperacion"] = DateTime.Now;
                    drNuevo["pago_sMonedaLocalId"] = Session[Constantes.CONST_SESION_TIPO_MONEDA_ID].ToString();

                    drNuevo["pago_FMontoMonedaLocal"] = objTarifa.tari_FCosto * dblTipoCambioConsular;
                    drNuevo["pago_FMontoSolesConsulares"] = objTarifa.tari_FCosto;

                    drNuevo["pago_FTipCambioBancario"] = Session[Constantes.CONST_SESION_TIPO_CAMBIO_BANCARIO].ToString();
                    drNuevo["pago_FTipCambioConsular"] = Session[Constantes.CONST_SESION_TIPO_CAMBIO].ToString();
                    drNuevo["pago_sPagoTipoId"] = Convert.ToInt16(ddlTipoPago.SelectedValue);

                    drNuevo["tari_sCalculoTipoId"] = objTarifa.tari_sCalculoTipoId;
                    drNuevo["tari_sNumero"] = objTarifa.tari_sNumero;
                    drNuevo["tari_vLetra"] = objTarifa.tari_vLetra;
                    drNuevo["tari_vProporcion"] = "";
                    drNuevo["tari_fCantidad"] = "1";

                    dt.Rows.Add(drNuevo);
                    #endregion
                }
            }
            #endregion

        }

        /// <summary>
        /// Obtener Cantidad de Participantes por tipo indicado
        /// </summary>
        /// <param name="enmParticipante">Tipo de participante</param>
        /// <returns>Total de participantes</returns>
        private static int ObtenerCantidadParticipantesPorTipo(string strIniciaRecibe="")
        {
            int intCantidad = 0;

            List<CBE_PARTICIPANTE> loPARTICIPANTES = (List<CBE_PARTICIPANTE>)HttpContext.Current.Session["ParticipanteContainer"];
            loPARTICIPANTES = loPARTICIPANTES.Where(x => x.anpa_cEstado != "E").ToList();

            foreach (CBE_PARTICIPANTE participante in loPARTICIPANTES)
            {
                if (strIniciaRecibe.Length==0)
                    intCantidad++;
                else
                {
                        //if (participante.anpa_sTipoParticipanteId == (int)Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE
                        //    || participante.anpa_sTipoParticipanteId == (int)Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE
                        //    || participante.anpa_sTipoParticipanteId == (int)Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE
                        //    || participante.anpa_sTipoParticipanteId == (int)Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR)

                    if (ObtenerIniciaRecibe(participante.anpa_sTipoParticipanteId) == strIniciaRecibe)
                    {
                        intCantidad++;
                    }
                        //if (participante.anpa_sTipoParticipanteId == (int)Enumerador.enmNotarialProtocolarTipoParticipante.APODERADO
                        //    || participante.anpa_sTipoParticipanteId == (int)Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPADO
                        //    || participante.anpa_sTipoParticipanteId == (int)Enumerador.enmNotarialProtocolarTipoParticipante.DONATARIO
                        //    || participante.anpa_sTipoParticipanteId == (int)Enumerador.enmNotarialProtocolarTipoParticipante.COMPRADOR)

                }
            }
            return intCantidad;
        }
        #endregion

        #region Pestaña: Vinculacion

        /// <summary>
        /// Lista de tarifas por tipo de acto notarial 
        /// </summary>
        /// <param name="tipoActo"></param>
        /// <param name="tipoModificatoria">8421: Acción de Registro</param>
        /// <returns></returns>
        //public DataTable cargar_tarifas(string tipoActo, string tipoModificatoria = "8421")
        //{
        //    DataTable dtTarifario = new DataTable();
        //    DataTable dtFiltrado = new DataTable();
        //    //ActoNotarialConsultaBL oActoNotarialConsultaBL = new ActoNotarialConsultaBL();
        //    //dtFiltrado = oActoNotarialConsultaBL.ObtenerTarifaTipoActo(Convert.ToInt16(tipoActo));

        //    TipoActoProtocolarTarifarioConsultasBL objTipoActoProtocolarTarifarioConsultaBL = new TipoActoProtocolarTarifarioConsultasBL();
        //    short intTipoActoProtocolarTarifarioId = 0;
        //    short intTarifaId = 0;
        //    int IntTotalCount = 0;
        //    int IntTotalPages = 0;
        //    short intTipoActoProtocolarId = Convert.ToInt16(tipoActo);

        //    dtFiltrado = objTipoActoProtocolarTarifarioConsultaBL.Consultar_TipoActoProtocolarTarifario(intTipoActoProtocolarTarifarioId, intTipoActoProtocolarId, intTarifaId, 10000, "1", "N", ref IntTotalCount, ref IntTotalPages);

        //    return dtFiltrado;
        //}

        //-----------------------------------------------
        //Fecha: 06/12/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Consultar las tarifas de la sección 2.
        //-----------------------------------------------

        public static DataTable cargar_tarifas()
        {
            DataTable dtTarifario = new DataTable();

            TarifarioConsultasBL TarifarioBL = new TarifarioConsultasBL();

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            dtTarifario = TarifarioBL.Consultar(2, "", 33, "1", 1000, ref IntTotalCount, ref IntTotalPages, true);

            return dtTarifario;
        }


        protected void ctrlPagActuacionInsumoDetalle_Click(object sender, EventArgs e)
        {
            BindGridActuacionesInsumoDetalle(Convert.ToInt64(hdn_acno_iActuacionId.Value));
            updFormato.Update();
        }

        protected void btnGrabarVinculacion_Click(object sender, EventArgs e)
        {
            try
            {
                string strScript = string.Empty;
                string strMensaje = string.Empty;
                int intResultado = 0;

                int intSel = Convert.ToInt32(hdn_seleccionado.Value);

                Int64 intActoNotarialDetalleId = Convert.ToInt64(hdn_actonotarialdetalle_id.Value);
                Int64 intActuacionId = Convert.ToInt64(hdn_actuacion_id.Value);
                Int64 intActuacionDetalleId = Convert.ToInt64(hdn_actuaciondetalle_id.Value);
                Int32 intTipoFormato = Convert.ToInt32(hdn_tipo_formato_proto.Value);
                Int64 intActoNotarialId = Convert.ToInt64(hdn_acno_iActoNotarialId.Value);

                DateTime dFecActual = Util.ObtenerFechaActual(
                                                Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sDiferenciaHoraria")),
                                                Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sHorarioVerano")));

                ActuacionMantenimientoBL oActuacionMantenimientoBL = oActuacionMantenimientoBL = new ActuacionMantenimientoBL();
                if (intTipoFormato == (int)Enumerador.enmNotarialTipoFormato.ESCRITURA)
                {
                    intResultado = oActuacionMantenimientoBL.VincularAutoadhesivo(intActuacionId, intActuacionDetalleId, (int)Enumerador.enmInsumoTipo.AUTOADHESIVO,
                        txtCodigoInsumo.Text, dFecActual, false, dFecActual, 0, Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                        Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]), ref strMensaje, Enumerador.enmNotarialTipoFormato.ESCRITURA);
                }
                else
                {
                    #region Actualizar Datos Notariales del Parte/Testimonio
                    if (intTipoFormato == (int)Enumerador.enmNotarialTipoFormato.PARTE || intTipoFormato == (int)Enumerador.enmNotarialTipoFormato.TESTIMONIO)
                    {
                        if (intActoNotarialDetalleId != 0 && txtNumeroOficioAdi.Text.Trim() != string.Empty && intActoNotarialId > 0)
                        {
                            if (HF_ValidaTablaFuncionarioResponsable.Value == "S")
                            {
                                int intFuncionarioResponsable = Convert.ToInt32(ddlFuncionarioResponsable.SelectedValue);
                                ActualizarDatosAcnoNotarialDetalle(intActoNotarialDetalleId, txtNumeroOficioAdi.Text.Trim(), intFuncionarioResponsable, intActuacionId);
                            }
                            else
                            {
                                ActualizarDatosAcnoNotarialDetalle(intActoNotarialDetalleId, txtNumeroOficioAdi.Text.Trim(), 0, intActuacionId);
                            }
                        }
                    }
                    #endregion

                    intResultado = oActuacionMantenimientoBL.VincularAutoadhesivo(intActuacionId, intActuacionDetalleId, (int)Enumerador.enmInsumoTipo.AUTOADHESIVO,
                        txtCodigoInsumo.Text, dFecActual, false, dFecActual, 0, Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                        Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]), ref strMensaje);
                }

                //---------------------------------------------
                //Fecha: 06/01/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Deshabilitar el codigo del insumo.
                //----------------------------------------------
                //txtCodigoInsumo.Enabled = true;
                if (strMensaje != string.Empty)
                {

                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "VINCULACIÓN", strMensaje, false, 200, 400);
                    Comun.EjecutarScript(Page, strScript);
                }
                else
                {
                    btnLimpiarVinc.Enabled = false;
                    txtCodigoInsumo.Enabled = false;
                    txtCodigoInsumo.Text = "";
                    btnGrabarVinculacion.Enabled = false;
                    txtNumeroOficioAdi.Visible = false;
                    lblNumeroOficioPaso.Visible = false;
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION,
                                "VINCULACIÓN", "La vinculación se realizó correctamente.", false, 200, 300);
                    Comun.EjecutarScript(Page, strScript);

                    if (Convert.ToInt32(hdn_AccionOperacion.Value) == (int)Enumerador.enmNotarialAccion.SOLICITUD)
                        BindGridActuacionesInsumoDetalle(Convert.ToInt64(hdn_acno_iActuacionId.Value), true);
                    else
                        BindGridActuacionesInsumoDetalle(Convert.ToInt64(hdn_acno_iActuacionId.Value));


                }


                btnAutoadhesivo.Enabled = true;
                //LimpiarDatosVinculacion();

                #region Validar Vinculación Completa
                bool bolFalta = true;
                string strCodigo = "";
                foreach (GridViewRow gdr in gdvVinculacion.Rows)
                {
                    strCodigo = Convert.ToString(Page.Server.HtmlDecode(gdr.Cells[10].Text)).Trim();
                    if (strCodigo != string.Empty)
                    {
                        bolFalta = false;
                    }
                    else
                    {
                        bolFalta = true;
                        break;
                    }
                }
                if (!bolFalta)
                {
                    #region Cambiar Estado : VINCULADA
                    Session["tab_activa_vinculacion"] = "NO";

                    RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();
                    ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();

                    lACTONOTARIAL.acno_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
                    lACTONOTARIAL = lActoNotarialConsultaBL.obtener(lACTONOTARIAL);

                    if (lACTONOTARIAL.acno_sEstadoId == (Int16)Enumerador.enmNotarialProtocolarEstado.PAGADA)
                    {
                        ActoNotarialMantenimiento bl = new ActoNotarialMantenimiento();
                        RE_ACTONOTARIAL actonotarialBE = new RE_ACTONOTARIAL();
                        actonotarialBE.acno_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
                        actonotarialBE.acno_sEstadoId = (int)Enumerador.enmNotarialProtocolarEstado.VINCULADA;
                        actonotarialBE.acno_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                        actonotarialBE.acno_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        actonotarialBE.acno_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();

                        if (bl.ActoNotarialActualizarEstado(actonotarialBE) == false)
                        {
                            LimpiarDatosVinculacion();
                            btnGrabarVinculacion.Enabled = false;
                            btnLimpiarVinc.Enabled = false;//--vpipa
                            Habilitar_Tab((int)Enumerador.enmNotarialProtocolarEstado.VINCULADA);

                            strScript = @"$(function(){{
                                                        HabilitarTabDigitalizac();
                                                    }});";
                            strScript = string.Format(strScript);
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitarTabDigitalizac", strScript, true);
                        }
                    }
                    updFormato.Update();
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                string strScript = string.Empty;
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Registro Notarial - Grabar Vinculación.", ex.Message);
                Comun.EjecutarScript(Page, strScript);

            }
        }



        /// <summary>
        /// Eventos para el manejo de la impresión del Autuadhesivo 
        /// </summary>
        /// <param name="iActuacionId"></param>
        private void BindGridActuacionesInsumoDetalle(Int64 iActuacionId, bool bolSoloNoVinculados = false)
        {
            ActoNotarialConsultaBL objBL = new ActoNotarialConsultaBL();
            DataTable dtActuacionInsumoDetalle = new DataTable();

            dtActuacionInsumoDetalle = objBL.ObtenerActoNotarialDetalle(iActuacionId, null, 0, bolSoloNoVinculados);

            #region Cargar Actuación Detalle Ids - insumos por vincular
            string strActuacionDetalleIds = hdn_vActuacionDetalleIds.Value;
            if (strActuacionDetalleIds == "" && bolSoloNoVinculados)
            {
                foreach (DataRow dr in dtActuacionInsumoDetalle.Rows)
                {
                    strActuacionDetalleIds += dr["ande_iActuacionDetalleId"] + ",";
                }
                if (strActuacionDetalleIds.Trim() != string.Empty)
                {
                    strActuacionDetalleIds = strActuacionDetalleIds.Substring(0, strActuacionDetalleIds.Length - 1);
                    hdn_vActuacionDetalleIds.Value = strActuacionDetalleIds;
                }
                dtActuacionInsumoDetalle = objBL.ObtenerActoNotarialDetalle(iActuacionId, 0, 0, bolSoloNoVinculados, strActuacionDetalleIds);
            }
            else if (bolSoloNoVinculados)
            {
                strActuacionDetalleIds = hdn_vActuacionDetalleIds.Value;
                dtActuacionInsumoDetalle = objBL.ObtenerActoNotarialDetalle(iActuacionId, 0, 0, bolSoloNoVinculados, strActuacionDetalleIds);
            }
            #endregion

            ///gdvVinculacion.SelectedIndex = -1;
            ///

            gdvVinculacion.DataSource = dtActuacionInsumoDetalle;
            gdvVinculacion.DataBind();

            gdvVinculacion.SelectedIndex = Convert.ToInt32(hdn_seleccionado.Value);

            Session["CodAutoadhesivo"] = null;

            // Limpiar Vinculación

            chkImpresionCorrecta.Checked = false;
            //txtCodigoInsumo.Text = "";
            txtNumeroOficioAdi.Text = "";
            btnFormato.Visible = true;
            // --


            DataTable dtActuacionInsumoDetalle_ = new DataTable();
            SGAC.Registro.Actuacion.BL.ActuacionMantenimientoBL objActuacionMantenimientoBL = new SGAC.Registro.Actuacion.BL.ActuacionMantenimientoBL();
            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_PAGE_SIZE_ADJUNTOS;
            int PaginaActual = 1;
            String strFlag = String.Empty;
            dtActuacionInsumoDetalle_ = objActuacionMantenimientoBL.Obtener_ActuacionInsumoDetalle(Convert.ToInt64(hdn_actuaciondetalle_id.Value), PaginaActual, intPaginaCantidad, ref IntTotalCount, ref  IntTotalPages);

            if (dtActuacionInsumoDetalle_ != null)
            {
                if (dtActuacionInsumoDetalle_.Rows.Count > 0)
                {
                    Session[Constantes.CONST_ACTUACION_INSUMO_DETALLE_ID] = dtActuacionInsumoDetalle_.Rows[0]["aide_iActuacionInsumoDetalleId"].ToString();
                }
            }

            //--------------------------------------------
            //Fecha: 06/01/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Desactivar el botón autoadhesivo y
            //          activar cuando se haya grabado.
            //--------------------------------------------

            //btnAutoadhesivo.Enabled = true;
            //---------------------------------------------
            //Fecha: 06/01/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Deshabilitar el codigo del insumo.
            //----------------------------------------------
            //txtCodigoInsumo.Enabled = true;
            if (strFlag.Equals("SI"))
            {
                chkImpresionCorrecta.Checked = true;
                btnAutoadhesivo.Enabled = false;
                hnd_ImpresionCorrecta.Value = "1";
                txtCodigoInsumo.Enabled = false;
            }


            updFormato.Update();
        }

        private short ObtenerIndiceListaParticipante(int iRowIndex)
        {
            if (iRowIndex < 0)
            {
                return -1;
            }

            string vNumeroDocumento = grd_Participantes.Rows[iRowIndex].Cells[ObtenerIndiceColumnaGrilla(grd_Participantes, "peid_vDocumentoNumero")].Text;
            short sDocumentoTipoId = Int16.Parse(grd_Participantes.Rows[iRowIndex].Cells[ObtenerIndiceColumnaGrilla(grd_Participantes, "peid_sDocumentoTipoId")].Text);

            //short sTipoParticipanteId = Int16.Parse(grd_Participantes.Rows[iRowIndex].Cells[ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_iActoNotarialParticipanteId")].Text);
            short sTipoParticipanteId = Int16.Parse(grd_Participantes.Rows[iRowIndex].Cells[ObtenerIndiceColumnaGrilla(grd_Participantes, "anpa_sTipoParticipanteId")].Text);

            List<CBE_PARTICIPANTE> loParticipanteContainer = (List<CBE_PARTICIPANTE>)Session["ParticipanteContainer"];
            var auxParticipanteContainerItem = loParticipanteContainer.Where(
                x => x.Identificacion.peid_sDocumentoTipoId == sDocumentoTipoId &&
                    x.anpa_cEstado != "E" &&
                 x.Identificacion.peid_vDocumentoNumero.Trim() == vNumeroDocumento.Trim());

            if (auxParticipanteContainerItem.Count() == 1)
            {
                return (short)loParticipanteContainer.IndexOf(auxParticipanteContainerItem.ElementAt(0));
            }

            return -1;
        }

        #region Acta de Conformidad
        protected void btn_confirmar_Click(object sender, EventArgs e)
        {
            try
            {
                string codPersona = Request.QueryString["CodPer"].ToString();
                string strUrl = "";
                bool resultado = ImprimirActaConformidadServer(Convert.ToInt32(HF_FORMATO_ACTIVO.Value));
                if (resultado)
                {
                    if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                    {
                        strUrl = "../Accesorios/VisorPDF.aspx?CodPer=" + codPersona + "&Juridica=1";
                    }
                    else
                    { // PERSONA NATURAL
                        strUrl = "../Accesorios/VisorPDF.aspx?CodPer=" + codPersona;
                    }

                }

                //if (HFGUID.Value.Length > 0)
                //{
                //    strUrl = "../Registro/FrmActaConformidadActoNotarial.aspx?GUID=" + HFGUID.Value;
                //}
                //else
                //{
                if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                {
                    strUrl = "../Registro/FrmActaConformidadActoNotarial.aspx?CodPer=" + codPersona + "&Juridica=1";
                }
                else
                { // PERSONA NATURAL
                    strUrl = "../Registro/FrmActaConformidadActoNotarial.aspx?CodPer=" + codPersona;
                }

                //}


                string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=500,height=700,left=(screen.width-500)/2,top=(screen.height-700)/2');";
                Comun.EjecutarScript(Page, strScript);
            }
            catch (Exception ex)
            {
            }
        }

        [System.Web.Services.WebMethod]
        public static Boolean ImprimirActaConformidad(Int32 intTipoActa, string strNOMBRE_RECURRENTE, string strDOCUMENTO, string strTipoFormato, string strtituloActaConformidad)
        {
            Boolean Resultado = false;
            //------------------------------------------------------
            //Fecha: 07/08/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Obtener el tipo de formato.
            //------------------------------------------------------
            int intFormatoSel = Convert.ToInt32(strTipoFormato);
            //------------------------------------------------------

            #region Datos de Recurrente

            String vNombreRecurrente = string.Empty;
            String vDocumento = string.Empty;


            if (strNOMBRE_RECURRENTE != null)
            {
                if (strNOMBRE_RECURRENTE.Trim() != string.Empty)
                {
                    vNombreRecurrente = strNOMBRE_RECURRENTE.Trim();
                }
            }
            if (strDOCUMENTO != null)
            {
                if (strDOCUMENTO.Trim() != string.Empty)
                {
                    vDocumento = strDOCUMENTO.Trim();
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
            sContenido.Append(ObtenerDocumentoConformidad(vNombreRecurrente, vDocumento, intTipoActa, intFormatoSel, strtituloActaConformidad));

            sScript.Append(sContenido);
            #endregion

            #region Impresión

            DataTable dtTMPReemplazar = new DataTable();
            dtTMPReemplazar = _CrearTmpTabla();

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
            objetos[0] = Convert.ToString(HttpContext.Current.Server.HtmlDecode(vNombreRecurrente));
            objetos[1] = vDocumento;
            objetos[2] = true;

            listObjects.Add(objetos);

            #endregion

            CreateFilePDFConformidad(dtTMPReemplazar, strRutaHtml, strRutaPDF, HttpContext.Current.Server.MapPath("~/Images/Escudo.PNG"), listObjects);

            if (File.Exists(strRutaHtml))
            {
                File.Delete(strRutaHtml);
            }

            if (System.IO.File.Exists(strRutaPDF))
            {
                WebClient User = new WebClient();
                Byte[] FileBuffer = User.DownloadData(strRutaPDF);
                if (FileBuffer != null)
                {
                    HttpContext.Current.Session["binaryData"] = FileBuffer;

                    Resultado = true;
                }
            }
            #endregion

            return Resultado;
        }


        private Boolean ImprimirActaConformidadServer(Int32 intTipoActa)
        {
            Boolean Resultado = false;

            #region Datos de Recurrente

            String vNombreRecurrente = string.Empty;
            String vDocumento = string.Empty;

            string strGUID = strClaveGUID;

            //---------------------------------------------
            //Fecha: 07/08/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Obtener el tipo de formato.
            //---------------------------------------------
            int intFormatoSel = Convert.ToInt32(hdn_tipo_formato_proto.Value);
            //---------------------------------------------

            if (HttpContext.Current.Session["Nombres" + strGUID] != null)
            {
                if (HttpContext.Current.Session["Nombres" + strGUID].ToString().Trim() != string.Empty)
                {
                    vNombreRecurrente += Comun.AplicarInicialMayuscula(HttpContext.Current.Session["Nombres" + strGUID].ToString());
                }
            }

            if (HttpContext.Current.Session["ApePat" + strGUID] != null)
            {
                if (HttpContext.Current.Session["ApePat" + strGUID].ToString().Trim() != string.Empty)
                {
                    vNombreRecurrente += " " + Comun.AplicarInicialMayuscula(HttpContext.Current.Session["ApePat" + strGUID].ToString());
                }
            }

            if (HttpContext.Current.Session["ApeMat" + strGUID] != null)
            {
                if (HttpContext.Current.Session["ApeMat" + strGUID].ToString().Trim() != string.Empty)
                {
                    vNombreRecurrente += " " + Comun.AplicarInicialMayuscula(HttpContext.Current.Session["ApeMat" + strGUID].ToString());
                }
            }


            if (HttpContext.Current.Session["DescTipDoc" + strGUID] != null)
            {
                if (HttpContext.Current.Session["DescTipDoc" + strGUID].ToString().Trim() != string.Empty)
                {
                    vDocumento += HttpContext.Current.Session["DescTipDoc" + strGUID].ToString();
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
            sContenido.Append(ObtenerDocumentoConformidad(vNombreRecurrente, vDocumento, intTipoActa, intFormatoSel));

            sScript.Append(sContenido);
            #endregion

            #region Impresión

            DataTable dtTMPReemplazar = new DataTable();
            dtTMPReemplazar = _CrearTmpTabla();

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
            objetos[0] = Convert.ToString(HttpContext.Current.Server.HtmlDecode(vNombreRecurrente));
            objetos[1] = vDocumento;
            objetos[2] = true;

            listObjects.Add(objetos);

            #endregion

            CreateFilePDFConformidad(dtTMPReemplazar, strRutaHtml, strRutaPDF, HttpContext.Current.Server.MapPath("~/Images/Escudo.PNG"), listObjects);

            if (File.Exists(strRutaHtml))
            {
                File.Delete(strRutaHtml);
            }

            if (System.IO.File.Exists(strRutaPDF))
            {
                WebClient User = new WebClient();
                Byte[] FileBuffer = User.DownloadData(strRutaPDF);
                if (FileBuffer != null)
                {
                    HttpContext.Current.Session["binaryData"] = FileBuffer;

                    Resultado = true;
                }
            }
            #endregion

            return Resultado;
        }
        private static void CreateFilePDFConformidad(DataTable TablaText, string HtmlPath, string PdfPath, string imgServerPAth, List<object[]> listFirmas, bool bAplicarCierreTexto = false)
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
                //iTextSharp.text.pdf.PdfPRow oPdfPRow;
                iTextSharp.text.pdf.PdfPCell oPdfPCell = null;
                //iTextSharp.text.Chunk oChunk;

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
                    frase.Add(new iTextSharp.text.Chunk("\n" + firma[0].ToString().ToUpper()));
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

        private static String ObtenerDocumentoConformidad(String vNombre, String vDocumento, int TipoActa, int intFormatoSel, string strtituloActaConformidad="")
        {
            bool bEsMujer = false;

            string strGUID = strClaveGUID;


            if (HttpContext.Current.Session["PER_GENERO" + strGUID] != null)
            {
                if (HttpContext.Current.Session["PER_GENERO" + strGUID].ToString() == Convert.ToInt32(Enumerador.enmGenero.FEMENINO).ToString())
                {
                    bEsMujer = true;
                }
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

            //sScript.Append(" CON ");
            //---------------------------------------
            //Autor: Miguel Márquez Beltrán.
            //Fecha: 22/05/2020
            //Obs: Se corrige según la observación No.2
            //---------------------------------------
            sScript.Append(" con ");
            //---------------------------------------

            sScript.Append(vDocumento.ToUpper());
            sScript.Append(", ");
            sScript.Append("declaro que he leído y revisado en su detalle el documento de ");

            //--------------------------------------------------------
            //Fecha: 11/01/2022
            //Autor: Miguel Márquez Beltrán
            //Motivo: Debe generarse un acta de conformidad
            //          para la Escritura Pública, Parte o Testimonio.
            //--------------------------------------------------------
            if (strtituloActaConformidad.Length > 0)
            {
                sScript.Append(strtituloActaConformidad);
            }
            else
            {
                switch (intFormatoSel)
                {
                    case (int)Enumerador.enmNotarialTipoFormato.ESCRITURA:
                        sScript.Append("ESCRITURA PÚBLICA");
                        break;
                    case (int)Enumerador.enmNotarialTipoFormato.PARTE:
                        sScript.Append("PARTE");
                        break;

                    case (int)Enumerador.enmNotarialTipoFormato.TESTIMONIO:
                        sScript.Append("TESTIMONIO");
                        break;
                    default:
                        sScript.Append("PODER GENERAL AMPLIO ABSOLUTO");
                        break;
                }
            }
            //--------------------------------------------------------

            //if (TipoActa == (int)Enumerador.enmProtocolarTipo.ANTICIPO_HERENCIA)
            //{
            //    sScript.Append("ANTICIPO HERENCIA");
            //}
            //else if (TipoActa == (int)Enumerador.enmProtocolarTipo.COMPRA_VENTA)
            //{
            //    sScript.Append("COMPRA VENTA");
            //}
            //else if (TipoActa == (int)Enumerador.enmProtocolarTipo.CONTRATOS_DE_LOCACIÓN_DE_SERVICIOS_OTROS)
            //{
            //    sScript.Append("CONTRATOS DE LOCACIÓN DE SERVICIOS OTROS");
            //}
            //else if (TipoActa == (int)Enumerador.enmProtocolarTipo.CONVENIO_ARBITRAL_DESIGNACIÓN_ARBITRO)
            //{
            //    sScript.Append("CONVENIO ARBITRAL DESIGNACIÓN ARBITRO");
            //}
            //else if (TipoActa == (int)Enumerador.enmProtocolarTipo.CONVENIOS_ARBITRALES_ACREEDORES_DEUDORES)
            //{
            //    sScript.Append("CONVENIOS ARBITRALES ACREEDORES DEUDORES");
            //}
            //else if (TipoActa == (int)Enumerador.enmProtocolarTipo.CUALQUIER_ACTO_JURIDICO_NO_ESPECIFICADO)
            //{
            //    sScript.Append("CUALQUIER ACTO JURÍDICO NO ESPECIFICADO");
            //}
            //else if (TipoActa == (int)Enumerador.enmProtocolarTipo.PODER_ESPECIAL)
            //{
            //    sScript.Append("PODER ESPECIAL");
            //}
            //else if (TipoActa == (int)Enumerador.enmProtocolarTipo.PODER_GENERAL_AMPLIO_ABSOLUTO)
            //{
            //    sScript.Append("PODER GENERAL AMPLIO ABSOLUTO");                
            //}
            //else if (TipoActa == (int)Enumerador.enmProtocolarTipo.RECONOCIMIENTO_PATERNIDAD)
            //{
            //    sScript.Append("RECONOCIMIENTO PATERNIDAD");
            //}
            //else if (TipoActa == (int)Enumerador.enmProtocolarTipo.RENDICIÓN_CUENTAS_TUTELA_CURATELA)
            //{
            //    sScript.Append("RENDICIÓN CUENTAS TUTELA CURATELA");
            //}
            //else if (TipoActa == (int)Enumerador.enmProtocolarTipo.RENUNCIA_NACIONALIDAD)
            //{
            //    sScript.Append("RENUNCIA NACIONALIDAD");
            //}
            //else if (TipoActa == (int)Enumerador.enmProtocolarTipo.SUSTITUCIÓN_REGIMEN_CONYUGAL)
            //{
            //    sScript.Append("SUSTITUCIÓN REGIMEN CONYUGAL");
            //}
            //else if (TipoActa == (int)Enumerador.enmProtocolarTipo.TESTAMENTO_ESCRITURA_PÚBLICA)
            //{
            //    sScript.Append("TESTAMENTO ESCRITURA PÚBLICA");
            //}
            //else if (TipoActa == (int)Enumerador.enmProtocolarTipo.TESTAMENTOS_CERRADOS)
            //{
            //    sScript.Append("TESTAMENTOS CERRADOS");
            //}


            sScript.Append(", que he tenido a la vista y me ha sido entregado en la fecha,");
            sScript.Append(" manifestando mi conformidad con su contenido.");
            sScript.Append("</p>");
            sScript.Append("<br />");

            sScript.Append("<p align=\"right\"; style=\"background-color:transparent; font-family:arial;\">");
            DateTime dt_Fecha = Comun.FormatearFecha(Accesorios.Comun.ObtenerFechaActualTexto(HttpContext.Current.Session));
            string str_Fecha = dt_Fecha.ToString("dd") + " de " + Comun.AplicarInicialMayuscula(dt_Fecha.ToString("MMMM")) + " de " + dt_Fecha.ToString("yyyy");

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

        static DataTable _CrearTmpTabla()
        {
            DataTable dtTablaTemporal = new DataTable();

            dtTablaTemporal.Columns.Add("strCadenaBuscar", typeof(string));
            dtTablaTemporal.Columns.Add("strCadenaReemplazar", typeof(string));

            return dtTablaTemporal;
        }

        #endregion

        #region Vinculación "n" tarifas

        protected void btnAutoadhesivo_Click(object sender, EventArgs e)
        {
            if (hdn_seleccionado.Value == "-1")
                return;

            #region Autoadhesivo Protocolar
            Session["ACTO_NOTARIAL"] = 1;
            #endregion
            Session["tab_activa_vinculacion"] = "SI";


            #region Habilitar ingreso de datos vinculación
            if (txtCodigoInsumo.Text.Trim() == string.Empty)
                chkImpresionCorrecta.Enabled = true;
            else
                chkImpresionCorrecta.Enabled = false;
            #endregion

            Int64 intActuacionId = Convert.ToInt64(hdn_actuacion_id.Value);
            Int64 intActuacionDetalleId = Convert.ToInt64(hdn_actuaciondetalle_id.Value);

            int intCantActu = 1;
            int intModoVista = (int)Enumerador.enmModoVista.HTML;
            object objModoVista = ConfigurationManager.AppSettings["ModoVistaAutoadhesivo"];
            if (objModoVista != null)
            {
                if (objModoVista.ToString().Trim() != string.Empty)
                {
                    intModoVista = Convert.ToInt32(objModoVista);
                }
            }
            if (ConfigurationManager.AppSettings["CantidadTarifaAutodhesivo"] != null)
            {
                intCantActu = Convert.ToInt32(ConfigurationManager.AppSettings["CantidadTarifaAutodhesivo"].ToString());
            }

            switch (intModoVista)
            {
                case (int)Enumerador.enmModoVista.ITEXT_SHARP:

                    DocumentoiTextSharp oDocumentoiTextSharp = new DocumentoiTextSharp(this.Page, string.Empty, HttpContext.Current.Server.MapPath("~/Images/Escudo.JPG"));
                    oDocumentoiTextSharp.Notarial = true;
                    oDocumentoiTextSharp.ActuacionId = intActuacionId;
                    oDocumentoiTextSharp.ActuacionDetalleId = intActuacionDetalleId;
                    oDocumentoiTextSharp.CantidadTarifas = intCantActu;
                    oDocumentoiTextSharp.CrearAutoAdhesivo();

                    break;
                default:

                    Session[Constantes.CONST_SESION_ACTUACION_ID] = intActuacionId;
                    Session[Constantes.CONST_SESION_ACTUACIONDET_ID] = intActuacionDetalleId;

                    if (btnFormato.Text.Contains("Escritura"))
                        Session["Autoadhesivo_titulo"] = "AUTOADHESIVO";
                    else
                        Session["Autoadhesivo_titulo"] = btnFormato.Text;

                    //string strUrl = "../Registro/FrmRepAutoadhesivoProtocolar.aspx";
                    //string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=0,width=500,height=700,left=100,top=100');";
                    string strScript = "abrirVentana('../Registro/FrmRepAutoadhesivoProtocolar.aspx?GUID=" + HFGUID.Value + "', 'AUTOADHESIVOS', 610, 450, '');";

                    Comun.EjecutarScript(Page, strScript);

                    break;
            }

            updFormato.Update();
        }

        protected void gdvVinculacion_RowCommand(object sender, GridViewCommandEventArgs e)
        {            
            int IntSeleccionado = Convert.ToInt32(e.CommandArgument);
            hdn_seleccionado.Value = IntSeleccionado.ToString();
            int indice = ObtenerIndice(IntSeleccionado);
            if (e.CommandName == "Select")
            {
                string strNotas = gdvVinculacion.DataKeys[IntSeleccionado].Values["acde_vNotas"].ToString();
                string strCodUnicoFabrica = gdvVinculacion.DataKeys[IntSeleccionado].Values["insu_vCodigoUnicoFabrica"].ToString();

                #region Color Selección
                int fila = 0;
                foreach (GridViewRow row in gdvVinculacion.Rows)
                {
                    if (fila % 2 == 0) row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    if (fila % 2 != 0) row.BackColor = ColorTranslator.FromHtml("#CCCCCC");
                    fila++;
                }
                gdvVinculacion.Rows[IntSeleccionado].BackColor = System.Drawing.ColorTranslator.FromHtml("#656873");
                #endregion

                hdn_actonotarialdetalle_id.Value = gdvVinculacion.Rows[IntSeleccionado].Cells[0].Text;
                hdn_actuacion_id.Value = gdvVinculacion.Rows[IntSeleccionado].Cells[1].Text;
                hdn_actuaciondetalle_id.Value = gdvVinculacion.Rows[IntSeleccionado].Cells[2].Text;
                hdn_tipo_formato_proto.Value = gdvVinculacion.Rows[IntSeleccionado].Cells[3].Text;
                hdn_correlativo.Value = gdvVinculacion.Rows[IntSeleccionado].Cells[5].Text;

                int intFormatoSel = Convert.ToInt32(gdvVinculacion.Rows[IntSeleccionado].Cells[3].Text);
                int intCorrelativo = Convert.ToInt32(gdvVinculacion.Rows[IntSeleccionado].Cells[5].Text);

                String strFlag = gdvVinculacion.Rows[IntSeleccionado].Cells[9].Text.ToString();


                #region Habilitar/Limpiar Campos
                //--------------------------------------------
                //Fecha: 06/01/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Desactivar el botón autoadhesivo y
                //          activar cuando se haya grabado.
                //--------------------------------------------

                //btnAutoadhesivo.Enabled = true;

                string strCodigo = Convert.ToString(Page.Server.HtmlDecode(gdvVinculacion.Rows[IntSeleccionado].Cells[10].Text)).Trim();
                txtNumeroOficioAdi.Text = Convert.ToString(Page.Server.HtmlDecode(gdvVinculacion.Rows[IntSeleccionado].Cells[8].Text)).Trim();

                hCodAutoadhesivo.Value  = Convert.ToString(Page.Server.HtmlDecode(gdvVinculacion.Rows[IntSeleccionado].Cells[11].Text)).Trim();


                chkImpresionCorrecta.Checked = false;
                string yy = DateTime.Now.Year.ToString();
                //txtNroOficio.Text = txtNroEscritura.Text.Trim() + "/" + yy.Substring(2, 2);

                if (txtNumeroOficioAdi.Text.Trim().Length == 0)
                {
                    txtNumeroOficioAdi.Text = txtNroEscritura.Text.Trim() + "/" + yy.Substring(2, 2);
                }


                chkImpresionCorrecta.Enabled = false;
                txtCodigoInsumo.Enabled = false;

                btnFormato.Visible = true;
                MostrarSoloParte(false);
                btnLimpiarVinc.Enabled = false;
                if (strCodigo == "")
                {
                    btnLimpiarVinc.Enabled = true;
                }

                if (strCodigo == string.Empty)
                {
                    btnAutoadhesivo.Enabled = false;
                    txtNumeroOficioAdi.Enabled = true;
                    btnGrabarVinculacion.Enabled = true;                    
                    ctrlReimprimirbtn1.Activar = false;
                    ctrlBajaAutoadhesivo1.Activar = false;
                }
                else
                {
                    btnAutoadhesivo.Enabled = true;
                    txtNumeroOficioAdi.Enabled = false;
                    btnGrabarVinculacion.Enabled = false;
                    chkImpresionCorrecta.Checked = true;
                    txtCodigoInsumo.Text = strCodigo;
                    ctrlBajaAutoadhesivo1.Activar = true;                
                }
                #endregion

                switch (intFormatoSel)
                {
                    case (int)Enumerador.enmNotarialTipoFormato.ESCRITURA:
                        btnFormato.Text = "Escritura Pública";
                        break;
                    case (int)Enumerador.enmNotarialTipoFormato.PARTE:
                        btnFormato.Text = "Parte " + intCorrelativo;
                        MostrarSoloParte(true);
                                               
                        break;
                    case (int)Enumerador.enmNotarialTipoFormato.TESTIMONIO:
                        btnFormato.Text = "Testimonio " + intCorrelativo;
                        break;
                    default:
                        btnFormato.Visible = false;
                        btnFormato.Text = "";
                        break;
                }

                if (intFormatoSel == (int)Enumerador.enmNotarialTipoFormato.PARTE ||
                    intFormatoSel == (int)Enumerador.enmNotarialTipoFormato.TESTIMONIO)
                {
                    #region Habilitar/Deshabilitar tabla Funcionario
                    bool bHabilitaTablaFuncionario = false;
                    if (strNotas.Length > 0 && strCodUnicoFabrica.Length == 0)
                    {
                        //------------------------------------------
                        //Solo aquellos que se crearon desde la
                        //pantalla de consulta de actos notariales.
                        //------------------------------------------
                        yy = DateTime.Now.Year.ToString();

                        txtNumeroOficioAdi.Text = txtNroEscritura.Text.Trim() + "/" + yy.Substring(2, 2);
                        if (strNotas.Equals("CONSULTA"))
                        {
                            bHabilitaTablaFuncionario = true;
                            string strScript = string.Empty;
                            strScript = @"$(function(){{
                                            HabilitarTablaFuncionarioResponsable(); 
                                            }});";
                            strScript = string.Format(strScript);
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitarTablaFuncionarioResponsable", strScript, true);
                        }
                    }

                    if (bHabilitaTablaFuncionario == false)
                    {
                        string strScript = string.Empty;
                        strScript = @"$(function(){{
                                            DeshabilitarTablaFuncionarioResponsable(); 
                                            }});";
                        strScript = string.Format(strScript);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "DeshabilitarTablaFuncionarioResponsable", strScript, true);
                    }
                    #endregion
                }

                chkImpresionCorrecta.Enabled = false;
                txtCodigoInsumo.Enabled = false;
                chkImpresionCorrecta.Checked = false;
                //---------------------------------------------
                //Fecha: 06/01/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Deshabilitar el codigo del insumo.
                //----------------------------------------------
                //txtCodigoInsumo.Enabled = true;
                if (strCodigo.Trim().Length > 0)
                {

                    DataTable dtActuacionInsumoDetalle = new DataTable();
                    SGAC.Registro.Actuacion.BL.ActuacionMantenimientoBL objActuacionMantenimientoBL = new SGAC.Registro.Actuacion.BL.ActuacionMantenimientoBL();
                    int IntTotalCount = 0;
                    int IntTotalPages = 0;
                    int intPaginaCantidad = Constantes.CONST_PAGE_SIZE_ADJUNTOS;
                    int PaginaActual = 1;

                    dtActuacionInsumoDetalle = objActuacionMantenimientoBL.Obtener_ActuacionInsumoDetalle(Convert.ToInt64(hdn_actuaciondetalle_id.Value), PaginaActual, intPaginaCantidad, ref IntTotalCount, ref  IntTotalPages);

                    if (dtActuacionInsumoDetalle != null)
                    {
                        if (dtActuacionInsumoDetalle.Rows.Count > 0)
                        {
                            Session[Constantes.CONST_ACTUACION_INSUMO_DETALLE_ID] = dtActuacionInsumoDetalle.Rows[0]["aide_iActuacionInsumoDetalleId"].ToString();
                        }
                    }


                    btnAutoadhesivo.Enabled = true;
                    txtCodigoInsumo.Enabled = false;
                    if (strFlag.Equals("SI"))
                    {
                        chkImpresionCorrecta.Checked = true;
                        btnAutoadhesivo.Enabled = false;
                        hnd_ImpresionCorrecta.Value = "1";
                        ctrlReimprimirbtn1.Activar = true;
                    }
                    else
                    {
                        //--------------------------------------------
                        //Fecha: 06/01/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Desactivar el botón reimprimir
                        //--------------------------------------------

                        ctrlReimprimirbtn1.Activar = false;
                    }
                }
                else
                {
                    CargarUltimoInsumo();
                    btnAutoadhesivo.Enabled = false;
                }

            }
        }

        protected void btnLimpiarVinc_Click(object sender, EventArgs e)
        {
            LimpiarDatosVinculacion();
        }

        [System.Web.Services.WebMethod]
        public static string ImprimirVistaPrevia(Int64 AcnoNotarialId, int TipoFormato, int Correlativo, String NumOficio, Int64 ActoNotarialDetalleId, String vVistaPrevia)
        {
            switch (TipoFormato)
            {
                case (int)Enumerador.enmNotarialTipoFormato.ESCRITURA:
                    return VerFormatoEscrituraPublica(AcnoNotarialId, vVistaPrevia == "1" ? true : false);
                case (int)Enumerador.enmNotarialTipoFormato.PARTE:

                    if (ActoNotarialDetalleId != 0)
                        ActualizarDatosAcnoNotarialDetalle(ActoNotarialDetalleId, NumOficio,0, AcnoNotarialId);

                    return VerFormatoParte(AcnoNotarialId, Correlativo, NumOficio, vVistaPrevia == "1" ? true : false);
                case (int)Enumerador.enmNotarialTipoFormato.TESTIMONIO:
                    //return VerFormatoTestimonio(AcnoNotarialId, Correlativo);
                    //----------------------------------------------------------------
                    //Fecha: 28/02/2017
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Imprimir Testimonio y la Escritura Pública
                    //----------------------------------------------------------------
                    return VerFormatoTestimonioEscrituraPublica(AcnoNotarialId, Correlativo, vVistaPrevia == "1" ? true : false, ActoNotarialDetalleId);
                default:
                    return VerFormatoEscrituraPublica(AcnoNotarialId, vVistaPrevia == "1" ? true : false);
            }
        }

        private static void ActualizarDatosAcnoNotarialDetalle(Int64 intActoNotarialDetalleId, String strNumOficio, int intFuncionarioAutorizadorId=0, Int64 intActoNotarialId=0)
        {
            ActoNotarialMantenimiento objBL = new ActoNotarialMantenimiento();

            RE_ACTONOTARIALDETALLE ActoNotarialDetalle = new RE_ACTONOTARIALDETALLE();
            ActoNotarialDetalle.ande_iActoNotarialDetalleId = intActoNotarialDetalleId;
            ActoNotarialDetalle.ande_vNumeroOficio = strNumOficio;
            if (intFuncionarioAutorizadorId > 0)
            {
                ActoNotarialDetalle.ande_IFuncionarioAutorizadorId = intFuncionarioAutorizadorId;
            }
            ActoNotarialDetalle.ande_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            ActoNotarialDetalle.ande_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
            ActoNotarialDetalle.ande_sOficinaConsularId = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            ActoNotarialDetalle.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            ActoNotarialDetalle.ande_iActoNotarialId = intActoNotarialId;
            ActoNotarialDetalle = objBL.ActualizarActoNotarialDetalle(ActoNotarialDetalle);
        }

        private static string VerFormatoEscrituraPublica(Int64 AcnoNotarialId, bool bVistaPrevia = false)
        {
            clsRespuesta oclsRespuesta = new clsRespuesta();
            string oRespuesta;
            string Mensaje = string.Empty;
            try
            {
                HttpContext.Current.Session["Acto_Notarial_ID"] = AcnoNotarialId;
                HttpContext.Current.Session["Acto_Notarial_Tipo_formato"] = (int)Enumerador.enmFormatoProtocolar.ESCRITURA;
                HttpContext.Current.Session["OficinaConsular"] = Convert.ToInt32(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                DataTable dtCuerpo = new DataTable();
                DataTable dtDatosPrincipales = new DataTable();
                ActoNotarialConsultaBL _bl = new ActoNotarialConsultaBL();

                dtDatosPrincipales = _bl.ActonotarialObtenerDatosPrincipales(AcnoNotarialId, Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                //--------------------------------------------------------------------------
                //Fecha: 06/04/2020
                //Autor: Miguel Márquez Beltrán
                //Motivo: Asignar los datos de la escritura a una sesión.
                //--------------------------------------------------------------------------
                HttpContext.Current.Session["dtDatosEscritura"] = dtDatosPrincipales;
                //--------------------------------------------------------------------------

                dtCuerpo = _bl.ObtenerCuerpo(AcnoNotarialId);

                string strCuerpo = string.Empty;
                string strDenominacion = string.Empty;
                string strNroEscrituraPublica = string.Empty;
                string vNumeroLibro = string.Empty;
                string strFojaInicial = string.Empty;


                if (dtCuerpo.Rows.Count == 0)
                {
                    //throw new Exception("El cuerpo no se generó correctamente, verifique los datos ingresados. Línea: 8161.");
                    oclsRespuesta.vMensaje = "El cuerpo no se generó correctamente, verifique los datos ingresados.";
                    oclsRespuesta.bResultado = false;
                    return oclsRespuesta.ToString();
                }
                //-------------------------------------------------
                //Fecha: 04/12/2020
                //Autor: Miguel Márquez Beltran
                //Motivo: Extraer el año de la escritura pública
                //--------------------------------------------------
                string strTipoEscrituraDes = "";
                string strAnioExcrituraPublica = "";
                if (dtCuerpo.Rows[0]["ancu_vCuerpo"] != null) strCuerpo = dtCuerpo.Rows[0]["ancu_vCuerpo"].ToString();
                if (dtDatosPrincipales.Rows[0]["vDenominacion"] != null) strDenominacion = dtDatosPrincipales.Rows[0]["vDenominacion"].ToString().Trim();
                if (dtDatosPrincipales.Rows[0]["NumeroEscrituraPublica"] != null) strNroEscrituraPublica = dtDatosPrincipales.Rows[0]["NumeroEscrituraPublica"].ToString().Trim();
                if (dtDatosPrincipales.Rows[0]["vNroLibro"] != null) vNumeroLibro = dtDatosPrincipales.Rows[0]["vNroLibro"].ToString().Trim();
                if (dtDatosPrincipales.Rows[0]["NumeroFojaInicial"] != null) strFojaInicial = dtDatosPrincipales.Rows[0]["NumeroFojaInicial"].ToString().Trim();
                if (dtDatosPrincipales.Rows[0]["SubTipoActoNotarialDesc"] != null) strTipoEscrituraDes = dtDatosPrincipales.Rows[0]["SubTipoActoNotarialDesc"].ToString().Trim();
                if (dtDatosPrincipales.Rows[0]["Fecha"] != null) strAnioExcrituraPublica = dtDatosPrincipales.Rows[0]["Fecha"].ToString().Substring(6);

                //-------------------------------------------
                strCuerpo = strCuerpo.Replace(" ,", ",");
                strCuerpo = strCuerpo.Replace(" .", ".");
                strCuerpo = strCuerpo.Replace(" ;", ";");
                strCuerpo = strCuerpo.Replace(" :", ":");
                //-------------------------------------------


                Int16 TotalOtorgante = 0;
                Int16 TotalApoderado = 0;

                List<CBE_PARTICIPANTE> loPARTICIPANTES = (List<CBE_PARTICIPANTE>)HttpContext.Current.Session["ParticipanteContainer"];
                //loPARTICIPANTES = loPARTICIPANTES.Where(x => x.anpa_cEstado != "E").ToList();


                //loPARTICIPANTES.Sort((x, y) => new { x.Persona.pers_vNombres, x.Persona.pers_vApellidoPaterno, x.Persona.pers_vApellidoMaterno }.pers_vNombres.CompareTo(y.Persona.pers_vNombres));

                loPARTICIPANTES = loPARTICIPANTES.Where(x => x.anpa_cEstado != "E").OrderBy(order => order.Persona.pers_vNombres).ThenBy(order => order.Persona.pers_vApellidoPaterno).ThenBy(order => order.Persona.pers_vApellidoMaterno).ToList();



                List<DocumentoFirma> listObjects = new List<DocumentoFirma>();
                DocumentoFirma oDocumentoFirma = null;

                List<string> listOtorgantes = new List<string>();
                List<string> listApoderados = new List<string>();
                List<string> listInterpretes = new List<string>();
                List<string> listTestigos = new List<string>();


                int intTotalOtorganteMasculino = 0;
                int intTotalOtorganteFemenino = 0;
                int intTotalApoderadoMasculino = 0;
                int intTotalApoderadoFemenino = 0;
                
                TablaMaestraConsultaBL lTablaMaestraConsultaBL = new TablaMaestraConsultaBL();


                foreach (CBE_PARTICIPANTE participante in loPARTICIPANTES)
                {
                    //if (participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONATARIO) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPADO) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.COMPRADOR))

                    if (ObtenerIniciaRecibe(participante.anpa_sTipoParticipanteId) == "INICIA" || ObtenerIniciaRecibe(participante.anpa_sTipoParticipanteId) == "RECIBE" ||                                                
                        participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO) ||                        
                        participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE))
                    {
                        if (participante.anpa_bFlagFirma == false || participante.anpa_bFlagHuella == false)
                        {
                            #region Firma

                            oDocumentoFirma = new DocumentoFirma();
                            oDocumentoFirma.bAplicaHuellaDigital = true;
                            oDocumentoFirma.bIncapacitado = participante.Persona.pers_bIncapacidadFlag;
                            oDocumentoFirma.sTipoParticipante = participante.anpa_sTipoParticipanteId;
                            oDocumentoFirma.vNombreCompleto = participante.Persona.pers_vNombres + ", " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno;

                            //--------------------------------------------------------------------
                            //Autor: Miguel Márquez Beltrán
                            //Fecha: 05/07/2017
                            //Objetivo: Obtener la descripción larga del tipo de documento.
                            //--------------------------------------------------------------------
                            BE.MRE.SC_MAESTRO.MA_DOCUMENTO_IDENTIDAD lDOCUMENTO_IDENTIDAD = new BE.MRE.SC_MAESTRO.MA_DOCUMENTO_IDENTIDAD();
                            lDOCUMENTO_IDENTIDAD.doid_sTipoDocumentoIdentidadId = participante.Identificacion.peid_sDocumentoTipoId;

                            //--------------------------------------------------------------------------------------------------------------------
                            //Fecha: 24/07/2020
                            //Autor: Miguel Márquez Beltrán
                            //Motivo: Asignar el tipo de documento cuando sea OTROS (id.4)
                            //--------------------------------------------------------------------------------------------------------------------
                            if (participante.Identificacion.peid_sDocumentoTipoId == 4)
                            {
                                oDocumentoFirma.vNroDocumentoCompleto = participante.Identificacion.peid_vTipodocumento.Trim() + ": " + participante.Identificacion.peid_vDocumentoNumero;
                            }
                            else
                            {
                                oDocumentoFirma.vNroDocumentoCompleto = participante.peid_sDocumentoTipoId_desc.Trim() + ": " + participante.Identificacion.peid_vDocumentoNumero;
                            }

                            listObjects.Add(oDocumentoFirma); // Se agregan los participantes para las firmas
                            #endregion
                        }
                        //if (participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE) ||
                        //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR) ||
                        //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE) ||
                        //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE))

                        if (ObtenerIniciaRecibe(participante.anpa_sTipoParticipanteId)=="INICIA")
                        {
                            #region OtorgantesSinTestigoInterprete
                            //------------------------------------------------------
                            //Fecha: 05/01/2021
                            //Autor: Miguel Márquez Beltrán
                            //Motivo: Se adiciona el apellido de casada.
                            //------------------------------------------------------
                            if (participante.Persona.pers_sGeneroId == Convert.ToInt16(Enumerador.enmGenero.FEMENINO)
                                && (participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.CASADO) ||
                                   participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.VIUDO)))
                            {
                                listOtorgantes.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno + " " + participante.Persona.pers_vApellidoCasada).ToUpper());
                            }
                            else
                            {
                                listOtorgantes.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno).ToUpper());
                            }
                            
                            TotalOtorgante++;

                            if (participante.Persona.pers_sGeneroId == (int)Enumerador.enmGenero.MASCULINO)
                            {
                                intTotalOtorganteMasculino++;
                            }
                            else
                            {
                                intTotalOtorganteFemenino++;
                            }
                            #endregion
                        }

                        if (participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE))
                        {
                            #region Interprete
                            //                            listInterpretes.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno).ToUpper());
                            //------------------------------------------------------
                            //Fecha: 05/01/2021
                            //Autor: Miguel Márquez Beltrán
                            //Motivo: Se adiciona el apellido de casada.
                            //------------------------------------------------------
                            if (participante.Persona.pers_sGeneroId == Convert.ToInt16(Enumerador.enmGenero.FEMENINO)
                                && (participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.CASADO) ||
                                   participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.VIUDO)))
                            {
                                listInterpretes.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno + " " + participante.Persona.pers_vApellidoCasada).ToUpper());
                            }
                            else
                            {
                                listInterpretes.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno).ToUpper());
                            }

                            #endregion
                        }

                        if (participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO))
                        {
                            #region TestigoARuego
                            //listTestigos.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno).ToUpper());
                            //------------------------------------------------------
                            //Fecha: 05/01/2021
                            //Autor: Miguel Márquez Beltrán
                            //Motivo: Se adiciona el apellido de casada.
                            //------------------------------------------------------
                            if (participante.Persona.pers_sGeneroId == Convert.ToInt16(Enumerador.enmGenero.FEMENINO)
                                && (participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.CASADO) ||
                                   participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.VIUDO)))
                            {
                                listTestigos.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno + " " + participante.Persona.pers_vApellidoCasada).ToUpper());
                            }
                            else
                            {
                                listTestigos.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno).ToUpper());
                            }
                            #endregion
                        }
                    }

                    //if (participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.APODERADO) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.COMPRADOR) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONATARIO) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPADO))

                    if (ObtenerIniciaRecibe(participante.anpa_sTipoParticipanteId) == "RECIBE")
                    {
                        #region Apoderados
                        //------------------------------------------------------
                        //Fecha: 05/01/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Se adiciona el apellido de casada.
                        //------------------------------------------------------
                        if (participante.Persona.pers_sGeneroId == Convert.ToInt16(Enumerador.enmGenero.FEMENINO)
                                && (participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.CASADO) ||
                                   participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.VIUDO)))
                        {
                            listApoderados.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno + " " + participante.Persona.pers_vApellidoCasada).ToUpper());
                        }
                        else
                        {
                            listApoderados.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno).ToUpper());
                        }
                        TotalApoderado++;
                        if (participante.Persona.pers_sGeneroId == (int)Enumerador.enmGenero.MASCULINO)
                        {
                            intTotalApoderadoMasculino++;
                        }
                        else
                        {
                            intTotalApoderadoFemenino++;
                        }
                        #endregion
                    }
                }

                //-----------------------------------------------
                //Ordenar la lista de otorgantes y apoderados
                //-----------------------------------------------

                listOtorgantes.Sort();
                listApoderados.Sort();
                listInterpretes.Sort();
                listTestigos.Sort();
                //----------------------------------------------------
                // Autor: Miguel Márquez Beltrán
                // Fecha: 23/06/2017
                // Objetivo: Calcular el número de fojas restantes. 
                //----------------------------------------------------
                DataTable dtLibros = new DataTable();

                LibroConsultasBL objLibroBL = new LibroConsultasBL();

                int intTotalRegistros = 0;
                int intTotalPaginas = 0;
                //----------------------------------------------------
                //Fecha: 04/12/2020
                //Autor: Miguel Márquez Beltran
                //Motivo: Filtrar por el año de la escritura pública
                //-----------------------------------------------------
                int intAnioEscrituraPublica = Convert.ToInt32(strAnioExcrituraPublica);
                dtLibros = objLibroBL.obtener(Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString()), intAnioEscrituraPublica,
                    1, 10, ref intTotalRegistros, ref intTotalPaginas);
                //dtLibros = objLibroBL.obtener(Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString()), 0,
                //    1, 10, ref intTotalRegistros, ref intTotalPaginas);


                DataTable dtFojasLibro = null;

                //----------------------------------------------------
                //Fecha: 28/12/2020
                //Autor: Miguel Márquez Beltrán
                //Motivo: Validar sino hay libros.
                //----------------------------------------------------
                if (dtLibros == null)
                {
                    throw new Exception("No existe libro de Escritura Pública.");
                }
                if (dtLibros.Rows.Count > 0)
                {

                    dtFojasLibro = (from dtLib in dtLibros.AsEnumerable()
                                    where Comun.ToNullInt32(dtLib["libr_sTipoLibroId"]) == ((int)Enumerador.enmNotarialLibroTipo.ESCRITURA_PUBLICA)
                                    && dtLib["libr_vEstadoLibro"].ToString().Equals("ABIERTO")
                                    select dtLib).CopyToDataTable();
                }
                else
                {
                    throw new Exception("No existe libro de Escritura Pública.");
                }
                Int32 intNumeroFojaTotal = 0;
                Int32 intNumeroFojaActual = 0;
                Int32 intNumeroFojaRestante = 0;
                //----------------------------------------------------
                //Fecha: 28/12/2020
                //Autor: Miguel Márquez Beltrán
                //Motivo: Validar si existen fojas en el libro.
                //----------------------------------------------------
                if (dtFojasLibro == null)
                {
                    throw new Exception("No existen fojas en el libro de Escritura Pública.");
                }
                if (dtFojasLibro.Rows.Count > 0)
                {
                    intNumeroFojaTotal = Comun.ToNullInt32(dtFojasLibro.Rows[0]["libr_iNumeroFolioTotal"].ToString());
                    intNumeroFojaActual = Comun.ToNullInt32(dtFojasLibro.Rows[0]["libr_iNumeroFolioActual"].ToString());

                    intNumeroFojaRestante = intNumeroFojaTotal - intNumeroFojaActual;
                }

                //-----------------------------------------------

                List<TextoPosicionadoITextSharp> listTextoPosicionado = new List<TextoPosicionadoITextSharp>();

                float fFontSize = 12;
                float fDocumentHeight = 842 - fFontSize;
                float fLineaHeight = fFontSize + 1.5f;

                iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Fonts/cour.ttf"), iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);

                int iFoja = Convert.ToInt32(strFojaInicial);

                string strFuncionarioNombre = string.Empty;
                if (dtDatosPrincipales.Rows[0]["NombreFuncionario"] != null)
                    strFuncionarioNombre = Convert.ToString(dtDatosPrincipales.Rows[0]["NombreFuncionario"]);

                DocumentoiTextSharp oDocumentoiTextSharp = new DocumentoiTextSharp(null, strCuerpo, HttpContext.Current.Server.MapPath("~/Images/Escudo.JPG"));


                oDocumentoiTextSharp.NombreFuncionario = strFuncionarioNombre;
                oDocumentoiTextSharp.ListPalabrasOmitirTextoNotarial = null;
                oDocumentoiTextSharp.ListDocumentoFirma = listObjects;
                oDocumentoiTextSharp.NombreConsulado = HttpContext.Current.Session["NombreConsulado"].ToString();
                oDocumentoiTextSharp.CuerpoBaseFont = baseFont;
                oDocumentoiTextSharp.iFojaActual = iFoja - 1;
                oDocumentoiTextSharp.iFojaRestante = intNumeroFojaRestante;
                oDocumentoiTextSharp.sTitulo = strDenominacion;
                oDocumentoiTextSharp.bEsVistaPrevia = bVistaPrevia;
                //oDocumentoiTextSharp.TipoActoNotarial = strTipoEscrituraDes;
                oDocumentoiTextSharp.NumeroEP = dtDatosPrincipales.Rows[0]["NumeroEscrituraPublica"].ToString();
                oDocumentoiTextSharp.bEsEscrituraPublica = true;
                oDocumentoiTextSharp.bEsTestimonio = false;

                if (Convert.ToBoolean(dtDatosPrincipales.Rows[0]["Minuta"]))
                {
                    oDocumentoiTextSharp.SMinutaEP = dtDatosPrincipales.Rows[0]["acno_iNumeroActoNotarial"].ToString();
                    oDocumentoiTextSharp.sEtiquetaMinuta = "CON MINUTA";
                }
                else
                {
                    oDocumentoiTextSharp.SMinutaEP = "S/N";
                    oDocumentoiTextSharp.sEtiquetaMinuta = "SIN MINUTA";
                }


                oDocumentoiTextSharp.bGenerarDocumentoAutomaticamente = false;
                oDocumentoiTextSharp.ListOtorgantes = listOtorgantes;
                oDocumentoiTextSharp.ListApoderados = listApoderados;
                oDocumentoiTextSharp.ListInterpretes = listInterpretes;
                oDocumentoiTextSharp.ListTestigos = listTestigos;

                #region Imágenes
                List<BE.MRE.RE_ACTONOTARIALDOCUMENTO> lstImagenes = (List<BE.MRE.RE_ACTONOTARIALDOCUMENTO>)HttpContext.Current.Session["ImagenesContainer"];
                if (lstImagenes != null)
                {
                    if (lstImagenes.Count > 0)
                    {
                        lstImagenes = lstImagenes.Where(x => x.ando_cEstado != "E").ToList();
                        List<ImagenNotarial> lstRutasImagen = new List<ImagenNotarial>();
                        String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
                        ImagenNotarial imagen = new ImagenNotarial();
                        foreach (RE_ACTONOTARIALDOCUMENTO ActoNotarialDocumento in lstImagenes)
                        {
                            imagen = new ImagenNotarial();
                            imagen.vRuta = uploadPath + ActoNotarialDocumento.ando_vRutaArchivo;
                            imagen.vTitulo = ActoNotarialDocumento.ando_vDescripcion;
                            lstRutasImagen.Add(imagen);
                        }
                        oDocumentoiTextSharp.ListImagenes = lstRutasImagen;
                    }
                }
                #endregion

                oDocumentoiTextSharp.CrearDocumentoPDF();
                
                //----------------------------------------------------
                //Fecha: 25/06/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Actualizar libros solo si se va a imprimir.
                //----------------------------------------------------
                Int32 resp = 1;
                oclsRespuesta.NumeroPagina = oDocumentoiTextSharp.IPageNumber;

                if (bVistaPrevia == false)
                {
                    #region Validar Fojas

                    String StrScript = String.Empty;
                    Int32 Fojainicial = 0, FojaFinal = 0, acno_iNumeroEscritura = 0;
                    String iNumeroLibro = String.Empty;

                    resp = _bl.ValidarFojas(AcnoNotarialId, Convert.ToInt16((int)Enumerador.enmNotarialLibroTipo.ESCRITURA_PUBLICA),
                        Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString()),
                        oclsRespuesta.NumeroPagina, 0, ref Mensaje, ref Fojainicial, ref FojaFinal, ref acno_iNumeroEscritura, ref iNumeroLibro);


                    #endregion
                    if (resp > 0)
                    {
                        #region Guardar_Escritura

                        //-------------------------------------------------------------
                        //Fecha: 06/12/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Guardar documento PDF de Escritura Pública
                        //          cuando es Vista previa.
                        //-------------------------------------------------------------
                        string uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
                        string strCarpetaEC = System.Configuration.ConfigurationManager.AppSettings["CarpetaEscrituraPublica"];
                        string uploadFileName = "";
                        string strRutaEC = uploadPath + @strCarpetaEC; 
                        if (Directory.Exists(strRutaEC))
                        {
                        }
                        else
                        {
                            try
                            {
                                Directory.CreateDirectory(strRutaEC);
                            }
                            catch
                            {
                                throw;
                            }
                        }
                        string _fileName = "documento.pdf";
                        string _RutafileName = "";


                        uploadFileName = Documento.GetUniqueUploadFileNamePDFEscrituraPublica(System.Web.HttpContext.Current.Session["ofco_vSiglas"].ToString(), Convert.ToInt64(HttpContext.Current.Session["Acto_Notarial_ID"].ToString()), strRutaEC, ref _fileName, ref _RutafileName, "E");
                        byte[] filePDF = (byte[])System.Web.HttpContext.Current.Session["binaryData"];
                        string strRutaArchivoDestino = Path.Combine(strRutaEC, uploadFileName);
                        if (filePDF != null)
                        {

                            try
                            {
                                
                                if (System.IO.File.Exists(strRutaArchivoDestino))
                                {
                                    System.IO.File.Delete(strRutaArchivoDestino);
                                }

                                System.IO.File.WriteAllBytes(strRutaArchivoDestino, filePDF);

                                RE_ACTONOTARIALDOCUMENTO lArchivoDigitalizado = new RE_ACTONOTARIALDOCUMENTO();
                                ActoNotarialMantenimiento ActoNotarialBL = new ActoNotarialMantenimiento();
                                RE_ACTONOTARIALDOCUMENTO DocumentoBE = new RE_ACTONOTARIALDOCUMENTO();


                                #region BuscaTipoDocumento
                                DataTable dt = new DataTable();
                                dt = comun_Part1.ObtenerParametrosPorGrupo(HttpContext.Current.Session, Enumerador.enmGrupo.ACTUACION_TIPO_ADJUNTO).Copy();
                                Int16 intTipoDocumentoId = 0;
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    if (dt.Rows[i]["para_vDescripcion"].ToString().Equals("ESCRITURA|.PDF"))
                                    {
                                        intTipoDocumentoId = Convert.ToInt16(dt.Rows[i]["para_sParametroId"].ToString());
                                        break;
                                    }
                                }

                                #endregion

                                #region OBJETO ARCHIVO (CUANDO ES NUEVO)
                                lArchivoDigitalizado.ando_iActoNotarialDocumentoId = 0;
                                lArchivoDigitalizado.ando_sTipoInformacionId = 0;
                                lArchivoDigitalizado.ando_sSubTipoInformacionId = 0;
                                lArchivoDigitalizado.ando_iActoNotarialId = Convert.ToInt64(HttpContext.Current.Session["Acto_Notarial_ID"].ToString());
                                lArchivoDigitalizado.ando_sTipoDocumentoId = intTipoDocumentoId;
                                lArchivoDigitalizado.ando_vDescripcion = "Escritura Pública";
                                lArchivoDigitalizado.ando_vRutaArchivo = _fileName;
                                lArchivoDigitalizado.ando_dFechaCreacion = DateTime.Today;//HardCode por mientras
                                lArchivoDigitalizado.ando_sUsuarioCreacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                                lArchivoDigitalizado.ando_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();


                                DocumentoBE = ActoNotarialBL.Obtener(lArchivoDigitalizado);
                                if (DocumentoBE.Error == false)
                                {
                                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                                    string strJson = "";

                                    if (DocumentoBE.ando_iActoNotarialDocumentoId == 0)
                                    {
                                        strJson = serializer.Serialize(ActoNotarialBL.InsertarActoNotarialDocumento(lArchivoDigitalizado));
                                    }
                                    else
                                    {

                                        string strMision = System.Web.HttpContext.Current.Session["ofco_vSiglas"].ToString();

                                        //20211207_172548_E_1363723710.pdf

                                        int intPos = DocumentoBE.ando_vRutaArchivo.IndexOf("_E_");

                                        if (DocumentoBE.ando_vRutaArchivo.Length > 0 && intPos > 0)
                                        {
                                            string strAnio = DocumentoBE.ando_vRutaArchivo.Substring(0, 4);
                                            string strMes = DocumentoBE.ando_vRutaArchivo.Substring(4, 2);
                                            string strDia = DocumentoBE.ando_vRutaArchivo.Substring(6, 2);
                                            string strpathMision = Path.Combine(strRutaEC, strMision);
                                            string strpathAnio = Path.Combine(strpathMision, strAnio);
                                            string strpathAnioMes = Path.Combine(strpathAnio, strMes);
                                            string strpathAnioMesDia = Path.Combine(strpathAnioMes, strDia);

                                            strRutaArchivoDestino = Path.Combine(strpathAnioMesDia, DocumentoBE.ando_vRutaArchivo);

                                            if (System.IO.File.Exists(strRutaArchivoDestino))
                                            {
                                                System.IO.File.Delete(strRutaArchivoDestino);
                                            }
                                            lArchivoDigitalizado.ando_iActoNotarialDocumentoId = DocumentoBE.ando_iActoNotarialDocumentoId;
                                            lArchivoDigitalizado.ando_sTipoDocumentoId = intTipoDocumentoId;
                                            lArchivoDigitalizado.ando_vRutaArchivo = _fileName;
                                            lArchivoDigitalizado.ando_dFechaModificacion = DateTime.Today;
                                            lArchivoDigitalizado.ando_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                                            lArchivoDigitalizado.ando_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();

                                            strJson = serializer.Serialize(ActoNotarialBL.ActualizarActoNotarialDocumento(lArchivoDigitalizado));
                                        }
                                    }
                                }
                                #endregion
                            }
                            catch
                            {

                                throw;
                            }


                        }
                        #endregion
                    }
                }
                else
                {
                    #region Guardar_Escritura

                    //-------------------------------------------------------------
                    //Fecha: 06/12/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Guardar documento PDF de Escritura Pública
                    //          cuando es Vista previa.
                    //-------------------------------------------------------------
                    string uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
                    string strCarpetaEC = System.Configuration.ConfigurationManager.AppSettings["CarpetaEscrituraPublica"];
                    string uploadFileName = "";
                    string strRutaEC = uploadPath + @strCarpetaEC;;
                    if (Directory.Exists(strRutaEC))
                    {
                    }
                    else
                    {
                        try
                        {
                            Directory.CreateDirectory(strRutaEC);
                        }
                        catch
                        {
                            throw;
                        }
                    }
                    string _fileName = "documento.pdf";
                    string _RutafileName = "";


                    uploadFileName = Documento.GetUniqueUploadFileNamePDFEscrituraPublica(System.Web.HttpContext.Current.Session["ofco_vSiglas"].ToString(), Convert.ToInt64(HttpContext.Current.Session["Acto_Notarial_ID"].ToString()), strRutaEC, ref _fileName, ref _RutafileName,"E");
                    byte[] filePDF = (byte[])System.Web.HttpContext.Current.Session["binaryData"];
                    string strRutaArchivoDestino = Path.Combine(strRutaEC,uploadFileName);
                    if (filePDF != null)
                    {

                        try
                        {
                            if (System.IO.File.Exists(strRutaArchivoDestino))
                            {
                                System.IO.File.Delete(strRutaArchivoDestino);
                            }
                            
                            byte[] fileMarcaAguaPDF = Comun.PonerMarcaAguaPDF(filePDF, "SIN VALOR").ToArray();
                            System.Web.HttpContext.Current.Session["binaryData"] = fileMarcaAguaPDF;
                            System.IO.File.WriteAllBytes(strRutaArchivoDestino, fileMarcaAguaPDF);

                            RE_ACTONOTARIALDOCUMENTO lArchivoDigitalizado = new RE_ACTONOTARIALDOCUMENTO();
                            ActoNotarialMantenimiento ActoNotarialBL = new ActoNotarialMantenimiento();
                            RE_ACTONOTARIALDOCUMENTO DocumentoBE = new RE_ACTONOTARIALDOCUMENTO();

                            #region BuscaTipoDocumento
                            DataTable dt = new DataTable();
                            dt = comun_Part1.ObtenerParametrosPorGrupo(HttpContext.Current.Session, Enumerador.enmGrupo.ACTUACION_TIPO_ADJUNTO).Copy();
                            Int16 intTipoDocumentoId = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i]["para_vDescripcion"].ToString().Equals("ESCRITURA|.PDF"))
                                {
                                    intTipoDocumentoId = Convert.ToInt16(dt.Rows[i]["para_sParametroId"].ToString());
                                    break;
                                }
                            }

                            #endregion
                            #region OBJETO ARCHIVO (CUANDO ES NUEVO)
                            lArchivoDigitalizado.ando_iActoNotarialDocumentoId = 0;
                            lArchivoDigitalizado.ando_sTipoInformacionId = 0;
                            lArchivoDigitalizado.ando_sSubTipoInformacionId = 0;
                            lArchivoDigitalizado.ando_iActoNotarialId = Convert.ToInt64(HttpContext.Current.Session["Acto_Notarial_ID"].ToString());
                            lArchivoDigitalizado.ando_sTipoDocumentoId = intTipoDocumentoId;
                            lArchivoDigitalizado.ando_vDescripcion = "Vista previa Escritura Pública";
                            lArchivoDigitalizado.ando_vRutaArchivo = _fileName;
                            lArchivoDigitalizado.ando_dFechaCreacion = DateTime.Today;//HardCode por mientras
                            lArchivoDigitalizado.ando_sUsuarioCreacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                            lArchivoDigitalizado.ando_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();


                            DocumentoBE = ActoNotarialBL.Obtener(lArchivoDigitalizado);
                            if (DocumentoBE.Error == false)
                            {
                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                string strJson = "";

                                if (DocumentoBE.ando_iActoNotarialDocumentoId == 0)
                                {                                    
                                    strJson= serializer.Serialize(ActoNotarialBL.InsertarActoNotarialDocumento(lArchivoDigitalizado));
                                }
                                else
                                {

                                    string strMision = System.Web.HttpContext.Current.Session["ofco_vSiglas"].ToString();

                                    //20211207_172548_E_1363723710.pdf
                                    int intPos = DocumentoBE.ando_vRutaArchivo.IndexOf("_E_");

                                    if (DocumentoBE.ando_vRutaArchivo.Length > 0 && intPos > 0)
                                    {
                                        string strAnio = DocumentoBE.ando_vRutaArchivo.Substring(0, 4);
                                        string strMes = DocumentoBE.ando_vRutaArchivo.Substring(4, 2);
                                        string strDia = DocumentoBE.ando_vRutaArchivo.Substring(6, 2);
                                        string strpathMision = Path.Combine(strRutaEC, strMision);
                                        string strpathAnio = Path.Combine(strpathMision, strAnio);
                                        string strpathAnioMes = Path.Combine(strpathAnio, strMes);
                                        string strpathAnioMesDia = Path.Combine(strpathAnioMes, strDia);

                                        strRutaArchivoDestino = Path.Combine(strpathAnioMesDia, DocumentoBE.ando_vRutaArchivo);

                                        if (System.IO.File.Exists(strRutaArchivoDestino))
                                        {
                                            System.IO.File.Delete(strRutaArchivoDestino);
                                        }
                                        lArchivoDigitalizado.ando_iActoNotarialDocumentoId = DocumentoBE.ando_iActoNotarialDocumentoId;
                                        lArchivoDigitalizado.ando_sTipoDocumentoId = intTipoDocumentoId;
                                        lArchivoDigitalizado.ando_vRutaArchivo = _fileName;
                                        lArchivoDigitalizado.ando_dFechaModificacion = DateTime.Today;
                                        lArchivoDigitalizado.ando_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                                        lArchivoDigitalizado.ando_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();

                                        strJson = serializer.Serialize(ActoNotarialBL.ActualizarActoNotarialDocumento(lArchivoDigitalizado));
                                    }
                                }
                            }
                            #endregion
                        }
                        catch
                        {

                            throw;
                        }


                    }
                    #endregion
                }
                //-----------------------------------------------------
                //Fecha: 17/07/2020
                //Autor: Miguel Márquez Beltrán
                //Motivo: Enviar el mensaje si no existe ningún libro.
                //-----------------------------------------------------
                if (resp == 0)
                {
                    oclsRespuesta.bResultado = false;
                }
                else
                {
                    oclsRespuesta.bResultado = true;
                }
                //-----------------------------------------------------
                oclsRespuesta.vMensaje = Mensaje;
                oRespuesta = new JavaScriptSerializer().Serialize(oclsRespuesta);
                return oRespuesta;
            }
            catch (Exception ex)
            {
                oclsRespuesta.bResultado = false;
                oclsRespuesta.vMensaje = ex.Message;
                oclsRespuesta.NumeroPagina = 0;
                oRespuesta = new JavaScriptSerializer().Serialize(oclsRespuesta);
                return oRespuesta;
            }
        }


        //----------------------------------------------------------------
        //Fecha: 28/02/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Imprimir Testimonio y la Escritura Pública
        //----------------------------------------------------------------


        private static string VerFormatoTestimonioEscrituraPublica(Int64 AcnoNotarialId, int Correlativo, bool bVistaPrevia = false, Int64 ActoNotarialDetalleId=0)
        {
            clsRespuesta oclsRespuesta = new clsRespuesta();
            string Mensaje = string.Empty;
            string oRespuesta = "";
            try
            {
                HttpContext.Current.Session["Acto_Notarial_ID"] = AcnoNotarialId;
                HttpContext.Current.Session["Acto_Notarial_Tipo_formato"] = (int)Enumerador.enmFormatoProtocolar.TESTIMONIO;
                HttpContext.Current.Session["OficinaConsular"] = Convert.ToInt32(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);


                DataTable dtdp = new DataTable();
                DataTable dt = new DataTable();

                ActoNotarialConsultaBL _bl = new ActoNotarialConsultaBL();
                ReportesNotarialesConsultasBL oReportesNotarialesConsultasBL = new ReportesNotarialesConsultasBL();
                String str_Testimonio = String.Empty;
                Int16 intOficinaConsular = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                str_Testimonio = oReportesNotarialesConsultasBL.USP_RP_NOTARIAL_PROTOCOLAR_TESTIMONIO(AcnoNotarialId, intOficinaConsular, Correlativo, ActoNotarialDetalleId);


                dtdp = _bl.ActonotarialObtenerDatosPrincipales(AcnoNotarialId, intOficinaConsular);

                //-----------------------------------------------------------------
                //Fecha: 10/02/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Obtener el tipo de Acto Notarial.
                //-----------------------------------------------------------------

                string strTipoActoNotarial = "";

                //if (dtdp.Rows[0]["SubTipoActoNotarialDesc"] != null)
                //{
                //    strTipoActoNotarial = dtdp.Rows[0]["SubTipoActoNotarialDesc"].ToString();
                //}
                if (dtdp.Rows[0]["vDenominacion"] != null)
                {
                    strTipoActoNotarial = dtdp.Rows[0]["vDenominacion"].ToString();
                }
                //-----------------------------------------------------------------
                string strCuerpo = string.Empty;
                string strDenominacion = string.Empty;
                string strNroEscrituraPublica = string.Empty;
                string vNumeroLibro = string.Empty;
                string strFojaInicial = string.Empty;

                strDenominacion = NumeroOrdinal(Correlativo) + " TESTIMONIO";

                if (dtdp.Rows[0]["NumeroEscrituraPublica"] != null) strNroEscrituraPublica = dtdp.Rows[0]["NumeroEscrituraPublica"].ToString().Trim();
                if (dtdp.Rows[0]["vNroLibro"] != null) vNumeroLibro = dtdp.Rows[0]["vNroLibro"].ToString().Trim();

                Int16 TotalOtorgante = 0;
                Int16 TotalApoderado = 0;
                List<CBE_PARTICIPANTE> loPARTICIPANTES = (List<CBE_PARTICIPANTE>)HttpContext.Current.Session["ParticipanteContainer"];

                //loPARTICIPANTES = loPARTICIPANTES.Where(x => x.anpa_cEstado != "E").ToList();

                //loPARTICIPANTES.Sort((x, y) => new { x.Persona.pers_vNombres, x.Persona.pers_vApellidoPaterno, x.Persona.pers_vApellidoMaterno }.pers_vNombres.CompareTo(y.Persona.pers_vNombres));

                loPARTICIPANTES = loPARTICIPANTES.Where(x => x.anpa_cEstado != "E").OrderBy(order => order.Persona.pers_vNombres).ThenBy(order => order.Persona.pers_vApellidoPaterno).ThenBy(order => order.Persona.pers_vApellidoMaterno).ToList();


                List<string> listOtorgantes = new List<string>();
                List<string> listApoderados = new List<string>();

                foreach (CBE_PARTICIPANTE participante in loPARTICIPANTES)
                {
                    //if (participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO))

                    if (ObtenerIniciaRecibe(participante.anpa_sTipoParticipanteId) == "INICIA" ||
                        participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO) ||
                        participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE))
                    {
                            if (participante.Persona.pers_sGeneroId == Convert.ToInt16(Enumerador.enmGenero.FEMENINO)
                               && (participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.CASADO) ||
                                  participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.VIUDO)))
                            {
                                listOtorgantes.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno).ToUpper() + " " + participante.Persona.pers_vApellidoCasada.ToUpper());
                            }
                            else
                            {
                                listOtorgantes.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno).ToUpper());
                            }
                            
                            
                            TotalOtorgante++;
                    }
                    //if (participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.APODERADO) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.COMPRADOR) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONATARIO) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPADO))

                    if (ObtenerIniciaRecibe(participante.anpa_sTipoParticipanteId) == "RECIBE")
                    {
                        if (participante.Persona.pers_sGeneroId == Convert.ToInt16(Enumerador.enmGenero.FEMENINO)
                               && (participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.CASADO) ||
                                  participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.VIUDO)))
                        {
                            listApoderados.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno).ToUpper() + " " + participante.Persona.pers_vApellidoCasada.ToUpper());
                        }
                        else
                        {
                            listApoderados.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno).ToUpper());
                        }
                        TotalApoderado++;
                    }
                }

                List<TextoPosicionadoITextSharp> listTextoPosicionado = new List<TextoPosicionadoITextSharp>();

                float fFontSize = 12;
                float fDocumentHeight = 842 - fFontSize;
                float fLineaHeight = fFontSize + 1.5f;

                iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Fonts/cour.ttf"), iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);

                DocumentoiTextSharp oDocumentoiTextSharp = new DocumentoiTextSharp(null, str_Testimonio, HttpContext.Current.Server.MapPath("~/Images/Escudo.JPG"));
                oDocumentoiTextSharp.ListPalabrasOmitirTextoNotarial = null;
                oDocumentoiTextSharp.NombreConsulado = HttpContext.Current.Session["NombreConsulado"].ToString();
                oDocumentoiTextSharp.CuerpoBaseFont = baseFont;

                //---------------------------------------------
                //Fecha: 10/02/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Asignar font en negrita al Titulo
                //---------------------------------------------
                iTextSharp.text.pdf.BaseFont TitulobaseFont = iTextSharp.text.pdf.BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Fonts/courbd.ttf"), iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);
                //---------------------------------------------
                oDocumentoiTextSharp.sTitulo = strDenominacion;
                oDocumentoiTextSharp.TituloBaseFont = TitulobaseFont;
                oDocumentoiTextSharp.bEsEscrituraPublica = true;
                oDocumentoiTextSharp.bEsTestimonio = true;

                oDocumentoiTextSharp.TipoActoNotarial = strTipoActoNotarial;


                if (Convert.ToBoolean(dtdp.Rows[0]["Minuta"]))
                {
                    oDocumentoiTextSharp.SMinutaEP = dtdp.Rows[0]["acno_iNumeroActoNotarial"].ToString();
                }
                else
                {
                    oDocumentoiTextSharp.SMinutaEP = "S/N";
                }

                oDocumentoiTextSharp.bGenerarDocumentoAutomaticamente = false;
                oDocumentoiTextSharp.ListOtorgantes = listOtorgantes;
                oDocumentoiTextSharp.ListApoderados = listApoderados;

                //--------------------------------------------------------------
                //Datos de la Escritura Pública
                //==============================================================================================
                DataTable dtCuerpo = new DataTable();
                DataTable dtDatosPrincipales = new DataTable();
                _bl = new ActoNotarialConsultaBL();

                dtDatosPrincipales = _bl.ActonotarialObtenerDatosPrincipales(AcnoNotarialId, Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                dtCuerpo = _bl.ObtenerCuerpo(AcnoNotarialId);

                if (dtCuerpo.Rows.Count == 0)
                {
                    throw new Exception("El cuerpo no se generó correctamente, verifique los datos ingresados. Línea: 8867.");
                }

                string strTipoEscrituraDes = "";
                if (dtCuerpo.Rows[0]["ancu_vCuerpo"] != null) strCuerpo = dtCuerpo.Rows[0]["ancu_vCuerpo"].ToString();
                if (dtDatosPrincipales.Rows[0]["vDenominacion"] != null) strDenominacion = dtDatosPrincipales.Rows[0]["vDenominacion"].ToString().Trim();
                if (dtDatosPrincipales.Rows[0]["NumeroEscrituraPublica"] != null) strNroEscrituraPublica = dtDatosPrincipales.Rows[0]["NumeroEscrituraPublica"].ToString().Trim();
                if (dtDatosPrincipales.Rows[0]["vNroLibro"] != null) vNumeroLibro = dtDatosPrincipales.Rows[0]["vNroLibro"].ToString().Trim();
                if (dtDatosPrincipales.Rows[0]["NumeroFojaInicial"] != null) strFojaInicial = dtDatosPrincipales.Rows[0]["NumeroFojaInicial"].ToString().Trim();
                if (dtDatosPrincipales.Rows[0]["SubTipoActoNotarialDesc"] != null) strTipoEscrituraDes = dtDatosPrincipales.Rows[0]["SubTipoActoNotarialDesc"].ToString().Trim();

                //-------------------------------------------
                strCuerpo = strCuerpo.Replace(" ,", ",");
                strCuerpo = strCuerpo.Replace(" .", ".");
                strCuerpo = strCuerpo.Replace(" ;", ";");
                strCuerpo = strCuerpo.Replace(" :", ":");
                //-------------------------------------------

                loPARTICIPANTES = (List<CBE_PARTICIPANTE>)HttpContext.Current.Session["ParticipanteContainer"];
                //loPARTICIPANTES = loPARTICIPANTES.Where(x => x.anpa_cEstado != "E").ToList();
                //loPARTICIPANTES.Sort((x, y) => new { x.Persona.pers_vNombres, x.Persona.pers_vApellidoPaterno, x.Persona.pers_vApellidoMaterno }.pers_vNombres.CompareTo(y.Persona.pers_vNombres));

                loPARTICIPANTES = loPARTICIPANTES.Where(x => x.anpa_cEstado != "E").OrderBy(order => order.Persona.pers_vNombres).ThenBy(order => order.Persona.pers_vApellidoPaterno).ThenBy(order => order.Persona.pers_vApellidoMaterno).ToList();


                List<DocumentoFirma> listObjects = new List<DocumentoFirma>();
                DocumentoFirma oDocumentoFirma = null;
                TotalOtorgante = 0;
                TotalApoderado = 0;

                List<string> listOtorgantesEC = new List<string>();
                List<string> listApoderadosEC = new List<string>();
                List<string> listInterpretesEC = new List<string>();
                List<string> listTestigosEC = new List<string>();

                int intTotalOtorganteMasculino = 0;
                int intTotalOtorganteFemenino = 0;
                int intTotalApoderadoMasculino = 0;
                int intTotalApoderadoFemenino = 0;
                string strTipoDocumentoDescLarga = "";
                TablaMaestraConsultaBL lTablaMaestraConsultaBL = new TablaMaestraConsultaBL();

                foreach (CBE_PARTICIPANTE participante in loPARTICIPANTES)
                {
                    //if (participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONATARIO) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPADO) ||
                    //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.COMPRADOR))

                    if (ObtenerIniciaRecibe(participante.anpa_sTipoParticipanteId) == "INICIA" || 
                        ObtenerIniciaRecibe(participante.anpa_sTipoParticipanteId) == "RECIBE" ||
                        participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO) ||
                        participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE))
                    {
                        oDocumentoFirma = new DocumentoFirma();
                        oDocumentoFirma.bAplicaHuellaDigital = true;
                        oDocumentoFirma.bIncapacitado = participante.Persona.pers_bIncapacidadFlag;
                        oDocumentoFirma.sTipoParticipante = participante.anpa_sTipoParticipanteId;
                        oDocumentoFirma.vNombreCompleto = participante.Persona.pers_vNombres + ", " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno;

                        //--------------------------------------------------------------------------------------------------------------------
                        //Autor: Miguel Márquez Beltrán
                        //Fecha: 05/07/2017
                        //Objetivo: Obtener la descripción larga del tipo de documento.
                        //--------------------------------------------------------------------------------------------------------------------
                        BE.MRE.SC_MAESTRO.MA_DOCUMENTO_IDENTIDAD lDOCUMENTO_IDENTIDAD = new BE.MRE.SC_MAESTRO.MA_DOCUMENTO_IDENTIDAD();
                        lDOCUMENTO_IDENTIDAD.doid_sTipoDocumentoIdentidadId = participante.Identificacion.peid_sDocumentoTipoId;

                        strTipoDocumentoDescLarga = lTablaMaestraConsultaBL.DOCUMENTO_IDENTIDAD_OBTENER(lDOCUMENTO_IDENTIDAD).doid_vDescripcionLarga;
                        //--------------------------------------------------------------------------------------------------------------------

                        //--------------------------------------------------------------------------------------------------------------------
                        //Fecha: 24/07/2020
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Asignar el tipo de documento cuando sea OTROS (id.4)
                        //--------------------------------------------------------------------------------------------------------------------

                        if (participante.Identificacion.peid_sDocumentoTipoId == 4)
                        {
                            oDocumentoFirma.vNroDocumentoCompleto = participante.Identificacion.peid_vTipodocumento.Trim() + ": " + participante.Identificacion.peid_vDocumentoNumero;
                        }
                        else
                        {
                            oDocumentoFirma.vNroDocumentoCompleto = strTipoDocumentoDescLarga + ": " + participante.Identificacion.peid_vDocumentoNumero;
                        }


                        listObjects.Add(oDocumentoFirma); // Se agregan los participantes para las firmas

                        //if (participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE) ||
                        //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR) ||
                        //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE) ||
                        //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE))

                        if (ObtenerIniciaRecibe(participante.anpa_sTipoParticipanteId) == "INICIA")
                        {
                            

                            if (participante.Persona.pers_sGeneroId == Convert.ToInt16(Enumerador.enmGenero.FEMENINO)
                               && (participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.CASADO) ||
                                  participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.VIUDO)))
                            {
                                listOtorgantesEC.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno).ToUpper() + " " + participante.Persona.pers_vApellidoCasada.ToUpper());
                            }
                            else
                            {
                                listOtorgantesEC.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno).ToUpper());
                            }
                            
                            
                            TotalOtorgante++;

                            if (participante.Persona.pers_sGeneroId == (int)Enumerador.enmGenero.MASCULINO)
                            {
                                intTotalOtorganteMasculino++;
                            }
                            else
                            {
                                intTotalOtorganteFemenino++;
                            }
                        }
                        if (participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE))
                        {
                            if (participante.Persona.pers_sGeneroId == Convert.ToInt16(Enumerador.enmGenero.FEMENINO)
                                && (participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.CASADO) ||
                                   participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.VIUDO)))
                            {
                                listInterpretesEC.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno + " " + participante.Persona.pers_vApellidoCasada).ToUpper());
                            }
                            else
                            {
                                listInterpretesEC.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno).ToUpper());
                            }
                        }

                        if (participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO))
                        {
                            if (participante.Persona.pers_sGeneroId == Convert.ToInt16(Enumerador.enmGenero.FEMENINO)
                                && (participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.CASADO) ||
                                   participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.VIUDO)))
                            {
                                listTestigosEC.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno + " " + participante.Persona.pers_vApellidoCasada).ToUpper());
                            }
                            else
                            {
                                listTestigosEC.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno).ToUpper());
                            }
                        }
                    }


                    if (participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.APODERADO) ||
                        participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.COMPRADOR) ||
                        participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONATARIO) ||
                        participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPADO))
                    {
                        if (participante.Persona.pers_sGeneroId == Convert.ToInt16(Enumerador.enmGenero.FEMENINO)
                                && (participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.CASADO) ||
                                   participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.VIUDO)))
                        {
                            listApoderadosEC.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno + " " + participante.Persona.pers_vApellidoCasada).ToUpper());
                        }
                        else
                        {
                            listApoderadosEC.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno).ToUpper());
                        }
                        TotalApoderado++;
                        if (participante.Persona.pers_sGeneroId == (int)Enumerador.enmGenero.MASCULINO)
                        {
                            intTotalApoderadoMasculino++;
                        }
                        else
                        {
                            intTotalApoderadoFemenino++;
                        }
                    }
                }

                //-----------------------------------------------
                //Ordenar la lista de otorgantes y apoderados
                //-----------------------------------------------

                listOtorgantesEC.Sort();
                listApoderadosEC.Sort();
                listInterpretesEC.Sort();
                listTestigosEC.Sort();

                listTextoPosicionado = new List<TextoPosicionadoITextSharp>();
                baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Fonts/cour.ttf"), iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);

                int iFoja = Convert.ToInt32(strFojaInicial);

                string strFuncionarioNombre = string.Empty;
                if (dtDatosPrincipales.Rows[0]["NombreFuncionario"] != null)
                    strFuncionarioNombre = Convert.ToString(dtDatosPrincipales.Rows[0]["NombreFuncionario"]);

                oDocumentoiTextSharp.NombreFuncionario = strFuncionarioNombre;
                oDocumentoiTextSharp.sCuerpoHtmlEC = strCuerpo;
                oDocumentoiTextSharp.ListPalabrasOmitirTextoNotarial = null;
                oDocumentoiTextSharp.ListDocumentoFirmaEC = listObjects;
                oDocumentoiTextSharp.CuerpoBaseFont = baseFont;
                oDocumentoiTextSharp.iFojaActual = iFoja - 1;
                oDocumentoiTextSharp.sTituloEC = strDenominacion;
                oDocumentoiTextSharp.bEsEscrituraPublica = true;
                oDocumentoiTextSharp.bEsVistaPrevia = bVistaPrevia;
                oDocumentoiTextSharp.NumeroEP = dtDatosPrincipales.Rows[0]["NumeroEscrituraPublica"].ToString();

                if (Convert.ToBoolean(dtDatosPrincipales.Rows[0]["Minuta"]))
                {
                    oDocumentoiTextSharp.SMinutaEP = dtDatosPrincipales.Rows[0]["acno_iNumeroActoNotarial"].ToString();
                    oDocumentoiTextSharp.sEtiquetaMinuta = "CON MINUTA";
                }
                else
                {
                    oDocumentoiTextSharp.SMinutaEP = "S/N";
                    oDocumentoiTextSharp.sEtiquetaMinuta = "SIN MINUTA";
                }

                oDocumentoiTextSharp.bGenerarDocumentoAutomaticamente = false;
                oDocumentoiTextSharp.ListOtorgantesEC = listOtorgantesEC;
                oDocumentoiTextSharp.ListApoderadosEC = listApoderadosEC;
                oDocumentoiTextSharp.ListInterpretesEC = listInterpretesEC;
                oDocumentoiTextSharp.ListTestigosEC = listTestigosEC;

                #region Imágenes
                List<BE.MRE.RE_ACTONOTARIALDOCUMENTO> lstImagenes = (List<BE.MRE.RE_ACTONOTARIALDOCUMENTO>)HttpContext.Current.Session["ImagenesContainer"];
                if (lstImagenes != null)
                {
                    if (lstImagenes.Count > 0)
                    {
                        lstImagenes = lstImagenes.Where(x => x.ando_cEstado != "E").ToList();
                        List<ImagenNotarial> lstRutasImagen = new List<ImagenNotarial>();
                        String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
                        ImagenNotarial imagen = new ImagenNotarial();
                        foreach (RE_ACTONOTARIALDOCUMENTO ActoNotarialDocumento in lstImagenes)
                        {
                            imagen = new ImagenNotarial();
                            imagen.vRuta = uploadPath + ActoNotarialDocumento.ando_vRutaArchivo;
                            imagen.vTitulo = ActoNotarialDocumento.ando_vDescripcion;
                            lstRutasImagen.Add(imagen);
                        }
                        oDocumentoiTextSharp.ListImagenes = lstRutasImagen;
                    }
                }
                #endregion

                oDocumentoiTextSharp.CrearTestimonioEscrituraPublicaPDF();

                #region Validar Fojas
                oclsRespuesta.NumeroPagina = oDocumentoiTextSharp.IPageNumber;

                String StrScript = String.Empty;
                Int32 Fojainicial = 0, FojaFinal = 0, acno_iNumeroEscritura = 0;
                String iNumeroLibro = String.Empty;

                Int32 resp = _bl.ValidarFojas(AcnoNotarialId, Convert.ToInt16((int)Enumerador.enmNotarialLibroTipo.ESCRITURA_PUBLICA),
                    Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString()),
                    oclsRespuesta.NumeroPagina, 0, ref Mensaje, ref Fojainicial, ref FojaFinal, ref acno_iNumeroEscritura, ref iNumeroLibro);
                #endregion
                //==============================================================================================                

                oclsRespuesta.bResultado = true;
                oclsRespuesta.vMensaje = Mensaje;
                oRespuesta = new JavaScriptSerializer().Serialize(oclsRespuesta);

                return oRespuesta;
            }
            catch (Exception ex)
            {
                oclsRespuesta.bResultado = false;
                oclsRespuesta.vMensaje = ex.Message;
                oclsRespuesta.NumeroPagina = 0;
                oRespuesta = new JavaScriptSerializer().Serialize(oclsRespuesta);
                return oRespuesta;
            }
        }


        private static string VerFormatoParte(Int64 AcnoNotarialId, int Correlativo, string strNumOficio, bool bVistaPrevia = false)
        {
            clsRespuesta oclsRespuesta = new clsRespuesta();
            string oRespuesta;
            string Mensaje = string.Empty;
            try
            {
                HttpContext.Current.Session["Acto_Notarial_ID"] = AcnoNotarialId;
                HttpContext.Current.Session["Acto_Notarial_Tipo_formato"] = (int)Enumerador.enmFormatoProtocolar.ESCRITURA;
                HttpContext.Current.Session["OficinaConsular"] = Convert.ToInt32(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                Int16 intOficinaConsular = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                ActoNotarialConsultaBL _bl = new ActoNotarialConsultaBL();
                ReportesNotarialesConsultasBL objReportesConsultaBL = new ReportesNotarialesConsultasBL();

                DataTable dtCuerpo = new DataTable();
                DataTable dtDatosPrincipales = new DataTable();
                DataTable dtDatosParte = new DataTable();

                dtDatosPrincipales = _bl.ActonotarialObtenerDatosPrincipales(AcnoNotarialId, intOficinaConsular);
                dtCuerpo = _bl.ObtenerCuerpo(AcnoNotarialId);
                dtDatosParte = objReportesConsultaBL.USP_RP_NOTARIAL_PROTOCOLAR_PARTE(AcnoNotarialId, intOficinaConsular, Convert.ToInt16(Correlativo), strNumOficio);

                string strCuerpo = string.Empty;
                string strCuerpoEscrituraPublica = string.Empty;
                string strDenominacion = string.Empty;
                string strNroEscrituraPublica = string.Empty;
                string vNumeroLibro = string.Empty;
                string vOficinaRegistral = string.Empty;

                if (dtCuerpo.Rows.Count == 0)
                {
                    throw new Exception("El cuerpo no se generó correctamente, verifique los datos ingresados. Línea: 9131.");
                }

//                if (dtCuerpo.Rows[0]["ancu_vCuerpo"] != null) strCuerpo = dtCuerpo.Rows[0]["ancu_vCuerpo"].ToString();
                if (dtCuerpo.Rows[0]["ancu_vCuerpo"] != null) strCuerpoEscrituraPublica = dtCuerpo.Rows[0]["ancu_vCuerpo"].ToString();

                #region Datos Parte
                string strParte = "";
                string strFecha = "";
                if (dtDatosParte.Rows.Count > 0)
                {
                    strFecha = dtDatosParte.Rows[0][0].ToString();

                    strParte += dtDatosParte.Rows[0][1].ToString();                    
                    strParte += dtDatosParte.Rows[0][2].ToString();
                    //strParte += strCuerpo;
                    strCuerpo = strParte;
                }
                #endregion

                if (dtDatosPrincipales.Rows[0]["vDenominacion"] != null) strDenominacion = dtDatosPrincipales.Rows[0]["vDenominacion"].ToString().Trim();
                if (dtDatosPrincipales.Rows[0]["NumeroEscrituraPublica"] != null) strNroEscrituraPublica = dtDatosPrincipales.Rows[0]["NumeroEscrituraPublica"].ToString().Trim();
                if (dtDatosPrincipales.Rows[0]["vNroLibro"] != null) vNumeroLibro = dtDatosPrincipales.Rows[0]["vNroLibro"].ToString().Trim();
                if (dtDatosPrincipales.Rows[0]["vOficinaRegistral"] != null) vOficinaRegistral = dtDatosPrincipales.Rows[0]["vOficinaRegistral"].ToString().Trim();

                Int16 TotalOtorgante = 0;
                Int16 TotalApoderado = 0;

                List<CBE_PARTICIPANTE> loPARTICIPANTES = (List<CBE_PARTICIPANTE>)HttpContext.Current.Session["ParticipanteContainer"];
                if (loPARTICIPANTES == null)
                {
                    oclsRespuesta.bResultado = false;
                    oclsRespuesta.vMensaje = "Ocurrió un error en la obtención de datos. Favor de verificar.";
                    oclsRespuesta.NumeroPagina = 0;
                    oRespuesta = new JavaScriptSerializer().Serialize(oclsRespuesta);
                    return oRespuesta;
                }

                List<DocumentoFirma> listObjects = new List<DocumentoFirma>();

                List<string> listOtorgantes = new List<string>();
                List<string> listApoderados = new List<string>();


                loPARTICIPANTES = loPARTICIPANTES.Where(x => x.anpa_cEstado != "E").OrderBy(order => order.Persona.pers_vNombres).ThenBy(order => order.Persona.pers_vApellidoPaterno).ThenBy(order => order.Persona.pers_vApellidoMaterno).ToList();


                foreach (CBE_PARTICIPANTE participante in loPARTICIPANTES)
                {
                        //if (participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE) ||
                        //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR) ||
                        //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE) ||
                        //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE))

                        if (ObtenerIniciaRecibe(participante.anpa_sTipoParticipanteId) == "INICIA")
                        {
                            if (participante.Persona.pers_sGeneroId == Convert.ToInt16(Enumerador.enmGenero.FEMENINO)
                               && (participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.CASADO) ||
                                  participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.VIUDO)))
                            {
                                listOtorgantes.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno + " " + participante.Persona.pers_vApellidoCasada).ToUpper());
                            }
                            else
                            {
                                listOtorgantes.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno).ToUpper());
                            }
                            
                            TotalOtorgante++;
                        }
                //if (participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.APODERADO) ||
                //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.COMPRADOR) ||
                //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONATARIO) ||
                //    participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPADO))

                        if (ObtenerIniciaRecibe(participante.anpa_sTipoParticipanteId) == "RECIBE")
                    {
                        if (participante.Persona.pers_sGeneroId == Convert.ToInt16(Enumerador.enmGenero.FEMENINO)
                               && (participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.CASADO) ||
                                  participante.Persona.pers_sEstadoCivilId == Convert.ToInt16(Enumerador.enmEstadoCivil.VIUDO)))
                        {
                            listApoderados.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno + " " + participante.Persona.pers_vApellidoCasada).ToUpper());
                        }
                        else
                        {
                            listApoderados.Add((participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno).ToUpper());
                        }
                        TotalApoderado++;
                    }
                }

                //-----------------------------------------------
                //Ordenar la lista de otorgantes y apoderados
                //-----------------------------------------------
                listOtorgantes.Sort();
                listApoderados.Sort();
                //-----------------------------------------------


                List<TextoPosicionadoITextSharp> listTextoPosicionado = new List<TextoPosicionadoITextSharp>();

                float fFontSize = 12;
                float fDocumentHeight = 842 - fFontSize;
                float fLineaHeight = fFontSize + 1.5f;

                iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Fonts/cour.ttf"), iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);

                DocumentoiTextSharp oDocumentoiTextSharp = new DocumentoiTextSharp(null, strCuerpo, HttpContext.Current.Server.MapPath("~/Images/Escudo.JPG"));

                List<string> lstPalabrasOmitir = new List<string>();
                lstPalabrasOmitir.Add("OFICIO N°");
                lstPalabrasOmitir.Add("SEÑOR REGISTRADOR.");
                lstPalabrasOmitir.Add("REGISTROS DE MANDATOS Y PODERES.");
                lstPalabrasOmitir.Add("SUPERINTENDENCIA NACIONAL DE REGISTROS PÚBLICOS.");
                lstPalabrasOmitir.Add("PARTE CONSULAR DE ESCRITURA PÚBLICA DE");
                lstPalabrasOmitir.Add("ZONA REGISTRAL");
                lstPalabrasOmitir.Add("OTORGADO POR");
                lstPalabrasOmitir.Add("ATENTAMENTE,");
                lstPalabrasOmitir.Add(strFecha);
                if (vOficinaRegistral.Length > 0)
                {
                    lstPalabrasOmitir.Add(vOficinaRegistral);
                }

                string strFuncionarioNombre = string.Empty;
                if (dtDatosPrincipales.Rows[0]["NombreFuncionario"] != null)
                    strFuncionarioNombre = Convert.ToString(dtDatosPrincipales.Rows[0]["NombreFuncionario"]);

                oDocumentoiTextSharp.NombreFuncionario = strFuncionarioNombre;
                oDocumentoiTextSharp.ListPalabrasOmitirTextoNotarial = lstPalabrasOmitir;

                oDocumentoiTextSharp.ListDocumentoFirma = listObjects;
                oDocumentoiTextSharp.NombreConsulado = HttpContext.Current.Session["NombreConsulado"].ToString();
                oDocumentoiTextSharp.CuerpoBaseFont = baseFont;
                oDocumentoiTextSharp.sTitulo = strDenominacion;

                oDocumentoiTextSharp.sFecha = strFecha;
                oDocumentoiTextSharp.bEsEscrituraPublica = false;
                oDocumentoiTextSharp.bEsParte = true;

                oDocumentoiTextSharp.bGenerarDocumentoAutomaticamente = false;
                oDocumentoiTextSharp.ListOtorgantes = listOtorgantes;
                oDocumentoiTextSharp.ListApoderados = listApoderados;

                oDocumentoiTextSharp.sNombrePresentante = dtDatosPrincipales.Rows[0]["vAcno_vPresentanteNombre"].ToString();


                #region Imágenes
                List<BE.MRE.RE_ACTONOTARIALDOCUMENTO> lstImagenes = (List<BE.MRE.RE_ACTONOTARIALDOCUMENTO>)HttpContext.Current.Session["ImagenesContainer"];
                if (lstImagenes != null)
                {
                    if (lstImagenes.Count > 0)
                    {
                        lstImagenes = lstImagenes.Where(x => x.ando_cEstado != "E").ToList();
                        List<ImagenNotarial> lstRutasImagen = new List<ImagenNotarial>();
                        String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
                        ImagenNotarial imagen = new ImagenNotarial();
                        foreach (RE_ACTONOTARIALDOCUMENTO ActoNotarialDocumento in lstImagenes)
                        {
                            imagen = new ImagenNotarial();
                            imagen.vRuta = uploadPath + ActoNotarialDocumento.ando_vRutaArchivo;
                            imagen.vTitulo = ActoNotarialDocumento.ando_vDescripcion;
                            lstRutasImagen.Add(imagen);
                        }
                        oDocumentoiTextSharp.ListImagenes = lstRutasImagen;
                    }
                }
                #endregion
                oDocumentoiTextSharp.iLineNumber = 40;
                oDocumentoiTextSharp.bEsSolaUnaHoja = true;
                oDocumentoiTextSharp.CrearDocumentoPDF();

                //-----------------------------------------------------------------------------------------
                byte[] filePartePDF = (byte[])System.Web.HttpContext.Current.Session["binaryData"];


                DocumentoiTextSharp oDocumentoiTextSharpEP = new DocumentoiTextSharp(null, strCuerpoEscrituraPublica, HttpContext.Current.Server.MapPath("~/Images/Escudo.JPG"));
                oDocumentoiTextSharpEP.NombreFuncionario = strFuncionarioNombre;
                oDocumentoiTextSharpEP.ListPalabrasOmitirTextoNotarial = lstPalabrasOmitir;

                oDocumentoiTextSharpEP.ListDocumentoFirma = listObjects;
                oDocumentoiTextSharpEP.NombreConsulado = HttpContext.Current.Session["NombreConsulado"].ToString();
                oDocumentoiTextSharpEP.CuerpoBaseFont = baseFont;
                oDocumentoiTextSharpEP.sTitulo = strDenominacion;

                oDocumentoiTextSharpEP.sFecha = strFecha;
                oDocumentoiTextSharpEP.bEsEscrituraPublica = false;
                oDocumentoiTextSharpEP.bEsParte = true;

                oDocumentoiTextSharpEP.bGenerarDocumentoAutomaticamente = false;
                oDocumentoiTextSharpEP.ListOtorgantes = listOtorgantes;
                oDocumentoiTextSharpEP.ListApoderados = listApoderados;

                oDocumentoiTextSharpEP.sNombrePresentante = dtDatosPrincipales.Rows[0]["vAcno_vPresentanteNombre"].ToString();

                if (lstImagenes != null)
                {
                    if (lstImagenes.Count > 0)
                    {
                        oDocumentoiTextSharpEP.ListImagenes = oDocumentoiTextSharp.ListImagenes;
                    }
                }
                oDocumentoiTextSharpEP.CrearDocumentoPDF();
                byte[] fileEP_PDF = (byte[])System.Web.HttpContext.Current.Session["binaryData"];
                
                List<byte[]> listaDocByte = new List<byte[]>();
                listaDocByte.Add(filePartePDF);
                listaDocByte.Add(fileEP_PDF);

                byte[] file_DocFusionado_PDF = DocumentoiTextSharp.MergeFiles(listaDocByte);

                System.Web.HttpContext.Current.Session["binaryData"] = file_DocFusionado_PDF;
                //-----------------------------------------------------------------------------------------
                Int32 resp = 1;
                oclsRespuesta.NumeroPagina = oDocumentoiTextSharp.IPageNumber;

                if (bVistaPrevia == false)
                {
                    #region Validar Fojas
                    oclsRespuesta.NumeroPagina = oDocumentoiTextSharp.IPageNumber;

                    String StrScript = String.Empty;
                    Int32 Fojainicial = 0, FojaFinal = 0, acno_iNumeroEscritura = 0;
                    String iNumeroLibro = String.Empty;

                    resp = _bl.ValidarFojas(AcnoNotarialId, Convert.ToInt16((int)Enumerador.enmNotarialLibroTipo.ESCRITURA_PUBLICA),
                        Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString()),
                        oclsRespuesta.NumeroPagina, 0, ref Mensaje, ref Fojainicial, ref FojaFinal, ref acno_iNumeroEscritura, ref iNumeroLibro);
                    #endregion
                    if (resp > 0)
                    {
                        #region Guardar_Parte
                        //------------------------------------------------------------------
                        //Fecha: 21/12/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Guardar documento PDF del Parte cuando es Vista previa.
                        //------------------------------------------------------------------
                        string uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
                        string strCarpetaEC = System.Configuration.ConfigurationManager.AppSettings["CarpetaEscrituraPublica"];
                        string uploadFileName = "";
                        string strRutaEC = uploadPath + @strCarpetaEC; ;
                        if (Directory.Exists(strRutaEC))
                        {
                        }
                        else
                        {
                            try
                            {
                                Directory.CreateDirectory(strRutaEC);
                            }
                            catch
                            {
                                throw;
                            }
                        }
                        string _fileName = "documento.pdf";
                        string _RutafileName = "";

                        uploadFileName = Documento.GetUniqueUploadFileNamePDFEscrituraPublica(System.Web.HttpContext.Current.Session["ofco_vSiglas"].ToString(), Convert.ToInt64(HttpContext.Current.Session["Acto_Notarial_ID"].ToString()), strRutaEC, ref _fileName, ref _RutafileName, "P");
                        byte[] filePDF = (byte[])System.Web.HttpContext.Current.Session["binaryData"];
                        string strRutaArchivoDestino = Path.Combine(strRutaEC, uploadFileName);
                        if (filePDF != null)
                        {

                            try
                            {
                                if (System.IO.File.Exists(strRutaArchivoDestino))
                                {
                                    System.IO.File.Delete(strRutaArchivoDestino);
                                }

                                System.IO.File.WriteAllBytes(strRutaArchivoDestino, filePDF);

                                RE_ACTONOTARIALDOCUMENTO lArchivoDigitalizado = new RE_ACTONOTARIALDOCUMENTO();
                                ActoNotarialMantenimiento ActoNotarialBL = new ActoNotarialMantenimiento();
                                RE_ACTONOTARIALDOCUMENTO DocumentoBE = new RE_ACTONOTARIALDOCUMENTO();

                                #region BuscaTipoDocumento
                                DataTable dt = new DataTable();
                                dt = comun_Part1.ObtenerParametrosPorGrupo(HttpContext.Current.Session, Enumerador.enmGrupo.ACTUACION_TIPO_ADJUNTO).Copy();
                                Int16 intTipoDocumentoId = 0;
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    if (dt.Rows[i]["para_vDescripcion"].ToString().Equals("PARTE|.PDF"))
                                    {
                                        intTipoDocumentoId = Convert.ToInt16(dt.Rows[i]["para_sParametroId"].ToString());
                                        break;
                                    }
                                }

                                #endregion

                                #region OBJETO ARCHIVO (CUANDO ES NUEVO)
                                lArchivoDigitalizado.ando_iActoNotarialDocumentoId = 0;
                                lArchivoDigitalizado.ando_sTipoInformacionId = 0;
                                lArchivoDigitalizado.ando_sSubTipoInformacionId = 0;
                                lArchivoDigitalizado.ando_iActoNotarialId = Convert.ToInt64(HttpContext.Current.Session["Acto_Notarial_ID"].ToString());
                                lArchivoDigitalizado.ando_sTipoDocumentoId = intTipoDocumentoId;
                                lArchivoDigitalizado.ando_vDescripcion = "Parte";
                                lArchivoDigitalizado.ando_vRutaArchivo = _fileName;
                                lArchivoDigitalizado.ando_dFechaCreacion = DateTime.Today;//HardCode por mientras
                                lArchivoDigitalizado.ando_sUsuarioCreacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                                lArchivoDigitalizado.ando_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();


                                DocumentoBE = ActoNotarialBL.Obtener(lArchivoDigitalizado);
                                if (DocumentoBE.Error == false)
                                {
                                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                                    string strJson = "";

                                    if (DocumentoBE.ando_iActoNotarialDocumentoId == 0)
                                    {
                                        strJson = serializer.Serialize(ActoNotarialBL.InsertarActoNotarialDocumento(lArchivoDigitalizado));
                                    }
                                    else
                                    {

                                        string strMision = System.Web.HttpContext.Current.Session["ofco_vSiglas"].ToString();

                                        //20211207_172548_P_1363723710.pdf
                                        int intPos = DocumentoBE.ando_vRutaArchivo.IndexOf("_P_");

                                        if (DocumentoBE.ando_vRutaArchivo.Length > 0 && intPos > 0)
                                        {
                                            string strAnio = DocumentoBE.ando_vRutaArchivo.Substring(0, 4);
                                            string strMes = DocumentoBE.ando_vRutaArchivo.Substring(4, 2);
                                            string strDia = DocumentoBE.ando_vRutaArchivo.Substring(6, 2);
                                            string strpathMision = Path.Combine(strRutaEC, strMision);
                                            string strpathAnio = Path.Combine(strpathMision, strAnio);
                                            string strpathAnioMes = Path.Combine(strpathAnio, strMes);
                                            string strpathAnioMesDia = Path.Combine(strpathAnioMes, strDia);

                                            strRutaArchivoDestino = Path.Combine(strpathAnioMesDia, DocumentoBE.ando_vRutaArchivo);

                                            if (System.IO.File.Exists(strRutaArchivoDestino))
                                            {
                                                System.IO.File.Delete(strRutaArchivoDestino);
                                            }
                                            lArchivoDigitalizado.ando_iActoNotarialDocumentoId = DocumentoBE.ando_iActoNotarialDocumentoId;
                                            lArchivoDigitalizado.ando_sTipoDocumentoId = intTipoDocumentoId;
                                            lArchivoDigitalizado.ando_vRutaArchivo = _fileName;
                                            lArchivoDigitalizado.ando_dFechaModificacion = DateTime.Today;
                                            lArchivoDigitalizado.ando_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                                            lArchivoDigitalizado.ando_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();

                                            strJson = serializer.Serialize(ActoNotarialBL.ActualizarActoNotarialDocumento(lArchivoDigitalizado));
                                        }
                                    }
                                }
                                #endregion
                            }
                            catch
                            {

                                throw;
                            }


                        }
                        #endregion
                    }
                }
                else
                {
                    #region Guardar_Parte
                    //------------------------------------------------------------------
                    //Fecha: 21/12/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Guardar documento PDF del Parte cuando es Vista previa.
                    //------------------------------------------------------------------
                    string uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
                    string strCarpetaEC = System.Configuration.ConfigurationManager.AppSettings["CarpetaEscrituraPublica"];
                    string uploadFileName = "";
                    string strRutaEC = uploadPath + @strCarpetaEC;
                    if (Directory.Exists(strRutaEC))
                    {
                    }
                    else
                    {
                        try
                        {
                            Directory.CreateDirectory(strRutaEC);
                        }
                        catch
                        {
                            throw;
                        }
                    }
                    string _fileName = "documento.pdf";
                    string _RutafileName = "";

                    uploadFileName = Documento.GetUniqueUploadFileNamePDFEscrituraPublica(System.Web.HttpContext.Current.Session["ofco_vSiglas"].ToString(), Convert.ToInt64(HttpContext.Current.Session["Acto_Notarial_ID"].ToString()), strRutaEC, ref _fileName, ref _RutafileName, "P");
                    byte[] filePDF = (byte[])System.Web.HttpContext.Current.Session["binaryData"];
                    string strRutaArchivoDestino = Path.Combine(strRutaEC, uploadFileName);
                    if (filePDF != null)
                    {

                        try
                        {
                            if (System.IO.File.Exists(strRutaArchivoDestino))
                            {
                                System.IO.File.Delete(strRutaArchivoDestino);
                            }

                            byte[] fileMarcaAguaPDF = Comun.PonerMarcaAguaPDF(filePDF, "SIN VALOR").ToArray();
                            System.Web.HttpContext.Current.Session["binaryData"] = fileMarcaAguaPDF;
                            System.IO.File.WriteAllBytes(strRutaArchivoDestino, fileMarcaAguaPDF);

                            RE_ACTONOTARIALDOCUMENTO lArchivoDigitalizado = new RE_ACTONOTARIALDOCUMENTO();
                            ActoNotarialMantenimiento ActoNotarialBL = new ActoNotarialMantenimiento();
                            RE_ACTONOTARIALDOCUMENTO DocumentoBE = new RE_ACTONOTARIALDOCUMENTO();

                            #region BuscaTipoDocumento
                            DataTable dt = new DataTable();
                            dt = comun_Part1.ObtenerParametrosPorGrupo(HttpContext.Current.Session, Enumerador.enmGrupo.ACTUACION_TIPO_ADJUNTO).Copy();
                            Int16 intTipoDocumentoId = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i]["para_vDescripcion"].ToString().Equals("PARTE|.PDF"))
                                {
                                    intTipoDocumentoId = Convert.ToInt16(dt.Rows[i]["para_sParametroId"].ToString());
                                    break;
                                }
                            }

                            #endregion

                            #region OBJETO ARCHIVO (CUANDO ES NUEVO)
                            lArchivoDigitalizado.ando_iActoNotarialDocumentoId = 0;
                            lArchivoDigitalizado.ando_sTipoInformacionId = 0;
                            lArchivoDigitalizado.ando_sSubTipoInformacionId = 0;
                            lArchivoDigitalizado.ando_iActoNotarialId = Convert.ToInt64(HttpContext.Current.Session["Acto_Notarial_ID"].ToString());
                            lArchivoDigitalizado.ando_sTipoDocumentoId = intTipoDocumentoId;
                            lArchivoDigitalizado.ando_vDescripcion = "Vista previa del Parte";
                            lArchivoDigitalizado.ando_vRutaArchivo = _fileName;
                            lArchivoDigitalizado.ando_dFechaCreacion = DateTime.Today;//HardCode por mientras
                            lArchivoDigitalizado.ando_sUsuarioCreacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                            lArchivoDigitalizado.ando_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();


                            DocumentoBE = ActoNotarialBL.Obtener(lArchivoDigitalizado);
                            if (DocumentoBE.Error == false)
                            {
                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                string strJson = "";

                                if (DocumentoBE.ando_iActoNotarialDocumentoId == 0)
                                {
                                    strJson = serializer.Serialize(ActoNotarialBL.InsertarActoNotarialDocumento(lArchivoDigitalizado));
                                }
                                else
                                {

                                    string strMision = System.Web.HttpContext.Current.Session["ofco_vSiglas"].ToString();

                                    //20211207_172548_P_1363723710.pdf
                                    int intPos = DocumentoBE.ando_vRutaArchivo.IndexOf("_P_");

                                    if (DocumentoBE.ando_vRutaArchivo.Length > 0 && intPos > 0)
                                    {
                                        string strAnio = DocumentoBE.ando_vRutaArchivo.Substring(0, 4);
                                        string strMes = DocumentoBE.ando_vRutaArchivo.Substring(4, 2);
                                        string strDia = DocumentoBE.ando_vRutaArchivo.Substring(6, 2);
                                        string strpathMision = Path.Combine(strRutaEC, strMision);
                                        string strpathAnio = Path.Combine(strpathMision, strAnio);
                                        string strpathAnioMes = Path.Combine(strpathAnio, strMes);
                                        string strpathAnioMesDia = Path.Combine(strpathAnioMes, strDia);

                                        strRutaArchivoDestino = Path.Combine(strpathAnioMesDia, DocumentoBE.ando_vRutaArchivo);

                                        if (System.IO.File.Exists(strRutaArchivoDestino))
                                        {
                                            System.IO.File.Delete(strRutaArchivoDestino);
                                        }
                                        lArchivoDigitalizado.ando_iActoNotarialDocumentoId = DocumentoBE.ando_iActoNotarialDocumentoId;
                                        lArchivoDigitalizado.ando_sTipoDocumentoId = intTipoDocumentoId;
                                        lArchivoDigitalizado.ando_vRutaArchivo = _fileName;
                                        lArchivoDigitalizado.ando_dFechaModificacion = DateTime.Today;
                                        lArchivoDigitalizado.ando_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                                        lArchivoDigitalizado.ando_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();

                                        strJson = serializer.Serialize(ActoNotarialBL.ActualizarActoNotarialDocumento(lArchivoDigitalizado));
                                    }
                                }
                            }
                            #endregion
                        }
                        catch
                        {

                            throw;
                        }


                    }
                    #endregion
                }
                //-----------------------------------------------------
                if (resp == 0)
                {
                    oclsRespuesta.bResultado = false;
                }
                else
                {
                    oclsRespuesta.bResultado = true;
                }
                //-----------------------------------------------------
                oclsRespuesta.vMensaje = Mensaje;
                oRespuesta = new JavaScriptSerializer().Serialize(oclsRespuesta);
                return oRespuesta;
            }
            catch (Exception ex)
            {
                oclsRespuesta.bResultado = false;
                oclsRespuesta.vMensaje = ex.Message;
                oclsRespuesta.NumeroPagina = 0;
                oRespuesta = new JavaScriptSerializer().Serialize(oclsRespuesta);
                return oRespuesta;
            }
        }

        private void LimpiarDatosVinculacion()
        {
            chkImpresionCorrecta.Enabled = false;
            chkImpresionCorrecta.Checked = false;

            //----------------------------------------------
            //Fecha: 22/05/2020
            //Autor: Miguel Márquez Beltrán
            //Obs: Observaciones_Escrituras_V1.doc
            //Req.: Req. Nro. 4. Al seleccionar el botón LIMPIAR, el botón 
            //      de ESCRITURAS PUBLICAS desaparece
            //      Y se deshabilita el text del código del autoadhesivo.
            //----------------------------------------------
            //txtCodigoInsumo.Enabled = false;
            //btnFormato.Visible = false;
            //----------------------------------------------

            txtCodigoInsumo.Text = "";
            txtCodigoInsumo.Enabled = true;
            hdn_seleccionado.Value = "-1";


            //--------------------------------------------
            //Fecha: 06/01/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Desactivar el botón autoadhesivo y
            //          activar cuando se haya grabado.
            //--------------------------------------------

            //gdvVinculacion.SelectedIndex = -1;
            Int32 intTipoFormato = Convert.ToInt32(hdn_tipo_formato_proto.Value);
            if (intTipoFormato == (int)Enumerador.enmNotarialTipoFormato.PARTE)
            {
                MostrarSoloParte(true);
            }
            
        }

        private void MostrarSoloParte(bool bolHabilitar = false)
        {
            lblNumeroOficioPaso.Visible = bolHabilitar;
            txtNumeroOficioAdi.Visible = bolHabilitar;
        }

        #endregion

        #endregion

        #region Pestaña: Digitalizacion

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

            Label lblNombreArchivo = (Label)ctrlUploader2.FindControl("lblNombreArchivo");
            lblNombreArchivo.Text = "";
            lblNombreArchivo.Visible = false;

            System.Web.UI.HtmlControls.HtmlTableCell msjeSucess = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader2.FindControl("msjeSucess");
            msjeSucess.Visible = false;

            System.Web.UI.HtmlControls.HtmlTableCell msjeWarning = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader2.FindControl("msjeWarning");
            msjeWarning.Visible = false;

            System.Web.UI.HtmlControls.HtmlTableCell msjeError = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader2.FindControl("msjeError");
            msjeError.Visible = false;

            //btnVisualizarDigitalizacion.Enabled = false;
        }

        protected void btnCancelarTab6_Click(object sender, EventArgs e)
        {
            string strScript = string.Empty;
            strScript = @"$(function(){{
                            LimpiarTabArchivoDigitalizado();
                        }});";
            strScript = string.Format(strScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "LimpiarTabArchivoDigitalizado", strScript, true);

            Label lblNombreArchivo = (Label)ctrlUploader2.FindControl("lblNombreArchivo");
            lblNombreArchivo.Text = "";
            lblNombreArchivo.Visible = false;

            System.Web.UI.HtmlControls.HtmlTableCell msjeSucess = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader2.FindControl("msjeSucess");
            msjeSucess.Visible = false;

            System.Web.UI.HtmlControls.HtmlTableCell msjeWarning = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader2.FindControl("msjeWarning");
            msjeWarning.Visible = false;

            System.Web.UI.HtmlControls.HtmlTableCell msjeError = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader2.FindControl("msjeError");
            msjeError.Visible = false;

            //btnVisualizarDigitalizacion.Enabled = false;
        }

        protected void MyUserControlUploader2Event_Click(object sender, EventArgs e)
        {
            string strScript;

            Label lblNombreArchivoDigitalizado = (Label)ctrlUploader2.FindControl("lblNombreArchivo");
            lblNombreArchivoDigitalizado.Visible = true;

            if (lblNombreArchivoDigitalizado.Text != "")
            {
                //btnVisualizarDigitalizacion.Enabled = true;
            }

            hidNomAdjFile2.Value = ctrlUploader2.FileName;

            strScript = string.Empty;
            strScript = @"$(function(){{
                            MoveTabIndex(6);
                        }});";
            strScript = string.Format(strScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoveTabIndex6", strScript, true);

            //Mantener deshabilitados los controles de los demas tabs
            strScript = string.Empty;
            strScript = @"$(function(){{
                            DeshabilitaElementosTabs();
                        }});";
            strScript = string.Format(strScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "DeshabilitaElementosTabs", strScript, true);

            //Mantener con datos los labels de introduccion y conclusion
            strScript = string.Empty;
            strScript = @"$(function(){{
                            Rellenar_introduccion_conclusion_escritura();
                        }});";
            strScript = string.Format(strScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "Rellenar_introduccion_conclusion_escritura", strScript, true);

            this.ddlFuncionario.SelectedValue = hdn_FuncionarioId.Value;
            
            //-------------------------------------------------
            //Fecha: 14/01/2022
            //Autor: Miguel Márquez Beltrán
            //Motivo: Adjuntar archivo a la grilla de adjuntos.
            //-------------------------------------------------
            strScript = string.Empty;
            strScript = @"$(function(){{
                            AddDocumentoDigitalizado();
                        }});";
            strScript = string.Format(strScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "AddDocumentoDigitalizado", strScript, true);

            //-------------------------------------------------
        }

        //protected void btnVisualizarDigitalizacion_Click(object sender, EventArgs e)
        //{
        //    string strScript = string.Empty;

        //    Label strNombreArchivo = (Label)ctrlUploader2.FindControl("lblNombreArchivo");
        //    string strArchivo = strNombreArchivo.Text;
        //    string strRuta = ConfigurationManager.AppSettings["UploadPath"].ToString() + "\\" + strArchivo;

        //    String[] Ext = strArchivo.Split('.');
        //    Session["strTipoArchivo"] = "." + Ext[1].ToString();

        //    if (File.Exists(strRuta))
        //    {
        //        try
        //        {
        //            string strUrl = "../Registro/FrmPreviewNotarial.aspx?Ruta=" + strArchivo;
        //            strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=700,height=500,left=100,top=100');";
        //            Comun.EjecutarScript(Page, strScript);
        //        }
        //        catch (Exception ex)
        //        {
        //            strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTOS NOTARIALES - PROTOCOLARES",
        //                "El archivo se pudo abrir. Vuelva a intentarlo." +
        //                "\n(" + ex.Message + ")");
        //            Comun.EjecutarScript(Page, strScript);
        //        }
        //    }
        //}

        protected void Gdv_Adjunto_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 lRowIndex = Convert.ToInt32(e.CommandArgument);
            List<RE_ACTONOTARIALDOCUMENTO> Archivo = (List<RE_ACTONOTARIALDOCUMENTO>)Session["DocumentoDigitalizadoContainer"];


            String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
            String uploadFile;

            int indice = 0;

            if (Archivo != null)
                indice = Archivo.Count;
            else
                indice = -1;

            switch (e.CommandName.ToString())
            {
                case "Visualizar":

                    //if (indice <= 0)
                    //{
                    //    string strScriptt = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", Constantes.CONST_MENSAJE_PERDIDA_SESSION_ACTUACION);
                    //    EjecutarScript(strScriptt);
                    //    return;
                    //}

                    Session["strTipoArchivo"] = ".pdf";

                    string strUrl = "../Registro/FrmPreviewNotarial.aspx?Ruta=" + Convert.ToString(Gdv_Adjunto.Rows[lRowIndex].Cells[2].Text);
                    string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=700,height=500,left=100,top=100');";
                    Comun.EjecutarScript(Page, strScript);

                    break;

                case "Editar":

                    //if (indice <= 0)
                    //{
                    //    string strScriptt = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", Constantes.CONST_MENSAJE_PERDIDA_SESSION_ACTUACION);
                    //    EjecutarScript(strScriptt);
                    //    return;
                    //}

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
                    lblNombreArchivoDigitalizado.Text = Page.Server.HtmlDecode(Gdv_Adjunto.Rows[lRowIndex].Cells[2].Text);

                    this.Txt_AdjuntoDescripcion.Text = Page.Server.HtmlDecode(Gdv_Adjunto.Rows[lRowIndex].Cells[3].Text);

                    break;

                case "Eliminar":


                    //if (indice <= 0)
                    //{
                    //    string strScriptt = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuación Notarial", Constantes.CONST_MENSAJE_PERDIDA_SESSION_ACTUACION);
                    //    EjecutarScript(strScriptt);
                    //    return;
                    //}


                    if (Comun.ToNullInt64(Convert.ToInt64(Gdv_Adjunto.Rows[lRowIndex].Cells[0].Text)) <= 0)
                    {
                        var existe = ((List<RE_ACTONOTARIALDOCUMENTO>)Session["DocumentoDigitalizadoContainer"]).Find(p => p.ando_iActoNotarialDocumentoId == Comun.ToNullInt64(Convert.ToInt64(Gdv_Adjunto.Rows[lRowIndex].Cells[0].Text)));
                        ((List<RE_ACTONOTARIALDOCUMENTO>)Session["DocumentoDigitalizadoContainer"]).Remove(existe);
                    }
                    else
                    {
                        ((List<RE_ACTONOTARIALDOCUMENTO>)Session["DocumentoDigitalizadoContainer"]).Find(p => p.ando_iActoNotarialDocumentoId == Comun.ToNullInt64(Convert.ToInt64(Gdv_Adjunto.Rows[lRowIndex].Cells[0].Text))).ando_cEstado = "E";
                    }

                    uploadFile = string.Empty;
                    uploadFile = Convert.ToString(Page.Server.HtmlDecode(Gdv_Adjunto.Rows[lRowIndex].Cells[2].Text)).Trim();

                    if (uploadFile.Length > 0)
                    {
                        if (System.IO.File.Exists(uploadPath + "/" + uploadFile))
                        {
                            System.IO.File.Delete(uploadPath + "/" + uploadFile);
                        }
                    }

                    Session["DocumentoDigitalizadoContainer"] = (List<RE_ACTONOTARIALDOCUMENTO>)Archivo;
                    this.Gdv_Adjunto.DataSource = CrearTablaDigitalizacionArchivo(null);
                    this.Gdv_Adjunto.DataBind();

                    Label lblNombreArchivo = (Label)ctrlUploader2.FindControl("lblNombreArchivo");
                    lblNombreArchivo.Text = "";
                    lblNombreArchivo.Visible = false;

                    System.Web.UI.HtmlControls.HtmlTableCell msjeSucess = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader2.FindControl("msjeSucess");
                    msjeSucess.Visible = false;

                    System.Web.UI.HtmlControls.HtmlTableCell msjeWarning = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader2.FindControl("msjeWarning");
                    msjeWarning.Visible = false;

                    System.Web.UI.HtmlControls.HtmlTableCell msjeError = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader2.FindControl("msjeError");
                    msjeError.Visible = false;

                    //btnVisualizarDigitalizacion.Enabled = false;

                    break;
            }
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
            #endregion

            this.Gdv_Adjunto.DataSource = dt;
            this.Gdv_Adjunto.DataBind();
        }

        [System.Web.Services.WebMethod]
        public static string adicionar_archivo(string archivo)
        {
            RE_ACTONOTARIALDOCUMENTO lArchivoDigitalizado = new RE_ACTONOTARIALDOCUMENTO();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var jsonObject = serializer.Deserialize<dynamic>(archivo);

            #region OBJETO ARCHIVO (CUANDO ES NUEVO)
            lArchivoDigitalizado.ando_iActoNotarialDocumentoId = 0;
            lArchivoDigitalizado.ando_iActoNotarialId = Convert.ToInt64(jsonObject[0]["ando_iActoNotarialId"]);
            lArchivoDigitalizado.ando_sTipoDocumentoId = Convert.ToInt16(jsonObject[0]["ando_sTipoDocumentoId"]);
            lArchivoDigitalizado.ando_sTipoInformacionId = Convert.ToInt16(jsonObject[0]["ando_sTipoInformacionId"]);
            lArchivoDigitalizado.ando_sSubTipoInformacionId = Convert.ToInt16(jsonObject[0]["ando_sSubTipoInformacionId"]);
            lArchivoDigitalizado.ando_vRutaArchivo = jsonObject[0]["ando_vRutaArchivo"].ToString();
            lArchivoDigitalizado.ando_vDescripcion = jsonObject[0]["ando_vDescripcion"].ToString();
            #endregion

            #region ACTUALIZANDO VARIABLE DE SESSION

            List<RE_ACTONOTARIALDOCUMENTO> ParametrosContainer = new List<RE_ACTONOTARIALDOCUMENTO>();

            if (HttpContext.Current.Session["DocumentoDigitalizadoContainer"] != null)
            {
                ParametrosContainer = (List<RE_ACTONOTARIALDOCUMENTO>)HttpContext.Current.Session["DocumentoDigitalizadoContainer"];
            }

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
            serializer.MaxJsonLength = Int32.MaxValue;
            var jsonObject = serializer.Deserialize<dynamic>(larchivoDigitalizado);

            List<RE_ACTONOTARIALDOCUMENTO> lArchivo = (List<RE_ACTONOTARIALDOCUMENTO>)HttpContext.Current.Session["DocumentoDigitalizadoContainer"];

            if (lArchivo == null)
            {
                return "";
            }

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
                obj.ando_dFechaCreacion = DateTime.Today;//HardCode por mientras
                obj.ando_sUsuarioCreacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                obj.ando_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();

                //Log : Modificación
                obj.ando_dFechaModificacion = DateTime.Today;//HardCode por mientras
                obj.ando_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                obj.ando_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
            }

            ActoNotarialMantenimiento mnt = new ActoNotarialMantenimiento();
            return serializer.Serialize(mnt.InsertarActoNotarialDocumento(lArchivo)).ToString();
        }

        #endregion

        #region Funciones

        private void CargaDatosIniciales()
        {
            Session["ActoNotarialParticipanteId"] = "0";
            ViewState["ActoNotarialParticipanteId"] = "0";
            ViewState["ExtraIndiceEdicion"] = -1;

            Session[strVarActuacionDetalleDT] = null; //Controla los modos de operacion del modulo

            LblDescMtoML.Text = Session[Constantes.CONST_SESION_TIPO_MONEDA].ToString();
            LblDescTotML.Text = Session[Constantes.CONST_SESION_TIPO_MONEDA].ToString();

            //string strGUID = "";

            //if (HFGUID.Value.Length > 0)
            //{
            //    strGUID = HFGUID.Value;
            //}

            if (ViewState["iPersonaId"] == null)
                return;

            this.hdn_actu_iPersonaRecurrenteId.Value = (ViewState["iPersonaId"]).ToString();

            if (Session[Constantes.CONST_SESION_ACTONOTARIAL_ID] == null)
            {
                this.hdn_acno_iActoNotarialId.Value = "0";
            }
            else
            {
                this.hdn_acno_iActoNotarialId.Value = (Session[Constantes.CONST_SESION_ACTONOTARIAL_ID]).ToString();
            }
            if (Session[Constantes.CONST_SESION_ACTUACION_ID] == null)
            {
                this.hdn_acno_iActuacionId.Value = "0";
            }
            else
            {
                this.hdn_acno_iActuacionId.Value = (Session[Constantes.CONST_SESION_ACTUACION_ID]).ToString();
            }

            // ID(s) que deben cambiar al EDITAR ....
            this.hdn_ancu_iActoNotarialCuerpoId.Value = "0";

            // ID(s) que deben cambiar al EDITAR ....
            this.hdn_ancu_iActoNotarialCuerpoId.Value = "0";

            this.hdn_acno_iActoNotarialReferenciaId.Value = "0";  // Para el Sub flujo de Rectificaciones ....    

            this.hdn_NoExistePersonaBuscada.Value = "0"; //Para búsqueda de participantes

            //Creando List<CBE_PARTICIPANTE>
            List<CBE_PARTICIPANTE> lParticipantes = new List<CBE_PARTICIPANTE>();
            Session["ParticipanteContainer"] = (List<CBE_PARTICIPANTE>)lParticipantes;

            List<BE.MRE.RE_ACTONOTARIALDOCUMENTO> lstImagenEliminada = new List<BE.MRE.RE_ACTONOTARIALDOCUMENTO>();
            Session["Imagenes_eliminadas"] = (List<BE.MRE.RE_ACTONOTARIALDOCUMENTO>)lstImagenEliminada;

            CBE_PARTICIPANTE lParticipante = new CBE_PARTICIPANTE();
            lParticipante.acpa_sTipoParticipanteId_desc = "";
            lParticipante.peid_sDocumentoTipoId_desc = "";
            lParticipante.Identificacion.peid_vDocumentoNumero = "";
            lParticipante.Persona.pers_vApellidoPaterno = "";
            lParticipante.Persona.pers_vApellidoMaterno = "";
            lParticipante.Persona.pers_vApellidoCasada = "";
            lParticipante.Persona.pers_vNombres = "";
            lParticipante.pers_sNacionalidadId_desc = "";

            #region creando datatable
            DataTable dt = new DataTable();
            dt.Columns.Add("acpa_sTipoParticipanteId_desc", typeof(string));
            dt.Columns.Add("peid_sDocumentoTipoId_desc", typeof(string));
            dt.Columns.Add("peid_vDocumentoNumero", typeof(string));
            dt.Columns.Add("participante", typeof(string));
            dt.Columns.Add("pers_sNacionalidadId_desc", typeof(string));

            dt.Columns.Add("pers_bIncapacidadFlag", typeof(Boolean));
            dt.Columns.Add("anpa_iActoNotarialParticipanteIdAux", typeof(Int64));
            dt.Columns.Add("anpa_sReferenciaParticipanteId", typeof(Int64));
            dt.Columns.Add("anpa_sTipoParticipanteId", typeof(Int16));

            dt.Columns.Add("pers_sGentilicioId_desc", typeof(string));
            dt.Columns.Add("pers_sIdiomaNatalId_desc", typeof(string));

            //-------------------------------------------------------------------
            //Fecha: 20/09/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Asignar la columna Fecha de suscripción
            //-------------------------------------------------------------------
            dt.Columns.Add("anpa_dFechaSuscripcion", typeof(DateTime));

            dt.Columns.Add("pers_vDescripcionIncapacidad", typeof(string));

            dt.Columns.Add("anpa_cEstado", typeof(string));

            //-----------------------------------------------------------------
            //Fecha: 01/12/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Mostrar la dirección y el ubigeo.
            //-----------------------------------------------------------------
            dt.Columns.Add("resi_vResidenciaDireccion", typeof(string));
            dt.Columns.Add("resi_cResidenciaUbigeo", typeof(string));
            //-----------------------------------------------------------------


            #endregion

            this.grd_Participantes.DataSource = dt;
            this.grd_Participantes.DataBind();

            this.grd_Otorgantes.DataSource = ObtenerOtorgantesOrdenados(dt);
            this.grd_Otorgantes.DataBind();
            updGrillaOtorgantes.Update();


            CrearListDigitalizacionArchivo();
            InicializarGrillaDigitalizacionArchivo();

            Session[strVarActuacionDetalleIndice] = -1;

            #region Datos Personales

            lblRecurrente.Text = string.Empty;
            string strEtiquetaSolicitante = string.Empty;
            HF_NOMBRE_RECURRENTE.Value = string.Empty;
            HF_DOCUMENTO.Value = string.Empty;

            if (ViewState["Nombre"] != null)
            {
                HF_NOMBRE_RECURRENTE.Value += Comun.AplicarInicialMayuscula(ViewState["Nombre"].ToString());
            }


            if (ViewState["ApePat"] != null)
            {
                strEtiquetaSolicitante += ViewState["ApePat"].ToString() + " ";
                HF_NOMBRE_RECURRENTE.Value += " " + Comun.AplicarInicialMayuscula(ViewState["ApePat"].ToString());
            }

            if (ViewState["ApeMat"] != null)
            {
                strEtiquetaSolicitante += ViewState["ApeMat"].ToString() + " ";
                HF_NOMBRE_RECURRENTE.Value += " " + Comun.AplicarInicialMayuscula(ViewState["ApeMat"].ToString());
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
                HF_DOCUMENTO.Value += ViewState["DescTipDoc"].ToString();
            }

            if (ViewState["NroDoc"] != null)
            {
                strEtiquetaSolicitante += ViewState["NroDoc"].ToString();
                HF_DOCUMENTO.Value += " " + ViewState["NroDoc"].ToString();
            }

            lblRecurrente.Text = strEtiquetaSolicitante;

            #endregion Datos Personales
        }

        private void CargaListasDesplegables()
        {
            //DataTable dtTipoActoNotarial = new DataTable();
            //dtTipoActoNotarial = Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_NOTARIAL_TIPO_ACTO_PROTOCOLAR);
            //DataView dvTipoActoNotarial = dtTipoActoNotarial.DefaultView;
            //DataTable dtOrdenadoTipActNotarial = dvTipoActoNotarial.ToTable();
            //dtOrdenadoTipActNotarial.DefaultView.Sort = "tOrden ASC";

            //----------------------------------------------------------------------------------------------------
            DataTable dtOrdenadoTipActNotarial = new DataTable();

            dtOrdenadoTipActNotarial = ObtenerTipoActoNotarial();

            Util.CargarDropDownList(Cmb_TipoActoNotarial, dtOrdenadoTipActNotarial, "descripcion", "Id", true);
            //----------------------------------------------------------------------------------------------------
            // Fecha: 01/04/2020
            // Autor: Miguel Márquez Beltrán
            // Motivo: Optimizar la lectura de registros.
            //----------------------------------------------------------------------------------------------------
            Enumerador.enmGrupo[] arrGrupos = { Enumerador.enmGrupo.PERSONA_TIPO, Enumerador.enmGrupo.EMPRESA_TIPO_DOCUMENTO, Enumerador.enmGrupo.PERSONA_GENERO,
                                                Enumerador.enmGrupo.PERSONA_NACIONALIDAD, Enumerador.enmGrupo.TRADUCCION_IDIOMA};
            DropDownList[] arrDDL = { ddl_pers_sPersonaTipoId, ddl_empr_sTipoDocumentoId, ddl_pers_sGeneroId, ddl_pers_sNacionalidadId, ddl_pers_sIdiomaNatalId };

            DataTable dtGrupoParametros = new DataTable();

            dtGrupoParametros = comun_Part1.ObtenerParametrosListaGrupos(Session, arrGrupos);

            Util.CargarParametroDropDownListDesdeListaGrupos(arrDDL, arrGrupos, dtGrupoParametros, true);



            Enumerador.enmGrupo[] arrGrupos2 = { Enumerador.enmGrupo.PERSONA_GENERO, Enumerador.enmGrupo.PERSONA_GENERO };
            DropDownList[] arrDDL2 = { ddl_GerenoPresentante, ddlAdicionalGenero };
            Util.CargarParametroDropDownListDesdeListaGrupos(arrDDL2, arrGrupos2, dtGrupoParametros, true);
            //----------------------------------------------------------------------------------------------------
            //Util.CargarParametroDropDownList(this.ddl_pers_sPersonaTipoId, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_TIPO), false);
            //Util.CargarParametroDropDownList(this.ddl_empr_sTipoDocumentoId, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.EMPRESA_TIPO_DOCUMENTO), true);

            //Util.CargarParametroDropDownList(this.ddl_pers_sGeneroId, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_GENERO), true);
            //Util.CargarParametroDropDownList(this.ddl_GerenoPresentante, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_GENERO), true);
            //Util.CargarParametroDropDownList(this.ddlAdicionalGenero, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_GENERO), true);

            Util.CargarParametroDropDownList(this.ddl_pers_sEstadoCivilId, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.ESTADO_CIVIL), true);
            Util.CargarDropDownList(ddl_peid_sDocumentoTipoId, comun_Part1.ObtenerDocumentoIdentidad(), "Valor", "Id", true);

            //comun_Part3.CargarUbigeo(Session, this.ddl_UbigeoPais, Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT, string.Empty, string.Empty, true);
            //comun_Part3.CargarUbigeo(Session, this.ddl_UbigeoRegion, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, string.Empty, string.Empty, true);
            //comun_Part3.CargarUbigeo(Session, this.ddl_UbigeoCiudad, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, string.Empty, string.Empty, true);

            //Util.CargarParametroDropDownList(this.ddl_pers_sNacionalidadId, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_NACIONALIDAD), true);
            Util.CargarParametroDropDownList(this.ddl_pers_sProfesionId, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.OCUPACION), true);
            //Util.CargarParametroDropDownList(this.ddl_pers_sIdiomaNatalId, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.TRADUCCION_IDIOMA), true);

            Session["Combo_Informacion"] = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_NOTARIAL_TIPO_INFORMACION_INSERTO);

            DataTable dtTipoPago = new DataTable();
            dtTipoPago = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACREDITACION_TIPO_COBRO);

            DataView dv = dtTipoPago.DefaultView;
            DataTable dtTipoPagoOrdenadoOrdenado = dv.ToTable();
            dtTipoPagoOrdenadoOrdenado.DefaultView.Sort = "torden ASC";

            Util.CargarParametroDropDownList(ddlTipoPago, dtTipoPagoOrdenadoOrdenado, true);

            DataTable dtFuncionarios = new DataTable();
            dtFuncionarios = funcionario.dtFuncionario(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), 0);
            
            CargarFuncionarios(ddlFuncionario, dtFuncionarios);
            CargarFuncionarios(ddlFuncionarioResponsable, dtFuncionarios);

            #region Carga Lista: Texto Normativo
            //DataTable s_Datos = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_NOTARIAL_SUB_TIPO_INFORMACION_INSERTO);
            //DataTable new_Data = null;
            //new_Data = (from dtsi in s_Datos.AsEnumerable()
            //            where Comun.ToNullInt64(dtsi["para_vReferencia"]) == ((int)Enumerador.enmProtocolarTipoInformacion.TEXTO_NORMATIVO)
            //            select dtsi).CopyToDataTable();
            //new_Data.DefaultView.Sort = "torden asc";

           
            //carga los checkbos de normativos
            //string html = "";
            //foreach (DataRow row in new_Data.Rows)
            //{
            //    html = html + "<input type='checkbox' id='check" + row["id"].ToString() + "' onclick='setearTextoNormativo();'/><label for='check" + row["id"].ToString() + "'>" + row["descripcion"].ToString() + "</label><br>";
            //}
            //divListNormativo.InnerHtml = html;

            #endregion

            //----------------------------------------------------
            DataTable dtOficinaRegistral = new DataTable();
            MaestroOficinasBL objOficinasRegistralesBL = new MaestroOficinasBL();
            int intTotalPages = 0;
            dtOficinaRegistral = objOficinasRegistralesBL.consultar(0, "", "", "A", "S", 1000, "1", "N", ref intTotalPages);
            Util.CargarDropDownList(ddl_OficinaRegistralRegistrador, dtOficinaRegistral, "ofic_vDescripcion", "ofic_iOficinaId", true);

            ddl_OficinaRegistralRegistrador.DataBind();
            //----------------------------------------------------

            DataTable dtTipDoc = new DataTable();
            dtTipDoc = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.DOCUMENTO_IDENTIDAD);
            DataView dv2 = dtTipDoc.DefaultView;
            DataTable dtOrdenado = dv2.ToTable();
            dtOrdenado.DefaultView.Sort = "Id ASC";


            Util.CargarParametroDropDownList(ddlNomBanco, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.BANCO), true);

            Util.CargarDropDownList(ddl_TipoDocrepresentante, dtOrdenado, "Valor", "Id", true);

            // Solicitud de Parte y/o Testimonio
            CargarFuncionarios(ddlAdicionalFuncionario, dtFuncionarios);

            Util.CargarDropDownList(ddlAdicionalTipoDoc, dtOrdenado, "Valor", "Id", true);
            //--------------------------------------------------
            //Fecha: 09/11/2021
            //Autor: Miguel Márquez
            //Motivo: LLenar la lista de oficina consular
            //--------------------------------------------------
            DataTable dtOficinaActivas = new DataTable();
            dtOficinaActivas = Comun.ObtenerOficinasActivas(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString()));
            Util.CargarDropDownList(ddlOficinaConsularPri, dtOficinaActivas, "ofco_vNombre", "ofco_sOficinaConsularId", true, "- SELECCIONAR -");
            //--------------------------------------------------
            DataTable dtTipoNorma = new DataTable();
            dtTipoNorma = comun_Part1.ObtenerParametrosPorGrupo(Session, "NORMA-DOCUMENTOS");
            Util.CargarParametroDropDownList(ddlFuenteNormaConsulta, dtTipoNorma, true);
            this.ddlTituloNormaConsulta.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
            this.ddlArticuloNormaConsulta.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
            ViewState["VIGENTE"] = ObtenerEstadoNorma("VIGENTE");
            ViewState["ESCRITURAS"] = ObtenerGrupoNorma("ESCRITURAS");
            //--------------------------------------------------
        }

        private void ModoEdicion()
        {
            try
            {
                string strScript;

                if (Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value) != 0)
                {
                    #region Registro
                    RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();

                    //Obtener
                    ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();
                    lACTONOTARIAL.acno_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
                    lACTONOTARIAL.acno_iActuacionId = Convert.ToInt64(this.hdn_acno_iActuacionId.Value);
                    lACTONOTARIAL.acno_sTipoActoNotarialId = Convert.ToInt16(Enumerador.enmNotarialTipoActo.PROTOCOLAR);
                    lACTONOTARIAL = lActoNotarialConsultaBL.obtener(lACTONOTARIAL);

                    //Mostrar
                    this.Cmb_TipoActoNotarial.SelectedValue = lACTONOTARIAL.acno_sSubTipoActoNotarialId.ToString();


                    
                    EjecutarScript("SetLabelActoNotarial('" + Cmb_TipoActoNotarial.SelectedItem.Text + "');");

                    Session["TipoActo"] = Cmb_TipoActoNotarial.SelectedValue;
                    //----------------------------------------------
                    //Fecha: 06/10/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Cambiar el texto de lblNroOficio
                    //      de "N° de Oficio: " a "Oficio a RENIEC: "
                    //      cuando el tipo de acto sea:
                    //      "RENUNCIA A LA NACIONALIDAD PERUANA"
                    //----------------------------------------------
                    string strTipoActoProtocolar = Cmb_TipoActoNotarial.SelectedItem.Text.Trim().ToUpper();
                    if (strTipoActoProtocolar == "RENUNCIA A LA NACIONALIDAD PERUANA")
                    {
                        lblNroOficio.Text = "Oficio a RENIEC: ";
                    }
                    else
                    {
                        lblNroOficio.Text = "N° de Oficio: ";
                    }

                    HF_FORMATO_ACTIVO.Value = lACTONOTARIAL.acno_sSubTipoActoNotarialId.ToString();
                    this.chk_acno_bFlagMinuta.Checked = lACTONOTARIAL.acno_bFlagMinuta;
                    Session["sEstadoActoNotarial"] = lACTONOTARIAL.acno_sEstadoId;

                    this.Txt_Denominacion.Text = lACTONOTARIAL.acno_vDenominacion;

                    this.ddlFuncionario.SelectedValue = lACTONOTARIAL.acno_IFuncionarioAutorizadorId.ToString();
                    hdn_FuncionarioId.Value = this.ddlFuncionario.SelectedValue;

                    if (lACTONOTARIAL.acno_vNumeroEscrituraPublica.Trim().Length == 0) { 
                        this.txtNroEscritura.Text = "MRE000"; 
                    }
                    else { 
                        this.txtNroEscritura.Text = lACTONOTARIAL.acno_vNumeroEscrituraPublica;
                        
                    }

                    this.txtNroFojaIni.Text = lACTONOTARIAL.acno_vNumeroFojaInicial.ToString();
                    this.txtNroFojaFinal.Text = lACTONOTARIAL.acno_vNumeroFojaFinal.ToString();
                    this.HF_NroHojas.Value = lACTONOTARIAL.acno_sNumeroHojas.ToString();

                    this.txtCostoEP.Text = lACTONOTARIAL.acno_nCostoEP.ToString();
                    this.txtCostoParte2.Text = lACTONOTARIAL.acno_nCostoParte2.ToString();
                    this.txtCostoTestimonio.Text = lACTONOTARIAL.acno_nCostoTestimonio.ToString();

                    if (lACTONOTARIAL.acno_vNumeroLibro.Trim().Length == 0)
                    {
                        this.txtNroLibro.Text = "LIBR000";
                    }
                    else
                    {
                        this.txtNroLibro.Text = lACTONOTARIAL.acno_vNumeroLibro;
                    }
                    // Jose Caycho -------------------------------------------------------------------------------------
                    if (lACTONOTARIAL.acno_iNumeroActoNotarial.ToString() != "0")
                        this.txt_acno_Numero_Minuta.Text = lACTONOTARIAL.acno_iNumeroActoNotarial.ToString();

                    if (lACTONOTARIAL.acno_vNumeroOficio != null)
                        this.txtNroOficio.Text = lACTONOTARIAL.acno_vNumeroOficio.ToString();

                    if (lACTONOTARIAL.acno_vRegistradorNombre != null)
                        this.txtRegistradorNombres.Text = lACTONOTARIAL.acno_vRegistradorNombre.ToString();


                    //---------------------------------------------------
                    //fecha: 06/09/2021
                    //autor: Miguel Márquez Beltrán
                    //Motivo: Asignar la oficina registral
                    //---------------------------------------------------
                    if (lACTONOTARIAL.acno_iOficinaRegistralId != null)
                    {
                        if (lACTONOTARIAL.acno_iOficinaRegistralId > 0)
                        {
                            ddl_OficinaRegistralRegistrador.SelectedValue = lACTONOTARIAL.acno_iOficinaRegistralId.ToString();

                            //this.ddl_PaisCiudadRegistrador.Items.Clear();
                            //FillWebCombo(comun_Part3.ObtenerProvincias(Session, ddl_ContDepRegistrador.SelectedValue.ToString()), ddl_PaisCiudadRegistrador, "ubge_vProvincia", "ubge_cUbi02");

                            //ddl_PaisCiudadRegistrador.SelectedValue = lACTONOTARIAL.acno_cRegistradorUbigeo.Substring(2, 2);

                            //this.ddl_CiudadDistritoRegistrador.Items.Clear();
                            //FillWebCombo(comun_Part3.ObtenerDistritos(Session, ddl_ContDepRegistrador.SelectedValue.ToString(), ddl_PaisCiudadRegistrador.SelectedValue.ToString()), ddl_CiudadDistritoRegistrador, "ubge_vDistrito", "ubge_cUbi03");

                            //ddl_CiudadDistritoRegistrador.SelectedValue = lACTONOTARIAL.acno_cRegistradorUbigeo.Substring(4, 2);
                        }
                        else
                        {
                            //----------------------------------------------
                            //Fecha: 13/01/2022
                            //Autor: Miguel Márquez Beltrán
                            //Motivo: Asignar la oficina lima por defecto.
                            //----------------------------------------------
                            ddl_OficinaRegistralRegistrador.SelectedValue = CON_Oficina_Registral_Lima;
                            //----------------------------------------------
                        }
                    }

                    if (lACTONOTARIAL.acno_vPresentanteNombre != null)
                        this.txtRepresentanteNombres.Text = lACTONOTARIAL.acno_vPresentanteNombre.ToString();

                    if (lACTONOTARIAL.acno_sPresentanteTipoDocumento != 0)
                        this.ddl_TipoDocrepresentante.SelectedValue = lACTONOTARIAL.acno_sPresentanteTipoDocumento.ToString();

                    if (lACTONOTARIAL.acno_vPresentanteNumeroDocumento != null)
                        this.txtRepresentanteNroDoc.Text = lACTONOTARIAL.acno_vPresentanteNumeroDocumento.ToString();

                    if (lACTONOTARIAL.acno_sPresentanteGenero != 0)
                        this.ddl_GerenoPresentante.SelectedValue = lACTONOTARIAL.acno_sPresentanteGenero.ToString();

                    if (lACTONOTARIAL.acno_vNombreColegiatura != null)
                        this.txt_acno_vNombreColegiatura.Text = Convert.ToString(lACTONOTARIAL.acno_vNombreColegiatura);


                    //----------------------------------------------------------------------------------------------------------------------------------
                    //Campos para el subFlujo de Rectificaciones
                    //----------------------------------------------------------------------------------------------------------------------------------
                    this.hdn_acno_iActoNotarialReferenciaId.Value = lACTONOTARIAL.acno_iActoNotarialReferenciaId.ToString();
                    this.hdf_ActoNotarialReferencialPriId.Value = lACTONOTARIAL.acno_iActoNotarialReferenciaId.ToString();

                    ActoNotarialPrimigeniaConsultaBL ANP_BL = new ActoNotarialPrimigeniaConsultaBL();
                    DataTable dtANPrimigenia = new DataTable();

                    dtANPrimigenia = ANP_BL.Consultar(0, lACTONOTARIAL.acno_iActoNotarialId);
                    if (dtANPrimigenia.Rows.Count > 0)
                    {
                        txtAnioEscrituraPri.Text = dtANPrimigenia.Rows[0]["anpr_cAnioEscritura"].ToString();
                        txtNumeroEscrituraPublicaPri.Text = dtANPrimigenia.Rows[0]["anpr_vNumeroEscrituraPublica"].ToString();
                        ddlOficinaConsularPri.SelectedValue = dtANPrimigenia.Rows[0]["anpr_sOficinaConsularId"].ToString();

                        string strFechaExpedicionPri = dtANPrimigenia.Rows[0]["anpr_dFechaExpedicion"].ToString();

                        ctrFechaExpedicionPri.Text = (Comun.FormatearFecha(strFechaExpedicionPri).ToString(ConfigurationManager.AppSettings["FormatoFechas"]));

                        txtTipoActoNotarialPri.Text = dtANPrimigenia.Rows[0]["anpr_vTipoActoNotarial"].ToString();
                        txtNotariaPri.Text = dtANPrimigenia.Rows[0]["anpr_vNotaria"].ToString();
                        hdf_ActoNotarialPrimigeniaId.Value = dtANPrimigenia.Rows[0]["anpr_iActoNotarialPrimigeniaId"].ToString();
                        ctrFechaExpedicionPri.Enabled = false;
                        txtTipoActoNotarialPri.Enabled = false;
                        hf_ValidaTablaPrimigenia.Value = "S";

                        //-------------------------------------------------------
                        //Fecha: 02/12/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Visualizar la tabla primigenia si se 
                        //          selecciona Revocación, Modificación
                        //          Ampliación o Rectificación.                
                        //-------------------------------------------------------
                        if (strTipoActoProtocolar.IndexOf("REVOCACIÓN") > -1 ||
                            strTipoActoProtocolar.IndexOf("MODIFICACIÓN") > -1 ||
                            strTipoActoProtocolar.IndexOf("AMPLIACIÓN") > -1 ||
                            strTipoActoProtocolar.IndexOf("RECTIFICACIÓN") > -1)
                        {                             
                            if (strTipoActoProtocolar.IndexOf("GENERAL") > -1)
                            {
                                hf_PG_PE.Value = "G";
                            }
                            else
                            {
                                hf_PG_PE.Value = "E";
                            }
                        }
                        strScript = string.Empty;
                        strScript = @"$(function(){{
                                        SoloHabilitarTablaPrimigenia();
                                        }});";
                        strScript = string.Format(strScript);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitarTablaPrimigenia", strScript, true);

                    }
                        //RE_ACTONOTARIAL lACTONOTARIALREC = new RE_ACTONOTARIAL();
                        //lACTONOTARIALREC.acno_iActoNotarialReferenciaId = Convert.ToInt64(hdn_acno_iActoNotarialReferenciaId.Value);
                        //lACTONOTARIALREC = lActoNotarialConsultaBL.obtener(lACTONOTARIALREC);

                        //this.Txt_EscrituraAnterior.Text = lACTONOTARIALREC.acno_vNumeroEscrituraPublica.ToString(); //Tiene que jalar el número de escritura anterior con respecto al número de referencia
                        //this.Txt_EscrituraAnterior.Enabled = false;                        
                        //this.txtNroEscritura.Text = "MRE000";

                        //Hacer visible los controles involucrados en este subflujo
                   

                    this.Cmb_TipoActoNotarial.Enabled = false;

                    hdn_acno_dFechaExtension.Value = lACTONOTARIAL.acno_dFechaExtension.ToString();

                    //----------------------------------------------------------------------------------------------------------------------------------
                    //Fecha: 25/06/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Obtener fojas disponibles.
                    //----------------------------------------------------------------------------------------------------------------------------------
                    
                    //-----------------------------------------
                    //Fecha: 05/08/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Obtener el año de la fecha.
                    //-----------------------------------------
                    //int intAnioEscrituraPublica = Convert.ToInt32(hdn_acno_dFechaExtension.Value.Substring(6,4));

                    int intAnioEscrituraPublica = DateTime.Parse(hdn_acno_dFechaExtension.Value).Year;

                    DataTable dtLibros = new DataTable();
                    LibroConsultasBL objLibroBL = new LibroConsultasBL();
                    int intTotalRegistros = 0;
                    int intTotalPaginas = 0;
                    dtLibros = objLibroBL.obtener(Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString()), intAnioEscrituraPublica,
                        1, 10, ref intTotalRegistros, ref intTotalPaginas);
                    DataTable dtFojasLibro = null;
                    if (dtLibros == null)
                    {
                        lblFoliosDisponibles.Text = "0";
                        throw new Exception("No existe libro de Escritura Pública.");
                    }
                    if (dtLibros.Rows.Count > 0)
                    {

                        dtFojasLibro = (from dtLib in dtLibros.AsEnumerable()
                                        where Comun.ToNullInt32(dtLib["libr_sTipoLibroId"]) == ((int)Enumerador.enmNotarialLibroTipo.ESCRITURA_PUBLICA)
                                        && dtLib["libr_vEstadoLibro"].ToString().Equals("ABIERTO")
                                        select dtLib).CopyToDataTable();
                    }
                    else
                    {
                        lblFoliosDisponibles.Text = "0";
                        throw new Exception("No existe libro de Escritura Pública.");
                    }
                    if (dtFojasLibro == null)
                    {
                        lblFoliosDisponibles.Text = "0";
                        throw new Exception("No existen fojas en el libro de Escritura Pública.");
                    }
                    if (dtFojasLibro.Rows.Count > 0)
                    {
                        Int32 intNumeroFojaTotal = 0;
                        Int32 intNumeroFojaActual = 0;
                        Int32 intNumeroFojaRestante = 0;

                        intNumeroFojaTotal = Comun.ToNullInt32(dtFojasLibro.Rows[0]["libr_iNumeroFolioTotal"].ToString());
                        intNumeroFojaActual = Comun.ToNullInt32(dtFojasLibro.Rows[0]["libr_iNumeroFolioActual"].ToString());

                        intNumeroFojaRestante = intNumeroFojaTotal - intNumeroFojaActual + 1;

                        lblFoliosDisponibles.Text = intNumeroFojaRestante.ToString();
                    }
                    else
                        {
                            lblFoliosDisponibles.Text = "0";
                    }

                    //----------------------------------------------------------------------------------------------------------------------------------
                    #endregion

                    #region Participantes
                    //-----------------------------------------------------------------------------------------------------
                    // TAB PARTICIPANTE(s)
                    //-----------------------------------------------------------------------------------------------------
                    RE_ACTONOTARIALPARTICIPANTE lACTONOTARIALPARTICIPANTE = new RE_ACTONOTARIALPARTICIPANTE();
                    lACTONOTARIALPARTICIPANTE.anpa_iActoNotarialId = lACTONOTARIAL.acno_iActoNotarialId;

                    ParametroConsultasBL lParametroConsultasBL = new ParametroConsultasBL();
                    TablaMaestraConsultaBL lTablaMaestraConsultaBL = new TablaMaestraConsultaBL();
                    SI_PARAMETRO lPARAMETRO = new SI_PARAMETRO();

                    ParticipanteConsultaBL lParticipanteConsultaBL = new ParticipanteConsultaBL();
                    List<RE_ACTONOTARIALPARTICIPANTE> lParticipantes = lParticipanteConsultaBL.Listar_ActoNotarial(lACTONOTARIALPARTICIPANTE);

                    List<CBE_PARTICIPANTE> loPARTICIPANTES = new List<CBE_PARTICIPANTE>();

                    foreach (RE_ACTONOTARIALPARTICIPANTE item in lParticipantes)
                    {
                        CBE_PARTICIPANTE lParticipante = new CBE_PARTICIPANTE();
                        SI_PARAMETRO lParametro = new SI_PARAMETRO();

                        #region Participante

                        lParticipante.anpa_iActoNotarialParticipanteId = item.anpa_iActoNotarialParticipanteId;
                        lParticipante.anpa_iActoNotarialId = item.anpa_iActoNotarialId;
                        lParticipante.anpa_iPersonaId = item.anpa_iPersonaId;
                        lParticipante.anpa_iEmpresaId = item.anpa_iEmpresaId;
                        lParticipante.anpa_sTipoParticipanteId = item.anpa_sTipoParticipanteId;
                        lParticipante.anpa_bFlagFirma = item.anpa_bFlagFirma;
                        lParticipante.anpa_bFlagHuella = item.anpa_bFlagHuella;
                        lParticipante.anpa_cEstado = item.anpa_cEstado;
                        lParticipante.anpa_sUsuarioCreacion = item.anpa_sUsuarioCreacion;
                        lParticipante.anpa_vIPCreacion = item.anpa_vIPCreacion;
                        lParticipante.anpa_dFechaCreacion = item.anpa_dFechaCreacion;
                        lParticipante.anpa_sUsuarioModificacion = item.anpa_sUsuarioModificacion;
                        lParticipante.anpa_vIPModificacion = item.anpa_vIPModificacion;
                        lParticipante.anpa_dFechaModificacion = item.anpa_dFechaModificacion;
                        lParticipante.anpa_iActoNotarialParticipanteIdAux = item.anpa_iActoNotarialParticipanteId;
                        lParticipante.anpa_iReferenciaParticipanteId = item.anpa_iReferenciaParticipanteId;

                        //-------------------------------------------------------------------
                        //Fecha: 20/09/2017
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: Asignar la columna Fecha de suscripción
                        //-------------------------------------------------------------------
                        if (item.anpa_dFechaSuscripcion == null || item.anpa_dFechaSuscripcion == DateTime.MinValue)
                        {
                            if (hdn_acno_dFechaExtension.Value.Length > 0)
                            {
                                lParticipante.anpa_dFechaSuscripcion = Comun.FormatearFecha(hdn_acno_dFechaExtension.Value);
                            }
                        }
                        else
                        {
                            lParticipante.anpa_dFechaSuscripcion = item.anpa_dFechaSuscripcion;
                        }

                        #endregion

                        RE_PERSONA lPERSONA = new RE_PERSONA();
                        lPERSONA.pers_iPersonaId = item.anpa_iPersonaId;

                        PersonaConsultaBL lPersonaConsultaBL = new PersonaConsultaBL();
                        PersonaIdentificacionConsultaBL lPersonaIdentificacionConsultaBL = new PersonaIdentificacionConsultaBL();
                        lParticipante.Persona = lPersonaConsultaBL.Obtener(lPERSONA);
                        lParticipante.Identificacion = lPersonaIdentificacionConsultaBL.Obtener(lParticipante.Persona);
                        lParticipante.Persona.Identificacion = lParticipante.Identificacion;
                        lParticipante.Persona.vNacionalidad = lParticipante.Persona.vNacionalidad;

                        lPARAMETRO = new SI_PARAMETRO();
                        lPARAMETRO.para_sParametroId = item.anpa_sTipoParticipanteId;
                        lParticipante.acpa_sTipoParticipanteId_desc = lParametroConsultasBL.Obtener(lPARAMETRO).para_vDescripcion;

                        BE.MRE.SC_MAESTRO.MA_DOCUMENTO_IDENTIDAD lDOCUMENTO_IDENTIDAD = new BE.MRE.SC_MAESTRO.MA_DOCUMENTO_IDENTIDAD();
                        lDOCUMENTO_IDENTIDAD.doid_sTipoDocumentoIdentidadId = lParticipante.Identificacion.peid_sDocumentoTipoId;
                        lParticipante.peid_sDocumentoTipoId_desc = lTablaMaestraConsultaBL.DOCUMENTO_IDENTIDAD_OBTENER(lDOCUMENTO_IDENTIDAD).doid_vDescripcionCorta;

                        lPARAMETRO = new SI_PARAMETRO();
                        lPARAMETRO.para_sParametroId = lPERSONA.pers_sNacionalidadId;
                        lParticipante.pers_sNacionalidadId_desc = lParametroConsultasBL.Obtener(lPARAMETRO).para_vDescripcion;


                        lPARAMETRO = new SI_PARAMETRO();
                        lPARAMETRO.para_sParametroId = lPERSONA.pers_sIdiomaNatalId;
                        lParticipante.pers_sIdiomaNatalId_desc = lParametroConsultasBL.Obtener(lPARAMETRO).para_vDescripcion;
                        loPARTICIPANTES.Add(lParticipante);
                        //-------------------------------------------------
                        //Fecha: 17/04/2020
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: Reemplazar la session por ViewState
                        //-------------------------------------------------

                        //Session["ModoEdicionProtocolar"] = true;
                        ViewState["ModoEdicionProtocolar"] = true;

                    }
                    Session["ParticipanteContainer"] = (List<CBE_PARTICIPANTE>)loPARTICIPANTES;

                    //------participantes
                    DataTable dtParticipantes = new DataTable();

                    dtParticipantes = CrearTablaParticipante(null);

                    this.grd_Participantes.DataSource = dtParticipantes;
                    this.grd_Participantes.DataBind();

                    this.grd_Otorgantes.DataSource = ObtenerOtorgantesOrdenados(dtParticipantes);
                    this.grd_Otorgantes.DataBind();
                    updGrillaOtorgantes.Update();

                    //-----presentantes
                    CBE_PRESENTANTE lp= new CBE_PRESENTANTE();
                    lp.anpr_iActoNotarialId = lACTONOTARIALPARTICIPANTE.anpa_iActoNotarialId;
                    List<CBE_PRESENTANTE> lPresentante = lParticipanteConsultaBL.listaPresentante(lp);
                    Session["PresentanteContainer"] = lPresentante;
                    this.GridViewPresentante.DataSource = lPresentante;
                    this.GridViewPresentante.DataBind();
                    UpdateGridPresentantes.Update();


                    //-----------------------------------------------------------------------------------------------------
                    #endregion

                    #region Cuerpo
                    RE_ACTONOTARIALCUERPO lACTONOTARIALCUERPO = new RE_ACTONOTARIALCUERPO();
                    ActoNotarialCuerpoConsultaBL lActoNotarialCuerpoConsultaBL = new ActoNotarialCuerpoConsultaBL();
                    lACTONOTARIALCUERPO.ancu_iActoNotarialId = lACTONOTARIAL.acno_iActoNotarialId;
                    lACTONOTARIALCUERPO = lActoNotarialCuerpoConsultaBL.obtener(lACTONOTARIALCUERPO);
                    if (lACTONOTARIALCUERPO.ancu_iActoNotarialCuerpoId != 0)
                    {
                        //lACTONOTARIALCUERPO.ancu_vCuerpo = lACTONOTARIALCUERPO.ancu_vCuerpo.Replace("#IMAGEN#", "");
                        string[] arrCuerpo = lACTONOTARIALCUERPO.ancu_vCuerpo.Split(new string[] { "<tagx></tagx>" }, StringSplitOptions.None);

                        // 0. Introducción
                        lblIntro.Text = arrCuerpo[0].ToString();
                        // 1. Texto Central
                        //-----------------------------------------------
                        //Fecha: 13/08/2020
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Validar el arreglo arrCuerpo
                        //          cuando tenga menos de un valor.
                        //-----------------------------------------------
                        //RichTextBox.Text = arrCuerpo[1].ToString();
                        if (arrCuerpo.Length > 1)
                        {
                            RichTextBox.Text = arrCuerpo[1].ToString();
                        }
                        else
                        {
                            RichTextBox.Text = "";
                        }
                        //-----------------------------------------------
                        // 2. Texto Normativo
                        //CargarListaTextoNormativo(lACTONOTARIALCUERPO.ancu_vTextoNormativo);
                        hf_idsNormativos.Value = lACTONOTARIALCUERPO.ancu_vTextoNormativo;
                        string StrScript = string.Empty;
                        StrScript = @"setearTextoNormativo();";
                        StrScript = string.Format(StrScript);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "cargLstTxtNormtivo", StrScript, true);


                        // Versión Antigua (Antes 18/09/2015)
                        if (arrCuerpo.Length == 7)
                        {
                            // 5. Conclusión
                            lblConcluciones.Text = arrCuerpo[5].ToString();
                            // 6. Texto Adicional
                            RichTextBoxAdicional.Text = arrCuerpo[6].ToString();
                        }
                        else
                        {
                            //0.strTextoIntroduccion + "<tagx></tagx>"
                            //1.strTextoCentral + "<tagx></tagx>" 
                            //2.strTextoPosterior + strFechaPosterior + "<tagx></tagx>" 
                            //3.strTextoMinuta + "<tagx></tagx>"
                            //4.vImagenes + "<tagx></tagx>" 
                            //5.strTextoConclusion + "<tagx></tagx>" 
                            //6.strTextNormativoFormateado + "<tagx></tagx>"
                            //7.strTextoAdicional + "<tagx></tagx>" 
                            //8.strTextoFinal;
                            //---------------------------------------------
                            // 0. Introducción + Texto Central Inicial
                            // 1. Texto Central
                            // 2. Texto Central Final
                            lblCierreTextoCentral.Text = arrCuerpo[2].ToString();
                            // 3. Texto Minuta                            
                            // 4. Texto Normativo
                            // 5. Imágenes (NEW)
                            // 6. Conclusión     
                            lblConcluciones.Text = arrCuerpo[5].ToString();
                            // 7. Final
                            //lblFinal.Text = arrCuerpo[7].ToString();
                            RichTextBoxAdicional.Text = arrCuerpo[7].ToString();
                            lblFinal.Text = arrCuerpo[8].ToString();
                            // 8. Texto Adicional
                            //RichTextBoxAdicional.Text = arrCuerpo[8].ToString();
                            
                        }

                        HF_CIUDAD_FECHA.Value = ObtenerCiudad_Fecha(this.hdn_acno_iActoNotarialId.Value);
                        this.txt_ancu_vFirmaIlegible.Text = lACTONOTARIALCUERPO.ancu_vFirmaIlegible.ToString();
                        this.txt_acno_sAutorizacionTipoId.Text = lACTONOTARIAL.acno_vAutorizacionTipo.ToString();
                        this.txt_acno_vNumeroColegiatura.Text = lACTONOTARIAL.acno_vNumeroColegiatura.ToString();
                        this.txtAutorizacionNroDocumento.Text = lACTONOTARIAL.acno_vAutorizacionDocumentoNumero.ToString();

                        hdn_ancu_iActoNotarialCuerpoId.Value = lACTONOTARIALCUERPO.ancu_iActoNotarialCuerpoId.ToString();

                        RichTextBoxDL1049Art55IncisoC.Text = lACTONOTARIALCUERPO.ancu_vDL1049Articulo55C.ToString();
                    }

                    int intEstadoId = Consultar_Estado_Cs(Convert.ToInt64(hdn_acno_iActoNotarialId.Value), Convert.ToInt64(hdn_acno_iActuacionId.Value));

                    if (intEstadoId == (int)Enumerador.enmNotarialProtocolarEstado.TRANSCRITA)
                    {
                        Btn_VistaPreviaAprobar.Enabled = true;
                    }

                    if (intEstadoId == (int)Enumerador.enmNotarialProtocolarEstado.APROBADA)
                    {
                        txtTextoNormativo.Enabled = false;
                        
                        txt_acno_Numero_Minuta.Enabled = false;
                        txt_acno_vNombreColegiatura.Enabled = false;

                        txt_acno_sAutorizacionTipoId.Enabled = false;
                        txtAutorizacionNroDocumento.Enabled = false;
                        txt_acno_vNumeroColegiatura.Enabled = false;
                        txt_ancu_vFirmaIlegible.Enabled = false;
                        //--vpipa check
                        string StrScript = string.Empty;
                        //StrScript = @"$('#MainContent_divListNormativo').addClass('disableElementsOfDiv');$('#MainContent_Btn_AfirmarTextoLeido').prop('disabled', true);$('#MainContent_cbxAfirmarTexto').prop('disabled', true);$('#MainContent_cbxAfirmarTexto').prop('checked', true);";
                        StrScript = @"$('#MainContent_Btn_AfirmarTextoLeido').prop('disabled', true);$('#MainContent_cbxAfirmarTexto').prop('disabled', true);$('#MainContent_cbxAfirmarTexto').prop('checked', true);";
                        StrScript = string.Format(StrScript);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitarChec", StrScript, true);
                        //cbxAfirmarTexto.Checked = true;
                        //cbxAfirmarTexto.Enabled = false; 



                        updDatosAbogado.Update();
                    }

                    #region Imágenes

                    RE_ACTONOTARIALDOCUMENTO imagen = new RE_ACTONOTARIALDOCUMENTO();
                    imagen.ando_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
                    imagen.ando_sTipoDocumentoId = (Int16)Enumerador.enmTipoAdjunto.FOTO;
                    var s_Lista_Imagenes = new SGAC.Registro.Actuacion.BL.ActoNotarialMantenimiento().ListaActoNotarialDocumento(imagen);

                    Session["ImagenesContainer"] = s_Lista_Imagenes.ToList();

                    this.gdvImagenes.DataSource = CrearTablaImagenes(null);
                    this.gdvImagenes.DataBind();

                    updImagenes.Update();

                    #endregion

                    updTxtNormativo.Update();

                    #endregion

                    #region Acto_Notarial_Normas
                    ActoNotarial_NormasConsultaBL objActoNotarialNormasConsultaBL = new ActoNotarial_NormasConsultaBL();
                    DataTable dtActoNotarialNormas = new DataTable();
                    RE_ACTONOTARIAL_NORMA actoNotarial_normaBE = new RE_ACTONOTARIAL_NORMA();
                    actoNotarial_normaBE.anra_iActoNotarialId = lACTONOTARIAL.acno_iActoNotarialId;
                    dtActoNotarialNormas = objActoNotarialNormasConsultaBL.ObtenerNormas(actoNotarial_normaBE);
                    gdv_Normas.DataSource = dtActoNotarialNormas;
                    gdv_Normas.DataBind();
                    if (dtActoNotarialNormas.Rows.Count == 0)
                    {
                        gdv_Normas.Visible = false;
                    }
                    else
                    {
                        gdv_Normas.Visible = true;
                    }
                    hf_idsNormativos.Value = ObtenerIdNormas(dtActoNotarialNormas);
                    updTxtNormativo.Update();

                    string StrScript2 = string.Empty;
                    StrScript2 = @"setearTextoNormativo();";
                    StrScript2 = string.Format(StrScript2);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "cargLstTxtNormtivo", StrScript2, true);
                    
                    #endregion

                    #region Pagos
                    ActoNotarialConsultaBL objActoNotarialBL = new ActoNotarialConsultaBL();
                    DataTable dtActuacionDetalle = new DataTable();
                    Int64 lngActuacionId = lACTONOTARIAL.acno_iActuacionId;
                    dtActuacionDetalle = objActoNotarialBL.ListarActuacionDetalle(lngActuacionId);

                    if (dtActuacionDetalle != null)
                    {
                        if (dtActuacionDetalle.Rows.Count > 0)
                        {
                            Gdv_Tarifa.DataSource = dtActuacionDetalle;
                            Gdv_Tarifa.DataBind();

                            // Datos Pago
                            double dblTotalSolesConsulares = 0;
                            double dblTotalMonedaLocal = 0;
                            foreach (DataRow dr in dtActuacionDetalle.Rows)
                            {
                                dblTotalMonedaLocal += Convert.ToDouble(dr["pago_FMontoMonedaLocal"]);
                                dblTotalSolesConsulares += Convert.ToDouble(dr["pago_FMontoSolesConsulares"]);
                            }

                            Session[Constantes.CONST_SESION_ACTUACIONDET_ID] = dtActuacionDetalle.Rows[0]["acde_iActuacionDetalleId"].ToString();

                            Lbl_TotalGeneral.Text = dblTotalSolesConsulares.ToString(ConfigurationManager.AppSettings["FormatoMonto"].ToString());
                            Lbl_TotalExtranjera.Text = dblTotalMonedaLocal.ToString(ConfigurationManager.AppSettings["FormatoMonto"].ToString());
                            txtMtoCancelado.Text = dblTotalSolesConsulares.ToString();
                            ddlTipoPago.SelectedValue = dtActuacionDetalle.Rows[0]["pago_sPagoTipoId"].ToString();
                            HF_TIPO_PAGO_ID.Value = dtActuacionDetalle.Rows[0]["pago_sPagoTipoId"].ToString();
                            Txt_VoucherNro.Text = dtActuacionDetalle.Rows[0]["pago_vNumeroVoucher"].ToString();

                            bool bActivarEdicionLey = false;

                            if (ddlTipoPago.SelectedItem.Text == "GRATUITO POR LEY" ||
                                ddlTipoPago.SelectedItem.Text == "INAFECTO POR INDIGENCIA")
                            {
                                bActivarEdicionLey = true;
                            }


                            if (dtActuacionDetalle.Rows[0]["pago_iNormaTarifarioId"].ToString().Trim() != "0" || bActivarEdicionLey)
                            {
                                LlenarListaExoneracion();
                                lblExoneracion.Visible = true;

                                ddlExoneracion.SelectedValue = dtActuacionDetalle.Rows[0]["pago_iNormaTarifarioId"].ToString().Trim();

                                if (dtActuacionDetalle.Rows[0]["pago_iNormaTarifarioId"].ToString().Trim() != "0")
                                {
                                    txtSustentoTipoPago.Enabled = false;
                                    RBNormativa.Checked = true;
                                    RBNormativa.Enabled = true;
                                    ddlExoneracion.Enabled = true;
                                }
                                else
                                {
                                    RBSustentoTipoPago.Checked = true;
                                    RBSustentoTipoPago.Enabled = true;
                                    txtSustentoTipoPago.Enabled = true;
                                    ddlExoneracion.Enabled = false;
                                }
                            }
                            else
                            {
                                ddlExoneracion.Items.Clear();
                                lblExoneracion.Visible = false;
                                ddlExoneracion.Visible = false;
                                ddlExoneracion.SelectedIndex = -1;
                                RBSustentoTipoPago.Checked = true;
                                RBSustentoTipoPago.Enabled = true;
                                RBNormativa.Checked = false;
                            }
                            txtSustentoTipoPago.Text = dtActuacionDetalle.Rows[0]["pago_vSustentoTipoPago"].ToString().Trim();
                            MostrarDL173_DS076_2005RE();

                            ddlTipoPago.Enabled = false;
                            Txt_VoucherNro.Enabled = false;
                            Gdv_Tarifa.Enabled = false;

                            Btn_AgregarTarifa.Enabled = false;
                            btnLimpiarTarifa.Enabled = false;
                            btnPresentanteAgregar.Enabled = false;
                            GridViewPresentante.Enabled = false;
                            btnAgregarNorma.Enabled = false;
                            gdv_Normas.Enabled = false;
                        }
                        else
                        {
                            Habilitar_Tab(lACTONOTARIAL.acno_sEstadoId);
                        }
                    }
                    else
                    {
                        Habilitar_Tab(lACTONOTARIAL.acno_sEstadoId);
                    }

                    // Deshabilitar
                    Txt_TarifaId.Enabled = true;
                    Txt_TarifaDescripcion.Enabled = false;

                    //----------------------------------------------------------------------------------------------------------------------------------
                    // Setear el tarifario con las tarifas correspondientes al tipo de acto seleccionado en el Tab Pagos
                    //----------------------------------------------------------------------------------------------------------------------------------
                    DataTable dtTarifario;
                    Session.Remove("dtTarifarioFiltrado");
                    //dtTarifario = cargar_tarifas(lACTONOTARIAL.acno_sSubTipoActoNotarialId.ToString());
                    //-----------------------------------------------
                    //Fecha: 06/12/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Consultar las tarifas de la sección 2.
                    //-----------------------------------------------

                    dtTarifario = cargar_tarifas();
                    HF_ACTONOTARIAL_POR_TARIFA.Value = lACTONOTARIAL.acno_sSubTipoActoNotarialId.ToString();
                    Session.Add("dtTarifarioFiltrado", dtTarifario);

                    this.Lst_Tarifario.DataSource = dtTarifario;
                    this.Lst_Tarifario.DataTextField = "tari_vDescripcionCorta";
                    this.Lst_Tarifario.DataValueField = "tari_sTarifarioId";
                    this.Lst_Tarifario.DataBind();
                    //--------------------------------------------------------------------------------------------------------------------
                    #endregion

                    #region Vinculacion
                    //--------------------------------------------------------------------------------------------------------------------
                    // TAB VINCULACION
                    //--------------------------------------------------------------------------------------------------------------------                
                    //  BindGridActuacionesInsumoDetalle(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]));
                    BindGridActuacionesInsumoDetalle(Convert.ToInt64(hdn_acno_iActuacionId.Value));
                    if (intEstadoId == (int)Enumerador.enmNotarialProtocolarEstado.VINCULADA)
                    {
                        txt_acno_Numero_Minuta.Enabled = false;
                        txt_acno_vNombreColegiatura.Enabled = false;

                        txt_acno_sAutorizacionTipoId.Enabled = false;
                        txtAutorizacionNroDocumento.Enabled = false;
                        txt_acno_vNumeroColegiatura.Enabled = false;
                        txt_ancu_vFirmaIlegible.Enabled = false;

                        updDatosAbogado.Update();
                    }

                    #region Validar Vinculación Completa
                    bool bolFalta = true;
                    string strCodigo = "";
                    String strImprimir = String.Empty;
                    foreach (GridViewRow gdr in gdvVinculacion.Rows)
                    {
                        strCodigo = Convert.ToString(Page.Server.HtmlDecode(gdr.Cells[10].Text)).Trim();
                        strImprimir = Convert.ToString(Page.Server.HtmlDecode(gdr.Cells[9].Text)).Trim();
                        if (strCodigo != string.Empty)
                        {
                            if (strImprimir.Equals("SI"))
                            { bolFalta = false; }
                            else
                            {
                                bolFalta = true;
                                break;
                            }
                        }
                        else
                        {
                            bolFalta = true;
                            break;
                        }
                    }
                    if (!bolFalta)
                    {
                        #region Cambiar Estado : VINCULADA
                        //RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();
                        //ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();

                        //lACTONOTARIAL.acno_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
                        //lACTONOTARIAL = lActoNotarialConsultaBL.obtener(lACTONOTARIAL);

                        if (lACTONOTARIAL.acno_sEstadoId == (Int16)Enumerador.enmNotarialProtocolarEstado.PAGADA)
                        {
                            ActoNotarialMantenimiento bl = new ActoNotarialMantenimiento();
                            RE_ACTONOTARIAL actonotarialBE = new RE_ACTONOTARIAL();
                            actonotarialBE.acno_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
                            actonotarialBE.acno_sEstadoId = (int)Enumerador.enmNotarialProtocolarEstado.VINCULADA;
                            actonotarialBE.acno_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                            actonotarialBE.acno_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                            actonotarialBE.acno_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();

                            if (bl.ActoNotarialActualizarEstado(actonotarialBE) == false)
                            {
                                LimpiarDatosVinculacion();
                                btnGrabarVinculacion.Enabled = false;
                                btnLimpiarVinc.Enabled = false;//--vpipa

                                Habilitar_Tab((int)Enumerador.enmNotarialProtocolarEstado.VINCULADA);

                                if (Session["tab_activa_vinculacion"].ToString() == "SI")
                                {
                                    strScript = @"$(function(){{ HabilitarTabVinculacion(); }});";
                                    
                                    strScript = string.Format(strScript);
                                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitarTabVinculacion", strScript, true);
                                    Session["tab_activa_vinculacion"] = "NO";
                                }
                                else
                                {
                                    strScript = @"$(function(){{
                                        HabilitarTabDigitalizac();
                                    }});";
                                    strScript = string.Format(strScript);
                                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitarTabDigitalizac", strScript, true);
                                }
                                
                            }
                        }
                        updFormato.Update();
                        #endregion
                    }
                    #endregion

                    //--------------------------------------------------------------------------------------------------------------------
                    #endregion

                    #region Digitalización
                    RE_ACTONOTARIALDOCUMENTO documento = new RE_ACTONOTARIALDOCUMENTO();
                    documento.ando_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
                    documento.ando_sTipoDocumentoId = (int)Enumerador.enmTipoAdjunto.DOCUMENTO_DIGITALIZA;
                    var s_lista_Documentos = new SGAC.Registro.Actuacion.BL.ActoNotarialMantenimiento().ListaActoNotarialDocumento(documento);

                    /*Documentos digitalizados*/
                    Session["DocumentoDigitalizadoContainer"] = s_lista_Documentos.ToList();

                    this.Gdv_Adjunto.DataSource = CrearTablaDigitalizacionArchivo(null);
                    this.Gdv_Adjunto.DataBind();

                    if (intEstadoId == (int)Enumerador.enmNotarialProtocolarEstado.DIGITALIZADA)
                    {
                        //btnSave_tab6.Enabled = false;
                        //btnCancelarTab6.Enabled = false;
                        //Txt_AdjuntoDescripcion.Enabled = false;

                        Label lblNombreArchivo = (Label)ctrlUploader2.FindControl("lblNombreArchivo");
                        lblNombreArchivo.Text = "";
                        lblNombreArchivo.Visible = false;

                        Button BtnUpload = (Button)ctrlUploader2.FindControl("btnUpload");
                        //BtnUpload.Enabled = false;

                        FileUpload FUupload = (FileUpload)ctrlUploader2.FindControl("FileUploader");
                        //FUupload.Enabled = false;

                        System.Web.UI.HtmlControls.HtmlTableCell msjeSucess = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader2.FindControl("msjeSucess");
                        msjeSucess.Visible = false;

                        System.Web.UI.HtmlControls.HtmlTableCell msjeWarning = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader2.FindControl("msjeWarning");
                        msjeWarning.Visible = false;

                        System.Web.UI.HtmlControls.HtmlTableCell msjeError = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader2.FindControl("msjeError");
                        msjeError.Visible = false;

                        //btnVisualizarDigitalizacion.Enabled = false;

                        //btnAgregarArchivo.Enabled = false;

                        txt_acno_Numero_Minuta.Enabled = false;
                        txt_acno_vNombreColegiatura.Enabled = false;

                        txt_acno_sAutorizacionTipoId.Enabled = false;
                        txtAutorizacionNroDocumento.Enabled = false;
                        txt_acno_vNumeroColegiatura.Enabled = false;
                        txt_ancu_vFirmaIlegible.Enabled = false;

                        updDatosAbogado.Update();

                        Gdv_Adjunto.Columns[4].Visible = true;
                        Gdv_Adjunto.Columns[5].Visible = false;
                        //Gdv_Adjunto.Columns[6].Visible = false;

                        updComandosTab6.Update();
                    }
                    //--------------------------------------------------------------------------------------------------------------------
                    #endregion

                    if (Session["tab_activa_vinculacion"].ToString() == "NO")
                    {
                        Habilitar_Tab(lACTONOTARIAL.acno_sEstadoId);
                    }

                    #region Según Acción: Consulta, Edición, Solicitud Parte/Testimonio
                    int intAccion = Convert.ToInt32(hdn_AccionOperacion.Value);

                    switch (intAccion)
                    {
                        case (int)Enumerador.enmNotarialAccion.CONSULTA:
                            txt_acno_Numero_Minuta.Enabled = false;
                            txt_acno_vNombreColegiatura.Enabled = false;

                            txt_acno_sAutorizacionTipoId.Enabled = false;
                            txtAutorizacionNroDocumento.Enabled = false;
                            txt_acno_vNumeroColegiatura.Enabled = false;
                            txt_ancu_vFirmaIlegible.Enabled = false;

                            updDatosAbogado.Update();

                            Gdv_Adjunto.Columns[4].Visible = true;
                            Gdv_Adjunto.Columns[5].Visible = false;
                            Gdv_Adjunto.Columns[6].Visible = false;

                            updComandosTab6.Update();

                            strScript = string.Empty;
                            strScript = @"$(function(){{
                                        DeshabilitaElementosTabs();
                                        }});";
                            strScript = string.Format(strScript);
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "DeshabilitaElementosTabs", strScript, true);

                            strScript = string.Empty;
                            strScript = @"$(function(){{
                                        DeshabilitaElementosTabDigitalizacion();
                                        }});";
                            strScript = string.Format(strScript);
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "DeshabilitaElementosTabDigitalizacion", strScript, true);

                            break;
                        case (int)Enumerador.enmNotarialAccion.EDICION:

                            break;
                        case (int)Enumerador.enmNotarialAccion.SOLICITUD:
                            pnlNuevoFormato.Visible = true;

                            Session.Remove("dtTarifarioFiltrado");


                            //dtTarifario = cargar_tarifas("0", ddlAccionR.SelectedValue);
                            //-----------------------------------------------
                            //Fecha: 06/12/2021
                            //Autor: Miguel Márquez Beltrán
                            //Motivo: Consultar las tarifas de la sección 2.
                            //-----------------------------------------------

                            dtTarifario = cargar_tarifas();

                            Session.Add("dtTarifarioFiltrado", dtTarifario);

                            this.Lst_Tarifario.DataSource = dtTarifario;
                            this.Lst_Tarifario.DataTextField = "tari_vDescripcionCorta";
                            this.Lst_Tarifario.DataValueField = "tari_sTarifarioId";
                            this.Lst_Tarifario.DataBind();

                            #region Verificar Primer Parte
                            ActoNotarialConsultaBL objBL = new ActoNotarialConsultaBL();
                            Int64 intActuacionId = Convert.ToInt64(hdn_acno_iActuacionId.Value);
                            DataTable dtPartes = objBL.ObtenerActoNotarialDetalle(intActuacionId, (Int16)Enumerador.enmNotarialTipoFormato.PARTE);
                            if (dtPartes.Rows.Count > 0)
                            {
                                this.Lst_Tarifario.Items[1].Enabled = false;
                            }
                            #endregion

                            #region Obtener Cantidad de folios por formato
                            int intCantFoliosParte = 0;
                            int intCantFoliosTestimonio = 0;
                            DataTable dtTestimonios = objBL.ObtenerActoNotarialDetalle(intActuacionId, (Int16)Enumerador.enmNotarialTipoFormato.TESTIMONIO);
                            if (dtTestimonios.Rows.Count > 0)
                            {
                                intCantFoliosTestimonio = Convert.ToInt32(dtTestimonios.Rows[0]["ande_sNumeroFoja"]);
                                hdn_cant_fojas_testimonio.Value = intCantFoliosTestimonio.ToString();
                            }
                            if (dtPartes.Rows.Count > 1)
                            {
                                intCantFoliosParte = Convert.ToInt32(dtPartes.Rows[1]["ande_sNumeroFoja"]);
                                hdn_cant_fojas_parte.Value = intCantFoliosParte.ToString();
                            }
                            #endregion

                            Txt_VoucherNro.Text = "";
                            ddlTipoPago.SelectedIndex = 0;
                            Gdv_Tarifa.DataSource = null;
                            Gdv_Tarifa.DataBind();
                            Lbl_TotalGeneral.Text = "0.00";
                            Lbl_TotalExtranjera.Text = "0.00";

                            this.Gdv_Tarifa.Enabled = true;
                            this.Btn_AgregarTarifa.Enabled = true;

                            updRegPago.Update();
                            break;
                        default:
                            break;
                    }

                    #endregion
                }
                else
                {
//                    Habilitar_Tab(0);
//                    int intAccion = Convert.ToInt32(hdn_AccionOperacion.Value);
//                    if (intAccion == (int)Enumerador.enmNotarialAccion.RECTIFICACION)
//                    {

//                        this.hdn_Rectificacion.Value = Session["iFlujoProtocolar"].ToString();

//                        this.hdn_acno_iActoNotarialReferenciaId.Value = Session["iActoNotarialReferenciaId"].ToString();

//                        this.Cmb_TipoActoNotarial.SelectedValue = Session["intActoNotarialSubTipoId"].ToString();
//                        this.Cmb_TipoActoNotarial.Enabled = false;

//                        this.Txt_EscrituraAnterior.Text = Session["NroEscritura"].ToString();
//                        this.Txt_EscrituraAnterior.Enabled = false;

//                        this.ddlAccionR.DataSource = (DataTable)Session["dtActoNotarialSubTipoId"];
//                        this.ddlAccionR.DataValueField = "id";
//                        this.ddlAccionR.DataTextField = "descripcion";
//                        this.ddlAccionR.DataBind();

//                        strScript = string.Empty;
//                        strScript = @"$(function(){{
//                                    HabilitaCamposRectificacion();
//                                }});";
//                        strScript = string.Format(strScript);
//                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitaCamposRectificacion", strScript, true);
//                    }

                }

                //----------------------------------------------------------------------------------------------------------------------------------
                if (Session["tab_activa_vinculacion"].ToString() == "SI")
                {
                    strScript = @"$(function(){{ HabilitarTabVinculacion(); }});";

                    strScript = string.Format(strScript);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitarTabVinculacion", strScript, true);
                    Session["tab_activa_vinculacion"] = "NO";
                }
                DataTable dtTipoParticipanteID = new DataTable();
                dtTipoParticipanteID = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REGISTRO_NOTARIAL_PROTOCOLAR_TIPO_PARTICIPANTE);

                //----------------------------------------------------------------------------------------------------------
                //Fecha: 11/04/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Asignar los participantes por acto notarial
                //----------------------------------------------------------------------------------------------------------
                DataView dvTipoParticipante = AsignarParticipantesPorTipoActoNotarial(dtTipoParticipanteID).DefaultView;
                //----------------------------------------------------------------------------------------------------------
                //DataView dvTipoParticipante = dtTipoParticipanteID.DefaultView;
                //----------------------------------------------------------------------------------------------------------
                DataTable dtOrdenadoTipParticipante = dvTipoParticipante.ToTable();
                dtOrdenadoTipParticipante.DefaultView.Sort = "para_tOrden ASC";

                //---------------------------------------------------------------------
                Util.CargarDropDownList(ddl_anpa_sTipoParticipanteId, dtOrdenadoTipParticipante, "descripcion", "Id", true);
                //-------------------------------------------------------------------------
                //Fecha: 02/04/2020
                //Autor: Miguel Márquez Beltrán
                //Motivo: Seleccionar el primer registro de la grilla.
                //-------------------------------------------------------------------------
                if (gdvVinculacion.Rows.Count > 0)
                {
                    gdvVinculacion.SelectedIndex = 0;

                    GridViewRow row = gdvVinculacion.Rows[0];
                    ImageButton imgbutton = (ImageButton)row.FindControl("btnSeleccionar");

                    gdvVinculacion_RowCommand(gdvVinculacion, new GridViewCommandEventArgs(imgbutton, new CommandEventArgs("Select", imgbutton.CommandArgument)));
                }
                //-----------------------------------------------------------------
                //Fecha: 12/01/2022
                //Autor: Miguel Márquez Beltrán
                //Motivo: En caso no tenga registrado los participantes
                //          se asigna por defecto al participante
                //          otorgante, vendedor, donante o anticipante.
                //------------------------------------------------------------------
                if (grd_Participantes.Rows.Count == 0)
                {
                    for (int i = 0; i < dtOrdenadoTipParticipante.Rows.Count; i++)
                    {
                        //if (dtOrdenadoTipParticipante.Rows[i]["id"].ToString() == Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE)
                        //    || dtOrdenadoTipParticipante.Rows[i]["id"].ToString() == Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR)
                        //    || dtOrdenadoTipParticipante.Rows[i]["id"].ToString() == Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE)
                        //    || dtOrdenadoTipParticipante.Rows[i]["id"].ToString() == Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE))

                        if (ObtenerIniciaRecibe(Convert.ToInt16(dtOrdenadoTipParticipante.Rows[i]["id"].ToString())) == "INICIA")
                        {
                            ddl_anpa_sTipoParticipanteId.SelectedValue = dtOrdenadoTipParticipante.Rows[i]["id"].ToString();
                            break;
                        }
                    }
                }
                //-------------------------------------------------------------------------
            }
            catch (Exception ex)
            {
                string strScript = string.Empty;
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Registro Notarial : Modo Edición.", ex.Message);
                Comun.EjecutarScript(Page, strScript);

            }
        }

        private string ObtenerTextoTipoDocumento(string value)
        {
            foreach (ListItem item in ddl_peid_sDocumentoTipoId.Items)
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

        #region WebMethods Genericos
        [System.Web.Services.WebMethod]
        public static void SetSession(string variable, string valor)
        {
            HttpContext.Current.Session[variable] = valor;
        }

        [System.Web.Services.WebMethod]
        public static string GetSession(string variable)
        {
            string strReturn = string.Empty;
            if (HttpContext.Current.Session[variable] != null)
                strReturn = HttpContext.Current.Session[variable].ToString();
            return strReturn.Trim();
        }
        #endregion

        //protected void ddl_ContDepRegistrador_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddl_ContDepRegistrador.SelectedIndex > 0)
        //    {
        //        comun_Part3.CargarUbigeo(Session, ddl_PaisCiudadRegistrador, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_ContDepRegistrador.SelectedValue.ToString(), "", true);
        //        comun_Part3.CargarUbigeo(Session, ddl_CiudadDistritoRegistrador, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, string.Empty, string.Empty, true);
        //    }
        //    else
        //    {
        //        comun_Part3.CargarUbigeo(Session, ddl_PaisCiudadRegistrador, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, string.Empty, string.Empty, true);
        //        comun_Part3.CargarUbigeo(Session, ddl_CiudadDistritoRegistrador, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, string.Empty, string.Empty, true);
        //    }
        //    UpdUbigeoRegistrador.Update();
        //}

        #endregion

        #region Método Generales - Formato
        private string ObtenerFechaParaFormatoEscritura(DateTime dfecha)
        {
            string vfechaReturn = string.Empty;
            string vmes = string.Empty;

            vmes = Comun.ObtenerMesNombre(dfecha.Month);

            vfechaReturn = string.Format("{0} de {1} del año {2}", dfecha.Day.ToString().PadLeft(2, '0'), vmes, dfecha.Year);

            return vfechaReturn;
        }

        private string ObtenerFechaParaIntroducccionEscritura(DateTime dfecha)
        {
            string vfechaReturn = string.Empty;
            string vmes = string.Empty;

            vmes = Comun.ObtenerMesNombre(dfecha.Month);

            vfechaReturn = string.Format("a los días {0} del mes {1} del {2}", dfecha.Day.ToString().PadLeft(2, '0'), vmes, dfecha.Year);

            return vfechaReturn;
        }

        private string ObtenerFechaParaConclusionEscritura(DateTime dfecha)
        {
            string vfechaReturn = string.Empty;
            string vmes = string.Empty;

            vmes = Comun.ObtenerMesNombre(dfecha.Month);

            vfechaReturn = string.Format("el {0} de {1} del {2}", dfecha.Day.ToString().PadLeft(2, '0'), vmes, dfecha.Year);

            return vfechaReturn;

        }
        #endregion

        #region General
        [System.Web.Services.WebMethod]
        public static Boolean isSessionTimeOut()
        {
            try
            {
                Boolean bResultado = true;
                HttpContext ctx = HttpContext.Current;
                if (ctx == null) bResultado = false;
                if (ctx.Session == null) bResultado = false;
                if (ctx.Session[Constantes.CONST_SESION_USUARIO] == null) bResultado = false;
                return bResultado;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void obtener_provinciasCs(string provincia)
        {
            SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
            DataTable lDataTable = objBL.Consultar(provincia, null, "01");
            DataView lDataView = lDataTable.DefaultView;
            lDataView.Sort = "ubge_vProvincia  ASC";

            ddl_UbigeoRegion.DataSource = lDataTable;
            ddl_UbigeoRegion.DataTextField = "ubge_vProvincia";
            ddl_UbigeoRegion.DataValueField = "ubge_cUbi02";
            ddl_UbigeoRegion.DataBind();
        }

        public void obtener_distritosCs(string departamento, string provincia)
        {
            SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
            DataTable lDataTable = objBL.Consultar(departamento, provincia, null);
            DataView lDataView = lDataTable.DefaultView;
            lDataView.Sort = "ubge_vDistrito  ASC";

            ddl_UbigeoCiudad.DataSource = lDataTable;
            ddl_UbigeoCiudad.DataTextField = "ubge_vDistrito";
            ddl_UbigeoCiudad.DataValueField = "ubge_cUbi03";
            ddl_UbigeoCiudad.DataBind();
        }

        private void CargarFuncionarios(DropDownList ddl, DataTable dt)
        {
            try
            {

                ddl.Items.Clear();
                ddl.DataTextField = "vFuncionario";
                ddl.DataValueField = "iFuncionarioId";
                ddl.DataSource = dt;
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //----------------------------------------------------------------------------------------------------
        //Fecha: 21/04/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Obtener los Tipos de Actos Notariales de acuerdo a los prestablecidos.
        //8050,8051,8041,8059,8060,8061,8062,8063,8064,8065,8066,8067,8068.
        //----------------------------------------------------------------------------------------------------
        private DataTable ObtenerTipoActoNotarial()
        {
            DataTable dtTipoActoNotarial = new DataTable();

            //dtTipoActoNotarial = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_NOTARIAL_TIPO_ACTO_PROTOCOLAR);

            //string FormatosProtocolar = ConfigurationManager.AppSettings["FormatoProtocolar"].ToString();

            //DataTable new_Data = null;

            //new_Data = (from dtsi in dtTipoActoNotarial.AsEnumerable()
            //            where FormatosProtocolar.Contains(dtsi["id"].ToString())
            //            select dtsi).CopyToDataTable();
            //new_Data.DefaultView.Sort = "torden asc";

            //return new_Data;
            TipoActoProtocolarTarifarioConsultasBL objTipoActoProtocolarTarifarioConsultaBL = new TipoActoProtocolarTarifarioConsultasBL();

            dtTipoActoNotarial = objTipoActoProtocolarTarifarioConsultaBL.Consultar_TipoActoProtocolar();

            return dtTipoActoNotarial;
        }

        private void FillWebCombo(DataTable pDataTable,
                                DropDownList pWebCombo,
                                String str_pDescripcion,
                                String str_pValor)
        {
            pWebCombo.DataSource = pDataTable;
            pWebCombo.DataTextField = str_pDescripcion;
            pWebCombo.DataValueField = str_pValor;
            pWebCombo.DataBind();
            pWebCombo.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
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

        private void SessionLugarSession()
        {
            String NombreConsulado = Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE].ToString();

            String[] Nombre = NombreConsulado.Split(',');
            Session["NombreConsulado"] = Nombre[0].ToString();
        }

        private void CargarComboApoderados()
        {
            ddl_Apoderado.Items.Clear();
            ddl_Apoderado.DataTextField = "descripcion";
            ddl_Apoderado.DataValueField = "id";
            ddl_Apoderado.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));

            List<CBE_PARTICIPANTE> Participantes = ((List<CBE_PARTICIPANTE>)Session["ParticipanteContainer"]).Where(x => x.anpa_cEstado != "E").ToList();

            foreach (CBE_PARTICIPANTE participante in Participantes)
            {
                //if (participante.anpa_sTipoParticipanteId == Convert.ToInt64(Enumerador.enmNotarialProtocolarTipoParticipante.APODERADO) ||
                //    participante.anpa_sTipoParticipanteId == Convert.ToInt64(Enumerador.enmNotarialProtocolarTipoParticipante.COMPRADOR) ||
                //    participante.anpa_sTipoParticipanteId == Convert.ToInt64(Enumerador.enmNotarialProtocolarTipoParticipante.DONATARIO) ||
                //    participante.anpa_sTipoParticipanteId == Convert.ToInt64(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPADO))

                if (ObtenerIniciaRecibe(participante.anpa_sTipoParticipanteId) == "RECIBE")
                {

                    ddl_Apoderado.Items.Add(new ListItem(participante.Persona.pers_vNombres + " " + participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno, participante.anpa_iActoNotarialParticipanteId + "," +
                        participante.Persona.Identificacion.peid_sDocumentoTipoId + "," +
                        participante.Persona.Identificacion.peid_vDocumentoNumero));

                }


            }
        }


        #endregion


        [System.Web.Services.WebMethod]
        public static String obtenerDatosParticipante(String parametros)
        {
            try
            {

                String[] ObjParametros = parametros.Split(',');
                Int64 iParticipanteId = Convert.ToInt64(ObjParametros[0]);
                List<CBE_PARTICIPANTE> Participantes = ((List<CBE_PARTICIPANTE>)HttpContext.Current.Session["ParticipanteContainer"]).Where(x => x.anpa_cEstado != "E" && x.anpa_iActoNotarialParticipanteId == iParticipanteId).ToList();

                RE_PERSONA oPersona = new RE_PERSONA();
                if (Participantes != null)
                {
                    if (Participantes.Count > 0)
                    {

                        oPersona.pers_vNombres = Participantes[0].Persona.pers_vNombres;
                        oPersona.pers_vApellidoPaterno = Participantes[0].Persona.pers_vApellidoPaterno;
                        oPersona.pers_vApellidoMaterno = Participantes[0].Persona.pers_vApellidoMaterno;
                        //---------------------------------------------------------
                        //Fecha: 05/01/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Asignar el apellido de casada.
                        //---------------------------------------------------------
                        oPersona.pers_vApellidoCasada = Participantes[0].Persona.pers_vApellidoCasada;
                        //-----------------------------------------------------------------------------------
                        oPersona.pers_sGeneroId = Participantes[0].Persona.pers_sGeneroId;
                        oPersona.Identificacion.peid_sDocumentoTipoId = Convert.ToInt16(ObjParametros[1].ToString());
                        oPersona.Identificacion.peid_vDocumentoNumero = ObjParametros[2].ToString();
                    }
                }
                string person = new JavaScriptSerializer().Serialize(oPersona);
                return person;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public void SeleccionarComboApoderado(Int16 sTipoDocumentoId = 0, String vNumeroDocumento = "")
        {

            List<CBE_PARTICIPANTE> Participantes = ((List<CBE_PARTICIPANTE>)Session["ParticipanteContainer"]).Where(x => x.anpa_cEstado != "E" && x.Identificacion.peid_sDocumentoTipoId == sTipoDocumentoId && x.Identificacion.peid_vDocumentoNumero == vNumeroDocumento).ToList();

            if (Participantes != null)
            {
                if (Participantes.Count > 0)
                {
                    rbOtros.Checked = false;
                    rbApoderado.Checked = true;

                    ddl_TipoDocrepresentante.Enabled = false;
                    txtRepresentanteNombres.Enabled = false;
                    txtRepresentanteNroDoc.Enabled = false;
                    ddl_GerenoPresentante.Enabled = false;
                    ddl_Apoderado.Enabled = true;

                    ddl_Apoderado.SelectedValue = Participantes[0].anpa_iActoNotarialParticipanteId + "," +
                           Participantes[0].Persona.Identificacion.peid_sDocumentoTipoId + "," +
                           Participantes[0].Persona.Identificacion.peid_vDocumentoNumero;
                }
            }

        }


        protected void btnDesabilitarAutoahesivo_Click(object sender, EventArgs e)
        {
            String strScript = String.Empty;
            BindGridActuacionesInsumoDetalle(Convert.ToInt64(hdn_acno_iActuacionId.Value));
            //LimpiarDatosVinculacion();
            #region Validar Vinculación Completa
            bool bolFalta = true;
            string strCodigo = "";
            String strImprimir = String.Empty;
            foreach (GridViewRow gdr in gdvVinculacion.Rows)
            {
                strCodigo = Convert.ToString(Page.Server.HtmlDecode(gdr.Cells[10].Text)).Trim();
                strImprimir = Convert.ToString(Page.Server.HtmlDecode(gdr.Cells[9].Text)).Trim();
                if (strCodigo != string.Empty)
                {
                    if (strImprimir.Equals("SI"))
                    { bolFalta = false; }
                    else
                    {
                        bolFalta = true;
                        break;
                    }
                }
                else
                {
                    bolFalta = true;
                    break;
                }
            }
            if (!bolFalta)
            {
                #region Cambiar Estado : VINCULADA
                RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();
                ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();

                lACTONOTARIAL.acno_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
                lACTONOTARIAL = lActoNotarialConsultaBL.obtener(lACTONOTARIAL);

                if (lACTONOTARIAL.acno_sEstadoId == (Int16)Enumerador.enmNotarialProtocolarEstado.PAGADA)
                {
                    ActoNotarialMantenimiento bl = new ActoNotarialMantenimiento();
                    RE_ACTONOTARIAL actonotarialBE = new RE_ACTONOTARIAL();
                    actonotarialBE.acno_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
                    actonotarialBE.acno_sEstadoId = (int)Enumerador.enmNotarialProtocolarEstado.VINCULADA;
                    actonotarialBE.acno_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    actonotarialBE.acno_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    actonotarialBE.acno_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();

                    if (bl.ActoNotarialActualizarEstado(actonotarialBE) == false)
                    {
                        LimpiarDatosVinculacion();
                        btnGrabarVinculacion.Enabled = false;
                        btnLimpiarVinc.Enabled = false;//--vpipa

                        Habilitar_Tab((int)Enumerador.enmNotarialProtocolarEstado.VINCULADA);

                        strScript = @"$(function(){{
                                        HabilitarTabDigitalizac();
                                    }});";
                        strScript = string.Format(strScript);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitarTabDigitalizac", strScript, true);
                    }
                }
                updFormato.Update();
                #endregion
            }
            #endregion
            updFormato.Update();
        }

        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 31/08/2016
        // Objetivo: Validar que el idioma del otorgante sea el castellano
        //           sino es así, deberá incluir un interprete.
        //------------------------------------------------------------------------
        [System.Web.Services.WebMethod]
        public static bool ValidarIdiomaCastellanoOtorgante(List<CBE_PARTICIPANTE> lParticipantes)
        {
            bool bconforme = true;
            string strIdioma = "";
            for (int i = 0; i < lParticipantes.Count; i++)
            {
                //if (lParticipantes[i].acpa_sTipoParticipanteId_desc.ToUpper().Trim() == "PADRE" ||
                //    lParticipantes[i].acpa_sTipoParticipanteId_desc.ToUpper().Trim() == "MADRE" ||
                //    lParticipantes[i].acpa_sTipoParticipanteId_desc.ToUpper().Trim() == "OTORGANTE" ||
                //    lParticipantes[i].acpa_sTipoParticipanteId_desc.ToUpper().Trim() == "VENDEDOR" ||
                //    lParticipantes[i].acpa_sTipoParticipanteId_desc.ToUpper().Trim() == "DONANTE" ||
                //    lParticipantes[i].acpa_sTipoParticipanteId_desc.ToUpper().Trim() == "ANTICIPANTE")

                if (ObtenerIniciaRecibe(lParticipantes[i].acpa_sTipoParticipanteId_desc.ToUpper().Trim()) == "INICIA")
                {
                    if (lParticipantes[i].anpa_cEstado != "E" && lParticipantes[i].pers_sIdiomaNatalId_desc != "CASTELLANO")
                    {
                        strIdioma = lParticipantes[i].pers_sIdiomaNatalId_desc.ToUpper().Trim();
                        bconforme = ValidarIdiomaExtranjeroInterprete(lParticipantes, strIdioma);
                        if (bconforme == false)
                        { break; }
                    }
                }
            }

            return bconforme;
        }

        [System.Web.Services.WebMethod]
        public static bool ValidarIdiomaExtranjeroInterprete(List<CBE_PARTICIPANTE> lParticipantes, string strIdioma)
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

        //----------------------------------------------------------
        //Fecha: 10/02/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Obtener el prefijo del titulo del testimonio
        //----------------------------------------------------------
        private static string NumeroOrdinal(int s_numero)
        {
            string v_Numero = string.Empty;
            switch (s_numero)
            {
                case 1:
                    v_Numero = "PRIMER";
                    break;
                case 2:
                    v_Numero = "SEGUNDO";
                    break;
                case 3:
                    v_Numero = "TERCER";
                    break;
                case 4:
                    v_Numero = "CUARTO";
                    break;
                case 5:
                    v_Numero = "QUINTO";
                    break;
                case 6:
                    v_Numero = "SEXTO";
                    break;
                case 7:
                    v_Numero = "SÉPTIMO";
                    break;
                case 8:
                    v_Numero = "OCTAVO";
                    break;
                case 9:
                    v_Numero = "NOVENO";
                    break;
                case 10:
                    v_Numero = "DÉCIMO";
                    break;
                case 11:
                    v_Numero = "UNDÉCIMO";
                    break;
                case 12:
                    v_Numero = "DUODÉCIMO";
                    break;
                case 13:
                    v_Numero = "DECIMOTERCER";
                    break;
                case 14:
                    v_Numero = "DECIMOCUARTO";
                    break;
                case 15:
                    v_Numero = "DECIMOQUINTO";
                    break;
                case 16:
                    v_Numero = "DECIMOSEXTO";
                    break;
                case 17:
                    v_Numero = "DECIMOSÉPTIMO";
                    break;
                case 18:
                    v_Numero = "DECIMOOCTAVO";
                    break;
                case 19:
                    v_Numero = "DECIMONOVENO";
                    break;
                case 20:
                    v_Numero = "VIGÉSIMO";
                    break;
                default:
                    v_Numero = "";
                    break;
            }
            return v_Numero;
        }
        //----------------------------------------------------------
        //-----------------------------------------------------------------------
        //Fecha: 02/03/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Obtener las Normas que se encuentran en el campo referencia
        //          del Tipo de Acto Notarial (tabla: Parametros)
        //-----------------------------------------------------------------------
        //public void AsignarNormasporTipoActoNotarial()
        //{
            //DataTable dtTipoActoNotarial = new DataTable();
            //dtTipoActoNotarial = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_NOTARIAL_TIPO_ACTO_PROTOCOLAR);

            //string strTipoActoNotarial = Cmb_TipoActoNotarial.SelectedValue;
            //string strReferencia = "";

            //for (int i = 0; i < dtTipoActoNotarial.Rows.Count; i++)
            //{
            //    if (dtTipoActoNotarial.Rows[i]["id"].ToString().Equals(strTipoActoNotarial))
            //    {
            //        strReferencia = dtTipoActoNotarial.Rows[i]["para_vReferencia"].ToString();
            //        break;
            //    }
            //}
            ////-------------------------------------------------------------------------            
            //DataTable s_Datos = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_NOTARIAL_SUB_TIPO_INFORMACION_INSERTO);

            //DataTable new_Data = null;
            //if (strReferencia.Length > 0)
            //{
            //    new_Data = (from dtsi in s_Datos.AsEnumerable()
            //                where Comun.ToNullInt64(dtsi["para_vReferencia"]) == ((int)Enumerador.enmProtocolarTipoInformacion.TEXTO_NORMATIVO)
            //                && strReferencia.Contains(dtsi["id"].ToString())
            //                select dtsi).CopyToDataTable();
            //    new_Data.DefaultView.Sort = "torden asc";

            //    //-----carga los checkbos de textos normativos
            //    string html = "";
            //    string normativo2 = hf_idsNormativos.Value;
            //    normativo2 = normativo2.Length > 0 ? normativo2.Substring(0, normativo2.Length - 1) : "";
            //    string[] lstArticulos2 = normativo2.Split('|');

            //    if (new_Data != null)
            //    {
            //        foreach (DataRow row in new_Data.Rows)
            //        {
            //            string selected = "";

            //            foreach (string strArticulo in lstArticulos2)
            //            {
            //                if (row["id"].ToString() == strArticulo)
            //                {
            //                    selected = "checked='checked'";
            //                    break;
            //                }
            //            }
            //            html = html + "<input type='checkbox' id='check" + row["id"].ToString() + "' " + selected + " onclick='setearTextoNormativo();' /><label for='check" + row["id"].ToString() + "'>" + row["descripcion"].ToString() + "</label><br>";
            //        }
            //    }
            //    divListNormativo.InnerHtml = html;
            //}
                      
            //---fin reemplazo
            
   //     }

        //-------------------------------------------
        //Fecha: 07/08/2018
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Comentar el evento del Genero.
        //          se controlara por javascript.
        //-------------------------------------------
     
        //-------------------------------------------
        //Fecha: 15/03/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Consultar el gentilicio
        //-------------------------------------------
        //protected void ddl_pers_sGeneroId_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //---------------------------------------------------
        //    //Fecha: 07/07/2020
        //    //Autor: Miguel Márquez Beltrán
        //    //Motivo: Se comenta el gentilicio.
        //    //---------------------------------------------------
        //    //DataTable dtPaises = new DataTable();

        //    //dtPaises = (DataTable)ViewState["Paises"];
        //    ////---------------------------------------------------
        //    //LblDescGentilicio.Text = Comun.AsignarGentilicio(dtPaises, ddlPaisOrigen, ddl_pers_sGeneroId).ToUpper();
        //    ActualizarGenero();
        //    ddl_pers_sEstadoCivilId.Enabled = true;
        //    //--------------------------------------
        //    MostrarOcultarIncapacidad();
        //    MostrarOcultarInterprete();
        //    //------------------------------------------------
        //    //Fecha: 24/07/2020
        //    //Autor: Miguel Márquez Beltrán
        //    //Motivo: Validar la presencia del check de huella
        //    //------------------------------------------------
        //    MostrarOcultarHuella();
        //    //---------------------------------------------------------------
        //    //Fecha: 30/07/2020
        //    //Autor: Miguel Márquez Beltrán
        //    //Motivo: LLenar la lista desplegable con los participantes 
        //    //          que tengan discapacidad.
        //    //---------------------------------------------------------------
        //    llenarParticipanteDiscapacidad();
        //    actualizarUbigeo();
        //    updDatosParticipantes.Update();
        //}

        //protected void ddl_peid_sDocumentoTipoId_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    txtDescOtroDocumento.Text = "";
        //    if (ddl_peid_sDocumentoTipoId.SelectedValue == Convert.ToString((int)Enumerador.enmTipoDocumento.OTROS))
        //    {
        //        LblOtroDocumento.Visible = true;
        //        txtDescOtroDocumento.Visible = true;
        //        txtDescOtroDocumento.Enabled = true;
        //    }
        //    else
        //    {
        //        LblOtroDocumento.Visible = false;
        //        txtDescOtroDocumento.Visible = false;
        //        txtDescOtroDocumento.Enabled = false;
        //    }
        //}
        //-------------------------------------------
        //Fecha: 15/03/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Actualizar el Genero
        //-------------------------------------------
        private void ActualizarGenero()
        {
            string strScript = string.Empty;
            strScript = @"$(function(){{
                                        ObtenerElementosGenero();
                                    }});";
            strScript = string.Format(strScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "Genero", strScript, true);
        }

        

        //----------------------------------------------------------------------------------------------------------
        //Fecha: 11/04/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Obtener los participantes por acto notarial
        //----------------------------------------------------------------------------------------------------------

        private DataTable AsignarParticipantesPorTipoActoNotarial(DataTable dt)
        {
            DataTable dtTipoActoNotarial = new DataTable();
            dtTipoActoNotarial = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_NOTARIAL_TIPO_ACTO_PROTOCOLAR);

            string strTipoActoNotarial = Cmb_TipoActoNotarial.SelectedValue;
            string strValor = "";

            for (int i = 0; i < dtTipoActoNotarial.Rows.Count; i++)
            {
                if (dtTipoActoNotarial.Rows[i]["id"].ToString().Equals(strTipoActoNotarial))
                {
                    strValor = dtTipoActoNotarial.Rows[i]["valor"].ToString();
                    break;
                }
            }

            DataTable dtNew = new DataTable();

            dtNew = dt.Clone();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (strValor.Contains(dt.Rows[i]["id"].ToString()))
                {
                    DataRow dr = dtNew.NewRow();

                    dr["id"] = dt.Rows[i]["id"].ToString();
                    dr["descripcion"] = dt.Rows[i]["descripcion"].ToString();

                    dtNew.Rows.Add(dr);
                }
            }

            return dtNew;
        }

        protected void Cmb_TipoActoNotarial_SelectedIndexChanged(object sender, EventArgs e)
        {
            chk_acno_bFlagMinuta.Checked = false;
            chk_acno_bFlagMinuta.Enabled = true;
            if (Cmb_TipoActoNotarial.SelectedIndex > 0)
            {
                string strTipoActoProtocolar = Cmb_TipoActoNotarial.SelectedItem.Text.Trim().ToUpper();
                //--------------------------------------------------
                // Fecha: 22/09/2021
                // Autor: Miguel Márquez Beltrán
                // Motivo: Habilitar check de Minuta como obligatorio.
                //--------------------------------------------------
                if (strTipoActoProtocolar == "COMPRA - VENTA" || strTipoActoProtocolar == "DONACIÓN" ||
                    strTipoActoProtocolar == "ANTICIPO DE LEGÍTIMA" || strTipoActoProtocolar == "ANTICIPO DE HERENCIA" ||
                    strTipoActoProtocolar == "DONACIÓN")
                {
                    chk_acno_bFlagMinuta.Enabled = false;
                    chk_acno_bFlagMinuta.Checked = true;
                }

                if (strTipoActoProtocolar == "RENUNCIA A LA NACIONALIDAD PERUANA")
                {
                    hRegistradorId.Style.Add("display", "none");
                    tablaRegistradorId.Style.Add("display", "none");
                    hPresentanteId.Style.Add("display", "none");
                    tablaPresentanteId.Style.Add("display", "none");
                }
                else
                {
                    hRegistradorId.Style.Add("display", "block");
                    tablaRegistradorId.Style.Add("display", "block");
                    hPresentanteId.Style.Add("display", "block");
                    tablaPresentanteId.Style.Add("display", "block");
                }
                UpdPresentante.Update();
                //----------------------------------------------
                //Fecha: 06/10/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Cambiar el texto de lblNroOficio
                //      de "N° de Oficio: " a "Oficio a RENIEC: "
                //      cuando el tipo de acto sea:
                //      "RENUNCIA A LA NACIONALIDAD PERUANA"
                //----------------------------------------------
                if (strTipoActoProtocolar == "RENUNCIA A LA NACIONALIDAD PERUANA")
                {
                    lblNroOficio.Text = "Oficio a RENIEC: "; 
                }
                else
                {
                    lblNroOficio.Text = "N° de Oficio: ";
                }
                //----------------------------------------------
                DataTable dtTipoParticipanteID = new DataTable();
                dtTipoParticipanteID = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REGISTRO_NOTARIAL_PROTOCOLAR_TIPO_PARTICIPANTE);                
                
                //----------------------------------------------------------------------------------------------------------
                //Fecha: 11/04/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Asignar los participantes por acto notarial
                //----------------------------------------------------------------------------------------------------------
                DataView dvTipoParticipante = AsignarParticipantesPorTipoActoNotarial(dtTipoParticipanteID).DefaultView;
                //----------------------------------------------------------------------------------------------------------
                //DataView dvTipoParticipante = dtTipoParticipanteID.DefaultView;
                //----------------------------------------------------------------------------------------------------------

                DataTable dtOrdenadoTipParticipante = dvTipoParticipante.ToTable();
                dtOrdenadoTipParticipante.DefaultView.Sort = "para_tOrden ASC";

                dtOrdenadoTipParticipante.DefaultView.RowFilter = "";

                Util.CargarDropDownList(ddl_anpa_sTipoParticipanteId, dtOrdenadoTipParticipante, "descripcion", "Id", true);
                //--------------------------------------------------------------------
                // Fecha: 09/09/2021
                // Autor: Miguel Márquez Beltrán
                // Motivo: Asignar al otorgante o Vendedor o Anticipante por defecto.
                //--------------------------------------------------------------------
                string strInicia = "";
                short iTipoParticipanteId = 0;
                for (int i = 0; i < dtOrdenadoTipParticipante.Rows.Count; i++)
                {
                    //if (dtOrdenadoTipParticipante.Rows[i]["id"].ToString() == Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE)
                    //    || dtOrdenadoTipParticipante.Rows[i]["id"].ToString() == Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR)
                    //    || dtOrdenadoTipParticipante.Rows[i]["id"].ToString() == Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE)
                    //    || dtOrdenadoTipParticipante.Rows[i]["id"].ToString() == Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE))

                    if (ObtenerIniciaRecibe(Convert.ToInt16(dtOrdenadoTipParticipante.Rows[i]["id"].ToString())) == "INICIA")
                    {
                        strInicia = "OTORGANTE";
                        if (strTipoActoProtocolar.IndexOf("ADELANTO") > -1)
                            strInicia = "ANTICIPANTE";
                        if (strTipoActoProtocolar.IndexOf("COMPRA") > -1 || strTipoActoProtocolar.IndexOf("VENTA") > -1)
                            strInicia = "VENDEDOR";
                        if (strTipoActoProtocolar.IndexOf("DONACIÓN") > -1)
                            strInicia = "DONATARIO";

                        iTipoParticipanteId = Convert.ToInt16(dtOrdenadoTipParticipante.Rows[i]["id"].ToString());

                        if (ObtenerNombreParticipante(iTipoParticipanteId) == strInicia)
                        {
                            ddl_anpa_sTipoParticipanteId.SelectedValue = iTipoParticipanteId.ToString();
                            HF_sTipoParticipanteId.Value = iTipoParticipanteId.ToString();
                            break;
                        }                                                
                    }
                }
                //-------------------------------------------------------
                //Fecha: 02/12/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Visualizar la tabla primigenia si se 
                //          selecciona Revocación, Modificación
                //          Ampliación o Rectificación.                
                //-------------------------------------------------------
                string strScript = string.Empty;
                hf_PG_PE.Value = "";
                if (strTipoActoProtocolar.IndexOf("REVOCACIÓN") > -1 ||
                    strTipoActoProtocolar.IndexOf("MODIFICACIÓN") > -1 ||
                    strTipoActoProtocolar.IndexOf("AMPLIACIÓN") > -1 ||
                    strTipoActoProtocolar.IndexOf("RECTIFICACIÓN") > -1)
                {                    
                    if (strTipoActoProtocolar.IndexOf("GENERAL") > -1)
                    {
                        hf_PG_PE.Value = "G";
                    }
                    else
                    {
                        hf_PG_PE.Value = "E";
                    }

                    strScript = @"$(function(){{
                                            HabilitarTablaPrimigenia();
                                            }});";                    
                }
                else
                {
                    strScript = @"$(function(){{
                                            DeshablitarTablaPrimigenia();
                                            }});";                    
                }
                strScript += "LimpiarTabCuerpo();";
                strScript = string.Format(strScript);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "TablaPrimigenia", strScript, true);
                //------------------------------------------------------------------------------------------------
                    ddl_pers_sPersonaTipoId.SelectedValue = Convert.ToString(ViewState["iTipoId"]);
                    //ddl_pers_sPersonaTipoId_SelectedIndexChanged(sender, e);
                    string scriptMover = string.Empty;
                    switch (Comun.ToNullInt64(ddl_pers_sPersonaTipoId.SelectedValue))
                    {
                        case (Int64)Enumerador.enmTipoPersona.JURIDICA:
                            ddl_empr_sTipoDocumentoId.SelectedValue = Convert.ToString(ViewState["iDocumentoTipoId"]);
                            txt_empr_vNumeroDocumento.Text = Convert.ToString(ViewState["NroDoc"]);
                            txt_empr_vRazonSocial.Text = Convert.ToString(ViewState["ApePat"]);
                            break;
                        case (Int64)Enumerador.enmTipoPersona.NATURAL:

                            string strTipoDocumento = Convert.ToString(ViewState["iDocumentoTipoId"]);
                            buscarValorTipoDocumento(ref ddl_peid_sDocumentoTipoId, strTipoDocumento);
                            
                            txt_peid_vDocumentoNumero.Text = Convert.ToString(ViewState["NroDoc"]);
                            break;
                    }
                    updDatosParticipantes.Update();
                //-------------------------------------------------------                
                updParticipantesOtogarntes.Update();
                Txt_Denominacion.Focus();
                updTipoActoNotarial.Update();
                updCuerpoCabecera.Update();
                updParte.Update();
            }
        }
        //-------------------------------------------------------------
        //Fecha: 21/04/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Obtener la ciudad y la fecha.
        //-------------------------------------------------------------
        [System.Web.Services.WebMethod]
        public static string ObtenerCiudadFecha(string cuerpo)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var jsonObject = serializer.Deserialize<dynamic>(cuerpo);

            string strCiudadFecha = "";

            if (Convert.ToInt64(jsonObject["ancu_iActoNotarialId"]) != 0)
            {
                Int64 intActoNotarialId = Convert.ToInt64(jsonObject["ancu_iActoNotarialId"]);

                ActoNotarialConsultaBL bl = new ActoNotarialConsultaBL();
                DataTable dtDatosEscritura = new DataTable();

                dtDatosEscritura = bl.ActonotarialObtenerDatosPrincipales(intActoNotarialId, Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                //--------------------------------------------------------------------------
                //Fecha: 03/04/2020
                //Autor: Miguel Márquez Beltrán
                //Motivo: Asignar los datos de la escritura a una sesión.
                //--------------------------------------------------------------------------
                HttpContext.Current.Session["dtDatosEscritura"] = dtDatosEscritura;
                //--------------------------------------------------------------------------
                string strUbigeoOficinaConsular = dtDatosEscritura.Rows[0]["CiudadOficinaConsular"].ToString().Trim();
                string strFecha = Util.ObtenerDiaMesLargoAnio(Comun.FormatearFecha(dtDatosEscritura.Rows[0]["Fecha"].ToString()), true);
                dtDatosEscritura.Dispose();
                strCiudadFecha = strUbigeoOficinaConsular + ", " + strFecha;
            }

            return serializer.Serialize(strCiudadFecha).ToString();
        }

        private string ObtenerCiudad_Fecha(string strActoNotarialId)
        {
            string strCiudadFecha = "";

            if (Convert.ToInt64(strActoNotarialId) != 0)
            {
                Int64 intActoNotarialId = Convert.ToInt64(strActoNotarialId);

                ActoNotarialConsultaBL bl = new ActoNotarialConsultaBL();
                DataTable dtDatosEscritura = new DataTable();

                dtDatosEscritura = bl.ActonotarialObtenerDatosPrincipales(intActoNotarialId, Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));

                //--------------------------------------------------------------------------
                //Fecha: 03/04/2020
                //Autor: Miguel Márquez Beltrán
                //Motivo: Asignar los datos de la escritura a una sesión.
                //--------------------------------------------------------------------------
                HttpContext.Current.Session["dtDatosEscritura"] = dtDatosEscritura;
                //--------------------------------------------------------------------------        

                string strUbigeoOficinaConsular = dtDatosEscritura.Rows[0]["CiudadOficinaConsular"].ToString().Trim();
                string strFecha = Util.ObtenerDiaMesLargoAnio(Comun.FormatearFecha(dtDatosEscritura.Rows[0]["Fecha"].ToString()), true);
                dtDatosEscritura.Dispose();
                strCiudadFecha = strUbigeoOficinaConsular + ", " + strFecha;
            }
            return strCiudadFecha;
        }


        //-------------------------------------------------------------
        //Fecha: 20/09/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Actualizar la Fecha de Suscripción del Otorgante
        //-------------------------------------------------------------

        protected void btnActualizarFechaOtorgante_Click(object sender, EventArgs e)
        {

            if (ctrFechaSuscripcion.Text == string.Empty)
            {
                ctrFechaSuscripcion.Focus();
                EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Acto Protocolar", "La fecha de suscripción no debe estar en blanco."), "MensajeFechaSuscripcion");
                return;
            }

            List<CBE_PARTICIPANTE> ParametrosContainer = (List<CBE_PARTICIPANTE>)Session["ParticipanteContainer"];

            string strActoNotarialParticipanteId = HFOtorganteId.Value;

            DateTime datFecha = new DateTime();

            if (ctrFechaSuscripcion.Text.Length == 0)
            {
                datFecha = DateTime.MinValue;
            }
            else
            {
                if (!DateTime.TryParse(ctrFechaSuscripcion.Text, out datFecha))
                {
                    datFecha = Comun.FormatearFecha(ctrFechaSuscripcion.Text);
                }
            }

            for (int i = 0; i < ParametrosContainer.Count; i++)
            {
                if (ParametrosContainer[i].anpa_iActoNotarialParticipanteId.ToString().Equals(strActoNotarialParticipanteId))
                {
                    ParametrosContainer[i].anpa_dFechaSuscripcion = datFecha;
                    break;
                }
            }
            Session["ParticipanteContainer"] = (List<CBE_PARTICIPANTE>)ParametrosContainer;

            DataTable dtParticipantes = new DataTable();
            dtParticipantes = CrearTablaParticipante(null);


            grd_Otorgantes.DataSource = ObtenerOtorgantesOrdenados(dtParticipantes);
            grd_Otorgantes.DataBind();

            updGrillaOtorgantes.Update();


            btnActualizarFechaOtorgante.Enabled = false;
            lblNombresOtorgante.Text = "Nombres del Otorgante";
            ctrFechaSuscripcion.Text = "";

            updFirmaOtorgantes.Update();
        }

        //---------------------------------------------------------------------
        //Fecha: 20/09/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Método para editar la Fecha de Suscripción del otorgante
        //---------------------------------------------------------------------

        protected void grd_Otorgantes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString().Equals("Editar"))
            {
                List<CBE_PARTICIPANTE> Participantes = (List<CBE_PARTICIPANTE>)Session["ParticipanteContainer"];

                GridViewRow gvrModificar;
                gvrModificar = (GridViewRow)((TableCell)((ImageButton)e.CommandSource).Parent).Parent;

                Label lblActoNotarialParticipanteId = (Label)gvrModificar.FindControl("lblActoNotarialParticipanteId");
                string strActoNotarialParticipanteId = lblActoNotarialParticipanteId.Text.ToString();

                HFOtorganteId.Value = strActoNotarialParticipanteId;

                lblNombresOtorgante.Text = "";
                ctrFechaSuscripcion.Text = "";
                btnActualizarFechaOtorgante.Enabled = false;

                for (int i = 0; i < Participantes.Count; i++)
                {
                    if (Participantes[i].anpa_iActoNotarialParticipanteId.ToString().Equals(strActoNotarialParticipanteId))
                    {
                        lblNombresOtorgante.Text = Participantes[i].Persona.pers_vApellidoPaterno.ToString() + " " + Participantes[i].Persona.pers_vApellidoMaterno.ToString() + ", " + Participantes[i].Persona.pers_vNombres.ToString();

                        string strFechaSuscripcion = Participantes[i].anpa_dFechaSuscripcion.ToString();

                        if (strFechaSuscripcion != "")
                        {
                            ctrFechaSuscripcion.Text = (Comun.FormatearFecha(strFechaSuscripcion).ToString(ConfigurationManager.AppSettings["FormatoFechas"]));
                        }
                        btnActualizarFechaOtorgante.Enabled = true;
                        break;
                    }
                }
                updFirmaOtorgantes.Update();
            }
        }

        //-----------------------------------------------------------------------------------
        //Fecha: 20/09/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Obtener Datatable de otorgantes ordenados por la Fecha de Suscripción
        //-----------------------------------------------------------------------------------

        private DataTable ObtenerOtorgantesOrdenados(DataTable dtParticipantes)
        {
            DataView dv = dtParticipantes.DefaultView;
            DataTable dts = dv.ToTable();

            try
            {
                //dts = (from otorgantes in dtParticipantes.AsEnumerable()
                //       where Convert.ToInt16(otorgantes["anpa_sTipoParticipanteId"]) != Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.APODERADO)
                //       orderby otorgantes["anpa_dFechaSuscripcion"]
                //       select otorgantes).CopyToDataTable();
                dts = (from otorgantes in dtParticipantes.AsEnumerable()
                       where ObtenerIniciaRecibe(Convert.ToInt16(otorgantes["anpa_sTipoParticipanteId"])) == "INICIA"
                       orderby otorgantes["anpa_dFechaSuscripcion"]
                       select otorgantes).CopyToDataTable();
            }
            catch
            {
                dts = null;
            }

            if (dts != null)
            {
                hdn_acno_dFechaConclusionFirma.Value = dts.Rows[dts.Rows.Count - 1]["anpa_dFechaSuscripcion"].ToString();
            }
            else
            {
                hdn_acno_dFechaConclusionFirma.Value = "";
            }


            return dts;
        }

        private static string ActualizarFechaSuscripcionOtorgantes()
        {
            try
            {
                string sMensaje = String.Empty;

                List<CBE_PARTICIPANTE> lParticipantes = (List<CBE_PARTICIPANTE>)HttpContext.Current.Session["ParticipanteContainer"];

                if (lParticipantes != null)
                {
                    //lParticipantes = lParticipantes.Where(x => Convert.ToInt16(x.anpa_sTipoParticipanteId) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE)
                    //                                    || Convert.ToInt16(x.anpa_sTipoParticipanteId) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR)
                    //                                    || Convert.ToInt16(x.anpa_sTipoParticipanteId) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE)
                    //                                    || Convert.ToInt16(x.anpa_sTipoParticipanteId) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE)
                    //                                    || Convert.ToInt16(x.anpa_sTipoParticipanteId) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE)
                    //                                    || Convert.ToInt16(x.anpa_sTipoParticipanteId) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO)
                    //    ).ToList();

                    lParticipantes = lParticipantes.Where(x => ObtenerIniciaRecibe(x.anpa_sTipoParticipanteId) == "INICIA"
                                                        || Convert.ToInt16(x.anpa_sTipoParticipanteId) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE)
                                                        || Convert.ToInt16(x.anpa_sTipoParticipanteId) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO)
                        ).ToList();


                    List<CBE_PARTICIPANTE> lActoNotarialParticipante = new List<CBE_PARTICIPANTE>();

                    CBE_PARTICIPANTE PARTICIPANTE_BE;

                    foreach (CBE_PARTICIPANTE p in lParticipantes)
                    {
                        PARTICIPANTE_BE = new CBE_PARTICIPANTE();
                        PARTICIPANTE_BE.anpa_iActoNotarialId = p.anpa_iActoNotarialId;
                        PARTICIPANTE_BE.anpa_iActoNotarialParticipanteId = p.anpa_iActoNotarialParticipanteId;
                        PARTICIPANTE_BE.anpa_dFechaSuscripcion = p.anpa_dFechaSuscripcion;
                        PARTICIPANTE_BE.anpa_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                        PARTICIPANTE_BE.anpa_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
                        PARTICIPANTE_BE.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                        lActoNotarialParticipante.Add(PARTICIPANTE_BE);
                    }


                    ActoNotarialMantenimiento mnt = new ActoNotarialMantenimiento();

                    mnt.ActualizarFechaSuscripcion(lActoNotarialParticipante, ref sMensaje);
                }
                if (sMensaje != String.Empty)
                    return "OK";
                else
                    return sMensaje;
            }
            catch (Exception ex)
            {
                string strScript = string.Empty;
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Registro Notarial : Cuerpo", ex.Message);
                throw ex;

            }
        }

        private void CargarTipoPagoNormaTarifario()
        {

            int IntTotalCount = 0;
            int IntTotalPages = 0;


            //Lst_Tarifario.Items[0].Text.Substring(0,Lst_Tarifario.Items[0].Text.IndexOf("-")).Trim()


            if (Lst_Tarifario.Items.Count == 0)
            { return; }

            if (Txt_TarifaId.Text.Trim().Length == 0)
            { return; }

            //string strTarifaLetra = Txt_TarifaId.Text.Trim().ToUpper();

            string strTarifaLetra = Lst_Tarifario.Items[0].Text.Substring(0, Lst_Tarifario.Items[0].Text.IndexOf("-")).Trim();

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
            if (Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Convert.ToInt16(Constantes.CONST_ID_CONSULADO_CARACAS))
            {
                if (ddlTipoPago.Items.FindByText("PAGO ARUBA") != null)
                {
                    ddlTipoPago.Items.FindByText("PAGO ARUBA").Enabled = false;
                }
                if (ddlTipoPago.Items.FindByText("PAGO OTRAS ISLAS CARIBEÑAS") != null)
                {
                    ddlTipoPago.Items.FindByText("PAGO OTRAS ISLAS CARIBEÑAS").Enabled = false;
                }
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

        private void MostrarOcultarIncapacidad()
        {
            String strScrip = String.Empty;
            strScrip += @"$(function(){{ ";
            //if (ddl_anpa_sTipoParticipanteId.SelectedValue == Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE) ||
            //    ddl_anpa_sTipoParticipanteId.SelectedValue == Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR) ||
            //    ddl_anpa_sTipoParticipanteId.SelectedValue == Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE) ||
            //    ddl_anpa_sTipoParticipanteId.SelectedValue == Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE))

            if (ObtenerIniciaRecibe(Convert.ToInt16(ddl_anpa_sTipoParticipanteId.SelectedValue)) == "INICIA")
            {
                strScrip += " MostrasIncapacidad(); ";
                strScrip += " OcularListaIncapacidad(); OcultarHuella();";
            }
            else
            {
                strScrip += " OcultarIncapacidad(); ";
                if (ddl_anpa_sTipoParticipanteId.SelectedValue == Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO))
                {
                    strScrip += " MostrarlistaIncapacidad(); ";
                }
                else
                {
                    strScrip += " OcularListaIncapacidad(); OcultarHuella();";
                }
            }
            strScrip += "}});";
            strScrip = String.Format(strScrip);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "MostrarOcultarListaIncapacidad", strScrip, true);
        }

        private void MostrarOcultarInterprete()
        {
            String strScrip = String.Empty;
            strScrip += @"$(function(){{ActivarOtrodocumento(); ";

            if (ddl_anpa_sTipoParticipanteId.SelectedValue == Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE))
            {
                if (ddl_Participante_Interprete.Items.Count > 2)
                {
                    strScrip += " MostrarlistaInterpretes(); ";
                }
                else
                {
                    strScrip += " OcularListaInterpretes(); ";
                }
            }
            else
            {
                strScrip += " OcularListaInterpretes(); ";
            }
            strScrip += "}});";
            strScrip = String.Format(strScrip);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "MostrarOcultarListaInterpretes", strScrip, true);

        }
        //------------------------------------------------
        //Fecha: 24/07/2020
        //Autor: Miguel Márquez Beltrán
        //Motivo: Validar la presencia del check de huella
        //------------------------------------------------
        private void MostrarOcultarHuella()
        {
            if (chkIncapacitado.Visible == true)
            {
                String strScrip = String.Empty;
                strScrip += @"$(function(){{";

                if (chkIncapacitado.Checked)
                {
                    strScrip += " MostrarHuella(); ";
                }
                else
                {
                    strScrip += " OcultarHuella(); ";
                }

                
                strScrip += "}});";
                strScrip = String.Format(strScrip);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MostrarOcultarHuellas", strScrip, true);
            }
         }
        //-----------------------------------------------------------//
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 27-10-2016
        // Objetivo: Llenar la lista de exoneraciones
        //-----------------------------------------------------------//
        private void LlenarListaExoneracion()
        {
            //-------------------------------------------
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

            if (strTarifaLetra.Length == 0)
                return;

            string strTipoPago = ddlTipoPago.SelectedItem.Text.Trim();

            DataTable dtExoneracion = new DataTable();

            NormaTarifarioDL objNormaTarifarioBL = new NormaTarifarioDL();
            int IntTotalCount = 0;
            int IntTotalPages = 0;

            string strFecha = Comun.syyyymmdd(DateTime.Now.ToShortDateString());
            Int16 intTipoPagoId = Convert.ToInt16(ddlTipoPago.SelectedValue);

            dtExoneracion = objNormaTarifarioBL.Consultar(intTipoPagoId, -1, strTarifaLetra, strFecha, false, 1000, 1, "N", ref IntTotalCount, ref IntTotalPages);

            Util.CargarDropDownList(ddlExoneracion, dtExoneracion, "norm_vDescripcionCorta", "nota_iNormaTarifarioId", true);
            if (dtExoneracion.Rows.Count > 0)
            {
                #region Si_ExisteRegistros

                if (dtExoneracion.Rows.Count == 1)
                {
                    ddlExoneracion.SelectedIndex = 1;
                }
                else
                {
                    ddlExoneracion.SelectedIndex = 0;
                }
                ddlExoneracion.Enabled = true;
                RBNormativa.Checked = true;
                txtSustentoTipoPago.Enabled = false;
                txtSustentoTipoPago.Text = "";
                lblExoneracion.Visible = true;
                ddlExoneracion.Visible = true;
                lblValExoneracion.Visible = true;

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

                    //----------------------------------------------------------------------
                    // Fecha: 09/09/2021
                    // Autor: Miguel Márquez Beltrán
                    // Motivo: Asignar valor al tipo de documento, número de documento
                    //          y tipo de participante.
                    //----------------------------------------------------------------------
                    bool bExisteDocumento = false;
                    string strTipoDocumentoId = dt.Rows[0]["sDocumentoTipoId"].ToString();
                    for (int i = 0; i < ddl_peid_sDocumentoTipoId.Items.Count; i++)
                    {
                        if (ddl_peid_sDocumentoTipoId.Items[i].Value.Equals(strTipoDocumentoId))
                        {
                            bExisteDocumento = true;
                            break;
                        }
                    }
                    if (bExisteDocumento)
                    {
                        HF_vDescTipDoc.Value = strTipoDocumentoId;
                    }
                    else
                    {
                        HF_vDescTipDoc.Value = "0";
                    }
                    HF_vNroDocumento.Value = dt.Rows[0]["vNroDocumento"].ToString();

                    //ddl_peid_sDocumentoTipoId.SelectedValue = dt.Rows[0]["sDocumentoTipoId"].ToString();

                    buscarValorTipoDocumento(ref ddl_peid_sDocumentoTipoId, dt.Rows[0]["sDocumentoTipoId"].ToString());
                    
                    txt_peid_vDocumentoNumero.Text = dt.Rows[0]["vNroDocumento"].ToString();

                    RE_PERSONA lPersona = new RE_PERSONA();
                    RE_PERSONAIDENTIFICACION lPersonaIdentificacion = new RE_PERSONAIDENTIFICACION();
                    lPersonaIdentificacion.peid_iPersonaId = LonPersonaId;
                    lPersonaIdentificacion.peid_sDocumentoTipoId = Convert.ToInt16(dt.Rows[0]["sDocumentoTipoId"].ToString());
                    lPersonaIdentificacion.peid_vDocumentoNumero = dt.Rows[0]["vNroDocumento"].ToString();

                    PersonaConsultaBL lPersonaConsultaBL = new PersonaConsultaBL();
                    lPersona.pers_iPersonaId = lPersonaIdentificacion.peid_iPersonaId;
                    lPersona.Identificacion = lPersonaIdentificacion;
                    lPersona = lPersonaConsultaBL.Obtener(lPersona);


                    txtDescOtroDocumento.Text = lPersona.Identificacion.peid_vTipodocumento;

                    if (lPersonaIdentificacion.peid_sDocumentoTipoId == 4)
                    {

                        tablaOtroDocumento.Style.Add("display", "block");
                        txtDescOtroDocumento.Enabled = true;
                    }
                    else
                    {
                        tablaOtroDocumento.Style.Add("display", "none");
                        txtDescOtroDocumento.Enabled = false;
                    }

                    ddl_pers_sNacionalidadId.SelectedValue = lPersona.pers_sNacionalidadId.ToString();

                    txt_pers_vApellidoPaterno.Text = lPersona.pers_vApellidoPaterno.ToString();
                    txt_pers_vApellidoMaterno.Text = lPersona.pers_vApellidoMaterno.ToString();
                    txt_pers_vApellidoCasada.Text = lPersona.pers_vApellidoCasada.ToString();
                    txt_pers_vNombres.Text = lPersona.pers_vNombres.ToString();

                    ddl_pers_sGeneroId.SelectedValue = lPersona.pers_sGeneroId.ToString();
                    ddlPaisOrigen.SelectedValue = lPersona.pers_sPaisId.ToString();
                    LblDescNacionalidadCopia.Text = lPersona.vNacionalidad.ToString();

                    ddl_pers_sEstadoCivilId.SelectedValue = lPersona.pers_sEstadoCivilId.ToString();
                    ddl_pers_sProfesionId.SelectedValue = lPersona.pers_sOcupacionId.ToString();

                    ddl_pers_sIdiomaNatalId.SelectedValue = lPersona.pers_sIdiomaNatalId.ToString();
                   
                    if (lPersona.Residencias[0].Residencia.resi_cResidenciaUbigeo != null)
                    {
                        if (lPersona.Residencias[0].Residencia.resi_iResidenciaId != 0)
                            hdn_acpa_residenciaId.Value = lPersona.Residencias[0].Residencia.resi_iResidenciaId.ToString();

                        string strUbigeo = lPersona.Residencias[0].Residencia.resi_cResidenciaUbigeo.ToString();
                        if (strUbigeo.Trim().Length == 6)
                        {
                            string ubi01 = strUbigeo.Substring(0, 2);
                            string ubi02 = strUbigeo.Substring(2, 2);
                            string ubi03 = strUbigeo.Substring(4, 2);
                            
                            hubigeo.Value = ubi01 + ubi02 + ubi03;
                            hubigeoLoad.Value = ubi01 + ubi02 + ubi03;
                            hdn_acpa_pais.Value = ubi01;
                            hdn_acpa_provincia.Value = ubi02;
                            hdn_acpa_distrito.Value = ubi03;
                            this.ddl_UbigeoPais.SelectedValue = ubi01;
                            actualizarUbigeo();
                        }
                    }
                    if (lPersona.Residencias[0].Residencia.resi_vResidenciaDireccion != null)
                    {
                        txt_resi_vResidenciaDireccion.Text = lPersona.Residencias[0].Residencia.resi_vResidenciaDireccion.ToString();
                    }
                    else
                    {
                        txt_resi_vResidenciaDireccion.Text = "";
                    }
                    if (lPersona.Residencias[0].Residencia.resi_vCodigoPostal != null)
                    {
                        txt_resi_vCodigoPostal.Text = lPersona.Residencias[0].Residencia.resi_vCodigoPostal.ToString();
                    }
                    else
                    {
                        txt_resi_vCodigoPostal.Text = "";
                    }
                                       
                    //----------------------------------------------------------------------
                }

                dt = null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      

        private void CargarUltimoInsumo()
        {
            DataTable _dt = new DataTable();
            InsumoConsultaBL _obj = new InsumoConsultaBL();
            _dt = _obj.ConsultarUltimoInsumoUsuario(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]));
            if (_dt.Rows.Count > 0)
            {
                if (txtCodigoInsumo.Text.Length == 0)
                {
                    txtCodigoInsumo.Text = _dt.Rows[0]["INSUMO"].ToString();
                }
            }

        }
        //--------------------------------------------------------------------------------------------------------------------------------
        //private List<participantes> obtenerArregloParticipantes(long intActoNotarialId)
        //{
        //    List<participantes> ListParticipantes = new List<participantes>();
        //    participantes par;

        //    ActoNotarialConsultaBL bl = new ActoNotarialConsultaBL();
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        dt = bl.ActonotarialObtenerParticipantes(intActoNotarialId);

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            par = new participantes();
        //            par.vEmpresa = dt.Rows[i]["vEmpresa"].ToString();
        //            par.pers_iPersonaId = Convert.ToInt64(dt.Rows[i]["pers_iPersonaId"].ToString());
        //            par.vTipoParticipanteId = dt.Rows[i]["vTipoParticipanteId"].ToString();
        //            par.vDescTipDoc = dt.Rows[i]["vDescTipDoc"].ToString();
        //            par.vDescLargaTipDoc = dt.Rows[i]["vDescLargaTipDoc"].ToString();
        //            par.vNroDocumento = dt.Rows[i]["vNroDocumento"].ToString();
        //            par.pers_sNacionalidadId = Convert.ToInt16(dt.Rows[i]["pers_sNacionalidadId"].ToString());
        //            par.vNacionalidad = dt.Rows[i]["vNacionalidad"].ToString();
        //            par.pers_sEstadoCivilId = Convert.ToInt16(dt.Rows[i]["pers_sEstadoCivilId"].ToString());
        //            par.vEstadoCivil = dt.Rows[i]["vEstadoCivil"].ToString();
        //            par.vDireccion = dt.Rows[i]["vDireccion"].ToString();
        //            par.cResidenciaUbigeo = dt.Rows[i]["cResidenciaUbigeo"].ToString();
        //            par.DptoCont = dt.Rows[i]["DptoCont"].ToString();
        //            par.ProvPais = dt.Rows[i]["ProvPais"].ToString();
        //            par.DistCiu = dt.Rows[i]["DistCiu"].ToString();
        //            par.vOcupacion = dt.Rows[i]["vOcupacion"].ToString();
        //            par.vProfesion = dt.Rows[i]["vProfesion"].ToString();
        //            par.vTipoDocumento = dt.Rows[i]["vTipoDocumento"].ToString();
        //            par.iGenero = Convert.ToInt16(dt.Rows[i]["iGenero"].ToString());
        //            par.pers_bIncapacidadFlag = Convert.ToBoolean(dt.Rows[i]["pers_bIncapacidadFlag"].ToString());
        //            par.pers_vDescripcionIncapacidad = dt.Rows[i]["pers_vDescripcionIncapacidad"].ToString();
        //            par.anpa_iReferenciaParticipanteId = Convert.ToInt64(dt.Rows[i]["anpa_iReferenciaParticipanteId"].ToString());
        //            par.anpa_iActoNotarialParticpanteId = Convert.ToInt64(dt.Rows[i]["anpa_iActoNotarialParticpanteId"].ToString());
        //            par.anpa_sTipoParticipanteId = Convert.ToInt16(dt.Rows[i]["anpa_sTipoParticipanteId"].ToString());
        //            par.pers_sIdiomaNatalId = Convert.ToInt16(dt.Rows[i]["pers_sIdiomaNatalId"].ToString());
        //            par.vIdioma = dt.Rows[i]["vIdioma"].ToString();
        //            par.vGentilicio = dt.Rows[i]["vGentilicio"].ToString();
        //            par.peid_sDocumentoTipoId = Convert.ToInt16(dt.Rows[i]["peid_sDocumentoTipoId"].ToString());
        //            par.peid_vTipoDocumento = dt.Rows[i]["peid_vTipoDocumento"].ToString();
        //            par.acno_sTipoActoNotarialId = Convert.ToInt16(dt.Rows[i]["acno_sTipoActoNotarialId"].ToString());
        //            par.acno_sSubTipoActoNotarialId = Convert.ToInt16(dt.Rows[i]["acno_sSubTipoActoNotarialId"].ToString());
        //            par.vNumeroEscrituraPublica = dt.Rows[i]["vNumeroEscrituraPublica"].ToString();
        //            par.vNumeroPartida = dt.Rows[i]["vNumeroPartida"].ToString();
        //            if (dt.Rows[i]["anpa_dFechaSuscripcion"] != null)
        //            {
        //                par.anpa_dFechaSuscripcion = Convert.ToDateTime(dt.Rows[i]["anpa_dFechaSuscripcion"].ToString());
        //            }
        //            par.vNacionalidadPais = dt.Rows[i]["vNacionalidadPais"].ToString();
        //            par.anpa_bFlagFirma = Convert.ToBoolean(dt.Rows[i]["vNacionalidadPais"].ToString());
        //            par.anpa_bFlagHuella = Convert.ToBoolean(dt.Rows[i]["anpa_bFlagHuella"].ToString());
        //            ListParticipantes.Add(par);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return ListParticipantes;
        //}
        //--------------------------------------------------------------------------------------------------------------------------------

        //----------------------------------------------------------
        //Fe

        void ctrlReimprimirbtn_btnReimprimirHandler()
        {
            //----------------------------------------------
            //Fecha: 19/08/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Habilitar o Deshabilitar los botones
            //          de reimpresión y autoadhesivo.
            //----------------------------------------------
            if (ctrlReimprimirbtn1.SeImprime == "OK")
            {
                if (ctrlReimprimirbtn1.Activar)
                {
                    btnAutoadhesivo.Enabled = true;
                    ctrlReimprimirbtn1.Activar = false;
                }
                else
                {
                    btnAutoadhesivo.Enabled = false;
                    ctrlReimprimirbtn1.Activar = true;
                }
            }
            else
            {
                btnAutoadhesivo.Enabled = true;
                ctrlReimprimirbtn1.Activar = false;
            }
            updFormato.Update();
            //----------------------------------------------
        }

        void ctrlBajaAutoadhesivo_btnAnularAutoahesivo()
        {
            ctrlBajaAutoadhesivo1.CodInsumo = hCodAutoadhesivo.Value;
            
            Comun.EjecutarScript(this, "Popup(" + hCodAutoadhesivo.Value.ToString() + ");");
        }

        void ctrlBajaAutoadhesivo_btnAceptarAnularAutoahesivo()
        {
            Session["tab_activa_vinculacion"] = "SI";
            ctrlBajaAutoadhesivo1.CodInsumo = hCodAutoadhesivo.Value;

            RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();
            ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();

            lACTONOTARIAL.acno_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
            lACTONOTARIAL = lActoNotarialConsultaBL.obtener(lACTONOTARIAL);

            if (lACTONOTARIAL.acno_sEstadoId == (Int16)Enumerador.enmNotarialProtocolarEstado.VINCULADA)
            {
                ActoNotarialMantenimiento bl = new ActoNotarialMantenimiento();
                RE_ACTONOTARIAL actonotarialBE = new RE_ACTONOTARIAL();
                actonotarialBE.acno_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
                actonotarialBE.acno_sEstadoId = (int)Enumerador.enmNotarialProtocolarEstado.PAGADA;
                actonotarialBE.acno_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                actonotarialBE.acno_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                actonotarialBE.acno_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();

                if (bl.ActoNotarialActualizarEstado(actonotarialBE) == false)
                {                    

                }
            }

            string codPersona = Request.QueryString["CodPer"].ToString();
            //Session[Constantes.CONST_SESION_ACTONOTARIAL_ID] = ViewState[Constantes.CONST_SESION_ACTONOTARIAL_ID];
            //Session[Constantes.CONST_SESION_ACTUACION_ID] = ViewState[Constantes.CONST_SESION_ACTUACION_ID];

            if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
            {
                Response.Redirect("FrmActoNotarialProtocolares.aspx?cod=0&CodPer=" + codPersona + "&Juridica=1", false);
            }
            else
            { // PERSONA NATURAL
                string codTipoDocEncriptada = "";
                string codNroDocumentoEncriptada = "";
                string AccionOperacion = "1056";

                if (Request.QueryString["CodTipoDoc"] != null && Request.QueryString["codNroDoc"] != null)
                {
                    codTipoDocEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString());
                    codNroDocumentoEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["codNroDoc"].ToString());
                }
                if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                {
                    Response.Redirect("FrmActoNotarialProtocolares.aspx?class=" + AccionOperacion + "&cod=0&CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                }
                else
                {
                    Response.Redirect("FrmActoNotarialProtocolares.aspx?class=" + AccionOperacion + "&cod=0&CodPer=" + codPersona, false);
                }
            }

            txtCodigoInsumo.Focus();
        }

        //---------------------------------------------------------------
        //Fecha: 30/07/2020
        //Autor: Miguel Márquez Beltrán
        //Motivo: LLenar la lista desplegable con los participantes 
        //          que tengan discapacidad.
        //---------------------------------------------------------------
        private void llenarParticipanteDiscapacidad()
        {
            if (ddl_anpa_sTipoParticipanteId.SelectedValue == Convert.ToString((int)Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO))
            {
                ddl_Participante_Discapacidad.Items.Clear();
                ListItem lListItem0 = new ListItem("-- SELECCIONAR --", "0");
                ddl_Participante_Discapacidad.Items.Add(lListItem0);

                lblParticipanteDiscapacidad.Visible = true;
                ddl_Participante_Discapacidad.Visible = true;

                foreach (GridViewRow row in grd_Participantes.Rows)
                {
                    string str_Participante = row.Cells[0].Text.ToString();
                    string Participante = row.Cells[4].Text.ToString();
                    if (row.Cells[11].Text.Length > 0)
                    {
                        Boolean str_TieneReferencia = Convert.ToBoolean(row.Cells[11].Text.ToString());
                        Int64 Aux = Convert.ToInt64(row.Cells[12].Text.ToString());
                        if (str_TieneReferencia == true)
                        {
                            //if (str_Participante == "OTORGANTE" || str_Participante == "VENDEDOR" || str_Participante == "ANTICIPANTE" || str_Participante == "DONANTE")

                            if (ObtenerIniciaRecibe(str_Participante) == "INICIA")
                            {
                                ListItem lListItem = new ListItem(Participante, Convert.ToString(Aux));

                                ddl_Participante_Discapacidad.Items.Add(lListItem);
                                HF_IREFERENCIAPARTICIPANTE.Value = Convert.ToString(Aux);
                            }
                        }
                    }
                }
                updDatosParticipantes.Update();
            }
            
        }
        //----------------------------------------------------------------

        //---------------------------------------------------------------
        //Fecha: 31/07/2020
        //Autor: Miguel Márquez Beltrán
        //Motivo: Verificar si existe el participante.
        //---------------------------------------------------------------
        private bool ExisteParticipante(DataTable dt, string strDocumentoTipoId, string strDocumentoNumero)
        {
            bool bExiste = false;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["peid_sDocumentoTipoId"].ToString() == strDocumentoTipoId && dt.Rows[i]["peid_vDocumentoNumero"].ToString() == strDocumentoNumero)
                {
                    bExiste = true;
                    break;
                }
            }
            return bExiste;
        }
        //-------------------------------------------
        //Fecha: 15/03/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Consultar el gentilicio
        //-------------------------------------------
       // protected void ddlPaisOrigen_SelectedIndexChanged(object sender, EventArgs e)
       // {
        //    DataTable dtPaises = new DataTable();

        //    dtPaises = (DataTable)ViewState["Paises"];

        //    //---------------------------------------------------
        //    //Fecha: 07/07/2020
        //    //Autor: Miguel Márquez Beltrán
        //    //Motivo: Se asigna la nacionalidad.
        //    //---------------------------------------------------
        //    LblDescNacionalidad.Text = "";

        //    for (int i = 0; i < dtPaises.Rows.Count; i++)
        //    {
        //        if (dtPaises.Rows[i]["PAIS_SPAISID"].ToString() == ddlPaisOrigen.SelectedValue)
        //        {
        //            LblDescNacionalidad.Text = dtPaises.Rows[i]["PAIS_VNACIONALIDAD"].ToString();
        //            break;
        //        }
        //    }
        //    //---------------------------------------------------
        //    //--------------------------------------
        //    //Fecha: 07/07/2020
        //    //Autor: Miguel Márquez Beltrán
        //    //Motivo: Ocultar Gentilicio
        //    //--------------------------------------
        //    //LblDescGentilicio.Text = Comun.AsignarGentilicio(dtPaises, ddlPaisOrigen, ddl_pers_sGeneroId).ToUpper();
        //    Comun.AsignarNacionalidad(Session, ddl_pers_sNacionalidadId, ddlPaisOrigen);
        //    string strPaisPeruId = System.Web.Configuration.WebConfigurationManager.AppSettings["Pais_PeruId"].ToString();
        //    if (ddlPaisOrigen.SelectedValue.Equals(strPaisPeruId))
        //    {
        //        ddl_pers_sNacionalidadId.SelectedValue = Convert.ToString((int)Enumerador.enmNacionalidad.PERUANA);
        //    }
        //    else
        //    {
        //        ddl_pers_sNacionalidadId.SelectedValue = Convert.ToString((int)Enumerador.enmNacionalidad.EXTRANJERA);
        //    }
        //    ActualizarGenero();
        //    actualizarUbigeo();
            
        //    MostrarOcultarIncapacidad();
        //    MostrarOcultarInterprete();
        //    //------------------------------------------------
        //    //Fecha: 24/07/2020
        //    //Autor: Miguel Márquez Beltrán
        //    //Motivo: Validar la presencia del check de huella
        //    //------------------------------------------------
        //    MostrarOcultarHuella();
        //    //---------------------------------------------------------------
        //    //Fecha: 30/07/2020
        //    //Autor: Miguel Márquez Beltrán
        //    //Motivo: LLenar la lista desplegable con los participantes 
        //    //          que tengan discapacidad.
        //    //---------------------------------------------------------------
        //    llenarParticipanteDiscapacidad();
        //    updDatosParticipantes.Update();

     //   }

        //------------------------------------------------
        //Fecha: 03/08/2020
        //Autor: Miguel Márquez Beltrán
        //Motivo: Actualizar el Ubigeo de la dirección.
        //------------------------------------------------
        private void actualizarUbigeo()
        {
                string strUbi01 = hdn_acpa_pais.Value;
                comun_Part3.CargarUbigeo(Session, this.ddl_UbigeoRegion, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, strUbi01, string.Empty, true);
                if (hdn_acpa_provincia.Value.Length > 0)
                {
                    string strUbi02 = hdn_acpa_provincia.Value;

                    //-----------------------------------------------
                    //Fecha: 27/07/2020
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Buscar el Id de la provincia.
                    //-----------------------------------------------
                    bool bExisteProvincia = false;
                    for (int i = 0; i < ddl_UbigeoRegion.Items.Count; i++)
                    {
                        if (ddl_UbigeoRegion.Items[i].Value == strUbi02)
                        {
                            bExisteProvincia = true;
                            break;
                        }
                    }
                    if (bExisteProvincia == true)
                    {
                        this.ddl_UbigeoRegion.SelectedValue = strUbi02;

                        comun_Part3.CargarUbigeo(Session, this.ddl_UbigeoCiudad, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, strUbi01, strUbi02, true);
                        if (hdn_acpa_distrito.Value.Length > 0)
                        {
                            string strUbi03 = hdn_acpa_distrito.Value;
                            //-----------------------------------------------
                            //Fecha: 27/07/2020
                            //Autor: Miguel Márquez Beltrán
                            //Motivo: Buscar el Id del distrito.
                            //-----------------------------------------------
                            bool bExisteDistrito = false;
                            for (int i = 0; i < ddl_UbigeoCiudad.Items.Count; i++)
                            {
                                if (ddl_UbigeoCiudad.Items[i].Value == strUbi03)
                                {
                                    bExisteDistrito = true;
                                    break;
                                }
                            }
                            if (bExisteDistrito == true)
                            {
                                ddl_UbigeoCiudad.SelectedValue = strUbi03;
                            }
                            else
                            {
                                ddl_UbigeoCiudad.SelectedIndex = 0;
                            }
                        }
                    }
                    else
                    {
                        ddl_UbigeoRegion.SelectedIndex = 0;
                        ddl_UbigeoCiudad.SelectedIndex = 0;
                    }
                }
        }
        //------------------------------------------------------
        //Fecha: 04/08/2020
        //Autor: Miguel Márquez Beltrán
        //Motivo: Obtener la Nacionalidad con AJAX.
        //------------------------------------------------------
        [System.Web.Services.WebMethod]
        public static String obtenerNacionalidad(String strPaisId)
        {
            try
            {
                Int16 iPaisId = Convert.ToInt16(strPaisId);
                DataTable dtPaises = new DataTable();
                dtPaises = Comun.ConsultarPaises();
                string strNacionalidad = "";

                for (int i = 0; i < dtPaises.Rows.Count; i++)
                {
                    if (dtPaises.Rows[i]["PAIS_SPAISID"].ToString() == iPaisId.ToString())
                    {
                        if (dtPaises.Rows[i]["pais_vnacionalidad"] != null)
                        {
                            strNacionalidad = dtPaises.Rows[i]["pais_vnacionalidad"].ToString();
                        }
                        break;
                    }
                }
                strNacionalidad = new JavaScriptSerializer().Serialize(strNacionalidad);
                return strNacionalidad;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        //===================================================================
        //Fecha: 20/08/2020
        //Autor: Miguel Márquez Beltrán
        //Motivo: Adicionar Participantes directamente en la base de datos.
        //===================================================================

        private bool AdicionarParticipante(CBE_PARTICIPANTE lParticipante)
        {
            bool bAdiciono = false;
            String sMensaje = String.Empty;
            ActoNotarialMantenimiento mnt = new ActoNotarialMantenimiento();

            List<CBE_PARTICIPANTE> ParticipantesContainerInsertar = new List<CBE_PARTICIPANTE>();

            if (lParticipante.Persona.pers_iPersonaId == 0)
                {
                    lParticipante.Persona.pers_sUsuarioCreacion = lParticipante.anpa_sUsuarioCreacion;
                    lParticipante.Persona.pers_vIPCreacion = lParticipante.anpa_vIPCreacion;
                    lParticipante.Persona.HostName = "";
                }

            lParticipante.anpa_vIPCreacion = lParticipante.anpa_vIPCreacion;
            lParticipante.anpa_sUsuarioCreacion = lParticipante.anpa_sUsuarioCreacion;
            lParticipante.anpa_dFechaCreacion = DateTime.Now;
            lParticipante.anpa_dFechaModificacion = DateTime.Now;
            lParticipante.anpa_iEmpresaId = lParticipante.Empresa.empr_iEmpresaId;
            lParticipante.OficinaConsultar = lParticipante.OficinaConsultar;

            ParticipantesContainerInsertar.Add(lParticipante);

            if (ParticipantesContainerInsertar.Count > 0)
            {
                mnt.InsertarActoNotarialParticipante_Persona(ParticipantesContainerInsertar, ref sMensaje);
            }
            if (sMensaje != String.Empty)
                bAdiciono = true;

            return bAdiciono;
        }
        //===================================================================
        //Fecha: 20/08/2020
        //Autor: Miguel Márquez Beltrán
        //Motivo: Actualizar Participantes directamente en la base de datos.
        //===================================================================
        private bool ActualizarParticipante(CBE_PARTICIPANTE lParticipante)
        {
            bool bActualizo = false;
            String sMensaje = String.Empty;
            ActoNotarialMantenimiento mnt = new ActoNotarialMantenimiento();

            List<CBE_PARTICIPANTE> ParticipantesContainerAcualizar = new List<CBE_PARTICIPANTE>();

            if (lParticipante.Persona.pers_iPersonaId == 0)
            {
                lParticipante.Persona.pers_sUsuarioCreacion = lParticipante.anpa_sUsuarioCreacion;
                lParticipante.Persona.pers_vIPCreacion = lParticipante.anpa_vIPCreacion;
                lParticipante.Persona.HostName = "";
            }

            lParticipante.anpa_vIPCreacion = lParticipante.anpa_vIPCreacion;
            lParticipante.anpa_sUsuarioCreacion = lParticipante.anpa_sUsuarioCreacion;
            lParticipante.anpa_dFechaCreacion = DateTime.Now;
            lParticipante.anpa_dFechaModificacion = DateTime.Now;
            lParticipante.anpa_iEmpresaId = lParticipante.Empresa.empr_iEmpresaId;
            lParticipante.OficinaConsultar = lParticipante.OficinaConsultar;
            lParticipante.anpa_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            lParticipante.anpa_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();

            ParticipantesContainerAcualizar.Add(lParticipante);

            if (ParticipantesContainerAcualizar.Count > 0)
            {
                mnt.ActualizarActoNotarialParticipante(ParticipantesContainerAcualizar, ref sMensaje);
            }
            if (sMensaje != String.Empty)
                bActualizo = true;
            return bActualizo;
        }
        //===================================================================
        
        //Fecha: 20/08/2020
        //Autor: Miguel Márquez Beltrán
        //Motivo: Validar todos los Participantes.
        //===================================================================
        private bool ValidarTotalParticipantes()
        {
            bool bCorrecto = true;
            String strScript = String.Empty;

            String TipoActaProtocar = Session["TipoActo"].ToString();

            
            List<CBE_PARTICIPANTE> lParticipantes = (List<CBE_PARTICIPANTE>)HttpContext.Current.Session["ParticipanteContainer"];

            if (lParticipantes.Count == 0)
            {
                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO PROTOCOLAR", "Debe registrar a los participantes.");
                Comun.EjecutarScript(Page, strScript);
                return false;
            }
            //if (lParticipantes.Where(x => (x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE) ||
            //    x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR) ||
            //    x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE) ||
            //    x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE))
            //    && x.anpa_cEstado != "E").Count() == 0)

            if (lParticipantes.Where(x => (ObtenerIniciaRecibe(x.anpa_sTipoParticipanteId) == "INICIA")
                && x.anpa_cEstado != "E").Count() == 0)
            {

                //if (TipoActaProtocar == Convert.ToString(Convert.ToInt32(Enumerador.enmProtocolarTipo.COMPRA_VENTA)))
                //{
                //    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO PROTOCOLAR", "Debe existir al menos un tipo de participante VENDEDOR.");
                //    Comun.EjecutarScript(Page, strScript);
                //    return false;
                //}
                //if (TipoActaProtocar == Convert.ToString(Convert.ToInt32(Enumerador.enmProtocolarTipo.ANTICIPO_HERENCIA)))
                //{
                //    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO PROTOCOLAR", "Debe existir al menos un tipo de participante ANTICIPANTE.");
                //    Comun.EjecutarScript(Page, strScript);
                //    return false;
                //}

                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO PROTOCOLAR", "Debe existir al menos un tipo de participante que INICIA el Acto Protocolar.");
                Comun.EjecutarScript(Page, strScript);
                return false;

            }
            //if (lParticipantes.Where(x => x.Persona.pers_bIncapacidadFlag == true &&
            //        (x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE) ||
            //        x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR) ||
            //        x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE) ||
            //        x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE))
            //        && x.anpa_cEstado != "E").Count() >= 1)

            if (lParticipantes.Where(x => x.Persona.pers_bIncapacidadFlag == true &&
                    (ObtenerIniciaRecibe(x.anpa_sTipoParticipanteId) == "INICIA")
                    && x.anpa_cEstado != "E").Count() >= 1)
            {

                if (lParticipantes.Where(x => x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO) && x.anpa_cEstado != "E").Count() == 0)
                {
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO PROTOCOLAR", "Debe existir al menos un tipo de participante TESTIGO A RUEGO.");
                    Comun.EjecutarScript(Page, strScript);
                    return false;
                }
            }


            if (ValidarIdiomaCastellanoOtorgante(lParticipantes) == false)
            {
                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO PROTOCOLAR", "Debe existir al menos un tipo de participante INTERPRETE.");
                Comun.EjecutarScript(Page, strScript);
                return false;
            }
            //if (lParticipantes.Where(x => (x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.APODERADO) ||
            //    x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.COMPRADOR) ||
            //    x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.DONATARIO) ||
            //    x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPADO))
            //    && x.anpa_cEstado != "E").Count() == 0)

            if (lParticipantes.Where(x => (ObtenerIniciaRecibe(x.anpa_sTipoParticipanteId) == "RECIBE")
                && x.anpa_cEstado != "E").Count() == 0)
            {
                //if (TipoActaProtocar == Convert.ToString(Convert.ToInt32(Enumerador.enmProtocolarTipo.COMPRA_VENTA)))
                //{
                //    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO PROTOCOLAR", "Debe existir al menos un tipo de participante COMPRADOR.");
                //    Comun.EjecutarScript(Page, strScript);
                //    return false;
                //}

                //if (TipoActaProtocar == Convert.ToString(Convert.ToInt32(Enumerador.enmProtocolarTipo.ANTICIPO_HERENCIA)))
                //{
                //    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO PROTOCOLAR", "Debe existir al menos un tipo de participante ANTICIPANTE.");
                //    Comun.EjecutarScript(Page, strScript);
                //    return false;
                //}

                //if (Cmb_TipoActoNotarial.SelectedItem.Text.Trim().ToUpper() == "DONACIÓN")
                //{
                //    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO PROTOCOLAR", "Debe existir al menos un tipo de participante DONATARIO.");
                //    Comun.EjecutarScript(Page, strScript);
                //    return false;
                //}
                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO PROTOCOLAR", "Debe existir al menos un tipo de participante que RECIBE en el Acto Protocolar");
                Comun.EjecutarScript(Page, strScript);
                return false;

                //-------------------------------------------------------------------------------
                //Fecha: 06/10/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: "RENUNCIA A LA NACIONALIDAD PERUANA" no debe solicitar apoderado.
                //-------------------------------------------------------------------------------
                if (Cmb_TipoActoNotarial.SelectedItem.Text.Trim().ToUpper() != "RENUNCIA A LA NACIONALIDAD PERUANA")
                {

                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO PROTOCOLAR", "Debe existir al menos un tipo de participante APODERADO.");
                    Comun.EjecutarScript(Page, strScript);
                    return false;
                }
            }
            
            //------------------------------------
            //Validar Interprete
            //------------------------------------
            if (lParticipantes.Where(x => (x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE))
                && x.anpa_cEstado != "E").Count() == 0)
            {                
                //if (TipoActaProtocar == Convert.ToString((int)Enumerador.enmProtocolarTipo.COMPRA_VENTA))
                //{
                //    if (lParticipantes.Where(x => x.pers_sIdiomaNatalId_desc != "CASTELLANO" && x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR) && x.anpa_cEstado != "E").Count() > 0)
                //    {
                //        strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO PROTOCOLAR", "No se ha registrado un Vendedor que requiera un Interprete.");
                //        Comun.EjecutarScript(Page, strScript);
                //        return false;
                //    }
                        
                //}
                //else if (TipoActaProtocar == Convert.ToString((int)Enumerador.enmProtocolarTipo.ANTICIPO_HERENCIA))
                //{
                //    if (lParticipantes.Where(x => x.pers_sIdiomaNatalId_desc != "CASTELLANO" && x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE) && x.anpa_cEstado != "E").Count() > 0)
                //    {
                //        strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO PROTOCOLAR", "No se ha registrado un Anticipante que requiera un Interprete.");
                //        Comun.EjecutarScript(Page, strScript);
                //        return false;
                //    }                        
                //}
                //else
                //{
                //    if (lParticipantes.Where(x => x.pers_sIdiomaNatalId_desc != "CASTELLANO" && x.anpa_sTipoParticipanteId == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE) && x.anpa_cEstado != "E").Count() > 0)
                //    {
                //        strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO PROTOCOLAR", "No se ha registrado un Otorgante que requiera un Interprete.");
                //        Comun.EjecutarScript(Page, strScript);
                //        return false;
                //    }                        
                //}
                if (lParticipantes.Where(x => x.pers_sIdiomaNatalId_desc != "CASTELLANO" && ObtenerIniciaRecibe(x.anpa_sTipoParticipanteId) == "INICIA" && x.anpa_cEstado != "E").Count() > 0)
                    {
                        strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO PROTOCOLAR", "No se ha registrado un Participante que INICIA el Acto Protocolar que requiera un Interprete.");
                        Comun.EjecutarScript(Page, strScript);
                        return false;
                    }
                //-------------------------------------------------                    
            }

            return bCorrecto;
        }
        //=======================================================
        //Fecha: 20/08/2020
        //Autor: Miguel Márquez Beltrán
        //Motivo: Adicionar o Modificar Participantes directamente
        //        en la base de datos.
        //=======================================================
        protected void btnAgregarParticipanteNew_Click(object sender, EventArgs e)
        {
            //---------------------------------------------------------------------
            //Fecha: 17/12/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Verificar que exista solo un participante otorgante.
            //---------------------------------------------------------------------
            string strTipoActoProtocolar = Cmb_TipoActoNotarial.SelectedItem.Text.Trim().ToUpper();

            if (strTipoActoProtocolar == "RENUNCIA A LA NACIONALIDAD PERUANA")
            {
                if (grd_Participantes.Rows.Count > 0)
                {
                    EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "Solo se permite un participante Otorgante."), "ExisteParticipante_Otorgante");
                    return;
                }
            }
            
            string strPaisPeruId = ConfigurationManager.AppSettings["Pais_PeruId"].ToString();

            string vNumeroDocumento = txt_peid_vDocumentoNumero.Text.ToUpper();
            string vTipoDocumento = ddl_peid_sDocumentoTipoId.Text.ToUpper();

            CBE_PARTICIPANTE lParticipante = new CBE_PARTICIPANTE();
            RE_EMPRESA lEmpresa = new RE_EMPRESA();
            RE_PERSONA lPersona = new RE_PERSONA();

            RE_REPRESENTANTELEGAL lRepresentanteLegal = new RE_REPRESENTANTELEGAL();
            RE_PERSONAIDENTIFICACION lPersonaIdentificacion = new RE_PERSONAIDENTIFICACION();
            RE_ACTONOTARIALPARTICIPANTE lActoNotarialParticipante = new RE_ACTONOTARIALPARTICIPANTE();

            if (HF_REGISTRO_NUEVO.Value == "-1")
            {
                ViewState["ExtraIndiceEdicion"] = -1;
            }
            int iIndiceValidacion = Convert.ToInt32(ViewState["ExtraIndiceEdicion"]);
            bool bEdicion = iIndiceValidacion >= 0 ? true : false;

            Session["TipoActo"] = Cmb_TipoActoNotarial.SelectedValue;


            if (iIndiceValidacion >= 0)
            {
                hdn_IndiceGrillaParticipantesEdicion.Value = "1";
            }
            else
            {
                hdn_IndiceGrillaParticipantesEdicion.Value = "0";               
            }

            //---------------------------------------------------------------------
            //Fecha: 13/04/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Verificar siempre que no exista duplicidad de participantes.
            //---------------------------------------------------------------------
            if (hdn_IndiceGrillaParticipantesEdicion.Value == "0")
            {
                for (int i = 0; i < grd_Participantes.Rows.Count; i++)
                {
                    if (grd_Participantes.Rows[i].Cells[2].Text == vTipoDocumento &&
                        grd_Participantes.Rows[i].Cells[3].Text == vNumeroDocumento)
                    {
                        EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "Ya existe el participante ingresado."), "ExisteParticipante1");
                        return;
                    }
                }
            }

            #region Identificación
            PersonaIdentificacionConsultaBL lPersonaIdentificacionConsultaBL = new PersonaIdentificacionConsultaBL();
            lPersonaIdentificacion.peid_sDocumentoTipoId = Convert.ToInt16(ddl_peid_sDocumentoTipoId.SelectedValue);
            lPersonaIdentificacion.peid_vDocumentoNumero = txt_peid_vDocumentoNumero.Text.ToUpper();
            lPersonaIdentificacion.peid_vTipodocumento = this.txtDescOtroDocumento.Text.ToString().ToUpper();
            lPersonaIdentificacion = lPersonaIdentificacionConsultaBL.Obtener(lPersonaIdentificacion);

            lParticipante.Identificacion = lPersonaIdentificacion;
            #endregion

            #region Verificar si existe Persona
            if (lPersonaIdentificacion.peid_iPersonaId != 0)
            {
                #region Persona Existe

                PersonaConsultaBL lPersonaConsultaBL = new PersonaConsultaBL();
                lPersona.pers_iPersonaId = lPersonaIdentificacion.peid_iPersonaId;
                lPersona.Identificacion = lPersonaIdentificacion;
                lParticipante.Persona = lPersonaConsultaBL.Obtener(lPersona); //Recibiendo Persona ...

                if (chkIncapacitado.Checked)
                {
                    lPersona.pers_bIncapacidadFlag = true;
                    lPersona.pers_vDescripcionIncapacidad = txtRegistroTipoIncapacidad.Text.ToUpper();

                    lParticipante.Persona.pers_bIncapacidadFlag = true;
                    lParticipante.Persona.pers_vDescripcionIncapacidad = txtRegistroTipoIncapacidad.Text.ToUpper();
                    lParticipante.anpa_bFlagHuella = chkNoHuella.Checked;
                    lParticipante.anpa_bFlagFirma = true; // No Firma
                }
                else
                {
                    lPersona.pers_bIncapacidadFlag = false;
                    lPersona.pers_vDescripcionIncapacidad = String.Empty;

                    lParticipante.Persona.pers_bIncapacidadFlag = false;
                    lParticipante.Persona.pers_vDescripcionIncapacidad = String.Empty;

                    lParticipante.anpa_bFlagHuella = false; // Si Huella
                    lParticipante.anpa_bFlagFirma = false; // Si Firma
                }
                #endregion
            }
            else
            {
                #region Persona No Existe

                lPersona.pers_sPersonaTipoId = (Int16)Enumerador.enmTipoPersona.NATURAL;

                //---------------------------------------------------------------
                //Fecha: 16/03/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Asignar la nacionalidad de acuerdo al País de Origen
                //---------------------------------------------------------------                

                if (ddlPaisOrigen.SelectedValue.Equals(strPaisPeruId))
                {
                    ddl_pers_sNacionalidadId.SelectedValue = Convert.ToString((int)Enumerador.enmNacionalidad.PERUANA);
                }
                else
                {
                    ddl_pers_sNacionalidadId.SelectedValue = Convert.ToString((int)Enumerador.enmNacionalidad.EXTRANJERA);
                }
                //---------------------------------------------------------------

                lPersona.pers_sNacionalidadId = Convert.ToInt16(ddl_pers_sNacionalidadId.SelectedValue);
                lPersona.pers_vApellidoPaterno = txt_pers_vApellidoPaterno.Text.ToUpper().Trim();
                lPersona.pers_vApellidoMaterno = txt_pers_vApellidoMaterno.Text.ToUpper().Trim();
                lPersona.pers_vNombres = txt_pers_vNombres.Text.ToUpper().Trim();
                //-------------------------------------------------------------
                //Fecha: 05/01/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Se adiciona el apellido de casada.
                //-------------------------------------------------------------
                lPersona.pers_vApellidoCasada = txt_pers_vApellidoCasada.Text.ToUpper().Trim();
                //-------------------------------------------------------------
                if (this.ddl_pers_sGeneroId.SelectedIndex > 0)
                {
                    lPersona.pers_sGeneroId = Convert.ToInt16(ddl_pers_sGeneroId.SelectedValue);
                }

                //----------------------------------------------------
                //Fecha: 15/03/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Asignar el pais origen
                //----------------------------------------------------
                if (this.ddlPaisOrigen.SelectedIndex > 0)
                {
                    lPersona.pers_sPaisId = Convert.ToInt16(ddlPaisOrigen.SelectedValue);
                }

                if (this.ddl_pers_sEstadoCivilId.SelectedIndex > 0)
                {
                    lPersona.pers_sEstadoCivilId = Convert.ToInt16(ddl_pers_sEstadoCivilId.SelectedValue);
                }

                if (this.ddl_pers_sProfesionId.SelectedIndex > 0)
                {
                    lPersona.pers_sOcupacionId = Convert.ToInt16(ddl_pers_sProfesionId.SelectedValue);
                }

                if (this.ddl_pers_sIdiomaNatalId.SelectedIndex > 0)
                {
                    lPersona.pers_sIdiomaNatalId = Convert.ToInt16(ddl_pers_sIdiomaNatalId.SelectedValue);
                }


                if (chkIncapacitado.Checked)
                {
                    lPersona.pers_bIncapacidadFlag = true;// Es incapacitado
                    lPersona.pers_vDescripcionIncapacidad = txtRegistroTipoIncapacidad.Text.ToUpper();
                }
                else
                {
                    lPersona.pers_bIncapacidadFlag = false;
                    lPersona.pers_vDescripcionIncapacidad = String.Empty;
                }

                lPersona.Identificacion = lPersonaIdentificacion;
                lParticipante.Persona = lPersona;
                #endregion
            }
            #endregion

            #region Tentando al objeto participante
            lParticipante.anpa_iActoNotarialId = Convert.ToInt64(hdn_acno_iActoNotarialId.Value);
            lParticipante.anpa_iPersonaId = lPersona.pers_iPersonaId;
            lParticipante.anpa_sTipoParticipanteId = Convert.ToInt16(ddl_anpa_sTipoParticipanteId.SelectedValue);
            lParticipante.anpa_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();

            if (ddl_anpa_sTipoParticipanteId.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO).ToString()) // Testigo a Ruego
            {
                lParticipante.anpa_vCampoAuxiliar = HF_IREFERENCIAPARTICIPANTE.Value;
                lParticipante.anpa_iReferenciaParticipanteId = Convert.ToInt64(HF_IREFERENCIAPARTICIPANTE.Value);
            }
            //-------------------------------------------------------
            //Fecha: 22/04/2020
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Asignar el participante Interprete.
            //-------------------------------------------------------
            if (ddl_anpa_sTipoParticipanteId.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE).ToString()) // Testigo a Ruego
            {
                if (HF_IREFERENCIAINTERPRETE.Value != "0")
                {
                    lParticipante.anpa_vCampoAuxiliar = HF_IREFERENCIAINTERPRETE.Value;
                    lParticipante.anpa_iReferenciaParticipanteId = Convert.ToInt64(HF_IREFERENCIAINTERPRETE.Value);
                }
            }
            //-------------------------------------------------------
            Random rdm = new Random();
            Int64 Numeroalatorio = rdm.Next(1000000, 9999999);
            lParticipante.anpa_iActoNotarialParticipanteId = 0;
            lParticipante.anpa_iActoNotarialParticipanteIdAux = Numeroalatorio;
            lParticipante.anpa_sUsuarioCreacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            lParticipante.anpa_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
            lParticipante.anpa_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            lParticipante.anpa_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
            lParticipante.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            #endregion

            #region Creando el Objeto Persona para el Rune Rapido
            lParticipante.Persona.pers_sPersonaTipoId = (Int16)Enumerador.enmTipoPersona.NATURAL;
            lParticipante.Identificacion.peid_sDocumentoTipoId = Convert.ToInt16(this.ddl_peid_sDocumentoTipoId.SelectedValue);
            lParticipante.Identificacion.peid_vDocumentoNumero = this.txt_peid_vDocumentoNumero.Text.ToString().ToUpper();
            lParticipante.Identificacion.peid_vTipodocumento = this.txtDescOtroDocumento.Text.ToString().ToUpper();

            lParticipante.Persona.pers_sNacionalidadId = Convert.ToInt16(ddl_pers_sNacionalidadId.SelectedValue);
            //----------------------------------------------------
            //Fecha: 17/03/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Asignar el pais origen
            //----------------------------------------------------
            if (this.ddlPaisOrigen.SelectedIndex > 0)
            {
                lParticipante.Persona.pers_sPaisId = Convert.ToInt16(ddlPaisOrigen.SelectedValue);
            }


            if (ddlPaisOrigen.SelectedValue.Equals(strPaisPeruId))
            {
                lParticipante.Persona.pers_sNacionalidadId = (int)Enumerador.enmNacionalidad.PERUANA;
            }
            else
            {
                lParticipante.Persona.pers_sNacionalidadId = (int)Enumerador.enmNacionalidad.EXTRANJERA;
            }

            lParticipante.Persona.pers_vApellidoPaterno = txt_pers_vApellidoPaterno.Text.ToUpper().Trim();
            lParticipante.Persona.pers_vApellidoMaterno = txt_pers_vApellidoMaterno.Text.ToUpper().Trim();
            lParticipante.Persona.pers_vNombres = txt_pers_vNombres.Text.ToUpper().Trim();
            //-------------------------------------------------------------
            //Fecha: 05/01/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Se adiciona el apellido de casada.
            //-------------------------------------------------------------
            lParticipante.Persona.pers_vApellidoCasada = txt_pers_vApellidoCasada.Text.ToUpper().Trim();
            //---------------------------------------------------------------
            if (this.ddl_pers_sGeneroId.SelectedIndex > 0)
            {
                lParticipante.Persona.pers_sGeneroId = Convert.ToInt16(ddl_pers_sGeneroId.SelectedValue);
            }

            //----------------------------------------------------
            //Fecha: 15/03/2017
            //Autor: Miguel Márquez Beltrán
            //Motivo: Asignar el pais origen
            //----------------------------------------------------
            if (this.ddlPaisOrigen.SelectedIndex > 0)
            {
                lParticipante.Persona.pers_sPaisId = Convert.ToInt16(ddlPaisOrigen.SelectedValue);

                //------------------------------------------------------
                //Fecha: 07/07/2020
                //Autor: Miguel Márquez Beltrán
                //Motivo: Comentar la asignación del Gentilicio.
                //lParticipante.Persona.pers_vGentilicio = LblDescGentilicio.Text;
                string strNacionalidad = obtenerNacionalidad(lParticipante.Persona.pers_sPaisId.ToString());
                lParticipante.Persona.vNacionalidad = strNacionalidad.Replace("\"", "");
            }
            //----------------------------------------------------

            if (this.ddl_pers_sEstadoCivilId.SelectedIndex > 0)
            {
                lParticipante.Persona.pers_sEstadoCivilId = Convert.ToInt16(ddl_pers_sEstadoCivilId.SelectedValue);
            }

            if (this.ddl_pers_sProfesionId.SelectedIndex > 0)
            {
                lParticipante.Persona.pers_sOcupacionId = Convert.ToInt16(ddl_pers_sProfesionId.SelectedValue);
            }

            if (this.ddl_pers_sIdiomaNatalId.SelectedIndex > 0)
            {
                lParticipante.Persona.pers_sIdiomaNatalId = Convert.ToInt16(ddl_pers_sIdiomaNatalId.SelectedValue);
            }

            if (chkIncapacitado.Checked)
            {
                lPersona.pers_bIncapacidadFlag = true; // Es incapacitado
                lPersona.pers_vDescripcionIncapacidad = txtRegistroTipoIncapacidad.Text.ToUpper();

                lParticipante.Persona.pers_bIncapacidadFlag = true;
                lParticipante.Persona.pers_vDescripcionIncapacidad = txtRegistroTipoIncapacidad.Text.ToUpper();
                lParticipante.anpa_bFlagHuella = chkNoHuella.Checked;
                lParticipante.anpa_bFlagFirma = true; // No Firma
            }
            else
            {
                lPersona.pers_bIncapacidadFlag = false;
                lPersona.pers_vDescripcionIncapacidad = String.Empty;

                lParticipante.Persona.pers_bIncapacidadFlag = false;
                lParticipante.Persona.pers_vDescripcionIncapacidad = String.Empty;

                lParticipante.anpa_bFlagHuella = false; // Si Huella
                lParticipante.anpa_bFlagFirma = false; // Si Firma
            }
            
            RE_PERSONARESIDENCIA objResidencia = new RE_PERSONARESIDENCIA();

            objResidencia.Residencia.resi_iResidenciaId = Convert.ToInt64(hdn_acpa_residenciaId.Value);
            objResidencia.Residencia.resi_sResidenciaTipoId = Convert.ToInt16((int)Enumerador.enmTipoResidencia.RESIDENCIA);
            objResidencia.Residencia.resi_vResidenciaDireccion = this.txt_resi_vResidenciaDireccion.Text.ToString().ToUpper();
            objResidencia.Residencia.resi_vCodigoPostal = this.txt_resi_vCodigoPostal.Text.ToString().ToUpper();
            

            lParticipante.Persona.Residencias.Add(objResidencia);

            lParticipante.Residencia.resi_iResidenciaId = Convert.ToInt64(hdn_acpa_residenciaId.Value);

            lParticipante.Residencia.resi_sResidenciaTipoId = Convert.ToInt16((int)Enumerador.enmTipoResidencia.RESIDENCIA);

            lParticipante.Residencia.resi_vResidenciaDireccion = this.txt_resi_vResidenciaDireccion.Text.ToString().ToUpper();
            lParticipante.Residencia.resi_vResidenciaTelefono = string.Empty;
            lParticipante.Residencia.resi_vCodigoPostal = this.txt_resi_vCodigoPostal.Text.ToString().ToUpper();
            
            string strUbigeo = hdn_acpa_pais.Value + hdn_acpa_provincia.Value + hdn_acpa_distrito.Value;
            if (strUbigeo.Length > 0)
            {
//                lParticipante.Residencia.resi_cResidenciaUbigeo = Convert.ToString(hdn_acpa_pais.Value + hdn_acpa_provincia.Value + hdn_acpa_distrito.Value);
                lParticipante.Residencia.resi_cResidenciaUbigeo = strUbigeo;
                objResidencia.Residencia.resi_cResidenciaUbigeo = strUbigeo;
            }

            lParticipante.Persona._ResidenciaTop = objResidencia.Residencia;

            #endregion

            #region Campos descriptivos (personalizados solo para vizualizar el GRID)
            lParticipante.acpa_sTipoParticipanteId_desc = this.ddl_anpa_sTipoParticipanteId.SelectedItem.Text.ToString();
            lParticipante.peid_sDocumentoTipoId_desc = this.ddl_peid_sDocumentoTipoId.SelectedItem.Text.ToString();
            lParticipante.pers_sNacionalidadId_desc = this.ddl_pers_sNacionalidadId.SelectedItem.Text.ToString();
            lParticipante.pers_sIdiomaNatalId_desc = this.ddl_pers_sIdiomaNatalId.SelectedItem.Text.ToString();
            #endregion

        

            if (!bEdicion)
            {
                //-----------------------------------
                //NUEVO PARTICIPANTE
                //-----------------------------------
                bool bAdiciono = false;

                bAdiciono = AdicionarParticipante(lParticipante);

                if (bAdiciono == true)
                {
                    btnGrabarParticipantesNew.Enabled = true;

                    DataTable dtParticipantes = new DataTable();

                    dtParticipantes = CrearTablaParticipante(lParticipante);


                    this.grd_Participantes.DataSource = dtParticipantes;
                    this.grd_Participantes.DataBind();
                    updGrillaParticipantes.Update();
                    this.grd_Otorgantes.DataSource = ObtenerOtorgantesOrdenados(dtParticipantes);
                    this.grd_Otorgantes.DataBind();
                    updGrillaOtorgantes.Update();
                }
            }
            else
            {
                //-----------------------------------
                //EDITAR PARTICIPANTE
                //-----------------------------------
                lParticipante.anpa_iActoNotarialParticipanteId = ((List<CBE_PARTICIPANTE>)Session["ParticipanteContainer"])[iIndiceValidacion].anpa_iActoNotarialParticipanteId;

                bool bActualizo = false;
                bActualizo = ActualizarParticipante(lParticipante);

                if (bActualizo == true)
                {
                    btnGrabarParticipantesNew.Enabled = true;

                    ((List<CBE_PARTICIPANTE>)Session["ParticipanteContainer"])[iIndiceValidacion] = lParticipante;

                    DataTable dtParticipantes = new DataTable();

                    dtParticipantes = CrearTablaParticipante(null);

                    this.grd_Participantes.DataSource = dtParticipantes;
                    this.grd_Participantes.DataBind();
                    updGrillaParticipantes.Update();
                    this.grd_Otorgantes.DataSource = ObtenerOtorgantesOrdenados(dtParticipantes);
                    this.grd_Otorgantes.DataBind();
                    
                    updGrillaOtorgantes.Update();
                }
            }
            //-----------------------------------------------------
            //Fecha: 24/09/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Activar Cmb_TipoActoNotarial cuando 
            //        no existan participantes.
            //-----------------------------------------------------
            if (grd_Otorgantes.Rows.Count > 0)
            {
                Cmb_TipoActoNotarial.Enabled = false;
                updTipoActoNotarial.Update();
            }
            else
            {
                Cmb_TipoActoNotarial.Enabled = true;
                updTipoActoNotarial.Update();
            }
            //-----------------------------------------------------

            ViewState["ActoNotarialParticipanteId"] = "0";
            ViewState["ExtraIndiceEdicion"] = -1;
            hdn_IndiceGrillaParticipantesEdicion.Value = "0";            

            string strScript = string.Empty;
            strScript = @"$(function(){{
                                        tab_02_Limpiar();ActivarOtrodocumento();
                                    }});";
            strScript = string.Format(strScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "limpiarTabParticipantes", strScript, true);
            chkIncapacitado.Checked = false;
            txtRegistroTipoIncapacidad.Text = "";
            btnAgregarParticipanteNew.Text = " Agregar";
            //-----------------------------------
           
        }
        //=======================================================
        //Fecha: 20/08/2020
        //Autor: Miguel Márquez Beltrán
        //Motivo: Cambiar estado y cargar ModoEdicion().
        //=======================================================
        protected void btnGrabarParticipantesNew_Click(object sender, EventArgs e)
        {
            if (ValidarTotalParticipantes() == true)
            {
                //---------------------------------------------------------------------
                // Solo si esta registrada entonces cambiara el estado a Asociada.
                //---------------------------------------------------------------------
                if (Consultar_Estado_Cs(Convert.ToInt64(hdn_acno_iActoNotarialId.Value), Convert.ToInt64(hdn_acno_iActuacionId.Value)) == (int)Enumerador.enmNotarialProtocolarEstado.REGISTRADO)
                {
                    ActoNotarialMantenimiento bl = new ActoNotarialMantenimiento();
                    RE_ACTONOTARIAL actonotarialBE = new RE_ACTONOTARIAL();
                    actonotarialBE.acno_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
                    actonotarialBE.acno_sEstadoId = (int)Enumerador.enmNotarialProtocolarEstado.ASOCIADA;
                    actonotarialBE.acno_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    actonotarialBE.acno_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    actonotarialBE.acno_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
                    bl.ActoNotarialActualizarEstado(actonotarialBE);
                }
                                              
                //-----------------------------------
                ModoEdicion();
                //-----------------------------
                CargarComboApoderados();
                SeleccionarComboApoderado(Convert.ToInt16(ddl_TipoDocrepresentante.SelectedValue), txtRepresentanteNroDoc.Text.ToString());
                updCuerpoCabecera.Update();
                updParte.Update();

                //if (Cmb_TipoActoNotarial.SelectedIndex > 0)
                //{
                //    AsignarNormasporTipoActoNotarial();
                    
                //    updTxtNormativo.Update();
                //}
                Actualizar_IncisoCArticulo55();

                //--vpipa check
                string StrScript = string.Empty;
                StrScript = @"$('#MainContent_Btn_AfirmarTextoLeido').prop('disabled', true);$('#MainContent_cbxAfirmarTexto').prop('disabled', true);";
                StrScript = string.Format(StrScript);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitarChec", StrScript, true);
                //cbxAfirmarTexto.Enabled = false;

                //----------------------------------------------
                //Fecha: 13/01/2022
                //Autor: Miguel Márquez Beltrán
                //Motivo: Asignar la oficina lima por defecto.
                //----------------------------------------------
                ddl_OficinaRegistralRegistrador.SelectedValue = CON_Oficina_Registral_Lima;
                UpdUbigeoRegistrador.Update();
                //-----------------------------------
                rbOtros.Checked = false;
                rbApoderado.Checked = true;
                rbOtros.Enabled = true;
                rbApoderado.Enabled = true;
                ddl_Apoderado.Enabled = true;
               
                updParte.Update();
            }
        }
        protected void  btnAgregarPresentantNew_Click(object sender, EventArgs e)
        {            
            
            if (rbApoderado.Checked == true)
            {
                if (ddl_Apoderado.SelectedIndex == 0)
                {
                    EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PRESENTANTES", "Seleccione al apoderado."), "Validar Presentante.");
                    return;
                }
            }
            else
            {
                if (rbOtros.Checked == true)
                {
                    if (ddl_TipoDocrepresentante.SelectedIndex == 0)
                    {
                        EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PRESENTANTES", "Seleccione el tipo de documento."), "Validar Presentante.");
                        return;
                    }
                    if (txtRepresentanteNroDoc.Text.Trim().Length == 0)
                    {
                        EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PRESENTANTES", "Digite el número de documento."), "Validar Presentante.");
                        return;
                    }
                    if (txtRepresentanteNombres.Text.Trim().Length == 0)
                    {
                        EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PRESENTANTES", "Digite el nombre del presentante."), "Validar Presentante.");
                        return;
                    }
                    if (ddl_GerenoPresentante.SelectedIndex == 0)
                    {
                        EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PRESENTANTES", "Seleccione el género del presentante."), "Validar Presentante.");
                        return;
                    }
                }
            }
            
            if (GridViewPresentante.Rows.Count >=5)
            {
                EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PRESENTANTES", "Solo se permite 5 Presentantes."), "ExistePresentante");
                return;
            }

            bool existePresentante = false;
            for (int i = 0; i < GridViewPresentante.Rows.Count; i++)
            {
                System.Diagnostics.Debug.Write(i + "_" + GridViewPresentante.Rows[i].Cells[6].Text + "," + GridViewPresentante.Rows[i].Cells[8].Text);
                
                if (GridViewPresentante.Rows[i].Cells[6].Text == txtRepresentanteNroDoc.Text)
                {
                    existePresentante = true;
                }
                
            }
            if (existePresentante && Convert.ToInt64(HF_presentanteId.Value)==0)
            {
                EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PRESENTANTES", "Ya existe ."), "ExistePresentante");
                return;
            }

            CBE_PRESENTANTE b = new CBE_PRESENTANTE();
            b.anpr_iActoNotarialId = Convert.ToInt64(hdn_acno_iActoNotarialId.Value);
            b.anpr_sTipoPresentante= rbApoderado.Checked?1:2; //1:apoderado;2:otros
            b.anpr_iActoNotarialPresentanteId = Convert.ToInt64(HF_presentanteId.Value);
            b.anpr_sPresentanteTipoDocumento = Convert.ToInt16( ddl_TipoDocrepresentante.SelectedValue);
            b.anpr_vPresentanteNumeroDocumento = txtRepresentanteNroDoc.Text;
            b.anpr_vPresentanteNombre = txtRepresentanteNombres.Text;
            b.anpr_sPresentanteGenero = Convert.ToInt16(ddl_GerenoPresentante.SelectedValue);
            b.anpr_cEtsado = "A";
            b.operacion = b.anpr_iActoNotarialPresentanteId==0?"INS":"UDP";
            b.anpr_sUsuario= Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            b.anpr_vIP = SGAC.Accesorios.Util.ObtenerDireccionIP();
            ActoNotarialMantenimiento mnt = new ActoNotarialMantenimiento();
            long result = mnt.InsertarActoNotarialPresentnte(b);
            if (result > 0)
            {
                //----cargar grilla-------
                ParticipanteConsultaBL lParticipanteConsultaBL = new ParticipanteConsultaBL();
                List<CBE_PRESENTANTE> lPresentante = lParticipanteConsultaBL.listaPresentante(b);
                Session["PresentanteContainer"] = lPresentante;
                this.GridViewPresentante.DataSource = lPresentante;
                this.GridViewPresentante.DataBind();
                UpdateGridPresentantes.Update();

                if (b.anpr_sTipoPresentante == 2)
                {
                    ddl_TipoDocrepresentante.Enabled = true;
                    txtRepresentanteNroDoc.Enabled = true;
                    txtRepresentanteNombres.Enabled = true;
                    ddl_GerenoPresentante.Enabled = true;
                }
                HF_presentanteId.Value = "0";
                ddl_TipoDocrepresentante.SelectedIndex = 0;
                txtRepresentanteNroDoc.Text = "";
                txtRepresentanteNombres.Text = "";
                ddl_GerenoPresentante.SelectedIndex = 0;
                updParte.Update();

            }
            else
            {
                EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PRESENTANTES", "Error, no se ha podido registrar, asegurate que los datos sean correctos"), "Presentante");

            }

        }

        protected void btnAfirmarTextoLeido_click(object sender, EventArgs e)
        {
            if (validarCuerpo())
            {
                Btn_Aprobar_Click(sender, e);
                updDatosAbogado.Update();
            }
        }
        protected void cbxAfirmarTexto_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxAfirmarTexto.Checked)
            {
                //Btn_Aprobar.Enabled = true;
                Btn_Aprobar_Click(sender, e);
                
            }
            else
            {
                //Btn_Aprobar.Enabled = false;
            }

            //updAprobacion.Update();
            updDatosAbogado.Update();
        }

      

        protected void Actualizar_IncisoCArticulo55()
        {
            //-----------------------------------------------------------------------------------
            //Fecha: 24/09/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Si existe un extranjero se debe activar RichTextBoxDL1049Art55IncisoC
            //-----------------------------------------------------------------------------------
            List<CBE_PARTICIPANTE> lParticipantes = (List<CBE_PARTICIPANTE>)HttpContext.Current.Session["ParticipanteContainer"];
            if (lParticipantes.Count == 0) return;

            ActoNotarialConsultaBL BL = new ActoNotarialConsultaBL();
            DataTable dtParticipantes = new DataTable();

            dtParticipantes = BL.ActonotarialObtenerParticipantes(lParticipantes[0].anpa_iActoNotarialId);

            bool bExisteExtranjero = false;

            for (int i = 0; i < dtParticipantes.Rows.Count; i++)
            {
                //if (Convert.ToInt32(dtParticipantes.Rows[i]["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE) ||
                //    Convert.ToInt32(dtParticipantes.Rows[i]["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR) ||
                //    Convert.ToInt32(dtParticipantes.Rows[i]["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE) ||
                //    Convert.ToInt32(dtParticipantes.Rows[i]["anpa_sTipoParticipanteId"].ToString()) == Convert.ToInt32(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE))

                if (ObtenerIniciaRecibe(Convert.ToInt16(dtParticipantes.Rows[i]["anpa_sTipoParticipanteId"].ToString())) == "INICIA")
                {
                    if (dtParticipantes.Rows[i]["vNacionalidad"].ToString() != "PERUANA")
                    {
                        bExisteExtranjero = true;
                        break;
                    }
                }
            }
            if (bExisteExtranjero == true)
            {
                tablaIncisoCArticulo55.Style.Add("display", "block");
            }
            else
            {
                tablaIncisoCArticulo55.Style.Add("display", "none");
            }
            updIncisoCArticulo55.Update();            
            
        }

        [System.Web.Services.WebMethod]
        public static string busqueda_escritura(string actonotarialPrimigenia)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var jsonObject = serializer.Deserialize<dynamic>(actonotarialPrimigenia);

            string strAnioEscrituraPublica = Convert.ToString(jsonObject["anpr_cAnioEscritura"]);
            string strNumeroEscrituraPublica= Convert.ToString(jsonObject["anpr_vNumeroEscrituraPublica"]);
            Int16 iOficinaConsularId = Convert.ToInt16(jsonObject["anpr_sOficinaConsularId"]);
            string strPG_PE = Convert.ToString(jsonObject["PG_PE"]);

            ActoNotarialConsultaBL ancBL = new ActoNotarialConsultaBL();

            return serializer.Serialize(ancBL.BusquedaEscrituraPublica(strAnioEscrituraPublica, strNumeroEscrituraPublica, iOficinaConsularId, strPG_PE)).ToString();            
        }

        //-----------------------------------------------------
        //Fecha: 21/12/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Nuevo Acta de Rectificación.
        //-----------------------------------------------------
        [System.Web.Services.WebMethod]
        public static string llenar_introduccion_escritura_Rectificacion(string cuerpo)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var jsonObject = serializer.Deserialize<dynamic>(cuerpo);

            string strTextoActoNotarial = Convert.ToString(jsonObject["textoActoNotarial"]);


            string sIdiomaCastellano = HttpContext.Current.Session["NotarialIdioma"].ToString();

            ActoNotarialConsultaBL bl = new ActoNotarialConsultaBL();
            DataTable dtDatosEscritura = new DataTable();
            DataTable dtParticipantesAll = new DataTable();


            //--------------------------------------------------------------------------
            // Fecha: 26/09/2017
            // Autor: Miguel Márquez Beltrán
            // Objetivo: Actualizar la fecha de Suscripción del Participante Otorgante
            //--------------------------------------------------------------------------

            string strMensaje = "";

            strMensaje = ActualizarFechaSuscripcionOtorgantes();

            //--------------------------------------------------------------------
            // Fecha: 26/09/2017
            // Autor: Miguel Márquez Beltrán
            // Objetivo: Actualizar la fecha de Conclusión de la Escritura Pública
            //--------------------------------------------------------------------
            string strFechaConclusionFirma = Convert.ToString(jsonObject["acno_dFechaConclusionFirma"]);

            if (strFechaConclusionFirma.Length > 0)
            {
                RE_ACTONOTARIAL actoNotarial = new RE_ACTONOTARIAL();

                actoNotarial.acno_iActoNotarialId = Convert.ToInt64(jsonObject["ancu_iActoNotarialId"]); ;
                actoNotarial.acno_sOficinaConsularId = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                actoNotarial.acno_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                actoNotarial.acno_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
                actoNotarial.acno_dFechaConclusionFirma = Comun.FormatearFecha(jsonObject["acno_dFechaConclusionFirma"]);

                ActoNotarialMantenimiento mnt = new ActoNotarialMantenimiento();

                strMensaje = mnt.ActualizarFechaConclusion(actoNotarial);
            }
            //--------------------------------------------------------------------
            //--------------------------------------------------------------------------
            //Fecha: 03/04/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Asignar los datos de la escritura de una sesión a un DataTable.
            //--------------------------------------------------------------------------

            if (HttpContext.Current.Session["dtDatosEscritura"] == null)
            {
                dtDatosEscritura = bl.ActonotarialObtenerDatosPrincipales(Convert.ToInt64(jsonObject["ancu_iActoNotarialId"]), Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                //--------------------------------------------------------------------------
                //Fecha: 06/04/2020
                //Autor: Miguel Márquez Beltrán
                //Motivo: Asignar los datos de la escritura a una sesión.
                //--------------------------------------------------------------------------
                HttpContext.Current.Session["dtDatosEscritura"] = dtDatosEscritura;
                //--------------------------------------------------------------------------

            }
            else
            {
                dtDatosEscritura = (DataTable)HttpContext.Current.Session["dtDatosEscritura"];
            }
            //--------------------------------------------------------------------------
            dtParticipantesAll = bl.ActonotarialObtenerParticipantes(Convert.ToInt64(jsonObject["ancu_iActoNotarialId"]));

            

            DataView dv = dtParticipantesAll.DefaultView;
            DataTable dts = dv.ToTable();
            dts = (from p in dtParticipantesAll.AsEnumerable()
                   where Convert.ToInt16(p["anpa_sTipoParticipanteId"]) != Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO)
                   orderby p["vPersona"]
                   select p).CopyToDataTable();




            DateTime dtFechaEscritura = Comun.FormatearFecha(dtDatosEscritura.Rows[0]["Fecha"].ToString());

            string strFecha = Util.ObtenerFechaParaDocumentoLegalProtocolar(dtFechaEscritura).ToUpper();
            string strAnioEscritura = "";
            string strSubTipoActoNotarialDesc = "";
            //---------------------------------------------------------------
            //Fecha: Fecha: 28/03/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Asignar el año de la Escritura Pública
            //---------------------------------------------------------------
            if (dtDatosEscritura.Rows[0]["Fecha"].ToString().Length == 10)
            {
                strAnioEscritura = dtDatosEscritura.Rows[0]["Fecha"].ToString().Substring(6);
            }
            //---------------------------------------------------------------

            DateTime datFechaEscritura = new DateTime();
            if (!DateTime.TryParse(dtDatosEscritura.Rows[0]["Fecha"].ToString(), out datFechaEscritura))
            {
                datFechaEscritura = Convert.ToDateTime(strFecha, System.Globalization.CultureInfo.GetCultureInfo("en-Us").DateTimeFormat);
            }

            strSubTipoActoNotarialDesc = dtDatosEscritura.Rows[0]["SubTipoActoNotarialDesc"].ToString().Trim();

            int intTipoEscrituraId = Convert.ToInt32(dtDatosEscritura.Rows[0]["SubTipoActoNotarialId"]);
            string strTipoEscrituraDes = dtDatosEscritura.Rows[0]["SubTipoActoNotarialDesc"].ToString().Trim();
            string strDenominacion = dtDatosEscritura.Rows[0]["vDenominacion"].ToString().Trim();

            string strOficinaConsularNombre = dtDatosEscritura.Rows[0]["NombreOficinaConsular"].ToString().Trim();
            string strFuncionarioAutorizador = dtDatosEscritura.Rows[0]["NombreFuncionario"].ToString().Trim();

            string strCargoFuncionarioAutorizador = dtDatosEscritura.Rows[0]["CargoFuncionario"].ToString().Trim();

            string strUbigeoOficinaConsular = dtDatosEscritura.Rows[0]["CiudadOficinaConsular"].ToString().Trim();

            string strUbigeoOficinaConsularCiudad = dtDatosEscritura.Rows[0]["CiudadOficinaConsular"].ToString().Trim() + ", " + dtDatosEscritura.Rows[0]["Provincia"].ToString().Trim();

            string strlMinuta = string.Empty;
            string strArticuloPersonaSexo = string.Empty;
            string strIdentificadoaPersonaSexo = string.Empty;
            Int32 acno_iNumeroActoNotarial = 0;
            if (Convert.ToBoolean(dtDatosEscritura.Rows[0]["Minuta"]))
            {
                strlMinuta = "CON MINUTA";
                if (jsonObject["acno_iNumeroActoNotarial"] != "")
                {
                    acno_iNumeroActoNotarial = Convert.ToInt32(Convert.ToInt64(jsonObject["acno_iNumeroActoNotarial"]));
                }
            }
            else
            {
                strlMinuta = "NO SE REQUIERE LA PRESENTACIÓN DE MINUTA";
            }

            string strNroEscrituraPublica = dtDatosEscritura.Rows[0]["NumeroEscrituraPublica"].ToString().Trim();
            string strNroFojaInicial = dtDatosEscritura.Rows[0]["NumeroFojaInicial"].ToString().Trim();
            string strNroFojaFinal = dtDatosEscritura.Rows[0]["vNumeroFojaFinal"].ToString().Trim();
            string strAutorizacionTipoId = Convert.ToString(jsonObject["acno_vAutorizacionTipo"]);
            string strAutorizacionNroDocumento = Convert.ToString(jsonObject["acno_vAutorizacionDocumentoNumero"]);

            string strNumeroColegiatura = Convert.ToString(jsonObject["acno_vNumeroColegiatura"]);
            string strFirmaIlegible = Convert.ToString(jsonObject["ancu_vFirmaIlegible"]);
            string strLibro = dtDatosEscritura.Rows[0]["vNroLibro"].ToString().Trim();
            string strNombreColegiatura = Convert.ToString(jsonObject["acno_vNombreColegiatura"]);
                      

            StringBuilder sScript = new StringBuilder();


            string strLetrasNumeroEscrituraPublica = "CERO";
            string strLibroRomanos = "LIBR000";

            if (strNroEscrituraPublica.Length == 0)
            {
                strNroEscrituraPublica = "MRE000";
            }

            sScript.Append("<p style='text-align:justify;font-weight:bold;'>");
            sScript.Append("ESCRITURA PÚBLICA NÚMERO " + strLetrasNumeroEscrituraPublica + "(" + strNroEscrituraPublica + ")" + " (LIBRO " + strLibroRomanos + ", AÑO " + strAnioEscritura + ").");
            sScript.Append("</p>");

            sScript.Append("<p style='text-align:justify;'>");
            sScript.Append("EN LA CIUDAD DE " + strUbigeoOficinaConsularCiudad + ", " + strFecha.Trim() + ", ANTE ");
            sScript.Append("MÍ, " + strFuncionarioAutorizador + ", " + strCargoFuncionarioAutorizador + " DEL PERÚ EN " + strUbigeoOficinaConsular + "; EN APLICACIÓN DEL ");
            sScript.Append("ARTÍCULO 48º DEL DECRETO LEGISLATIVO 1049 Y EL ARTÍCULO 26º DEL DECRETO ");
            sScript.Append("SUPREMO 003-2009-JUS, SUSCRIBO EL PRESENTE INSTRUMENTO COMO \"ACTA ");
            sScript.Append("PROTOCOLAR DE RECTIFICACIÓN\", DEJANDO CONSTANCIA QUE LA EXTENSIÓN DEL ");
            sScript.Append("MISMO SE EFECTUARÁ SIN LA PARTICIPACIÓN DEL OTORGANTE, Y DECLARO LO ");
            sScript.Append("SIGUIENTE:");
            sScript.Append("</p>");
         
            string innerString = sScript.ToString();

            return innerString.ToString();
        }

        [System.Web.Services.WebMethod]
        public static string llenar_conclusion_escritura_Rectificacion(string cuerpo)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var jsonObject = serializer.Deserialize<dynamic>(cuerpo);


            string strTextoActoNotarial = Convert.ToString(jsonObject["textoActoNotarial"]);


            DataTable dt = new DataTable();
            DataTable dtdp = new DataTable();

            ActoNotarialConsultaBL bl = new ActoNotarialConsultaBL();

            //--------------------------------------------------------------------------
            //Fecha: 03/04/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Asignar los datos de la escritura de una sesión a un DataTable.
            //--------------------------------------------------------------------------

            if (HttpContext.Current.Session["dtDatosEscritura"] == null)
            {
                dtdp = bl.ActonotarialObtenerDatosPrincipales(Convert.ToInt64(jsonObject["ancu_iActoNotarialId"]), Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                //--------------------------------------------------------------------------
                //Fecha: 03/04/2020
                //Autor: Miguel Márquez Beltrán
                //Motivo: Asignar los datos de la escritura a una sesión.
                //--------------------------------------------------------------------------
                HttpContext.Current.Session["dtDatosEscritura"] = dtdp;
                //--------------------------------------------------------------------------
            }
            else
            {
                dtdp = (DataTable)HttpContext.Current.Session["dtDatosEscritura"];
            }
            //--------------------------------------------------------------------------

            dt = bl.ActonotarialObtenerParticipantes(Convert.ToInt64(jsonObject["ancu_iActoNotarialId"]));

            

            string strDL1049Articulo55C = Convert.ToString(jsonObject["ancu_vDL1049Articulo55C"]);

            DataView dv = dt.DefaultView;
            DataTable dts = dv.ToTable();
            dts = (from p in dt.AsEnumerable()
                   orderby p["vPersona"]
                   select p
                       ).CopyToDataTable();

            //string strFecha = Util.ObtenerFechaParaDocumentoLegal(Comun.FormatearFecha((dtdp.Rows[0]["Fecha"]).ToString())).ToUpper().Trim();
            string strFecha = Util.ObtenerFechaParaDocumentoLegalProtocolar(Comun.FormatearFecha((dtdp.Rows[0]["Fecha"]).ToString())).ToUpper().Trim();

            int strTipoEscrituraId = Convert.ToInt32(dtdp.Rows[0]["SubTipoActoNotarialId"]);
            string strTipoParticipante = string.Empty;


            string strFuncionarioAutorizador = dtdp.Rows[0]["NombreFuncionario"].ToString().Trim();
            string strUbigeoOficinaConsular = dtdp.Rows[0]["CiudadOficinaConsular"].ToString().Trim();
            string strProvincia = dtdp.Rows[0]["Provincia"].ToString().Trim();

            string strCargoFuncionarioAutorizador = dtdp.Rows[0]["CargoFuncionario"].ToString().Trim();

            string strNroEscrituraPublica = dtdp.Rows[0]["NumeroEscrituraPublica"].ToString().Trim();
            string strNroFojaInicial = dtdp.Rows[0]["NumeroFojaInicial"].ToString().Trim();
            string strNroFojaFinal = dtdp.Rows[0]["vNumeroFojaFinal"].ToString().Trim();

            
           

            StringBuilder sScript = new StringBuilder();

            sScript.Append("<p style='text-align:justify;'>");

           
            sScript.Append("<b>CONCLUSIÓN:</b> FORMALIZADO EL INSTRUMENTO, YO, <b>");

            sScript.Append(strFuncionarioAutorizador + ", </b>" + strCargoFuncionarioAutorizador + " DEL PERÚ EN " + strUbigeoOficinaConsular + ", " + strProvincia);
            sScript.Append(", DOY FE DEL OBJETO Y FINES DE LA PRESENTE ESCRITURA PÚBLICA.");
            sScript.Append("</p>");
                
            //-------------------------------------------

            Documento odoc = new Documento();

            string strLetrasNumeroEscrituraPublica = "";

            if (Convert.ToString(jsonObject["acno_vNumeroEscrituraPublica"]) == "MRE000")
            {
                strLetrasNumeroEscrituraPublica = "CERO";
            }
            else
            {
                strLetrasNumeroEscrituraPublica = odoc.ConvertirNumeroLetras(Convert.ToString(jsonObject["acno_vNumeroEscrituraPublica"]), true);
            }


            //-------------------------------------------



            string innerString = sScript.ToString();

            return innerString.ToString();
        }

        [System.Web.Services.WebMethod]
        public static string llenar_final_escritura_Rectificacion(string cuerpo)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var jsonObject = serializer.Deserialize<dynamic>(cuerpo);

            string strTextoActoNotarial = Convert.ToString(jsonObject["textoActoNotarial"]);

            ActoNotarialConsultaBL bl = new ActoNotarialConsultaBL();
            DataTable dtdp = new DataTable();
            //--------------------------------------------------------------------------
            //Fecha: 03/04/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Asignar los datos de la escritura de una sesión a un DataTable.
            //--------------------------------------------------------------------------                        
            if (HttpContext.Current.Session["dtDatosEscritura"] == null)
            {
                dtdp = bl.ActonotarialObtenerDatosPrincipales(Convert.ToInt64(jsonObject["ancu_iActoNotarialId"]), Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                //--------------------------------------------------------------------------
                //Fecha: 03/04/2020
                //Autor: Miguel Márquez Beltrán
                //Motivo: Asignar los datos de la escritura a una sesión.
                //--------------------------------------------------------------------------
                HttpContext.Current.Session["dtDatosEscritura"] = dtdp;
                //--------------------------------------------------------------------------
            }
            else
            {
                dtdp = (DataTable)HttpContext.Current.Session["dtDatosEscritura"];
            }
           // string strSubTipoActoNotarialDesc = dtdp.Rows[0]["SubTipoActoNotarialDesc"].ToString().Trim();
            //--------------------------------------------------------------------------
            //DataTable dtParticipantesAll = new DataTable();

            //dtParticipantesAll = bl.ActonotarialObtenerParticipantes(Convert.ToInt64(jsonObject["ancu_iActoNotarialId"]));


           // DataView dv = dtParticipantesAll.DefaultView;
           // DataTable dtsOtorgantes = dv.ToTable();

            //dtsOtorgantes = (from p in dtParticipantesAll.AsEnumerable()
            //                 where Convert.ToInt16(p["anpa_sTipoParticipanteId"]) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE)
            //                     || Convert.ToInt16(p["anpa_sTipoParticipanteId"]) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE)
            //                     || Convert.ToInt16(p["anpa_sTipoParticipanteId"]) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR)
            //                     || Convert.ToInt16(p["anpa_sTipoParticipanteId"]) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE)
            //                     || Convert.ToInt16(p["anpa_sTipoParticipanteId"]) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE)
            //                     || Convert.ToInt16(p["anpa_sTipoParticipanteId"]) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO)
            //                 orderby p["vPersona"]
            //                 select p).CopyToDataTable();

            //dtsOtorgantes = (from p in dtParticipantesAll.AsEnumerable()
            //                 where ObtenerIniciaRecibe(Convert.ToInt16(p["anpa_sTipoParticipanteId"])) == "INICIA"
            //                     || Convert.ToInt16(p["anpa_sTipoParticipanteId"]) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE)
            //                     || Convert.ToInt16(p["anpa_sTipoParticipanteId"]) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO)
            //                 orderby p["vPersona"]
            //                 select p).CopyToDataTable();

            //DataTable dtsApoderados = dv.ToTable();

            //DataView dvApoderados = dtsApoderados.DefaultView;

            //dvApoderados.RowFilter = "anpa_sTipoParticipanteId = " + Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONATARIO)
            //                          + " or anpa_sTipoParticipanteId = " + Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPADO)
            //                          + " or anpa_sTipoParticipanteId = " + Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.COMPRADOR);

            //DataTable dtApoderadoFiltrado = dvApoderados.ToTable();


            //dtApoderadoFiltrado.DefaultView.Sort = "vPersona";

          
           
            //------------------------------------------Final de datos de los Apoderados.
            //-----------------------------------------------------------------------------
           // string strFuncionarioAutorizador = dtdp.Rows[0]["NombreFuncionario"].ToString().Trim();

            //string strCargoFuncionarioAutorizador = dtdp.Rows[0]["CargoFuncionario"].ToString().Trim();
           // string strUbigeoOficinaConsular = dtdp.Rows[0]["CiudadOficinaConsular"].ToString().Trim();

            //string strFechaLarga = Util.ObtenerNumeroDiaLetrasDiaNumerosMesLargoAnio(Comun.FormatearFecha(dtdp.Rows[0]["Fecha"].ToString()), true);
            string strFechaLarga = "";

            if (dtdp.Rows[0]["FechaconclusionFirma"].ToString().Length > 0)
            {
                strFechaLarga = Util.ObtenerNumeroDiaLetrasDiaNumerosMesLargoAnio(Comun.FormatearFecha(dtdp.Rows[0]["FechaconclusionFirma"].ToString()), true);
            }

            string strCadFojas = "0";
            string strCadFojasLetras = "CERO";

            string strLibroRomanos = "LIBR000";

            StringBuilder sScript = new StringBuilder();
          
            //----------------------------------------------------------------------

            sScript.Append("<p style='text-align:justify;'>");

            sScript.Append("LA PRESENTE ESCRITURA PÚBLICA SE ENCUENTRA INSCRITA EN LAS FOJAS " + strCadFojasLetras + " (" + strCadFojas + ") ");

            sScript.Append("DEL <b>LIBRO</b> " + strLibroRomanos + " DEL REGISTRO DE INSTRUMENTOS PÚBLICOS DE ESTE CONSULADO GENERAL, ");
            sScript.Append("DE LO QUE DOY FE.");
            sScript.Append("</p>");

            sScript.Append("<p style='text-align:justify;'>");
            sScript.Append("CONCLUYE LA SUSCRIPCIÓN DE ESTA ESCRITURA PÚBLICA ");
           
            sScript.Append("EL DÍA " + strFechaLarga + ".");

            sScript.Append("</p>");

            //------------------------------------------------------------


            string innerString = sScript.ToString();

            return innerString.ToString();

        }


        [System.Web.Services.WebMethod]
        public static string llenar_introduccion_escritura_Regular(string cuerpo)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var jsonObject = serializer.Deserialize<dynamic>(cuerpo);

            string strTextoActoNotarial = Convert.ToString(jsonObject["textoActoNotarial"]);


            string sIdiomaCastellano = HttpContext.Current.Session["NotarialIdioma"].ToString();

            ActoNotarialConsultaBL bl = new ActoNotarialConsultaBL();
            DataTable dtDatosEscritura = new DataTable();
            DataTable dtParticipantesAll = new DataTable();


            //--------------------------------------------------------------------------
            // Fecha: 26/09/2017
            // Autor: Miguel Márquez Beltrán
            // Objetivo: Actualizar la fecha de Suscripción del Participante Otorgante
            //--------------------------------------------------------------------------

            string strMensaje = "";

            strMensaje = ActualizarFechaSuscripcionOtorgantes();

            //--------------------------------------------------------------------
            // Fecha: 26/09/2017
            // Autor: Miguel Márquez Beltrán
            // Objetivo: Actualizar la fecha de Conclusión de la Escritura Pública
            //--------------------------------------------------------------------
            string strFechaConclusionFirma = Convert.ToString(jsonObject["acno_dFechaConclusionFirma"]);

            if (strFechaConclusionFirma.Length > 0)
            {
                RE_ACTONOTARIAL actoNotarial = new RE_ACTONOTARIAL();

                actoNotarial.acno_iActoNotarialId = Convert.ToInt64(jsonObject["ancu_iActoNotarialId"]); ;
                actoNotarial.acno_sOficinaConsularId = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                actoNotarial.acno_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                actoNotarial.acno_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
                actoNotarial.acno_dFechaConclusionFirma = Comun.FormatearFecha(jsonObject["acno_dFechaConclusionFirma"]);

                ActoNotarialMantenimiento mnt = new ActoNotarialMantenimiento();

                strMensaje = mnt.ActualizarFechaConclusion(actoNotarial);
            }
            //--------------------------------------------------------------------
            //--------------------------------------------------------------------------
            //Fecha: 03/04/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Asignar los datos de la escritura de una sesión a un DataTable.
            //--------------------------------------------------------------------------

            if (HttpContext.Current.Session["dtDatosEscritura"] == null)
            {
                dtDatosEscritura = bl.ActonotarialObtenerDatosPrincipales(Convert.ToInt64(jsonObject["ancu_iActoNotarialId"]), Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                //--------------------------------------------------------------------------
                //Fecha: 06/04/2020
                //Autor: Miguel Márquez Beltrán
                //Motivo: Asignar los datos de la escritura a una sesión.
                //--------------------------------------------------------------------------
                HttpContext.Current.Session["dtDatosEscritura"] = dtDatosEscritura;
                //--------------------------------------------------------------------------

            }
            else
            {
                dtDatosEscritura = (DataTable)HttpContext.Current.Session["dtDatosEscritura"];
            }
            //--------------------------------------------------------------------------
            //if (HttpContext.Current.Session["dtParticipantesAll"] == null)
            //{
            dtParticipantesAll = bl.ActonotarialObtenerParticipantes(Convert.ToInt64(jsonObject["ancu_iActoNotarialId"]));

            //--------------------------------------------------------------------------
            //Fecha: 06/04/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Asignar los datos de la escritura a una sesión.
            //--------------------------------------------------------------------------
            //  HttpContext.Current.Session["dtParticipantesAll"] = dtParticipantesAll;
            //--------------------------------------------------------------------------
            //}
            //else
            //{
            //    dtParticipantesAll = (DataTable) HttpContext.Current.Session["dtParticipantesAll"];
            //}

            DataView dv = dtParticipantesAll.DefaultView;
            DataTable dts = dv.ToTable();
            dts = (from p in dtParticipantesAll.AsEnumerable()
                   where Convert.ToInt16(p["anpa_sTipoParticipanteId"]) != Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO)
                   orderby p["vPersona"]
                   select p).CopyToDataTable();

            string strTipoParticipante = string.Empty;
            string vNombreParticipante = string.Empty;
            string strParticipante = string.Empty;
            string strOtorganteNotarial = string.Empty;
            string strApoderadoNotarial = string.Empty;

            string strParticipanteOtorgante = string.Empty;
            string strParticipanteFavorecido = string.Empty;
            string strParticipanteCuerpoPoder = string.Empty;

            string strParticipanteInterprete = string.Empty;

            string strUbigeoParticipante = string.Empty;

            DateTime dtFechaEscritura = Comun.FormatearFecha(dtDatosEscritura.Rows[0]["Fecha"].ToString());

            string strFecha = Util.ObtenerFechaParaDocumentoLegalProtocolar(dtFechaEscritura).ToUpper();
            string strAnioEscritura = "";
            string strSubTipoActoNotarialDesc = "";
            //---------------------------------------------------------------
            //Fecha: Fecha: 28/03/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Asignar el año de la Escritura Pública
            //---------------------------------------------------------------
            if (dtDatosEscritura.Rows[0]["Fecha"].ToString().Length == 10)
            {
                strAnioEscritura = dtDatosEscritura.Rows[0]["Fecha"].ToString().Substring(6);
            }
            //---------------------------------------------------------------

            DateTime datFechaEscritura = new DateTime();
            if (!DateTime.TryParse(dtDatosEscritura.Rows[0]["Fecha"].ToString(), out datFechaEscritura))
            {
                datFechaEscritura = Convert.ToDateTime(strFecha, System.Globalization.CultureInfo.GetCultureInfo("en-Us").DateTimeFormat);
            }

            strSubTipoActoNotarialDesc = dtDatosEscritura.Rows[0]["SubTipoActoNotarialDesc"].ToString().Trim();

            int intTipoEscrituraId = Convert.ToInt32(dtDatosEscritura.Rows[0]["SubTipoActoNotarialId"]);
            string strTipoEscrituraDes = dtDatosEscritura.Rows[0]["SubTipoActoNotarialDesc"].ToString().Trim();
            string strDenominacion = dtDatosEscritura.Rows[0]["vDenominacion"].ToString().Trim();

            string strOficinaConsularNombre = dtDatosEscritura.Rows[0]["NombreOficinaConsular"].ToString().Trim();
            string strFuncionarioAutorizador = dtDatosEscritura.Rows[0]["NombreFuncionario"].ToString().Trim();

            string strCargoFuncionarioAutorizador = dtDatosEscritura.Rows[0]["CargoFuncionario"].ToString().Trim();

            string strUbigeoOficinaConsular = dtDatosEscritura.Rows[0]["CiudadOficinaConsular"].ToString().Trim();

            string strUbigeoOficinaConsularCiudad = dtDatosEscritura.Rows[0]["CiudadOficinaConsular"].ToString().Trim() + ", " + dtDatosEscritura.Rows[0]["Provincia"].ToString().Trim();

            string strlMinuta = string.Empty;
            string strArticuloPersonaSexo = string.Empty;
            string strIdentificadoaPersonaSexo = string.Empty;
            Int32 acno_iNumeroActoNotarial = 0;
            if (Convert.ToBoolean(dtDatosEscritura.Rows[0]["Minuta"]))
            {
                strlMinuta = "CON MINUTA";
                if (jsonObject["acno_iNumeroActoNotarial"] != "")
                {
                    acno_iNumeroActoNotarial = Convert.ToInt32(Convert.ToInt64(jsonObject["acno_iNumeroActoNotarial"]));
                }
            }
            else
            {
                strlMinuta = "NO SE REQUIERE LA PRESENTACIÓN DE MINUTA";
            }

            string strNroEscrituraPublica = dtDatosEscritura.Rows[0]["NumeroEscrituraPublica"].ToString().Trim();
            string strNroFojaInicial = dtDatosEscritura.Rows[0]["NumeroFojaInicial"].ToString().Trim();
            string strNroFojaFinal = dtDatosEscritura.Rows[0]["vNumeroFojaFinal"].ToString().Trim();
            string strAutorizacionTipoId = Convert.ToString(jsonObject["acno_vAutorizacionTipo"]);
            string strAutorizacionNroDocumento = Convert.ToString(jsonObject["acno_vAutorizacionDocumentoNumero"]);
            string strNumeroColegiatura = Convert.ToString(jsonObject["acno_vNumeroColegiatura"]);
            string strFirmaIlegible = Convert.ToString(jsonObject["ancu_vFirmaIlegible"]);
            string strLibro = dtDatosEscritura.Rows[0]["vNroLibro"].ToString().Trim();
            string strNombreColegiatura = Convert.ToString(jsonObject["acno_vNombreColegiatura"]);

            int contadorParticipantes = 0;

            Int32 ContadorParticipanteHombres = 0;
            Int32 ContadorParticipanteMujeres = 0;
            string strParticipanteQuienSP = string.Empty;
            string strParticipanteElSP = string.Empty;
            string strParticipanteCompareceSP = string.Empty;

            String str_participante_piden_plural = String.Empty;

            int contadorOtorgantes = 0;
            int contadortestigoruego = 0;
            int contadorApoderado = 0;
            int contarhombrestestigo = 0;
            int contarmuejerestestigo = 0;
            int contadorApoderadoHombre = 0;
            int contadorApoderadoMujer = 0;
            string stroOtorganteEsSonSP = string.Empty;
            string stroPoderdanteSingularPlural = string.Empty;
            string stroOtorganteInteligenteSP = string.Empty;
            string strOtorganteObligaSP = string.Empty;
            string strOtorganteMayorSP = string.Empty;
            string strOtorganteOtorgaSP = string.Empty;
            string strOtorganteArticuloSexoPS = string.Empty;
            string strTestigoRuegoArticuloSexoPS = string.Empty;
            string strTestigoRuego = string.Empty;
            string strOtorganteGeneroDiscapacidad = string.Empty;
            string strOtorganteArticuloSexoTestigoRuego = string.Empty;
            string strApoderanteSexoIdentificado = string.Empty;
            string strApoderadoOtorgaSP = string.Empty;
            string strllamado_a = string.Empty;

            int strGenero = 0;
            string strEstadoCivil = "";

            string vIdioma = string.Empty;
            string strSubTipoActoNotarial = string.Empty;
            string strCodigoPostal = string.Empty;


            foreach (DataRow dr in dts.Rows)
            {
                #region foreach (DataRow dr in dts.Rows)

                strGenero = Convert.ToInt32(dr["iGenero"]);
                strEstadoCivil = dr["vEstadoCivil"].ToString().Trim();
                strUbigeoParticipante = dr["cResidenciaUbigeo"].ToString().Trim();
                strCodigoPostal = dr["resi_vCodigoPostal"].ToString().Trim();

                if (string.IsNullOrEmpty(strEstadoCivil))
                {
                    strEstadoCivil = "<ESTADO CIVIL>";
                }
                else
                {
                    if (strGenero == (int)Enumerador.enmGenero.MASCULINO)
                    {
                        #region Masculino

                        if (strEstadoCivil == "SOLTERO")
                            strEstadoCivil = "SOLTERO";
                        else if (strEstadoCivil == "CASADO")
                            strEstadoCivil = "CASADO";
                        else if (strEstadoCivil == "DIVORCIADO")
                            strEstadoCivil = "DIVORCIADO";
                        else if (strEstadoCivil == "VIUDO")
                            strEstadoCivil = "VIUDO";

                        strIdentificadoaPersonaSexo = "IDENTIFICADO";
                        #endregion

                    }
                    else if (strGenero == (int)Enumerador.enmGenero.FEMENINO)
                    {
                        #region Femenino

                        if (strEstadoCivil == "SOLTERO")
                            strEstadoCivil = "SOLTERA";
                        else if (strEstadoCivil == "CASADO")
                            strEstadoCivil = "CASADA";
                        else if (strEstadoCivil == "DIVORCIADO")
                            strEstadoCivil = "DIVORCIADA";
                        else if (strEstadoCivil == "VIUDO")
                            strEstadoCivil = "VIUDA";

                        ContadorParticipanteMujeres++;
                        strIdentificadoaPersonaSexo = "IDENTIFICADA";

                        #endregion
                    }
                }

                string strOcupacion = dr["vOcupacion"].ToString().Trim();
                if (string.IsNullOrEmpty(strEstadoCivil))
                {
                    strOcupacion = "<OCUPACIÓN>";
                }

                string strDireccion = dr["vDireccion"].ToString().Trim().ToUpper();
                if (string.IsNullOrEmpty(strDireccion))
                {
                    strDireccion = "<DIRECCION>";
                }

                string strDistCiu = dr["DistCiu"].ToString().Trim();
                if (string.IsNullOrEmpty(strDistCiu))
                {
                    strDistCiu = "<DISTRITO>";
                }

                string strProvPais = dr["ProvPais"].ToString().Trim();
                if (string.IsNullOrEmpty(strProvPais))
                {
                    strProvPais = "<PAIS>";
                }

                string strDptoCont = dr["DptoCont"].ToString().Trim();
                if (string.IsNullOrEmpty(strDptoCont))
                {
                    strDptoCont = "<DEPARTAMENTO>";
                }
                else
                {
                    if (strUbigeoParticipante.Length >= 2)
                    {
                        if (Convert.ToInt32(strUbigeoParticipante.Substring(0, 2)) >= 90)
                            strDptoCont = strProvPais;
                    }
                }

                strSubTipoActoNotarial = dr["acno_sSubTipoActoNotarialId"].ToString();
                strTipoParticipante = dr["vTipoParticipanteId"].ToString();

                //if (strTipoParticipante == "APODERADO" || strTipoParticipante == "COMPRADOR"
                //    || strTipoParticipante == "DONATARIO" || strTipoParticipante == "ANTICIPADO")

                if (ObtenerIniciaRecibe(strTipoParticipante) == "RECIBE")
                {
                    #region Apoderado

                    strParticipanteFavorecido += dr["vPersona"].ToString() + "<br>";
                    string strDescTipoDocumento = "";


                    if (dr["peid_sDocumentoTipoId"].ToString() == Convert.ToString((int)Enumerador.enmTipoDocumento.OTROS))
                    {
                        strDescTipoDocumento = dr["peid_vTipoDocumento"].ToString().Trim();
                        if (strDescTipoDocumento.Length == 0)
                        {
                            strDescTipoDocumento = dr["vDescLargaTipDoc"].ToString().Trim();
                        }
                    }
                    else
                    {
                        strDescTipoDocumento = dr["vDescLargaTipDoc"].ToString().Trim();
                    }

                    strParticipanteCuerpoPoder += dr["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + dr["vNroDocumento"].ToString() + ", ";


                    //-----------------------------------------------------------
                    //Fecha: 10/12/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Mostrar el código postal cuando el participante
                    //          es del país de Estados Unidos de América.
                    //-----------------------------------------------------------
                    if (strUbigeoParticipante.Length == 6)
                    {
                        // Solo valido para Estados Unidos de América.
                        if (strUbigeoParticipante.Substring(0, 4) == "9213")
                        {
                            strApoderadoNotarial +=
                                                dr["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + dr["vNroDocumento"].ToString() +
                                                ", DE NACIONALIDAD " + dr["vNacionalidadPais"].ToString().ToUpper() +
                                                ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                                ", CON DOMICILIO EN " + strDireccion + ", " + strDistCiu + " " + strCodigoPostal + ", " + strDptoCont + "; ";
                        }
                        else
                        {
                            strApoderadoNotarial +=
                                                dr["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + dr["vNroDocumento"].ToString() +
                                                ", DE NACIONALIDAD " + dr["vNacionalidadPais"].ToString().ToUpper() +
                                                ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                                ", CON DOMICILIO EN " + strDireccion + ", " + strDistCiu + ", " + strDptoCont + "; ";
                        }
                    }
                    else
                    {

                        strApoderadoNotarial +=
                                            dr["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + dr["vNroDocumento"].ToString() +
                                            ", DE NACIONALIDAD " + dr["vNacionalidadPais"].ToString().ToUpper() +
                                            ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                            ", CON DOMICILIO EN " + strDireccion + ", " + strDistCiu + ", " + strDptoCont + "; ";
                    }
                    //-----------------------------------------------------------

                    strGenero = Convert.ToInt32(dr["iGenero"]);

                    if (strGenero == (int)Enumerador.enmGenero.MASCULINO)
                    {
                        contadorApoderadoHombre = contadorApoderadoHombre + 1;
                    }
                    else
                    {
                        contadorApoderadoMujer = contadorApoderadoMujer + 1;
                    }
                    contadorApoderado++;

                    #endregion
                }


                //if (strTipoParticipante == "OTORGANTE" || strTipoParticipante == "PODERDANTE"
                //    || strTipoParticipante == "VENDEDOR"
                //    || strTipoParticipante == "ANTICIPANTE" || strTipoParticipante == "DONANTE")

                if (ObtenerIniciaRecibe(strTipoParticipante) == "INICIA")
                {
                    #region Otorgante, Vendedor, Anticipante

                    strParticipanteOtorgante += dr["vPersona"].ToString() + "<br>";


                    if (strGenero == (int)Enumerador.enmGenero.MASCULINO)
                    {
                        strArticuloPersonaSexo = "EL";
                        ContadorParticipanteHombres++;
                        strOtorganteGeneroDiscapacidad = "INCAPACITADO";
                        strOtorganteArticuloSexoTestigoRuego = "DEL";

                    }
                    else if (strGenero == (int)Enumerador.enmGenero.FEMENINO)
                    {
                        strArticuloPersonaSexo = "LA";
                        strOtorganteGeneroDiscapacidad = "INCAPACITADA";
                        strOtorganteArticuloSexoTestigoRuego = "DE LA";
                    }

                    contadorOtorgantes = contadorOtorgantes + 1;

                    if (contadorOtorgantes == 1)
                        vIdioma = vIdioma + dr["vIdioma"].ToString();
                    else
                        vIdioma = vIdioma + " Y " + dr["vIdioma"].ToString();


                    //vIdioma = dr["vIdioma"].ToString();


                    if (contadorOtorgantes == 1)
                    {
                        #region PrimerOtorgante

                        if (strGenero == (int)Enumerador.enmGenero.MASCULINO)
                        {
                            strOtorganteArticuloSexoPS = "EL";

                            //---------------------------------------------------------
                            //Fecha: 20/10/2021
                            //Autor: Miguel Márquez Beltrán
                            //Motivo: Cambiar el nombre del otorgante o poderdante de 
                            //          acuerdo al tipo de participante.
                            //Requerimiento: OBSERVACIONES_EP_19102021.
                            //---------------------------------------------------------
                            stroPoderdanteSingularPlural = strTipoParticipante;

                            if (strTipoParticipante == "OTORGANTE")
                            {
                                stroPoderdanteSingularPlural = "PODERDANTE";
                            }
                            

                            stroOtorganteEsSonSP = "ES";
                        }
                        else if (strGenero == (int)Enumerador.enmGenero.FEMENINO)
                        {
                            strOtorganteArticuloSexoPS = "LA";

                            //---------------------------------------------------------
                            //Fecha: 20/10/2021
                            //Autor: Miguel Márquez Beltrán
                            //Motivo: Cambiar el nombre del otorgante o poderdante de 
                            //          acuerdo al tipo de participante.
                            //Requerimiento: OBSERVACIONES_EP_19102021.
                            //---------------------------------------------------------
                            stroPoderdanteSingularPlural = strTipoParticipante;
                            if (strTipoParticipante == "OTORGANTE")
                            {
                                stroPoderdanteSingularPlural = "PODERDANTE";
                            }
                            if (strTipoParticipante == "VENDEDOR")
                            {
                                stroPoderdanteSingularPlural = "VENDEDORA";
                            }
                            stroOtorganteEsSonSP = "ES";
                        }
                        #endregion
                    }
                    else
                    {
                        if (strGenero == (int)Enumerador.enmGenero.MASCULINO)
                        {
                            #region Otorgante Masculino

                            strOtorganteArticuloSexoPS = "LOS";

                            stroPoderdanteSingularPlural = strTipoParticipante.Trim() + "S";

                            if (strTipoParticipante == "OTORGANTE")
                            {
                                stroPoderdanteSingularPlural = "PODERDANTES";
                            }

                            if (strTipoParticipante == "VENDEDOR")
                            {
                                stroPoderdanteSingularPlural = "VENDEDORES";
                            }


                            stroOtorganteEsSonSP = "SON";

                            #endregion
                        }
                        else if (strGenero == (int)Enumerador.enmGenero.FEMENINO)
                        {
                            #region Otorgante Femenino

                            if (ContadorParticipanteHombres > 0)
                            {
                                strOtorganteArticuloSexoPS = "LOS";
                                stroPoderdanteSingularPlural = strTipoParticipante.Trim() + "S";

                                if (strTipoParticipante == "OTORGANTE")
                                {
                                    stroPoderdanteSingularPlural = "PODERDANTES";
                                }
                                if (strTipoParticipante == "VENDEDOR")
                                {
                                    stroPoderdanteSingularPlural = "VENDEDORES";
                                }


                                stroOtorganteEsSonSP = "SON";
                            }
                            else
                            {
                                strOtorganteArticuloSexoPS = "LAS";
                                stroPoderdanteSingularPlural = strTipoParticipante.Trim() + "S";
                                if (strTipoParticipante == "OTORGANTE")
                                {
                                    stroPoderdanteSingularPlural = "PODERDANTES";
                                }
                                if (strTipoParticipante == "VENDEDOR")
                                {
                                    stroPoderdanteSingularPlural = "VENDEDORAS";
                                }


                                stroOtorganteEsSonSP = "SON";
                            }
                            #endregion
                        }
                    }

                    DataTable dtp = new DataTable();

                    //if (HttpContext.Current.Session["dtParticipantesAll"] == null)
                    //{
                    dtp = bl.ActonotarialObtenerParticipantes(Convert.ToInt64(jsonObject["ancu_iActoNotarialId"]));

                    //--------------------------------------------------------------------------
                    //Fecha: 06/04/2020
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Asignar los datos de la escritura a una sesión.
                    //--------------------------------------------------------------------------
                    //  HttpContext.Current.Session["dtParticipantesAll"] = dtp;
                    //--------------------------------------------------------------------------
                    //}
                    //else
                    //{
                    //    dtp = (DataTable)HttpContext.Current.Session["dtParticipantesAll"];
                    //}

                    DataTable dtp1 = new DataTable();
                    try
                    {
                        dtp1 = (from ps in dtp.AsEnumerable()
                                where Convert.ToInt64(ps["anpa_iReferenciaParticipanteId"]) == Convert.ToInt64(dr["anpa_iActoNotarialParticipanteId"].ToString())
                                select ps
                       ).CopyToDataTable();
                    }
                    catch
                    {
                        dtp1 = null;
                    }


                    if (dtp1 != null)
                    {
                        #region if (dtp1 != null)

                        string strDescTipoDocumento = "";

                        if (dr["peid_sDocumentoTipoId"].ToString() == Convert.ToString((int)Enumerador.enmTipoDocumento.OTROS))
                        {
                            strDescTipoDocumento = dr["peid_vTipoDocumento"].ToString().Trim();
                            if (strDescTipoDocumento.Length == 0)
                            {
                                strDescTipoDocumento = dr["vDescLargaTipDoc"].ToString().Trim();
                            }
                        }
                        else
                        {
                            strDescTipoDocumento = dr["vDescLargaTipDoc"].ToString().Trim();
                        }



                        //------------------------------------
                        //Fecha: 03/10/2019
                        //Autor: Miguel Márquez Beltrán
                        //------------------------------------
                        string strDocumentoLetras = Util.ObtenerUnidadesPalabrasComas(dr["vNroDocumento"].ToString(), true);



                        if (strUbigeoParticipante.Length == 6)
                        {
                            // Solo valido para Estados Unidos de América.
                            if (strUbigeoParticipante.Substring(0, 4) == "9213")
                            {
                                strParticipante +=
                                    "<p style='text-align:justify;'>" +
                                           dr["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + dr["vNroDocumento"].ToString() + " " + strDocumentoLetras +
                                           ", DE NACIONALIDAD " + dr["vNacionalidadPais"].ToString().ToUpper() +
                                           ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                           ", CON DOMICILIO EN " + strDireccion + ", " + strDistCiu + " " + strCodigoPostal + ", " + strDptoCont +
                                           "; A QUIEN PLENAMENTE IDENTIFICO Y QUIEN PROCEDE POR DERECHO PROPIO, DE LO QUE DOY FE Y SE ENCUENTRA " + strOtorganteGeneroDiscapacidad +
                                           " DE FIRMAR POR " + dr["pers_vDescripcionIncapacidad"] + ", POR LO QUE IMPRIME SU HUELLA DACTILAR E INTERVIENE:" +
                                          "</p>";
                            }
                            else
                            {
                                strParticipante +=
                                    "<p style='text-align:justify;'>" +
                                           dr["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + dr["vNroDocumento"].ToString() + " " + strDocumentoLetras +
                                           ", DE NACIONALIDAD " + dr["vNacionalidadPais"].ToString().ToUpper() +
                                           ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                           ", CON DOMICILIO EN " + strDireccion + ", " + strDistCiu + ", " + strDptoCont +
                                           "; A QUIEN PLENAMENTE IDENTIFICO Y QUIEN PROCEDE POR DERECHO PROPIO, DE LO QUE DOY FE Y SE ENCUENTRA " + strOtorganteGeneroDiscapacidad +
                                           " DE FIRMAR POR " + dr["pers_vDescripcionIncapacidad"] + ", POR LO QUE IMPRIME SU HUELLA DACTILAR E INTERVIENE:" +
                                          "</p>";
                            }
                        }
                        else
                        {
                            strParticipante +=
                                "<p style='text-align:justify;'>" +
                                       dr["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + dr["vNroDocumento"].ToString() + " " + strDocumentoLetras +
                                       ", DE NACIONALIDAD " + dr["vNacionalidadPais"].ToString().ToUpper() +
                                       ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                       ", CON DOMICILIO EN " + strDireccion + ", " + strDistCiu + ", " + strDptoCont +
                                       "; A QUIEN PLENAMENTE IDENTIFICO Y QUIEN PROCEDE POR DERECHO PROPIO, DE LO QUE DOY FE Y SE ENCUENTRA " + strOtorganteGeneroDiscapacidad +
                                       " DE FIRMAR POR " + dr["pers_vDescripcionIncapacidad"] + ", POR LO QUE IMPRIME SU HUELLA DACTILAR E INTERVIENE:" +
                                      "</p>";

                        }




                        if (dtp1.Rows.Count == 0)
                        {
                            if (strUbigeoParticipante.Length == 6)
                            {
                                // Solo valido para Estados Unidos de América.
                                if (strUbigeoParticipante.Substring(0, 4) == "9213")
                                {
                                    strOtorganteNotarial += dr["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + dr["vNroDocumento"].ToString() + " " + strDocumentoLetras +
                                   ", DE NACIONALIDAD " + dr["vNacionalidadPais"].ToString().ToUpper() +
                                   ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                   ", CON DOMICILIO EN " + strDireccion + ", " + strDistCiu + " " + strCodigoPostal + ", " + strDptoCont + "; ";

                                }
                                else
                                {
                                    strOtorganteNotarial += dr["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + dr["vNroDocumento"].ToString() + " " + strDocumentoLetras +
                                                            ", DE NACIONALIDAD " + dr["vNacionalidadPais"].ToString().ToUpper() +
                                                            ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                                            ", CON DOMICILIO EN " + strDireccion + ", " + strDistCiu + ", " + strDptoCont + "; ";
                                }
                            }
                            else
                            {
                                strOtorganteNotarial += dr["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + dr["vNroDocumento"].ToString() + " " + strDocumentoLetras +
                                                        ", DE NACIONALIDAD " + dr["vNacionalidadPais"].ToString().ToUpper() +
                                                        ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                                        ", CON DOMICILIO EN " + strDireccion + ", " + strDistCiu + ", " + strDptoCont + "; ";

                            }

                        }
                        else
                        {
                            if (strUbigeoParticipante.Length == 6)
                            {
                                // Solo valido para Estados Unidos de América.
                                if (strUbigeoParticipante.Substring(0, 4) == "9213")
                                {
                                    strOtorganteNotarial += dr["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + dr["vNroDocumento"].ToString() + " " + strDocumentoLetras +
                                                           ", DE NACIONALIDAD " + dr["vNacionalidadPais"].ToString().ToUpper() +
                                                           ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                                           ", CON DOMICILIO EN " + strDireccion + ", " + strDistCiu + " " + strCodigoPostal + ", " + strDptoCont + ", QUIEN CUENTA CON TESTIGO A RUEGO ";

                                }
                                else
                                {
                                    strOtorganteNotarial += dr["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + dr["vNroDocumento"].ToString() + " " + strDocumentoLetras +
                                                           ", DE NACIONALIDAD " + dr["vNacionalidadPais"].ToString().ToUpper() +
                                                           ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                                           ", CON DOMICILIO EN " + strDireccion + ", " + strDistCiu + ", " + strDptoCont + ", QUIEN CUENTA CON TESTIGO A RUEGO ";

                                }
                            }
                            else
                            {
                                strOtorganteNotarial += dr["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + dr["vNroDocumento"].ToString() + " " + strDocumentoLetras +
                                                       ", DE NACIONALIDAD " + dr["vNacionalidadPais"].ToString().ToUpper() +
                                                       ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                                       ", CON DOMICILIO EN " + strDireccion + ", " + strDistCiu + ", " + strDptoCont + ", QUIEN CUENTA CON TESTIGO A RUEGO ";

                            }
                        }


                        contadortestigoruego += 1;
                        foreach (DataRow drAUX in dtp1.Rows)
                        {
                            #region Foreach drAUX

                            strGenero = Convert.ToInt32(drAUX["iGenero"]);
                            stroOtorganteEsSonSP = "SON";

                            strEstadoCivil = drAUX["vEstadoCivil"].ToString().Trim();
                            strUbigeoParticipante = dr["cResidenciaUbigeo"].ToString().Trim();
                            strCodigoPostal = dr["resi_vCodigoPostal"].ToString().Trim();

                            if (string.IsNullOrEmpty(strEstadoCivil))
                            {
                                strEstadoCivil = "<ESTADO CIVIL>";
                            }
                            else
                            {
                                if (strGenero == (int)Enumerador.enmGenero.MASCULINO)
                                {
                                    #region Masculino

                                    strllamado_a = "LLAMADO";
                                    if (strEstadoCivil == "SOLTERO")
                                    {
                                        strEstadoCivil = "SOLTERO";
                                    }
                                    else if (strEstadoCivil == "CASADO")
                                    {
                                        strEstadoCivil = "CASADO";
                                    }
                                    else if (strEstadoCivil == "DIVORCIADO")
                                    {
                                        strEstadoCivil = "DIVORCIADO";
                                    }
                                    else if (strEstadoCivil == "VIUDO")
                                    {
                                        strEstadoCivil = "VIUDO";
                                    }

                                    strIdentificadoaPersonaSexo = "IDENTIFICADO";

                                    if (contadortestigoruego > 1)
                                    {
                                        strTestigoRuego = " Y LOS TESTIGOS A RUEGO ";
                                    }
                                    else
                                    {
                                        strTestigoRuego = " Y EL TESTIGO A RUEGO ";
                                    }

                                    contarhombrestestigo++;

                                    #endregion
                                }
                                else if (strGenero == (int)Enumerador.enmGenero.FEMENINO)
                                {
                                    #region Femenino
                                    strllamado_a = "LLAMADA";
                                    if (strEstadoCivil == "SOLTERO")
                                    {
                                        strEstadoCivil = "SOLTERA";
                                    }
                                    else if (strEstadoCivil == "CASADO")
                                    {
                                        strEstadoCivil = "CASADA";
                                    }
                                    else if (strEstadoCivil == "DIVORCIADO")
                                    {
                                        strEstadoCivil = "DIVORCIADA";
                                    }
                                    else if (strEstadoCivil == "VIUDO")
                                    {
                                        strEstadoCivil = "VIUDA";
                                    }
                                    ContadorParticipanteMujeres++;
                                    strIdentificadoaPersonaSexo = "IDENTIFICADA";

                                    if (contadortestigoruego >= 1)
                                    {
                                        strTestigoRuego = " Y LOS TESTIGOS A RUEGO ";
                                    }
                                    else
                                    {
                                        strTestigoRuego = " Y LA TESTIGO A RUEGO ";
                                    }

                                    contarmuejerestestigo++;

                                    #endregion
                                }
                            }


                            strOcupacion = drAUX["vOcupacion"].ToString().Trim();
                            if (string.IsNullOrEmpty(strEstadoCivil))
                            {
                                strOcupacion = "<OCUPACIÓN>";
                            }

                            strDireccion = drAUX["vDireccion"].ToString().Trim().ToUpper();
                            if (string.IsNullOrEmpty(strDireccion))
                            {
                                strDireccion = "<DIRECCION>";
                            }

                            strDistCiu = drAUX["DistCiu"].ToString().Trim();
                            if (string.IsNullOrEmpty(strDistCiu))
                            {
                                strDistCiu = "<DISTRITO>";
                            }

                            strProvPais = drAUX["ProvPais"].ToString().Trim();
                            if (string.IsNullOrEmpty(strProvPais))
                            {
                                strProvPais = "<PAIS>";
                            }

                            strDptoCont = drAUX["DptoCont"].ToString().Trim();
                            if (string.IsNullOrEmpty(strDptoCont))
                            {
                                strDptoCont = "<DEPARTAMENTO>";
                            }

                            strDescTipoDocumento = "";

                            if (drAUX["peid_sDocumentoTipoId"].ToString() == Convert.ToString((int)Enumerador.enmTipoDocumento.OTROS))
                            {
                                strDescTipoDocumento = drAUX["peid_vTipoDocumento"].ToString().Trim();
                                if (strDescTipoDocumento.Length == 0)
                                {
                                    strDescTipoDocumento = drAUX["vDescLargaTipDoc"].ToString().Trim();
                                }
                            }
                            else
                            {
                                strDescTipoDocumento = drAUX["vDescLargaTipDoc"].ToString().Trim();
                            }

                            string strDescOtorgante = "PODERDANTE";

                            if (strSubTipoActoNotarialDesc == "COMPRA - VENTA")
                                strDescOtorgante = "VENDEDOR";
                            if (strSubTipoActoNotarialDesc == "DONACIÓN")
                                strDescOtorgante = "DONANTE";
                            if (strSubTipoActoNotarialDesc == "ADELANTO DE LEGÍTIMA")
                                strDescOtorgante = "ANTICIPANTE";



                            strDocumentoLetras = Util.ObtenerUnidadesPalabrasComas(drAUX["vNroDocumento"].ToString(), true);


                            if (strUbigeoParticipante.Length == 6)
                            {
                                // Solo valido para Estados Unidos de América.
                                if (strUbigeoParticipante.Substring(0, 4) == "9213")
                                {
                                    strParticipante += "<p style='text-align:justify;'>" +
                                                drAUX["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + drAUX["vNroDocumento"].ToString() + " " + strDocumentoLetras +
                                                ", DE NACIONALIDAD " + drAUX["vNacionalidadPais"].ToString().ToUpper() +
                                                ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                                ", CON DOMICILIO EN " + strDireccion + ", " + strDistCiu + " " + strCodigoPostal + ", " + strDptoCont +
                                                "; QUIEN PROCEDE EN CALIDAD DE TESTIGO A RUEGO " + strOtorganteArticuloSexoTestigoRuego + " " + strDescOtorgante +
                                                ", DE CONFORMIDAD CON EL INCISO G) DEL ARTÍCULO 54° DEL DECRETO LEGISLATIVO N° 1049." +
                                                "</p>";

                                    strOtorganteNotarial += strllamado_a + " " + drAUX["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + drAUX["vNroDocumento"].ToString() + " " + strDocumentoLetras +
                                                        ", DE NACIONALIDAD " + drAUX["vNacionalidadPais"].ToString().ToUpper() +
                                                        ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                                        ", CON DOMICILIO EN " + strDireccion + ", " + strProvPais + " " + strCodigoPostal + ", " + strDptoCont + "; ";
                                }
                                else
                                {
                                    strParticipante += "<p style='text-align:justify;'>" +
                                                drAUX["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + drAUX["vNroDocumento"].ToString() + " " + strDocumentoLetras +
                                                ", DE NACIONALIDAD " + drAUX["vNacionalidadPais"].ToString().ToUpper() +
                                                ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                                ", CON DOMICILIO EN " + strDireccion + ", " + strDistCiu + ", " + strDptoCont +
                                                "; QUIEN PROCEDE EN CALIDAD DE TESTIGO A RUEGO " + strOtorganteArticuloSexoTestigoRuego + " " + strDescOtorgante +
                                                ", DE CONFORMIDAD CON EL INCISO G) DEL ARTÍCULO 54° DEL DECRETO LEGISLATIVO N° 1049." +
                                                "</p>";

                                    strOtorganteNotarial += strllamado_a + " " + drAUX["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + drAUX["vNroDocumento"].ToString() + " " + strDocumentoLetras +
                                                        ", DE NACIONALIDAD " + drAUX["vNacionalidadPais"].ToString().ToUpper() +
                                                        ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                                        ", CON DOMICILIO EN " + strDireccion + ", " + strDistCiu + ", " + strDptoCont + "; ";

                                }
                            }
                            else
                            {
                                strParticipante += "<p style='text-align:justify;'>" +
                                            drAUX["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + drAUX["vNroDocumento"].ToString() + " " + strDocumentoLetras +
                                            ", DE NACIONALIDAD " + drAUX["vNacionalidadPais"].ToString().ToUpper() +
                                            ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                            ", CON DOMICILIO EN " + strDireccion + ", " + strDistCiu + ", " + strDptoCont +
                                            "; QUIEN PROCEDE EN CALIDAD DE TESTIGO A RUEGO " + strOtorganteArticuloSexoTestigoRuego + " " + strDescOtorgante +
                                            ", DE CONFORMIDAD CON EL INCISO G) DEL ARTÍCULO 54° DEL DECRETO LEGISLATIVO N° 1049." +
                                            "</p>";

                                strOtorganteNotarial += strllamado_a + " " + drAUX["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + drAUX["vNroDocumento"].ToString() + " " + strDocumentoLetras +
                                                    ", DE NACIONALIDAD " + drAUX["vNacionalidadPais"].ToString().ToUpper() +
                                                    ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                                    ", CON DOMICILIO EN " + strDireccion + ", " + strDistCiu + ", " + strDptoCont + "; ";

                            }


                            #endregion
                        }

                        string vidiomaP = dr["pers_sIdiomaNatalId"].ToString();
                        string vNomIdiomaP = dr["vIdioma"].ToString();
                        if (vidiomaP != sIdiomaCastellano)
                        {
                            strDescTipoDocumento = "";

                            foreach (DataRow dr1 in dts.Rows)
                            {
                                if (dr1["vTipoParticipanteId"].ToString() == Convert.ToString(Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE) && dr1["pers_sIdiomaNatalId"].ToString() == vidiomaP)
                                {
                                    #region Tnterprete

                                    string identificacion = Convert.ToInt32(dr1["iGenero"]).ToString() == Convert.ToString((int)Enumerador.enmGenero.MASCULINO) ? "IDENTIFICADO" : "IDENTIFICADA";

                                    if (dr1["peid_sDocumentoTipoId"].ToString() == Convert.ToString((int)Enumerador.enmTipoDocumento.OTROS))
                                    {
                                        strDescTipoDocumento = dr1["peid_vTipoDocumento"].ToString().Trim();
                                        if (strDescTipoDocumento.Length == 0)
                                        {
                                            strDescTipoDocumento = dr1["vDescLargaTipDoc"].ToString().Trim();
                                        }
                                    }
                                    else
                                    {
                                        strDescTipoDocumento = dr1["vDescLargaTipDoc"].ToString().Trim();
                                    }
                                    string strDescOtorgante = "PODERDANTE";

                                    if (strSubTipoActoNotarialDesc == "COMPRA - VENTA")
                                        strDescOtorgante = "VENDEDOR";
                                    if (strSubTipoActoNotarialDesc == "DONACIÓN")
                                        strDescOtorgante = "DONANTE";
                                    if (strSubTipoActoNotarialDesc == "ADELANTO DE LEGÍTIMA")
                                        strDescOtorgante = "ANTICIPANTE";


                                    //strParticipanteInterprete =
                                    //    "<p style='text-align:justify;'>" +
                                    //    " " + strArticuloPersonaSexo + " " + strDescOtorgante + " ES HÁBIL EN EL IDIOMA " + vNomIdiomaP + ", DECLARANDO NO CONOCER EL " +
                                    //    "IDIOMA CASTELLANO, POR LO QUE DESIGNA A: " +
                                    //    dr1["vPersona"].ToString() + ", " + identificacion + " CON " + strDescTipoDocumento + " Nº " + dr1["vnroDocumento"].ToString() +
                                    //    ", DE NACIONALIDAD " + dr1["vNacionalidadPais"].ToString().ToUpper() +
                                    //    ", DE  ESTADO CIVIL " + dr1["vEstadoCivil"].ToString() + ", DE OCUPACIÓN " + dr1["vOcupacion"].ToString() +
                                    //    ", CON DOMICILIO EN " + dr1["vDireccion"].ToString() + ", " + dr1["DistCiu"].ToString() + ", " + dr1["DptoCont"].ToString() +
                                    //    "; QUIEN PROCEDE EN CALIDAD DE INTÉRPRETE DE CONFORMIDAD CON EL INCISO F) DEL ARTÍCULO 54° DEL " +
                                    //    "DECRETO LEGISLATIVO Nº 1049, Y MANIFIESTA SER HÁBIL EN EL IDIOMA " +
                                    //    dr1["vIdioma"].ToString() + " Y EL CASTELLANO, ASÍ COMO TENER EL CONOCIMIENTO Y EXPERIENCIA " +
                                    //    "SUFICIENTE PARA EFECTUAR LA TRADUCCIÓN QUE LE HA SIDO SOLICITADA. DECLARANDO ADEMÁS QUE LA " +
                                    //    "PRESENTE ESCRITURA PÚBLICA RECOGE FIELMENTE LA VOLUNTAD DEL PODERDANTE." +
                                    //    "</p>";

                                    //-----------------------------------------------------------------------------------------------------------
                                    //Fecha: 25/02/2020
                                    //Autor: Miguel Márquez Beltrán
                                    //Motivo:
                                    //Página 25
                                    //Debería decir: EL OTORGANTE ES HÁBIL EN EL IDIOMA CHINO, DECLARANDO NO CONOCER EL IDIOMA CASTELLANO, 
                                    //POR LO QUE DESIGNA A:=====================================================================
                                    //ANDRES PALERMO TRUJILLO, DE NACIONALIDAD PERUANA, IDENTIFICADO CON …….,  DE ESTADO CIVIL…, DE OCUPACIÓN….., CON DOMICILIO…..;
                                    //-----------------------------------------------------------------------------------------------------------
                                    strUbigeoParticipante = dr1["cResidenciaUbigeo"].ToString().Trim();
                                    strCodigoPostal = dr1["resi_vCodigoPostal"].ToString().Trim();
                                    strDptoCont = dr1["DptoCont"].ToString().Trim();
                                    strProvPais = dr1["ProvPais"].ToString().Trim();
                                    strDistCiu = dr1["DistCiu"].ToString().Trim();

                                    if (strUbigeoParticipante.Length == 6)
                                    {
                                        // Solo valido para Estados Unidos de América.
                                        if (strUbigeoParticipante.Substring(0, 4) == "9213")
                                        {
                                            strParticipanteInterprete =
                                                "<p style='text-align:justify;'>" +
                                                " " + strArticuloPersonaSexo + " " + strDescOtorgante + " ES HÁBIL EN EL IDIOMA " + vNomIdiomaP + ", DECLARANDO NO CONOCER EL " +
                                                "IDIOMA CASTELLANO, POR LO QUE DESIGNA A: " + dr1["vPersona"].ToString() +
                                                ", DE NACIONALIDAD " + dr1["vNacionalidadPais"].ToString().ToUpper() +
                                                ", " + identificacion + " CON " + strDescTipoDocumento + " Nº " + dr1["vnroDocumento"].ToString() +
                                                ", DE  ESTADO CIVIL " + dr1["vEstadoCivil"].ToString() + ", DE OCUPACIÓN " + dr1["vOcupacion"].ToString() +
                                                ", CON DOMICILIO EN " + dr1["vDireccion"].ToString() + ", " + strDistCiu + " " + strCodigoPostal + ", " + strDptoCont +
                                                "; QUIEN PROCEDE EN CALIDAD DE INTÉRPRETE DE CONFORMIDAD CON EL INCISO F) DEL ARTÍCULO 54° DEL " +
                                                "DECRETO LEGISLATIVO Nº 1049, Y MANIFIESTA SER HÁBIL EN EL IDIOMA " +
                                                dr1["vIdioma"].ToString() + " Y EL CASTELLANO, ASÍ COMO TENER EL CONOCIMIENTO Y EXPERIENCIA " +
                                                "SUFICIENTE PARA EFECTUAR LA TRADUCCIÓN QUE LE HA SIDO SOLICITADA. DECLARANDO ADEMÁS QUE LA " +
                                                "PRESENTE ESCRITURA PÚBLICA RECOGE FIELMENTE LA VOLUNTAD DEL PODERDANTE." +
                                                "</p>";
                                        }
                                        else
                                        {
                                            strParticipanteInterprete =
                                                "<p style='text-align:justify;'>" +
                                                " " + strArticuloPersonaSexo + " " + strDescOtorgante + " ES HÁBIL EN EL IDIOMA " + vNomIdiomaP + ", DECLARANDO NO CONOCER EL " +
                                                "IDIOMA CASTELLANO, POR LO QUE DESIGNA A: " + dr1["vPersona"].ToString() +
                                                ", DE NACIONALIDAD " + dr1["vNacionalidadPais"].ToString().ToUpper() +
                                                ", " + identificacion + " CON " + strDescTipoDocumento + " Nº " + dr1["vnroDocumento"].ToString() +
                                                ", DE  ESTADO CIVIL " + dr1["vEstadoCivil"].ToString() + ", DE OCUPACIÓN " + dr1["vOcupacion"].ToString() +
                                                ", CON DOMICILIO EN " + dr1["vDireccion"].ToString() + ", " + dr1["DistCiu"].ToString() + ", " + dr1["DptoCont"].ToString() +
                                                "; QUIEN PROCEDE EN CALIDAD DE INTÉRPRETE DE CONFORMIDAD CON EL INCISO F) DEL ARTÍCULO 54° DEL " +
                                                "DECRETO LEGISLATIVO Nº 1049, Y MANIFIESTA SER HÁBIL EN EL IDIOMA " +
                                                dr1["vIdioma"].ToString() + " Y EL CASTELLANO, ASÍ COMO TENER EL CONOCIMIENTO Y EXPERIENCIA " +
                                                "SUFICIENTE PARA EFECTUAR LA TRADUCCIÓN QUE LE HA SIDO SOLICITADA. DECLARANDO ADEMÁS QUE LA " +
                                                "PRESENTE ESCRITURA PÚBLICA RECOGE FIELMENTE LA VOLUNTAD DEL PODERDANTE." +
                                                "</p>";

                                        }
                                    }
                                    else
                                    {
                                        strParticipanteInterprete =
                                            "<p style='text-align:justify;'>" +
                                            " " + strArticuloPersonaSexo + " " + strDescOtorgante + " ES HÁBIL EN EL IDIOMA " + vNomIdiomaP + ", DECLARANDO NO CONOCER EL " +
                                            "IDIOMA CASTELLANO, POR LO QUE DESIGNA A: " + dr1["vPersona"].ToString() +
                                            ", DE NACIONALIDAD " + dr1["vNacionalidadPais"].ToString().ToUpper() +
                                            ", " + identificacion + " CON " + strDescTipoDocumento + " Nº " + dr1["vnroDocumento"].ToString() +
                                            ", DE  ESTADO CIVIL " + dr1["vEstadoCivil"].ToString() + ", DE OCUPACIÓN " + dr1["vOcupacion"].ToString() +
                                            ", CON DOMICILIO EN " + dr1["vDireccion"].ToString() + ", " + dr1["DistCiu"].ToString() + ", " + dr1["DptoCont"].ToString() +
                                            "; QUIEN PROCEDE EN CALIDAD DE INTÉRPRETE DE CONFORMIDAD CON EL INCISO F) DEL ARTÍCULO 54° DEL " +
                                            "DECRETO LEGISLATIVO Nº 1049, Y MANIFIESTA SER HÁBIL EN EL IDIOMA " +
                                            dr1["vIdioma"].ToString() + " Y EL CASTELLANO, ASÍ COMO TENER EL CONOCIMIENTO Y EXPERIENCIA " +
                                            "SUFICIENTE PARA EFECTUAR LA TRADUCCIÓN QUE LE HA SIDO SOLICITADA. DECLARANDO ADEMÁS QUE LA " +
                                            "PRESENTE ESCRITURA PÚBLICA RECOGE FIELMENTE LA VOLUNTAD DEL PODERDANTE." +
                                            "</p>";

                                    }



                                    strParticipante += strParticipanteInterprete;
                                    break;

                                    #endregion
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region if (dtp1 == null)

                        string strDescTipoDocumento = "";

                        if (dr["peid_sDocumentoTipoId"].ToString() == Convert.ToString((int)Enumerador.enmTipoDocumento.OTROS))
                        {
                            strDescTipoDocumento = dr["peid_vTipoDocumento"].ToString().Trim();
                            if (strDescTipoDocumento.Length == 0)
                            {
                                strDescTipoDocumento = dr["vDescLargaTipDoc"].ToString().Trim();
                            }
                        }
                        else
                        {
                            strDescTipoDocumento = dr["vDescLargaTipDoc"].ToString().Trim();
                        }

                        string strDocumentoLetras = Util.ObtenerUnidadesPalabrasComas(dr["vNroDocumento"].ToString(), true);


                        if (strUbigeoParticipante.Length == 6)
                        {
                            // Solo valido para Estados Unidos de América.
                            if (strUbigeoParticipante.Substring(0, 4) == "9213")
                            {
                                strParticipante += "<p style='text-align:justify;'>" +
                                                   dr["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + dr["vNroDocumento"].ToString() + " " + strDocumentoLetras +
                                                   ", DE NACIONALIDAD " + dr["vNacionalidadPais"].ToString().ToUpper() +
                                                   ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                                   ", CON DOMICILIO EN " + strDireccion + ", " + strDistCiu + " " + strCodigoPostal + ", " + strDptoCont;


                                strOtorganteNotarial += dr["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + dr["vNroDocumento"].ToString() + " " + strDocumentoLetras +
                                                        ", DE NACIONALIDAD " + dr["vNacionalidadPais"].ToString().ToUpper() +
                                                        ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                                        ", CON DOMICILIO EN " + strDireccion + ", " + strProvPais + " " + strCodigoPostal + ", " + strDptoCont + "; ";

                            }
                            else
                            {
                                strParticipante += "<p style='text-align:justify;'>" +
                                                   dr["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + dr["vNroDocumento"].ToString() + " " + strDocumentoLetras +
                                                   ", DE NACIONALIDAD " + dr["vNacionalidadPais"].ToString().ToUpper() +
                                                   ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                                   ", CON DOMICILIO EN " + strDireccion + ", " + strDistCiu + ", " + strDptoCont;


                                strOtorganteNotarial += dr["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + dr["vNroDocumento"].ToString() + " " + strDocumentoLetras +
                                                        ", DE NACIONALIDAD " + dr["vNacionalidadPais"].ToString().ToUpper() +
                                                        ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                                        ", CON DOMICILIO EN " + strDireccion + ", " + strDistCiu + ", " + strDptoCont + "; ";

                            }
                        }
                        else
                        {
                            strParticipante += "<p style='text-align:justify;'>" +
                                               dr["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + dr["vNroDocumento"].ToString() + " " + strDocumentoLetras +
                                               ", DE NACIONALIDAD " + dr["vNacionalidadPais"].ToString().ToUpper() +
                                               ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                               ", CON DOMICILIO EN " + strDireccion + ", " + strDistCiu + ", " + strDptoCont;


                            strOtorganteNotarial += dr["vPersona"] + ", " + strIdentificadoaPersonaSexo + " CON " + strDescTipoDocumento + " Nº " + dr["vNroDocumento"].ToString() + " " + strDocumentoLetras +
                                                    ", DE NACIONALIDAD " + dr["vNacionalidadPais"].ToString().ToUpper() +
                                                    ", DE ESTADO CIVIL " + strEstadoCivil + ", DE OCUPACIÓN " + strOcupacion +
                                                    ", CON DOMICILIO EN " + strDireccion + ", " + strDistCiu + ", " + strDptoCont + "; ";

                        }


                        //------------------------------------------------------------------
                        //Fecha: 29/03/2017
                        //Autor: Miguel Márquez Beltrán
                        //------------------------------------------------------------------
                        string strIdentifico = "";
                        strIdentifico = "; A QUIEN PLENAMENTE IDENTIFICO Y QUIEN PROCEDE POR DERECHO PROPIO, DE LO QUE DOY FE.";

                        strParticipante += strIdentifico;


                        string vidiomaP = dr["pers_sIdiomaNatalId"].ToString();
                        string vNomIdiomaP = dr["vIdioma"].ToString();
                        if (vidiomaP != sIdiomaCastellano)
                        {
                            strDescTipoDocumento = "";

                            foreach (DataRow dr1 in dts.Rows)
                            {
                                if (dr1["vTipoParticipanteId"].ToString() == Convert.ToString(Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE) && dr1["pers_sIdiomaNatalId"].ToString() == vidiomaP)
                                {
                                    #region Interprete

                                    string identificacion = Convert.ToInt32(dr1["iGenero"]).ToString() == Convert.ToString((int)Enumerador.enmGenero.MASCULINO) ? "IDENTIFICADO" : "IDENTIFICADA";

                                    if (dr1["peid_sDocumentoTipoId"].ToString() == Convert.ToString((int)Enumerador.enmTipoDocumento.OTROS))
                                    {
                                        strDescTipoDocumento = dr1["peid_vTipoDocumento"].ToString().Trim();
                                        if (strDescTipoDocumento.Length == 0)
                                        {
                                            strDescTipoDocumento = dr1["vDescLargaTipDoc"].ToString().Trim();
                                        }
                                    }
                                    else
                                    {
                                        strDescTipoDocumento = dr1["vDescLargaTipDoc"].ToString().Trim();
                                    }
                                    string strDescOtorgante = "PODERDANTE";

                                    if (strSubTipoActoNotarialDesc == "COMPRA - VENTA")
                                        strDescOtorgante = "VENDEDOR";
                                    if (strSubTipoActoNotarialDesc == "DONACIÓN")
                                        strDescOtorgante = "DONANTE";
                                    if (strSubTipoActoNotarialDesc == "ADELANTO DE LEGÍTIMA")
                                        strDescOtorgante = "ANTICIPANTE";

                                    //strParticipanteInterprete =
                                    //    "<p style='text-align:justify;'>" +
                                    //    " " + strArticuloPersonaSexo + " " + strDescOtorgante + " ES HÁBIL EN EL IDIOMA " + vNomIdiomaP + ", DECLARANDO NO CONOCER EL " +
                                    //    "IDIOMA CASTELLANO, POR LO QUE DESIGNA A: " + dr1["vPersona"].ToString() +
                                    //    ", " + identificacion + " CON " + strDescTipoDocumento + " Nº " + dr1["vnroDocumento"].ToString() +
                                    //    ", DE NACIONALIDAD " + dr1["vNacionalidadPais"].ToString().ToUpper() +
                                    //    ", DE ESTADO CIVIL " + dr1["vEstadoCivil"].ToString() + ", DE OCUPACIÓN " + dr1["vOcupacion"].ToString() +
                                    //    ", CON DOMICILIO EN " + dr1["vDireccion"].ToString() + ", " + dr1["DistCiu"].ToString() + ", " + dr1["DptoCont"].ToString() +
                                    //    "; QUIEN PROCEDE EN CALIDAD DE INTÉRPRETE DE CONFORMIDAD CON EL INCISO F) DEL ARTÍCULO 54° DEL " +
                                    //    "DECRETO LEGISLATIVO Nº 1049, Y MANIFIESTA SER HÁBIL EN EL IDIOMA " +
                                    //     dr1["vIdioma"].ToString() + " Y EL CASTELLANO, ASÍ COMO TENER EL CONOCIMIENTO Y EXPERIENCIA " +
                                    //    "SUFICIENTE PARA EFECTUAR LA TRADUCCIÓN QUE LE HA SIDO SOLICITADA. DECLARANDO ADEMÁS QUE LA " +
                                    //    "PRESENTE ESCRITURA PÚBLICA RECOGE FIELMENTE LA VOLUNTAD DEL PODERDANTE." +
                                    //    "</p>";
                                    //-----------------------------------------------------------------------------------------------------------
                                    //Fecha: 25/02/2020
                                    //Autor: Miguel Márquez Beltrán
                                    //Motivo:
                                    //Página 25
                                    //Debería decir: EL OTORGANTE ES HÁBIL EN EL IDIOMA CHINO, DECLARANDO NO CONOCER EL IDIOMA CASTELLANO, 
                                    //POR LO QUE DESIGNA A:=====================================================================
                                    //ANDRES PALERMO TRUJILLO, DE NACIONALIDAD PERUANA, IDENTIFICADO CON …….,  DE ESTADO CIVIL…, DE OCUPACIÓN….., CON DOMICILIO…..;
                                    //-----------------------------------------------------------------------------------------------------------
                                    strUbigeoParticipante = dr1["cResidenciaUbigeo"].ToString().Trim();
                                    strCodigoPostal = dr1["resi_vCodigoPostal"].ToString().Trim();
                                    strDptoCont = dr1["DptoCont"].ToString().Trim();
                                    strProvPais = dr1["ProvPais"].ToString().Trim();
                                    strDistCiu = dr1["DistCiu"].ToString().Trim();

                                    if (strUbigeoParticipante.Length == 6)
                                    {
                                        // Solo valido para Estados Unidos de América.
                                        if (strUbigeoParticipante.Substring(0, 4) == "9213")
                                        {
                                            strParticipanteInterprete =
                                                "<p style='text-align:justify;'>" +
                                                " " + strArticuloPersonaSexo + " " + strDescOtorgante + " ES HÁBIL EN EL IDIOMA " + vNomIdiomaP + ", DECLARANDO NO CONOCER EL " +
                                                "IDIOMA CASTELLANO, POR LO QUE DESIGNA A: " + dr1["vPersona"].ToString() +
                                                ", DE NACIONALIDAD " + dr1["vNacionalidadPais"].ToString().ToUpper() +
                                                ", " + identificacion + " CON " + strDescTipoDocumento + " Nº " + dr1["vnroDocumento"].ToString() +
                                                ", DE ESTADO CIVIL " + dr1["vEstadoCivil"].ToString() + ", DE OCUPACIÓN " + dr1["vOcupacion"].ToString() +
                                                ", CON DOMICILIO EN " + dr1["vDireccion"].ToString() + ", " + strDistCiu + " " + strCodigoPostal + ", " + dr1["DptoCont"].ToString() +
                                                "; QUIEN PROCEDE EN CALIDAD DE INTÉRPRETE DE CONFORMIDAD CON EL INCISO F) DEL ARTÍCULO 54° DEL " +
                                                "DECRETO LEGISLATIVO Nº 1049, Y MANIFIESTA SER HÁBIL EN EL IDIOMA " +
                                                 dr1["vIdioma"].ToString() + " Y EL CASTELLANO, ASÍ COMO TENER EL CONOCIMIENTO Y EXPERIENCIA " +
                                                "SUFICIENTE PARA EFECTUAR LA TRADUCCIÓN QUE LE HA SIDO SOLICITADA. DECLARANDO ADEMÁS QUE LA " +
                                                "PRESENTE ESCRITURA PÚBLICA RECOGE FIELMENTE LA VOLUNTAD DEL PODERDANTE." +
                                                "</p>";

                                        }
                                        else
                                        {
                                            strParticipanteInterprete =
                                                "<p style='text-align:justify;'>" +
                                                " " + strArticuloPersonaSexo + " " + strDescOtorgante + " ES HÁBIL EN EL IDIOMA " + vNomIdiomaP + ", DECLARANDO NO CONOCER EL " +
                                                "IDIOMA CASTELLANO, POR LO QUE DESIGNA A: " + dr1["vPersona"].ToString() +
                                                ", DE NACIONALIDAD " + dr1["vNacionalidadPais"].ToString().ToUpper() +
                                                ", " + identificacion + " CON " + strDescTipoDocumento + " Nº " + dr1["vnroDocumento"].ToString() +
                                                ", DE ESTADO CIVIL " + dr1["vEstadoCivil"].ToString() + ", DE OCUPACIÓN " + dr1["vOcupacion"].ToString() +
                                                ", CON DOMICILIO EN " + dr1["vDireccion"].ToString() + ", " + dr1["DistCiu"].ToString() + ", " + dr1["DptoCont"].ToString() +
                                                "; QUIEN PROCEDE EN CALIDAD DE INTÉRPRETE DE CONFORMIDAD CON EL INCISO F) DEL ARTÍCULO 54° DEL " +
                                                "DECRETO LEGISLATIVO Nº 1049, Y MANIFIESTA SER HÁBIL EN EL IDIOMA " +
                                                 dr1["vIdioma"].ToString() + " Y EL CASTELLANO, ASÍ COMO TENER EL CONOCIMIENTO Y EXPERIENCIA " +
                                                "SUFICIENTE PARA EFECTUAR LA TRADUCCIÓN QUE LE HA SIDO SOLICITADA. DECLARANDO ADEMÁS QUE LA " +
                                                "PRESENTE ESCRITURA PÚBLICA RECOGE FIELMENTE LA VOLUNTAD DEL PODERDANTE." +
                                                "</p>";

                                        }
                                    }
                                    else
                                    {
                                        strParticipanteInterprete =
                                            "<p style='text-align:justify;'>" +
                                            " " + strArticuloPersonaSexo + " " + strDescOtorgante + " ES HÁBIL EN EL IDIOMA " + vNomIdiomaP + ", DECLARANDO NO CONOCER EL " +
                                            "IDIOMA CASTELLANO, POR LO QUE DESIGNA A: " + dr1["vPersona"].ToString() +
                                            ", DE NACIONALIDAD " + dr1["vNacionalidadPais"].ToString().ToUpper() +
                                            ", " + identificacion + " CON " + strDescTipoDocumento + " Nº " + dr1["vnroDocumento"].ToString() +
                                            ", DE ESTADO CIVIL " + dr1["vEstadoCivil"].ToString() + ", DE OCUPACIÓN " + dr1["vOcupacion"].ToString() +
                                            ", CON DOMICILIO EN " + dr1["vDireccion"].ToString() + ", " + dr1["DistCiu"].ToString() + ", " + dr1["DptoCont"].ToString() +
                                            "; QUIEN PROCEDE EN CALIDAD DE INTÉRPRETE DE CONFORMIDAD CON EL INCISO F) DEL ARTÍCULO 54° DEL " +
                                            "DECRETO LEGISLATIVO Nº 1049, Y MANIFIESTA SER HÁBIL EN EL IDIOMA " +
                                             dr1["vIdioma"].ToString() + " Y EL CASTELLANO, ASÍ COMO TENER EL CONOCIMIENTO Y EXPERIENCIA " +
                                            "SUFICIENTE PARA EFECTUAR LA TRADUCCIÓN QUE LE HA SIDO SOLICITADA. DECLARANDO ADEMÁS QUE LA " +
                                            "PRESENTE ESCRITURA PÚBLICA RECOGE FIELMENTE LA VOLUNTAD DEL PODERDANTE." +
                                            "</p>";

                                    }


                                    strParticipante += strParticipanteInterprete;
                                    break;

                                    #endregion
                                }
                            }
                        }
                        strParticipante += "</p>";

                        #endregion
                    }

                    #endregion

                }


                contadorParticipantes = contadorParticipantes + 1;


                #endregion

            }

            //MANEJO DEL SINGULAR-PLURAL DE LOS PARTICIPANTES EN GENERAL
            if (contadorParticipantes == 1)
            {
                strParticipanteQuienSP = "QUIEN";
                strParticipanteElSP = "EL";
                strParticipanteCompareceSP = "COMPARECE";
            }
            else if (contadorParticipantes > 1)
            {
                strParticipanteQuienSP = "QUIENES";
                strParticipanteElSP = "LES";
                strParticipanteCompareceSP = "COMPARECEN";
            }

            //MANEJO DEL SINGULAR-PLURAL DE LOS OTORGANTES
            if (contadorOtorgantes == 1)
            {
                stroOtorganteInteligenteSP = "INTELIGENTE";
                strOtorganteObligaSP = "OBLIGA";
                strOtorganteMayorSP = "MAYOR";
                strOtorganteOtorgaSP = "OTORGA";
                str_participante_piden_plural = "PIDIÓ";
                strParticipanteCompareceSP = "COMPARECE";
                //----------------------------------------------
                //Fecha: 07/08/2020
                //Autor: Miguel Márquez Beltrán
                //Motivo: Poner la pluralidad cuando tiene 
                //          por lo menos un testigo a ruego.
                //  De acuerdo al documento: observaciones_EP_27072020
                //  Nro. 5.
                //----------------------------------------------
                if (contadortestigoruego > 0)
                {
                    strOtorganteObligaSP = "OBLIGAN";
                    strOtorganteOtorgaSP = "OTORGAN";
                }
                //----------------------------------------------
            }
            else
            {
                stroOtorganteInteligenteSP = "INTELIGENTES";
                strOtorganteObligaSP = "OBLIGAN";
                strOtorganteMayorSP = "MAYORES";
                strOtorganteOtorgaSP = "OTORGAN";
                str_participante_piden_plural = "PIDIERON";
                strParticipanteCompareceSP = "COMPARECEN";
            }

            if (contadorApoderado <= 1)
            {
                strApoderadoOtorgaSP = "OTORGA";
            }
            else
            {
                strApoderadoOtorgaSP = "OTORGAN";
            }

            if (contadortestigoruego >= 1)
            {
                strOtorganteMayorSP = "MAYORES";
                stroOtorganteInteligenteSP = "INTELIGENTES";
            }

            #region Asignar testigo a ruego

            if (contarhombrestestigo == 1 && contarmuejerestestigo == 0)
            {
                strTestigoRuego = " Y EL TESTIGO A RUEGO ";
            }
            else
            {

                if (contarhombrestestigo == 0 && contarmuejerestestigo == 1)
                {
                    strTestigoRuego = " Y LA TESTIGO A RUEGO ";
                }
                else
                {
                    if (contarhombrestestigo > 1 && contarmuejerestestigo == 0)
                    {
                        strTestigoRuego = " Y LOS TESTIGOS A RUEGO ";
                    }
                    else
                    {
                        if (contarhombrestestigo == 0 && contarmuejerestestigo > 1)
                        {
                            strTestigoRuego = " Y LAS TESTIGOS A RUEGO ";
                        }
                        else
                        {
                            if (contarhombrestestigo >= 1 && contarmuejerestestigo >= 1)
                            {
                                strTestigoRuego = " Y LOS TESTIGOS A RUEGO ";
                            }
                            else
                            {
                                strTestigoRuego = "";
                            }
                        }
                    }
                }

            }

            #endregion

            StringBuilder sScript = new StringBuilder();


            string strLetrasNumeroEscrituraPublica = "CERO";
            string strLibroRomanos = "LIBR000";

            if (strNroEscrituraPublica.Length == 0)
            {
                strNroEscrituraPublica = "MRE000";
            }

            sScript.Append("<p style='text-align:justify;font-weight:bold;'>");
            sScript.Append("ESCRITURA PÚBLICA NÚMERO " + strLetrasNumeroEscrituraPublica + "(" + strNroEscrituraPublica + ")" + " (LIBRO " + strLibroRomanos + ", AÑO " + strAnioEscritura + ").");
            sScript.Append("</p>");

            sScript.Append("<p style='text-align:justify;'>");
            sScript.Append("EN LA CIUDAD DE " + strUbigeoOficinaConsularCiudad + ", " + strFecha.Trim() + ", ANTE ");
            sScript.Append("MÍ, " + strFuncionarioAutorizador + ", " + strCargoFuncionarioAutorizador + " DEL PERÚ EN " + strUbigeoOficinaConsular + "; " + strParticipanteCompareceSP + ":");
            sScript.Append("</p>");

            sScript.Append(strParticipante);


            sScript.Append("<p style='text-align:justify;'>");

            sScript.Append(strOtorganteArticuloSexoPS + " " + stroPoderdanteSingularPlural + strTestigoRuego + " " + stroOtorganteEsSonSP + " " + strOtorganteMayorSP + " DE EDAD");

            sScript.Append(", ");


            if (strParticipanteInterprete.Length == 0)
            {
                sScript.Append(stroOtorganteInteligenteSP + " EN EL IDIOMA CASTELLANO, ");
            }
            else
            {
                sScript.Append(stroOtorganteInteligenteSP + " EN EL IDIOMA ");
                sScript.Append(vIdioma + " POR LO QUE INTERVIENE UN INTÉRPRETE, ");
            }

            sScript.Append("SE " + strOtorganteObligaSP + " ");
            sScript.Append("CON CAPACIDAD, LIBERTAD Y PLENO CONOCIMIENTO DEL ACTO JURÍDICO ");
            sScript.Append("QUE " + strOtorganteOtorgaSP + ", DE LO QUE DOY FE; ");



            if (Convert.ToBoolean(dtDatosEscritura.Rows[0]["Minuta"]))
            {
                //--------------------------------------------------------
                //FECHA: 16/01/2022
                // AUTOR: MIGUEL MÁRQUEZ BELTRÁN
                // MOTIVO: CAMBIOS EN EL CONTENIDO DEL TEXTO.
                //--------------------------------------------------------

                sScript.Append("Y ME " + str_participante_piden_plural + " QUE ELEVARA A ESCRITURA PÚBLICA LA SIGUIENTE MINUTA ");
                sScript.Append("AUTORIZADA Y FIRMADA POR EL/LA ABOGADO(A) ");
                sScript.Append(strAutorizacionTipoId); //NOMBRE DEL ABOGADO(A)
                sScript.Append(" DE NACIONALIDAD ");
                sScript.Append("PERUANA "); //PERUANA
                sScript.Append("IDENTIFICADO(A) CON ");
                sScript.Append("DOCUMENTO NACIONAL DE IDENTIDAD "); //DOCUMENTO NACIONAL DE IDENTIDAD
                sScript.Append("Nº " + strAutorizacionNroDocumento);
                sScript.Append(" CON REGISTRO Nº " + strNumeroColegiatura); //NRO. REGISTRO
                sScript.Append(" DEL ");
                sScript.Append(strNombreColegiatura); // COLEGIO DE ABOGADOS DE LIMA
                sScript.Append(" LA CUAL QUEDA ARCHIVADA EN SU LEGAJO RESPECTIVO Y CUYO TENOR ES EL SIGUIENTE:");
                
                sScript.Append("</p>");

                sScript.Append("<p>");
                sScript.Append("MINUTA:");
                sScript.Append("</p>");
            }
            else
            {
                #region Sin Minuta

                sScript.Append("DEJANDO CONSTANCIA QUE EN LA FORMALIZACIÓN DEL PRESENTE INSTRUMENTO ");
                sScript.Append(strlMinuta + " DE CONFORMIDAD CON LO DISPUESTO EN EL ARTÍCULO 58º DEL DECRETO LEGISLATIVO Nº 1049, ");
                sScript.Append("MANIFESTÁNDOME " + strOtorganteArticuloSexoPS + " " + stroPoderdanteSingularPlural + " LO SIGUIENTE:");
                sScript.Append("</p>");
                sScript.Append(CON_CentralAnterior_1);
                sScript.Append("<p style='text-align:justify;'>");

                if (contadorOtorgantes == 1)
                {
                    //-----------------------------------------------------------
                    //Fecha: 14/10/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Si es RENUNCIA A LA NACIONALIDAD
                    //-----------------------------------------------------------
                    if (dtDatosEscritura.Rows[0]["SubTipoActoNotarialId"].ToString() == "8069")
                    {
                        sScript.Append("SÍRVASE EXTENDER EN EL REGISTRO DE ESCRITURAS PUBLICAS A SU CARGO, UNA DE RENUNCIA A LA NACIONALIDAD, QUE OTORGO YO, ");
                        sScript.Append(strOtorganteNotarial);
                        sScript.Append("EN LOS SIGUIENTES TÉRMINOS: ");
                        //dtDatosEscritura
                    }
                    else
                    {
                        sScript.Append(CON_CentralAnterior_2 + "<b>" + strDenominacion + " </b>" + CON_CentralAnterior_3);
                    }
                    //-----------------------------------------------------------                    
                }
                else
                {
                    sScript.Append(CON_CentralAnterior_2 + "<b>" + strDenominacion + " </b>" + CON_CentralAnterior_4);
                }
                if (dtDatosEscritura.Rows[0]["SubTipoActoNotarialId"].ToString() != "8069")
                {
                    #region Diferente a Renuncia de Nacionalidad
                    sScript.Append(strOtorganteNotarial);

                    if (contadorOtorgantes == 1)
                    {
                        sScript.Append(" A QUIEN EN ADELANTE SE DENOMINARÁ ");
                    }
                    else
                    {
                        sScript.Append(" A QUIENES EN ADELANTE SE LES DENOMINARÁN ");
                    }

                    sScript.Append(" <b>" + strOtorganteArticuloSexoPS + " " + stroPoderdanteSingularPlural);


                    sScript.Append(",</b> A FAVOR ");
                    if (contadorApoderado == 1)
                    { sScript.Append("DE "); }
                    else
                    {
                        sScript.Append("DE LOS SIGUIENTES ");
                    }
                    sScript.Append(strApoderadoNotarial);



                    if (contadorApoderado == 1)
                    {
                        sScript.Append(" A QUIEN EN ADELANTE SE LE DENOMINARÁ ");
                    }
                    else
                    {
                        sScript.Append(" A QUIENES EN ADELANTE SE LES DENOMINARÁN ");
                    }

                    if (contadorApoderadoHombre == 0)
                    {
                        if (contadorApoderadoMujer == 1)
                        {
                            string strLaApoderada = "<b>LA APODERADA.</b>";

                            if (strSubTipoActoNotarialDesc == "COMPRA - VENTA")
                                strLaApoderada = "<b>LA COMPRADORA.</b>";
                            if (strSubTipoActoNotarialDesc == "DONACIÓN")
                                strLaApoderada = "<b>LA DONATARIA.</b>";
                            if (strSubTipoActoNotarialDesc == "ADELANTO DE LEGÍTIMA")
                                strLaApoderada = "<b>LA ANTICIPADA.</b>";

                            sScript.Append(strLaApoderada);
                        }
                        if (contadorApoderadoMujer > 1)
                        {
                            string strLasApoderadas = "<b>LAS APODERADAS.</b>";

                            if (strSubTipoActoNotarialDesc == "COMPRA - VENTA")
                                strLasApoderadas = "<b>LAS COMPRADORAS.</b>";
                            if (strSubTipoActoNotarialDesc == "DONACIÓN")
                                strLasApoderadas = "<b>LAS DONATARIAS.</b>";
                            if (strSubTipoActoNotarialDesc == "ADELANTO DE LEGÍTIMA")
                                strLasApoderadas = "<b>LAS ANTICIPADAS.</b>";

                            sScript.Append(strLasApoderadas);

                        }
                    }
                    else
                    {
                        if (contadorApoderadoHombre == 1 && contadorApoderado == 1)
                        {
                            string strElApoderado = "<b>EL APODERADO.</b>";

                            if (strSubTipoActoNotarialDesc == "COMPRA - VENTA")
                                strElApoderado = "<b>EL COMPRADOR.</b>";
                            if (strSubTipoActoNotarialDesc == "DONACIÓN")
                                strElApoderado = "<b>EL DONATARIO.</b>";
                            if (strSubTipoActoNotarialDesc == "ADELANTO DE LEGÍTIMA")
                                strElApoderado = "<b>EL ANTICIPADO.</b>";

                            sScript.Append(strElApoderado);

                        }
                        else
                        {
                            string strLosApoderados = "<b>LOS APODERADOS.</b>";

                            if (strSubTipoActoNotarialDesc == "COMPRA - VENTA")
                                strLosApoderados = "<b>LOS COMPRADORES.</b>";
                            if (strSubTipoActoNotarialDesc == "DONACIÓN")
                                strLosApoderados = "<b>LOS DONATARIOS.</b>";
                            if (strSubTipoActoNotarialDesc == "ADELANTO DE LEGÍTIMA")
                                strLosApoderados = "<b>LOS ANTICIPADOS.</b>";

                            sScript.Append(strLosApoderados);

                        }
                    }
                    #endregion
                }
                //--------------------------------------
                sScript.Append("</p>");
                //----------------------------------------------------------
                //Fecha: 29/03/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Borrar el fragmento de la escritura pública
                //          según nuevo formato.
                //----------------------------------------------------------
                //sScript.Append("<p style='text-align:justify;'>");
                //sScript.Append("QUE " + strOtorganteOtorgaSP + " PODER A FAVOR DE " + strParticipanteCuerpoPoder + " PARA QUE ACTÚE EN SU NOMBRE ");
                //sScript.Append("Y REPRESENTACIÓN EJERCITANDO LAS ATRIBUCIONES Y FACULTADES SIGUIENTES:");

                #endregion
            }


            string innerString = sScript.ToString();

            return innerString.ToString();

        }

        [System.Web.Services.WebMethod]
        public static string llenar_conclusion_escritura_Regular(string cuerpo)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var jsonObject = serializer.Deserialize<dynamic>(cuerpo);

            string strTextoActoNotarial = Convert.ToString(jsonObject["textoActoNotarial"]);


            DataTable dt = new DataTable();
            DataTable dtdp = new DataTable();

            ActoNotarialConsultaBL bl = new ActoNotarialConsultaBL();

            //--------------------------------------------------------------------------
            //Fecha: 03/04/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Asignar los datos de la escritura de una sesión a un DataTable.
            //--------------------------------------------------------------------------

            if (HttpContext.Current.Session["dtDatosEscritura"] == null)
            {
                dtdp = bl.ActonotarialObtenerDatosPrincipales(Convert.ToInt64(jsonObject["ancu_iActoNotarialId"]), Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                //--------------------------------------------------------------------------
                //Fecha: 03/04/2020
                //Autor: Miguel Márquez Beltrán
                //Motivo: Asignar los datos de la escritura a una sesión.
                //--------------------------------------------------------------------------
                HttpContext.Current.Session["dtDatosEscritura"] = dtdp;
                //--------------------------------------------------------------------------
            }
            else
            {
                dtdp = (DataTable)HttpContext.Current.Session["dtDatosEscritura"];
            }
            //--------------------------------------------------------------------------

            //if (HttpContext.Current.Session["dtParticipantesAll"] == null)
            //{
            dt = bl.ActonotarialObtenerParticipantes(Convert.ToInt64(jsonObject["ancu_iActoNotarialId"]));

            //--------------------------------------------------------------------------
            //Fecha: 06/04/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Asignar los datos de la escritura a una sesión.
            //--------------------------------------------------------------------------
            //  HttpContext.Current.Session["dtParticipantesAll"] = dt;
            //--------------------------------------------------------------------------
            //}
            //else
            //{
            //    dt = (DataTable)HttpContext.Current.Session["dtParticipantesAll"];
            //}

            string strDL1049Articulo55C = Convert.ToString(jsonObject["ancu_vDL1049Articulo55C"]);

            DataView dv = dt.DefaultView;
            DataTable dts = dv.ToTable();
            dts = (from p in dt.AsEnumerable()
                   orderby p["vPersona"]
                   select p
                       ).CopyToDataTable();

            //string strFecha = Util.ObtenerFechaParaDocumentoLegal(Comun.FormatearFecha((dtdp.Rows[0]["Fecha"]).ToString())).ToUpper().Trim();
            string strFecha = Util.ObtenerFechaParaDocumentoLegalProtocolar(Comun.FormatearFecha((dtdp.Rows[0]["Fecha"]).ToString())).ToUpper().Trim();

            int strTipoEscrituraId = Convert.ToInt32(dtdp.Rows[0]["SubTipoActoNotarialId"]);
            string strTipoParticipante = string.Empty;


            string strFuncionarioAutorizador = dtdp.Rows[0]["NombreFuncionario"].ToString().Trim();
            string strUbigeoOficinaConsular = dtdp.Rows[0]["CiudadOficinaConsular"].ToString().Trim();
            string strProvincia = dtdp.Rows[0]["Provincia"].ToString().Trim();

            string strCargoFuncionarioAutorizador = dtdp.Rows[0]["CargoFuncionario"].ToString().Trim();

            string strNroEscrituraPublica = dtdp.Rows[0]["NumeroEscrituraPublica"].ToString().Trim();
            string strNroFojaInicial = dtdp.Rows[0]["NumeroFojaInicial"].ToString().Trim();
            string strNroFojaFinal = dtdp.Rows[0]["vNumeroFojaFinal"].ToString().Trim();

            string strArticuloPersonaSexo = string.Empty;
            string strInformadoaPersonaSexo = string.Empty;
            string strInformadoaPersonaPS = string.Empty;

            string strArticuloInterpreteSexo = string.Empty;
            int contadorInterprete = 0;
            int contadorInterpreteHombres = 0;
            string strPersonaInterprete = string.Empty;

            //string strOtorganteInformarSP = string.Empty;

            int contadorOtorgantes = 0;
            string strOtorganteArticuloSexoPS = string.Empty;
            string strOtorganteArticuloSexoTraduccion = string.Empty;

            string strOtorgantePS = string.Empty;
            string strOtorganteAfirmarSP = string.Empty;
            string strOtorganteRatificarSP = string.Empty;
            string strOtorganteHaSP = string.Empty;

            string strParticipanteOtorgantes = string.Empty;

            string strParticipanteQuienSP = string.Empty;

            int ContadorParticipanteHombres = 0;

            string strTestigoRuegoConclusion = string.Empty;
            int contadortestigoruego = 0;
            int ContadorTestigoRuegoHombres = 0;
            string strTestigoRuego = string.Empty;
            string strArticuloPersonaSexoAL_DEL = string.Empty;


            //------------------------------------------------------------------------------
            //Fecha: 08/03/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Inicializar las variables para insertar el articulo 55 
            //             del Decreto Legislativo N°1049
            //------------------------------------------------------------------------------
            string strvNacionalidad = "";
            bool isOtorgantePeruano = false;
            string strNombresOtorgante = "";
            string strvNroDocumento = "";
            string strTodosDocumentos = "";
            string strNombresTestigos = "";
            //------------------------------------------------------------------------------
            int intNumeroMasculinos = 0;
            int intNumeroFemeninos = 0;
            bool bIncapacitado = false;
            bool bFlagFirma = false;
            bool bFlagHuella = false;

            foreach (DataRow dr in dts.Rows)
            {

                int strGenero = Convert.ToInt32(dr["iGenero"]);

                if (Convert.ToBoolean(dr["pers_bIncapacidadFlag"]))
                {
                    bIncapacitado = true;
                }
                if (Convert.ToBoolean(dr["anpa_bFlagFirma"].ToString()))
                {
                    bFlagFirma = true;
                }
                if (Convert.ToBoolean(dr["anpa_bFlagHuella"].ToString()))
                {
                    bFlagHuella = true;
                }
                strTipoParticipante = dr["vTipoParticipanteId"].ToString();

                //if ((strTipoEscrituraId == (int)Enumerador.enmProtocolarTipo.PODER_GENERAL_AMPLIO_ABSOLUTO) ||
                //    (strTipoEscrituraId == (int)Enumerador.enmProtocolarTipo.PODER_ESPECIAL) ||
                //    (strTipoEscrituraId == (int)Enumerador.enmProtocolarTipo.COMPRA_VENTA))
                //{
                //------------------------------------------------------------------------------
                //Fecha: 08/03/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Determinar si el otorgante es PERUANO
                //------------------------------------------------------------------------------
                strvNacionalidad = dr["vNacionalidad"].ToString().ToUpper().Trim();
                //if (strvNacionalidad.Equals("PERUANA") && strTipoParticipante.Equals("OTORGANTE"))
                //{ isOtorgantePeruano = true; }

                //if (strvNacionalidad.Equals("PERUANA") && strTipoParticipante.Equals("VENDEDOR"))
                //{ isOtorgantePeruano = true; }

                //if (strvNacionalidad.Equals("PERUANA") && strTipoParticipante.Equals("DONANTE"))
                //{ isOtorgantePeruano = true; }

                //if (strvNacionalidad.Equals("PERUANA") && strTipoParticipante.Equals("ANTICIPANTE"))
                //{ isOtorgantePeruano = true; }


                if (strvNacionalidad.Equals("PERUANA") && ObtenerIniciaRecibe(strTipoParticipante) =="INICIA")
                { isOtorgantePeruano = true; }

                //------------------------------------------------------------------------------
                strTodosDocumentos += dr["vNroDocumento"].ToString() + ", ";


                //------------------------------------------------------------------------------
                if (strTipoParticipante == "INTERPRETE")
                {
                    strPersonaInterprete += dr["vPersona"].ToString() + ", ";
                    contadorInterprete++;
                    if (strGenero == (int)Enumerador.enmGenero.MASCULINO)
                    {
                        strArticuloInterpreteSexo = "EL";
                        contadorInterpreteHombres++;
                    }
                    else
                    {
                        strArticuloInterpreteSexo = "LA";
                    }
                }

                //------------------------------------------------------------------------------
                //if (strTipoParticipante == "OTORGANTE" || strTipoParticipante == "VENDEDOR"
                //    || strTipoParticipante == "ANTICIPANTE" || strTipoParticipante == "DONANTE")

                if (ObtenerIniciaRecibe(strTipoParticipante) == "INICIA")
                {
                    #region Otorgante_Vendedor_Anticipante

                    #region Verificar Existencia Testigo a Ruego


                    DataTable dtp = new DataTable();

                    //if (HttpContext.Current.Session["dtParticipantesAll"] == null)
                    //{
                    dtp = bl.ActonotarialObtenerParticipantes(Convert.ToInt64(jsonObject["ancu_iActoNotarialId"]));

                    //--------------------------------------------------------------------------
                    //Fecha: 06/04/2020
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Asignar los datos de la escritura a una sesión.
                    //--------------------------------------------------------------------------
                    // HttpContext.Current.Session["dtParticipantesAll"] = dtp;
                    //--------------------------------------------------------------------------
                    //}
                    //else
                    //{
                    //    dtp = (DataTable)HttpContext.Current.Session["dtParticipantesAll"];
                    //}

                    DataTable dtp1 = new DataTable();
                    try
                    {
                        dtp1 = (from ps in dtp.AsEnumerable()
                                where Convert.ToInt64(ps["anpa_iReferenciaParticipanteId"]) == Convert.ToInt64(dr["anpa_iActoNotarialParticipanteId"].ToString())
                                select ps
                       ).CopyToDataTable();
                    }
                    catch
                    {
                        dtp1 = null;
                    }
                    #endregion

                    if (dtp1 != null)
                    {
                        strParticipanteOtorgantes += "IMPRIME SU HUELLA DACTILAR " + dr["vPersona"].ToString() + " EL " + strFecha + ", ";
                        strParticipanteOtorgantes += "FIRMA E IMPRIME SU HUELLA DACTILAR " + dtp1.Rows[0]["vPersona"].ToString() + " EL " + strFecha + ",";
                    }
                    else
                    {
                        strParticipanteOtorgantes += "FIRMA E IMPRIME SU HUELLA DACTILAR " + dr["vPersona"].ToString() + " EL " + strFecha + ",";
                    }
                    strNombresOtorgante += dr["vPersona"].ToString() + ", ";
                    strvNroDocumento += dr["vNroDocumento"].ToString() + ", ";


                    if (strGenero == (int)Enumerador.enmGenero.MASCULINO)
                    {
                        strArticuloPersonaSexo = "EL";
                        strInformadoaPersonaSexo = "INFORMADO";
                        ContadorParticipanteHombres++;
                        strArticuloPersonaSexoAL_DEL = "AL";
                        intNumeroMasculinos++;
                    }
                    else if (strGenero == (int)Enumerador.enmGenero.FEMENINO)
                    {
                        strArticuloPersonaSexo = "LA";
                        strInformadoaPersonaSexo = "INFORMADA";
                        strArticuloPersonaSexoAL_DEL = "A LA";
                        intNumeroFemeninos++;
                    }

                    contadorOtorgantes = contadorOtorgantes + 1;

                    if (contadorOtorgantes == 1)
                    {
                        #region Un_Otorgante

                        if (strGenero == (int)Enumerador.enmGenero.MASCULINO)
                        {
                            strOtorganteArticuloSexoPS = "EL";
                            strOtorganteArticuloSexoTraduccion = "DEL";
                            //strOtorganteInformarSP = "INFORMADO";
                            strInformadoaPersonaSexo = "INFORMADO";
                            strArticuloPersonaSexoAL_DEL = "AL";
                        }
                        else if (strGenero == (int)Enumerador.enmGenero.FEMENINO)
                        {
                            strOtorganteArticuloSexoPS = "LA";
                            strOtorganteArticuloSexoTraduccion = "DE LA";
                            //strOtorganteInformarSP = "INFORMADA";
                            strInformadoaPersonaSexo = "INFORMADA";

                            strArticuloPersonaSexoAL_DEL = "A LA";
                        }

                        strOtorganteAfirmarSP = "AFIRMÓ";
                        strOtorganteRatificarSP = "RATIFICÓ";
                        strOtorganteHaSP = "HA";
                        
                        if (strTipoParticipante == "OTORGANTE")
                        { strOtorgantePS = "PODERDANTE"; }
                        else
                        {
                            if (strTipoParticipante == "VENDEDOR")
                            {
                                strOtorgantePS = "VENDEDOR";
                                if (strGenero == (int)Enumerador.enmGenero.FEMENINO)
                                { strOtorgantePS = "VENDEDORA"; }
                            }
                            else
                            {
                                strOtorgantePS = strTipoParticipante;
                            }
                        }

                        strParticipanteQuienSP = "QUIEN";
                        #endregion
                    }
                    else
                    {
                        #region Mas_Otorgantes

                        if (strGenero == (int)Enumerador.enmGenero.MASCULINO)
                        {
                            strOtorganteArticuloSexoPS = "LOS";
                            strOtorganteArticuloSexoTraduccion = "DE LOS";
                            strInformadoaPersonaSexo = "INFORMADOS";

                            //strOtorganteInformarSP = "INFORMADOS";

                            strArticuloPersonaSexoAL_DEL = "A LOS";
                        }
                        else if (strGenero == (int)Enumerador.enmGenero.FEMENINO)
                        {
                            if (ContadorParticipanteHombres > 0)
                            {
                                strOtorganteArticuloSexoPS = "LOS";
                                strOtorganteArticuloSexoTraduccion = "DE LOS";
                                strInformadoaPersonaSexo = "INFORMADOS";
                                //strOtorganteInformarSP = "INFORMADOS";
                                strArticuloPersonaSexoAL_DEL = "A LOS";
                            }
                            else
                            {
                                strOtorganteArticuloSexoPS = "LAS";
                                strOtorganteArticuloSexoTraduccion = "DE LAS";
                                strInformadoaPersonaSexo = "INFORMADAS";
                                //strOtorganteInformarSP = "INFORMADAS";
                                strArticuloPersonaSexoAL_DEL = "A LAS";
                            }
                        }

                        strOtorganteAfirmarSP = "AFIRMARON";
                        strOtorganteRatificarSP = "RATIFICARON";
                        strOtorganteHaSP = "HAN";

                        strOtorgantePS = strTipoParticipante.Trim() + "S";

                        if (strTipoParticipante.Equals("OTORGANTE"))
                            strOtorgantePS = "PODERDANTES";
                        

                        if (strTipoParticipante.Equals("VENDEDOR"))
                        {
                            strOtorgantePS = "VENDEDORES";
                            if (strOtorganteArticuloSexoPS == "LAS")
                                strOtorgantePS = "VENDEDORAS";
                        }
                                               
                        strParticipanteQuienSP = "QUIENES";
                        #endregion
                    }

                    #region Testigo_A_Ruego

                    // Tiene testigo a ruego
                    if (dtp1 != null)
                    {
                        foreach (DataRow drAUX in dtp1.Rows)
                        {
                            strGenero = Convert.ToInt32(drAUX["iGenero"]);
                            strOtorganteHaSP = "HAN";
                            contadortestigoruego += 1;
                            strNombresTestigos += drAUX["vPersona"].ToString() + ", ";

                            if (strGenero == (int)Enumerador.enmGenero.MASCULINO)
                            {
                                #region Masculino

                                strOtorganteHaSP = "HA";
                                //strOtorganteArticuloSexoPS = "EL";

                                if (contadortestigoruego > 1)
                                {
                                    //strOtorganteInformarSP = "INFORMADOS";
                                    strTestigoRuego = "Y LOS TESTIGOS A RUEGO";
                                }
                                else
                                {
                                    //strOtorganteInformarSP = "INFORMADO";
                                    strTestigoRuego = " Y EL TESTIGO A RUEGO ";
                                }

                                ContadorTestigoRuegoHombres++;

                                #endregion
                            }
                            else if (strGenero == (int)Enumerador.enmGenero.FEMENINO)
                            {
                                #region Femenino

                                //strOtorganteArticuloSexoPS = "LA";
                                //strOtorganteInformarSP = "INFORMADAS";

                                if (ContadorTestigoRuegoHombres > 0)
                                {
                                    //strOtorganteArticuloSexoPS = "EL";
                                    //strOtorganteInformarSP = "INFORMADOS";
                                    strTestigoRuego = "Y LOS TESTIGOS A RUEGO";
                                }
                                else
                                {
                                    if (contadortestigoruego > 1)
                                    {
                                        strTestigoRuego = "Y LAS TESTIGOS A RUEGO";
                                        //strOtorganteInformarSP = "INFORMADAS";
                                    }
                                    else
                                    {
                                        strTestigoRuego = "Y LA TESTIGO A RUEGO";
                                        //strOtorganteInformarSP = "INFORMADA";
                                    }
                                }

                                #endregion
                            }
                        }

                        //strTestigoRuegoConclusion = " PROCEDIENDO A COLOCAR SU FIRMA Y SUS HUELLAS DACTILARES, " + strTestigoRuego + " A FIRMAR CONMIGO;";
                    }
                    #endregion

                    #endregion
                }

            }

            //-------------------------------------------------------
            //Fecha: 07/04/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Modificar el texto de la conclusión de acuerdo
            //        al nro. de otorgantes y al flag de incapacitado
            //        y al flag de huella.
            //-------------------------------------------------------
            if (contadortestigoruego >= 1)
            {
                if ((contadorOtorgantes - contadortestigoruego) == 0)
                {
                    if (bFlagHuella == false)
                    {
                        strTestigoRuegoConclusion = " PROCEDIENDO A COLOCAR SU HUELLA DACTILAR " + strTestigoRuego + " A FIRMAR CONMIGO;";
                    }
                    else
                    {
                        strTestigoRuegoConclusion = strTestigoRuego + " A FIRMAR CONMIGO;";
                    }
                }
                else
                {
                    if (bFlagHuella == false)
                    {
                        // string strLas_Los_NumeroLetras_Poderdantes = "";
                        //Documento objDoc = new Documento();
                        //string strNumeroLetras = "";
                        //strNumeroLetras = objDoc.ConvertirNumeroLetras(contadorOtorgantes.ToString(), true);
                        //strLas_Los_NumeroLetras_Poderdantes = strOtorganteArticuloSexoPS + " " + strNumeroLetras;
                        strTestigoRuegoConclusion = " PROCEDIENDO A COLOCAR SU FIRMA Y SUS HUELLAS DACTILARES " + strTestigoRuego + " A FIRMAR CONMIGO;";
                    }
                    else
                    {
                        string strEl_La_Poderdante_Sano_a = strOtorganteArticuloSexoPS + " PODERDANTE SANO ";

                        strTestigoRuegoConclusion = " PROCEDIENDO A COLOCAR SU FIRMA Y SU HUELLA DACTILAR SOLO " + strEl_La_Poderdante_Sano_a + strTestigoRuego + " A FIRMAR CONMIGO;";
                    }
                }

            }
            //-------------------------------------------------------


            if (contadortestigoruego >= 1)
            {
                strOtorganteHaSP = "HAN";
            }

            StringBuilder sScript = new StringBuilder();

            //------------------------------------------------------------------
            //Fecha: 07/03/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Insertar el texto para todas las escrituras públicas.
            //Responsable: Lily Atencio. (Abogada de TRC)
            //------------------------------------------------------------------
            sScript.Append("<p style='text-align:justify;'>");

            if (contadorInterprete == 0)
            {
                //PROCEDIENDO A COLOCAR SU FIRMA Y SU HUELLA DACTILAR

                if (contadortestigoruego >= 1)
                {
                    sScript.Append("<b>CONCLUSIÓN:</b> FORMALIZADO EL INSTRUMENTO, YO, <b>" + strFuncionarioAutorizador + ", </b>" + strCargoFuncionarioAutorizador + " DEL PERÚ EN " + strUbigeoOficinaConsular + ", " + strProvincia + ", DOY FE DE HABER INSTRUIDO " + strArticuloPersonaSexoAL_DEL + " " + strOtorgantePS + " ");
                    sScript.Append("SOBRE EL OBJETO Y FINES DE LA PRESENTE ESCRITURA PÚBLICA, QUE FUE LEÍDA POR MÍ EN SU INTEGRIDAD, " + strParticipanteQuienSP + " DESPUÉS DE LO CUAL SE " + strOtorganteAfirmarSP + " Y " + strOtorganteRatificarSP + " ");
                    sScript.Append("EN TODO SU CONTENIDO, " + strTestigoRuegoConclusion + " DECLARANDO ADEMÁS, QUE " + strOtorganteHaSP + " VERIFICADO QUE LOS DATOS CONSIGNADOS COMO: ");
                    sScript.Append("NOMBRE, APELLIDOS, ESTADO CIVIL, OCUPACIÓN, DIRECCIÓN E IDENTIFICACIÓN SON CORRECTOS. ");
                }
                else
                {
                    if (contadorOtorgantes == 1)
                    {
                        sScript.Append("<b>CONCLUSIÓN:</b> FORMALIZADO EL INSTRUMENTO, YO, <b>" + strFuncionarioAutorizador + ", </b>" + strCargoFuncionarioAutorizador + " DEL PERÚ EN " + strUbigeoOficinaConsular + ", " + strProvincia + ", DOY FE DE HABER INSTRUIDO " + strArticuloPersonaSexoAL_DEL + " " + strOtorgantePS + " ");
                        sScript.Append("SOBRE EL OBJETO Y FINES DE LA PRESENTE ESCRITURA PÚBLICA, QUE FUE LEÍDA POR MÍ EN SU INTEGRIDAD, " + strParticipanteQuienSP + " DESPUÉS DE LO CUAL SE " + strOtorganteAfirmarSP + " Y " + strOtorganteRatificarSP + " ");
                        sScript.Append("EN TODO SU CONTENIDO, PROCEDIENDO A COLOCAR SU FIRMA Y SU HUELLA DACTILAR, DECLARANDO ADEMÁS, QUE " + strOtorganteHaSP + " VERIFICADO QUE LOS DATOS CONSIGNADOS COMO: ");
                        sScript.Append("NOMBRE, APELLIDOS, ESTADO CIVIL, OCUPACIÓN, DIRECCIÓN E IDENTIFICACIÓN SON CORRECTOS. ");
                    }
                    else
                    {
                        sScript.Append("<b>CONCLUSIÓN:</b> FORMALIZADO EL INSTRUMENTO, YO, <b>" + strFuncionarioAutorizador + ", </b>" + strCargoFuncionarioAutorizador + " DEL PERÚ EN " + strUbigeoOficinaConsular + ", " + strProvincia + ", DOY FE DE HABER INSTRUIDO " + strArticuloPersonaSexoAL_DEL + " " + strOtorgantePS + " ");
                        sScript.Append("SOBRE EL OBJETO Y FINES DE LA PRESENTE ESCRITURA PÚBLICA, QUE FUE LEÍDA POR MÍ EN SU INTEGRIDAD, " + strParticipanteQuienSP + " DESPUÉS DE LO CUAL SE " + strOtorganteAfirmarSP + " Y " + strOtorganteRatificarSP + " ");
                        sScript.Append("EN TODO SU CONTENIDO, PROCEDIENDO A COLOCAR SUS FIRMAS Y SUS HUELLAS DACTILARES, DECLARANDO ADEMÁS, QUE " + strOtorganteHaSP + " VERIFICADO QUE LOS DATOS CONSIGNADOS COMO: ");
                        sScript.Append("NOMBRE, APELLIDOS, ESTADO CIVIL, OCUPACIÓN, DIRECCIÓN E IDENTIFICACIÓN SON CORRECTOS. ");
                    }

                }

            }
            else
            {
                sScript.Append("<b>CONCLUSIÓN:</b> FORMALIZADO EL INSTRUMENTO, YO, <b>" + strFuncionarioAutorizador + ", </b>" + strCargoFuncionarioAutorizador + " DEL PERÚ EN " + strUbigeoOficinaConsular + ", " + strProvincia + ", DOY FE DE HABER INSTRUIDO " + strArticuloPersonaSexoAL_DEL + " " + strOtorgantePS + " ");
                sScript.Append("SOBRE EL OBJETO Y FINES DE LA PRESENTE ESCRITURA PÚBLICA, QUE FUE LEÍDA POR MÍ EN SU INTEGRIDAD ");
                sScript.Append("Y TRADUCIDA SIMULTÁNEAMENTE POR " + strArticuloInterpreteSexo + " INTÉRPRETE, ");
                sScript.Append(strPersonaInterprete);
                sScript.Append("QUIÉN EN CUMPLIMIENTO DEL ARTÍCULO 30° DEL DECRETO LEGISLATIVO N°1049, ");
                sScript.Append("DECLARA BAJO RESPONSABILIDAD LA CONFORMIDAD DE LA TRADUCCIÓN REALIZADA A SOLICITUD ");
                sScript.Append(strOtorganteArticuloSexoTraduccion);
                sScript.Append(" " + strOtorgantePS + " " + strNombresOtorgante.Substring(0, strNombresOtorgante.Length - 2) + ". ");
                sScript.Append("POR LO QUE " + strOtorganteArticuloSexoPS + " " + strOtorgantePS);

                if (contadorOtorgantes == 1)
                {
                    sScript.Append(" DESPUÉS DE LO CUAL SE " + strOtorganteAfirmarSP + " Y " + strOtorganteRatificarSP + " EN TODO SU CONTENIDO, ");
                    sScript.Append("PROCEDIENDO A COLOCAR SU FIRMA Y SU HUELLA DACTILAR.");
                }
                else
                {
                    sScript.Append(" DESPUÉS DE LO CUAL SE " + strOtorganteAfirmarSP + " Y " + strOtorganteRatificarSP + " EN TODO SU CONTENIDO, ");
                    sScript.Append("PROCEDIENDO A COLOCAR SUS FIRMAS Y SUS HUELLAS DACTILARES.");
                }

                sScript.Append("</p>");

                sScript.Append("<p style='text-align:justify;'>");
                sScript.Append("DECLARANDO ADEMÁS, QUE " + strOtorganteHaSP + " VERIFICADO QUE LOS DATOS CONSIGNADOS COMO: ");
                sScript.Append("NOMBRE, APELLIDOS, ESTADO CIVIL, OCUPACIÓN, DIRECCIÓN E IDENTIFICACIÓN SON CORRECTOS. ");
            }

            //-----------------------------
            sScript.Append("DEJANDO EXPRESA CONSTANCIA QUE EL MECANISMO QUE UTILICÉ, EN MI CALIDAD DE " + strCargoFuncionarioAutorizador + ", PARA VERIFICAR LA IDENTIDAD DE " + strOtorganteArticuloSexoPS + " " + strOtorgantePS);

            if (contadorOtorgantes == 1)
            {
                sScript.Append(" DE NOMBRE ");
            }
            else
            {
                sScript.Append(" DE NOMBRES ");
            }
            sScript.Append(strNombresOtorgante);

            if (isOtorgantePeruano)
            {
                sScript.Append(" FUE EL DESCRITO EN EL LITERAL B) DEL ARTÍCULO 55° DEL DECRETO LEGISLATIVO N°1049, MODIFICADO POR EL DECRETO LEGISLATIVO N°1232; ES DECIR, TUVE A LA VISTA ");

                if (contadorOtorgantes == 1)
                {
                    if (intNumeroMasculinos == 1)
                    {
                        sScript.Append("SU DOCUMENTO NACIONAL DE IDENTIDAD Y ADEMÁS REALICE LA RESPECTIVA CONSULTA DEL SERVICIO EN LÍNEA DE RENIEC, EN LA CUAL VERIFIQUÉ LAS IMÁGENES Y DATOS DEL CITADO CIUDADANO PERUANO.");
                    }
                    else
                    {
                        sScript.Append("SU DOCUMENTO NACIONAL DE IDENTIDAD Y ADEMÁS REALICE LA RESPECTIVA CONSULTA DEL SERVICIO EN LÍNEA DE RENIEC, EN LA CUAL VERIFIQUÉ LAS IMÁGENES Y DATOS DE LA CITADA CIUDADANA PERUANA.");
                    }
                }
                else
                {
                    if (intNumeroMasculinos >= 1)
                    {
                        sScript.Append("SUS DOCUMENTOS NACIONALES DE IDENTIDAD Y ADEMÁS REALICE LAS RESPECTIVAS CONSULTAS DEL SERVICIO EN LÍNEA DE RENIEC, EN LA CUAL VERIFIQUÉ LAS IMÁGENES Y DATOS DE LOS CITADOS CIUDADANOS PERUANOS.");
                    }
                    else
                    {
                        sScript.Append("SUS DOCUMENTOS NACIONALES DE IDENTIDAD Y ADEMÁS REALICE LAS RESPECTIVAS CONSULTAS DEL SERVICIO EN LÍNEA DE RENIEC, EN LA CUAL VERIFIQUÉ LAS IMÁGENES Y DATOS DE LAS CITADAS CIUDADANAS PERUANAS.");
                    }
                }
            }
            else
            {
                sScript.Append(" FUE EL DESCRITO EN EL LITERAL C) DEL ARTÍCULO 55° DEL DECRETO LEGISLATIVO N°1049, MODIFICADO POR EL DECRETO LEGISLATIVO N°1232.");
                sScript.Append("</p>");

                sScript.Append("<p style='text-align:justify;'>");
                sScript.Append(strDL1049Articulo55C);
            }
            sScript.Append("</p>");

            //-------------------------------------------
            string strTipoParticipanteInterprete = string.Empty;
            string strInterpreteArticuloSexoPS = string.Empty;
            int intGeneroInterprete = 0;
            string strInterpretePS = string.Empty;
            bool isInterpretePeruano = false;
            string strNacionalidadInterprete = string.Empty;
            string strNombresInterprete = string.Empty;

            if (contadorInterprete > 0)
            {
                foreach (DataRow dr in dts.Rows)
                {
                    intGeneroInterprete = Convert.ToInt32(dr["iGenero"]);
                    strTipoParticipanteInterprete = dr["vTipoParticipanteId"].ToString();

                    if (strTipoParticipanteInterprete == "INTERPRETE")
                    {
                        strNacionalidadInterprete = dr["vNacionalidad"].ToString().ToUpper().Trim();
                        isInterpretePeruano = false;
                        strNombresInterprete += dr["vPersona"].ToString() + ", ";

                        if (strNacionalidadInterprete.Equals("PERUANA"))
                        { isInterpretePeruano = true; }


                        if (contadorInterprete == 1)
                        {
                            strInterpretePS = "INTERPRETE";

                            if (intGeneroInterprete == (int)Enumerador.enmGenero.MASCULINO)
                            {
                                strInterpreteArticuloSexoPS = "EL";
                            }
                            else
                            {
                                strInterpreteArticuloSexoPS = "LA";
                            }
                        }
                        else
                        {
                            strInterpretePS = "INTERPRETES";

                            if (intGeneroInterprete == (int)Enumerador.enmGenero.MASCULINO)
                            {
                                strInterpreteArticuloSexoPS = "LOS";
                            }
                            else
                            {
                                if (contadorInterpreteHombres > 0)
                                {
                                    strInterpreteArticuloSexoPS = "LOS";
                                }
                                else
                                {
                                    strInterpreteArticuloSexoPS = "LAS";
                                }
                            }
                        }
                        sScript.Append("<p style='text-align:justify;'>");
                        sScript.Append("ASIMISMO, DEJANDO EXPRESA CONSTANCIA QUE EL MECANISMO QUE UTILICÉ, EN MI CALIDAD DE " + strCargoFuncionarioAutorizador + ", PARA VERIFICAR LA IDENTIDAD DE " + strInterpreteArticuloSexoPS + " " + strInterpretePS);
                        if (contadorInterprete == 1)
                        {
                            sScript.Append(" DE NOMBRE ");
                        }
                        else
                        {
                            sScript.Append(" DE NOMBRES ");
                        }

                        sScript.Append(strNombresInterprete);

                        if (isInterpretePeruano)
                        {
                            sScript.Append(" FUE EL DESCRITO EN EL LITERAL B) DEL ARTÍCULO 55° DEL DECRETO LEGISLATIVO N°1049, MODIFICADO POR EL DECRETO LEGISLATIVO N°1232; ES DECIR, TUVE A LA VISTA ");

                            if (contadorInterprete == 1)
                            {
                                if (contadorInterpreteHombres == 1)
                                {
                                    sScript.Append("SU DOCUMENTO NACIONAL DE IDENTIDAD Y ADEMÁS REALICE LA RESPECTIVA CONSULTA DEL SERVICIO EN LÍNEA DE RENIEC, EN LA CUAL VERIFIQUÉ LAS IMÁGENES Y DATOS DEL CITADO CIUDADANO PERUANO.");
                                }
                                else
                                {
                                    sScript.Append("SU DOCUMENTO NACIONAL DE IDENTIDAD Y ADEMÁS REALICE LA RESPECTIVA CONSULTA DEL SERVICIO EN LÍNEA DE RENIEC, EN LA CUAL VERIFIQUÉ LAS IMÁGENES Y DATOS DE LA CITADA CIUDADANA PERUANA.");
                                }
                            }
                            else
                            {
                                if (contadorInterpreteHombres >= 1)
                                {
                                    sScript.Append("SUS DOCUMENTOS NACIONALES DE IDENTIDAD Y ADEMÁS REALICE LAS RESPECTIVAS CONSULTAS DEL SERVICIO EN LÍNEA DE RENIEC, EN LA CUAL VERIFIQUÉ LAS IMÁGENES Y DATOS DE LOS CITADOS CIUDADANOS PERUANOS.");
                                }
                                else
                                {
                                    sScript.Append("SUS DOCUMENTOS NACIONALES DE IDENTIDAD Y ADEMÁS REALICE LAS RESPECTIVAS CONSULTAS DEL SERVICIO EN LÍNEA DE RENIEC, EN LA CUAL VERIFIQUÉ LAS IMÁGENES Y DATOS DE LAS CITADAS CIUDADANAS PERUANAS.");
                                }
                            }
                        }
                        else
                        {
                            sScript.Append(" FUE EL DESCRITO EN EL LITERAL C) DEL ARTÍCULO 55° DEL DECRETO LEGISLATIVO N°1049, MODIFICADO POR EL DECRETO LEGISLATIVO N°1232.");
                        }


                        sScript.Append("</p>");
                    }
                }
            }
            //-------------------------------------------
            string strTipoParticipanteTestigo = string.Empty;
            string strTestigoArticuloSexoPS = string.Empty;
            int intGeneroTestigo = 0;
            string strTestigoPS = string.Empty;
            bool isTestigoPeruano = false;
            string strNacionalidadTestigo = string.Empty;
            string strNombresTestigo = string.Empty;

            if (contadortestigoruego > 0)
            {
                foreach (DataRow dr in dts.Rows)
                {
                    intGeneroTestigo = Convert.ToInt32(dr["iGenero"]);
                    strTipoParticipanteTestigo = dr["vTipoParticipanteId"].ToString();

                    if (strTipoParticipanteTestigo == "TESTIGO A RUEGO")
                    {
                        strNacionalidadTestigo = dr["vNacionalidad"].ToString().ToUpper().Trim();
                        isTestigoPeruano = false;
                        strNombresTestigo += dr["vPersona"].ToString() + ", ";

                        if (strNacionalidadTestigo.Equals("PERUANA"))
                        { isTestigoPeruano = true; }

                        if (contadortestigoruego == 1)
                        {
                            strTestigoPS = "TESTIGO A RUEGO";

                            if (intGeneroTestigo == (int)Enumerador.enmGenero.MASCULINO)
                            {
                                strTestigoArticuloSexoPS = "EL";
                            }
                            else
                            {
                                strTestigoArticuloSexoPS = "LA";
                            }
                        }
                        else
                        {
                            strTestigoPS = "TESTIGOS A RUEGO";

                            if (intGeneroTestigo == (int)Enumerador.enmGenero.MASCULINO)
                            {
                                strTestigoArticuloSexoPS = "LOS";
                            }
                            else
                            {
                                if (ContadorTestigoRuegoHombres > 0)
                                {
                                    strTestigoArticuloSexoPS = "LOS";
                                }
                                else
                                {
                                    strTestigoArticuloSexoPS = "LAS";
                                }
                            }
                        }

                        sScript.Append("<p style='text-align:justify;'>");
                        sScript.Append("ASIMISMO, DEJANDO EXPRESA CONSTANCIA QUE EL MECANISMO QUE UTILICÉ, EN MI CALIDAD DE " + strCargoFuncionarioAutorizador + ", PARA VERIFICAR LA IDENTIDAD DE " + strTestigoArticuloSexoPS + " " + strTestigoPS);
                        if (contadortestigoruego == 1)
                        {
                            sScript.Append(" DE NOMBRE ");
                        }
                        else
                        {
                            sScript.Append(" DE NOMBRES ");
                        }

                        sScript.Append(strNombresTestigo);

                        if (isTestigoPeruano)
                        {
                            sScript.Append(" FUE EL DESCRITO EN EL LITERAL B) DEL ARTÍCULO 55° DEL DECRETO LEGISLATIVO N°1049, MODIFICADO POR EL DECRETO LEGISLATIVO N°1232; ES DECIR, TUVE A LA VISTA ");

                            if (contadortestigoruego == 1)
                            {
                                if (ContadorTestigoRuegoHombres == 1)
                                {
                                    sScript.Append("SU DOCUMENTO NACIONAL DE IDENTIDAD Y ADEMÁS REALICE LA RESPECTIVA CONSULTA DEL SERVICIO EN LÍNEA DE RENIEC, EN LA CUAL VERIFIQUÉ LAS IMÁGENES Y DATOS DEL CITADO CIUDADANO PERUANO.");
                                }
                                else
                                {
                                    sScript.Append("SU DOCUMENTO NACIONAL DE IDENTIDAD Y ADEMÁS REALICE LA RESPECTIVA CONSULTA DEL SERVICIO EN LÍNEA DE RENIEC, EN LA CUAL VERIFIQUÉ LAS IMÁGENES Y DATOS DE LA CITADA CIUDADANA PERUANA.");
                                }
                            }
                            else
                            {
                                if (ContadorTestigoRuegoHombres >= 1)
                                {
                                    sScript.Append("SUS DOCUMENTOS NACIONALES DE IDENTIDAD Y ADEMÁS REALICE LAS RESPECTIVAS CONSULTAS DEL SERVICIO EN LÍNEA DE RENIEC, EN LA CUAL VERIFIQUÉ LAS IMÁGENES Y DATOS DE LOS CITADOS CIUDADANOS PERUANOS.");
                                }
                                else
                                {
                                    sScript.Append("SUS DOCUMENTOS NACIONALES DE IDENTIDAD Y ADEMÁS REALICE LAS RESPECTIVAS CONSULTAS DEL SERVICIO EN LÍNEA DE RENIEC, EN LA CUAL VERIFIQUÉ LAS IMÁGENES Y DATOS DE LAS CITADAS CIUDADANAS PERUANAS.");
                                }
                            }
                        }
                        else
                        {
                            sScript.Append(" FUE EL DESCRITO EN EL LITERAL C) DEL ARTÍCULO 55° DEL DECRETO LEGISLATIVO N°1049, MODIFICADO POR EL DECRETO LEGISLATIVO N°1232.");
                        }
                        sScript.Append("</p>");
                    }
                }
            }

            //-------------------------------------------

            Documento odoc = new Documento();


            string strLetrasNumeroEscrituraPublica = "";

            if (Convert.ToString(jsonObject["acno_vNumeroEscrituraPublica"]) == "MRE000")
            {
                strLetrasNumeroEscrituraPublica = "CERO";
            }
            else
            {
                strLetrasNumeroEscrituraPublica = odoc.ConvertirNumeroLetras(Convert.ToString(jsonObject["acno_vNumeroEscrituraPublica"]), true);
            }

            sScript.Append("<p style='text-align:justify;'>");
            sScript.Append("EN ATENCIÓN A LA MODIFICACIÓN DEL ARTÍCULO 59° INCISO K) DEL DECRETO LEGISLATIVO N° 1049, MODIFICADO POR EL DECRETO LEGISLATIVO N°1232, DEJO EXPRESA CONSTANCIA DE HABER EFECTUADO ");
            sScript.Append("LAS MÍNIMAS ACCIONES DE CONTROL Y DEBIDA DILIGENCIA EN MATERIA DE PREVENCIÓN DEL LAVADO DE ACTIVOS, ESPECIALMENTE VINCULADO A LA MINERÍA ILEGAL U OTRAS FORMAS DE CRIMEN ORGANIZADO, ");
            sScript.Append("RESPECTO A TODAS LAS PARTES INTERVINIENTES EN LA TRANSACCIÓN, ESPECÍFICAMENTE CON RELACIÓN AL ORIGEN DE LOS FONDOS, BIENES U OTROS ACTIVOS INVOLUCRADOS EN DICHA TRANSACCIÓN, ASÍ COMO CON LOS MEDIOS DE PAGO UTILIZADOS.");
            sScript.Append("</p>");


            sScript.Append("<p style='text-align:justify;'>");
            sScript.Append("CON LA SUSCRIPCIÓN DEL PRESENTE DOCUMENTO, " + strOtorganteArticuloSexoPS + " " + strOtorgantePS);

            if (contadorOtorgantes == 1)
            {
                sScript.Append(" DECLARA ");
            }
            else
            {
                sScript.Append(" DECLARAN ");
            }

            if (contadorOtorgantes == 1)
            {
                sScript.Append("BAJO JURAMENTO QUE LOS FONDOS, BIENES Y/O ACTIVOS DE LA TRANSACCIÓN PROCEDEN DE ACTIVIDADES LÍCITAS Y QUE LOS DATOS E INFORMACIÓN CONSIGNADOS RESPECTO DE ELLOS SON CORRECTOS, ASUMIENDO LA RESPONSABILIDAD QUE CORRESPONDA.");
            }
            else
            {
                sScript.Append("BAJO JURAMENTO QUE LOS FONDOS, BIENES Y/O ACTIVOS DE LA TRANSACCIÓN PROCEDEN DE ACTIVIDADES LÍCITAS Y QUE LOS DATOS E INFORMACIÓN CONSIGNADOS RESPECTO DE ELLOS SON CORRECTOS, ASUMIENDO LAS RESPONSABILIDADES QUE CORRESPONDAN.");
            }

            sScript.Append("</p>");
            sScript.Append("<p style='text-align:justify;'>");
            sScript.Append("ASIMISMO " + strOtorganteArticuloSexoPS + " " + strOtorgantePS);

            //----------------------------------------------
            //Fecha: 24/07/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Aplicar el genero en el texto.
            //----------------------------------------------

            if (intNumeroMasculinos == 0)
            {
                if (intNumeroFemeninos == 1)
                {
                    sScript.Append(" HA SIDO INFORMADA ");
                }
                if (intNumeroFemeninos > 1)
                {
                    sScript.Append(" HAN SIDO INFORMADAS ");
                }
            }
            else
            {
                if (intNumeroMasculinos == 1 && contadorOtorgantes == 1)
                {
                    sScript.Append(" HA SIDO INFORMADO ");
                }
                else
                {
                    sScript.Append(" HAN SIDO INFORMADOS ");
                }
            }
            //----------------------------------------------
            //if (contadorOtorgantes == 1)
            //{ sScript.Append(" HA SIDO INFORMADO "); }
            //else
            //{ sScript.Append(" HAN SIDO INFORMADOS "); }

            sScript.Append("QUE TODA MODIFICACIÓN O RECTIFICACIÓN REQUERIRÁ EL OTORGAMIENTO DE UNA NUEVA ESCRITURA PÚBLICA DE ACUERDO AL ARTÍCULO 48 DE LA LEY DEL NOTARIADO Y DEL ARTÍCULO 433 Y SIGUIENTES DEL REGLAMENTO CONSULAR DEL PERÚ.");

            sScript.Append("</p>");

            sScript.Append("<p style='text-align:justify;'>");

            sScript.Append("SE PROCEDE A TRANSCRIBIR COMO INSERTOS DE LA PRESENTE ESCRITURA PÚBLICA LO SIGUIENTE:");

            sScript.Append("</p>");

            string innerString = sScript.ToString();

            return innerString.ToString();
        }

        [System.Web.Services.WebMethod]
        public static string llenar_final_escritura_Regular(string cuerpo)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var jsonObject = serializer.Deserialize<dynamic>(cuerpo);

            string strTextoActoNotarial = Convert.ToString(jsonObject["textoActoNotarial"]);

            ActoNotarialConsultaBL bl = new ActoNotarialConsultaBL();
            DataTable dtdp = new DataTable();
            //--------------------------------------------------------------------------
            //Fecha: 03/04/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Asignar los datos de la escritura de una sesión a un DataTable.
            //--------------------------------------------------------------------------                        
            if (HttpContext.Current.Session["dtDatosEscritura"] == null)
            {
                dtdp = bl.ActonotarialObtenerDatosPrincipales(Convert.ToInt64(jsonObject["ancu_iActoNotarialId"]), Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                //--------------------------------------------------------------------------
                //Fecha: 03/04/2020
                //Autor: Miguel Márquez Beltrán
                //Motivo: Asignar los datos de la escritura a una sesión.
                //--------------------------------------------------------------------------
                HttpContext.Current.Session["dtDatosEscritura"] = dtdp;
                //--------------------------------------------------------------------------
            }
            else
            {
                dtdp = (DataTable)HttpContext.Current.Session["dtDatosEscritura"];
            }
            string strSubTipoActoNotarialDesc = dtdp.Rows[0]["SubTipoActoNotarialDesc"].ToString().Trim();
            //--------------------------------------------------------------------------
            DataTable dtParticipantesAll = new DataTable();


            dtParticipantesAll = bl.ActonotarialObtenerParticipantes(Convert.ToInt64(jsonObject["ancu_iActoNotarialId"]));


            DataView dv = dtParticipantesAll.DefaultView;
            DataTable dtsOtorgantes = dv.ToTable();

            //dtsOtorgantes = (from p in dtParticipantesAll.AsEnumerable()
            //                 where Convert.ToInt16(p["anpa_sTipoParticipanteId"]) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE)
            //                     || Convert.ToInt16(p["anpa_sTipoParticipanteId"]) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE)
            //                     || Convert.ToInt16(p["anpa_sTipoParticipanteId"]) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR)
            //                     || Convert.ToInt16(p["anpa_sTipoParticipanteId"]) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE)
            //                     || Convert.ToInt16(p["anpa_sTipoParticipanteId"]) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE)
            //                     || Convert.ToInt16(p["anpa_sTipoParticipanteId"]) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO)
            //                 orderby p["vPersona"]
            //                 select p).CopyToDataTable();


            dtsOtorgantes = (from p in dtParticipantesAll.AsEnumerable()
                             where ObtenerIniciaRecibe(Convert.ToInt16(p["anpa_sTipoParticipanteId"])) == "INICIA"
                                 || Convert.ToInt16(p["anpa_sTipoParticipanteId"]) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE)
                                 || Convert.ToInt16(p["anpa_sTipoParticipanteId"]) == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO)
                             orderby p["vPersona"]
                             select p).CopyToDataTable();
            //------------------------------------------------------------------------
            //Fecha: 17/11/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Incluir a los participantes: DONATARIO, ANTICIPADO Y COMPRADOR.
            //-------------------------------------------------------------------------            

            DataTable dtsApoderados = dv.ToTable();


            dtsApoderados = (from p in dtParticipantesAll.AsEnumerable()
                             where ObtenerIniciaRecibe(Convert.ToInt16(p["anpa_sTipoParticipanteId"])) == "RECIBE"                                 
                             orderby p["vPersona"]
                             select p).CopyToDataTable();

            //DataView dvApoderados = dtsApoderados.DefaultView;

            //dvApoderados.RowFilter = "anpa_sTipoParticipanteId = " + Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONATARIO)
            //                          + " or anpa_sTipoParticipanteId = " + Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPADO)
            //                          + " or anpa_sTipoParticipanteId = " + Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.COMPRADOR);

            //DataTable dtApoderadoFiltrado = dvApoderados.ToTable();


            //dtApoderadoFiltrado.DefaultView.Sort = "vPersona";


            //------------------------------------------
            //Datos de los Integrantes
            //------------------------------------------
            int strGenero = 0;
            string strOtorganteArticuloSexoPS = string.Empty;
            string stroOtorganteSingularPlural = string.Empty;

            string strApoderadoArticuloSexoPS = string.Empty;
            string strApoderadoSingularPlural = string.Empty;

            int contadorOtorgantes = 0;
            int contadorApoderados = 0;

            int ContadorParticipanteOtorganteHombres = 0;
            int ContadorParticipanteOtorganteMujer = 0;

            int ContadorParticipanteApoderadosHombres = 0;
            int ContadorParticipanteApoderadosMujer = 0;

            //string strNombreParticipanteOtorgante = string.Empty;

            ArrayList arrListaOtorgantes = new ArrayList();
            ArrayList arrListaApoderados = new ArrayList();
            ArrayList arrListaInterpretes = new ArrayList();
            ArrayList arrListatestigos = new ArrayList();

            string strFechaSuscripcion = "";
            string strTipoParticipante = string.Empty;
            int contadorInterprete = 0;
            int contadorTestigo = 0;

            //---------------------------------------------
            foreach (DataRow dr in dtsOtorgantes.Rows)
            {
                strGenero = Convert.ToInt32(dr["iGenero"]);
                strTipoParticipante = dr["vTipoParticipanteId"].ToString();

                if (dr["anpa_dFechasuscripcion"].ToString().Length > 0)
                {
                    strFechaSuscripcion = Util.ObtenerNumeroDiaLetrasDiaNumerosMesLargoAnio(Comun.FormatearFecha((dr["anpa_dFechasuscripcion"].ToString()).ToString())).ToUpper().Trim();
                }
                else
                {
                    strFechaSuscripcion = "";
                }
                //---------------------------------------------
                if (strTipoParticipante == "INTERPRETE")
                {
                    contadorInterprete++;
                    if (strGenero == (int)Enumerador.enmGenero.MASCULINO)
                    {
                        arrListaInterpretes.Add("EL INTÉRPRETE: " + dr["vPersona"] + " CON FECHA " + strFechaSuscripcion + ".");
                    }
                    else
                    {
                        arrListaInterpretes.Add("LA INTÉRPRETE: " + dr["vPersona"] + " CON FECHA " + strFechaSuscripcion + ".");
                    }
                }
                else
                {
                    if (strTipoParticipante == "TESTIGO A RUEGO")
                    {
                        contadorTestigo++;

                        if (strGenero == (int)Enumerador.enmGenero.MASCULINO)
                        {
                            arrListatestigos.Add("EL TESTIGO A RUEGO: " + dr["vPersona"] + " CON FECHA " + strFechaSuscripcion + ".");
                        }
                        else
                        {
                            arrListatestigos.Add("LA TESTIGO A RUEGO: " + dr["vPersona"] + " CON FECHA " + strFechaSuscripcion + ".");
                        }
                    }
                    else
                    {
                        //pers_bIncapacidadFlag
                        if (strTipoParticipante=="OTORGANTE")
                        {
                            strTipoParticipante = "PODERDANTE";
                        }
                        if (strGenero == (int)Enumerador.enmGenero.MASCULINO)
                        {
                            ContadorParticipanteOtorganteHombres = ContadorParticipanteOtorganteHombres + 1;

                            if (Convert.ToBoolean(dr["pers_bIncapacidadFlag"].ToString()) == true)
                            {
                                if (Convert.ToBoolean(dr["anpa_bFlagHuella"].ToString()) == false)
                                {
                                    arrListaOtorgantes.Add("IMPRESIÓN DACTILAR: EL " + strTipoParticipante + ": " + dr["vPersona"] + " CON FECHA " + strFechaSuscripcion + ".");
                                }
                            }
                            else
                            {
                                arrListaOtorgantes.Add("FIRMA E IMPRESIÓN DACTILAR: EL " + strTipoParticipante + ": " + dr["vPersona"] + " CON FECHA " + strFechaSuscripcion + ".");
                            }

                        }
                        else
                        {
                            ContadorParticipanteOtorganteMujer = ContadorParticipanteOtorganteMujer + 1;

                            if (strTipoParticipante == "VENDEDOR")
                            {
                                strTipoParticipante = "VENDEDORA";
                            }
                            if (strTipoParticipante == "OTORGANTE")
                            {
                                strTipoParticipante = "PODERDANTE";
                            }

                            if (Convert.ToBoolean(dr["pers_bIncapacidadFlag"].ToString()) == true)
                            {


                                arrListaOtorgantes.Add("IMPRESIÓN DACTILAR: LA " + strTipoParticipante + ": " + dr["vPersona"] + " CON FECHA " + strFechaSuscripcion + ".");
                            }
                            else
                            {
                                arrListaOtorgantes.Add("FIRMA E IMPRESIÓN DACTILAR: LA " + strTipoParticipante + ": " + dr["vPersona"] + " CON FECHA " + strFechaSuscripcion + ".");
                            }

                        }
                        //strNombreParticipanteOtorgante += dr["vPersona"] + ", ";

                        contadorOtorgantes = contadorOtorgantes + 1;
                    }
                }
            }
            //---------------------------------------------
            //strNombreParticipanteOtorgante = strNombreParticipanteOtorgante.Substring(0, strNombreParticipanteOtorgante.Length - 2);

            if (contadorOtorgantes == 1)
            {
                stroOtorganteSingularPlural = "PODERDANTE";
                if (strSubTipoActoNotarialDesc == "COMPRA - VENTA")
                { stroOtorganteSingularPlural = "VENDEDOR"; }
                if (strSubTipoActoNotarialDesc == "DONACIÓN")
                { stroOtorganteSingularPlural = "DONANTE"; }
                if (strSubTipoActoNotarialDesc == "ADELANTO DE LEGÍTIMA")
                { stroOtorganteSingularPlural = "ANTICIPANTE"; }
            }
            else
            {
                stroOtorganteSingularPlural = "PODERDANTES";
                if (strSubTipoActoNotarialDesc == "COMPRA - VENTA")
                { stroOtorganteSingularPlural = "VENDEDORES"; }
                if (strSubTipoActoNotarialDesc == "DONACIÓN")
                { stroOtorganteSingularPlural = "DONANTES"; }
                if (strSubTipoActoNotarialDesc == "ADELANTO DE LEGÍTIMA")
                { stroOtorganteSingularPlural = "ANTICIPANTES"; }
            }

            if (ContadorParticipanteOtorganteHombres > 0)
            {
                if (ContadorParticipanteOtorganteHombres == 1)
                {
                    strOtorganteArticuloSexoPS = "EL";
                }
                else
                {
                    strOtorganteArticuloSexoPS = "LOS";
                }
            }
            else
            {
                if (ContadorParticipanteOtorganteMujer == 1)
                {
                    strOtorganteArticuloSexoPS = "LA";
                    if (strSubTipoActoNotarialDesc == "COMPRA - VENTA")
                    { stroOtorganteSingularPlural = "VENDEDORA"; }
                }
                else
                {
                    strOtorganteArticuloSexoPS = "LAS";
                    if (strSubTipoActoNotarialDesc == "COMPRA - VENTA")
                    { stroOtorganteSingularPlural = "VENDEDORAS"; }
                }
            }

            //------------------------------------------Final de datos de los otorgantes.
            #region Apoderados

            foreach (DataRow dr in dtsApoderados.Rows)
            {
                strGenero = Convert.ToInt32(dr["iGenero"]);
                strTipoParticipante = dr["vTipoParticipanteId"].ToString();

                if (dr["anpa_dFechasuscripcion"].ToString().Length > 0)
                {
                    strFechaSuscripcion = Util.ObtenerNumeroDiaLetrasDiaNumerosMesLargoAnio(Comun.FormatearFecha((dr["anpa_dFechasuscripcion"].ToString()).ToString())).ToUpper().Trim();
                }
                else
                {
                    strFechaSuscripcion = "";
                }
                if (strGenero == (int)Enumerador.enmGenero.MASCULINO)
                {

                    ContadorParticipanteApoderadosHombres = ContadorParticipanteApoderadosHombres + 1;
                    arrListaApoderados.Add("FIRMA E IMPRESIÓN DACTILAR: EL " + strTipoParticipante + ": " + dr["vPersona"] + " CON FECHA " + strFechaSuscripcion + ".");
                }
                else
                {
                    ContadorParticipanteApoderadosMujer = ContadorParticipanteApoderadosMujer + 1;

                    if (strTipoParticipante == "COMPRADOR")
                    {
                        strTipoParticipante = "COMPRADORA";
                    }
                    if (strTipoParticipante == "DONATARIO")
                    {
                        strTipoParticipante = "DONATARIA";
                    }
                    if (strTipoParticipante == "ANTICIPADO")
                    {
                        strTipoParticipante = "ANTICIPADA";
                    }
                    if (strTipoParticipante == "APODERADO")
                    {
                        strTipoParticipante = "APODERADA";
                    }
                    arrListaApoderados.Add("FIRMA E IMPRESIÓN DACTILAR: LA " + strTipoParticipante + ": " + dr["vPersona"] + " CON FECHA " + strFechaSuscripcion + ".");
                }
                contadorApoderados = contadorApoderados + 1;
            }

            //-----------------------------------------------------------------------------
            if (contadorApoderados == 1)
            {
                if (strSubTipoActoNotarialDesc == "COMPRA - VENTA")
                { strApoderadoSingularPlural = "COMPRADOR"; }
                if (strSubTipoActoNotarialDesc == "DONACIÓN")
                { strApoderadoSingularPlural = "DONATARIO"; }
                if (strSubTipoActoNotarialDesc == "ADELANTO DE LEGÍTIMA")
                { strApoderadoSingularPlural = "ANTICIPADO"; }
            }
            else
            {
                if (strSubTipoActoNotarialDesc == "COMPRA - VENTA")
                { strApoderadoSingularPlural = "COMPRADORES"; }
                if (strSubTipoActoNotarialDesc == "DONACIÓN")
                { strApoderadoSingularPlural = "DONATARIOS"; }
                if (strSubTipoActoNotarialDesc == "ADELANTO DE LEGÍTIMA")
                { strApoderadoSingularPlural = "ANTICIPADOS"; }
            }
            if (ContadorParticipanteApoderadosHombres > 0)
            {
                if (ContadorParticipanteApoderadosHombres == 1)
                {
                    strApoderadoArticuloSexoPS = "EL";
                }
                else
                {
                    strApoderadoArticuloSexoPS = "LOS";
                }
            }
            else
            {
                if (ContadorParticipanteApoderadosMujer == 1)
                {
                    strApoderadoArticuloSexoPS = "LA";
                    if (strSubTipoActoNotarialDesc == "COMPRA - VENTA")
                    { strApoderadoSingularPlural = "COMPRADORA"; }
                    if (strSubTipoActoNotarialDesc == "DONACIÓN")
                    { strApoderadoSingularPlural = "DONATARIA"; }
                    if (strSubTipoActoNotarialDesc == "ADELANTO DE LEGÍTIMA")
                    { strApoderadoSingularPlural = "ANTICIPADA"; }
                }
                else
                {
                    strApoderadoArticuloSexoPS = "LAS";
                    if (strSubTipoActoNotarialDesc == "COMPRA - VENTA")
                    { strApoderadoSingularPlural = "COMPRADORAS"; }
                    if (strSubTipoActoNotarialDesc == "DONACIÓN")
                    { strApoderadoSingularPlural = "DONATARIAS"; }
                    if (strSubTipoActoNotarialDesc == "ADELANTO DE LEGÍTIMA")
                    { strApoderadoSingularPlural = "ANTICIPADAS"; }
                }
            }
            #endregion
            //------------------------------------------Final de datos de los Apoderados.
            //-----------------------------------------------------------------------------
            string strFuncionarioAutorizador = dtdp.Rows[0]["NombreFuncionario"].ToString().Trim();

            string strCargoFuncionarioAutorizador = dtdp.Rows[0]["CargoFuncionario"].ToString().Trim();
            string strUbigeoOficinaConsular = dtdp.Rows[0]["CiudadOficinaConsular"].ToString().Trim();

            //string strFechaLarga = Util.ObtenerNumeroDiaLetrasDiaNumerosMesLargoAnio(Comun.FormatearFecha(dtdp.Rows[0]["Fecha"].ToString()), true);
            string strFechaLarga = "";

            if (dtdp.Rows[0]["FechaconclusionFirma"].ToString().Length > 0)
            {
                strFechaLarga = Util.ObtenerNumeroDiaLetrasDiaNumerosMesLargoAnio(Comun.FormatearFecha(dtdp.Rows[0]["FechaconclusionFirma"].ToString()), true);
            }

            string strCadFojas = "0";
            string strCadFojasLetras = "CERO";

            string strLibroRomanos = "LIBR000";

            StringBuilder sScript = new StringBuilder();


            if (contadorInterprete > 0)
            {
                #region Interprete

                sScript.Append("<p style='text-align:justify;'>");
                //-------------------------------------------
                //ARTÍCULO 30 DEL DECRETO LEGISLATIVO N°1049               
                //-------------------------------------------
                DataTable dtNorma = new DataTable();
                NormaTarifarioDL objNormaBL = new NormaTarifarioDL();

                int IntTotalCount = 0;
                int IntTotalPages = 0;
                short intGrupoNormaId = 0;
                string strNorma = "ARTÍCULO 30 DEL DECRETO LEGISLATIVO N°1049";
                intGrupoNormaId = ObtenerGrupoNorma("ESCRITURAS");

                dtNorma = objNormaBL.ConsultarNorma(0, 0, strNorma, "", "", 0, intGrupoNormaId, 1, 1, "S", ref IntTotalCount, ref IntTotalPages);

                string strNormaDescripcion = "";
                //\r\n

                if (dtNorma.Rows.Count > 0)
                {
                    strNormaDescripcion = dtNorma.Rows[0]["norm_vDescripcion"].ToString().Trim();
                    int intIndice = strNormaDescripcion.IndexOf("\r\n");

                    if (intIndice > -1)
                    {
                        sScript.Append("INSERTO: " + strNorma + ".-" + strNormaDescripcion.Substring(0, intIndice));
                        sScript.Append("<p style='text-align:justify;'>");
                        sScript.Append(strNormaDescripcion.Substring(intIndice + 2));
                        sScript.Append("</p>");
                    }
                    else
                    {
                        sScript.Append("INSERTO: " + strNorma + ".-" + strNormaDescripcion);
                    }
                }
                //-------------------------------------------
                sScript.Append("</p>");
                #endregion
            }

            if (contadorTestigo > 0)
            {
                #region Testigo
                sScript.Append("<p style='text-align:justify;'>");
                //-----------------------------------------------------------
                //INCISO G) DEL ARTÍCULO 54 DEL DECRETO LEGISLATIVO N°1049               
                //-----------------------------------------------------------
                DataTable dtNorma = new DataTable();
                NormaTarifarioDL objNormaBL = new NormaTarifarioDL();

                int IntTotalCount = 0;
                int IntTotalPages = 0;
                short intGrupoNormaId = 0;
                intGrupoNormaId = ObtenerGrupoNorma("ESCRITURAS");
                string strNorma = "INCISO G) DEL ARTÍCULO 54 DEL DECRETO LEGISLATIVO N°1049";

                dtNorma = objNormaBL.ConsultarNorma(0, 0, strNorma, "", "", 0, intGrupoNormaId,1, 1, "S", ref IntTotalCount, ref IntTotalPages);

                if (dtNorma.Rows.Count > 0)
                {
                    sScript.Append("INSERTO: " + strNorma + ".-" + dtNorma.Rows[0]["norm_vDescripcion"].ToString().Trim());
                }

                //-----------------------------------------------------------
                sScript.Append("</p>");
                #endregion
            }

            //----------------------------------------------------------------------
            //Fecha: 14/10/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Insertos para la Renuncia a la Nacionalidad Peruana
            //----------------------------------------------------------------------
            if (dtdp.Rows[0]["SubTipoActoNotarialId"].ToString() == "8069")
            {
                sScript.Append("<p style='text-align:justify;'>");
                sScript.Append("CONSTITUCIÓN POLÍTICA DEL PERÚ: ARTÍCULO 53.- ADQUISICIÓN Y RENUNCIA DE LA NACIONALIDAD. LA LEY REGULA LAS FORMAS EN QUE SE ADQUIERE O RECUPERA LA NACIONALIDAD. LA NACIONALIDAD PERUANA NO SE PIERDE, SALVO POR RENUNCIA EXPRESA ANTE AUTORIDAD PERUANA.");
                sScript.Append("</p>");
                sScript.Append("<p style='text-align:justify;'>");
                sScript.Append("LEY N° 26574.- LEY DE NACIONALIDAD.- ARTÍCULO 7.- PERDIDA DE LA NACIONALIDAD. LA NACIONALIDAD PERUANA SE PIERDE POR RENUNCIA EXPRESA ANTE AUTORIDAD COMPETENTE.");
                sScript.Append("</p>");
            }
            //----------------------------------------------------------------------

            //----------------------------------------------------------------------

            sScript.Append("<p style='text-align:justify;'>");

            sScript.Append("LA PRESENTE ESCRITURA PÚBLICA SE ENCUENTRA INSCRITA EN LAS FOJAS " + strCadFojasLetras + " (" + strCadFojas + ") ");

            sScript.Append("DEL <b>LIBRO</b> " + strLibroRomanos + " DEL REGISTRO DE INSTRUMENTOS PÚBLICOS DE ESTE CONSULADO.");
            sScript.Append("</p>");

            sScript.Append("<p style='text-align:justify;'>");
            //----------------------------------------------------------------
            //Fecha: 01/04/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Modificar el texto para cuando sea un solo otorgante. 
            //----------------------------------------------------------------

            if ((contadorOtorgantes + contadorApoderados) == 1)
            {
                sScript.Append("CONCLUYE EL PROCESO DE FIRMA E IMPRESIÓN DACTILAR ANTE MÍ, ");
            }
            else
            {
                sScript.Append("CONCLUYE EL PROCESO DE FIRMAS E IMPRESIONES DACTILARES ANTE MÍ, ");
            }
            //----------------------------------------------------------------
            sScript.Append("<b>" + strFuncionarioAutorizador + ",</b> " + strCargoFuncionarioAutorizador + " DEL PERÚ EN ESTA CIUDAD, EL DÍA " + strFechaLarga + ", DE LO QUE DOY FE.");

            sScript.Append("</p>");

            for (int i = 0; i < arrListaOtorgantes.Count; i++)
            {
                sScript.Append("<p style='text-align:justify;'>");
                // sScript.Append("FIRMA E IMPRESIÓN DACTILAR: " + arrListaOtorgantes[i]);
                sScript.Append(arrListaOtorgantes[i]);
                sScript.Append("</p>");
            }

            for (int i = 0; i < arrListaInterpretes.Count; i++)
            {
                sScript.Append("<p style='text-align:justify;'>");
                sScript.Append("FIRMA E IMPRESIÓN DACTILAR: " + arrListaInterpretes[i]);
                sScript.Append("</p>");
            }

            for (int i = 0; i < arrListatestigos.Count; i++)
            {
                sScript.Append("<p style='text-align:justify;'>");
                sScript.Append("FIRMA E IMPRESIÓN DACTILAR: " + arrListatestigos[i]);
                sScript.Append("</p>");
            }
            //------------------------------------------------------------
            //for (int i = 0; i < arrListaApoderados.Count; i++)
            //{
            //    sScript.Append("<p style='text-align:justify;'>");
            //    sScript.Append(arrListaApoderados[i]);
            //    sScript.Append("</p>");
            //}
            //------------------------------------------------------------

            sScript.Append("<p style='text-align:justify;'>");
            sScript.Append("<b>FIRMA: " + strFuncionarioAutorizador + ",</b> CARGO: " + strCargoFuncionarioAutorizador + " DEL PERÚ EN " + strUbigeoOficinaConsular + ".");

            sScript.Append("</p>");



            string innerString = sScript.ToString();

            return innerString.ToString();

        }

        protected void ibRecargar_Click(object sender, ImageClickEventArgs e)
        {
            DataTable dtTarifario;
            dtTarifario = cargar_tarifas();

            Session.Remove("dtTarifarioFiltrado");
            Session.Add("dtTarifarioFiltrado", dtTarifario);

            Txt_TarifaId.Text = string.Empty;
            Txt_TarifaDescripcion.Text = string.Empty;
            LimpiarDatosTarifaPago();
            DataTable dtTarifarioFiltrado = (DataTable)Session["dtTarifarioFiltrado"];
            CargarListaTarifario(dtTarifarioFiltrado);
            updRegPago.Update();
        }

        protected void btnCancelarAprobacion_Click(object sender, EventArgs e)
        {
            ActoNotarialMantenimiento bl = new ActoNotarialMantenimiento();
            RE_ACTONOTARIAL actonotarialBE = new RE_ACTONOTARIAL();
            actonotarialBE.acno_iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
            actonotarialBE.acno_sEstadoId = (int)Enumerador.enmNotarialProtocolarEstado.TRANSCRITA;
            actonotarialBE.acno_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            actonotarialBE.acno_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            actonotarialBE.acno_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
            bl.ActoNotarialActualizarEstado(actonotarialBE);
            //---------------------------------------------------
            /*DataTable dt = new DataTable();
            dt = (DataTable)Session["dtActuacionDetalle"];
            dt.Rows.Clear();
            */


            Lbl_TotalGeneral.Text = "0.00";
            Lbl_TotalExtranjera.Text = "0.00";

            string StrScript = string.Empty;
//            StrScript = @"$('#MainContent_divListNormativo').removeClass('disableElementsOfDiv');$('#MainContent_Btn_AfirmarTextoLeido').prop('disabled', true);$('#MainContent_cbxAfirmarTexto').prop('disabled', true);";
            StrScript = @"$('#MainContent_Btn_AfirmarTextoLeido').prop('disabled', true);$('#MainContent_cbxAfirmarTexto').prop('disabled', true);";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitarChec", StrScript, true);
            //cbxAfirmarTexto.Enabled = false;
            //------limpiamos la grilla de detale montos
            ddlTipoPago.SelectedIndex = 0;
            Gdv_Tarifa.DataSource = null;
            Gdv_Tarifa.DataBind();
            Session["dtActuacionDetalle"] = null;
            //------refrescamos las tarifas
            DataTable dtTarifario;
            dtTarifario = cargar_tarifas();
            Session.Remove("dtTarifarioFiltrado");
            Session.Add("dtTarifarioFiltrado", dtTarifario);
            Txt_TarifaId.Text = string.Empty;
            Txt_TarifaDescripcion.Text = string.Empty;
            LimpiarDatosTarifaPago();
            DataTable dtTarifarioFiltrado = (DataTable)Session["dtTarifarioFiltrado"];
            CargarListaTarifario(dtTarifarioFiltrado);
            updRegPago.Update();

            //-----------------------------------------------
            // Presentante
            //-----------------------------------------------
            rbApoderado.Enabled = true;
            rbOtros.Enabled = true;
            rbApoderado.Checked = true;
            ddl_TipoDocrepresentante.Enabled = false;
            txtRepresentanteNombres.Enabled = false;
            txtRepresentanteNroDoc.Enabled = false;
            ddl_GerenoPresentante.Enabled = false;
            ddl_Apoderado.Enabled = true;
            updParte.Update();
            //-----------------------------------------------
            string script = @"$(function(){{";
            script += "pageLoadedHandler();}});";
            script = string.Format(script);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "Habilitar_Tab", script, true);

            string script2 = @"$(function(){{";
            script2 += "habilitarTabCuerpo();limpiarCamposAprobacionPagos();Desabilitar_Tab(3);}});";
            script2 = string.Format(script2);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "Habilitar_Tab_Cuerpo", script2, true);
            
        }

        [System.Web.Services.WebMethod]
        public static string CalculoVisual_EP_Testimonio_Parte(string strActoNotarialId, string strActuacionId,
            string strActoNotarialTipo, string strTipoActoNotarial, string strNumeroPagina)
        {
            //string strTipoActoProtocolar = Cmb_TipoActoNotarial.SelectedItem.Text.Trim().ToUpper();
            Int64 intActoNotarialId = Convert.ToInt64(strActoNotarialId);
            Int64 intActuacionId = Convert.ToInt64(strActuacionId);
            Int16 intCantidadFojas = 0;
            Int16 intActoNotarialTipo = Convert.ToInt16(strActoNotarialTipo);
            intCantidadFojas = Convert.ToInt16(strNumeroPagina);
            intCantidadFojas++;

            DataTable dtTarifas = new DataTable();
            dtTarifas = (DataTable)HttpContext.Current.Session["tarifasActoProtocolar"];

            double dCostoEP = 0;
            double dMontoEP = 0;
            double dCostoApoderado = 0;
            double dMontoApoderado = 0;
            double dCostoOtorgante = 0;
            double dMontoOtorgante = 0;
            double dCostoTestimonio = 0;
            double dCostoParte = 0;
            double dMontoTestimonio = 0;
            double dMontoParte = 0;
            string strTarifa = "";

            for (int i = 0; i < dtTarifas.Rows.Count; i++)
            {
                if (strTipoActoNotarial.Equals(dtTarifas.Rows[i]["tipo"].ToString()))
                {
                    strTarifa = dtTarifas.Rows[i]["tarifa"].ToString();
                    dCostoEP = Convert.ToDouble(dtTarifas.Rows[i]["costo"].ToString());
                    break;
                }
            }
            //-------------------------------------
            int intCantidadOtorgante = 0;
            int intCantidadApoderado = 0;
            
            if (intActoNotarialTipo == (int)Enumerador.enmProtocolarTipo.PODER_GENERAL_AMPLIO_ABSOLUTO ||
                intActoNotarialTipo == (int)Enumerador.enmProtocolarTipo.PODER_ESPECIAL)
            {
                if (strTarifa.Equals("12A") || strTarifa.Equals("13A"))
                {
                    //intCantidadOtorgante = ObtenerCantidadParticipantesPorTipo(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE);
                    //intCantidadApoderado = ObtenerCantidadParticipantesPorTipo(Enumerador.enmNotarialProtocolarTipoParticipante.APODERADO);
                    intCantidadOtorgante = ObtenerCantidadParticipantesPorTipo("INICIA");
                    intCantidadApoderado = ObtenerCantidadParticipantesPorTipo("RECIBE");
                }
                if (intActoNotarialTipo == (int)Enumerador.enmProtocolarTipo.PODER_GENERAL_AMPLIO_ABSOLUTO)
                {
                    for (int i = 0; i < dtTarifas.Rows.Count; i++)
                    {
                        if (dtTarifas.Rows[i]["tarifa"].ToString().Equals("12B"))
                        {
                            dCostoOtorgante = Convert.ToDouble(dtTarifas.Rows[i]["costo"].ToString());
                            break;
                        }
                    }
                    for (int i = 0; i < dtTarifas.Rows.Count; i++)
                    {
                        if (dtTarifas.Rows[i]["tarifa"].ToString().Equals("12C"))
                        {
                            dCostoApoderado = Convert.ToDouble(dtTarifas.Rows[i]["costo"].ToString());
                            break;
                        }
                    }
                }
                if (intActoNotarialTipo == (int)Enumerador.enmProtocolarTipo.PODER_ESPECIAL)
                {
                    for (int i = 0; i < dtTarifas.Rows.Count; i++)
                    {
                        if (dtTarifas.Rows[i]["tarifa"].ToString().Equals("13B"))
                        {
                            dCostoOtorgante = Convert.ToDouble(dtTarifas.Rows[i]["costo"].ToString());
                            break;
                        }
                    }
                    for (int i = 0; i < dtTarifas.Rows.Count; i++)
                    {
                        if (dtTarifas.Rows[i]["tarifa"].ToString().Equals("13C"))
                        {
                            dCostoApoderado = Convert.ToDouble(dtTarifas.Rows[i]["costo"].ToString());
                            break;
                        }
                    }
                }
            }

            if (intCantidadOtorgante > 1)
            {
                dMontoOtorgante = (intCantidadOtorgante - 1) * dCostoOtorgante;
            }
            else
            {
                dMontoOtorgante = 0;
            }
            if (intCantidadApoderado > 1)
            {
                dMontoApoderado = (intCantidadApoderado - 1) * dCostoApoderado;
            }
            else
            {
                dMontoApoderado = 0;
            }
            dMontoEP = dCostoEP + dMontoOtorgante + dMontoApoderado;
            //-----------------------------------------------------
            //Testimonio
            //-----------------------------------------------------
            for (int i = 0; i < dtTarifas.Rows.Count; i++)
            {
                if (dtTarifas.Rows[i]["tarifa"].ToString().Equals("17A"))
                {
                    dCostoTestimonio = Convert.ToDouble(dtTarifas.Rows[i]["costo"].ToString());
                    break;
                }
            }
            dMontoTestimonio = dCostoTestimonio + (intCantidadFojas - 1) * 8;
            //-----------------------------------------------------
            //Parte adicional
            //-----------------------------------------------------
            for (int i = 0; i < dtTarifas.Rows.Count; i++)
            {
                if (dtTarifas.Rows[i]["tarifa"].ToString().Equals("17C"))
                {
                    dCostoParte = Convert.ToDouble(dtTarifas.Rows[i]["costo"].ToString());
                    break;
                }
            }
            dMontoParte = dCostoParte + (intCantidadFojas - 1) * 8;
            string strFormato = ConfigurationManager.AppSettings["FormatoMonto"].ToString();

            clsRespuestaEP objRespuesta = new clsRespuestaEP();
            objRespuesta.CostoEP = dMontoEP.ToString(strFormato); 
            objRespuesta.CostoParte2 = dMontoParte.ToString(strFormato);
            objRespuesta.CostoTestimonio = dMontoTestimonio.ToString(strFormato);
            string oRespuesta;
            oRespuesta = new JavaScriptSerializer().Serialize(objRespuesta);
            return oRespuesta;
        }

        private static void llenarTablaTarifasActoProtocolar()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("tipo", typeof(string));
            dt.Columns.Add("concepto", typeof(string));
            dt.Columns.Add("tarifa", typeof(string));
            dt.Columns.Add("costo", typeof(double));
            DataRow dr;
            dr = dt.NewRow();
            dr["tipo"] = "ADELANTO DE LEGÍTIMA";
            dr["concepto"] = "ADELANTO DE LEGÍTIMA";
            dr["tarifa"] = "";
            dr["costo"] = 0;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["tipo"] = "PODER GENERAL";
            dr["concepto"] = "PODER GENERAL";
            dr["tarifa"] = "12A";
            dr["costo"] = 0;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["tipo"] = "PODER GENERAL";
            dr["concepto"] = "POR CADA PODERDANTE ADEMAS";
            dr["tarifa"] = "12B";
            dr["costo"] = 0;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["tipo"] = "PODER GENERAL";
            dr["concepto"] = "POR CADA APODERADO ADEMAS";
            dr["tarifa"] = "12C";
            dr["costo"] = 0;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["tipo"] = "PODER ESPECIAL";
            dr["concepto"] = "PODER ESPECIAL";
            dr["tarifa"] = "13A";
            dr["costo"] = 0;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["tipo"] = "PODER ESPECIAL";
            dr["concepto"] = "POR PODERDANTE ADICIONAL";
            dr["tarifa"] = "13B";
            dr["costo"] = 0;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["tipo"] = "PODER ESPECIAL";
            dr["concepto"] = "POR APODERADO ADICIONAL";
            dr["tarifa"] = "13C";
            dr["costo"] = 0;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["tipo"] = "COMPRA - VENTA";
            dr["concepto"] = "COMPRA VENTA Y OTROS";
            dr["tarifa"] = "5A";
            dr["costo"] = 0;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["tipo"] = "REVOCACIÓN DE PODER GENERAL";
            dr["concepto"] = "POR REVOCACIÓN PODER GENERAL";
            dr["tarifa"] = "12D";
            dr["costo"] = 0;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["tipo"] = "MODIFICACIÓN DE PODER GENERAL";
            dr["concepto"] = "MODIFICACIÓN PODER GENERAL";
            dr["tarifa"] = "12E";
            dr["costo"] = 0;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["tipo"] = "AMPLIACIÓN DE PODER GENERAL";
            dr["concepto"] = "AMPLIACIÓN PODER GENERAL";
            dr["tarifa"] = "12F";
            dr["costo"] = 0;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["tipo"] = "REVOCACIÓN DE PODER ESPECIAL";
            dr["concepto"] = "REVOCACIÓN PODER ESPECIAL";
            dr["tarifa"] = "13D";
            dr["costo"] = 0;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["tipo"] = "MODIFICACIÓN DE PODER ESPECIAL";
            dr["concepto"] = "MODIFICACIÓN PODER ESPECIAL";
            dr["tarifa"] = "13E";
            dr["costo"] = 0;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["tipo"] = "AMPLIACIÓN DE PODER ESPECIAL";
            dr["concepto"] = "AMPLIACIÓN PODER ESPECIAL";
            dr["tarifa"] = "13F";
            dr["costo"] = 0;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["tipo"] = "CUALQUIER ACTO JURÍDICO NO ESPECIFICADO";
            dr["concepto"] = "CUALQUIER ACTO JURÍDICO NO ESPECIFICADO";
            dr["tarifa"] = "20A";
            dr["costo"] = 0;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["tipo"] = "DONACIÓN";
            dr["concepto"] = "COMPRA VENTA Y OTROS";
            dr["tarifa"] = "5A";
            dr["costo"] = 0;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["tipo"] = "RECTIFICACIÓN DE PODER ESPECIAL";
            dr["concepto"] = "RECTIFICACIÓN PODER ESPECIAL";
            dr["tarifa"] = "13H";
            dr["costo"] = 0;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["tipo"] = "RENUNCIA A LA NACIONALIDAD PERUANA";
            dr["concepto"] = "";
            dr["tarifa"] = "";
            dr["costo"] = 0;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["tipo"] = "RECTIFICACIÓN DE PODER GENERAL";
            dr["concepto"] = "RECTIFICACIÓN PODER GENERAL";
            dr["tarifa"] = "12H";
            dr["costo"] = 0;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["tipo"] = "";
            dr["concepto"] = "TESTIMONIO DE ESCRITURA PUBLICA";
            dr["tarifa"] = "17A";
            dr["costo"] = 0;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["tipo"] = "";
            dr["concepto"] = "EXPEDICIÓN PARTE ADICIONAL";
            dr["tarifa"] = "17C";
            dr["costo"] = 0;
            dt.Rows.Add(dr);

            //--------------------------------------------------
            DataTable dtTarifario = new DataTable();
            dtTarifario = cargar_tarifas();
            string strTarifa = "";
            string strNumeroLetra = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strTarifa = dt.Rows[i]["tarifa"].ToString();
                if (strTarifa.Trim().Length > 0)
                {
                    for (int x = 0; x < dtTarifario.Rows.Count; x++)
                    {
                        strNumeroLetra = dtTarifario.Rows[x]["tari_sNumero"].ToString() + dtTarifario.Rows[x]["tari_vLetra"].ToString();
                        if (strTarifa.Equals(strNumeroLetra))
                        {
                            dt.Rows[i]["costo"] = Convert.ToDouble(dtTarifario.Rows[x]["tari_FCosto"].ToString());
                            break;
                        }
                    }
                }
            }
            //-----------------------------------
            HttpContext.Current.Session["tarifasActoProtocolar"] = dt;
        }

        private void buscarValorTipoDocumento(ref DropDownList ddlTipoDocumento, string strTipoDocumentoId)
        {
            bool bExisteDocumento = false;

            for (int i = 0; i < ddlTipoDocumento.Items.Count; i++)
            {
                if (ddlTipoDocumento.Items[i].Value.Equals(strTipoDocumentoId))
                {
                    bExisteDocumento = true;
                    break;
                }
            }
            if (bExisteDocumento)
            {
                ddlTipoDocumento.SelectedValue = strTipoDocumentoId;
            }
            else
            {
                ddlTipoDocumento.SelectedIndex = 0;
            }             
        }

        private bool validarCuerpo()
        {
            if (RichTextBox.Text.Trim().Length == 0)
            {
                EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "PRESENTANTES", "Debe registrar en el texto central."), "Información");
                return false;
            }

            if (GridViewPresentante.Rows.Count == 0)
            {
                EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "PRESENTANTES", "Debe adicionar un presentante."), "Información");
                return false;
            }
            return true;
        }

        static private short ObtenerGrupoNorma(string strDescripcion)
        {
            DataTable dtGrupoNorma = new DataTable();

            dtGrupoNorma = comun_Part1.ObtenerParametrosPorGrupo(HttpContext.Current.Session, "NORMA-GRUPO");
            Int16 intGrupoNormaId = 0;

            for (int i = 0; i < dtGrupoNorma.Rows.Count; i++)
            {
                if (dtGrupoNorma.Rows[i]["descripcion"].ToString().Equals(strDescripcion))
                {
                    intGrupoNormaId = Convert.ToInt16(dtGrupoNorma.Rows[i]["id"].ToString());
                    break;
                }
            }
            return intGrupoNormaId;
        }
       

        //---------------------------------------------------
        //Fecha: 25/04/2022
        //Autor: Miguel Márquez Beltrán
        //Motivo: Requerimiento SGAC_22_04_2022_EP
        //---------------------------------------------------

        protected void ddlFuenteNormaConsulta_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strTipoNormaId = "0";

            if (ddlFuenteNormaConsulta.SelectedIndex > 0)
            {
                strTipoNormaId = ddlFuenteNormaConsulta.SelectedValue;
                short iTipoNormaId = Convert.ToInt16(strTipoNormaId);
                NormaTarifarioDL objNormasoDL = new NormaTarifarioDL();
                DataTable dtTitulosNormas = new DataTable();
                short iEstadoNormaId = 0;
                short iGrupoNormaId = 0;
                if (ViewState["VIGENTE"] != null)
                {
                    iEstadoNormaId = Convert.ToInt16(ViewState["VIGENTE"].ToString());
                }
                if (ViewState["ESCRITURAS"] != null)
                {
                    iGrupoNormaId = Convert.ToInt16(ViewState["ESCRITURAS"].ToString());
                }

                dtTitulosNormas = objNormasoDL.ListaTitulosArticulosNorma(0,iTipoNormaId, 0, iEstadoNormaId, iGrupoNormaId);
                Util.CargarParametroDropDownList(ddlTituloNormaConsulta, dtTitulosNormas,true);
                this.ddlArticuloNormaConsulta.Items.Clear();
                this.ddlArticuloNormaConsulta.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
                this.txtSumillaNormaConsulta.Text = "";
                txtDescNormaConsulta.Text = "";
            }
            else
            {
                this.ddlTituloNormaConsulta.Items.Clear();
                this.ddlTituloNormaConsulta.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
                this.ddlArticuloNormaConsulta.Items.Clear();
                this.ddlArticuloNormaConsulta.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
                this.txtSumillaNormaConsulta.Text = "";
                txtDescNormaConsulta.Text = "";
            }

        }
        protected void ddlTituloNormaConsulta_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strTipoNormaId = "0";
            string strTituloId = "0";
            if (ddlTituloNormaConsulta.SelectedIndex > 0)
            {
                strTipoNormaId = ddlFuenteNormaConsulta.SelectedValue;
                short iTipoNormaId = Convert.ToInt16(strTipoNormaId);

                strTituloId = ddlTituloNormaConsulta.SelectedValue;
                NormaTarifarioDL objNormasoDL = new NormaTarifarioDL();
                DataTable dtArticulosNormas = new DataTable();
                short iTituloId = Convert.ToInt16(strTituloId);
                short iEstadoNorma = 0;
                short iGrupoNorma = 0;
                if (ViewState["VIGENTE"] != null)
                {
                    iEstadoNorma = Convert.ToInt16(ViewState["VIGENTE"].ToString());
                }
                if (ViewState["ESCRITURAS"] != null)
                {
                    iGrupoNorma = Convert.ToInt16(ViewState["ESCRITURAS"].ToString());
                }
                dtArticulosNormas = objNormasoDL.ListaTitulosArticulosNorma(0,iTipoNormaId, iTituloId, iEstadoNorma, iGrupoNorma);
                Util.CargarParametroDropDownList(ddlArticuloNormaConsulta, dtArticulosNormas,true);
                Session["ArticulosNormas"] = dtArticulosNormas;
                this.txtSumillaNormaConsulta.Text = "";
                txtDescNormaConsulta.Text = "";
            }
            else
            {
                this.ddlArticuloNormaConsulta.Items.Clear();
                this.ddlArticuloNormaConsulta.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
                this.txtSumillaNormaConsulta.Text = "";
                txtDescNormaConsulta.Text = "";

                Session.Remove("ArticulosNormas");
            }

//            string strScript = string.Empty;
//            strScript = @"$(function(){{
//                                            LimpiarDescripcionNorma(); 
//                                            }});";
//            strScript = string.Format(strScript);
//            ScriptManager.RegisterStartupScript(Page, typeof(Page), "limpiarDescripcion", strScript, true);

        }
        protected void ddlArticuloNormaConsulta_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strArticuloid = "0";
            if (ddlArticuloNormaConsulta.SelectedIndex > 0 && Session["ArticulosNormas"] != null)
            {
                strArticuloid = ddlArticuloNormaConsulta.SelectedValue;
                DataTable dtArticulosNormas = new DataTable();
                dtArticulosNormas = (DataTable)Session["ArticulosNormas"];

                this.txtSumillaNormaConsulta.Text = "";

                for (int i = 0; i < dtArticulosNormas.Rows.Count; i++)
                {
                    if (dtArticulosNormas.Rows[i]["id"].ToString().Equals(strArticuloid))
                    {
                        txtSumillaNormaConsulta.Text = dtArticulosNormas.Rows[i]["norm_vNombreArticulo"].ToString();
                        string strDescripcion="";
                        strDescripcion = dtArticulosNormas.Rows[i]["norm_vDescripcion"].ToString();
                        txtDescNormaConsulta.Text = strDescripcion.Replace("<p>", "").Replace("</p>","");
                        
                        break;
                    }
                }                
            }
            else
            {
                txtSumillaNormaConsulta.Text = "";
                txtDescNormaConsulta.Text = "";
            }
            
        }
        protected void btnAceptarNormaConsulta_Click(object sender, EventArgs e)
        {
            if (validarNormas())
            {
                EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "ACTO NOTARIAL - NORMAS", "No se permite la duplicidad de normas."), "ValidarNorma");
                return;
            }

            ActoNotarial_NormaMantenimientoBL objNormasMantenimientoBL = new ActoNotarial_NormaMantenimientoBL();
            RE_ACTONOTARIAL_NORMA actoNotarial_normaBE = new RE_ACTONOTARIAL_NORMA();
             Int64 iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
             string strArticuloid = ddlArticuloNormaConsulta.SelectedValue;
             short iNormaId = Convert.ToInt16(strArticuloid);

             actoNotarial_normaBE.anra_iActoNotarialId = iActoNotarialId;
             actoNotarial_normaBE.anra_sNormaId = iNormaId;
             actoNotarial_normaBE.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
             actoNotarial_normaBE.anra_sUsuarioCreacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
             actoNotarial_normaBE.anra_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
             objNormasMantenimientoBL.insertar(actoNotarial_normaBE);
             if (actoNotarial_normaBE.Error == false)
             {
                 ActoNotarial_NormasConsultaBL objActoNotarialNormasConsultaBL = new ActoNotarial_NormasConsultaBL();
                 DataTable dtActoNotarialNormas = new DataTable();
                 dtActoNotarialNormas = objActoNotarialNormasConsultaBL.ObtenerNormas(actoNotarial_normaBE);
                 gdv_Normas.DataSource = dtActoNotarialNormas;
                 gdv_Normas.DataBind();
                 hf_idsNormativos.Value = ObtenerIdNormas(dtActoNotarialNormas);
                 if (dtActoNotarialNormas.Rows.Count == 0)
                 {
                     gdv_Normas.Visible = false;
                 }
                 else
                 {
                     gdv_Normas.Visible = true;
                 }
                 updTxtNormativo.Update();

                 string StrScript2 = string.Empty;
                 StrScript2 = @"setearTextoNormativo();";
                 StrScript2 = string.Format(StrScript2);
                 ScriptManager.RegisterStartupScript(Page, typeof(Page), "cargLstTxtNormtivo", StrScript2, true);
                 
                 //------------------------------------
                 string strScript = string.Empty;
                 strScript = @"$(function(){{
                                            limpiarNorma(); 
                                            }});";
                 strScript = string.Format(strScript);
                 ScriptManager.RegisterStartupScript(Page, typeof(Page), "cerrarPopupBusquedaNorma", strScript, true);

             }
        }
        protected void gdv_Normas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 lRowIndex = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName.ToString())
            {
                case "AnularNorma":
                    Int64 iActoNotarialNormaId = Convert.ToInt64(gdv_Normas.DataKeys[lRowIndex].Values["anra_iActoNotarialNormaId"].ToString());

                    ActoNotarial_NormaMantenimientoBL objActoNotarialMantenimientoBL = new ActoNotarial_NormaMantenimientoBL();
                    
                    RE_ACTONOTARIAL_NORMA ActoNotarialNormaBE = new RE_ACTONOTARIAL_NORMA();
                    ActoNotarialNormaBE.anra_iActoNotarialNormaId = iActoNotarialNormaId;
                    ActoNotarialNormaBE.anra_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                    ActoNotarialNormaBE.anra_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
                    ActoNotarialNormaBE.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    ActoNotarialNormaBE = objActoNotarialMantenimientoBL.anular(ActoNotarialNormaBE);

                    if (ActoNotarialNormaBE.Error == false)
                    {
                        ActoNotarial_NormasConsultaBL objActoNotarialNormasConsultaBL = new ActoNotarial_NormasConsultaBL();
                        DataTable dtActoNotarialNormas = new DataTable();
                        RE_ACTONOTARIAL_NORMA actoNotarial_normaBE = new RE_ACTONOTARIAL_NORMA();
                        Int64 iActoNotarialId = Convert.ToInt64(this.hdn_acno_iActoNotarialId.Value);
                        actoNotarial_normaBE.anra_iActoNotarialId = iActoNotarialId;
                        dtActoNotarialNormas = objActoNotarialNormasConsultaBL.ObtenerNormas(actoNotarial_normaBE);
                        
                        gdv_Normas.DataSource = dtActoNotarialNormas;
                        gdv_Normas.DataBind();

                        if (dtActoNotarialNormas.Rows.Count == 0)
                        {
                            gdv_Normas.Visible = false;
                        }
                        else
                        {
                            gdv_Normas.Visible = true;
                        }
                        //Ejemplo: 8157|8159|8160|8151|
                        hf_idsNormativos.Value = ObtenerIdNormas(dtActoNotarialNormas);
                        updTxtNormativo.Update();

                        string StrScript2 = string.Empty;
                        StrScript2 = @"setearTextoNormativo();";
                        StrScript2 = string.Format(StrScript2);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "cargLstTxtNormtivo", StrScript2, true);                        
                        //----------------------------------------------------------

                    }
                    else
                    {
                        EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTO NOTARIAL - NORMAS", "No se ha anulado la norma del Acto Notarial"), "anularNormaActoNotarial");
                    }
                    break;
                case "VisualizarNorma":
                    short iNormaId = Convert.ToInt16(gdv_Normas.DataKeys[lRowIndex].Values["anra_sNormaId"].ToString());

                    NormaTarifarioDL objNormasoDL = new NormaTarifarioDL(); 
                    
                    DataTable dtArticulosNormas = new DataTable();
                    dtArticulosNormas = objNormasoDL.ListaTitulosArticulosNorma(iNormaId);
                    if (dtArticulosNormas.Rows.Count > 0)
                    {
                        ddlFuenteNormaConsulta.SelectedValue = dtArticulosNormas.Rows[0]["norm_sTipoNormaId"].ToString();
                        ddlFuenteNormaConsulta_SelectedIndexChanged(sender, e);                        
                        ddlTituloNormaConsulta.SelectedValue = dtArticulosNormas.Rows[0]["norm_sObjetoNormaId"].ToString();
                        ddlTituloNormaConsulta_SelectedIndexChanged(sender, e);                        
                        ddlArticuloNormaConsulta.SelectedValue = dtArticulosNormas.Rows[0]["norm_sNormaId"].ToString();                        
                        txtSumillaNormaConsulta.Text = dtArticulosNormas.Rows[0]["norm_vNombreArticulo"].ToString();
                        string strDescripcion = "";
                        strDescripcion = dtArticulosNormas.Rows[0]["norm_vDescripcion"].ToString();
                        txtDescNormaConsulta.Text = strDescripcion.Replace("<p>", "").Replace("</p>", "");
                        //-----------------------------------------------------------
                        
                        updBusquedaNorma.Update();

                        string StrScript = string.Empty;
                        StrScript = @"$('#MainContent_ddlFuenteNormaConsulta').prop('disabled', true);$('#MainContent_ddlTituloNormaConsulta').prop('disabled', true);
$('#MainContent_ddlArticuloNormaConsulta').prop('disabled', true);$('#MainContent_txtSumillaNormaConsulta').prop('disabled', true);$('#MainContent_txtDescNormaConsulta').prop('disabled', true);
$('#MainContent_btnAceptarNormaConsulta').prop('disabled', true);$('#MainContent_btnLimpiarNormaConsulta').prop('disabled', true);";
                        StrScript = string.Format(StrScript);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "DeshabilitarNorma", StrScript, true);
                        
                        //----------------------------------------------------------

                    }                                        

                    break;
            }

        }

        static private short ObtenerEstadoNorma(string strEstado)
        {
            DataTable dtEstadoNorma = new DataTable();
            short iEstadoNorma = 0;
            dtEstadoNorma = comun_Part1.ObtenerParametrosPorGrupoMRE("NORMA-ESTADO");

            for (int i = 0; i < dtEstadoNorma.Rows.Count; i++)
            {
                if (dtEstadoNorma.Rows[i]["descripcion"].ToString().Equals(strEstado))
                {
                    iEstadoNorma = Convert.ToInt16(dtEstadoNorma.Rows[i]["id"].ToString());
                    break;
                }
            }
            return iEstadoNorma;
        }

        static private string ObtenerIdNormas(DataTable dtActoNotarialNormas)
        {
            string strIds = "";
            for (int i = 0; i < dtActoNotarialNormas.Rows.Count; i++)
            {
                strIds += dtActoNotarialNormas.Rows[i]["anra_sNormaId"].ToString() + "|";
            }

            return strIds;
        }

        //-------------------------------------------------------------
        //Fecha: 03/05/2022
        //Autor: Miguel Márquez Beltrán
        //Motivo: Obtener de los participantes quién INICIA O RECIBE.
        //-------------------------------------------------------------
        static private string ObtenerIniciaRecibe(string strParticipante)
        {
            string strIniciaRecibe = "";

            if (HttpContext.Current.Session["vwparticipantes"] != null)
            {
                DataTable dtparticipantes = new DataTable();
                dtparticipantes = (DataTable)HttpContext.Current.Session["vwparticipantes"];

                for (int i = 0; i < dtparticipantes.Rows.Count; i++)
                {
                    if (dtparticipantes.Rows[i]["PARA_VDESCRIPCION"].ToString().Equals(strParticipante))
                    {
                        strIniciaRecibe = dtparticipantes.Rows[i]["PARA_VVALOR"].ToString().ToUpper().Trim();
                        break;
                    }
                }
            }
            return strIniciaRecibe;
        }
        static string ObtenerIniciaRecibe(short iParticipanteId)
        {
            string strIniciaRecibe = "";

            if (HttpContext.Current.Session["vwparticipantes"] != null)
            {
                DataTable dtparticipantes = new DataTable();
                dtparticipantes = (DataTable)HttpContext.Current.Session["vwparticipantes"];

                for (int i = 0; i < dtparticipantes.Rows.Count; i++)
                {
                    if (Convert.ToInt16(dtparticipantes.Rows[i]["PARA_SPARAMETROID"].ToString()) == iParticipanteId)
                    {
                        strIniciaRecibe = dtparticipantes.Rows[i]["PARA_VVALOR"].ToString().ToUpper().Trim();
                        break;
                    }
                }
            }
            return strIniciaRecibe;
        }

        static private string ObtenerNombreParticipante(short iParticipanteId)
        {
            string strParticipante = "";

            if (HttpContext.Current.Session["vwparticipantes"] != null)
            {
                DataTable dtparticipantes = new DataTable();
                dtparticipantes = (DataTable)HttpContext.Current.Session["vwparticipantes"];

                for (int i = 0; i < dtparticipantes.Rows.Count; i++)
                {
                    if (Convert.ToInt16(dtparticipantes.Rows[i]["PARA_SPARAMETROID"].ToString()) == iParticipanteId)
                    {
                        strParticipante = dtparticipantes.Rows[i]["PARA_VDESCRIPCION"].ToString().ToUpper().Trim();
                        break;
                    }
                }
            }
            return strParticipante;
        }

        private bool validarNormas()
        {
            string strArticuloid = ddlArticuloNormaConsulta.SelectedValue;
            short iNormaId = Convert.ToInt16(strArticuloid);
            bool bExiste = false;
            for (int i = 0; i < gdv_Normas.Rows.Count; i++)
            {
                if (gdv_Normas.Rows[i].Cells[1].Text == iNormaId.ToString())
                {
                    bExiste = true;
                    break;
                }
            }
            return bExiste;
        }

        //------------------------------------------------
    }
}
public class clsRespuestaEP
{
    public string CostoEP { get; set; }
    public string CostoParte2 { get; set; }
    public string CostoTestimonio { get; set; }
}
