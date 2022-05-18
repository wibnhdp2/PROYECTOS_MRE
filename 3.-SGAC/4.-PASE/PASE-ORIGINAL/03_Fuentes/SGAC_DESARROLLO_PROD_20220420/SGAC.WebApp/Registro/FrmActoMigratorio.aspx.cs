using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.WebApp.Accesorios;
using System.Data;
using SGAC.BE;
using System.Configuration;
using System.Web.Script.Serialization;
using SGAC.BE.MRE.Custom;
using SGAC.Registro.Actuacion.BL;
using System.Web.Services;
using System.Globalization;
using SGAC.Controlador;
using System.Web.Configuration;
using System.Text;
using System.Net;
using System.IO;
using SGAC.Configuracion.Sistema.BL;
using Microsoft.Security.Application;

namespace SGAC.WebApp.Registro
{
    public partial class FrmActoMigratorio : MyBasePage
    { 
        private string strVariableAccion = "Actuacion_Accion";
        private int IntEdadMinima = Convert.ToInt16(WebConfigurationManager.AppSettings["edadmaximo"]);

        protected void Page_Load(object sender, EventArgs e)
        {

            hId_TipoActuacion.Value = Convert.ToString(Request.QueryString["vClass"]);
            btnGrabarVinculacion.OnClientClick = "return abrirPopupEspera()";
            carga_controles();
            ctrlAdjunto.IsMigratorio = true;

            txtFecExpiracion.StartDate = ObtenerFechaActual(HttpContext.Current.Session);
            txtFecExpiracion.AllowFutureDate = true;
            txt_amfr_dFechaExpedicion.StartDate = ObtenerFechaActual(HttpContext.Current.Session).AddYears(-50);
            txt_amfr_dFechaExpedicion.EndDate = ObtenerFechaActual(HttpContext.Current.Session);

            hDocumento_Pasaporte.Value = ((int)Enumerador.enmDocumentoMigratorio.PASAPORTE).ToString();
            hDocumento_Salvoconducto.Value = ((int)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO).ToString();
            hDocumento_Visa.Value = ((int)Enumerador.enmDocumentoMigratorio.VISAS).ToString();
            hPasaporte_Expedido.Value = ((int)Enumerador.enmPasaporteTipo.EXPEDIDO).ToString();
            hPasaporte_Revalidado.Value = ((int)Enumerador.enmPasaporteTipo.REVALIDADO).ToString();

            hVisa_Estado_Registrado.Value = ((int)Enumerador.enmEstadoVisa.REGISTRADO).ToString();
            hSalvoconducto_Estado_Registrado.Value = ((int)Enumerador.enmEstadoSalvodonducto.REGISTRADO).ToString();

            hVisaTipo_Temporal.Value = ((int)Enumerador.enmMigratorioVisaTipoTemporal.TEMPORAL).ToString();
            hPago_Lima.Value = ((int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA).ToString();

            lblAnotacion.Text = "Anotaciones";
            switch (Comun.ToNullInt32(hId_TipoActuacion.Value))
            {
                case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                    lblAnotacion.Text = "Pagos";
                    break;
            }
            
            txt_amfr_dFechaExpiracion.AllowFutureDate = true;

            txtRREEFecha.StartDate = ObtenerFechaActual(HttpContext.Current.Session);
            txtRREEFecha.AllowFutureDate = true;
            txtDIGEMINFecha.StartDate = ObtenerFechaActual(HttpContext.Current.Session);
            txtDIGEMINFecha.AllowFutureDate = true;

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["GUID"] != null)
                {
                    HFGUID.Value = Sanitizer.GetSafeHtmlFragment(Request.QueryString["GUID"].ToString());
                    LinkButton1.PostBackUrl = "~/Registro/FrmTramite.aspx?GUID=" + HFGUID.Value;
                }
                else
                {
                    HFGUID.Value = "";
                }
                
                btn_confirmar.Enabled = false;
                Session.Remove("strBusqueda");

                hdn_CONFORMIDAD_DE_TEXTO.Value = Comun.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.AVISOS, Convert.ToInt32(Enumerador.enmNotarialAvisos.CONFORMIDAD_DE_TEXTO), "valor");

                chk_verificar.Text = hdn_CONFORMIDAD_DE_TEXTO.Value;

                hacmi_iActuacionDetalleId.Value = Convert.ToString(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);

                txtFecExpedicion.Enabled = false;
                txtFecExpedicion.Text = ObtenerFechaActual(HttpContext.Current.Session).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                
                CargarListadosDesplegables();
                PintarDatosPestaniaRegistro();
                
                ctrlToolBarRegistroActuacion_btnGrabarHandler();

                CargarDatosIniciales();
                check_3.Visible = false;

                CargarFuncionarios(Comun.ToNullInt32(Session["MIGRATORIO_OFICINACONSULTAR_ID"]), 0);

                chk_passaporte.Visible = false;
                Label21.Visible = false;
                Label23.Visible = false;
                txtCodPassaporte.Visible = false;

                Cargar_Datos_Iniciales();

                switch (Comun.ToNullInt32(hId_Estado.Value))
                {
                    case (Int16)Enumerador.enmEstadoSalvodonducto.APROBADO:
                    case (Int16)Enumerador.enmEstadoVisa.APROBADO:
                    case (Int16)Enumerador.enmMigratorioPasaporteEstados.BAJA:
                        chk_verificar.Checked = true;
                        chk_verificar_CheckedChanged(sender, EventArgs.Empty);
                        btn_solicitar.Text = "     Corregir";
                        Habilitar_Controles(true);
                        btn_solicitar.Attributes.Remove("style");

                        UpdAdjuntos.Update();
                        break;
                    case (Int16)Enumerador.enmMigratorioPasaporteEstados.IMPRESO:
                    case (Int16)Enumerador.enmEstadoSalvodonducto.IMPRESO:
                    case (Int16)Enumerador.enmEstadoVisa.CORREGIDO:
                    case (Int16)Enumerador.enmEstadoVisa.ENTREGADO:
                    case (Int16)Enumerador.enmEstadoSalvodonducto.ENTREGADO:
                    case (Int16)Enumerador.enmMigratorioPasaporteEstados.ENTREGADO:

                        Session[strVariableAccion] = (int)Enumerador.enmTipoOperacion.CONSULTA;
                        Habilitar_Controles(false);
                        btn_solicitar.Attributes.Add("style", "display:none;");
                        UpdAdjuntos.Update();
                        break;
                    case (Int16)Enumerador.enmEstadoSalvodonducto.SOLICITADO:
                    case (Int16)Enumerador.enmEstadoVisa.SOLICITADO:
                    case (Int16)Enumerador.enmMigratorioPasaporteEstados.ANULADO:
                    case (Int16)Enumerador.enmEstadoSalvodonducto.CORREGIDO:
                    case (Int16)Enumerador.enmMigratorioPasaporteEstados.CORREGIDO:
                        Habilitar_Controles(false);
                        Session[strVariableAccion] = (int)Enumerador.enmTipoOperacion.CONSULTA;
                        String scriptMover = String.Empty;
                        scriptMover = @"$(function(){{ Habilitar_Tab(2);}});";

                        scriptMover = string.Format(scriptMover);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);

                        ctrlAdjunto.BtnGrabActAdj.Enabled = false;
                        ctrlAdjunto.HabilitarControlesAdjunto(false);
                        UpdAdjuntos.Update();
                        break;
                    case (Int16)Enumerador.enmEstadoTraminte.CANCELADO:
                    case (Int16)Enumerador.enmEstadoTraminte.RECHAZADO:
                        Session[strVariableAccion] = (int)Enumerador.enmTipoOperacion.CONSULTA;
                        ctrlAdjunto.BtnGrabActAdj.Enabled = false;
                        ctrlAdjunto.HabilitarControlesAdjunto(false);
                        UpdAdjuntos.Update();
                        break;
                    case (Int16)Enumerador.enmEstadoSalvodonducto.REGISTRADO:
                    case (Int16)Enumerador.enmEstadoVisa.REGISTRADO:
                    case (Int16)Enumerador.enmMigratorioPasaporteEstados.EXPEDIDO:
                        if (Comun.ToNullInt32(hId_TipoActuacion.Value) == (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO)
                        {
                            txtNumeroPass.Enabled = false;
                        }
                        ctrlToolBarActuacion.btnGrabar.Enabled = true;
                        ctrlToolBarActuacion.btnEditar.Enabled = false;
                        chk_verificar.Checked = true;
                        chk_verificar.Enabled = false;
                        break; 
                    case (Int16)Enumerador.enmEstadoSalvodonducto.ANULADO:
                    case (Int16)Enumerador.enmMigratorioPasaporteEstados.ANULADO_2:
                        Session[strVariableAccion] = (int)Enumerador.enmTipoOperacion.CONSULTA;

                        Habilitar_Controles(false);
                        ctrlToolBarActuacion.btnGrabar.Enabled = false;
                        break;
                    case (Int16)Enumerador.enmPedidoEstado.PENDIENTE:
                        Habilitar_Controles(true);
                        ctrlToolBarActuacion.btnGrabar.Enabled = false;
                        break;
                    default:
                        Habilitar_Controles(true);
                        ctrlToolBarActuacion.btnGrabar.Enabled = false;
                        break;
                }

                if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
                {
                    ctrlToolBarActuacion.btnEditar.Enabled = false;
                    chk_verificar.Checked = true;
                    chk_verificar.Enabled = false;

                    ctrlToolBarRegistro.btnGrabar.Enabled = false;

                    switch (Comun.ToNullInt32(hId_Estado.Value))
                    {
                        case (Int16)Enumerador.enmEstadoSalvodonducto.ANULADO:
                        case (Int16)Enumerador.enmMigratorioPasaporteEstados.ANULADO_2:
                        case (Int16)Enumerador.enmMigratorioPasaporteEstados.ANULADO:
                        case (Int16)Enumerador.enmEstadoSalvodonducto.CORREGIDO:
                        case (Int16)Enumerador.enmMigratorioPasaporteEstados.CORREGIDO:
                        case (Int16)Enumerador.enmEstadoVisa.ENTREGADO:
                        case (Int16)Enumerador.enmEstadoSalvodonducto.ENTREGADO:
                        case (Int16)Enumerador.enmMigratorioPasaporteEstados.ENTREGADO:
                            btn_solicitar.Attributes.Add("style", "display:none;");
                            UpdAdjuntos.Update();
                            break;
                    }

                    ctrlAdjunto.BtnGrabActAdj.Enabled = false;
                    txtNumeroExp.Enabled = false;

                    ctrlToolBarActuacion.btnGrabar.Enabled = false;
                    updConsulta.Update();

                    if (Comun.ToNullInt32(hId_TipoActuacion.Value) != (Int32)Enumerador.enmDocumentoMigratorio.VISAS)
                    {
                        txtNumeroPass.Enabled = false;
                    }
                    else
                    {
                        ctrlToolBarActuacion.btnGrabar.Enabled = true;
                        updConsulta.Update();

                        if (Comun.ToNullInt32(hId_Estado.Value) == (Int16)Enumerador.enmEstadoVisa.CORREGIDO || 
                            Comun.ToNullInt32(hId_Estado.Value) == (Int16)Enumerador.enmEstadoVisa.ENTREGADO)
                        {
                            txtNumeroPass.Enabled = false;
                            ctrlToolBarActuacion.btnGrabar.Enabled = false;
                            updConsulta.Update();
                        }
                    }
                    txtNumSal_Lam.Enabled = false;
                    
                    txtFecExpiracion.EnabledText = false;
                    
                    Habilitar_Controles(false);

                    #region - Habilitar siempre el número de pasaporte a pedido de MRE -
                    if (Comun.ToNullInt32(hId_Estado.Value) == (Int16)Enumerador.enmMigratorioPasaporteEstados.BAJA || 
                        Comun.ToNullInt32(hId_Estado.Value) == (Int16)Enumerador.enmEstadoSalvodonducto.APROBADO)
                    {
                        if (Comun.ToNullInt32(hId_TipoActuacion.Value) == (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO)
                        {
                            txtNumeroPass.Enabled = false;
                            txtOtros.Enabled = true;
                            ctrlToolBarActuacion.btnGrabar.Enabled = true;
                            updConsulta.Update();
                        }
                    }
                    #endregion
                    ctrlAdjunto.HabilitarControlesAdjunto(false);
                    UpdAdjuntos.Update();
                }
                
            }
        }
        

        void Habilitar_Controles(bool valor)
        {
            ddlVisaTipo.Enabled = valor;
            ddlVisaSubTipo.Enabled = valor;
            ddlTitularFamiliar.Enabled = valor;
            txtTiempoPermanencia.Enabled = valor;
            ddlTipoDocumento.Enabled = valor;
            txtNumDocumento.Enabled = valor;
            txt_nroPasaporte_Anterior.Enabled = valor;
            ddlFuncionario.Enabled = valor;
            txtObservacion.Enabled = valor;
            ddlAutorizado.Enabled = valor;
            cbo_cargo_diplomatico.Enabled = valor;
            txtMotivo.Enabled = valor;
            txtInstSolicito.Enabled = valor;
            txtInstTramito.Enabled = valor;
            dllOficinaConcilleria.Enabled = valor;
            txtDocAutoriza.Enabled = valor;
            txtVisaMedioComu.Enabled = valor;
            cbo_cargo_prensa.Enabled = valor;
            txtVisaPrensaMotivo.Enabled = valor;
            txtNumSal_Lam.Enabled = valor;
            txtFecExpiracion.EnabledText = valor;
            txtRREEDoc.Enabled = valor;
            txtRREENum.Enabled = valor;
            txtRREEFecha.EnabledText = valor;
            txtDIGEMINDoc.Enabled = valor;
            txtDIGEMINNum.Enabled = valor;
            txtDIGEMINFecha.EnabledText = valor;
            ddl_amfr_sOficinaConsularId.Enabled = valor;
            ddl_amfr_sOficinaMigracionId.Enabled = valor;
            txt_amfr_dFechaExpedicion.EnabledText = valor;
            txt_amfr_dFechaExpiracion.EnabledText = valor;
            txtOtros.Enabled = valor;
            txtObservaciones.Enabled = valor;
            txtDomicilioExtra.Enabled = valor;
            txtTelefonoExtr.Enabled = valor;
            txtDomicilioPeru.Enabled = valor;
            txtTelefonoPeru.Enabled = valor;
            ddlDomicilioPeruDepa.Enabled = valor;
            ddlDomicilioPeruProv.Enabled = valor;
            ddlDomicilioPeruDist.Enabled = valor;
            ddlDomicilioExtrDepa.Enabled = valor;
            ddlDomicilioExtrProv.Enabled = valor;
            ddlDomicilioExtrDist.Enabled = valor;
            chkAcuerdoAP.Enabled = valor;
            ddlOcupacionAct.Enabled = valor;
            txtFecNacimiento.EnabledText = valor;
            ddlEstadoCivil.Enabled = valor;
            ddlOcupacionAct.Enabled = valor;
            ddlGenero.Enabled = valor;
            ddlDomicilioNacDepa.Enabled = valor;
            ddlDomicilioNacProv.Enabled = valor;
            ddlDomicilioNacDist.Enabled = valor;
            txtPadreNombres.Enabled = valor;
            txtMadreNombres.Enabled = valor;
            ddlColorOjo.Enabled = valor;
            ddlColorCabello.Enabled = valor;
            txtEstatura.Enabled = valor;

        }

