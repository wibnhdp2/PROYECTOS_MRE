using System;
using System.Data;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.Controlador;
using SGAC.WebApp.Accesorios.SharedControls;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Drawing;

namespace SGAC.WebApp.Accesorios
{

    public class Comun
    {

        #region Vista Previa
        public static void VerVistaPreviaCertificadoAntecedentePenal(HttpSessionState Session, Page Page, string strRPT,Enumerador.enmReporteContable enmReporte, string strRutaFoto)
        {
            Session[Constantes.CONST_SESION_REPORTE_TIPO] = enmReporte;
            Session["RutaImagenCertificadoAntecedentePenal"] = strRutaFoto;
            string strUrl = "../Contabilidad/FrmReporteContables.aspx?lst=" + strRPT;
            string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";
            EjecutarScript(Page, strScript);
        }

        public static void VerVistaPrevia(HttpSessionState Session, Page Page, DataSet ds, Enumerador.enmReporteContable enmReporte)
        {
            Session[Constantes.CONST_SESION_REPORTE_TIPO] = enmReporte;
            Session[Constantes.CONST_SESION_REPORTE_DT] = ds;

            string strUrl = "../Contabilidad/FrmReporteContables.aspx" ;
            string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";
            EjecutarScript(Page, strScript);
        }
        public static void VerVistaPreviaRGE(HttpSessionState Session, Page Page, List<csReporteRGE> lst, Enumerador.enmReporteContable enmReporte)
        {
            Session[Constantes.CONST_SESION_REPORTE_TIPO] = enmReporte;
            Session[Constantes.CONST_SESION_REPORTE_DT] = lst;

            string strUrl = "../Contabilidad/FrmReporteContables.aspx?lst=1";
            string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";
            EjecutarScript(Page, strScript);
        }
        public static void VerVistaPrevia(HttpSessionState Session, Page Page, DataSet ds, Enumerador.enmReporteContable enmReporte,string strConsulado)
        {
            Session[Constantes.CONST_SESION_REPORTE_TIPO] = enmReporte;
            Session[Constantes.CONST_SESION_REPORTE_DT] = ds;

            string strUrl = "../Contabilidad/FrmReporteContables.aspx?Cs=" + strConsulado;
            string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";
            EjecutarScript(Page, strScript);
        }
        #endregion

        #region Script
        public static void EjecutarScript(Page Page, string strScript)
        {
            strScript = string.Format(strScript);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "OpenPopup" + DateTime.Now.Ticks.ToString(), strScript, true);
        }

        public static void EjecutarScriptUpdatePanel(UpdatePanel udp, string strScript)
        {
            strScript = string.Format(strScript);
            ScriptManager.RegisterStartupScript(udp, udp.GetType(), "OpenPopup" + DateTime.Now.Ticks.ToString(), strScript, true);
        }

