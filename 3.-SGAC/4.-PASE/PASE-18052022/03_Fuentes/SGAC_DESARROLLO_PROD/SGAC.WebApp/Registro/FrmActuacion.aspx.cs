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
using SGAC.Registro.Actuacion.BL;
using System.Configuration;
using SGAC.Controlador;
using SGAC.Registro.Persona.BL;
using SGAC.Configuracion.Sistema.BL;
using System.Web.Services;
using Microsoft.Security.Application;
using System.Web.Configuration;

namespace SGAC.WebApp.Registro
{
    public partial class FrmActuacion : MyBasePage
    {
        #region Campos
        private string strNombreEntidad = "ACTUACIÓN";
        private string strVariableAccion = "Actuacion_Accion";
        private DataTable dtTarifarioFiltrado;
        private BE.MRE.SI_TARIFARIO objTarifarioBE;
        private string strSessionIndice = "ACT_INDICE_SEL";
        private string strVariableTarifario = "objTarifarioBE";
        #endregion
          

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlToolBarRegistroActuacion.VisibleIButtonGrabar = true;
            ctrlToolBarRegistroActuacion.VisibleIButtonCancelar = true;

            ctrlToolBarRegistroActuacion.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonGrabarClick(ctrlToolBarRegistroActuacion_btnGrabarHandler);
            ctrlToolBarRegistroActuacion.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonCancelarClick(ctrlToolBarRegistroActuacion_btnCancelarHandler);

            Button BtnGrabar = (Button)ctrlToolBarRegistroActuacion.FindControl("btnGrabar");
            BtnGrabar.OnClientClick = "return ValidarRegistroActuacion();";

            ctrFecPago.EndDate = DateTime.Today;
            

            if (!Page.IsPostBack)
            {
                //------------------------------------------------------------
                //Autor: Miguel Márquez Beltrán
                //Fecha: 28/08/2018
                //Objetivo: Guardar el parametro GUID en un control Hidden
                //------------------------------------------------------------
                string codPersona = "0";
                string codTipoDoc = "";
                string codNroDocumento = "";
                if (Request.QueryString["CodPer"] != null)
                {
                    codPersona = Util.DesEncriptar(Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString()));

                    if (Convert.ToInt64(codPersona) > 0)
                    {
                        //------------------------------------------------------
                        //Fecha: 19/10/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Obtener el tipo y numero de documento
                        //------------------------------------------------------

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
                        if (ViewState["NroDoc"] == null)
                        {
                            if (ViewState["NroDoc"].ToString() == "")
                            {
                                Response.Redirect("~/Default.aspx", false);
                            }
                        }
                    }
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Formulario", "alert('Por favor evitar abrir 2 pestañas, o copiar el formulario');", true);
                    Response.Redirect("../Default.aspx", false);
                    return;
                }

               

                HF_PAGADO_EN_LIMA.Value = Convert.ToString((int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA);
                
                if (Session["strBusqueda"] != null)
                {
                    Session.Remove("strBusqueda");
                }

               
                if (Convert.ToInt64(codPersona) > 0)
                {
                    hid_iRecurrenteId.Value = codPersona;
                    //GetDataPersona(Convert.ToInt64(codPersona));
                    //if (ViewState["NroDoc"] == null)
                    //{
                    //    if (ViewState["NroDoc"].ToString() == "")
                    //    {
                    //        Response.Redirect("~/Default.aspx", false);
                    //    }
                    //}
                }
                else
                {
                    string StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTUACIÓN", "Por problemas de conexión, por favor realice nuevamente el proceso de busqueda y registro de trámite", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }

                  
                

                ObtenerSaldoAutoadhesivo();
                CargarDatosIniciales();
                CargarListadosDesplegables();

                txtIdTarifa.Focus();
                GetValoresEspeciales();
                
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
                // Fecha: 27-10-2016
                // Objetivo: Ocultar controles de exoneración
                //----------------------------------------//
                lblExoneracion.Visible = false;
                ddlExoneracion.Visible = false;
                //----------------------------------------//
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 05-12-2018
                // Objetivo: Ocultar controles de Sustento
                //----------------------------------------//
                lblSustentoTipoPago.Visible = false;
                txtSustentoTipoPago.Visible = false;
                lblValSustentoTipoPago.Visible = false;
                RBNormativa.Visible = false;
                RBSustentoTipoPago.Visible = false;
                lblValExoneracion.Visible = false;
                //----------------------------------------//
                
            }
        }

