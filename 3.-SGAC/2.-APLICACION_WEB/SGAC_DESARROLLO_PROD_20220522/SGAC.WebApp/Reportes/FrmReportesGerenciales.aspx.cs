using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.Controlador;
using SGAC.Reportes.BL;
using System.Data;
using SGAC.WebApp.Accesorios;
using System.Configuration;
using SGAC.Contabilidad.Reportes.BL;
using SGAC.Configuracion.Sistema.BL;
using SGAC.Configuracion.Seguridad.BL;
namespace SGAC.WebApp.Reportes
{
    public partial class FrmReportesGerenciales : MyBasePage
    {
        Boolean bolVistaPrevia = false;
        Boolean bolReporteContable = false;   

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlOficinaConsular.ddlOficinaConsular.AutoPostBack = true;
            ctrlOficinaConsular.ddlOficinaConsular.SelectedIndexChanged += new EventHandler(ddlOficinaConsular_SelectedIndexChanged);
            
            String strFormatoFechas = String.Empty;
            String strFormatoFechasInicio = String.Empty;

            strFormatoFechas = ConfigurationManager.AppSettings["FormatoFechas"].ToString();
            strFormatoFechasInicio = ConfigurationManager.AppSettings["FormatoFechasInicio"].ToString();

            if (!IsPostBack) {
                try
                {
                    llenarOficinasActivas();
                    this.dtpFecInicio.StartDate = new DateTime(1900, 1, 1);
                    this.dtpFecInicio.EndDate = DateTime.Today;
                    this.dtpFecInicio.EndDate = DateTime.Now;
                    this.dtpFecInicio.Text = DateTime.Now.ToString(strFormatoFechasInicio);

                    this.dtpFecFin.StartDate = new DateTime(1900, 1, 1);
                    this.dtpFecFin.EndDate = DateTime.Today;
                    this.dtpFecFin.EndDate = DateTime.Now;
                    dtpFecFin.Text = Comun.ObtenerFechaActualTexto(Session);

                    lblAnioMes.Visible = false;
                    ddlAnio.Visible = false;
                    ddlMes.Visible = false;
                    btnExportar.Visible = false;

                    lblDel.Visible = false;
                    lblAl.Visible = false;
                    ddlAnioHasta.Visible = false;
                    ddlMesHasta.Visible = false;

                    DesactivarTodo();
                    CargarListadosDesplegables();
                    SetearDireccion(sender, e);

                    //------------------------------------------------------------------------
                    // Autor: Miguel Angel Márquez Beltrán
                    // Fecha: 02/09/2016
                    // Objetivo: Si es consulado mostrar solo la oficina consular
                    //------------------------------------------------------------------------
                    ddlOficinaConsular_SelectedIndexChanged(sender, e);
                    Session["idtarifa_MRE"] = 0;
                    //---------------------------------------------------------------------------
                    //Fecha: 07/05/2021
                    //Autor: Miguel Màrquez Beltràn
                    //Motivo: Ocultar check de trámites sin vincular
                    //---------------------------------------------------------------------------
                    chk_TramitesSinVincular.Visible = false;
                    chk_TramitesSinVincular.Checked = false;
                    //---------------------------------------------------------------------------
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
        }
    
        public static void EjecutarScript(Page Page, string strScript)
        {
            strScript = string.Format(strScript);
            ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), "OpenPopup", strScript, true);
        }

        private void SetearDireccion(object sender, EventArgs e)
        {
            try
            {
                string strUbigeo = Session[Constantes.CONST_SESION_UBIGEO].ToString();
                string strDpto = strUbigeo.Substring(0, 2);
                string strProv = strUbigeo.Substring(2, 2);
                string strDist = strUbigeo.Substring(4, 2);

                CmbDptoContDir.SelectedValue = strDpto;
                CmbDptoContDir_SelectedIndexChanged(sender, e);
                CmbProvPaisDir.SelectedValue = strProv;
                CmbProvPaisDir_SelectedIndexChanged(sender, e);
                CmbDistCiuDir.SelectedValue = strDist;
                Session["CmbDistCiuDir"] = CmbDistCiuDir.SelectedValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CargarListadosDesplegables()
        {
            try
            {
                //JONATAN SILVA CACHAY 
                // 12/06/2017
                // SE AGREGA PARA CARGAR DATOS PARA EL REPORTE DE NACIONALIDAD

                FillWebCombo(comun_Part3.ObtenerDepartamentos(Session), CmbDptoContDir, "ubge_vDepartamento", "ubge_cUbi01");
                this.CmbProvPaisDir.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
                this.CmbDistCiuDir.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));


                //-------------------------------------------------------------
                //Fecha: 05/02/2020
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Cargar las tablas independientemente.
                //-------------------------------------------------------------
                Util.CargarParametroDropDownList(CmbEstCiv, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.ESTADO_CIVIL), true);

                Util.CargarParametroDropDownList(CmbOcupacion, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.OCUPACION), true);

