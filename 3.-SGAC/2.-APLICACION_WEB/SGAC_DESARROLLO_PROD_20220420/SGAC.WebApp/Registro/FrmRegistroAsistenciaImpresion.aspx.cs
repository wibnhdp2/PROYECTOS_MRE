using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;
using SGAC.Controlador;
using SGAC.Accesorios;
using Microsoft.Reporting.WebForms;
using SGAC.Registro.Persona.BL;
using System.Configuration;

namespace SGAC.WebApp.Registro
{
    public partial class FrmRegistroAsistenciaImpresion : MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SeteraControles2();

                ctrlOficinaConsular.ddlOficinaConsular.AutoPostBack = true;
                ctrlOficinaConsular.ddlOficinaConsular.SelectedIndexChanged += new EventHandler(ddlOficinaConsular_SelectedIndexChanged);

                ctrlToolBarConsulta.btnImprimir.OnClientClick = "return ValidarDatos()";

                if (!Page.IsPostBack)
                {
                    this.dtpFecInicio.StartDate = new DateTime(1900, 1, 1);
                    this.dtpFecInicio.EndDate = DateTime.Today;
                    this.dtpFecInicio.EndDate = DateTime.Now;
                    this.dtpFecInicio.Text = DateTime.Now.ToString("MMM-01-yyyy");

                    this.dtpFecFin.StartDate = new DateTime(1900, 1, 1);
                    this.dtpFecFin.EndDate = DateTime.Today;
                    this.dtpFecFin.EndDate = DateTime.Now;
                    dtpFecFin.Text = Comun.ObtenerFechaActualTexto(Session);

                    CargarCombos();
                    ddlOficinaConsular_SelectedIndexChanged(sender, e);
                    OptOpcion1.Checked = true;

                    if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Constantes.CONST_OFICINACONSULAR_LIMA)
                    {
                        OptOpcion2.Visible = false;
                        OptOpcion1.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Session["_LastException"] = ex;
                Response.Redirect("../PageError/GenericErrorPage.aspx");
            }
        }

        void ddlOficinaConsular_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int sOficinaConsularId = 0;

                if (OptOpcion2.Checked)
                {
                    sOficinaConsularId = Constantes.CONST_OFICINACONSULAR_LIMA;
                }
                else
                {
                    sOficinaConsularId = Convert.ToInt16(ctrlOficinaConsular.SelectedValue);

                }

                object[] arrParametros = { sOficinaConsularId };

                //Proceso p = new Proceso();
                //DataTable dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.SE_USUARIO", "LISTAR");

                SGAC.Configuracion.Seguridad.BL.UsuarioConsultasBL objUsuarioConsultaBL = new SGAC.Configuracion.Seguridad.BL.UsuarioConsultasBL();
                DataTable dt = new DataTable();

                dt = objUsuarioConsultaBL.ObtenerLista(sOficinaConsularId);

                Util.CargarDropDownList(ddl_Usuario, dt, "usua_vAlias", "usua_sUsuarioId", true, " - TODOS - ");

            }
            catch (Exception ex)
            {
                Session["_LastException"] = ex;
                Response.Redirect("../PageError/GenericErrorPage.aspx");
            }

        }

        public static void EjecutarScript(Page Page, string strScript)
        {
            strScript = string.Format(strScript);
            ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), "OpenPopup", strScript, true);
        }

        void SeteraControles2()
        {
            ctrlToolBarConsulta.btnPrintHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonPrintClick(ctrlToolBarConsulta_btnPrintHandler);

            ctrlToolBarConsulta.VisibleIButtonNuevo = false;
            ctrlToolBarConsulta.VisibleIButtonEditar = false;
            ctrlToolBarConsulta.VisibleIButtonGrabar = false;
            ctrlToolBarConsulta.VisibleIButtonCancelar = false;
            ctrlToolBarConsulta.VisibleIButtonBuscar = false;
            ctrlToolBarConsulta.VisibleIButtonPrint = true;
            ctrlToolBarConsulta.VisibleIButtonEliminar = false;
            ctrlToolBarConsulta.VisibleIButtonConfiguration = false;
            ctrlToolBarConsulta.VisibleIButtonSalir = false;
        }

        void ctrlToolBarConsulta_btnPrintHandler()
        {
            try
            {
                DataTable xDt = new DataTable();
                ReportParameter[] parameters = new ReportParameter[5];
                Proceso MiProc = new Proceso();
                string strTitulo = string.Empty;

                if (dtpFecInicio.Text == "")
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA, true, Enumerador.enmTipoMensaje.ERROR);
                    return;
                }

                if (dtpFecFin.Text == "")
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA, true, Enumerador.enmTipoMensaje.ERROR);
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
                    return;
                }
                
                updConsulta.Update();

                if (OptOpcion1.Checked == true)
                {
                    Session["strNombreArchivo"] = "rsAsistenciaPAHL.rdlc";
                    strTitulo = "PROGRAMA DE ASISTENCIA LEGAL Y HUMANITARIA - PALH";
                    Session["strNombreArchivo"] = "rsAsistenciaPAHL.rdlc";
                }

                if (OptOpcion2.Checked == true)
                {
                    Session["strNombreArchivo"] = "rsAsistenciaPAH.rdlc";
                    strTitulo = "PROGRAMA DE ASISTENCIA HUMANITARIA - PAH";
                    Session["strNombreArchivo"] = "rsAsistenciaPAH.rdlc";
                }

                string[] oficina = Convert.ToString(Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE]).Split(',');


                parameters[0] = new ReportParameter("Titulo1", "SERVICIO CONSULAR DEL PERU");
                parameters[1] = new ReportParameter("Titulo2", strTitulo);
                parameters[2] = new ReportParameter("OficinaConsular", oficina[0]);
                parameters[3] = new ReportParameter("Usuario", (string)Session[Constantes.CONST_SESION_USUARIO]);
                parameters[4] = new ReportParameter("fecha", "Del  " + datFechaInicio.ToString("MMM-dd-yyyy") + "  Al  " + datFechaFin.ToString("MMM-dd-yyyy"));

                string strCodContinente = null;
                Int16? sOficinaConsularId = null;
                Int16? sUsuarioId = null;
                Int16? sModalidadPahlId = null;
                Int16? sModalidadPahId = null;

                if (ddl_Continente.SelectedValue != "")
                {
                    if (ddl_Continente.SelectedValue == "00")
                    {
                        strCodContinente = null;
                    }
                    else
                    {
                        if (ddl_Continente.SelectedValue == Convert.ToInt16(Enumerador.enmContinente.AFRICA).ToString())
                        {
                            strCodContinente = Convert.ToInt16(Enumerador.enmContinenteUbigeo.AFRICA).ToString(); // AFRICA 91
                        }

                        if (ddl_Continente.SelectedValue == Convert.ToInt16(Enumerador.enmContinente.AMERICA).ToString())
                        {
                            strCodContinente = Convert.ToInt16(Enumerador.enmContinenteUbigeo.AMERICA).ToString(); // AMERICA 92
                        }

                        if (ddl_Continente.SelectedValue == Convert.ToInt16(Enumerador.enmContinente.ASIA).ToString())
                        {
                            strCodContinente = Convert.ToInt16(Enumerador.enmContinenteUbigeo.ASIA).ToString(); // ASIA 93
                        }

                        if (ddl_Continente.SelectedValue == Convert.ToInt16(Enumerador.enmContinente.EUROPA).ToString())
                        {
                            strCodContinente = Convert.ToInt16(Enumerador.enmContinenteUbigeo.EUROPA).ToString(); // EUROPA 94
                        }

                        if (ddl_Continente.SelectedValue == Convert.ToInt16(Enumerador.enmContinente.OCEANIA).ToString())
                        {
                            strCodContinente = Convert.ToInt16(Enumerador.enmContinenteUbigeo.OCEANIA).ToString(); // OCEANIA 95
                        }
                    }
                }


                if (ctrlOficinaConsular.SelectedValue != "")
                {
                    sOficinaConsularId = Convert.ToInt16(ctrlOficinaConsular.SelectedValue);
                }

                if (ddl_Usuario.SelectedValue.ToString() != "0")
                {
                    sUsuarioId = Convert.ToInt16(ddl_Usuario.SelectedValue);
                }

                if (ddl_Modalidad.SelectedValue != "00")
                {
                    if (OptOpcion1.Checked == true) //PALH
                    {
                        sModalidadPahlId = Convert.ToInt16(ddl_Modalidad.SelectedValue);
                    }

                    if (OptOpcion2.Checked == true) //PAH
                    {
                        sModalidadPahId = Convert.ToInt16(ddl_Modalidad.SelectedValue);
                    }
                }

                xDt.Clear();

                SGAC.Registro.Persona.BL.PersonaAsistenciaConsultaBL FunAsistencia = new SGAC.Registro.Persona.BL.PersonaAsistenciaConsultaBL();
                if (OptOpcion1.Checked == true) //PALH
                {
                    xDt = FunAsistencia.ListarAsistenciaPALH(datFechaInicio,
                                                             datFechaFin,
                                                             strCodContinente,
                                                             sModalidadPahlId,
                                                             Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                                                             sOficinaConsularId,
                                                             sUsuarioId, txtTitular.Text);
                }

                if (OptOpcion2.Checked == true) //PAH
                {
                    xDt = FunAsistencia.ListarAsistenciaPAH(datFechaInicio,
                                                            datFechaFin,
                                                            strCodContinente,
                                                            sModalidadPahId,
                                                            Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                                                            sOficinaConsularId,
                                                            sUsuarioId, txtTitular.Text);
                }

                Session["DtDatos"] = xDt;
                Session["objParametroReportes"] = parameters;

                string strUrl = "../Colas/FrmVisorRDCL.aspx";
                string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=1000,height=700,left=100,top=100');";
                EjecutarScript(Page, strScript);
            }
            catch (Exception ex)
            {
                Session["_LastException"] = ex;
                Response.Redirect("../PageError/GenericErrorPage.aspx");
            }
        }

        void CargarCombos()
        {
            try
            {
                DataTable xDt = new DataTable();
                DataTable xDtServicios = new DataTable();
                DataTable xDtUsuarios = new DataTable();

                xDt = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.CONTINENTE);

                
                // Oficina Consular
                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
                {
                    ctrlOficinaConsular.Cargar(true, true, " - TODAS - ");
                }
                else
                {
                    ctrlOficinaConsular.Cargar(false, true, " - TODAS - ");
                }

                ctrlOficinaConsular.SelectedValue = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString();

                // CARGAMOS EL DATATABLE CON LOS SERVICIOS
                SGAC.Configuracion.Maestro.BL.ServicioConsultaBL FunServicio = new SGAC.Configuracion.Maestro.BL.ServicioConsultaBL();
                xDtServicios = FunServicio.Consulta();
                Session["xDtServicios"] = xDtServicios;
                if (OptOpcion1.Checked == true)
                {
                    xDtServicios = SGAC.Accesorios.Util.DataTableFiltrar((DataTable)Session["xDtServicios"], "serv_vGrupo  = 'PAH'", "");
                }
                else {
                    xDtServicios = SGAC.Accesorios.Util.DataTableFiltrar((DataTable)Session["xDtServicios"], "serv_vGrupo  = 'PALH'", ""); 
                }
                FillWebCombo2(xDtServicios, ddl_Modalidad, "serv_vDescripcionCorta", "serv_sServicioId");

                //CARGAMOS LOS CONTINENTES
                FillWebCombo2(xDt, ddl_Continente, "descripcion", "id");

                //CARGAMOS LOS USUARIOS
                //SGAC.Configuracion.Seguridad.BL.UsuarioConsultasBL FunUsuario = new SGAC.Configuracion.Seguridad.BL.UsuarioConsultasBL();
                //xDtUsuarios = FunUsuario.ObtenerTodo();
                //Session["xDtUsuarios"] = xDtUsuarios;

                ////FILTRAMOS EL DATATABLE CON LOS USUARIOS DE LA OFICINA SELECCIONADA
                //xDtUsuarios = Util.DataTableFiltrar(xDtUsuarios, "usro_sOficinaConsularId = " + ctrlOficinaConsular.SelectedValue + "", "usua_vAlias ASC");
                //ddl_Usuario.DataTextField = "usua_vAlias";
                //ddl_Usuario.DataValueField = "usua_sUsuarioId";
                //ddl_Usuario.DataSource = xDtUsuarios;
                //ddl_Usuario.DataBind();
                //ddl_Usuario.Items.Insert(0, new ListItem("- TODOS -", "00"));
                

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable ddl_comboUsuario()
        {
            try
            {
                int IntTotalCount = 0;
                int IntTotalPages = 0;
                int IntTamañoPagina = Constantes.CONST_PAGE_SIZE_PERSONA_RUNE;

                DataTable DtPersona = new DataTable();

                PersonaConsultaBL PersonaBL = new PersonaConsultaBL();

                Proceso MiProc = new Proceso();
                Object[] miArray = new Object[8] { 1,
                                               "",
                                               "b",
                                               "",
                                               1,
                                               IntTamañoPagina,
                                               IntTotalCount,
                                               IntTotalPages };

                DtPersona = (DataTable)MiProc.Invocar(ref miArray,
                                                      "SGAC.BE.RE_PERSONA",
                                                      Enumerador.enmAccion.CONSULTAR,
                                                      Enumerador.enmAplicacion.WEB);
                return DtPersona;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void FillWebCombo(DataTable pDataTable, DropDownList pWebCombo, String str_pDescripcion, String str_pValor)
        {
            try
            {
                pWebCombo.DataSource = pDataTable;
                pWebCombo.DataTextField = str_pDescripcion;
                pWebCombo.DataValueField = str_pValor;
                pWebCombo.DataBind();
                pWebCombo.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void FillWebCombo2(DataTable pDataTable, DropDownList pWebCombo, String str_pDescripcion, String str_pValor)
        {
            try
            {
                pWebCombo.DataSource = pDataTable;
                pWebCombo.DataTextField = str_pDescripcion;
                pWebCombo.DataValueField = str_pValor;
                pWebCombo.DataBind();
                pWebCombo.Items.Insert(0, new ListItem("- TODOS -", "00"));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void OptOpcion1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (OptOpcion1.Checked == true)
                {
                    DataTable xDtServicios = new DataTable();
                    xDtServicios = SGAC.Accesorios.Util.DataTableFiltrar((DataTable)Session["xDtServicios"], "serv_vGrupo  = 'PALH'", "serv_vDescripcionCorta ASC");
                    FillWebCombo2(xDtServicios, ddl_Modalidad, "serv_vDescripcionCorta", "serv_sServicioId");
                    OptOpcion2.Checked = false;

                    ctrlOficinaConsular.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                Session["_LastException"] = ex;
                Response.Redirect("../PageError/GenericErrorPage.aspx");
            }

        }

        protected void OptOpcion2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (OptOpcion2.Checked == true)
                {
                    DataTable xDtServicios = new DataTable();
                    xDtServicios = SGAC.Accesorios.Util.DataTableFiltrar((DataTable)Session["xDtServicios"], "serv_vGrupo  = 'PAH'", "serv_vDescripcionCorta ASC");
                    FillWebCombo2(xDtServicios, ddl_Modalidad, "serv_vDescripcionCorta", "serv_sServicioId");
                    OptOpcion1.Checked = false;

                    ctrlOficinaConsular.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                Session["_LastException"] = ex;
                Response.Redirect("../PageError/GenericErrorPage.aspx");
            }

        }

        protected void ddl_Continente_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strCodContinente = string.Empty;

            if (ddl_Continente.SelectedValue == "00")
            {
                strCodContinente = "0";
                ctrlOficinaConsular.Cargar(true);
            }
            else
            {
                if (ddl_Continente.SelectedValue == Convert.ToInt16(Enumerador.enmContinente.AFRICA).ToString())
                {
                    strCodContinente = Convert.ToInt16(Enumerador.enmContinenteUbigeo.AFRICA).ToString(); // AFRICA 91
                }

                if (ddl_Continente.SelectedValue == Convert.ToInt16(Enumerador.enmContinente.AMERICA).ToString())
                {
                    strCodContinente = Convert.ToInt16(Enumerador.enmContinenteUbigeo.AMERICA).ToString(); // AMERICA 92
                }

                if (ddl_Continente.SelectedValue == Convert.ToInt16(Enumerador.enmContinente.ASIA).ToString())
                {
                    strCodContinente = Convert.ToInt16(Enumerador.enmContinenteUbigeo.ASIA).ToString(); // ASIA 93
                }

                if (ddl_Continente.SelectedValue == Convert.ToInt16(Enumerador.enmContinente.EUROPA).ToString())
                {
                    strCodContinente = Convert.ToInt16(Enumerador.enmContinenteUbigeo.EUROPA).ToString(); // EUROPA 94
                }

                if (ddl_Continente.SelectedValue == Convert.ToInt16(Enumerador.enmContinente.OCEANIA).ToString())
                {
                    strCodContinente = Convert.ToInt16(Enumerador.enmContinenteUbigeo.OCEANIA).ToString(); // OCEANIA 95
                }
                
                ctrlOficinaConsular.Cargar(true, true, " - TODOS - ", strCodContinente);
            }

            ddl_Usuario.Items.Clear();
            ddl_Usuario.Items.Insert(0, new ListItem(" - TODOS - ", "0"));
        }
    }
}