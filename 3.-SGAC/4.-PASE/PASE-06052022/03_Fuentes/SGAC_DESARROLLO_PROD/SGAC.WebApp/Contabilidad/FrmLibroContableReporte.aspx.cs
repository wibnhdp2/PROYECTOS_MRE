using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.Controlador;
using SGAC.WebApp.Accesorios;
using SGAC.Contabilidad.Reportes.BL;
using SGAC.Configuracion.Sistema.BL;
using SGAC.Registro.Actuacion.BL;
using SGAC.Contabilidad.CuentaCorriente.BL;
using System.IO;
using System.Linq;
//-------------------------
using System.Text;
using System.Web.Configuration;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

using System.Net;

namespace SGAC.WebApp.Contabilidad
{
    public partial class LibroContableReporte : MyBasePage
    {
        #region CAMPOS
        //private string strNombreEntidad = "REPORTE CONTABLE";
        private string strVariableDt = "ReporteContable_Tabla";
        private string strVariableIndice = "ReporteContable_Indice";
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            lblUserName.Text = SGAC.Accesorios.Util.GetSessionVariableValue(Session, Constantes.CONST_SESION_USUARIO);
            ctrlOficinaConsular.ddlOficinaConsular.AutoPostBack = true;
            ctrlOficinaConsular.ddlOficinaConsular.SelectedIndexChanged += new EventHandler(ddlOficinaConsular_SelectedIndexChanged);
            //ctrlToolBarConsulta.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBarConsulta_btnBuscarHandler);            
            //ctrlToolBarConsulta.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarConsulta_btnCancelarHandler);
            //ctrlToolBarConsulta.btnImprimir.OnClientClick = "return abrirPopupEspera();";
            btnImprimir.OnClientClick = "return abrirPopupEspera();";

