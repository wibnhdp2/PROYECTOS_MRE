using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.Configuracion.Sistema.BL;
using SGAC.Registro.Actuacion.BL;
using SGAC.WebApp.Accesorios;
using SGAC.BE.MRE;
using SGAC.BE.MRE.Custom;
using SGAC.Configuracion.Maestro.BL;
using SGAC.Registro.Persona.BL;
using System.Configuration;
using Microsoft.Security.Application;

namespace SGAC.WebApp.Consulta
{
    public partial class FrmProtocolar : MyBasePage
    {
        private string strNombreEntidad = "CONSULTAS ACTOS NOTARIALES";

        #region Eventos
        private void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginadorProtocolar.PageSize = Constantes.CONST_PAGE_SIZE_ACTUACIONES;
            ctrlPaginadorProtocolar.Visible = false;
            ctrlPaginadorProtocolar.PaginaActual = 1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlOficinaConsular.ddlOficinaConsular.SelectedIndexChanged += new EventHandler(ddlOficinaConsular_SelectedIndexChanged);
            if (!Page.IsPostBack)
            {
                //if (Request.QueryString["GUID"] != null)
                //{
                //    HFGUID.Value = Sanitizer.GetSafeHtmlFragment(Request.QueryString["GUID"].ToString());
                //}
                //else
                //{
                //    HFGUID.Value = PageUniqueId.Replace("-", "");
                //}
                
                CargarListadosDesplegables();
                CargarDatosIniciales();
                txtNroEP.Enabled = false;
                tablaDestinoAutorizacionViaje.Style.Add("display", "none");
            }
            
        }

        void ddlOficinaConsular_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarFuncionarios(ddlFuncionarioAutorizador, true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ocultar.Visible = false;
            grvParticipantes.DataSource = null;

            DateTime FechIni = ctrlDateFecExtIni.Value();
            DateTime FechFin = ctrlDateFecExtFin.Value();

            DateTime? FechIniPago = ctrFecPagoIni.Value();
            DateTime? FechFinPago = ctrFecPagoFin.Value();

            if (ctrFecPagoIni.Text.Trim() == string.Empty && ctrFecPagoFin.Text.Trim() == string.Empty)
            {
                if (ctrlDateFecExtIni.Text.Trim() == string.Empty && ctrlDateFecExtIni.Text.Trim() == string.Empty)
                {
                    ctrlValProtocolar.MostrarValidacion("Debe Ingresar un rango de fecha.", true, Enumerador.enmTipoMensaje.WARNING);
                    return;
                }
                else {
                    if (ctrlDateFecExtIni.Text.Trim() != string.Empty && ctrlDateFecExtFin.Text.Trim() != string.Empty)
                    {
                        if (FechIni <= FechFin)
                        {
                            TimeSpan ts = Convert.ToDateTime(FechFin) - Convert.ToDateTime(FechIni); 
                            if (Convert.ToInt16(ts.Days) > 30)
                            {
                                ctrlValProtocolar.MostrarValidacion("Debe Ingresar un rango de fecha no mayor a un mes.", true, Enumerador.enmTipoMensaje.WARNING);
                                return;
                            }
                        }
                        else
                        {
                            ctrlValProtocolar.MostrarValidacion("La fecha fin debe ser mayor a la fecha de inicio.", true, Enumerador.enmTipoMensaje.WARNING);
                            return;
                        }
                    }
                    else
                    {
                        ctrlValProtocolar.MostrarValidacion("Debe Ingresar un rango de fecha.", true, Enumerador.enmTipoMensaje.WARNING);
                        return;
                    }
                }
            }
            else {
                if (ctrFecPagoIni.Text.Trim() != string.Empty && ctrFecPagoFin.Text.Trim() != string.Empty)
                {
                    if (FechFinPago >= FechIniPago)
                    {
                        TimeSpan ts = Convert.ToDateTime(FechFinPago) - Convert.ToDateTime(FechIniPago);
                        if (Convert.ToInt16(ts.Days) > 30)
                        {
                            ctrlValProtocolar.MostrarValidacion("Debe Ingresar un rango de fecha no mayor a un mes.", true, Enumerador.enmTipoMensaje.WARNING);
                            return;
                        }
                    }
                    else {
                        ctrlValProtocolar.MostrarValidacion("La fecha fin debe ser mayor a la fecha de inicio.", true, Enumerador.enmTipoMensaje.WARNING);
                        return;
                    }
                }
                else {
                    ctrlValProtocolar.MostrarValidacion("Debe Ingresar un rango de fecha.", true, Enumerador.enmTipoMensaje.WARNING);
                    return;
                }
            }


            if (ctrFecPagoIni.Text.Trim() != string.Empty)
            {
                if (Comun.EsFecha(ctrFecPagoIni.Text.Trim()) == false)
                {
                    ctrlValProtocolar.MostrarValidacion("La fecha de extensión inicial no es válida.", true, Enumerador.enmTipoMensaje.WARNING);
                    return;
                }
            }
            if (ctrFecPagoFin.Text.Trim() != string.Empty)
            {
                if (Comun.EsFecha(ctrFecPagoFin.Text.Trim()) == false)
                {
                    ctrlValProtocolar.MostrarValidacion("La fecha de extensión final no es válida.", true, Enumerador.enmTipoMensaje.WARNING);

                    return;
                }
            }
            if (ctrlDateFecExtIni.Text.Trim() != string.Empty)
            {
                if (Comun.EsFecha(ctrlDateFecExtIni.Text.Trim()) == false)
                {
                    ctrlValProtocolar.MostrarValidacion("La fecha de creación inicial no es válida.", true, Enumerador.enmTipoMensaje.WARNING);

                    return;
                }
            }

            if (ctrlDateFecExtFin.Text.Trim() != string.Empty)
            {
                if (Comun.EsFecha(ctrlDateFecExtFin.Text.Trim()) == false)
                {
                    ctrlValProtocolar.MostrarValidacion("La fecha de creación final no es válida.", true, Enumerador.enmTipoMensaje.WARNING);

                    return;
                }
            }

          
            if (!validarConsulta()) { return; }

            
            if (ctrlDateFecExtIni.Text.Trim() != string.Empty &&
                ctrlDateFecExtFin.Text.Trim() != string.Empty)
            {                
                if (FechIni > FechFin)
                {
                    ctrlPaginadorProtocolar.InicializarPaginador();

                    gdvProtocolar.DataSource = new DataTable();
                    gdvProtocolar.DataBind();

                    ctrlValProtocolar.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);

                    return;
                }
            }

            MostrarSeguimiento(false);

            //---------------------------------
            // Autor del cambio: Miguel Márquez
            // Fecha del cambio: 15/09/2016
            // Objetivo: Anular el parametro año.
            //---------------------------------
            //------------------------------------------------------------------------
            // Autor: Miguel Angel Márquez Beltrán
            // Fecha: 19/09/2016
            // Objetivo: Se adiciono el parametro Sub tipo de acto notarial
            //------------------------------------------------------------------------  
            //------------------------------------------------------------------------
            // Autor: Miguel Angel Márquez Beltrán
            // Fecha: 20/10/2021
            // Objetivo: Se adiciono el parametro UbigeoDestino para los actos 
            //          Extraprotocolares de autorización de viaje de menor.
            //------------------------------------------------------------------------  
         
            string strUbigeoDestino = "";

            if (ddlTipoActo.SelectedValue == Convert.ToInt16(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR).ToString())
            {
                 
                strUbigeoDestino = ddl_UbigeoPaisViajeDestino.SelectedValue.ToString() +
                            ddl_UbigeoRegionViajeDestino.SelectedValue.ToString() +
                            ddl_UbigeoCiudadViajeDestino.SelectedValue.ToString();
                if (strUbigeoDestino == "000")
                { strUbigeoDestino = ""; }
                else
                {
                    string strPais = "00";
                    string strRegion = "00";
                    string strCiudad = "00";
                    if (ddl_UbigeoPaisViajeDestino.SelectedIndex > 0)
                    {
                        strPais = ddl_UbigeoPaisViajeDestino.SelectedValue;
                    }
                    if (ddl_UbigeoRegionViajeDestino.SelectedIndex > 0)
                    {
                        strRegion = ddl_UbigeoRegionViajeDestino.SelectedValue;
                    }
                    if (ddl_UbigeoCiudadViajeDestino.SelectedIndex > 0)
                    {
                        strCiudad = ddl_UbigeoCiudadViajeDestino.SelectedValue;
                    }
                    strUbigeoDestino = strPais + strRegion + strCiudad;
                }
            }
            //------------------------------------------------------------------------  