        private void CargarFuncionarios(int sOfConsularId, int IFuncionarioId)
        {
            Session["Obj_Funcionarios"] = null;
            try
            {
                DataTable dt = new DataTable();
                dt = funcionario.dtFuncionario(sOfConsularId, IFuncionarioId);
                Session["Obj_Funcionarios"] = dt;

                ddlFuncionario.Items.Clear();
                ddlFuncionario.DataTextField = "vFuncionario";
                ddlFuncionario.DataValueField = "IFuncionarioId";
                ddlFuncionario.DataSource = (DataTable)Session["Obj_Funcionarios"];
                ddlFuncionario.DataBind();
                ddlFuncionario.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static string[] Cargar_Datos_Iniciales(long iPersonaId, long hacmi_iActuacionDetalleId,
            int iActuacion)
        {

            string hId_Padre = "0";
            string hId_Madre = "0";
            string hdn_resi_iResidenciaId_extranjero = "0";
            string hdn_resi_iResidenciaId_peru = "0";
            string hacmi_iActoMigratorioId = "0";
            string hiActoMigratorioFormatoId = "0";
            string acmi_sEstadoId = "0";
            string hId_Estado = "0";
            try
            {
                var existe_registro = new SGAC.Registro.Actuacion.BL.ActoMigratorioConsultaBL().Consultar_Acto_Migratorio(
                     iPersonaId,
                    hacmi_iActuacionDetalleId);

                if (existe_registro != null)
                {
                    /*Llenando los datos de persona*/
                    var obj_Persona = existe_registro.PERSONA;
                    #region - Persona -


                    #region - filiaciones-



                    if (obj_Persona.FILIACIONES.Count > 0)
                    {
                        hId_Padre = Convert.ToString(obj_Persona.FILIACIONES[0].pefi_iPersonaFilacionId);

                        if (obj_Persona.FILIACIONES.Count == 2)
                            hId_Madre = Convert.ToString(obj_Persona.FILIACIONES[1].pefi_iPersonaFilacionId);
                    }

                    #endregion

                    #endregion

                    #region - Residencias -
                    foreach (BE.RE_RESIDENCIA oRE_RESIDENCIA in obj_Persona.RESIDENCIAS)
                    {
                        if (oRE_RESIDENCIA.resi_cEstado.Equals("E"))
                            hdn_resi_iResidenciaId_extranjero = Convert.ToString(oRE_RESIDENCIA.resi_iResidenciaId);

                        if (oRE_RESIDENCIA.resi_cEstado.Equals("P"))
                            hdn_resi_iResidenciaId_peru = Convert.ToString(oRE_RESIDENCIA.resi_iResidenciaId);
                    }


                    #endregion


                    /*Llenado los datos de migratorio*/

                    var obj_Migratorio = existe_registro.ACTO;

                    hacmi_iActoMigratorioId = obj_Migratorio.acmi_iActoMigratorioId.ToString();


                    acmi_sEstadoId = Variar_Estados(obj_Migratorio.acmi_sEstadoId);
                    hId_Estado = obj_Migratorio.acmi_sEstadoId.ToString();

                    #region - cargando los datos del formato -
                    var obj_Formato = existe_registro.FORMATO;
                    if (obj_Formato != null)
                        hiActoMigratorioFormatoId = obj_Formato.amfr_iActoMigratorioFormatoId.ToString();
                    #endregion
                }
            }
            catch
            {
            }

            string s_Resultado = hacmi_iActoMigratorioId + "|" + hiActoMigratorioFormatoId +
                "|" + hdn_resi_iResidenciaId_peru + "|" + hdn_resi_iResidenciaId_extranjero +
                "|" + hId_Padre + "|" + hId_Madre + "|" + acmi_sEstadoId + "|" + hId_Estado;

            return s_Resultado.Split('|');
        }

        public static string Variar_Estados(int i_Estado)
        {
            string s_Estado = string.Empty;
            switch (i_Estado)
            {
                case (Int16)Enumerador.enmEstadoSalvodonducto.REGISTRADO:
                case (Int16)Enumerador.enmEstadoVisa.REGISTRADO:
                case (Int16)Enumerador.enmMigratorioPasaporteEstados.EXPEDIDO:
                    s_Estado = "REGISTRADO";
                    break;
                case (Int16)Enumerador.enmEstadoSalvodonducto.SOLICITADO:
                case (Int16)Enumerador.enmEstadoVisa.SOLICITADO:
                case (Int16)Enumerador.enmMigratorioPasaporteEstados.ANULADO:
                    s_Estado = "SOLICITADO";
                    break;
                case (Int16)Enumerador.enmEstadoSalvodonducto.APROBADO:
                case (Int16)Enumerador.enmEstadoVisa.APROBADO:
                case (Int16)Enumerador.enmMigratorioPasaporteEstados.BAJA:
                    s_Estado = "APROBADO";
                    break;
                case (Int16)Enumerador.enmEstadoSalvodonducto.RECHAZADO:
                case (Int16)Enumerador.enmEstadoVisa.RECHAZADO:
                case (Int16)Enumerador.enmMigratorioPasaporteEstados.RECHAZADO:
                    s_Estado = "RECHAZADO";
                    break;
                case (Int16)Enumerador.enmEstadoSalvodonducto.OBSERVADO:
                case (Int16)Enumerador.enmEstadoVisa.OBSERVADO:
                case (Int16)Enumerador.enmMigratorioPasaporteEstados.OBSERVADO:
                    s_Estado = "OBSERVADO";
                    break;
                case (Int16)Enumerador.enmEstadoTraminte.CANCELADO:
                    s_Estado = "CANCELADO";
                    break;
                case (Int16)Enumerador.enmEstadoTraminte.RECHAZADO:
                    s_Estado = "RECHAZADO";
                    break;
                case (Int16)Enumerador.enmEstadoSalvodonducto.ANULADO:
                case (Int16)Enumerador.enmMigratorioPasaporteEstados.ANULADO_2:
                    s_Estado = "ANULADO";
                    break;
                case (Int16)Enumerador.enmEstadoSalvodonducto.CORREGIDO:
                case (Int16)Enumerador.enmMigratorioPasaporteEstados.CORREGIDO:
                    s_Estado = "CORREGIDO";
                    break;
                case (Int16)Enumerador.enmEstadoSalvodonducto.IMPRESO:
                case (Int16)Enumerador.enmMigratorioPasaporteEstados.IMPRESO:
                    s_Estado = "IMPRESO";

                    break;
                case (Int16)Enumerador.enmEstadoSalvodonducto.ENTREGADO:
                case (Int16)Enumerador.enmMigratorioPasaporteEstados.ENTREGADO:
                case (Int16)Enumerador.enmEstadoVisa.ENTREGADO:
                    s_Estado = "ENTREGADO";
                    break;
            }
            return s_Estado;
        }

        void Cargar_Datos_Iniciales()
        {
            try
            {
                string strGUID = "";

                if (HFGUID.Value.Length > 0)
                {
                    strGUID = HFGUID.Value;
                }

                var existe_registro = new SGAC.Registro.Actuacion.BL.ActoMigratorioConsultaBL().Consultar_Acto_Migratorio(
                        Comun.ToNullInt64(Session["iPersonaId" + strGUID]),
                        Comun.ToNullInt64(hacmi_iActuacionDetalleId.Value));

                if (existe_registro != null)
                {
                    /*Llenando los datos de persona*/
                    var obj_Persona = existe_registro.PERSONA;
                    #region - Persona -
                    txtNumDocumento.Text = obj_Persona.Identificacion.peid_vDocumentoNumero;
                    ddlGenero.SelectedValue = obj_Persona.pers_sGeneroId.ToString();
                    if (ddlGenero.SelectedIndex > 0) ddlGenero.Enabled = false;
                    ddlEstadoCivil.SelectedValue = obj_Persona.pers_sEstadoCivilId.ToString();
                    if (ddlEstadoCivil.SelectedIndex > 0) ddlEstadoCivil.Enabled = false;
                    ddlOcupacionAct.SelectedValue = obj_Persona.pers_sOcupacionId.ToString();
                    if (ddlOcupacionAct.SelectedIndex > 0) ddlOcupacionAct.Enabled = false;
                    ddlTipoDocumento.SelectedValue = obj_Persona.Identificacion.peid_sDocumentoTipoId.ToString();
                    txtApePat.Text = obj_Persona.pers_vApellidoPaterno;
                    txtApeMat.Text = obj_Persona.pers_vApellidoMaterno;
                    txtNombres.Text = obj_Persona.pers_vNombres;
                    if (obj_Persona.pers_dNacimientoFecha != DateTime.MinValue)
                        txtFecNacimiento.Text = obj_Persona.pers_dNacimientoFecha.ToString("MMM-dd-yyyy");
                    else
                        txtFecNacimiento.Text = "";
                    if (txtFecNacimiento.Text != "") txtFecNacimiento.Enabled = false;
                    ddlColorOjo.SelectedValue = obj_Persona.pers_sColorOjosId.ToString();
                    if (ddlColorOjo.SelectedValue != "0") ddlColorOjo.Enabled = false;
                    ddlColorCabello.SelectedValue = obj_Persona.pers_sColorCabelloId.ToString();
                    if (ddlColorCabello.SelectedValue != "0") ddlColorCabello.Enabled = false;
                    hId_Persona.Value = obj_Persona.pers_iPersonaId.ToString();
                    txtEstatura.Text = obj_Persona.pers_vEstatura;
                    if (txtEstatura.Text.Trim() != "") txtEstatura.Enabled = false;
                    #region - filiaciones-

                    ddlTitularFiliacionPadre.SelectedValue = ((int)Enumerador.enmVinculo.PADRE).ToString();
                    ddlTitularFiliacionMadre.SelectedValue = ((int)Enumerador.enmVinculo.MADRE).ToString();

                    hId_Padre.Value = "0";
                    hId_Madre.Value = "0";

                    if (obj_Persona.FILIACIONES.Count > 0)
                    {
                        txtPadreNombres.Text = obj_Persona.FILIACIONES[0].pefi_vNombreFiliacion;
                        if (txtPadreNombres.Text != "") txtPadreNombres.Enabled = false;
                        hId_Padre.Value = Convert.ToString(obj_Persona.FILIACIONES[0].pefi_iPersonaFilacionId);

                        if (obj_Persona.FILIACIONES.Count == 2)
                        {
                            txtMadreNombres.Text = obj_Persona.FILIACIONES[1].pefi_vNombreFiliacion;
                            hId_Madre.Value = Convert.ToString(obj_Persona.FILIACIONES[1].pefi_iPersonaFilacionId);
                            if (txtMadreNombres.Text != "") txtMadreNombres.Enabled = false;
                        }
                    }

                    #endregion

                    if (obj_Persona.pers_cNacimientoLugar != string.Empty)
                    {

                        Comun.CargarUbigeo(Session, ddlDomicilioNacDepa, Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT, "2051", "", true, Enumerador.enmNacionalidad.PERUANA);

                        SetearUbigeo(obj_Persona.pers_cNacimientoLugar,
                            ddlDomicilioNacDepa, ddlDomicilioNacProv, ddlDomicilioNacDist);
                    }
                    else
                    {
                        Comun.CargarUbigeo(Session, ddlDomicilioNacDepa, Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT, "2051", "", true, Enumerador.enmNacionalidad.PERUANA);
                    }
                    #endregion

                    if (ddlDomicilioNacDepa.SelectedValue != "0")
                    {
                        ddlDomicilioNacDepa.Enabled = false;
                        ddlDomicilioNacProv.Enabled = false;
                        ddlDomicilioNacDist.Enabled = false;
                    }

                    #region - Residencias -
                    hdn_resi_iResidenciaId_extranjero.Value = "0";
                    hdn_resi_iResidenciaId_peru.Value = "0";
                    foreach (BE.RE_RESIDENCIA oRE_RESIDENCIA in obj_Persona.RESIDENCIAS)
                    {
                        if (oRE_RESIDENCIA.resi_cEstado.Equals("E"))
                        {
                            if (oRE_RESIDENCIA.resi_cResidenciaUbigeo != string.Empty)
                            {
                                SetearUbigeo(oRE_RESIDENCIA.resi_cResidenciaUbigeo,
                                    ddlDomicilioExtrDepa, ddlDomicilioExtrProv, ddlDomicilioExtrDist);
                            }
                            if (ddlDomicilioExtrDepa.SelectedValue != "0")
                            {
                                ddlDomicilioExtrDepa.Enabled = false;
                                ddlDomicilioExtrProv.Enabled = false;
                                ddlDomicilioExtrDist.Enabled = false;
                            }
                            hdn_resi_iResidenciaId_extranjero.Value = Convert.ToString(oRE_RESIDENCIA.resi_iResidenciaId);
                            txtDomicilioExtra.Text = oRE_RESIDENCIA.resi_vResidenciaDireccion;
                            if (txtDomicilioExtra.Text.Trim() != "") txtDomicilioExtra.Enabled = false;
                            txtTelefonoExtr.Text = oRE_RESIDENCIA.resi_vResidenciaTelefono;
                            if (txtTelefonoExtr.Text.Trim() != "") txtTelefonoExtr.Enabled = false;
                        }

                        if (oRE_RESIDENCIA.resi_cEstado.Equals("P"))
                        {
                            if (oRE_RESIDENCIA.resi_cResidenciaUbigeo != string.Empty)
                            {
                                SetearUbigeo(oRE_RESIDENCIA.resi_cResidenciaUbigeo,
                                    ddlDomicilioPeruDepa, ddlDomicilioPeruProv, ddlDomicilioPeruDist);
                            }
                            if (ddlDomicilioPeruDepa.SelectedValue != "0")
                            {
                                ddlDomicilioPeruDepa.Enabled = false;
                                ddlDomicilioPeruProv.Enabled = false;
                                ddlDomicilioPeruDist.Enabled = false;
                            }
                            hdn_resi_iResidenciaId_peru.Value = Convert.ToString(oRE_RESIDENCIA.resi_iResidenciaId);
                            txtDomicilioPeru.Text = oRE_RESIDENCIA.resi_vResidenciaDireccion;
                            if (txtDomicilioPeru.Text.Trim() != "") txtDomicilioPeru.Enabled = false;
                            txtTelefonoPeru.Text = oRE_RESIDENCIA.resi_vResidenciaTelefono;
                            if (txtTelefonoPeru.Text.Trim() != "") txtTelefonoPeru.Enabled = false;
                        }
                    }


                    #endregion


                    #region - Migratorio -

                    /*Llenado los datos de migratorio*/

                    var obj_Migratorio = existe_registro.ACTO;

                    #region - Validar Estados -
                    lblEstado.Text = Variar_Estados(obj_Migratorio.acmi_sEstadoId);
                    hId_Estado.Value = obj_Migratorio.acmi_sEstadoId.ToString();
                    #endregion

                    if (!string.IsNullOrEmpty(obj_Migratorio.acmi_vNumeroExpediente))
                    {
                        if (lblEstado.Text == "SOLICITADO")
                        {
                            txtNumeroExp.Text = obj_Migratorio.acmi_vNumeroExpediente;
                        }
                        else if (lblEstado.Text == "REGISTRADO")
                        {
                            txtNumeroExp.Text = "000";
                        }
                    }
                    else
                    {
                        txtNumeroExp.Text = "000";
                    }
                    
                    if (obj_Migratorio.acmi_dFechaExpedicion == DateTime.MinValue)
                        txtFecExpedicion.Text = ObtenerFechaActual(HttpContext.Current.Session).ToString(ConfigurationManager.AppSettings["FormatoFechas"].ToString());
                    else
                        txtFecExpedicion.Text = obj_Migratorio.acmi_dFechaExpedicion.ToString("MMM-dd-yyyy");

                    if (obj_Migratorio.acmi_dFechaExpiracion == DateTime.MinValue)
                        txtFecExpiracion.Text = "";
                    else
                        txtFecExpiracion.Text = obj_Migratorio.acmi_dFechaExpiracion.ToString("MMM-dd-yyyy");
                    txtObservacion.Text = obj_Migratorio.acmi_vObservaciones;
                    txtNumeroPass.Text = obj_Migratorio.acmi_vNumeroDocumento;
                    txtNumSal_Lam.Text = obj_Migratorio.acmi_vNumeroLamina;
                    hacmi_iActoMigratorioId.Value = obj_Migratorio.acmi_iActoMigratorioId.ToString();
                    ddlFuncionario.SelectedValue = obj_Migratorio.acmi_IFuncionarioId.ToString();

                    // T#000263
                    if (obj_Migratorio.acmi_sTipoId != (int) Enumerador.enmPasaporteTipo.REVALIDADO)
                        txtCodLamina.Text = txtNumSal_Lam.Text;

                    ddlPais.SelectedValue = obj_Migratorio.acmi_sPaisId.ToString();
                    if ((obj_Migratorio.acmi_sEstadoId == (Int16)Enumerador.enmEstadoVisa.REGISTRADO) || (obj_Migratorio.acmi_sEstadoId == (Int16)Enumerador.enmEstadoSalvodonducto.REGISTRADO) || (obj_Migratorio.acmi_sEstadoId == (Int16)Enumerador.enmMigratorioPasaporteEstados.EXPEDIDO))
                    {
                        String scriptMover = String.Empty;
                        scriptMover = @"$(function(){{ Habilitar_Tab(2);}});";

                        scriptMover = string.Format(scriptMover);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);

                        ctrlAdjunto_Click(null, EventArgs.Empty);
                    }

                    if ((obj_Migratorio.acmi_sEstadoId == (Int16)Enumerador.enmEstadoVisa.APROBADO) || (obj_Migratorio.acmi_sEstadoId == (Int16)Enumerador.enmEstadoSalvodonducto.APROBADO) || (obj_Migratorio.acmi_sEstadoId == (Int16)Enumerador.enmMigratorioPasaporteEstados.BAJA) ||
                             (obj_Migratorio.acmi_sEstadoId == (Int16)Enumerador.enmMigratorioPasaporteEstados.IMPRESO) ||
                             (obj_Migratorio.acmi_sEstadoId == (Int16)Enumerador.enmEstadoSalvodonducto.IMPRESO) ||
                             (obj_Migratorio.acmi_sEstadoId == (Int16)Enumerador.enmEstadoVisa.CORREGIDO) ||
                             (obj_Migratorio.acmi_sEstadoId == (Int16)Enumerador.enmMigratorioPasaporteEstados.ENTREGADO) ||
                             (obj_Migratorio.acmi_sEstadoId == (Int16)Enumerador.enmEstadoSalvodonducto.ENTREGADO) ||
                             (obj_Migratorio.acmi_sEstadoId == (Int16)Enumerador.enmEstadoVisa.ENTREGADO))
                    {
                        if (!txtNumeroPass.Text.Trim().Equals(""))
                        {
                            String scriptMover = String.Empty;

                           
                            scriptMover = @"$(function(){{ Habilitar_Tab(4);}});";

                            scriptMover = string.Format(scriptMover);
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);
                        }
                    }
                    #region - Validar Estados -
                    lblEstado.Text = Variar_Estados(obj_Migratorio.acmi_sEstadoId);
                    hId_Estado.Value = obj_Migratorio.acmi_sEstadoId.ToString();
                    #endregion

                    #region - Documnetos de pasaporte - 


                    DataTable dt_Documento = BindGridDocumentos(Comun.ToNullInt64(Session["iPersonaId" + strGUID]));
                    DataTable dt_Existe = null;

                    switch (Comun.ToNullInt32(hId_TipoActuacion.Value))
                    {
                        case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                            try
                            {
                                dt_Existe = (from obj_dt in dt_Documento.AsEnumerable()
                                             where obj_dt["vDesTipoDoc"].ToString() == "PASAPORTE E"
                                             select obj_dt).CopyToDataTable();
                            }
                            catch
                            {
                                dt_Existe = new DataTable();
                            }
                            break;
                        default:
                            try
                            {
                                dt_Existe = (from obj_dt in dt_Documento.AsEnumerable()
                                             where obj_dt["vDesTipoDoc"].ToString() == "PASAPORTE P"
                                             select obj_dt).CopyToDataTable();
                            }
                            catch
                            {
                                dt_Existe = new DataTable();
                            }
                            break;
                    }

                    #endregion


                    switch (Comun.ToNullInt32(hId_TipoActuacion.Value))
                    {
                        case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                            txtFecExpiracion.EnabledText = false;
                            txtOtros.Text = obj_Migratorio.acmi_vNumeroDocumentoAnterior;

                            ddlTipoDocumento.Enabled = false;
                            txtNumDocumento.Enabled = false;


                            #region - Cargando los Passaportes -
                            var s_Documentos_SA = obj_Persona.IDENTIFICACION;

                            if (s_Documentos_SA != null)
                            {
                                if (s_Documentos_SA.peid_vDocumentoNumero == null)
                                {

                                    if (dt_Existe.Rows.Count > 0)
                                        txtNumeroPass.Text = Convert.ToString(dt_Existe.Rows[dt_Existe.Rows.Count - 1]["vDocumentoNumero"]).ToUpper();
                                }
                                else
                                {
                                    if (txtNumeroPass.Text.Equals(""))
                                        txtNumeroPass.Text = s_Documentos_SA.peid_vDocumentoNumero;
                                }
                            }
                            
                            #endregion

                            chk_passaporte.Visible = true;
                            Label21.Visible = true;
                            Label23.Visible = true;
                            txtCodPassaporte.Visible = true;

                            break;
                        case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                            txtNumeroPass.Enabled = false;
                            if ((obj_Migratorio.acmi_sEstadoId == (Int16)Enumerador.enmEstadoVisa.APROBADO) || (obj_Migratorio.acmi_sEstadoId == (Int16)Enumerador.enmEstadoSalvodonducto.APROBADO) || (obj_Migratorio.acmi_sEstadoId == (Int16)Enumerador.enmMigratorioPasaporteEstados.BAJA))
                                txtNumeroPass.Enabled = true;
                            ddlVisaTipo.SelectedValue = obj_Migratorio.acmi_sTipoId.ToString();
                            ddlVisaTipo_SelectedIndexChanged(null, EventArgs.Empty);
                            ddlVisaSubTipo.SelectedValue = obj_Migratorio.acmi_sSubTipoId.ToString();
                            ddlVisaSubTipo_SelectedIndexChanged(null, EventArgs.Empty);
                            txtOtros.Text = "";
                            hiActoMigratorioFormatoId.Value = "0";
                            #region - cargando los datos del formato -
                            var obj_Formato = existe_registro.FORMATO;
                            if (obj_Formato != null)
                            {
                                ddlTitularFamiliar.SelectedValue = obj_Formato.amfr_sTitularFamiliaId.ToString();
                                txtTiempoPermanencia.Text = obj_Formato.amfr_sDiasPermanencia.ToString();
                                chkAcuerdoAP.Checked = obj_Formato.amfr_bAcuerdoProgramaFlag;
                                ddlAutorizado.SelectedValue = obj_Formato.amfr_sTipoAutorizacionId.ToString();
                                ddlAutorizado_SelectedIndexChanged(null, EventArgs.Empty);
                                txtRREEDoc.SelectedValue = obj_Formato.amfr_sTipoDocumentoRREEId.ToString();
                                ddlTipoDocumento.SelectedValue = obj_Formato.amfr_sTipoNumeroPasaporteId;
                                txtNumDocumento.Text = obj_Formato.amfr_vNumeroPasaporte;

                                txtRREENum.Text = obj_Formato.amfr_vNumDocumentoRREE;
                                if (obj_Formato.amfr_dFechaRREE != DateTime.MinValue)
                                    txtRREEFecha.set_Value = (DateTime)obj_Formato.amfr_dFechaRREE;
                                txtDIGEMINDoc.Text = obj_Formato.amfr_sTipoDocumentoDIGEMINId.ToString();
                                txtDIGEMINNum.Text = obj_Formato.amfr_vNumDocumentoDIGEMIN;
                                if (obj_Formato.amfr_dFechaDIGEMIN != DateTime.MinValue)
                                    txtDIGEMINFecha.set_Value = (DateTime)obj_Formato.amfr_dFechaDIGEMIN;
                                txt_Funcionario_Cargo.Text = obj_Formato.amfr_vCargoFuncionario.ToString();
                                cbo_cargo_diplomatico.SelectedValue = obj_Formato.amfr_sCargoDiplomaticoId.ToString();
                                txtMotivo.Text = obj_Formato.amfr_vMotivoVisaDiplomatica;
                                txtInstSolicito.Text = obj_Formato.amfr_vInstitucionSolicitaVisaDiplomatica;
                                txtInstTramito.Text = obj_Formato.amfr_vInstitucionRealizaVisaDiplomatica;
                                dllOficinaConcilleria.SelectedValue = obj_Formato.amfr_sCancilleriaSolicitaAutorizacionId.ToString();
                                txtDocAutoriza.Text = obj_Formato.amfr_vDocumentoAutoriza;
                                txtVisaMedioComu.Text = obj_Formato.amfr_vMedioComunicacionPrensa;
                                cbo_cargo_prensa.SelectedValue = obj_Formato.amfr_sCargoPrensaId.ToString();
                                txtVisaPrensaMotivo.Text = obj_Formato.amfr_vMotivoPrensa;
                                txtObservacion.Text = obj_Formato.amfr_vObservaciones;

                                hiActoMigratorioFormatoId.Value = obj_Formato.amfr_iActoMigratorioFormatoId.ToString();
                            }
                            #endregion

                            if ((obj_Migratorio.acmi_sEstadoId == (Int16)Enumerador.enmEstadoVisa.APROBADO) || (obj_Migratorio.acmi_sEstadoId == (Int16)Enumerador.enmEstadoSalvodonducto.APROBADO) || (obj_Migratorio.acmi_sEstadoId == (Int16)Enumerador.enmMigratorioPasaporteEstados.BAJA))
                            {
                                String scriptMover = String.Empty;
                                if ((obj_Migratorio.acmi_sEstadoId == (Int16)Enumerador.enmEstadoVisa.APROBADO))
                                {
                                    if (CtrlPago_Visa.iTipoPago == 0)
                                        scriptMover = @"$(function(){{ Habilitar_Tab(3);}});";
                                    else
                                        scriptMover = @"$(function(){{ Habilitar_Tab(4);}});";
                                }
                                else
                                    scriptMover = @"$(function(){{ Habilitar_Tab(4);}});";

                                scriptMover = string.Format(scriptMover);
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);
                            }

                            txtNumDocumento.Text = "";
                            #region - Cargando los Passaportes -
                            var s_Documentos_VISA = obj_Persona.IDENTIFICACION;

                            if (s_Documentos_VISA != null)
                            {
                                if (s_Documentos_VISA.peid_vDocumentoNumero == null)
                                {
                                    if (dt_Existe.Rows.Count > 0)
                                    {
                                        if (dt_Existe.Rows.Count > 1)
                                            txtNumDocumento.Text = Convert.ToString(dt_Existe.Rows[dt_Existe.Rows.Count - 2]["vDocumentoNumero"]).ToUpper();
                                        else
                                            txtNumDocumento.Text = Convert.ToString(dt_Existe.Rows[dt_Existe.Rows.Count - 1]["vDocumentoNumero"]).ToUpper();
                                    }
                                }
                                else
                                    txtNumDocumento.Text = s_Documentos_VISA.peid_vDocumentoNumero;
                            }
                            #endregion

                            
                            break;
                        case (Int32)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                            txtNumeroPass.Enabled = false;
                            
                            if (obj_Migratorio.acmi_vNumeroDocumentoAnterior == null)
                            {
                                
                                if (dt_Existe.Rows.Count > 0)
                                {
                                    if (dt_Existe.Rows.Count > 1)
                                        txt_nroPasaporte_Anterior.Text = Convert.ToString(dt_Existe.Rows[dt_Existe.Rows.Count - 2]["vDocumentoNumero"]).ToUpper();
                                    else
                                        txt_nroPasaporte_Anterior.Text = Convert.ToString(dt_Existe.Rows[dt_Existe.Rows.Count - 1]["vDocumentoNumero"]).ToUpper();
                                }
                            }
                            else
                                txt_nroPasaporte_Anterior.Text = obj_Migratorio.acmi_vNumeroDocumentoAnterior;
                            ddlTipoDocumento.Enabled = false;
                            txtNumDocumento.Enabled = false;

                            if (Comun.ToNullInt32(hdn_acmi_sTipoId.Value) == (int)Enumerador.enmPasaporteTipo.REVALIDADO)
                                txtNumeroPass.Enabled = true;

                            #region - cargando los datos del formato -
                            var obj_Formato_1 = existe_registro.FORMATO;
                            if (obj_Formato_1 != null)
                            {
                                ddlTipoDocumento.SelectedValue = obj_Formato_1.amfr_sTipoNumeroPasaporteId;
                                
                                hiActoMigratorioFormatoId.Value = obj_Formato_1.amfr_iActoMigratorioFormatoId.ToString();

                                if (Comun.ToNullInt32(hdn_acmi_sTipoId.Value) == (int)Enumerador.enmPasaporteTipo.REVALIDADO)
                                {
                                    ddl_amfr_sOficinaConsularId.SelectedValue = obj_Formato_1.amfr_sPasaporteRevalidarOficinaConsularId.ToString();
                                    if (ddl_amfr_sOficinaConsularId.SelectedIndex == 0)
                                    {
                                        ddl_amfr_sOficinaMigracionId.SelectedIndex = 0;
                                    }
                                    else
                                    {
                                        ddl_amfr_sOficinaMigracionId.SelectedValue = obj_Formato_1.amfr_sPasaporteRevalidarOficinaMigracionId.ToString();
                                        if (ddl_amfr_sOficinaMigracionId.SelectedIndex > 0)
                                        {
                                            ddl_amfr_sOficinaConsularId.SelectedIndex = 0;
                                        }
                                    }
                                    
                                    txt_amfr_dFechaExpedicion.set_Value = (DateTime)obj_Formato_1.amfr_dPasaporteRevalidarFechaExpedicion;
                                    txt_amfr_dFechaExpiracion.set_Value = (DateTime)obj_Formato_1.amfr_dPasaporteRevalidarFechaExpiracion;
                                    
                                    txtNumeroPass.Enabled = false;
                                }
                                else
                                {
                                    txtNumDocumento.Text = obj_Formato_1.amfr_vNumeroPasaporte;
                                }
                            }
                            #endregion

                            #region - Cargando los Passaportes -
                            if (Comun.ToNullInt32(hdn_acmi_sTipoId.Value) == (int)Enumerador.enmPasaporteTipo.REVALIDADO)
                            {
                                if (dt_Existe.Rows.Count > 0)
                                {
                                    txtNumeroPass.Text = Convert.ToString(dt_Existe.Rows[dt_Existe.Rows.Count - 1]["vDocumentoNumero"]).ToUpper();

                                    var dFecha1 = Convert.ToString(dt_Existe.Rows[dt_Existe.Rows.Count - 1]["dFecExpedicion"]);
                                    var dFecha2 = Convert.ToString(dt_Existe.Rows[dt_Existe.Rows.Count - 1]["dFecVcto"]);

                                    if (!string.IsNullOrEmpty(dFecha1))
                                        txt_amfr_dFechaExpedicion.set_Value = Comun.FormatearFecha(dFecha1);

                                    if (!string.IsNullOrEmpty(dFecha2))
                                        txt_amfr_dFechaExpiracion.set_Value = Comun.FormatearFecha(dFecha2);

                                }

                            }
                            else
                            {
                                if (txt_nroPasaporte_Anterior.Text.Trim().Equals(""))
                                    if (dt_Existe.Rows.Count > 0)
                                        txt_nroPasaporte_Anterior.Text = Convert.ToString(dt_Existe.Rows[dt_Existe.Rows.Count - 1]["vDocumentoNumero"]).ToUpper();

                                if (txtNumeroPass.Text.Trim().Equals(txt_nroPasaporte_Anterior.Text.Trim()))
                                    txt_nroPasaporte_Anterior.Text = "";
                            }
                            #endregion

                            chk_passaporte.Visible = true;
                            Label21.Visible = true;
                            Label23.Visible = true;
                            txtCodPassaporte.Visible = true;
                            break;
                    }
                    #endregion

                    #region - Adjuntos -

                    ctrlAdjunto.Grd_Archivos.DataSource = null;
                    ctrlAdjunto.Grd_Archivos.DataBind();

                    DataTable dtAdjuntos = new DataTable();

                    int IntTotalCount = 0;
                    int IntTotalPages = 0;

                    dtAdjuntos = new SGAC.Registro.Actuacion.BL.ActuacionAdjuntoConsultaBL().ActuacionAdjuntosObtener(
                            Comun.ToNullInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]), "1",
                            Constantes.CONST_PAGE_SIZE_ADJUNTOS, ref IntTotalCount, ref IntTotalPages);