            //Comun.CargarPermisos(Session, ctrlToolBarConsulta, null, gdvReporte, HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath);
            if (!Page.IsPostBack)
            {
                llenarOficinasActivas();
                CargarListadosDesplegables();
                CargarDatosIniciales();

                this.dtpFecInicio.StartDate = new DateTime(1900, 1, 1);
                this.dtpFecInicio.EndDate = DateTime.Now;

                this.dtpFecFin.StartDate = new DateTime(1900, 1, 1);
                ddlTipoPago.Visible = false;
                lblTipoPago.Visible = false;
                //------------------------------------------------------------------------
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 28/12/2016
                // Objetivo: Tipo de Reporte RENIEC
                //------------------------------------------------------------------------
                lblTipoReporteReniec.Visible = false;
                ddlTipoReporteReniec.Visible = false;
                //------------------------------------------------------------------------
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 18/08/2016
                // Objetivo: Calcular los dias habiles para el cierre de cuenta
                // Referencia: Requerimiento No.001_3.doc
                //------------------------------------------------------------------------
                CalcularDiasCierreCuenta();
                chkFechaActuacion.Visible = false;
                chkFechaAnulacion.Visible = false;
                //------------------------------------------------------------------------
                // Autor: Sandra del Carmen Acosta Celis
                // Fecha: 02/12/2016
                // Objetivo: Deshabilitar la visibilidad del campo Periodo, así como 
                //           los dropdownlists de Mes y Año.
                //------------------------------------------------------------------------
                lblAnioMes.Visible = false;
                ddlAnio.Visible = false;
                ddlMes.Visible = false;
                //--------------------------------------------------------------------------------
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 28/12/2016
                // Objetivo: Deshabilitar la visibilidad del control Tarifa,
                //          estado de la ficha y Nro. Guía
                //---------------------------------------------------------------------------------
                lblTipoTarifa.Visible=false;
                ddlTipoTarifaReniec.Visible=false;

                lblEstadoFichaRegistral.Visible=false;
                ddlEstadoFichaRegistral.Visible = false;
                lblGuiaDespacho.Visible = false;
                ddlGuiaDespacho.Visible = false;
                chkTodos.Visible = false;
                //------------------------------------------------------------------------

                ddlTipoReporteRCivil.Visible = false;
                lblTipoReporteRegistroCivil.Visible = false;
                btnDownload.Visible = false;
                ValidacionReporteReniec();
                ddlOficinaConsular_SelectedIndexChanged(sender, e);
                btnExportarPDF.Visible = false;
                //------------------------------------------------------
                //Fecha: 12/05/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Exportar e Imprimir el tipo de reporte:
                //        Rendición de Cuentas por Tarifas RENIEC.
                //Documento: OBSERVACIONES_SGAC_12052021.doc
                //------------------------------------------------------
                btnExportar.Visible = false;
            }
        }
        void ddlOficinaConsular_SelectedIndexChanged(object sender, EventArgs e)
        {
            //------------------------------------------------------------------------
            // Autor: Jonatan Silva Cachay
            // Fecha: 20/06/2017
            // Objetivo: Se agrega la carga de bancos
            //------------------------------------------------------------------------
            TransaccionConsultasBL objTransaccionBL = new TransaccionConsultasBL();
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                DataTable dt = objTransaccionBL.ObtenerBancoCuenta(Convert.ToInt32(ctrlOficinaConsular.ddlOficinaConsular.SelectedValue));
                SGAC.Accesorios.Util.CargarDropDownList(ddlBancoConsulta, dt, "descripcion", "id", true);
            }
            else
            {
                DataTable dt = objTransaccionBL.ObtenerBancoCuenta(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                SGAC.Accesorios.Util.CargarDropDownList(ddlBancoConsulta, dt, "descripcion", "id", true);
            }
        }
        private void ValidacionReporteReniec()
        {
            string valor = Convert.ToString(Request.QueryString["Rep"]);
            if (valor == "RENIEC")
            {
                lblTituloLibroContableRpt.Text = "Reportes: RENIEC";
                ddlLibroContableTipo.SelectedValue = ddlLibroContableTipo.Items.FindByText("RENIEC").Value;
                ddlLibroContableTipo.Enabled = false;
                lblTipoReporteReniec.Visible = true;
                ddlTipoReporteReniec.Visible = true;
                ddlAnio.Visible = true;
                ddlMes.Visible = true;
                lblAnioMes.Visible = true;
                ddlAnio.Enabled = true;
                ddlMes.Enabled = true;
                lblGuiaDespacho.Visible = true;
                ddlGuiaDespacho.Visible = true;
                chkTodos.Visible = true;
                lblFecInicio.Visible = false;
                dtpFecInicio.Visible = false;
                lblFechaFin.Visible = false;
                dtpFecFin.Visible = false;
                ddlTipoReporteRCivil.Visible = false;
                lblTipoReporteRegistroCivil.Visible = false;
                //--------------------------------------------------------
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 05/12/2016
                // Objetivo: Asignar valores por defecto a los controles
                //--------------------------------------------------------
                ddlAnio.SelectedValue = DateTime.Now.Year.ToString();
                ddlMes.SelectedIndex = DateTime.Now.Month - 1;
                //--------------------------------------------------------

                SGAC.Accesorios.Util.CargarParametroDropDownList(ddlTipoReporteReniec, comun_Part1.ObtenerParametrosPorGrupo(Session, SGAC.Accesorios.Constantes.CONST_REPORTES_RENIEC), true);

                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Constantes.CONST_OFICINACONSULAR_LIMA)
                {
                    System.Web.UI.WebControls.ListItem itemToRemove = ddlTipoReporteReniec.Items.FindByText("REPORTE DE CONCILIACIÓN DE RENIEC");

                    if (itemToRemove != null)
                    {
                        ddlTipoReporteReniec.Items.Remove(itemToRemove);
                    }
                }
                
                lblMsjDiasHabilesCierreCuenta.Visible = false;
            }
            else if (valor == "REGISTROCIVIL")
            {
                for (int i = 0; i < ddlLibroContableTipo.Items.Count; i++)
                {
                   ddlLibroContableTipo.Items[i].Enabled = false;
                }
                //int iIndiceComboFormatos = Util.ObtenerIndiceComboPorText(ddlLibroContableTipo, "FORMATOS REGISTRO CIVIL");
                int iIndiceComboReportes = SGAC.Accesorios.Util.ObtenerIndiceComboPorText(ddlLibroContableTipo, "REPORTES REGISTRO CIVIL");

                if (iIndiceComboReportes>=0)
                {
                    //ddlLibroContableTipo.Items[iIndiceComboFormatos].Enabled = true;
                    ddlLibroContableTipo.Items[iIndiceComboReportes].Enabled = true;
                }

                lblTituloLibroContableRpt.Text = "Reportes: REGISTRO CIVIL";
                lblFecInicio.Visible = true;
                dtpFecInicio.Visible = true;
                lblFechaFin.Visible = true;
                dtpFecFin.Visible = true;
                ddlTipoReporteRCivil.Visible = true;
                lblTipoReporteRegistroCivil.Visible = true;
                //ddlLibroContableTipo.Visible = false;
                ddlLibroContableTipo.SelectedValue = ddlLibroContableTipo.Items.FindByText("REPORTES REGISTRO CIVIL").Value;
                lblTipoLibro.Visible = true;
                SGAC.Accesorios.Util.CargarParametroDropDownList(ddlTipoReporteRCivil, comun_Part1.ObtenerParametrosPorGrupo(Session, SGAC.Accesorios.Constantes.CONST_REPORTES_REGISTRO_CIVIL), true);
                lblMsjDiasHabilesCierreCuenta.Visible = false;
            }
            else
            {
                //int iIndiceComboFormatos = Util.ObtenerIndiceComboPorText(ddlLibroContableTipo, "FORMATOS REGISTRO CIVIL");
                int iIndiceComboReportes = SGAC.Accesorios.Util.ObtenerIndiceComboPorText(ddlLibroContableTipo, "REPORTES REGISTRO CIVIL");
                int iIndiceComboReniec = SGAC.Accesorios.Util.ObtenerIndiceComboPorText(ddlLibroContableTipo, "RENIEC");

                if ( iIndiceComboReportes >= 0 && iIndiceComboReniec >=0)
                {
                    //ddlLibroContableTipo.Items[iIndiceComboFormatos].Enabled = false;
                    ddlLibroContableTipo.Items[iIndiceComboReportes].Enabled = false;
                    ddlLibroContableTipo.Items[iIndiceComboReniec].Enabled = false;
                }

                //for (int i = 0; i < ddlLibroContableTipo.Items.Count; i++)
                //{
                //    if (ddlLibroContableTipo.Items[i].Text == "RENIEC" || ddlLibroContableTipo.Items[i].Text == "FORMATOS REGISTRO CIVIL"
                //        || ddlLibroContableTipo.Items[i].Text == "REPORTES REGISTRO CIVIL")
                //    {
                //        ddlLibroContableTipo.Items.RemoveAt(i);
                //    }
                //}
            }
        }
        protected void ddlLibroContableTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            gdvReporte = SGAC.Accesorios.Util.BorrarGrillaColumnas(gdvReporte);
            Session[strVariableIndice] = ddlLibroContableTipo.SelectedIndex;

            //--------------------------------------------------------------------------------
            // Autor: Miguel Angel Márquez Beltrán
            // Fecha: 28/12/2016
            // Objetivo: Deshabilitar la visibilidad del control Tarifa, estado de la ficha
            //          y Nro. Guía
            //---------------------------------------------------------------------------------
            lblTipoTarifa.Visible = false;
            ddlTipoTarifaReniec.Visible = false;

            lblEstadoFichaRegistral.Visible = false;
            ddlEstadoFichaRegistral.Visible = false;
            lblGuiaDespacho.Visible = false;
            ddlGuiaDespacho.Visible = false;
            chkTodos.Visible = false;
            lblFecInicio.Visible = true;
            dtpFecInicio.Visible = true;
            lblFechaFin.Visible = true;
            dtpFecFin.Visible = true;
            btnDownload.Visible = false;
            //ctrlToolBarConsulta.btnImprimir.Visible = true;
            btnImprimir.Visible = true;
            btnExportarPDF.Visible = false;
            //------------------------------------------------------------------------
            // Autor: Miguel Angel Márquez Beltrán
            // Fecha: 13/10/2016
            // Objetivo: Adiciono la funcionalidad de filtrar por clase de fecha
            //           (por fecha de actuación o por fecha de anulación
            //------------------------------------------------------------------------

            if (ddlLibroContableTipo.SelectedItem.Text == "REPORTE DE ACTUACIONES ANULADAS")
            {
                chkFechaActuacion.Visible = true;
                chkFechaAnulacion.Visible = true;
            }
            else
            {
                chkFechaActuacion.Visible = false;
                chkFechaAnulacion.Visible = false;
            }
            //------------------------------------------------------------------------
            // Autor: Jonatan Silva Cachay
            // Fecha: 18/01/2017
            // Objetivo: Adiciono la funcionalidad de filtrar Banco y cuenta bancaria.
            //------------------------------------------------------------------------
            if (ddlLibroContableTipo.SelectedValue == Convert.ToString((int)Enumerador.enmLibroContable.SALDOS_CONSULARES))
            {
                DataTable DtOficCH = new DataTable();

                int IntTotalCount = 0;
                int IntTotalPages = 0;
                int intPaginaCantidad = 1000;
                int intOficinaConsularId;
                OficinaConsularConsultasBL BL = new OficinaConsularConsultasBL();

                DtOficCH = BL.ObtenerDependientes(Convert.ToInt16(ctrlOficinaConsular.ddlOficinaConsular.SelectedValue),
                                                 "1",
                                                 intPaginaCantidad,
                                                 ref IntTotalCount,
                                                 ref IntTotalPages);

                if (DtOficCH .Rows.Count > 0)
                {
                    Panel.Visible = true;
                }
                else { Panel.Visible = false; }
                

            }
            else { Panel.Visible = false; }
            //------------------------------------------------------------------------
            // Autor: Sandra del Carmen Acosta Celis
            // Fecha: 02/12/2016
            // Objetivo: Adiciono la funcionalidad de filtrar por Periodo de mes y Año.
            //------------------------------------------------------------------------
            if (ddlLibroContableTipo.SelectedValue == Convert.ToString((int)Enumerador.enmLibroContable.AUTOADHESIVO_CONSULAR))
            {
                lblAnioMes.Visible = true;
                ddlAnio.Visible = true;
                ddlMes.Visible = true;

                lblAnioMes.Enabled = true;
                ddlAnio.Enabled = true;
                ddlMes.Enabled = true;
                //-------------------------------
                lblFecInicio.Visible = false;
                dtpFecInicio.Visible = false;
                lblFechaFin.Visible = false;
                dtpFecFin.Visible = false;
                //--------------------------------------------------------
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 05/12/2016
                // Objetivo: Asignar valores por defecto a los controles
                //--------------------------------------------------------
                ddlAnio.SelectedValue = DateTime.Now.Year.ToString();
                ddlMes.SelectedIndex = DateTime.Now.Month - 1;
                //--------------------------------------------------------
                //Fecha: 12/03/2021
                asignarFechaInicioFin();
                //--------------------------------------------------------

                lblTipoReporteReniec.Visible = false;
                ddlTipoReporteReniec.Visible = false;
                ctrlValidacion.MostrarValidacion("", false);
            }
            else
            {
                if (ddlLibroContableTipo.SelectedValue == Convert.ToString((int)Enumerador.enmLibroContable.RENIEC))
                {
                    lblTipoReporteReniec.Visible = true;
                    ddlTipoReporteReniec.Visible = true;
                    ddlAnio.Visible = true;
                    ddlMes.Visible = true;
                    lblAnioMes.Visible = true;
                    ddlAnio.Enabled = true;
                    ddlMes.Enabled = true;
                    lblGuiaDespacho.Visible = true;
                    ddlGuiaDespacho.Visible = true;
                    chkTodos.Visible = true;
                    lblFecInicio.Visible = false;
                    dtpFecInicio.Visible = false;
                    lblFechaFin.Visible = false;
                    dtpFecFin.Visible = false;
                    //--------------------------------------------------------
                    // Autor: Miguel Angel Márquez Beltrán
                    // Fecha: 05/12/2016
                    // Objetivo: Asignar valores por defecto a los controles
                    //--------------------------------------------------------
                    ddlAnio.SelectedValue = DateTime.Now.Year.ToString();
                    ddlMes.SelectedIndex = DateTime.Now.Month - 1;
                    //--------------------------------------------------------

                    SGAC.Accesorios.Util.CargarParametroDropDownList(ddlTipoReporteReniec, comun_Part1.ObtenerParametrosPorGrupo(Session, SGAC.Accesorios.Constantes.CONST_REPORTES_RENIEC), true);
                }
                else
                {
                    lblTipoReporteReniec.Visible = false;
                    ddlTipoReporteReniec.Visible = false;

                       //------------------------------------------------
                       //Fecha: 04/05/2021
                       //Autor: Miguel Márquez Beltrán
                       //Motivo: Libro Caja se imprime por rango de fecha.
                       //------------------------------------------------
//                    if (ddlLibroContableTipo.SelectedItem.Text == "LIBRO CAJA" || ddlLibroContableTipo.SelectedItem.Text == "LIBRO SALDOS CONSULARES")

                    if (ddlLibroContableTipo.SelectedItem.Text == "LIBRO SALDOS CONSULARES")
                    {
                        //--------------------------------------------------
                        //Fecha: 12/03/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Mostrar Año y mes y ocultar 
                        //          Fecha de inicio y fecha fin.
                        //--------------------------------------------------
                        ddlAnio.Visible = true;
                        ddlMes.Visible = true;
                        lblAnioMes.Visible = true;
                        ddlAnio.Enabled = true;
                        ddlMes.Enabled = true;
                        lblFecInicio.Visible = false;
                        dtpFecInicio.Visible = false;
                        lblFechaFin.Visible = false;
                        dtpFecFin.Visible = false;
                        ddlAnio.SelectedValue = DateTime.Now.Year.ToString();
                        ddlMes.SelectedIndex = DateTime.Now.Month - 1;
                        asignarFechaInicioFin();
                        //--------------------------------------------------
                    }
                    else
                    {
                        lblAnioMes.Visible = false;
                        ddlAnio.Visible = false;
                        ddlMes.Visible = false;
                        lblFecInicio.Visible = true;
                        dtpFecInicio.Visible = true;
                        lblFechaFin.Visible = true;
                        dtpFecFin.Visible = true;
                    }

                    if (ddlLibroContableTipo.SelectedItem.Text == "REPORTES REGISTRO CIVIL")
                    {
                        SGAC.Accesorios.Util.CargarParametroDropDownList(ddlTipoReporteRCivil, comun_Part1.ObtenerParametrosPorGrupo(Session, SGAC.Accesorios.Constantes.CONST_REPORTES_REGISTRO_CIVIL), true);
                        btnDownload.Visible = false;
                    }
                    else if (ddlLibroContableTipo.SelectedItem.Text == "FORMATOS REGISTRO CIVIL")
                    {
                        SGAC.Accesorios.Util.CargarParametroDropDownList(ddlTipoReporteRCivil, comun_Part1.ObtenerParametrosPorGrupo(Session, SGAC.Accesorios.Constantes.CONST_FORMATO_REGISTRO_CIVIL), true);
                        lblFecInicio.Visible = false;
                        dtpFecInicio.Visible = false;
                        lblFechaFin.Visible = false;
                        dtpFecFin.Visible = false;
                        btnDownload.Visible = true;                        
                        btnImprimir.Visible = false;
                    }
                    else if (ddlLibroContableTipo.SelectedItem.Text == "REGISTRO GENERAL DE ENTRADAS")
                    {
                        btnImprimir.Visible = true;
                        if (Mostrar_btnExportarPDF())
                        {
                            btnExportarPDF.Visible = true;
                            btnImprimir.Text = "Imprimir";
                        }
                    }
                }
            }
        }

        protected void ddlTipoReporteReniec_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strTipoReporteReniec = ddlTipoReporteReniec.SelectedItem.Text.Trim();

            //--------------------------------------------------------------------------------
            // Autor: Miguel Angel Márquez Beltrán
            // Fecha: 28/12/2016
            // Objetivo: Deshabilitar la visibilidad del control Tarifa y estado de la ficha
            //---------------------------------------------------------------------------------
            lblTipoTarifa.Visible = false;
            ddlTipoTarifaReniec.Visible = false;

            lblEstadoFichaRegistral.Visible = false;
            ddlEstadoFichaRegistral.Visible = false;
            lblGuiaDespacho.Visible = true;
            ddlGuiaDespacho.Visible = true;
            chkTodos.Visible = true;
            if (strTipoReporteReniec != SGAC.Accesorios.Constantes.CONST_RENIEC_RENDICION_CUENTAS && strTipoReporteReniec != SGAC.Accesorios.Constantes.CONST_REPORTE_RENIEC_INCOMPLETOS
                && strTipoReporteReniec != SGAC.Accesorios.Constantes.CONST_RENIEC_CONCILIACION)
            {
                    string strMensaje = "<b>Condiciones:</b><br/>";
                    strMensaje += "- Guía de Despacho con estado: registrado.<br/>";
                    strMensaje += "- Formato de Envio por Tarifa con estado: registrado o reimpreso.<br/>";
                    strMensaje += "- Formato de Envio por Estado con estado: recuperado o reproceso.<br/>";
                    strMensaje += "- La Guía de Despacho es el consolidado de los formatos de envio.";
                    ctrlValidacion.MostrarValidacion(strMensaje, true, Enumerador.enmTipoMensaje.INFORMATION);
            }
            else
            {
                ctrlValidacion.MostrarValidacion("", false);
            }
            if (strTipoReporteReniec.Equals(SGAC.Accesorios.Constantes.CONST_RENIEC_FORMATO_ENVIO_TARIFA))
            {
                lblTipoTarifa.Visible = true;
                ddlTipoTarifaReniec.Visible = true;

                DataTable dtTarifa = new DataTable();
                //TarifarioConsultasBL objTarifarioBL = new TarifarioConsultasBL();

                dtTarifa = Comun.ObtenerTarifarioCargaInicial(Session);

                //if (Session[Constantes.CONST_SESION_DT_TARIFARIO] != null)
                if (dtTarifa != null)
                {
                    //dtTarifa = (DataTable)Session[Constantes.CONST_SESION_DT_TARIFARIO];

                    DataView dv = dtTarifa.DefaultView;
                    dv.RowFilter = " tari_sSeccionId = 8";
                    DataTable dtTarifaSeccion8 = dv.ToTable();

                    SGAC.Accesorios.Util.CargarDropDownList(ddlTipoTarifaReniec, dtTarifaSeccion8, "tari_vDescripcionCorta", "tari_sTarifarioId", true, "- TODOS -");
                }
                else
                {
                    SGAC.Accesorios.Util.CargarParametroDropDownList(ddlTipoTarifaReniec, new DataTable(), true);
                }
            }
            if (strTipoReporteReniec.Equals(SGAC.Accesorios.Constantes.CONST_RENIEC_FORMATO_ENVIO_ESTADO))
            {
                lblEstadoFichaRegistral.Visible = true;
                ddlEstadoFichaRegistral.Visible = true;

                DataTable dtEstado = new DataTable();

//                dtEstado = Comun.ObtenerParametrosPorGrupo((DataTable)Session[Constantes.CONST_SESION_DT_ESTADO], SGAC.Accesorios.Constantes.CONST_FICHA_ESTADO);
                dtEstado = comun_Part1.ObtenerParametrosPorGrupoMRE(SGAC.Accesorios.Constantes.CONST_FICHA_ESTADO);

                DataView dvEstados = dtEstado.DefaultView;
                dvEstados.RowFilter = " VALOR = 'RECUPERADO' OR VALOR = 'REPROCESO'";
                DataTable dtEstadoFiltrado = dvEstados.ToTable();

                SGAC.Accesorios.Util.CargarDropDownList(ddlEstadoFichaRegistral, dtEstadoFiltrado, "VALOR", "ID", true, "- TODOS -");
            }
            if (strTipoReporteReniec.Equals(SGAC.Accesorios.Constantes.CONST_RENIEC_RENDICION_CUENTAS))
            {
                lblGuiaDespacho.Visible = false;
                ddlGuiaDespacho.Visible = false;
                chkTodos.Visible = false;
                rbOrderFecha.Visible = true;
                rbOrderTarifa.Visible = true;
                //------------------------------------------------------
                //Fecha: 12/05/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Exportar e Imprimir el tipo de reporte:
                //        Rendición de Cuentas por Tarifas RENIEC.
                //Documento: OBSERVACIONES_SGAC_12052021.doc
                //------------------------------------------------------

                btnExportar.Visible = true;
            }
            else
            {
                rbOrderFecha.Visible = false;
                rbOrderTarifa.Visible = false;

                //------------------------------------------------------
                //Fecha: 12/05/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Exportar e Imprimir el tipo de reporte:
                //        Rendición de Cuentas por Tarifas RENIEC.
                //Documento: OBSERVACIONES_SGAC_12052021.doc
                //------------------------------------------------------
                btnExportar.Visible = false;
            }

            if (strTipoReporteReniec.Equals(SGAC.Accesorios.Constantes.CONST_REPORTE_RENIEC_INCOMPLETOS))
            {
                lblGuiaDespacho.Visible = false;
                ddlGuiaDespacho.Visible = false;
                chkTodos.Visible = false;
            }
            if (strTipoReporteReniec.Equals(SGAC.Accesorios.Constantes.CONST_RENIEC_CONCILIACION))
            {
                lblGuiaDespacho.Visible = false;
                ddlGuiaDespacho.Visible = false;
                chkTodos.Visible = false;
                ctrlOficinaConsular.Enabled = false;
                ctrlOficinaConsular.SelectedValue = Convert.ToInt16(Constantes.CONST_OFICINACONSULAR_LIMA).ToString();
            }
            else {
                ctrlOficinaConsular.Enabled = true;
            }
            
            CargarNumeroGuiaDespacho(0);
            chkTodos.Checked = false;            
        }


        //void ctrlToolBarConsulta_btnBuscarHandler()
        //{
        //    Session[strVariableDt] = new DataTable();
        //    gdvReporte.DataSource = new DataTable();
        //    gdvReporte.DataBind();

        //    if (dtpFecInicio.Text == string.Empty || dtpFecFin.Text == string.Empty)
        //    {
        //        ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_VACIA, true, Enumerador.enmTipoMensaje.ERROR);
        //        return;
        //    }

        //    DateTime datFechaInicio = new DateTime();
        //    DateTime datFechaFin = new DateTime();
        //    if (!DateTime.TryParse(dtpFecInicio.Text, out datFechaInicio))
        //    {
        //        datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
        //    }
        //    if (!DateTime.TryParse(dtpFecFin.Text, out datFechaFin))
        //    {
        //        datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
        //    }
        //    if (datFechaInicio > datFechaFin)
        //    {
        //        ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);
        //    }
        //    else
        //    {
        //        CargarGrilla();
        //    }
        //}
        private void DescargarArchivo(string carpeta, string NombreArchivo)
        {
            string FolderPath = ConfigurationManager.AppSettings["UploadPath"];
            string FilePath;
            if (carpeta != "")
            {
                FilePath = FolderPath + @"\" + carpeta + @"\" + NombreArchivo;
            }
            else {
                FilePath = FolderPath + @"\" + NombreArchivo;
            }


            // Limpiamos la salida
            Response.Clear();
            // Con esto le decimos al browser que la salida sera descargable
            Response.ContentType = "application/octet-stream";
            // esta linea es opcional, en donde podemos cambiar el nombre del fichero a descargar (para que sea diferente al original)
            Response.AddHeader("Content-Disposition", "attachment; filename=" + NombreArchivo);
            // Escribimos el fichero a enviar 
            Response.WriteFile(FilePath);
            // volcamos el stream 
            Response.Flush();
            // Enviamos todo el encabezado ahora
            Response.End();
        }

        //void ctrlToolBarConsulta_btnPrintHandler()
        //{
        //    //ctrlToolBarConsulta.btnImprimir.Enabled = false;
        //    btnImprimir.Enabled = false;
        //    if (ddlLibroContableTipo.SelectedItem.Text == "FORMATOS REGISTRO CIVIL")
        //    {
        //        if(ddlTipoReporteRCivil.SelectedIndex>0)
        //        {
        //            DataTable _dt = Comun.ObtenerParametroPorId(Session,Convert.ToInt32(ddlTipoReporteRCivil.SelectedValue));
        //            string strNombreArchivo;
        //            string strCarpeta = ConfigurationManager.AppSettings["CarpetaFormatosRegCivil"];

        //            if (_dt.Rows.Count > 0)
        //            {
        //                strNombreArchivo = _dt.Rows[0]["valor"].ToString();
        //                DescargarArchivo(strCarpeta, strNombreArchivo);
        //            }        
        //        }
        //        //ctrlToolBarConsulta.btnImprimir.Enabled = true;
        //        btnImprimir.Enabled = true;
        //        return;
        //    }

        //    //------------------------------------------------------------------------
        //    // Autor: Jonatan Silva Cachay
        //    // Fecha: 18/01/2017
        //    // Objetivo: Validación para reporte de saldos consulares.
        //    //------------------------------------------------------------------------
        //    if (ddlLibroContableTipo.SelectedValue == Convert.ToString((int)Enumerador.enmLibroContable.SALDOS_CONSULARES))
        //    {
        //        if (ctrlOficinaConsular.ddlOficinaConsular.Items.Count > 1)
        //        {
        //            if (ddlCuentaCorrienteConsulta.SelectedIndex == 0 || ddlCuentaCorrienteConsulta.SelectedIndex == -1)
        //            {
        //                ctrlValidacion.MostrarValidacion("Para visualizar el Reporte De Saldos Consulares debe seleccionar el nro. de cuenta", true, Enumerador.enmTipoMensaje.ERROR);
        //                //ctrlToolBarConsulta.btnImprimir.Enabled = true;
        //                btnImprimir.Enabled = true;
        //                return;
        //            }
        //        }
        //    }
        //    if (ddlLibroContableTipo.Visible == true)
        //    {
        //        if (ddlLibroContableTipo.SelectedIndex == 0)
        //        {
        //            ctrlValidacion.MostrarValidacion("Seleccion el tipo de libro contable", true, Enumerador.enmTipoMensaje.ERROR);
        //            //ctrlToolBarConsulta.btnImprimir.Enabled = true;
        //            btnImprimir.Enabled = true;
        //            return;
        //        }
        //    }
            
        //    if (ddlLibroContableTipo.SelectedValue == Convert.ToString((int)Enumerador.enmLibroContable.RENIEC))
        //    {
        //        if (ddlTipoReporteReniec.SelectedIndex == 0)
        //        {
        //            ctrlValidacion.MostrarValidacion("Seleccion el tipo de Reporte de RENIEC", true, Enumerador.enmTipoMensaje.ERROR);
        //            //ctrlToolBarConsulta.btnImprimir.Enabled = true;
        //            btnImprimir.Enabled = true;
        //            return;
        //        }

        //        string strTipoReporteReniec = ddlTipoReporteReniec.SelectedItem.Text.Trim();
        //        if (strTipoReporteReniec != SGAC.Accesorios.Constantes.CONST_RENIEC_RENDICION_CUENTAS && strTipoReporteReniec != SGAC.Accesorios.Constantes.CONST_REPORTE_RENIEC_INCOMPLETOS)
        //        {
        //            if (ddlGuiaDespacho.SelectedIndex == -1)
        //            {
        //                ctrlValidacion.MostrarValidacion("Seleccione el N° de Guía de Despacho", true, Enumerador.enmTipoMensaje.ERROR);
        //                //ctrlToolBarConsulta.btnImprimir.Enabled = true;
        //                btnImprimir.Enabled = true;
        //                return;
        //            }
        //        }
                

        //        if (strTipoReporteReniec.Equals(SGAC.Accesorios.Constantes.CONST_RENIEC_GUIA_DESPACHO))
        //        {
        //            //ctrlToolBarConsulta.btnImprimir.Enabled = true;
        //            btnImprimir.Enabled = true;
        //            Session["IdOficinaConsular_contabilidad"] = ctrlOficinaConsular.SelectedValue;
        //            mdlpopup.Show();
        //            return;
        //        }

        //        Session["FechaIntervalo"] = " Del Periodo: " + ddlAnio.SelectedItem.Text + "-" + ddlMes.SelectedItem.Text;
        //        Session["IdOficinaConsular_contabilidad"] = ctrlOficinaConsular.SelectedValue;
        //        VerVistaPrevia();
        //        CargarNumeroGuiaDespacho(0);
        //    }
        //    else
        //    {
        //        //REPORTE REGISTRO CIVIL
        //        if (ddlLibroContableTipo.SelectedItem.Text == "REPORTES REGISTRO CIVIL")
        //        {
        //            if (ddlTipoReporteRCivil.SelectedIndex == 0)
        //            {
        //                ctrlValidacion.MostrarValidacion("Seleccion el tipo de Reporte de REGISTRO CIVIL", true, Enumerador.enmTipoMensaje.ERROR);
        //                //ctrlToolBarConsulta.btnImprimir.Enabled = true;
        //                btnImprimir.Enabled = true;
        //                return;
        //            }

        //            string strTipoReporteRegistroCivil = ddlTipoReporteRCivil.SelectedItem.Text.Trim();
        //            Session["FechaIntervalo"] = " Del " + dtpFecInicio.Text + " al " + dtpFecFin.Text;
        //            Session["IdOficinaConsular_contabilidad"] = ctrlOficinaConsular.SelectedValue;
        //            VerVistaPrevia();
        //            //CargarNumeroGuiaDespacho(0);
        //        }
        //        else {
        //            if (ddlLibroContableTipo.SelectedValue != Convert.ToString((int)Enumerador.enmLibroContable.AUTOADHESIVO_CONSULAR))
        //            {


        //                if (dtpFecInicio.Text == string.Empty || dtpFecFin.Text == string.Empty)
        //                {
        //                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_VACIA, true, Enumerador.enmTipoMensaje.ERROR);
        //                    //ctrlToolBarConsulta.btnImprimir.Enabled = true;
        //                    btnImprimir.Enabled = true;
        //                    return;
        //                }

        //                DateTime datFechaInicio = new DateTime();
        //                DateTime datFechaFin = new DateTime();

        //                if (!DateTime.TryParse(dtpFecInicio.Text, out datFechaInicio))
        //                {
        //                    datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
        //                }
        //                if (!DateTime.TryParse(dtpFecFin.Text, out datFechaFin))
        //                {
        //                    datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
        //                }

        //                Session["FechaIntervalo"] = " Del " + dtpFecInicio.Text + " al " + dtpFecFin.Text;
        //                Session["IdOficinaConsular_contabilidad"] = ctrlOficinaConsular.SelectedValue;
        //                if (datFechaInicio > datFechaFin)
        //                {
        //                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);
        //                }
        //                else
        //                {
        //                    VerVistaPrevia();
        //                }
        //            }
        //            else
        //            {
        //                Session["FechaIntervalo"] = " Del Periodo: " + ddlAnio.SelectedItem.Text + "-" + ddlMes.SelectedItem.Text;
        //                Session["IdOficinaConsular_contabilidad"] = ctrlOficinaConsular.SelectedValue;
        //                VerVistaPrevia();
        //            }
        //        }
                
        //    }
        //    //ctrlToolBarConsulta.btnImprimir.Enabled = true;
        //    btnImprimir.Enabled = true;
        //    chkTodos.Checked = false;
        //}
       
        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            //
        }

        


        #endregion

        #region Métodos
        private void CargarDatosIniciales()
        {
            dtpFecInicio.Text = DateTime.Today.ToString("MMM-01-yyyy");
            dtpFecFin.Text = Comun.ObtenerFechaActualTexto(Session);

            ctrlOficinaConsular.SelectedValue = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString();
        }
        private void llenarOficinasActivas()
        {
            try
            {
                DataTable dtOficinasActivas = new DataTable();
                dtOficinasActivas = Comun.ObtenerOficinasActivas(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString()));
                grdOficinasActivasUniversal.DataSource = dtOficinasActivas;
                grdOficinasActivasUniversal.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private DataTable CrearDataTable()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("ofco_sOficinaConsularId", typeof(String));
            dt.Columns.Add("ofco_vCodigo", typeof(String));
            dt.Columns.Add("ofco_vNombre", typeof(String));
            dt.Columns.Add("ofco_iReferenciaPadreId", typeof(String));
            dt.Columns.Add("ofco_iJefaturaFlag", typeof(String));
            dt.Columns.Add("ofco_IRemesaLimaFlag", typeof(String));
            dt.Columns.Add("ofco_cUbigeoCodigo", typeof(String));
            dt.Columns.Add("ofco_cUbigeoCodigoPais", typeof(String));
            dt.Columns.Add("vPaisNombre", typeof(String));
            return dt;
        }
        private DataTable obtenerOficinasActivas()
        {
            DataTable dt = new DataTable();

            dt = CrearDataTable();
            string strcelda = "";

            for (int i = 0; i < grdOficinasActivasUniversal.Rows.Count; i++)
            {
                DataRow dr;
                GridViewRow row = grdOficinasActivasUniversal.Rows[i];
                dr = dt.NewRow();
                for (int x = 0; x < row.Cells.Count; x++)
                {
                    strcelda = HttpUtility.HtmlDecode(row.Cells[x].Text);

                    dr[x] = strcelda;
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }
        private void CargarListadosDesplegables()
        {
            // Lima - Carga todas las Misiones

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                //ctrlOficinaConsular.Cargar(true, false);
                DataTable _dt = new DataTable();

                _dt = obtenerOficinasActivas();

                //DataTable _dt = (DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULARACTIVAS];
                Util.CargarDropDownList(ctrlOficinaConsular.ddlOficinaConsular, _dt, "ofco_vNombre", "ofco_sOficinaConsularId", true, "- SELECCIONAR -");
            }
            else
            {
                ctrlOficinaConsular.Cargar(false, false);
            }

            SGAC.Accesorios.Util.CargarParametroDropDownList(ddlLibroContableTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONTA_TIPO_LIBRO), true);
            SGAC.Accesorios.Util.CargarParametroDropDownList(ddlTipoPago, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACREDITACION_TIPO_COBRO), true);
            //------------------------------------------------------------------------
            // Autor: Sandra del Carmen Acosta Celis
            // Fecha: 02/12/2016
            // Objetivo: Adiciono la funcionalidad de llenar las listas de mes y Año.
            //------------------------------------------------------------------------
            SGAC.Accesorios.Util.CargarComboAnios(ddlAnio, 2016, DateTime.Now.Year);
            DataTable dtMes = new DataTable();

            dtMes = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONFIG_MES);            

            SGAC.Accesorios.Util.CargarDropDownList(ddlMes, dtMes, "valor", "id");
            //------------------------------------------------------------------------
            // Autor: Jonatan Silva Cachay
            // Fecha: 18/01/2017
            // Objetivo: Se agrega la carga de bancos
            //------------------------------------------------------------------------
            TransaccionConsultasBL objTransaccionBL = new TransaccionConsultasBL();
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                DataTable dt = objTransaccionBL.ObtenerBancoCuenta(Convert.ToInt32(ctrlOficinaConsular.ddlOficinaConsular.SelectedValue));
                SGAC.Accesorios.Util.CargarDropDownList(ddlBancoConsulta, dt, "descripcion", "id", true);
            }
            else {
                DataTable dt = objTransaccionBL.ObtenerBancoCuenta(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                SGAC.Accesorios.Util.CargarDropDownList(ddlBancoConsulta, dt, "descripcion", "id", true);
            }
            
        }
        
        private void CargarGrilla()
        {
        }

        private object[] ObtenerFiltro(bool bolVerEnGrilla = true)
        {
            object[] arrParametros = new object[7];
            arrParametros[0] = Convert.ToInt32(ctrlOficinaConsular.SelectedValue);

            DateTime datFechaInicio = new DateTime();
            DateTime datFechaFin = new DateTime();

            datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
            datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);

            arrParametros[1] = datFechaInicio;
            arrParametros[2] = datFechaFin;

            // auditoría
            arrParametros[3] = SGAC.Accesorios.Util.ObtenerHostName();
            arrParametros[4] = Convert.ToInt32(Session[Constantes.CONST_SESION_USUARIO_ID]);
            arrParametros[5] = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
            arrParametros[6] = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            return arrParametros;
        }


       
        private void VerVistaPrevia()
        {
            // Limpiar Mensaje
            ctrlValidacion.MostrarValidacion("", false);

            DataSet ds = new DataSet();
            bool bolVistaPrevia = false;
            bool bolReporteSeleccionado = true;
            int sReferenciaIdLima = 0;
            Session["NumeroGuia"] = "";
            

            if (ddlLibroContableTipo.SelectedIndex > 0 && ddlLibroContableTipo.SelectedItem.Text != "REPORTES REGISTRO CIVIL")
            {
                // Información del reporte
                object[] arrParametros = ObtenerFiltro(false);
                Session["SP_PARAMETROS"] = arrParametros;

                // Indicadores
                bool bolEsLima = false;
                bool bolEsJefatura = false;

                // Verificar si Lima es la Misión Logeada
                #region Verificar si Lima es la Misión Logeada
                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
                    bolEsLima = true;

                if (bolEsLima)
                {
                    // 1. Verificar si Misión Seleccionada es Lima
                    if (Convert.ToInt32(ctrlOficinaConsular.SelectedValue) == Constantes.CONST_OFICINACONSULAR_LIMA)
                        bolEsLima = true;

                    // 2. Verificar si misión Seleccionada es Jefatura
                    // 2.1. Obtener Id Misión seleccionada
                    int intOficinaConsularSel = Convert.ToInt32(ctrlOficinaConsular.SelectedValue);
                    // 2.2. Obtener Oficinas Consulares
                    DataTable dtOficinasConsulares = new DataTable();

                    dtOficinasConsulares = Comun.ObtenerOficinasConsularesCargaInicial();

                    //dtOficinasConsulares = (DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULAR];
                    // 2.3. Obtener Datos de Misión Seleccionada
                    DataView dv = dtOficinasConsulares.DefaultView;
                    dv.RowFilter = "ofco_sOficinaConsularId = " + intOficinaConsularSel;
                    // 2.4. Indicador de Jefatura
                    DataTable dtMisionSel = dv.ToTable();

                    //-----------------------------------------------------------
                    //Fecha: 12/05/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Validar cuando el datatable este vacio.
                    //-----------------------------------------------------------
                    if (dtMisionSel.Rows.Count > 0)
                    {
                        bolEsJefatura = Convert.ToBoolean(Convert.ToInt32(dtMisionSel.Rows[0]["ofco_iJefaturaFlag"]));
                    }
                }
                else
                {
                    DataTable dtOficinaConsular = new DataTable();
                    dtOficinaConsular = Comun.ObtenerOficinaConsularPorId(Session);
                    //DataTable dtOficinaConsular = (DataTable)Session[Constantes.CONST_SESION_OFICINACONSULTA_DT];
                    bolEsJefatura = Convert.ToBoolean(Convert.ToInt32(dtOficinaConsular.Rows[0]["ofco_iJefaturaFlag"]));
                    sReferenciaIdLima = Convert.ToInt32(dtOficinaConsular.Rows[0]["ofco_iReferenciaPadreId"]);
                }
                #endregion
                switch (Convert.ToInt32(ddlLibroContableTipo.SelectedValue))
                {
                    case (int)Enumerador.enmLibroContable.SALDOS_CONSULARES:
                        if (bolEsLima)
                        {
                            // Verificar Jefatura
                            // 1. Obtener Id Misión seleccionada
                            int intOficinaConsularSel = Convert.ToInt32(ctrlOficinaConsular.SelectedValue);
                            // 2. Obtener Oficinas Consulares
                            DataTable dtOficinasConsulares = new DataTable();
                            dtOficinasConsulares = Comun.ObtenerOficinasConsularesCargaInicial();

                            //dtOficinasConsulares = (DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULAR];
                            // 3. Obtener Datos de Misión Seleccionada
                            DataView dv = dtOficinasConsulares.DefaultView;
                            dv.RowFilter = "ofco_sOficinaConsularId = " + intOficinaConsularSel;
                            // 4. Indicador de Jefatura
                            DataTable dtMisionSel = dv.ToTable();
                            bolEsJefatura = Convert.ToBoolean(Convert.ToInt32(dtMisionSel.Rows[0]["ofco_iJefaturaFlag"]));
                        }

                        if (bolEsJefatura)
                        {
                            #region Obtener Datos Reporte
                            ds = ObtenerDataTableReporte(arrParametros);
                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                                    if (intCantidadRegistros > 0)
                                    {
                                        if (ds.Tables[0].Rows[0]["Resultado"].ToString() == "-99")
                                        {
                                            ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA, true, Enumerador.enmTipoMensaje.INFORMATION);
                                        }
                                        else {
                                            Session["dtDatos"] = ds.Tables[0];
                                            Session["dtDatos1"] = ds.Tables[1];
                                            Session["dtDatos2"] = ds.Tables[3]; // TOP 1
                                            Session["dtDatos3"] = ds.Tables[2]; // Sumatorias de totales - Moneda Extranjera
                                            bolVistaPrevia = true;
                                            Comun.VerVistaPrevia(Session, Page, ds, Enumerador.enmReporteContable.SALDOS_CONSULARES);
                                        }
                                        
                                    }
                                }
                            }
                            if (!bolVistaPrevia)
                            {
                                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA, true, Enumerador.enmTipoMensaje.INFORMATION);
                            }
                            #endregion
                        }
                        else
                        {
                            ctrlValidacion.MostrarValidacion("Misión Consular Seleccionada (" + ctrlOficinaConsular.SelectedItem.ToString() +
                                                            ") no es Jefatura, no genera Libro de Saldos Consulares.", true, Enumerador.enmTipoMensaje.WARNING);
                        }
                        break;
                    case (int)Enumerador.enmLibroContable.AUTOADHESIVO_CONSULAR:
                        #region Obtener Datos Reporte

                        
                        //ds = ObtenerDataTableReporte(arrParametros);
                        Int16 intOficinaConsularId = Convert.ToInt16(ctrlOficinaConsular.SelectedValue);
                        Int16 intAnio = Convert.ToInt16(ddlAnio.Text);
                        Int16 intMes = Convert.ToInt16(ddlMes.SelectedIndex + 1);
                        Int16 intIDResultado = 0;
                        string strMensaje = "";

                        ReporteConsultasBL oReportesConsultasBL = new ReporteConsultasBL();

                        ds = oReportesConsultasBL.ObtenerReporteLibroAutoadhesivo(intOficinaConsularId, intAnio, intMes, ref intIDResultado, ref strMensaje);

                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                                if (intCantidadRegistros > 0)
                                {
                                    Session["dtDatos"] = ds.Tables[0];
                                    Comun.VerVistaPrevia(Session, Page, ds, Enumerador.enmReporteContable.AUTOADHESIVO);
                                    bolVistaPrevia = true;
                                }
                            }
                        }
                        if (!bolVistaPrevia)
                        {
                            ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA, true, Enumerador.enmTipoMensaje.INFORMATION);
                        }
                        #endregion
                        break;
                    case (int)Enumerador.enmLibroContable.LIBRO_CAJA:
                        #region Obtener Datos Reporte
                        ds = ObtenerDataTableReporte(arrParametros);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                                if (intCantidadRegistros > 0)
                                {
                                    Session["dtDatos"] = ds.Tables[0];
                                    Session["dtDatos1"] = ds.Tables[1];
                                    Comun.VerVistaPrevia(Session, Page, ds, Enumerador.enmReporteContable.CAJA);
                                    bolVistaPrevia = true;
                                }
                            }
                        }
                        if (!bolVistaPrevia)
                        {
                            ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA, true, Enumerador.enmTipoMensaje.INFORMATION);
                        }
                        #endregion
                        break;
                    case (int)Enumerador.enmLibroContable.DOCUMENTO_UNICO:
                        if (!bolEsJefatura &&
                            Convert.ToInt32((ctrlOficinaConsular.SelectedValue)) != Constantes.CONST_OFICINACONSULAR_LIMA)
                        {
                            #region Obtener Datos Reporte
                            ds = ObtenerDataTableReporte(arrParametros);
                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                                    if (intCantidadRegistros > 0)
                                    {
                                        Session["dtDatos"] = ds.Tables[0];
                                        Comun.VerVistaPrevia(Session, Page, ds, Enumerador.enmReporteContable.DOCUMENTO_UNICO);
                                        bolVistaPrevia = true;
                                    }
                                }
                            }
                            if (!bolVistaPrevia)
                            {
                                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA, true, Enumerador.enmTipoMensaje.INFORMATION);
                            }
                            #endregion
                        }
                        else
                        {
                            ctrlValidacion.MostrarValidacion("Misión Consular Seleccionada (" + ctrlOficinaConsular.SelectedItem.ToString() +
                                                    ") no genera Documento Único, registra más de 50 actuaciones al mes.", true, Enumerador.enmTipoMensaje.WARNING);
                        }
                        break;

                    case (int)Enumerador.enmLibroContable.REGISTRO_GENERAL_ENTRADAS:
                        #region Obtener Datos Reporte
                        if (Mostrar_btnExportarPDF())
                        {
                            List<csReporteRGE> lstReporte = ObtenerListaReporte(arrParametros);
                            if (lstReporte.Count > 0)
                            {
                                int intCantidadRegistros = lstReporte.Count;
                                Session["dtDatos"] = lstReporte;
                                Comun.VerVistaPreviaRGE(Session, Page, lstReporte, Enumerador.enmReporteContable.REGISTRO_GENERAL_ENTRADAS);
                                bolVistaPrevia = true;
                            }
                        }
                        else {
                            ds = ObtenerDataTableReporte(arrParametros);
                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                                    if (intCantidadRegistros > 0)
                                    {
                                        Session["dtDatos"] = ds.Tables[0];
                                        Comun.VerVistaPrevia(Session, Page, ds, Enumerador.enmReporteContable.REGISTRO_GENERAL_ENTRADAS);
                                        bolVistaPrevia = true;
                                    }
                                }
                            }
                        }
                        
                        if (!bolVistaPrevia)
                        {
                            ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA, true, Enumerador.enmTipoMensaje.INFORMATION);
                        }
                        #endregion
                        break;
                    //------------------------------------------------------------------------
                    // Autor: Miguel Angel Márquez Beltrán
                    // Fecha: 05/09/2016
                    // Objetivo: Parametros para el listado de actuaciones anuladas
                    //------------------------------------------------------------------------

                    case (int)Enumerador.enmLibroContable.REPORTE_ACTUACIONES_ANULADAS:
                        #region Obtener Datos Reporte
                        
                        ds = ObtenerDataTableReporte(arrParametros);                        

                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                                if (intCantidadRegistros > 0)
                                {
                                    Session["dtDatos"] = ds.Tables[0];
                                    Comun.VerVistaPrevia(Session, Page, ds, Enumerador.enmReporteContable.REPORTE_ACTUACIONES_ANULADAS);
                                    bolVistaPrevia = true;
                                }
                            }
                        }
                        if (!bolVistaPrevia)
                        {
                            ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA, true, Enumerador.enmTipoMensaje.INFORMATION);
                        }
                        #endregion
                        break;

                    //-----------------------------------------------------------------------------------
                    // Autor: Miguel Angel Márquez Beltrán
                    // Fecha: 21/11/2016
                    // Objetivo: Parametros para el listado de rendición de cuentas por tarifas RENIEC.
                    //-----------------------------------------------------------------------------------

                    case (int)Enumerador.enmLibroContable.RENIEC:
                    #region Obtener Datos Reporte

                        string strTipoReporteReniec = ddlTipoReporteReniec.SelectedItem.Text.Trim();

                        ds = ObtenerDataTableReporte(arrParametros);

                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                                if (intCantidadRegistros > 0)
                                {
                                    Session["dtDatos"] = ds.Tables[0];
                                    if (strTipoReporteReniec != SGAC.Accesorios.Constantes.CONST_RENIEC_RENDICION_CUENTAS && strTipoReporteReniec != SGAC.Accesorios.Constantes.CONST_REPORTE_RENIEC_INCOMPLETOS
                                        && strTipoReporteReniec != SGAC.Accesorios.Constantes.CONST_RENIEC_CONCILIACION)
                                    {
                                        Session["NumeroGuia"] = ddlGuiaDespacho.SelectedItem.Text;
                                    }
                                    else
                                    {
                                        Session["NumeroGuia"] = "";
                                    }
                                    if (strTipoReporteReniec.Equals(SGAC.Accesorios.Constantes.CONST_RENIEC_FORMATO_ENVIO_TARIFA))
                                    {                                        
                                        Comun.VerVistaPrevia(Session, Page, ds, Enumerador.enmReporteContable.RENIEC_FORMATO_ENVIO_TARIFA);
                                    }
                                    if (strTipoReporteReniec.Equals(SGAC.Accesorios.Constantes.CONST_RENIEC_FORMATO_ENVIO_ESTADO))
                                    {                                        
                                        Comun.VerVistaPrevia(Session, Page, ds, Enumerador.enmReporteContable.RENIEC_FORMATO_ENVIO_ESTADO);
                                    }
                                    if (strTipoReporteReniec.Equals(SGAC.Accesorios.Constantes.CONST_RENIEC_GUIA_DESPACHO))
                                    {
                                        if (chkTodos.Checked == false)
                                        {
                                            GuiaDespachoBL objGuiaDespachoBL = new GuiaDespachoBL();

                                            SGAC.BE.MRE.RE_GUIADESPACHO objGuiaDespachoBE = new BE.MRE.RE_GUIADESPACHO();

                                            objGuiaDespachoBE.gude_iGuiaDespachoId = Convert.ToInt64(ddlGuiaDespacho.SelectedValue);
                                            objGuiaDespachoBE.gude_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                                            objGuiaDespachoBE.gude_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                                            objGuiaDespachoBE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                                            objGuiaDespachoBL.actualizarEnviado(objGuiaDespachoBE);
                                            string strScript = string.Empty;
                                            if (!(objGuiaDespachoBL.isError))
                                            {
                                                Comun.VerVistaPrevia(Session, Page, ds, Enumerador.enmReporteContable.RENIEC_GUIA_DESPAHO);
                                            }
                                            else
                                            {
                                                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Guía de Despacho - Enviado", Constantes.CONST_MENSAJE_OPERACION_FALLIDA);
                                                Comun.EjecutarScript(Page, strScript);
                                                return;
                                            }
                                        }
                                        else {
                                            Comun.VerVistaPrevia(Session, Page, ds, Enumerador.enmReporteContable.RENIEC_GUIA_DESPAHO);
                                        }
                                    }
                                    if (strTipoReporteReniec.Equals(SGAC.Accesorios.Constantes.CONST_RENIEC_RENDICION_CUENTAS))
                                    {
                                        Comun.VerVistaPrevia(Session, Page, ds, Enumerador.enmReporteContable.RENIEC_RENDICION_CUENTAS);
                                    }
                                    if (strTipoReporteReniec.Equals(SGAC.Accesorios.Constantes.CONST_REPORTE_RENIEC_INCOMPLETOS))
                                    {
                                        Comun.VerVistaPrevia(Session, Page, ds, Enumerador.enmReporteContable.CONST_REPORTE_RENIEC_INCOMPLETOS);
                                    }
                                    if (strTipoReporteReniec.Equals(SGAC.Accesorios.Constantes.CONST_RENIEC_CONCILIACION))
                                    {
                                        Comun.VerVistaPrevia(Session, Page, ds, Enumerador.enmReporteContable.REPORTE_RENIEC_CONCILIACION);
                                    }
                                    bolVistaPrevia = true;
                                }
                            }
                        }                                                                                               
                        
                        if (!bolVistaPrevia)
                        {
                            ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA, true, Enumerador.enmTipoMensaje.INFORMATION);
                        }
                        #endregion
                        break;
                }

                
                
            }
            else if (ddlLibroContableTipo.SelectedItem.Text == "REPORTES REGISTRO CIVIL")
            {
                switch (ddlTipoReporteRCivil.SelectedItem.Text)
                {
                    case Constantes.CONST_REPORTE_ACTA_NACIMIENTO:
                        //-----------------------------------------------------------------------------------
                        // Autor: Jonatan Silva Cachay
                        // Fecha: 23/04/2018
                        // Objetivo: REPORTE ACTA DE NACIMIENTO
                        //-----------------------------------------------------------------------------------
                        #region Obtener Datos Reporte
                        object[] arrParametros = ObtenerFiltro(false);
                        Session["SP_PARAMETROS"] = arrParametros;
                        //string strTipoReporteReniec = ddlTipoReporteReniec.SelectedItem.Text.Trim();

                        ds = ObtenerDataTableReporte(arrParametros);

                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                                if (intCantidadRegistros > 0)
                                {
                                    Session["dtDatos"] = ds.Tables[0];
                                    Comun.VerVistaPrevia(Session, Page, ds, Enumerador.enmReporteContable.REGCIVIL_ACTA_NACIMIENTO);
                                    bolVistaPrevia = true;
                                }
                            }
                        }

                        if (!bolVistaPrevia)
                        {
                            ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA, true, Enumerador.enmTipoMensaje.INFORMATION);
                        }
                        #endregion
                        break;                        
                }
            
            }
            else
            {
                bolReporteSeleccionado = false;
            }

            if (!bolReporteSeleccionado)
            {
                ctrlValidacion.MostrarValidacion("Seleccione Libro Contable", true, Enumerador.enmTipoMensaje.WARNING);
            }
        }
        
        private void CrearReporte(int intReporteId, DataTable dt)
        {
            gdvReporte = SGAC.Accesorios.Util.BorrarGrillaColumnas(gdvReporte);
            BoundField col1;
            BoundField col2;
            BoundField col3;
            BoundField col4;
            BoundField col5;
            BoundField col6;
            BoundField col7;
            BoundField col8;
            BoundField col9;
            BoundField col10;
            BoundField col11;
            BoundField col12;

            switch (intReporteId)
            {
                case (int)Enumerador.enmLibroContable.REGISTRO_GENERAL_ENTRADAS:
                    #region Registro General de Entradas
                    col1 = new BoundField();
                    col2 = new BoundField();
                    col3 = new BoundField();
                    col4 = new BoundField();
                    col5 = new BoundField();
                    col6 = new BoundField();
                    col7 = new BoundField();
                    col8 = new BoundField();
                    col9 = new BoundField();
                    col10 = new BoundField();
                    col11 = new BoundField();
                    col12 = new BoundField();

                    col1.HeaderText = "N° Orden Correlativo";
                    col1.DataField = "iNumeroOrden";
                    col1.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col1.ItemStyle.Width = 40;

                    col2.HeaderText = "Fecha Actuac.";
                    col2.DataField = "dFecha";
                    col2.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col2.ItemStyle.Width = 40;

                    col3.HeaderText = "NATURALEZA ACTO";
                    col3.DataField = "vTarifaDesc";
                    col3.ItemStyle.Width = 100;

                    col4.HeaderText = "APELLIDOS";
                    col4.DataField = "vApellidos";
                    col4.ItemStyle.Width = 80;

                    col5.HeaderText = "NOMBRES";
                    col5.DataField = "vNombres";
                    col5.ItemStyle.Width = 80;

                    col6.HeaderText = "Autoadhesivo Consular";
                    col6.DataField = "vAutoadhesivoCod";
                    col6.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col6.ItemStyle.Width = 60;

                    col7.HeaderText = "N° Tarifa";
                    col7.DataField = "vTarifaNro";
                    col7.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col7.ItemStyle.Width = 40;

                    col8.HeaderText = "Nro. Orden Actuac.";
                    col8.DataField = "iNumeroActuacion";
                    col8.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col8.ItemStyle.Width = 40;

                    col9.HeaderText = "Moneda Extranjera";
                    col9.DataField = "FMonedaExtranjera";
                    col9.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    col9.ItemStyle.Width = 50;

                    col10.HeaderText = "Soles consulares";
                    col10.DataField = "FSolesConsular";
                    col10.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    col10.ItemStyle.Width = 50;

                    col11.HeaderText = "T.C. Consular";
                    col11.DataField = "FValorTCConsular";
                    col11.ItemStyle.Width = 50;
                    col11.ItemStyle.HorizontalAlign = HorizontalAlign.Right;

                    col12.HeaderText = "Observaciones";
                    col12.DataField = "vObservacion";
                    col12.ItemStyle.Width = 80;

                    gdvReporte.Columns.Add(col1);
                    gdvReporte.Columns.Add(col2);
                    gdvReporte.Columns.Add(col3);
                    gdvReporte.Columns.Add(col4);
                    gdvReporte.Columns.Add(col5);
                    gdvReporte.Columns.Add(col6);
                    gdvReporte.Columns.Add(col7);
                    gdvReporte.Columns.Add(col8);
                    gdvReporte.Columns.Add(col9);
                    gdvReporte.Columns.Add(col10);
                    gdvReporte.Columns.Add(col11);
                    gdvReporte.Columns.Add(col12);

                    #endregion
                    break;
                case (int)Enumerador.enmLibroContable.SALDOS_CONSULARES:
                    #region Saldos Consulares

                    #endregion
                    break;
                case (int)Enumerador.enmLibroContable.AUTOADHESIVO_CONSULAR:
                    #region Autoadhesivo Consular
                    col1 = new BoundField();
                    col2 = new BoundField();
                    col3 = new BoundField();
                    col4 = new BoundField();

                    col1.HeaderText = "Fecha";
                    col1.DataField = "movi_dFechaRegistro";
                    col1.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col1.ItemStyle.Width = 40;

                    col2.HeaderText = "Tipo";
                    col2.DataField = "movi_vMovimientoTipo";
                    col2.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    col2.ItemStyle.Width = 150;

                    col3.HeaderText = "MOTIVO";
                    col3.DataField = "movi_vMovimientoMotivo";
                    col3.ItemStyle.Width = 150;

                    col4.HeaderText = "CANTIDAD";
                    col4.DataField = "mode_ICantidad";
                    col4.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    col4.ItemStyle.Width = 150;

                    gdvReporte.Columns.Add(col1);
                    gdvReporte.Columns.Add(col2);
                    gdvReporte.Columns.Add(col3);
                    gdvReporte.Columns.Add(col4);
                    #endregion
                    break;
                case (int)Enumerador.enmLibroContable.LIBRO_CAJA:
                    #region Libro Caja
                    col1 = new BoundField();
                    col2 = new BoundField();
                    col3 = new BoundField();
                    col4 = new BoundField();
                    col5 = new BoundField();
                    col6 = new BoundField();
                    col7 = new BoundField();

                    col1.HeaderText = "Sección";
                    col1.DataField = "vSeccionDesc";
                    col1.ItemStyle.Width = 120;

                    col2.HeaderText = "N° Actuac.";
                    //col2.DataField = "";
                    //col2.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col2.ItemStyle.Width = 40;

                    col3.HeaderText = "N° Tarifa";
                    col3.DataField = "vTarifaNro";
                    col3.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col3.ItemStyle.Width = 40;

                    col4.HeaderText = "ASIENTOS SEGUN Art. 183°";
                    col4.DataField = "vTarifaDesc";
                    col4.ItemStyle.Width = 180;

                    col5.HeaderText = "Tipo Cambio Consular";
                    col5.DataField = "FTipoCambioConsular";
                    col5.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    col5.ItemStyle.Width = 40;

                    col6.HeaderText = "Moneda Extranjera";
                    col6.DataField = "FMonedaExtranjera";
                    col6.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    col6.ItemStyle.Width = 40;

                    col7.HeaderText = "Soles Consulares US$";
                    col7.DataField = "FSolesConsulares";
                    col7.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    col7.ItemStyle.Width = 40;

                    gdvReporte.Columns.Add(col1);
                    gdvReporte.Columns.Add(col2);
                    gdvReporte.Columns.Add(col3);
                    gdvReporte.Columns.Add(col4);
                    gdvReporte.Columns.Add(col5);
                    gdvReporte.Columns.Add(col6);
                    gdvReporte.Columns.Add(col7);
                    #endregion
                    break;
                case (int)Enumerador.enmLibroContable.DOCUMENTO_UNICO:
                    #region Documento Unico
                    col1 = new BoundField();
                    col2 = new BoundField();
                    col3 = new BoundField();
                    col4 = new BoundField();
                    col5 = new BoundField();
                    col6 = new BoundField();
                    col7 = new BoundField();
                    col8 = new BoundField();

                    col1.HeaderText = "Nro. Orden";
                    col1.DataField = "iNumeroOrden";
                    col1.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col1.ItemStyle.Width = 50;

                    col2.HeaderText = "Fecha Actuac.";
                    col2.DataField = "dFecha";
                    col2.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col2.ItemStyle.Width = 60;

                    col3.HeaderText = "Nombre del interesado";
                    col3.DataField = "vRecurrente";
                    col3.ItemStyle.Width = 200;

                    //vAutoadhesivoCod

                    col4.HeaderText = "Naturaleza del acto";
                    col4.DataField = "vTarifaDesc";
                    col4.ItemStyle.Width = 200;

                    col5.HeaderText = "Nro. Tarifa";
                    col5.DataField = "vTarifaNro";
                    col5.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col5.ItemStyle.Width = 50;

                    col6.HeaderText = "Nro. Actuación";
                    col6.DataField = "iNumeroActuacion";
                    col6.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col6.ItemStyle.Width = 50;

                    col7.HeaderText = "Moneda Extranjera";
                    col7.DataField = "FMonedaExtranjera";
                    col7.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    col7.ItemStyle.Width = 50;

                    col8.HeaderText = "Soles Consulares";
                    col8.DataField = "FSolesConsular";
                    col8.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    col8.ItemStyle.Width = 50;

                    gdvReporte.Columns.Add(col1);
                    gdvReporte.Columns.Add(col2);
                    gdvReporte.Columns.Add(col3);
                    gdvReporte.Columns.Add(col4);
                    gdvReporte.Columns.Add(col5);
                    gdvReporte.Columns.Add(col6);
                    gdvReporte.Columns.Add(col7);
                    gdvReporte.Columns.Add(col8);
                    #endregion
                    break; 
            }
            gdvReporte.DataBind();
        }

        private List<csReporteRGE> ObtenerListaReporte(object[] arrParametros)
        {
            int intLibroContable = Convert.ToInt32(ddlLibroContableTipo.SelectedValue);
            string valor = Convert.ToString(Request.QueryString["Rep"]);
            Session["SP_PARAMETROS"] = arrParametros;
            ReporteConsultasBL oBL = new ReporteConsultasBL();
            List<csReporteRGE> lstReporte = null;
            DateTime datFechaInicio = new DateTime();
            DateTime datFechaFin = new DateTime();
            int sUsuarioId = 0;
            string strDireccionIP = "";
            int intOficinaConsularLogeo = 0;
            switch (intLibroContable)
            {
                case (int)Enumerador.enmLibroContable.REGISTRO_GENERAL_ENTRADAS:
                    int sOficinaConsular = Convert.ToInt32(ctrlOficinaConsular.SelectedValue);
                    datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
                    datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
                    sUsuarioId = Convert.ToInt32(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    strDireccionIP = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    intOficinaConsularLogeo = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                    ReporteConsultasBL _objReporteRGE = new ReporteConsultasBL();
                    lstReporte = _objReporteRGE.ObtenerReporteRegistroGeneralEntradasTimeOut_Reader(sOficinaConsular, datFechaInicio, datFechaFin, SGAC.Accesorios.Util.ObtenerHostName(), sUsuarioId, strDireccionIP, intOficinaConsularLogeo);
                    lstReporte = lstReporte.OrderByDescending(p => p.iNumero).ToList(); 
                    break;
            }
            return lstReporte;
        }
        private DataSet ObtenerDataTableReporte(object[] arrParametros)
        {
          //  Proceso p = new Proceso();
            DataSet ds = new DataSet();

            int intLibroContable = Convert.ToInt32(ddlLibroContableTipo.SelectedValue);
            string valor = Convert.ToString(Request.QueryString["Rep"]);
            Session["SP_PARAMETROS"] = arrParametros;
            ReporteConsultasBL oBL = new ReporteConsultasBL();

            DateTime datFechaInicio = new DateTime();
            DateTime datFechaFin = new DateTime();
            int sUsuarioId = 0;
            string strDireccionIP = "";
            int intOficinaConsularLogeo = 0;

            switch (intLibroContable)
            {
                case (int)Enumerador.enmLibroContable.SALDOS_CONSULARES:
                    ReporteConsultasBL _obj = new ReporteConsultasBL();
                    
                    datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
                    datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);

                    if (Panel.Visible)
                    {
                        ds = _obj.ObtenerReporteSaldosConsularesTimeuot(Convert.ToInt32(ctrlOficinaConsular.SelectedValue),
                                        datFechaInicio, datFechaFin, SGAC.Accesorios.Util.ObtenerHostName(), Convert.ToInt32(Session[Constantes.CONST_SESION_USUARIO_ID]),
                                        Session[Constantes.CONST_SESION_DIRECCION_IP].ToString(), Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                                        Convert.ToInt32(ddlCuentaCorrienteConsulta.SelectedValue));
                    }
                    else {

                        ds = _obj.ObtenerReporteSaldosConsularesTimeuot(Convert.ToInt32(ctrlOficinaConsular.SelectedValue),
                                        datFechaInicio, datFechaFin, SGAC.Accesorios.Util.ObtenerHostName(), Convert.ToInt32(Session[Constantes.CONST_SESION_USUARIO_ID]),
                                        Session[Constantes.CONST_SESION_DIRECCION_IP].ToString(), Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                                        0);
                    }
                    

                    //ds = (DataSet)p.Invocar(ref arrParametros, "SGAC.BE.CO_REPORTE", "SALDOS");
                    //ReporteConsultasBL objReporteBL = new ReporteConsultasBL();

                    //ds = objReporteBL.ObtenerReporteRegistroGeneralEntradas(Convert.toIn
                    break;
                case (int)Enumerador.enmLibroContable.AUTOADHESIVO_CONSULAR:
                    //ds = (DataSet)p.Invocar(ref arrParametros, "SGAC.BE.CO_REPORTE", "AUTOADHESIVO");

                    ReporteConsultasBL oReportesContablesBL = new ReporteConsultasBL();
                    Int16 intOficinaConsularId = Convert.ToInt16(ctrlOficinaConsular.SelectedValue);


                    break;
                case (int)Enumerador.enmLibroContable.LIBRO_CAJA:

                    //ds = (DataSet)p.Invocar(ref arrParametros, "SGAC.BE.CO_REPORTE", "CAJA");
                    ReporteConsultasBL obj = new ReporteConsultasBL();
                    datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
                    datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
                    ds = obj.ObtenerReporteLibroCaja(Convert.ToInt32(ctrlOficinaConsular.SelectedValue),
                        datFechaInicio, datFechaFin, SGAC.Accesorios.Util.ObtenerHostName(), Convert.ToInt32(Session[Constantes.CONST_SESION_USUARIO_ID]),
                        Session[Constantes.CONST_SESION_DIRECCION_IP].ToString(), Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                    break;
                case (int)Enumerador.enmLibroContable.DOCUMENTO_UNICO:
                    //ds = (DataSet)p.Invocar(ref arrParametros, "SGAC.BE.CO_REPORTE", "DOCUNICO");

                    SGAC.Contabilidad.Reportes.BL.ReporteConsultasBL objReporteConsultaBL = new ReporteConsultasBL();

                    ds = objReporteConsultaBL.ObtenerReporteDocumentoUnico(Convert.ToInt16(ctrlOficinaConsular.SelectedValue), Comun.FormatearFecha(dtpFecInicio.Text), Comun.FormatearFecha(dtpFecFin.Text),
                                                                            SGAC.Accesorios.Util.ObtenerHostName(), Convert.ToInt32(Session[Constantes.CONST_SESION_USUARIO_ID]),
                                                                            Session[Constantes.CONST_SESION_DIRECCION_IP].ToString(), Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));



                    break;
                case (int)Enumerador.enmLibroContable.REGISTRO_GENERAL_ENTRADAS:
                    int sOficinaConsular = Convert.ToInt32(ctrlOficinaConsular.SelectedValue);
                    datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
                    datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
                    sUsuarioId = Convert.ToInt32(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    strDireccionIP =  Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    intOficinaConsularLogeo = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                    ReporteConsultasBL _objReporteRGE = new ReporteConsultasBL();
                    ds = _objReporteRGE.ObtenerReporteRegistroGeneralEntradasTimeOut(sOficinaConsular, datFechaInicio, datFechaFin, SGAC.Accesorios.Util.ObtenerHostName(), sUsuarioId, strDireccionIP, intOficinaConsularLogeo);
                    break;
                case (int)Enumerador.enmLibroContable.REPORTE_ACTUACIONES_ANULADAS:

                    ReporteConsultasBL oReportesConsultasBL = new ReporteConsultasBL();
                    int sOficinaConsularId = Convert.ToInt32(ctrlOficinaConsular.SelectedValue);
                    //DateTime datFechaInicio = new DateTime();
                    //DateTime datFechaFin = new DateTime();
                    datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
                    datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
                    sUsuarioId = Convert.ToInt32(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    strDireccionIP =  Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    intOficinaConsularLogeo = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    string strClaseFecha = "";
                    if (chkFechaActuacion.Checked == true)
                    {
                        strClaseFecha = "R";
                    }
                    else
                    {
                        strClaseFecha = "A";
                    }

                    ds = oReportesConsultasBL.ObtenerReporteActuacionesAnuladas(sOficinaConsularId, datFechaInicio, datFechaFin, sUsuarioId, strDireccionIP, strClaseFecha); 
                                        
                    break;

                case (int)Enumerador.enmLibroContable.RENIEC:

                        string strTipoReporteReniec = ddlTipoReporteReniec.SelectedItem.Text.Trim();

                        if (strTipoReporteReniec.Equals(SGAC.Accesorios.Constantes.CONST_RENIEC_FORMATO_ENVIO_TARIFA))
                        {
                            string strTarifaId = ddlTipoTarifaReniec.SelectedValue;
                            Int16 intTarifarioId = Convert.ToInt16(strTarifaId);

                            Int16 intEstadoId = -1;

                            ReporteConsultasBL oReportesGuiaDespachoBL = new ReporteConsultasBL();

                            Int16 sOficinaConsularIdRC = Convert.ToInt16(ctrlOficinaConsular.SelectedValue);
                            string strNumeroGuia = ddlGuiaDespacho.SelectedItem.Text;

                            string strMes = (ddlMes.SelectedIndex + 1).ToString("00");
                            string strAnioMes = ddlAnio.SelectedValue + strMes;

                            Session["FechaIntervalo"] = SGAC.Accesorios.Util.ObtenerMesLargo(strMes).ToUpper() + " " + ddlAnio.SelectedValue;

                            ds = oReportesGuiaDespachoBL.ObtenerReporteFormatoEnvio(sOficinaConsularIdRC, strNumeroGuia, strAnioMes, intTarifarioId, intEstadoId);

                        }

                        if (strTipoReporteReniec.Equals(SGAC.Accesorios.Constantes.CONST_RENIEC_FORMATO_ENVIO_ESTADO))
                        {
                            string strEstadoId = ddlEstadoFichaRegistral.SelectedValue;
                            Int16 intEstadoId = Convert.ToInt16(strEstadoId);
                            Int16 intTarifarioId = -1;

                            ReporteConsultasBL oReportesGuiaDespachoBL = new ReporteConsultasBL();

                            Int16 sOficinaConsularIdRC = Convert.ToInt16(ctrlOficinaConsular.SelectedValue);
                            string strNumeroGuia = ddlGuiaDespacho.SelectedItem.Text;

                            string strMes = (ddlMes.SelectedIndex + 1).ToString("00");
                            string strAnioMes = ddlAnio.SelectedValue + strMes;

                            Session["FechaIntervalo"] = SGAC.Accesorios.Util.ObtenerMesLargo(strMes).ToUpper() + " " + ddlAnio.SelectedValue;

                            ds = oReportesGuiaDespachoBL.ObtenerReporteFormatoEnvioRecuparados(sOficinaConsularIdRC, strNumeroGuia, strAnioMes, intTarifarioId, intEstadoId);
                        }

                        if (strTipoReporteReniec.Equals(SGAC.Accesorios.Constantes.CONST_RENIEC_GUIA_DESPACHO))
                        {
                            ReporteConsultasBL oReportesGuiaDespachoBL = new ReporteConsultasBL();

                            Int16 sOficinaConsularIdRC = Convert.ToInt16(ctrlOficinaConsular.SelectedValue);
                            string strNumeroGuia = ddlGuiaDespacho.SelectedItem.Text;

                            string strMes = (ddlMes.SelectedIndex + 1).ToString("00");
                            string strAnioMes = ddlAnio.SelectedValue + strMes;


                            Session["FechaIntervalo"] = SGAC.Accesorios.Util.ObtenerMesLargo(strMes).ToUpper() + " " + ddlAnio.SelectedValue;

                            ds = oReportesGuiaDespachoBL.ObtenerReporteGuiaDespacho(sOficinaConsularIdRC, strNumeroGuia, strAnioMes);

                        }
                        if (strTipoReporteReniec.Equals(SGAC.Accesorios.Constantes.CONST_RENIEC_RENDICION_CUENTAS))
                        {
                            ReporteConsultasBL oReportesRendicionCuentaBL = new ReporteConsultasBL();
                            long intFichaRegistralId = 0;
                            string strNumeroFicha = "";
                            Int16 intEstadoId = 0; 
                            string strNumeroGuia = "";

                            string strMes = (ddlMes.SelectedIndex + 1).ToString("00");
                            string strAnioMes = ddlAnio.SelectedValue + strMes;
                                                                
                            Int16 sOficinaConsularIdRC = Convert.ToInt16(ctrlOficinaConsular.SelectedValue);
                            Int16 OrdenadoPor = 1;

                            if (rbOrderFecha.Checked)
                            {
                                OrdenadoPor = 1;
                            }
                            else if (rbOrderTarifa.Checked)
                            {
                                OrdenadoPor = 2;
                            }

                            ds = oReportesRendicionCuentaBL.ObtenerReporteRendicionCuenta(intFichaRegistralId, strNumeroFicha, intEstadoId,
                                                                        strAnioMes, strNumeroGuia, sOficinaConsularIdRC, OrdenadoPor);
                        }
                        if (strTipoReporteReniec.Equals(SGAC.Accesorios.Constantes.CONST_REPORTE_RENIEC_INCOMPLETOS))
                        {
                            ReporteConsultasBL oReportesRendicionCuentaBL = new ReporteConsultasBL();
                       
                            string strMes = (ddlMes.SelectedIndex + 1).ToString("00");
                            string strAnioMes = ddlAnio.SelectedValue + strMes;

                            Int16 sOficinaConsularIdRC = Convert.ToInt16(ctrlOficinaConsular.SelectedValue);

                            ds = oReportesRendicionCuentaBL.ObtenerReporteTramitesIncompletos(strAnioMes, sOficinaConsularIdRC);
                        }
                        if (strTipoReporteReniec.Equals(SGAC.Accesorios.Constantes.CONST_RENIEC_CONCILIACION))
                        {
                            ReporteConsultasBL oReportesRendicionCuentaBL = new ReporteConsultasBL();

                            string strMes = (ddlMes.SelectedIndex + 1).ToString("00");
                            string strAnioMes = ddlAnio.SelectedValue + strMes;

                            Int16 sOficinaConsularIdRC = Convert.ToInt16(ctrlOficinaConsular.SelectedValue);

                            ds = oReportesRendicionCuentaBL.ObtenerReporteConciliacionReniec(strAnioMes);
                        }  
                    break;
                //case (int)Enumerador.enmLibroContable.AUTOADHESIVOS_USUARIO_OFICINA_CONSULAR:
                //    ds = oBL.USP_REPORTE_AUTOADHESIVOS_USUARIO_OFICINACONSULAR(sOficinaConsularId, datFechaInicio, datFechaFin, 0);
                //    break;
                //case (int)Enumerador.enmLibroContable.ACTUACIONES_USUARIO_OFICINA_CONSULAR:
                //    ds = oBL.USP_REPORTE_ACTUACIONES_USUARIO_OFICINACONSULAR(sOficinaConsularId, datFechaInicio, datFechaFin, sUsuarioId,intTipoPago,intTipoRol);
               //break;                    
            }

            if (valor == "REGISTROCIVIL")
            {
                switch (ddlTipoReporteRCivil.SelectedItem.Text)
                {
                    case Constantes.CONST_REPORTE_ACTA_NACIMIENTO:
                        //-----------------------------------------------------------------------------------
                        // Autor: Jonatan Silva Cachay
                        // Fecha: 23/04/2018
                        // Objetivo: REPORTE ACTA DE NACIMIENTO
                        //-----------------------------------------------------------------------------------
                        datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
                        datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);

                        ReporteConsultasBL oReportesBL = new ReporteConsultasBL();
                        Int16 sOficinaConsularIdRC = Convert.ToInt16(ctrlOficinaConsular.SelectedValue);
                        ds = oReportesBL.ObtenerReporteListadoRegCivil(datFechaInicio, datFechaFin, sOficinaConsularIdRC);
                        break;
                }
            }
            
            return ds;
        }

        private int ObtenerTotalRegistroDataSet(DataSet ds)
        {
            int intTotalRegistros = 0;
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                if (ds.Tables[i] != null)
                {
                    intTotalRegistros += ds.Tables[i].Rows.Count;
                }
            }
            return intTotalRegistros;
        }

        private void CalcularDiasCierreCuenta()
        {
            //------------------------------------------------------------------------
            // Autor: Miguel Angel Márquez Beltrán
            // Fecha: 11/01/2021
            // Motivo: De acuerdo a la oficina consular seleccionada
            //          obtener si es jefatura o no.
            //------------------------------------------------------------------------
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                if (ctrlOficinaConsular.SelectedIndex <= 0)
                    return;
            }
            //int intOficinaConsularId = Convert.ToInt32(ctrlOficinaConsular.SelectedValue);
            //-------------------------------------------------------------
            // Fecha: 11/01/2021
            // Autor: Miguel Márquez Beltrán
            // Motivo: Obtener datos de la oficina consular seleccionada.
            //--------------------------------------------------------------
            

            //DataTable dtOficinaConsularPorId = new DataTable();
            //SGAC.Configuracion.Sistema.BL.OficinaConsularConsultasBL BL = new SGAC.Configuracion.Sistema.BL.OficinaConsularConsultasBL();

            //dtOficinaConsularPorId = BL.ObtenerPorId(intOficinaConsularId);
            int intEsJefatura = 0;
            //double dblDiferenciaHoraria = 0;
            //double dblHorarioVerano = 0;

            //if (dtOficinaConsularPorId.Rows.Count > 0)
            //{
            //    if (Convert.ToBoolean(dtOficinaConsularPorId.Rows[0]["ofco_iJefaturaFlag"]) == true)
            //    {
            //        intEsJefatura = 1;
            //    }
            //    dblDiferenciaHoraria = Convert.ToDouble(dtOficinaConsularPorId.Rows[0]["ofco_sDiferenciaHoraria"].ToString());
            //    dblHorarioVerano = Convert.ToDouble(dtOficinaConsularPorId.Rows[0]["ofco_sHorarioVerano"].ToString());
            //}
            //else
            //{
            //    return;
            //}
            //--------------------------------------------------------------
            intEsJefatura = Convert.ToInt32(Session[Constantes.CONST_SESION_JEFATURA_FLAG]);
            
            int intDiasHabiles = 0;
            string strDiasHabiles = "";
            if (intEsJefatura == 1)
            {
                strDiasHabiles = ConfigurationManager.AppSettings["sDiasActuacionesHabilesJefatura"].ToString();
            }
            else
            {
                strDiasHabiles = ConfigurationManager.AppSettings["sDiasActuacionesHabilesConsulado"].ToString();
            }
            
            intDiasHabiles = Convert.ToInt32(strDiasHabiles);
           DateTime dFecActual = Comun.FormatearFecha(Comun.ObtenerFechaActualTexto(Session));

          //  DateTime dFecActual = Comun.FormatearFecha(ObtenerFechaActualTexto(dblDiferenciaHoraria, dblHorarioVerano));
                        
            
            int intMesFinalFecha = dFecActual.Month;
            int intAnioFinalFecha = dFecActual.Year;
            int intDiaFinalFecha = dFecActual.Day;

            if (intDiaFinalFecha > intDiasHabiles)
            {
                if (intMesFinalFecha == 12)
                {
                    intMesFinalFecha = 1;
                    intAnioFinalFecha++;
                }
                else
                {
                    intMesFinalFecha++;
                }
            }

            DateTime dFecUltima = new DateTime(intAnioFinalFecha, intMesFinalFecha, intDiasHabiles);

            TimeSpan ts = dFecUltima - dFecActual;
            int intDiferenciaDias = ts.Days + 1;
            //solo quedan 10 días calendario para el cierre de cuenta, fecha fin: 10/08/216

            string strMesPeriodo = "";
            int intMesPeriodo = 0;
          
            if (intMesFinalFecha == 1)
            {
                intMesPeriodo = 12;
            }
            else
            {
                intMesPeriodo = intMesFinalFecha - 1;
            }
            if (intDiferenciaDias == 1)
            {
                lblMsjDiasHabilesCierreCuenta.Text = "Solo queda " + intDiferenciaDias.ToString();
            }
            else
            {
                lblMsjDiasHabilesCierreCuenta.Text = "Solo quedan " + intDiferenciaDias.ToString();
            }
            

            switch (intMesPeriodo)
            {
                case 1: strMesPeriodo = "Enero"; break;
                case 2: strMesPeriodo = "Febrero"; break;
                case 3: strMesPeriodo = "Marzo"; break;
                case 4: strMesPeriodo = "Abril"; break;
                case 5: strMesPeriodo = "Mayo"; break;
                case 6: strMesPeriodo = "Junio"; break;
                case 7: strMesPeriodo = "Julio"; break;
                case 8: strMesPeriodo = "Agosto"; break;
                case 9: strMesPeriodo = "Setiembre"; break;
                case 10: strMesPeriodo = "Octubre"; break;
                case 11: strMesPeriodo = "Noviembre"; break;
                case 12: strMesPeriodo = "Diciembre"; break;
                default:
                    break;
            }
            if (intDiferenciaDias == 1)
            {
                lblMsjDiasHabilesCierreCuenta.Text = lblMsjDiasHabilesCierreCuenta.Text + " día calendario para el cierre de la cuenta del mes de " + strMesPeriodo + ", fecha fin: " + dFecUltima.ToShortDateString();
            }
            else
            {
                lblMsjDiasHabilesCierreCuenta.Text = lblMsjDiasHabilesCierreCuenta.Text + " días calendario para el cierre de la cuenta del mes de " + strMesPeriodo + ", fecha fin: " + dFecUltima.ToShortDateString();
            }
        }
        #endregion

        protected void chkFechaActuacion_CheckedChanged(object sender, EventArgs e)
        {
            chkFechaAnulacion.Checked = !chkFechaActuacion.Checked;
        }

        protected void chkFechaAnulacion_CheckedChanged(object sender, EventArgs e)
        {
            chkFechaActuacion.Checked = !chkFechaAnulacion.Checked;
        }


        private Int16 ObtenerIdEstadoGuiaDespacho(string strEstado)
        {
            Int16 intEstadoGuiaId = 0;

            DataTable dtEstadoGuia = new DataTable();

//            dtEstadoGuia = Comun.ObtenerParametrosPorGrupo((DataTable)Session[Constantes.CONST_SESION_DT_ESTADO], SGAC.Accesorios.Constantes.CONST_GUIA_DESPACHO_ESTADO);
            dtEstadoGuia = comun_Part1.ObtenerParametrosPorGrupoMRE(SGAC.Accesorios.Constantes.CONST_GUIA_DESPACHO_ESTADO);
            
            for (int i = 0; i < dtEstadoGuia.Rows.Count; i++)
            {
                if (dtEstadoGuia.Rows[i]["descripcion"].Equals(strEstado))
                {
                    intEstadoGuiaId = Convert.ToInt16(dtEstadoGuia.Rows[i]["ID"].ToString());
                    break;
                }
            }
            return intEstadoGuiaId;
        }

        private void CargarNumeroGuiaDespacho(Int16 estadoGuiaId)
        {
            if (ddlTipoReporteReniec.SelectedIndex > 0)
            {
                string strAnioMes = ddlAnio.SelectedValue + (ddlMes.SelectedIndex + 1).ToString("00");
                int intOficinaConsularId = Convert.ToInt16(ctrlOficinaConsular.SelectedValue);

                DataTable dtNumeroGuiaDespacho = new DataTable();
                GuiaDespachoBL objGuiaDespachoBL = new GuiaDespachoBL();
                Int16 intEstadoGuiaId;
                if (estadoGuiaId == 0)
                {
                    intEstadoGuiaId = ObtenerIdEstadoGuiaDespacho("REGISTRADO");
                }
                else {
                    intEstadoGuiaId = estadoGuiaId;
                }
                
                dtNumeroGuiaDespacho = objGuiaDespachoBL.ConsultarNumeroGuiaDespacho(intOficinaConsularId, 0, strAnioMes, intEstadoGuiaId);
                SGAC.Accesorios.Util.CargarDropDownList(ddlGuiaDespacho, dtNumeroGuiaDespacho, "VNUMEROGUIADESPACHO", "IGUIADESPACHOID");
            }
        }

        protected void ddlAnio_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarNumeroGuiaDespacho(0);
            //--------------------------------------------------
            //Fecha: 12/03/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Se asignan las fechas de inicio y fin.
            //--------------------------------------------------
            asignarFechaInicioFin();
        }

        protected void ddlMes_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarNumeroGuiaDespacho(0);
            //--------------------------------------------------
            //Fecha: 12/03/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Se asignan las fechas de inicio y fin.
            //--------------------------------------------------
            asignarFechaInicioFin();
        }

        protected void ddlBancoConsultas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBancoConsulta.SelectedIndex > 0)
            {
                int intBancoId = Convert.ToInt32(ddlBancoConsulta.SelectedValue);
                int intOficinaConsularId = Convert.ToInt32(ctrlOficinaConsular.ddlOficinaConsular.SelectedValue);
                //object[] arrParametros = { intOficinaConsularId, intBancoId, 1, 1000, 0, 0 };

                //Proceso p = new Proceso();

                //DataTable dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.CO_CUENTACORRIENTE", Enumerador.enmAccion.CONSULTAR);

                CuentaConsultasBL _obj = new CuentaConsultasBL();
                int intTotalRegistros = 0;
                int intTotalPaginas = 0;
                DataTable dt = _obj.Consultar(intOficinaConsularId, intBancoId, 1, 1000, ref intTotalRegistros, ref intTotalPaginas);

                if (dt.Rows.Count > 0)
                {
                    SGAC.Accesorios.Util.CargarDropDownList(ddlCuentaCorrienteConsulta, dt, "cuco_vNumero", "cuco_sCuentaCorrienteId", true);
                }
            }
            else
            {
                SGAC.Accesorios.Util.CargarParametroDropDownList(ddlCuentaCorrienteConsulta, new DataTable(), true, "");
            }
        }
