using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;
using SGAC.Accesorios;
using SGAC.WebApp.Accesorios;

namespace SGAC.WebApp.Contabilidad
{
    public partial class FrmReporteContables : MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                reporte();
            }
        }

        ReportParameter[] parameters;
        String sNombreOficinaConsular = String.Empty;
        //---------------------------------------------
        //Fecha: 21/11/2016
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Obtener el páis del consulado
        //---------------------------------------------

        String sNombrePaisConsular = String.Empty;

        String sNombreDsReporteServices = String.Empty;
        String sNombreDsReporteServices1 = String.Empty;
        String sNombreDsReporteServices2 = String.Empty;
        String sNombreDsReporteServices3 = String.Empty;

        String strRutaBase = String.Empty;
        
        Int32 iNroDate = 0;
        Int32 iNroDate1 = 0;
        Int32 iNroDate2 = 0;

        DataTable dt = null;
        DataTable dt1 = null;
        DataTable dt2 = null;
        DataTable dt3 = null;

        string strFechaActualConsulado = "";
        string strHoraActualConsulado = "";

        private void reporte()
        {
            string reporteLista = string.Empty;
            if (Request.QueryString["lst"] != null)
            {
                reporteLista = Request.QueryString["lst"];
            }
            dt = new DataTable();
            dt1 = new DataTable();
            dt2 = new DataTable();
            dt3 = new DataTable();
            List<csReporteRGE> lst = new List<csReporteRGE>();

            if (reporteLista == "1")
            {
                lst = (List<csReporteRGE>)Session["dtDatos"];
                
            }
            else {
                dt = (DataTable)Session["dtDatos"];
            }
            

            Enumerador.enmReporteContable enmReporte = (Enumerador.enmReporteContable)Session[Constantes.CONST_SESION_REPORTE_TIPO];

            sNombreOficinaConsular = comun_Part2.ObtenerNombreOficinaPorId(Session, Convert.ToInt32(Session["IdOficinaConsular_contabilidad"]));
            sNombrePaisConsular = comun_Part2.ObtenerPaisOficinaPorId( Convert.ToInt32(Session["IdOficinaConsular_contabilidad"]));

            //---------------------------------------------------
            //Fecha: 12/05/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Validar texto en blanco.
            //---------------------------------------------------
            if (sNombreOficinaConsular.Length > 0)
            {
                sNombreOficinaConsular = sNombreOficinaConsular.Split('-')[1].ToString().Trim();
            }
            else
            {
                sNombreOficinaConsular = "TODOS";
            }
                
            //---------------------------------------------------

            if (sNombrePaisConsular == "")
            {
                sNombrePaisConsular = comun_Part2.ObtenerPaisOficinaPorIdDT(Session, Convert.ToInt32(Session["IdOficinaConsular_contabilidad"]));
            }

            //-----------------------------------------------------
            // Autor: Miguel Márquez Beltrán
            // Fecha: 19/11/2019
            // Objetivo: Consulta de fecha y hora unificada.
            //-----------------------------------------------------

            //string strFechaActualConsulado = "";
            //string strHoraActualConsulado = "";

            Comun.ObtenerFechaHoraActualTexto(HttpContext.Current.Session, ref strFechaActualConsulado, ref strHoraActualConsulado);

            strFechaActualConsulado = Comun.FormatearFecha(strFechaActualConsulado).ToString("MMM-dd-yyyy");
            //----------------------------
            //strFechaActualConsulado = Comun.FormatearFecha((Accesorios.Comun.ObtenerFechaActualTexto(HttpContext.Current.Session))).ToString("MMM-dd-yyyy");
            //strHoraActualConsulado = Accesorios.Comun.ObtenerHoraActualTexto(HttpContext.Current.Session);


            switch (enmReporte)
            {
                case Enumerador.enmReporteContable.SALDOS_CONSULARES:
                    #region Saldo Consular
                    rptSaldoConsular();
                    #endregion
                    break;
                case Enumerador.enmReporteContable.REMESA:
                    #region REMESAS
                    rptRemesa();
                    #endregion
                    break;
                case Enumerador.enmReporteContable.ESTADO_BANCARIO:
                    #region Estado Bancario
                    rptEstadoBancario();
                    #endregion
                    break;
                case Enumerador.enmReporteContable.DOCUMENTO_UNICO:
                    #region Reporte de Libro de Documentos Unicos
                    rptRegistroUnico();
                    #endregion
                    break;
                case Enumerador.enmReporteContable.REGISTRO_GENERAL_ENTRADAS:
                    #region Reporte Registro General de Entradas
                    rptRegistroGeneralesEntrada();
                    #endregion
                    break;
                //------------------------------------------------------------------------
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 05/09/2016
                // Objetivo: Parametros para el listado de actuaciones anuladas
                //------------------------------------------------------------------------
                case Enumerador.enmReporteContable.REPORTE_ACTUACIONES_ANULADAS:
                    rptActuacionesAnuladas();
                    break;
                //------------------------------------------------------------------------
                case Enumerador.enmReporteContable.CAJA:
                    rptLibroCaja();
                    break;
                case Enumerador.enmReporteContable.AUTOADHESIVO:
                    rptAutodhesivoConsulares();
                    break;
                case Enumerador.enmReporteContable.CONCILIACION:
                    rptConciliacionBancaria();
                    break;
                case Enumerador.enmReporteContable.AUTOADHESIVOS_USUARIO_OFICINA_CONSULAR:
                    rptAutoadhesivosxUsuarioOficinaConsular();
                    break;
                case Enumerador.enmReporteContable.ACTUACIONES_USUARIO_OFICINA_CONSULAR:
                    rptActuacionesxUsuarioOficinaConsular();
                    break;
                case Enumerador.enmReporteContable.REGCIVIL_ACTA_NACIMIENTO:
                    rptRegCivilActaNacimiento();
                    break;

                //------------------------------------------------------------------------
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 09/01/2017
                // Objetivo: Parametros para el reporte de Formato de Envio RENIEC
                //------------------------------------------------------------------------
                case Enumerador.enmReporteContable.RENIEC_FORMATO_ENVIO_TARIFA:
                    rptFormatoEnvioRENIEC();
                    break;
                //------------------------------------------------------------------------
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 09/01/2017
                // Objetivo: Parametros para el reporte de Formato de Envio RENIEC
                //------------------------------------------------------------------------
                case Enumerador.enmReporteContable.RENIEC_FORMATO_ENVIO_ESTADO:
                    rptFormatoEnvioRecuperadosRENIEC();
                    break;
                //------------------------------------------------------------------------
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 29/12/2016
                // Objetivo: Parametros para el reporte de Guía de Remisión RENIEC
                //------------------------------------------------------------------------
                case Enumerador.enmReporteContable.RENIEC_GUIA_DESPAHO:
                    rptGuiaDespachoRENIEC();
                    break;
                //------------------------------------------------------------------------
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 21/11/2016
                // Objetivo: Parametros para el reporte de rendición de cuentas RENIEC
                //------------------------------------------------------------------------
                case Enumerador.enmReporteContable.RENIEC_RENDICION_CUENTAS:
                    rptRendicionCuentasRENIEC();
                    break;
                //------------------------------------------------------
                //Fecha: 12/05/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Exportar e Imprimir el tipo de reporte:
                //        Rendición de Cuentas por Tarifas RENIEC.
                //Documento: OBSERVACIONES_SGAC_12052021.doc
                //------------------------------------------------------
                case Enumerador.enmReporteContable.RENIEC_RENDICION_CUENTAS_NOHEAD:
                    rptRendicionCuentasRENIEC(true);
                    break;

                // Autor: Jonatan Silva Cachay
                // Fecha: 13/06/2017
                // Objetivo: Parametros para el reporte de Tramites Incompletos
                //------------------------------------------------------------------------
                case Enumerador.enmReporteContable.CONST_REPORTE_RENIEC_INCOMPLETOS:
                    rptTramitesIncompletos();
                    break;

                //--------------------------------------------------------------------------------
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 23/07/2018
                // Objetivo: Parametros para el reporte de Certificado de Antecedentes Penales
                //--------------------------------------------------------------------------------
                case Enumerador.enmReporteContable.CERTIFICADOS_CONSULARES_ANTECEDENTES_PENALES:
                    rptAntecedentesPenales(false);
                    break;
                //--------------------------------------------------------------------------------
                // Autor: JONATAN SILVA
                // Fecha: 26/07/2021
                // Objetivo: Parametros para el reporte de Certificado de Antecedentes Penales agrupado por usuario
                //--------------------------------------------------------------------------------
                case Enumerador.enmReporteContable.CERTIFICADOS_CONSULARES_ANTECEDENTES_PENALES_USUARIO:
                    rptAntecedentesPenales(true);
                    break;
                // Autor: Jonatan Silva Cachay
                // Fecha: 19/09/2019
                // Objetivo: Parametros para el reporte de Conciliacón RENIEC
                //------------------------------------------------------------------------
                case Enumerador.enmReporteContable.REPORTE_RENIEC_CONCILIACION:
                    rptConciliacionRENIEC();
                    break;
                //--------------------------------------------------------------------------------
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 16/09/2019
                // Objetivo: Parametros para el Certificado de Antecedente Penal
                //--------------------------------------------------------------------------------
                case Enumerador.enmReporteContable.CERTIFICADO_ANTECEDENTE_PENAL:
                    rptCertificadoAntecedentePenal(reporteLista);
                    break;

}

            dsReport.LocalReport.ReportEmbeddedResource = strRutaBase;
            dsReport.LocalReport.ReportPath = strRutaBase;

            dsReport.LocalReport.DataSources.Clear();

            ReportDataSource datasource = null;
            if (reporteLista == "1")
            {
                datasource = new ReportDataSource(sNombreDsReporteServices, lst);
            }
            else {
                datasource = new ReportDataSource(sNombreDsReporteServices, dt);
            }
            
            ReportDataSource datasource1 = null;
            ReportDataSource datasource2 = null;
            ReportDataSource datasource3 = null;

            if (iNroDate == 1)
            {
                dt1 = (DataTable)Session["dtDatos1"];
                datasource1 = new ReportDataSource(sNombreDsReporteServices1, dt1);
            }

            if (iNroDate1 == 2)
            {
                dt2 = (DataTable)Session["dtDatos2"];
                datasource2 = new ReportDataSource(sNombreDsReporteServices2, dt2);
            }
            if (iNroDate2 == 3)
            {
                dt3 = (DataTable)Session["dtDatos3"];
                datasource3 = new ReportDataSource(sNombreDsReporteServices3, dt3);
            }

            dsReport.LocalReport.SetParameters(parameters);
            
            
            dsReport.LocalReport.DataSources.Add(datasource);

            if (iNroDate == 1)
            {
                dsReport.LocalReport.DataSources.Add(datasource1);
            }
            if (iNroDate1 == 2)
            {
                dsReport.LocalReport.DataSources.Add(datasource2);
            }
            if (iNroDate2 == 3)
            {
                dsReport.LocalReport.DataSources.Add(datasource3);
            }
            
            //--------------------------------------------
            //Eliminar la sesión
            //--------------------------------------------
            Session.Remove("dtDatos");
            Session.Remove("dtDatos1");
            Session.Remove("dtDatos2");
            Session.Remove("dtDatos3");

            //--------------------------------------------
        }


        private void rptAutoadhesivosxUsuarioOficinaConsular()
        {
            iNroDate = 0;
            parameters = new ReportParameter[7];
            parameters[0] = new ReportParameter("TituloReporte", "AUTOADHESIVOS POR USUARIO");
            parameters[1] = new ReportParameter("SubTituloReporte", Constantes.CONST_REPORTE_SUB_TITULO);
            parameters[2] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
            parameters[3] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
            parameters[4] = new ReportParameter("FechaHaber", Session["FechaIntervalo"].ToString());
            parameters[5] = new ReportParameter("FechaActual", strFechaActualConsulado);
            parameters[6] = new ReportParameter("HoraActual", strHoraActualConsulado);

            sNombreDsReporteServices = "rsAutoadhesivosUsuario";
            strRutaBase = Server.MapPath("~/Contabilidad/rsAutoAdhesivosUsuarioOficinaConsular.rdlc");
        }

        private void rptActuacionesxUsuarioOficinaConsular()
        {
            iNroDate = 0;
            parameters = new ReportParameter[11];
            parameters[0] = new ReportParameter("TituloReporte", "ACTUACIONES - DETALLE POR USUARIO");
            parameters[1] = new ReportParameter("SubTituloReporte", Constantes.CONST_REPORTE_SUB_TITULO);
            parameters[2] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
            parameters[3] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
            parameters[4] = new ReportParameter("FechaHaber", Session["FechaIntervalo"].ToString());
            parameters[5] = new ReportParameter("FechaActual", strFechaActualConsulado);
            parameters[6] = new ReportParameter("HoraActual", strHoraActualConsulado);

            string Parametro = Session["ParametrosReporte"].ToString();
            string[] parametros;
            parametros = Parametro.Split('|');

            parameters[7] = new ReportParameter("TipoPago", parametros[0].ToString());
            parameters[8] = new ReportParameter("Usuarios", parametros[1].ToString());
            parameters[9] = new ReportParameter("Tarifa", parametros[2].ToString());
            parameters[10] = new ReportParameter("Clasificacion", parametros[3].ToString());
            

            sNombreDsReporteServices = "rsActuacionesUsuarioOficinaC";
            strRutaBase = Server.MapPath("~/Contabilidad/rsActuacionesUsuarioOficinaConsular.rdlc");
        }

        private void rptRegCivilActaNacimiento()
        {
            iNroDate = 0;
            parameters = new ReportParameter[7];
            parameters[0] = new ReportParameter("TituloReporte", "LISTADO DE REGISTRO CIVIL - ACTA DE NACIMIENTO - MAYORES DE EDAD");
            parameters[1] = new ReportParameter("SubTituloReporte", Constantes.CONST_REPORTE_SUB_TITULO);
            parameters[2] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
            parameters[3] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
            parameters[4] = new ReportParameter("FechaHaber", Session["FechaIntervalo"].ToString());
            parameters[5] = new ReportParameter("FechaActual", strFechaActualConsulado);
            parameters[6] = new ReportParameter("HoraActual", strHoraActualConsulado);


            sNombreDsReporteServices = "dtActaNacimiento";
            strRutaBase = Server.MapPath("~/Contabilidad/rsListadoRegistroCivilActaNacimiento.rdlc");
        }
        
        private void rptRemesa() {
            iNroDate = 0;
            parameters = new ReportParameter[7];
            parameters[0] = new ReportParameter("TituloReporte", "REPORTE DE REMESAS CONSULARES - LIMA");
            parameters[1] = new ReportParameter("SubTituloReporte", Constantes.CONST_REPORTE_SUB_TITULO);
            parameters[2] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
            parameters[3] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
            parameters[4] = new ReportParameter("FechaHaber", Session["FechaIntervalo"].ToString());
            parameters[5] = new ReportParameter("FechaActual", strFechaActualConsulado);
            parameters[6] = new ReportParameter("HoraActual", strHoraActualConsulado);
            sNombreDsReporteServices = "rsRemesaConsular";
            strRutaBase = Server.MapPath("~/Contabilidad/rsRemesaConsular.rdlc");
        }

        private void rptEstadoBancario() {
            iNroDate = 0;
            if (sNombreOficinaConsular == "SEDE CENTRAL")
            {
                string strConsulado = Convert.ToString(Request.QueryString["Cs"]);
                sNombreOficinaConsular = strConsulado;
                sNombreOficinaConsular = sNombreOficinaConsular.Split('-')[1].ToString().Trim();
            }

            DataTable dtOficinaConsular = new DataTable();

            dtOficinaConsular = Comun.ObtenerOficinaConsularPorId(Session);

            parameters = new ReportParameter[7];
            parameters[0] = new ReportParameter("TituloReporte", "AUXILIAR BANCOS");
            parameters[1] = new ReportParameter("SubTituloReporte", Constantes.CONST_REPORTE_SUB_TITULO);
            parameters[2] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
            parameters[3] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
            parameters[4] = new ReportParameter("NumeroCuenta", Session["Reporte_vCuenta"].ToString());
            parameters[5] = new ReportParameter("DireccionOficina", dtOficinaConsular.Rows[0]["ofco_vDireccion"].ToString());
            parameters[6] = new ReportParameter("TelefonoOficina", dtOficinaConsular.Rows[0]["ofco_vTelefono"].ToString());
            sNombreDsReporteServices = "dsEstadoBancario";
            strRutaBase = Server.MapPath("~/Contabilidad/rsEstadoBancario.rdlc");
        }

        private void rptRegistroUnico() {
            iNroDate = 0;
            parameters = new ReportParameter[5];
            parameters[0] = new ReportParameter("TituloReporte", "DOCUMENTO ÚNICO");
            parameters[1] = new ReportParameter("SubTituloReporte", Constantes.CONST_REPORTE_SUB_TITULO);
            parameters[2] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
            parameters[3] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
            parameters[4] = new ReportParameter("FechaHaber", Session["FechaIntervalo"].ToString());
            sNombreDsReporteServices = "rsDocumentoUnico";
            strRutaBase = Server.MapPath("~/Contabilidad/rsLibroDocumentoUnico.rdlc");
        }

        private void rptRegistroGeneralesEntrada(){
            iNroDate = 0;
            parameters = new ReportParameter[7];
            parameters[0] = new ReportParameter("TituloReporte", "REGISTRO GENERAL DE ENTRADAS");
            parameters[1] = new ReportParameter("SubTituloReporte", Constantes.CONST_REPORTE_SUB_TITULO);
            parameters[2] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
            parameters[3] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
            parameters[4] = new ReportParameter("FechaHaber", Session["FechaIntervalo"].ToString());
            parameters[5] = new ReportParameter("FechaActual", strFechaActualConsulado);
            parameters[6] = new ReportParameter("HoraActual", strHoraActualConsulado);
            sNombreDsReporteServices = "rsRegistroGenerales";
            if (Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Convert.ToInt16(Constantes.CONST_ID_CONSULADO_CARACAS))
            {
                strRutaBase = Server.MapPath("~/Contabilidad/rsRegistroGeneralCaracas.rdlc");
            }
            else { strRutaBase = Server.MapPath("~/Contabilidad/rsRegistroGeneral.rdlc"); }
            
        }
        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 05/09/2016
        // Objetivo: Parametros para el listado de actuaciones anuladas
        //------------------------------------------------------------------------
        private void rptActuacionesAnuladas()
        {
            iNroDate = 0;
            parameters = new ReportParameter[6];
            parameters[0] = new ReportParameter("TituloReporte", "REPORTE DE ACTUACIONES ANULADAS");
            parameters[1] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
            parameters[2] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
            parameters[3] = new ReportParameter("FechaHaber", Session["FechaIntervalo"].ToString());
            parameters[4] = new ReportParameter("FechaActual", strFechaActualConsulado);
            parameters[5] = new ReportParameter("HoraActual", strHoraActualConsulado);
            sNombreDsReporteServices = "rsRegistroGenerales";
            strRutaBase = Server.MapPath("~/Contabilidad/rsActuacionesAnuladas.rdlc");
        }
        //------------------------------------------------------------------------

        private void rptRendicionCuentasRENIEC(bool bNOHEAD=false)
        {
            iNroDate = 0;
            parameters = new ReportParameter[7];
            parameters[0] = new ReportParameter("TituloReporte", "RENDICIÓN DE CUENTAS POR TARIFAS RENIEC");
            parameters[1] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
            parameters[2] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
            parameters[3] = new ReportParameter("FechaHaber", Session["FechaIntervalo"].ToString());
            parameters[4] = new ReportParameter("FechaActual", strFechaActualConsulado);
            parameters[5] = new ReportParameter("HoraActual", strHoraActualConsulado);
            parameters[6] = new ReportParameter("Pais", sNombrePaisConsular);
            sNombreDsReporteServices = "rsFichaRegistral";
            if (bNOHEAD == false)
            {
                strRutaBase = Server.MapPath("~/Contabilidad/rsRendicionCuentas.rdlc");
            }
            else
            {
                //------------------------------------------------------
                //Fecha: 12/05/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Exportar e Imprimir el tipo de reporte:
                //        Rendición de Cuentas por Tarifas RENIEC.
                //Documento: OBSERVACIONES_SGAC_12052021.doc
                //------------------------------------------------------
                strRutaBase = Server.MapPath("~/Contabilidad/rsRendicionCuentasNOHEAD.rdlc");            
            }

        }


        private void rptTramitesIncompletos()
        {
            iNroDate = 0;
            parameters = new ReportParameter[7];
            parameters[0] = new ReportParameter("TituloReporte", "LISTADO DE TRAMITES INCOMPLETOS - RENIEC");
            parameters[1] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
            parameters[2] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
            parameters[3] = new ReportParameter("FechaHaber", Session["FechaIntervalo"].ToString());
            parameters[4] = new ReportParameter("FechaActual", strFechaActualConsulado);
            parameters[5] = new ReportParameter("HoraActual", strHoraActualConsulado);
            parameters[6] = new ReportParameter("Pais", sNombrePaisConsular);
            sNombreDsReporteServices = "dtTramitesIncompletos";
            strRutaBase = Server.MapPath("~/Reportes/Reniec/rsTramitesIncompletos.rdlc");
        }

        private void rptGuiaDespachoRENIEC()
        {
            iNroDate = 0;
            parameters = new ReportParameter[6];
            parameters[0] = new ReportParameter("TituloReporte", "GUÍA DE DESPACHO DNIs N°" + Session["NumeroGuia"].ToString().Trim());
            parameters[1] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
            parameters[2] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
            parameters[3] = new ReportParameter("FechaHaber", Session["FechaIntervalo"].ToString());
            parameters[4] = new ReportParameter("FechaActual", strFechaActualConsulado);
            parameters[5] = new ReportParameter("HoraActual", strHoraActualConsulado);
            sNombreDsReporteServices = "rsGuiaDespachoRENIEC";
            strRutaBase = Server.MapPath("~/Contabilidad/rsGuiaDespachoRENIEC.rdlc");
        }

        private void rptFormatoEnvioRENIEC()
        {
            iNroDate = 0;
            parameters = new ReportParameter[6];
            parameters[0] = new ReportParameter("TituloReporte", "FORMATO DE ENVIO - GUÍA DE DESPACHO N°" + Session["NumeroGuia"].ToString().Trim());
            parameters[1] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
            parameters[2] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
            parameters[3] = new ReportParameter("FechaHaber", Session["FechaIntervalo"].ToString());
            parameters[4] = new ReportParameter("FechaActual", strFechaActualConsulado);
            parameters[5] = new ReportParameter("HoraActual", strHoraActualConsulado);

            sNombreDsReporteServices = "rsFormatoEnvioRENIEC";
            strRutaBase = Server.MapPath("~/Contabilidad/rsFormatoEnvioRENIEC.rdlc");
        }
        private void rptFormatoEnvioRecuperadosRENIEC()
        {
            iNroDate = 0;
            parameters = new ReportParameter[6];
            parameters[0] = new ReportParameter("TituloReporte", "FORMATO DE ENVIO - GUÍA DE DESPACHO N°" + Session["NumeroGuia"].ToString().Trim());
            parameters[1] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
            parameters[2] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
            parameters[3] = new ReportParameter("FechaHaber", Session["FechaIntervalo"].ToString());
            parameters[4] = new ReportParameter("FechaActual", strFechaActualConsulado);
            parameters[5] = new ReportParameter("HoraActual", strHoraActualConsulado);

            sNombreDsReporteServices = "dsReporteReniec";
            strRutaBase = Server.MapPath("~/Contabilidad/rsFormatoEnvioRecuperadosRENIEC.rdlc");
        }
        private void rptLibroCaja() {
            iNroDate =1;
           
            parameters = new ReportParameter[7];
            parameters[0] = new ReportParameter("TituloReporte", "LIBRO CAJA");
            parameters[1] = new ReportParameter("SubTituloReporte", Constantes.CONST_REPORTE_SUB_TITULO);
            parameters[2] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
            parameters[3] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
            parameters[4] = new ReportParameter("FechaHaber", Session["FechaIntervalo"].ToString());
            parameters[5] = new ReportParameter("FechaActual", strFechaActualConsulado);
            parameters[6] = new ReportParameter("HoraActual", strHoraActualConsulado);
            sNombreDsReporteServices = "rsLibroCajaDebe";
            sNombreDsReporteServices1 = "rsLibroCajaHaber";
          
            strRutaBase = Server.MapPath("~/Contabilidad/rsLibroCaja.rdlc");

        }

        private void rptSaldoConsular()
        {
            string abc = strFechaActualConsulado;
            iNroDate = 1;
            iNroDate1 = 2;
            iNroDate2 = 3;
            parameters = new ReportParameter[7];
            parameters[0] = new ReportParameter("TituloReporte", "LIBRO DE SALDOS CONSULARES");
            parameters[1] = new ReportParameter("SubTituloReporte", Constantes.CONST_REPORTE_SUB_TITULO);
            parameters[2] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
            parameters[3] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
            parameters[4] = new ReportParameter("FechaHaber", Session["FechaIntervalo"].ToString());
            parameters[5] = new ReportParameter("FechaActual", strFechaActualConsulado);
            parameters[6] = new ReportParameter("HoraActual", strHoraActualConsulado);
            sNombreDsReporteServices = "rsSaldoConsularDebe";
            sNombreDsReporteServices1 = "rsSaldoConsularHaber";
            sNombreDsReporteServices2 = "dtTablet";
            sNombreDsReporteServices3 = "dtTotal";
            strRutaBase = Server.MapPath("~/Contabilidad/rsSaldosConsulares.rdlc");
        }

        private string ObtenerTotal()
        {
            double total = 0;
            DataTable dt = (DataTable)Session["dtDatos3"];
            if (dt.Rows.Count > 0)
                total += Convert.ToDouble(dt.Rows[0]["totalMonedaExtranjera"]);
            return total.ToString();
        }

        private void rptAutodhesivoConsulares()
        {
            iNroDate = 0;
            parameters = new ReportParameter[7];
            parameters[0] = new ReportParameter("TituloReporte", "LIBRO DE AUTOADHESIVOS CONSULARES");
            parameters[1] = new ReportParameter("SubTituloReporte", Constantes.CONST_REPORTE_SUB_TITULO);
            parameters[2] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
            parameters[3] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
            parameters[4] = new ReportParameter("FechaHaber", Session["FechaIntervalo"].ToString());
            parameters[5] = new ReportParameter("FechaActual", strFechaActualConsulado);
            parameters[6] = new ReportParameter("HoraActual", strHoraActualConsulado);
            sNombreDsReporteServices = "dsLibroAdhesivos";
            strRutaBase = Server.MapPath("~/Contabilidad/rsLibroAutoadhesivos.rdlc");
        }

        private void rptConciliacionBancaria()
        {
            iNroDate = 1;
            iNroDate1 = 2;

            parameters = new ReportParameter[9];
            parameters[0] = new ReportParameter("TituloReporte", "CONCILIACIÓN BANCARIA");
            parameters[1] = new ReportParameter("SubTituloReporte", Constantes.CONST_REPORTE_SUB_TITULO);
            parameters[2] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
            parameters[3] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
            
            string strCuenta = "";
            string strFechaInicio = "";
            string strFechaFin = "";
            string strSaldo = "0.00";
            if (Session["conciliacion_ini"] != null)
                strFechaInicio = Convert.ToString(Session["conciliacion_ini"]);
            if (Session["conciliacion_fin"] != null)
                strFechaFin = Convert.ToString(Session["conciliacion_fin"]);
            if (Session["nro_cuenta"] != null)
                strCuenta = Session["nro_cuenta"].ToString();
            if (Session["saldo_contabilidad"] != null)
                strSaldo = Session["saldo_contabilidad"].ToString();

            parameters[4] = new ReportParameter("SaldoInicial", "Saldo Inicial " + strSaldo + " al " + strFechaInicio);
            parameters[5] = new ReportParameter("FechaDetalle", strFechaInicio + " al " + strFechaFin);
            parameters[6] = new ReportParameter("CuentaBancaria", "Cuenta N° " + strCuenta);
            parameters[7] = new ReportParameter("FechaActual", strFechaActualConsulado);
            parameters[8] = new ReportParameter("HoraActual", strHoraActualConsulado);

            sNombreDsReporteServices = "DsConciliacionBancaria";
            sNombreDsReporteServices1 = "dtRegistroGeneral";
            sNombreDsReporteServices2 = "dtCajaChica";
            strRutaBase = Server.MapPath("~/Contabilidad/rsConciliacionBancaria.rdlc");
        }

        private void rptAntecedentesPenales(bool usuario = false)
        {
            iNroDate = 0;
            parameters = new ReportParameter[6];
            parameters[0] = new ReportParameter("TituloReporte", "LISTADO MENSUAL DE CERTIFICADOS CONSULARES DE ANTECEDENTES PENALES SOLICITADOS A TRAVÉS DEL MODULO MSIAP");
            parameters[1] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
            parameters[2] = new ReportParameter("FechaIntervalo", Session["FechaIntervalo"].ToString());
            parameters[3] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
            parameters[4] = new ReportParameter("FechaActual", strFechaActualConsulado);
            parameters[5] = new ReportParameter("HoraActual", strHoraActualConsulado);
            sNombreDsReporteServices = "DSAntecedentePenal";
            if (usuario)
            {
                strRutaBase = Server.MapPath("~/Reportes/RSAntecedentes/rsAntecedentePenal_Usuario.rdlc");
            }
            else {
                strRutaBase = Server.MapPath("~/Reportes/RSAntecedentes/rsAntecedentePenal.rdlc");
            }
            
     
            
        }
	private void rptConciliacionRENIEC()
        {
            iNroDate = 0;
            parameters = new ReportParameter[5];
            parameters[0] = new ReportParameter("TituloReporte", Constantes.CONST_RENIEC_CONCILIACION);
            parameters[1] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
            parameters[2] = new ReportParameter("FechaHaber", Session["FechaIntervalo"].ToString());
            parameters[3] = new ReportParameter("FechaActual", strFechaActualConsulado);
            parameters[4] = new ReportParameter("HoraActual", strHoraActualConsulado);
            sNombreDsReporteServices = "dsConciliacionRENIEC";
            strRutaBase = Server.MapPath("~/Reportes/Reniec/rsConciliacionRENIEC.rdlc");
        }

	private void rptCertificadoAntecedentePenal(string reporteLista)
        {
            iNroDate = 0;
            
            sNombreDsReporteServices = "dsCertificadoConsularAntecedentesPenales";
            strRutaBase = Server.MapPath("~/Reportes/rsCertificadoConsularAntecedentesPenales_" + reporteLista + ".rdlc");
            string strIdioma = Session[Constantes.CONST_SESION_IDIOMA_TEXTO].ToString();
            string strRutaImagen = Session["RutaImagenCertificadoAntecedentePenal"].ToString();
            Session["RutaImagenCertificadoAntecedentePenal"] = null;
            dsReport.LocalReport.EnableExternalImages = true;
            if (strIdioma == "CASTELLANO")
            {                
                parameters = new ReportParameter[2];
                parameters[0] = new ReportParameter("NombreOficina", sNombreOficinaConsular);

                if (strRutaImagen.Length > 0)
                {
                    string imagePath = new Uri(strRutaImagen).AbsoluteUri;
                    parameters[1] = new ReportParameter("rutaImagen", imagePath);
                }
                else
                {
                    string imagePath = "";
                    //imagePath = new Uri(Server.MapPath("~/Images/imagen-no-disponible.jpg")).AbsoluteUri;

                    parameters[1] = new ReportParameter("rutaImagen", imagePath);
                }
            }
            else
            {
                #region Si_el_Idioma_es_diferente_al_Castellano
                #region LeerTipoPlantilla
                DataTable dtPlantilla = new DataTable();
                
                dtPlantilla = comun_Part1.ObtenerParametrosPorGrupo(Session, "ACTUACIÓN-TIPO PLANTILLA");
                Int16 intTipoPlantillaAntecedentesPenales = 0;

                for (int i = 0; i < dtPlantilla.Rows.Count; i++)
                {
                    if (dtPlantilla.Rows[i]["descripcion"].ToString().Trim().ToUpper().Equals("CERTIFICADO DE ANTECEDENTES PENALES"))
                    {
                        intTipoPlantillaAntecedentesPenales = Convert.ToInt16(dtPlantilla.Rows[i]["id"].ToString());
                        break;
                    }
                }
                #endregion

                #region LeerTraduccion
                if (intTipoPlantillaAntecedentesPenales > 0)
                {
                    DataTable dtTraduccion = new DataTable();
                    SGAC.Configuracion.Sistema.BL.PlantillaTraduccionConsultasBL BL = new SGAC.Configuracion.Sistema.BL.PlantillaTraduccionConsultasBL();
                    int IntTotalCount = 0;
                    int IntTotalPages = 0;
                    
                    Int16 intIdiomaId = Convert.ToInt16(Session[Constantes.CONST_SESION_IDIOMA_ID].ToString());
                    string strEtiqueta = "";
                    string strTraduccion = "";

                    if (intIdiomaId > 0)
                    {
                        dtTraduccion = BL.Consultar(0, intTipoPlantillaAntecedentesPenales, intIdiomaId, 0, "A", "1", 1000, "N", ref IntTotalCount, ref IntTotalPages);
                        if (dtTraduccion.Rows.Count > 0)
                        {
                            int intTotalIndices = 16;
                            parameters = new ReportParameter[intTotalIndices];
                            parameters[0] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
                            if (strRutaImagen.Length > 0)
                            {
                                string imagePath = new Uri(strRutaImagen).AbsoluteUri;
                                parameters[1] = new ReportParameter("rutaImagen", imagePath);
                            }
                            else
                            {
                                string imagePath = "";
                                //imagePath = new Uri(Server.MapPath("~/Images/imagen-no-disponible.jpg")).AbsoluteUri;
                                parameters[1] = new ReportParameter("rutaImagen", imagePath);
                            }

                            for (int i = 2; i < intTotalIndices; i++)
                            {
                                strEtiqueta = dtTraduccion.Rows[i - 2]["etiq_vEtiqueta"].ToString().Trim().ToUpper();
                                strTraduccion = dtTraduccion.Rows[i - 2]["pltr_vTraduccion"].ToString().Trim();
                                parameters[i] = new ReportParameter(strEtiqueta, strTraduccion);
                            }
                        }
                    }
                    //---------------------------
                }
                #endregion
                #endregion
            }
          
        }
        //FINAL-----------

    }
}