                    if (dtAdjuntos.Rows.Count > 0)
                    {
                        ctrlAdjunto.Grd_Archivos.DataSource = dtAdjuntos;
                        ctrlAdjunto.Grd_Archivos.DataBind();
                    }

                    #endregion Adjuntos

                    BindGridActuacionesInsumoDetalle(Comun.ToNullInt64(hacmi_iActuacionDetalleId.Value));
                    #region - Mostrar Imagen -
                    ctrlAdjunto_Click(null, EventArgs.Empty);
                    #endregion
                    UpdAdjuntos.Update();
                    updVinculacion.Update();
                }

                switch (Comun.ToNullInt32(hId_TipoActuacion.Value))
                {
                    case (Int32)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                        #region Fechas de vigencia
                        if (Convert.ToInt64(hacmi_iActoMigratorioId.Value) == 0)
                        {   // Mostrar Fecha actual y la vigencia por tarifa

                            txtFecExpedicion.Text = Comun.ObtenerFechaActualTexto(Session);

                            string strTarifaDescripcion = lblTituloTarifa.Text.Trim().ToUpper();
                            if (strTarifaDescripcion.Contains("UN AÑO") || strTarifaDescripcion.Contains("1 AÑO") ||
                                strTarifaDescripcion.Contains("UN  AÑO") || strTarifaDescripcion.Contains("1  AÑO"))
                            {
                                txtFecExpiracion.Text = txtFecExpedicion.Value().AddYears((int)Enumerador.enmMigratorioPasaporteVigencia.UN_1).ToString(ConfigurationManager.AppSettings["FormatoFechas"].ToString());
                            }
                            else if (strTarifaDescripcion.Contains("DOS AÑOS") || strTarifaDescripcion.Contains("2 AÑOS") ||
                                strTarifaDescripcion.Contains("DOS  AÑOS") || strTarifaDescripcion.Contains("2  AÑOS"))
                            {
                                txtFecExpiracion.Text = txtFecExpedicion.Value().AddYears((int)Enumerador.enmMigratorioPasaporteVigencia.DOS_2).ToString(ConfigurationManager.AppSettings["FormatoFechas"].ToString());
                            }
                            else if (strTarifaDescripcion.Contains("CINCO AÑOS") || strTarifaDescripcion.Contains("5 AÑOS") ||
                                strTarifaDescripcion.Contains("CINCO  AÑOS") || strTarifaDescripcion.Contains("5  AÑOS"))
                            {
                                txtFecExpiracion.Text = txtFecExpedicion.Value().AddYears((int)Enumerador.enmMigratorioPasaporteVigencia.CINCO_5).ToString(ConfigurationManager.AppSettings["FormatoFechas"].ToString());
                            }
                        }
                        updCaberaFormato.Update();
                        #endregion
                        break;
                    case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                        #region Fechas de vigencia
                        if (Convert.ToInt64(hacmi_iActoMigratorioId.Value) == 0)
                        {   // Mostrar Fecha actual y la vigencia por tarifa
                            txtFecExpedicion.Text = Comun.ObtenerFechaActualTexto(Session);
                            txtFecExpedicion.Enabled = false;
                            string strTarifaDescripcion = lblTituloTarifa.Text.Trim().ToUpper();
                            if (strTarifaDescripcion.Contains("UN AÑO") || strTarifaDescripcion.Contains("UN  AÑO"))
                            {
                                txtFecExpiracion.Text = txtFecExpedicion.Value().AddYears((int)Enumerador.enmMigratorioPasaporteVigencia.UN_1).ToString(ConfigurationManager.AppSettings["FormatoFechas"].ToString());
                            }
                            if (strTarifaDescripcion.Contains("30 DIAS") || strTarifaDescripcion.Contains("30 DIAS"))
                            {
                                txtFecExpiracion.Text = txtFecExpedicion.Value().AddDays((int)Enumerador.enmMigratorioSalvoconductoVigencia.TREINTA).ToString(ConfigurationManager.AppSettings["FormatoFechas"].ToString());
                            }                            
                        }
                        updCaberaFormato.Update();
                        #endregion
                        break;
                    case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:

                        lblTitularTipoDocumento.Text = "Tipo Pasaporte:";
                        lblTitularNumDocumento.Text = "Número Pasaporte:";
                        
                        break;
                }

                if (txtFecExpiracion.Text.Trim().Equals(""))
                    txtFecExpiracion.EnabledText = true;
            }
            catch
            {
            }
        }

        private DataTable BindGridDocumentos(long LonPersonaId)
        {
            DataTable DtDocumentos = new DataTable();
            Proceso MiProc = new Proceso();

            Object[] miArray = new Object[1] { LonPersonaId };

            DtDocumentos = (DataTable)MiProc.Invocar(ref miArray,
                                                     "SGAC.BE.RE_PERSONAIDENTIFICACION",
                                                     Enumerador.enmAccion.CONSULTAR,
                                                     Enumerador.enmAplicacion.WEB);
            return DtDocumentos;
        }

        private void SetearUbigeo(string strUbigeo, DropDownList ddlDepartamento, DropDownList ddlProvincia, DropDownList ddlDistrito)
        {
            string strCodigo = string.Empty;
            strCodigo = strUbigeo.Substring(0, 2);

            ddlDepartamento.SelectedValue = strCodigo;
            Comun.CargarUbigeo(Session, ddlProvincia, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddlDepartamento.SelectedValue.ToString(), "", true);
            Comun.CargarUbigeo(Session, ddlDistrito, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, string.Empty, string.Empty, true);

            strCodigo = strUbigeo.Substring(2, 2);
            ddlProvincia.SelectedValue = strCodigo;
            Comun.CargarUbigeo(Session, ddlDistrito, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddlDepartamento.SelectedValue, strCodigo, true);

            strCodigo = strUbigeo.Substring(4, 2);
            ddlDistrito.SelectedValue = strCodigo;
        }

        private DataTable BindListTarifario(string IntSeccionId)
        {
            DataTable dtTarifario;
            int NroRegistros = 0;

            object[] arrParametros = { 0, txtIdTarifa.Text, 
                                       "", 
                                       ((char)Enumerador.enmEstado.ACTIVO).ToString(),
                                       1, 50, 0, 0 };

            dtTarifario = Comun.ObtenerTarifario(Session, ref arrParametros);
            Session.Remove("dtTarifarioFiltrado");

            if (dtTarifario != null)
            {
                NroRegistros = dtTarifario.Rows.Count;

                if (NroRegistros == 0)
                {
                }
                else
                {
                    Session.Add("dtTarifarioFiltrado_1", dtTarifario);
                }
            }

            return dtTarifario;
        }

        BE.MRE.SI_TARIFARIO objTarifarioBE = null;

        private void CargarObjetoTarifario(DataTable dtTarifarioFiltrado, int intIndiceSeleccionado)
        {
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
        }

        private string strVariableTarifario = "objTarifarioBE_1";

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
                objTarifaPago.fCosto = Convert.ToDouble(dr["tari_FCosto"]);
                objTarifaPago.sTipoPagoId = Convert.ToInt16(dr["pago_sPagoTipoId"]);
                objTarifaPago.dblCantidad = Convert.ToDouble(dr["Cantidad"]);
                objTarifaPago.dblMontoSolesConsulares = Convert.ToDouble(dr["FSolesConsular"]);
                objTarifaPago.dblMontoMonedaLocal = Convert.ToDouble(dr["FMonedaExtranjera"]);
                objTarifaPago.dblTotalSolesConsulares = Convert.ToDouble(dr["FTOTALSOLESCONSULARES"]);
                objTarifaPago.dblTotalMonedaLocal = Convert.ToDouble(dr["FTOTALMONEDALocal"]);
                objTarifaPago.vObservaciones = dr["acde_vNotas"].ToString();
                objTarifaPago.vMonedaLocal = Convert.ToString(dr["vMonedaLocal"]);
                if (objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA ||
                    objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA ||
                    objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA)
                {
                    objTarifaPago.vNumeroOperacion = Convert.ToString(dr["pago_vBancoNumeroOperacion"]);
                    objTarifaPago.sBancoId = Convert.ToInt16(dr["pago_sBancoId"]);
                    objTarifaPago.datFechaPago = Comun.FormatearFecha(dr["pago_dFechaOperacion"].ToString());
                    objTarifaPago.dblMontoCancelado = Convert.ToDouble(dr["FTOTALSOLESCONSULARES"]);
                }
            }
            return objTarifaPago;
        }

        private void PintarDatosPestaniaRegistro()
        {
            BE.RE_TARIFA_PAGO objTarifaPago = ObtenerDatosTarifaPago(Comun.ToNullInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]));
            Session.Add(Constantes.CONST_SESION_OBJ_TARIFA_PAGO, objTarifaPago);

            objTarifaPago = (BE.RE_TARIFA_PAGO)Session[Constantes.CONST_SESION_OBJ_TARIFA_PAGO];

            txtIdTarifa.Text = objTarifaPago.vTarifa;

            hTipoPago.Value = objTarifaPago.sTipoPagoId.ToString();
            var dtTarifarioFiltrado = BindListTarifario(objTarifaPago.tari_sSeccionId.ToString());

            DataTable dt_Filtro = null;
            try
            {
                dt_Filtro = (from dt in dtTarifarioFiltrado.AsEnumerable()
                             where Comun.ToNullInt32(dt["tari_sTarifarioId"]) == objTarifaPago.sTarifarioId
                             select dt).CopyToDataTable();

                dtTarifarioFiltrado = dt_Filtro.Copy();

                dt_Filtro.Dispose();
            }
            catch
            {

            }

            CargarObjetoTarifario(dtTarifarioFiltrado, 0);

            dtTarifarioFiltrado.Dispose();

            Session[strVariableTarifario] = objTarifarioBE;

            // Título según tarifa
            lblTituloTarifa.Text = objTarifaPago.vTarifaDescripcion;

            txtDescTarifa.Text = objTarifaPago.vTarifaDescripcion;

            txtMontoSC.Text = string.Format("{0:0.00}", objTarifaPago.fCosto);
            txtMontoML.Text = string.Format("{0:0.00}", CalculaCostoML(objTarifaPago.dblTotalSolesConsulares, Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO])));

            LblFecha.Text = objTarifaPago.datFechaRegistroActuacion.ToString(ConfigurationManager.AppSettings["FormatoFechas"]);

            txtTotalSC.Text = string.Format("{0:0.00}", 0);
            txtTotalML.Text = string.Format("{0:0.00}", 0);

            if (objTarifaPago.sTipoPagoId != 0)
            {
                txtTotalSC.Text = string.Format("{0:0.00}", objTarifaPago.dblTotalSolesConsulares);
                txtTotalML.Text = string.Format("{0:0.00}", objTarifaPago.dblTotalMonedaLocal);
            }

            ucCtrlActuacionPago.iTipoPago = objTarifaPago.sTipoPagoId;
            ucCtrlActuacionPago.vMonto = txtMontoML.Text;
            ucCtrlActuacionPago.bEnabledControl = false;
            ddlTipoPago.SelectedValue = objTarifaPago.sTipoPagoId.ToString();

            if (objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA ||
                objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA ||
                objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA)
            {
                txtNroOperacion.Text = objTarifaPago.vNumeroOperacion;
                ddlNomBanco.SelectedValue = objTarifaPago.sBancoId.ToString();
                ctrFecPago.Text = objTarifaPago.datFechaPago.ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
            }

            txtCantidad.Text = objTarifaPago.dblCantidad.ToString();
            txtObservaciones.Text = objTarifaPago.vObservaciones;
            LblDescMtoML.Text = objTarifaPago.vMonedaLocal.ToString();
            LblDescTotML.Text = objTarifaPago.vMonedaLocal.ToString();

            
            /*Datos para pagos*/

            // Tarifa de la Actuación:                
            var decMontoSC = (double)objTarifarioBE.tari_FCosto;

            // Tarifario:
            if (string.IsNullOrEmpty(txtIdTarifa.Text))
            {
                return;
            }

            CtrlPago_Visa.vTarifaLetra = txtIdTarifa.Text.Trim().ToUpper();

            var decTotalSC = Tarifario.Calculo(objTarifarioBE, 1);
            var decMontoML = CalculaCostoML(decMontoSC, Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]));
            var decTotalML = CalculaCostoML(decTotalSC, Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]));

            // Asignando valores a los controles:
            string strFormato = ConfigurationManager.AppSettings["FormatoMonto"].ToString();

            txt_montosc.Text = decMontoSC.ToString(strFormato);
            txt_montoml.Text = decMontoML.ToString(strFormato);

            txt_total_sc.Text = decTotalSC.ToString(strFormato);
            txt_total.Text = decTotalML.ToString(strFormato);


            CtrlPago_Visa.iTipoPago = objTarifaPago.sTipoPagoId;
            CtrlPago_Visa.vMonto = txt_montoml.Text;

            if (objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA ||
                objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA ||
                objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA)
            {
                CtrlPago_Visa.vNroOperacion = objTarifaPago.vNumeroOperacion;
                CtrlPago_Visa.iBank = objTarifaPago.sBancoId;
                CtrlPago_Visa.dFechaPago = objTarifaPago.datFechaPago;
            }

            LblDescMtoML.Text = objTarifaPago.vMonedaLocal.ToString();
            LblDescTotML.Text = objTarifaPago.vMonedaLocal.ToString();


            if (objTarifaPago.sTipoPagoId != 0)
                HabiltaCamposPagoActuacion(false);

            if (objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.GRATIS)
            {
                ToolBarButtonContent_Pagos.btnGrabar.Enabled = true;
            }
        }

        
        private void CargarListadosDesplegables()
        {
            try
            {

                //Cargar Pais
                Util.CargarDropDownList(ddlPais, new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL().Consultar_Pais(), "PAIS_VNOMBRE", "PAIS_SPAISID", true);

                // DATOS TITULAR
                Util.CargarDropDownList(ddlDomicilioPeruDepa, Comun.ObtenerContDepa(Session, Enumerador.enmNacionalidad.PERUANA), "ubge_vDepartamento", "ubge_cUbi01", true);
                Util.CargarParametroDropDownList(ddlDomicilioPeruProv, new DataTable(), true);
                Util.CargarParametroDropDownList(ddlDomicilioPeruDist, new DataTable(), true);

                Util.CargarDropDownList(ddlDomicilioExtrDepa, Comun.ObtenerContDepa(Session, Enumerador.enmNacionalidad.EXTRANJERA), "ubge_vDepartamento", "ubge_cUbi01", true);
                Util.CargarParametroDropDownList(ddlDomicilioExtrProv, new DataTable(), true);
                Util.CargarParametroDropDownList(ddlDomicilioExtrDist, new DataTable(), true);

                Util.CargarParametroDropDownList(ddlDomicilioNacProv, new DataTable(), true);
                Util.CargarParametroDropDownList(ddlDomicilioNacDist, new DataTable(), true);

                Util.CargarParametroDropDownList(ddlTipoPago, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACREDITACION_TIPO_COBRO), true);

                Util.CargarParametroDropDownList(ddlGenero, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_GENERO), true);
                Util.CargarParametroDropDownList(ddlEstadoCivil, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.ESTADO_CIVIL), true);
                Util.CargarParametroDropDownList(ddlOcupacionAct, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.OCUPACION), true);


                Util.CargarParametroDropDownList(ddlColorOjo, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_COLOR_OJOS), true);
                Util.CargarParametroDropDownList(ddlColorCabello, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_COLOR_CABELLO), true);

                Util.CargarParametroDropDownList(ddlVisaTipo, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_MIGRA_VISA_TIPO), true);

                // Formato
                DataTable dtTipDoc = new DataTable();
                dtTipDoc = Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.DOCUMENTO_IDENTIDAD);
                DataView dv = dtTipDoc.DefaultView;
                DataTable dtOrdenado = dv.ToTable();
                dtOrdenado.DefaultView.Sort = "Id ASC";

                Util.CargarParametroDropDownList(ddlTitularFiliacionPadre, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_TIPO_VINCULO), true);
                Util.CargarParametroDropDownList(ddlTitularFiliacionMadre, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_TIPO_VINCULO), true);

                if (Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Convert.ToInt16(Constantes.CONST_ID_CONSULADO_CARACAS))
                {
                    ddlTipoPago.Items.FindByText("PAGO ARUBA").Enabled = false;
                    ddlTipoPago.Items.FindByText("PAGO OTRAS ISLAS CARIBEÑAS").Enabled = false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ddlVisaTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["SubTipoVisa"] = null;
            try
            {

                if (Convert.ToInt32(ddlVisaTipo.SelectedValue) == 0)
                {
                    ddlVisaSubTipo.Items.Clear();
                    ddlVisaSubTipo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- SELECCIONAR -", "0"));

                    ddlTitularFamiliar.SelectedIndex = -1;
                }
                else
                {
                    if (ddlVisaTipo.SelectedItem.Text.Trim() == "RESIDENTE")
                    {
                        Session["SubTipoVisa"] = Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_MIGRA_VISA_TIPO_RESIDENTE);
                        Util.CargarParametroDropDownList(ddlVisaSubTipo, (DataTable)Session["SubTipoVisa"], true);
                    }
                    else
                    {
                        Session["SubTipoVisa"] = Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_MIGRA_VISA_TIPO_TEMPORAL);
                        Util.CargarParametroDropDownList(ddlVisaSubTipo, (DataTable)Session["SubTipoVisa"], true);
                    }
                }
            }
            catch (Exception ex)
            {
                Session["_LastException"] = ex;
                Response.Redirect("../PageError/GenericErrorPage.aspx");
            }

        }

        //67a
        private void formato_visa(bool valor)
        {
            try
            {
                tr_Pais.Visible = false;
                
                btn_formato.Text = "Formato DGC-005";
                ctrlToolBarActuacion.btnEditar.Text = "   Vista Previa";
                ctrlToolBarActuacion.btnEditar.Width = 150;
                tbOtros.Visible = !valor;

                lblTexto_5.Visible = !valor;
                txtOtros.Visible = !valor;
                lblCO_txtOtros.Visible = !valor;

                pnlVisa_1.Visible = valor;
                pnlVisa_2.Visible = valor;

                lbl_Texto12.Visible = valor;

                txtNumeroPass.MaxLength = 12;

                lblTexto_2.Visible = valor;
                txtNumSal_Lam.Visible = valor;

                lbl_Cargo_funcionario.Visible = valor;
                txt_Funcionario_Cargo.Visible = valor;
                
                lbl_Pasaporte_Anterior.Visible = !valor;
                txt_nroPasaporte_Anterior.Visible = !valor;

                lblTexto_1.Text = "Nro. Visa:";
                lblTexto_3.Text = "Fecha Inicio:";
                lblTexto_4.Text = "Fecha Fin:";
                lblTexto_2.Text = "Nro. Lámina:";

                lblTexto_2.Visible = false;
                txtNumSal_Lam.Visible = false;

                lblDatosSolicitante.Text = "2. DATOS DEL SOLICITANTE";

                divFiliacion.Attributes.Add("style", "Display:none;");

                Util.CargarParametroDropDownList(ddlVisaTipo, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_MIGRA_VISA_TIPO), true);
                Util.CargarParametroDropDownList(ddlTitularFamiliar, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_MIGRA_TITULAR_FAMILIA), true);

                Util.CargarParametroDropDownList(txtDIGEMINDoc, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_MIGRA_VISA_TIPO_DOCUMENTO_DIGEMIN), true);
                Util.CargarParametroDropDownList(txtRREEDoc, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_MIGRA_VISA_TIPO_DOCUMENTO_RREE), true);

                Util.CargarParametroDropDownList(ddlAutorizado, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_MIGRA_VISA_AUTORIZACION), true);
                ddlAutorizado_SelectedIndexChanged(null, EventArgs.Empty);

                //Cancillería
                Util.CargarParametroDropDownList(dllOficinaConcilleria, new SGAC.Configuracion.Sistema.BL.OficinaComplementariaConsultaBL().Consultar((int)SGAC.Accesorios.Enumerador.enmOficinaComplementaria.CANCILLERIA), true);
                Util.CargarParametroDropDownList(cbo_cargo_diplomatico, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_MIGRA_VISA_CARGO_DIPLOMATIVO), true);
                Util.CargarParametroDropDownList(cbo_cargo_prensa, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_MIGRA_VISA_CARGO_PRENSA), true);


                var s_TipoDocumento = Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTO_MIGRATORIO_TIPO_NRO_PASAPORTE);

                Label27.Visible = true;
                lblCO_ddlDomicilioPeruProv.Visible = true;
                lblCO_ddlDomicilioPeruDist.Visible = true;
                Label30.Visible = true;
                Label28.Visible = true;
                lblCO_ddlDomicilioExtrProv.Visible = true;
                lblCO_ddlDomicilioExtrDist.Visible = true;
                Label31.Visible = true;


                Util.CargarParametroDropDownList(ddlTipoDocumento, s_TipoDocumento, true);

                divRevalidado.Attributes.Add("style", "Display:none;");

                btn_Lamina.Text = "   Etiqueta";

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LimpiarVinculacion()
        {
            btnGrabarVinculacion.Enabled = true;
            chk_PrintAutoadhesivo.Checked = false;
            chk_PrintAutoadhesivo.Enabled = true;
            chk_PintLamina.Checked = false;
            chk_PintLamina.Enabled = true;
            txtCodAutoadhesivo.Text = string.Empty;
            Grd_ActInsDet.DataSource = null;
            Grd_ActInsDet.DataBind();
        }

        private void ctrlToolBarRegistro_btnGrabarHandler()
        {
            //----------------------------------------//
            // Autor: Jonatan Silva Cachay
            // Fecha: 02/06/2017
            // Objetivo: Verificar Enabled
            //----------------------------------------//
            if (Convert.ToInt32(hTipoPago.Value) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA) ||
            Convert.ToInt32(hTipoPago.Value) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.NO_COBRADO) ||
            Convert.ToInt32(hTipoPago.Value) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.GRATIS) ||
            Convert.ToInt32(hTipoPago.Value) == Convert.ToInt32(ddlTipoPago.Items.FindByText("PAGO ARUBA").Value) ||
            Convert.ToInt32(hTipoPago.Value) == Convert.ToInt32(ddlTipoPago.Items.FindByText("PAGO OTRAS ISLAS CARIBEÑAS").Value))
            {
                if (txtCodAutoadhesivo.Text.Length > 0 && txtCodAutoadhesivo.Enabled == false)
                {
                    string StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", "No se puede modificar el Tipo de Pago porque el autodesivo ya fue vinculado.", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }
            }
            ActualizarActuacionDetalle();
            DesabilitarTipoPago();
        }


        void ToolBarButtonContent_Pagos_btnGrabarHandler()
        {
            BE.RE_PAGO ObjPagoBE = new BE.RE_PAGO();
           
            string StrScript = string.Empty;

            string strGUID = "";

            if (HFGUID.Value.Length > 0)
            {
                strGUID = HFGUID.Value;
            }

            int intTipoPersona = Convert.ToInt32(Session["iTipoId" + strGUID]);

            #region AGREGAR

            CtrlPago_Visa.vMonto = txt_montoml.Text;

            objTarifarioBE = (BE.MRE.SI_TARIFARIO)Session[strVariableTarifario];

            #region Cargar Datos Pago Actuación
            ObjPagoBE.pago_sPagoTipoId = Convert.ToInt16(CtrlPago_Visa.iTipoPago);
            ObjPagoBE.pago_sMonedaLocalId = Convert.ToInt16(Session[Constantes.CONST_SESION_TIPO_MONEDA_ID]);
            ObjPagoBE.pago_iActuacionDetalleId = Comun.ToNullInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);
            ObjPagoBE.pago_FTipCambioBancario = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO_BANCARIO]);
            ObjPagoBE.pago_FTipCambioConsular = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]);

            ObjPagoBE.pago_sUsuarioModificacion = Comun.ToNullInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            ObjPagoBE.pago_vIPModificacion = Util.ObtenerDireccionIP();

            if (Convert.ToInt32(ObjPagoBE.pago_sPagoTipoId) == (int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA ||
                Convert.ToInt32(ObjPagoBE.pago_sPagoTipoId) == (int)Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA ||
                Convert.ToInt32(ObjPagoBE.pago_sPagoTipoId) == (int)Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA)
            {
                ObjPagoBE.pago_sMonedaLocalId = Convert.ToInt16(Session[Constantes.CONST_SESION_TIPO_MONEDA_ID]);
                if (CtrlPago_Visa.iBank > 0)
                    ObjPagoBE.pago_sBancoId = Convert.ToInt16(CtrlPago_Visa.iBank);

                ObjPagoBE.pago_vBancoNumeroOperacion = CtrlPago_Visa.vNroOperacion.Trim();
                ObjPagoBE.pago_dFechaOperacion = CtrlPago_Visa.dFechaPago;
                ObjPagoBE.pago_vNumeroVoucher = CtrlPago_Visa.vNroVoucher.Trim();

                if (CtrlPago_Visa.vMonto.Length > 0)
                {
                    if (Comun.IsNumeric(CtrlPago_Visa.vMonto))
                    {
                        ObjPagoBE.pago_FMontoMonedaLocal = Convert.ToDouble(CtrlPago_Visa.vMonto);
                        if (objTarifarioBE.tari_sCalculoTipoId == (int)Enumerador.enmTipoCalculoTarifario.PORCENTAJE)
                        { ObjPagoBE.pago_FMontoSolesConsulares = Convert.ToDouble(txt_total_sc.Text); }
                        else
                        { ObjPagoBE.pago_FMontoSolesConsulares = Convert.ToDouble(txt_montosc.Text); }
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

                if (objTarifarioBE.tari_sCalculoTipoId == (int)Enumerador.enmTipoCalculoTarifario.PORCENTAJE)
                {
                    ObjPagoBE.pago_FMontoMonedaLocal = Convert.ToDouble(txt_total.Text);
                    ObjPagoBE.pago_FMontoSolesConsulares = Convert.ToDouble(txt_total_sc.Text);
                }
                else
                {
                    ObjPagoBE.pago_FMontoMonedaLocal = Convert.ToDouble(txt_montoml.Text);
                    ObjPagoBE.pago_FMontoSolesConsulares = Convert.ToDouble(txt_montosc.Text);
                }
            }

            if (Convert.ToInt32(ObjPagoBE.pago_sPagoTipoId) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.NO_COBRADO) || 
                Convert.ToInt32(ObjPagoBE.pago_sPagoTipoId) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
            {
                ObjPagoBE.pago_FMontoMonedaLocal = 0;
                ObjPagoBE.pago_FMontoSolesConsulares = 0;
            }

            ObjPagoBE.pago_bPagadoFlag = false;
            ObjPagoBE.pago_vComentario = "";
            #endregion

            int IntRpta = new ActuacionPagoMantenimientoBL().Actualizar(ObjPagoBE,
                Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));

            #endregion



            ToolBarButtonContent_Pagos.btnGrabar.Enabled = false;
            if (IntRpta > 0)
            {
                HabiltaCamposPagoActuacion(false);
                if (Convert.ToInt32(ObjPagoBE.pago_sPagoTipoId) == ((int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA) ||
                    Convert.ToInt32(ObjPagoBE.pago_sPagoTipoId) == ((int)Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA) ||
                    Convert.ToInt32(ObjPagoBE.pago_sPagoTipoId) == ((int)Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA))
                {
                    CtrlPago_Visa.bEnabledControl = false;
                }

                String scriptMover = String.Empty;
                scriptMover = @"$(function(){{ Habilitar_Tab(1);}});";

                scriptMover = string.Format(scriptMover);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);

            }
            else
            {
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", Constantes.CONST_MENSAJE_OPERACION_FALLIDA, false, 190, 320);
                Comun.EjecutarScript(Page, StrScript);
            }
        }

        private void HabiltaCamposPagoActuacion(bool bolHabilitar = true)
        {
            CtrlPago_Visa.bEnabledControl = bolHabilitar;
            ToolBarButtonContent_Pagos.btnGrabar.Enabled = bolHabilitar;
        }
        private void DesabilitarTipoPago()
        {
            //----------------------------------------//
            // Autor: Jonatan Silva Cachay
            // Fecha: 02-06-2017
            // Objetivo: Verificar Enabled
            //----------------------------------------//

            if (txtMontoSC.Text == "0.000")
            {
                ddlTipoPago.Enabled = false;
                return;
            }

            if (Convert.ToInt32(hTipoPago.Value) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA) ||
            Convert.ToInt32(hTipoPago.Value) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.NO_COBRADO) ||
            Convert.ToInt32(hTipoPago.Value) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.GRATIS) ||
            Convert.ToInt32(hTipoPago.Value) == Convert.ToInt32(ddlTipoPago.Items.FindByText("PAGO ARUBA").Value) ||
            Convert.ToInt32(hTipoPago.Value) == Convert.ToInt32(ddlTipoPago.Items.FindByText("PAGO OTRAS ISLAS CARIBEÑAS").Value))
            {
                if (txtCodAutoadhesivo.Text.Length > 0 && txtCodAutoadhesivo.Enabled == false)
                {
                    ddlTipoPago.Enabled = false;
                }
            }
        }

        private bool ValidaRegistroNuevaTarifa()
        {
            bool bolValidado = true;

            if (txtCantidad.Text.Trim().Length == 0)
                bolValidado = false;
            if (ddlTipoPago.SelectedIndex < 1)
                bolValidado = false;
            else if (Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA) ||
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

        private void ActualizarActuacionDetalle()
        {
            string StrScript = string.Empty;
            int IntRpta = 0;
            long lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);
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
                if (ddlTipoPago.Enabled)
                {
                    intTipoPago = Convert.ToInt16(ddlTipoPago.SelectedValue);
                    hTipoPago.Value = ddlTipoPago.SelectedValue.ToString();
                    #region Actualizar Pago
                    BE.RE_PAGO ObjPagoBE = new BE.RE_PAGO();
                    ObjPagoBE.pago_sPagoTipoId = intTipoPago;
                    ObjPagoBE.pago_sMonedaLocalId = Convert.ToInt16(Session[Constantes.CONST_SESION_TIPO_MONEDA_ID]);
                    ObjPagoBE.pago_iActuacionDetalleId = Comun.ToNullInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);
                    ObjPagoBE.pago_FTipCambioBancario = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO_BANCARIO]);
                    ObjPagoBE.pago_FTipCambioConsular = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]);

                    ObjPagoBE.pago_sUsuarioModificacion = Comun.ToNullInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    ObjPagoBE.pago_vIPModificacion = Util.ObtenerDireccionIP();

                    if (Convert.ToInt32(ObjPagoBE.pago_sPagoTipoId) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA) ||
                        Convert.ToInt32(ObjPagoBE.pago_sPagoTipoId) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA) ||
                        Convert.ToInt32(ObjPagoBE.pago_sPagoTipoId) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA))
                    {
                        ObjPagoBE.pago_sMonedaLocalId = Convert.ToInt16(Session[Constantes.CONST_SESION_TIPO_MONEDA_ID]);
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
                        else
                        {
                            ObjPagoBE.pago_FMontoMonedaLocal = Convert.ToDouble(txtMontoML.Text);
                            ObjPagoBE.pago_FMontoSolesConsulares = Convert.ToDouble(txtMontoSC.Text);
                        }
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

        private void carga_controles()
        {
            try
            {
                ctrlToolBarRegistro.VisibleIButtonGrabar = true;
                ctrlToolBarRegistro.VisibleIButtonCancelar = true;
                ctrlToolBarRegistro.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonGrabarClick(ctrlToolBarRegistro_btnGrabarHandler);
                ctrlToolBarRegistro.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonCancelarClick(ctrlToolBarActuacion_btnCancelarHandler);

                ToolBarButtonContent_Pagos.VisibleIButtonCancelar = true;
                ToolBarButtonContent_Pagos.VisibleIButtonGrabar = true;
                ToolBarButtonContent_Pagos.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonGrabarClick(ToolBarButtonContent_Pagos_btnGrabarHandler);


                ctrlToolBarActuacion.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonCancelarClick(ctrlToolBarActuacion_btnCancelarHandler);
                ctrlToolBarActuacion.btnPrintHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonPrintClick(ctrlToolBarActuacion_btnPrintHandler);
                ctrlToolBarActuacion.btnEditar.OnClientClick = "return Imprimir_Formatos();";

                ToolBarButtonContent_Pagos.btnGrabar.OnClientClick = "return Validar_Registro_Pago();";
                ToolBarButtonContent_Pagos.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonCancelarClick(ctrlToolBarActuacion_btnCancelarHandler);

                ctrlAdjunto.Click += new EventHandler(ctrlAdjunto_Click);

                ctrlToolBarActuacion.VisibleIButtonNuevo = false;
                ctrlToolBarActuacion.VisibleIButtonEditar = true;
                ctrlToolBarActuacion.VisibleIButtonGrabar = true;
                ctrlToolBarActuacion.VisibleIButtonCancelar = true;
                ctrlToolBarActuacion.VisibleIButtonBuscar = false;
                ctrlToolBarActuacion.VisibleIButtonPrint = false;

                ctrlToolBarActuacion.VisibleIButtonConfiguration = false;
                ctrlToolBarActuacion.VisibleIButtonSalir = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region ctrlToolBarActuacion_evento


        private void CargarDatosIniciales()
        {
            #region Datos Personales

            lblDestino.Text = string.Empty;
            string strEtiquetaSolicitante = string.Empty;

            string strGUID = "";
            if (HFGUID.Value.Length > 0)
            {
                strGUID = HFGUID.Value;
            }

            if (Session["ApePat" + strGUID] != null)
            {
                strEtiquetaSolicitante += Session["ApePat" + strGUID].ToString() + " ";
            }

            if (Session["ApeMat" + strGUID] != null)
            {
                strEtiquetaSolicitante += Session["ApeMat" + strGUID].ToString() + " ";
            }
            if (Session["ApeCasada" + strGUID] != null)
            {
                if (Session["ApeCasada" + strGUID].ToString() != "&nbsp;")
                {
                    strEtiquetaSolicitante += Session["ApeCasada" + strGUID].ToString() + " ";
                }
            }
            if (Session["Nombres" + strGUID] != null)
            {
                if (Session["Nombres" + strGUID].ToString().Trim() != string.Empty)
                {
                    strEtiquetaSolicitante += ", " + Session["Nombres" + strGUID].ToString() + " ";
                }
            }
            if (Session["DescTipDoc" + strGUID] != null)
            {
                strEtiquetaSolicitante += "- " + Session["DescTipDoc" + strGUID].ToString() + ": ";
            }

            if (Session["NroDoc" + strGUID] != null)
            {
                strEtiquetaSolicitante += Session["NroDoc" + strGUID].ToString();
            }

            #endregion Datos Personales


            Session["COD_AUTOADHESIVO" + HFGUID.Value] = string.Empty;

            lblDestino.Text = strEtiquetaSolicitante;

            long lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);


            #region Oficina Registral

            string strUbigeo = string.Empty;
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                strUbigeo = Constantes.CONST_OFICINACONSULAR_LIMA_UBIGEO;
            }
            else
            {
                strUbigeo = Comun.ObtenerDatoOficinaConsular(Session, "ofco_cUbigeoCodigo").ToString();
            }


            #endregion Oficina Registral
        }

        private void ctrlToolBarActuacion_btnCancelarHandler()
        {
            if (HFGUID.Value.Length > 0)
            {
                Response.Redirect("~/Registro/FrmTramite.aspx?GUID=" + HFGUID.Value);
            }
            else
            {
                Response.Redirect("~/Registro/FrmTramite.aspx");
            }
        }

        private void ctrlToolBarActuacion_btnPrintHandler()
        {
            //...
        }

        protected void ctrlPagActuacionInsumoDetalle_Click(object sender, EventArgs e)
        {
            BindGridActuacionesInsumoDetalle(Convert.ToInt64(Session["ActuacionDetalleId" + HFGUID.Value]));
            updVinculacion.Update();
        }
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
            dtActuacionInsumoDetalle = objActuacionMantenimientoBL.Obtener_ActuacionInsumoDetalle(iActuacionDetalleId, PaginaActual, intPaginaCantidad, ref IntTotalCount, ref  IntTotalPages);

            if (dtActuacionInsumoDetalle.Rows.Count > 0)
            {
                string strFlag = string.Empty;
                btn_confirmar.Enabled = true;

                String scriptMover = String.Empty;
                scriptMover = @"$(function(){{ Habilitar_Tab(4);}});";

                scriptMover = string.Format(scriptMover);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);


                foreach (System.Data.DataRow row in dtActuacionInsumoDetalle.Rows)
                {
                    strFlag = row["aide_bFlagImpresion"].ToString();
                    switch (Comun.ToNullInt32(row["TipoInsumo"]))
                    {
                        case (Int32)Enumerador.enmInsumoTipo.AUTOADHESIVO:
                            txtCodAutoadhesivo.Text = row["insu_vCodigoUnicoFabrica"].ToString();
                            Session[Constantes.CONST_ACTUACION_INSUMO_DETALLE_ID + HFGUID.Value] = row["aide_iActuacionInsumoDetalleId"].ToString();
                            txtCodAutoadhesivo.Enabled = false;
                            btnVistaPrev.Enabled = true;
                            chk_PrintAutoadhesivo.Enabled = false;
                            ctrlAdjunto.SetCodigoVinculacion(txtCodAutoadhesivo.Text);




                            chk_PrintAutoadhesivo.Checked = false;
                            if (strFlag.Equals("SI"))
                            {
                                chk_PrintAutoadhesivo.Checked = true;
                                btnVistaPrev.Enabled = false;
                                hdn_ImpresionCorrecta.Value = "1";
                            }
                            
                            
                            break;
                        case (Int32)Enumerador.enmInsumoTipo.LAMINA:
                        case (Int32)Enumerador.enmInsumoTipo.LAMINA_REPOSICION:
                        case (Int32)Enumerador.enmInsumoTipo.LAMINA_RENOVACION:
                        case (Int32)Enumerador.enmInsumoTipo.ETIQUETA:
                        case (Int32)Enumerador.enmInsumoTipo.LAMINA_SALVOCONDUCTO:
                        case (Int32)Enumerador.enmInsumoTipo.LAMINA_SALVOCONDUCTO_REPOSICION:
                            
                            Session[Constantes.CONST_ACTUACION_INSUMO_DETALLE_ID_LAMINA] = row["aide_iActuacionInsumoDetalleId"].ToString();

                            txtCodLamina.Text = row["insu_vCodigoUnicoFabrica"].ToString();
                            txtCodLamina.Enabled = false;
                            chk_PintLamina.Checked = true;
                            chk_PintLamina.Enabled = false;
                            ctrlAdjunto.SetCodigoVinculacion(txtCodLamina.Text);
                            chk_PintLamina.Checked = false;
                            if (strFlag.Equals("SI"))
                            {
                                chk_PintLamina.Checked = true;
                                hdn_ImpresionCorrecta.Value = "1";
                            }

                            
                            break;
                        case (Int32)Enumerador.enmInsumoTipo.PASAPORTE:
                        case (Int32)Enumerador.enmInsumoTipo.SALVOCONDUCTO:
                            txtCodPassaporte.Text = row["insu_vCodigoUnicoFabrica"].ToString();
                            Session[Constantes.CONST_ACTUACION_INSUMO_DETALLE_ID_PAS] = row["aide_iActuacionInsumoDetalleId"].ToString();

                            chk_passaporte.Checked = false;
                            if (txtCodPassaporte.Text.Trim() != string.Empty)
                            {
                                txtCodPassaporte.Enabled = false;
                                chk_passaporte.Checked = true;
                                chk_passaporte.Enabled = false;
                                btn_formato.Enabled = true;

                               



                            }
                            ctrlAdjunto.SetCodigoVinculacion(txtCodPassaporte.Text);
                            break;
                    }
                }

                if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
                {
                   
                    
                    if (txtCodAutoadhesivo.Text.Trim() != string.Empty)
                    {
                        txtCodAutoadhesivo.Enabled = false;
                        //btnVistaPrev.Enabled = false;
                        //chk_PrintAutoadhesivo.Checked = true;
                        //chk_PrintAutoadhesivo.Enabled = false;
                    }

                    if (txtCodLamina.Text.Trim() != string.Empty)
                    {
                        txtCodLamina.Enabled = false;
                        //chk_PintLamina.Checked = true;
                        chk_PintLamina.Enabled = false;
                        btn_Lamina.Enabled = true;
                    }

                    if (txtCodPassaporte.Text.Trim() != string.Empty)
                    {
                        txtCodPassaporte.Enabled = false;
                        //chk_passaporte.Checked = true;
                        chk_passaporte.Enabled = false;
                        btn_formato.Enabled = true;
                    }

                    if (txtCodAutoadhesivo.Text.Trim() != string.Empty &&
                        txtCodLamina.Text.Trim() != string.Empty &&
                        txtCodPassaporte.Text.Trim() != string.Empty)
                    {
                        btnGrabarVinculacion.Enabled = false;
                    }
                    updConsulta.Update();
                    updVinculacion.Update();
                }
                else
                {
                    switch (Comun.ToNullInt32(hId_TipoActuacion.Value))
                    {
                        case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                        case (Int32)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                            if (dtActuacionInsumoDetalle.Rows.Count == 3)
                            {
                                btnGrabarVinculacion.Enabled = false;
                                updConsulta.Update();
                            }
                            break;
                        case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                            if (dtActuacionInsumoDetalle.Rows.Count == 2)
                            {
                                btnGrabarVinculacion.Enabled = false;
                                updConsulta.Update();
                            }

                            Habilitar_Controles(false);
                            btn_solicitar.Attributes.Add("style", "display:none;");
                            UpdAdjuntos.Update();
                            break;
                    }
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
                if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
                {
                    txtCodAutoadhesivo.Enabled = false;
                    //btnVistaPrev.Enabled = false;
                    chk_PrintAutoadhesivo.Checked = true;
                    chk_PrintAutoadhesivo.Enabled = false;

                    txtCodLamina.Enabled = false;
                    chk_PintLamina.Checked = true;
                    chk_PintLamina.Enabled = false;

                    txtCodPassaporte.Enabled = false;
                    chk_passaporte.Checked = true;
                    chk_passaporte.Enabled = false;


                    btnGrabarVinculacion.Enabled = false;
                    updConsulta.Update();
                }
            }
        }

        protected void ctrlAdjunto_Click(object sender, EventArgs e)
        {
            String scriptMover = String.Empty;
            scriptMover = @"$(function(){{ Habilitar_Tab(2);}});";

            if (Comun.ToNullInt32(hId_TipoActuacion.Value) != (Int32)Enumerador.enmDocumentoMigratorio.VISAS)
            {
                if (lblEstado.Text.ToString().ToUpper().Equals("APROBADO"))
                {
                    scriptMover = @"$(function(){{ Habilitar_Tab(4);}});";
                    scriptMover = string.Format(scriptMover);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab_4", scriptMover, true);
                }
            }

            if (sender != null)
            {
                scriptMover = String.Empty;
                scriptMover = @"$(function(){{ Habilitar_Tab(2);}});";

                scriptMover = string.Format(scriptMover);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);
            }
            try
            {
                /*Cargar las imagenes en la previsualización*/
                var load_Imagen = Load_Imagenes();
                int i_Foto = 0; int i_Firma = 0; int i_Huella = 0;

                #region - Verificando si tiene las 3 fotos registradas -

                if (load_Imagen.Rows.Count > 0)
                {
                    foreach (DataRow row in load_Imagen.Rows)
                    {
                        switch (Comun.ToNullInt32(row["sAdjuntoTipoId"]))
                        {
                            case (Int32)Enumerador.enmTipoAdjunto.FOTO:
                                i_Foto = 1;
                                break;
                            case (Int32)Enumerador.enmTipoAdjunto.FIRMA:
                                i_Firma = 1;
                                break;
                            case (Int32)Enumerador.enmTipoAdjunto.HUELLA:
                                i_Huella = 1;
                                break;
                        }
                    }

                    btn_solicitar.Text = "     Solicitar";

                    switch (Comun.ToNullInt32(hId_TipoActuacion.Value))
                    {
                        case (Int32)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                        case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                            if (i_Foto == 1 || i_Firma == 1 || i_Huella == 1)
                            {
                                btn_solicitar.Attributes.Remove("style");
                                if (Comun.ToNullInt32(hId_Estado.Value) == (Int32)Enumerador.enmMigratorioPasaporteEstados.ANULADO ||
                                    Comun.ToNullInt32(hId_Estado.Value) == (Int32)Enumerador.enmEstadoSalvodonducto.SOLICITADO ||
                                    Comun.ToNullInt32(hId_Estado.Value) == (Int32)Enumerador.enmEstadoSalvodonducto.APROBADO ||
                                    Comun.ToNullInt32(hId_Estado.Value) == (Int32)Enumerador.enmMigratorioPasaporteEstados.BAJA ||
                                    Comun.ToNullInt32(hId_Estado.Value) == (Int32)Enumerador.enmMigratorioPasaporteEstados.IMPRESO ||
                                    Comun.ToNullInt32(hId_Estado.Value) == (Int32)Enumerador.enmMigratorioPasaporteEstados.ENTREGADO ||
                                    Comun.ToNullInt32(hId_Estado.Value) == (Int32)Enumerador.enmEstadoSalvodonducto.IMPRESO ||
                                    Comun.ToNullInt32(hId_Estado.Value) == (Int32)Enumerador.enmEstadoSalvodonducto.ENTREGADO)
                                {
                                    btn_solicitar.Attributes.Add("style", "display:none;");
                                }
                                
                            }
                            else
                                btn_solicitar.Attributes.Add("style", "display:none;");

                            if ((Comun.ToNullInt32(hId_Estado.Value) == (int)Enumerador.enmEstadoSalvodonducto.OBSERVADO) ||
                                (Comun.ToNullInt32(hId_Estado.Value) == (int)Enumerador.enmMigratorioPasaporteEstados.OBSERVADO))
                            {
                                btn_solicitar.Text = "     Corregir";
                            }
                         
                                
                            break;
                        case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                            if (i_Foto == 1 || i_Firma == 1 || i_Huella == 1)
                                btn_solicitar.Attributes.Remove("style");
                            else
                                btn_solicitar.Attributes.Add("style", "display:none;");

                            if ((Comun.ToNullInt32(hId_Estado.Value) == (int)Enumerador.enmEstadoVisa.ENTREGADO))
                                btn_solicitar.Attributes.Add("style", "display:none;");
                            break;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Session["_LastException"] = ex;
                Response.Redirect("../PageError/GenericErrorPage.aspx");

            }
        }

        DataTable Load_Imagenes()
        {
            int IntTotalCount = 0;
            int IntTotalPages = 0;

            var load_Imagen = new SGAC.Registro.Actuacion.BL.ActuacionAdjuntoConsultaBL().ActuacionAdjuntosObtener(
                Comun.ToNullInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]), "1",
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

            string i_Imagen = "imagen-no-disponible.jpg";

            imgFirma.ImageUrl = string.Format("~/LoadImagen.ashx?vClass={0}", i_Imagen);
            imgFoto.ImageUrl = string.Format("~/LoadImagen.ashx?vClass={0}", i_Imagen);
            imgHuella.ImageUrl = string.Format("~/LoadImagen.ashx?vClass={0}", i_Imagen);

            String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];

            foreach (DataRow row in Ordenado.Rows)
            {
                switch (Comun.ToNullInt32(row["sAdjuntoTipoId"]))
                {
                    case (Int32)Enumerador.enmTipoAdjunto.FOTO:
                        imgFoto.ImageUrl = string.Format("~/LoadImagen.ashx?vClass={0}", Convert.ToString(row["vNombreArchivo"]));
                        break;
                    case (Int32)Enumerador.enmTipoAdjunto.FIRMA:
                        imgFirma.ImageUrl = string.Format("~/LoadImagen.ashx?vClass={0}", Convert.ToString(row["vNombreArchivo"]));
                        break;
                    case (Int32)Enumerador.enmTipoAdjunto.HUELLA:
                        imgHuella.ImageUrl = string.Format("~/LoadImagen.ashx?vClass={0}", Convert.ToString(row["vNombreArchivo"]));
                        break;
                }
            }

            return load_Imagen;
        }
        #endregion ctrlToolBarActuacion_evento
        private void formato_Pasaporte(bool valor)
        {
            try
            {
                tr_Pais.Visible = false;

                pnlVisa_1.Visible = !valor;
                pnlVisa_2.Visible = !valor;
                pnlVisa_3.Visible = !valor;
                tbOtros.Visible = valor;
                lbl_Texto12.Visible = !valor;
                lbl_Cargo_funcionario.Visible = !valor;
                txt_Funcionario_Cargo.Visible = !valor;
                txtNumeroPass.MaxLength = 7;
                divRevalidado.Attributes.Add("style", "Display:none;");

                lblTexto_5.Visible = !valor;
                txtOtros.Visible = !valor;
                lblCO_txtOtros.Visible = !valor;
                lbl_Pasaporte_Anterior.Visible = valor;
                txt_nroPasaporte_Anterior.Visible = valor;

                lblTexto_2.Visible = valor;
                txtNumSal_Lam.Visible = valor;

                lblTexto_1.Text = "Nro. Pasaporte:";
                lblTexto_2.Text = "Nro. Lámina:";
                lblTexto_3.Text = "Fecha Expedición";
                lblTexto_4.Text = "Fecha Expiración";

                lblTexto_5.Text = "Nro. Pasaporte Anterior";

                txtFecExpedicion.Enabled = !valor;
                txtFecExpiracion.EnabledText = !valor;

                CargarControlesPorTarifa();

                DataTable s_Documento = Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.DOCUMENTO_IDENTIDAD);

                Label27.Visible = false;
                lblCO_ddlDomicilioPeruProv.Visible = false;
                lblCO_ddlDomicilioPeruDist.Visible = false;
                Label30.Visible = false;
                Label28.Visible = false;
                lblCO_ddlDomicilioExtrProv.Visible = false;
                lblCO_ddlDomicilioExtrDist.Visible = false;
                Label31.Visible = false;


                Util.CargarDropDownList(ddlTipoDocumento, Comun.ObtenerDocumentoIdentidad(Session), "Valor", "Id", true);

                ddlTipoDocumento.SelectedValue = ((int) Enumerador.enmTipoDocumento.DNI).ToString();

                this.ddlTipoDocumento.Items.Add(new System.Web.UI.WebControls.ListItem(Convert.ToString(Constantes.CONST_EXCEPCION_CUI), Convert.ToString(Constantes.CONST_EXCEPCION_CUI_ID)));

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Formulario para tarifas: Pasaporte Expedido y Revalidado
        /// </summary>
        private void CargarControlesPorTarifa()
        {
            string strTarifaDescripcion = txtDescTarifa.Text.Trim();
            if (strTarifaDescripcion.Contains("RENOVA") || strTarifaDescripcion.Contains("REVALIDA"))
            {   // revalidación, renovación
                hdn_acmi_sTipoId.Value = Convert.ToString((int)Enumerador.enmPasaporteTipo.REVALIDADO);

                lbl_Pasaporte_Anterior.Visible = false;
                txt_nroPasaporte_Anterior.Visible = false;
                divRevalidado.Attributes.Remove("style");
                lblTexto_3.Text = "Fecha Revalidación:";
                btn_formato.Text = "Formato DGC-002";
                ctrlToolBarActuacion.btnEditar.Text = "   Vista Previa";
                ctrlToolBarActuacion.btnEditar.Width = 150;
                // Listados Desplegables
                Util.CargarParametroDropDownList(ddl_amfr_sOficinaMigracionId, new SGAC.Configuracion.Sistema.BL.OficinaComplementariaConsultaBL().Consultar((int)SGAC.Accesorios.Enumerador.enmOficinaComplementaria.MIGRACIONES), true);

                OficinaConsularConsultasBL objBL = new OficinaConsularConsultasBL();
                int intTotalCount = 0;
                int intTotalPages = 0;
                DataTable dt = objBL.Consultar(0, "", "", "", "1", 1000, ref intTotalCount, ref intTotalPages, "A");
                dt = dt.AsEnumerable().OrderBy(x => x["ofco_vNombreMostrar"]).CopyToDataTable();

                Util.CargarDropDownList(ddl_amfr_sOficinaConsularId, dt, "ofco_vNombreMostrar", "ofco_sOficinaConsularId", true);
            }
            else
            {   // expedido, expedición
                hdn_acmi_sTipoId.Value = Convert.ToString((int)Enumerador.enmPasaporteTipo.EXPEDIDO);

                lbl_Pasaporte_Anterior.Visible = true;
                txt_nroPasaporte_Anterior.Visible = true;
                btn_formato.Text = "Formato DGC-001";
                ctrlToolBarActuacion.btnEditar.Text = "   Vista Previa";
                ctrlToolBarActuacion.btnEditar.Width = 150;
                divRevalidado.Attributes.Add("style", "Display:none;");
                lblTexto_3.Text = "Fecha Expedición:";
            }

        }

        void ctrlToolBarRegistroActuacion_btnGrabarHandler()
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
            //----------------------------------------//
            // Autor: Jonatan Silva Cachay
            // Fecha: 02-06-2017
            // Objetivo: Verificar Enabled
            //----------------------------------------//
            if (Convert.ToInt32(hTipoPago.Value) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA) ||
            Convert.ToInt32(hTipoPago.Value) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.NO_COBRADO) ||
            Convert.ToInt32(hTipoPago.Value) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.GRATIS) ||
            Convert.ToInt32(hTipoPago.Value) == Convert.ToInt32(ddlTipoPago.Items.FindByText("PAGO ARUBA").Value) ||
            Convert.ToInt32(hTipoPago.Value) == Convert.ToInt32(ddlTipoPago.Items.FindByText("PAGO OTRAS ISLAS CARIBEÑAS").Value))
            {
                if (txtCodAutoadhesivo.Text.Length > 0 && txtCodAutoadhesivo.Enabled == false)
                {
                    string StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", "No se puede modificar el Tipo de Pago porque el autodesivo ya fue vinculado.", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }
            }

           txtNumeroExp.Text = new SGAC.Registro.Actuacion.BL.ActoMigratorioConsultaBL().Consultar_Correlativo(Comun.ToNullInt32(hId_TipoActuacion.Value)).ToString();
            //if (string.IsNullOrEmpty(txtNumeroExp.Text))
            //{
            //    txtNumeroExp.Text = new SGAC.Registro.Actuacion.BL.ActoMigratorioConsultaBL().Consultar_Correlativo_Tipo_Doc_Migratorio(Comun.ToNullInt32(hId_TipoActuacion.Value)).ToString();
            //}
            //else
            //{ 
                
            //}

            if (Comun.ToNullInt64(txtNumeroExp.Text) <= 0)
                txtNumeroExp.Enabled = true;
            else
                txtNumeroExp.Enabled = false;

            switch (Comun.ToNullInt32(hId_TipoActuacion.Value))
            {
                case (Int32)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                    formato_Pasaporte(true);
                    break;
                case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                    formato_ExpedirSalvoconducto(true);
                    break;
                case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                    formato_visa(true);
                    break;
            }
            DesabilitarTipoPago();

            string strScript = string.Empty;
            strScript += Util.HabilitarTab(1);
            Comun.EjecutarScript(Page, strScript);
            updConsulta.Update();
        }

        

        protected void ddlDomicilioPeruProv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDomicilioPeruProv.SelectedIndex > 0)
                {
                    Util.CargarDropDownList(ddlDomicilioPeruDist,
                        Comun.ObtenerRegiDist(Session,
                            ddlDomicilioPeruDepa.SelectedValue, ddlDomicilioPeruProv.SelectedValue),
                            "ubge_vDistrito", "ubge_cUbi03", true);

                    ScriptManager.GetCurrent(Page).SetFocus(ddlDomicilioPeruProv.ClientID);
                }
                else
                {
                    Util.CargarParametroDropDownList(ddlDomicilioPeruDist, new DataTable(), true);
                }
            }
            catch (Exception ex)
            {
                Session["_LastException"] = ex;
                Response.Redirect("../PageError/GenericErrorPage.aspx");
            }

        }

        protected void ddlDomicilioExtrDepa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDomicilioExtrDepa.SelectedIndex > 0)
                {
                    Util.CargarDropDownList(ddlDomicilioExtrProv,
                        Comun.ObtenerPaisProv(Session, ddlDomicilioExtrDepa.SelectedValue),
                            "ubge_vProvincia", "ubge_cUbi02", true);

                    Util.CargarParametroDropDownList(ddlDomicilioExtrDist, new DataTable(), true);

                    ScriptManager.GetCurrent(Page).SetFocus(ddlDomicilioExtrDepa.ClientID);

                    lblCO_ddlDomicilioExtrProv.Visible = true;
                    lblCO_ddlDomicilioExtrDist.Visible = true;
                }
                else
                {
                    Util.CargarParametroDropDownList(ddlDomicilioExtrProv, new DataTable(), true);
                    Util.CargarParametroDropDownList(ddlDomicilioExtrDist, new DataTable(), true);

                    lblCO_ddlDomicilioExtrProv.Visible = false;
                    lblCO_ddlDomicilioExtrDist.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Session["_LastException"] = ex;
                Response.Redirect("../PageError/GenericErrorPage.aspx");
            }
        }

        protected void ddlDomicilioExtrProv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDomicilioExtrProv.SelectedIndex > 0)
                {
                    Util.CargarDropDownList(ddlDomicilioExtrDist,
                        Comun.ObtenerRegiDist(Session, ddlDomicilioExtrDepa.SelectedValue, ddlDomicilioExtrProv.SelectedValue),
                            "ubge_vDistrito", "ubge_cUbi03", true);

                    ScriptManager.GetCurrent(Page).SetFocus(ddlDomicilioExtrProv.ClientID);
                }
                else
                {
                    Util.CargarParametroDropDownList(ddlDomicilioExtrDist, new DataTable(), true);
                }

            }
            catch (Exception ex)
            {
                Session["_LastException"] = ex;
                Response.Redirect("../PageError/GenericErrorPage.aspx");
            }

        }

        //63a
        private void formato_ExpedirSalvoconducto(bool valor)
        {
            try
            {
                tr_Pais.Visible = true;
                btn_formato.Text = "Formato DGC-006";
                ctrlToolBarActuacion.btnEditar.Text = "   Vista Previa";
                ctrlToolBarActuacion.btnEditar.Width = 150;
                pnlVisa_1.Visible = !valor;
                pnlVisa_2.Visible = !valor;
                pnlVisa_3.Visible = !valor;
                tbOtros.Visible = valor;

                txtNumeroPass.MaxLength = 12;

                lblTexto_5.Visible = valor;
                txtOtros.Visible = valor;
                lblCO_txtOtros.Visible = !valor;

                lblTexto_2.Visible = valor;
                txtNumSal_Lam.Visible = valor;
                lbl_Texto12.Visible = !valor;
                lbl_Cargo_funcionario.Visible = !valor;
                txt_Funcionario_Cargo.Visible = !valor;
                
                lbl_Pasaporte_Anterior.Visible = !valor;
                txt_nroPasaporte_Anterior.Visible = !valor;

                txtNumSal_Lam.Text = "";
                lblTexto_1.Text = "Nro. Pasaporte:";
                lblTexto_2.Text = "Nro. Lámina:";
                lblTexto_2.Visible = false;
                txtNumSal_Lam.Visible = false;
                lblTexto_3.Text = "Fecha Expedición:";
                lblTexto_4.Text = "Fecha Expiración:";
                lblTexto_5.Text = "Nro. Salvoconducto:";
                lblCO_txtOtros.Visible = false;
                Util.CargarDropDownList(ddlTipoDocumento, Comun.ObtenerDocumentoIdentidad(Session), "Valor", "Id", true);
                this.ddlTipoDocumento.Items.Add(new System.Web.UI.WebControls.ListItem(Convert.ToString(Constantes.CONST_EXCEPCION_CUI), Convert.ToString(Constantes.CONST_EXCEPCION_CUI_ID)));
                ddlTipoDocumento.SelectedValue = "1";

                divRevalidado.Attributes.Add("style", "Display:none;");

                Label27.Visible = false;
                lblCO_ddlDomicilioPeruProv.Visible = false;
                lblCO_ddlDomicilioPeruDist.Visible = false;
                Label30.Visible = false;
                Label28.Visible = false;
                lblCO_ddlDomicilioExtrProv.Visible = false;
                lblCO_ddlDomicilioExtrDist.Visible = false;
                Label31.Visible = false;


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
                EnPersona objEn = new EnPersona();

                object[] arrParametros = { objEn };

                return arrParametros;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void btnPersona_Click(object sender, EventArgs e)
        {
            try
            {
                object[] arrParametros = ObtenerFiltro();
                EnPersona objEn = new EnPersona();

                objEn = SGAC.WebApp.Accesorios.Persona.oPersona(arrParametros);

            }
            catch (Exception ex)
            {
                Session["_LastException"] = ex;
                Response.Redirect("../PageError/GenericErrorPage.aspx");
            }
        }

       

        private static DateTime Validar_Fecha(string s_Fecha)
        {
            DateTime d_fecha;
            try
            {
                d_fecha = Comun.FormatearFecha(s_Fecha);
            }
            catch
            {
                d_fecha = DateTime.MinValue;
            }
            return d_fecha;
        }

        [WebMethod]
        public static string insert_registro(string actonomigratorio, string strGUID)
        {
            string s_Resultado = string.Empty;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var jsonObject = serializer.Deserialize<dynamic>(actonomigratorio);

            var jsonPersona = serializer.Deserialize<dynamic>(jsonObject["Titular"]);
            var jsonPadre = serializer.Deserialize<dynamic>(jsonObject["Padre"]);
            var jsonMadre = serializer.Deserialize<dynamic>(jsonObject["Madre"]);

            CBE_MIGRATORIO obj_Migratorio = new CBE_MIGRATORIO();

            string s_Tipo_Actuacion = Convert.ToString(jsonObject["acmi_sTipoActoMigratorio"]);
            
            obj_Migratorio.ACTO.acmi_vNumeroLamina = Convert.ToString(jsonObject["acmi_vNumeroLamina"]);
            
            #region - Validar el número de lámina -
            int IntExisteLamina = Verificar_Lamina_Reposicion(strGUID);

            if (obj_Migratorio.ACTO.acmi_vNumeroLamina != "")
            {
                int i_Resultado = new SGAC.Registro.Actuacion.BL.ActoMigratorioMantenimientoBL().Validar_Insumo(
                    (IntExisteLamina == 1) ? (Int32)Enumerador.enmInsumoTipo.LAMINA_REPOSICION : (Int32)Enumerador.enmInsumoTipo.LAMINA, obj_Migratorio.ACTO.acmi_vNumeroLamina, Convert.ToInt32(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                    Convert.ToInt32(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]), obj_Migratorio.ACTO.acmi_vNumeroLamina, ref s_Resultado);

                if (s_Resultado != "")
                    return s_Resultado;
            }

            #endregion

            #region - llenando los datos de persona -
            obj_Migratorio.PERSONA.pers_iPersonaId = Convert.ToInt64(jsonPersona["pers_iPersonaId"]);
            obj_Migratorio.PERSONA.pers_dNacimientoFecha = Comun.FormatearFecha(jsonPersona["pers_dNacimientoFecha"]);
            obj_Migratorio.PERSONA.IDENTIFICACION.peid_sDocumentoTipoId = Convert.ToInt16(jsonPersona["peid_sDocumentoTipoId"]);
            obj_Migratorio.PERSONA.IDENTIFICACION.peid_vDocumentoNumero = Convert.ToString(jsonPersona["peid_vDocumentoNumero"]);

            obj_Migratorio.PERSONA.IDENTIFICACION.peid_vLugarExpedicion = HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE].ToString();
            obj_Migratorio.PERSONA.IDENTIFICACION.peid_dFecExpedicion = Comun.FormatearFecha(jsonObject["acmi_dFechaExpedicion"]);
            obj_Migratorio.PERSONA.IDENTIFICACION.peid_dFecVcto = Comun.FormatearFecha(jsonObject["acmi_dFechaExpiracion"]);

            obj_Migratorio.PERSONA.pers_sGeneroId = Convert.ToInt16(jsonPersona["pers_sGeneroId"]);
            obj_Migratorio.PERSONA.pers_sEstadoCivilId = Convert.ToInt16(jsonPersona["pers_sEstadoCivilId"]);
            obj_Migratorio.PERSONA.pers_sOcupacionId = Convert.ToInt16(jsonPersona["pers_sOcupacionId"]);
            obj_Migratorio.PERSONA.pers_vApellidoPaterno = Convert.ToString(jsonPersona["pers_vApellidoPaterno"]);
            obj_Migratorio.PERSONA.pers_vApellidoMaterno = Convert.ToString(jsonPersona["pers_vApellidoMaterno"]);
            obj_Migratorio.PERSONA.pers_vNombres = Convert.ToString(jsonPersona["pers_vNombres"]);
            obj_Migratorio.PERSONA.pers_cNacimientoLugar = Convert.ToString(jsonPersona["pers_cNacimientoLugar"]);
            obj_Migratorio.PERSONA.pers_sColorOjosId = Convert.ToInt16(jsonPersona["pers_sColorOjosId"]);
            obj_Migratorio.PERSONA.pers_sColorCabelloId = Convert.ToInt16(jsonPersona["pers_sColorCabelloId"]);
            obj_Migratorio.PERSONA.pers_vEstatura = Convert.ToString(jsonPersona["pers_vEstatura"]);

            obj_Migratorio.PERSONA.pers_sUsuarioCreacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            obj_Migratorio.PERSONA.pers_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
            obj_Migratorio.PERSONA.pers_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            obj_Migratorio.PERSONA.pers_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
            obj_Migratorio.PERSONA.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            BE.RE_RESIDENCIA residenciaPeru = new BE.RE_RESIDENCIA();
            residenciaPeru.resi_iResidenciaId = Convert.ToInt64(jsonPersona["resi_iResidenciaId_peru"]);
            residenciaPeru.resi_vResidenciaDireccion = Convert.ToString(jsonPersona["resi_vResidenciaDireccion_peru"]);
            residenciaPeru.resi_cResidenciaUbigeo = Convert.ToString(jsonPersona["resi_cResidenciaUbigeo_peru"]);
            residenciaPeru.resi_vResidenciaTelefono = Convert.ToString(jsonPersona["resi_vResidenciaTelefono_peru"]);
            residenciaPeru.resi_sUsuarioCreacion = obj_Migratorio.PERSONA.pers_sUsuarioCreacion;
            residenciaPeru.resi_vIPCreacion = obj_Migratorio.PERSONA.pers_vIPCreacion;
            residenciaPeru.resi_sUsuarioModificacion = obj_Migratorio.PERSONA.pers_sUsuarioModificacion;
            residenciaPeru.resi_vIPModificacion = obj_Migratorio.PERSONA.pers_vIPModificacion;
            obj_Migratorio.PERSONA.RESIDENCIAS.Add(residenciaPeru);

            BE.RE_RESIDENCIA residenciaExtranjero = new BE.RE_RESIDENCIA();
            residenciaExtranjero.resi_iResidenciaId = Convert.ToInt64(jsonPersona["resi_iResidenciaId_extranjero"]);
            residenciaExtranjero.resi_vResidenciaDireccion = Convert.ToString(jsonPersona["resi_vResidenciaDireccion_extranjero"]);
            residenciaExtranjero.resi_vResidenciaDireccion = Convert.ToString(jsonPersona["resi_vResidenciaDireccion_extranjero"]);
            residenciaExtranjero.resi_cResidenciaUbigeo = Convert.ToString(jsonPersona["resi_cResidenciaUbigeo_extranjero"]);
            residenciaExtranjero.resi_vResidenciaTelefono = Convert.ToString(jsonPersona["resi_vResidenciaTelefono_extranjero"]);
            residenciaExtranjero.resi_sUsuarioCreacion = obj_Migratorio.PERSONA.pers_sUsuarioCreacion;
            residenciaExtranjero.resi_vIPCreacion = obj_Migratorio.PERSONA.pers_vIPCreacion;
            residenciaExtranjero.resi_sUsuarioModificacion = obj_Migratorio.PERSONA.pers_sUsuarioModificacion;
            residenciaExtranjero.resi_vIPModificacion = obj_Migratorio.PERSONA.pers_vIPModificacion;
            obj_Migratorio.PERSONA.RESIDENCIAS.Add(residenciaExtranjero);

            #endregion

            #region - llenando los datos del padre -
            CBE_FILIACION cbeFiliacionPadre = new CBE_FILIACION();
            cbeFiliacionPadre.pefi_iPersonaId = Convert.ToInt64(jsonPersona["pers_iPersonaId"]);
            cbeFiliacionPadre.pefi_iPersonaFilacionId = Convert.ToInt64(jsonPadre["pefi_iPersonaFilacionId"]);
            cbeFiliacionPadre.pefi_sTipoFilacionId = Convert.ToInt16(jsonPadre["pefi_sTipoFilacionId"]);
            cbeFiliacionPadre.pefi_vNombreFiliacion = Convert.ToString(jsonPadre["pefi_vNombreFiliacion"]);
            cbeFiliacionPadre.pefi_sUsuarioCreacion = obj_Migratorio.PERSONA.pers_sUsuarioCreacion;
            cbeFiliacionPadre.pefi_vIPCreacion = obj_Migratorio.PERSONA.pers_vIPCreacion;
            cbeFiliacionPadre.pefi_sUsuarioModificacion = obj_Migratorio.PERSONA.pers_sUsuarioModificacion;
            cbeFiliacionPadre.pefi_vIPModificacion = obj_Migratorio.PERSONA.pers_vIPModificacion;

            obj_Migratorio.PERSONA.FILIACIONES.Add(cbeFiliacionPadre);
            #endregion

            #region - llenando los datos de la madre -
            CBE_FILIACION cbeFiliacionMadre = new CBE_FILIACION();
            cbeFiliacionMadre.pefi_iPersonaId = Convert.ToInt64(jsonPersona["pers_iPersonaId"]);
            cbeFiliacionMadre.pefi_iPersonaFilacionId = Convert.ToInt64(jsonMadre["pefi_iPersonaFilacionId"]);
            cbeFiliacionMadre.pefi_sTipoFilacionId = Convert.ToInt16(jsonMadre["pefi_sTipoFilacionId"]);
            cbeFiliacionMadre.pefi_vNombreFiliacion = Convert.ToString(jsonMadre["pefi_vNombreFiliacion"]);
            cbeFiliacionMadre.pefi_sUsuarioCreacion = obj_Migratorio.PERSONA.pers_sUsuarioCreacion;
            cbeFiliacionMadre.pefi_vIPCreacion = obj_Migratorio.PERSONA.pers_vIPCreacion;
            cbeFiliacionMadre.pefi_sUsuarioModificacion = obj_Migratorio.PERSONA.pers_sUsuarioModificacion;
            cbeFiliacionMadre.pefi_vIPModificacion = obj_Migratorio.PERSONA.pers_vIPModificacion;

            obj_Migratorio.PERSONA.FILIACIONES.Add(cbeFiliacionMadre);
            #endregion

            obj_Migratorio.ACTO.acmi_iActuacionDetalleId = Convert.ToInt64(jsonObject["acmi_iActuacionDetalleId"]);
            obj_Migratorio.ACTO.acmi_iActoMigratorioId = Convert.ToInt64(jsonObject["acmi_iActoMigratorioId"]);
            obj_Migratorio.ACTO.acmi_iActuacionDetalleId = Convert.ToInt64(jsonObject["acmi_iActuacionDetalleId"]);
            obj_Migratorio.ACTO.acmi_IFuncionarioId = Convert.ToInt32(jsonObject["acmi_IFuncionarioId"]);
            obj_Migratorio.ACTO.acmi_sTipoId = Convert.ToInt16(jsonObject["acmi_sTipoId"]);
            obj_Migratorio.ACTO.acmi_sSubTipoId = Convert.ToInt16(jsonObject["acmi_sSubTipoId"]);
            obj_Migratorio.ACTO.acmi_vNumeroExpediente = Convert.ToString(jsonObject["acmi_vNumeroExpediente"]);
            obj_Migratorio.ACTO.acmi_dFechaExpedicion = Comun.FormatearFecha(jsonObject["acmi_dFechaExpedicion"]);
            obj_Migratorio.ACTO.acmi_vNumeroDocumento = Convert.ToString(jsonObject["acmi_vNumeroDocumento"]);
            obj_Migratorio.ACTO.acmi_iRecurrenteId = Convert.ToInt64(jsonPersona["pers_iPersonaId"]);
            obj_Migratorio.ACTO.acmi_vObservaciones = Convert.ToString(jsonObject["acmi_vObservaciones"]);

            bool s_Imprimir = Convert.ToBoolean(Convert.ToString(jsonObject["acmi_bImprimir"]));

            if (s_Imprimir)
                obj_Migratorio.ACTO.acmi_sEstadoId = (Int16)Enumerador.enmPedidoEstado.PENDIENTE;
            else
            {
                switch (Convert.ToInt32(s_Tipo_Actuacion))
                {
                    case (Int32)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                        obj_Migratorio.ACTO.acmi_sEstadoId = (Int16)Enumerador.enmMigratorioPasaporteEstados.EXPEDIDO;
                        break;
                    case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                        obj_Migratorio.ACTO.acmi_sEstadoId = (Int16)Enumerador.enmEstadoVisa.REGISTRADO;
                        break;
                    case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                        obj_Migratorio.ACTO.acmi_sEstadoId = (Int16)Enumerador.enmEstadoSalvodonducto.REGISTRADO;

                        break;
                }
            }
            obj_Migratorio.ACTO.acmi_sUsuarioCreacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            obj_Migratorio.ACTO.acmi_dFechaCreacion = new MyBasePage().ObtenerFechaActual(HttpContext.Current.Session);
            obj_Migratorio.ACTO.acmi_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
            obj_Migratorio.ACTO.acmi_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();;
            obj_Migratorio.ACTO.acmi_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            obj_Migratorio.ACTO.acmi_dFechaModificacion = new MyBasePage().ObtenerFechaActual(HttpContext.Current.Session);
            obj_Migratorio.ACTO.HostName = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_HOSTNAME]);
            obj_Migratorio.ACTO.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            #region - Llenado los datos historicos -
            BE.MRE.RE_ACTOMIGRATORIOHISTORICO oRE_ACTOMIGRATORIOHISTORICO = new BE.MRE.RE_ACTOMIGRATORIOHISTORICO();
            oRE_ACTOMIGRATORIOHISTORICO.amhi_IFuncionarioId = obj_Migratorio.ACTO.acmi_IFuncionarioId;
            oRE_ACTOMIGRATORIOHISTORICO.amhi_sMotivoId = (Int16)Enumerador.enmMigratorioMotivos.NINGUNO;
            oRE_ACTOMIGRATORIOHISTORICO.amhi_dFechaRegistro = new MyBasePage().ObtenerFechaActual(HttpContext.Current.Session);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_vObservaciones = "";
            oRE_ACTOMIGRATORIOHISTORICO.amhi_sUsuarioCreacion = obj_Migratorio.ACTO.acmi_sUsuarioCreacion;
            oRE_ACTOMIGRATORIOHISTORICO.amhi_vIPCreacion = obj_Migratorio.ACTO.acmi_vIPCreacion;
            oRE_ACTOMIGRATORIOHISTORICO.amhi_dFechaCreacion = new MyBasePage().ObtenerFechaActual(HttpContext.Current.Session);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_sUsuarioModificacion = obj_Migratorio.ACTO.acmi_sUsuarioModificacion;
            oRE_ACTOMIGRATORIOHISTORICO.amhi_vIPModificacion = obj_Migratorio.ACTO.acmi_vIPModificacion;
            oRE_ACTOMIGRATORIOHISTORICO.amhi_dFechaModificacion = obj_Migratorio.ACTO.acmi_dFechaModificacion;
            oRE_ACTOMIGRATORIOHISTORICO.HostName = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_HOSTNAME]);
            oRE_ACTOMIGRATORIOHISTORICO.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_sEstadoId = obj_Migratorio.ACTO.acmi_sEstadoId;
            #endregion


            obj_Migratorio.HISTORICO.Add(oRE_ACTOMIGRATORIOHISTORICO);


            switch (Convert.ToInt16(s_Tipo_Actuacion))
            {
                case (Int16)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                    obj_Migratorio.ACTO.acmi_sTipoDocumentoMigratorioId = (int)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO;
                    obj_Migratorio.ACTO.acmi_vNumeroDocumentoAnterior = Convert.ToString(jsonObject["acmi_vNumeroDocumentoAnterior"]);
                    obj_Migratorio.ACTO.acmi_dFechaExpiracion = obj_Migratorio.ACTO.acmi_dFechaExpedicion.AddMonths(1);
                    obj_Migratorio.PERSONA.IDENTIFICACION.peid_vDocumentoNumero = Convert.ToString(jsonObject["acmi_vNumeroDocumento"]);
                    obj_Migratorio.PERSONA.IDENTIFICACION.peid_dFecVcto = obj_Migratorio.ACTO.acmi_dFechaExpiracion;
                    obj_Migratorio.ACTO.acmi_sPaisId = Convert.ToInt16(jsonObject["acmi_sPaisId"]);

                    if (obj_Migratorio.ACTO.acmi_iActoMigratorioId == 0)
                        s_Resultado = new ActoMigratorioMantenimientoBL().InsertarSalvoconducto(obj_Migratorio);
                    else
                        s_Resultado = new ActoMigratorioMantenimientoBL().ActualizarSalvoconducto(obj_Migratorio);

                    break;
                case (Int16)Enumerador.enmDocumentoMigratorio.VISAS:
                    

                    if (HttpContext.Current.Session["DescTipDoc" + strGUID].ToString().Contains("CE"))
                        obj_Migratorio.PERSONA.Tipo = "CE";
                    obj_Migratorio.ACTO.acmi_sTipoDocumentoMigratorioId = (int)Enumerador.enmDocumentoMigratorio.VISAS;
                    obj_Migratorio.ACTO.acmi_dFechaExpiracion = Comun.FormatearFecha(jsonObject["acmi_dFechaExpiracion"]);
                    obj_Migratorio.FORMATO.amfr_iActoMigratorioFormatoId = Convert.ToInt64(jsonObject["amfr_iActoMigratorioFormatoId"]);
                    obj_Migratorio.FORMATO.amfr_iActoMigratorioId = Convert.ToInt64(jsonObject["amfr_iActoMigratorioId"]);
                    obj_Migratorio.FORMATO.amfr_sTitularFamiliaId = Convert.ToInt16(jsonObject["amfr_sTitularFamiliaId"]);
                    obj_Migratorio.FORMATO.amfr_bAcuerdoProgramaFlag = Convert.ToBoolean(jsonObject["amfr_bAcuerdoProgramaFlag"]);
                    obj_Migratorio.FORMATO.amfr_vVisaCodificacion = Convert.ToString(jsonObject["amfr_vVisaCodificacion"]);
                    obj_Migratorio.FORMATO.amfr_sDiasPermanencia = Convert.ToInt16(jsonObject["amfr_sDiasPermanencia"]);
                    obj_Migratorio.FORMATO.amfr_sTipoAutorizacionId = Convert.ToInt16(jsonObject["amfr_sTipoAutorizacionId"]);

                    obj_Migratorio.FORMATO.amfr_sTipoDocumentoRREEId = Convert.ToInt16(jsonObject["amfr_sTipoDocumentoRREEId"]);
                    obj_Migratorio.FORMATO.amfr_vNumDocumentoRREE = Convert.ToString(jsonObject["amfr_vNumDocumentoRREE"]);
                    string s_fecha = Convert.ToString(jsonObject["amfr_dFechaRREE"]);
                    if (!s_fecha.Equals(""))
                        obj_Migratorio.FORMATO.amfr_dFechaRREE = Comun.FormatearFecha(s_fecha);

                    obj_Migratorio.FORMATO.amfr_sTipoDocumentoDIGEMINId = Convert.ToInt16(jsonObject["amfr_sTipoDocumentoDIGEMINId"]);
                    obj_Migratorio.FORMATO.amfr_vNumDocumentoDIGEMIN = Convert.ToString(jsonObject["amfr_vNumDocumentoDIGEMIN"]);

                    s_fecha = Convert.ToString(jsonObject["amfr_dFechaDIGEMIN"]);
                    if (!s_fecha.Equals(""))
                        obj_Migratorio.FORMATO.amfr_dFechaDIGEMIN = Comun.FormatearFecha(s_fecha);
                    obj_Migratorio.FORMATO.amfr_vCargoFuncionario = Convert.ToString(jsonObject["amfr_vCargoFuncionario"]);

                    obj_Migratorio.FORMATO.amfr_sCargoDiplomaticoId = Convert.ToInt16(jsonObject["amfr_sCargoDiplomaticoId"]);
                    obj_Migratorio.FORMATO.amfr_vMotivoVisaDiplomatica = Convert.ToString(jsonObject["amfr_vMotivoVisaDiplomatica"]);
                    obj_Migratorio.FORMATO.amfr_vInstitucionSolicitaVisaDiplomatica = Convert.ToString(jsonObject["amfr_vInstitucionSolicitaVisaDiplomatica"]);
                    obj_Migratorio.FORMATO.amfr_vInstitucionRealizaVisaDiplomatica = Convert.ToString(jsonObject["amfr_vInstitucionRealizaVisaDiplomatica"]);
                    obj_Migratorio.FORMATO.amfr_sCancilleriaSolicitaAutorizacionId = Convert.ToInt16(jsonObject["amfr_sCancilleriaSolicitaAutorizacionId"]);
                    obj_Migratorio.FORMATO.amfr_vDocumentoAutoriza = Convert.ToString(jsonObject["amfr_vDocumentoAutoriza"]);
                    obj_Migratorio.FORMATO.amfr_sTipoNumeroPasaporteId = Convert.ToString(jsonObject["amfr_sTipoNumeroPasaporteId"]);
                    obj_Migratorio.FORMATO.amfr_vNumeroPasaporte = Convert.ToString(jsonObject["amfr_vNumeroPasaporte"]);

                    obj_Migratorio.FORMATO.amfr_vMedioComunicacionPrensa = Convert.ToString(jsonObject["amfr_vMedioComunicacionPrensa"]);
                    obj_Migratorio.FORMATO.amfr_sCargoPrensaId = Convert.ToInt16(jsonObject["amfr_sCargoPrensaId"]);
                    obj_Migratorio.FORMATO.amfr_vMotivoPrensa = Convert.ToString(jsonObject["amfr_vMotivoPrensa"]);
                    obj_Migratorio.FORMATO.amfr_vObservaciones = Convert.ToString(jsonObject["amfr_vObservaciones"]);

                    obj_Migratorio.FORMATO.amfr_sUsuarioCreacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                    obj_Migratorio.FORMATO.amfr_dFechaCreacion = new MyBasePage().ObtenerFechaActual(HttpContext.Current.Session);
                    obj_Migratorio.FORMATO.amfr_vIPCreacion = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    obj_Migratorio.FORMATO.amfr_vIPModificacion = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    obj_Migratorio.FORMATO.amfr_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                    obj_Migratorio.FORMATO.amfr_dFechaModificacion = new MyBasePage().ObtenerFechaActual(HttpContext.Current.Session);
                    obj_Migratorio.FORMATO.amfr_cEstado = "A";
                    obj_Migratorio.FORMATO.amfr_bFlagVisaDiplomatica = false;
                    obj_Migratorio.FORMATO.amfr_bFlagVisaPrensa = false;
                    obj_Migratorio.FORMATO.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    obj_Migratorio.FORMATO.HostName = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_HOSTNAME]);

                    if (obj_Migratorio.FORMATO.amfr_iActoMigratorioFormatoId == 0)
                        s_Resultado = new ActoMigratorioMantenimientoBL().InsertarVisa(obj_Migratorio);
                    else
                        s_Resultado = new ActoMigratorioMantenimientoBL().ActualizarVisa(obj_Migratorio);
                    break;
                case (Int16)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                    obj_Migratorio.ACTO.acmi_sTipoDocumentoMigratorioId = (int)Enumerador.enmDocumentoMigratorio.PASAPORTE;
                    obj_Migratorio.ACTO.acmi_dFechaExpiracion = Comun.FormatearFecha(jsonObject["acmi_dFechaExpiracion"]);
                    obj_Migratorio.PERSONA.IDENTIFICACION.peid_vDocumentoNumero = Convert.ToString(jsonObject["acmi_vNumeroDocumento"]);

                    obj_Migratorio.FORMATO.amfr_iActoMigratorioFormatoId = Convert.ToInt64(jsonObject["amfr_iActoMigratorioFormatoId"]);

                    #region Pasaporte Revalidado - FORMATO
                    if (obj_Migratorio.ACTO.acmi_sTipoId == (int)Enumerador.enmPasaporteTipo.REVALIDADO)
                    {
                        obj_Migratorio.PERSONA.Tipo = "RE";    
                        obj_Migratorio.FORMATO.amfr_sPasaporteRevalidarOficinaConsularId = Convert.ToInt16(jsonObject["amfr_sPasaporteRevalidarOficinaConsularId"]);
                        obj_Migratorio.FORMATO.amfr_sPasaporteRevalidarOficinaMigracionId = Convert.ToInt16(jsonObject["amfr_sPasaporteRevalidarOficinaMigracionId"]);
                        obj_Migratorio.FORMATO.amfr_dPasaporteRevalidarFechaExpedicion = Comun.FormatearFecha(jsonObject["amfr_dPasaporteRevalidarFechaExpedicion"]);
                        obj_Migratorio.FORMATO.amfr_dPasaporteRevalidarFechaExpiracion = Comun.FormatearFecha(jsonObject["amfr_dPasaporteRevalidarFechaExpiracion"]);

                    }
                    else
                    {
                        obj_Migratorio.ACTO.acmi_vNumeroDocumentoAnterior = Convert.ToString(jsonObject["acmi_vNumeroDocumentoAnterior"]);
                        obj_Migratorio.FORMATO.amfr_sTipoNumeroPasaporteId = Convert.ToString(jsonPersona["peid_sDocumentoTipoId"]);
                        obj_Migratorio.FORMATO.amfr_vNumeroPasaporte = Convert.ToString(jsonPersona["peid_vDocumentoNumero"]);
                    }

                    obj_Migratorio.FORMATO.amfr_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                    obj_Migratorio.FORMATO.amfr_sUsuarioCreacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                    obj_Migratorio.FORMATO.amfr_vIPCreacion = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    obj_Migratorio.FORMATO.amfr_dFechaCreacion = new MyBasePage().ObtenerFechaActual(HttpContext.Current.Session);
                    obj_Migratorio.FORMATO.amfr_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                    obj_Migratorio.FORMATO.amfr_vIPModificacion = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    obj_Migratorio.FORMATO.amfr_dFechaModificacion = new MyBasePage().ObtenerFechaActual(HttpContext.Current.Session);
                    obj_Migratorio.FORMATO.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    obj_Migratorio.FORMATO.HostName = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_HOSTNAME]);
                    #endregion

                    if (obj_Migratorio.ACTO.acmi_iActoMigratorioId == 0)
                    {
                        if (obj_Migratorio.ACTO.acmi_sTipoId == (int)Enumerador.enmPasaporteTipo.EXPEDIDO)
                            s_Resultado = new ActoMigratorioMantenimientoBL().InsertarPasaporteExpedido(obj_Migratorio);
                        else
                            s_Resultado = new ActoMigratorioMantenimientoBL().InsertarPasaporteRevalidado(obj_Migratorio);
                    }
                    else
                    {
                        if (obj_Migratorio.ACTO.acmi_sTipoId == (int)Enumerador.enmPasaporteTipo.EXPEDIDO)
                            s_Resultado = new ActoMigratorioMantenimientoBL().ActualizarPasaporteExpedido(obj_Migratorio);
                        else
                            s_Resultado = new ActoMigratorioMantenimientoBL().ActualizarPasaporteRevalidado(obj_Migratorio);
                    }
                    break;
            }

            return s_Resultado;
        }

        protected void ddlDomicilioNacDepa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDomicilioNacDepa.SelectedIndex > 0)
                {
                    Util.CargarDropDownList(ddlDomicilioNacProv,
                        Comun.ObtenerPaisProv(Session, ddlDomicilioNacDepa.SelectedValue),
                            "ubge_vProvincia", "ubge_cUbi02", true);
                    ddlDomicilioNacProv_SelectedIndexChanged(sender, e);
                    ScriptManager.GetCurrent(Page).SetFocus(ddlDomicilioNacDepa.ClientID);
                    
                }
                else
                {
                    Util.CargarParametroDropDownList(ddlDomicilioNacProv, new DataTable(), true);
                    Util.CargarParametroDropDownList(ddlDomicilioNacDist, new DataTable(), true);
                }
            }
            catch (Exception ex)
            {
                Session["_LastException"] = ex;
                Response.Redirect("../PageError/GenericErrorPage.aspx");
            }
        }

        protected void ddlDomicilioNacProv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDomicilioNacProv.SelectedIndex > 0)
                {
                    Util.CargarDropDownList(ddlDomicilioNacDist,
                        Comun.ObtenerRegiDist(Session,
                            ddlDomicilioNacDepa.SelectedValue, ddlDomicilioNacProv.SelectedValue),
                            "ubge_vDistrito", "ubge_cUbi03", true);
                    ScriptManager.GetCurrent(Page).SetFocus(ddlDomicilioNacProv.ClientID);
                }
                else
                {
                    Util.CargarParametroDropDownList(ddlDomicilioNacDist, new DataTable(), true);
                }
            }
            catch (Exception ex)
            {
                Session["_LastException"] = ex;
                Response.Redirect("../PageError/GenericErrorPage.aspx");
            }

        }

        protected void btnVistaPrev_Click(object sender, EventArgs e)
        {
            bool s_Imprime = true;
            switch (Comun.ToNullInt32(hId_TipoActuacion.Value))
            {
                case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                    if ((int)Session[Constantes.CONST_SESION_OFICINACONSULAR_ID] == Constantes.CONST_OFICINACONSULAR_LIMA)
                        s_Imprime = false;
                    break;
            }

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
                        Int64 intActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);

                        DocumentoiTextSharp oDocumentoiTextSharp = new DocumentoiTextSharp(this.Page, string.Empty, HttpContext.Current.Server.MapPath("~/Images/Escudo.JPG"));
                        oDocumentoiTextSharp.ActuacionDetalleId = intActuacionDetalleId;
                        oDocumentoiTextSharp.CrearAutoAdhesivo();

                        break;
                    default:
                        //string strUrl = "../Registro/FrmRepAutoadhesivo.aspx";
                        //strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=500,height=700,left=100,top=100');";
                        strScript = "abrirVentana('../Registro/FrmRepAutoadhesivo.aspx?GUID=" + HFGUID.Value + "', 'AUTOADHESIVOS', 610, 450, '');";
                        Comun.EjecutarScript(Page, strScript);
                        Session["FEC_IMPRESION"] = Util.ObtenerFechaActual(
                            Convert.ToInt16(Comun.ObtenerDatoOficinaConsular(Session, "ofco_sDiferenciaHoraria")),
                            Convert.ToInt16(Comun.ObtenerDatoOficinaConsular(Session, "ofco_sHorarioVerano")));
                        break;
                }
            }
            else
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTUACIÓN CONSULAR", "LIMA no puede imprimir al Autoadhesivo");
                Comun.EjecutarScript(Page, strScript);
                return;
            }
        }

        protected void btnGrabarVinculacion_Click(object sender, EventArgs e)
        {
            String strScript = String.Empty;

            string s_Validacion = string.Empty;

            if (!chk_PrintAutoadhesivo.Checked)
                s_Validacion = "[Autoadhesivos] - ";
            if (!chk_PintLamina.Checked)
                s_Validacion += "[Lámina]";
            if(!chk_passaporte.Checked)
                s_Validacion += "[Librillo]";

            switch (Comun.ToNullInt32(hId_TipoActuacion.Value))
            { 
                case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                    s_Validacion = s_Validacion.Replace("[Lámina]", "[Etiqueta]");
                    s_Validacion = s_Validacion.Replace("[Librillo]", " ");
                    break;
            }

            string s_Mensaje_AutoAdhesivo = string.Empty;
            if (Comun.ToNullInt64(hacmi_iActoMigratorioId.Value) > 0)
            {
                //if (chk_PrintAutoadhesivo.Checked && chk_PintLamina.Checked)
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
                                                Convert.ToInt16(Comun.ObtenerDatoOficinaConsular(Session, "ofco_sDiferenciaHoraria")),
                                                Convert.ToInt16(Comun.ObtenerDatoOficinaConsular(Session, "ofco_sHorarioVerano")));

                DateTime dFecImpresion = Comun.FormatearFecha("01/01/1800");

                #region - Validando si es lámina de reposición -
                int IntExisteLamina = Verificar_Lamina_Reposicion(HFGUID.Value);
                if (IntExisteLamina == 1)
                    s_Validacion.Replace("[Lámina]", "[Lámina - Reposición]");
                #endregion

                int Tipo_Librillo = 0;
                string s_Numero = string.Empty;
                switch (Comun.ToNullInt32(hdn_acmi_sTipoId.Value))
                {
                    case (int)Enumerador.enmPasaporteTipo.EXPEDIDO:
                    case (int)Enumerador.enmPasaporteTipo.REVALIDADO:
                        Tipo_Librillo = (Int32)Enumerador.enmInsumoTipo.PASAPORTE;
                        break;
                    default:
                        switch (Comun.ToNullInt32(hId_TipoActuacion.Value))
                        {
                            case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                                Tipo_Librillo = (Int32)Enumerador.enmInsumoTipo.SALVOCONDUCTO;
                                break;
                            case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                                Tipo_Librillo = 0;
                                txtCodPassaporte.Text = "";
                                break;
                        }
                        break;
                }

                string strCodAutoadhesivo = txtCodAutoadhesivo.Text.Trim();
                string strCodLamina = txtCodLamina.Text.Trim();
                string strCodPasaporte = txtCodPassaporte.Text.Trim();

                if (!chk_PrintAutoadhesivo.Enabled && !txtCodAutoadhesivo.Enabled &&
                    txtCodAutoadhesivo.Text.Trim() != string.Empty)
                    strCodAutoadhesivo = "";

                if (!chk_PintLamina.Enabled && !txtCodLamina.Enabled &&
                    txtCodLamina.Text.Trim() != string.Empty)
                    strCodLamina = "";

                if (!chk_passaporte.Enabled && !txtCodPassaporte.Enabled &&
                    txtCodPassaporte.Text.Trim() != string.Empty)
                    strCodPasaporte = "";

                // Verificar si hay algún insumo para vincular
                if (strCodAutoadhesivo == string.Empty &&
                    strCodLamina == string.Empty &&
                    strCodPasaporte == string.Empty)
                {
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "VINCULACIÓN", "No hay insumos por vincular.", false, 200, 400);
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }

                ActuacionMantenimientoBL oActuacionMantBL = new ActuacionMantenimientoBL();
                int intInsumoLaminaId = 0;
                
                if (Comun.ToNullInt32(hId_TipoActuacion.Value) == (Int32)Enumerador.enmDocumentoMigratorio.VISAS) {
                    intInsumoLaminaId = (Int32)Enumerador.enmInsumoTipo.ETIQUETA;
                }
                else if (Comun.ToNullInt32(hId_TipoActuacion.Value) == (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO)
                {
                    if (IntExisteLamina == 1) intInsumoLaminaId = (Int32)Enumerador.enmInsumoTipo.LAMINA_SALVOCONDUCTO_REPOSICION;
                    else intInsumoLaminaId = (Int32)Enumerador.enmInsumoTipo.LAMINA_SALVOCONDUCTO;
                }
                else 
                {
                    if (Comun.ToNullInt32(hdn_acmi_sTipoId.Value) == (int)Enumerador.enmPasaporteTipo.REVALIDADO)
                    {
                        if (IntExisteLamina == 1) intInsumoLaminaId = (Int32)Enumerador.enmInsumoTipo.LAMINA_REPOSICION;
                        else intInsumoLaminaId = (Int32)Enumerador.enmInsumoTipo.LAMINA_RENOVACION;
                    }
                    else { 
                        if (IntExisteLamina == 1) intInsumoLaminaId = (Int32)Enumerador.enmInsumoTipo.LAMINA_REPOSICION;
                        else intInsumoLaminaId = (Int32)Enumerador.enmInsumoTipo.LAMINA;
                    }
                }



                oActuacionMantBL.Vincular_Insumo(
                    Comun.ToNullInt64(Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value]),
                    Comun.ToNullInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]),
                    (Int32)Enumerador.enmInsumoTipo.AUTOADHESIVO,
                   // ((Comun.ToNullInt32(hId_TipoActuacion.Value) == (Int32)Enumerador.enmDocumentoMigratorio.VISAS) ? (Int32)Enumerador.enmInsumoTipo.ETIQUETA : ((Comun.ToNullInt32(hdn_acmi_sTipoId.Value) == (int)Enumerador.enmPasaporteTipo.REVALIDADO) ? ((IntExisteLamina == 1) ? (Int32)Enumerador.enmInsumoTipo.LAMINA_REPOSICION : (Int32)Enumerador.enmInsumoTipo.LAMINA_RENOVACION) : (IntExisteLamina == 1) ? (Int32)Enumerador.enmInsumoTipo.LAMINA_REPOSICION : (Int32)Enumerador.enmInsumoTipo.LAMINA)),
                    intInsumoLaminaId,
                    Tipo_Librillo,
                    strCodAutoadhesivo,
                    strCodLamina,
                    strCodPasaporte,
                    dFecActual,
                    false,
                    dFecActual,
                    0, // FUNCIONARIO
                    Comun.ToNullInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                    Comun.ToNullInt32(Session[Constantes.CONST_SESION_USUARIO_ID]), txtNumSal_Lam.Text, "",
                    ref s_Mensaje_AutoAdhesivo
                );
                
                switch (Comun.ToNullInt32(hdn_acmi_sTipoId.Value))
                {
                    case (int)Enumerador.enmPasaporteTipo.REVALIDADO:
                        s_Mensaje_AutoAdhesivo.Replace("[Lámina]", "[Lámina - Renovación]");
                        break;
                }

                if (IntExisteLamina == 1)
                    s_Mensaje_AutoAdhesivo.Replace("[Lámina]", "[Lámina - Reposición]");

                if (string.IsNullOrEmpty(s_Mensaje_AutoAdhesivo))
                {
                    if (chk_PrintAutoadhesivo.Checked && txtCodAutoadhesivo.Text.Trim() != string.Empty &&
                        chk_PintLamina.Checked && txtCodLamina.Text.Trim() != string.Empty &&
                        chk_passaporte.Checked && txtCodPassaporte.Text.Trim() != string.Empty)
                    {
                        btnGrabarVinculacion.Enabled = false;
                    }

                    if (chk_PrintAutoadhesivo.Checked && txtCodAutoadhesivo.Text.Trim() != string.Empty)
                    {
                        txtCodAutoadhesivo.Enabled = false;
                        chk_PrintAutoadhesivo.Enabled = false;
                        //btnVistaPrev.Enabled = false;              
                    }

                             
                    txtNumSal_Lam.Text = txtCodLamina.Text;
                        
                    txtNumSal_Lam.Enabled = false;

                    if (chk_PintLamina.Checked && txtCodLamina.Text.Trim() != string.Empty)
                    {
                        txtCodLamina.Enabled = false;
                        chk_PintLamina.Enabled = false;
                    }

                    ctrlToolBarRegistro.btnGrabar.Enabled = false;
                    ctrlToolBarActuacion.btnGrabar.Enabled = false;
                    txtNumeroPass.Enabled = false;
                    updCaberaFormato.Update();
                    updConsulta.Update();

                    Session[strVariableAccion] = (int)Enumerador.enmTipoOperacion.CONSULTA;

                    ctrlAdjunto.BtnGrabActAdj.Enabled = false;
                    ctrlAdjunto.HabilitarControlesAdjunto(false);
                    UpdAdjuntos.Update();
                    ctrlToolBarRegistro.btnGrabar.Enabled = false;
                    updRegPago.Update();
                    Habilitar_Controles(false);
                    updConsulta.Update();
                    txtNumeroPass.Enabled = false;
                    ctrlToolBarActuacion.btnGrabar.Enabled = false;
                    updCaberaFormato.Update();

                    //Verificando los datos del insumo de librillo pasaporte y librillo salvoconducto
                    SGAC.BE.MRE.RE_ACTOMIGRATORIO oRE_ACTOMIGRATORIO = new BE.MRE.RE_ACTOMIGRATORIO();

                    switch (Comun.ToNullInt32(hdn_acmi_sTipoId.Value))
                    {
                        case (int)Enumerador.enmPasaporteTipo.EXPEDIDO:
                        case (int)Enumerador.enmPasaporteTipo.REVALIDADO:

                            if (chk_passaporte.Checked && txtCodPassaporte.Text.Trim() != string.Empty)
                            {
                                txtCodPassaporte.Enabled = false;
                                chk_passaporte.Enabled = false;
                            }
                            if (txtCodAutoadhesivo.Text.Trim() != string.Empty && txtCodLamina.Text.Trim() != string.Empty && txtCodPassaporte.Text.Trim() != string.Empty)
                            {

                                #region - Actualizando los datos de la lámina  -
                                oRE_ACTOMIGRATORIO = new BE.MRE.RE_ACTOMIGRATORIO();
                                oRE_ACTOMIGRATORIO.acmi_iActoMigratorioId = Comun.ToNullInt64(hacmi_iActoMigratorioId.Value);

                                // T#000263
                                if (Comun.ToNullInt32(hdn_acmi_sTipoId.Value) == (int)Enumerador.enmPasaporteTipo.EXPEDIDO)
                                    oRE_ACTOMIGRATORIO.acmi_vNumeroLamina = txtCodLamina.Text.Trim();

                                oRE_ACTOMIGRATORIO.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                                oRE_ACTOMIGRATORIO.acmi_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP(); ;
                                oRE_ACTOMIGRATORIO.acmi_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                                oRE_ACTOMIGRATORIO.acmi_dFechaModificacion = ObtenerFechaActual(HttpContext.Current.Session);
                                oRE_ACTOMIGRATORIO.HostName = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_HOSTNAME]);

                                new SGAC.Registro.Actuacion.BL.ActoMigratorioMantenimientoBL().Actualizar_Lamina(oRE_ACTOMIGRATORIO);

                                #endregion

                                updCaberaFormato.Update();
                            }
                            break;
                        default:

                            switch (Comun.ToNullInt32(hId_TipoActuacion.Value))
                            {
                                case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                                    if (chk_passaporte.Checked && txtCodPassaporte.Text.Trim() != string.Empty)
                                    {
                                        txtCodPassaporte.Enabled = false;
                                        chk_passaporte.Enabled = false;
                                    }

                                    #region - Actualizando los datos de la lámina  -
                                    oRE_ACTOMIGRATORIO.acmi_iActoMigratorioId = Comun.ToNullInt64(hacmi_iActoMigratorioId.Value);
                                        oRE_ACTOMIGRATORIO.acmi_vNumeroLamina = txtCodLamina.Text.Trim();
                                        oRE_ACTOMIGRATORIO.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                                        oRE_ACTOMIGRATORIO.acmi_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP(); ;
                                        oRE_ACTOMIGRATORIO.acmi_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                                        oRE_ACTOMIGRATORIO.acmi_dFechaModificacion = ObtenerFechaActual(HttpContext.Current.Session);
                                        oRE_ACTOMIGRATORIO.HostName = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_HOSTNAME]);

                                        new SGAC.Registro.Actuacion.BL.ActoMigratorioMantenimientoBL().Actualizar_Lamina(oRE_ACTOMIGRATORIO);

                                        #endregion

                                        updCaberaFormato.Update();
                                    break;
                                default:
                                    if (txtCodAutoadhesivo.Text.Trim() != string.Empty && txtCodLamina.Text.Trim() != string.Empty)
                                    {
                                        #region - Actualizando los datos de la lámina  -
                                        oRE_ACTOMIGRATORIO = new BE.MRE.RE_ACTOMIGRATORIO();
                                        oRE_ACTOMIGRATORIO.acmi_iActoMigratorioId = Comun.ToNullInt64(hacmi_iActoMigratorioId.Value);
                                        oRE_ACTOMIGRATORIO.acmi_vNumeroLamina = txtCodLamina.Text.Trim();
                                        oRE_ACTOMIGRATORIO.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                                        oRE_ACTOMIGRATORIO.acmi_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP(); ;
                                        oRE_ACTOMIGRATORIO.acmi_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                                        oRE_ACTOMIGRATORIO.acmi_dFechaModificacion = ObtenerFechaActual(HttpContext.Current.Session);
                                        oRE_ACTOMIGRATORIO.HostName = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_HOSTNAME]);

                                        new SGAC.Registro.Actuacion.BL.ActoMigratorioMantenimientoBL().Actualizar_Lamina(oRE_ACTOMIGRATORIO);

                                        #endregion

                                        updCaberaFormato.Update();
                                    }
                                    break;
                            }
                            break;
                    }

                    lblEstado.Text = "IMPRESO";

                    #region Tipo Adjunto
                    ctrlAdjunto.CargarTipoArchivo();
                    UpdAdjuntos.Update();
                    #endregion

                    ////#region Validacion
                    ////String CodAutoAdhesivo = String.Empty;
                    //String CodLamina = String.Empty;
                    //String CodLibrillo = String.Empty;
                    ////CodAutoAdhesivo =  txtCodAutoadhesivo.Text.Trim();
                    ////CodLamina = txtCodLamina.Text.Trim();
                    ////CodLibrillo = txtCodPassaporte.Text.Trim();

                    ////if (CodAutoAdhesivo == String.Empty)
                    ////{ 
                    ////    chk_PrintAutoadhesivo.Checked = false; 
                    ////    chk_PrintAutoadhesivo.Enabled = true; 
                    ////    txtCodAutoadhesivo.Enabled = true;
                    ////    txtCodAutoadhesivo.Text = String.Empty; 
                    ////}
                    //if (CodLamina == String.Empty)
                    //{
                    //    chk_PintLamina.Checked = false;
                    //    chk_PintLamina.Enabled = true;
                    //    txtCodLamina.Enabled = true;
                    //    txtCodLamina.Text = String.Empty;
                    //}

                    //if (CodLibrillo == String.Empty)
                    //{
                    //    chk_passaporte.Checked = false;
                    //    chk_passaporte.Enabled = true;
                    //    txtCodPassaporte.Enabled = true;
                    //    txtCodPassaporte.Text = String.Empty;
                    //}


                    //#endregion

                    
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION,
                        "VINCULACIÓN", "La vinculación se realizó correctamente.", false, 200, 300);
                    Comun.EjecutarScript(Page, strScript);
                    BindGridActuacionesInsumoDetalle(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]));
                    updVinculacion.Update();
                        
                }
                else
                {
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "VINCULACIÓN", s_Mensaje_AutoAdhesivo, false, 200, 400);
                    Comun.EjecutarScript(Page, strScript);
                }
            }
            else
            {
                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "VINCULACIÓN", "Debe Grabar el acto migratorio antes de realizar la vinculación.", false, 200, 400);
                Comun.EjecutarScript(Page, strScript);
                return;
            }

            ddlTipoPago.SelectedValue = hTipoPago.Value.ToString();
            DesabilitarTipoPago();
        }

        public static Int32 Verificar_Lamina_Reposicion(string strGUID)
        {
            int IntExisteLamina = 0;
            int IntTotalCount = 0;
            int IntTotalPages = 0;

            DataTable dtActuacionInsumoDetalle = new SGAC.Registro.Actuacion.BL.ActuacionMantenimientoBL().Obtener_ActuacionInsumoDetalleAll(Comun.ToNullInt64(HttpContext.Current.Session[Constantes.CONST_SESION_ACTUACIONDET_ID + strGUID]),
                1, 200, ref IntTotalCount, ref  IntTotalPages);

            if (dtActuacionInsumoDetalle.Rows.Count > 0)
            {
                IntExisteLamina = 0;
                foreach (System.Data.DataRow row in dtActuacionInsumoDetalle.Rows)
                {
                    switch (Comun.ToNullInt32(row["TipoInsumo"]))
                    {
                        case (Int32)Enumerador.enmInsumoTipo.LAMINA:
                        case (Int32)Enumerador.enmInsumoTipo.LAMINA_RENOVACION:
                        case (Int32)Enumerador.enmInsumoTipo.LAMINA_SALVOCONDUCTO:
                            IntExisteLamina = 1;
                            break;
                    }
                }
            }

            return IntExisteLamina;
        }

        protected void chk_PrintAutoadhesivo_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_PrintAutoadhesivo.Checked)
                txtCodAutoadhesivo.Enabled = true;
            else
                txtCodAutoadhesivo.Enabled = false;

            updVinculacion.Update();
        }

        protected void chk_PintLamina_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_PintLamina.Checked)
                txtCodLamina.Enabled = true;
            else
                txtCodLamina.Enabled = false;

            updVinculacion.Update();
        }

        [WebMethod]
        public static string Habilitar_Formato_DGC(string hId_TipoActuacion, string ddlVisaSubTipo, string hdn_acmi_sTipoId,
            string hacmi_iActoMigratorioId)
        {
            string s_Visa_Tipo = string.Empty;

            switch (Comun.ToNullInt32(hId_TipoActuacion))
            {
                case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                    HttpContext.Current.Session["iTipo_Reporte"] = Enumerador.enmMigratorioFormato.DGC_005_VISA;
                    switch (Comun.ToNullInt64(ddlVisaSubTipo))
                    {
                        case (int)Enumerador.enmMigratorioVisaTipoTemporal.DIPLOMATICA:
                        case (int)Enumerador.enmMigratorioVisaTipoTemporal.FAMILIAR_OFICIAL:
                        case (int)Enumerador.enmMigratorioVisaTipoTemporal.INTERCAMBIO:
                        case (int)Enumerador.enmMigratorioVisaTipoTemporal.OFICIAL:
                            s_Visa_Tipo = "&VisaTipo=DIPLOMATICA";
                            break;
                        case (int)Enumerador.enmMigratorioVisaTipoTemporal.PERIODISTA:
                        case (int)Enumerador.enmMigratorioVisaTipoResidente.PERIODISTA:
                            s_Visa_Tipo = "&VisaTipo=PRENSA";
                            break;
                        default:
                            break;
                    }
                    
                    break;
                case (Int32)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                    if (Comun.ToNullInt32(hdn_acmi_sTipoId) == (int)Enumerador.enmPasaporteTipo.EXPEDIDO)
                        HttpContext.Current.Session["iTipo_Reporte"] = Enumerador.enmMigratorioFormato.DGC_001_PASAPORTE_EXPEDIDO;
                    else
                        HttpContext.Current.Session["iTipo_Reporte"] = Enumerador.enmMigratorioFormato.DGC_002_PASAPORTE_REVALIDADO;
                    break;
                case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                    HttpContext.Current.Session["iTipo_Reporte"] = Enumerador.enmMigratorioFormato.DGC_006_SALVOCONDUCTO;
                    break;
            }

            HttpContext.Current.Session["Acto_Migratorio_ID"] = hacmi_iActoMigratorioId;

            return s_Visa_Tipo;

        }
        protected void btn_formato_Click(object sender, EventArgs e)
        {
            string s_Visa_Tipo = string.Empty;
            try
            {
                switch (Comun.ToNullInt32(hId_TipoActuacion.Value))
                {
                    case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                        Session["iTipo_Reporte"] = Enumerador.enmMigratorioFormato.DGC_005_VISA;
                        if (Comun.ToNullInt64(ddlVisaSubTipo.SelectedValue) == 9055)
                            s_Visa_Tipo = "&VisaTipo=DIPLOMATICA";
                        break;
                    case (Int32)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                        if (Comun.ToNullInt32(hdn_acmi_sTipoId.Value) == (int)Enumerador.enmPasaporteTipo.EXPEDIDO)
                            Session["iTipo_Reporte"] = Enumerador.enmMigratorioFormato.DGC_001_PASAPORTE_EXPEDIDO;
                        else
                            Session["iTipo_Reporte"] = Enumerador.enmMigratorioFormato.DGC_002_PASAPORTE_REVALIDADO;
                        break;
                    case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                        Session["iTipo_Reporte"] = Enumerador.enmMigratorioFormato.DGC_006_SALVOCONDUCTO;
                        break;
                }


                if (Session[Constantes.CONST_ACTUACION_INSUMO_DETALLE_ID_PAS] != null)
                    Session[Constantes.CONST_ACTUACION_INSUMO_DETALLE_ID + HFGUID.Value] = Session[Constantes.CONST_ACTUACION_INSUMO_DETALLE_ID_PAS].ToString();
                else
                    Session[Constantes.CONST_ACTUACION_INSUMO_DETALLE_ID + HFGUID.Value] = 0;

                Session["Acto_Migratorio_ID"] = hacmi_iActoMigratorioId.Value;
                string strUrl = "../Registro/frmReporteMigratorio.aspx?vClass=1" + s_Visa_Tipo;
                //string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=500,height=700,left=100,top=100');";

                string strScript = "abrirVentana('" + strUrl + "', 'AUTOADHESIVOS', 500, 700, '');";
                Comun.EjecutarScript(Page, strScript);
            }
            catch (Exception ex)
            {
               string strScript_ = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "MIGRATORIO", ex.Message.ToString(), false, 200, 400);
                Comun.EjecutarScript(Page, strScript_);
            }
        }

        protected void btn_Lamina_Click(object sender, EventArgs e)
        {
            try
            {

                switch (Comun.ToNullInt32(hId_TipoActuacion.Value))
                {
                    case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                        Session["iTipo_Reporte"] = Enumerador.enmMigratorioFormato.DGC_005_VISA_LAMINA;
                        Session["Formato"] = "0";
                        break;
                    case (Int32)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                        if (Comun.ToNullInt32(hdn_acmi_sTipoId.Value) == (int)Enumerador.enmPasaporteTipo.EXPEDIDO)
                            Session["iTipo_Reporte"] = Enumerador.enmMigratorioFormato.DGC_001_PASAPORTE_EXPEDIDO_LAMINA;
                        else
                            Session["iTipo_Reporte"] = Enumerador.enmMigratorioFormato.DGC_002_PASAPORTE_REVALIDADO_LAMINA;

                        Session["Formato"] = "1";
                        break;
                    case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                        Session["iTipo_Reporte"] = Enumerador.enmMigratorioFormato.DGC_006_SALVOCONDUCTO_LAMINA;
                        Session["Formato"] = "1";
                        break;
                }
                Session[Constantes.CONST_ACTUACION_INSUMO_DETALLE_ID + HFGUID.Value] = Session[Constantes.CONST_ACTUACION_INSUMO_DETALLE_ID_LAMINA].ToString(); 
                Session["Acto_Migratorio_ID"] = hacmi_iActoMigratorioId.Value;
                string NumeroFormato = "234";
                string strUrl = "../Registro/FrmReporteMigratorioHtml.aspx?vClass=2&FormatoDGC=" + NumeroFormato + "&GUID=" + HFGUID.Value;
                //string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=500,height=700,left=100,top=100');";

                string strScript = "abrirVentana('" + strUrl + "', 'AUTOADHESIVOS', 500, 700, '');";

                Comun.EjecutarScript(Page, strScript);
            }
            catch (Exception ex)
            {
                string strScript_ = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "MIGRATORIO", ex.Message.ToString(), false, 200, 400);
                Comun.EjecutarScript(Page, strScript_);
            }
        }

        protected void btn_solicitar_Click(object sender, EventArgs e)
        {
            string strGUID = "";
            if (HFGUID.Value.Length > 0)
            {
                strGUID = HFGUID.Value;
            }

            var existe_registro = new SGAC.Registro.Actuacion.BL.ActoMigratorioConsultaBL().Consultar_Acto_Migratorio(
                        Comun.ToNullInt64(Session["iPersonaId" + strGUID]),
                        Comun.ToNullInt64(hacmi_iActuacionDetalleId.Value));

            var obj_Migratorio = existe_registro.ACTO;

            txtNumeroExp.Text = obj_Migratorio.acmi_vNumeroExpediente;
         
            CBE_MIGRATORIO oRE_ACTOMIGRATORIO = new CBE_MIGRATORIO();

            #region - Llenado los datos historicos -
            BE.MRE.RE_ACTOMIGRATORIOHISTORICO oRE_ACTOMIGRATORIOHISTORICO = new BE.MRE.RE_ACTOMIGRATORIOHISTORICO();
            oRE_ACTOMIGRATORIOHISTORICO.amhi_IFuncionarioId = Comun.ToNullInt16(ddlFuncionario.SelectedItem.Value);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_sMotivoId = (Int16)Enumerador.enmMigratorioMotivos.NINGUNO;
            oRE_ACTOMIGRATORIOHISTORICO.amhi_dFechaRegistro = ObtenerFechaActual(HttpContext.Current.Session);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_vObservaciones = "";
            oRE_ACTOMIGRATORIOHISTORICO.amhi_sUsuarioCreacion = Comun.ToNullInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();;
            oRE_ACTOMIGRATORIOHISTORICO.amhi_dFechaCreacion = ObtenerFechaActual(HttpContext.Current.Session);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_sUsuarioModificacion = Comun.ToNullInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();;
            oRE_ACTOMIGRATORIOHISTORICO.amhi_dFechaModificacion = ObtenerFechaActual(HttpContext.Current.Session);
            oRE_ACTOMIGRATORIOHISTORICO.HostName = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_HOSTNAME]);
            oRE_ACTOMIGRATORIOHISTORICO.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            #endregion

            Int16 i_Estado_Solicitado = 0;
            Int32 i_Resultado = 0;

            switch (Comun.ToNullInt32(hId_TipoActuacion.Value))
            {
                case (Int32)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                    if (Comun.ToNullInt32(hId_Estado.Value) == (Int32)Enumerador.enmMigratorioPasaporteEstados.OBSERVADO)
                        i_Estado_Solicitado = (Int32)Enumerador.enmMigratorioPasaporteEstados.CORREGIDO;
                    else
                        i_Estado_Solicitado = (Int32)Enumerador.enmMigratorioPasaporteEstados.ANULADO;
                    break;
                case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                    if (Comun.ToNullInt32(hId_Estado.Value) == (Int32)Enumerador.enmEstadoSalvodonducto.OBSERVADO)
                        i_Estado_Solicitado = (Int32)Enumerador.enmEstadoSalvodonducto.CORREGIDO;
                    else
                        i_Estado_Solicitado = (Int32)Enumerador.enmEstadoSalvodonducto.SOLICITADO;
                    break;
                case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                    i_Estado_Solicitado = (Int32)Enumerador.enmEstadoVisa.SOLICITADO;
                    break;
            }

            oRE_ACTOMIGRATORIOHISTORICO.amhi_sEstadoId = i_Estado_Solicitado;
            oRE_ACTOMIGRATORIO.ACTO.acmi_sEstadoId = i_Estado_Solicitado;

            oRE_ACTOMIGRATORIO.HISTORICO.Add(oRE_ACTOMIGRATORIOHISTORICO);

            oRE_ACTOMIGRATORIO.ACTO.acmi_iActoMigratorioId = Comun.ToNullInt64(hacmi_iActoMigratorioId.Value);
            oRE_ACTOMIGRATORIO.ACTO.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            oRE_ACTOMIGRATORIO.ACTO.acmi_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();;
            oRE_ACTOMIGRATORIO.ACTO.acmi_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            oRE_ACTOMIGRATORIO.ACTO.acmi_dFechaModificacion = ObtenerFechaActual(HttpContext.Current.Session);
            oRE_ACTOMIGRATORIO.ACTO.HostName = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_HOSTNAME]);

            i_Resultado = new SGAC.Registro.Actuacion.BL.ActoMigratorioMantenimientoBL().Actualizar_Estados(oRE_ACTOMIGRATORIO);

            if (i_Resultado <= 0)
            {
                var StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", Constantes.CONST_MENSAJE_OPERACION_FALLIDA_ESTADO, false, 190, 320);
                Comun.EjecutarScript(Page, StrScript);
            }
            else
            {
                lblEstado.Text = Variar_Estados(oRE_ACTOMIGRATORIO.ACTO.acmi_sEstadoId);
                hId_Estado.Value = oRE_ACTOMIGRATORIO.ACTO.acmi_sEstadoId.ToString();
                btn_solicitar.Attributes.Add("style", "display:none;");

                Session[strVariableAccion] = (int)Enumerador.enmTipoOperacion.CONSULTA;

                if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
                {
                    ctrlAdjunto.BtnGrabActAdj.Enabled = false;
                    ctrlAdjunto.HabilitarControlesAdjunto(false);
                    UpdAdjuntos.Update();
                    ctrlToolBarRegistro.btnGrabar.Enabled = false;
                    updRegPago.Update();
                    Habilitar_Controles(false);
                    updConsulta.Update();
                    txtNumeroPass.Enabled = false;
                    ctrlToolBarActuacion.btnGrabar.Enabled = false;
                    updCaberaFormato.Update();
                }
            }
        }

        protected void ddlVisaSubTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (ddlVisaSubTipo.SelectedItem.Value == "9061")
            {
                check_3.Visible = true;
            }
            else
            {
                check_3.Visible = false;
                ddlTitularFamiliar.SelectedValue = "0";
                ddlTitularFamiliar.Enabled = true;
            }

            switch (Comun.ToNullInt32(ddlVisaSubTipo.SelectedItem.Value))
            {
                case (Int32)Enumerador.enmMigratorioVisaTipoTemporal.TURISTAS:
                case (Int32)Enumerador.enmMigratorioVisaTipoTemporal.NEGOCIOS:
                    txtFecExpiracion.set_Value = txtFecExpedicion.Value().AddYears(1);
                    break;
                default:
                    txtFecExpiracion.set_Value = txtFecExpedicion.Value().AddMonths(6);
                    break;
            }

            DateTime oldDate = txtFecExpedicion.Value();
            DateTime newDate = txtFecExpiracion.Value();

            // Difference in days, hours, and minutes.
            TimeSpan ts = newDate - oldDate;

            // Difference in days.
            int differenceInDays = ts.Days;

            txtTiempoPermanencia.Text = differenceInDays.ToString();

            DataTable dt = null;
            try
            {
                dt = (from obj in ((DataTable)(Session["SubTipoVisa"])).AsEnumerable()
                      where Comun.ToNullInt32(obj["id"]) == Comun.ToNullInt32(ddlVisaSubTipo.SelectedItem.Value)
                      select obj).CopyToDataTable();

                lblSigla.Text = Convert.ToString(dt.Rows[0]["valor"]);
            }
            catch
            {
                lblSigla.Text = "";
            }
            dt = null;

            /*Validando las visas*/

            switch (Comun.ToNullInt32(ddlVisaTipo.SelectedItem.Value))
            {
                case (Int32)Enumerador.enmMigratorioVisaTipo.RESIDENTE:
                    pnlVisa_3.Visible = true;
                    if (Comun.ToNullInt32(ddlVisaSubTipo.SelectedItem.Value) == (Int32)Enumerador.enmMigratorioVisaTipoResidente.PERIODISTA)
                    {
                        VDiplomatica.Attributes.Add("style", "display:none;");
                        vPrensa.Attributes.Remove("style");

                        lblVisaPrensaGlosa.Text = "4. SOLO PARA VISAS DE PRENSA";
                    }
                    else
                    {
                        VDiplomatica.Attributes.Add("style", "display:none;");
                        vPrensa.Attributes.Add("style", "display:none;");

                        lblVisaPrensaGlosa.Text = "5. SOLO PARA VISAS DE PRENSA";
                    }
                    break;
                case (Int32)Enumerador.enmMigratorioVisaTipo.TEMPORAL:
                    pnlVisa_3.Visible = true;
                    switch (Comun.ToNullInt32(ddlVisaSubTipo.SelectedItem.Value))
                    {
                        case (Int32)Enumerador.enmMigratorioVisaTipoTemporal.DIPLOMATICA:
                        case (Int32)Enumerador.enmMigratorioVisaTipoTemporal.FAMILIAR_OFICIAL:
                        case (Int32)Enumerador.enmMigratorioVisaTipoTemporal.INTERCAMBIO:
                        case (Int32)Enumerador.enmMigratorioVisaTipoTemporal.OFICIAL:
                            VDiplomatica.Visible = true;
                            vPrensa.Attributes.Add("style", "display:none;");

                            lblVisaPrensaGlosa.Text = "5. SOLO PARA VISAS DE PRENSA";
                            break;
                        case (Int32)Enumerador.enmMigratorioVisaTipoTemporal.PERIODISTA:
                            VDiplomatica.Attributes.Add("style", "display:none;");
                            vPrensa.Attributes.Remove("style");

                            lblVisaPrensaGlosa.Text = "4. SOLO PARA VISAS DE PRENSA";
                            break;
                        default:
                            pnlVisa_3.Visible = false;
                            VDiplomatica.Attributes.Add("style", "display:none;");
                            vPrensa.Attributes.Add("style", "display:none;");

                            lblVisaPrensaGlosa.Text = "5. SOLO PARA VISAS DE PRENSA";
                            break;
                    }
                    break;
            }

            updConsulta.Update();
            
        }

       [WebMethod(EnableSession=true)]
        public static string ddlFuncionario_SelectedIndexChanged(string hId_TipoActuacion, string ddlFuncionario)
        {
            if (Comun.ToNullInt32(hId_TipoActuacion) == (Int32)Enumerador.enmDocumentoMigratorio.VISAS)
            {
                DataTable dt = (DataTable)HttpContext.Current.Session["Obj_Funcionarios"];
                DataTable dt_existe = null;
                try
                {
                    dt_existe = (from obj_dt in dt.AsEnumerable()
                                 where Comun.ToNullInt32(obj_dt["IFuncionarioId"]) == Comun.ToNullInt32(ddlFuncionario)
                                 select obj_dt).CopyToDataTable();

                    if (dt_existe.Rows.Count > 0)
                        return Convert.ToString(dt_existe.Rows[0]["CARGO"]);
                    else
                        return "";
                }
                catch
                {
                    return "";
                }
            }
            else
                return "";
            
        }

        protected void btn_confirmar_Click(object sender, EventArgs e)
        {
            try
            {
                String NumeroFormato = btn_formato.Text.Trim().Replace("Formato", "");

                #region Datos de Recurrente

                String vNombreRecurrente = string.Empty;
                String vDocumento = string.Empty;


                string strGUID = "";

                if (HFGUID.Value.Length > 0)
                {
                    strGUID = HFGUID.Value;
                }

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
                        vNombreRecurrente += " " + AplicarInicialMayuscula(HttpContext.Current.Session["ApeCasada" + strGUID].ToString());
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
                sContenido.Append(ObtenerDocumentoConformidad(vNombreRecurrente, vDocumento, NumeroFormato));
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
                objetos[0] = vNombreRecurrente;
                objetos[1] = vDocumento;
                objetos[2] = true;

                listObjects.Add(objetos);


                #endregion

                DocumentoiTextSharp.CreateFilePDFConformidad(dtTMPReemplazar, strRutaHtml, strRutaPDF, HttpContext.Current.Server.MapPath("~/Images/Escudo.PNG"), listObjects);

                if (System.IO.File.Exists(strRutaPDF))
                {
                    WebClient User = new WebClient();
                    Byte[] FileBuffer = User.DownloadData(strRutaPDF);
                    if (FileBuffer != null)
                    {
                        Actualizar_Estado_Entregado();
                        
                        HttpContext.Current.Session["binaryData"] = FileBuffer;

                        string strUrl = "../Accesorios/VisorPDF.aspx";
                        string strScript = "window.open('" + strUrl + "', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";
                        Comun.EjecutarScript(Page, strScript);
                        Cargar_Datos_Iniciales();
                        //string strUrl = "../Registro/FrmReporteMigratorioHtml.aspx?vClass=4&FormatoDGC=" + NumeroFormato;
                        //string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=500,height=700,left=(screen.width-500)/2,top=(screen.height-700)/2');";
                        //Comun.EjecutarScript(Page, strScript);
                    }
                }
                #endregion

            }
            catch
            {
            }
        }
        static DataTable CrearTmpTabla()
        {
            DataTable dtTablaTemporal = new DataTable();

            dtTablaTemporal.Columns.Add("strCadenaBuscar", typeof(string));
            dtTablaTemporal.Columns.Add("strCadenaReemplazar", typeof(string));

            return dtTablaTemporal;
        }

        private void Actualizar_Estado_Entregado()
        {
            string script = string.Empty;

            SGAC.BE.MRE.RE_ACTOMIGRATORIO oRE_ACTOMIGRATORIO = new BE.MRE.RE_ACTOMIGRATORIO();

            #region - Actualizando los datos de la lámina  -
            oRE_ACTOMIGRATORIO = new BE.MRE.RE_ACTOMIGRATORIO();
            oRE_ACTOMIGRATORIO.acmi_iActoMigratorioId = Comun.ToNullInt64(this.hacmi_iActoMigratorioId.Value);
            oRE_ACTOMIGRATORIO.acmi_vNumeroLamina = "NO ACTUALIZAR";
            oRE_ACTOMIGRATORIO.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            oRE_ACTOMIGRATORIO.acmi_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP(); ;
            oRE_ACTOMIGRATORIO.acmi_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            oRE_ACTOMIGRATORIO.acmi_dFechaModificacion = new MyBasePage().ObtenerFechaActual(HttpContext.Current.Session);
            oRE_ACTOMIGRATORIO.HostName = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_HOSTNAME]);

            script = new SGAC.Registro.Actuacion.BL.ActoMigratorioMantenimientoBL().Actualizar_Lamina(oRE_ACTOMIGRATORIO).ToString();

            #endregion
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
        private String ObtenerDocumentoConformidad(String vNombre, String vDocumento, String NumeroFormato)
        {

            bool bEsMujer = false;
            

            if (HttpContext.Current.Session["PER_GENERO" + HFGUID.Value].ToString() == Convert.ToInt32(Enumerador.enmGenero.FEMENINO).ToString())
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

            sScript.Append(NumeroFormato);

            sScript.Append(", que he tenido a la vista y me ha sido entregado en la fecha,");
            sScript.Append(" manifestando mi conformidad con su contenido.");
            sScript.Append("</p>");
            sScript.Append("<br />");


            sScript.Append("<p align=\"right\"; style=\"background-color:transparent; font-family:arial;\">");
            DateTime dt_Fecha = Comun.FormatearFecha(Accesorios.Comun.ObtenerFechaActualTexto(HttpContext.Current.Session));
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
        protected void chkAcuerdoAP_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAcuerdoAP.Checked)
            {
                ddlTitularFamiliar.SelectedValue = "0";
                ddlTitularFamiliar.Enabled = false;
            }
            else
            {
                ddlTitularFamiliar.SelectedValue = "0";
                ddlTitularFamiliar.Enabled = true;
            }
        }

        protected void btn_actualza_estado_Click(object sender, EventArgs e)
        {

            lblEstado.Text = Variar_Estados(Comun.ToNullInt32(hId_Estado.Value));
        }

        protected void ddl_TipoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculoxTarifarioxTipoPagoxCantidad();
        }

        private void CalculoxTarifarioxTipoPagoxCantidad()
        {
            int intCantidad = 1;
            string strScript = string.Empty;
            string strDescripcionTarifa = string.Empty;
            double decMontoSC = 0, decTotalSC = 0;
            double decMontoML = 0, decTotalML = 0;

            lbl_monto.Text = Session[Constantes.CONST_SESION_TIPO_MONEDA].ToString();
            lbl_monto_total.Text = Session[Constantes.CONST_SESION_TIPO_MONEDA].ToString();

            // Evalua habilitacion de controles:            
            if (Session[strVariableTarifario] == null)
            {
                //LimpiarDatosTarifaPago();
                return;
            }
            else
            {
                objTarifarioBE = (BE.MRE.SI_TARIFARIO)Session[strVariableTarifario];
                if (objTarifarioBE.tari_sNumero == 0)
                {
                    return;
                }


                // Tarifa de la Actuación:                
                decMontoSC = (double)objTarifarioBE.tari_FCosto;

                // Tarifario:
                if (string.IsNullOrEmpty(txtIdTarifa.Text))
                {
                    return;
                }

                // Montos calculados:
                if (intCantidad > 0)
                {
                    decTotalSC = Tarifario.Calculo(objTarifarioBE, intCantidad);
                    decMontoML = CalculaCostoML(decMontoSC, Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]));
                    decTotalML = CalculaCostoML(decTotalSC, Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]));
                }

                // Asignando valores a los controles:
                string strFormato = ConfigurationManager.AppSettings["FormatoMonto"].ToString();

                txt_montosc.Text = decMontoSC.ToString(strFormato);
                txt_montoml.Text = decMontoML.ToString(strFormato);

                txt_total_sc.Text = decTotalSC.ToString(strFormato);
                txt_total.Text = decTotalML.ToString(strFormato);

                if (Convert.ToInt32(CtrlPago_Visa.iTipoPago) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA) ||
                    Convert.ToInt32(CtrlPago_Visa.iTipoPago) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA) ||
                    Convert.ToInt32(CtrlPago_Visa.iTipoPago) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA))
                {
                    HabilitaDatosPago();
                    CtrlPago_Visa.vMonto = txt_montoml.Text;
                }
                else if (Convert.ToInt32(CtrlPago_Visa.iTipoPago) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.NO_COBRADO) || 
                    Convert.ToInt32(CtrlPago_Visa.iTipoPago) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
                {
                    txt_total_sc.Text = "0.00";
                    txt_total.Text = "0.00";
                }
                else
                {

                }
                // Salvando los valores en sesión:
                Session["intCantidad"] = intCantidad;
                Session["decMontoSC"] = decMontoSC;
            }
        }

        private void HabilitaDatosPago(bool bolHabilitar = true)
        {
            CtrlPago_Visa.bEnabledControl = bolHabilitar;
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

        protected void chk_verificar_CheckedChanged(object sender, EventArgs e)
        {
            ctrlToolBarActuacion.btnGrabar.Enabled = chk_verificar.Checked;
            ctrlToolBarActuacion.btnEditar.Enabled = !chk_verificar.Checked;
        }

        protected void ddlAutorizado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAutorizado.SelectedIndex > 0)
            {
                if (Convert.ToInt32(ddlAutorizado.SelectedValue) == (int)Enumerador.enmVisaAutorizacion.MISION_CONSULAR)
                {
                    txtRREEDoc.Enabled = true;
                    txtRREENum.Enabled = true;

                    txtDIGEMINDoc.Enabled = true;
                    txtDIGEMINNum.Enabled = true;

                    txtRREEFecha.EnabledText = true;
                    txtDIGEMINFecha.EnabledText = true;
                }
                else
                {
                    txtRREEDoc.SelectedValue = "0";
                    txtRREENum.Text = string.Empty;
                    txtRREEFecha.Text = string.Empty;
                    txtDIGEMINDoc.SelectedValue = "0";
                    txtDIGEMINNum.Text = string.Empty;
                    txtDIGEMINFecha.Text = string.Empty;

                    txtRREEDoc.Enabled = false;
                    txtRREENum.Enabled = false;

                    txtDIGEMINDoc.Enabled = false;
                    txtDIGEMINNum.Enabled = false;
                    txtRREEFecha.EnabledText = false;
                    txtDIGEMINFecha.EnabledText = false;
                }
            }
            else
            {
                txtRREEDoc.SelectedValue = "0";
                txtRREENum.Text = string.Empty;
                txtRREEFecha.Text = string.Empty;
                txtDIGEMINDoc.SelectedValue = "0";
                txtDIGEMINNum.Text = string.Empty;
                txtDIGEMINFecha.Text = string.Empty;

                txtRREEDoc.Enabled = false;
                txtRREENum.Enabled = false;

                txtDIGEMINDoc.Enabled = false;
                txtDIGEMINNum.Enabled = false;

                txtRREEFecha.EnabledText = false;
                txtDIGEMINFecha.EnabledText = false;
            }
        }

        protected void ddlDomicilioPeruDepa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDomicilioPeruDepa.SelectedIndex > 0)
                {
                    Util.CargarDropDownList(ddlDomicilioPeruProv,
                        Comun.ObtenerPaisProv(Session, ddlDomicilioPeruDepa.SelectedValue),
                            "ubge_vProvincia", "ubge_cUbi02", true);

                    lblCO_ddlDomicilioPeruProv.Visible = true;
                    lblCO_ddlDomicilioPeruDist.Visible = true;
                    ScriptManager.GetCurrent(Page).SetFocus(ddlDomicilioPeruDepa.ClientID);
                    Util.CargarParametroDropDownList(ddlDomicilioPeruDist, new DataTable(), true);
                }
                else
                {
                    Util.CargarParametroDropDownList(ddlDomicilioPeruProv, new DataTable(), true);
                    Util.CargarParametroDropDownList(ddlDomicilioPeruDist, new DataTable(), true);
                    lblCO_ddlDomicilioPeruProv.Visible = false;
                    lblCO_ddlDomicilioPeruDist.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Session["_LastException"] = ex;
                Response.Redirect("../PageError/GenericErrorPage.aspx");
            }
        }

        protected void ddl_amfr_sOficinaConsularId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_amfr_sOficinaConsularId.SelectedIndex > 0)
            {
                ddl_amfr_sOficinaMigracionId.SelectedIndex = 0;
            }
        }

        protected void ddl_amfr_sOficinaMigracionId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_amfr_sOficinaMigracionId.SelectedIndex > 0)
            {
                ddl_amfr_sOficinaConsularId.SelectedIndex = 0;
            }
        }
        
        protected void btnDesabilitarAutoahesivo_Click(object sender, EventArgs e)
        {
            btnVistaPrev.Enabled = false;
            chk_PrintAutoadhesivo.Checked = true;
            chk_PrintAutoadhesivo.Enabled = false;
            txtCodAutoadhesivo.Enabled = false;
            hdn_ImpresionCorrecta.Value = "1";
            BindGridActuacionesInsumoDetalle(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]));
            updVinculacion.Update();
            
        }

        protected void btnDesabilitarLamina_Click(object sender, EventArgs e)
        {
           
            //switch (Comun.ToNullInt32(hId_TipoActuacion.Value))
            //{
            //    case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                  

            //        break;
            //    case (Int32)Enumerador.enmDocumentoMigratorio.PASAPORTE:
            //    case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
            //        btn_formato.Enabled = true;
            //        chk_passaporte.Checked = true;
            //        chk_passaporte.Enabled = false;
            //        txtCodPassaporte.Enabled = false;

            //        break;
            //}

            //btn_Lamina.Enabled = true;
            //chk_PintLamina.Checked = true;
            //chk_PintLamina.Enabled = false;
            //txtCodLamina.Enabled = false;
            //hdn_ImpresionCorrecta.Value = "1";
            BindGridActuacionesInsumoDetalle(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]));
            updVinculacion.Update();
            
        }

        protected void ddlTipoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA) ||
                    Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA) ||
                    Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA))
            {
                HabilitaDatosPago();
                pnlPagLima.Visible = true;
                txtMtoCancelado.Text = txtTotalML.Text;
                if (Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA))
                {
                    txtTotalML.Text = "0.00";
                }
            }
            else if (Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.NO_COBRADO) ||
                Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
            {
                pnlPagLima.Visible = false;
                txtTotalSC.Text = "0.00";
                txtTotalML.Text = "0.00";
            }
            else
            {
                pnlPagLima.Visible = false;
                string strFormato = ConfigurationManager.AppSettings["FormatoMonto"].ToString();
                txtTotalSC.Text = (Convert.ToDouble(txtCantidad.Text) * Convert.ToDouble(txtMontoSC.Text)).ToString(strFormato);
                txtTotalML.Text = (Convert.ToDouble(txtCantidad.Text) * Convert.ToDouble(txtMontoML.Text)).ToString(strFormato);
            }
            txtMtoCancelado.Enabled = false; // Caso: Acto Migratorio

            //----------------------------------------//
            // Autor: Jonatan Silva Cachay
            // Fecha: 14-02-2017
            // Objetivo: Verificar Enabled
            //----------------------------------------//
            if (Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA) ||
            Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.NO_COBRADO) ||
            Convert.ToInt32(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.GRATIS) ||
            Convert.ToInt32(hTipoPago.Value) == Convert.ToInt32(ddlTipoPago.Items.FindByText("PAGO ARUBA").Value) ||
            Convert.ToInt32(hTipoPago.Value) == Convert.ToInt32(ddlTipoPago.Items.FindByText("PAGO OTRAS ISLAS CARIBEÑAS").Value))
            {
                if (txtCodAutoadhesivo.Text.Length > 0 && txtCodAutoadhesivo.Enabled == false)
                {
                    ctrlToolBarRegistro.btnGrabar.Enabled = false;
                }
                else { ctrlToolBarRegistro.btnGrabar.Enabled = true; }
            }
            else { ctrlToolBarRegistro.btnGrabar.Enabled = true; }

            if (CalcularTarifarioEspecial(Convert.ToDouble(txtCantidad.Text)) != 0)
            {

                double decTotalSC = CalcularTarifarioEspecial(Convert.ToDouble(txtCantidad.Text));
                double decTotalML = CalculaCostoML(decTotalSC, Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]));

                string strFormato = ConfigurationManager.AppSettings["FormatoMonto"].ToString();

                txtTotalSC.Text = decTotalSC.ToString(strFormato);
                txtTotalML.Text = decTotalML.ToString(strFormato);
            }
        }
       
    }

}