protected void btnSIEnviarGuiaDespacho_Click(object sender, EventArgs e)
        {
            VerVistaPrevia();
        }

protected void chkTodos_CheckedChanged(object sender, EventArgs e)
{
    if (chkTodos.Checked)
    {
        Int16 intEstadoGuiaId = ObtenerIdEstadoGuiaDespacho("ENVIADO");
        CargarNumeroGuiaDespacho(intEstadoGuiaId);
    }
    else { CargarNumeroGuiaDespacho(0); }
}


protected void lkbDescargar_Click(object sender, EventArgs e)
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

    if (ddlLibroContableTipo.SelectedItem.Text == "FORMATOS REGISTRO CIVIL")
    {
        if (ddlTipoReporteRCivil.SelectedIndex > 0)
        {
            DataTable _dt = comun_Part1.ObtenerParametroPorId(Session, Convert.ToInt32(ddlTipoReporteRCivil.SelectedValue));
            string strNombreArchivo;
            string strCarpeta = "FormatosRegistroCivil";

            if (_dt.Rows.Count > 0)
            {
                strNombreArchivo = _dt.Rows[0]["valor"].ToString();
                DescargarArchivo(strCarpeta, strNombreArchivo);
            }
        }
        //ctrlToolBarConsulta.btnImprimir.Enabled = true;
        btnImprimir.Enabled = true;
        return;
    }
}