                Util.CargarParametroDropDownList(ddl_Profesion, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.PROFESION), true);
                //-------------------------------------------------------------

                Util.CargarParametroDropDownList(CmbTipRes, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_TIPO_RESIDENCIA), true);
                Util.CargarParametroDropDownList(CmbGenero, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_GENERO), true);
                Util.CargarParametroDropDownList(CmbGradInst, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_GRADO_INSTRUCCION), true);
                Util.CargarParametroDropDownList(CmbNacRecurr, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_NACIONALIDAD), true);

                FiltrarCombo();
                DataTable dtTarifa = new DataTable();

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

                string valor = Convert.ToString(Request.QueryString["Rep"]);
                if (valor == "ARQUEO")
                {
                    lblTituloLibroContableRpt.Text = "Reporte de cuentas consulares:";
                    Util.CargarParametroDropDownList(ddlReportesGerenciales, comun_Part1.ObtenerParametrosPorGrupo(Session, SGAC.Accesorios.Constantes.CONST_REPORTE_ARQUEO), true);
                }
                else
                {
                    lblTituloLibroContableRpt.Text = "Reportes Gerenciales:";
                    DataTable dtReportesGerenciales = new DataTable();
                    dtReportesGerenciales = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_REPORTE_GERENCIAL);


                    if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Constantes.CONST_OFICINACONSULAR_LIMA)
                    {
                        //-------------------------------------------------------------
                        //Fecha: 08/11/2019
                        //Modificado por: Miguel Márquez Beltrán
                        //Motivo: Cuando se removia mas de un item presentaba error
                        //-------------------------------------------------------------
                        int intConsolidado_ACtuaciones_Tipo_Pago = (int)Enumerador.enmReportesGerenciales.CONSOLIDADO_ACTUACIONES_TIPO_PAGO;


                        RemoverFilas(ref dtReportesGerenciales, "id", intConsolidado_ACtuaciones_Tipo_Pago.ToString(), true);
                        RemoverFilas(ref dtReportesGerenciales, "descripcion", Constantes.CONST_REPORTES_RANKING_RECAUDACION, false);
                        RemoverFilas(ref dtReportesGerenciales, "descripcion", Constantes.CONST_REPORTES_RANKING_CAPTACION, false);
                        RemoverFilas(ref dtReportesGerenciales, "descripcion", Constantes.CONST_REPORTES_CUADRO_SALDOS_AUTOADHESIVOS, false);
                        RemoverFilas(ref dtReportesGerenciales, "descripcion", Constantes.CONST_REPORTES_CUADRO_AUTOADHESIVOS_UTILIZADOS, false);

                        #region Comentado
  
                        #endregion
                    }
                    else
                    {
                        Util.CargarComboAnios(ddlAnio, 2015, DateTime.Now.Year);
                        Util.CargarDropDownList(ddlMes, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONFIG_MES), "valor", "id");
                    }

                    Util.CargarComboAnios(ddlAnioConsulta, 2015, DateTime.Now.Year);
                    ddlAnioConsulta.SelectedValue = DateTime.Now.Year.ToString();
                    Util.CargarParametroDropDownList(ddlReportesGerenciales, dtReportesGerenciales, true);
                }

                ParametroConsultasBL _obj = new ParametroConsultasBL();
                DataTable _dtTipoPago = new DataTable();
                DataTable _dtTipoPagoExoneracion = new DataTable();
                _dtTipoPago = _obj.ConsultarParametroPorValor("ACREDITACIÓN-TIPO COBRO", "", "- TODOS -");
                _dtTipoPagoExoneracion = _obj.ConsultarParametroPorValor("ACREDITACIÓN-TIPO COBRO", "", "", "E", "EXONERACION POR INDIGENCIA");
                _dtTipoPago.Merge(_dtTipoPagoExoneracion);
                Util.CargarParametroDropDownListDATATABLE(ddlTipoPago, _dtTipoPago, "para_vDescripcion", "para_sParametroId");

                Util.CargarParametroDropDownList(ddlCategoriaOficina, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONFIG_CATEGORIA_OFICINA_CONSULAR), true);
                Util.CargarParametroDropDownList(ddlClasificacion, comun_Part1.ObtenerParametrosPorGrupo(Session, SGAC.Accesorios.Constantes.CONST_ACTUACION_CLASIFICACION_TARIFA), true);
                FillWebCombo(comun_Part3.ObtenerContinente(Session), ddlContinente, "ubge_vDepartamento", "ubge_cUbi01");

                if (Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Convert.ToInt16(Constantes.CONST_ID_CONSULADO_CARACAS))
                {
                    ddlTipoPago.Items.FindByText("PAGO ARUBA").Enabled = false;
                    ddlTipoPago.Items.FindByText("PAGO OTRAS ISLAS CARIBEÑAS").Enabled = false;
                }

                //------------------------------------------------
                //Fecha: 24/10/2016
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Mostrar todas las tarifas consulares
                //------------------------------------------------
                object[] arrParametros = { 0, "", 
                                       "", 
                                       ((char)Enumerador.enmEstado.ACTIVO).ToString(),
                                       1, 500, 0, 0 };

                dtTarifa = comun_Part2.ObtenerTarifarioConsulta(Session, ref arrParametros);
                //---------------------------------------------

                FillWebCombo(dtTarifa, ddlTarifa, "tari_vDescripcionCorta", "tari_sTarifarioId", "- TODOS -");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FiltrarCombo()
        {
            //------------------------------------------------
            //Fecha: 01/09/2017
            //Autor: Jonatan Silva Cachay
            //Objetivo: Filtrar Combo
            //------------------------------------------------
            DataTable dtEstadoInsumo = new DataTable();
            dtEstadoInsumo = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmEstadoGrupo.INSUMO);

            Util.CargarParametroDropDownList(ddlEstAutoadhesivo, dtEstadoInsumo, true);
            ddlEstAutoadhesivo.Items.Remove(ddlEstAutoadhesivo.Items.FindByText("RESERVADO"));
            ddlEstAutoadhesivo.Items.Remove(ddlEstAutoadhesivo.Items.FindByText("ANULADO"));
            ddlEstAutoadhesivo.Items.Remove(ddlEstAutoadhesivo.Items.FindByText("ASIGNADO"));
        }
        void FillWebCombo(DataTable pDataTable, DropDownList pWebCombo, String str_pDescripcion, String str_pValor, string strItemTexto="")
        {
            pWebCombo.Items.Clear();
            pWebCombo.DataSource = pDataTable;
            pWebCombo.DataTextField = str_pDescripcion;
            pWebCombo.DataValueField = str_pValor;
            pWebCombo.DataBind();

            if (strItemTexto.Length == 0)
            {
                pWebCombo.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
            }
            else
            {
                pWebCombo.Items.Insert(0, new ListItem(strItemTexto, "00"));
            }

        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            bool esUsuarioValido = ddlusuario.SelectedValue == "0" ? false : true;
            bool esAdhesivoValido = ddlEstAutoadhesivo.SelectedValue == "0" ? false : true;
            bool esReporteValido = ddlReportesGerenciales.SelectedValue == "5006" ? true : false;

            if (esReporteValido) { 
                if (!esUsuarioValido && !esAdhesivoValido)
                {
                    return;
                }            
            }
                        
            Impresion("S");                        
        }

        void llenarComboUsuarios(int sOficinaConsularId, string cabecera)
        {
            UsuarioConsultasBL obj = new UsuarioConsultasBL();
            DataTable dt = obj.ObtenerLista(sOficinaConsularId);
            Util.CargarDropDownList(ddlusuario, dt, "usua_vAlias", "usua_sUsuarioId", true, cabecera);
        } 

        void ddlOficinaConsular_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int sOficinaConsularId = 0;

                sOficinaConsularId = Convert.ToInt16(ctrlOficinaConsular.SelectedValue);
                object[] arrParametros = { sOficinaConsularId };

                //Proceso p = new Proceso();
                //DataTable dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.SE_USUARIO", "LISTAR");
                llenarComboUsuarios(sOficinaConsularId, "- TODOS -");
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

        private void VerVistaPrevia() 
        {
            DateTime fechainicio = DateTime.Now;
            DateTime fechafin = DateTime.Now;
            Int16? idoficinaconsular;
            string Ordenado = string.Empty;
            string pagoLima = "N";

            if (rbMonto.Checked)
            {
                Ordenado = "M";
            }
            else if (rbPaisConsulado.Checked)
            {
                Ordenado = "C";
            }
            if (rdioPagoLima.Checked)
            {
                pagoLima = "S";
            }

            if (dtpFecInicio.Text != "") { fechainicio = Comun.FormatearFecha(dtpFecInicio.Text); }
            if (dtpFecFin.Text != "") { fechafin = Comun.FormatearFecha(dtpFecFin.Text); }
            
            Session["FechaIntervalo"] = " Del " + dtpFecInicio.Text + " al " + dtpFecFin.Text;
            Int16 intAnio = 0;
            if (ddlAnioConsulta.SelectedIndex > -1)
            {
                intAnio = Convert.ToInt16(ddlAnioConsulta.SelectedValue);
            }
            
            if (ddlReportesGerenciales.SelectedIndex > 0) {
                DataSet ds = new DataSet();
                ReportesGerencialesConsultaBL oReportesGerencialesConsultaBL = new ReportesGerencialesConsultaBL();

                switch (Convert.ToInt32(ddlReportesGerenciales.SelectedValue))
                {
                    case (int)Enumerador.enmReportesGerenciales.RGE_CONSOLIDADO: 
                        #region Obtener Datos Reporte                        
                        string idtarifaconsular = "";
                        Int16? idtipopago;
                        Int16? idcategoriaoficinaconsular;
                        string strContinente = string.Empty;
                        string strPais = string.Empty;
                        Int16? intUsuarioId = null;

                        if (ddlContinente.SelectedValue.ToString() == "00")
                            strContinente = null;
                        else
                            strContinente = ddlContinente.SelectedValue;

                        if (ddlPais.Items.Count == 0)
                            strPais = null;
                        else
                        {
                            if (ddlPais.SelectedIndex == 0)
                                strPais = null;
                            else
                                strPais = ddlPais.SelectedValue;
                        }

                        if (ddlCategoriaOficina.SelectedValue.ToString() == "0")
                            idcategoriaoficinaconsular = null;
                        else
                            idcategoriaoficinaconsular = Convert.ToInt16(ddlCategoriaOficina.SelectedValue.ToString());

                        if (ctrlOficinaConsular.SelectedValue.ToString()=="0")
                            idoficinaconsular  = null;
                        else
                            idoficinaconsular  = Convert.ToInt16(ctrlOficinaConsular.SelectedValue.ToString());
                        
                        if (ddlTarifa.SelectedValue.ToString() == "")
                            idtarifaconsular = ""; 
                        else
                            idtarifaconsular = ddlTarifa.SelectedValue.ToString();  

                        if (ddlTipoPago.SelectedValue.ToString() == "0")
                            idtipopago = null;
                        else
                            idtipopago = Convert.ToInt16(ddlTipoPago.SelectedValue.ToString());

                        if (ddlusuario.SelectedIndex > 0)
                            intUsuarioId = Convert.ToInt16(ddlusuario.SelectedValue.ToString());

                        ds = oReportesGerencialesConsultaBL.ObtenerConsolidado(strContinente, strPais, idcategoriaoficinaconsular, idoficinaconsular, idtarifaconsular, idtipopago, fechainicio, fechafin, intUsuarioId);
                        
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                                if (intCantidadRegistros > 0)
                                {
                                    Session["dtDatos"] = ds.Tables[0];
                                    Session[Constantes.CONST_SESION_REPORTE_TIPO] = Enumerador.enmReportesGerenciales.RGE_CONSOLIDADO;
                                    bolVistaPrevia = true;
                                }
                            }
                        }
                        #endregion
                        break;

                    case (int)Enumerador.enmReportesGerenciales.RECORD_DE_VENTA: 
                        #region Obtener Datos Reporte
                        Int16? idUsuario;

                        if (ctrlOficinaConsular.SelectedValue.ToString() == "0")
                        {
                            idoficinaconsular = null;
                        }
                        else
                        {
                            idoficinaconsular = Convert.ToInt16(ctrlOficinaConsular.SelectedValue.ToString());
                        }

                        if ((ddlusuario.SelectedValue.ToString() == "0") || (ddlusuario.SelectedValue.ToString() == ""))
                        {
                            idUsuario = null;
                        }
                        else
                        {
                            idUsuario = Convert.ToInt16(ddlusuario.SelectedValue.ToString());
                        }
                        
                        ds = oReportesGerencialesConsultaBL.ObtenerRecordVenta(idUsuario, idoficinaconsular, fechainicio, fechafin);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                                if (intCantidadRegistros > 0)
                                {
                                    Session["dtDatos"] = ds.Tables[0];
                                    Session[Constantes.CONST_SESION_REPORTE_TIPO] = Enumerador.enmReportesGerenciales.RECORD_DE_VENTA;
                                    bolVistaPrevia = true;
                                }
                            }
                        }
                        #endregion
                        break;

                    case (int)Enumerador.enmReportesGerenciales.RGE_POR_CATEGORIA: 
                        #region Obtener Datos Reporte
                        ds = oReportesGerencialesConsultaBL.ObtenerRGExCategoria(fechainicio, fechafin);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                                if (intCantidadRegistros > 0)
                                {
                                    Session["dtDatos"] = ds.Tables[0];
                                    Session[Constantes.CONST_SESION_REPORTE_TIPO] = Enumerador.enmReportesGerenciales.RGE_POR_CATEGORIA;
                                    bolVistaPrevia = true;
                                }
                            }
                        }
                        #endregion
                        break;

                    case (int)Enumerador.enmReportesGerenciales.MAYOR_VENTA_Y_DETALLE: 
                        #region Obtener Datos Reporte
                        ds = oReportesGerencialesConsultaBL.ObtenerMayorVentaDetalle(fechainicio, fechafin);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                                if (intCantidadRegistros > 0)
                                {
                                    Session["dtDatos"] = ds.Tables[0];
                                    Session["dtDatos1"] = ds.Tables[1];
                                    Session[Constantes.CONST_SESION_REPORTE_TIPO] = Enumerador.enmReportesGerenciales.MAYOR_VENTA_Y_DETALLE;
                                    bolVistaPrevia = true;
                                }
                            }
                        }
                        #endregion
                    break;

                    case (int)Enumerador.enmReportesGerenciales.VENTAS_POR_MES: 
                    #region Obtener Datos Reporte

                    Int16 intOficinaConsularSel = Convert.ToInt16(ctrlOficinaConsular.SelectedValue);
                    ds = oReportesGerencialesConsultaBL.ObtenerVentaMes(intOficinaConsularSel, fechainicio, fechafin);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                            if (intCantidadRegistros > 0)
                            {
                                Session["dtDatos"] = ds.Tables[0];
                                Session[Constantes.CONST_SESION_REPORTE_TIPO] = Enumerador.enmReportesGerenciales.VENTAS_POR_MES;
                                bolVistaPrevia = true;
                            }
                        }
                    }
                    #endregion
                    break;

                    case (int)Enumerador.enmReportesGerenciales.TARIFA_CONSULAR_POR_PAIS: 
                    #region Obtener Datos Reporte

                    string tari_sTarifarioId = "" ;

                    if (ddlTarifa.SelectedValue.ToString() == "00")
                    {
                        tari_sTarifarioId = null;
                    }
                    else
                    {
                        tari_sTarifarioId = ddlTarifa.SelectedValue.ToString();
                    }

                    ds = oReportesGerencialesConsultaBL.ObtenerTarifaConsularPaís(tari_sTarifarioId, fechainicio, fechafin);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                            if (intCantidadRegistros > 0)
                            {
                                Session["dtDatos"] = ds.Tables[0];
                                Session[Constantes.CONST_SESION_REPORTE_TIPO] = Enumerador.enmReportesGerenciales.TARIFA_CONSULAR_POR_PAIS;
                                bolVistaPrevia = true;
                            }
                        }
                    }
                    #endregion
                    break;

                    case (int)Enumerador.enmReportesGerenciales.RECORD_DE_ACTUACIONES: 
                    #region Obtener Datos Reporte
                    ds = oReportesGerencialesConsultaBL.ObtenerRecordActuacion(fechainicio, fechafin);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                            if (intCantidadRegistros > 0)
                            {
                                Session["dtDatos"] = ds.Tables[0];
                                Session[Constantes.CONST_SESION_REPORTE_TIPO] = Enumerador.enmReportesGerenciales.RECORD_DE_ACTUACIONES;
                                bolVistaPrevia = true;
                            }
                        }
                    }
                    #endregion
                    break;

                    case (int)Enumerador.enmReportesGerenciales.TOP_14_MAYOR_VENTA_POR_PAIS: 
                    #region Obtener Datos Reporte
                    ds = oReportesGerencialesConsultaBL.ObtenerMayorVentaPais(fechainicio, fechafin);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                            if (intCantidadRegistros > 0)
                            {
                                Session["dtDatos"] = ds.Tables[0];
                                Session[Constantes.CONST_SESION_REPORTE_TIPO] = Enumerador.enmReportesGerenciales.TOP_14_MAYOR_VENTA_POR_PAIS;
                                bolVistaPrevia = true;
                            }
                        }
                    }
                    #endregion
                    break;

                    case (int)Enumerador.enmReportesGerenciales.RGE_POR_CONTIENENTE:
                    #region Obtener Datos Reporte

                    DateTime dFecInicio = dtpFecInicio.Value();
                    DateTime dFecFin = dtpFecFin.Value();

                    ds = oReportesGerencialesConsultaBL.ObtenerMayorVentaContinente(dFecInicio, dFecFin);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                            if (intCantidadRegistros > 0)
                            {
                                Session["dtDatos"] = ds.Tables[0];
                                Session[Constantes.CONST_SESION_REPORTE_TIPO] = Enumerador.enmReportesGerenciales.RGE_POR_CONTIENENTE;
                                bolVistaPrevia = true;
                            }
                        }
                    }
                    #endregion
                    break;

                    case (int)Enumerador.enmReportesGerenciales.RGE_POR_CATEGORIA_POR_RECORD_DE_VENTA: 
                    #region Obtener Datos Reporte
                    ds = oReportesGerencialesConsultaBL.ObtenerCategoriaVentas(fechainicio, fechafin);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                            if (intCantidadRegistros > 0)
                            {
                                Session["dtDatos"] = ds.Tables[0];
                                Session[Constantes.CONST_SESION_REPORTE_TIPO] = Enumerador.enmReportesGerenciales.RGE_POR_CATEGORIA_POR_RECORD_DE_VENTA;
                                bolVistaPrevia = true;
                            }
                        }
                    }
                    #endregion
                    break;

                    case (int)Enumerador.enmReportesGerenciales.AUTOADHESIVOS_USUARIO_OFICINA_CONSULAR:
                    #region Obtener Datos Reporte
                    int iOficinaConsularID = 0;
                    int iUsuarioId = 0;
                    Int16 sEstadoInsumo = 0;
                    if (ddlEstAutoadhesivo.SelectedIndex > 0)
                    {
                        sEstadoInsumo = Convert.ToInt16(ddlEstAutoadhesivo.SelectedValue.ToString());
                    }

                    if (ddlusuario.SelectedIndex > 0)
                        iUsuarioId = Convert.ToInt32(ddlusuario.SelectedValue.ToString());

                    if (ctrlOficinaConsular.SelectedValue.ToString() == "0")
                    { iOficinaConsularID = 0; }
                    else
                    { iOficinaConsularID = Convert.ToInt32(ctrlOficinaConsular.SelectedValue.ToString()); }

                    if (chkSinFecha.Checked)
                    {
                        fechainicio = Convert.ToDateTime("01/01/2015");
                        fechafin = DateTime.Now;
                        Session["FechaIntervalo"] = " Todas las fechas";
                    }

                    ds = oReportesGerencialesConsultaBL.USP_REPORTE_AUTOADHESIVOS_USUARIO_OFICINACONSULAR(iOficinaConsularID, fechainicio, fechafin, iUsuarioId, sEstadoInsumo);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                            if (intCantidadRegistros > 0)
                            {
                                Session["dtDatos"] = ds.Tables[0];
                                Session[Constantes.CONST_SESION_REPORTE_TIPO] = Enumerador.enmReportesGerenciales.AUTOADHESIVOS_USUARIO_OFICINA_CONSULAR;
                                bolVistaPrevia = true;
                            }
                        }
                    }
                    #endregion
                    break;

                    case (int)Enumerador.enmReportesGerenciales.ACTUACIONES_USUARIO_OFICINA_CONSULAR:
                    #region Obtener Datos Reporte
                        int idtipopagoAct  = 0;
                        int intUsuarioIdAct = 0;
                        int iOficinaConsularIdAct = 0;
                        int intTarifaId = 0;
                        int intClasificacionId = 0;
                        //----------------------------------------------------
                        //Fecha: 07/05/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Inicializar variable Trámite sin Vincular.
                        //Requerimiento: OBSERVACIONES_SGAC_06052021.doc 
                        //                 Item 1.
                        //----------------------------------------------------
                        bool bTramiteSinVincular = false;

                        //--------------------------------------------------
                        //Fecha: 19/10/2016
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: La impresión de actuaciones por tarifa
                        //--------------------------------------------------
                        if (ctrlOficinaConsular.SelectedValue.ToString() == "0")
                            iOficinaConsularIdAct = 0;
                        else
                            iOficinaConsularIdAct = Convert.ToInt32(ctrlOficinaConsular.SelectedValue.ToString());

                        if (ddlTipoPago.SelectedValue.ToString() == "0")
                            idtipopagoAct = 0;
                        else
                            idtipopagoAct = Convert.ToInt32(ddlTipoPago.SelectedValue.ToString());

                        if (ddlusuario.SelectedIndex > 0)
                            intUsuarioIdAct = Convert.ToInt32(ddlusuario.SelectedValue.ToString());
                        if (ddlTarifa.SelectedIndex > 0)
                            intTarifaId = Convert.ToInt32(ddlTarifa.SelectedValue.ToString());

                        //----------------------------------------------------
                        //Fecha: 07/05/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Asignar valor a la variable: bTramiteSinVincular
                        //Requerimiento: OBSERVACIONES_SGAC_06052021.doc 
                        //                 Item 1.
                        //----------------------------------------------------
                        bTramiteSinVincular = chk_TramitesSinVincular.Checked;
                        //----------------------------------------------------
                        //----------------------------------------------------
                        Session["idtarifa_MRE"] = intTarifaId;
                        int intTipoRol = Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]);
                        //------------------------------------------------
                        //Fecha: 29/12/2016
                        //Autor: Jonatan Silva Cachay
                        //Objetivo: Mostrar Clasificación de Tarifa, para la tarifa 20B
                        //------------------------------------------------
                        Session["Clasificacion"] = " ";
                        if (intTarifaId == Constantes.CONST_PROTOCOLAR_ID_TARIFA_20B) //TARIFA 20B
                        {
                            if (ddlClasificacion.SelectedIndex > 0)
                            {
                                intClasificacionId = Convert.ToInt32(ddlClasificacion.SelectedValue.ToString());
                                Session["Clasificacion"] = " - " + ddlClasificacion.SelectedItem.Text.ToString();
                                //----------------------------------------------------
                                //Fecha: 07/05/2021
                                //Autor: Miguel Márquez Beltrán
                                //Motivo: Asignar el parametro bTramiteSinVincular.
                                //Requerimiento: OBSERVACIONES_SGAC_06052021.doc  Item 1.
                                //----------------------------------------------------
                                ds = oReportesGerencialesConsultaBL.USP_REPORTE_ACTUACIONES_USUARIO_OFICINACONSULAR_PARATARIFA20B(iOficinaConsularIdAct, fechainicio, fechafin, intUsuarioIdAct, idtipopagoAct, intTipoRol, intTarifaId, intClasificacionId, bTramiteSinVincular);
                            }
                            else {
                                //----------------------------------------------------
                                //Fecha: 07/05/2021
                                //Autor: Miguel Márquez Beltrán
                                //Motivo: Asignar el parametro bTramiteSinVincular.
                                //Requerimiento: OBSERVACIONES_SGAC_06052021.doc  Item 1.
                                //----------------------------------------------------
                                ds = oReportesGerencialesConsultaBL.USP_REPORTE_ACTUACIONES_USUARIO_OFICINACONSULAR(iOficinaConsularIdAct, fechainicio, fechafin, intUsuarioIdAct, idtipopagoAct, intTipoRol, intTarifaId, bTramiteSinVincular);
                            }
                        }
                        else {
                            //----------------------------------------------------
                            //Fecha: 07/05/2021
                            //Autor: Miguel Márquez Beltrán
                            //Motivo: Asignar el parametro bTramiteSinVincular.
                            //Requerimiento: OBSERVACIONES_SGAC_06052021.doc  Item 1.
                            //----------------------------------------------------
                            ds = oReportesGerencialesConsultaBL.USP_REPORTE_ACTUACIONES_USUARIO_OFICINACONSULAR(iOficinaConsularIdAct, fechainicio, fechafin, intUsuarioIdAct, idtipopagoAct, intTipoRol, intTarifaId, bTramiteSinVincular);
                        }

                        
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                            if (intCantidadRegistros > 0)
                            {

                                string TipoPago = "Todos", Usuarios = "Todos", Tarifa = "Todos", Clasificacion = "-";

                                if (ddlTipoPago.SelectedIndex > 0)
                                {
                                    TipoPago = ddlTipoPago.SelectedItem.Text;
                                }
                                if (ddlusuario.SelectedIndex > 0)
                                {
                                    Usuarios = ddlusuario.SelectedItem.Text;
                                }
                                if (ddlTarifa.SelectedIndex > 0)
                                {
                                    Tarifa = ddlTarifa.SelectedItem.Text;
                                }
                                if (ddlClasificacion.SelectedIndex > 0)
                                {
                                    Clasificacion = ddlClasificacion.SelectedItem.Text;
                                }

                                Session["ParametrosReporte"] = TipoPago + "|" + Usuarios + "|" + Tarifa + "|" + Clasificacion ;

                                Session["dtDatos"] = ds.Tables[0];
                                Session[Constantes.CONST_SESION_REPORTE_TIPO] = Enumerador.enmReportesGerenciales.ACTUACIONES_USUARIO_OFICINA_CONSULAR;
                                bolVistaPrevia = true;
                            }
                        }
                    }
                    #endregion
                    break;
                    //------------------------------------------------
                    //Fecha: 29/12/2016
                    //Autor: Jonatan Silva Cachay
                    //Objetivo: Obtener Datos de Reporte de Personas
                    //------------------------------------------------
                    case (int)Enumerador.enmReportesGerenciales.PERSONAS_USUARIO_OFICINA_CONSULAR:
                    #region Obtener Datos Reporte
                        int intOficinaConsularID = 0;
                    //int iCodUsuarioId = 0;

                    //if (ddlusuario.SelectedIndex > 0)
                    //{
                    //    iCodUsuarioId = Convert.ToInt32(ddlusuario.SelectedValue.ToString());
                    //}
                    if (ctrlOficinaConsular.SelectedValue.ToString() == "0")
                    {
                        intOficinaConsularID = 0;
                    }
                    else
                    {
                        intOficinaConsularID = Convert.ToInt32(ctrlOficinaConsular.SelectedValue.ToString());
                    }
                    
                    // Nuevos Parametros
                    // JOnatan Silva Cachay
                    // 12/06/2017

                    Int16 EstadoCivil = 0;
                    Int16 GeneroId = 0;                        
                    string CodigoPostal = "";
                    Int16 OcupacionId = 0;
                    Int16 ProfesionId = 0;
                    Int16 GradoInstruccionID = 0;
                    bool BuscarResidencia = false;
                    Int16 TipoResidencia = 0;
                    string ResidenciaUbigeo = "";
                    Int16 Nacionalidad = 0;

                    EstadoCivil = Convert.ToInt16(CmbEstCiv.SelectedValue);
                    GeneroId = Convert.ToInt16(CmbGenero.SelectedValue);
                    CodigoPostal = txtCodPostal.Text;
                    OcupacionId = Convert.ToInt16(CmbOcupacion.SelectedValue);
                    ProfesionId = Convert.ToInt16(ddl_Profesion.SelectedValue);
                    GradoInstruccionID = Convert.ToInt16(CmbGradInst.SelectedValue);
                    BuscarResidencia = chkDireccion.Checked;
                    Nacionalidad = Convert.ToInt16(CmbNacRecurr.SelectedValue);

                    if (BuscarResidencia)
                    {
                        TipoResidencia = Convert.ToInt16(CmbTipRes.SelectedValue);
                        ResidenciaUbigeo = CmbDptoContDir.SelectedValue.ToString() + CmbProvPaisDir.SelectedValue.ToString() + CmbDistCiuDir.SelectedValue.ToString();
                    }
                    else {
                        TipoResidencia = 0;
                        ResidenciaUbigeo = "";
                    }

                    ds = oReportesGerencialesConsultaBL.USP_REPORTE_PERSONAS_USUARIO_OFICINACONSULAR(intOficinaConsularID, fechainicio, fechafin, 
                        EstadoCivil, GeneroId, CodigoPostal, OcupacionId, ProfesionId, GradoInstruccionID, BuscarResidencia, TipoResidencia, ResidenciaUbigeo, Nacionalidad);

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                            if (intCantidadRegistros > 0)
                            {
                                Session["dtDatos"] = ds.Tables[0];

                                string EstCiv = "-", CodPostal = "-", Profesión = "-", Genero = "-", Ocupacion = "-", GradoInstru = "-";

                                if (CmbEstCiv.SelectedIndex > 0)
                                {
                                    EstCiv = CmbEstCiv.SelectedItem.Text;
                                }
                                if (txtCodPostal.Text.Length > 0)
                                {
                                    CodPostal = txtCodPostal.Text;
                                }
                                if (ddl_Profesion.SelectedIndex > 0)
                                {
                                    Profesión = ddl_Profesion.SelectedItem.Text;
                                }
                                if (CmbGenero.SelectedIndex > 0)
                                {
                                    Genero = CmbGenero.SelectedItem.Text;
                                }
                                if (CmbOcupacion.SelectedIndex > 0)
                                {
                                    Ocupacion = CmbOcupacion.SelectedItem.Text;
                                }
                                if (CmbGradInst.SelectedIndex > 0)
                                {
                                    GradoInstru = CmbGradInst.SelectedItem.Text;
                                }
                                Session["ParametrosReporte"] = EstCiv + "|" + CodPostal + "|" + Profesión + "|" + Genero + "|" + Ocupacion + "|" + GradoInstru;
                                Session[Constantes.CONST_SESION_REPORTE_TIPO] = Enumerador.enmReportesGerenciales.PERSONAS_USUARIO_OFICINA_CONSULAR;
                                bolVistaPrevia = true;
                            }
                        }
                    }
                    #endregion
                    break;
                    //------------------------------------------------
                    //Fecha: 29/12/2016
                    //Autor: Jonatan Silva Cachay
                    //Objetivo: Obtener Datos de Reporte de Cantidad actuaciones
                    //------------------------------------------------
                    case (int)Enumerador.enmReportesGerenciales.CANTIDAD_ACTUACIONES_CONSULADO:
                    #region Obtener Datos Reporte
                    string stridtarifaconsular = "";
                    Int16? Intidtipopago;
                    Int16? Intidcategoriaoficinaconsular;
                    string strNomContinente = string.Empty;
                    string strNomPais = string.Empty;
                    Int16? intCodUsuarioId = null;

                    if (ddlContinente.SelectedValue.ToString() == "00")
                        strNomContinente = null;
                    else
                        strNomContinente = ddlContinente.SelectedValue;

                    if (ddlPais.Items.Count == 0)
                        strNomPais = null;
                    else
                    {
                        if (ddlPais.SelectedIndex == 0)
                            strNomPais = null;
                        else
                            strNomPais = ddlPais.SelectedValue;
                    }

                    if (ddlCategoriaOficina.SelectedValue.ToString() == "0")
                        Intidcategoriaoficinaconsular = null;
                    else
                        Intidcategoriaoficinaconsular = Convert.ToInt16(ddlCategoriaOficina.SelectedValue.ToString());

                    if (ctrlOficinaConsular.SelectedValue.ToString() == "0")
                        idoficinaconsular = null;
                    else
                        idoficinaconsular = Convert.ToInt16(ctrlOficinaConsular.SelectedValue.ToString());

                    if (ddlTarifa.SelectedValue.ToString() == "")
                        stridtarifaconsular = "";
                    else
                        stridtarifaconsular = ddlTarifa.SelectedValue.ToString();

                    if (ddlTipoPago.SelectedValue.ToString() == "0")
                        Intidtipopago = null;
                    else
                        Intidtipopago = Convert.ToInt16(ddlTipoPago.SelectedValue.ToString());

                    if (ddlusuario.SelectedIndex > 0)
                        intCodUsuarioId = Convert.ToInt16(ddlusuario.SelectedValue.ToString());

                    ds = oReportesGerencialesConsultaBL.ObtenerCantidadActuacionesPorConsulado(strNomContinente, strNomPais, Intidcategoriaoficinaconsular, idoficinaconsular, stridtarifaconsular, Intidtipopago, fechainicio, fechafin, intCodUsuarioId);

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                            if (intCantidadRegistros > 0)
                            {
                                string Usuario = "-", TipoPago = "-", Tarifa = "-", Clasificacion = "-";

                                if (ddlusuario.SelectedIndex > 0)
                                {
                                    Usuario = ddlusuario.SelectedItem.Text;
                                }
                                if (ddlTipoPago.SelectedIndex > 0)
                                {
                                    TipoPago = ddlTipoPago.SelectedItem.Text;
                                }
                                if (ddlTarifa.SelectedIndex > 0)
                                {
                                    Tarifa = ddlTarifa.SelectedItem.Text;
                                }
                                if (ddlClasificacion.SelectedIndex > 0)
                                {
                                    Clasificacion = ddlClasificacion.SelectedItem.Text;
                                }
                                Session["ParametrosReporte"] = Usuario + "|" + TipoPago + "|" + Tarifa + "|" + Clasificacion;
                                Session["dtDatos"] = ds.Tables[0];
                                Session[Constantes.CONST_SESION_REPORTE_TIPO] = Enumerador.enmReportesGerenciales.CANTIDAD_ACTUACIONES_CONSULADO;
                                bolVistaPrevia = true;
                            }
                        }
                    }
                    #endregion
                    break;
                    //------------------------------------------------
                    //Fecha: 29/12/2016
                    //Autor: Jonatan Silva Cachay
                    //Objetivo: Obtener Datos de Reporte de Cantidad tarifas
                    //------------------------------------------------
                    case (int)Enumerador.enmReportesGerenciales.CANTIDAD_TARIFAS_REGISTRADAS:
                    #region Obtener Datos Reporte
                    string stridtarifaconsularrep = "";
                    Int16? Intidtipopagorep;
                    Int16? Intidcategoriaoficinaconsularrep;
                    string strNomContinenterep = string.Empty;
                    string NomPais = string.Empty;
                    Int16? intCodUsuario = null;

                    if (ddlContinente.SelectedValue.ToString() == "00")
                        strNomContinenterep = null;
                    else
                        strNomContinenterep = ddlContinente.SelectedValue;

                    if (ddlPais.Items.Count == 0)
                        NomPais = null;
                    else
                    {
                        if (ddlPais.SelectedIndex == 0)
                            NomPais = null;
                        else
                            NomPais = ddlPais.SelectedValue;
                    }
                         
                    if (ddlCategoriaOficina.SelectedValue.ToString() == "0")
                        Intidcategoriaoficinaconsularrep = null;
                    else
                        Intidcategoriaoficinaconsularrep = Convert.ToInt16(ddlCategoriaOficina.SelectedValue.ToString());

                    if (ctrlOficinaConsular.SelectedValue.ToString() == "0")
                        idoficinaconsular = null;
                    else
                        idoficinaconsular = Convert.ToInt16(ctrlOficinaConsular.SelectedValue.ToString());

                    if (ddlTarifa.SelectedValue.ToString() == "")
                        stridtarifaconsularrep = "";
                    else
                        stridtarifaconsularrep = ddlTarifa.SelectedValue.ToString();

                    if (ddlTipoPago.SelectedValue.ToString() == "0")
                        Intidtipopagorep = null;
                    else
                        Intidtipopagorep = Convert.ToInt16(ddlTipoPago.SelectedValue.ToString());

                    if (ddlusuario.SelectedIndex > 0)
                        intCodUsuario = Convert.ToInt16(ddlusuario.SelectedValue.ToString());

                    ds = oReportesGerencialesConsultaBL.ObtenerCantidadMaximaTarifas(strNomContinenterep, NomPais, Intidcategoriaoficinaconsularrep, idoficinaconsular, stridtarifaconsularrep, Intidtipopagorep, fechainicio, fechafin, intCodUsuario);

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                            if (intCantidadRegistros > 0)
                            {
                                string Usuario = "-", TipoPago = "-", Tarifa = "-", Clasificacion = "-";

                                if (ddlusuario.SelectedIndex > 0)
                                {
                                    Usuario = ddlusuario.SelectedItem.Text;
                                }
                                if (ddlTipoPago.SelectedIndex > 0)
                                {
                                    TipoPago = ddlTipoPago.SelectedItem.Text;
                                }
                                if (ddlTarifa.SelectedIndex > 0)
                                {
                                    Tarifa = ddlTarifa.SelectedItem.Text;
                                }
                                if (ddlClasificacion.SelectedIndex > 0)
                                {
                                    Clasificacion = ddlClasificacion.SelectedItem.Text;
                                }
                                Session["ParametrosReporte"] = Usuario + "|" + TipoPago + "|" + Tarifa + "|" + Clasificacion;
                                Session["dtDatos"] = ds.Tables[0];
                                Session[Constantes.CONST_SESION_REPORTE_TIPO] = Enumerador.enmReportesGerenciales.CANTIDAD_TARIFAS_REGISTRADAS;
                                bolVistaPrevia = true;
                            }
                        }
                    }
                    #endregion
                    break;
                    case (int)Enumerador.enmLibroContable.REPORTE_ACTUACIONES_ANULADAS:
                    #region Obtener Datos Reporte
                    // Información del reporte
                    object[] arrParametros = ObtenerFiltro(false);
                    Session["SP_PARAMETROS"] = arrParametros;

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
                                bolReporteContable = true;
                            }
                        }
                    }
                    if (!bolVistaPrevia)
                    {
                        ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA, true, Enumerador.enmTipoMensaje.INFORMATION);
                    }
                    #endregion
                    break;                
