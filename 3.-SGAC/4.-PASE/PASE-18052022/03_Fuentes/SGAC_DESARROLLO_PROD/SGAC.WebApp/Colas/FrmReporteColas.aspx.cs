using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using Microsoft.Reporting.WebForms;
using SGAC.Accesorios;
using SGAC.Controlador;
using SGAC.WebApp.Accesorios;
using SGAC.Cliente.Colas.BL;

namespace SGAC.WebApp.Colas
{
    public partial class FrmReporteColas : MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SeteraControles2();

            if (!Page.IsPostBack)
            {
                try
                {
                    CargarDatosIniciales();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        void SeteraControles2()
        {
            ctrlToolBarConsulta.btnPrintHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonPrintClick(ctrlToolBarConsulta_btnPrintHandler);
            ctrlToolBarConsulta.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarConsulta_btnCancelarHandler);
            ctrlToolBarConsulta.VisibleIButtonNuevo = false;
            ctrlToolBarConsulta.VisibleIButtonEditar = false;
            ctrlToolBarConsulta.VisibleIButtonGrabar = false;
            ctrlToolBarConsulta.VisibleIButtonCancelar = true;
            ctrlToolBarConsulta.VisibleIButtonBuscar = false;
            ctrlToolBarConsulta.VisibleIButtonPrint = true;
            ctrlToolBarConsulta.VisibleIButtonEliminar = false;
            ctrlToolBarConsulta.VisibleIButtonConfiguration = false;
            ctrlToolBarConsulta.VisibleIButtonSalir = false;

            ctrlToolBarConsulta.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBarConsulta.btnCancelar.Text = "    Limpiar";
        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            ctrlOficinaConsular.SelectedIndex = -1;
            dtpFecInicio.Text = DateTime.Today.ToString("MMM-01-yyyy");
            dtpFecFin.Text = Comun.ObtenerFechaActualTexto(Session);
            ctrlOficinaConsular.SelectedValue = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString();
            ddlTipoReporte.SelectedIndex = -1;
        }

        private void CargarDatosIniciales()
        {
            try
            {
                ctrlOficinaConsular.Cargar(false, false);

                dtpFecInicio.Text = DateTime.Today.ToString("MMM-01-yyyy");
                dtpFecFin.Text = Comun.ObtenerFechaActualTexto(Session);

                Util.CargarParametroDropDownList(ddlTipoReporte, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.COLAS_REPORTES), true);
                ctrlOficinaConsular.SelectedValue = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ddlTipoReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        public static void EjecutarScript(Page Page, string strScript)
        {
            strScript = string.Format(strScript);
            ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), "OpenPopup", strScript, true);
        }

        //---