protected void ddlCuentaCorrienteConsulta_SelectedIndexChanged(object sender, EventArgs e)
{
    int intBancoId = Convert.ToInt32(ddlBancoConsulta.SelectedValue);
    string strNumeroCuenta = ddlCuentaCorrienteConsulta.SelectedItem.Text;
    Int16 CodNumeroCuenta = Convert.ToInt16(ddlCuentaCorrienteConsulta.SelectedValue);

    object[] arrParametros = { Convert.ToInt32(ctrlOficinaConsular.ddlOficinaConsular.SelectedValue), 
                                           intBancoId, 
                                           strNumeroCuenta };

    CuentaConsultasBL _obj = new CuentaConsultasBL();


    //Proceso p = new Proceso();
    //DataTable dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.CO_CUENTACORRIENTE", Enumerador.enmAccion.LEER);

    DataTable dt = _obj.ObtenerPorNroCuenta(Convert.ToInt32(ctrlOficinaConsular.ddlOficinaConsular.SelectedValue), intBancoId, strNumeroCuenta, CodNumeroCuenta);
    //if (p.IErrorNumero == 0)
    //{
    if (dt.Rows.Count > 0)
    {
        lblMoneda.Text = dt.Rows[0]["cuco_vMoneda"].ToString();
    }
}

