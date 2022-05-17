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
using SGAC.BE.MRE;
using SGAC.Registro.Actuacion.BL;

namespace SGAC.WebApp.Registro
{
    public partial class FrmReporteSupervivencia : System.Web.UI.Page
    {

        ActoNotarialConsultaBL BL = new ActoNotarialConsultaBL();
        DataTable dt = new DataTable();
        String sNombreDsReporteServices = String.Empty;
        ReportParameter[] parameters = new ReportParameter[4];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarReporte();               
            }
        }

        private void CargarReporte()
        {
            
            parameters[0] = new ReportParameter("TituloReporte", "REPORTE ACTO NOTARIAL");
            parameters[1] = new ReportParameter("SubTituloReporte", "Extraprotocolar");
            parameters[2] = new ReportParameter("NombreOficina", Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE].ToString());
            parameters[3] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
            String strRutaBase=String.Empty;
            //int enmReporte=2;
            int intTipo = 0;
            if (Session["TipoReporteExtraprotocolar"] != null)
            {
                intTipo = Convert.ToInt32(Session["TipoReporteExtraprotocolar"]);
                switch (intTipo)
                {
                    case (int)Enumerador.enmExtraprotocolarTipo.CERTIFICADO_SUPERVIVENCIA:
                        dt = (DataTable)BL.ReporteSupervivencia(Convert.ToInt32(Session[Constantes.CONST_SESION_ACTONOTARIAL_ID]), Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                        sNombreDsReporteServices = "dsSupervivencia";
                        strRutaBase = Server.MapPath("~/Reportes/RSNotarial/rsSupervivencia.rdlc");
                        break;
                    case (int)Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR:
                        dt = (DataTable)BL.ReporteAutorizacionViaje(Convert.ToInt32(Session[Constantes.CONST_SESION_ACTONOTARIAL_ID]), Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                        sNombreDsReporteServices = "dsAutorizacionViaje";
                        strRutaBase = Server.MapPath("~/Reportes/RSNotarial/rsAutorizacionViaje.rdlc");
                        break;
                    case (int)Enumerador.enmExtraprotocolarTipo.PODER_FUERA_REGISTRO:
                        dt = (DataTable)BL.ReporteSupervivencia(Convert.ToInt32(Session[Constantes.CONST_SESION_ACTONOTARIAL_ID]), Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                        dt.Rows[0]["Ciudad"] = Session["PoderFueraRegistroCuerpo"].ToString();
                        Session.Remove("PoderFueraRegistroCuerpo");

                        sNombreDsReporteServices = "dsAutorizacionViaje";
                        strRutaBase = Server.MapPath("~/Reportes/RSNotarial/rsPoderFueraRegistro.rdlc");
                        break;
                }
             }    
    
            ReportViewer1.LocalReport.ReportEmbeddedResource = strRutaBase;
            ReportViewer1.LocalReport.ReportPath = strRutaBase;

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource datasource = new ReportDataSource(sNombreDsReporteServices, dt);

            ReportViewer1.LocalReport.SetParameters(parameters);
            ReportViewer1.LocalReport.DataSources.Add(datasource);
            dt = null;

         }    

    }
}