        void ctrlToolBarRegistroActuacion_btnCancelarHandler()
        {
            //if (HFGUID.Value.Length > 0)
            //{
            //    Response.Redirect("~/Registro/FrmTramite.aspx?GUID=" + HFGUID.Value);
            //}
            //else
            //{
            string codPersona = "";
            codPersona =  Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString());

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
        private bool validarTipoCambio()
        {
            bool respuesta = true;
            if (Session[Constantes.CONST_SESION_TIPO_CAMBIO_BANCARIO] == null)
            {
                respuesta = false;
            }
            if (Session[Constantes.CONST_SESION_TIPO_CAMBIO] == null)
            {
                respuesta = false;
            }

            double pago_FTipCambioBancario = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO_BANCARIO]);
            double pago_FTipCambioConsular = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]);

            if (pago_FTipCambioBancario == 0)
            {
                respuesta = false;
            }
            if (pago_FTipCambioConsular == 0)
            {
                respuesta = false;
            }
            return respuesta;
        }

        void ctrlToolBarRegistroActuacion_btnGrabarHandler()
        {
            if (!validarTipoCambio())
            {
                string StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", "Registre el tipo de cambio");
                Comun.EjecutarScript(Page, StrScript);
                return;
            }
            #region Validación Tarifa 1 Por edad
            if (!ValidaTarifa1())
            {
                Registrar();
                //lblTexto.Text = lblTexto.Text;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "modal", "Popup();", true);
            }
            #endregion
            //Registrar();
        }
        protected void btnSIDeclarante_Click(object sender, EventArgs e)
        {
            Registrar(true);
        }

        protected void btnNODeclarante_Click(object sender, EventArgs e)
        {
            Registrar(false);
        }
        private void Registrar(bool ResupuestaTarifa1 =false)
        {
            #region Validación Tarifas Únicas
            if (ExistenMasTarifa())
                return;
            #endregion

            if (ResupuestaTarifa1 == false)
            {
                #region Validación Tarifa 1 Por edad
                if (ValidaTarifa1())
                    return;
                #endregion
            }

            BE.RE_PAGO ObjPagoBE = new BE.RE_PAGO();
            BE.MRE.SI_TARIFARIO objTarifarioBE = new BE.MRE.SI_TARIFARIO();
            BE.RE_ACTUACION ObjActuacBE = new BE.RE_ACTUACION();
            BE.RE_ACTUACIONDETALLE ObjActuacDetBE = new BE.RE_ACTUACIONDETALLE();
            BE.RE_PERSONA objPersona = new BE.RE_PERSONA();

            long LonActuacionId1 = 0;
            long LonActuacionId2 = 0;
            string StrScript = string.Empty;
            int IntRpta = 0;
            DataTable dt = new DataTable();
            //Convert.ToInt64(hid_iRecurrenteId.Value);

            //if (Convert.ToInt32(Session["iTipoId" + HFGUID.Value].ToString()) == (int)Enumerador.enmTipoPersona.NATURAL)
            //{
            if (Convert.ToInt32(ViewState["iTipoId"].ToString()) == (int)Enumerador.enmTipoPersona.NATURAL)
            {
                PersonaConsultaBL oPersonaBL = new PersonaConsultaBL();
                
                dt = oPersonaBL.PersonaGetById(Convert.ToInt64(hid_iRecurrenteId.Value));
                Boolean bFallecidoFlag = false;
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["pers_bFallecidoFlag"] != null)
                        {
                            bFallecidoFlag = Convert.ToBoolean(dt.Rows[0]["pers_bFallecidoFlag"]);
                        }
                    }
                }
                StrScript = string.Empty;
                IntRpta = 0;

                if (bFallecidoFlag)
                {

                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", Constantes.CONST_MENSAJE_ACTUACION_PERSONA_FALLECIDA, false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }
            }
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
            //    DataTable _dtVerifica = new DataTable();
            //    ActuacionPagoConsultaBL _obj = new ActuacionPagoConsultaBL();
            //    _dtVerifica = _obj.verificarOperacion(null, Convert.ToInt16(ddlNomBanco.SelectedValue), txtNroOperacion.Text, Convert.ToInt16(ddlTipoPago.SelectedValue), Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), Comun.FormatearFecha(ctrFecPago.Text));

            //    if (_dtVerifica.Rows.Count > 0)
            //    {
            //        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", _dtVerifica.Rows[0]["Mensaje"].ToString(), false, 190, 250);
            //        Comun.EjecutarScript(Page, StrScript);
            //        return;
            //    }
            //}

            Session[strSessionIndice] = 0;
            Session["Actuacion_Accion"] = (int)Enumerador.enmTipoOperacion.REGISTRO;
            Session["ActoCivil_Accion"] = (int)Enumerador.enmTipoOperacion.REGISTRO;
            Proceso MiProc = new Proceso();
            if (Convert.ToBoolean(Session["NuevoRegistro"]) == true)
            {
                if (Session[strVariableTarifario] != null)
                {

                    objTarifarioBE = (BE.MRE.SI_TARIFARIO)Session[strVariableTarifario];

                    bool bNoCobrado = ExisteInafecto_Exoneracion(ddlTipoPago.SelectedValue);

                    if (bNoCobrado ||
                        Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
                    {
                        objTarifarioBE.tari_FCosto = 0;
                    }
                }
                else
                {
                    throw new DataException("Faltan parámetros para guardar datos!");
                }

                #region Validación Tarifa Judicial y Notarial

                int intFlujoGeneral = 0;
                object objFlujoGeneral = ConfigurationManager.AppSettings["FlujoActoGeneral"];
                if (objFlujoGeneral != null)
                    intFlujoGeneral = Comun.ToNullInt32(objFlujoGeneral);
                if (intFlujoGeneral == 0)
                {
                    if (objTarifarioBE.tari_sSeccionId == (int)Enumerador.enmSeccion.ACTO_JUDICIAL)
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones",
                                                                   Constantes.CONST_MENSAJE_VALIDA_ACTO_JUDICIAL);
                        Comun.EjecutarScript(Page, StrScript);
                        return;
                    }
                    else if (objTarifarioBE.tari_sSeccionId == (int)Enumerador.enmSeccion.ACTO_NOTARIAL)
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones",
                                                                                       Constantes.CONST_MENSAJE_VALIDA_ACTO_NOTARIAL);
                        Comun.EjecutarScript(Page, StrScript);
                        return;
                    }
                }
                #endregion

                #region Validación por Tipo de Persona (Natural y Jurídica) y Extranjero

                int intTipoPersona = 0;

                //if (HFGUID.Value.Length > 0)
                //{
                //    intTipoPersona = Comun.ToNullInt32(Session["iTipoId" + HFGUID.Value]);
                //}
                //else
                //{
                //    intTipoPersona = Comun.ToNullInt32(Session["iTipoId"]);
                //}
                intTipoPersona = Comun.ToNullInt32(ViewState["iTipoId"]);
                
                
                if (objTarifarioBE.tari_sSeccionId == (int)Enumerador.enmSeccion.ACTO_CIVIL && objTarifarioBE.tari_sNumero != 2)
                {
                    if (intTipoPersona == (int)Enumerador.enmTipoPersona.JURIDICA)
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones",
                                                       Constantes.CONST_MENSAJE_VALIDA_TIPO_PERSONA);
                        Comun.EjecutarScript(Page, StrScript);
                        return;
                    }
                }
                else if (objTarifarioBE.tari_sSeccionId == (int)Enumerador.enmSeccion.REGISTRO_MILITAR)
                {
                    int intPerNacinoalidad = 0;

                    //if (HFGUID.Value.Length > 0)
                    //{
                    //    intPerNacinoalidad = Comun.ToNullInt32(ViewState["PER_NACIONALIDAD" + HFGUID.Value]);
                    //}
                    //else
                    //{
                    //    intPerNacinoalidad = Comun.ToNullInt32(ViewState["PER_NACIONALIDAD"]);
                    //}
                    intPerNacinoalidad = Comun.ToNullInt32(ViewState["PER_NACIONALIDAD"]);
                    if (intTipoPersona == (int)Enumerador.enmTipoPersona.JURIDICA)
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones",
                                                           Constantes.CONST_MENSAJE_VALIDA_TIPO_PERSONA);
                        Comun.EjecutarScript(Page, StrScript);
                        return;
                    }
                    else if (intPerNacinoalidad == (int)Enumerador.enmNacionalidad.EXTRANJERA)
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones",
                                                       Constantes.CONST_MENSAJE_TARIFA_SOLO_PERUANO);
                        Comun.EjecutarScript(Page, StrScript);
                        return;
                    }
                }
                else if (objTarifarioBE.tari_sSeccionId == (int)Enumerador.enmSeccion.PASAPORTE_SALVOCONDUCTO ||
                    objTarifarioBE.tari_sSeccionId == (int)Enumerador.enmSeccion.VISA)
                {
                    if (intTipoPersona == (int)Enumerador.enmTipoPersona.JURIDICA)
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones",
                                                           Constantes.CONST_MENSAJE_VALIDA_TIPO_PERSONA);
                        Comun.EjecutarScript(Page, StrScript);
                        return;
                    }
                    else if (objTarifarioBE.tari_sSeccionId == (int)Enumerador.enmSeccion.PASAPORTE_SALVOCONDUCTO)
                    {
                        //if (HFGUID.Value.Length > 0)
                        //{
                        //    if (Comun.ToNullInt32(Session[Constantes.CONST_SESION_NACIONALIDAD_ID + HFGUID.Value]) == (int)Enumerador.enmNacionalidad.EXTRANJERA && Convert.ToBoolean(dt.Rows[0]["TienePadrePeruano"]) == false)
                        //    {
                        //        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", Constantes.CONST_MENSAJE_TARIFA_SOLO_PERUANO);
                        //        Comun.EjecutarScript(Page, StrScript);
                        //        return;
                        //    }                           
                        //}
                        //else
                        //{
                        if (Comun.ToNullInt32(ViewState["PER_NACIONALIDAD"]) == (int)Enumerador.enmNacionalidad.EXTRANJERA && Convert.ToBoolean(dt.Rows[0]["TienePadrePeruano"]) == false)
                            {
                                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", Constantes.CONST_MENSAJE_TARIFA_SOLO_PERUANO);
                                Comun.EjecutarScript(Page, StrScript);
                                return;
                            }   
                        //}
                    }
                    else if (objTarifarioBE.tari_sSeccionId == (int)Enumerador.enmSeccion.VISA)
                    {
                        //if (HFGUID.Value.Length > 0)
                        //{
                        //    if (Comun.ToNullInt32(Session[Constantes.CONST_SESION_NACIONALIDAD_ID + HFGUID.Value]) == (int)Enumerador.enmNacionalidad.PERUANA)
                        //    {
                        //        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", Constantes.CONST_MENSAJE_TARIFA_SOLO_EXTRANJERO);
                        //        Comun.EjecutarScript(Page, StrScript);
                        //        return;
                        //    }
                        //}
                        //else
                        //{
                            if (Comun.ToNullInt32(ViewState["PER_NACIONALIDAD"]) == (int)Enumerador.enmNacionalidad.PERUANA)
                            {
                                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", Constantes.CONST_MENSAJE_TARIFA_SOLO_EXTRANJERO);
                                Comun.EjecutarScript(Page, StrScript);
                                return;
                            }
                        //}
                    }
                }
                #endregion

                #region Validación de tarifa 76 - Solo extranjeros
                // si tarifa es solo para nacionales o extranjeros validar por nacionalidad

                //if (HFGUID.Value.Length > 0)
                //{
                //    if (objTarifarioBE.tari_sNumero == Constantes.CONST_TARIFA_SOLO_EXTRANJERO_76
                //        && Comun.ToNullInt32(Session[Constantes.CONST_SESION_NACIONALIDAD_ID + HFGUID.Value]) == (int)Enumerador.enmNacionalidad.PERUANA)
                //    {
                //        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones",
                //                                           Constantes.CONST_MENSAJE_TARIFA_SOLO_EXTRANJERO);
                //        Comun.EjecutarScript(Page, StrScript);
                //        return;
                //    }
                //}
                //else
                //{
                    if (objTarifarioBE.tari_sNumero == Constantes.CONST_TARIFA_SOLO_EXTRANJERO_76
                        && Comun.ToNullInt32(ViewState["PER_NACIONALIDAD"]) == (int)Enumerador.enmNacionalidad.PERUANA)
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones",
                                                           Constantes.CONST_MENSAJE_TARIFA_SOLO_EXTRANJERO);
                        Comun.EjecutarScript(Page, StrScript);
                        return;
                    }
                //}

                #endregion

                #region AGREGAR
                ObjActuacBE.actu_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                ObjActuacBE.actu_FCantidad = Comun.ToNullInt32(txtCantidad.Text);
                if (intTipoPersona == (int)Enumerador.enmTipoPersona.JURIDICA)
                {
                    ObjActuacBE.actu_iEmpresaRecurrenteId = Convert.ToInt64(hid_iRecurrenteId.Value);
                    ObjActuacBE.actu_iPersonaRecurrenteId = 0;
                }
                else
                {
                    ObjActuacBE.actu_iEmpresaRecurrenteId = 0;
                    ObjActuacBE.actu_iPersonaRecurrenteId = Convert.ToInt64(hid_iRecurrenteId.Value);
                }
                ObjActuacBE.actu_IFuncionarioId = null;
                ObjActuacBE.actu_dFechaRegistro = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                ObjActuacBE.actu_sEstado = (int)Enumerador.enmActuacionEstado.REGISTRADO;
                ObjActuacBE.actu_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                ObjActuacBE.actu_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                if (Session[Constantes.CONST_SESION_CIUDAD_ITINERANTE].ToString() != "")
                {
                    ObjActuacBE.actu_sCiudadItinerante = Convert.ToInt16(Session[Constantes.CONST_SESION_CIUDAD_CODIGO_ITINERANTE].ToString());
                }

                ((DataTable)Session["DtDetActuaciones" + HFGUID.Value]).Clear();

                // Actuacion Detalle                                             
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
                        rowDetAct["vNotas"] = txtObservaciones.Text.ToUpper().Replace("'", "''");
                        //--------------------------------------------
                        // Creador por: Miguel Angel Márquez Beltrán
                        // Fecha: 15-08-2016
                        // Objetivo: Adicionar la columna Clasificacion
                        // Referencia: Requerimiento No.001_2.doc
                        //--------------------------------------------
                        if (ddlClasificacion.SelectedIndex > 0 && ddlClasificacion.Visible == true)
                        {
                            rowDetAct["sClasificacionTarifaId"] = ddlClasificacion.SelectedValue;
                        }
                        else
                        {
                            rowDetAct["sClasificacionTarifaId"] = DBNull.Value;
                        }
                        //--------------------------------------------
                        // Creador por: Miguel Angel Márquez Beltrán
                        // Fecha: 27-10-2016
                        // Objetivo: Adicionar la columna NormaTarifario
                        //--------------------------------------------
                        rowDetAct["iNormaTarifarioId"] = DBNull.Value;


                        if (ddlExoneracion.Visible == true && ddlExoneracion.Enabled == true && ddlExoneracion.SelectedIndex > 0)
                        {
                            rowDetAct["iNormaTarifarioId"] = ddlExoneracion.SelectedValue;
                        }
                       

                        //--------------------------------------------

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
                    rowDetAct["vNotas"] = txtObservaciones.Text.ToUpper().Replace("'", "''");
                    //--------------------------------------------
                    // Creador por: Miguel Angel Márquez Beltrán
                    // Fecha: 15-08-2016
                    // Objetivo: Adicionar la columna Clasificacion
                    // Referencia: Requerimiento No.001_2.doc
                    //--------------------------------------------
                    if (ddlClasificacion.SelectedIndex > 0 && ddlClasificacion.Visible == true)
                    {
                        rowDetAct["sClasificacionTarifaId"] = ddlClasificacion.SelectedValue;
                    }
                    else
                    {
                        rowDetAct["sClasificacionTarifaId"] = DBNull.Value;
                    }
                    //--------------------------------------------
                    // Creador por: Miguel Angel Márquez Beltrán
                    // Fecha: 27-10-2016
                    // Objetivo: Adicionar la columna NormaTarifario
                    //--------------------------------------------
                    if (ddlExoneracion.SelectedIndex > 0 && ddlExoneracion.Visible == true)
                    {
                        rowDetAct["iNormaTarifarioId"] = ddlExoneracion.SelectedValue;
                    }
                    else
                    {
                        rowDetAct["iNormaTarifarioId"] = DBNull.Value;
                    }
                    //--------------------------------------------
                    ((DataTable)Session["DtDetActuaciones" + HFGUID.Value]).Rows.Add(rowDetAct);
                }

                #region Cargar Datos Pago Actuación

                ObjPagoBE.pago_sMonedaLocalId = Comun.ObtenerMonedaLocalId(Session, ddlTipoPago.SelectedValue, txtIdTarifa.Text);


                ObjPagoBE.pago_sPagoTipoId = Convert.ToInt16(ddlTipoPago.SelectedValue);


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


                if (Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA) ||
                    Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA) ||
                    Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA))
                {                    

                    if (ddlNomBanco.SelectedIndex > 0)
                        ObjPagoBE.pago_sBancoId = Convert.ToInt16(ddlNomBanco.SelectedValue);

                    ObjPagoBE.pago_vBancoNumeroOperacion = txtNroOperacion.Text.Trim().ToUpper();
                    ObjPagoBE.pago_dFechaOperacion = Comun.FormatearFecha(ctrFecPago.Text);

                    if (txtMtoCancelado.Text.Length > 0)
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
                        }
                        else
                        {
                            StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", "El formato del monto de pago es incorrecto.", false, 190, 250);
                            Comun.EjecutarScript(Page, StrScript);
                            return;
                        }
                    }
                    else
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", "No ha colocado el monto de pago.", false, 190, 250);
                        Comun.EjecutarScript(Page, StrScript);
                        return;
                    }
                }
                else
                {
                    ObjPagoBE.pago_vBancoNumeroOperacion = "";

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

                    bool bNoCobrado = ExisteInafecto_Exoneracion(ObjPagoBE.pago_sPagoTipoId.ToString());

                    if (bNoCobrado)
                    {
                        ObjPagoBE.pago_FMontoMonedaLocal = 0;
                        ObjPagoBE.pago_FMontoSolesConsulares = 0;
                    }
                }
                ObjPagoBE.pago_FTipCambioBancario = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO_BANCARIO]);
                ObjPagoBE.pago_FTipCambioConsular = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]);

                bool bNoCobrado2 = ExisteInafecto_Exoneracion(ddlTipoPago.SelectedValue);

                if (bNoCobrado2 ||
                    Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
                {
                    ObjPagoBE.pago_FMontoMonedaLocal = 0;
                    ObjPagoBE.pago_FMontoSolesConsulares = 0;
                }

                ObjPagoBE.pago_bPagadoFlag = false;
                ObjPagoBE.pago_vComentario = "";
                
                #endregion

                //object[] miArray;
                //object[] miArray = { ObjActuacBE,
                //                     (DataTable)Session["DtDetActuaciones"],                                                  
                //                     ObjPagoBE,
                //                     LonActuacionId1 };
                DataTable dt1 = (DataTable)Session["DtDetActuaciones" + HFGUID.Value];

                //IntRpta = (int)MiProc.Invocar(ref miArray, "SGAC.BE.RE_ACTUACION", Enumerador.enmAccion.INSERTAR);

                ActuacionMantenimientoBL obj = new ActuacionMantenimientoBL();
                IntRpta = obj.Insertar(ObjActuacBE, dt1, ObjPagoBE, ref LonActuacionId1);

                if (IntRpta < 0)
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", Constantes.CONST_MENSAJE_OPERACION_FALLIDA, false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }

                //-----------------------------------------------------
                // Fecha: 08/07/2019
                //Autor: Se asigna el ID de la Actuación detalle a 
                //      la sesión.
                //-----------------------------------------------------

                DataTable dtActuacionDetalleConsulta = new DataTable();
                ActuacionConsultaBL objActuacionConsultaBL = new ActuacionConsultaBL();
                dtActuacionDetalleConsulta = objActuacionConsultaBL.ActuacionDetalleObtener(LonActuacionId1, 0);
                if (dtActuacionDetalleConsulta.Rows.Count > 0)
                {
                    long intActuacionDetalleID = Convert.ToInt64(dtActuacionDetalleConsulta.Rows[0]["iActuacionDetalleId"].ToString());
                    Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value] = intActuacionDetalleID;
                    Session[Constantes.CONST_SESION_ACTUACIONDET_ID] = intActuacionDetalleID;                 
                }
                else
                {                   
                    Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value] = "0";
                   
                    Session[Constantes.CONST_SESION_ACTUACIONDET_ID] = "0";
                   
                }
                //-----------------------------------------------------

                
                gdvActuacionesGeneradas.Focus();

                //LonActuacionId1 = Convert.ToInt64(miArray[3]);

                Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value] = LonActuacionId1; // AUTOADHESIVO

                #region Tarifa 1 y 3a
                /*Si la tarifa la numero 1 entonces se registra la tarifa 3A*/
                if (objTarifarioBE.tari_sTarifarioId == Convert.ToInt16(Constantes.CONST_EXCEPCION_TARIFA_ID_1))
                {
                    ObjActuacBE.actu_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    ObjActuacBE.actu_FCantidad = 1;
                    ObjActuacBE.actu_iPersonaRecurrenteId = Convert.ToInt64(hid_iRecurrenteId.Value);
                    ObjActuacBE.actu_IFuncionarioId = null;
                    ObjActuacBE.actu_dFechaRegistro = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                    ObjActuacBE.actu_sEstado = (int)Enumerador.enmActuacionEstado.REGISTRADO;
                    ObjActuacBE.actu_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    ObjActuacBE.actu_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                    ((DataTable)Session["DtDetActuaciones" + HFGUID.Value]).Clear();
                    DataRow rowDetAct;

                    rowDetAct = ((DataTable)Session["DtDetActuaciones" + HFGUID.Value]).NewRow();
                    rowDetAct["sTarifarioId"] = Constantes.CONST_EXCEPCION_TARIFA_ID_5;
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
                    rowDetAct["vNotas"] = txtObservaciones.Text.ToUpper().Replace("'", "''");
                    //--------------------------------------------
                    // Creador por: Miguel Angel Márquez Beltrán
                    // Fecha: 15-08-2016
                    // Objetivo: Adicionar la columna Clasificacion
                    // Referencia: Requerimiento No.001_2.doc
                    //--------------------------------------------
                    rowDetAct["sClasificacionTarifaId"] = DBNull.Value;
                    //--------------------------------------------                    
                    // Creador por: Miguel Angel Márquez Beltrán
                    // Fecha: 27-10-2016
                    // Objetivo: Adicionar la columna NormaTarifario
                    //--------------------------------------------
                    intNormaTarifarioId = 0;
                    if (ddlExoneracion.Visible == true)
                    {
                        intNormaTarifarioId = Convert.ToInt64(ddlExoneracion.SelectedValue);
                    }
                    rowDetAct["iNormaTarifarioId"] = intNormaTarifarioId;

                    ((DataTable)Session["DtDetActuaciones" + HFGUID.Value]).Rows.Add(rowDetAct);

                    /***************************************************************************************/
                    /*****************Agregamos el pago de la actuacion************************************/
                    /*************************************************************************************/
                    ObjPagoBE.pago_sPagoTipoId = Convert.ToInt16(ddlTipoPago.SelectedValue);
                    ObjPagoBE.pago_iNormaTarifarioId = intNormaTarifarioId;
                    //-----------------------------------------------------------//
                    // Autor: Miguel Angel Márquez Beltrán
                    // Fecha: 19-10-2016
                    // Objetivo: Asignar la moneda estadounidense para los tipos 
                    //           de pago: pagados en lima en los consulados
                    //-----------------------------------------------------------//
                    ObjPagoBE.pago_sMonedaLocalId = Comun.ObtenerMonedaLocalId(Session, ddlTipoPago.SelectedValue, txtIdTarifa.Text);
                    //-----------------------------------------------------------//

                    if (ddlNomBanco.SelectedIndex > 0)
                    {
                        ObjPagoBE.pago_sBancoId = Convert.ToInt16(ddlNomBanco.SelectedValue);
                    }

                    ObjPagoBE.pago_vBancoNumeroOperacion = "";
                    ObjPagoBE.pago_FTipCambioBancario = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO_BANCARIO]);
                    ObjPagoBE.pago_FTipCambioConsular = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]);
                    ObjPagoBE.pago_dFechaOperacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                    ObjPagoBE.pago_FMontoMonedaLocal = 0;
                    ObjPagoBE.pago_FMontoSolesConsulares = 0;

                    ObjPagoBE.pago_bPagadoFlag = false;
                    ObjPagoBE.pago_vComentario = "";

                    //miArray = new Object[4] { ObjActuacBE, (DataTable)Session["DtDetActuaciones"],                                                  
                    //                          ObjPagoBE, LonActuacionId2 };

                    //IntRpta = (int)MiProc.Invocar(ref miArray, "SGAC.BE.RE_ACTUACION", Enumerador.enmAccion.INSERTAR);

                    IntRpta = obj.Insertar(ObjActuacBE, (DataTable)Session["DtDetActuaciones" + HFGUID.Value], ObjPagoBE, ref LonActuacionId2);

                    //LonActuacionId2 = Convert.ToInt64(miArray[3]);

                }
                #endregion

                #endregion
            }
            else
            {
                #region EDITAR
                /****************************************************************************************************/
                /*****************************Agregamos la cabecera de la actuación**********************************/
                /****************************************************************************************************/
                ObjActuacDetBE.acde_iActuacionDetalleId = Convert.ToInt64(Session["ActuacionDetalleId" + HFGUID.Value]);
                ObjActuacDetBE.acde_vNotas = txtObservaciones.Text.ToUpper().Replace("'", "''");
                ObjActuacBE.actu_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                ObjActuacBE.actu_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                ObjActuacBE.actu_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                /****************************************************************************************************/



                Object[] miArray = new Object[2] { ObjActuacBE,
                                                   ObjActuacDetBE };

                IntRpta = (int)MiProc.Invocar(ref miArray,
                                              "SGAC.BE.RE_ACTUACION",
                                              Enumerador.enmAccion.MODIFICAR);

                #endregion
            }

            ctrlToolBarRegistroActuacion.btnGrabar.Enabled = false;
            if (IntRpta > 0)
            {
                HabiltaCamposPagoActuacion(false);
                if (ddlTipoPago.SelectedValue == ((int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA).ToString() ||
                    ddlTipoPago.SelectedValue == ((int)Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA).ToString() ||
                    ddlTipoPago.SelectedValue == ((int)Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA).ToString())
                {
                    pnlPagLima.Visible = true;
                    txtNroOperacion.Enabled = false;
                    ddlNomBanco.Enabled = false;
                    ctrFecPago.Enabled = false;
                    txtMtoCancelado.Enabled = false;
                    MostrarMonedaDolar();
                }
                BindGridDetalleActuaciones(LonActuacionId1, LonActuacionId2);


                /*Para Enviar mensaje cuando se registra una actuación*/
                //--------------------------------------------                    
                // Creador por: Jonatan Silva Cachay
                // Fecha: 02/02/2017
                // Objetivo: Envio de Correo y actualiza el flag de enviado
                //--------------------------------------------
                //Fecha: 09/12/2019
                //Autor: Miguel Márquez Beltrán
                //Motivo: Se comento este proceso por ser el mismo campo para cumplir un punto
                //          de Auditoria de notificación de correo al connacional.
                //          mas adelante crear un campo de fecha de control.
                //-------------------------------------------------------
                //DateTime dtFechaInicioEnvio = Convert.ToDateTime(Comun.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.CONFIG_SERVIDOR_CORREO, (int)Enumerador.enmConfiguracionCorreo.FECHA_INICIO_ENVIO, "descripcion"));

                //if (DateTime.Now >= dtFechaInicioEnvio)
                //{
                //    if (dt.Rows[0]["vCorreoElectronico"].ToString() != "")
                //    {
                //        bool bEnvio = false;
                //        DataTable _dt = new DataTable();
                //        _dt = crearTabla(hCorrelativo.Value.ToString());
                //        bEnvio = EnviarCorreoRegistro(_dt, dt.Rows[0]["vCorreoElectronico"].ToString());
                //        if (bEnvio)
                //        {
                //            ActuacionMantenimientoBL BL = new ActuacionMantenimientoBL();
                //            BL.ActualizarFlagEnvioCorreo(ObjActuacBE);
                //        }
                //    }
                //}
            }
            else
            {
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", Constantes.CONST_MENSAJE_OPERACION_FALLIDA, false, 190, 250);
                Comun.EjecutarScript(Page, StrScript);
            }
        
        }

        #region Búsqueda en el tarifario
        /// <summary>
        /// En este evento se manejaran las reglas de negocio referente a los tipos de pago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmb_TipoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                CalculoxTarifarioxTipoPagoxCantidad();

                TextBox txtCodAutoadhesivo = new TextBox();
                txtCodAutoadhesivo.Text = "";

                Comun.ActualizarControlPago(Session, "", txtIdTarifa.Text, txtCantidad.Text,
                        ref ctrlToolBarRegistroActuacion.btnGrabar, ref ddlTipoPago, ref txtNroOperacion, ref txtCodAutoadhesivo,
                        ref ddlNomBanco, ref ctrFecPago, ref ddlExoneracion, ref lblExoneracion, ref lblValExoneracion,
                        ref txtSustentoTipoPago, ref lblSustentoTipoPago, ref lblValSustentoTipoPago,
                        ref RBNormativa, ref RBSustentoTipoPago, ref txtMontoML, ref txtMontoSC,
                        ref txtTotalML, ref txtTotalSC, ref LblDescMtoML, ref LblDescTotML,
                        ref pnlPagLima, ref txtMtoCancelado);

                ctrlToolBarRegistroActuacion.EnabledButtonGrabar = ctrlToolBarRegistroActuacion.btnGrabar.Enabled;

                if (lblSaldoInsumo.Text == "0")
                {
                    ctrlToolBarRegistroActuacion.btnGrabar.Enabled = false;
                }

                ObtenerSaldoAutoadhesivo();

                updRegPago.Update();
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

        protected void LstTarifario_SelectedIndexChanged(object sender, EventArgs e)
        {            
            string StrScript = string.Empty;

            lblExoneracion.Visible = false;
            ddlExoneracion.Visible = false;
            lblValExoneracion.Visible = false;
            RBNormativa.Visible = false;
            RBSustentoTipoPago.Visible = false;

            lblSustentoTipoPago.Visible = false;
            txtSustentoTipoPago.Visible = false;
            txtSustentoTipoPago.Text = "";
            lblValSustentoTipoPago.Visible = false;

            if (LstTarifario.SelectedIndex == -1)
            {
                return;
            }

            if (Session["dtTarifarioFiltrado"] != null)
            {
                dtTarifarioFiltrado = (DataTable)Session["dtTarifarioFiltrado"];

                LimpiarDatosTarifaPago();

                if (LstTarifario.SelectedItem.Text.Contains("58B"))
                {
                    PersonaConsultaBL obj = new PersonaConsultaBL();
                    if (obj.Tiene58A(Convert.ToInt64(hid_iRecurrenteId.Value)) == 0)
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", "El Connacional no cuenta con una tarifa 58A para poder registrar la tarifa 58B.", false, 190, 250);
                        Comun.EjecutarScript(Page, StrScript);
                        txtDescTarifa.Text = String.Empty;
                        txtIdTarifa.Text = "";
                        Session["NuevoRegistro"] = true;
                        Session.Remove(strVariableTarifario);

                        pnlPagLima.Visible = false;
                        LimpiarDatosTarifaPago();
                        updRegPago.Update();
                        return;
                    }
                }

                if (LstTarifario.SelectedValue != Convert.ToString(Constantes.CONST_EXCEPCION_TARIFA_ID_122))
                {
                    CargarObjetoTarifario(dtTarifarioFiltrado, LstTarifario.SelectedIndex);

                    txtIdTarifa.Text = objTarifarioBE.tari_sNumero + objTarifarioBE.tari_vLetra;
                    CargarTipoPagoNormaTarifario();
                    

                    txtDescTarifa.Text = LstTarifario.SelectedItem.Text;
                    hdn_tari_vDescripcionLarga.Value = objTarifarioBE.tari_vDescripcion;
                    hdn_tope_min.Value = Comun.ToNullInt32(objTarifarioBE.tari_ITopeCantidadMinima).ToString();

                    Session[strVariableTarifario] = objTarifarioBE;

                    
                    HabilitaPorTarifa();

                    CalculoxTarifarioxTipoPagoxCantidad();

                    if (Comun.ToNullInt32(hacde_sTarifarioId.Value) == (int)Enumerador.enmSeccion.VISA)
                    {
                        ddlTipoPago.SelectedValue = "0";
                        ddlTipoPago.Enabled = false;
                    }

                    updRegPago.Update();
                }
                else
                {
                    Session["NuevoRegistro"] = true;
                    Session.Remove(strVariableTarifario);

                    pnlPagLima.Visible = false;

                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", "Esta tarifa solo se aplica en el RUNE.", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    txtDescTarifa.Text = String.Empty;
                }
            }

            MostrarDL173_DS076_2005RE();
            BloquearParaTarifasGratuitas();

            
            if (Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
            {
                ctrlToolBarRegistroActuacion.EnabledButtonGrabar = true;
            }
        }

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
            objTarifarioBE.tari_ITopeCantidad = Comun.ToNullInt32(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_ITopeCantidad"]);
            objTarifarioBE.tari_ITopeCantidadMinima = Comun.ToNullInt32(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_ITopeCantidadMinima"]);

            if (dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_FMontoExceso"] != System.DBNull.Value)
            {
                objTarifarioBE.tari_FMontoExceso = Convert.ToDouble(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_FMontoExceso"]);
            }
            else
            {
                objTarifarioBE.tari_FMontoExceso = 0;
            }

            objTarifarioBE.tari_bTarifarioDependienteFlag = Convert.ToBoolean(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_bTarifarioDependienteFlag"]);
            objTarifarioBE.tari_bHabilitaCantidad = Convert.ToBoolean(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_bHabilitaCantidad"]);
            objTarifarioBE.tari_bFlujoGeneral = Convert.ToBoolean(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_bFlujoGeneral"]);
            objTarifarioBE.tari_vTipoPagoExcepcion = Convert.ToString(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_vTipoPagoExcepcion"]);
        }

        private void CargarListaTarifario(DataTable dtTarifarioFiltrado)
        {
            if (dtTarifarioFiltrado.Rows.Count > 1)
            {
                LstTarifario.DataSource = dtTarifarioFiltrado;
                LstTarifario.DataTextField = "tari_vDescripcionCorta";
                LstTarifario.DataValueField = "tari_sTarifarioId";
                LstTarifario.DataBind();
            }
            else
            {
                txtDescTarifa.Text = dtTarifarioFiltrado.Rows[0]["tari_vDescripcion"].ToString();
                hdn_tari_vDescripcionLarga.Value = dtTarifarioFiltrado.Rows[0]["tari_vDescripcion"].ToString();
            }
        }

        protected void imgBuscarTarifarioM_Click(object sender, ImageClickEventArgs e)
        {
            BuscarTarifario();
        }

        protected void txtIdTarifa_TextChanged(object sender, EventArgs e)
        {
            try
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
                pnlPagLima.Visible = false;
                txtNroOperacion.Enabled = false;
                ddlNomBanco.Enabled = false;
                ctrFecPago.Enabled = false;
                //----------------------------------------------
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 15-08-2016
                // Objetivo: Activar la lista desplegable de clasificación 
                //           si la tarifa es la 20B.
                // Referencia: Requerimiento No.001_2.doc
                //----------------------------------------------
                if (txtIdTarifa.Text.Trim().ToUpper().Equals("20B"))
                {
                    lblClasificacion.Visible = true;
                    ddlClasificacion.Visible = true;

                }
                else
                {
                    lblClasificacion.Visible = false;
                    ddlClasificacion.Visible = false;

                }

                
                if (txtIdTarifa.Text.Trim().ToUpper().Equals("58B"))
                {
                    PersonaConsultaBL obj = new PersonaConsultaBL();
                    
                    if (obj.Tiene58A(Convert.ToInt64(hid_iRecurrenteId.Value)) == 0)
                    {
                        string StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", "El Connacional no cuenta con una tarifa 58A para poder registrar la tarifa 58B.", false, 190, 250);
                        Comun.EjecutarScript(Page, StrScript);
                        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "verificar", "alert('El connacional no cuenta con una tarifa 58A para poder registrar la tarifa 58B')", true);
                        txtIdTarifa.Text = "";
                        return;
                    }
                }

                //----------------------------------------------      
                BuscarTarifario();
                MostrarDL173_DS076_2005RE();
                BloquearParaTarifasGratuitas();
                CargarTipoPagoNormaTarifario();
                if (lblSaldoInsumo.Text == "0")
                {
                    ctrlToolBarRegistroActuacion.btnGrabar.Enabled = false;
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
       
        protected void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            cmb_TipoPago_SelectedIndexChanged(sender, e);
        }

        #endregion

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

        protected void gdvActuacionesGeneradas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Session[strVariableAccion] = "0";
            Session["ACTO_GENERAL_MRE"] = "0";

            int intSeleccionado = Comun.ToNullInt32(e.CommandArgument);

            Session["IntTarifarioId"] = Comun.ToNullInt32(Page.Server.HtmlDecode(gdvActuacionesGeneradas.Rows[intSeleccionado].Cells[2].Text));

            Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value] = Comun.ToNullInt32(Page.Server.HtmlDecode(gdvActuacionesGeneradas.Rows[intSeleccionado].Cells[0].Text));
            Session[Constantes.CONST_SESION_ACTUACIONDET_ID] = Comun.ToNullInt32(Page.Server.HtmlDecode(gdvActuacionesGeneradas.Rows[intSeleccionado].Cells[0].Text));

            Session["MIGRATORIO_OFICINACONSULTAR_ID"] = Comun.ToNullInt32(Page.Server.HtmlDecode(gdvActuacionesGeneradas.Rows[intSeleccionado].Cells[10].Text));

            if (e.CommandName == "Editar")
            {
                Session[strVariableAccion] = Enumerador.enmTipoOperacion.REGISTRO;
            }
            else if (e.CommandName == "Consultar")
            {
                Session[strVariableAccion] = Enumerador.enmTipoOperacion.CONSULTA;
            }

            //Proceso MiProc = new Proceso();
            BE.MRE.SI_TARIFARIO objTarifario = (BE.MRE.SI_TARIFARIO)Session[strVariableTarifario];
            if (objTarifario != null)
            {
                CargarDatosTarifaPago();

                Int16 intTarifaId = Convert.ToInt16(Session["IntTarifarioId"]);

                Session["iACTUACION_ID" + HFGUID.Value] = Comun.ToNullInt32(gdvActuacionesGeneradas.Rows[intSeleccionado].Cells[1].Text);

                //object[] arrParametros = { intTarifaId, (int)Enumerador.enmTipoOperacion.REGISTRO };
                //string strFormulario = (string)MiProc.Invocar(ref arrParametros, "ACTUACIONDET_FORMATO", Enumerador.enmAccion.CONSULTAR);

                ActuacionMantenimientoBL obj = new ActuacionMantenimientoBL();
                string strFormulario = obj.ObtenerFormularioPorTarifa(intTarifaId, (int)Enumerador.enmTipoOperacion.REGISTRO);
                string codPersona = "";
                codPersona = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString());

                if (strFormulario != null)
                {
                    if (strFormulario != string.Empty)
                    {
                        string[] arrDatos = strFormulario.Split('-');
                        if (arrDatos.Length > 0)
                        {
                            BE.RE_TARIFA_PAGO objTarifaPago = ObtenerDatosTarifaPago(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]));

                            Session.Add(Constantes.CONST_SESION_OBJ_TARIFA_PAGO, objTarifaPago);                            
                            Session.Add(Constantes.CONST_SESION_ACTUACIONDET_TABS, arrDatos[1]);

                            string codTipoDocEncriptada = "";
                            string codNroDocumentoEncriptada = "";
                            
                            if (e.CommandName == "IrGeneral")
                            {
                                Session["ACTO_GENERAL_MRE"] = "1";


                                if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                                {
                                    Response.Redirect("~/Registro/FrmActoGeneral.aspx?CodPer=" + codPersona + "&Juridica=1", false);
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
                                        Response.Redirect("~/Registro/FrmActoGeneral.aspx?CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                                    }
                                    else
                                    {
                                        Response.Redirect("~/Registro/FrmActoGeneral.aspx?CodPer=" + codPersona, false);
                                    }
                                }
                                
                                //}
                            }
                            else
                            {
                                if (intTarifaId == 1)
                                {
                                    Session["InicioTramite"] = "1";

                                    if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                                    {
                                        Response.Redirect("~/Registro/" + arrDatos[0] + "?bIni=1&CodPer=" + codPersona + "&Juridica=1", false);
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
                                            Response.Redirect("~/Registro/" + arrDatos[0] + "?bIni=1&CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                                        }
                                        else
                                        {
                                            Response.Redirect("~/Registro/" + arrDatos[0] + "?bIni=1&CodPer=" + codPersona, false);
                                        }
                                    }
                                    
                                   
                                }
                                else {
                                   
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
                                }
                                
                            }
                        }
                    }
                }
            }
        }

        protected void gdvActuacionesGeneradas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int intActoGeneral = 0;
            object objActoGeneral = ConfigurationManager.AppSettings["FlujoActoGeneral"];
            if (objActoGeneral != null)
                intActoGeneral = Comun.ToNullInt32(ConfigurationManager.AppSettings["FlujoActoGeneral"]);

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (intActoGeneral == 1)
                {
                    e.Row.FindControl("btnIrGeneral").Visible = true;
                    gdvActuacionesGeneradas.Columns[11].Visible = true;
                }
                else
                {
                    e.Row.FindControl("btnIrGeneral").Visible = false;
                    gdvActuacionesGeneradas.Columns[11].Visible = false;
                }
            }
        }

        #endregion
         
        #region Métodos

        private void CargarDatosIniciales()
        {
            // Datos Personales
            lblDestino.Text = string.Empty;
            lblTexto.Text = string.Empty;
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

            //    lblTexto.Text = strEtiquetaSolicitante + " ES EL DECLARANTE?";
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

                lblTexto.Text = strEtiquetaSolicitante + " ES EL DECLARANTE?";
                if (ViewState["DescTipDoc"] != null)
                    strEtiquetaSolicitante += "- " + ViewState["DescTipDoc"].ToString() + ": ";
                if (ViewState["NroDoc"] != null)
                    strEtiquetaSolicitante += ViewState["NroDoc"].ToString();
            //}




            lblDestino.Text = strEtiquetaSolicitante;

            // Varios
            txtCantidad.Enabled = false;
            ddlTipoPago.Enabled = false;
            //LblFecha.Text = DateTime.Now.ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
            //--------------------------------------------------------------------
            // Autor: Miguel Márquez Beltrán
            // Fecha: 07/10/2016
            // Objetivo: Mostrar la fecha actual del consulado.
            //--------------------------------------------------------------------
            LblFecha.Text = Comun.FormatearFecha((Accesorios.Comun.ObtenerFechaActualTexto(HttpContext.Current.Session))).ToString("MMM-dd-yyyy");
            //-------------------------------------------------------------------
            Session["DtDetActuaciones" + HFGUID.Value] = CrearDtRegDtDetActuaciones();
            ((DataTable)Session["DtDetActuaciones" + HFGUID.Value]).Clear();

            LblDescMtoML.Text = Session[Constantes.CONST_SESION_TIPO_MONEDA].ToString();
            LblDescTotML.Text = Session[Constantes.CONST_SESION_TIPO_MONEDA].ToString();

            Session["NuevoRegistro"] = true;
            hidEventNuevo.Value = "";

            lblTituloActGeneradas.Visible = false;

        }

        private void CargarListadosDesplegables()
        {          
            //--------------------------------------------------
            DataTable dtTipoPago = new DataTable();
            dtTipoPago = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACREDITACION_TIPO_COBRO);

            DataView dv = dtTipoPago.DefaultView;
            DataTable dtTipoPagoOrdenadoOrdenado = dv.ToTable();
            dtTipoPagoOrdenadoOrdenado.DefaultView.Sort = "torden ASC";

            Util.CargarParametroDropDownList(ddlTipoPago, dtTipoPagoOrdenadoOrdenado, true);

            Util.CargarParametroDropDownList(ddlNomBanco, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.BANCO), true);
            Util.CargarParametroDropDownList(ddlClasificacion, comun_Part1.ObtenerParametrosPorGrupo(Session, SGAC.Accesorios.Constantes.CONST_ACTUACION_CLASIFICACION_TARIFA), true);

            //if (Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Convert.ToInt16(Constantes.CONST_ID_CONSULADO_CARACAS))
            //{
            //    ddlTipoPago.Items.Remove(ddlTipoPago.Items.FindByText("PAGO ARUBA"));
            //    ddlTipoPago.Items.Remove(ddlTipoPago.Items.FindByText("PAGO OTRAS ISLAS CARIBEÑAS"));                
            //}
        }

        /// <summary>
        /// Crea el registro temporal del detalle de la actuación 
        /// </summary>
        /// <returns></returns>
        private DataTable CrearDtRegDtDetActuaciones()
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

            //--------------------------------------------
            // Creador por: Miguel Angel Márquez Beltrán
            // Fecha: 15-08-2016
            // Objetivo: Adicionar la columna Clasificacion
            // Referencia: Requerimiento No.001_2.doc
            //--------------------------------------------
            DataColumn dcClasificacion = DtDetActuaciones.Columns.Add("sClasificacionTarifaId", typeof(int));
            dcClasificacion.AllowDBNull = true;
            dcClasificacion.Unique = false;
            //--------------------------------------------
            // Creador por: Miguel Angel Márquez Beltrán
            // Fecha: 27-10-2016
            // Objetivo: Adicionar la columna NormaTarifario
            //--------------------------------------------
            DataColumn dcExoneracion = DtDetActuaciones.Columns.Add("iNormaTarifarioId", typeof(Int64));
            dcExoneracion.AllowDBNull = true;
            dcExoneracion.Unique = false;
            //--------------------------------------------

            return DtDetActuaciones;
        }        

        private void LimpiarListaTarifa()
        {
            LstTarifario.DataSource = null;
            LstTarifario.Items.Clear();
            LstTarifario.ClearSelection();
        }

        private void LimpiarDatosTarifaPago()
        {
            lblCantidad.Text = "Cantidad:";
            txtCantidad.Text = "1";
            txtCantidad.Enabled = false;
            ddlTipoPago.Enabled = false;

            hdn_tope_min.Value = "0";

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

            if (ddlTipoPago.Items.Count > 0)
                ddlTipoPago.SelectedIndex = 0;
            ctrlToolBarRegistroActuacion.EnabledButtonGrabar = false;
        }

        private DataTable BindListTarifario(string IntSeccionId, ref string strDescripcion)
        {
            try
            {
                DataTable dtTarifario;
                DataTable dtTarifarioFiltrado = new DataTable();
                int NroRegistros = 0;

                object[] arrParametros = { 0, txtIdTarifa.Text, 
                                       "", 
                                       ((char)Enumerador.enmEstado.ACTIVO).ToString(),
                                       1, 50, 0, 0 };

                dtTarifario = comun_Part2.ObtenerTarifario(Session, ref arrParametros, "S");

                dtTarifarioFiltrado = ObtenerTarifarioFiltrado(dtTarifario, txtIdTarifa.Text);

                Session.Remove("dtTarifarioFiltrado");

                if (dtTarifarioFiltrado != null)
                {
                    NroRegistros = dtTarifarioFiltrado.Rows.Count;

                    if (NroRegistros == 0)
                    {
                        LimpiarListaTarifa();
                        LimpiarDatosTarifaPago();
                    }
                    else
                    {
                        Session.Add("dtTarifarioFiltrado", dtTarifarioFiltrado);
                    }
                }

                return dtTarifarioFiltrado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string FormatearMonto(double decMonto)
        {
            return decMonto.ToString("#,##0.00");
        }

        private double CalculaCostoUnitario(int intCantidad, double decCosto)
        {
            return (intCantidad * decCosto);
        }

        private double CalculaCostoML(double decMontoSC, double decTipoCambio)
        {
            return (decMontoSC * decTipoCambio);
        }

        private void BuscarTarifario()
        {
            try
            {
                string StrScript = string.Empty;
                string strDescripcionTarifa = string.Empty;

                LimpiarListaTarifa();
                if (txtIdTarifa.Text.Length > 0)
                {
                    hidEventNuevo.Value = "ev";

                    dtTarifarioFiltrado = BindListTarifario(txtIdTarifa.Text, ref strDescripcionTarifa);

                    if (dtTarifarioFiltrado != null)
                    {
                        txtDescTarifa.Text = strDescripcionTarifa;
                        if (dtTarifarioFiltrado.Rows.Count > 0)
                        {

                            if (dtTarifarioFiltrado.Rows.Count == 1)
                            {
                                hdn_tope_min.Value = dtTarifarioFiltrado.Rows[0]["tari_ITopeCantidadMinima"].ToString();
                                txtIdTarifa.Text = dtTarifarioFiltrado.Rows[0]["tari_sNumero"].ToString() +
                                    dtTarifarioFiltrado.Rows[0]["tari_vLetra"].ToString().ToUpper();

                                hdn_tari_vDescripcionLarga.Value = dtTarifarioFiltrado.Rows[0]["tari_vDescripcion"].ToString();
                            }
                            else
                            {
                                LstTarifario.Focus();
                            }

                            CargarObjetoTarifario(dtTarifarioFiltrado, 0);
                            CargarListaTarifario(dtTarifarioFiltrado);
                            hSeccionId.Value = dtTarifarioFiltrado.Rows[0]["tari_sSeccionId"].ToString();
                            hacde_sTarifarioId.Value = dtTarifarioFiltrado.Rows[0]["tari_sTarifarioId"].ToString();

                            if (dtTarifarioFiltrado.Rows[0]["tari_sTarifarioId"].ToString() == Constantes.CONST_EXCEPCION_TARIFA_ID_122.ToString())
                            {
                                Session["NuevoRegistro"] = true;

                                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", "Esta tarifa solo se aplica en el RUNE.", false, 190, 250);
                                Comun.EjecutarScript(Page, StrScript);
                                txtDescTarifa.Text = String.Empty;
                                return;
                            }

                            Session[strVariableTarifario] = objTarifarioBE;
                            CalculoxTarifarioxTipoPagoxCantidad();


                        }
                        else
                        {
                            Session.Remove(strVariableTarifario);
                            LimpiarDatosTarifaPago();
                            txtIdTarifa.Text = "";

                            String strScript = String.Empty;
                            strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuación", "La Tarifa Consular no Existe.");
                            Comun.EjecutarScript(Page, strScript);
                            updRegPago.Update();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void HabilitaDatosPago(bool bolHabilitar = true)
        {
            txtNroOperacion.Enabled = bolHabilitar;
            ddlNomBanco.Enabled = bolHabilitar;
        }

        private void BloquearParaTarifasGratuitas()
        {
            try
            {
                double decMontoSC = 0;
                if (txtMontoSC.Text == "")
                {
                    txtMontoSC.Text = "0";
                }
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void HabilitaPorTarifa()
        {
            double decMontoSC = 0;
            objTarifarioBE = (BE.MRE.SI_TARIFARIO)Session[strVariableTarifario];
            decMontoSC = (double)objTarifarioBE.tari_FCosto;
            ddlTipoPago.Enabled = true;

            
            //----------------------------------------------
            // Autor: Miguel Angel Márquez Beltrán
            // Fecha: 15-08-2016
            // Objetivo: Activar la lista desplegable de clasificación 
            //           si la tarifa es la 20B.
            // Referencia: Requerimiento No.001_2.doc
            //----------------------------------------------
            if (txtIdTarifa.Text.Trim().ToUpper().Equals("20B"))
            {
                lblClasificacion.Visible = true;
                ddlClasificacion.Visible = true;
                //lblddlClasificacion.Visible = true;
                //chkObligatorio.Visible = true;
                //ddlClasificacion.SelectedIndex = 0;
            }
            else
            {
                lblClasificacion.Visible = false;
                ddlClasificacion.Visible = false;
                //lblddlClasificacion.Visible = false;
                //chkObligatorio.Visible = false;
            }
            //----------------------------------------------
            

            if (decMontoSC == 0)
            {
                ctrlToolBarRegistroActuacion.EnabledButtonGrabar = true;
                ddlTipoPago.Enabled = false;
                ddlTipoPago.SelectedValue = ((int)Enumerador.enmTipoCobroActuacion.GRATIS).ToString();
                txtCantidad.Text = "1";
                LlenarListaExoneracion();
            }
            else
            {               
                if (txtDescTarifa.Text == string.Empty)
                {
                    ddlTipoPago.Enabled = false;
                }
                else
                {
                    bool bNoCobrado = ExisteInafecto_Exoneracion(ddlTipoPago.SelectedValue);

                    if (ddlTipoPago.SelectedValue == "0")
                    {
                        ddlTipoPago.Focus();
                    }
                    else if (Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.GRATIS) ||
                        bNoCobrado)
                    {
                        txtMtoCancelado.Text = "0.00";
                        decMontoSC = 0;
                    }

                   
                }
            }

            switch (Comun.ToNullInt32(objTarifarioBE.tari_sCalculoTipoId))
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

        private void CalculoxTarifarioxTipoPagoxCantidad()
        {
            int intCantidad = 1;
            string strScript = string.Empty;
            string strDescripcionTarifa = string.Empty;
            double decMontoSC = 0, decTotalSC = 0;
            double decMontoML = 0, decTotalML = 0;

            // Evalua habilitacion de controles:            
            if (Session[strVariableTarifario] == null)
            {
                LimpiarDatosTarifaPago();
                return;
            }
            else
            {
                objTarifarioBE = (BE.MRE.SI_TARIFARIO)Session[strVariableTarifario];

                txtCantidad.Enabled = (bool)objTarifarioBE.tari_bHabilitaCantidad;

                // Tarifa de la Actuación:                
                decMontoSC = (double)objTarifarioBE.tari_FCosto;

                // Tarifario:
                if (string.IsNullOrEmpty(txtIdTarifa.Text))
                {
                    return;
                }

          
                HabilitaPorTarifa();

                if (!string.IsNullOrEmpty(txtCantidad.Text))
                {
                    intCantidad = Comun.ToNullInt32(txtCantidad.Text);
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


                if (objTarifarioBE.tari_sCalculoTipoId != (int)Enumerador.enmTipoCalculoTarifario.MONTO_FIJO)
                {
                    txtMontoSC.Text = decTotalSC.ToString(strFormato);
                    txtMontoML.Text = decTotalML.ToString(strFormato);

                    txtTotalSC.Text = decTotalSC.ToString(strFormato);
                    txtTotalML.Text = decTotalML.ToString(strFormato);
                }
                else
                {
                    txtMontoSC.Text = decMontoSC.ToString(strFormato);
                    txtMontoML.Text = decMontoML.ToString(strFormato);

                    txtTotalSC.Text = decTotalSC.ToString(strFormato);
                    txtTotalML.Text = decTotalML.ToString(strFormato);
                }
                

                // Salvando los valores en sesión:
                Session["intCantidad"] = intCantidad;
                Session["decMontoSC"] = decMontoSC;


                updRegPago.Update();
            }
        }

        private void PintarDatosPago(int IntIndex)
        {
            string StrScript = string.Empty;
            Proceso MiProc = new Proceso();

            LimpiarDatosTarifaPago();

            int intTarifarioId = 0;
            double DblMontoSC = 0; double DblMontML = 0; double DblTotSC = 0;
            double DblTotML = 0; double MtoCancelado = 0;

            object[] miArray = { 1  };

            DataTable dtRegPago = (DataTable)MiProc.Invocar(ref miArray,
                                                             "SGAC.BE.AC_PAGO", "LEERREGISTRO");

            if (dtRegPago.Rows.Count > 0)
            {
                HabilitaDatosPago(false);

                intTarifarioId = Comun.ToNullInt32(dtRegPago.Rows[0]["INumero"]);
                hidPagoId.Value = dtRegPago.Rows[0]["pago_iPagoId"].ToString();

                string strFecha = dtRegPago.Rows[0][2].ToString();
                if (strFecha != string.Empty)
                {
                    LblFecha.Text = Comun.FormatearFecha(strFecha).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                }
                else
                {
                    LblFecha.Text = string.Empty;
                }

                txtIdTarifa.Text = dtRegPago.Rows[0]["Tarifa"].ToString();
                txtDescTarifa.Text = dtRegPago.Rows[0]["descripcion"].ToString();
                hdn_tari_vDescripcionLarga.Value = dtRegPago.Rows[0]["descripcion"].ToString();

                ddlTipoPago.SelectedValue = dtRegPago.Rows[0]["pago_spagoTipoId"].ToString();
                if (Comun.ToNullInt32(ddlTipoPago.SelectedValue) == (int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA ||
                    Comun.ToNullInt32(ddlTipoPago.SelectedValue) == (int)Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA ||
                    Comun.ToNullInt32(ddlTipoPago.SelectedValue) == (int)Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA)
                {
                    pnlPagLima.Visible = true;

                    txtNroOperacion.Text = dtRegPago.Rows[0]["vBancoNumeroOperacion"].ToString();
                    if (dtRegPago.Rows[0]["sBancoId"] != null)
                    {
                        if (dtRegPago.Rows[0]["sBancoId"].ToString() != string.Empty)
                        {
                            ddlNomBanco.SelectedValue = dtRegPago.Rows[0]["sBancoId"].ToString();
                        }
                    }

                    ctrFecPago.Text = Comun.FormatearFecha(dtRegPago.Rows[0]["Fecha"].ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                    MtoCancelado = Convert.ToDouble(dtRegPago.Rows[0]["TotMonedaLocal"].ToString());
                    txtMtoCancelado.Text = MtoCancelado.ToString("#,##0.00");
                    txtObservaciones.Text = dtRegPago.Rows[0]["vObservaciones"].ToString();
                    MostrarMonedaDolar();
                }
                else
                {
                    pnlPagLima.Visible = false;

                    txtNroOperacion.Text = string.Empty;
                    ddlNomBanco.SelectedIndex = 0;
                    ctrFecPago.Text = string.Empty;
                    txtMtoCancelado.Text = string.Empty;
                    txtObservaciones.Text = string.Empty;
                }

                txtCantidad.Text = dtRegPago.Rows[0]["Cantidad"].ToString();

                DblMontoSC = Convert.ToDouble(dtRegPago.Rows[0]["MtoSC"]);
                txtMontoSC.Text = DblMontoSC.ToString("#,##0.00");

                DblMontML = Convert.ToDouble(dtRegPago.Rows[0]["MonedaLocal"].ToString());
                txtMontoML.Text = DblMontML.ToString("#,##0.00");

                DblTotSC = Convert.ToDouble(dtRegPago.Rows[0]["TotMtoSC"].ToString());
                txtTotalSC.Text = DblTotSC.ToString("#,##0.00");

                DblTotML = Convert.ToDouble(dtRegPago.Rows[0]["TotMonedaLocal"].ToString());
                txtTotalML.Text = DblTotML.ToString("#,##0.00");

                LblDescMtoML.Text = dtRegPago.Rows[0]["vMonedaLocal"].ToString();
                LblDescTotML.Text = dtRegPago.Rows[0]["vMonedaLocal"].ToString();

                txtObservaciones.Text = ""; 
            }
        }

        private void CargarDatosTarifaPago()
        {
            BE.RE_TARIFA_PAGO objTarifaPago = new RE_TARIFA_PAGO();
            objTarifaPago.sTarifarioId = Comun.ToNullInt32(Session["IntTarifarioId"]);
            objTarifaPago.vTarifa = txtIdTarifa.Text.Trim();
            objTarifaPago.vTarifaDescripcion = txtDescTarifa.Text.Trim();
            objTarifaPago.vTarifaDescripcionLarga = hdn_tari_vDescripcionLarga.Value.ToString();
            var tarifa_Sec = (BE.MRE.SI_TARIFARIO)Session[strVariableTarifario];
            if (tarifa_Sec != null)
                objTarifaPago.tari_sSeccionId = tarifa_Sec.tari_sSeccionId;

            objTarifaPago.datFechaRegistro = Comun.FormatearFecha(LblFecha.Text);
            objTarifaPago.sTipoActuacion = 0;

            objTarifaPago.sTipoPagoId = Convert.ToInt16(ddlTipoPago.SelectedValue);
            objTarifaPago.dblCantidad = Convert.ToDouble(txtCantidad.Text);
            objTarifaPago.dblMontoSolesConsulares = Convert.ToDouble(txtMontoSC.Text);
            objTarifaPago.dblMontoMonedaLocal = Convert.ToDouble(txtMontoML.Text);
            objTarifaPago.dblTotalSolesConsulares = Convert.ToDouble(txtTotalSC.Text);
            objTarifaPago.dblTotalSolesConsulares = Convert.ToDouble(txtTotalML.Text);
            objTarifaPago.vObservaciones = txtObservaciones.Text.Trim().ToUpper();

            if (objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA ||
                objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA ||
                objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA)
            {
                objTarifaPago.vNumeroOperacion = txtNroOperacion.Text.Trim() ;
                objTarifaPago.sBancoId = Convert.ToInt16(ddlNomBanco.SelectedValue);                
                objTarifaPago.datFechaPago = Comun.FormatearFecha(ctrFecPago.Text);
                objTarifaPago.dblMontoCancelado = Convert.ToDouble(txtMtoCancelado.Text);
            }

            Session[Constantes.CONST_SESION_OBJ_TARIFA_PAGO] = objTarifaPago;
        }

        private void BindGridDetalleActuaciones(long LonActuacionIdPrimario, long LonActuacionIdSec)
        {
            DataTable DtActuacionDetalle = new DataTable();
            ActuacionConsultaBL BL = new ActuacionConsultaBL();

            lblTituloActGeneradas.Visible = true;

            DtActuacionDetalle = BL.ActuacionDetalleObtener(LonActuacionIdPrimario, LonActuacionIdSec);
            if (DtActuacionDetalle.Rows.Count > 0)
            {
                lblTituloActGeneradas.Visible = true;
                gdvActuacionesGeneradas.SelectedIndex = -1;
                gdvActuacionesGeneradas.DataSource = DtActuacionDetalle;
                gdvActuacionesGeneradas.DataBind();
                long CantCorrelativos = 0;
                CantCorrelativos = Convert.ToInt64(DtActuacionDetalle.Rows[0]["vCorrelativoTarifario"]);
                CantCorrelativos = CantCorrelativos + DtActuacionDetalle.Rows.Count - 1;
                string strCorrelativos = "";
                strCorrelativos = DtActuacionDetalle.Rows[0]["vCorrelativoTarifario"].ToString();

                if (DtActuacionDetalle.Rows.Count == 1)
                {
                    hCorrelativo.Value = strCorrelativos; 
                }
                else { 
                    hCorrelativo.Value = strCorrelativos + " - " + CantCorrelativos.ToString(); 
                }
            }
            else
            {
                lblTituloActGeneradas.Visible = false;
                gdvActuacionesGeneradas.DataSource = null;
                gdvActuacionesGeneradas.DataBind();                
            }
            updRegPago.Update();
        }

        private void HabiltaCamposPagoActuacion(bool bolHabilitar = true)
        {
            txtIdTarifa.Enabled = bolHabilitar;
            LstTarifario.Enabled = bolHabilitar; 
            imgBuscarTarifarioM.Enabled = bolHabilitar;
            ddlTipoPago.Enabled = bolHabilitar;
            txtCantidad.Enabled = bolHabilitar;
            pnlPagLima.Visible = bolHabilitar;            
            txtObservaciones.Enabled = bolHabilitar;
        }

        private bool ExistenMasTarifa()
        {
            if (txtIdTarifa.Text.ToUpper() == Constantes.CONST_EXCEPCION_TARIFA_79A ||
                txtIdTarifa.Text.ToUpper() == Constantes.CONST_EXCEPCION_TARIFA_3A ||
                txtIdTarifa.Text.ToUpper() == Constantes.CONST_EXCEPCION_TARIFA_58A)
            {
                //Proceso p = new Proceso();
                //object[] arrParametros = { Convert.ToInt64(hid_iRecurrenteId.Value), ((BE.MRE.SI_TARIFARIO)Session[strVariableTarifario]).tari_sTarifarioId };
                //int intCantidad = Comun.ToNullInt32(p.Invocar(ref arrParametros, "SGAC.BE.RE_ACTUACION", "OBTENER_CANT"));

                ActuacionConsultaBL objActuacionConsultaBL = new ActuacionConsultaBL();

                int intCantidad = Comun.ToNullInt32(objActuacionConsultaBL.ObtenerCantidadPorTarifa(Convert.ToInt64(hid_iRecurrenteId.Value), ((BE.MRE.SI_TARIFARIO)Session[strVariableTarifario]).tari_sTarifarioId));
  
                if (intCantidad > 0)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO ACTUACION", Constantes.CONST_MENSAJE_VALIDA_TARIFA_CANTIDAD));
                    return true;
                }
            }
            return false;
        }

        private bool ValidaTarifa1(bool MostrarMensaje = true)
        {
            if (txtIdTarifa.Text.ToUpper() == Constantes.CONST_EXCEPCION_TARIFA_ID_1.ToString())
            {
                if (Request.QueryString["RecuE"] == "--")
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO ACTUACION", "FECHA DE NACIMIENTO INCORRECTA."));
                    return false;
                }
                else
                {
                    Int16 iEdad = Convert.ToInt16(Request.QueryString["RecuE"]);

                    if (iEdad == 200)
                    {
                        if (MostrarMensaje)
                        {
                            Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO ACTUACION", Constantes.CONST_MENSAJE_VALIDA_TARIFA_EDAD_SINFECHA));
                        }
                        return true;
                    }
                    Int16 intMayorEdad = Convert.ToInt16(WebConfigurationManager.AppSettings["Recurrente.Tarifa1.MayorEdad"].ToString());

                    if (iEdad < intMayorEdad)
                    {
                        if (MostrarMensaje)
                        {
                            Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO ACTUACION", Constantes.CONST_MENSAJE_VALIDA_TARIFA_EDAD));
                        }
                        return true;
                    }
                }
            }
            if (RBNormativa.Checked)
            {
                if (ddlExoneracion.Visible == true && ddlExoneracion.Enabled == true)
                {
                    if (ddlExoneracion.SelectedIndex == 0)
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO ACTUACION", "Seleccione la Ley que exonera el Pago"));
                        return true;
                    }
                }
            }
            if (RBSustentoTipoPago.Checked)
            {
                if (txtSustentoTipoPago.Visible == true && txtSustentoTipoPago.Text.Trim().Length == 0)
                {
                    txtSustentoTipoPago.Enabled = true;
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO ACTUACION", "Digite el sustento"));
                    return true;
                }
            }
            //---------------------------------------------------------------------------
            bool bisSeccionIII = Comun.isSeccionIII(Session, txtIdTarifa.Text);
            string strTarifa = txtIdTarifa.Text.Trim().ToUpper();
            if (Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
            {
                if (bisSeccionIII == false && strTarifa != "2")
                {
                    if (txtSustentoTipoPago.Visible == true && txtSustentoTipoPago.Enabled == true && txtSustentoTipoPago.Text.Trim().Length == 0)
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REGISTRO ACTUACION", "Digite el sustento"));
                        return true;
                    }
                }
            }
            //---------------------------------------------------------------------------
            return false;
        }

        private void ObtenerSaldoAutoadhesivo()
        {
            string StrScript = string.Empty;

            ActuacionConsultaBL ActuacionConsultaBL = new ActuacionConsultaBL();
            int intStock = ActuacionConsultaBL.ObtenerSaldoAutoadhesivos(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                                                                         Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]),
                                                                         Convert.ToInt16(Enumerador.enmInsumoTipo.AUTOADHESIVO));
            lblSaldoInsumo.Text = Convert.ToString(intStock);
            ctrlToolBarRegistroActuacion.btnGrabar.Enabled = true;
            if (intStock <= 0)
            {
                msjeWarningStock.Visible = true;
                lblMsjeWarnigStock.Text = Constantes.CONST_MENSAJE_SALDO_INSUFICIENTE;

                ctrlToolBarRegistroActuacion.btnGrabar.Enabled = false;
            }
            else
            {
                if (Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]) == 0)
                {
                    msjeWarningStock.Visible = true;
                    lblMsjeWarnigStock.Text = Constantes.CONST_MENSAJE_SIN_TIPOCAMBIO;

                    ctrlToolBarRegistroActuacion.btnGrabar.Enabled = false;
                }
                else
                {
                    msjeWarningStock.Visible = false;
                    lblMsjeWarnigStock.Text = "";

                    ctrlToolBarRegistroActuacion.btnGrabar.Enabled = true;
                }
            }
        }

        #endregion
   
        public void GetValoresEspeciales()
        {
            String TextoAlfaNumero = String.Empty;
            String CaracterEspecial2 = String.Empty;
            String CaracterEspecial3 = String.Empty;
            TextoAlfaNumero = ConfigurationManager.AppSettings["ValidarText"].ToString();
            CaracterEspecial2 = ConfigurationManager.AppSettings["ValidarNumeroEntero"].ToString();
            CaracterEspecial3 = ConfigurationManager.AppSettings["ValidarNumeroDecimal"].ToString();
            HFValidarNumero.Value = CaracterEspecial2;
            HFValidarNumeroDecimal.Value = CaracterEspecial3;
        }

        [WebMethod]
        public static string Validaciones_Migratorio(string acde_sTarifarioId)
        {
            string s_Resultado = string.Empty;
            try
            {
                var obj_Actuaciones = (DataTable)HttpContext.Current.Session["TRAMITE_DT"];
                if (obj_Actuaciones != null)
                {
                    if (obj_Actuaciones.Rows.Count > 0)
                    {

                    }
                }
            }
            catch
            {
            }
            return "Probando";
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
                objTarifaPago.tari_sSeccionId = Comun.ToNullInt32(dr["tari_sSeccionId"]);
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
                    if (dr["pago_sBancoId"].ToString() != string.Empty)
                    {
                        if (Convert.ToInt32(dr["pago_sBancoId"]) != 0)
                        {
                            objTarifaPago.vNumeroOperacion = Convert.ToString(dr["pago_vBancoNumeroOperacion"]);
                            objTarifaPago.sBancoId = Convert.ToInt16(dr["pago_sBancoId"]);
                            objTarifaPago.datFechaPago = Comun.FormatearFecha(dr["pago_dFechaOperacion"].ToString());                            
                        }
                    }
                    objTarifaPago.dblMontoCancelado = Convert.ToDouble(dr["FTOTALSOLESCONSULARES"]);
                }

                //--------------------------------------------
                // Creador por: Miguel Angel Márquez Beltrán
                // Fecha: 15-08-2016
                // Objetivo: Adicionar la columna Clasificacion
                // Referencia: Requerimiento No.001_2.doc
                //--------------------------------------------
                objTarifaPago.dblClasificacion = Convert.ToDouble(dr["acde_sClasificacionTarifaId"]);
                //--------------------------------------------     
                objTarifaPago.dblNormaTarifario = Convert.ToDouble(dr["pago_iNormaTarifarioId"]);
                objTarifaPago.vSustentoTipoPago = dr["pago_vSustentoTipoPago"].ToString();
            }

            return objTarifaPago;
        }

       

        //-----------------------------------------------------------//
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 19-10-2016
        // Objetivo: Asignar la moneda estadounidense para los tipos 
        //           de pago: pagados en lima en los consulados
        // Fecha de cambio: 31/10/2016
        // Objetivo: Asignar moneda dolar cuando es gratuito por ley
        //           para la tarifa de la sección III.   
        //-----------------------------------------------------------//

        private void MostrarMonedaDolar()
        {
            string strMonedaDescDolar = "";

            if (Comun.ToNullInt32(ddlTipoPago.SelectedValue) == (int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA
                && Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Constantes.CONST_OFICINACONSULAR_LIMA)
            {                
                if (ConfigurationManager.AppSettings["MonedaDescDolar"] != null)
                    strMonedaDescDolar = ConfigurationManager.AppSettings["MonedaDescDolar"].ToString();
         
                LblDescMtoML.Text = strMonedaDescDolar;
                LblDescTotML.Text = strMonedaDescDolar;
                txtMontoML.Text = "0.00";
                txtTotalML.Text = "0.00";
            }
            else
            {               
                LblDescMtoML.Text = Session[Constantes.CONST_SESION_TIPO_MONEDA].ToString();
                LblDescTotML.Text = Session[Constantes.CONST_SESION_TIPO_MONEDA].ToString();               
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
            string strTarifaLetra = txtIdTarifa.Text.Trim().ToUpper();
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

        //--------------------------------------------                    
        // Creador por: Jonatan Silva Cachay
        // Fecha: 02/02/2017
        // Objetivo: Crea un datatable que se utiliza para reemplazar datos de la plantilla HTML del formato del correo
        //--------------------------------------------
        private DataTable crearTabla(string LonActuacionId1)
        {
            DataTable dtReemplazaCorreo = new DataTable();
            DataColumn dc1 = new DataColumn();
            DataColumn dc2 = new DataColumn();
            dc1.ColumnName = "valor";
            dc2.ColumnName = "reemplazo";
            dtReemplazaCorreo.Columns.Add(dc1);
            dtReemplazaCorreo.Columns.Add(dc2);

            /*Obtener Connacional*/
            string strEtiquetaSolicitante = string.Empty;

            //if (HFGUID.Value.Length > 0)
            //{
            //    if (Session["ApePat" + HFGUID.Value] != null)
            //        strEtiquetaSolicitante += Session["ApePat" + HFGUID.Value].ToString() + " ";
            //    if (Session["ApeMat" + HFGUID.Value] != null)
            //        strEtiquetaSolicitante += Session["ApeMat" + HFGUID.Value].ToString() + " ";
            //    if (Session["Nombres" + HFGUID.Value] != null)
            //    {
            //        if (Session["Nombres" + HFGUID.Value].ToString().Trim() != string.Empty)
            //            strEtiquetaSolicitante += ", " + Session["Nombres" + HFGUID.Value].ToString() + " ";
            //    }
            //}
            //else
            //{
            if (ViewState["ApePat"] != null)
                strEtiquetaSolicitante += ViewState["ApePat"].ToString() + " ";
            if (ViewState["ApeMat"] != null)
                strEtiquetaSolicitante += ViewState["ApeMat"].ToString() + " ";
            if (ViewState["Nombre"] != null)
                {
                    if (ViewState["Nombre"].ToString().Trim() != string.Empty)
                        strEtiquetaSolicitante += ", " + ViewState["Nombre"].ToString() + " ";
                }
            //}
            

            DataRow dr = default(DataRow);
            dr = dtReemplazaCorreo.NewRow();
            dr[0] = "{CONNACIONAL}";
            dr[1] = strEtiquetaSolicitante;
            dtReemplazaCorreo.Rows.Add(dr);

            DataRow dr1 = default(DataRow);
            dr1 = dtReemplazaCorreo.NewRow();
            dr1[0] = "{NROACTUACION}";
            dr1[1] = Convert.ToString(LonActuacionId1);
            dtReemplazaCorreo.Rows.Add(dr1);

            DataRow dr2 = default(DataRow);
            dr2 = dtReemplazaCorreo.NewRow();
            dr2[0] = "{FECHAACTUACION}";
            dr2[1] = System.DateTime.Now;
            dtReemplazaCorreo.Rows.Add(dr2);

            DataRow dr3 = default(DataRow);
            dr3 = dtReemplazaCorreo.NewRow();
            dr3[0] = "{NATURALEZA}";
            dr3[1] = this.txtDescTarifa.Text;
            dtReemplazaCorreo.Rows.Add(dr3);

            DataRow dr4 = default(DataRow);
            dr4 = dtReemplazaCorreo.NewRow();
            dr4[0] = "{MISIONCONSULAR}";
            dr4[1] = comun_Part2.ObtenerNombreOficinaPorId(Session, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID])); 
            dtReemplazaCorreo.Rows.Add(dr4);
            
            DataRow dr5 = default(DataRow);
            dr5 = dtReemplazaCorreo.NewRow();
            dr5[0] = "{MONTO}";
            dr5[1] = this.txtTotalSC.Text;
            dtReemplazaCorreo.Rows.Add(dr5);

            DataRow dr6 = default(DataRow);
            dr6 = dtReemplazaCorreo.NewRow();
            dr6[0] = "{SITUACION}";
            dr6[1] = "REGISTRADO";
            dtReemplazaCorreo.Rows.Add(dr6);

            DataRow dr7 = default(DataRow);
            dr7 = dtReemplazaCorreo.NewRow();
            dr7[0] = "{fechaActual}";
            dr7[1] = System.DateTime.Now;
            dtReemplazaCorreo.Rows.Add(dr7);

            DataRow dr8 = default(DataRow);
            dr8 = dtReemplazaCorreo.NewRow();
            dr8[0] = "{MONTOMONEDALOCAL}";
            dr8[1] = this.txtTotalML.Text;
            dtReemplazaCorreo.Rows.Add(dr8);

            DataRow dr9 = default(DataRow);
            dr9 = dtReemplazaCorreo.NewRow();
            dr9[0] = "{MONEDALOCAL}";
            dr9[1] = this.LblDescTotML.Text;
            dtReemplazaCorreo.Rows.Add(dr9);

            return dtReemplazaCorreo;
        }
        //--------------------------------------------                    
        // Creador por: Jonatan Silva Cachay
        // Fecha: 02/02/2017
        // Objetivo: Envio de Correo
        //--------------------------------------------
        private bool EnviarCorreoRegistro(DataTable _dtReemplazo,string CorreoElectronico)
        {
            #region Envío Correo
            string strScript = string.Empty;

            string strSMTPServer = string.Empty;
            string strSMTPPuerto = string.Empty;
            string strEmailFrom = string.Empty;
            string strEmailPassword = string.Empty;
            string strEmailTo = string.Empty;

            strSMTPServer = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.CONFIG_SERVIDOR_CORREO, (int)Enumerador.enmConfiguracionCorreo.SERVIDOR, "descripcion");
            strSMTPPuerto = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.CONFIG_SERVIDOR_CORREO, (int)Enumerador.enmConfiguracionCorreo.PUERTO, "descripcion");

            strEmailFrom = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.CONFIG_SERVIDOR_CORREO, (int)Enumerador.enmConfiguracionCorreo.CORREO_DE, "descripcion");
            strEmailPassword = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.CONFIG_SERVIDOR_CORREO, (int)Enumerador.enmConfiguracionCorreo.CONTRASENIA, "descripcion");

            strEmailTo = CorreoElectronico;
            string strTitulo = "Registro de Actuación - " + Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE];
 
            // ENVIAR CORREO
            Enumerador.enmTipoMensaje enmTipoMensaje = Enumerador.enmTipoMensaje.INFORMATION;
            bool bEnviado = false;
            string strMensaje = string.Empty;
            string strCorreo = string.Empty;
            string strRutaCorreo = string.Empty;
            strRutaCorreo = Server.MapPath("~") + "/Registro/Plantillas/CorreoRegistroActuacion.html";
            try
            {
                bEnviado = Correo.EnviarCorreoPlantillaHTML(strRutaCorreo, _dtReemplazo, strSMTPServer, strSMTPPuerto,
                                               strEmailFrom, strEmailPassword,
                                               strEmailTo,strTitulo, System.Net.Mail.MailPriority.High, null);
            }
            catch (Exception ex)
            {
                strCorreo += " (Error al enviar el correo: " + ex.Message + ")";

                #region Registro Incidencia
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_vValoresTabla = "ENVIO CORREO",
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
            //--

            strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, "Correo enviado exitosamente.", false, 160, 300);
            
            #endregion

            string strScriptCorreo = string.Empty;
            if (!bEnviado)
            {
                enmTipoMensaje = Enumerador.enmTipoMensaje.ERROR;
                strCorreo = "Correo no enviado." + strCorreo;
                strScriptCorreo = Mensaje.MostrarMensaje(enmTipoMensaje, "ENVÍO CORREO", strCorreo);

                Comun.EjecutarScript(Page, strScriptCorreo);
            }
            else
            {
                if (strMensaje != string.Empty)
                {
                    strScript += Mensaje.MostrarMensaje(enmTipoMensaje, "ENVÍO CORREO", strMensaje);
                }

                Comun.EjecutarScript(Page, strScript);
            }
            return bEnviado;
        }


        private void CargarTipoPagoNormaTarifario()
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void MostrarDL173_DS076_2005RE()
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private bool ExisteInafecto_Exoneracion(string strID)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private DataTable ObtenerTarifarioFiltrado(DataTable dtTarifario, string strTarifaLetra)
        {
            try
            {
                int IntTotalCount = 0;
                int IntTotalPages = 0;

                DataTable dt = new DataTable();

                Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                DataTable dtConsuladoTipoPagoTarifa = new DataTable();
                OficinaConsularTarifarioTipoPagoDL objConsuladoTarifarioTipoPagoBL = new OficinaConsularTarifarioTipoPagoDL();

                dtConsuladoTipoPagoTarifa = objConsuladoTarifarioTipoPagoBL.Consultar(intOficinaConsularId, 0, "", false, 20000, 1, "N", ref IntTotalCount, ref IntTotalPages);

                DataView dvConsultadoTipoPagoTarifa = dtConsuladoTipoPagoTarifa.DefaultView;

                dvConsultadoTipoPagoTarifa.RowFilter = "FlagExcepcionTarifa = 1";

                DataTable dtConsuladoTarifarioSel = dvConsultadoTipoPagoTarifa.ToTable(true, "ofpa_sTarifarioId");


                if (dtConsuladoTarifarioSel.Rows.Count == 0)
                {
                    DataView dvFiltrado = dtTarifario.DefaultView;
                    dvFiltrado.RowFilter = "tari_bFlagExcepcion = 0";
                    dt = dvFiltrado.ToTable();
                    return dt;
                }
                else
                {
                    bool bExiste = false;
                    Int16 intTarifaId = 0;

                    for (int i = 0; i < dtTarifario.Rows.Count; i++)
                    {
                        if (dtTarifario.Rows[i]["tari_bFlagExcepcion"].ToString().Equals("True"))
                        {
                            intTarifaId = Convert.ToInt16(dtTarifario.Rows[i]["tari_sTarifarioId"].ToString());
                            bExiste = false;

                            for (int x = 0; x < dtConsuladoTarifarioSel.Rows.Count; x++)
                            {
                                if (intTarifaId == Convert.ToInt16(dtConsuladoTarifarioSel.Rows[x]["ofpa_sTarifarioId"].ToString()))
                                {
                                    bExiste = true;
                                    break;
                                }
                            }

                            if (bExiste == false)
                            {
                                dtTarifario.Rows.RemoveAt(i);
                            }
                        }
                    }


                    return dtTarifario;
                }
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
    }
}