            CargarGrillaProtocolares(Convert.ToInt16(ctrlOficinaConsular.SelectedValue),
                                     txtNroEP.Text,
                                     Convert.ToInt16(ddlEstadoEP.SelectedValue),
                                     FechIni,
                                     FechFin,
                                     Convert.ToInt16(ddlFuncionarioAutorizador.SelectedValue),
                                     Convert.ToInt16(ddlActoNotarial.SelectedValue),
                                     Convert.ToInt16(ddlTipoActo.SelectedValue),
                                     txtPrimerApellido.Text,
                                     txtSegundoApellido.Text,
                                     txtNombres.Text,
                                     Convert.ToInt16(ddlDocumentoTipo.SelectedValue),
                                     txtDocumentoNumero.Text,
                                     Convert.ToInt16(ddlParticipanteTipo.SelectedValue),
                                     0, FechIniPago, FechFinPago, strUbigeoDestino);


        }

        protected void ctrlPaginadorProtocolar_Click(object sender, EventArgs e)
        {
            DateTime FechIni = ctrlDateFecExtIni.Value();
            DateTime FechFin = ctrlDateFecExtFin.Value();

            DateTime? FechIniPago = ctrFecPagoIni.Value();
            DateTime? FechFinPago = ctrFecPagoFin.Value();
            //---------------------------------
            // Autor del cambio: Miguel Márquez
            // Fecha del cambio: 15/09/2016
            // Objetivo: Anular el parametro año.
            //---------------------------------
            //------------------------------------------------------------------------
            // Autor: Miguel Angel Márquez Beltrán
            // Fecha: 19/09/2016
            // Objetivo: Se adiciono el parametro Sub tipo de acto notarial
            //------------------------------------------------------------------------

            CargarGrillaProtocolares(Convert.ToInt16(ctrlOficinaConsular.SelectedValue),
                                     txtNroEP.Text,
                                     Convert.ToInt16(ddlEstadoEP.SelectedValue),
                                     FechIni,
                                     FechFin,
                                     Convert.ToInt16(ddlFuncionarioAutorizador.SelectedValue),
                                     Convert.ToInt16(ddlActoNotarial.SelectedValue),
                                     Convert.ToInt16(ddlTipoActo.SelectedValue),   
                                     txtPrimerApellido.Text,
                                     txtSegundoApellido.Text,
                                     txtNombres.Text,
                                     Convert.ToInt16(ddlDocumentoTipo.SelectedValue),
                                     txtDocumentoNumero.Text,
                                     Convert.ToInt16(ddlParticipanteTipo.SelectedValue),
                                     0, FechIniPago, FechFinPago);
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            if (!validarConsulta()) { return; }

            if (Session["dtReporteEscrituras"] != null)
            {
                //-----------------------------------------------------------------------------
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 19/09/2016
                // Objetivo: Se adiciono la opción del reporte de Autorización Viaje de Menor
                //-----------------------------------------------------------------------------

                if (ddlTipoActo.SelectedValue == Convert.ToInt16(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR).ToString())
                {
                    Session["Acto_Notarial_ID"] = 0;
                    Session["Acto_Notarial_Tipo_formato"] = (int)Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR;
                }
                else
                {
                    Session["Acto_Notarial_ID"] = 11776;
                    Session["Acto_Notarial_Tipo_formato"] = (int)Enumerador.enmFormatoProtocolar.LISTADO_ESCRITURAS;
                }

                Session["OficinaConsular"] = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                string strUrl = "../Reportes/frmVisorProtocolar.aspx";

                string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=1100,height=700,left=100,top=100');";
                EjecutarScript(Page, strScript);
            }
            else
            {
                EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "CONSULTA", "No ha realizado la consulta previamente."), "Accion0");
            }
        }

        protected void gdvProtocolar_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 lRowIndex = Convert.ToInt32(e.CommandArgument);

            Int64 intActuacionId = Convert.ToInt64(gdvProtocolar.Rows[lRowIndex].Cells[0].Text);
            Int64 intActoNotarialId = Convert.ToInt64(gdvProtocolar.Rows[lRowIndex].Cells[1].Text);
            int intActoNotarialTipoId = Convert.ToInt32(gdvProtocolar.DataKeys[lRowIndex].Values["sTipoActoNotarialId"]);
            int intActoNotarialSubTipoId = Convert.ToInt32(gdvProtocolar.DataKeys[lRowIndex].Values["sSubTipoActoNotarialId"]);
            int intActoNotarialEstadoId = Convert.ToInt32(gdvProtocolar.DataKeys[lRowIndex].Values["acno_sEstadoId"]);

            ViewState["ActuacionId"] = gdvProtocolar.Rows[lRowIndex].Cells[0].Text;
            ViewState["ActoNotarialId"] = gdvProtocolar.Rows[lRowIndex].Cells[1].Text;

            string vEstadoActo = gdvProtocolar.DataKeys[lRowIndex].Values["vEstado"].ToString();
            
            Int64 intPersonaId = Convert.ToInt64(gdvProtocolar.DataKeys[lRowIndex].Values["iPersonaRecurrente"]);
            string codTipoDocumento = gdvProtocolar.DataKeys[lRowIndex].Values["sTipDocumento"].ToString();
            string codNumeroDocumento = gdvProtocolar.DataKeys[lRowIndex].Values["vDocumentoNumero"].ToString();

            string strRutaArchivo = gdvProtocolar.DataKeys[lRowIndex].Values["vRutaArchivo"].ToString();

            Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value] = intActuacionId;
            Session[Constantes.CONST_SESION_ACTONOTARIAL_ID] = intActoNotarialId;
            //Session[Constantes.CONST_SESION_PERSONA_ID + HFGUID.Value] = intPersonaId;

            string CodPersona = Util.Encriptar(intPersonaId.ToString());
            string codTipoDocEncriptada = Util.Encriptar(codTipoDocumento);
            string codNroDocumentoEncriptada = Util.Encriptar(codNumeroDocumento);


            String[] arrRecurrente = gdvProtocolar.Rows[lRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(gdvProtocolar, "vPersonaRecurrente")].Text.Split(',');
            String AccionOperacion = String.Empty;
            String vNombreFuncionario = gdvProtocolar.Rows[lRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(gdvProtocolar, "vNombreFuncionario")].Text;
            vNombreFuncionario = Convert.ToString(Page.Server.HtmlDecode(vNombreFuncionario));
            gdvParte.DataSource = null;
            gdvParte.DataBind();
            gdvTestimonio.DataSource = null;
            gdvTestimonio.DataBind();
            MostrarSeguimiento(false);


            //ViewState["ApePat"] = arrRecurrente[0].ToString();
            //ViewState["Nombres"] = arrRecurrente[1].ToString();
            //ViewState["iDocumentoTipoId"] = gdvProtocolar.Rows[lRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(gdvProtocolar, "sTipDocumento")].Text;
            //ViewState["DescTipDoc"] = gdvProtocolar.Rows[lRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(gdvProtocolar, "vDescDocumento")].Text;
            //ViewState["NroDoc"] = gdvProtocolar.Rows[lRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(gdvProtocolar, "vDocumentoNumero")].Text;
            Session["NombreConsulado"] = gdvProtocolar.Rows[lRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(gdvProtocolar, "vNombreConsulado")].Text;

            //ViewState["PER_GENERO"] = gdvProtocolar.Rows[lRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(gdvProtocolar, "pers_sGeneroId")].Text;
            switch (e.CommandName.ToString())
            {
                case "Consultar":
                    /*
                     En caso que se quiera consultar los datos de una escritura publica. todos los
                     Campos del formulario de protocolares deberan estar desabilitados
                    */
                    #region Consultar
                    
                    if (vEstadoActo == "ANULADA")
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, "No se puede visualizar un documento anulado."));
                        break;
                    }
                    if (intActoNotarialTipoId == (int)Enumerador.enmNotarialTipoActo.PROTOCOLAR)
                        Session["iFlujoProtocolar"] = (int)Enumerador.enmFlujoProtocolar.CONSULTA;
                    else if (intActoNotarialTipoId == (int)Enumerador.enmNotarialTipoActo.EXTRAPROTOCOLAR)
                        Session["iFlujoExtraProtocolar"] = (int)Enumerador.enmFlujoProtocolar.CONSULTA;

                    Session["Actuacion_Accion"] = Enumerador.enmTipoOperacion.CONSULTA;
                    AccionOperacion = Convert.ToInt32(Enumerador.enmNotarialAccion.CONSULTA).ToString(); ;


                    
                    if (intActoNotarialTipoId == (int)Enumerador.enmNotarialTipoActo.PROTOCOLAR)
                    {
                        if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                        {
                            Response.Redirect("~/Registro/FrmActoNotarialProtocolares.aspx?class=" + AccionOperacion + "&CodPer=" + CodPersona + "&Juridica=1",false);
                        }
                        else
                        { // PERSONA NATURAL
                            if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                            {
                                Response.Redirect("~/Registro/FrmActoNotarialProtocolares.aspx?class=" + AccionOperacion + "&CodPer=" + CodPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                            }
                            else
                            {
                                Response.Redirect("~/Registro/FrmActoNotarialProtocolares.aspx?class=" + AccionOperacion + "&CodPer=" + CodPersona, false);
                            }
                        }
                        
                    }
                    else
                    {
                        if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                        {
                            Response.Redirect("~/Registro/FrmActuacionNotarialExtraProtocolar.aspx?CodPer=" + CodPersona + "&Juridica=1", false);
                        }
                        else
                        { // PERSONA NATURAL
                            if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                            {
                                Response.Redirect("~/Registro/FrmActuacionNotarialExtraProtocolar.aspx?CodPer=" + CodPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                            }
                            else
                            {
                                Response.Redirect("~/Registro/FrmActuacionNotarialExtraProtocolar.aspx?CodPer=" + CodPersona, false);
                            }
                        }
                       
                    }
                    //}
                    #endregion
                    break;

                case "Editar":
                    #region Editar
                    
                    string strFechaRegistro = Comun.ObtenerFechaActualTexto(Session);
                    //if (gdvProtocolar.Rows[lRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(gdvProtocolar, "vEstado")].Text == "PAGADO")
                    //{
                        if (gdvProtocolar.Rows[lRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(gdvProtocolar, "pago_dFechaCreacion")].Text != string.Empty)
                        {
                            strFechaRegistro = gdvProtocolar.Rows[lRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(gdvProtocolar, "pago_dFechaCreacion")].Text;
                        }
                    //}
                    if (Comun.CalcularDiasHabilesModificacion(Session, Page, strFechaRegistro) == false)
                    {
                        return;
                    }

                    /*
                     En caso que se quiera editar el registro de la escritura publica en proceso. Para lo cual
                     se debera obtener el estado actual del registro y en base a este se habilitaran las pestañas
                     que correspondan                     
                    */
                    if (vEstadoActo == "ANULADA")
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, "No se puede editar un documento anulado."));
                        break;
                    }
                    if (intActoNotarialTipoId == (int)Enumerador.enmNotarialTipoActo.PROTOCOLAR)
                    {
                        Session["iFlujoProtocolar"] = (int)Enumerador.enmFlujoProtocolar.EDICION;
                        Session["ModoEdicionProtocolar"] = true;
                    }

                    Session["Actuacion_Accion"] = Enumerador.enmTipoOperacion.ACTUALIZACION;
                    AccionOperacion = Convert.ToInt32(Enumerador.enmNotarialAccion.EDICION).ToString();

                    //if (HFGUID.Value.Length > 0)
                    //{
                    //    if (intActoNotarialTipoId == (int)Enumerador.enmNotarialTipoActo.PROTOCOLAR)
                    //        Response.Redirect("~/Registro/FrmActoNotarialProtocolares.aspx?class=" + AccionOperacion + "&GUID=" + HFGUID.Value);
                    //    else
                    //        Response.Redirect("~/Registro/FrmActuacionNotarialExtraProtocolar.aspx?GUID=" + HFGUID.Value);
                    //}
                    //else
                    //{
                    if (intActoNotarialTipoId == (int)Enumerador.enmNotarialTipoActo.PROTOCOLAR)
                    {
                        if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                        {
                            Response.Redirect("~/Registro/FrmActoNotarialProtocolares.aspx?class=" + AccionOperacion + "&CodPer=" + CodPersona + "&Juridica=1", false);
                        }
                        else
                        { // PERSONA NATURAL
                            if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                            {
                                Response.Redirect("~/Registro/FrmActoNotarialProtocolares.aspx?class=" + AccionOperacion + "&CodPer=" + CodPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                            }
                            else
                            {
                                Response.Redirect("~/Registro/FrmActoNotarialProtocolares.aspx?class=" + AccionOperacion + "&CodPer=" + CodPersona, false);
                            }
                        }
                        
                    }
                    else
                    {
                        if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                        {
                            Response.Redirect("~/Registro/FrmActuacionNotarialExtraProtocolar.aspx?CodPer=" + CodPersona + "&Juridica=1", false);
                        }
                        else
                        { // PERSONA NATURAL
                            if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                            {
                                Response.Redirect("~/Registro/FrmActuacionNotarialExtraProtocolar.aspx?CodPer=" + CodPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                            }
                            else
                            {
                                Response.Redirect("~/Registro/FrmActuacionNotarialExtraProtocolar.aspx?CodPer=" + CodPersona, false);
                            }
                        }
                        
                    }
                    //}
                    #endregion
                    break;

                case "Rectificacion":
                    #region Rectificación
                    /*
                     En caso que se quiera generar una escritura publica a partir de otra anteriormente creada.
                     Para lo cual es necesario captura el numero de la EP creada y en el formulario de protocolares
                     solo se deberian cargar las tarifas correspondientes a AMPLIACION, RECTIFICACION, REVOCACION o relacionadas
                     a un mandato judicial.
                    */
                    if (intActoNotarialTipoId == (int)Enumerador.enmNotarialTipoActo.PROTOCOLAR)
                    {
                        if ((intActoNotarialSubTipoId == (int)Enumerador.enmProtocolarTipo.TESTAMENTOS_CERRADOS) ||
                            (intActoNotarialSubTipoId == (int)Enumerador.enmProtocolarTipo.TESTAMENTO_ESCRITURA_PÚBLICA) ||
                            (intActoNotarialSubTipoId == (int)Enumerador.enmProtocolarTipo.PODER_GENERAL_AMPLIO_ABSOLUTO) ||
                            (intActoNotarialSubTipoId == (int)Enumerador.enmProtocolarTipo.PODER_ESPECIAL))
                        {

                            if (Consultar_Estado_Cs(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTONOTARIAL_ID]), Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value])) == (int)Enumerador.enmNotarialProtocolarEstado.DIGITALIZADA)
                            {
                                Session[Constantes.CONST_SESION_ACTONOTARIAL_ID] = 0;
                                Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value] = 0;

                                Session["NroEscritura"] = gdvProtocolar.Rows[lRowIndex].Cells[Util.ObtenerIndiceColumnaGrilla(gdvProtocolar, "vNumeroEscrituraPublica")].Text;
                                Session["iActoNotarialReferenciaId"] = intActoNotarialId;
                                Session["iFlujoProtocolar"] = (int)Enumerador.enmFlujoProtocolar.RECTIFICACION;
                                Session["intActoNotarialSubTipoId"] = intActoNotarialSubTipoId;
                                Session["dtActoNotarialSubTipoId"] = cargar_cboAccionesRectificacionCS(intActoNotarialSubTipoId);
                                AccionOperacion = Convert.ToInt32(Enumerador.enmNotarialAccion.RECTIFICACION).ToString();

                                //if (HFGUID.Value.Length > 0)
                                //{
                                //    Response.Redirect("~/Registro/FrmActoNotarialProtocolares.aspx?class=" + AccionOperacion + "&GUID=" + HFGUID.Value);
                                //}
                                //else
                                //{
                                if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                                {
                                    Response.Redirect("~/Registro/FrmActoNotarialProtocolares.aspx?class=" + AccionOperacion + "&CodPer=" + CodPersona + "&Juridica=1", false);
                                }
                                else
                                { // PERSONA NATURAL
                                    if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                                    {
                                        Response.Redirect("~/Registro/FrmActoNotarialProtocolares.aspx?class=" + AccionOperacion + "&CodPer=" + CodPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                                    }
                                    else
                                    {
                                        Response.Redirect("~/Registro/FrmActoNotarialProtocolares.aspx?class=" + AccionOperacion + "&CodPer=" + CodPersona, false);
                                    }
                                }
                                    
                                //}
                            }
                            else
                            {
                                EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "CONSULTA", "No puede realizar esta accion con una escritura que aun no ha finalizado su proceso."), "Accion0");
                                return;
                            }
                        }
                        else
                        {
                            EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "CONSULTA", "No se puede realizar esta acción con este tipo de acto protocolar."), "Accion1");
                            return;
                        }
                    }
                    else
                    {
                        EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "CONSULTA", "No puede realizar esta accion con un acto extraprotocolar."), "Accion2");
                        return;
                    }
                    #endregion
                    break;

                case "VerBorrador":
                    //if (intActoNotarialEstadoId == (int)Enumerador.enmNotarialProtocolarEstado.DIGITALIZADA)
                    //{
                    VerVistaPrevia(intActoNotarialId, vNombreFuncionario, intActuacionId);
                    //}                    
                    break;
                case "SolicitarFormato":
                    #region Solicitar Formato Adicional: Parte o/y testimonio
                    //if (intActoNotarialEstadoId == (int)Enumerador.enmNotarialProtocolarEstado.DIGITALIZADA)
                    //{
                    Session["Actuacion_Accion"] = Enumerador.enmTipoOperacion.ACTUALIZACION;
                    AccionOperacion = Convert.ToInt32(Enumerador.enmNotarialAccion.SOLICITUD).ToString();

                   
                    if (intActoNotarialTipoId == (int)Enumerador.enmNotarialTipoActo.PROTOCOLAR)
                    {
                        if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                        {
                            Response.Redirect("~/Registro/FrmActoNotarialProtocolares.aspx?class=" + AccionOperacion + "&CodPer=" + CodPersona + "&Juridica=1", false);
                        }
                        else
                        { // PERSONA NATURAL
                            if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                            {
                                Response.Redirect("~/Registro/FrmActoNotarialProtocolares.aspx?class=" + AccionOperacion + "&CodPer=" + CodPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                            }
                            else
                            {
                                Response.Redirect("~/Registro/FrmActoNotarialProtocolares.aspx?class=" + AccionOperacion + "&CodPer=" + CodPersona, false);
                            }
                        }
                        
                    }
                    else
                    {
                        if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                        {
                            Response.Redirect("~/Registro/FrmActuacionNotarialExtraProtocolar.aspx" + "?CodPer=" + CodPersona + "&Juridica=1", false);
                        }
                        else
                        { // PERSONA NATURAL
                            if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                            {
                                Response.Redirect("~/Registro/FrmActuacionNotarialExtraProtocolar.aspx" + "?CodPer=" + CodPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                            }
                            else
                            {
                                Response.Redirect("~/Registro/FrmActuacionNotarialExtraProtocolar.aspx" + "?CodPer=" + CodPersona, false);
                            }
                        }
                        
                    }
                    //}
                    //}
                    #endregion

                    break;
                case "Seguimiento":
                    #region Seguimiento: Parte o/y Testimonio
                    ActoNotarialConsultaBL objBL = new ActoNotarialConsultaBL();

                    DataTable dtPartes = objBL.ObtenerActoNotarialDetalle(intActuacionId, (Int16)Enumerador.enmNotarialTipoFormato.PARTE);

                    gdvParte.DataSource = dtPartes;
                    gdvParte.DataBind();

                    

                    DataTable dtTestimonios = objBL.ObtenerActoNotarialDetalle(intActuacionId, (Int16)Enumerador.enmNotarialTipoFormato.TESTIMONIO);

                    gdvTestimonio.DataSource = dtTestimonios;
                    gdvTestimonio.DataBind();
                    

                    MostrarSeguimiento(true, Enumerador.enmNotarialTipoFormato.PARTE);
                    MostrarSeguimiento(true, Enumerador.enmNotarialTipoFormato.TESTIMONIO);

                    #endregion
                    break;
                case "VerParticipantes":
                    #region Ver Participantes
                    ActoNotarialConsultaBL obj = new ActoNotarialConsultaBL();

                    DataTable dtParticipantes = obj.ActonotarialObtenerParticipantes(intActoNotarialId);

                    grvParticipantes.DataSource = dtParticipantes;
                    grvParticipantes.DataBind();

                    ocultar.Visible = true;
                    #endregion
                    break;
                case "VerProyectoEP":
                    //-------------------------------------------------------------
                    //Fecha: 07/12/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Visualizar el documento PDF de Escritura Pública.
                    //-------------------------------------------------------------
                    #region VerEscrituraPublica
                    
                    string strScript = string.Empty;

                    if (strRutaArchivo.Length == 0)
                    {
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTOS NOTARIALES - CONSULTA",
                            "No existe ningún archivo PDF.");
                        Comun.EjecutarScript(Page, strScript);

                        return;
                    }
                    if (strRutaArchivo.IndexOf("_E_") == -1)
                    {
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTOS NOTARIALES - CONSULTA",
                            "No existe ningún archivo de Escritura Pública.");
                        Comun.EjecutarScript(Page, strScript);

                        return;
                    }

                    string uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
                    string strCarpetaEC = System.Configuration.ConfigurationManager.AppSettings["CarpetaEscrituraPublica"];
                    string strRutaEC = uploadPath + @strCarpetaEC;
                    string Ext = ".pdf";
                    Session["strTipoArchivo"] = Ext;

                    string strMision = gdvProtocolar.DataKeys[lRowIndex].Values["vNombreConsulado"].ToString();
                   
                    string strAnio = strRutaArchivo.Substring(0, 4);
                    string strMes = strRutaArchivo.Substring(4, 2);
                    string strDia = strRutaArchivo.Substring(6, 2);
                    
                    string strpathMision = System.IO.Path.Combine(strRutaEC, strMision);
                    string strpathAnio = System.IO.Path.Combine(strpathMision, strAnio);
                    string strpathAnioMes = System.IO.Path.Combine(strpathAnio, strMes);
                    string strpathAnioMesDia = System.IO.Path.Combine(strpathAnioMes, strDia);

                    Session["strpathAnioMesDia"] = strpathAnioMesDia;

                    string strRutaArchivoDestino = System.IO.Path.Combine(strpathAnioMesDia, strRutaArchivo);


                    if (System.IO.File.Exists(strRutaArchivoDestino))
                    {
                        try
                        {
                            string strUrl = "../Registro/FrmPreviewEP.aspx?filename=" + strRutaArchivo;
                            strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=auto,height=auto,left=0,top=0');";
                            Comun.EjecutarScript(Page, strScript);
                        }
                        catch (Exception ex)
                        {
                            strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTOS NOTARIALES - CONSULTA",
                                "El archivo se pudo abrir. Vuelva a intentarlo." +
                                "\n(" + ex.Message + ")");
                            Comun.EjecutarScript(Page, strScript);

                        }
                    }
                    else
                    {
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTOS NOTARIALES - CONSULTA",
                          "No existe el archivo de Escritura Pública.");
                        Comun.EjecutarScript(Page, strScript);
                    }
                    #endregion
                    break;
               
            }
        }

        protected void gdvProtocolar_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e != null)
            {
                if (e.Row != null)
                {
                    if (e.Row.RowIndex > -1)
                    {
                        int iTipoActoNotarial = Convert.ToInt32(gdvProtocolar.DataKeys[e.Row.RowIndex].Values["sTipoActoNotarialId"]);
                        short sEstado = Convert.ToInt16(gdvProtocolar.DataKeys[e.Row.RowIndex].Values["acno_sEstadoId"]);
                        string strRutaArchivo = gdvProtocolar.DataKeys[e.Row.RowIndex].Values["vRutaArchivo"].ToString();


                        LinkButton imgButtonVerProyectoEP = e.Row.FindControl("lnkVerProyectoEP") as LinkButton;
                        ImageButton btnEditar = e.Row.FindControl("btnEditar") as ImageButton;
                        if (strRutaArchivo.Length == 0)
                        {
                            imgButtonVerProyectoEP.Visible = false;
                        }
                        if (strRutaArchivo.IndexOf("_E_") == -1)
                        {
                            imgButtonVerProyectoEP.Visible = false;
                        }

                        if (sEstado == Convert.ToInt16(Enumerador.enmNotarialProtocolarEstado.ANULADA))
                        {
                            imgButtonVerProyectoEP.Visible = false;
                            btnEditar.Visible = false;
                        }

                        if (iTipoActoNotarial == Convert.ToInt32(Enumerador.enmNotarialTipoActo.EXTRAPROTOCOLAR))
                        {
                            ImageButton imgButtonFormato = e.Row.FindControl("btnVerFormato") as ImageButton;
                            if (imgButtonFormato != null)
                            {
                                imgButtonFormato.Visible = false;
                            }
                            ImageButton imgButtonSolicitar = e.Row.FindControl("btnSolicitarFormato") as ImageButton;
                            if (imgButtonSolicitar != null)
                            {
                                imgButtonSolicitar.Visible = false;
                            }
                            ImageButton imgButtonRectifi = e.Row.FindControl("btnRectificacion") as ImageButton;
                            if (imgButtonRectifi != null)
                            {
                                imgButtonRectifi.Visible = false;
                            }
                            ImageButton imgButtonSegui = e.Row.FindControl("btnSeguimiento") as ImageButton;
                            if (imgButtonSegui != null)
                            {
                                imgButtonSegui.Visible = false;
                            }
                        }
                        else if (iTipoActoNotarial == Convert.ToInt32(Enumerador.enmNotarialTipoActo.PROTOCOLAR))
                        {
                            if (sEstado != Convert.ToInt16(Enumerador.enmNotarialProtocolarEstado.DIGITALIZADA))
                            {
                                ImageButton imgButtonRectificacion = e.Row.FindControl("btnRectificacion") as ImageButton;
                                if (imgButtonRectificacion != null)
                                {
                                    imgButtonRectificacion.Visible = false;
                                }

                                ImageButton btnSolicitarFormato = e.Row.FindControl("btnSolicitarFormato") as ImageButton;
                                if (btnSolicitarFormato != null)
                                {
                                    btnSolicitarFormato.Visible = false;
                                }

                                ImageButton btnVerFormato = e.Row.FindControl("btnVerFormato") as ImageButton;
                                if (btnVerFormato != null)
                                {
                                    btnVerFormato.Visible = false;
                                }


                            }
                        }
                    }

                }
            }
            if (e.Row.RowType != DataControlRowType.DataRow) return;


            ImageButton btnEditar2 = e.Row.FindControl("btnEditar") as ImageButton;


            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                ImageButton[] arrImageButtons = { btnEditar2 };
                Comun.ModoLectura(ref arrImageButtons);
            }
        }

        protected void ddlTipoActo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //--------------------------------------------------------------
            // Autor: Miguel Márquez Beltrán
            // Fecha: 16/09/2016
            // Objetivo: Adicionar Participantes por Tipo de Acto Notarial 
            //--------------------------------------------------------------
            ddlParticipanteTipo.Items.Clear();
            ddlParticipanteTipo.Items.Insert(0, new ListItem("- TODOS -", "0"));
            tablaDestinoAutorizacionViaje.Style.Add("display", "none");

            if (ddlActoNotarial.SelectedValue == Convert.ToInt16(Enumerador.enmNotarialTipoActo.EXTRAPROTOCOLAR).ToString())
            {                
                if (ddlTipoActo.SelectedValue == Convert.ToInt16(Enumerador.enmExtraprotocolarTipo.CERTIFICADO_SUPERVIVENCIA).ToString())
                {
                    ddlParticipanteTipo.Items.Insert(1, new ListItem("TITULAR", Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.TITULAR).ToString()));
                }
                if (ddlTipoActo.SelectedValue == Convert.ToInt16(Enumerador.enmExtraprotocolarTipo.PODER_FUERA_REGISTRO).ToString())
                {
                    ddlParticipanteTipo.Items.Insert(1, new ListItem("OTORGANTE", Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.OTORGANTE).ToString()));
                    ddlParticipanteTipo.Items.Insert(2, new ListItem("APODERADO", Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.APODERADO).ToString()));
                    ddlParticipanteTipo.Items.Insert(3, new ListItem("TESTIGO A RUEGO", Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO).ToString()));
                    ddlParticipanteTipo.Items.Insert(4, new ListItem("INTERPRETE", Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.INTERPRETE).ToString()));
                }
                if (ddlTipoActo.SelectedValue == Convert.ToInt16(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR).ToString())
                {
                    ddlParticipanteTipo.Items.Insert(1, new ListItem("APODERADO", Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.APODERADO).ToString()));
                    ddlParticipanteTipo.Items.Insert(2, new ListItem("PADRE", Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.PADRE).ToString()));
                    ddlParticipanteTipo.Items.Insert(3, new ListItem("MADRE", Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.MADRE).ToString()));
                    ddlParticipanteTipo.Items.Insert(4, new ListItem("ACOMPAÑANTE", Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.ACOMPANANTE).ToString()));
                    ddlParticipanteTipo.Items.Insert(5, new ListItem("INTERPRETE", Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.INTERPRETE).ToString()));
                    ddlParticipanteTipo.Items.Insert(6, new ListItem("MENOR DE EDAD", Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.MENOR).ToString()));
                    tablaDestinoAutorizacionViaje.Style.Add("display", "block");
                }
            }
            else
            {                
                    ddlParticipanteTipo.Items.Insert(1, new ListItem("OTORGANTE", Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.OTORGANTE).ToString()));
                    ddlParticipanteTipo.Items.Insert(2, new ListItem("APODERADO", Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.APODERADO).ToString()));
                    ddlParticipanteTipo.Items.Insert(3, new ListItem("TESTIGO A RUEGO", Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO).ToString()));
                    ddlParticipanteTipo.Items.Insert(4, new ListItem("INTERPRETE", Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.INTERPRETE).ToString()));                
            }
        }

        #endregion

        #region Métodos
        private void CargarDatosIniciales()
        {
            ctrlDateFecExtIni.Text = string.Empty;
            ctrlDateFecExtFin.Text = string.Empty;
        }

        private void CargarListadosDesplegables()
        {
            /* Filtra por atributos del usuario */
            if ((int)Session[Constantes.CONST_SESION_ACCESO_ID] == (int)Enumerador.enmAccesoUsuario.INDIVIDUAL ||
                (int)Session[Constantes.CONST_SESION_OFICINACONSULAR_ID] != Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                ctrlOficinaConsular.Cargar(false, false);
                ctrlOficinaConsular.ddlOficinaConsular.Enabled = false;
            }
            else
            {
                ctrlOficinaConsular.Cargar(true, true, "- SELECCIONAR -");
                ctrlOficinaConsular.ddlOficinaConsular.Enabled = true;
                ctrlOficinaConsular.SelectedValue = "1";
            }

            Util.CargarDropDownList(ddlDocumentoTipo, comun_Part1.ObtenerDocumentoIdentidad(), "Valor", "Id", true, " - TODOS - ");
            Util.CargarParametroDropDownList(ddlEstadoEP, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmEstadoGrupo.PROTOCOLARES_ESTADO_ESCRITURA), false);
            ddlEstadoEP.Items.Insert(0, new ListItem("- TODOS -", "0"));
            CargarFuncionarios(ddlFuncionarioAutorizador, true);

            //---------------------------------------
            // Autor: Miguel Márquez Beltrán
            // Fecha: 16/09/2016
            // Objetivo: Adicionar Acto Notarial 
            //---------------------------------------

            ddlActoNotarial.Items.Insert(0, new ListItem("EXTRAPROTOCOLAR", Convert.ToInt16(Enumerador.enmNotarialTipoActo.EXTRAPROTOCOLAR).ToString()));
            ddlActoNotarial.Items.Insert(1, new ListItem("PROTOCOLAR", Convert.ToInt16(Enumerador.enmNotarialTipoActo.PROTOCOLAR).ToString()));

            Util.CargarParametroDropDownList(ddlTipoActo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_NOTARIAL_TIPO_ACTO_EXTRAPROTOCOLAR), true, " - TODOS - ");
            for (int i = 0; i < ddlTipoActo.Items.Count; i++)
            {
                if (ddlTipoActo.Items[i].Value == Convert.ToInt16(Enumerador.enmExtraprotocolarTipo.OTRAS_CERTIFICACIONES_NOTARIALES).ToString())
                {
                    ddlTipoActo.Items.RemoveAt(i);
                }
            }
            ddlParticipanteTipo.Items.Insert(0, new ListItem("- TODOS -", "0"));
            //------------------------------------------------------
            //Fecha: 20/10/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Cargar las listas de UBIGEO.
            //------------------------------------------------------
            comun_Part3.CargarUbigeo(Session, ddl_UbigeoPaisViajeDestino, Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT, string.Empty, string.Empty, true);
            comun_Part3.CargarUbigeo(Session, ddl_UbigeoRegionViajeDestino, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, string.Empty, string.Empty, true);
            comun_Part3.CargarUbigeo(Session, ddl_UbigeoCiudadViajeDestino, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, string.Empty, string.Empty, true);

            //-----------------------------------------------------------
        }

        private DataTable CrearTablaEscrituras()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("vNumeroEscrituraPublica", typeof(string));
            dt.Columns.Add("dFechaExtension", typeof(string));
            dt.Columns.Add("vPersonaRecurrente", typeof(string));
            dt.Columns.Add("vTipoActoNotarial", typeof(string));
            dt.Columns.Add("vSubTipoActoNotarial", typeof(string));
            dt.Columns.Add("vEstado", typeof(string));
            dt.Columns.Add("actu_iActuacionId", typeof(long));
            dt.Columns.Add("acno_iActoNotarialId", typeof(long));
            dt.Columns.Add("acno_sTipoActoNotarialId", typeof(int));
            dt.Columns.Add("iPersonaRecurrente", typeof(long));

            DataRow lDataRow = dt.NewRow();
            lDataRow["_vNumeroEscrituraPublica"] = "";
            lDataRow["dFechaExtension"] = "";
            lDataRow["vPersonaRecurrente"] = "";
            lDataRow["vTipoActoNotarial"] = "";
            lDataRow["vSubTipoActoNotarial"] = "";
            lDataRow["vEstado"] = "";
            lDataRow["actu_iActuacionId"] = 0;
            lDataRow["acno_iActoNotarialId"] = 0;
            lDataRow["acno_sTipoActoNotarialId"] = 0;
            lDataRow["iPersonaRecurrente"] = 0;
            dt.Rows.Add(lDataRow);

            return dt;
        }
        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 19/09/2016
        // Objetivo: Se adiciono el parametro Sub tipo de acto notarial
        //------------------------------------------------------------------------

        private void CargarGrillaProtocolares(int intOficinaConsularId,
                                              string strNumProyecto,
                                              int intEstadoId,
                                              DateTime dateFechaInicio,
                                              DateTime dateFechaFinal,
                                              int intFuncionarioAutorizadorId,
                                              int intTipoActoNotarialId,
                                              int intSubTipoActoNotarialId,
                                              string strApellidoPaterno,
                                              string strApellidoMaterno,
                                              string strNombres,
                                              short sTipoDocumento,
                                              string strNumeroDocumento,
                                              short sTipoParticipante,
                                              short sAnio,
                                              DateTime? FechIniPago,
                                              DateTime? FechFinPago,
                                              string strUbigeoDestino = "")
        {
            DataTable dtActosNotariales = new DataTable();

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int IntTotalCountx = 0;
            int IntTotalPagesx = 0;
            

            ActoNotarialConsultaBL objBL = new ActoNotarialConsultaBL();
            //------------------------------------------------------------------------
            // Autor: Miguel Angel Márquez Beltrán
            // Fecha: 20/09/2016
            // Objetivo: Adiciona la consulta de registros de Viaje de Menor
            //------------------------------------------------------------------------

            
            dtActosNotariales = objBL.ActoProtocolarConsulta(intOficinaConsularId,
                                                                            strNumProyecto,
                                                                            intEstadoId,
                                                                            dateFechaInicio,
                                                                            dateFechaFinal,
                                                                            intFuncionarioAutorizadorId,
                                                                            intTipoActoNotarialId,
                                                                            intSubTipoActoNotarialId,
                                                                            strApellidoPaterno,
                                                                            strApellidoMaterno,
                                                                            strNombres,
                                                                            sTipoDocumento,
                                                                            strNumeroDocumento,
                                                                            sTipoParticipante,
                                                                            sAnio,
                                                                            FechIniPago,
                                                                            FechFinPago,
                                                                            txtRGE.Text == "" ?  0 : Convert.ToInt32(txtRGE.Text),
                                                                            ctrlPaginadorProtocolar.PaginaActual,
                                                                            ctrlPaginadorProtocolar.PageSize,
                                                                            ref IntTotalCount,
                                                                            ref IntTotalPages,
                                                                            strUbigeoDestino);
            
            
            if (dtActosNotariales != null)
            {

                if (ddlTipoActo.SelectedValue == Convert.ToInt16(Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR).ToString())
                {
                    Session["dtReporteEscrituras"] = (DataTable) objBL.AutorizacionViajeMenorConsulta(intOficinaConsularId,
                                                                                strNumProyecto,
                                                                                intEstadoId,
                                                                                dateFechaInicio,
                                                                                dateFechaFinal,
                                                                                intFuncionarioAutorizadorId,
                                                                                intTipoActoNotarialId,
                                                                                intSubTipoActoNotarialId,
                                                                                sTipoParticipante,
                                                                                strApellidoPaterno,
                                                                                strApellidoMaterno,
                                                                                strNombres,
                                                                                sTipoDocumento,
                                                                                strNumeroDocumento,
                                                                                FechIniPago,
                                                                                FechFinPago,
                                                                                1,
                                                                                90000,
                                                                                ref IntTotalCountx,
                                                                                ref IntTotalPagesx,
                                                                                strUbigeoDestino);
                    
                }
                else
                {
                    //---------------------------------
                    // Autor del cambio: Miguel Márquez
                    // Fecha del cambio: 15/09/2016
                    // Objetivo: Anular el parametro año.
                    //---------------------------------
                    int IntTotalCountFull = 0;
                    int IntTotalPagesFull = 0;
                    Session["dtReporteEscrituras"] = (DataTable)objBL.ActoProtocolarConsulta(intOficinaConsularId,
                                                                            strNumProyecto,
                                                                            intEstadoId,
                                                                            dateFechaInicio,
                                                                            dateFechaFinal,
                                                                            intFuncionarioAutorizadorId,
                                                                            intTipoActoNotarialId,
                                                                            intSubTipoActoNotarialId,
                                                                            strApellidoPaterno,
                                                                            strApellidoMaterno,
                                                                            strNombres,
                                                                            sTipoDocumento,
                                                                            strNumeroDocumento,
                                                                            sTipoParticipante,
                                                                            sAnio,
                                                                            FechIniPago,
                                                                            FechFinPago,
                                                                            txtRGE.Text == "" ? 0 : Convert.ToInt32(txtRGE.Text),
                                                                            1,
                                                                            10000,
                                                                            ref IntTotalCountFull,
                                                                            ref IntTotalPagesFull,
                                                                            strUbigeoDestino);
                }

                if (dtActosNotariales.Rows.Count == 0)
                {
                    ctrlValProtocolar.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                    ctrlPaginadorProtocolar.Visible = false;
                    ctrlPaginadorProtocolar.PaginaActual = 1;
                    ctrlPaginadorProtocolar.InicializarPaginador();

                    gdvProtocolar.DataSource = null;
                    gdvProtocolar.DataBind();
                }
                else
                {
                    gdvProtocolar.DataSource = dtActosNotariales;
                    gdvProtocolar.DataBind();

                    ctrlValProtocolar.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + IntTotalCount,
                                                        true,
                                                        Enumerador.enmTipoMensaje.INFORMATION);
                }

                ctrlPaginadorProtocolar.TotalResgistros = Convert.ToInt32(IntTotalCount);
                ctrlPaginadorProtocolar.TotalPaginas = Convert.ToInt32(IntTotalPages);

                ctrlPaginadorProtocolar.Visible = false;
                if (ctrlPaginadorProtocolar.TotalPaginas > 1)
                {
                    ctrlPaginadorProtocolar.Visible = true;
                }
            }
            else
            {
                ctrlValProtocolar.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                ctrlPaginadorProtocolar.Visible = false;
                ctrlPaginadorProtocolar.PaginaActual = 1;
                ctrlPaginadorProtocolar.InicializarPaginador();

                gdvProtocolar.DataSource = null;
                gdvProtocolar.DataBind();
            }
        }

        private void CargarFuncionarios(DropDownList ddlFuncionarios, bool bSeleccion = false)
        {
            DataTable dt = new DataTable();
            if (ctrlOficinaConsular.SelectedIndex >= 0)
            {
                dt = funcionario.dtFuncionario(Convert.ToInt32(ctrlOficinaConsular.SelectedValue), 0);
            }
            else
            {
                dt = funcionario.dtFuncionario(0, 0);
            }
            ddlFuncionarios.Items.Clear();
            ddlFuncionarios.DataTextField = "vFuncionario";
            ddlFuncionarios.DataValueField = "iFuncionarioId";
            ddlFuncionarios.DataSource = dt;
            ddlFuncionarios.DataBind();

            if (ctrlOficinaConsular.SelectedIndex >= 0)
            {
                if (bSeleccion)
                    ddlFuncionarios.Items.Insert(0, new ListItem("- TODOS -", "0"));
            }
            else
            {
                ddlFuncionarios.Items.Insert(0, new ListItem("- TODOS -", "0"));
            }
        }

        private void Limpiar()
        {
            txtNroEP.Text = "";
            ddlEstadoEP.SelectedIndex = -1;
            ctrlDateFecExtIni.Text = "";
            ctrlDateFecExtFin.Text = "";
            ddlFuncionarioAutorizador.SelectedIndex = -1;
            ddlTipoActo.SelectedIndex = -1;

            txtPrimerApellido.Text = "";
            txtSegundoApellido.Text = "";
            txtNombres.Text = "";

            gdvProtocolar.DataSource = null;
            gdvProtocolar.DataBind();
            ctrlPaginadorProtocolar.InicializarPaginador();
            ctrlPaginadorProtocolar.Visible = false;
            ddl_UbigeoPaisViajeDestino.SelectedValue = "0";
            ddl_UbigeoRegionViajeDestino.SelectedValue = "0";
            ddl_UbigeoCiudadViajeDestino.SelectedValue = "0";
            //-----------------------------------------------
            ctrlOficinaConsular.ddlOficinaConsular.Attributes["style"] = "border: solid #888888 1px";
            ddlDocumentoTipo.Attributes["style"] = "border: solid #888888 1px";
            txtDocumentoNumero.Attributes["style"] = "border: solid #888888 1px";
            ctrFecPagoIni.Text = "";
            ctrFecPagoFin.Text = "";
            ctrlDateFecExtIni.Text = "";
            ctrlDateFecExtFin.Text = "";
            ddlParticipanteTipo.SelectedIndex = 0;
            ddlDocumentoTipo.SelectedIndex = 0;
            txtDocumentoNumero.Text = "";
            txtRGE.Text = "";

            MostrarSeguimiento(false);
        }

        public DataTable cargar_cboAccionesRectificacionCS(int tipoActo)
        {
            DataTable dtAcciones = new DataTable();
            dtAcciones.Columns.Add("id", typeof(string));
            dtAcciones.Columns.Add("descripcion", typeof(string));

            DataRow lDataRow;

            lDataRow = dtAcciones.NewRow();
            lDataRow["id"] = "0";
            lDataRow["descripcion"] = "- SELECCIONAR -";
            dtAcciones.Rows.Add(lDataRow);

            if (tipoActo == (int)Enumerador.enmProtocolarTipo.TESTAMENTOS_CERRADOS)
            {
                lDataRow = dtAcciones.NewRow();
                lDataRow["id"] = Convert.ToInt32(Enumerador.enmProtocolarAccionModificatoria.REVOCATORIA).ToString();
                lDataRow["descripcion"] = Enumerador.enmProtocolarAccionModificatoria.REVOCATORIA;
                dtAcciones.Rows.Add(lDataRow);
            }
            else if (tipoActo == (int)Enumerador.enmProtocolarTipo.TESTAMENTO_ESCRITURA_PÚBLICA)
            {
                lDataRow = dtAcciones.NewRow();
                lDataRow["id"] = Convert.ToInt32(Enumerador.enmProtocolarAccionModificatoria.REVOCATORIA).ToString();
                lDataRow["descripcion"] = Enumerador.enmProtocolarAccionModificatoria.REVOCATORIA;
                dtAcciones.Rows.Add(lDataRow);
            }
            else if (tipoActo == (int)Enumerador.enmProtocolarTipo.PODER_GENERAL_AMPLIO_ABSOLUTO)
            {
                lDataRow = dtAcciones.NewRow();
                lDataRow["id"] = Convert.ToInt32(Enumerador.enmProtocolarAccionModificatoria.REVOCATORIA).ToString();
                lDataRow["descripcion"] = Enumerador.enmProtocolarAccionModificatoria.REVOCATORIA;
                dtAcciones.Rows.Add(lDataRow);

                lDataRow = dtAcciones.NewRow();
                lDataRow["id"] = Convert.ToInt32(Enumerador.enmProtocolarAccionModificatoria.MODIFICACIÓN).ToString();
                lDataRow["descripcion"] = Enumerador.enmProtocolarAccionModificatoria.MODIFICACIÓN;
                dtAcciones.Rows.Add(lDataRow);

                lDataRow = dtAcciones.NewRow();
                lDataRow["id"] = Convert.ToInt32(Enumerador.enmProtocolarAccionModificatoria.AMPLIACIÓN).ToString();
                lDataRow["descripcion"] = Enumerador.enmProtocolarAccionModificatoria.AMPLIACIÓN;
                dtAcciones.Rows.Add(lDataRow);

                lDataRow = dtAcciones.NewRow();
                lDataRow["id"] = Convert.ToInt32(Enumerador.enmProtocolarAccionModificatoria.SUSTITUCIÓN).ToString();
                lDataRow["descripcion"] = Enumerador.enmProtocolarAccionModificatoria.SUSTITUCIÓN;
                dtAcciones.Rows.Add(lDataRow);

                lDataRow = dtAcciones.NewRow();
                lDataRow["id"] = Convert.ToInt32(Enumerador.enmProtocolarAccionModificatoria.RECTIFICACIÓN).ToString();
                lDataRow["descripcion"] = Enumerador.enmProtocolarAccionModificatoria.RECTIFICACIÓN;
                dtAcciones.Rows.Add(lDataRow);
            }
            else if (tipoActo == (int)Enumerador.enmProtocolarTipo.PODER_ESPECIAL)
            {
                lDataRow = dtAcciones.NewRow();
                lDataRow["id"] = Convert.ToInt32(Enumerador.enmProtocolarAccionModificatoria.REVOCATORIA).ToString();
                lDataRow["descripcion"] = Enumerador.enmProtocolarAccionModificatoria.REVOCATORIA;
                dtAcciones.Rows.Add(lDataRow);

                lDataRow = dtAcciones.NewRow();
                lDataRow["id"] = Convert.ToInt32(Enumerador.enmProtocolarAccionModificatoria.MODIFICACIÓN).ToString();
                lDataRow["descripcion"] = Enumerador.enmProtocolarAccionModificatoria.MODIFICACIÓN;
                dtAcciones.Rows.Add(lDataRow);

                lDataRow = dtAcciones.NewRow();
                lDataRow["id"] = Convert.ToInt32(Enumerador.enmProtocolarAccionModificatoria.AMPLIACIÓN).ToString();
                lDataRow["descripcion"] = Enumerador.enmProtocolarAccionModificatoria.AMPLIACIÓN;
                dtAcciones.Rows.Add(lDataRow);

                lDataRow = dtAcciones.NewRow();
                lDataRow["id"] = Convert.ToInt32(Enumerador.enmProtocolarAccionModificatoria.SUSTITUCIÓN).ToString();
                lDataRow["descripcion"] = Enumerador.enmProtocolarAccionModificatoria.SUSTITUCIÓN;
                dtAcciones.Rows.Add(lDataRow);

                lDataRow = dtAcciones.NewRow();
                lDataRow["id"] = Convert.ToInt32(Enumerador.enmProtocolarAccionModificatoria.RECTIFICACIÓN).ToString();
                lDataRow["descripcion"] = Enumerador.enmProtocolarAccionModificatoria.RECTIFICACIÓN;
                dtAcciones.Rows.Add(lDataRow);
            }

            return dtAcciones;
        }

        public int Consultar_Estado_Cs(Int64 iActoNotarialId, Int64 iActuacionId)
        {
            RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();
            ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();

            int intValor = 0;
            if (iActoNotarialId != 0)
            {
                lACTONOTARIAL.acno_iActoNotarialId = iActoNotarialId;
                lACTONOTARIAL.acno_iActuacionId = iActuacionId;
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

        public static void EjecutarScript(Page Page, string strScript)
        {
            strScript = string.Format(strScript);
            ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), "OpenPopup", strScript, true);
        }

        private void EjecutarScript(string script, string uniqueId)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), uniqueId, script, true);
        }

        private void MostrarSeguimiento(bool bolMostrar = true, Enumerador.enmNotarialTipoFormato enmNotarialFormato = Enumerador.enmNotarialTipoFormato.OTROS)
        {
            switch (enmNotarialFormato)
            {
                case Enumerador.enmNotarialTipoFormato.PARTE:
                    lblListaParte.Visible = bolMostrar;
                    gdvParte.Visible = bolMostrar;
                    break;
                case Enumerador.enmNotarialTipoFormato.TESTIMONIO:
                    lblListaTestimonio.Visible = bolMostrar;
                    gdvTestimonio.Visible = bolMostrar;
                    break;
                default:
                    lblListaParte.Visible = bolMostrar;
                    gdvParte.Visible = bolMostrar;

                    lblListaTestimonio.Visible = bolMostrar;
                    gdvTestimonio.Visible = bolMostrar;
                    break;
            }
        }

        private void VerVistaPrevia(Int64 AcnoNotarialId, String Funcionario, long iActuacionId)
        {

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
                dtCuerpo = _bl.ObtenerCuerpo(AcnoNotarialId);

                string strCuerpo = string.Empty;
                string strDenominacion = string.Empty;
                string strNroEscrituraPublica = string.Empty;
                string vNumeroLibro = string.Empty;

                if (dtCuerpo.Rows.Count == 0)
                {
                    throw new Exception("El cuerpo no se generó correctamente, verique los datos ingresados.");
                }

                if (dtCuerpo.Rows[0]["ancu_vCuerpo"] != null) strCuerpo = dtCuerpo.Rows[0]["ancu_vCuerpo"].ToString();
                if (dtDatosPrincipales.Rows[0]["vDenominacion"] != null) strDenominacion = dtDatosPrincipales.Rows[0]["vDenominacion"].ToString().Trim();
                if (dtDatosPrincipales.Rows[0]["NumeroEscrituraPublica"] != null) strNroEscrituraPublica = dtDatosPrincipales.Rows[0]["NumeroEscrituraPublica"].ToString().Trim();
                if (dtDatosPrincipales.Rows[0]["vNroLibro"] != null) vNumeroLibro = dtDatosPrincipales.Rows[0]["vNroLibro"].ToString().Trim();

                Int16 TotalOtorgante = 0;
                Int16 TotalApoderado = 0;

                #region Participantes

                RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();

                //Obtener
                ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();
                lACTONOTARIAL.acno_iActoNotarialId = AcnoNotarialId;
                lACTONOTARIAL.acno_iActuacionId = iActuacionId;
                lACTONOTARIAL.acno_sTipoActoNotarialId = Convert.ToInt16(Enumerador.enmNotarialTipoActo.PROTOCOLAR);
                lACTONOTARIAL = lActoNotarialConsultaBL.obtener(lACTONOTARIAL);


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
                    #endregion

                    RE_PERSONA lPERSONA = new RE_PERSONA();
                    lPERSONA.pers_iPersonaId = item.anpa_iPersonaId;

                    PersonaConsultaBL lPersonaConsultaBL = new PersonaConsultaBL();
                    PersonaIdentificacionConsultaBL lPersonaIdentificacionConsultaBL = new PersonaIdentificacionConsultaBL();
                    lParticipante.Persona = lPersonaConsultaBL.Obtener(lPERSONA);
                    lParticipante.Identificacion = lPersonaIdentificacionConsultaBL.Obtener(lParticipante.Persona);
                    lParticipante.Persona.Identificacion = lParticipante.Identificacion;

                    lPARAMETRO = new SI_PARAMETRO();
                    lPARAMETRO.para_sParametroId = item.anpa_sTipoParticipanteId;
                    lParticipante.acpa_sTipoParticipanteId_desc = lParametroConsultasBL.Obtener(lPARAMETRO).para_vDescripcion;

                    BE.MRE.SC_MAESTRO.MA_DOCUMENTO_IDENTIDAD lDOCUMENTO_IDENTIDAD = new BE.MRE.SC_MAESTRO.MA_DOCUMENTO_IDENTIDAD();
                    lDOCUMENTO_IDENTIDAD.doid_sTipoDocumentoIdentidadId = lParticipante.Identificacion.peid_sDocumentoTipoId;
                    lParticipante.peid_sDocumentoTipoId_desc = lTablaMaestraConsultaBL.DOCUMENTO_IDENTIDAD_OBTENER(lDOCUMENTO_IDENTIDAD).doid_vDescripcionCorta;

                    lPARAMETRO = new SI_PARAMETRO();
                    lPARAMETRO.para_sParametroId = lPERSONA.pers_sNacionalidadId;
                    lParticipante.pers_sNacionalidadId_desc = lParametroConsultasBL.Obtener(lPARAMETRO).para_vDescripcion;

                    loPARTICIPANTES.Add(lParticipante);

                    Session["ModoEdicionProtocolar"] = true;
                }



                //Session["ParticipanteContainer"] = (List<CBE_PARTICIPANTE>)loPARTICIPANTES;

                #endregion

                loPARTICIPANTES = loPARTICIPANTES.Where(x => x.anpa_cEstado != "E").ToList();

                List<DocumentoFirma> listObjects = new List<DocumentoFirma>();
                DocumentoFirma oDocumentoFirma = null;

                List<string> listOtorgantes = new List<string>();
                List<string> listApoderados = new List<string>();

                foreach (CBE_PARTICIPANTE participante in loPARTICIPANTES)
                {
                    if (participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE) ||
                                            participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO))
                    {
                        oDocumentoFirma = new DocumentoFirma();
                        oDocumentoFirma.bAplicaHuellaDigital = true;
                        oDocumentoFirma.vNombreCompleto = participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno + ", " + participante.Persona.pers_vNombres;
                        oDocumentoFirma.vNroDocumentoCompleto = participante.peid_sDocumentoTipoId_desc + ": " + participante.Identificacion.peid_vDocumentoNumero;
                        listObjects.Add(oDocumentoFirma);

                        if (participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE))
                        {
                            listOtorgantes.Add((participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno + " " + participante.Persona.pers_vNombres).ToUpper());
                            TotalOtorgante++;
                        }
                    }

                    if (participante.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.APODERADO))
                    {
                        listApoderados.Add((participante.Persona.pers_vApellidoPaterno + " " + participante.Persona.pers_vApellidoMaterno + " " + participante.Persona.pers_vNombres).ToUpper());
                        TotalApoderado++;
                    }
                }

                oDocumentoFirma = new DocumentoFirma();
                oDocumentoFirma.bAplicaHuellaDigital = false;
                oDocumentoFirma.vNombreCompleto = Funcionario;
                oDocumentoFirma.vNroDocumentoCompleto = "";
                listObjects.Add(oDocumentoFirma);

                List<TextoPosicionadoITextSharp> listTextoPosicionado = new List<TextoPosicionadoITextSharp>();

                float fFontSize = 12;
                float fDocumentHeight = 842 - fFontSize;
                float fLineaHeight = fFontSize + 1.5f;

                iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Fonts/cour.ttf"), iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);

                SI_LIBRO oSI_LIBRO = new SI_LIBRO();
                oSI_LIBRO.libr_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                oSI_LIBRO.libr_sTipoLibroId = Convert.ToInt16(Enumerador.enmNotarialLibroTipo.ESCRITURA_PUBLICA);

                LibroConsultasBL oLibroConsultasBL = new LibroConsultasBL();
                int iFojaActual = oLibroConsultasBL.ObtenerFojaActual(oSI_LIBRO);

                DocumentoiTextSharp oDocumentoiTextSharp = new DocumentoiTextSharp(this.Page, strCuerpo, HttpContext.Current.Server.MapPath("~/Images/Escudo.JPG"));
                oDocumentoiTextSharp.ListPalabrasOmitirTextoNotarial = null;
                oDocumentoiTextSharp.ListDocumentoFirma = listObjects;
                oDocumentoiTextSharp.NombreConsulado = HttpContext.Current.Session["NombreConsulado"].ToString();
                oDocumentoiTextSharp.CuerpoBaseFont = baseFont;
                oDocumentoiTextSharp.bEsVistaPrevia = true;

                oDocumentoiTextSharp.sTitulo = strDenominacion;
                oDocumentoiTextSharp.bEsEscrituraPublica = true;

                oDocumentoiTextSharp.NumeroEP = dtDatosPrincipales.Rows[0]["NumeroEscrituraPublica"].ToString();

                if (Convert.ToBoolean(dtDatosPrincipales.Rows[0]["Minuta"]))
                {
                    oDocumentoiTextSharp.SMinutaEP = dtDatosPrincipales.Rows[0]["acno_iNumeroActoNotarial"].ToString();
                }
                else
                {
                    oDocumentoiTextSharp.SMinutaEP = "S/N";
                }

                oDocumentoiTextSharp.bGenerarDocumentoAutomaticamente = false;
                oDocumentoiTextSharp.ListOtorgantes = listOtorgantes;
                oDocumentoiTextSharp.ListApoderados = listApoderados;
                oDocumentoiTextSharp.bGenerarDocumentoAutomaticamente = true;
                oDocumentoiTextSharp.bEsBorrador = true;

                RE_ACTONOTARIALDOCUMENTO imagen = new RE_ACTONOTARIALDOCUMENTO();
                imagen.ando_iActoNotarialId = AcnoNotarialId;
                imagen.ando_sTipoDocumentoId = (Int16)Enumerador.enmTipoAdjunto.FOTO;
                List<BE.MRE.RE_ACTONOTARIALDOCUMENTO> lstImagenes = new SGAC.Registro.Actuacion.BL.ActoNotarialMantenimiento().ListaActoNotarialDocumento(imagen);

                if (lstImagenes != null)
                {
                    if (lstImagenes.Count > 0)
                    {
                        List<ImagenNotarial> lstRutasImagen = new List<ImagenNotarial>();
                        String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
                        ImagenNotarial clImagen = new ImagenNotarial();
                        foreach (RE_ACTONOTARIALDOCUMENTO ActoNotarialDocumento in lstImagenes)
                        {
                            clImagen = new ImagenNotarial();
                            clImagen.vRuta = uploadPath + ActoNotarialDocumento.ando_vRutaArchivo;
                            clImagen.vTitulo = ActoNotarialDocumento.ando_vDescripcion;
                            lstRutasImagen.Add(clImagen);
                        }
                        oDocumentoiTextSharp.ListImagenes = lstRutasImagen;
                    }
                }

                oDocumentoiTextSharp.CrearDocumentoPDF();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        protected void ddlActoNotarial_SelectedIndexChanged(object sender, EventArgs e)
        {
            //--------------------------------------------------------------
            // Autor: Miguel Márquez Beltrán
            // Fecha: 16/09/2016
            // Objetivo: Adicionar Tipo de Acto Notarial y remover 
            //           de la lista otras certificaciones notariales
            //--------------------------------------------------------------

            if (ddlActoNotarial.SelectedValue == Convert.ToInt16(Enumerador.enmNotarialTipoActo.EXTRAPROTOCOLAR).ToString())
            {
                Util.CargarParametroDropDownList(ddlTipoActo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_NOTARIAL_TIPO_ACTO_EXTRAPROTOCOLAR), true, " - TODOS - ");
                for (int i = 0; i < ddlTipoActo.Items.Count; i++)
                {
                    if (ddlTipoActo.Items[i].Value == Convert.ToInt16(Enumerador.enmExtraprotocolarTipo.OTRAS_CERTIFICACIONES_NOTARIALES).ToString())
                    {
                        ddlTipoActo.Items.RemoveAt(i);
                    }
                }
                ddlTipoActo.Enabled = true;
                ddlParticipanteTipo.Items.Clear();
                ddlParticipanteTipo.Items.Insert(0, new ListItem("- TODOS -", "0"));
                txtNroEP.Text = "";
                txtNroEP.Enabled = false;
            }
            else
            {
                Util.CargarParametroDropDownList(ddlTipoActo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_NOTARIAL_TIPO_ACTO_PROTOCOLAR), true);

                Util.CargarDropDownList(ddlTipoActo,ObtenerTipoActoNotarial(), "descripcion", "id", true, " - TODOS - ");
               
                ddlTipoActo.Enabled = true;
                ddlParticipanteTipo.Items.Clear();
                ddlParticipanteTipo.Items.Insert(0, new ListItem("- TODOS -", "0"));
                ddlParticipanteTipo.Items.Insert(1, new ListItem("OTORGANTE", Enumerador.enmNotarialTipoParticipante.OTORGANTE.ToString()));
                ddlParticipanteTipo.Items.Insert(2, new ListItem("APODERADO", Enumerador.enmNotarialTipoParticipante.APODERADO.ToString()));
                ddlParticipanteTipo.Items.Insert(3, new ListItem("TESTIGO A RUEGO", Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO.ToString()));
                ddlParticipanteTipo.Items.Insert(4, new ListItem("INTERPRETE", Enumerador.enmNotarialTipoParticipante.INTERPRETE.ToString()));
                txtNroEP.Enabled = true;
                txtNroEP.Focus();
            }
        }

        protected void ddl_UbigeoPaisViajeDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_UbigeoPaisViajeDestino.SelectedIndex > 0)
            {
                comun_Part3.CargarUbigeo(Session, ddl_UbigeoRegionViajeDestino, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_UbigeoPaisViajeDestino.SelectedValue.ToString(), "", true);
                comun_Part3.CargarUbigeo(Session, ddl_UbigeoCiudadViajeDestino, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, string.Empty, string.Empty, true);
               
                ddl_UbigeoPaisViajeDestino.Focus();
            }
            else
            {
                ddl_UbigeoRegionViajeDestino.SelectedIndex = 0;
                ddl_UbigeoCiudadViajeDestino.SelectedIndex = 0;
            }
         
        }
        protected void ddl_UbigeoRegionViajeDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_UbigeoRegionViajeDestino.SelectedIndex > 0)
            {
                comun_Part3.CargarUbigeo(Session, ddl_UbigeoCiudadViajeDestino, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddl_UbigeoPaisViajeDestino.SelectedValue.ToString(), ddl_UbigeoRegionViajeDestino.SelectedValue.ToString(), true);

                ddl_UbigeoRegionViajeDestino.Focus();
            }
            else
            {
                ddl_UbigeoCiudadViajeDestino.SelectedIndex = 0;
            }
        }

        protected void ddl_UbigeoCiudadViajeDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
                ddl_UbigeoCiudadViajeDestino.Focus();
        }

        private DataTable ObtenerTipoActoNotarial()
        {
            DataTable dtTipoActoNotarial = new DataTable();

            TipoActoProtocolarTarifarioConsultasBL objTipoActoProtocolarTarifarioConsultaBL = new TipoActoProtocolarTarifarioConsultasBL();

            dtTipoActoNotarial = objTipoActoProtocolarTarifarioConsultaBL.Consultar_TipoActoProtocolar();

            return dtTipoActoNotarial;

        }
        //----------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 21/09/2016
        // Objetivo: Validar los campos obligatorios
        //----------------------------------------------
        private bool validarConsulta()
        {
            bool bCorrecto = true;
            if ((int)Session[Constantes.CONST_SESION_OFICINACONSULAR_ID] == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                if (ctrlOficinaConsular.ddlOficinaConsular.Items.Count > 1)
                {
                    if (ctrlOficinaConsular.ddlOficinaConsular.SelectedIndex == 0)
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, "Se debe seleccionar la oficina consular."));
                        ctrlOficinaConsular.ddlOficinaConsular.Focus();
                        ctrlOficinaConsular.ddlOficinaConsular.Attributes["style"] = "border: solid Red 1px";

                        return false;
                    }
                    else
                    {
                        ctrlOficinaConsular.ddlOficinaConsular.Attributes["style"] = "border: solid #888888 1px";
                    }
                }
            }
            //if (ddlTipoActo.SelectedIndex == 0)
            //{
            //    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, "Se debe seleccionar el tipo de acto notarial."));
            //    ddlTipoActo.Focus();
            //    ddlTipoActo.Attributes["style"] = "border: solid Red 1px";
            //    return false;
            //}
            //else
            //{
            //    ddlTipoActo.Attributes["style"] = "border: solid #888888 1px";
            //}
            //if (ddlEstadoEP.SelectedIndex == 0)
            //{
            //    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, "Se debe seleccionar el estado del trámite."));
            //    ddlEstadoEP.Focus();
            //    ddlEstadoEP.Attributes["style"] = "border: solid Red 1px";
            //    return false;
            //}
            //else
            //{
            //    ddlEstadoEP.Attributes["style"] = "border: solid #888888 1px";
            //}
            //---------------------------------------------------------
            //FECHA: 03/03/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: La fecha es opcional no debe validarse
            //---------------------------------------------------------
            //if (ctrlDateFecExtIni.Text.Trim() == string.Empty)
            //{
            //    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, "Se debe ingresar la fecha de extensión (inicio)."));
            //    ctrlDateFecExtIni.Focus();
            //    TextBox txtFecha = (TextBox) ctrlDateFecExtIni.FindControl("TxtFecha");
            //    txtFecha.Attributes["style"] = "border: solid Red 1px";
            //    return false;
            //}
            //else
            //{
            //    TextBox txtFecha = (TextBox)ctrlDateFecExtIni.FindControl("TxtFecha");
            //    txtFecha.Attributes["style"] = "border: solid #888888 1px";
            //}

            //if (ctrlDateFecExtFin.Text.Trim() == string.Empty)
            //{
            //    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, "Se debe ingresar la fecha de extensión (fin)."));
            //    ctrlDateFecExtFin.Focus();
            //    TextBox txtFecha = (TextBox)ctrlDateFecExtFin.FindControl("TxtFecha");
            //    txtFecha.Attributes["style"] = "border: solid Red 1px";
            //    return false;
            //}
            //else
            //{
            //    TextBox txtFecha = (TextBox)ctrlDateFecExtFin.FindControl("TxtFecha");
            //    txtFecha.Attributes["style"] = "border: solid #888888 1px";
            //}


            bool bDocumentoTipo = false;
            bool bDocumentoNumero = false;

            ddlDocumentoTipo.Attributes["style"] = "border: solid #888888 1px";
            txtDocumentoNumero.Attributes["style"] = "border: solid #888888 1px";

            if (ddlDocumentoTipo.SelectedIndex > 0)
            {
                bDocumentoTipo = true;
            }
            if (txtDocumentoNumero.Text.Trim().Length > 0)
            {
                bDocumentoNumero = true;
            }
            if (bDocumentoTipo != bDocumentoNumero)
            {
                if (!bDocumentoTipo)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, "Se debe ingresar el tipo de documento."));
                    ddlDocumentoTipo.Focus();
                    ddlDocumentoTipo.Attributes["style"] = "border: solid Red 1px";
                }
                else
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, "Se debe ingresar el número de documento."));
                    txtDocumentoNumero.Focus();
                    txtDocumentoNumero.Attributes["style"] = "border: solid Red 1px";
                }
                return false;
            }

            return bCorrecto;
        }

        //------------------------------------------
        //Fecha: 02/04/2019
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Crear GUID 
        //------------------------------------------        
        private string _pageUniqueId = Guid.NewGuid().ToString();

        public string PageUniqueId
        {
            get { return _pageUniqueId; }
        }
        //---------------------------------------------------------------------------
        private void GenerarParte(Int64 intActuacionId, Int64 intActoNotarialId)
        {
                        
            SGAC.BE.MRE.RE_ACTUACIONDETALLE objAD = new SGAC.BE.MRE.RE_ACTUACIONDETALLE();
            SGAC.BE.MRE.RE_PAGO objPago = new SGAC.BE.MRE.RE_PAGO();
            SGAC.BE.MRE.RE_ACTONOTARIALDETALLE objANDE = new SGAC.BE.MRE.RE_ACTONOTARIALDETALLE();

            short intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            //------------------------------------------------------------------------------------
            ActoNotarialConsultaBL objActuacionConsulta = new ActoNotarialConsultaBL();
            DataTable dtActoNotarialSeguimiento = new DataTable();

            short intTipoFormato = (short)Enumerador.enmNotarialTipoFormato.PARTE;

            short intTipoActoprotocolar = Convert.ToInt16(Enumerador.enmNotarialTipoActo.PROTOCOLAR);

            dtActoNotarialSeguimiento = objActuacionConsulta.ObtenerActoNotarialDetalle(intActuacionId, intTipoFormato);

            if (dtActoNotarialSeguimiento.Rows.Count > 0)
            {
                //---------------------------------------------
                //Parte adicional
                //---------------------------------------------
                objAD.acde_iActuacionId = intActuacionId;
                objAD.acde_sTarifarioId = 52; // 17C - EXPEDICION PARTE ADICIONAL
                objAD.acde_sItem = 1;
                objAD.acde_dFechaRegistro = DateTime.Now;
                objAD.acde_bRequisitosFlag = false;
                objAD.acde_ICorrelativoActuacion = 0;
                objAD.acde_ICorrelativoTarifario = 0;
                objAD.acde_vNotas = "CONSULTA";
                objAD.acde_sEstadoId = (Int16)Enumerador.enmNotarialProtocolarEstado.PAGADA;
                objAD.acde_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objAD.acde_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
                //---------------------------------------------
                objPago.pago_sPagoTipoId = Convert.ToInt16(Enumerador.enmTipoCobroActuacion.EFECTIVO);
                objPago.pago_dFechaOperacion = DateTime.Now;
                objPago.pago_sMonedaLocalId = Convert.ToInt16(Session[Constantes.CONST_SESION_TIPO_MONEDA_ID].ToString());
                                
                objPago.pago_FTipCambioBancario = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO_BANCARIO].ToString());
                objPago.pago_FTipCambioConsular = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO].ToString());
                objPago.pago_sBancoId = 0;
                objPago.pago_vBancoNumeroOperacion = "";
                objPago.pago_bPagadoFlag = true;
                objPago.pago_vComentario = "";
                objPago.pago_vNumeroVoucher = "";
                objPago.pago_vSustentoTipoPago = "";
                objPago.pago_iNormaTarifarioId = 0;
                objPago.pago_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objPago.pago_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
                //---------------------------------------------
                objANDE.ande_sTipoFormatoId = intTipoFormato;

                RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();
                lACTONOTARIAL.acno_iActoNotarialId = Convert.ToInt64(intActoNotarialId);
                lACTONOTARIAL.acno_iActuacionId = Convert.ToInt64(intActuacionId);
                lACTONOTARIAL.acno_sTipoActoNotarialId = Convert.ToInt16(Enumerador.enmNotarialTipoActo.PROTOCOLAR);
                lACTONOTARIAL = objActuacionConsulta.obtener(lACTONOTARIAL);
                //--------------------------------------------
                objANDE.ande_IFuncionarioAutorizadorId = lACTONOTARIAL.acno_IFuncionarioAutorizadorId;
                objANDE.ande_vNumeroOficio = lACTONOTARIAL.acno_vNumeroOficio;

                Int16 intFojaInicial = Convert.ToInt16(lACTONOTARIAL.acno_vNumeroFojaInicial.ToString());
                Int16 intFojaFinal = Convert.ToInt16(lACTONOTARIAL.acno_vNumeroFojaFinal.ToString());
                Int16 intNumeroFojas = Convert.ToInt16(intFojaFinal - intFojaInicial + 1);

                objANDE.ande_sNumeroFoja = intNumeroFojas;
                objANDE.ande_vPresentanteNombre = lACTONOTARIAL.acno_vPresentanteNombre;
                objANDE.ande_sPresentanteTipoDocumento = lACTONOTARIAL.acno_sPresentanteTipoDocumento;
                objANDE.ande_vPresentanteNumeroDocumento = lACTONOTARIAL.acno_vPresentanteNumeroDocumento;
                objANDE.ande_sPresentanteGenero = lACTONOTARIAL.acno_sPresentanteGenero;
                //-----------------------------------------------
                double dblTipoCambioConsular = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO].ToString());
                double decTotalSC = 16 + intNumeroFojas * 8;                
                objPago.pago_FMontoSolesConsulares = decTotalSC;
                objPago.pago_FMontoMonedaLocal = decTotalSC * dblTipoCambioConsular;

                objANDE.ande_dFechaExtension = DateTime.Now;
                objANDE.ande_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objANDE.ande_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
                //---------------------------------------------
            }
            ActoNotarialMantenimiento objActoNotarialBL = new ActoNotarialMantenimiento();
            string strScript = "";

            objActoNotarialBL.Registro_ActuacionDetalle_Pago_ActoNotarial(objAD, objPago, objANDE, intOficinaConsularId);
            if (objANDE.Error == false)
            {
                Int64 intActuacionDetalleid = objAD.acde_iActuacionDetalleId;
                Int64 intActoNotarialDetalleId = objANDE.ande_iActoNotarialDetalleId;

                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Parte Notarial", "Parte Notarial registrado correctamente.");
                ActoNotarialConsultaBL objBL = new ActoNotarialConsultaBL();
                DataTable dtPartes = new DataTable();
                dtPartes = objBL.ObtenerActoNotarialDetalle(intActuacionId, (Int16)Enumerador.enmNotarialTipoFormato.PARTE);

                gdvParte.DataSource = dtPartes;
                gdvParte.DataBind();
            }
            else
            {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Parte Notarial", objANDE.Message);
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "Mensaje", strScript, true);
        }

        protected void gdvParte_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e != null)
            {
                if (e.Row != null)
                {
                    if (e.Row.RowIndex > -1)
                    {
                        ImageButton btnGenerarParte = e.Row.FindControl("btnGenerarParte") as ImageButton;
                        string strCodigoUnicoFabrica = gdvParte.DataKeys[e.Row.RowIndex].Values["insu_vCodigoUnicoFabrica"].ToString();
                        string strTarifa = gdvParte.DataKeys[e.Row.RowIndex].Values["tari_vTarifa"].ToString();

                        if (strCodigoUnicoFabrica.Length > 0 && strTarifa.Equals("17B"))
                        {
                            btnGenerarParte.Visible = true;
                        }
                        else
                        {
                            btnGenerarParte.Visible = false;
                        }
                    }
                }
            }
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            ImageButton btnEditar2 = e.Row.FindControl("btnGenerarParte") as ImageButton;

            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                ImageButton[] arrImageButtons = { btnEditar2 };
                Comun.ModoLectura(ref arrImageButtons);
            }
        }

        protected void gdvParte_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToString())
            {
                case "GenerarParte":
                    //-------------------------------------------------------------
                    //Fecha: 23/12/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Generar el Parte.
                    //-------------------------------------------------------------
                    if (ViewState["ActuacionId"] != null && ViewState["ActoNotarialId"] != null)
                    {
                        if (ViewState["ActuacionId"].ToString().Length > 0 && ViewState["ActoNotarialId"].ToString().Length > 0)
                        {
                            Int64 intActuacionId = Convert.ToInt64(ViewState["ActuacionId"].ToString());
                            Int64 intActoNotarialId = Convert.ToInt64(ViewState["ActoNotarialId"].ToString());
                            if (intActuacionId > 0 && intActoNotarialId > 0)
                            {
                                GenerarParte(intActuacionId, intActoNotarialId);
                            }
                        }
                    }
                    //-------------------------------------------------------------
                    break;
            }
        }

        protected void gdvTestimonio_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToString())
            {
                case "GenerarTestimonio":
                    //-------------------------------------------------------------
                    //Fecha: 23/12/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Generar el Parte.
                    //-------------------------------------------------------------
                    if (ViewState["ActuacionId"] != null && ViewState["ActoNotarialId"] != null)
                    {
                        if (ViewState["ActuacionId"].ToString().Length > 0 && ViewState["ActoNotarialId"].ToString().Length > 0)
                        {
                            Int64 intActuacionId = Convert.ToInt64(ViewState["ActuacionId"].ToString());
                            Int64 intActoNotarialId = Convert.ToInt64(ViewState["ActoNotarialId"].ToString());
                            if (intActuacionId > 0 && intActoNotarialId > 0)
                            {
                                GenerarTestimonio(intActuacionId, intActoNotarialId);
                            }
                        }
                    }
                    //-------------------------------------------------------------
                    break;
            }
        }

        protected void gdvTestimonio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e != null)
            {
                if (e.Row != null)
                {
                    if (e.Row.RowIndex > -1)
                    {
                        ImageButton btnGenerarTestimonio = e.Row.FindControl("btnGenerarTestimonio") as ImageButton;
                        string strCodigoUnicoFabrica = gdvTestimonio.DataKeys[e.Row.RowIndex].Values["insu_vCodigoUnicoFabrica"].ToString();
                        string strTarifa = gdvTestimonio.DataKeys[e.Row.RowIndex].Values["tari_vTarifa"].ToString();

                        if (strCodigoUnicoFabrica.Length > 0 && strTarifa.Equals("17A"))
                        {
                            btnGenerarTestimonio.Visible = true;
                        }
                        else
                        {
                            btnGenerarTestimonio.Visible = false;
                        }
                    }
                }
            }
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            ImageButton btnEditar2 = e.Row.FindControl("btnGenerarTestimonio") as ImageButton;

            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                ImageButton[] arrImageButtons = { btnEditar2 };
                Comun.ModoLectura(ref arrImageButtons);
            }

        }

        private void GenerarTestimonio(Int64 intActuacionId, Int64 intActoNotarialId)
        {

            SGAC.BE.MRE.RE_ACTUACIONDETALLE objAD = new SGAC.BE.MRE.RE_ACTUACIONDETALLE();
            SGAC.BE.MRE.RE_PAGO objPago = new SGAC.BE.MRE.RE_PAGO();
            SGAC.BE.MRE.RE_ACTONOTARIALDETALLE objANDE = new SGAC.BE.MRE.RE_ACTONOTARIALDETALLE();

            short intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            //------------------------------------------------------------------------------------
            ActoNotarialConsultaBL objActuacionConsulta = new ActoNotarialConsultaBL();
            DataTable dtActoNotarialSeguimiento = new DataTable();

            short intTipoFormato = (short)Enumerador.enmNotarialTipoFormato.TESTIMONIO;

            short intTipoActoprotocolar = Convert.ToInt16(Enumerador.enmNotarialTipoActo.PROTOCOLAR);

            dtActoNotarialSeguimiento = objActuacionConsulta.ObtenerActoNotarialDetalle(intActuacionId, intTipoFormato);

            if (dtActoNotarialSeguimiento.Rows.Count > 0)
            {
                //---------------------------------------------
                //Parte adicional
                //---------------------------------------------
                objAD.acde_iActuacionId = intActuacionId;
                objAD.acde_sTarifarioId = 50; // 17A - TESTIMONIO DE ESCRITURA PUBLICA
                objAD.acde_sItem = 1;
                objAD.acde_dFechaRegistro = DateTime.Now;
                objAD.acde_bRequisitosFlag = false;
                objAD.acde_ICorrelativoActuacion = 0;
                objAD.acde_ICorrelativoTarifario = 0;
                objAD.acde_vNotas = "CONSULTA";
                objAD.acde_sEstadoId = (Int16)Enumerador.enmNotarialProtocolarEstado.PAGADA;
                objAD.acde_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objAD.acde_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
                //---------------------------------------------
                objPago.pago_sPagoTipoId = Convert.ToInt16(Enumerador.enmTipoCobroActuacion.EFECTIVO);
                objPago.pago_dFechaOperacion = DateTime.Now;
                objPago.pago_sMonedaLocalId = Convert.ToInt16(Session[Constantes.CONST_SESION_TIPO_MONEDA_ID].ToString());

                objPago.pago_FTipCambioBancario = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO_BANCARIO].ToString());
                objPago.pago_FTipCambioConsular = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO].ToString());
                objPago.pago_sBancoId = 0;
                objPago.pago_vBancoNumeroOperacion = "";
                objPago.pago_bPagadoFlag = true;
                objPago.pago_vComentario = "";
                objPago.pago_vNumeroVoucher = "";
                objPago.pago_vSustentoTipoPago = "";
                objPago.pago_iNormaTarifarioId = 0;
                objPago.pago_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objPago.pago_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
                //---------------------------------------------
                objANDE.ande_sTipoFormatoId = intTipoFormato;

                RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();
                lACTONOTARIAL.acno_iActoNotarialId = Convert.ToInt64(intActoNotarialId);
                lACTONOTARIAL.acno_iActuacionId = Convert.ToInt64(intActuacionId);
                lACTONOTARIAL.acno_sTipoActoNotarialId = Convert.ToInt16(Enumerador.enmNotarialTipoActo.PROTOCOLAR);
                lACTONOTARIAL = objActuacionConsulta.obtener(lACTONOTARIAL);
                //--------------------------------------------
                objANDE.ande_IFuncionarioAutorizadorId = lACTONOTARIAL.acno_IFuncionarioAutorizadorId;
                objANDE.ande_vNumeroOficio = lACTONOTARIAL.acno_vNumeroOficio;

                Int16 intFojaInicial = Convert.ToInt16(lACTONOTARIAL.acno_vNumeroFojaInicial.ToString());
                Int16 intFojaFinal = Convert.ToInt16(lACTONOTARIAL.acno_vNumeroFojaFinal.ToString());
                Int16 intNumeroFojas = Convert.ToInt16(intFojaFinal - intFojaInicial + 1);

                objANDE.ande_sNumeroFoja = intNumeroFojas;
                objANDE.ande_vPresentanteNombre = lACTONOTARIAL.acno_vPresentanteNombre;
                objANDE.ande_sPresentanteTipoDocumento = lACTONOTARIAL.acno_sPresentanteTipoDocumento;
                objANDE.ande_vPresentanteNumeroDocumento = lACTONOTARIAL.acno_vPresentanteNumeroDocumento;
                objANDE.ande_sPresentanteGenero = lACTONOTARIAL.acno_sPresentanteGenero;
                //-----------------------------------------------
                double dblTipoCambioConsular = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO].ToString());
                double decTotalSC = 16 + intNumeroFojas * 8;
                objPago.pago_FMontoSolesConsulares = decTotalSC;
                objPago.pago_FMontoMonedaLocal = decTotalSC * dblTipoCambioConsular;

                objANDE.ande_dFechaExtension = DateTime.Now;
                objANDE.ande_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objANDE.ande_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
                //---------------------------------------------
            }
            ActoNotarialMantenimiento objActoNotarialBL = new ActoNotarialMantenimiento();
            string strScript = "";

            objActoNotarialBL.Registro_ActuacionDetalle_Pago_ActoNotarial(objAD, objPago, objANDE, intOficinaConsularId);
            if (objANDE.Error == false)
            {
                Int64 intActuacionDetalleid = objAD.acde_iActuacionDetalleId;
                Int64 intActoNotarialDetalleId = objANDE.ande_iActoNotarialDetalleId;

                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Testimonio Notarial", "Testimonio Notarial registrado correctamente.");

                ActoNotarialConsultaBL objBL = new ActoNotarialConsultaBL();

                DataTable dtTestimonios = new DataTable();
                dtTestimonios = objBL.ObtenerActoNotarialDetalle(intActuacionId, (Int16)Enumerador.enmNotarialTipoFormato.TESTIMONIO);

                gdvTestimonio.DataSource = dtTestimonios;
                gdvTestimonio.DataBind();

            }
            else
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Testimonio Notarial", objANDE.Message);
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "Mensaje", strScript, true);
        }      
        //---------------------------------------------------------------------------
    }
}