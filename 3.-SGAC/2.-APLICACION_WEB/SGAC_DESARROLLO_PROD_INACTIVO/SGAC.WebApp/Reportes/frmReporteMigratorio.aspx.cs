using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;
using SGAC.Accesorios;
using System.Data;
using SGAC.Controlador;

namespace SGAC.WebApp.Reportes
{
    public partial class frmReporteMigratorio : MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlOficinaConsular.ddlOficinaConsular.AutoPostBack = true;
            ctrlOficinaConsular.ddlOficinaConsular.SelectedIndexChanged += new EventHandler(ddlOficinaConsular_SelectedIndexChanged);
            
            //dtpFecInicio.AllowFutureDate = true;
            this.dtpFecInicio.StartDate = new DateTime(1900, 1, 1);
            dtpFecFin.AllowFutureDate = true;

            this.dtpFecFin.StartDate = this.dtpFecInicio.StartDate = new DateTime(1900, 1, 1); 
            if (!IsPostBack)
            {
               

                int anio = 2015;

                ddlAnio.Items.Add(new ListItem { Value = "0", Text = "- SELECCIONE -" });

                for (int n = anio; n <= DateTime.Now.Year; n++)
                    ddlAnio.Items.Add(new ListItem { Value = n.ToString(), Text = n.ToString() });

                CargarListadosDesplegables();

                IDV_1.Visible = false;
                IDV_2.Visible = false;
                CTA.Visible = false;
                PG_1.Visible = false;
                PG_2.Visible = false;
                PG_3.Visible = false;
                PG_5.Visible = false;
                PG_6.Visible = false;
                PG_7.Visible = false;
                PG_8.Visible = false;


            }
        }

        private void CargarListadosDesplegables()
        {
            DataTable dtTarifa = new DataTable();

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                ctrlOficinaConsular.Cargar(true, false);
                ddlMision_IDV.Cargar(true, false);
            }
            else
            {
                ctrlOficinaConsular.Cargar(false, false);
                ddlMision_IDV.Cargar(false, false);
            }

            Util.CargarParametroDropDownList(ddl_TipoReporte, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_REPORTE_MIGRATORIO), true);
            Util.CargarParametroDropDownList(ddlEstadoPasaporte, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmEstadoGrupo.PASAPORTE), true);
            Util.CargarParametroDropDownList(ddlEstadoPass_IDV, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmEstadoGrupo.PASAPORTE), true);

            

            Limpiar_Datos();

            Util.CargarDropDownList(ddlTipoDocumento, comun_Part1.ObtenerDocumentoIdentidad(), "Valor", "Id", true);
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
                
                SGAC.Configuracion.Seguridad.BL.UsuarioConsultasBL objUsuarioConsultasBL = new SGAC.Configuracion.Seguridad.BL.UsuarioConsultasBL();
                DataTable dt = new DataTable();

                dt = objUsuarioConsultasBL.ObtenerLista(sOficinaConsularId);

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

        protected void ddl_TipoReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            ctrlOficinaConsular.Cargar(true, true, " - TODOS - ", "");
            ddlMision_IDV.Cargar(true, true, " - TODOS - ", "");
            Limpiar_Datos();

            Label3.Text = "Número Pasaporte :";
            Label4.Visible = true;
            ddlEstadoPasaporte.Visible = true;
            chk_expediente.Visible = true;
            Label7.Visible = true;

            switch (Comun.ToNullInt32(ddl_TipoReporte.SelectedValue))
            {
                case (int)Enumerador.enmReportesActoMigratorio.PASAPORTES_EN_GENERAL:
                    IDV_1.Visible = false;
                    IDV_2.Visible = false;
                    CTA.Visible = false;
                    PG_1.Visible = true;
                    PG_2.Visible = true;
                    PG_3.Visible = true;
                    PG_5.Visible = true;
                    PG_6.Visible = true;
                    PG_7.Visible = true;
                    PG_8.Visible = true;
                    ctrlOficinaConsular.SelectedValue = Comun.ToNullInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString();
                    break;
                case (int)Enumerador.enmReportesActoMigratorio.CONSOLIDADO_DE_TRAMITES_POR_ANIO:
                    IDV_1.Visible = false;
                    IDV_2.Visible = false;
                    CTA.Visible = true;
                    PG_1.Visible = false;
                    PG_2.Visible = false;
                    PG_3.Visible = false;
                    PG_5.Visible = false;
                    PG_6.Visible = false;
                    PG_7.Visible = false;
                    PG_8.Visible = false;
                    break;
                case (int)Enumerador.enmReportesActoMigratorio.INVENTARIO_DE_DOCUMENTOS_DE_VIAJE:
                    IDV_1.Visible = true;
                    IDV_2.Visible = true;
                    CTA.Visible = false;
                    PG_1.Visible = false;
                    PG_2.Visible = false;
                    PG_3.Visible = false;
                    PG_5.Visible = false;
                    PG_6.Visible = false;
                    PG_7.Visible = false;
                    PG_8.Visible = false;
                    ddlMision_IDV.SelectedValue = Comun.ToNullInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString();
                    break;
                case (int)Enumerador.enmReportesActoMigratorio.SALVOCONDUCTOS_EN_GENERAL:
                    IDV_1.Visible = false;
                    IDV_2.Visible = false;
                    CTA.Visible = false;
                    PG_1.Visible = true;
                    PG_2.Visible = true;
                    PG_3.Visible = true;
                    PG_5.Visible = true;
                    PG_6.Visible = true;
                    PG_7.Visible = true;
                    PG_8.Visible = true;
                    Label4.Visible = false;
                    ddlEstadoPasaporte.Visible = false;
                    Label3.Text = "Número Salvoconducto :";
                    chk_expediente.Visible = false;
                    Label7.Visible = false;
                    ctrlOficinaConsular.SelectedValue = Comun.ToNullInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString();
                    break;
                default:
                    IDV_1.Visible = false;
                    IDV_2.Visible = false;
                    CTA.Visible = false;
                    PG_1.Visible = false;
                    PG_2.Visible = false;
                    PG_3.Visible = false;
                    PG_5.Visible = false;
                    PG_6.Visible = false;
                    PG_7.Visible = false;
                    PG_8.Visible = false;
                    break;
            }
            updReportesGerenciales.Update();
        }

        Boolean bolVistaPrevia = false;

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            VerVistaPrevia();

            if (this.dtpFecInicio.Visible && this.dtpFecFin.Visible)
            {

            }

            if (bolVistaPrevia)
            {
                bolVistaPrevia = false;
                String strUrl = "../Reportes/frmPreviewMigratorio.aspx?iReporte=" + ddl_TipoReporte.SelectedItem.Value;
                String strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=yes,resizable=yes,titlebar=no,toolbar=no,location=no,status=no,menubar=no,width=1100,height=650,left=150,top=10,directories=no');";
                EjecutarScript(Page, strScript);
            }
            else
            {
                ctrlValidacion.MostrarValidacion("No se han encontrado registros con los criterios indicados", true, Enumerador.enmTipoMensaje.WARNING);
                updReportesGerenciales.Update();
            }

        }


        private void VerVistaPrevia()
        {
            DateTime? fechainicio = null;
            DateTime? fechafin = null;
            
            if (dtpFecInicio.Text != "") { fechainicio = Comun.FormatearFecha(dtpFecInicio.Text); }
            if (dtpFecFin.Text != "") { fechafin = Comun.FormatearFecha(dtpFecFin.Text); }

            bolVistaPrevia = false;

            Session["dtDatosMigratorio"] = new SGAC.Reportes.BL.ReportesMigratorioConsultaBL().ReporteMigratorio(Comun.ToNullInt32(ddl_TipoReporte.SelectedItem.Value),
                txt_nro_pasaporte_ini.Text.Trim(), txt_nro_pasaporte_fin.Text.Trim(), Comun.ToNullInt32(ddlMision_IDV.SelectedItem.Value), Comun.ToNullInt32(ddlEstadoPass_IDV.SelectedItem.Value),
                ddlAnio.SelectedItem.Value, Comun.ToNullInt32(ctrlOficinaConsular.SelectedItem.Value), txt_nun_Pasaporte.Text.Trim(), chk_expediente.Checked,
                Comun.ToNullInt32(ddlTipoDocumento.SelectedItem.Value), txt_nro_Documento.Text.Trim(), Comun.ToNullInt32(ddlEstadoPasaporte.SelectedItem.Value),
                txt_nro_Expediente.Text.Trim(), txt_apellido_parterno.Text.Trim(), txt_apellido_materno.Text.Trim(), txt_nombres.Text.Trim(), fechainicio, fechafin);

            if (Session["dtDatosMigratorio"] != null)
            {
                if (((DataTable)Session["dtDatosMigratorio"]).Rows.Count > 0)
                    bolVistaPrevia = true;
            }
        }

        public static void EjecutarScript(Page Page, string strScript)
        {
            strScript = string.Format(strScript);
            ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), "OpenPopup", strScript, true);
        }

        internal void Limpiar_Datos()
        {
            txt_nro_pasaporte_ini.Text = "";
            txt_nro_pasaporte_fin.Text = "";
            ddlMision_IDV.SelectedIndex = -1;
            ddlEstadoPass_IDV.SelectedIndex = -1;
            ddlAnio.SelectedIndex = -1;
            ctrlOficinaConsular.SelectedIndex = -1;
            txt_nun_Pasaporte.Text = "";
            ddlTipoDocumento.SelectedIndex = -1;
            txt_nro_Documento.Text = "";
            chk_expediente.Checked = false;
            ddlEstadoPasaporte.SelectedIndex = -1;
            txt_nro_Expediente.Text = "";
            txt_apellido_parterno.Text = "";
            txt_apellido_materno.Text = "";
            txt_nombres.Text = "";
            dtpFecInicio.Text = "";
            dtpFecFin.Text = "";

        }
    }
}