//---------------------------------------------------

protected void btnImprimir_Click(object sender, EventArgs e)
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
   
    btnImprimir.Enabled = false;


    if (ddlLibroContableTipo.SelectedItem.Text == "FORMATOS REGISTRO CIVIL")
    {
        if (ddlTipoReporteRCivil.SelectedIndex > 0)
        {
            DataTable _dt = comun_Part1.ObtenerParametroPorId(Session, Convert.ToInt32(ddlTipoReporteRCivil.SelectedValue));
            string strNombreArchivo;
            string strCarpeta = ConfigurationManager.AppSettings["CarpetaFormatosRegCivil"];

            if (_dt.Rows.Count > 0)
            {
                strNombreArchivo = _dt.Rows[0]["valor"].ToString();
                DescargarArchivo(strCarpeta, strNombreArchivo);
            }
        }
        btnImprimir.Enabled = true;
        return;
    }

    //------------------------------------------------------------------------
    // Autor: Jonatan Silva Cachay
    // Fecha: 18/01/2017
    // Objetivo: Validación para reporte de saldos consulares.
    //------------------------------------------------------------------------
    if (ddlLibroContableTipo.SelectedValue == Convert.ToString((int)Enumerador.enmLibroContable.SALDOS_CONSULARES))
    {
        if (ctrlOficinaConsular.ddlOficinaConsular.Items.Count > 1)
        {
            if (ddlCuentaCorrienteConsulta.SelectedIndex == 0 || ddlCuentaCorrienteConsulta.SelectedIndex == -1)
            {
                ctrlValidacion.MostrarValidacion("Para visualizar el Reporte De Saldos Consulares debe seleccionar el nro. de cuenta", true, Enumerador.enmTipoMensaje.ERROR);
                //ctrlToolBarConsulta.btnImprimir.Enabled = true;
                btnImprimir.Enabled = true;
                return;
            }
        }
    }
    if (ddlLibroContableTipo.Visible == true)
    {
        if (ddlLibroContableTipo.SelectedIndex == 0)
        {
            ctrlValidacion.MostrarValidacion("Seleccion el tipo de libro contable", true, Enumerador.enmTipoMensaje.ERROR);
            //ctrlToolBarConsulta.btnImprimir.Enabled = true;
            btnImprimir.Enabled = true;
            return;
        }
    }

    if (ddlLibroContableTipo.SelectedValue == Convert.ToString((int)Enumerador.enmLibroContable.RENIEC))
    {
        if (ddlTipoReporteReniec.SelectedIndex == 0)
        {
            ctrlValidacion.MostrarValidacion("Seleccion el tipo de Reporte de RENIEC", true, Enumerador.enmTipoMensaje.ERROR);
            //ctrlToolBarConsulta.btnImprimir.Enabled = true;
            btnImprimir.Enabled = true;
            return;
        }

        string strTipoReporteReniec = ddlTipoReporteReniec.SelectedItem.Text.Trim();
        if (strTipoReporteReniec != SGAC.Accesorios.Constantes.CONST_RENIEC_RENDICION_CUENTAS && strTipoReporteReniec != SGAC.Accesorios.Constantes.CONST_REPORTE_RENIEC_INCOMPLETOS
            && strTipoReporteReniec != SGAC.Accesorios.Constantes.CONST_RENIEC_CONCILIACION)
        {
            if (ddlGuiaDespacho.SelectedIndex == -1)
            {
                ctrlValidacion.MostrarValidacion("Seleccione el N° de Guía de Despacho", true, Enumerador.enmTipoMensaje.ERROR);
                //ctrlToolBarConsulta.btnImprimir.Enabled = true;
                btnImprimir.Enabled = true;
                return;
            }
        }
        
        

        

        if (strTipoReporteReniec.Equals(SGAC.Accesorios.Constantes.CONST_RENIEC_GUIA_DESPACHO))
        {
            //ctrlToolBarConsulta.btnImprimir.Enabled = true;
            btnImprimir.Enabled = true;
            Session["IdOficinaConsular_contabilidad"] = ctrlOficinaConsular.SelectedValue;
            mdlpopup.Show();
            return;
        }

        Session["FechaIntervalo"] = " Del Periodo: " + ddlAnio.SelectedItem.Text + "-" + ddlMes.SelectedItem.Text;
        Session["IdOficinaConsular_contabilidad"] = ctrlOficinaConsular.SelectedValue;
        VerVistaPrevia();
        CargarNumeroGuiaDespacho(0);
    }
    else
    {
        //REPORTE REGISTRO CIVIL
        if (ddlLibroContableTipo.SelectedItem.Text == "REPORTES REGISTRO CIVIL")
        {
            if (ddlTipoReporteRCivil.SelectedIndex == 0)
            {
                ctrlValidacion.MostrarValidacion("Seleccion el tipo de Reporte de REGISTRO CIVIL", true, Enumerador.enmTipoMensaje.ERROR);
                //ctrlToolBarConsulta.btnImprimir.Enabled = true;
                btnImprimir.Enabled = true;
                return;
            }

            string strTipoReporteRegistroCivil = ddlTipoReporteRCivil.SelectedItem.Text.Trim();
            Session["FechaIntervalo"] = " Del " + dtpFecInicio.Text + " al " + dtpFecFin.Text;
            Session["IdOficinaConsular_contabilidad"] = ctrlOficinaConsular.SelectedValue;
            VerVistaPrevia();
            //CargarNumeroGuiaDespacho(0);
        }
        else
        {
            if (ddlLibroContableTipo.SelectedValue != Convert.ToString((int)Enumerador.enmLibroContable.AUTOADHESIVO_CONSULAR))
            {
                if (dtpFecInicio.Text == string.Empty || dtpFecFin.Text == string.Empty)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_VACIA, true, Enumerador.enmTipoMensaje.ERROR);
                    //ctrlToolBarConsulta.btnImprimir.Enabled = true;
                    btnImprimir.Enabled = true;
                    return;
                }

                DateTime datFechaInicio = new DateTime();
                DateTime datFechaFin = new DateTime();

                if (!DateTime.TryParse(dtpFecInicio.Text, out datFechaInicio))
                {
                    datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
                }
                if (!DateTime.TryParse(dtpFecFin.Text, out datFechaFin))
                {
                    datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
                }

                Session["FechaIntervalo"] = " Del " + dtpFecInicio.Text + " al " + dtpFecFin.Text;
                Session["IdOficinaConsular_contabilidad"] = ctrlOficinaConsular.SelectedValue;
                if ( datFechaInicio > datFechaFin)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);
                }
                else
                {
                    VerVistaPrevia();
                }
            }
            else
            {
                Session["FechaIntervalo"] = " Del Periodo: " + ddlAnio.SelectedItem.Text + "-" + ddlMes.SelectedItem.Text;
                Session["IdOficinaConsular_contabilidad"] = ctrlOficinaConsular.SelectedValue;
                VerVistaPrevia();
            }
        }

    }
    btnImprimir.Enabled = true;
    chkTodos.Checked = false;
    
}