        private void Imprimir_rsSeguimientoControl(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                Proceso p = new Proceso();
                DataTable dt = new DataTable();

                ReportParameter[] parameters = new ReportParameter[3];
                parameters = parametros("SEGUIMIENTO Y CONTROL");

                object[] arrParametros = { idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]) };

                VentanillaConsultaBL objBL = new VentanillaConsultaBL();
                dt = objBL.ImprimeSeguimientoControl(idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]));


                Session["strNombreArchivo"] = "rsSeguimientoControl.rdlc";
                Session["DtDatos"] = dt;
                Session["objParametroReportes"] = parameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Imprimir_rsAtencionxTipoAtencion(int idOficinaConsular, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                //Proceso p = new Proceso();
                DataTable dt = new DataTable();

                ReportParameter[] parameters = new ReportParameter[3];
                parameters = parametros("NÚMERO DE ATENCIONES POR TIPO DE ATENCIONES");

                //object[] arrParametros = { idOficinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]) };

                //dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.CL_VENTANILLA", "IMPR_ATENCIONXTIPOATENCION");

                VentanillaConsultaBL objVentanillaConsultaBL = new VentanillaConsultaBL();

                dt = objVentanillaConsultaBL.ImprimeAtencionesxTipoAtencion(idOficinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]));


                Session["strNombreArchivo"] = "rsAtencionxTipoAtencion.rdlc";
                Session["DtDatos"] = dt;
                Session["objParametroReportes"] = parameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Imprimir_rsEvaluacionAtencionxTipoAtencion(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                //Proceso p = new Proceso();
                DataTable dt = new DataTable();

                ReportParameter[] parameters = new ReportParameter[3];
                parameters = parametros("EVALUACIÓN DE TIEMPO DE ATENCIÓN POR TIPO DE ATENCIÓN");

                //object[] arrParametros = { idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]) };

                //dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.CL_VENTANILLA", "IMPR_EVALUACIONTIEMPOATENCION");

                VentanillaConsultaBL objVentanillaConsultaBL = new VentanillaConsultaBL();
                dt = objVentanillaConsultaBL.ImprimeEvaluacionAtencionxTipoAtencion(idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]));


                Session["strNombreArchivo"] = "rsEvaluacionAtencionxTipoAtencion.rdlc";
                Session["DtDatos"] = dt;
                Session["objParametroReportes"] = parameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Imprimir_rsProductividadOperadores(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                //Proceso p = new Proceso();
                DataTable dt = new DataTable();

                ReportParameter[] parameters = new ReportParameter[3];
                parameters = parametros("PRODUCTIVIDAD DE OPERADORES");

                //object[] arrParametros = { idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]) };

                //dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.CL_VENTANILLA", "IMPR_PRODUCTIVIDADOPERADORES");

                VentanillaConsultaBL objVentanillaConsultaBL = new VentanillaConsultaBL();
                dt = objVentanillaConsultaBL.ImprimeProductividadxOperador(idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]));


                Session["strNombreArchivo"] = "rsProductividadOperadores.rdlc";
                Session["DtDatos"] = dt;
                Session["objParametroReportes"] = parameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Imprimir_rsRendimientoProcesos(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                //Proceso p = new Proceso();
                DataTable dt = new DataTable();

                ReportParameter[] parameters = new ReportParameter[3];
                parameters = parametros("INDICADORES DE RENDIMIENTO DE LOS PROCESOS");

                //object[] arrParametros = { idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]) };

                //dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.CL_VENTANILLA", "IMPR_RENDIMIENTOPROCESOS");

                VentanillaConsultaBL objVentanillaConsultaBL = new VentanillaConsultaBL();
                dt = objVentanillaConsultaBL.ImprimerRendimientoProcesos(idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]));


                Session["strNombreArchivo"] = "rsRendimientoProcesos.rdlc";
                Session["DtDatos"] = dt;
                Session["objParametroReportes"] = parameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Imprimir_rsTicketsEmitidos(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                Proceso p = new Proceso();
                DataTable dt = new DataTable();

                ReportParameter[] parameters = new ReportParameter[3];
                parameters = parametros("LISTA DE TICKETS EMITIDOS");

                object[] arrParametros = { idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]) };

                VentanillaConsultaBL objBL = new VentanillaConsultaBL();
                dt = objBL.ImprimeTicketsEmitidos(idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]));

                Session["strNombreArchivo"] = "rsListaTicketsEmitidos.rdlc";
                Session["DtDatos"] = dt;
                Session["objParametroReportes"] = parameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Imprimir_rsTicketsAtendidosNoAtendidos(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                //Proceso p = new Proceso();
                DataTable dt = new DataTable();

                ReportParameter[] parameters = new ReportParameter[3];
                parameters = parametros("TICKETS ATENDIDOS Y NO ATENDIDOS");

                //object[] arrParametros = { idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]) };

                //dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.CL_VENTANILLA", "IMPR_TICKETSATENDIDO");

                VentanillaConsultaBL objVentanillaConsultaBL = new VentanillaConsultaBL();
                dt = objVentanillaConsultaBL.ImprimeTicketsAtendidosNoAtendidos(idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]));


                Session["strNombreArchivo"] = "rsTicketsAtendidosNoAtendidos.rdlc";
                Session["DtDatos"] = dt;
                Session["objParametroReportes"] = parameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Imprimir_rsTiempoAtencionxTransaccion(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                //Proceso p = new Proceso();
                DataTable dt = new DataTable();

                ReportParameter[] parameters = new ReportParameter[3];
                parameters = parametros("TIEMPO DE ATENCIÓN POR TRANSACCIÓN, MÁXIMO, MINIMO Y PROMEDIO");

                //object[] arrParametros = { idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]) };

                //dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.CL_VENTANILLA", "IMPR_ATENCIONXTRANSACCION");

                VentanillaConsultaBL objVentanillaConsultaBL = new VentanillaConsultaBL();
                dt = objVentanillaConsultaBL.ImprimeTiempoAtencionxTransaccion(idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]));

                Session["strNombreArchivo"] = "rsTiempoAtencionxTransaccion.rdlc";
                Session["DtDatos"] = dt;
                Session["objParametroReportes"] = parameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Imprimir_rsTiempoEsperaxCliente(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                //Proceso p = new Proceso();
                DataTable dt = new DataTable();

                ReportParameter[] parameters = new ReportParameter[3];
                parameters = parametros("TIEMPO DE ESPERA POR CLIENTE");

                //object[] arrParametros = { idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]) };

                //dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.CL_VENTANILLA", "IMPR_TIEMPOESPERACLIENTE");

                VentanillaConsultaBL objVentanillaConsultaBL = new VentanillaConsultaBL();
                dt = objVentanillaConsultaBL.ImprimeTiempoEsperaxCliente(idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]));


                Session["strNombreArchivo"] = "rsTiempoEsperaxCliente.rdlc";
                Session["DtDatos"] = dt;
                Session["objParametroReportes"] = parameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Imprimir_AtencionesporUsusario(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                //Proceso p = new Proceso();
                DataTable dt = new DataTable();

                ReportParameter[] parameters = new ReportParameter[3];
                parameters = parametros("NÚMERO DE ATENCIONES POR RECURRENTE");

                //object[] arrParametros = { idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]) };

                //dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.CL_VENTANILLA", "IMPR_ATENCIONXUSUARIO");

                VentanillaConsultaBL objVentanillaConsultaBL = new VentanillaConsultaBL();
                dt = objVentanillaConsultaBL.ImprimeAtencionesxUsusario(idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]));


                Session["strNombreArchivo"] = "rsAtencionesUsuario.rdlc";
                Session["DtDatos"] = dt;
                Session["objParametroReportes"] = parameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Imprimir_AtencionesporVentanilla(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                //Proceso p = new Proceso();
                DataTable dt = new DataTable();

                ReportParameter[] parameters = new ReportParameter[3];
                parameters = parametros("NÚMERO DE ATENCIÓN POR VENTANILLA");

                //object[] arrParametros = { idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]) };

                //dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.CL_VENTANILLA", "IMPR_ATENCIONXVENTANILLA");

                VentanillaConsultaBL objVentanillaConsultaBL = new VentanillaConsultaBL();
                dt = objVentanillaConsultaBL.ImprimeAtencionxVentanilla(idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]));


                Session["strNombreArchivo"] = "rsAfluenciaVentanillaAtendidas.rdlc";
                Session["DtDatos"] = dt;
                Session["objParametroReportes"] = parameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Imprimir_rsAfluenciaVentanilla(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                //Proceso p = new Proceso();
                DataTable dt = new DataTable();

                ReportParameter[] parameters = new ReportParameter[3];
                parameters = parametros("AFLUENCIA POR VENTANILLA");

                //object[] arrParametros = { idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]) };

                //dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.CL_VENTANILLA", "IMPR_AFLUVENTANILLA");

                VentanillaConsultaBL objVentanillaConsultaBL = new VentanillaConsultaBL();
                dt = objVentanillaConsultaBL.ImprimeAfluenciaVentanilla(idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]));


                Session["strNombreArchivo"] = "rsAfluenciaVentanilla.rdlc";
                Session["DtDatos"] = dt;
                Session["objParametroReportes"] = parameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Imprimir_rsAfluenciaClientes(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                //Proceso p = new Proceso();
                DataTable dt = new DataTable();

                ReportParameter[] parameters = new ReportParameter[3];
                parameters = parametros("AFLUENCIA DE CONNACIONALES");

                //object[] arrParametros = { idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]) };

                //dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.CL_VENTANILLA", "IMPR_AFLUCLIENTE");

                VentanillaConsultaBL objVentanillaConsultaBL = new VentanillaConsultaBL();
                dt = objVentanillaConsultaBL.ImprimeAfluenciaClientes(idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]));


                Session["strNombreArchivo"] = "rsAfluenciaClientes.rdlc";
                Session["DtDatos"] = dt;
                Session["objParametroReportes"] = parameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Imprimir_rsAgenciaRemota(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                //Proceso p = new Proceso();
                DataTable dt = new DataTable();

                ReportParameter[] parameters = new ReportParameter[3];
                parameters = parametros("AGENCIAS REMOTAS");

                //object[] arrParametros = { idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]) };

                //dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.CL_VENTANILLA", "IMPR_AGENREMOTA");

                VentanillaConsultaBL objVentanillaConsultaBL = new VentanillaConsultaBL();
                dt = objVentanillaConsultaBL.ImprimeAgenciaRemota(idOfinaConsular, fechaInicio, fechaFin, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]));


                Session["strNombreArchivo"] = "rsAgenciaRemota.rdlc";
                Session["DtDatos"] = dt;
                Session["objParametroReportes"] = parameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private ReportParameter[] parametros(string Titulo2)
        {
            try
            {
                DateTime FechaInicio = new DateTime();
                DateTime FechaFin = new DateTime();

                if (!DateTime.TryParse(dtpFecInicio.Text, out FechaInicio))
                {
                    FechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
                }
                if (!DateTime.TryParse(dtpFecFin.Text, out FechaFin))
                {
                    FechaFin = Comun.FormatearFecha(dtpFecFin.Text);
                }

                ReportParameter[] parameters = new ReportParameter[6];
                parameters[0] = new ReportParameter("Titulo1", "SERVICIO CONSULAR DEL PERÚ");
                parameters[1] = new ReportParameter("Titulo2", Titulo2);
                parameters[2] = new ReportParameter("Usuario", "Usuario Impresión : " + Session[Constantes.CONST_SESION_USUARIO].ToString());
                parameters[3] = new ReportParameter("fechaImpresion", DateTime.Now.ToString("MMM-dd-yyyy"));
                parameters[4] = new ReportParameter("fecha","Del "+FechaInicio.ToString("MMM-dd-yyyy") + " Al " + FechaFin.ToString("MMM-dd-yyyy"));
                parameters[5]=new ReportParameter("OficinaConsular", ctrlOficinaConsular.SelectedItem.Text);

                return parameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ctrlToolBarConsulta_btnPrintHandler()
        {
            if (dtpFecInicio.Text.Trim().Length == 0 || dtpFecFin.Text.Trim().Length == 0)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_VACIA, true, Enumerador.enmTipoMensaje.WARNING);
                return;
            }

            if (Comun.EsFecha(dtpFecInicio.Text.Trim()) == false)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_INICIAL, true, Enumerador.enmTipoMensaje.WARNING);
                return;
            }
            if (Comun.EsFecha(dtpFecFin.Text.Trim()) == false)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_FINAL, true, Enumerador.enmTipoMensaje.WARNING);
                return;
            }


            try
            {
                DateTime FechaInicio = new DateTime();
                DateTime FechaFin = new DateTime();

                if (!DateTime.TryParse(dtpFecInicio.Text, out FechaInicio))
                {
                    FechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
                }
                if (!DateTime.TryParse(dtpFecFin.Text, out FechaFin))
                {
                    FechaFin = Comun.FormatearFecha(dtpFecFin.Text);
                }

                if (FechaInicio > FechaFin)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);
                    return;
                }


                if (ddlTipoReporte.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "Ingrese un tipo de Reporte" + "');", true);
                    return;
                }

                int reporte = Convert.ToInt16(ddlTipoReporte.SelectedValue);

                int idConsular = Convert.ToInt16(ctrlOficinaConsular.SelectedValue);

                switch (reporte)
                {
                    case 7751:
                        Imprimir_rsAgenciaRemota(idConsular, FechaInicio, FechaFin);
                        break;
                    case 7752:
                        Imprimir_rsAfluenciaClientes(idConsular, FechaInicio, FechaFin);
                        break;
                    case 7753:
                        Imprimir_rsAfluenciaVentanilla(idConsular, FechaInicio, FechaFin);
                        break;
                    case 7754:
                        Imprimir_AtencionesporVentanilla(idConsular, FechaInicio, FechaFin);
                        break;
                    case 7755:
                        Imprimir_AtencionesporUsusario(idConsular, FechaInicio, FechaFin);
                        break;
                    case 7756:
                        Imprimir_rsAtencionxTipoAtencion(idConsular, FechaInicio, FechaFin);
                        break;
                    case 7757:
                        Imprimir_rsEvaluacionAtencionxTipoAtencion(idConsular, FechaInicio, FechaFin);
                        break;
                    case 7758:
                        Imprimir_rsProductividadOperadores(idConsular, FechaInicio, FechaFin);
                        break;
                    case 7759:
                        Imprimir_rsRendimientoProcesos(idConsular, FechaInicio, FechaFin);
                        break;
                    case 7760:
                        Imprimir_rsTicketsAtendidosNoAtendidos(idConsular, FechaInicio, FechaFin);
                        break;
                    case 7761:
                        Imprimir_rsTiempoAtencionxTransaccion(idConsular, FechaInicio, FechaFin);
                        break;
                    case 7762:
                        Imprimir_rsTiempoEsperaxCliente(idConsular, FechaInicio, FechaFin);
                        break;
                    case 7763:
                        Imprimir_rsTicketsEmitidos(idConsular, FechaInicio, FechaFin);
                        break;
                    case 7764:
                        Imprimir_rsSeguimientoControl(idConsular, FechaInicio, FechaFin);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            string strUrl = "../Colas/frmVisorColas.aspx";
            string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=1200,height=700,left=100,top=0');";
            EjecutarScript(Page, strScript);
        }

    }
}