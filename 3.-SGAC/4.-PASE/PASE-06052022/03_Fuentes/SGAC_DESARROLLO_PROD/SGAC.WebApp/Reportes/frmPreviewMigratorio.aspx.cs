using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;
using SGAC.Accesorios;
using Microsoft.Reporting.WebForms;
using System.Data;

namespace SGAC.WebApp.Reportes
{
    public partial class frmPreviewMigratorio : MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var str_Reporte = Request.QueryString["iReporte"];

            string strReporteDatos = string.Empty;

            DataTable dt = (DataTable)Session["dtDatosMigratorio"];

            if (!IsPostBack)
            {
                switch (Comun.ToNullInt32(str_Reporte))
                {
                    case (int)Enumerador.enmReportesActoMigratorio.PASAPORTES_EN_GENERAL:
                        strReporteDatos = "rsPasaporteGeneral.rdlc";
                        break;
                    case (int)Enumerador.enmReportesActoMigratorio.CONSOLIDADO_DE_TRAMITES_POR_ANIO:
                        strReporteDatos = "rsConsolidadoPorAnio.rdlc";
                        break;
                    case (int)Enumerador.enmReportesActoMigratorio.INVENTARIO_DE_DOCUMENTOS_DE_VIAJE:
                        strReporteDatos = "rsInventarioDocumentos.rdlc";
                        break;
                    case (int)Enumerador.enmReportesActoMigratorio.SALVOCONDUCTOS_EN_GENERAL:
                        strReporteDatos = "rsSalvoconductoGeneral.rdlc";
                        break;
                }

                dsReport.LocalReport.ReportEmbeddedResource = "Reportes\\RSMigratorio\\" + strReporteDatos;
                dsReport.LocalReport.ReportPath = @"Reportes\\RSMigratorio\\" + strReporteDatos;

                dsReport.LocalReport.DataSources.Clear();

                ReportParameter[] parameters = parameters = new ReportParameter[2];//Transparente
                parameters[0] = new ReportParameter("NombreOficina", Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE].ToString());
                parameters[1] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());


                ReportDataSource datasource = new ReportDataSource("dsMigratorio", dt);
                dsReport.LocalReport.SetParameters(parameters);
                dsReport.LocalReport.DataSources.Add(datasource);

            }
        }
    }
}