protected void btnExportarPDF_Click(object sender, EventArgs e)
{
 
    PDFComun pdf = new PDFComun();

    MemoryStream msDocumento = new MemoryStream();


    string struploadPath = WebConfigurationManager.AppSettings["UploadPath"];


    string strRutaFile = "RGE.pdf";
    string strRutaUnicaFilePDF = pdf.GetUniqueUploadFileName(struploadPath, strRutaFile);


    CreatePDF_RGE(strRutaUnicaFilePDF);

    if (File.Exists(strRutaUnicaFilePDF))
    {
        DonwloadFromFile(strRutaUnicaFilePDF, "RGE.pdf", "pdf");
    }

}

public void CreatePDF_RGE(string strFileName)
{
    if (ValidarReporteRGE())
    {
        return;
    }
    ctrlValidacion.MostrarValidacion("", false);
    try
    {
        
        //-----------------------------------------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha: 09/08/2018
        // Objetivo: Parametros para la consulta RGE (Obtiene un List<csRGE> del Objeto DataReader)
        //------------------------------------------------------------------------------------------
        DateTime datFechaInicio = new DateTime();
        DateTime datFechaFin = new DateTime();

        int intSelOficinaConsularId = Convert.ToInt32(ctrlOficinaConsular.SelectedValue);
        datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
        datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
        int sUsuarioId = Convert.ToInt32(Session[Constantes.CONST_SESION_USUARIO_ID]);
        string strDireccionIP = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
        int intOficinaConsularLogeo = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

        ReporteConsultasBL objReporteBL = new ReporteConsultasBL();
        List<csRGE> listaRGE = new List<csRGE>();
        //List<csRGE> listaRGE_Detalle = new List<csRGE>();
        //List<csRGE> listaRGE_Resumen = new List<csRGE>();

        listaRGE = objReporteBL.ObtenerReporteRGE_Reader(intSelOficinaConsularId, datFechaInicio, datFechaFin, SGAC.Accesorios.Util.ObtenerHostName(), sUsuarioId, strDireccionIP, intOficinaConsularLogeo);

                

        if (listaRGE.Count == 0)
        {
            ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA, true, Enumerador.enmTipoMensaje.INFORMATION);
            return;
        }

        //--------------------------------------------------------------------------------


        int intCantidadRegistros = listaRGE.Count;


        string strNombreOficinaconsular = comun_Part2.ObtenerNombreOficinaPorId(Session, intSelOficinaConsularId);

        strNombreOficinaconsular = strNombreOficinaconsular.Split('-')[1].ToString().Trim();

        string strUsuario = Session[Constantes.CONST_SESION_USUARIO].ToString();

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
                
        //-------------------------------------------------

        PDFComun pdf = new PDFComun();
        pdf.marginLeft = 10f;
        pdf.marginRight = 10f;
        pdf.marginTop = 10f;
        pdf.marginBottom = 10f;
        pdf.pagesize = iTextSharp.text.PageSize.A4;
        pdf.ImprimirNumeroPagina = true;

        //-------------------------------------------------

        pdf.Consulado = strNombreOficinaconsular;
        pdf.SubTitulo = Constantes.CONST_REPORTE_SUB_TITULO;
        pdf.Titulo = "REGISTRO GENERAL DE ENTRADAS";
        pdf.TituloFiltro = "Del " + dtpFecInicio.Text + " al " + dtpFecFin.Text;
        pdf.TituloFiltroDerecha = "Cantidad de Actuaciones: " + intCantidadRegistros.ToString();
        //-----------------------------------------
        //BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
        iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Fonts/arialn.ttf"), iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.EMBEDDED);



        PdfPTable pdftableCabeza = new PdfPTable(13);
        pdftableCabeza.SetWidths(new float[] { 21f, 40f, 45f, 120f, 55f, 150f, 28f, 40f, 51f, 52f, 37f, 105f, 10f });
        pdftableCabeza.TotalWidth = 800;
        pdftableCabeza.LockedWidth = true;
        pdftableCabeza.SpacingBefore = 200.0f;
        pdftableCabeza.SpacingAfter = 20.0f;
        pdftableCabeza.DefaultCell.Border = 0;
        //--------------------------------------
        List<PDFCeldas> listaColumnas = new List<PDFCeldas>();

        listaColumnas.Add(new PDFCeldas("N° Item", Element.ALIGN_CENTER, Element.ALIGN_CENTER, 1, 7, 0, 0));
        listaColumnas.Add(new PDFCeldas("Corr. Actuación", Element.ALIGN_CENTER, Element.ALIGN_CENTER, 1, 7, 0, 0));
        listaColumnas.Add(new PDFCeldas("Fecha Actuación", Element.ALIGN_CENTER, Element.ALIGN_CENTER, 1, 7, 0, 0));
        listaColumnas.Add(new PDFCeldas("Nombre del Interesado", Element.ALIGN_CENTER, Element.ALIGN_CENTER, 1, 7, 0, 0));
        listaColumnas.Add(new PDFCeldas("Autoadhesivo Consular", Element.ALIGN_CENTER, Element.ALIGN_CENTER, 1, 7, 0, 0));
        listaColumnas.Add(new PDFCeldas("Naturaleza del Acto", Element.ALIGN_CENTER, Element.ALIGN_CENTER, 1, 7, 0, 0));
        listaColumnas.Add(new PDFCeldas("N° Tarifa", Element.ALIGN_CENTER, Element.ALIGN_CENTER, 1, 7, 0, 0));
        listaColumnas.Add(new PDFCeldas("N° Actuación", Element.ALIGN_CENTER, Element.ALIGN_CENTER, 1, 7, 0, 0));
        listaColumnas.Add(new PDFCeldas("Moneda Extranjera S/.", Element.ALIGN_CENTER, Element.ALIGN_CENTER, 1, 7, 0, 0));
        listaColumnas.Add(new PDFCeldas("Soles Consular S/C", Element.ALIGN_CENTER, Element.ALIGN_CENTER, 1, 7, 0, 0));
        listaColumnas.Add(new PDFCeldas("T. C. Consular", Element.ALIGN_CENTER, Element.ALIGN_CENTER, 1, 7, 0, 0));
        listaColumnas.Add(new PDFCeldas("Observación", Element.ALIGN_CENTER, Element.ALIGN_CENTER, 1, 7, 0, 0));
        listaColumnas.Add(new PDFCeldas("I", Element.ALIGN_CENTER, Element.ALIGN_CENTER, 1, 7, 0, 0));

        pdf.Celdas(ref pdftableCabeza, listaColumnas, baseFont, 6f, 0.9f, true);

        //-------------------------------------------------


        PdfPTable pdftableDetalle = new PdfPTable(13);
        pdftableDetalle.SetWidths(new float[] { 21f, 40f, 45f, 120f, 55f, 150f, 28f, 40f, 51f, 52f, 37f, 105f, 10f });
        pdftableDetalle.TotalWidth = 800;
        pdftableDetalle.LockedWidth = true;
        pdftableDetalle.SpacingBefore = 200.0f;
        pdftableDetalle.SpacingAfter = 20.0f;
        pdftableDetalle.DefaultCell.Border = 0;
        //--------------------------------------


        List<PDFCeldas> listaFilasDetalle = new List<PDFCeldas>();
        //int n = 0;
        Double intTotalRecaudadoME = 0;
        Double intTotalRecaudadoMN = 0;
        Double intTotalPagadoLimaME = 0;
        Double intTotalPagadoLimaMN = 0;
        Double intTotalRecaudadoOfME = 0;
        Double intTotalRecaudadoOfMN = 0;
        Int64 intCantidadAutoAdhesivosEmpleados = Convert.ToInt32(listaRGE[0].iCantAutoadhesivo);
        Double intMonedaExtranjera = 0;
        Double intSolesConsulares = 0;

        //-------------------------------------------------

        for (int i = 0; i < listaRGE.Count; i++)
        {
            //n++;
            //--------------------------------------------------------------
            listaFilasDetalle = new List<PDFCeldas>();
            
            listaFilasDetalle.Add(new PDFCeldas(listaRGE[i].iNumero, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 1, 3, 0, 0));                        
            listaFilasDetalle.Add(new PDFCeldas(listaRGE[i].iNumeroOrden, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 1, 3, 0, 0));
            listaFilasDetalle.Add(new PDFCeldas(listaRGE[i].dFecha.Substring(0,10), Element.ALIGN_CENTER, Element.ALIGN_CENTER, 1, 3, 0, 0));                                  
            listaFilasDetalle.Add(new PDFCeldas(listaRGE[i].vSolicitante, Element.ALIGN_CENTER, Element.ALIGN_LEFT, 1, 3, 0, 0));            
            listaFilasDetalle.Add(new PDFCeldas(listaRGE[i].vAutoadhesivoCod, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 1, 3, 0, 0));
            listaFilasDetalle.Add(new PDFCeldas(listaRGE[i].vTarifaDesc, Element.ALIGN_CENTER, Element.ALIGN_LEFT, 1, 3, 0, 0));
            listaFilasDetalle.Add(new PDFCeldas(listaRGE[i].vTarifaNro, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 1, 3, 0, 0));            
            listaFilasDetalle.Add(new PDFCeldas(listaRGE[i].iNumeroActuacion, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 1, 3, 0, 0));
            
            intMonedaExtranjera = Convert.ToDouble(listaRGE[i].FMonedaExtranjera);
            intSolesConsulares= Convert.ToDouble(listaRGE[i].FSolesConsular);
            
            listaFilasDetalle.Add(new PDFCeldas(intMonedaExtranjera.ToString("#,##0.00"), Element.ALIGN_CENTER, Element.ALIGN_RIGHT, 1, 3, 2, 0));
            listaFilasDetalle.Add(new PDFCeldas(intSolesConsulares.ToString("#,##0.00"), Element.ALIGN_CENTER, Element.ALIGN_RIGHT, 1, 3, 2, 0));
            listaFilasDetalle.Add(new PDFCeldas(listaRGE[i].FValorTCConsular, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 1, 3, 0, 0));
            listaFilasDetalle.Add(new PDFCeldas(listaRGE[i].vObservacion, Element.ALIGN_CENTER, Element.ALIGN_LEFT, 1, 3, 0, 0));
            listaFilasDetalle.Add(new PDFCeldas(listaRGE[i].Itinerante, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 1, 3, 0, 0));

            
            intTotalRecaudadoME = intTotalRecaudadoME + intMonedaExtranjera;
            intTotalRecaudadoMN = intTotalRecaudadoMN + intSolesConsulares;            

            intTotalPagadoLimaME = Convert.ToDouble(listaRGE[i].PagadoLimaME);
            intTotalPagadoLimaMN = Convert.ToDouble(listaRGE[i].PagadoLimaSC);

            pdf.Celdas(ref pdftableDetalle, listaFilasDetalle, baseFont, 6f, 1f, true);
            //--------------------------------------------------------------
        }
        intTotalRecaudadoOfME = intTotalRecaudadoME - intTotalPagadoLimaME;
        intTotalRecaudadoOfMN = intTotalRecaudadoMN - intTotalPagadoLimaMN;


        PdfPTable pdftableResumen = new PdfPTable(13);
        pdftableResumen.SetWidths(new float[] { 21f, 40f, 45f, 120f, 55f, 150f, 28f, 40f, 51f, 52f, 37f, 105f, 10f });
        pdftableResumen.TotalWidth = 800;
        pdftableResumen.LockedWidth = true;
        pdftableResumen.SpacingBefore = 0f;
        pdftableResumen.SpacingAfter = 0f;
        pdftableResumen.DefaultCell.Border = 0;
        //--------------------------------------


        List<PDFCeldas> listaFilasResumen = new List<PDFCeldas>();

        listaFilasResumen.Add(new PDFCeldas(5, 2));
        listaFilasResumen.Add(new PDFCeldas("Total recaudado", Element.ALIGN_CENTER, Element.ALIGN_RIGHT, 1, 2, 10, 0, 3));
        listaFilasResumen.Add(new PDFCeldas(Convert.ToDouble(intTotalRecaudadoME.ToString()).ToString("#,##0.00"), Element.ALIGN_MIDDLE, Element.ALIGN_RIGHT, 1, 2, 2, 0));
        listaFilasResumen.Add(new PDFCeldas(Convert.ToDouble(intTotalRecaudadoMN.ToString()).ToString("#,##0.00"), Element.ALIGN_MIDDLE, Element.ALIGN_RIGHT, 1, 2, 2, 0));
        listaFilasResumen.Add(new PDFCeldas(3, 2));

        pdf.Celdas(ref pdftableResumen, listaFilasResumen, baseFont, 8f, 1f, false);

        //---------------------------------------------
        listaFilasResumen = new List<PDFCeldas>();

        listaFilasResumen.Add(new PDFCeldas(5, 2));
        listaFilasResumen.Add(new PDFCeldas("Actuaciones pagadas en Lima", Element.ALIGN_CENTER, Element.ALIGN_RIGHT, 1, 2, 10, 0, 3));
        listaFilasResumen.Add(new PDFCeldas(Convert.ToDouble(intTotalPagadoLimaME.ToString()).ToString("#,##0.00"), Element.ALIGN_CENTER, Element.ALIGN_RIGHT, 1, 2, 2, 0));
        listaFilasResumen.Add(new PDFCeldas(Convert.ToDouble(intTotalPagadoLimaMN.ToString()).ToString("#,##0.00"), Element.ALIGN_CENTER, Element.ALIGN_RIGHT, 1, 2, 2, 0));
        listaFilasResumen.Add(new PDFCeldas(3, 2));

        pdf.Celdas(ref pdftableResumen, listaFilasResumen, baseFont, 8f, 1f, false);
        //---------------------------------------------

        listaFilasResumen = new List<PDFCeldas>();

        listaFilasResumen.Add(new PDFCeldas(5, 2));
        listaFilasResumen.Add(new PDFCeldas("Total recaudado en Oficina Consular", Element.ALIGN_CENTER, Element.ALIGN_RIGHT, 1, 2, 10, 0, 3));
        listaFilasResumen.Add(new PDFCeldas(Convert.ToDouble(intTotalRecaudadoOfME.ToString()).ToString("#,##0.00"), Element.ALIGN_CENTER, Element.ALIGN_RIGHT, 1, 2, 2, 0));
        listaFilasResumen.Add(new PDFCeldas(Convert.ToDouble(intTotalRecaudadoOfMN.ToString()).ToString("#,##0.00"), Element.ALIGN_CENTER, Element.ALIGN_RIGHT, 1, 2, 2, 0));
        listaFilasResumen.Add(new PDFCeldas(3, 2));

        pdf.Celdas(ref pdftableResumen, listaFilasResumen, baseFont, 8f, 1f, false);
        //---------------------------------------------

        listaFilasResumen = new List<PDFCeldas>();

        listaFilasResumen.Add(new PDFCeldas(5, 2));
        listaFilasResumen.Add(new PDFCeldas("Total de Autoadhesivos empleados:", Element.ALIGN_CENTER, Element.ALIGN_RIGHT, 1, 2, 0, 0, 2));
        listaFilasResumen.Add(new PDFCeldas(intCantidadAutoAdhesivosEmpleados.ToString(), Element.ALIGN_CENTER, Element.ALIGN_CENTER, 1, 2, 0, 20, 1));
        listaFilasResumen.Add(new PDFCeldas(5, 2));

        pdf.Celdas(ref pdftableResumen, listaFilasResumen, baseFont, 8f, 1f, false);
        //---------------------------------------------
        listaFilasResumen = new List<PDFCeldas>();

        listaFilasResumen.Add(new PDFCeldas("(I) Actuaciones registradas desde una ciudad Itinerante.", Element.ALIGN_CENTER, Element.ALIGN_RIGHT, 1, 0, 0, 20, 5));
        listaFilasResumen.Add(new PDFCeldas(8, 2));

        pdf.Celdas(ref pdftableResumen, listaFilasResumen, baseFont, 8f, 1f, false);
        //---------------------------------------------

        PdfPTable pdftablePie = new PdfPTable(6);
        pdftablePie.SetWidths(new float[] { 100f, 5f, 100f, 400f, 30f, 200f });
        pdftablePie.TotalWidth = 800;
        pdftablePie.LockedWidth = true;
        pdftablePie.SpacingBefore = 0f;
        pdftablePie.SpacingAfter = 0f;
        pdftablePie.DefaultCell.Border = 0;        
        //--------------------------------------


        List<PDFCeldas> listaFilasPie = new List<PDFCeldas>();

        listaFilasPie.Add(new PDFCeldas("Usuario de Impresión:", Element.ALIGN_CENTER, Element.ALIGN_RIGHT, 1, 1, 0, 0, 1));
        listaFilasPie.Add(new PDFCeldas(1, 1));
        listaFilasPie.Add(new PDFCeldas(strUsuario, Element.ALIGN_MIDDLE, Element.ALIGN_LEFT, 1, 1, 0, 0, 1));
        listaFilasPie.Add(new PDFCeldas(3, 1));

        pdf.Celdas(ref pdftablePie, listaFilasPie, baseFont, 7f, 1f, false);
        //--------------------------------------

        listaFilasPie = new List<PDFCeldas>();

        listaFilasPie.Add(new PDFCeldas("Fecha de Impresión:", Element.ALIGN_CENTER, Element.ALIGN_RIGHT, 1, 1, 0, 0, 1));
        listaFilasPie.Add(new PDFCeldas(1, 1));
        listaFilasPie.Add(new PDFCeldas(strFechaActualConsulado, Element.ALIGN_MIDDLE, Element.ALIGN_LEFT, 1, 1, 0, 0, 1));
        listaFilasPie.Add(new PDFCeldas(1, 1));
        listaFilasPie.Add(new PDFCeldas("(Fdo.)", Element.ALIGN_MIDDLE, Element.ALIGN_LEFT, 1, 1, 0, 0, 1));
        listaFilasPie.Add(new PDFCeldas(1, 1));

        pdf.Celdas(ref pdftablePie, listaFilasPie, baseFont, 7f, 1f, false);
        //-------------------------------------
        listaFilasPie = new List<PDFCeldas>();

        listaFilasPie.Add(new PDFCeldas("Hora de Impresión:", Element.ALIGN_CENTER, Element.ALIGN_RIGHT, 1, 1, 0, 0, 1));
        listaFilasPie.Add(new PDFCeldas(1, 1));
        listaFilasPie.Add(new PDFCeldas(strHoraActualConsulado, Element.ALIGN_MIDDLE, Element.ALIGN_LEFT, 1, 1, 0, 0, 1));
        listaFilasPie.Add(new PDFCeldas(1, 1));        
        listaFilasPie.Add(new PDFCeldas(1, 1));
        listaFilasPie.Add(new PDFCeldas("(Jefe de la Oficina Consular)", Element.ALIGN_CENTER, Element.ALIGN_CENTER, 1, 1, 0, 0, 1, Rectangle.TOP_BORDER));

        pdf.Celdas(ref pdftablePie, listaFilasPie, baseFont, 7f, 1f, false);


        //--------------------------------------
        pdf.pdftableCabeza = pdftableCabeza;
        pdf.pdftableDetalle = pdftableDetalle;
        pdf.pdftablePie = pdftablePie;
        pdf.pdftableResumen = pdftableResumen;


        byte[] file = pdf.CrearDocumentoPDF();
        File.WriteAllBytes(strFileName, file);



    }
    catch (Exception ex)
    {
        throw ex;
    }
}