//------------------------------------------------
                    //Fecha: 26/06/2017
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Obtener Consolidado de actuaciones ppor tipo de pago
                    //------------------------------------------------
                    case (int)Enumerador.enmReportesGerenciales.CONSOLIDADO_ACTUACIONES_TIPO_PAGO:
                    #region Obtener Datos Reporte

                        Int16 intOficinaConsularId = Convert.ToInt16(ctrlOficinaConsular.SelectedValue);
                        string strPeriodo = ddlAnio.Text + (ddlMes.SelectedIndex + 1).ToString("00");


                        ds = oReportesGerencialesConsultaBL.ObtenerConsolidadoActuacionesPorTipoPago(intOficinaConsularId, strPeriodo);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                                if (intCantidadRegistros > 0)
                                {
                                    string strNumeroMes = (ddlMes.SelectedIndex + 1).ToString("00");
                                    Session["dtDatos"] = ds.Tables[0];
                                    Session["FechaIntervalo"] = ddlAnio.Text + " - " + Util.ObtenerMesLargo(strNumeroMes).ToUpper();
                                    Session[Constantes.CONST_SESION_REPORTE_TIPO] = Enumerador.enmReportesGerenciales.CONSOLIDADO_ACTUACIONES_TIPO_PAGO;
                                    bolVistaPrevia = true;
                                }
                            }
                        }

                    #endregion
                    break;
}

                //------------------------------------------------
                //Fecha: 11/04/2017
                //Autor: Jonatan Silva Cachay
                //Objetivo: Lanza el reporte de RANKING DE RECAUDACIÓN
                //------------------------------------------------
                switch (ddlReportesGerenciales.SelectedItem.Text)
                {
                    case Constantes.CONST_REPORTES_RANKING_RECAUDACION:
                    #region Obtener Datos Reporte
                        if (ctrlOficinaConsular.SelectedValue.ToString() == "0")
                            idoficinaconsular = null;
                        else
                            idoficinaconsular = Convert.ToInt16(ctrlOficinaConsular.SelectedValue.ToString());
                        ds = oReportesGerencialesConsultaBL.ObtenerRankingRecaudacion(idoficinaconsular, fechainicio, fechafin);

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                            if (intCantidadRegistros > 0)
                            {
                                Session["dtDatos"] = ds.Tables[0];

                                //Session[Constantes.CONST_SESION_REPORTE_TIPO] = Constantes.CONST_REPORTES_RANKING_RECAUDACION;
                                bolVistaPrevia = true;
                            }
                        }
                    }
                    break;
                    #endregion

                    case Constantes.CONST_REPORTES_RANKING_CAPTACION:
                        #region Obtener Datos Reporte
                        Session["PagoLima"] = rdioPagoLima.Checked ? "SE INCLUYE LOS PAGOS EN LIMA" : " NO INCLUYE PAGOS EN LIMA";
                        Session["AnioConsulta"] = "AÑO " + intAnio.ToString(); 
                        if (ctrlOficinaConsular.SelectedValue.ToString() == "0")
                            idoficinaconsular = null;
                        else
                            idoficinaconsular = Convert.ToInt16(ctrlOficinaConsular.SelectedValue.ToString());
                        ds = oReportesGerencialesConsultaBL.ObtenerRankingCaptacion(idoficinaconsular, intAnio, Ordenado, pagoLima);

                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                                if (intCantidadRegistros > 0)
                                {
                                    Session["dtDatos"] = ds.Tables[0];

                                    bolVistaPrevia = true;
                                }
                            }
                        }
                        break;
                        #endregion

                    case Constantes.CONST_REPORTES_CUADRO_SALDOS_AUTOADHESIVOS:
                    #region Obtener Datos Reporte

                    Session["AnioConsulta"] = intAnio.ToString();
                    if (ctrlOficinaConsular.SelectedValue.ToString() == "0")
                        idoficinaconsular = null;
                    else
                        idoficinaconsular = Convert.ToInt16(ctrlOficinaConsular.SelectedValue.ToString());

                    ds = oReportesGerencialesConsultaBL.ObtenerCuadroAutoahdesivos(idoficinaconsular, intAnio, Ordenado);

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                            if (intCantidadRegistros > 0)
                            {
                                Session["dtDatos"] = ds.Tables[0];

                                bolVistaPrevia = true;
                            }
                        }
                    }
                    break;
                    #endregion

                    case Constantes.CONST_REPORTES_CUADRO_AUTOADHESIVOS_UTILIZADOS:
                    #region Obtener Datos Reporte

                    Session["AnioConsulta"] = intAnio.ToString();
                    if (ctrlOficinaConsular.SelectedValue.ToString() == "0")
                        idoficinaconsular = null;
                    else
                        idoficinaconsular = Convert.ToInt16(ctrlOficinaConsular.SelectedValue.ToString());
                    ds = oReportesGerencialesConsultaBL.ObtenerCuadroAutoahdesivosUtilizados(idoficinaconsular, intAnio, Ordenado);

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                            if (intCantidadRegistros > 0)
                            {
                                Session["dtDatos"] = ds.Tables[0];

                                bolVistaPrevia = true;
                            }
                        }
                    }
                    break;
                    #endregion

                    case Constantes.CONST_REPORTES_RESUMEN_DIA_ACTUACIONES_USUARIO:
                    #region Obtener Datos Reporte
                        int idtipopagoAct  = 0;
                        int intUsuarioIdAct = 0;
                        int iOficinaConsularIdAct = 0;
                        int intTarifaId = 0;
              
                        if (ctrlOficinaConsular.SelectedValue.ToString() == "0")
                            iOficinaConsularIdAct = 0;
                        else
                            iOficinaConsularIdAct = Convert.ToInt32(ctrlOficinaConsular.SelectedValue.ToString());

                        if (ddlTipoPago.SelectedValue.ToString() == "0")
                            idtipopagoAct = 0;
                        else
                            idtipopagoAct = Convert.ToInt32(ddlTipoPago.SelectedValue.ToString());

                        if (ddlusuario.SelectedIndex > 0)
                            intUsuarioIdAct = Convert.ToInt32(ddlusuario.SelectedValue.ToString());
                        if (ddlTarifa.SelectedIndex > 0)
                            intTarifaId = Convert.ToInt32(ddlTarifa.SelectedValue.ToString());

                        Session["idtarifa_MRE"] = intTarifaId;

                        if (iOficinaConsularIdAct == 0)
                        {
                            ds = null;
                        }
                        else {
                            ds = oReportesGerencialesConsultaBL.USP_REPORTE_ACTUACIONES_PERIODO_USUARIO_OFICINACONSULAR(iOficinaConsularIdAct, fechainicio, fechafin, intUsuarioIdAct, idtipopagoAct, intTarifaId);
                        }
                        
                        
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                            if (intCantidadRegistros > 0)
                            {

                                string TipoPago = "Todos", Usuarios = "Todos", Tarifa = "Todos", Clasificacion = "-";

                                if (ddlTipoPago.SelectedIndex > 0)
                                {
                                    TipoPago = ddlTipoPago.SelectedItem.Text;
                                }
                                if (ddlusuario.SelectedIndex > 0)
                                {
                                    Usuarios = ddlusuario.SelectedItem.Text;
                                }
                                if (ddlTarifa.SelectedIndex > 0)
                                {
                                    Tarifa = ddlTarifa.SelectedItem.Text;
                                }
                                if (ddlClasificacion.SelectedIndex > 0)
                                {
                                    Clasificacion = ddlClasificacion.SelectedItem.Text;
                                }

                                Session["ParametrosReporte"] = TipoPago + "|" + Usuarios + "|" + Tarifa + "|" + Clasificacion ;
                                
                                Session["dtDatos"] = ds.Tables[0];
                                bolVistaPrevia = true;
                            }
                        }
                    }
                    break;
                    #endregion

                    case Constantes.CONST_REPORTES_CONSOLIDADO_ACTUACIONES_USUARIO:
                    #region Obtener Datos Reporte

                    if (ctrlOficinaConsular.SelectedValue.ToString() == "0")
                    {
                        idoficinaconsular = 0;
                    }
                    else
                    { idoficinaconsular = Convert.ToInt16(ctrlOficinaConsular.SelectedValue.ToString()); }

                    if (idoficinaconsular == 0)
                    { ds = null; }
                    else { ds = oReportesGerencialesConsultaBL.USP_REPORTE_CONSOLIDADO_ACTUACIONES_USUARIO_OFICINACONSULAR(Convert.ToInt32(idoficinaconsular), fechainicio, fechafin); }
                    

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                            if (intCantidadRegistros > 0)
                            {
                                Session["dtDatos"] = ds.Tables[0];

                                bolVistaPrevia = true;
                            }
                        }
                    }
                    break;
                    #endregion

                    case Constantes.CONST_REPORTES_ITINERANTES:
                    #region Obtener Datos Reporte
                    if (ctrlOficinaConsular.SelectedValue.ToString() == "0")
                        idoficinaconsular = null;
                    else
                        idoficinaconsular = Convert.ToInt16(ctrlOficinaConsular.SelectedValue.ToString());
                    ds = oReportesGerencialesConsultaBL.ReporteItinerantes(idoficinaconsular, fechainicio, fechafin);

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                            if (intCantidadRegistros > 0)
                            {
                                Session["dtDatos"] = ds.Tables[0];

                                //Session[Constantes.CONST_SESION_REPORTE_TIPO] = Constantes.CONST_REPORTES_RANKING_RECAUDACION;
                                bolVistaPrevia = true;
                            }
                        }
                    }
                    break;
                    #endregion

                    case Constantes.CONST_REPORTES_TITULARES:
                    #region Obtener Datos Reporte

                            Int16 intOficinaConsularID = 0;

                            if (ctrlOficinaConsular.SelectedValue.ToString() == "0")
                            {
                                intOficinaConsularID = 0;
                            }
                            else
                            {
                                intOficinaConsularID = Convert.ToInt16(ctrlOficinaConsular.SelectedValue.ToString());
                            }

                             Int16 EstadoCivil = 0;
                            Int16 GeneroId = 0;                        
                            string CodigoPostal = "";
                            Int16 OcupacionId = 0;
                            Int16 ProfesionId = 0;
                            Int16 GradoInstruccionID = 0;
                            Int16 TipoResidencia = 0;
                            string ResidenciaUbigeo = "";
                            Int16 Nacionalidad = 0;
                            bool BuscarResidencia = false;

                            EstadoCivil = Convert.ToInt16(CmbEstCiv.SelectedValue);
                            GeneroId = Convert.ToInt16(CmbGenero.SelectedValue);
                            CodigoPostal = txtCodPostal.Text;
                            OcupacionId = Convert.ToInt16(CmbOcupacion.SelectedValue);
                            ProfesionId = Convert.ToInt16(ddl_Profesion.SelectedValue);
                            GradoInstruccionID = Convert.ToInt16(CmbGradInst.SelectedValue);
                            Nacionalidad = Convert.ToInt16(CmbNacRecurr.SelectedValue);
                            BuscarResidencia = chkDireccion.Checked;

                            
                            

                            ReporteConsultasBL objReportesConsultasBL = new ReporteConsultasBL();

                            if (BuscarResidencia)
                            {
                                TipoResidencia = Convert.ToInt16(CmbTipRes.SelectedValue);
                                ResidenciaUbigeo = CmbDptoContDir.SelectedValue.ToString() + CmbProvPaisDir.SelectedValue.ToString() + CmbDistCiuDir.SelectedValue.ToString();
                            }
                            else
                            {
                                TipoResidencia = 0;
                                ResidenciaUbigeo = "";
                            }

                            ds = objReportesConsultasBL.ObtenerReporteTitulares(intOficinaConsularID, fechainicio, fechafin, 
                                EstadoCivil, GeneroId, CodigoPostal, OcupacionId, ProfesionId, GradoInstruccionID, TipoResidencia, ResidenciaUbigeo, Nacionalidad);

                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                                    if (intCantidadRegistros > 0)
                                    {
                                        Session["dtDatos"] = ds.Tables[0];

                                        string EstCiv = "-", CodPostal = "-", Profesión = "-", Genero = "-", Ocupacion = "-", GradoInstru = "-";

                                        if (CmbEstCiv.SelectedIndex > 0)
                                        {
                                            EstCiv = CmbEstCiv.SelectedItem.Text;
                                        }
                                        if (txtCodPostal.Text.Length > 0)
                                        {
                                            CodPostal = txtCodPostal.Text;
                                        }
                                        if (ddl_Profesion.SelectedIndex > 0)
                                        {
                                            Profesión = ddl_Profesion.SelectedItem.Text;
                                        }
                                        if (CmbGenero.SelectedIndex > 0)
                                        {
                                            Genero = CmbGenero.SelectedItem.Text;
                                        }
                                        if (CmbOcupacion.SelectedIndex > 0)
                                        {
                                            Ocupacion = CmbOcupacion.SelectedItem.Text;
                                        }
                                        if (CmbGradInst.SelectedIndex > 0)
                                        {
                                            GradoInstru = CmbGradInst.SelectedItem.Text;
                                        }
                                        Session["ParametrosReporte"] = EstCiv + "|" + CodPostal + "|" + Profesión + "|" + Genero + "|" + Ocupacion + "|" + GradoInstru;

                                        bolVistaPrevia = true;                                        
                                    }
                                }
                            }
                        break;
                    #endregion

                    case Constantes.CONST_REPORTES_CORRELATIVOS:
                    #region Obtener Datos Reporte
                        if (ctrlOficinaConsular.SelectedValue.ToString() == "0")
                    {
                        idoficinaconsular = 0;
                    }
                    else
                    { 
                        idoficinaconsular = Convert.ToInt16(ctrlOficinaConsular.SelectedValue.ToString()); 
                    }

                    if (idoficinaconsular == 0)
                    { 
                        ds = null;
                    }
                    else 
                    {
                        ds = oReportesGerencialesConsultaBL.USP_REPORTE_CORRELATIVOS_TARIFA(Convert.ToInt32(idoficinaconsular), intAnio); 
                    }

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                            if (intCantidadRegistros > 0)
                            {
                                Session["dtDatos"] = ds.Tables[0];

                                bolVistaPrevia = true;
                            }
                        }
                    }
                    break;
                    #endregion

                    case Constantes.CONST_REPORTES_RECAUDACION_MENSUAL:
                    #region Obtener Datos Reporte
                    if (ctrlOficinaConsular.SelectedValue.ToString() == "0")
                    {
                        idoficinaconsular = 0;
                    }
                    else
                    {
                        idoficinaconsular = Convert.ToInt16(ctrlOficinaConsular.SelectedValue.ToString());
                    }

                    if (idoficinaconsular == 0)
                    {
                        ds = null;
                    }
                    else
                    {
                        ds = oReportesGerencialesConsultaBL.USP_REPORTE_RECAUDACION_MENSUAL(Convert.ToInt32(idoficinaconsular), intAnio);
                    }

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                            if (intCantidadRegistros > 0)
                            {
                                Session["dtDatos"] = ds.Tables[0];

                                bolVistaPrevia = true;
                            }
                        }
                    }
                    break;
                    #endregion

                    case Constantes.CONST_REPORTES_RECAUDACION_DIARIA:
                    #region Obtener Datos Reporte
                    if (ctrlOficinaConsular.SelectedValue.ToString() == "0")
                    {
                        idoficinaconsular = 0;
                    }
                    else
                    {
                        idoficinaconsular = Convert.ToInt16(ctrlOficinaConsular.SelectedValue.ToString());
                    }
                    if (ddlAnio.SelectedIndex > -1 && ddlMes.SelectedIndex > -1)
                    {
                        fechainicio = Comun.FormatearFecha("01/" + Convert.ToString(ddlMes.SelectedIndex + 1) + "/" + ddlAnio.SelectedItem.Text);
                    }
                    
                    if (idoficinaconsular == 0)
                    {
                        ds = null;
                    }
                    else
                    {
                        ds = oReportesGerencialesConsultaBL.USP_REPORTE_RECAUDACION_DIARIO(Convert.ToInt32(idoficinaconsular), fechainicio);
                    }

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                            if (intCantidadRegistros > 0)
                            {
                                Session["dtDatos"] = ds.Tables[0];

                                bolVistaPrevia = true;
                            }
                        }
                    }
                    break;
                    #endregion

                    case Constantes.CONST_REPORTES_RECAUDACION_TARIFA:
                    #region Obtener Datos Reporte
                    if (ctrlOficinaConsular.SelectedValue.ToString() == "0")
                    {
                        idoficinaconsular = 0;
                    }
                    else
                    {
                        idoficinaconsular = Convert.ToInt16(ctrlOficinaConsular.SelectedValue.ToString());
                    }

                    if (idoficinaconsular == 0)
                    {
                        ds = null;
                    }
                    else
                    {
                        ds = oReportesGerencialesConsultaBL.USP_REPORTE_RECAUDACION_TARIFA(Convert.ToInt32(idoficinaconsular), intAnio);
                    }

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                            if (intCantidadRegistros > 0)
                            {
                                Session["dtDatos"] = ds.Tables[0];

                                bolVistaPrevia = true;
                            }
                        }
                    }
                    break;
                    #endregion
                        
                    case Constantes.CONST_REPORTES_CARGA_INICIAL_CORRELATIVO:
                        #region Obtener Datos Reporte
                    if (ctrlOficinaConsular.SelectedValue.ToString() == "0")
                    {
                        idoficinaconsular = 0;
                    }
                    else
                    {
                        idoficinaconsular = Convert.ToInt16(ctrlOficinaConsular.SelectedValue.ToString());
                    }

                    if (idoficinaconsular == 0)
                    {
                        ds = null;
                    }
                    else
                    {
                        ds = oReportesGerencialesConsultaBL.USP_REPORTE_CARGA_INICIAL_CORRELATIVO(Convert.ToInt32(idoficinaconsular));
                    }

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                            if (intCantidadRegistros > 0)
                            {
                                Session["dtDatos"] = ds.Tables[0];

                                bolVistaPrevia = true;
                            }
                        }
                    }
                    break;
                    #endregion

                    case Constantes.CONST_REPORTES_ACTUACIONES_MENSUALES_X_CONSULADO:
                        #region Obtener Datos Reporte
                            int anioDesde=Convert.ToInt16(ddlAnio.SelectedItem.Text);
                            int mesDesde=Convert.ToInt16(ddlMes.SelectedIndex + 1);
                            int anioHasta=Convert.ToInt16(ddlAnioHasta.SelectedItem.Text);;
                            int mesHasta=Convert.ToInt16(ddlMesHasta.SelectedIndex + 1);
                            string tipoReporte = rdioActuacion.Checked?"cantidad":"monto";
                            Session["actuacionTipoReporte"] = tipoReporte;
                            Session["actuacionTipoReporte"] = tipoReporte;
                            string[] meses = { "ENERO", "FEBRERO", "MARZO", "ABRIL","MAYO","JUNIO","JULIO","AGOSTO","SEPTIEMBRE","OCTUBRE","NOVIEMBRE","DICIEMBRE" };
                            Session["rangoFechas"] = meses[mesDesde - 1] + " DE " + anioDesde + "  AL " + meses[mesHasta - 1] + " DE " + anioHasta;
                            ds = oReportesGerencialesConsultaBL.USP_REPORTE_Actuaciones_Mensuales( anioDesde, mesDesde, anioHasta, mesHasta,tipoReporte);
                    
                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                                    if (intCantidadRegistros > 0)
                                    {
                                        Session["dtDatos"] = ds.Tables[0];

                                        bolVistaPrevia = true;
                                    }
                                }
                            }
                            break;
                        #endregion
                }
            }
        }
        private DataSet ObtenerDataTableReporte(object[] arrParametros)
        {
            DataSet ds = new DataSet();
            Session["SP_PARAMETROS"] = arrParametros;

            DateTime datFechaInicio = new DateTime();
            DateTime datFechaFin = new DateTime();
            
            ReporteConsultasBL oReportesConsultasBL = new ReporteConsultasBL();
            int sOficinaConsularId = Convert.ToInt32(ctrlOficinaConsular.SelectedValue);
            datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
            datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
            int sUsuarioId = Convert.ToInt32(Session[Constantes.CONST_SESION_USUARIO_ID]);
            Int16 sUsuarioEliminaId = Convert.ToInt16(ddlusuario.SelectedValue); 
            string strDireccionIP = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
            int intOficinaConsularLogeo = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            string strClaseFecha = "";
            if (chkFechaActuacion.Checked == true)
            {
                strClaseFecha = "R";
            }
            else
            {
                strClaseFecha = "A";
            }

            ds = oReportesConsultasBL.ObtenerReporteActuacionesAnuladas(sOficinaConsularId, datFechaInicio, datFechaFin, sUsuarioId, strDireccionIP, strClaseFecha, sUsuarioEliminaId);
            return ds;
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
            arrParametros[3] = Util.ObtenerHostName();
            arrParametros[4] = Convert.ToInt32(Session[Constantes.CONST_SESION_USUARIO_ID]);
            arrParametros[5] = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
            arrParametros[6] = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            return arrParametros;
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
        protected void chkFechaActuacion_CheckedChanged(object sender, EventArgs e)
        {
            chkFechaAnulacion.Checked = !chkFechaActuacion.Checked;
        }

        protected void chkFechaAnulacion_CheckedChanged(object sender, EventArgs e)
        {
            chkFechaActuacion.Checked = !chkFechaAnulacion.Checked;
        }
        protected void ddlReportesGerenciales_SelectedIndexChanged(object sender, EventArgs e)
        {
            ocultar.Visible = false;
            btnExportar.Visible = false;
            //------------------------------------------------
            //Fecha: 30/05/2022
            //Autor: Martín Muñoz Selmi
            //Objetivo: Renombrado dinámico de primer elemento 
            //          del ddlusuario 
            //------------------------------------------------
            int sOficinaConsularId = Convert.ToInt16(ctrlOficinaConsular.SelectedValue);
            string cabecera = ddlReportesGerenciales.SelectedValue == "5006" ? "- SELECCIONAR -" : "- TODOS -";
            llenarComboUsuarios(sOficinaConsularId, cabecera);

            if (Convert.ToInt32(ddlReportesGerenciales.SelectedValue) == (int)Enumerador.enmReportesGerenciales.VENTAS_POR_MES)
            {
                ctrlOficinaConsular.AutoPostBack = true;            
                DataTable _dt = new DataTable();

                _dt = obtenerOficinasActivas();
                //DataTable _dt = (DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULARACTIVAS];
                Util.CargarDropDownList(ctrlOficinaConsular.ddlOficinaConsular, _dt, "ofco_vNombre", "ofco_sOficinaConsularId", true, "- SELECCIONAR -");
            }
            else
            {
                //------------------------------------------------------------------------
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 02/09/2016
                // Objetivo: Si es consulado mostrar solo la oficina consular
                //------------------------------------------------------------------------

                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
                {
                    ctrlOficinaConsular.AutoPostBack = true;
                    //ctrlOficinaConsular.Cargar(true, true, " - TODOS - ", "");

                    DataTable _dt = new DataTable();
                    _dt = obtenerOficinasActivas();
                    //DataTable _dt = (DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULARACTIVAS];
                    Util.CargarDropDownList(ctrlOficinaConsular.ddlOficinaConsular, _dt, "ofco_vNombre", "ofco_sOficinaConsularId", true, "- TODOS -");
                }
            }
            //------------------------------------------------------------------------
            // Autor: Miguel Angel Márquez Beltrán
            // Fecha: 02/09/2016
            // Objetivo: Si es consulado mostrar solo la oficina consular
            //------------------------------------------------------------------------

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                ctrlOficinaConsular.SelectedIndex = 0;
            }
            //------------------------------------------------------------------------
            
            ddlCategoriaOficina.SelectedIndex = 0;
            ddlTipoPago.SelectedIndex = 0;
            ddlContinente.SelectedIndex = 0;
            ddlPais.Items.Clear();
            //ddlusuario.Items.Clear();
            ddlTarifa.SelectedIndex = 0;
            lblAnioMes.Visible = false;
            ddlAnio.Visible = false;
            ddlMes.Visible = false;
            dtpFecInicio.Enabled = true;
            dtpFecFin.Enabled = true;
            lblDel.Visible = false; 
            lblAl.Visible = false; 
            ddlAnioHasta.Visible = false;
            ddlMesHasta.Visible = false;
            divTipoReporteActuacion.Visible = false;

            

            DesactivarTodo();
            
            if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_RESUMEN_DIA_ACTUACIONES_USUARIO)
            {
                //------------------------------------------------------------------------
                // Autor: Jonatan Silva Cachay
                // Fecha: 11/10/2017
                // Objetivo: Si es consulado mostrar solo la oficina consular
                //------------------------------------------------------------------------

                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA || Session[Constantes.CONST_SESION_USUARIO_ROL].ToString() == "SGAC_SUPERADMINISTRADOR")
                {
                    ctrlOficinaConsular.Enabled = true;
                }
                //------------------------------------------------------------------------
                ddlusuario.Enabled = true;
                dtpFecInicio.Enabled = true;
                dtpFecFin.Enabled = true;
                ddlTipoPago.Enabled = true;
                ddlTarifa.Enabled = true;
            }
            if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_CONSOLIDADO_ACTUACIONES_USUARIO)
            {
                //------------------------------------------------------------------------
                // Autor: Jonatan Silva Cachay
                // Fecha: 11/10/2017
                // Objetivo: Si es consulado mostrar solo la oficina consular
                //------------------------------------------------------------------------

                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA || Session[Constantes.CONST_SESION_USUARIO_ROL].ToString() == "SGAC_SUPERADMINISTRADOR")
                {
                    ctrlOficinaConsular.Enabled = true;
                }
            }
            if (ddlReportesGerenciales.SelectedItem.Text == "REPORTE DE ACTUACIONES ANULADAS")
            {
                chkFechaActuacion.Visible = true;
                chkFechaAnulacion.Visible = true;
                ddlusuario.Enabled = true;
                RangoFechas.Visible = true;
                ConsultaAnio.Visible = false;
                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
                {
                    ctrlOficinaConsular.Enabled = true;
                    ctrlOficinaConsular.AutoPostBack = true;
                    //ctrlOficinaConsular.Cargar(true, true, " - TODOS - ", "");
                    DataTable _dt = new DataTable();
                    _dt = obtenerOficinasActivas();

                    //DataTable _dt = (DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULARACTIVAS];
                    Util.CargarDropDownList(ctrlOficinaConsular.ddlOficinaConsular, _dt, "ofco_vNombre", "ofco_sOficinaConsularId", true, "- SELECCIONAR -");
                }
            }
            else if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_RANKING_CAPTACION || ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_CUADRO_SALDOS_AUTOADHESIVOS
                || ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_CUADRO_AUTOADHESIVOS_UTILIZADOS)
            {
                ConsultaAnio.Visible = true;
                RangoFechas.Visible = false;
                
            }
            else
            {
                chkFechaActuacion.Visible = false;
                chkFechaAnulacion.Visible = false;
                ConsultaAnio.Visible = false;
                RangoFechas.Visible = true;
            }
            
            if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_ITINERANTES)
            {
                //------------------------------------------------------------------------
                // Autor: Jonatan Silva Cachay
                // Fecha: 24/10/2017
                // Objetivo: Si es consulado mostrar solo la oficina consular
                //------------------------------------------------------------------------

                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA || Session[Constantes.CONST_SESION_USUARIO_ROL].ToString() == "SGAC_SUPERADMINISTRADOR")
                {
                    ctrlOficinaConsular.Enabled = true;

                }

  
            }

            if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_CORRELATIVOS)
            {
                //------------------------------------------------------------------------
                // Autor: Jonatan Silva Cachay
                // Fecha: 09/01/2019
                //------------------------------------------------------------------------

                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA || Session[Constantes.CONST_SESION_USUARIO_ROL].ToString() == "SGAC_SUPERADMINISTRADOR")
                {
                    ctrlOficinaConsular.Enabled = true;

                }
                Util.CargarComboAnios(ddlAnioConsulta, 2015, DateTime.Now.Year);
                ddlAnioConsulta.SelectedValue = DateTime.Now.Year.ToString();
                RangoFechas.Visible = false;
                ConsultaAnio.Visible = true;
                lblOrdenado.Visible = false;
                rbMonto.Visible = false;
                rbPaisConsulado.Visible = false;
            }
            if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_RECAUDACION_MENSUAL)
            {
                //------------------------------------------------------------------------
                // Autor: Jonatan Silva Cachay
                // Fecha: 09/01/2019
                //------------------------------------------------------------------------

                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA || Session[Constantes.CONST_SESION_USUARIO_ROL].ToString() == "SGAC_SUPERADMINISTRADOR")
                {
                    ctrlOficinaConsular.Enabled = true;

                }
                Util.CargarComboAnios(ddlAnioConsulta, 2015, DateTime.Now.Year);
                ddlAnioConsulta.SelectedValue = DateTime.Now.Year.ToString();
                RangoFechas.Visible = false;
                ConsultaAnio.Visible = true;
                lblOrdenado.Visible = false;
                rbMonto.Visible = false;
                rbPaisConsulado.Visible = false;
            }

            if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_RECAUDACION_DIARIA)
            {
                //------------------------------------------------------------------------
                // Autor: Jonatan Silva Cachay
                // Fecha: 09/01/2019
                //------------------------------------------------------------------------

                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA || Session[Constantes.CONST_SESION_USUARIO_ROL].ToString() == "SGAC_SUPERADMINISTRADOR")
                {
                    ctrlOficinaConsular.Enabled = true;

                }
                Util.CargarComboAnios(ddlAnio, 2015, DateTime.Now.Year);
                Util.CargarDropDownList(ddlMes, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONFIG_MES), "valor", "id");
                ddlAnio.SelectedValue = DateTime.Now.Year.ToString();
                RangoFechas.Visible = false;
                ConsultaAnio.Visible = false;
                lblAnioMes.Visible = true;
                ddlAnio.Visible = true;
                ddlMes.Visible = true;
            }
            if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_RECAUDACION_TARIFA)
            {
                //------------------------------------------------------------------------
                // Autor: Jonatan Silva Cachay
                // Fecha: 09/01/2019
                //------------------------------------------------------------------------

                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA || Session[Constantes.CONST_SESION_USUARIO_ROL].ToString() == "SGAC_SUPERADMINISTRADOR")
                {
                    ctrlOficinaConsular.Enabled = true;

                }
                Util.CargarComboAnios(ddlAnioConsulta, 2015, DateTime.Now.Year);
                ddlAnioConsulta.SelectedValue = DateTime.Now.Year.ToString();
                RangoFechas.Visible = false;
                ConsultaAnio.Visible = true;
                lblOrdenado.Visible = false;
                rbMonto.Visible = false;
                rbPaisConsulado.Visible = false;
            }
            if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_ACTUACIONES_MENSUALES_X_CONSULADO)
            {
                //#######################################################################
                // Autor: Vidal Pipa
                // Fecha: 24/11/2020
                // Objetivo: nuevo reporte ACTUACIONES MENSUALES X CONSULADO
                //#######################################################################

                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA || Session[Constantes.CONST_SESION_USUARIO_ROL].ToString() == "SGAC_SUPERADMINISTRADOR")
                {
                    //ctrlOficinaConsular.Enabled = true;
                }
                //------------------------------------------------------------------------

                dtpFecInicio.Enabled = false;
                dtpFecFin.Enabled = false;

                lblAnioMes.Visible = true;
                ddlAnio.Visible = true;
                ddlMes.Visible = true;

                lblDel.Visible = true;
                lblAl.Visible = true;
                ddlAnioHasta.Visible = true;
                ddlMesHasta.Visible = true;
                divTipoReporteActuacion.Visible = true;
                RangoFechas.Visible = false;

                Util.CargarComboAnios(ddlAnio, 2015, DateTime.Now.Year);
                Util.CargarDropDownList(ddlMes, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONFIG_MES), "valor", "id");
                ddlAnio.SelectedValue = DateTime.Now.Year.ToString();

                Util.CargarComboAnios(ddlAnioHasta, 2015, DateTime.Now.Year);
                Util.CargarDropDownList(ddlMesHasta, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONFIG_MES), "valor", "id");
                ddlAnioHasta.SelectedValue = DateTime.Now.Year.ToString();
                ddlMesHasta.SelectedIndex = DateTime.Now.Month - 1;

            }
            switch (Convert.ToInt32(ddlReportesGerenciales.SelectedValue))
            {
                case 0:
                    ddlReportesGerenciales.SelectedIndex = 0;

                    //------------------------------------------------------------------------
                    // Autor: Miguel Angel Márquez Beltrán
                    // Fecha: 02/09/2016
                    // Objetivo: Si es consulado mostrar solo la oficina consular
                    //------------------------------------------------------------------------

                    if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
                    {
                        ctrlOficinaConsular.SelectedIndex = 0;
                    }
                    //------------------------------------------------------------------------

                    ddlCategoriaOficina.SelectedIndex = 0;
                    ddlTipoPago.SelectedIndex = 0;
                    ddlContinente.SelectedIndex = 0;
                    ddlTarifa.SelectedIndex = 0;
                    //txtsTarifaId.Text = "";
                    dtpFecInicio.Text = "";
                    dtpFecFin.Text = "";
                    break;
                case (int)Enumerador.enmReportesGerenciales.RGE_CONSOLIDADO:
                    //------------------------------------------------------------------------
                    // Autor: Miguel Angel Márquez Beltrán
                    // Fecha: 02/09/2016
                    // Objetivo: Si es consulado mostrar solo la oficina consular
                    //------------------------------------------------------------------------

                    if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA || Session[Constantes.CONST_SESION_USUARIO_ROL].ToString() == "SGAC_SUPERADMINISTRADOR")
                    {
                        ctrlOficinaConsular.Enabled = true;
                    }
                    //------------------------------------------------------------------------

                    ddlusuario.Enabled = true;
                    //txtsTarifaId.Enabled = true;
                    ddlTarifa.Enabled = true;

                    dtpFecInicio.Enabled = true;
                    dtpFecFin.Enabled = true;

                    ddlContinente.Enabled = true;
                    ddlTipoPago.Enabled = true;
                    ddlCategoriaOficina.Enabled = true;
                    break;
                case (int)Enumerador.enmReportesGerenciales.RECORD_DE_VENTA:
                    //------------------------------------------------------------------------
                    // Autor: Miguel Angel Márquez Beltrán
                    // Fecha: 02/09/2016
                    // Objetivo: Si es consulado mostrar solo la oficina consular
                    //------------------------------------------------------------------------

                    if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA || Session[Constantes.CONST_SESION_USUARIO_ROL].ToString() == "SGAC_SUPERADMINISTRADOR")
                    {
                        ctrlOficinaConsular.Enabled = true;
                    }
                    //------------------------------------------------------------------------
                    ddlusuario.Enabled = true;
                    break;
                case (int)Enumerador.enmReportesGerenciales.RGE_POR_CATEGORIA:
                    break;
                case (int)Enumerador.enmReportesGerenciales.MAYOR_VENTA_Y_DETALLE:
                    break;
                case (int)Enumerador.enmReportesGerenciales.VENTAS_POR_MES:
                    //------------------------------------------------------------------------
                    // Autor: Miguel Angel Márquez Beltrán
                    // Fecha: 02/09/2016
                    // Objetivo: Si es consulado mostrar solo la oficina consular
                    //------------------------------------------------------------------------

                    if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA || Session[Constantes.CONST_SESION_USUARIO_ROL].ToString() == "SGAC_SUPERADMINISTRADOR")
                    {
                        ctrlOficinaConsular.Enabled = true;
                    }
                    //------------------------------------------------------------------------
                    break;
                case (int)Enumerador.enmReportesGerenciales.TARIFA_CONSULAR_POR_PAIS:
                    ddlTarifa.Enabled = true;
                    break;
                case (int)Enumerador.enmReportesGerenciales.RECORD_DE_ACTUACIONES:
                    break;
                case (int)Enumerador.enmReportesGerenciales.TOP_14_MAYOR_VENTA_POR_PAIS:
                    break;
                case (int)Enumerador.enmReportesGerenciales.RGE_POR_CONTIENENTE:
                    dtpFecInicio.Enabled = true;
                    dtpFecFin.Enabled = true;
                    break;
                case (int)Enumerador.enmReportesGerenciales.RGE_POR_CATEGORIA_POR_RECORD_DE_VENTA:
                    break;
                case (int)Enumerador.enmReportesGerenciales.AUTOADHESIVOS_USUARIO_OFICINA_CONSULAR:
                    //------------------------------------------------------------------------
                    // Autor: Miguel Angel Márquez Beltrán
                    // Fecha: 02/09/2016
                    // Objetivo: Si es consulado mostrar solo la oficina consular
                    //------------------------------------------------------------------------

                    if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA || Session[Constantes.CONST_SESION_USUARIO_ROL].ToString() == "SGAC_SUPERADMINISTRADOR")
                    {
                        ctrlOficinaConsular.Enabled = true;
                    }
                    //------------------------------------------------------------------------
                    ddlusuario.Enabled = true;
                     dtpFecInicio.Enabled = true;
                     dtpFecFin.Enabled = true;
                     DivEstadoAutoadhesivo.Visible = true;
                     chkSinFecha.Visible = true;
                     //ddlTipoPago.Enabled = true;

                    break;
                case (int)Enumerador.enmReportesGerenciales.ACTUACIONES_USUARIO_OFICINA_CONSULAR:
                    //------------------------------------------------------------------------
                    // Autor: Miguel Angel Márquez Beltrán
                    // Fecha: 02/09/2016
                    // Objetivo: Si es consulado mostrar solo la oficina consular
                    //------------------------------------------------------------------------

                    if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA || Session[Constantes.CONST_SESION_USUARIO_ROL].ToString() == "SGAC_SUPERADMINISTRADOR")
                    {
                        ctrlOficinaConsular.Enabled = true;
                    }
                    //------------------------------------------------------------------------
                    ddlusuario.Enabled = true;
                    dtpFecInicio.Enabled = true;
                    dtpFecFin.Enabled = true;
                    ddlTipoPago.Enabled = true;
                    ddlTarifa.Enabled = true;
                    
                    break;
                //------------------------------------------------
                //Fecha: 29/12/2016
                //Autor: Jonatan Silva Cachay
                //Objetivo: Activa y desactiva controles cuando es reporte de personas
                //------------------------------------------------
            case (int)Enumerador.enmReportesGerenciales.PERSONAS_USUARIO_OFICINA_CONSULAR:
                    if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA || Session[Constantes.CONST_SESION_USUARIO_ROL].ToString() == "SGAC_SUPERADMINISTRADOR")
                    {
                        ctrlOficinaConsular.Enabled = true;
                        
                    }
              
                    if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
                    {
                        CmbDptoContDir.Enabled = true;
                        CmbProvPaisDir.Enabled = true;
                    }
                    else
                    {
                        CmbDptoContDir.Enabled = false;
                        CmbProvPaisDir.Enabled = false;
                    }
                    //------------------------------------------------------------------------
                    ddlusuario.Enabled = false;
                    dtpFecInicio.Enabled = true;
                    dtpFecFin.Enabled = true;
                    ocultar.Visible = true;
                    btnExportar.Visible = true;
                    break;
            //------------------------------------------------
            //Fecha: 02/01/2017
            //Autor: Jonatan Silva Cachay
            //Objetivo: Activa y desactiva controles cuando es reporte Cantidad de actuaciones
            //------------------------------------------------
            case (int)Enumerador.enmReportesGerenciales.CANTIDAD_ACTUACIONES_CONSULADO:
                    if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA || Session[Constantes.CONST_SESION_USUARIO_ROL].ToString() == "SGAC_SUPERADMINISTRADOR")
                    {
                        ctrlOficinaConsular.Enabled = true;
                    }
                    //------------------------------------------------------------------------

                    ddlusuario.Enabled = true;
                    ddlTarifa.Enabled = true;

                    dtpFecInicio.Enabled = true;
                    dtpFecFin.Enabled = true;

                    ddlContinente.Enabled = true;
                    ddlTipoPago.Enabled = true;
                    ddlCategoriaOficina.Enabled = true;
                    break;
            //------------------------------------------------
            //Fecha: 02/01/2017
            //Autor: Jonatan Silva Cachay
            //Objetivo: Activa y desactiva controles cuando es reporte Cantidad de tarifas
            //------------------------------------------------
            case (int)Enumerador.enmReportesGerenciales.CANTIDAD_TARIFAS_REGISTRADAS:
                    if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA || Session[Constantes.CONST_SESION_USUARIO_ROL].ToString() == "SGAC_SUPERADMINISTRADOR")
                    {
                        ctrlOficinaConsular.Enabled = true;
                    }
                    //------------------------------------------------------------------------

                    ddlusuario.Enabled = true;
                    ddlTarifa.Enabled = true;

                    dtpFecInicio.Enabled = true;
                    dtpFecFin.Enabled = true;

                    ddlContinente.Enabled = true;
                    ddlTipoPago.Enabled = true;
                    ddlCategoriaOficina.Enabled = true;
                    break;

                case (int)Enumerador.enmReportesGerenciales.CONSOLIDADO_ACTUACIONES_TIPO_PAGO:
                     //------------------------------------------------------------------------
                    // Autor: Miguel Angel Márquez Beltrán
                    // Fecha: 26/06/2017
                    // Objetivo: Solo habilitar oficina consular y fecha inicio y fin.
                    //------------------------------------------------------------------------
                    if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
                    {
                        ctrlOficinaConsular.Enabled = true;
                        lblAnioMes.Visible = true;
                        ddlAnio.Visible = true;
                        ddlMes.Visible = true;
                        ddlAnio.SelectedValue = DateTime.Now.Year.ToString();
                        ddlMes.SelectedIndex = DateTime.Now.Month - 1;
                        dtpFecInicio.Enabled = false;
                        dtpFecFin.Enabled = false;

                    }

                    break;                
            }

            //----------------------------------------------------------------------------------------
            // Fecha: 26/09/2018
            // Autor: Miguel Márquez Beltrán
            // Objetivo: Habilitar los controles para emitir el reporte de Titulares
            //----------------------------------------------------------------------------------------
            if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_TITULARES)
            {
                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA || Session[Constantes.CONST_SESION_USUARIO_ROL].ToString() == "SGAC_SUPERADMINISTRADOR")
                {
                    ctrlOficinaConsular.Enabled = true;
                }
                dtpFecInicio.Enabled = true;
                dtpFecFin.Enabled = true;

                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
                {
                    CmbDptoContDir.Enabled = true;
                    CmbProvPaisDir.Enabled = true;
                }
                else
                {
                    CmbDptoContDir.Enabled = false;
                    CmbProvPaisDir.Enabled = false;
                }
                //------------------------------------------------------------------------
                ddlusuario.Enabled = false;
                dtpFecInicio.Enabled = true;
                dtpFecFin.Enabled = true;
                ocultar.Visible = true;
                btnExportar.Visible = true;
            }
            if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_CARGA_INICIAL_CORRELATIVO)
            {
                //------------------------------------------------------------------------
                // Autor: Jonatan Silva Cachay
                // Fecha: 09/01/2019
                //------------------------------------------------------------------------

                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA || Session[Constantes.CONST_SESION_USUARIO_ROL].ToString() == "SGAC_SUPERADMINISTRADOR")
                {
                    ctrlOficinaConsular.Enabled = true;

                }
                //Util.CargarComboAnios(ddlAnioConsulta, 2015, DateTime.Now.Year);
                //ddlAnioConsulta.SelectedValue = DateTime.Now.Year.ToString();
                RangoFechas.Visible = false;
                ConsultaAnio.Visible = false;
                lblOrdenado.Visible = false;
                rbMonto.Visible = false;
                rbPaisConsulado.Visible = false;
            }
            //----------------------------------------------------------------------------------------
            //Jonatan: para evitar que de cualquier oficina consular puedan hacer consultas generales
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                ddlContinente.Enabled = false;
            }
            if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_RANKING_CAPTACION)
            {
                divPagosEnLima.Visible = true;
                rdioPagoLima.Visible = true;
                rdioSinPagoLima.Visible = true;
                lblOrdenado .Visible = false;
                rbMonto.Visible = false;
                rbPaisConsulado.Visible = false;
            }
            //---------------------------------------------------------------------------
            //Fecha: 07/05/2021
            //Autor: Miguel Màrquez Beltràn
            //Motivo: Habilitar check de tràmites sin vincular
            //          cuando el reporte sea: ACTUACIONES - DETALLE POR USUARIO.
            //---------------------------------------------------------------------------
            if (Convert.ToInt32(ddlReportesGerenciales.SelectedValue) == (int)Enumerador.enmReportesGerenciales.ACTUACIONES_USUARIO_OFICINA_CONSULAR)
            {
                chk_TramitesSinVincular.Visible = true;
                chk_TramitesSinVincular.Checked = false;
            }
            else
            {
                chk_TramitesSinVincular.Visible = false;
                chk_TramitesSinVincular.Checked = false;
            }
            //---------------------------------------------------------------------------
            updReportesGerenciales.Update();            
        }

        void DesactivarTodo()
        {
            ctrlOficinaConsular.Enabled = false;
            ddlusuario.Enabled = false;
            ddlTarifa.Enabled = false;

            ddlPais.Enabled = false;
            ddlContinente.Enabled = false;
            ddlTipoPago.Enabled = false;
            ddlCategoriaOficina.Enabled = false;

            ddlClasificacion.Enabled = false;
            DivEstadoAutoadhesivo.Visible = false;
            chkSinFecha.Visible = false;
            divPagosEnLima.Visible = false;

        }

        protected void ddlContinente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlContinente.SelectedValue.ToString() == "")
            {
                return;
            }
            //Jonatan: para evitar que de cualquier oficina consular puedan hacer consultas generales
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                ddlContinente.SelectedValue = "00";
                return;
            }
            ddlPais.Enabled = true;
            FillWebCombo(comun_Part3.ObtenerProvincias(Session, ddlContinente.SelectedValue.ToString()), ddlPais, "ubge_vProvincia", "ubge_cUbi02");
            
            string strCodContinente = string.Empty;

            if (ddlContinente.SelectedValue == "00")
            {
                ddlPais.Items.Clear();
                strCodContinente = "0";
                //ctrlOficinaConsular.Cargar(true);
                DataTable _dt = new DataTable();

                _dt = obtenerOficinasActivas();

                //DataTable _dt = (DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULARACTIVAS];
                Util.CargarDropDownList(ctrlOficinaConsular.ddlOficinaConsular, _dt, "ofco_vNombre", "ofco_sOficinaConsularId", true, "- SELECCIONAR -");
            }
            else
            {
                strCodContinente = ddlContinente.SelectedValue.ToString();
                ctrlOficinaConsular.Cargar(true, true, " - TODOS - ", strCodContinente);
            }

            ddlusuario.Items.Clear();
            ddlusuario.Items.Insert(0, new ListItem(" - TODOS - ", "0"));
        }

        protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPais.SelectedValue.ToString() == "")
            {
                return;
            }
            
            string strCodContinente = string.Empty;
            string strCodPais = string.Empty;

            strCodContinente = ddlContinente.SelectedValue.ToString();

            if (ddlPais.SelectedValue == "00")
            {
                //ctrlOficinaConsular.Cargar(true, true, " - TODOS - ", strCodContinente);
                DataTable _dt = new DataTable();

                _dt = obtenerOficinasActivas();
                //DataTable _dt = (DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULARACTIVAS];
                Util.CargarDropDownList(ctrlOficinaConsular.ddlOficinaConsular, _dt, "ofco_vNombre", "ofco_sOficinaConsularId", true, "- TODOS -");
            }
            else
            {
                strCodPais = ddlPais.SelectedValue.ToString();
                ctrlOficinaConsular.CargarContinentePais(true, true, " - TODOS - ", strCodContinente, strCodPais);
            }

            ddlusuario.Items.Clear();
            ddlusuario.Items.Insert(0, new ListItem(" - TODOS - ", "0"));
        }

        //------------------------------------------------
        //Fecha: 29/12/2016
        //Autor: Jonatan Silva Cachay
        //Objetivo: Activa y desactiva controles de clasificación cuando cambia el control de tarifa a tarifa 20B
        //------------------------------------------------
        protected void ddlTarifa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTarifa.SelectedValue.ToString() == Constantes.CONST_PROTOCOLAR_ID_TARIFA_20B.ToString()) //TARIFA 20B
            {
                if (Convert.ToInt32(ddlReportesGerenciales.SelectedValue) == (int)Enumerador.enmReportesGerenciales.ACTUACIONES_USUARIO_OFICINA_CONSULAR)
                { ddlClasificacion.Enabled = true; }
            }
            else { 
                ddlClasificacion.Enabled = false;
                ddlClasificacion.SelectedIndex = 0;
            }
            
        }

        protected void CmbDptoContDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbDptoContDir.SelectedIndex > 0)
            {
                CmbProvPaisDir.Enabled = true;

                FillWebCombo(comun_Part3.ObtenerProvincias(Session, CmbDptoContDir.SelectedValue.ToString()),
                             CmbProvPaisDir,
                             "ubge_vProvincia",
                             "ubge_cUbi02");

                CmbDptoContDir.Focus();
            }
            else
            {
                this.CmbProvPaisDir.Items.Clear();
                this.CmbProvPaisDir.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));

            }

            this.CmbDistCiuDir.Items.Clear();
            this.CmbDistCiuDir.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
        }

        protected void CmbProvPaisDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbProvPaisDir.SelectedIndex > 0)
            {
                CmbDistCiuDir.Enabled = true;

                FillWebCombo(comun_Part3.ObtenerDistritos(Session, CmbDptoContDir.SelectedValue.ToString(), CmbProvPaisDir.SelectedValue.ToString()),
                             CmbDistCiuDir,
                             "ubge_vDistrito",
                             "ubge_cUbi03");

                Session["CmbProvPaisDir"] = CmbProvPaisDir.SelectedValue;

                CmbProvPaisDir.Focus();
            }
            else
            {
                this.CmbDistCiuDir.Items.Clear();
                this.CmbDistCiuDir.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
            }
        }

        protected void chkDireccion_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDireccion.Checked)
            {
                ocultarDir.Visible = true;
            }
            else { ocultarDir.Visible = false; }
        }

        protected void chkSinFecha_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSinFecha.Checked)
            {
                RangoFechas.Visible = false;
            }
            else{
                RangoFechas.Visible = true;
            }
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            Impresion("N");
        }

        private void Impresion(string strConCabecera="S")
        {
            bool booError = false;


            if (ddlReportesGerenciales.SelectedIndex == 0)
            {
                ctrlValidacion.MostrarValidacion("Seleccione el tipo de reporte", true, Enumerador.enmTipoMensaje.WARNING);
                updReportesGerenciales.Update(); return;
            }

            int intTipoReporteGerencial = Convert.ToInt32(ddlReportesGerenciales.SelectedValue);
            bool bValidarRangoFechas = true;

            //--------------------------------------------------
            //Fecha: 01/09/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Adicionar que no valide rando de fecha 
            //          cuando es el reporte de correlativo
            //          por tarifas.                        
            //--------------------------------------------------

            if (intTipoReporteGerencial == (int)Enumerador.enmReportesGerenciales.CONSOLIDADO_ACTUACIONES_TIPO_PAGO 
                || ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_ACTUACIONES_MENSUALES_X_CONSULADO
                || ddlReportesGerenciales.SelectedItem.Text ==Constantes.CONST_REPORTES_RANKING_CAPTACION
                || ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_CORRELATIVOS)
            {
                bValidarRangoFechas = false;
            }

            if (bValidarRangoFechas)
            {
                #region RangoFechas

                // AQUI SE EVALUA LAS FECHAS PORQUE SON REQUERIDAS
                if (dtpFecInicio.Text == "")
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_VACIA, true, Enumerador.enmTipoMensaje.ERROR);
                    updReportesGerenciales.Update(); return;
                }

                if (dtpFecFin.Text == "")
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_VACIA, true, Enumerador.enmTipoMensaje.ERROR);
                    updReportesGerenciales.Update(); return;
                }
                if (Comun.EsFecha(dtpFecInicio.Text.Trim()) == false)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_INICIAL, true, Enumerador.enmTipoMensaje.WARNING);
                    updReportesGerenciales.Update(); return;
                }
                if (Comun.EsFecha(dtpFecFin.Text.Trim()) == false)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_FINAL, true, Enumerador.enmTipoMensaje.WARNING);

                    updReportesGerenciales.Update(); return;
                }
                if (Comun.FormatearFecha(dtpFecFin.Text) < Comun.FormatearFecha(dtpFecInicio.Text))
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);
                    updReportesGerenciales.Update(); return;
                }
                #endregion
            }

            switch (Convert.ToInt32(ddlReportesGerenciales.SelectedValue))
            {

                case (int)Enumerador.enmReportesGerenciales.RGE_CONSOLIDADO:
                    break;
                case (int)Enumerador.enmReportesGerenciales.RECORD_DE_VENTA:
                    break;
                case (int)Enumerador.enmReportesGerenciales.RGE_POR_CATEGORIA:
                    break;
                case (int)Enumerador.enmReportesGerenciales.MAYOR_VENTA_Y_DETALLE:
                    break;
                case (int)Enumerador.enmReportesGerenciales.VENTAS_POR_MES:
                    if (ctrlOficinaConsular.SelectedValue.ToString() == "0")
                    {
                        ctrlValidacion.MostrarValidacion("No ha seleccionado la Oficina Consular", true, Enumerador.enmTipoMensaje.WARNING);
                        booError = true;
                    }
                    break;
                case (int)Enumerador.enmReportesGerenciales.TARIFA_CONSULAR_POR_PAIS:
                    if (ddlTarifa.SelectedValue == "")
                    {
                        ctrlValidacion.MostrarValidacion("No ha ingresado la tarifa ", true, Enumerador.enmTipoMensaje.WARNING);
                        booError = true;
                    }
                    break;
                case (int)Enumerador.enmReportesGerenciales.RECORD_DE_ACTUACIONES:
                    break;
                case (int)Enumerador.enmReportesGerenciales.TOP_14_MAYOR_VENTA_POR_PAIS:
                    break;
                case (int)Enumerador.enmReportesGerenciales.RGE_POR_CONTIENENTE:
                    break;
                case (int)Enumerador.enmReportesGerenciales.RGE_POR_CATEGORIA_POR_RECORD_DE_VENTA:
                    break;
                case (int)Enumerador.enmReportesGerenciales.AUTOADHESIVOS_USUARIO_OFICINA_CONSULAR:
                    break;
                case (int)Enumerador.enmReportesGerenciales.ACTUACIONES_USUARIO_OFICINA_CONSULAR:
                    break;
                case (int)Enumerador.enmReportesGerenciales.PERSONAS_USUARIO_OFICINA_CONSULAR:
                    if (chkDireccion.Checked)
                    {
                        if (CmbTipRes.SelectedIndex == 0)
                        {
                            ctrlValidacion.MostrarValidacion("Debe Ingresar el tipo de residencia ", true, Enumerador.enmTipoMensaje.WARNING);
                            booError = true;
                        }

                        if (CmbDptoContDir.SelectedIndex == 0)
                        {
                            ctrlValidacion.MostrarValidacion("Debe Ingresar el Departamento / Pais ", true, Enumerador.enmTipoMensaje.WARNING);
                            booError = true;
                        }
                        if (CmbProvPaisDir.SelectedIndex == 0)
                        {
                            ctrlValidacion.MostrarValidacion("Debe Ingresar la Provincia / Pais ", true, Enumerador.enmTipoMensaje.WARNING);
                            booError = true;
                        }
                        if (CmbDistCiuDir.SelectedIndex == 0)
                        {
                            ctrlValidacion.MostrarValidacion("Debe Ingresar el Distrito / Ciudad ", true, Enumerador.enmTipoMensaje.WARNING);
                            booError = true;
                        }
                    }

                    break;
                case (int)Enumerador.enmReportesGerenciales.CONSOLIDADO_ACTUACIONES_TIPO_PAGO:

                    if (ctrlOficinaConsular.SelectedValue.ToString() == "0")
                    {
                        ctrlValidacion.MostrarValidacion("No ha seleccionado la Oficina Consular", true, Enumerador.enmTipoMensaje.WARNING);
                        booError = true;
                    }
                    break;
            }

            if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_TITULARES)
            {
                if (chkDireccion.Checked)
                {
                    if (CmbTipRes.SelectedIndex == 0)
                    {
                        ctrlValidacion.MostrarValidacion("Debe Ingresar el tipo de residencia ", true, Enumerador.enmTipoMensaje.WARNING);
                        booError = true;
                    }

                    if (CmbDptoContDir.SelectedIndex == 0)
                    {
                        ctrlValidacion.MostrarValidacion("Debe Ingresar el Departamento / Pais ", true, Enumerador.enmTipoMensaje.WARNING);
                        booError = true;
                    }
                    if (CmbProvPaisDir.SelectedIndex == 0)
                    {
                        ctrlValidacion.MostrarValidacion("Debe Ingresar la Provincia / Pais ", true, Enumerador.enmTipoMensaje.WARNING);
                        booError = true;
                    }
                    if (CmbDistCiuDir.SelectedIndex == 0)
                    {
                        ctrlValidacion.MostrarValidacion("Debe Ingresar el Distrito / Ciudad ", true, Enumerador.enmTipoMensaje.WARNING);
                        booError = true;
                    }
                }
            }

            Session["IdOficinaConsular_contabilidad"] = ctrlOficinaConsular.SelectedValue;
            Session["TituloReporte"] = ddlReportesGerenciales.SelectedItem.Text;
            updReportesGerenciales.Update();

            if (booError == true) { return; }

            VerVistaPrevia();
            if (bolReporteContable == false)
            {
                if (bolVistaPrevia)
                {
                    Session["vistaPrevia_MRE"] = 1;

                    bolVistaPrevia = false;
                    if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_RANKING_RECAUDACION)
                    {
                        String strUrl = "../Reportes/FrmPreviewReportesGerenciales.aspx?rep=" + Constantes.CONST_REPORTES_RANKING_RECAUDACION;
                        String strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";

                        EjecutarScript(Page, strScript);
                    }
                    else if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_RANKING_CAPTACION)
                    {
                        String strUrl = "../Reportes/FrmPreviewReportesGerenciales.aspx?rep=" + Constantes.CONST_REPORTES_RANKING_CAPTACION;
                        String strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";

                        EjecutarScript(Page, strScript);
                    }
                    else if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_CUADRO_SALDOS_AUTOADHESIVOS)
                    {
                        String strUrl = "../Reportes/FrmPreviewReportesGerenciales.aspx?rep=" + Constantes.CONST_REPORTES_CUADRO_SALDOS_AUTOADHESIVOS;
                        String strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";

                        EjecutarScript(Page, strScript);
                    }
                    else if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_CUADRO_AUTOADHESIVOS_UTILIZADOS)
                    {
                        String strUrl = "../Reportes/FrmPreviewReportesGerenciales.aspx?rep=" + Constantes.CONST_REPORTES_CUADRO_AUTOADHESIVOS_UTILIZADOS;
                        String strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";

                        EjecutarScript(Page, strScript);
                    }
                    else if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_CONSOLIDADO_ACTUACIONES_USUARIO)
                    {
                        String strUrl = "../Reportes/FrmPreviewReportesGerenciales.aspx?rep=" + Constantes.CONST_REPORTES_CONSOLIDADO_ACTUACIONES_USUARIO;
                        String strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";

                        EjecutarScript(Page, strScript);
                    }
                    else if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_RESUMEN_DIA_ACTUACIONES_USUARIO)
                    {
                        String strUrl = "../Reportes/FrmPreviewReportesGerenciales.aspx?rep=" + Constantes.CONST_REPORTES_RESUMEN_DIA_ACTUACIONES_USUARIO;
                        String strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";

                        EjecutarScript(Page, strScript);
                    }
                    else if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_ITINERANTES)
                    {
                        Session["FechaIntervalo"] = dtpFecInicio.Text + " al " + dtpFecFin.Text;
                        String strUrl = "../Reportes/FrmPreviewReportesGerenciales.aspx?rep=" + Constantes.CONST_REPORTES_ITINERANTES;
                        String strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";

                        EjecutarScript(Page, strScript);
                    }
                    else if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_TITULARES)
                    {
                        Session["FechaIntervalo"] = dtpFecInicio.Text + " al " + dtpFecFin.Text;
                        String strUrl = "../Reportes/FrmPreviewReportesGerenciales.aspx?rep=" + Constantes.CONST_REPORTES_TITULARES + "&Head=" + strConCabecera;
                        String strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";

                        EjecutarScript(Page, strScript);
                    }
                    else if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_CORRELATIVOS)
                    {
                        Session["FechaIntervalo"] = ddlAnioConsulta.SelectedItem.Text;
                        String strUrl = "../Reportes/FrmPreviewReportesGerenciales.aspx?rep=" + Constantes.CONST_REPORTES_CORRELATIVOS;
                        String strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";

                        EjecutarScript(Page, strScript);
                    }
                    else if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_RECAUDACION_MENSUAL)
                    {
                        Session["FechaIntervalo"] = ddlAnioConsulta.SelectedItem.Text;
                        String strUrl = "../Reportes/FrmPreviewReportesGerenciales.aspx?rep=" + Constantes.CONST_REPORTES_RECAUDACION_MENSUAL;
                        String strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";

                        EjecutarScript(Page, strScript);
                    }
                    else if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_RECAUDACION_DIARIA)
                    {
                        Session["FechaIntervalo"] = ddlMes.SelectedItem.Text + "-" + ddlAnio.SelectedItem.Text;
                        String strUrl = "../Reportes/FrmPreviewReportesGerenciales.aspx?rep=" + Constantes.CONST_REPORTES_RECAUDACION_DIARIA;
                        String strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";

                        EjecutarScript(Page, strScript);
                    }
                    else if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_RECAUDACION_TARIFA)
                    {
                        Session["FechaIntervalo"] = ddlAnioConsulta.SelectedItem.Text;
                        String strUrl = "../Reportes/FrmPreviewReportesGerenciales.aspx?rep=" + Constantes.CONST_REPORTES_RECAUDACION_TARIFA;
                        String strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";

                        EjecutarScript(Page, strScript);
                    }
                    else if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_CARGA_INICIAL_CORRELATIVO)
                    {
                        String strUrl = "../Reportes/FrmPreviewReportesGerenciales.aspx?rep=" + Constantes.CONST_REPORTES_CARGA_INICIAL_CORRELATIVO;
                        String strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";

                        EjecutarScript(Page, strScript);
                    }
                    else if (ddlReportesGerenciales.SelectedItem.Text == Constantes.CONST_REPORTES_ACTUACIONES_MENSUALES_X_CONSULADO)
                    {
                        String strUrl = "../Reportes/FrmPreviewReportesGerenciales.aspx?rep=" + Constantes.CONST_REPORTES_ACTUACIONES_MENSUALES_X_CONSULADO;
                        String strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";

                        EjecutarScript(Page, strScript);
                    }
                    else
                    {
                        String strUrl = "../Reportes/FrmPreviewReportesGerenciales.aspx?Head=" + strConCabecera;
                        String strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";

                        EjecutarScript(Page, strScript);
                    }

                    

                }
                else
                {
                    ctrlValidacion.MostrarValidacion("No se han encontrado registros con los criterios indicados", true, Enumerador.enmTipoMensaje.WARNING);
                    updReportesGerenciales.Update();
                }
            }

        }

        private void llenarOficinasActivas()
        {
            try
            {
                DataTable dtOficinasActivas = new DataTable();
                dtOficinasActivas = Comun.ObtenerOficinasActivas(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString()));

                //-----------------------------------------------------
                //Autor: Miguel Márquez Beltrán
                //Fecha: 12/01/2021
                //Motivo: Retirar a Sede Central Lima.
                //-----------------------------------------------------
                DataView dv = dtOficinasActivas.DefaultView;
                dv.RowFilter = "ofco_sOficinaConsularId <> 1";
                DataTable dtOficinasActivasFiltrada = dv.ToTable();
                //-----------------------------------------------------
              //  grdOficinasActivasUniversal.DataSource = dtOficinasActivas;

                grdOficinasActivasUniversal.DataSource = dtOficinasActivasFiltrada;

                grdOficinasActivasUniversal.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        //-------------------------------------------------------------
        //Fecha: 08/11/2019
        //Creado por: Miguel Márquez Beltrán
        //Objetivo: Remover las filas de un Datatable por valor
        //-------------------------------------------------------------

        private void RemoverFilas(ref DataTable dt, string strCampoBusqueda, string strValor, bool isNumeric)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (isNumeric)
                {
                    if (Convert.ToInt32(dt.Rows[i][strCampoBusqueda].ToString()) == Convert.ToInt32(strValor))
                    {
                        dt.Rows.RemoveAt(i);
                        break;
                    }
                }
                else
                {
                    if (dt.Rows[i][strCampoBusqueda].ToString() == strValor)
                    {
                        dt.Rows.RemoveAt(i);
                        break;
                    }
                }
            }

        }

//-------------------------------------------------
    }
}