        public static void EjecutarScriptUniqueIdDinamico(Page Page, string strScript, string uniqueId)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "OpenPopup" + uniqueId, strScript, true);
        }

        #endregion

        #region Otros
        private static string ObtenerSeparacion(string strNumeroLetra)
        {
            char[] arrNumeros = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            int intContador = 0;
            bool esNumero = false;
            foreach (char caracter in strNumeroLetra.ToCharArray())
            {
                esNumero = false;
                foreach (char numero in arrNumeros)
                {
                    if (caracter == numero)
                    {
                        esNumero = true;
                        intContador++;
                        break;
                    }
                }
                if (!esNumero)
                    break;
            }
            if (intContador == 0)
            {
                strNumeroLetra = 0 + "|" +
                                strNumeroLetra.Substring(intContador, strNumeroLetra.Length - intContador);
            }
            else
            {
                strNumeroLetra = strNumeroLetra.Substring(0, intContador) + "|" +
                                 strNumeroLetra.Substring(intContador, strNumeroLetra.Length - intContador);
            }
            return strNumeroLetra;
        }
        #endregion

        #region Validaciones
        public static bool fechaValida(Object obj)
        {
            string strDate = obj.ToString();

            if ((strDate.Length == 0) || (strDate == "__/__/____"))
            {
                return true;
            }

            int curr_year = DateTime.Today.Year;
            int anio, dia, mes;

            string fecha = (Convert.ToDateTime(strDate)).ToString("dd/MM/yyyy");

            anio = int.Parse(fecha.Substring(6, 4));
            mes = int.Parse(fecha.Substring(3, 2));
            dia = int.Parse(fecha.Substring(0, 2));

            if (anio <= 1900 || anio > curr_year)
            {
                return false;
            }

            if (mes < 1 || mes > 12)
            {
                return false;
            }

            if (mes == 1 || mes == 3 || mes == 5 || mes == 7 || mes == 8 || mes == 10 || mes == 12)
            {
                if (dia < 1 || dia > 31)
                {
                    return false;
                }
            }

            if (mes == 4 || mes == 6 || mes == 9 || mes == 11)
            {
                if (dia < 1 || dia > 30)
                {
                    return false;
                }
            }

            if (mes == 2)
            {
                if (anio % 4 == 0 && (anio % 100 != 0 || anio % 400 == 0) == false)
                {
                    if (dia < 1 || dia > 28)
                    {
                        return false;
                    }
                }
                else
                {
                    if (dia < 1 || dia > 29)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        public static bool IsNumeric(object Expression)
        {
            bool isNum;
            double retNum;

            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }
        #endregion

        #region Permisos
        public static void CargarPermisos(HttpSessionState Session,
                                          ctrlToolBarConfirm toolBarConsulta,
                                          ctrlToolBarConfirm toolBarMantenimiento,
                                          GridView gdvGrillaConsulta,
                                          string strRutaFormulario)
        {
            DataTable dtConfiguracion;
            int intFilaIndice = 0;
            bool bolEncontro = false;
            string strAcciones = string.Empty;
            string[] arrAcciones;

            // 1. Buscar ID formulario
            #region Formulario-Acciones
            dtConfiguracion = (DataTable)Session[Constantes.CONST_SESION_DT_CONFIGURACION];
            if (dtConfiguracion != null)
            {
                intFilaIndice = 0;
                bolEncontro = false;
                foreach (DataRow dr in dtConfiguracion.Rows)
                {
                    if (dr["form_vRuta"].ToString().Contains(strRutaFormulario))
                    {
                        bolEncontro = true;
                        break;
                    }
                    intFilaIndice++;
                }
            }
            #endregion

            // 2. Configuración de Permisos (Visible)
            #region Visibilidad Acciones
            if (toolBarConsulta != null)
            {
                toolBarConsulta.VisibleIButtonBuscar = false;
                toolBarConsulta.VisibleIButtonCancelar = false;
                toolBarConsulta.VisibleIButtonConfiguration = false;
                toolBarConsulta.VisibleIButtonEditar = false;
                toolBarConsulta.VisibleIButtonEliminar = false;
                toolBarConsulta.VisibleIButtonGrabar = false;
                toolBarConsulta.VisibleIButtonNuevo = false;
                toolBarConsulta.VisibleIButtonPrint = false;
                toolBarConsulta.VisibleIButtonSalir = false;
            }
            if (toolBarMantenimiento != null)
            {
                toolBarMantenimiento.VisibleIButtonBuscar = false;
                toolBarMantenimiento.VisibleIButtonCancelar = false;
                toolBarMantenimiento.VisibleIButtonConfiguration = false;
                toolBarMantenimiento.VisibleIButtonEditar = false;
                toolBarMantenimiento.VisibleIButtonEliminar = false;
                toolBarMantenimiento.VisibleIButtonGrabar = false;
                toolBarMantenimiento.VisibleIButtonNuevo = false;
                toolBarMantenimiento.VisibleIButtonPrint = false;
                toolBarMantenimiento.VisibleIButtonSalir = false;
            }

            if (bolEncontro)
            {
                strAcciones = dtConfiguracion.Rows[intFilaIndice]["form_vAcciones"].ToString();
                arrAcciones = strAcciones.Split('|');

                if (strAcciones.Length == 0)
                    return;

                if (toolBarConsulta != null && toolBarMantenimiento == null)
                {
                    #region Solo ToolBarConsulta
                    for (int i = 0; i < arrAcciones.Length; i++)
                    {
                        switch (Convert.ToChar(arrAcciones[i]))
                        {
                            case (char)Enumerador.enmPermisoAccion.BUSCAR:
                                toolBarConsulta.VisibleIButtonBuscar = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.IMPRIMIR:
                                toolBarConsulta.VisibleIButtonPrint = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.CANCELAR_C:
                                toolBarConsulta.VisibleIButtonCancelar = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.CONFIGURAR:
                                toolBarConsulta.VisibleIButtonConfiguration = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.CERRAR:
                                toolBarConsulta.VisibleIButtonSalir = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.NUEVO:
                                if (toolBarMantenimiento == null)
                                    toolBarConsulta.VisibleIButtonNuevo = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.MODIFICAR:
                                if (toolBarMantenimiento == null)
                                    toolBarConsulta.VisibleIButtonEditar = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.ELIMINAR:
                                if (toolBarMantenimiento == null)
                                    toolBarConsulta.VisibleIButtonEliminar = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.GRABAR:
                                if (toolBarMantenimiento == null)
                                    toolBarConsulta.VisibleIButtonGrabar = true;
                                break;
                        }
                    }
                    #endregion
                }
                else if (toolBarMantenimiento != null && toolBarConsulta == null)
                {
                    #region Solo ToolBarMantenimiento
                    for (int i = 0; i < arrAcciones.Length; i++)
                    {
                        switch (Convert.ToChar(arrAcciones[i]))
                        {
                            case (char)Enumerador.enmPermisoAccion.BUSCAR:
                                toolBarMantenimiento.VisibleIButtonBuscar = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.IMPRIMIR:
                                toolBarMantenimiento.VisibleIButtonPrint = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.CANCELAR_C:
                                toolBarMantenimiento.VisibleIButtonCancelar = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.CONFIGURAR:
                                toolBarMantenimiento.VisibleIButtonConfiguration = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.CERRAR:
                                toolBarMantenimiento.VisibleIButtonSalir = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.NUEVO:
                                toolBarMantenimiento.VisibleIButtonNuevo = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.MODIFICAR:
                                toolBarMantenimiento.VisibleIButtonEditar = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.ELIMINAR:
                                toolBarMantenimiento.VisibleIButtonEliminar = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.GRABAR:
                                toolBarMantenimiento.VisibleIButtonGrabar = true;
                                break;
                        }
                    }
                    #endregion
                }
                else
                {
                    #region ToolBar: Consulta y Mantenimiento
                    // Consulta
                    for (int i = 0; i < arrAcciones.Length; i++)
                    {
                        switch (Convert.ToChar(arrAcciones[i]))
                        {
                            case (char)Enumerador.enmPermisoAccion.BUSCAR:
                                toolBarConsulta.VisibleIButtonBuscar = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.IMPRIMIR:
                                toolBarConsulta.VisibleIButtonPrint = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.CANCELAR_C:
                                // Pintar Botón
                                toolBarConsulta.btnCancelar.CssClass = "btnLimpiar";
                                toolBarConsulta.btnCancelar.Text = "    Limpiar";
                                toolBarConsulta.VisibleIButtonCancelar = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.CERRAR:
                                toolBarConsulta.VisibleIButtonSalir = true;
                                break;
                        }
                    }
                    // Mantenimiento
                    for (int i = 0; i < arrAcciones.Length; i++)
                    {
                        switch (Convert.ToChar(arrAcciones[i]))
                        {
                            case (char)Enumerador.enmPermisoAccion.NUEVO:
                                toolBarMantenimiento.VisibleIButtonNuevo = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.CONFIGURAR:
                                toolBarMantenimiento.VisibleIButtonConfiguration = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.CERRAR:
                                toolBarMantenimiento.VisibleIButtonSalir = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.MODIFICAR:
                                toolBarMantenimiento.VisibleIButtonEditar = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.ELIMINAR:
                                toolBarMantenimiento.VisibleIButtonEliminar = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.GRABAR:
                                toolBarMantenimiento.VisibleIButtonGrabar = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.CANCELAR_M:
                                toolBarMantenimiento.VisibleIButtonCancelar = true;
                                break;
                        }
                    }
                    #endregion
                }
            }
            #endregion
        }

        // Implementación para RUNE - 
        public static void CargarPermisos(HttpSessionState Session,
                                         ctrlToolBarButton toolBarConsulta,
                                         string strRutaFormulario)
        {
            DataTable dtConfiguracion;
            int intFilaIndice = 0;
            bool bolEncontro = false;
            string strAcciones = string.Empty;
            string[] arrAcciones;

            // 1. Buscar ID formulario
            #region Formulario-Acciones
            dtConfiguracion = (DataTable)Session[Constantes.CONST_SESION_DT_CONFIGURACION];
            if (dtConfiguracion != null)
            {
                intFilaIndice = 0;
                bolEncontro = false;
                foreach (DataRow dr in dtConfiguracion.Rows)
                {
                    if (dr["form_vRuta"].ToString().ToUpper().Contains(strRutaFormulario.ToUpper()))
                    {
                        bolEncontro = true;
                        break;
                    }
                    intFilaIndice++;
                }
            }
            #endregion

            // 2. Configuración de Permisos (Visible)
            #region Visibilidad Acciones
            if (toolBarConsulta != null)
            {
                toolBarConsulta.VisibleIButtonNuevo = false;
                toolBarConsulta.VisibleIButtonEditar = false;
                toolBarConsulta.VisibleIButtonGrabar = false;
                toolBarConsulta.VisibleIButtonBuscar = false;
                toolBarConsulta.VisibleIButtonCancelar = false;
                toolBarConsulta.VisibleIButtonPrint = false;
                toolBarConsulta.VisibleIButtonEliminar = false;
                toolBarConsulta.VisibleIButtonConfiguration = false;
                toolBarConsulta.VisibleIButtonSalir = false;
            }

            if (bolEncontro)
            {
                strAcciones = dtConfiguracion.Rows[intFilaIndice]["form_vAcciones"].ToString();
                arrAcciones = strAcciones.Split('|');

                if (toolBarConsulta != null)
                {
                    #region Solo ToolBarConsulta
                    for (int i = 0; i < arrAcciones.Length; i++)
                    {                        
                        switch (Convert.ToChar(arrAcciones[i]))
                        {
                            case (char)Enumerador.enmPermisoAccion.NUEVO:
                                toolBarConsulta.VisibleIButtonNuevo = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.MODIFICAR:
                                toolBarConsulta.VisibleIButtonEditar = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.GRABAR:
                                toolBarConsulta.VisibleIButtonGrabar = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.BUSCAR:
                                toolBarConsulta.VisibleIButtonBuscar = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.CANCELAR_C:
                                toolBarConsulta.VisibleIButtonCancelar = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.IMPRIMIR:
                                toolBarConsulta.VisibleIButtonPrint = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.ELIMINAR:
                                toolBarConsulta.VisibleIButtonEliminar = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.CONFIGURAR:
                                toolBarConsulta.VisibleIButtonConfiguration = true;
                                break;
                            case (char)Enumerador.enmPermisoAccion.CERRAR:
                                toolBarConsulta.VisibleIButtonSalir = true;
                                break;
                        }
                    }
                    #endregion
                }
            }
            #endregion
        }
        #endregion

        #region Alerta (Envío remesa)
        public static void EnviarAlertaContable(HttpSessionState Session, Page Page)
        {
            string strScript = string.Empty;
            if (Session["MostrarAlertaPendiente"] != null)
            {
                if (Convert.ToInt32(Session["MostrarAlertaPendiente"]) == 1)
                {
                    Session["MostrarAlertaPendiente"] = 2;
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING,
                        "AVISO REMESA CONSULAR", "Tiene remesas pendiente de envío (Plazo finalizado).");
                    EjecutarScript(Page, strScript);
                }
            }
        }
        #endregion

        #region Accesorios
        public static bool EsNumero(char cLetra)
        {
            char[] arrNumeros = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            foreach (char cNumero in arrNumeros)
            {
                if (cNumero == cLetra)
                    return true;
            }
            return false;
        }

        public static List<object> ObtenerLista(int cantidad)
        {
            List<object> lstVarios = new List<object>();
            for (int i = 0; i < cantidad; i++)
            {
                lstVarios.Add(new object());
            }
            return lstVarios;
        }

        public static DateTime FormatearFecha(string strFecha)
        {
            DateTime datFecha = new DateTime();

            if (strFecha != null)
            {
                strFecha = strFecha.Replace(".", "");
                strFecha = strFecha.Replace("Ene", "Jan");
                strFecha = strFecha.Replace("Abr", "Apr");
                strFecha = strFecha.Replace("Ago", "Aug");
                strFecha = strFecha.Replace("Set", "Sep");
                strFecha = strFecha.Replace("Dic", "Dec");

                strFecha = strFecha.Replace("ene", "Jan");
                strFecha = strFecha.Replace("abr", "Apr");
                strFecha = strFecha.Replace("ago", "Aug");
                strFecha = strFecha.Replace("set", "Sep");
                strFecha = strFecha.Replace("dic", "Dec");

                strFecha = strFecha.Replace("ENE", "Jan");
                strFecha = strFecha.Replace("ABR", "Apr");
                strFecha = strFecha.Replace("AGO", "Aug");
                strFecha = strFecha.Replace("SET", "Sep");
                strFecha = strFecha.Replace("DIC", "Dec");
            }
            
            if (!DateTime.TryParse(strFecha, out datFecha))
            {
                datFecha = Convert.ToDateTime(strFecha, System.Globalization.CultureInfo.GetCultureInfo("en-Us").DateTimeFormat);
            }
            return datFecha;
        }

        public static DateTime FormatearFechaCompleta(string strFecha)
        {
            DateTime datFecha = new DateTime();
            strFecha = strFecha.Replace(".", "");
            strFecha = strFecha.Replace("Ene", "Jan");
            strFecha = strFecha.Replace("Abr", "Apr");
            strFecha = strFecha.Replace("Ago", "Aug");
            strFecha = strFecha.Replace("Set", "Sep");
            strFecha = strFecha.Replace("Dic", "Dec");
            if (!DateTime.TryParse(strFecha + " " + DateTime.Now.ToString("HH:mm"), out datFecha))
            {
                datFecha = Convert.ToDateTime(strFecha + " " + DateTime.Now.ToString("HH:mm"), System.Globalization.CultureInfo.GetCultureInfo("en-Us").DateTimeFormat);
            }
            return datFecha;

        }

        public static string ObtenerFechaActualTexto(HttpSessionState Session)
        {
            DateTime datFecha = DateTime.UtcNow;

            //DataTable dtOficinaConsularPorId = new DataTable();

            //dtOficinaConsularPorId = Comun.ObtenerOficinaConsularPorId(Session);
            double dblDiferenciaHoraria = 0;
            double dblHorarioVerano = 0;
            double dblHorasConsiderar = 0;

            //*----------------------------------*
            //*Fecha: 03/12/2019
            //*Autor: Miguel Márquez Beltrán
            //*Motivo: Usar la sesión creada.
            //*----------------------------------*

            //if (dtOficinaConsularPorId != null)
            //{
            //    if (dtOficinaConsularPorId.Rows.Count > 0)
            //    {
            //        dblDiferenciaHoraria = Convert.ToDouble(dtOficinaConsularPorId.Rows[0]["ofco_sDiferenciaHoraria"].ToString());
            //        dblHorarioVerano = Convert.ToDouble(dtOficinaConsularPorId.Rows[0]["ofco_sHorarioVerano"].ToString());
            //    }
            //}

            //--------------------------------------------------------
            //Fecha: 09/12/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Validar la sesion cuando es nula.
            //--------------------------------------------------------
            if (Session[Constantes.CONST_SESION_DIFERENCIA_HORARIA] != null)
            {
                dblDiferenciaHoraria = Convert.ToDouble(Session[Constantes.CONST_SESION_DIFERENCIA_HORARIA].ToString());
            }
            if (Session[Constantes.CONST_SESION_HORARIO_VERANO] != null)
            {
                dblHorarioVerano = Convert.ToDouble(Session[Constantes.CONST_SESION_HORARIO_VERANO].ToString().ToString());
            }

            dblHorasConsiderar = Convert.ToDouble(dblDiferenciaHoraria + dblHorarioVerano);

            datFecha = datFecha.AddHours(dblHorasConsiderar);
            return datFecha.ToString(ConfigurationManager.AppSettings["FormatoFechas"]).Replace(".","");
        }

        public static string ObtenerHoraActualTexto(HttpSessionState Session)
        {
            DateTime datFecha = DateTime.UtcNow;

            //DataTable dtOficinaConsularPorId = new DataTable();

            //dtOficinaConsularPorId = Comun.ObtenerOficinaConsularPorId(Session);
            double dblDiferenciaHoraria = 0;
            double dblHorarioVerano = 0;
            double dblHorasConsiderar = 0;

            //*----------------------------------*
            //*Fecha: 03/12/2019
            //*Autor: Miguel Márquez Beltrán
            //*Motivo: Usar la sesión creada.
            //*----------------------------------*

            //if (dtOficinaConsularPorId != null)
            //{
            //    if (dtOficinaConsularPorId.Rows.Count > 0)
            //    {

            //        dblDiferenciaHoraria = Convert.ToDouble(dtOficinaConsularPorId.Rows[0]["ofco_sDiferenciaHoraria"].ToString());
            //        dblHorarioVerano = Convert.ToDouble(dtOficinaConsularPorId.Rows[0]["ofco_sHorarioVerano"].ToString());
            //    }
            //}
            dblDiferenciaHoraria = Convert.ToDouble(Session[Constantes.CONST_SESION_DIFERENCIA_HORARIA].ToString());
            dblHorarioVerano = Convert.ToDouble(Session[Constantes.CONST_SESION_HORARIO_VERANO].ToString().ToString());

            
            dblHorasConsiderar = Convert.ToDouble(dblDiferenciaHoraria + dblHorarioVerano);

            datFecha = datFecha.AddHours(dblHorasConsiderar);
            return datFecha.ToString(ConfigurationManager.AppSettings["FormatoHora"]);
        }

        //-----------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 19/11/2019
        // Objetivo: Unificar la consulta de fecha y hora
        //-----------------------------------------------------
        public static void ObtenerFechaHoraActualTexto(HttpSessionState Session, ref string strFecha, ref string strHora)
        {
            DateTime datFecha = DateTime.UtcNow;

           // DataTable dtOficinaConsularPorId = new DataTable();

           // dtOficinaConsularPorId = Comun.ObtenerOficinaConsularPorId(Session);
            double dblDiferenciaHoraria = 0;
            double dblHorarioVerano = 0;
            double dblHorasConsiderar = 0;

            //*----------------------------------*
            //*Fecha: 03/12/2019
            //*Autor: Miguel Márquez Beltrán
            //*Motivo: Usar la sesión creada.
            //*----------------------------------*
            //if (dtOficinaConsularPorId != null)
            //{
            //    if (dtOficinaConsularPorId.Rows.Count > 0)
            //    {
            //        dblDiferenciaHoraria = Convert.ToDouble(dtOficinaConsularPorId.Rows[0]["ofco_sDiferenciaHoraria"].ToString());
            //        dblHorarioVerano = Convert.ToDouble(dtOficinaConsularPorId.Rows[0]["ofco_sHorarioVerano"].ToString());
            //    }
            //}

            dblDiferenciaHoraria = Convert.ToDouble(Session[Constantes.CONST_SESION_DIFERENCIA_HORARIA].ToString());
            dblHorarioVerano = Convert.ToDouble(Session[Constantes.CONST_SESION_HORARIO_VERANO].ToString().ToString());


            dblHorasConsiderar = Convert.ToDouble(dblDiferenciaHoraria + dblHorarioVerano);

            datFecha = datFecha.AddHours(dblHorasConsiderar);

            strFecha = datFecha.ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
            strHora = datFecha.ToString(ConfigurationManager.AppSettings["FormatoHora"]);            
        }
        //-----------------------------------------------------

        public static int CalcularEdad(DateTime dFechaNacimiento)
        {
            DateTime now = DateTime.Today;
            int age = now.Year - dFechaNacimiento.Year;
            if (now.Month < dFechaNacimiento.Month || (now.Month == dFechaNacimiento.Month && now.Day < dFechaNacimiento.Day))
                age--;
            return age;
        }

        #endregion

        #region ItextSharp

        public static string ObtenerTextoNotarialCierre(List<string> listTextos, float fAnchoAreaDocumento, List<iTextSharp.text.Font> listFonts)
        {
            float posx = 0;
            float posxAcumulado = 0;
            string textoAcumulado = string.Empty;
            string parrafo = string.Empty;
            int indiceTexto = 0;

            foreach (string texto in listTextos)
            {
                parrafo += texto.Trim();

                //if (texto.Trim() != "." && texto.Trim() != "," && texto.Trim() != ":" && texto.Trim() != ";" && texto.Trim() != "(" && texto.Trim() != ")" && texto.Trim() != "-")
                //{
                    parrafo += " ";
                //}
            }

            parrafo = parrafo.ToUpper().Trim();
            
            int cont = 0;
            foreach (string texto in parrafo.Split(' '))
            {
                //float WidthWithCharSpacing = chunk.GetWidthPoint() + chunk.GetCharacterSpacing() * (chunk.Content.Length - 1);

                iTextSharp.text.Font font = listFonts[ObtenerIndiceTextoEvaluado(listTextos, indiceTexto - 1)];
                
                posxAcumulado += new iTextSharp.text.Chunk(texto.Trim(), font).GetWidthPoint();
                

                posx = posxAcumulado;

                if (posx > fAnchoAreaDocumento)
                {
                    textoAcumulado = string.Empty;
                    posxAcumulado = new iTextSharp.text.Chunk(texto, font).GetWidthPoint();
                 

                    posx = posxAcumulado;
                }

                textoAcumulado += texto.Trim();

                //if (texto.Trim() != "." && texto.Trim() != "," && texto.Trim() != ":" && texto.Trim() != ";" && texto.Trim() != "(" && texto.Trim() != ")" && texto.Trim() != "-")
                //{
                    textoAcumulado += " ";
                    posxAcumulado += new iTextSharp.text.Chunk(" ", font).GetWidthPoint();
                //}


                indiceTexto += texto.Trim().Length + 1;



                cont++;

            }

            float posDiferencia = fAnchoAreaDocumento - posx;
            textoAcumulado = string.Empty;
            posx = 0;
            iTextSharp.text.Font font2 = new iTextSharp.text.Font(listFonts[0]);
            font2.SetStyle(0);

            //-------------------------------------------------------------
            //Fecha: 04/04/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Ajuste en el calculo de las lineas punteadas.
            //          Se disminuyo la variable: posDiferencia en -1.
            //-------------------------------------------------------------
            if (new iTextSharp.text.Chunk("=", font2).GetWidthPoint() <= posDiferencia-1)
            {
                while (new iTextSharp.text.Chunk(textoAcumulado + "=", font2).GetWidthPoint() <= posDiferencia - 1)
                {

                    textoAcumulado += "=";
                }
            }
            

            return textoAcumulado;
        }

    
        private static int ObtenerIndiceTextoEvaluado(List<string> listTextos, int indiceTexto)
        {

            int indice = 0;
            int longitudEvaluada = 0;
            foreach (string texto in listTextos)
            {
                if (texto.Length + longitudEvaluada > indiceTexto)
                {
                    return indice;
                }


                longitudEvaluada += texto.Length + 1;
                indice++;

                if (indice == listTextos.Count - 1)
                    return indice;
            }

            return indice;
        }

        string vLinea5Titulo = System.Configuration.ConfigurationManager.AppSettings["Linea5Titulo"].ToString();

        private iTextSharp.text.pdf.BaseFont _cuerpoBaseFont_;

        public iTextSharp.text.pdf.BaseFont CuerpoBaseFont_
        {
            get { return _cuerpoBaseFont_; }
            set { _cuerpoBaseFont_ = value; }
        }

        public static void CrearDocumentoiTextSharp(Page page, string textoHtml, string strConsulado, string imgLogo, List<DocumentoFirma> listFirmas = null, bool bAplicaCierreTextoNotarial = false, List<string> listPalabrasOmitirTextoNotarial = null, string nombreDocumento = "ItextSharpDocument", Boolean bImprimir = true)
        {
            try
            {

                #region Inicializando Variables


                float _fCuerpoFontSize = 10;

                float fMargenIzquierdaDoc = 50;
                float fMargenDerechaDoc = 50;
                float fMargenSuperiorDoc = 80;
                float fMargenInferiorDoc = 80;
                float fImageMargin = 50f;
                float fImageWidth = 57.77f;


                iTextSharp.text.IElement oIElement;
                iTextSharp.text.Paragraph oParagraph = null;

                List<iTextSharp.text.IElement> objects;
                string strContent = string.Empty;

                iTextSharp.text.FontFactory.RegisterDirectories();

                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, fMargenIzquierdaDoc, fMargenDerechaDoc, fMargenSuperiorDoc, fMargenInferiorDoc);

                iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);

                //String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
                //string strRutaPDF = uploadPath + @"\" + nombreDocumento + "_" + DateTime.Now.Ticks.ToString() + ".pdf";
                //string strRutaHtml = uploadPath + @"\" + nombreDocumento + "_" + DateTime.Now.Ticks.ToString() + ".html";

                #endregion
                //iTextSharp.text.Font _fontGeneral = new iTextSharp.text.Font(iTextSharp.text.pdf.BaseFont.TIMES_ROMAN, _fCuerpoFontSize);
                //iTextSharp.text.Font _fontGeneral = new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 10);

                

                using (var ms = new MemoryStream())
                {
                    StreamWriter str = new StreamWriter(ms, Encoding.Default);
                    str.Write("                    " + textoHtml);
                    str.Flush();

                    ms.Position = 0;

                    StreamReader oStreamReader = new StreamReader(ms, System.Text.Encoding.Default);

                    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms);

                    document.Open();
                    document.NewPage();

                    objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(oStreamReader, styles);
                    float fAnchoAreaTexto = document.PageSize.Width - fMargenDerechaDoc - fMargenIzquierdaDoc;

                    #region Imagen

                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imgLogo);
                    img.SetAbsolutePosition(fImageMargin, document.PageSize.Height - 100);
                    img.ScaleAbsoluteHeight(85f);
                    img.ScaleAbsoluteWidth(fImageWidth);
                    document.Add(img);

                    #endregion

                    #region Consulado Imagen

                    iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                    iTextSharp.text.pdf.BaseFont bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);

                    cb.BeginText();

                    cb.SetFontAndSize(bfTimes, 6);

                      string texto = string.Empty;

                    float pos = 0;
                    float tamPalabra = 0;
                    float ancho = 80f;
                    iTextSharp.text.Font fontConsulado = iTextSharp.text.FontFactory.GetFont("Arial", 6);

                    if (strConsulado.ToUpper().Contains("CONSULADO GENERAL DEL PERÚ"))
                    {
                        ancho = new iTextSharp.text.Chunk("CONSULADO GENERAL DEL PERÚ", fontConsulado).GetWidthPoint() + 5;
                        strConsulado = strConsulado.ToUpper().Replace("PERÚ EN", "PERÚ");
                    }

                    int iPosicionComa = strConsulado.IndexOf(",");

                    if (iPosicionComa >= 0)
                        strConsulado = strConsulado.Substring(0, iPosicionComa);


                    float posxAcumulado = tamPalabra;

                    foreach (string palabra in strConsulado.Split(' '))
                    {
                        tamPalabra = new iTextSharp.text.Chunk(palabra.Trim(), fontConsulado).GetWidthPoint();

                        if (posxAcumulado + tamPalabra > ancho)
                        {



                            cb.SetTextMatrix(fImageMargin + (fImageWidth / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f), document.PageSize.Height - 110 + pos);
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
                        cb.SetTextMatrix(fImageMargin + (fImageWidth / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f), document.PageSize.Height - 110 + pos);
                        cb.ShowText(texto.Trim());
                    }





                    cb.EndText();

                    #endregion

                    #region Cuerpo Documento

                    #region Font Settings

                    string vFontFamilyExtraProtocolar = System.Configuration.ConfigurationManager.AppSettings["vFontFamilyExtraProtocolar"].ToString();
                    float fFontSizeExtraProtocolar = float.Parse(System.Configuration.ConfigurationManager.AppSettings["fFontSizeExtraProtocolarPFD"].ToString());


                    if (fFontSizeExtraProtocolar <= 0)
                    {
                        fFontSizeExtraProtocolar = 12;
                    }

                    string fontPath=HttpContext.Current.Server.MapPath("~/Fonts/"+ vFontFamilyExtraProtocolar +".ttf");


                    iTextSharp.text.pdf.BaseFont baseFont;
                    iTextSharp.text.Font fontGeneral = new iTextSharp.text.Font();

                    if (File.Exists(fontPath))
                    {
                        baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(fontPath, iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);
                        fontGeneral= new iTextSharp.text.Font(baseFont,fFontSizeExtraProtocolar);
                    }
                    else
                    {

                        fontGeneral = iTextSharp.text.FontFactory.GetFont("Arial",fFontSizeExtraProtocolar);
                    }


                    #endregion


                    for (int k = 0; k < objects.Count; k++)
                    {
                        oIElement = (iTextSharp.text.IElement)objects[k];
                        if (objects[k].GetType().FullName == "iTextSharp.text.Paragraph")
                        {
                            oParagraph = new iTextSharp.text.Paragraph();
                            oParagraph.Alignment = ((iTextSharp.text.Paragraph)objects[k]).Alignment;
                            
                            cb = writer.DirectContent;
                            iTextSharp.text.pdf.ColumnText ct = new iTextSharp.text.pdf.ColumnText(cb);

                            for (int z = 0; z < oIElement.Chunks.Count; z++)
                            {
                                strContent = oIElement.Chunks[z].Content.ToString().ToUpper();

                                if (!bAplicaCierreTextoNotarial)
                                {
                                    if (strContent.ToUpper() == "PODER FUERA DE REGISTRO" ||
                                            strContent.ToUpper() == "CERTIFICADO DE SUPERVIVENCIA" ||
                                            //strContent.ToUpper() == "AUTORIZACIÓN DE VIAJE DE MENOR AL EXTERIOR")
                                            strContent.ToUpper() == "AUTORIZACIÓN DE VIAJE DE MENOR")
                                    {
                                        oParagraph.Add(new iTextSharp.text.Chunk(strContent, oIElement.Chunks[z].Font));
                                    }
                                    else
                                    {
                                        oParagraph.Add(new iTextSharp.text.Chunk(strContent, fontGeneral));
                                    }
                                }
                                else
                                {
                                    #region Aplica cierre Texto Notarial, es decir, "============"

                                    if (strContent != "\n")
                                    {
                                        strContent = strContent.Trim();
                                        if (strContent.ToUpper() == "PODER FUERA DE REGISTRO" ||
                                            strContent.ToUpper() == "CERTIFICADO DE SUPERVIVENCIA" ||
                                            //strContent.ToUpper() == "AUTORIZACIÓN DE VIAJE DE MENOR AL EXTERIOR")
                                            strContent.ToUpper() == "AUTORIZACIÓN DE VIAJE DE MENOR")
                                        {
                                            oParagraph.Add(new iTextSharp.text.Chunk(strContent, oIElement.Chunks[z].Font));
                                        }
                                        else
                                        {
                                            oParagraph.Add(new iTextSharp.text.Chunk(strContent, fontGeneral));
                                        }
                                    }
                                    else
                                    {
                                        oParagraph.Add(new iTextSharp.text.Chunk(strContent, fontGeneral));
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
                                            listFonts.Add(fontGeneral);

                                        }


                                        bool bEscribirTextoCierro = true;

                                        if (listPalabrasOmitirTextoNotarial != null)
                                        {

                                            foreach (string palabra in listPalabrasOmitirTextoNotarial)
                                            {
                                                if (strContent.Trim() != string.Empty && strContent.Trim() == palabra)
                                                {
                                                    bEscribirTextoCierro = false;
                                                    break;
                                                }

                                            }
                                            if (strContent.Trim() != string.Empty &&
                                                 strContent.Trim() != "PODER FUERA DE REGISTRO" &&
                                                 strContent.Trim() != "CONCLUSIÓN:")
                                            {

                                            }
                                        }

                                        if (bEscribirTextoCierro)
                                        {
                                            textoNotarialCierre = Comun.ObtenerTextoNotarialCierre(listTextos, fAnchoAreaTexto, listFonts);
                                        }


                                        if (textoNotarialCierre != string.Empty)
                                        {
                                            iTextSharp.text.Font font = new iTextSharp.text.Font(fontGeneral);
                                            
                                            font.SetStyle(0);
                                            oParagraph.Add(new iTextSharp.text.Chunk(textoNotarialCierre, font));
                                        }

                                    }
                                    else
                                    {
                                        oParagraph.Add(new iTextSharp.text.Chunk(" ", fontGeneral));
                                    }

                                    #endregion
                                }

                            }

                            oParagraph.SetLeading(0.0f, 0.9f);
                            document.Add(oParagraph);
                        }

                    }


                    #endregion

                    #region Firmas

                    if (listFirmas != null)
                    {
                        iTextSharp.text.Paragraph parrafo = new iTextSharp.text.Paragraph();
                        iTextSharp.text.Phrase frase = new iTextSharp.text.Phrase();

                        foreach (DocumentoFirma docFirma in listFirmas)
                        {
                            parrafo = new iTextSharp.text.Paragraph();
                            frase = new iTextSharp.text.Phrase();

                            if (writer.GetVerticalPosition(false) >= (fMargenSuperiorDoc + 120))
                            {

                                frase.Add(new iTextSharp.text.Chunk("\n\n\n\n\n"));
                                parrafo.Add(frase);
                                document.Add(parrafo);
                            }
                            else
                            {

                                while (writer.GetVerticalPosition(false) < (fMargenSuperiorDoc + 120))
                                {
                                    document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n")));
                                }
                            }

                            parrafo = new iTextSharp.text.Paragraph();
                            frase = new iTextSharp.text.Phrase();
                            frase.Add(new iTextSharp.text.pdf.draw.LineSeparator(0.5F, 30.0F, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_LEFT, 1));
                            parrafo.Add(frase);
                            document.Add(parrafo);

                            if (docFirma.bAplicaHuellaDigital)
                            {
                                cb = writer.DirectContent;
                                cb.Rectangle(document.PageSize.Width - 360, writer.GetVerticalPosition(false) - 15, 50f, 65f);
                                cb.Stroke();
                            }

                            parrafo = new iTextSharp.text.Paragraph();
                            frase = new iTextSharp.text.Phrase();
                            frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNombreCompleto,fontGeneral));
                            frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNroDocumentoCompleto,fontGeneral));
                            parrafo.Add(frase);
                            document.Add(parrafo);

                        }
                    }

                    #endregion

                    document.Close();

                    #region Impresion en Navegador



                    Byte[] FileBuffer = ms.ToArray();
                    if (FileBuffer != null)
                    {
                        System.Web.HttpContext.Current.Session["binaryData"] = FileBuffer;

                        if (bImprimir)
                        {
                            string strUrl = "../Accesorios/VisorPDF.aspx";
                            string strScript = "window.open('" + strUrl + "', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";
                            Comun.EjecutarScript(page, strScript);
                        }
                    }


                    str.Close();
                    oStreamReader.Close();
                    oStreamReader.Dispose();

                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CrearDocumentoiTextSharpExtProto(Page page, string textoHtml, string strConsulado, string imgLogo, List<DocumentoFirma> listFirmas = null, bool bAplicaCierreTextoNotarial = false, List<string> listPalabrasOmitirTextoNotarial = null, string nombreDocumento = "ItextSharpDocument", Boolean bImprimir = true, double _tamanoLetra = 10)
        {
            try
            {

                #region Inicializando Variables


                float _fCuerpoFontSize = 10;

                float fMargenIzquierdaDoc = 80;
                float fMargenDerechaDoc = 80;
                float fMargenSuperiorDoc = 100;
                float fMargenInferiorDoc = 30;
                float fImageMargin = 50f;
                float fImageWidth = 57.77f;


                iTextSharp.text.IElement oIElement;
                iTextSharp.text.Paragraph oParagraph = null;

                List<iTextSharp.text.IElement> objects;
                string strContent = string.Empty;

                iTextSharp.text.FontFactory.RegisterDirectories();

                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, fMargenIzquierdaDoc, fMargenDerechaDoc, fMargenSuperiorDoc, fMargenInferiorDoc);

                iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);

                //String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
                //string strRutaPDF = uploadPath + @"\" + nombreDocumento + "_" + DateTime.Now.Ticks.ToString() + ".pdf";
                //string strRutaHtml = uploadPath + @"\" + nombreDocumento + "_" + DateTime.Now.Ticks.ToString() + ".html";

                #endregion
                //iTextSharp.text.Font _fontGeneral = new iTextSharp.text.Font(iTextSharp.text.pdf.BaseFont.TIMES_ROMAN, _fCuerpoFontSize);
                //iTextSharp.text.Font _fontGeneral = new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 10);



                using (var ms = new MemoryStream())
                {
                    StreamWriter str = new StreamWriter(ms, Encoding.Default);
                    str.Write("                    " + textoHtml);
                    str.Flush();

                    ms.Position = 0;

                    StreamReader oStreamReader = new StreamReader(ms, System.Text.Encoding.Default);

                    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms);

                    document.Open();
                    document.NewPage();

                    objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(oStreamReader, styles);
                    float fAnchoAreaTexto = document.PageSize.Width - fMargenDerechaDoc - fMargenIzquierdaDoc;

                    #region Imagen

                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imgLogo);
                    img.SetAbsolutePosition(fImageMargin, document.PageSize.Height - 100);
                    img.ScaleAbsoluteHeight(85f);
                    img.ScaleAbsoluteWidth(fImageWidth);
                    document.Add(img);

                    #endregion

                    #region Consulado Imagen

                    iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                    iTextSharp.text.pdf.BaseFont bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);

                    cb.BeginText();

                    cb.SetFontAndSize(bfTimes, 6);

                    string texto = string.Empty;

                    float pos = 0;
                    float tamPalabra = 0;
                    float ancho = 80f;
                    iTextSharp.text.Font fontConsulado = iTextSharp.text.FontFactory.GetFont("Arial", 6);

                    if (strConsulado.ToUpper().Contains("CONSULADO GENERAL DEL PERÚ"))
                    {
                        ancho = new iTextSharp.text.Chunk("CONSULADO GENERAL DEL PERÚ", fontConsulado).GetWidthPoint() + 5;
                        strConsulado = strConsulado.ToUpper().Replace("PERÚ EN", "PERÚ");
                    }

                    int iPosicionComa = strConsulado.IndexOf(",");

                    if (iPosicionComa >= 0)
                        strConsulado = strConsulado.Substring(0, iPosicionComa);


                    float posxAcumulado = tamPalabra;

                    foreach (string palabra in strConsulado.Split(' '))
                    {
                        tamPalabra = new iTextSharp.text.Chunk(palabra.Trim(), fontConsulado).GetWidthPoint();

                        if (posxAcumulado + tamPalabra > ancho)
                        {



                            cb.SetTextMatrix(fImageMargin + (fImageWidth / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f), document.PageSize.Height - 110 + pos);
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
                        cb.SetTextMatrix(fImageMargin + (fImageWidth / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f), document.PageSize.Height - 110 + pos);
                        cb.ShowText(texto.Trim());
                    }





                    cb.EndText();

                    #endregion

                    #region Cuerpo Documento

                    #region Font Settings

                    string vFontFamilyExtraProtocolar = System.Configuration.ConfigurationManager.AppSettings["vFontFamilyExtraProtocolar"].ToString();
                    //float fFontSizeExtraProtocolar = float.Parse(System.Configuration.ConfigurationManager.AppSettings["fFontSizeExtraProtocolar"].ToString());
                    float fFontSizeExtraProtocolar = 0;

                    try
                    {
                        fFontSizeExtraProtocolar = float.Parse(_tamanoLetra.ToString());
                    }
                    catch (Exception)
                    {
                        fFontSizeExtraProtocolar = 0;
                        throw;
                    }


                    if (fFontSizeExtraProtocolar <= 0)
                    {
                        fFontSizeExtraProtocolar = 12;
                    }

                    string fontPath = HttpContext.Current.Server.MapPath("~/Fonts/" + vFontFamilyExtraProtocolar + ".ttf");


                    iTextSharp.text.pdf.BaseFont baseFont;
                    iTextSharp.text.Font fontGeneral = new iTextSharp.text.Font();
                    iTextSharp.text.Font fontGeneralBold = new iTextSharp.text.Font();

                    if (File.Exists(fontPath))
                    {
                        baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(fontPath, iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);                        
                        fontGeneral = new iTextSharp.text.Font(baseFont, fFontSizeExtraProtocolar);
                    }
                    else
                    {
                        fontGeneral = iTextSharp.text.FontFactory.GetFont("Arial", fFontSizeExtraProtocolar);
                    }

                    iTextSharp.text.pdf.BaseFont baseFontBold = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);
                    fontGeneralBold = new iTextSharp.text.Font(baseFontBold, fFontSizeExtraProtocolar);

                    #endregion

                    bool isCertificadoSupervivencia = false;
                    bool isAutorizacionViajeMenorExtranjero = false;

                    for (int k = 0; k < objects.Count; k++)
                    {
                        oIElement = (iTextSharp.text.IElement)objects[k];
                        if (objects[k].GetType().FullName == "iTextSharp.text.Paragraph")
                        {
                            oParagraph = new iTextSharp.text.Paragraph();
                            oParagraph.Alignment = ((iTextSharp.text.Paragraph)objects[k]).Alignment;

                            cb = writer.DirectContent;
                            iTextSharp.text.pdf.ColumnText ct = new iTextSharp.text.pdf.ColumnText(cb);

                            for (int z = 0; z < oIElement.Chunks.Count; z++)
                            {
                                strContent = oIElement.Chunks[z].Content.ToString().ToUpper();

                                if (!bAplicaCierreTextoNotarial)
                                {
                                    if (strContent.ToUpper() == "CERTIFICADO DE SUPERVIVENCIA")
                                    {isCertificadoSupervivencia = true;}

                                    //if (strContent.ToUpper() == "AUTORIZACIÓN DE VIAJE DE MENOR AL EXTERIOR")
                                    if (strContent.ToUpper() == "AUTORIZACIÓN DE VIAJE DE MENOR")
                                    { isAutorizacionViajeMenorExtranjero = true; }

                                    if (strContent.ToUpper() == "PODER FUERA DE REGISTRO" ||
                                            strContent.ToUpper() == "CERTIFICADO DE SUPERVIVENCIA" ||
                                            //strContent.ToUpper() == "AUTORIZACIÓN DE VIAJE DE MENOR AL EXTERIOR")
                                            strContent.ToUpper() == "AUTORIZACIÓN DE VIAJE DE MENOR")
                                    {
                                        oParagraph.Add(new iTextSharp.text.Chunk(strContent, oIElement.Chunks[z].Font));
                                    }
                                    else
                                    {
                                        //---------------------------------------------------------------
                                        //Fecha: 06/03/2017
                                        //Autor: Miguel Márquez Beltrán
                                        //Objetivo: Poner el negrita el texto: CERTIFICO
                                        //---------------------------------------------------------------

                                        if (isCertificadoSupervivencia && strContent.IndexOf("CERTIFICO") > -1)
                                        {
                                            int intCertifico = strContent.IndexOf("CERTIFICO");

                                            oParagraph.Add(new iTextSharp.text.Chunk(strContent.Substring(0,intCertifico), fontGeneral));
                                            oParagraph.Add(new iTextSharp.text.Chunk("CERTIFICO", fontGeneralBold));
                                            oParagraph.Add(new iTextSharp.text.Chunk(strContent.Substring(intCertifico+9), fontGeneral));
                                        }
                                        else
                                        {
                                            oParagraph.Add(new iTextSharp.text.Chunk(strContent, fontGeneral));
                                        }
                                    }
                                }
                                else
                                {
                                    #region Aplica cierre Texto Notarial, es decir, "============"

                                    if (strContent != "\n")
                                    {
                                        strContent = strContent.Trim();
                                        if (strContent.ToUpper() == "PODER FUERA DE REGISTRO" ||
                                            strContent.ToUpper() == "CERTIFICADO DE SUPERVIVENCIA" ||
                                            //strContent.ToUpper() == "AUTORIZACIÓN DE VIAJE DE MENOR AL EXTERIOR")
                                            strContent.ToUpper() == "AUTORIZACIÓN DE VIAJE DE MENOR")
                                        {

                                            //if (strContent.ToUpper() == "AUTORIZACIÓN DE VIAJE DE MENOR AL EXTERIOR")
                                            if (strContent.ToUpper() == "AUTORIZACIÓN DE VIAJE DE MENOR")
                                            { isAutorizacionViajeMenorExtranjero = true; }

                                            oParagraph.Add(new iTextSharp.text.Chunk(strContent, oIElement.Chunks[z].Font));
                                        }
                                        else
                                        {
                                            oParagraph.Add(new iTextSharp.text.Chunk(strContent, fontGeneral));
                                        }
                                    }
                                    else
                                    {
                                        oParagraph.Add(new iTextSharp.text.Chunk(strContent, fontGeneral));
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
                                            listFonts.Add(fontGeneral);

                                        }


                                        bool bEscribirTextoCierro = true;

                                        if (listPalabrasOmitirTextoNotarial != null)
                                        {

                                            foreach (string palabra in listPalabrasOmitirTextoNotarial)
                                            {
                                                if (strContent.Trim() != string.Empty && strContent.Trim() == palabra)
                                                {
                                                    bEscribirTextoCierro = false;
                                                    break;
                                                }

                                            }
                                            if (strContent.Trim() != string.Empty &&
                                                 strContent.Trim() != "PODER FUERA DE REGISTRO" &&
                                                 strContent.Trim() != "CONCLUSIÓN:")
                                            {

                                            }
                                        }

                                        if (bEscribirTextoCierro)
                                        {
                                            textoNotarialCierre = Comun.ObtenerTextoNotarialCierre(listTextos, fAnchoAreaTexto, listFonts);
                                        }


                                        if (textoNotarialCierre != string.Empty)
                                        {
                                            iTextSharp.text.Font font = new iTextSharp.text.Font(fontGeneral);
                                            font.SetStyle(0);
                                            oParagraph.Add(new iTextSharp.text.Chunk(textoNotarialCierre, font));
                                        }

                                    }
                                    else
                                    {
                                        oParagraph.Add(new iTextSharp.text.Chunk(" ", fontGeneral));
                                    }

                                    #endregion
                                }

                            }

                            oParagraph.SetLeading(0.0f, 1.6f);
                            document.Add(oParagraph);
                        }

                    }


                    #endregion

                    #region Firmas

                    if (listFirmas != null)
                    {
                        iTextSharp.text.Paragraph parrafo = new iTextSharp.text.Paragraph();
                        iTextSharp.text.Phrase frase = new iTextSharp.text.Phrase();

                        foreach (DocumentoFirma docFirma in listFirmas)
                        {
                            parrafo = new iTextSharp.text.Paragraph();
                            frase = new iTextSharp.text.Phrase();

                            if (writer.GetVerticalPosition(false) >= (fMargenSuperiorDoc + 70))
                            {

                                frase.Add(new iTextSharp.text.Chunk("\n\n\n\n"));
                                parrafo.Add(frase);
                                document.Add(parrafo);
                            }
                            else
                            {

                                while (writer.GetVerticalPosition(false) < (fMargenSuperiorDoc + 70))
                                {
                                    document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n")));
                                }
                            }

                            if (isAutorizacionViajeMenorExtranjero == false)
                            {
                                //Cuando es Certificado de Supervivencia
                                if (docFirma.bAplicaHuellaDigital)
                                {                                
                                    parrafo = new iTextSharp.text.Paragraph();
                                    frase = new iTextSharp.text.Phrase();
                                    
                                    frase.Add(new iTextSharp.text.pdf.draw.LineSeparator(1F, 50.0F, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_LEFT, 1));

                                    parrafo.Add(frase);
                                    document.Add(parrafo);

                                    cb = writer.DirectContent;

                                    cb.Rectangle(document.PageSize.Width - 290, writer.GetVerticalPosition(false) - 15, 60f, 70f);

                                    cb.Stroke();

                                    //---------------------------------------------
                                    //Imprimir el Nombre completo
                                    //---------------------------------------------
                                    #region Imprimir el Nombre Completo
                                    parrafo = new iTextSharp.text.Paragraph();

                                    frase = new iTextSharp.text.Phrase();

                                    frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNombreCompleto, fontGeneral));
                                    frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNroDocumentoCompleto, fontGeneral));

                                    parrafo.Add(frase);
                                    document.Add(parrafo);
                                    #endregion
                                    //------------------------------------------------
                                }                                                                   
                            }
                            else
                            {
                                #region Es Autorizacion Viaje de Menor

                                //---------------------------------------------------------------------------------------
                                //Fecha: 18/09/2017
                                //Autor: Miguel Márquez Beltrán
                                // Objetivo: Fragmento de código solo para Autorización de Viaje de Menor al Exterior
                                //---------------------------------------------------------------------------------------
                                parrafo = new iTextSharp.text.Paragraph();
                                frase = new iTextSharp.text.Phrase();

                                if (docFirma.bIncapacitado && docFirma.bImprimirFirma == false)
                                {
                                    //Esta incapacitado y no puede firmar ni poner su huella
                                    //----------------------------------
                                    // Testear
                                    //----------------------------------
                                    iTextSharp.text.pdf.BaseFont bfHelvetica = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA, iTextSharp.text.pdf.BaseFont.CP1252, false);
                                    cb = writer.DirectContent;

                                    #region Testear la Firma
                                    //-------------------------------------
                                    //Fecha: 20/09/2017
                                    //Autor: Miguel Márquez Beltrán
                                    //Objetivo: Testear la firma
                                    //-------------------------------------
                                    cb.SetFontAndSize(bfHelvetica, 7);
                                    cb.SetLineDash(7f, 2f, 0f);
                                   
                                    cb.MoveTo(fMargenIzquierdaDoc, writer.GetVerticalPosition(false) + 5);
                                    cb.LineTo(fMargenIzquierdaDoc + 215, writer.GetVerticalPosition(false) + 5);
                                    cb.Stroke();

                                    cb.MoveTo(fMargenIzquierdaDoc, writer.GetVerticalPosition(false) + 8);
                                    cb.LineTo(fMargenIzquierdaDoc + 215, writer.GetVerticalPosition(false) + 8);
                                    cb.Stroke();
                                    #endregion

                                    if (docFirma.bAplicaHuellaDigital == false)
                                    {
                                        //-------------------------------------

                                        #region Testear Huella Dactilar

                                        //--------------------------------------
                                        //Fecha: 20/09/2017
                                        //Autor: Miguel Márquez Beltrán
                                        //Objetivo: Testear la huella dactilar
                                        //--------------------------------------
                                        cb.MoveTo(fMargenIzquierdaDoc + 229, writer.GetVerticalPosition(false) + 5);
                                        cb.LineTo(fMargenIzquierdaDoc + 270, writer.GetVerticalPosition(false) + 5);
                                        cb.Stroke();

                                        cb.MoveTo(fMargenIzquierdaDoc + 229, writer.GetVerticalPosition(false) + 8);
                                        cb.LineTo(fMargenIzquierdaDoc + 270, writer.GetVerticalPosition(false) + 8);
                                        cb.Stroke();
                                        #endregion
                                    }
                                    parrafo.Add(frase);
                                    document.Add(parrafo);

                                    //----------------------------------
                                    frase = new iTextSharp.text.Phrase();
                                    parrafo = new iTextSharp.text.Paragraph();
                                }
                                                               
                                //---------------------------------------------
                                //Imprimir la linea para la firma
                                //---------------------------------------------
                                frase.Add(new iTextSharp.text.pdf.draw.LineSeparator(1F, 50.0F, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_LEFT, 1));
                                //---------------------------------------------
                                parrafo.Add(frase);
                                document.Add(parrafo);

                                //---------------------------------------------
                                //Imprimir el Cuadro de la huella digital
                                //---------------------------------------------
                                #region Imprimir el Cuadro de la huella digital
                                cb = writer.DirectContent;
                                cb.SetLineDash(1);
                                cb.Rectangle(document.PageSize.Width - 290, writer.GetVerticalPosition(false) - 15, 50f, 60f);

                                cb.Stroke();
                                parrafo.Add(frase);
                                document.Add(parrafo);
                                #endregion
                                //---------------------------------------------
                                //Imprimir el Nombre completo
                                //---------------------------------------------

                                #region Imprimir el Nombre Completo

                                if (docFirma.vSubTipoActa == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE Y MADRE")
                                {
                                    parrafo = new iTextSharp.text.Paragraph();

                                    frase = new iTextSharp.text.Phrase();

                                    frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNombreCompletoRepresentar, fontGeneral));
                                    frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNroDocumentoCompletoRepresentar, fontGeneral));
                                    frase.Add(new iTextSharp.text.Chunk("\n" + "EN REPRESENTACIÓN DE:", fontGeneral));
                                    frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNombreCompleto, fontGeneral));
                                    frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNroDocumentoCompleto, fontGeneral));

                                    parrafo.Add(frase);
                                    document.Add(parrafo);
                                }

                                else if (docFirma.vSubTipoActa == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A MADRE" ||
                                    docFirma.vSubTipoActa == "REPRESENTACIÓN DE APODERADO - APODERADO REPRESENTA A PADRE")
                                {
                                    if (docFirma.sTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.APODERADO))
                                    {
                                        parrafo = new iTextSharp.text.Paragraph();

                                        frase = new iTextSharp.text.Phrase();

                                        frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNombreCompleto, fontGeneral));
                                        frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNroDocumentoCompleto, fontGeneral));
                                        frase.Add(new iTextSharp.text.Chunk("\n" + "EN REPRESENTACIÓN DE:", fontGeneral));
                                        frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNombreCompletoRepresentar, fontGeneral));
                                        frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNroDocumentoCompletoRepresentar, fontGeneral));
                                        parrafo.Add(frase);
                                        document.Add(parrafo);
                                    }
                                    else
                                    {
                                        parrafo = new iTextSharp.text.Paragraph();

                                        frase = new iTextSharp.text.Phrase();

                                        frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNombreCompleto, fontGeneral));
                                        frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNroDocumentoCompleto, fontGeneral));

                                        parrafo.Add(frase);
                                        document.Add(parrafo);
                                    }

                                }
                                else if (docFirma.vSubTipoActa == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - PADRE REPRESENTA A MADRE")
                                {
                                    if (docFirma.sTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.PADRE))
                                    {
                                        parrafo = new iTextSharp.text.Paragraph();

                                        frase = new iTextSharp.text.Phrase();

                                        frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNombreCompleto, fontGeneral));
                                        frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNroDocumentoCompleto, fontGeneral));
                                        frase.Add(new iTextSharp.text.Chunk("\n" + "EN REPRESENTACIÓN DE:", fontGeneral));
                                        frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNombreCompletoRepresentar, fontGeneral));
                                        frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNroDocumentoCompletoRepresentar, fontGeneral));
                                        parrafo.Add(frase);
                                        document.Add(parrafo);
                                    }
                                    else {
                                        parrafo = new iTextSharp.text.Paragraph();

                                        frase = new iTextSharp.text.Phrase();

                                        frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNombreCompleto, fontGeneral));
                                        frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNroDocumentoCompleto, fontGeneral));

                                        parrafo.Add(frase);
                                        document.Add(parrafo);
                                    }
                                    
                                }
                                else if (docFirma.vSubTipoActa == "DERECHO PROPIO Y EN REPRESENTACIÓN DE - MADRE REPRESENTA A PADRE")
                                {
                                    if (docFirma.sTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.MADRE))
                                    {
                                        parrafo = new iTextSharp.text.Paragraph();

                                        frase = new iTextSharp.text.Phrase();

                                        frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNombreCompleto, fontGeneral));
                                        frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNroDocumentoCompleto, fontGeneral));
                                        frase.Add(new iTextSharp.text.Chunk("\n" + "EN REPRESENTACIÓN DE:", fontGeneral));
                                        frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNombreCompletoRepresentar, fontGeneral));
                                        frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNroDocumentoCompletoRepresentar, fontGeneral));
                                        parrafo.Add(frase);
                                        document.Add(parrafo);
                                    }
                                    else {
                                        parrafo = new iTextSharp.text.Paragraph();

                                        frase = new iTextSharp.text.Phrase();

                                        frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNombreCompleto, fontGeneral));
                                        frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNroDocumentoCompleto, fontGeneral));

                                        parrafo.Add(frase);
                                        document.Add(parrafo);
                                    }
                                }
                                else {
                                    parrafo = new iTextSharp.text.Paragraph();

                                    frase = new iTextSharp.text.Phrase();

                                    frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNombreCompleto, fontGeneral));
                                    frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNroDocumentoCompleto, fontGeneral));

                                    parrafo.Add(frase);
                                    document.Add(parrafo);
                                }
                                
                                #endregion

                                #endregion
                                //-----------------------------------------------                                                                                   
                            }
                        }
                    }

                    #endregion

                    document.Close();

                    #region Impresion en Navegador



                    Byte[] FileBuffer = ms.ToArray();
                    if (FileBuffer != null)
                    {
                        System.Web.HttpContext.Current.Session["binaryData"] = FileBuffer;

                        if (bImprimir)
                        {
                            string strUrl = "../Accesorios/VisorPDF.aspx";
                            string strScript = "window.open('" + strUrl + "', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";
                            Comun.EjecutarScript(page, strScript);
                        }
                    }


                    str.Close();
                    oStreamReader.Close();
                    oStreamReader.Dispose();

                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void CrearDocumentoiTextSharpGeneral(Page page, string textoHtml, string nombreConsulado, string imgLogo, float ffontSize, ref int iPageNumber, List<DocumentoFirma> listFirmas = null, bool bAplicaCierreTextoNotarial = false, List<string> listPalabrasOmitirTextoNotarial = null, List<TextoPosicionadoITextSharp> listTextoPosicionado = null)
        {
            try
            {

                #region Inicializando Variables

                iTextSharp.text.Paragraph parrafo = new iTextSharp.text.Paragraph();
                iTextSharp.text.Phrase frase = new iTextSharp.text.Phrase();

                float fTextLeading = 1.7f;
                float fImageMargin=25f;
                float fMargenIzquierdaDoc = 80;
                float fMargenDerechaDoc = 80;
                float fMargenInferiorDoc = 110;
                float fMargenSuperiorDoc = (842 - (ffontSize * fTextLeading * 25) - fMargenInferiorDoc);
                

                iTextSharp.text.IElement oIElement;
                iTextSharp.text.Paragraph oParagraph = null;

                List<iTextSharp.text.IElement> objects;
                string strContent = string.Empty;

                iTextSharp.text.FontFactory.RegisterDirectories();

                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, fMargenIzquierdaDoc, fMargenDerechaDoc, fMargenSuperiorDoc, fMargenInferiorDoc);


                

                iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);


                #endregion


                using (var ms = new MemoryStream())
                {



                    StreamWriter str = new StreamWriter(ms, Encoding.Default);
                    str.Write("                    " + textoHtml);
                    str.Flush();


                    ms.Position = 0;

                    StreamReader oStreamReader = new StreamReader(ms, System.Text.Encoding.Default);


                    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms);

                    document.Open();
                    document.NewPage();

                    objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(oStreamReader, styles);
                    float fAnchoAreaTexto = document.PageSize.Width - fMargenDerechaDoc - fMargenIzquierdaDoc;

                    float fPosicion = fMargenSuperiorDoc;
                    float fTextSize = 0;

                    #region Imagen

                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imgLogo);
                    img.SetAbsolutePosition(fImageMargin, document.PageSize.Height - 100);
                    img.ScaleAbsoluteHeight(83.2f);
                    img.ScaleAbsoluteWidth(80f);
                    document.Add(img);

                    #endregion

                    #region Consulado Imagen

                    iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                    iTextSharp.text.pdf.BaseFont bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);

                    cb.BeginText();

                    cb.SetFontAndSize(bfTimes, 7);

                    //string consuladoBlaBla = "CONSULADO GENERAL DEL PERÚ EN MIAMI";

                    string texto = string.Empty;

                    float pos = 0;
                    float tamPalabra = 0;
                    float ancho = 80f;

                    iTextSharp.text.Font fontConsulado = iTextSharp.text.FontFactory.GetFont("Arial",7);


                    float posxAcumulado = tamPalabra;

                    foreach (string palabra in nombreConsulado.Split(' '))
                    {
                        tamPalabra = new iTextSharp.text.Chunk(palabra.Trim(), fontConsulado).GetWidthPoint();

                        if (posxAcumulado + tamPalabra > ancho)
                        {



                            cb.SetTextMatrix(fImageMargin + (ancho / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f), document.PageSize.Height - 117 + pos);
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
                        cb.SetTextMatrix(fImageMargin + (ancho / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f), document.PageSize.Height - 117 + pos);
                        cb.ShowText(texto.Trim());
                    }





                    cb.EndText();

                    #endregion

                    #region Texto Posicionado


                    cb.BeginText();


                    foreach (TextoPosicionadoITextSharp textoPosicionado in listTextoPosicionado)
                    {
                        

                        cb.SetFontAndSize(textoPosicionado.BaseFont, textoPosicionado.FontSize);
                        
                        float tamanoPalabra = new iTextSharp.text.Chunk(textoPosicionado.Texto, textoPosicionado.FontFamily).GetWidthPoint();

                        if (textoPosicionado.TextAligment == HorizontalAlign.NotSet)
                        {
                            cb.SetTextMatrix(textoPosicionado.FXPosition, document.PageSize.Height - textoPosicionado.FYPosition);
                        }
                        else if (textoPosicionado.TextAligment == HorizontalAlign.Left)
                        {
                            cb.SetTextMatrix(fMargenIzquierdaDoc, document.PageSize.Height - textoPosicionado.FYPosition);
                        }
                        else if (textoPosicionado.TextAligment == HorizontalAlign.Right)
                        {                            
                            float posicionDerecha = (document.PageSize.Width - fMargenDerechaDoc) - tamanoPalabra;
                            cb.SetTextMatrix(posicionDerecha, document.PageSize.Height -  textoPosicionado.FYPosition);
                        }
                        else if (textoPosicionado.TextAligment == HorizontalAlign.Center)
                        {                            
                            float posicionCentrada = ((document.PageSize.Width/2) - (tamanoPalabra/2));
                            cb.SetTextMatrix(posicionCentrada, document.PageSize.Height- textoPosicionado.FYPosition);
                        }
                        cb.ShowText(textoPosicionado.Texto);

                        

                        if (textoPosicionado.FYPosition > fPosicion)
                        {
                            fPosicion = textoPosicionado.FYPosition;
                            fTextSize = textoPosicionado.FontSize;
                        }    
                    }

                    cb.EndText();


                    #endregion

                    #region Cuerpo Documento


                    float fVerticalPositionActual = writer.GetVerticalPosition(false);

                    iPageNumber = 1;
                  
                    for (int k = 0; k < objects.Count; k++)
                    {

                        

                        oIElement = (iTextSharp.text.IElement)objects[k];
                        if (objects[k].GetType().FullName == "iTextSharp.text.Paragraph")
                        {
                            oParagraph = new iTextSharp.text.Paragraph();
                            oParagraph.Alignment = ((iTextSharp.text.Paragraph)objects[k]).Alignment;
                            oParagraph.Font.Size = ffontSize;


                            cb = writer.DirectContent;
                            iTextSharp.text.pdf.ColumnText ct = new iTextSharp.text.pdf.ColumnText(cb);

                            for (int z = 0; z < oIElement.Chunks.Count; z++)
                            {
                                strContent = oIElement.Chunks[z].Content.ToString();

                                if (!bAplicaCierreTextoNotarial)
                                {
                                    oParagraph.Add(new iTextSharp.text.Chunk(strContent, oIElement.Chunks[z].Font));
                                }
                                else
                                {
                                    #region Aplica cierre Texto Notarial, es decir, "============"

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


                                        bool bEscribirTextoCierro = true;

                                        if (listPalabrasOmitirTextoNotarial != null)
                                        {

                                            foreach (string palabra in listPalabrasOmitirTextoNotarial)
                                            {
                                                if (strContent.Trim() != string.Empty && strContent.Trim() == palabra)
                                                {
                                                    bEscribirTextoCierro = false;
                                                    break;
                                                }

                                            }
                                            if (strContent.Trim() != string.Empty &&
                                                 strContent.Trim() != "PODER FUERA DE REGISTRO" &&
                                                 strContent.Trim() != "CONCLUSIÓN:")
                                            {

                                            }
                                        }

                                        if (bEscribirTextoCierro)
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

                                    #endregion
                                }

                            }

                            oParagraph.SetLeading(0.0f, fTextLeading);
                            document.Add(oParagraph);


                            if (fVerticalPositionActual < writer.GetVerticalPosition(false))
                            {
                                iPageNumber++;

                                #region Imagen

                                img = iTextSharp.text.Image.GetInstance(imgLogo);
                                img.SetAbsolutePosition(fImageMargin, document.PageSize.Height - 100);
                                img.ScaleAbsoluteHeight(83.2f);
                                img.ScaleAbsoluteWidth(80f);
                                document.Add(img);

                                #endregion

                                #region Consulado Imagen


                                bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);

                                cb.BeginText();

                                cb.SetFontAndSize(bfTimes, 7);

                                //string consuladoBlaBla = "CONSULADO GENERAL DEL PERÚ EN MIAMI";

                                texto = string.Empty;

                                pos = 0;
                                tamPalabra = 0;
                                ancho = 80f;

                                fontConsulado = iTextSharp.text.FontFactory.GetFont("Arial", 7);


                                posxAcumulado = tamPalabra;

                                foreach (string palabra in nombreConsulado.Split(' '))
                                {
                                    tamPalabra = new iTextSharp.text.Chunk(palabra.Trim(), fontConsulado).GetWidthPoint();

                                    if (posxAcumulado + tamPalabra > ancho)
                                    {



                                        cb.SetTextMatrix(fImageMargin + (ancho / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f), document.PageSize.Height - 117 + pos);
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
                                    cb.SetTextMatrix(fImageMargin + (ancho / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f), document.PageSize.Height - 117 + pos);
                                    cb.ShowText(texto.Trim());
                                }





                                cb.EndText();

                                #endregion

                                #region Texto Posicionado


                                cb.BeginText();


                                foreach (TextoPosicionadoITextSharp textoPosicionado in listTextoPosicionado)
                                {

                                    if (textoPosicionado.bRepetir)
                                    {
                                        cb.SetFontAndSize(textoPosicionado.BaseFont, textoPosicionado.FontSize);

                                        float tamanoPalabra = new iTextSharp.text.Chunk(textoPosicionado.Texto, textoPosicionado.FontFamily).GetWidthPoint();

                                        if (textoPosicionado.TextAligment == HorizontalAlign.NotSet)
                                        {
                                            cb.SetTextMatrix(textoPosicionado.FXPosition, document.PageSize.Height - textoPosicionado.FYPosition);
                                        }
                                        else if (textoPosicionado.TextAligment == HorizontalAlign.Left)
                                        {
                                            cb.SetTextMatrix(fMargenIzquierdaDoc, document.PageSize.Height - textoPosicionado.FYPosition);
                                        }
                                        else if (textoPosicionado.TextAligment == HorizontalAlign.Right)
                                        {
                                            float posicionDerecha = (document.PageSize.Width - fMargenDerechaDoc) - tamanoPalabra;
                                            cb.SetTextMatrix(posicionDerecha, document.PageSize.Height - textoPosicionado.FYPosition);
                                        }
                                        else if (textoPosicionado.TextAligment == HorizontalAlign.Center)
                                        {
                                            float posicionCentrada = ((document.PageSize.Width / 2) - (tamanoPalabra / 2));
                                            cb.SetTextMatrix(posicionCentrada, document.PageSize.Height - textoPosicionado.FYPosition);
                                        }
                                        cb.ShowText(textoPosicionado.Texto);



                                        if (textoPosicionado.FYPosition > fPosicion)
                                        {
                                            fPosicion = textoPosicionado.FYPosition;
                                            fTextSize = textoPosicionado.FontSize;
                                        }
                                    }
                                }

                                cb.EndText();


                                #endregion

                            }

                            fVerticalPositionActual = writer.GetVerticalPosition(false);
                            
                        }

                    }


                    #endregion

                    #region Firmas

                    if (listFirmas != null)
                    {
                        parrafo = new iTextSharp.text.Paragraph();
                        frase = new iTextSharp.text.Phrase();

                        foreach (DocumentoFirma docFirma in listFirmas)
                        {
                            parrafo = new iTextSharp.text.Paragraph();
                            frase = new iTextSharp.text.Phrase();

                            if (writer.GetVerticalPosition(false) >= (fMargenSuperiorDoc + 120))
                            {

                                frase.Add(new iTextSharp.text.Chunk("\n\n\n\n\n"));
                                parrafo.Add(frase);
                                document.Add(parrafo);
                            }
                            else
                            {

                                while (writer.GetVerticalPosition(false) < (fMargenSuperiorDoc + 120))
                                {
                                    document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n")));
                                }
                            }

                            parrafo = new iTextSharp.text.Paragraph();
                            frase = new iTextSharp.text.Phrase();
                            frase.Add(new iTextSharp.text.pdf.draw.LineSeparator(1F, 50.0F, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_LEFT, 1));
                            parrafo.Add(frase);
                            document.Add(parrafo);

                            if (docFirma.bAplicaHuellaDigital)
                            {
                                cb = writer.DirectContent;
                                cb.Rectangle(document.PageSize.Width - 270, writer.GetVerticalPosition(false) - 15, 65f, 80f);
                                cb.Stroke();
                            }

                            parrafo = new iTextSharp.text.Paragraph();
                            frase = new iTextSharp.text.Phrase();
                            frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNombreCompleto));
                            frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNroDocumentoCompleto));
                            parrafo.Add(frase);
                            document.Add(parrafo);

                        }
                    }

                    #endregion

                   

                    document.Close();

                    #region Impresion en Navegador



                    Byte[] FileBuffer = ms.ToArray();
                    if (FileBuffer != null)
                    {
                        System.Web.HttpContext.Current.Session["binaryData"] = FileBuffer;
                        string strUrl = "../Accesorios/VisorPDF.aspx";
                        string strScript = "window.open('" + strUrl + "', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";
                        Comun.EjecutarScript(page, strScript);
                    }




                    str.Close();
                    oStreamReader.Close();
                    oStreamReader.Dispose();



                    #endregion

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static Boolean _CrearDocumentoiTextSharpGeneral(string textoHtml, string nombreConsulado, string imgLogo, ref int iPageNumber, ref string Mensaje, List<DocumentoFirma> listFirmas = null, bool bAplicaCierreTextoNotarial = false, List<string> listPalabrasOmitirTextoNotarial = null, List<TextoPosicionadoITextSharp> listTextoPosicionado = null)
        {
            Boolean Resultado = false;
            try
            {
               

                #region Inicializando Variables

                iTextSharp.text.Paragraph parrafo = new iTextSharp.text.Paragraph();
                iTextSharp.text.Phrase frase = new iTextSharp.text.Phrase();

                float fTextLeading = 1.7f;
                float fImageMargin = 25f;
                float fMargenIzquierdaDoc = 80;
                float fMargenDerechaDoc = 80;
                float fMargenInferiorDoc = 140;
                float fMargenSuperiorDoc = (842 - (12 * fTextLeading * 25) - fMargenInferiorDoc);


                iTextSharp.text.IElement oIElement;
                iTextSharp.text.Paragraph oParagraph = null;

                List<iTextSharp.text.IElement> objects;
                string strContent = string.Empty;

                iTextSharp.text.FontFactory.RegisterDirectories();

                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, fMargenIzquierdaDoc, fMargenDerechaDoc, fMargenSuperiorDoc, fMargenInferiorDoc);




                iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);


                #endregion


                using (var ms = new MemoryStream())
                {



                    StreamWriter str = new StreamWriter(ms, Encoding.Default);
                    str.Write("                    " + textoHtml);
                    str.Flush();


                    ms.Position = 0;

                    StreamReader oStreamReader = new StreamReader(ms, System.Text.Encoding.Default);


                    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms);

                    document.Open();
                    document.NewPage();

                    objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(oStreamReader, styles);
                    float fAnchoAreaTexto = document.PageSize.Width - fMargenDerechaDoc - fMargenIzquierdaDoc;

                    float fPosicion = fMargenSuperiorDoc;
                    float fTextSize = 0;

                    #region Imagen

                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imgLogo);
                    img.SetAbsolutePosition(fImageMargin, document.PageSize.Height - 100);
                    img.ScaleAbsoluteHeight(83.2f);
                    img.ScaleAbsoluteWidth(80f);
                    document.Add(img);

                    #endregion

                    #region Consulado Imagen

                    iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                    iTextSharp.text.pdf.BaseFont bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);

                    cb.BeginText();

                    cb.SetFontAndSize(bfTimes, 7);

                    //string consuladoBlaBla = "CONSULADO GENERAL DEL PERÚ EN MIAMI";

                    string texto = string.Empty;

                    float pos = 0;
                    float tamPalabra = 0;
                    float ancho = 80f;

                    iTextSharp.text.Font fontConsulado = iTextSharp.text.FontFactory.GetFont("Arial", 7);


                    float posxAcumulado = tamPalabra;

                    foreach (string palabra in nombreConsulado.Split(' '))
                    {
                        tamPalabra = new iTextSharp.text.Chunk(palabra.Trim(), fontConsulado).GetWidthPoint();

                        if (posxAcumulado + tamPalabra > ancho)
                        {



                            cb.SetTextMatrix(fImageMargin + (ancho / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f), document.PageSize.Height - 117 + pos);
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
                        cb.SetTextMatrix(fImageMargin + (ancho / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f), document.PageSize.Height - 117 + pos);
                        cb.ShowText(texto.Trim());
                    }





                    cb.EndText();

                    #endregion

                    #region Texto Posicionado


                    cb.BeginText();


                    foreach (TextoPosicionadoITextSharp textoPosicionado in listTextoPosicionado)
                    {


                        cb.SetFontAndSize(textoPosicionado.BaseFont, textoPosicionado.FontSize);

                        float tamanoPalabra = new iTextSharp.text.Chunk(textoPosicionado.Texto, textoPosicionado.FontFamily).GetWidthPoint();

                        if (textoPosicionado.TextAligment == HorizontalAlign.NotSet)
                        {
                            cb.SetTextMatrix(textoPosicionado.FXPosition, document.PageSize.Height - textoPosicionado.FYPosition);
                        }
                        else if (textoPosicionado.TextAligment == HorizontalAlign.Left)
                        {
                            cb.SetTextMatrix(fMargenIzquierdaDoc, document.PageSize.Height - textoPosicionado.FYPosition);
                        }
                        else if (textoPosicionado.TextAligment == HorizontalAlign.Right)
                        {
                            float posicionDerecha = (document.PageSize.Width - fMargenDerechaDoc) - tamanoPalabra;
                            cb.SetTextMatrix(posicionDerecha, document.PageSize.Height - textoPosicionado.FYPosition);
                        }
                        else if (textoPosicionado.TextAligment == HorizontalAlign.Center)
                        {
                            float posicionCentrada = ((document.PageSize.Width / 2) - (tamanoPalabra / 2));
                            cb.SetTextMatrix(posicionCentrada, document.PageSize.Height - textoPosicionado.FYPosition);
                        }
                        cb.ShowText(textoPosicionado.Texto);



                        if (textoPosicionado.FYPosition > fPosicion)
                        {
                            fPosicion = textoPosicionado.FYPosition;
                            fTextSize = textoPosicionado.FontSize;
                        }
                    }

                    cb.EndText();


                    #endregion

                    #region Cuerpo Documento


                    float fVerticalPositionActual = writer.GetVerticalPosition(false);

                    iPageNumber = 1;

                    for (int k = 0; k < objects.Count; k++)
                    {



                        oIElement = (iTextSharp.text.IElement)objects[k];
                        if (objects[k].GetType().FullName == "iTextSharp.text.Paragraph")
                        {
                            oParagraph = new iTextSharp.text.Paragraph();
                            oParagraph.Alignment = ((iTextSharp.text.Paragraph)objects[k]).Alignment;



                            cb = writer.DirectContent;
                            iTextSharp.text.pdf.ColumnText ct = new iTextSharp.text.pdf.ColumnText(cb);

                            for (int z = 0; z < oIElement.Chunks.Count; z++)
                            {
                                strContent = oIElement.Chunks[z].Content.ToString();

                                if (!bAplicaCierreTextoNotarial)
                                {
                                    oParagraph.Add(new iTextSharp.text.Chunk(strContent, oIElement.Chunks[z].Font));
                                }
                                else
                                {
                                    #region Aplica cierre Texto Notarial, es decir, "============"

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


                                        bool bEscribirTextoCierro = true;

                                        if (listPalabrasOmitirTextoNotarial != null)
                                        {

                                            foreach (string palabra in listPalabrasOmitirTextoNotarial)
                                            {
                                                if (strContent.Trim() != string.Empty && strContent.Trim() == palabra)
                                                {
                                                    bEscribirTextoCierro = false;
                                                    break;
                                                }

                                            }
                                            if (strContent.Trim() != string.Empty &&
                                                 strContent.Trim() != "PODER FUERA DE REGISTRO" &&
                                                 strContent.Trim() != "CONCLUSIÓN:")
                                            {

                                            }
                                        }

                                        if (bEscribirTextoCierro)
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

                                    #endregion
                                }

                            }

                            oParagraph.SetLeading(0.0f, fTextLeading);
                            document.Add(oParagraph);


                            if (fVerticalPositionActual < writer.GetVerticalPosition(false))
                            {
                                iPageNumber++;

                            }

                            fVerticalPositionActual = writer.GetVerticalPosition(false);

                        }

                    }


                    #endregion

                    #region Firmas

                    if (listFirmas != null)
                    {
                        parrafo = new iTextSharp.text.Paragraph();
                        frase = new iTextSharp.text.Phrase();

                        foreach (DocumentoFirma docFirma in listFirmas)
                        {
                            parrafo = new iTextSharp.text.Paragraph();
                            frase = new iTextSharp.text.Phrase();

                            if (writer.GetVerticalPosition(false) >= (fMargenSuperiorDoc + 120))
                            {

                                frase.Add(new iTextSharp.text.Chunk("\n\n\n\n\n"));
                                parrafo.Add(frase);
                                document.Add(parrafo);
                            }
                            else
                            {

                                while (writer.GetVerticalPosition(false) < (fMargenSuperiorDoc + 120))
                                {
                                    document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n")));
                                }
                            }

                            parrafo = new iTextSharp.text.Paragraph();
                            frase = new iTextSharp.text.Phrase();
                            frase.Add(new iTextSharp.text.pdf.draw.LineSeparator(1F, 50.0F, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_LEFT, 1));
                            parrafo.Add(frase);
                            document.Add(parrafo);

                            if (docFirma.bAplicaHuellaDigital)
                            {
                                cb = writer.DirectContent;
                                cb.Rectangle(document.PageSize.Width - 270, writer.GetVerticalPosition(false) - 15, 65f, 80f);
                                cb.Stroke();
                            }

                            parrafo = new iTextSharp.text.Paragraph();
                            frase = new iTextSharp.text.Phrase();
                            frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNombreCompleto));
                            frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNroDocumentoCompleto));
                            parrafo.Add(frase);
                            document.Add(parrafo);

                        }
                    }

                    #endregion



                    document.Close();

                    #region Impresion en Navegador



                    Byte[] FileBuffer = ms.ToArray();
                    if (FileBuffer != null)
                    {
                        //Session["binaryData"] = FileBuffer;
                        Resultado = true;
                        Mensaje = "ok";
                    }




                    str.Close();
                    oStreamReader.Close();
                    oStreamReader.Dispose();



                    #endregion

                }

                return Resultado;

            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                return false;
            }

        }

        #endregion

        internal static string ObtenerMensajeTotalRegistros(int intPaginaActual, int intTotalRegistros, int intPaginaTamanio)
        {
            string strMensaje = string.Empty;
            strMensaje = Constantes.CONST_MENSAJE_BUSQUEDA_EXITO;

            int intInicio = (intPaginaActual - 1) * intPaginaTamanio + 1;
            int intFin = (intPaginaActual - 1) * intPaginaTamanio + intPaginaTamanio;
            if (intFin > intTotalRegistros) intFin = intTotalRegistros;

            strMensaje += intInicio + " - " + intFin + " de " + intTotalRegistros;

            return strMensaje;
        }

        public static string ObtenerParteTexto(string strCadena, char cSimboloNoConsiderar)
        {
            string strCadenaNueva = strCadena;

            if (strCadenaNueva.Contains(cSimboloNoConsiderar.ToString()))
            {
                int intIndice = strCadena.IndexOf(cSimboloNoConsiderar);
                if (intIndice > 0)
                    strCadenaNueva = strCadena.Substring(0, intIndice-1);
            }
            return strCadenaNueva;
        }

        #region Fecha Descripción

        public static string ObtenerMesNombre(int intMesNumero, bool bolMayusculas = false)
        {
            string vmes = string.Empty;
            switch (intMesNumero)
            {
                case 1:
                    vmes = "Enero";
                    break;

                case 2:
                    vmes = "Febrero";
                    break;

                case 3:
                    vmes = "Marzo";
                    break;

                case 4:
                    vmes = "Abril";
                    break;

                case 5:
                    vmes = "Mayo";
                    break;

                case 6:
                    vmes = "Junio";
                    break;

                case 7:
                    vmes = "Julio";
                    break;

                case 8:
                    vmes = "Agosto";
                    break;

                case 9:
                    vmes = "Septiembre";
                    break;

                case 10:
                    vmes = "Octubre";
                    break;

                case 11:
                    vmes = "Noviembre";
                    break;

                case 12:
                    vmes = "Diciembre";
                    break;
            }

            if (bolMayusculas)
                vmes = vmes.ToUpper();

            return vmes;
        }

        #endregion

        #region Validación Null
        public static int ToNullInt32(object Objeto)
        {
            int iNumero = 0;
            try
            {
                iNumero = Convert.ToInt32(Objeto);
            }
            catch
            {
                iNumero = 0;
            }
            return iNumero;
        }

        public static Int16 ToNullInt16(object s_Valor)
        {
            Int16 i_Resultado = 0;
            try
            {
                i_Resultado = Convert.ToInt16(s_Valor);
            }
            catch
            {
                i_Resultado = 0;
            }
            return i_Resultado;
        }

        public static Int64 ToNullInt64(object s_Valor)
        {
            Int64 i_Resultado = 0;
            try
            {
                i_Resultado = Convert.ToInt64(s_Valor);
            }
            catch
            {
                i_Resultado = 0;
            }
            return i_Resultado;
        }

        public static string toNullString(object obj)
        {
            string strValor = "";
            try
            {
                strValor = Convert.ToString(obj);
            }
            catch
            {
                strValor = "";
            }
            return strValor;
        }
        #endregion

        public static string AplicarInicialMayuscula(string palabra)
        {
            if (palabra.Trim() != string.Empty)
            {
                palabra = palabra.ToLower();
                return palabra[0].ToString().ToUpper() + palabra.Substring(1, palabra.Length - 1); ;
            }

            return string.Empty;
        }


        public static string DiferenciaFechas(DateTime newdt, DateTime olddt, string strTextoInvalido,bool numerico = false)
        {
            int intanios;
            int intmeses;
            int intdias;
            string strDiferencia = "";

            intanios = (newdt.Year - olddt.Year);
            intmeses = (newdt.Month - olddt.Month);
            intdias = (newdt.Day - olddt.Day);

            //if (newdt.Month < olddt.Month || (newdt.Month == olddt.Month && newdt.Day < olddt.Day))
            //{ intanios--; }

            if (newdt.Month < olddt.Month || (newdt.Month == olddt.Month && newdt.Day < olddt.Day))
            {
                intanios -= 1;
                intmeses += 12;
            }
            if (intdias < 0)
            {
                intmeses -= 1;
                //intdias += DateTime.DaysInMonth(newdt.Year, newdt.Month);
                intdias = Math.Abs(intdias);
            }

            if (intanios == 0)
            {
                if (numerico)
                {
                    strDiferencia = "0";
                }
                else {
                    if (intmeses > 0)
                    {
                        strDiferencia = strDiferencia + intmeses.ToString() + "m";
                    }
                    if (intdias > 0)
                    {
                        strDiferencia = strDiferencia + intdias.ToString() + "d";
                    }
                }
                
            }
            else
            {
                if (intanios < 0)
                {
                    return strTextoInvalido;
                }
                else
                {
                    strDiferencia = strDiferencia + intanios.ToString();
                }
            }
            return strDiferencia;
        }


        //------------------------------------------------------------
        //Fecha: 27/12/2016
        //Objetivo: Convierte formato de fecha: dd/mm/yyyy a yyyymmdd
        //Autor: Miguel Márquez Beltrán
        //------------------------------------------------------------
        public static string syyyymmdd(string sddmmyyyy)
        {
            if (sddmmyyyy.Length == 10)
            { return sddmmyyyy.Substring(6) + sddmmyyyy.Substring(3, 2) + sddmmyyyy.Substring(0, 2); }
            else
            {
                return "";
            }
        }
        //--------------------------------------------------
        //Fecha: 15/03/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Consultar la tabla PS_SISTEMA.SI_Pais
        //--------------------------------------------------
        public static DataTable ConsultarPaises(int intPaisId = 0, string strEstado = "A", int intPageSize = 1000, int intPageNumber = 1, string strContar = "N")
        {
            int ITotalPages = 0;
            int ITotalRecords = 0;
            short sContinenteId = 0;
            string strNombrePais = "";

            DataTable dtPaises = new DataTable();

            SGAC.Configuracion.Sistema.BL.PaisConsultasBL objPaisBL = new SGAC.Configuracion.Sistema.BL.PaisConsultasBL();

            dtPaises = objPaisBL.Consultar_Pais(intPaisId, strEstado, intPageNumber.ToString(), intPageSize, strContar, ref ITotalPages, ref ITotalRecords, sContinenteId, strNombrePais);

            return dtPaises;
        }

        //Jonatan Silva, asignar gentilicio sin ir a la base de datos 12/01/2018
        public static string AsignarGentilicioPorCodigoPais(HttpSessionState Session, string strUbi01, string strUbi02, string strGeneroId)
        {
            string strGentilicio = "";

            string strCodPais = strUbi01 + strUbi02;


            if (strUbi01.Length > 0 && strUbi02.Length > 0)
            {
                DataTable dtPaises = new DataTable();
                dtPaises = Comun.ConsultarPaises();
                //dtPaises = (DataTable)Session[Constantes.CONST_SESION_TABLA_PAISES];

                for (int i = 0; i < dtPaises.Rows.Count; i++)
                {
                    string strCodigoPaisDT = dtPaises.Rows[i]["PAIS_CUBIGEO"].ToString().Substring(0, 4);

                    if (strCodigoPaisDT.Equals(strCodPais))
                    {
                        strGentilicio = dtPaises.Rows[i]["PAIS_VGENTILICIO_MAS"].ToString().ToUpper();

                        if (strGeneroId.Length > 0)
                        {
                            if (strGeneroId == ((int)Enumerador.enmGenero.FEMENINO).ToString())
                            {
                                strGentilicio = dtPaises.Rows[i]["PAIS_VGENTILICIO_FEM"].ToString().ToUpper();
                            }
                        }
                        break;
                    }
                }
            }
            return strGentilicio;
        }

        //-------------------------------------------
        //Fecha: 15/03/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Consultar el gentilicio
        //-------------------------------------------
        public static string AsignarGentilicio(HttpSessionState Session, string strUbi01, string strUbi02, string strGeneroId)
        {
            string strGentilicio = "";

            if (strUbi01.Length > 0 && strUbi02.Length > 0)
            {
                SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objUbigeoBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
                DataTable dt = objUbigeoBL.Consultar(strUbi01, strUbi02, null);

                if (dt.Rows.Count > 0)
                {
                    string strSiglasPais = dt.Rows[0]["ubge_vSiglaPais"].ToString();

                    DataTable dtPaises = new DataTable();
                    dtPaises = Comun.ConsultarPaises();
                    //dtPaises = (DataTable)Session[Constantes.CONST_SESION_TABLA_PAISES];

                    for (int i = 0; i < dtPaises.Rows.Count; i++)
                    {
                        if (dtPaises.Rows[i]["PAIS_CLETRA_ISO_3166"].ToString().Equals(strSiglasPais))
                        {
                            strGentilicio = dtPaises.Rows[i]["PAIS_VGENTILICIO_MAS"].ToString().ToUpper();

                            if (strGeneroId.Length > 0)
                            {
                                if (strGeneroId == ((int)Enumerador.enmGenero.FEMENINO).ToString())
                                {
                                    strGentilicio = dtPaises.Rows[i]["PAIS_VGENTILICIO_FEM"].ToString().ToUpper();
                                }
                            }

                            break;
                        }
                    }
                }
            }
            return strGentilicio;
        }
        //-------------------------------------------
        //Fecha: 15/03/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Obtener el país de origen
        //-------------------------------------------
        public static string AsignarPaisOrigen(HttpSessionState Session, DropDownList CmbDptoContNac, DropDownList CmbProvPaisNac)
        {
            string strPaisOrigenId = "0";
            string strUbi01 = "";
            string strUbi02 = "";
            if (CmbDptoContNac.SelectedIndex > 0 && CmbProvPaisNac.SelectedIndex > 0)
            {
                strUbi01 = CmbDptoContNac.SelectedValue;
                strUbi02 = CmbProvPaisNac.SelectedValue;

                SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objUbigeoBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
                DataTable dt = objUbigeoBL.Consultar(strUbi01, strUbi02, null);

                string strSiglasPais = dt.Rows[0]["ubge_vSiglaPais"].ToString();

                DataTable dtPaises = new DataTable();
                dtPaises = Comun.ConsultarPaises();
                //dtPaises = (DataTable)Session[Constantes.CONST_SESION_TABLA_PAISES];

                for (int i = 0; i < dtPaises.Rows.Count; i++)
                {
                    if (dtPaises.Rows[i]["PAIS_CLETRA_ISO_3166"].ToString().Equals(strSiglasPais))
                    {
                        strPaisOrigenId = dtPaises.Rows[i]["PAIS_SPAISID"].ToString();

                        break;
                    }
                }
            }

            return strPaisOrigenId;
        }

        //--------------------------------------------------------------
        //Fecha: 03/04/2020
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Crear AsignarGentilicio con DataTable de Paises.
        //--------------------------------------------------------------
        public static string AsignarGentilicio(DataTable dtPaises, DropDownList ddlPaisOrigen, DropDownList ddl_pers_sGeneroId)
        {
            try
            {
                string strGentilicio = "";

                if (ddlPaisOrigen.SelectedIndex > 0)
                {
                    
                    string strPaisOrigenId = ddlPaisOrigen.SelectedValue;

                    for (int i = 0; i < dtPaises.Rows.Count; i++)
                    {
                        if (dtPaises.Rows[i]["PAIS_SPAISID"].ToString().Equals(strPaisOrigenId))
                        {
                            strGentilicio = dtPaises.Rows[i]["PAIS_VGENTILICIO_MAS"].ToString();

                            if (ddl_pers_sGeneroId.SelectedIndex > 0)
                            {
                                if (ddl_pers_sGeneroId.SelectedValue.ToString() == ((int)Enumerador.enmGenero.FEMENINO).ToString())
                                {
                                    strGentilicio = dtPaises.Rows[i]["PAIS_VGENTILICIO_FEM"].ToString();
                                }
                            }
                            break;
                        }
                    }
                }

                return strGentilicio;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
  
        //-------------------------------------------
        //Fecha: 15/03/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Obtener la Nacionalidad
        //-------------------------------------------
        public static void AsignarNacionalidad(HttpSessionState Session, DropDownList ddl_pers_sNacionalidadId, DropDownList ddlPaisOrigen)
        {
            try
            {
                if (ddlPaisOrigen.SelectedIndex > 0)
                {
                    string strPaisPeruId = ConfigurationManager.AppSettings["Pais_PeruId"].ToString();
                    if (ddlPaisOrigen.SelectedValue == strPaisPeruId)
                    {
                        ddl_pers_sNacionalidadId.SelectedValue = Convert.ToString((int)Enumerador.enmNacionalidad.PERUANA);
                    }
                    else
                    {
                        ddl_pers_sNacionalidadId.SelectedValue = Convert.ToString((int)Enumerador.enmNacionalidad.EXTRANJERA);
                    }
                }
                else
                {
                    ddl_pers_sNacionalidadId.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        //--------------------------------------------------------------
        //Fecha: 31/03/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Consultar La Tabla Ubigeo y asignar a un objeto List
        //--------------------------------------------------------------

        public static List<listaUbigeo> ObtenerUbigeo(string strUbi01, string strUbi02, string strUbi03,
           string strValor, string strDescripcion, bool bolAgregarItemAdicional, string pstrItemText, string pstrItemValue)
        {
            try
            {
                List<listaUbigeo> ArrLista = new List<listaUbigeo>();
                SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objUbigeoBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();

                ArrLista = objUbigeoBL.ObtenerUbigeo(strUbi01, strUbi02, strUbi03, strValor, strDescripcion, bolAgregarItemAdicional, pstrItemText, pstrItemValue);
                return ArrLista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        //------------------------------------------------

        public static List<listaUbigeo> CargarUbigeoDptoCont(bool bolAgregarItemAdicional = false, string pstrItemText = "--SELECCIONAR--", string pstrItemValue = "0")
        {
            try
            {
                List<listaUbigeo> ArrLista = new List<listaUbigeo>();
                string strDescripcion = string.Empty;
                string strValor = string.Empty;

                strDescripcion = "ubge_vDepartamento";
                strValor = "ubge_cUbi01";

                ArrLista = ObtenerUbigeo("", "", "", strValor, strDescripcion, bolAgregarItemAdicional, pstrItemText, pstrItemValue);

                return ArrLista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public static void limpiaCombo(string pstrItemText, params object[] comboParams)
        {
            DropDownList combo;
            for (int i = 0; i <= comboParams.GetUpperBound(0); i++)
            {
                combo = new DropDownList();
                combo = (DropDownList)comboParams[i];
                combo.Items.Clear();
                combo.Items.Insert(0, new ListItem(pstrItemText, "0"));
                combo.SelectedIndex = 0;
            }
        }


        public static List<listaUbigeo> LlenarListaUbigeoDptoCont()
        {
            try
            {
                SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objUbigeoBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
                string strDescripcion = "ubge_vDepartamento",
                       strValor = "ubge_cUbi01";
                string pstrItemText = "--SELECCIONAR--";
                string pstrItemValue = "0";
                List<listaUbigeo> listaUbigeo = new List<listaUbigeo>();
                listaUbigeo = objUbigeoBL.ObtenerUbigeo("", "", "", strValor, strDescripcion, true, pstrItemText, pstrItemValue);
                return listaUbigeo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LlenarUbigeoDptoCont_ListaItems(HttpSessionState Session, params object[] comboParams)
        {
            try
            {
                DropDownList combo;

                List<listaUbigeo> listaUbigeo = Comun.CargarUbigeoDptoCont(true, "--SELECCIONAR--", "0");
                //            List<listaUbigeo> listaUbigeo = (List<listaUbigeo>)Session[Constantes.CONST_SESION_LISTA_DPTO];

                for (int i = 0; i <= comboParams.GetUpperBound(0); i++)
                {
                    combo = new DropDownList();
                    combo = (DropDownList)comboParams[i];
                    combo.Items.Clear();

                    foreach (listaUbigeo item in listaUbigeo)
                    {
                        combo.Items.Add(new ListItem(item.PartName, item.PartId));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CargarUbigeo(Enumerador.enmTipoUbigeo enmUbigeo, string strDptoCont = "", string strProvPais = "", string strDistCiud = "",
          bool bolAgregarItemAdicional = false, string pstrItemText = "--SELECCIONAR--", string pstrItemValue = "0",
          Enumerador.enmNacionalidad enmNacionalidad = Enumerador.enmNacionalidad.NINGUNA, DropDownList ddl_combo = null)
        {
            try
            {
                string strDescripcion = string.Empty;
                string strValor = string.Empty;

                switch (enmUbigeo)
                {
                    case Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT:
                        strDescripcion = "ubge_vDepartamento";
                        strValor = "ubge_cUbi01";
                        break;
                    case Enumerador.enmTipoUbigeo.PROVINCIA_PAIS:
                        strDescripcion = "ubge_vProvincia";
                        strValor = "ubge_cUbi02";

                        break;

                    case Enumerador.enmTipoUbigeo.DISTRITO_CIUD:
                        strDescripcion = "ubge_vDistrito";
                        strValor = "ubge_cUbi03";

                        break;
                }
                SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objUbigeoBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();

                List<listaUbigeo> listaUbigeo = new List<listaUbigeo>();


                listaUbigeo = objUbigeoBL.ObtenerUbigeo(strDptoCont, strProvPais, strDistCiud, strValor, strDescripcion, bolAgregarItemAdicional, pstrItemText, pstrItemValue);

                ddl_combo.Items.Clear();
                ddl_combo.ClearSelection();

                foreach (listaUbigeo item in listaUbigeo)
                {
                    ddl_combo.Items.Add(new ListItem(item.PartName, item.PartId));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CrearDocumentoiTextSharpTamanoLetra(Page page, string textoHtml, string strConsulado, string imgLogo, List<DocumentoFirma> listFirmas = null, bool bAplicaCierreTextoNotarial = false, List<string> listPalabrasOmitirTextoNotarial = null, string nombreDocumento = "ItextSharpDocument", Boolean bImprimir = true, double _tamanoLetra = 10)
        {
            try
            {

                #region Inicializando Variables


                float _fCuerpoFontSize = 10;

                float fMargenIzquierdaDoc = 40;
                float fMargenDerechaDoc = 40;
                float fMargenSuperiorDoc = 80;
                float fMargenInferiorDoc = 50f;
                float fImageMargin = 50f;
                float fImageWidth = 53.77f;

                float fMargenSuperiorDocFirma = 20;

                iTextSharp.text.IElement oIElement;
                iTextSharp.text.Paragraph oParagraph = null;

                List<iTextSharp.text.IElement> objects;
                string strContent = string.Empty;

                iTextSharp.text.FontFactory.RegisterDirectories();

                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, fMargenIzquierdaDoc, fMargenDerechaDoc, fMargenSuperiorDoc, fMargenInferiorDoc);

                iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);

                //String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
                //string strRutaPDF = uploadPath + @"\" + nombreDocumento + "_" + DateTime.Now.Ticks.ToString() + ".pdf";
                //string strRutaHtml = uploadPath + @"\" + nombreDocumento + "_" + DateTime.Now.Ticks.ToString() + ".html";

                #endregion
                //iTextSharp.text.Font _fontGeneral = new iTextSharp.text.Font(iTextSharp.text.pdf.BaseFont.TIMES_ROMAN, _fCuerpoFontSize);
                //iTextSharp.text.Font _fontGeneral = new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 10);



                using (var ms = new MemoryStream())
                {
                    StreamWriter str = new StreamWriter(ms, Encoding.Default);
                    str.Write("                    " + textoHtml);
                    str.Flush();

                    ms.Position = 0;

                    StreamReader oStreamReader = new StreamReader(ms, System.Text.Encoding.Default);

                    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms);

                    document.Open();
                    document.NewPage();

                    objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(oStreamReader, styles);
                    float fAnchoAreaTexto = document.PageSize.Width - fMargenDerechaDoc - fMargenIzquierdaDoc;

                    #region Imagen

                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imgLogo);
                    img.SetAbsolutePosition(fImageMargin, document.PageSize.Height - 85);
                    img.ScaleAbsoluteHeight(75f);
                    img.ScaleAbsoluteWidth(fImageWidth);
                    document.Add(img);

                    #endregion

                    #region Consulado Imagen

                    iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                    iTextSharp.text.pdf.BaseFont bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);

                    cb.BeginText();

                    cb.SetFontAndSize(bfTimes, 6);

                    string texto = string.Empty;

                    float pos = 0;
                    float tamPalabra = 0;
                    float ancho = 80f;
                    iTextSharp.text.Font fontConsulado = iTextSharp.text.FontFactory.GetFont("Arial", 6);

                    if (strConsulado.ToUpper().Contains("CONSULADO GENERAL DEL PERÚ"))
                    {
                        ancho = new iTextSharp.text.Chunk("CONSULADO GENERAL DEL PERÚ", fontConsulado).GetWidthPoint() + 5;
                        strConsulado = strConsulado.ToUpper().Replace("PERÚ EN", "PERÚ");
                    }

                    int iPosicionComa = strConsulado.IndexOf(",");

                    if (iPosicionComa >= 0)
                        strConsulado = strConsulado.Substring(0, iPosicionComa);


                    float posxAcumulado = tamPalabra;

                    foreach (string palabra in strConsulado.Split(' '))
                    {
                        tamPalabra = new iTextSharp.text.Chunk(palabra.Trim(), fontConsulado).GetWidthPoint();

                        if (posxAcumulado + tamPalabra > ancho)
                        {

                            cb.SetTextMatrix(fImageMargin + (fImageWidth / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f), document.PageSize.Height - 95 + pos);
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
                        cb.SetTextMatrix(fImageMargin + (fImageWidth / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f), document.PageSize.Height - 95 + pos);
                        cb.ShowText(texto.Trim());
                    }


                    cb.EndText();

                    #endregion

                    #region Cuerpo Documento

                    #region Font Settings

                    string vFontFamilyExtraProtocolar = System.Configuration.ConfigurationManager.AppSettings["vFontFamilyExtraProtocolar"].ToString();
                    float fFontSizeExtraProtocolar = float.Parse(_tamanoLetra.ToString());


                    if (fFontSizeExtraProtocolar <= 0)
                    {
                        fFontSizeExtraProtocolar = 12;
                    }

                    string fontPath = HttpContext.Current.Server.MapPath("~/Fonts/" + vFontFamilyExtraProtocolar + ".ttf");


                    iTextSharp.text.pdf.BaseFont baseFont;
                    iTextSharp.text.Font fontGeneral = new iTextSharp.text.Font();

                    if (File.Exists(fontPath))
                    {
                        baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(fontPath, iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);
                        fontGeneral = new iTextSharp.text.Font(baseFont, fFontSizeExtraProtocolar);
                    }
                    else
                    {

                        fontGeneral = iTextSharp.text.FontFactory.GetFont("Arial", fFontSizeExtraProtocolar);
                    }


                    #endregion


                    for (int k = 0; k < objects.Count; k++)
                    {
                        oIElement = (iTextSharp.text.IElement)objects[k];
                        if (objects[k].GetType().FullName == "iTextSharp.text.Paragraph")
                        {
                            oParagraph = new iTextSharp.text.Paragraph();
                            oParagraph.Alignment = ((iTextSharp.text.Paragraph)objects[k]).Alignment;

                            cb = writer.DirectContent;
                            iTextSharp.text.pdf.ColumnText ct = new iTextSharp.text.pdf.ColumnText(cb);

                            for (int z = 0; z < oIElement.Chunks.Count; z++)
                            {
                                strContent = oIElement.Chunks[z].Content.ToString().ToUpper();

                                if (!bAplicaCierreTextoNotarial)
                                {
                                    if (strContent.ToUpper() == "PODER FUERA DE REGISTRO" ||
                                            strContent.ToUpper() == "CERTIFICADO DE SUPERVIVENCIA" ||
                                            //strContent.ToUpper() == "AUTORIZACIÓN DE VIAJE DE MENOR AL EXTERIOR")
                                            strContent.ToUpper() == "AUTORIZACIÓN DE VIAJE DE MENOR")
                                    {
                                        oParagraph.Add(new iTextSharp.text.Chunk(strContent, oIElement.Chunks[z].Font));
                                    }
                                    else
                                    {
                                        oParagraph.Add(new iTextSharp.text.Chunk(strContent, fontGeneral));
                                    }
                                }
                                else
                                {
                                    #region Aplica cierre Texto Notarial, es decir, "============"

                                    if (strContent != "\n")
                                    {
                                        strContent = strContent.Trim();
                                        if (strContent.ToUpper() == "PODER FUERA DE REGISTRO" ||
                                            strContent.ToUpper() == "CERTIFICADO DE SUPERVIVENCIA" ||
                                            //strContent.ToUpper() == "AUTORIZACIÓN DE VIAJE DE MENOR AL EXTERIOR")
                                            strContent.ToUpper() == "AUTORIZACIÓN DE VIAJE DE MENOR")
                                        {
                                            oParagraph.Add(new iTextSharp.text.Chunk(strContent, oIElement.Chunks[z].Font));
                                        }
                                        else
                                        {
                                            oParagraph.Add(new iTextSharp.text.Chunk(strContent, fontGeneral));
                                        }
                                    }
                                    else
                                    {
                                        oParagraph.Add(new iTextSharp.text.Chunk(strContent, fontGeneral));
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
                                            listFonts.Add(fontGeneral);

                                        }


                                        bool bEscribirTextoCierro = true;

                                        if (listPalabrasOmitirTextoNotarial != null)
                                        {

                                            foreach (string palabra in listPalabrasOmitirTextoNotarial)
                                            {
                                                if (strContent.Trim() != string.Empty && strContent.Trim() == palabra)
                                                {
                                                    bEscribirTextoCierro = false;
                                                    break;
                                                }

                                            }
                                            if (strContent.Trim() != string.Empty &&
                                                 strContent.Trim() != "PODER FUERA DE REGISTRO" &&
                                                 strContent.Trim() != "CONCLUSIÓN:")
                                            {

                                            }
                                        }

                                        if (bEscribirTextoCierro)
                                        {
                                            textoNotarialCierre = Comun.ObtenerTextoNotarialCierre(listTextos, fAnchoAreaTexto, listFonts);
                                        }


                                        if (textoNotarialCierre != string.Empty)
                                        {
                                            iTextSharp.text.Font font = new iTextSharp.text.Font(fontGeneral);

                                            font.SetStyle(0);
                                            oParagraph.Add(new iTextSharp.text.Chunk(textoNotarialCierre, font));
                                        }

                                    }
                                    else
                                    {
                                        oParagraph.Add(new iTextSharp.text.Chunk(" ", fontGeneral));
                                    }

                                    #endregion
                                }

                            }

                            oParagraph.SetLeading(0.0f, 0.9f);
                            document.Add(oParagraph);
                        }

                    }


                    #endregion

                    #region Firmas

                    if (listFirmas != null)
                    {
                        iTextSharp.text.Paragraph parrafo = new iTextSharp.text.Paragraph();
                        iTextSharp.text.Phrase frase = new iTextSharp.text.Phrase();
                    
                        int cantidad = 0;
                        bool Espar = false;
                        foreach (DocumentoFirma docFirma in listFirmas)
                        {
                            cantidad = cantidad + 1;
                            if ((cantidad % 2) == 0)
                            {
                                Espar = true;
                            }
                            else { Espar = false; }
                            parrafo = new iTextSharp.text.Paragraph();
                        }

                        //modificado
                        if (cantidad == 1)
                        {
                            PintarUnaFirmaIzquierda(cantidad, fMargenSuperiorDocFirma, parrafo, frase, document, writer, listFirmas, fontGeneral, cb, 0, fFontSizeExtraProtocolar);
                        }
                        //FIN
                        if (cantidad == 2)
                        {

                            PintarDosFirmasIzquierdaDerecha(cantidad, fMargenSuperiorDocFirma, parrafo, frase, document, writer, listFirmas, fontGeneral, fFontSizeExtraProtocolar, cb, 0, 1);
                        }


                        if (cantidad > 2)
                        {
                            int mitad = cantidad / 2;
                            if (Espar)
                            {
                                for (int a = 0; a <= mitad; a = a + 2)
                                {
                                    PintarDosFirmasIzquierdaDerecha(cantidad, fMargenSuperiorDocFirma, parrafo, frase, document, writer, listFirmas, fontGeneral, fFontSizeExtraProtocolar, cb, a, a + 1);
                                }
                            }
                            else {
                                for (int a = 0; a < mitad; a = a + 1)
                                {
                                    PintarDosFirmasIzquierdaDerecha(cantidad, fMargenSuperiorDocFirma, parrafo, frase, document, writer, listFirmas, fontGeneral, fFontSizeExtraProtocolar, cb, a, a + 1);
                                    PintarUnaFirmaIzquierda(cantidad, fMargenSuperiorDocFirma, parrafo, frase, document, writer, listFirmas, fontGeneral, cb, a + 2, fFontSizeExtraProtocolar);
                                }
                            }
                            
                        
                        }

                        
                    }

                    #endregion

                    document.Close();

                    #region Impresion en Navegador



                    Byte[] FileBuffer = ms.ToArray();
                    if (FileBuffer != null)
                    {
                        System.Web.HttpContext.Current.Session["binaryData"] = FileBuffer;

                        if (bImprimir)
                        {
                            string strUrl = "../Accesorios/VisorPDF.aspx";
                            string strScript = "window.open('" + strUrl + "', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";
                            Comun.EjecutarScript(page, strScript);
                        }
                    }


                    str.Close();
                    oStreamReader.Close();
                    oStreamReader.Dispose();

                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void PintarUnaFirmaIzquierda(int cantidad, float fMargenSuperiorDocFirma,
                                            iTextSharp.text.Paragraph parrafo, iTextSharp.text.Phrase frase,
                                            iTextSharp.text.Document document, iTextSharp.text.pdf.PdfWriter writer,
                                            List<DocumentoFirma> listFirmas, iTextSharp.text.Font fontGeneral,
                                            iTextSharp.text.pdf.PdfContentByte cb,
                                            int indice, float fFontSizeExtraProtocolar)
        {
            
           
            parrafo.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
          
            frase = new iTextSharp.text.Phrase();

            if (writer.GetVerticalPosition(false) >= (fMargenSuperiorDocFirma + 120))
            {
                frase.Add(new iTextSharp.text.Chunk("\n\n\n\n\n"));
                parrafo.Add(frase);
                document.Add(parrafo);
            }
            else
            {
                while (writer.GetVerticalPosition(false) < (fMargenSuperiorDocFirma + 120))
                {
                    document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n")));
                }
            }

            //------------------------------------------------------------------
            //Fecha: 18/10/2017
            //Autor: Miguel Márquez Beltrán
            // Objetivo: Fragmento de código solo para Poder Fuera de Registro
            //------------------------------------------------------------------

            parrafo = new iTextSharp.text.Paragraph();
            frase = new iTextSharp.text.Phrase();
            float fMargenIzquierdaDoc = 80;

            if (listFirmas[indice].bIncapacitado && listFirmas[indice].bImprimirFirma == false)
            {
                //Esta incapacitado y no puede firmar ni poner su huella
                //----------------------------------
                // Testear
                //----------------------------------
                iTextSharp.text.pdf.BaseFont bfHelvetica = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA, iTextSharp.text.pdf.BaseFont.CP1252, false);
                cb = writer.DirectContent;
                
                #region Testear la Firma
                //-------------------------------------
                //Fecha: 20/09/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Testear la firma
                //-------------------------------------
                cb.SetFontAndSize(bfHelvetica, 7);
                cb.SetLineDash(7f, 2f, 0f);

                float ValorX = 40;

                cb.MoveTo(ValorX, writer.GetVerticalPosition(false) + 5);
                cb.LineTo(ValorX + 160, writer.GetVerticalPosition(false) + 5);
                cb.Stroke();

                cb.MoveTo(ValorX, writer.GetVerticalPosition(false) + 8);
                cb.LineTo(ValorX + 160, writer.GetVerticalPosition(false) + 8);
                cb.Stroke();
                #endregion

                #region Testear Huella Dactilar
                if (listFirmas[indice].bAplicaHuellaDigital == false)
                {
                    //--------------------------------------
                    //Fecha: 20/09/2017
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Testear la huella dactilar
                    //--------------------------------------
                    cb.MoveTo(fMargenIzquierdaDoc + 135, writer.GetVerticalPosition(false) + 5);
                    cb.LineTo(fMargenIzquierdaDoc + 185, writer.GetVerticalPosition(false) + 5);
                    cb.Stroke();

                    cb.MoveTo(fMargenIzquierdaDoc + 135, writer.GetVerticalPosition(false) + 8);
                    cb.LineTo(fMargenIzquierdaDoc + 185, writer.GetVerticalPosition(false) + 8);
                    cb.Stroke();
                }
                #endregion

                parrafo.Add(frase);
                document.Add(parrafo);

                //----------------------------------
                frase = new iTextSharp.text.Phrase();
                parrafo = new iTextSharp.text.Paragraph();
            }
            //---------------------------------------------
            //Imprimir la linea para la firma
            //---------------------------------------------
            frase.Add(new iTextSharp.text.pdf.draw.LineSeparator(0.5F, 30.0F, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_LEFT, 1));

            parrafo.Add(frase);
            document.Add(parrafo);

            //---------------------------------------------
            //Imprimir el Cuadro de la huella digital
            //---------------------------------------------
            #region Imprimir el Cuadro de la huella digital
            cb = writer.DirectContent;
            cb.SetLineDash(1);
            cb.SetTextMatrix(100, 400);
            cb.Rectangle(document.PageSize.Width - 380, writer.GetVerticalPosition(false) - 15, 50f, 60f);            

            cb.Stroke();
            parrafo.Add(frase);
            document.Add(parrafo);
            #endregion
            //---------------------------------------------
            //Imprimir el Nombre completo
            //---------------------------------------------
            #region Imprimir el Nombre Completo

            cb.BeginText();

            iTextSharp.text.pdf.BaseFont bf = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA, iTextSharp.text.pdf.BaseFont.CP1252, false);
            int tamanoX;
            int tamanoY;

            tamanoX = 555;            
            tamanoY = 25;
                        
            cb.SetFontAndSize(bf, fFontSizeExtraProtocolar);
            cb.SetTextMatrix(document.PageSize.Width - tamanoX, writer.GetVerticalPosition(false) - tamanoY);
            cb.ShowText("\n" + listFirmas[indice].vNombreCompleto);
            tamanoY = 35;
            cb.SetTextMatrix(document.PageSize.Width - tamanoX, writer.GetVerticalPosition(false) - tamanoY);
            cb.ShowText("\n" + listFirmas[indice].vNroDocumentoCompleto);
            cb.EndText();

            #endregion
            //------------------------------------------------

        
        }

        private static void PintarDosFirmasIzquierdaDerecha(int cantidad, float fMargenSuperiorDocFirma,
                                           iTextSharp.text.Paragraph parrafo, iTextSharp.text.Phrase frase,
                                           iTextSharp.text.Document document, iTextSharp.text.pdf.PdfWriter writer,
                                           List<DocumentoFirma> listFirmas, iTextSharp.text.Font fontGeneral,
                                           float fFontSizeExtraProtocolar,
                                           iTextSharp.text.pdf.PdfContentByte cb,
                                           int indiceIzquierdo, int indiceDerecho)
        {
                     
            parrafo.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
            
            frase = new iTextSharp.text.Phrase();


            if (writer.GetVerticalPosition(false) >= (fMargenSuperiorDocFirma + 120))
            {
                frase.Add(new iTextSharp.text.Chunk("\n\n\n\n\n"));
                parrafo.Add(frase);
                document.Add(parrafo);
            }
            else
            {
                while (writer.GetVerticalPosition(false) < (fMargenSuperiorDocFirma + 120))
                {
                    document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n")));
                }
            }

            parrafo = new iTextSharp.text.Paragraph();
            frase = new iTextSharp.text.Phrase();
            
            int tamanoX;
            int tamanoY;
            tamanoX = 555;
            //==========================================
            if (listFirmas[indiceIzquierdo].bIncapacitado && listFirmas[indiceIzquierdo].bImprimirFirma == false)
            {
                //Esta incapacitado y no puede firmar ni poner su huella

                //----------------------------------
                // Testear
                //----------------------------------
                iTextSharp.text.pdf.BaseFont bfHelvetica = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA, iTextSharp.text.pdf.BaseFont.CP1252, false);
                cb = writer.DirectContent;

                #region Testear la Firma
                //-------------------------------------
                //Fecha: 20/09/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Testear la firma
                //-------------------------------------
                cb.SetFontAndSize(bfHelvetica, 7);
                cb.SetLineDash(7f, 2f, 0f);

                cb.MoveTo(document.PageSize.Width - tamanoX, writer.GetVerticalPosition(false) + 5);
                cb.LineTo(document.PageSize.Width - tamanoX + 160, writer.GetVerticalPosition(false) + 5);
                cb.Stroke();

                cb.MoveTo(document.PageSize.Width - tamanoX, writer.GetVerticalPosition(false) + 8);
                cb.LineTo(document.PageSize.Width - tamanoX + 160, writer.GetVerticalPosition(false) + 8);
                cb.Stroke();
                #endregion

                #region Testear Huella Dactilar
                tamanoX = 560;
                if (listFirmas[indiceIzquierdo].bAplicaHuellaDigital == false)
                {
                    //--------------------------------------
                    //Fecha: 20/09/2017
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Testear la huella dactilar
                    //--------------------------------------
                    cb.MoveTo(document.PageSize.Width - tamanoX + 180, writer.GetVerticalPosition(false) + 5);
                    cb.LineTo(document.PageSize.Width - tamanoX + 230, writer.GetVerticalPosition(false) + 5);
                    cb.Stroke();

                    cb.MoveTo(document.PageSize.Width - tamanoX + 180, writer.GetVerticalPosition(false) + 8);
                    cb.LineTo(document.PageSize.Width - tamanoX + 230, writer.GetVerticalPosition(false) + 8);
                    cb.Stroke();
                }
                #endregion

                parrafo.Add(frase);
                document.Add(parrafo);

                //----------------------------------
                frase = new iTextSharp.text.Phrase();
                parrafo = new iTextSharp.text.Paragraph();
            }

            if (listFirmas[indiceDerecho].bIncapacitado && listFirmas[indiceDerecho].bImprimirFirma == false)
            {

                //Esta incapacitado y no puede firmar ni poner su huella

                //----------------------------------
                // Testear
                //----------------------------------
                iTextSharp.text.pdf.BaseFont bfHelvetica = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA, iTextSharp.text.pdf.BaseFont.CP1252, false);
                cb = writer.DirectContent;
                tamanoX = 270;
                #region Testear la Firma
                //-------------------------------------
                //Fecha: 20/09/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Testear la firma
                //-------------------------------------
                cb.SetFontAndSize(bfHelvetica, 7);
                cb.SetLineDash(7f, 2f, 0f);

                cb.MoveTo(document.PageSize.Width - tamanoX, writer.GetVerticalPosition(false) + 5);
                cb.LineTo(document.PageSize.Width - tamanoX + 155, writer.GetVerticalPosition(false) + 5);
                cb.Stroke();

                cb.MoveTo(document.PageSize.Width - tamanoX, writer.GetVerticalPosition(false) + 8);
                cb.LineTo(document.PageSize.Width - tamanoX + 155, writer.GetVerticalPosition(false) + 8);
                cb.Stroke();
                #endregion

                #region Testear Huella Dactilar
                tamanoX = 280;
                if (listFirmas[indiceDerecho].bAplicaHuellaDigital == false)
                {
                    //--------------------------------------
                    //Fecha: 20/09/2017
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Testear la huella dactilar
                    //--------------------------------------
                    cb.MoveTo(document.PageSize.Width - tamanoX + 180, writer.GetVerticalPosition(false) + 5);
                    cb.LineTo(document.PageSize.Width - tamanoX + 230, writer.GetVerticalPosition(false) + 5);
                    cb.Stroke();

                    cb.MoveTo(document.PageSize.Width - tamanoX + 180, writer.GetVerticalPosition(false) + 8);
                    cb.LineTo(document.PageSize.Width - tamanoX + 230, writer.GetVerticalPosition(false) + 8);
                    cb.Stroke();
                }
                #endregion

                parrafo.Add(frase);
                document.Add(parrafo);

                //----------------------------------
                frase = new iTextSharp.text.Phrase();
                parrafo = new iTextSharp.text.Paragraph();
            }

            //==========================================

            //---------------------------------------------
            //Imprimir la linea para la firma
            //---------------------------------------------
            frase.Add(new iTextSharp.text.pdf.draw.LineSeparator(0.5F, 30.0F, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_LEFT, 1));
            frase.Add(new iTextSharp.text.pdf.draw.LineSeparator(0.5F, 45.0F, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_RIGHT, 1));
            frase.Add(new iTextSharp.text.pdf.draw.LineSeparator(2.5F, 15.0F, iTextSharp.text.BaseColor.WHITE, iTextSharp.text.Element.ALIGN_RIGHT, 1));

            parrafo.Add(frase);
            document.Add(parrafo);

            cb = writer.DirectContent;
            cb.LineTo(100, 500);
            cb.Stroke();
            cb.LineTo(document.PageSize.Width - 560, writer.GetVerticalPosition(false) - 25);
            cb.Stroke();

            #region Impresión de la caja de la huella dactilar izquierda y derecha

            cb = writer.DirectContent;
            cb.SetLineDash(1);
            cb.Rectangle(document.PageSize.Width - 100, writer.GetVerticalPosition(false) - 15, 50f, 60f);
            

            cb.Rectangle(document.PageSize.Width - 380, writer.GetVerticalPosition(false) - 15, 50f, 60f);
            
            cb.Stroke();

            #endregion
            //=================================================
            #region Firma de la izquierda

            cb.BeginText();

            iTextSharp.text.pdf.BaseFont bf = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA, iTextSharp.text.pdf.BaseFont.CP1252, false);

            tamanoX = 560;
            
            tamanoY = 25;

            /*Validación en caso el texto sea muy grande - FIRMA IZQUIERDA*/
            
            string strNombrePersona;
            strNombrePersona = listFirmas[indiceIzquierdo].vNombreCompleto;

            if (strNombrePersona.Length > 28 && fFontSizeExtraProtocolar >= 8 && fFontSizeExtraProtocolar <= 11)
            {
                #region Dividir Nombre largo
                
                string[] PartesNombre = strNombrePersona.Split(' ');           


                string strPart1_Nombres = "";
                string strPart2_Nombres = "";
                int intCantidadCaracteres = 0;

                for (int i = 0; i < PartesNombre.Length; i++)
                {
                    if (intCantidadCaracteres != 29)
                    {
                        intCantidadCaracteres = strPart1_Nombres.Length + (PartesNombre[i].ToString() + " ").Length;
                    }
                    if (intCantidadCaracteres <= 28)
                    {
                        strPart1_Nombres = strPart1_Nombres + PartesNombre[i].ToString() + " ";
                    }
                    else
                    {
                        intCantidadCaracteres = 29;
                        strPart2_Nombres = strPart2_Nombres + PartesNombre[i].ToString() + " ";
                    }
                }

                cb.SetFontAndSize(bf, fFontSizeExtraProtocolar);
                cb.SetTextMatrix(document.PageSize.Width - tamanoX, writer.GetVerticalPosition(false) - tamanoY);
                cb.ShowText("\n" + strPart1_Nombres);

                // en una nueva linea pintamos el texto que falta del nombre
                tamanoY = 35;
                cb.SetTextMatrix(document.PageSize.Width - tamanoX, writer.GetVerticalPosition(false) - tamanoY);
                cb.ShowText("\n" + strPart2_Nombres);
                tamanoY = 45;
                #endregion
            }
            else if (strNombrePersona.Length > 21 && fFontSizeExtraProtocolar >= 12)
            {
                #region Dividir Nombre largo

                string[] PartesNombre = strNombrePersona.Split(' ');


                string strPart1_Nombres = "";
                string strPart2_Nombres = "";
                int intCantidadCaracteres = 0;

                for (int i = 0; i < PartesNombre.Length; i++)
                {
                    if (intCantidadCaracteres != 22)
                    {
                        intCantidadCaracteres = strPart1_Nombres.Length + (PartesNombre[i].ToString() + " ").Length;
                    }
                    if (intCantidadCaracteres <= 21)
                    {
                        strPart1_Nombres = strPart1_Nombres + PartesNombre[i].ToString() + " ";
                    }
                    else
                    {
                        intCantidadCaracteres = 22;
                        strPart2_Nombres = strPart2_Nombres + PartesNombre[i].ToString() + " ";
                    }
                }

                cb.SetFontAndSize(bf, fFontSizeExtraProtocolar);
                cb.SetTextMatrix(document.PageSize.Width - tamanoX, writer.GetVerticalPosition(false) - tamanoY);
                cb.ShowText("\n" + strPart1_Nombres);

                // en una nueva linea pintamos el texto que falta del nombre
                tamanoY = 35;
                cb.SetTextMatrix(document.PageSize.Width - tamanoX, writer.GetVerticalPosition(false) - tamanoY);
                cb.ShowText("\n" + strPart2_Nombres);
                tamanoY = 45;
                #endregion
            }
            else
            {
                cb.SetFontAndSize(bf, fFontSizeExtraProtocolar);
                cb.SetTextMatrix(document.PageSize.Width - tamanoX, writer.GetVerticalPosition(false) - tamanoY);
                cb.ShowText("\n" + listFirmas[indiceIzquierdo].vNombreCompleto);
                tamanoY = 35;
            }            
            
            cb.SetTextMatrix(document.PageSize.Width - tamanoX, writer.GetVerticalPosition(false) - tamanoY);
            cb.ShowText("\n" + listFirmas[indiceIzquierdo].vNroDocumentoCompleto);
            cb.EndText();

            #endregion
            //=================================================

            //=================================================
            #region Firma de la derecha

            cb.BeginText();
            tamanoX = 270;
            
            //tamanoY = 30;
            tamanoY = 25;
            /*Validación en caso la el texto sea muy grande - FIRMA DERECHA*/
            strNombrePersona = listFirmas[indiceDerecho].vNombreCompleto;
            if (strNombrePersona.Length > 28 && fFontSizeExtraProtocolar >= 9 && fFontSizeExtraProtocolar <= 11)
            {
                #region Nombre de la Persona mayor a 28 caracteres

                string[] PartesNombre = strNombrePersona.Split(' ');


                string strPart1_Nombres = "";
                string strPart2_Nombres = "";
                int intCantidadCaracteres = 0;
                for (int i = 0; i < PartesNombre.Length; i++)
                {
                    if (intCantidadCaracteres != 29)
                    {
                        intCantidadCaracteres = strPart1_Nombres.Length + (PartesNombre[i].ToString() + " ").Length;
                    }

                    if (intCantidadCaracteres <= 28)
                    {
                        strPart1_Nombres = strPart1_Nombres + PartesNombre[i].ToString() + " ";
                    }
                    else
                    {
                        intCantidadCaracteres = 29;
                        strPart2_Nombres = strPart2_Nombres + PartesNombre[i].ToString() + " ";
                    }
                }

                cb.SetFontAndSize(bf, fFontSizeExtraProtocolar);
                cb.SetTextMatrix(document.PageSize.Width - tamanoX, writer.GetVerticalPosition(false) - tamanoY);
                cb.ShowText("\n" + strPart1_Nombres);

                // en una nueva linea pintamos el texto que falta del nombre
                tamanoY = 35;
                cb.SetTextMatrix(document.PageSize.Width - tamanoX, writer.GetVerticalPosition(false) - tamanoY);
                cb.ShowText("\n" + strPart2_Nombres);
                tamanoY = 45;

                #endregion
            }
            else if (strNombrePersona.Length > 21 && fFontSizeExtraProtocolar >= 12)
            {
                #region Nombre de la Persona mayor a 21 caracteres y la letra es mayor a 12

                string[] PartesNombre = strNombrePersona.Split(' ');


                string strPart1_Nombres = "";
                string strPart2_Nombres = "";
                int intCantidadCaracteres = 0;

                for (int i = 0; i < PartesNombre.Length; i++)
                {
                    if (intCantidadCaracteres != 22)
                    {
                        intCantidadCaracteres = strPart1_Nombres.Length + (PartesNombre[i].ToString() + " ").Length;
                    }

                    if (intCantidadCaracteres <= 21)
                    {
                        strPart1_Nombres = strPart1_Nombres + PartesNombre[i].ToString() + " ";
                    }
                    else
                    {
                        intCantidadCaracteres = 22;
                        strPart2_Nombres = strPart2_Nombres + PartesNombre[i].ToString() + " ";
                    }
                }

                cb.SetFontAndSize(bf, fFontSizeExtraProtocolar);
                cb.SetTextMatrix(document.PageSize.Width - tamanoX, writer.GetVerticalPosition(false) - tamanoY);
                cb.ShowText("\n" + strPart1_Nombres);

                // en una nueva linea pintamos el texto que falta del nombre
                tamanoY = 35;
                cb.SetTextMatrix(document.PageSize.Width - tamanoX, writer.GetVerticalPosition(false) - tamanoY);
                cb.ShowText("\n" + strPart2_Nombres);
                tamanoY = 45;

                #endregion
            }
            else {
                cb.SetFontAndSize(bf, fFontSizeExtraProtocolar);
                cb.SetTextMatrix(document.PageSize.Width - tamanoX, writer.GetVerticalPosition(false) - tamanoY);
                cb.ShowText("\n" + listFirmas[indiceDerecho].vNombreCompleto);
                tamanoY = 35;
            }
            
            //Imprime número del documento
            
            cb.SetTextMatrix(document.PageSize.Width - tamanoX, writer.GetVerticalPosition(false) - tamanoY);
            cb.ShowText("\n" + listFirmas[indiceDerecho].vNroDocumentoCompleto);
            cb.EndText();
            #endregion
            //=================================================
        }

        public static void ModoLectura(ref Button[] botones)
        {
            for (int i = 0; i < botones.Length; i++)
            {
                botones[i].Visible = false;
            }
        }
        public static void ModoLectura(ref GridView[] gridview)
        {
            for (int x = 0; x < gridview.Length; x++)
            {
                gridview[x].Enabled = false;
            }
        }

        public static void ModoLectura(ref ImageButton[] imgButton)
        {
            for (int x = 0; x < imgButton.Length; x++)
            {
                imgButton[x].Visible = false;
            }
        }

        //-----------------------------------------------------------//
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 31/10/2016
        // Objetivo: Asignar moneda dolar cuando es gratuito por ley
        //           para la tarifa de la sección III.   
        //-----------------------------------------------------------//
        public static bool isSeccionIII(HttpSessionState Session, string strTarifaId)
        {
            try
            {
                bool bSeccionIII = false;

                DataTable dtTarifario;
                object[] arrParametros = { 0, strTarifaId, 
                                       "", 
                                       ((char)Enumerador.enmEstado.ACTIVO).ToString(),
                                       1, 50, 0, 0 };
                dtTarifario = comun_Part2.ObtenerTarifario(Session, ref arrParametros);
                if (dtTarifario != null)
                {
                    if (dtTarifario.Rows.Count > 0)
                    {
                        if (dtTarifario.Rows[0]["tari_sSeccionId"].ToString().Trim() == "3")
                        {
                            bSeccionIII = true;
                        }
                    }
                }
                return bSeccionIII;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        //------------------------------------------------------------------------------//
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 14/03/2019
        // Objetivo: Verificar si el tipopago es Exoneración o Inafecto por Indigencia
        //------------------------------------------------------------------------------//

        public static bool ExisteInafecto_Exoneracion(ref DropDownList ddlTipoPago)
        {
            bool bExiste = false;
            string strTexto = "";
            string strTipoPagoId = "";

            string strID = ddlTipoPago.SelectedValue;

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
        public static double CalculaCostoML(double decMontoSC, double decTipoCambio)
        {
            return (decMontoSC * decTipoCambio);
        }
        public static double CalcularTarifarioEspecial(string strTarifaId, double intCantidad)
        {
            double douMonto = 0;
            switch (strTarifaId)
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

        //-----------------------------------------------------------//
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 14-03-2019
        // Objetivo: Asignar la moneda estadounidense para los tipos 
        //           de pago: pagados en lima en los consulados
        //-----------------------------------------------------------//
        public static void MostrarMonedaDolar(HttpSessionState Session, ref DropDownList ddlTipoPago, string strTarifaId,
            ref Label LblDescMtoML, ref Label LblDescTotML, ref TextBox txtMontoML, ref TextBox txtTotalML, ref TextBox txtMtoCancelado,
            string txtCantidad, string strFormato, ref TextBox txtMontoSC)
        {
            try
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
                    //if (Comun.ToNullInt32(ddlTipoPago.SelectedValue) == (int)Enumerador.enmTipoCobroActuacion.GRATIS
                    //    && Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Constantes.CONST_OFICINACONSULAR_LIMA
                    //    && Comun.isSeccionIII(Session, strTarifaId) == true)
                    LblDescMtoML.Text = Session[Constantes.CONST_SESION_TIPO_MONEDA].ToString();
                    LblDescTotML.Text = Session[Constantes.CONST_SESION_TIPO_MONEDA].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static string ObtenerDescripcionTipoPago(HttpSessionState Session, string strTipoPagoId)
        {
            try
            {
                DataTable dtTipoPago = new DataTable();
                dtTipoPago = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACREDITACION_TIPO_COBRO);

                string strDescTipoPago = "";

                for (int i = 0; i < dtTipoPago.Rows.Count; i++)
                {
                    if (dtTipoPago.Rows[i]["id"].ToString().Equals(strTipoPagoId))
                    {
                        strDescTipoPago = dtTipoPago.Rows[i]["descripcion"].ToString();
                        break;
                    }
                }
                return strDescTipoPago;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //------------------------------------------------------------------------------//
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 14/03/2019
        // Objetivo: Controlar los objetos del proceso de pago.
        //------------------------------------------------------------------------------//        

        public static void ActualizarControlPago(HttpSessionState Session, string strTipoPagoOrigen, string strTarifaId, string txtCantidad, 
            ref Button btnGrabar, ref DropDownList ddlTipoPagoDestino, ref TextBox txtNroOperacion, ref TextBox txtCodAutoadhesivo, 
            ref DropDownList ddlBanco, ref Accesorios.SharedControls.ctrlDate txtFechaPago, ref DropDownList ddlExoneracion, ref Label lblExoneracion, ref Label lblValExoneracion,
            ref TextBox txtSustentoTipoPago, ref Label lblSustentoTipoPago, ref Label lblValSustentoTipoPago,
            ref RadioButton rbNormativa, ref RadioButton rbSustentoTipoPago, ref TextBox txtMontoML, ref TextBox txtMontoSC,
            ref TextBox txtTotalML, ref TextBox txtTotalSC, ref Label LblDescMtoML, ref Label LblDescTotML,
            ref System.Web.UI.HtmlControls.HtmlTableCell pnlPagLima, ref TextBox txtMtoCancelado)
        {
            try
            {
                //------------------------------
                pnlPagLima.Visible = false;
                txtNroOperacion.Enabled = false;
                ddlBanco.Enabled = false;
                txtFechaPago.Enabled = false;
                //------------------------------
                lblExoneracion.Visible = false;
                ddlExoneracion.Enabled = false;
                ddlExoneracion.Visible = false;
                lblValExoneracion.Visible = false;
                lblSustentoTipoPago.Visible = false;
                txtSustentoTipoPago.Enabled = false;
                txtSustentoTipoPago.Visible = false;
                lblValSustentoTipoPago.Visible = false;
                rbNormativa.Visible = false;
                rbNormativa.Enabled = false;
                rbNormativa.Checked = false;
                rbSustentoTipoPago.Visible = false;
                rbSustentoTipoPago.Enabled = false;
                rbSustentoTipoPago.Checked = false;
                txtMtoCancelado.Enabled = false;
                btnGrabar.Enabled = false;
                ddlTipoPagoDestino.Enabled = true;

                //--------------------------------------

                if (ddlTipoPagoDestino.SelectedIndex == 0)
                {
                    return;
                }


                strTarifaId = strTarifaId.Trim().ToUpper();

                if (string.IsNullOrEmpty(strTarifaId))
                {
                    return;
                }
                string strFormato = ConfigurationManager.AppSettings["FormatoMonto"].ToString();

                strTipoPagoOrigen = strTipoPagoOrigen.Trim().ToUpper();

                int intCantidad = 0;
                double decMontoSC = 0;
                double decTotalSC = 0;
                double decMontoML = 0;
                double decTotalML = 0;

                if (!string.IsNullOrEmpty(txtCantidad))
                {
                    BE.MRE.SI_TARIFARIO objTarifarioBE = new BE.MRE.SI_TARIFARIO();
                    objTarifarioBE = Comun.ObtenerTarifario(Session, strTarifaId);

                    if (objTarifarioBE != null)
                    {
                        double dCantidad = Convert.ToDouble(txtCantidad);
                        intCantidad = Convert.ToInt32(dCantidad);

                        decMontoSC = (double)objTarifarioBE.tari_FCosto;
                        decTotalSC = Tarifario.Calculo(objTarifarioBE, intCantidad);
                        decMontoML = CalculaCostoML(decMontoSC, Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]));
                        decTotalML = CalculaCostoML(decTotalSC, Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]));

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
                        txtMtoCancelado.Text = txtTotalML.Text;
                    }
                }

                MostrarMonedaDolar(Session, ref ddlTipoPagoDestino, strTarifaId, ref LblDescMtoML, ref LblDescTotML,
                                    ref txtMontoML, ref txtTotalML, ref txtMtoCancelado, txtCantidad, strFormato, ref txtMontoSC);


                BE.RE_TARIFA_PAGO objTarifaPago = new BE.RE_TARIFA_PAGO();
                objTarifaPago = (BE.RE_TARIFA_PAGO)Session[Constantes.CONST_SESION_OBJ_TARIFA_PAGO];


                bool isVinculado = false;

                if (txtCodAutoadhesivo.Text.Length > 0 && txtCodAutoadhesivo.Enabled == false)
                {
                    isVinculado = true;
                }

                bool isSeccionIII = Comun.isSeccionIII(Session, strTarifaId);
                bool isCostoCero = Comun.isCostoCero(Session, strTarifaId);

                if (isVinculado == true)
                {
                    //-------------------
                    //Vinculado
                    //-------------------

                    #region Vinculado
                    switch (strTipoPagoOrigen)
                    {
                        case "PAGADO EN LIMA":

                            if (ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "PAGADO EN LIMA")
                            {
                                pnlPagLima.Visible = true;
                                txtNroOperacion.Enabled = true;
                                ddlBanco.Enabled = true;
                                txtFechaPago.Enabled = true;
                                btnGrabar.Enabled = true;

                                txtMontoML.Text = "0.00";
                                txtTotalML.Text = "0.00";
                            }
                            break;
                        case "EFECTIVO":
                            if (ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "TRANSFERENCIA BANCARIA" ||
                               ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "DEPÓSITO EN CUENTA")
                            {
                                pnlPagLima.Visible = true;
                                ddlTipoPagoDestino.Enabled = true;
                                txtNroOperacion.Enabled = true;
                                ddlBanco.Enabled = true;
                                txtFechaPago.Enabled = true;
                                btnGrabar.Enabled = true;
                            }
                            if (ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "EFECTIVO" ||
                                ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "TARJETA DE CRÉDITO" ||
                                ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "MONEY ORDER" ||
                                ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "TARJETA DE DEBITO")
                            {
                                ddlTipoPagoDestino.Enabled = true;
                                btnGrabar.Enabled = true;

                            }
                            break;
                        case "TARJETA DE CRÉDITO":
                            if (ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "TRANSFERENCIA BANCARIA" ||
                               ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "DEPÓSITO EN CUENTA")
                            {
                                pnlPagLima.Visible = true;
                                ddlTipoPagoDestino.Enabled = true;
                                txtNroOperacion.Enabled = true;
                                ddlBanco.Enabled = true;
                                txtFechaPago.Enabled = true;
                                btnGrabar.Enabled = true;
                            }

                            if (ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "TARJETA DE CRÉDITO" ||
                                ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "MONEY ORDER" ||
                                ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "TARJETA DE DEBITO" ||
                                ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "EFECTIVO")
                            {
                                ddlTipoPagoDestino.Enabled = true;
                                btnGrabar.Enabled = true;
                            }
                            break;
                        case "MONEY ORDER":
                            if (ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "TRANSFERENCIA BANCARIA" ||
                                ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "DEPÓSITO EN CUENTA")
                            {
                                pnlPagLima.Visible = true;
                                ddlTipoPagoDestino.Enabled = true;
                                txtNroOperacion.Enabled = true;
                                ddlBanco.Enabled = true;
                                txtFechaPago.Enabled = true;
                                btnGrabar.Enabled = true;
                            }

                            if (ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "MONEY ORDER" ||
                                ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "TARJETA DE CRÉDITO" ||
                                ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "EFECTIVO" ||
                                ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "TARJETA DE DEBITO")
                            {
                                ddlTipoPagoDestino.Enabled = true;
                                btnGrabar.Enabled = true;
                            }
                            break;
                        case "TARJETA DE DEBITO":
                            if (ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "TRANSFERENCIA BANCARIA" ||
                                ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "DEPÓSITO EN CUENTA")
                            {
                                pnlPagLima.Visible = true;
                                ddlTipoPagoDestino.Enabled = true;
                                txtNroOperacion.Enabled = true;
                                ddlBanco.Enabled = true;
                                txtFechaPago.Enabled = true;
                                btnGrabar.Enabled = true;
                            }
                            if (ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "TARJETA DE DEBITO" ||
                                ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "TARJETA DE CRÉDITO" ||
                                ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "MONEY ORDER" ||
                                ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "EFECTIVO")
                            {
                                ddlTipoPagoDestino.Enabled = true;
                                btnGrabar.Enabled = true;
                            }
                            break;
                        case "TRANSFERENCIA BANCARIA":
                            if (ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "TRANSFERENCIA BANCARIA" ||
                                ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "DEPÓSITO EN CUENTA")
                            {
                                pnlPagLima.Visible = true;
                                ddlTipoPagoDestino.Enabled = true;
                                txtNroOperacion.Enabled = true;
                                ddlBanco.Enabled = true;
                                txtFechaPago.Enabled = true;
                                btnGrabar.Enabled = true;
                            }
                            if (ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "TARJETA DE CRÉDITO" ||
                                ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "MONEY ORDER" ||
                                ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "EFECTIVO" ||
                                ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "TARJETA DE DEBITO")
                            {
                                ddlTipoPagoDestino.Enabled = true;
                                btnGrabar.Enabled = true;
                            }
                            break;
                        case "DEPÓSITO EN CUENTA":
                            if (ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "DEPÓSITO EN CUENTA" ||
                                ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "TRANSFERENCIA BANCARIA")
                            {
                                ddlTipoPagoDestino.Enabled = true;

                                pnlPagLima.Visible = true;
                                txtNroOperacion.Enabled = true;
                                ddlBanco.Enabled = true;
                                txtFechaPago.Enabled = true;
                                btnGrabar.Enabled = true;
                            }
                            if (ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "TARJETA DE CRÉDITO" ||
                                ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "MONEY ORDER" ||
                                ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "EFECTIVO" ||
                                ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "TARJETA DE DEBITO")
                            {
                                ddlTipoPagoDestino.Enabled = true;
                                btnGrabar.Enabled = true;
                            }
                            break;
                        case "GRATUITO POR LEY":

                            if (ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "GRATUITO POR LEY")
                            {
                                lblExoneracion.Visible = true;
                                ddlExoneracion.Enabled = true;
                                ddlExoneracion.Visible = true;
                                lblValExoneracion.Visible = true;
                                lblSustentoTipoPago.Visible = true;
                                txtSustentoTipoPago.Enabled = true;
                                txtSustentoTipoPago.Visible = true;
                                lblValSustentoTipoPago.Visible = true;
                                btnGrabar.Enabled = true;
                                txtTotalSC.Text = "0.00";
                                txtTotalML.Text = "0.00";

                                if (isSeccionIII == true || strTarifaId == "2")
                                {
                                    rbNormativa.Visible = true;
                                    rbNormativa.Enabled = true;
                                    rbSustentoTipoPago.Visible = true;
                                    rbSustentoTipoPago.Enabled = true;
                                    txtSustentoTipoPago.Enabled = false;
                                    if (txtSustentoTipoPago.Text.Trim().Length == 0)
                                    {
                                        rbNormativa.Checked = true;
                                    }
                                    else
                                    {
                                        rbSustentoTipoPago.Checked = true;
                                        ddlExoneracion.Enabled = false;
                                        txtSustentoTipoPago.Enabled = true;
                                    }
                                }
                                //-----------------------------------------------------------------------
                                //Autor: Miguel Márquez Beltrán
                                //Fecha: 22/03/2019
                                //Objetivo: No editar el tipo de pago para las tarifas que son gratuitas.
                                //-----------------------------------------------------------------------
                                if (isCostoCero == true)
                                {
                                    ddlTipoPagoDestino.Enabled = false;
                                }
                                //-----------------------------------------------------------------------
                            }


                            break;
                        case "INAFECTO POR INDIGENCIA":
                            if (ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "INAFECTO POR INDIGENCIA")
                            {
                                lblExoneracion.Visible = true;
                                ddlExoneracion.Enabled = true;
                                ddlExoneracion.Visible = true;
                                lblValExoneracion.Visible = true;

                                btnGrabar.Enabled = true;
                                txtTotalSC.Text = "0.00";
                                txtTotalML.Text = "0.00";

                            }
                            else
                            {
                                if (ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "GRATUITO POR LEY")
                                {
                                    lblExoneracion.Visible = true;
                                    ddlExoneracion.Enabled = true;
                                    ddlExoneracion.Visible = true;
                                    lblValExoneracion.Visible = true;
                                    lblSustentoTipoPago.Visible = true;
                                    txtSustentoTipoPago.Enabled = true;
                                    txtSustentoTipoPago.Visible = true;
                                    lblValSustentoTipoPago.Visible = true;
                                    btnGrabar.Enabled = true;

                                    if (isSeccionIII == true || strTarifaId == "2")
                                    {
                                        rbNormativa.Visible = true;
                                        rbNormativa.Enabled = true;

                                        rbSustentoTipoPago.Visible = true;
                                        rbSustentoTipoPago.Enabled = true;
                                        txtSustentoTipoPago.Enabled = false;

                                        if (txtSustentoTipoPago.Text.Trim().Length == 0)
                                        {
                                            rbNormativa.Checked = true;
                                        }
                                        else
                                        {
                                            rbSustentoTipoPago.Checked = true;
                                            ddlExoneracion.Enabled = false;
                                            txtSustentoTipoPago.Enabled = true;
                                        }
                                    }
                                    txtTotalSC.Text = "0.00";
                                    txtTotalML.Text = "0.00";
                                }
                            }

                            break;

                        case "PAGO OTRAS ISLAS CARIBEÑAS":
                        case "PAGO ARUBA":
                            if (ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "PAGO OTRAS ISLAS CARIBEÑAS" ||
                                ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper() == "PAGO ARUBA")
                            {
                                btnGrabar.Enabled = true;
                                txtMontoML.Text = "0.00";
                                txtTotalML.Text = "0.00";
                            }
                            break;
                        default:
                            break;
                    }
                    #endregion
                }
                else
                {
                    //-------------------
                    //No Vinculado
                    //-------------------
                    #region NO_Vinculado

                    switch (ddlTipoPagoDestino.SelectedItem.Text.Trim().ToUpper())
                    {
                        case "PAGADO EN LIMA":
                        case "TRANSFERENCIA BANCARIA":
                        case "DEPÓSITO EN CUENTA":
                            pnlPagLima.Visible = true;
                            txtNroOperacion.Enabled = true;
                            ddlBanco.Enabled = true;
                            txtFechaPago.Enabled = true;
                            btnGrabar.Enabled = true;

                            if (Comun.ToNullInt32(ddlTipoPagoDestino.SelectedValue) == (int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA)
                            {
                                txtMontoML.Text = "0.00";
                                txtTotalML.Text = "0.00";
                            }

                            break;
                        case "EFECTIVO":
                        case "TARJETA DE CRÉDITO":
                        case "MONEY ORDER":
                        case "TARJETA DE DEBITO":
                            btnGrabar.Enabled = true;

                            break;

                        case "GRATUITO POR LEY":
                            lblExoneracion.Visible = true;
                            ddlExoneracion.Visible = true;
                            ddlExoneracion.Enabled = true;
                            lblValExoneracion.Visible = true;
                            lblSustentoTipoPago.Visible = true;
                            txtSustentoTipoPago.Enabled = true;
                            txtSustentoTipoPago.Visible = true;
                            lblValSustentoTipoPago.Visible = true;
                            btnGrabar.Enabled = true;
                            if (isSeccionIII == true || strTarifaId == "2")
                            {
                                rbNormativa.Visible = true;
                                rbNormativa.Enabled = true;

                                rbSustentoTipoPago.Visible = true;
                                rbSustentoTipoPago.Enabled = true;
                                txtSustentoTipoPago.Enabled = false;
                                if (txtSustentoTipoPago.Text.Trim().Length == 0)
                                {
                                    rbNormativa.Checked = true;
                                }
                                else
                                {
                                    rbSustentoTipoPago.Checked = true;
                                    ddlExoneracion.Enabled = false;
                                    txtSustentoTipoPago.Enabled = true;
                                }
                            }
                            //-----------------------------------------------------------------------
                            //Autor: Miguel Márquez Beltrán
                            //Fecha: 22/03/2019
                            //Objetivo: No editar el tipo de pago para las tarifas que son gratuitas.
                            //-----------------------------------------------------------------------

                            if (isCostoCero == true)
                            {
                                ddlTipoPagoDestino.Enabled = false;
                            }

                            txtTotalSC.Text = "0.00";
                            txtTotalML.Text = "0.00";

                            break;
                        case "INAFECTO POR INDIGENCIA":
                            lblExoneracion.Visible = true;
                            ddlExoneracion.Enabled = true;
                            ddlExoneracion.Visible = true;
                            lblValExoneracion.Visible = true;

                            btnGrabar.Enabled = true;

                            txtTotalSC.Text = "0.00";
                            txtTotalML.Text = "0.00";

                            break;
                        case "PAGO OTRAS ISLAS CARIBEÑAS":
                        case "PAGO ARUBA":
                            btnGrabar.Enabled = true;
                            txtMontoML.Text = "0.00";
                            txtTotalML.Text = "0.00";
                            break;

                        default:
                            break;
                    }
                    #endregion
                }

                if (ddlExoneracion.Visible == true)
                {
                    DataTable dtExoneracion = new DataTable();
                    SGAC.Configuracion.Sistema.BL.NormaTarifarioDL objNormaTarifario = new SGAC.Configuracion.Sistema.BL.NormaTarifarioDL();
                    int IntTotalCount = 0;
                    int IntTotalPages = 0;

                    string strFecha = Comun.syyyymmdd(DateTime.Now.ToShortDateString());
                    Int16 intTipoPagoId = Convert.ToInt16(ddlTipoPagoDestino.SelectedValue);

                    dtExoneracion = objNormaTarifario.Consultar(intTipoPagoId, -1, strTarifaId, strFecha, false, 1000, 1, "N", ref IntTotalCount, ref IntTotalPages);

                    Util.CargarDropDownList(ddlExoneracion, dtExoneracion, "norm_vDescripcionCorta", "nota_iNormaTarifarioId", true);

                    if (ddlExoneracion.Items.Count > 1)
                    {
                        ddlExoneracion.SelectedIndex = 1;
                    }
                }
                else
                {
                    if (ddlExoneracion.Items.Count > 0)
                    {
                        ddlExoneracion.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
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
        public static Int16 ObtenerMonedaLocalId(HttpSessionState Session, string strTipoPagoId, string strTarifaId)
        {
            try
            {
                Int16 intMonedaIdDolar = Convert.ToInt16(Session[Constantes.CONST_SESION_TIPO_MONEDA_ID]);


                if (Comun.ToNullInt32(strTipoPagoId) == (int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA
                    && Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Constantes.CONST_OFICINACONSULAR_LIMA)
                {
                    if (ConfigurationManager.AppSettings["MonedaIdDolar"] != null)
                        intMonedaIdDolar = Convert.ToInt16(ConfigurationManager.AppSettings["MonedaIdDolar"].ToString());
                }
                else
                {
                    if (Comun.ToNullInt32(strTipoPagoId) == (int)Enumerador.enmTipoCobroActuacion.GRATIS
                                       && Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Constantes.CONST_OFICINACONSULAR_LIMA
                        && Comun.isSeccionIII(Session, strTarifaId) == true)
                    {
                        if (ConfigurationManager.AppSettings["MonedaIdDolar"] != null)
                            intMonedaIdDolar = Convert.ToInt16(ConfigurationManager.AppSettings["MonedaIdDolar"].ToString());
                    }
                }


                return intMonedaIdDolar;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool isCostoCero(HttpSessionState Session, string strTarifaId)
        {
            try
            {
                bool bCostoCero = false;

                DataTable dtTarifario;
                object[] arrParametros = { 0, strTarifaId, 
                                       "", 
                                       ((char)Enumerador.enmEstado.ACTIVO).ToString(),
                                       1, 50, 0, 0 };
                dtTarifario = comun_Part2.ObtenerTarifario(Session, ref arrParametros);
                if (dtTarifario != null)
                {
                    if (dtTarifario.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dtTarifario.Rows[0]["tari_FCosto"].ToString()) == 0)
                        {
                            bCostoCero = true;
                        }
                    }
                }
                return bCostoCero;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public static BE.MRE.SI_TARIFARIO ObtenerTarifario(HttpSessionState Session, string strTarifaId)
        {
            try
            {
                DataTable dtTarifario;
                BE.MRE.SI_TARIFARIO objTarifarioBE = new BE.MRE.SI_TARIFARIO();

                object[] arrParametros = { 0, strTarifaId, 
                                       "", 
                                       ((char)Enumerador.enmEstado.ACTIVO).ToString(),
                                       1, 50, 0, 0 };

                dtTarifario = comun_Part2.ObtenerTarifario(Session, ref arrParametros, "S");

                if (dtTarifario.Rows.Count > 0)
                {
                    objTarifarioBE.tari_sTarifarioId = Convert.ToInt16(dtTarifario.Rows[0]["tari_sTarifarioId"]);
                    objTarifarioBE.tari_sSeccionId = Convert.ToInt16(dtTarifario.Rows[0]["tari_sSeccionId"]);
                    objTarifarioBE.tari_sNumero = Convert.ToInt16(dtTarifario.Rows[0]["tari_sNumero"]);
                    objTarifarioBE.tari_vLetra = dtTarifario.Rows[0]["tari_vLetra"].ToString();
                    objTarifarioBE.tari_FCosto = Convert.ToDouble(dtTarifario.Rows[0]["tari_FCosto"]);
                    objTarifarioBE.tari_vDescripcion = dtTarifario.Rows[0]["tari_vDescripcion"].ToString();
                    objTarifarioBE.tari_vDescripcionCorta = dtTarifario.Rows[0]["tari_vDescripcionCorta"].ToString();

                    objTarifarioBE.tari_sBasePercepcionId = Convert.ToInt16(dtTarifario.Rows[0]["tari_sBasePercepcionId"]);
                    objTarifarioBE.tari_sCalculoTipoId = Convert.ToInt16(dtTarifario.Rows[0]["tari_sCalculoTipoId"]);
                    objTarifarioBE.tari_vCalculoFormula = dtTarifario.Rows[0]["tari_vCalculoFormula"].ToString();

                    objTarifarioBE.tari_sTopeUnidadId = Convert.ToInt16(dtTarifario.Rows[0]["tari_sTopeUnidadId"]);
                    objTarifarioBE.tari_ITopeCantidad = Comun.ToNullInt32(dtTarifario.Rows[0]["tari_ITopeCantidad"]);
                    objTarifarioBE.tari_ITopeCantidadMinima = Comun.ToNullInt32(dtTarifario.Rows[0]["tari_ITopeCantidadMinima"]);

                    if (dtTarifario.Rows[0]["tari_FMontoExceso"] != System.DBNull.Value)
                    {
                        objTarifarioBE.tari_FMontoExceso = Convert.ToDouble(dtTarifario.Rows[0]["tari_FMontoExceso"]);
                    }
                    else
                    {
                        objTarifarioBE.tari_FMontoExceso = 0;
                    }

                    objTarifarioBE.tari_bTarifarioDependienteFlag = Convert.ToBoolean(dtTarifario.Rows[0]["tari_bTarifarioDependienteFlag"]);
                    objTarifarioBE.tari_bHabilitaCantidad = Convert.ToBoolean(dtTarifario.Rows[0]["tari_bHabilitaCantidad"]);
                    objTarifarioBE.tari_bFlujoGeneral = Convert.ToBoolean(dtTarifario.Rows[0]["tari_bFlujoGeneral"]);
                    objTarifarioBE.tari_vTipoPagoExcepcion = Convert.ToString(dtTarifario.Rows[0]["tari_vTipoPagoExcepcion"]);

                }
                return objTarifarioBE;
            }
            catch (Exception ex)
            {
                return new BE.MRE.SI_TARIFARIO();
                throw new Exception(ex.Message, ex.InnerException);
            }
           
        }

        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 18/08/2016
        // Objetivo: Calcular los dias habiles permitidos para la anulación
        //           según sea Jeafatura o Consulado.
        // Referencia: Requerimiento No.001_3.doc
        //------------------------------------------------------------------------
        public static bool CalcularDiasHabilesModificacion(HttpSessionState Session, Page page, string strFechaRegistro)
        {
            try
            {
                bool bpermitir = true;

                DateTime dFecRegistro = Comun.FormatearFecha(strFechaRegistro);
                DateTime dFecActual = Comun.FormatearFecha(Comun.ObtenerFechaActualTexto(Session));

                //DateTime dFecActual = Comun.FormatearFecha("oct-11-2016");

                int intEsJefatura = Convert.ToInt32(Session[Constantes.CONST_SESION_JEFATURA_FLAG]);
                string strDiasHabiles = "";
                string strMsjHabiles = "";
                int intDiasHabiles = 0;

                if (intEsJefatura == 1)
                {
                    strDiasHabiles = ConfigurationManager.AppSettings["sDiasActuacionesHabilesJefatura"].ToString();
                }
                else
                {
                    strDiasHabiles = ConfigurationManager.AppSettings["sDiasActuacionesHabilesConsulado"].ToString();
                }


                strMsjHabiles = ConfigurationManager.AppSettings["vMsjDiasActuacionHabiles"].ToString();

                intDiasHabiles = Convert.ToInt32(strDiasHabiles);

                int intMesFinalFecha = dFecRegistro.Month;
                int intAnioFinalFecha = dFecRegistro.Year;
                int intDiaFinalFecha = dFecRegistro.Day;

                if (intMesFinalFecha == 12)
                {
                    intMesFinalFecha = 1;
                    intAnioFinalFecha++;
                }
                else
                {
                    intMesFinalFecha++;
                }

                DateTime dFecUltima = new DateTime(intAnioFinalFecha, intMesFinalFecha, intDiasHabiles);

                TimeSpan ts = dFecUltima - dFecActual;
                int intDiferenciaDias = ts.Days + 1;


                if (intDiferenciaDias <= 0)
                {
                    Comun.EjecutarScript(page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "TRÁMITE", strMsjHabiles));
                    bpermitir = false;
                }
                return bpermitir;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //------------------------------------------------------------------------
        }

        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 12/06/2019
        // Objetivo: Validar la fecha. Y el año debe ser mayor a 1753 y menor a 9999
        // Fecha de modificación: 02/09/2021
        // Motivo: 
        //------------------------------------------------------------------------
        public static Boolean EsFecha(String strFecha)
        {
            DateTime datFecha = new DateTime();
            try
            {
                if (strFecha != null)
                {
                    strFecha = strFecha.Replace(".", "");
                    strFecha = strFecha.Replace("Ene", "Jan");
                    strFecha = strFecha.Replace("Abr", "Apr");
                    strFecha = strFecha.Replace("Ago", "Aug");
                    strFecha = strFecha.Replace("Set", "Sep");
                    strFecha = strFecha.Replace("Dic", "Dec");

                    strFecha = strFecha.Replace("ene", "Jan");
                    strFecha = strFecha.Replace("abr", "Apr");
                    strFecha = strFecha.Replace("ago", "Aug");
                    strFecha = strFecha.Replace("set", "Sep");
                    strFecha = strFecha.Replace("dic", "Dec");

                }

                if (!DateTime.TryParse(strFecha, out datFecha))
                {
                    datFecha = Convert.ToDateTime(strFecha, System.Globalization.CultureInfo.GetCultureInfo("en-Us").DateTimeFormat);
                }


                int year = datFecha.Year;
                if (year > 1753 && year < 9999)
                { return true; }
                else
                { return false; }                
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 30/10/2019
        // Objetivo: Consultar los documentos de identidad
        //------------------------------------------------------------------------
        public static DataTable ObtenerListaDocumentoIdentidad(Int16 intTipoDocumentoId = 0)
        {
            try
            {
                DataTable dtDocumentoIdentidad = new DataTable();
                SGAC.Configuracion.Maestro.BL.TablaMaestraConsultaBL BL = new SGAC.Configuracion.Maestro.BL.TablaMaestraConsultaBL();

                Int16 intCantidadPaginas = 0;
                Int16 intPageSize = 10000;
                Int16 intNumeroPagina = 1;
                string strDesCorta = "";
                string strContar = "N";
                string strEstado = "A";

                dtDocumentoIdentidad = BL.ConsultarDocumentoIdentidad_MRE(intTipoDocumentoId, strDesCorta, strEstado,
                                                        intPageSize, intNumeroPagina, strContar, ref intCantidadPaginas);

                return dtDocumentoIdentidad;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 04/11/2019
        // Objetivo: Consultar las Bovedas
        //------------------------------------------------------------------------
        public static DataTable ObtenerBovedas()
        {
            try
            {
                DataTable dtBovedas = new DataTable();
                SGAC.Almacen.BL.InsumoConsultaBL BL = new SGAC.Almacen.BL.InsumoConsultaBL();

                dtBovedas = BL.ConsultarBovedas();
                dtBovedas.DefaultView.Sort = "Descripcion ASC";
                return dtBovedas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ObtenerTarifarioCargaInicial(HttpSessionState Session)
        {
            try
            {
                Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());

                SGAC.Configuracion.Sistema.BL.TarifarioConsultasBL BL = new SGAC.Configuracion.Sistema.BL.TarifarioConsultasBL();
                DataTable dtTarifario = new DataTable();

                dtTarifario = BL.ConsultarTarifarioCargaInicial(intOficinaConsularId);

                return dtTarifario;
            }
            catch (Exception ex)
            {
                return new DataTable();
                throw new Exception(ex.Message, ex.InnerException);
            }
            
        }
        public static DataTable ObtenerParametrosCargaInicial()
        {
            try
            {
                SGAC.Configuracion.Maestro.BL.TablaMaestraConsultaBL BL = new SGAC.Configuracion.Maestro.BL.TablaMaestraConsultaBL();
                DataTable dtTablasMaestras = new DataTable();

                dtTablasMaestras = BL.ConsultarTablasMaestrasCargaInicial();

                return dtTablasMaestras;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public static DataTable ObtenerSistemasCargaInicial()
        {
            try
            {
                SGAC.Configuracion.Seguridad.BL.SistemaConsultaBL BL = new SGAC.Configuracion.Seguridad.BL.SistemaConsultaBL();
                DataTable dtSistemas = new DataTable();

                dtSistemas = BL.ConsultarSistemasCargaInicial();

                return dtSistemas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public static DataTable ObtenerTarifarioTotalCargaInicial()
        {
            try
            {
                SGAC.Configuracion.Sistema.BL.TarifarioConsultasBL BL = new SGAC.Configuracion.Sistema.BL.TarifarioConsultasBL();
                DataTable dtTarifario = new DataTable();

                dtTarifario = BL.ConsultarTarifarioTotalCargaInicial();

                return dtTarifario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public static DataTable ObtenerOficinaConsularPorId(HttpSessionState Session)
        {
            try
            {
                int intOficinaConsularId = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
                DataTable dtOficinaConsularPorId = new DataTable();
                SGAC.Configuracion.Sistema.BL.OficinaConsularConsultasBL BL = new SGAC.Configuracion.Sistema.BL.OficinaConsularConsultasBL();

                dtOficinaConsularPorId = BL.ObtenerPorId(intOficinaConsularId);
                return dtOficinaConsularPorId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public static DataTable ObtenerOficinasActivas(Int16 OficinaConsular)
         {
            try
            {
                int intOficinaConsularId = OficinaConsular;

                DataTable dtOficinasActivas = new DataTable();
                SGAC.Configuracion.Sistema.BL.ParametroConsultasBL BL = new SGAC.Configuracion.Sistema.BL.ParametroConsultasBL();

                dtOficinasActivas = BL.ConsultarOficinasActivasCargaInicial(intOficinaConsularId);

                return dtOficinasActivas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public static DataTable ObtenerParametroPorValor(string strGrupo,
                                               string strValor, string strItemInicial = "- SELECCIONAR -", string strEstado = "A",
                                                string strDescripcion = "")
        {
            try
            {
                DataTable dtParametro = new DataTable();

                SGAC.Configuracion.Sistema.BL.ParametroConsultasBL BL = new SGAC.Configuracion.Sistema.BL.ParametroConsultasBL();
                dtParametro = BL.ConsultarParametroPorValor(strGrupo, strValor, strItemInicial, strEstado, strDescripcion);

                return dtParametro;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public static DataTable ObtenerOficinasConsularesCargaInicial()
        {
            try
            {
                DataTable dtOficinasConsulares = new DataTable();
                SGAC.Configuracion.Sistema.BL.OficinaConsularConsultasBL BL = new SGAC.Configuracion.Sistema.BL.OficinaConsularConsultasBL();

                dtOficinasConsulares = BL.ConsultarOficinasConsularesCargaInicial();
                return dtOficinasConsulares;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public static void SeleccionarItem(DropDownList ddl, string strDescripcion)
        {
            for (int i = 0; i < ddl.Items.Count; i++)
            {
                if (ddl.Items[i].Text == strDescripcion)
                {
                    ddl.SelectedValue = ddl.Items[i].Value;
                    break;
                }
            }
        }

        public static List<beUbicaciongeografica> obtenerListaUbiGeo(string listaNro, string codigoUbigeo01, string codigoUbigeo02, List<beUbicaciongeografica> listaUbigeo)
        {
            List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
            switch (listaNro)
            {
                case "02":
                    lbeUbicaciongeografica = listaUbigeo.FindAll(x => x.Ubi01.Equals(codigoUbigeo01) | x.Ubi02.Equals("00"));
                    break;
                case "03":
                    lbeUbicaciongeografica = listaUbigeo.FindAll(x => x.Ubi01.Equals(codigoUbigeo01) & x.Ubi02.Equals(codigoUbigeo02) | x.Ubi03.Equals("00"));
                    break;
            }
            return (lbeUbicaciongeografica);
        }

        //-----------------------------------------------------------
        // Fecha: 24/06/2021
        // Autor: Miguel Márquez Beltrán
        // Motivo: Crear documento de texto en diagonal
        //-----------------------------------------------------------
        public static void CrearDocumentoiTextSharpTextoDiagonal(Page page, string textoHtml, int[,] Rangos)
        {
            try
            {
                #region Inicializando Variables


                float fMargenIzquierdaDoc = 80;
                float fMargenDerechaDoc = 80;
                float fMargenSuperiorDoc = 100;
                float fMargenInferiorDoc = 30;
                float fFontSize = 70f;
                float fFontSizePag = 10f;

                string strContent = string.Empty;

                iTextSharp.text.FontFactory.RegisterDirectories();

                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, fMargenIzquierdaDoc, fMargenDerechaDoc, fMargenSuperiorDoc, fMargenInferiorDoc);

                iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);
                
                iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(document.PageSize.Height, document.PageSize.Width);

                iTextSharp.text.Font fontGeneral = new iTextSharp.text.Font();

                fontGeneral = iTextSharp.text.FontFactory.GetFont("Arial", fFontSize);

                iTextSharp.text.Font fontPagina = new iTextSharp.text.Font();

                fontPagina = iTextSharp.text.FontFactory.GetFont("Arial", fFontSizePag);
                

                #endregion

                using (var ms = new MemoryStream())
                {
                   
                    ms.Position = 0;

                    StreamReader oStreamReader = new StreamReader(ms, System.Text.Encoding.Default);

                    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms);



                    document.Open();

                    iTextSharp.text.pdf.PdfContentByte contentByte = writer.DirectContent;

                    iTextSharp.text.pdf.PdfTemplate template = contentByte.CreateTemplate(500, 600);
                    iTextSharp.text.pdf.PdfTemplate templatePagina = contentByte.CreateTemplate(100, 200);
                    float fx = 320;
                    float fy = 400;
                    float frotacion = 50;
                    int intNroPagina = 0;
                    int intPagIni = 0;
                    int intPagFin = 0;
                    int intTotal = Rangos.Length/2;

                    int[] lPaginas = new int[10000];
                    int intIndice=0;
                    for (int i = 0; i < intTotal; i++)
                    {
                        intPagIni = Rangos[i, 0];
                        intPagFin = Rangos[i, 1];
                        for (int j = intPagIni; j <= intPagFin; j++)
                        {                        
                            intNroPagina = j;
                            lPaginas[intIndice] = intNroPagina;
                            intIndice++;
                            #region CrearDocumento
                            //--------------------------------
                            document.NewPage();
                           
                            //--------------------------------------------
                            template.BeginText();

                            iTextSharp.text.pdf.ColumnText.ShowTextAligned(template, iTextSharp.text.Element.ALIGN_CENTER, new iTextSharp.text.Phrase(textoHtml, fontGeneral),
                                fx, fy, frotacion);


                            template.EndText();
                            contentByte.AddTemplate(template, 0, 0);
                            //--------------------------------------------

                            iTextSharp.text.Paragraph parrafo = new iTextSharp.text.Paragraph();
                            iTextSharp.text.Phrase frase = new iTextSharp.text.Phrase();

                            frase.Add(new iTextSharp.text.Chunk("\n"));
                            parrafo.Add(frase);
                            document.Add(parrafo);

                            float _iLineNumber = 25f;
                            float _fCuerpoFontSize = 12;
                            float fDocumentHeight = 842;
                            int lineaMultiplo = 1;
                            float fLineaHeight = 12 + 1.5f;
                            float fDocumentPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                            float FCuerpoLeading = (fDocumentPosition - (100)) / (_fCuerpoFontSize * _iLineNumber);

                            int iNumLineas = (int)Math.Truncate((decimal)((writer.GetVerticalPosition(false) - fMargenInferiorDoc) / (FCuerpoLeading * _fCuerpoFontSize)));
                            for (int y = 0; y < iNumLineas - 1; y++)
                            {
                                document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n", fontPagina)));
                            }
                            //------------------------------
                            #endregion
                        }                      
                    }
                    Array.Resize(ref lPaginas, intIndice);
                    document.Close();
                    

                    #region Impresion en Navegador

                    Byte[] FileBuffer = ms.ToArray();
                    if (FileBuffer != null)
                    {
                        FileBuffer = EnumerarPDF(FileBuffer, lPaginas).ToArray();
                        System.Web.HttpContext.Current.Session["binaryData"] = FileBuffer;

                        string strUrl = "../Accesorios/VisorPDF.aspx";
                        string strScript = "window.open('" + strUrl + "', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";
                        Comun.EjecutarScript(page, strScript);
                    }
                    
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //------------------------------------------------------------------
        // Fecha: 25/06/2021
        // Autor: Miguel Márquez Beltrán
        // Motivo: Enumerar documento PDF de acuerdo a la lista de paginas.
        //-------------------------------------------------------------------

        public static MemoryStream EnumerarPDF(Byte[] dataPDF, int[] lPaginas)
        {
            MemoryStream ms = null;
            iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Fonts/cour.ttf"), iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);

            using (ms = new MemoryStream())
            {
                iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(dataPDF);
                iTextSharp.text.pdf.PdfStamper pdfStamper = new iTextSharp.text.pdf.PdfStamper(reader, ms);
                iTextSharp.text.pdf.BaseFont bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);
                iTextSharp.text.Rectangle rect = reader.GetPageSizeWithRotation(1);


                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    iTextSharp.text.pdf.PdfContentByte pdfContentByte = pdfStamper.GetOverContent(i);

                    pdfContentByte.BeginText();
                    pdfContentByte.SetFontAndSize(baseFont, 10);
                    pdfContentByte.SetTextMatrix(rect.Width - 105, rect.Height - 50);

                    pdfContentByte.ShowText("FOJA: " + lPaginas[i-1].ToString());

                    pdfContentByte.EndText();
                }
                pdfStamper.Close();
            }
            return ms;
        }

        //------------------------------------------------------------------
        // Fecha: 07/12/2021
        // Autor: Miguel Márquez Beltrán
        // Motivo: Poner marca de agua a las paginas del PDF
        //-------------------------------------------------------------------

        public static MemoryStream PonerMarcaAguaPDF(Byte[] dataPDF, string strMarcaAgua, float fontsize=90, float x=340, float y=380, float rotacion=45)
        {
            MemoryStream ms = null;
            iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Fonts/cour.ttf"), iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);

            using (ms = new MemoryStream())
            {
                iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(dataPDF);
                iTextSharp.text.pdf.PdfStamper pdfStamper = new iTextSharp.text.pdf.PdfStamper(reader, ms);

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    iTextSharp.text.pdf.PdfContentByte pdfContentByte = pdfStamper.GetOverContent(i);

                    pdfContentByte.BeginText();
                    pdfContentByte.SetFontAndSize(baseFont, fontsize);
                    pdfContentByte.SetColorFill(iTextSharp.text.pdf.CMYKColor.LIGHT_GRAY);
                    pdfContentByte.ShowTextAligned(iTextSharp.text.Element.ALIGN_CENTER, strMarcaAgua, x, y, rotacion);

                    pdfContentByte.EndText();
                }
                pdfStamper.Close();
            }
            return ms;
        }


        //-----------------------------------------------------------
    }
}