private bool ValidarReporteRGE()
{
    bool bExisteError = false;

    if (ddlLibroContableTipo.Visible == true)
    {
        if (ddlLibroContableTipo.SelectedIndex == 0)
        {
            ctrlValidacion.MostrarValidacion("Seleccion el tipo de libro contable", true, Enumerador.enmTipoMensaje.ERROR);            
            bExisteError = true;
            return bExisteError;
        }
    }
    
       
    if (dtpFecInicio.Text == string.Empty || dtpFecFin.Text == string.Empty)
    {
        ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_VACIA, true, Enumerador.enmTipoMensaje.ERROR);        
        bExisteError = true;
        return bExisteError;
    }

    if (Comun.EsFecha(dtpFecInicio.Text.Trim()) == false)
    {
        ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_INICIAL, true, Enumerador.enmTipoMensaje.WARNING);
        bExisteError = true;
        return bExisteError; 
    }
    if (Comun.EsFecha(dtpFecFin.Text.Trim()) == false)
    {
        ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_FINAL, true, Enumerador.enmTipoMensaje.WARNING);
        bExisteError = true;
        return bExisteError;
    }
    
    DateTime datFechaInicio = new DateTime();
    DateTime datFechaFin = new DateTime();

    if (!DateTime.TryParse(dtpFecInicio.Text, out datFechaInicio))
    {
        datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
    }
    if (!DateTime.TryParse(dtpFecFin.Text, out datFechaFin))
    {
        datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
    }
    if (datFechaInicio > datFechaFin)
    {
        ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);
        bExisteError = true;
    }
    return bExisteError;
}

private void DonwloadFromFile(string strRutaFileName, string strFileName, string strTipoFile)
{
    Response.Clear();
    Response.ClearHeaders();
    Response.ContentType = "application/" + strTipoFile;
    Response.AddHeader("Content-Disposition", "attachment;filename=" + strFileName);
    Response.TransmitFile(strRutaFileName);
    Response.Flush();
    if (File.Exists(strRutaFileName))
    {
        File.Delete(strRutaFileName);
    }

}

private bool Mostrar_btnExportarPDF()
{
    bool bMostrarBotonExportarPDF = false;
    int intFilaIndice = 0;
    bool bolEncontro = false;
    string strAcciones = string.Empty;
    string[] arrAcciones;

    string strRutaFormulario = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath;

    DataTable dtConfiguracion;
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
        if (bolEncontro)
        {
            strAcciones = dtConfiguracion.Rows[intFilaIndice]["form_vAcciones"].ToString();
            arrAcciones = strAcciones.Split('|');
            for (int i = 0; i < arrAcciones.Length; i++)
            {
                if (Convert.ToChar(arrAcciones[i]).Equals((char)Enumerador.enmPermisoAccion.GRABAR))
                {
                    bMostrarBotonExportarPDF = true;
                    break;
                }
            }
        }
    }


    return bMostrarBotonExportarPDF;
}
        //--------------------------------------------------
        //Fecha: 12/03/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Se asignan las fechas de inicio y fin.
        //--------------------------------------------------
        private void asignarFechaInicioFin()
        {
            string strAnio = "";
            string strMes = "";
            string strDiaInicio = "01";            
            string strFechaInicio = "";
            string strFechaFinal = "";
            DateTime datFechaFin = new DateTime();
            Int16 intMes;

            strAnio = ddlAnio.SelectedItem.Text;
            intMes = Convert.ToInt16(ddlMes.SelectedIndex + 1);
            strMes = (ddlMes.SelectedIndex + 1).ToString("D2");
            strFechaInicio = strDiaInicio + "/" + strMes + "/" + strAnio;
           

            if (intMes == 12){
                intMes = 1;
                strAnio =  ""+(Int32.Parse(strAnio) + 1);
            }
            else{
                intMes++;
            }
            strMes = intMes.ToString("D2");
            strFechaFinal = strDiaInicio + "/" + strMes + "/" + strAnio;
            datFechaFin = Comun.FormatearFecha(strFechaFinal);
            datFechaFin = datFechaFin.AddDays(-1);

            dtpFecInicio.Text = Comun.FormatearFecha(strFechaInicio).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
            dtpFecFin.Text = Comun.FormatearFecha(datFechaFin.ToShortDateString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
        }

        //------------------------------------------------------
        //Fecha: 12/05/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Exportar e Imprimir el tipo de reporte:
        //        Rendición de Cuentas por Tarifas RENIEC.
        //Documento: OBSERVACIONES_SGAC_12052021.doc
        //------------------------------------------------------
        protected void btnExportar_Click(object sender, EventArgs e)
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
            if (ddlLibroContableTipo.Visible == true)
            {
                if (ddlLibroContableTipo.SelectedIndex == 0)
                {
                    ctrlValidacion.MostrarValidacion("Seleccion el tipo de libro contable", true, Enumerador.enmTipoMensaje.ERROR);
                    return;
                }
            }
            if (ddlLibroContableTipo.SelectedValue == Convert.ToString((int)Enumerador.enmLibroContable.RENIEC))

            {
                if (ddlTipoReporteReniec.SelectedIndex == 0)
                {
                    ctrlValidacion.MostrarValidacion("Seleccion el tipo de Reporte de RENIEC", true, Enumerador.enmTipoMensaje.ERROR);
                    return;
                }
                string strTipoReporteReniec = ddlTipoReporteReniec.SelectedItem.Text.Trim();
                if (strTipoReporteReniec == SGAC.Accesorios.Constantes.CONST_RENIEC_RENDICION_CUENTAS)
                {

                    Session["FechaIntervalo"] = " Del Periodo: " + ddlAnio.SelectedItem.Text + "-" + ddlMes.SelectedItem.Text;
                    Session["IdOficinaConsular_contabilidad"] = ctrlOficinaConsular.SelectedValue;

                    ctrlValidacion.MostrarValidacion("", false);
                    DataSet ds = new DataSet();
                    bool bolVistaPrevia = false;
                    int sReferenciaIdLima = 0;
                    Session["NumeroGuia"] = "";

                    // Información del reporte
                    object[] arrParametros = ObtenerFiltro(false);
                    Session["SP_PARAMETROS"] = arrParametros;

                    // Indicadores
                    bool bolEsLima = false;
                    bool bolEsJefatura = false;

                    // Verificar si Lima es la Misión Logeada
                    #region Verificar si Lima es la Misión Logeada
                    if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
                        bolEsLima = true;
                    if (bolEsLima)
                    {
                        // 1. Verificar si Misión Seleccionada es Lima
                        if (Convert.ToInt32(ctrlOficinaConsular.SelectedValue) == Constantes.CONST_OFICINACONSULAR_LIMA)
                            bolEsLima = true;

                        // 2. Verificar si misión Seleccionada es Jefatura
                        // 2.1. Obtener Id Misión seleccionada
                        int intOficinaConsularSel = Convert.ToInt32(ctrlOficinaConsular.SelectedValue);
                        // 2.2. Obtener Oficinas Consulares
                        DataTable dtOficinasConsulares = new DataTable();

                        dtOficinasConsulares = Comun.ObtenerOficinasConsularesCargaInicial();

                        //dtOficinasConsulares = (DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULAR];
                        // 2.3. Obtener Datos de Misión Seleccionada
                        DataView dv = dtOficinasConsulares.DefaultView;
                        dv.RowFilter = "ofco_sOficinaConsularId = " + intOficinaConsularSel;
                        // 2.4. Indicador de Jefatura
                        DataTable dtMisionSel = dv.ToTable();

                        //-----------------------------------------------------------
                        //Fecha: 12/05/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Validar cuando el datatable este vacio.
                        //-----------------------------------------------------------
                        if (dtMisionSel.Rows.Count > 0)
                        {
                            bolEsJefatura = Convert.ToBoolean(Convert.ToInt32(dtMisionSel.Rows[0]["ofco_iJefaturaFlag"]));
                        }
                    }
                    else
                    {
                        DataTable dtOficinaConsular = new DataTable();
                        dtOficinaConsular = Comun.ObtenerOficinaConsularPorId(Session);
                        //DataTable dtOficinaConsular = (DataTable)Session[Constantes.CONST_SESION_OFICINACONSULTA_DT];
                        bolEsJefatura = Convert.ToBoolean(Convert.ToInt32(dtOficinaConsular.Rows[0]["ofco_iJefaturaFlag"]));
                        sReferenciaIdLima = Convert.ToInt32(dtOficinaConsular.Rows[0]["ofco_iReferenciaPadreId"]);
                    }
                    #endregion

                    switch (Convert.ToInt32(ddlLibroContableTipo.SelectedValue))
                    {
                        case (int)Enumerador.enmLibroContable.RENIEC:
                            ds = ObtenerDataTableReporte(arrParametros);

                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                                    if (intCantidadRegistros > 0)
                                    {
                                        Session["dtDatos"] = ds.Tables[0];
                                        Session["NumeroGuia"] = "";

                                        Comun.VerVistaPrevia(Session, Page, ds, Enumerador.enmReporteContable.RENIEC_RENDICION_CUENTAS_NOHEAD);
                                        bolVistaPrevia = true;
                                    }
                                }
                            }
                            if (!bolVistaPrevia)
                            {
                                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA, true, Enumerador.enmTipoMensaje.INFORMATION);
                            }
                            break;
                    }
                }


            }

        }
//-------------------------------------------------
    }
}
